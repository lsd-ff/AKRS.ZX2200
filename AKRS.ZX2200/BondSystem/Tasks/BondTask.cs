using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate;
using AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.TransportUnitSystem.Service;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

namespace AKRS.ZX2200.BondSystem.Tasks
{
    /// <summary>
    /// BondModule工作线程
    /// </summary>
    public class BondTask
    {
        /// <summary>
        /// BondDomain
        /// </summary>
        [JsonIgnore]
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        [JsonIgnore]
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

        /// <summary>
        /// Bond Module 控制器
        /// </summary>
        [JsonIgnore]
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 当前传输单元/载具 
        /// </summary>
        [JsonIgnore]
        private TransportUnit transportUnit =>
            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

        /// <summary>
        /// Bond Module 控制器
        /// </summary>
        [JsonIgnore]
        private SlideFluxerController slideFluxerController => System2Domain.GetInstance().SlideFluxerController;

        /// <summary>
        /// Bond工作线程
        /// </summary>
        [JsonIgnore]
        public Task BondWorkTask;

        /// <summary>
        /// 是否启用
        /// </summary>
        [JsonIgnore]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// 工作
        /// </summary>
        public void DoWork()
        {
            //// 先排好序
            //this.system2Domain.ActionNodesService.Init();

            // 重置运行信息
            System2RunTimeProvider.ResetInformation();

            // 总循环
            while (Signal.WaitStart())
            {
                // 所有轴移动到安全位
                this.nozzleShelfController.MoveShelfToHome();
                this.bondModuleController.MoveToSafePos();
                this.bondHeadController.ResetBondhead();

                BaseCarrierConfig component = System2Domain.GetInstance().ActionNodesService.GetFirstComponent();

                #region 提前换吸嘴、搜精

                if (this.Enable
                    && System2Configuration.GetInstance().IsChangeNozzleAdvance
                    && component != null)
                {
                    //// 判断是否第一次启动
                    //if (RunTimeProvider.IsFirstStart)
                    //{
                    //    // 芯片名称传给晶圆台
                    //    WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                    //    // 给晶圆台发要料信号
                    //    SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                    //    RunTimeProvider.IsFirstStart = false;
                    //}

                    if (this.system2Controller.IsNeedToChangeNozzle(component.NozzleName))
                    {
                        // 换吸嘴动作
                        bool ret = this.system2Controller.ChangeNozzle(component.NozzleName);
                        if (ret == false)
                        {
                            // 换吸嘴失败停止工作
                            Machine.GetInstance().Stop();
                            return;
                        }
                    }
                }

                #endregion

                System2RunTimeProvider.RecordTime("Bond线程", "固晶区等流道来料信号......");

                // 等流道来料信号（从信号池获取）
                if (!this.GetIncomingSignal())
                {
                    return;
                }

                UcMainSystem.ReFreshSystem2TuAction();

                // 取片温漂补偿
                if (!TpMarkCompensate.PickUpCompensationVision(true))
                {
                    return;
                }

                // 温飘补偿
                if (!this.system2Controller.System2TemperatureCompensation(true))
                {
                    return;
                }

                // 温飘补偿
                if (!RealTimeCorrection.GetInstance().ScanReferencePoints(true))
                {
                    return;
                }

                this.bondModuleController.MoveToSafePos();

                StatisticsDomain.GetInstance().CalculateBondCycle();

                System2RunTimeProvider.RecordTime("Bond线程", "固晶区等流道来料信号成功");

                //TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit = new TransportUnit("测试");

                if (MachineStateModel.GetInstance().IsCompensateWork)
                {
                    TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit = new TransportUnit("测试");
                }

                // 注入制程
                this.system2Domain.ActionNodesService.InjectSteps(
                    TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit);

                System2RunTimeProvider.IsFirstDispense = true;

                if (this.Enable)
                {
                    if (!TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit.TransportUnitInfo.IsProduct
                        && MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
                    {
                        TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit =
                            new TransportUnit(CurrentMachineSystemEnum.System2);
                        TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit.TransportUnitInfo.IsProduct =
                           true;
                    }

                    System2RunTimeProvider.IsNewProduct = true;

                    #region 循环执行动作节点

                    // 遍历所有bondActionNodeMessages
                    while (Signal.WaitStart())
                    {
                        ActionNode actionNode = this.system2Domain.ActionNodesService.GetNextActionNode();

                        // 所有动作做完
                        if (actionNode == null)
                        {
                            break;
                        }
                        
                        RetryCommand:
                     
                        if (actionNode.IsDoWork())
                        {
                            System2RunTimeProvider.RecordTime("动作流程", $"开始执行动作{actionNode.Name}");

                            this.CloseDispenseCylinder(actionNode);

                            ExcuteResult ret = actionNode.DoWork();

                            switch (ret)
                            {
                                case ExcuteResult.Abort:

                                    System2RunTimeProvider.RecordTime("Bond线程", "返回：ExcuteResult.Abort");

                                    Machine.GetInstance().Stop();

                                    // 焊头去安全高度
                                    this.bondHeadController.MoveBondZToSafePos();

                                    if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
                                    {
                                        // 胶盘缩回
                                        this.slideFluxerController.SlideFluxerHomeWaitArrive();
                                    }

                                    return;

                                case ExcuteResult.Retry:

                                    System2RunTimeProvider.RecordTime("Bond线程", "返回：ExcuteResult.Retry");

                                    // 重试不结束当前动作节点
                                    continue;

                                case ExcuteResult.Exception:

                                    System2RunTimeProvider.RecordTime("Bond线程", "返回：ExcuteResult.Exception");

                                    Machine.GetInstance().Stop();

                                    // 焊头去安全高度
                                    this.bondHeadController.MoveBondZToSafePos();

                                    if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
                                    {
                                        // 胶盘缩回
                                        this.slideFluxerController.SlideFluxerHomeWaitArrive();
                                    }

                                    return;

                                case ExcuteResult.Alarm:

                                    System2RunTimeProvider.RecordTime("Bond线程", "返回：ExcuteResult.Alarm");

                                    string message =
                                        $"系统2 ：{actionNode.Name} 流程运行失败！请选择如何处理！\r\n";

                                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                        message,
                                        "报警",
                                        new string[] { "终止", "重试", "忽略" },
                                        new DialogResult[] { DialogResult.Abort, DialogResult.Retry, DialogResult.Ignore },
                                        AlarmLevel.SecondLevel);

                                    switch (dialogResult)
                                    {
                                        case DialogResult.Abort:
                                            Machine.GetInstance().Stop();

                                            // 焊头去安全高度
                                            this.bondHeadController.MoveBondZToSafePos();

                                            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
                                            {
                                                // 胶盘缩回
                                                this.slideFluxerController.SlideFluxerHomeWaitArrive();
                                            }

                                            return;

                                        case DialogResult.Retry:
                                            goto RetryCommand;

                                        case DialogResult.Ignore:
                                            break;
                                    }

                                    break;
                            }
                        }

                        // 结束当前动作节点
                        this.system2Domain.ActionNodesService.FinishCurrentActionNode();

                        // Todo:点暂停Z轴抬起
                        //if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause)
                        //{
                        //    this.bondHeadController.MoveBondZToSafePos();
                        //}
                    }
                    #endregion
                }

                // Bond模组去安全位
                this.bondModuleController.MoveToSafePos();

                // 整个产品做完
                this.system2Domain.ActionNodesService.Finish();
                System2RunTimeProvider.RecordTime("Bond线程", "整个产品做完");

                DefectCompensate.NormalizationOffset(this.transportUnit.Name);
                System2RunTimeProvider.RecordTime("Bond线程", "归一化焊后补偿完成");

                if (System2Configuration.GetInstance().IsSaveEjectionTimeData)
                {
                    // 保存顶起缩回耗时
                    System2RunTimeProvider.SaveEjectionTime();
                    System2RunTimeProvider.RecordTime("Bond线程", "保存顶针耗时完成");
                }

                 this.SendAway();
                System2RunTimeProvider.RecordTime("Bond线程", "送走载具完成");

                UcMainSystem.ReFreshSystem2TuAction();
                System2RunTimeProvider.RecordTime("Bond线程", "刷新主界面UI显示完成");

                // 自动工作结束，把当前吸嘴放回吸嘴架，暂时还不知道怎么判断
            }
        }

        /// <summary>
        /// 固晶线程启动
        /// </summary>
        /// <returns>结果</returns>
        public bool Start()
        {
            if (this.BondWorkTask == null || this.BondWorkTask.Status != TaskStatus.Running)
            {
                this.BondWorkTask = Task.Factory.StartNew(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("固晶工作线程");
                            this.DoWork();
                        },
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }

            return true;
        }

        /// <summary>
        /// AbortThread
        /// </summary>
        public void AbortThread()
        {
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns>结果来料信号</returns>
        private bool GetIncomingSignal()
        {
            if (MachineStateModel.GetInstance().IsCompensateWork)
            {
                return true;
            }

            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                // TODO 应该还要执行预点胶
                return SignalPool.GetInstance().TransferToBondFinishedSignal.Wait();
            }
            else
            {
                if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit == null)
                {
                    DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                        @"未发现载具上有载具，请放上产品后继续工作",
                        $"警告",
                        new[] { "已放上产品", "停止" },
                        new[] { DialogResult.Yes, DialogResult.No });

                    if (dr == DialogResult.Yes)
                    {
                        TransportDomain.GetInstance().TransportController.BondSubSectionController.MapBelt(true);

                        if (TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit != null)
                        {

                            return true;
                        }
                        else
                        {
                            Machine.GetInstance().Stop();
                            return false;
                        }
                    }
                    else
                    {
                        Machine.GetInstance().Stop();
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// 送走载具
        /// </summary>
        private void SendAway()
        {
            TransportProvider.RecordTime("流道调试", "产品生产完成，准备送走载具");

            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                // 点停止不给流道发信号
                if (Machine.GetInstance().IsStop() == false)
                {
                    // 固精执行完成，告诉流道移走产品，等待产品流走
                    SignalPool.GetInstance().AllowBondTransferToWaitingUnloadSignal.Set();
                    TransportProvider.RecordTime("流道调试", "给流道发允许传送信号");
                }
                else
                {
                    TransportProvider.RecordTime("流道调试", "设备停止，不发允许传送信号");
                }
            }
            else
            {
                if (Machine.GetInstance().IsStop() == false)
                {
                    TransportProvider.RecordTime("流道调试", "准备提示人员拿走产品");

                    string message = string.Empty;

                    if (MachineSoftwareConfiguration.GetInstance().AutoSaveMapping)
                    {
                        TuService.SaveMappingToLocal(TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit);
                        message = "Mapping 成功保存,";
                        TransportProvider.RecordTime("流道调试", "保存产品Mapping成功");
                    }

                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit = null;
                    // TransportDomain.GetInstance().TransportController.BondSubSectionController.UnClamp();
                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.SubSectionState =
                        SubSectionStateEnum.NoMaterial;
                    AKRSMessageBoxExt.Show(@"此片载具已经生产完成," + message + "请手动拿走", $"警告", new[] { "已拿走" }, new[] { DialogResult.Yes });
                    Machine.GetInstance().Stop();
                    TransportProvider.RecordTime("流道调试", "人员已拿走产品");
                }
                else
                {
                    TransportProvider.RecordTime("流道调试", "设备停止，不发允许传送信号");
                }
            }
        }

        /// <summary>
        /// 关闭点胶气缸
        /// </summary>
        /// <param name="actionNode">动作</param>
        private void CloseDispenseCylinder(ActionNode actionNode)
        {
            if (!actionNode.IsCloseCylinder)
            {
                this.system2Domain.S2DispenseController.CloseDispenseHeightMeasurementCylinder();
            }
        }
    }
}
