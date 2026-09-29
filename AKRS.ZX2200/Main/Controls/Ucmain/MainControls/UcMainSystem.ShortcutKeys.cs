namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Controllers;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.XtraEditors;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// 主界面快捷键
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 视觉定位窗体
        /// </summary>
        private Task visionTask;

        /// <summary>
        /// 流道传输控制器
        /// </summary>
        private readonly TransportController transportController = new TransportController();

        /// <summary>
        /// 回车触发事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void UcMainSystem_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F9)
            {
                if (MachineStateModel.GetInstance().IsSingleStepWork)
                {
                    if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                    {
                        // 单步执行
                        SignalPool.GetInstance().System2SingleStepSignal.Set();
                    }
                    else
                    {
                        // 单步执行
                        SignalPool.GetInstance().System1SingleStepSignal.Set();
                    }
                }
            }

            if (Control.ModifierKeys == Keys.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.F1:
                        this.SubstrateIndex();
                        break;
                    case Keys.F2:
                        MachineStateModel.GetInstance().VisionPreviousBondPositionSystem1();
                        break;
                    case Keys.F3:
                        MachineStateModel.GetInstance().VisionPreviousBondPositionSystem2();
                        break;
                    case Keys.F4:

                        if (!this.IsVisionScanIsReady())
                        {
                            return;
                        }

                        if (this.visionTask == null)
                        {
                            this.visionTask = new Task(this.VisionScan);
                        }

                        if (this.visionTask.Status == TaskStatus.Running)
                        {
                            return;
                        }

                        this.visionTask = new Task(this.VisionScan);
                        this.visionTask.Start();

                        break;
                    case Keys.F5:
                        this.ShowForm<FrmVmVisionResult>();
                        break;
                    case Keys.F6:
                        break;
                    case Keys.F7:
                        break;
                    case Keys.F8:
                        break;
                    case Keys.F9:
                        MachineStateModel.GetInstance().IsSingleStepWork =
                            !MachineStateModel.GetInstance().IsSingleStepWork;

                        break;
                    case Keys.F10:
                        break;
                    case Keys.F11:
                        break;
                    case Keys.F12:
                        break;
                }
            }
        }

        /// <summary>
        /// 定位扫描
        /// 受mapping影响
        /// </summary>
        private void VisionScan()
        {
            Machine.GetInstance().SetState(MachineStateEnum.Working);

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                transportUnit.SetSystem2OffSet();

                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
                {
                    System2Domain.System2MatterVision(transportUnit);
                    if (!Signal.WaitStart())
                    {
                        return;
                    }
                }

                System2Domain.System2ObjectVision(transportUnit);
                if (!Signal.WaitStart())
                {
                    return;
                }

                foreach (Substrate substrate in transportUnit.Substrates)
                {
                    if (substrate.MatterProductState != MatterProductState.Disable)
                    {
                        System2Domain.System2ObjectVision(substrate);
                    }

                    if (!Signal.WaitStart())
                    {
                        return;
                    }

                    foreach (Module module in substrate.Modules)
                    {
                        if (module.MatterProductState != MatterProductState.Disable)
                        {
                            System2Domain.System2ObjectVision(module);
                        }

                        if (!Signal.WaitStart())
                        {
                            return;
                        }

                        foreach (BondPosition bondPosition in module.BondPositions)
                        {
                            if (bondPosition.MatterProductState != MatterProductState.Disable)
                            {
                                System2Domain.System2ObjectVision(bondPosition);
                            }

                            if (!Signal.WaitStart())
                            {
                                return;
                            }
                        }
                    }
                }

                Machine.GetInstance().Stop();
                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"载具定位扫描完成\r\n",
                    "提示",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Yes },
                    AlarmLevel.SecondLevel);
            }
            else
            {
                transportUnit.SetSystem1OffSet();
                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
                {
                    System1Domain.GetInstance().System1MatterVision(transportUnit);
                    if (!Signal.WaitStart())
                    {
                        return;
                    }
                }

                List<int> list = new List<int>();

                if (TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.IsTuInDispense1)
                {
                    list = ProductConfiguration.GetInstance().SubstrateConfig.GetBackSubstrateIndex();
                }
                else
                {
                    list = ProductConfiguration.GetInstance().SubstrateConfig.GetFrontSubstrateIndex();
                    transportUnit.System1MoveDistance();
                }

                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing != SubstrateProcessingEnum.Divide)
                {
                    list = transportUnit.Substrates.Select(it => it.Index).ToList();
                }

                for (int i = 0; i < list.Count; i++)
                {
                    Substrate substrate = transportUnit.Substrates.Find(it => it.Index == list[i]);

                    // 如果TU是分段的情况下，需要去执行
                    if (substrate.MatterProductState != MatterProductState.Disable)
                    {
                        System1Domain.GetInstance().System1MatterVision(substrate);
                    }

                    if (!Signal.WaitStart())
                    {
                        return;
                    }

                    foreach (Module module in substrate.Modules)
                    {
                        if (module.MatterProductState != MatterProductState.Disable)
                        {
                            System1Domain.GetInstance().System1MatterVision(module);
                        }

                        if (!Signal.WaitStart())
                        {
                            return;
                        }

                        foreach (BondPosition bondPosition in module.BondPositions)
                        {
                            if (!Signal.WaitStart())
                            {
                                return;
                            }

                            if (bondPosition.MatterProductState != MatterProductState.Disable)
                            {
                                System1Domain.GetInstance().System1MatterVision(bondPosition);
                            }
                        }
                    }
                }

                Machine.GetInstance().Stop();
                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"载具定位扫描完成\r\n",
                    "提示",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Yes },
                    AlarmLevel.SecondLevel);
            }
        }

        /// <summary>
        /// 判断视觉扫描的前提条件满足
        /// </summary>
        /// <returns>结果</returns>
        private bool IsVisionScanIsReady()
        {
            // 判断设备有没有准备好
            if (!Machine.GetInstance().MachineInitSuccess())
            {
                return false;
            }

            // 判断机台状态,不是暂停不能启动
            if (Machine.GetInstance().IsWorking() || Machine.GetInstance().IsPause())
            {
                AKRSXtraMessageBox.Show("Machine state is not Stop, can not start");
                return false;
            }

            // 判断当前系统上面有没有材料
            if (TUAssistantHelper.JudgeTuExist() == null)
            {
                return false;
            }

            // 判断产品是否示教完成
            if (!ProductConfiguration.GetInstance().TransportUnitConfig.IsAssistantSucceed)
            {
                AKRSXtraMessageBox.Show("产品框架未示教完成，不能执行定位");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 流道单布传送
        /// </summary>
        private void SubstrateIndex()
        {
            Task t1 = Task.Run(() =>
                {
                    this.transportController.TransferLoadBeltToDispense(true);
                });

            Task t2 = Task.Run(() => this.transportController.TransferDispenseToBond(true));
            Task t3 = Task.Run(() => this.transportController.TransferBondToWaitingUnload(true));
            Task t4 = Task.Run(this.transportController.TransferWaitingUnloadToUnloadBelt);

            t1.Wait();
            t2.Wait();
            t3.Wait();
            t4.Wait();
        }
    }
}
