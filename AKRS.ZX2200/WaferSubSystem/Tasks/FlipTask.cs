using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Services;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Tasks
{
    /// <summary>
    ///  翻转台线程
    /// </summary>
    public class FlipTask
    {
        /// <summary>
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 翻转台线程
        /// </summary>
        public Task FlipTableTask;

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// 芯片,翻转台只考虑单芯片制作，所以这里获取当前颗和下一颗都是对的
        /// </summary>
        private BaseCarrierConfig component ;

        /// <summary>
        /// 工作
        /// </summary>
        public void DoWork()
        {          
            this.flipTableController.FlipTableGoHome();

            // 总循环
            while (Signal.WaitStart())
            {
                this.component = this.actionNodesService.GetCurrentComponent().component;

                if (this.component == null)
                {
                    Thread.Sleep(100);

                    //if (sw.Elapsed.TotalMinutes > 1)
                    //{
                    //    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    //                        $"翻转台获取当前芯片超时！ \r\n忽略：继续等待\r\n终止：退出工作",
                    //                        "翻转台报警",
                    //                        new string[] { "忽略", "终止" },
                    //                        new DialogResult[]
                    //                            {
                    //                            DialogResult.Ignore, DialogResult.Abort
                    //                                    },
                    //                        AlarmLevel.SecondLevel);

                    //    if (dialogResult == DialogResult.Abort)
                    //    {
                    //        Machine.GetInstance().Stop();
                    //        return;
                    //    }
                    //}

                    continue;
                }

                // 翻转台取片动作
                if (this.component.IsUseFlipTable)
                {
                    // 判断翻转吸嘴上有无芯片
                    if (Static.IsComponentOnFlipTool == false) 
                    {
                    NextDie:

                        // 等晶圆允许取料信号
                        if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
                        {
                            return;
                        }

                        // 通知晶圆搜晶
                        Action notifyWafer = new Action(() =>
                        {
                            // 异步判断翻转台位置，如果大于60度就允许搜晶
                            Task.Run(
                                () =>
                                {
                                    CommonUtil.SetCurrentThreadName("翻转台给晶圆发要料信号线程");

                                    Stopwatch sw = Stopwatch.StartNew();

                                    while (true)
                                    {
                                        double curPos = this.flipTableController.GetTRealPos();

                                        if (curPos > WaferSubDevicePara.GetInstance().FlipChipDevicePara.SearchComponentAvoidancePos)
                                        {
                                            string nextComponentName = actionNodesService.GetNextComponentName();

                                            // 判断是不是最后一颗芯片
                                            if (nextComponentName != null)
                                            {
                                                if (this.component.CarrierType == CarrierTypeEnum.Wafer)
                                                {
                                                    // 顶针缩回
                                                    WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                                                }
                     

                                                // 发要料信号
                                                // 刷新Map信息
                                                WaferSystemDomain.GetInstance().Block.RefreshMap();

                                                // 芯片名称传给晶圆台
                                                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                                                // 给晶圆台发要料信号
                                                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                                                Static.RecordTime("取片信号交互", $"取片完成，给晶圆发要料信号，芯片名称{component.Name}");
                                            }

                                            break;
                                        }

                                        if (sw.ElapsedMilliseconds > 10000)
                                        {
                                            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                            $"翻转台发送搜晶信号超时！ \r\n忽略：继续等待\r\n终止：退出工作",
                                            "翻转台报警",
                                            new string[] { "忽略", "终止" },
                                            new DialogResult[]
                                                {
                                                DialogResult.Ignore, DialogResult.Abort
                                                        },
                                            AlarmLevel.SecondLevel);

                                            switch (dialogResult)
                                            {
                                                case DialogResult.Ignore:
                                                    sw.Restart();
                                                    break;

                                                case DialogResult.Abort:
                                                    Machine.GetInstance().Stop();
                                                    return;
                                            }
                                        }

                                        // 停止退出
                                        if (Machine.GetInstance().IsStop())
                                        {
                                            return;
                                        }

                                        Thread.Sleep(10);
                                    }
                                });

                        });

                        // todo: 判断焊头位置

                        Static.RecordTime("取片信号交互", $"开始执行翻转动作");
                        ExcuteResult ret ;

                        // 空跑模式
                        if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                        {
                            ret = WaferSubController.GetInstance().FlipChipActionDryRun(this.component, notifyWafer);
                        }
                        else
                        {
                            ret = WaferSubController.GetInstance().FlipChipAction(this.component, notifyWafer);
                        }

                        switch (ret)
                        {
                            case ExcuteResult.Success:
                                Static.IsComponentOnFlipTool = true;
                                break;

                            case ExcuteResult.Exception:

                                if (this.component.CarrierType == CarrierTypeEnum.Wafer)
                                {
                                    // 顶针缩回
                                    WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                                }                          

                                // 设备停止
                                return;

                            case ExcuteResult.Abort:

                                if (this.component.CarrierType == CarrierTypeEnum.Wafer)
                                {
                                    // 顶针缩回
                                    WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                                }

                                return;

                            // 取下一颗
                            case ExcuteResult.NoStart:
                                goto NextDie;
                        }
                    }           

                    // 发送Bond允许取料信号
                    SignalPool.GetInstance().IsFlipTableAllowPickSignal.Set();

                    // 等Bond取料成功信号
                    if (!SignalPool.GetInstance().IsBondPickSucceedSignal.Wait())
                    {
                        return;
                    }

                    Static.RecordTime("取片信号交互", $"取片完成，接收到Bond取料成功信号，芯片名称{component.Name}");
                }
                //else
                //{
                //    //// 不使用翻转台直接退出
                //    //return;
                //}
            }

            this.flipTableController.FlipTableGoHome();
        }

        /// <summary>
        /// 翻转台线程启动
        /// </summary>
        /// <returns>结果</returns>
        public bool Start()
        {
            if (this.FlipTableTask == null || this.FlipTableTask.Status != TaskStatus.Running)
            {
                Machine.GetInstance().SetState(MachineStateEnum.Working);
                this.FlipTableTask = Task.Factory.StartNew(
                    () =>
                    {
                        CommonUtil.SetCurrentThreadName("翻转台线程");
                        this.DoWork();
                    },
                    CancellationToken.None,
                    TaskCreationOptions.LongRunning,
                    TaskScheduler.Default);
            }

            return true;
        }
    }
}
