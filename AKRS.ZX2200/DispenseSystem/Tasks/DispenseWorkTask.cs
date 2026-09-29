using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Product;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportSystem.Models;
using DevExpress.XtraEditors;
using log4net.Core;

namespace AKRS.ZX2200.DispenseSystem.Tasks
{
    using System;
    using System.Threading.Tasks;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.Product.Statistics;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using Newtonsoft.Json;

    /// <summary>
    /// 点胶实际工作线程
    /// </summary>
    public class DispenseWorkTask
    {
        /// <summary>
        /// 系统1控制器
        /// </summary>
        public S1ActionNodeController S1ActionNodeController => System1Domain.GetInstance().ActionNodeController;

        /// <summary>
        /// 点胶控制器
        /// </summary>
        public DispenseController DispenseController => System1Domain.GetInstance().DispenseController; 

        /// <summary>
        /// 点胶线程
        /// </summary>
        public Task DispenseTask { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [JsonIgnore]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 点胶模块执行点胶过程
        /// </summary>
        public void DoWork()
        {
            // 启动之前先把动作排好
            this.S1ActionNodeController.Init();

            // 总循环
            while (Signal.WaitStart())
            {
                // 移动到安全位置
                this.DispenseController.MoveToSafePos();

                DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"点胶线程等待来料信号");

                if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                {
                    // 等待流道信号             
                    if (!this.GetInputSignal())
                    {
                        DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"点胶线程未收到信号，回到安全位置");

                        // 点胶Z轴移动到安全位置
                        this.DispenseController.MoveToSafePos();
                        return;
                    }
                }

                DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"点胶线程收到信号，准备开始工作");

                DispenseRunTimeProvider.IsFirstDispense = true;

                if (this.Enable)
                {
                    if (!TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit.TransportUnitInfo.IsProduct)
                    {
                        TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit =
                            new TransportUnit(CurrentMachineSystemEnum.System1);
                        TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit.TransportUnitInfo.IsProduct =
                            true;

                        DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"点胶重置产品对象成功");
                    }
 
                    // 注入制程
                    this.S1ActionNodeController.InjectSteps(
                        TransportProgram.GetInstance().DispenseSubSectionProgram.TransportUnit);

                    DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"点胶注入产品对象信息");

                    // 开始生产
                    StatisticsDomain.GetInstance().CalculateDispenseCycle();

                    DispenseRunTimeProvider.RecordTime("点胶区等流道来料信号......", $"开始动作循环");

                    // 不分段，中间和流道交互的过程封装成动作，到时候直接注入到动作中
                    while (Signal.WaitStart())
                    {
                        ActionNode actionNode = this.S1ActionNodeController.GetNextActionNode();

                        // 没有actionNode了，代表全部都已经做完了，直接退出
                        if (actionNode == null)
                        {
                            break;
                        }

                        if (!actionNode.IsDoWork())
                        {
                            goto Finish;
                        }

                        // 如果这个动作不是测高动作则收回点胶气缸
                        if (actionNode.Name != "S1DispenseMeasureActionNode" 
                            && actionNode.Name != "S1CarrierMeasureHeightActionNode"
                            && actionNode.Name != "S1SubstrateMeasureHeightActionNode"
                            && actionNode.Name != "S1ModuleMeasureHeightActionNode")
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                            {
                                // 收回点胶气缸
                                System1Domain.GetInstance().DispenseMeasureHeightController
                                    .CloseDispenseHeightMeasurementCylinder();
                            }

                            DispenseRunTimeProvider.RecordTime("系统1", "收回点胶气缸完成");
                        }

                        // 执行动作的主要方法都在这个里面
                        ExcuteResult ret = actionNode.DoWork();

                        if (ret == ExcuteResult.Retry)
                        {
                            continue;
                        }
                        else if (ret != ExcuteResult.Success)
                        {
                            // 如果当前动作没有成功，直接退出线程
                            this.DispenseController.MoveToSafePos();
                            Machine.GetInstance().Stop();
                            DispenseRunTimeProvider.RecordTime("点胶工作线程", $"点胶线程,动作：{actionNode.Name}执行未成功，线程结束");
                            return;
                        }

                        Finish:
                        this.S1ActionNodeController.FinishCurrentActionNode();
                    }
                }

                this.DispenseController.MoveToSafePos();

                // 如果循环因为停止而退出的，就不要给流道发送可以传料的信号了
                if (Machine.GetInstance().IsStop() == false)
                {
                    // 点胶执行完成，告诉流道移走产品，等待产品流走
                    SignalPool.GetInstance().AllowDispenseTransferToBondSignal.Set();
                }

                DispenseRunTimeProvider.RecordTime("点胶工作完成", $"点胶工作完成，告诉轨道可以传输产品");

                // 全部完成后重置产品状态
                this.S1ActionNodeController.Finish();
            }
        }

        /// <summary>
        /// 启动点胶线程
        /// </summary>
        /// <returns>结果</returns>
        public bool Start()
        {
            if (this.DispenseTask == null || this.DispenseTask.Status != TaskStatus.Running)
            {
                // MachineStateModel.GetInstance().MachineState = Galaxy2.Machine.Enums.MachineStateEnum.Working;

                this.DispenseTask = Task.Factory.StartNew(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("点胶工作线程");
                            this.DoWork();
                        },
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }

            return true;
        }

        /// <summary>
        /// AbortWaferSubThread
        /// </summary>
        public void AbortThread()
        {
            this.DispenseTask.Dispose();
        }

        /// <summary>
        /// 获取来料信号
        /// </summary>
        /// <returns>结果</returns>
        private bool GetInputSignal()
        {
            while (true)
            {
                Thread.Sleep(10);


                if (Machine.GetInstance().IsPause())
                {
                    continue;
                }

                // 执行预点胶
                ExcuteResult result = System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.DoWork();

                if (result != ExcuteResult.Success)
                {
                    return false;
                }

                if (Machine.GetInstance().IsStop())
                {
                    return false;
                }
                else if (Machine.GetInstance().IsPause())
                {
                    continue;
                }
                else if (SignalPool.GetInstance().TransferToDispense1FinishedSignal.State == SignalStateEnum.Set)
                {
                    SignalPool.GetInstance().TransferToDispense1FinishedSignal.State = SignalStateEnum.ReSet;
                    return true;
                }
                else if (SignalPool.GetInstance().TransferToDispense1FinishedSignal.State == SignalStateEnum.ReSet)
                {
                    continue;
                }
                else
                {
                    throw new Exception("点胶入料载台来料信号异常");
                }
            }
        }
    }
}
