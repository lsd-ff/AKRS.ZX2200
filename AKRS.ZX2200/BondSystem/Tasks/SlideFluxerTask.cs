using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

namespace AKRS.ZX2200.BondSystem.Tasks
{
    /// <summary>
    /// 刮胶盘线程
    /// </summary>
    public class SlideFluxerTask
    {
        /// <summary>
        /// BondDomain
        /// </summary>
        [JsonIgnore]
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 系统2配置
        /// </summary>
        private System2Configuration system2Configuration => System2Configuration.GetInstance();

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
        /// Bond Module 控制器
        /// </summary>
        [JsonIgnore]
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// Bond Module 控制器
        /// </summary>
        [JsonIgnore]
        private SlideFluxerController slideFluxerController => System2Domain.GetInstance().SlideFluxerController;

        /// <summary>
        /// Bond程式
        /// </summary>
        private SlideFluxerProgram slideFluxerProgram => BondProgram.GetInstance().SlideFluxerProgram;

        /// <summary>
        /// 刮胶盘参数
        /// </summary>
        private SlideFluxerParam slideFluxerParam =>
            BondDevicePara.GetInstance().SlideFluxerParam;

        /// <summary>
        /// Bond工作线程
        /// </summary>
        [JsonIgnore]
        public Task SlideFluxTask;

        /// <summary>
        /// 伸出超时计时
        /// </summary>
        private Stopwatch sp = new Stopwatch();

        /// <summary>
        /// 工作
        /// </summary>
        public void DoWork()
        {
            this.slideFluxerController.SlideFluxerHomeWaitArrive();

            // 总循环
            while (Signal.WaitStart())
            {
                while (System2RunTimeProvider.IsNeedDip)
                {
                    this.sp.Restart();

                    // 防撞：等焊头离开刮胶盘区域再伸出
                    while (true)
                    {
                        AKRSPoint3D leftTopPos =this.bondModuleController.ConvertG0ToMachinePos(this.slideFluxerParam.SlideFluxerLeftTopPos) ;
                        AKRSPoint3D rightBottom = this.bondModuleController.ConvertG0ToMachinePos(this.slideFluxerParam.SlideFluxerRightBottomPos);
                        if (GeometryService.IsBond2DPosInRange(leftTopPos, rightBottom) == false)
                        {
                            break;
                        }

                        if (Machine.GetInstance().IsPause())
                        {
                            this.sp.Restart();
                            continue;
                        }

                        if (sp.Elapsed.TotalSeconds > 15)
                        {
                            DialogResult dialog = AKRSMessageBoxExt.Show(
                                 $"刮胶盘工作线程：等焊头离开刮胶盘区域超时！ \r\n"  + "忽略：继续等待\r\n" + "终止：退出工作\r\n",
                                 "报警",
                                 new string[] { "忽略", "终止" },
                                 new DialogResult[] { DialogResult.Ignore, DialogResult.Abort });

                            if (dialog == DialogResult.Abort)
                            {
                                Machine.GetInstance().Stop();
                                return;
                            }
                            else 
                            {
                                this.sp.Restart();
                            }

                                //throw new Exception("刮胶盘工作线程：等焊头离开刮胶盘区域超时！");
                        }

                        Thread.Sleep(2);
                    }

                    // 胶盘伸出
                    this.slideFluxerController.SlideFluxerOutWaitArrive();

                    // 给Bond发允许蘸胶信号
                    SignalPool.GetInstance().IsAllowDipFluxSignal.Set();

                    // 等蘸胶完成信号
                    if (!SignalPool.GetInstance().BondDipFluxFinishSignal.Wait())
                    {
                        // 胶盘缩回
                        this.slideFluxerController.SlideFluxerHomeWaitArrive();
                        return;
                    }

                    this.slideFluxerController.SlideFluxerHomeWaitArrive();

                    // 改成在固精位的时候缩回，所以不要判断安全高度
                    //// todo:这里抬高2.5临时加的
                    //double safeLevel = this.bondModuleController.ConvertG0ToMachinePos(
                    //                       BondDevicePara.GetInstance().SlideFluxerParam
                    //                           .SlideFluxerRightBottomPos).Z + this.bondHeadController
                    //                       .GetCurrentNozzle().MeasureHeightOffset+2.5;

                    //this.sp.Restart();

                    //// 等抬到安全高度
                    //while (true)
                    //{
                    //    if (this.bondHeadController.GetAxisZRealPos() > safeLevel)
                    //    {
                    //        this.slideFluxerController.SlideFluxerHomeWaitArrive();
                    //        break;
                    //    }

                    //    if (this.sp.Elapsed.TotalSeconds > 10)
                    //    {
                    //        if (Machine.GetInstance().IsPause())
                    //        {
                    //            this.sp.Restart();
                    //            Thread.Sleep(20);
                    //            continue;
                    //        }

                    //        DialogResult dialog = AKRSMessageBoxExt.Show(
                    //            $"刮胶盘缩回超时！",
                    //            "刮胶线程报警",
                    //            new string[] { "忽略", "终止" },
                    //            new DialogResult[] { DialogResult.Ignore, DialogResult.Abort },
                    //            AlarmLevel.SecondLevel);

                    //        switch (dialog)
                    //        {
                    //            case DialogResult.Ignore:
                    //                this.sp.Restart();

                    //                break;

                    //            case DialogResult.Abort:
                    //                Machine.GetInstance().Stop();
                    //                return;
                    //        }
                    //    }
                    //}

                    // 等刮胶盘允许伸出信号
                    if (!SignalPool.GetInstance().AllowSlideOutSignal.Wait())
                    {
                        return;
                    }

                    Thread.Sleep(40);
                }

                Thread.Sleep(20);
            }

            // 胶盘缩回?
            this.slideFluxerController.SlideFluxerHomeWaitArrive();
        }

        /// <summary>
        /// 刮胶线程启动
        /// </summary>
        /// <returns>结果</returns>
        public bool Start()
        {
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
            {
                throw new Exception("配置异常：未配置刮胶盘！");
            }

            if (this.SlideFluxTask == null || this.SlideFluxTask.Status != TaskStatus.Running)
            {
                // 注释：贺强， 在设备开始按钮里有改变机台状态这句 所以这里先注释掉
                //MachineStateModel.GetInstance().MachineState = Galaxy2.Machine.Enums.MachineStateEnum.Working;
                this.SlideFluxTask = Task.Factory.StartNew(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("刮胶盘线程");
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
