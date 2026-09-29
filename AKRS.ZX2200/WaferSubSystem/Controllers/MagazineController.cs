using System;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using Newtonsoft.Json;

namespace AKRS.ZX2200.WaferSubSystem.Controllers
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// MagazineController
    /// </summary>
    public class MagazineController
    {
        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// 允许误差
        /// </summary>
        private double range = 1;

        /// <summary>
        /// Magazine模组
        /// </summary>
        [JsonIgnore]
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// Magazine设备参数
        /// </summary>
        [JsonIgnore]
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// Magazine当前位置
        /// </summary>
        [JsonIgnore]
        public AKRSPoint3D MagazineAxisZG0Pos
        {
            get
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured==false)
                {
                    return new AKRSPoint3D(0, 0, 0);
                }

                return MachineCoordinateSystem.GetInstance().NullCoordinateSystem.SelfPosToG0(new AKRSPoint3D(0, 0, this.MagazineBoxModule.MagazineAxisZ.GetRealPosition()));
            }
        }

        /// <summary>
        /// 移动magazine到安全位置
        /// </summary>
        public void MoveMagazineToSafePosition()
        {
            if (Math.Abs(this.MagazineAxisZG0Pos.Z - MagazineBoxDevicePara.SafePosition.Z) > this.range)
            {
                this.MoveMagazineAxisZToG0Pos(MagazineBoxDevicePara.SafePosition);
            }
        }

        /// <summary>
        /// 移动magazine到mark位置
        /// </summary>
        public void MoveMagazineToMarkPosition()
        {
            if (Math.Abs(this.MagazineAxisZG0Pos.Z - MagazineBoxDevicePara.MarkPosition.Z) > this.range)
            {
                this.MoveMagazineAxisZToG0Pos(MagazineBoxDevicePara.MarkPosition);
            }
        }

        /// <summary>
        /// 移动magazine到某个槽位
        /// </summary>
        /// <param name="i">i</param>
        public void MoveMagazineToSlotPosition(int i)
        {
            this.MoveMagazineAxisZToG0Pos(MagazineBoxDevicePara.Position[i]);
        }

        /// <summary>
        /// 推料气缸推出
        /// </summary>
        public void SetWaferPushCylinder()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            WaferSubController.GetInstance().SetCyc(MagazineBoxModule.WaferPushCylinder, MagazineBoxModule.WaferPushCylinderPLimitSensor, 0, MagazineBoxDevicePara.DelayPushCycActionAfter);
            //if (Static.CurrentBoxConfig.Push)
            //{
            //    WaferSubController.GetInstance().SetCyc(MagazineBoxModule.PushCyc, MagazineBoxModule.PushTarSensor, MagazineBoxDevicePara.DelayPushCycAction);
            //}

        }

        /// <summary>
        /// 推料气缸缩回
        /// </summary>
        public void ResetWaferPushCylinder()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            WaferSubController.GetInstance().ResetCyc(MagazineBoxModule.WaferPushCylinder, MagazineBoxModule.WaferPushCylinderNLimitSensor, 0, MagazineBoxDevicePara.DelayPushCycActionAfter);
            //if (Static.CurrentBoxConfig.Push)
            //{
            //    WaferSubController.GetInstance().ResetCyc(MagazineBoxModule.PushCyc, MagazineBoxModule.PushIniSensor, MagazineBoxDevicePara.DelayPushCycAction);
            //}
        }

        /// <summary>
        /// 夹紧提篮
        /// </summary>
        public void SetMagazineFixedCylinder()
        {
            WaferSubController.GetInstance().SetCyc(
                MagazineBoxModule.FixedCylinder,
                MagazineBoxModule.FixedCylinderPLimitSensor,
                0,
                10);
        }

        /// <summary>
        /// 放松提篮
        /// </summary>
        public void ResetMagazineFixedCylinder()
        {
            WaferSubController.GetInstance().ResetCyc(
                MagazineBoxModule.FixedCylinder,
                MagazineBoxModule.FixedCylinderNLimitSensor,
                0,
                10);
        }

        /// <summary>
        /// 判断所需芯片在哪一层
        /// </summary>
        /// <param name="chipName">芯片名称</param>
        /// <returns>layerIndex</returns>
        public int JudgeNeedChipAtLayerIndex(string chipName)
        {
            for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount; i++)
            {
                if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].SlotState == SlotStatuEnum.Good)
                {
                    if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is WaferTablet)
                    {
                        WaferTablet wt = (WaferTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
                        if (CarrierConfigRepository.GetInstance().IsExists(wt.Name))
                        {
                            if (wt.Name == chipName)
                            {
                                return i;
                            }
                        }
                    }
                    else if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is AdapterTablet)
                    {
                        AdapterTablet ad = (AdapterTablet)WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i];
                        if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                        {
                            for (int j = 0; j < ad.AdapterSetting.MaxUseWaffleCount; j++)
                            {
                                if (CarrierConfigRepository.GetInstance().IsExists(ad.AdapterSetting.WaffleArray[j].Name))
                                {
                                    if (ad.AdapterSetting.WaffleArray[j].Name == chipName && ad.AdapterState.WafflePlateSlotsState[j].SlotState == SlotStatuEnum.Good)
                                    {
                                        return i;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 判断所需芯片在适配器哪一盒
        /// </summary>
        /// <param name="chipName">芯片名称</param>
        /// <returns>slotIndex</returns>
        public int JudgeNeedChipAtAdapterSlotIndex(string chipName)
        {
            BaseCarrierConfig currentNeedCarrierConfig;
            if (WaferSystemProgram.GetInstance().GetCarriers().Exists(a => a.Name == chipName))
            {
                currentNeedCarrierConfig =
                    (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(chipName);
            }
            else
            {
                //throw new Exception($"The required pieces do not exist -{chipName}");
                throw new Exception($"不存在Bond需要的料片 -{chipName}");
            }

            if (currentNeedCarrierConfig.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet is AdapterTablet)
                {
                    AdapterTablet ad = (AdapterTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet;
                    if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                    {
                        for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
                        {
                            if (CarrierConfigRepository.GetInstance()
                                .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
                            {
                                if (ad.AdapterSetting.WaffleArray[i].Name == chipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet is AdapterTablet)
                {
                    AdapterTablet ad = (AdapterTablet)WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentTablet;
                    if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                    {
                        for (int i = 0; i < ad.AdapterSetting.MaxUseWaffleCount; i++)
                        {
                            if (CarrierConfigRepository.GetInstance()
                                .IsExists(ad.AdapterSetting.WaffleArray[i].Name))
                            {
                                if (ad.AdapterSetting.WaffleArray[i].Name == chipName && ad.AdapterState.WafflePlateSlotsState[i].SlotState == SlotStatuEnum.Good)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// 槽位扫描
        /// </summary>
        public void SlotScan()
        {
            for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount; i++)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState = true;
                    continue;
                }

                this.MoveMagazineToSlotPosition(i);

                // 如果感应到了，查看该层是否有料
                bool ret = MagazineBoxModule.SlotScanSensor.CheckStateForNums(true, 3, 200);
                if (ret)
                {
                    if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is NullTablet)
                    {
                        WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState = false;
                    }
                    else
                    {
                        WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState = true;
                    }
                }
                else
                {
                    if (!(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i] is NullTablet))
                    {
                        WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState = false;
                    }
                    else
                    {
                        WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState = true;
                    }
                }
            }

            MagazineAllocationsConfigRepository.GetInstance().Save();
            for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount; i++)
            {
                if (!WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].IsOKSlotState)
                {
                    //throw new Exception("Material layer status Error detected, please check material layer status!");
                    throw new Exception("料层状态检测错误, 请人工确认料层状态后重试！");
                }
            }
        }

        /// <summary>
        /// 扫描某一个槽位
        /// </summary>
        /// <param name="index">index</param>
        public void SlotScan(int index)
        {
            this.MoveMagazineToSlotPosition(index);

            // 增加料盒感应判断
            //CheckMagazine();

            void CheckMagazine()
            {
                bool isExistMagazine = MagazineBoxModule.MagazineCheckSensor.CheckStateForNums(true, 3, 200);
                if (!isExistMagazine)
                {
                    this.res = AKRSXtraMessageBox.Show($"未检测到提篮，请确认提篮是否存在？\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    switch (this.res)
                    {
                        case DialogResult.OK:
                            CheckMagazine();
                            break;
                        case DialogResult.Cancel:
                            throw new Exception($"未检测到提篮！");
                    }
                }
            }
            
            return;

            if (MachineStateModel.GetInstance().IsNormalWork)
            {
                bool ret = MagazineBoxModule.SlotScanSensor.CheckStateForNums(true, 3, 200);
                if (ret)
                {
                    if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[index] is NullTablet)
                    {
                        //this.res = AKRSXtraMessageBox.Show($"The sensor detects a tablet, but there is no a tablet in the program. The LayerNo = {index + 1}\r\n" + "Please check the sheet in slot with manually!\r\n" + "Click OK: continue,\r\n" + "Click Cancel: stop", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        this.res = AKRSXtraMessageBox.Show($"感应器检测到一个料片, 但是magazine记忆并没有； 层号 = {index + 1}\r\n" + "请手动检查该层料片的状态！\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        switch (this.res)
                        {
                            case DialogResult.OK:
                                break;
                            case DialogResult.Cancel:
                                //throw new Exception($"Check tablet in slot failed!");
                                throw new Exception($"检查料片状态失败！");
                        }
                    }
                }
                else
                {
                    if (!(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[index] is NullTablet))
                    {
                        //this.res = AKRSXtraMessageBox.Show($"The sensor does not detect a tablet, but there is a tablet in the program. The LayerNo = {index + 1}\r\n" + "Please check the sheet in slot with manually!\r\n" + "Click OK: continue,\r\n" + "Click Cancel: stop", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        this.res = AKRSXtraMessageBox.Show($"感应器没有检测到一个料片, 但是magazine记忆有； 层号 = {index + 1}\r\n" + "请手动检查该层料片的状态！\r\n" + "点击 OK: 继续,\r\n" + "点击 Cancel: 停止", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        switch (this.res)
                        {
                            case DialogResult.OK:
                                break;
                            case DialogResult.Cancel:
                                //throw new Exception($"Check tablet in slot failed!");
                                throw new Exception($"检查料片状态失败！");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 动作推料气缸
        /// </summary>
        public void ActionPushCyc()
        {
            this.SetWaferPushCylinder();
            Thread.Sleep(MagazineBoxDevicePara.DelayPushHold);
            this.ResetWaferPushCylinder();
        }

        /// <summary>
        /// 获取晶圆推料气缸状态
        /// </summary>
        /// <returns>result</returns>
        public bool GetWaferPushCylinder()
        {
            return this.MagazineBoxModule.WaferPushCylinder.CurrentOutputValue;
        }

        /// <summary>
        /// 移动上晶圆Z轴电机到指定位置
        /// </summary>
        /// <param name="g0Pos">g0Pos</param>
        public void MoveMagazineAxisZToG0Pos(AKRSPoint3D g0Pos)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsMagazineConfigured == false)
            {
                return;
            }

            WaferSystemDomain.GetInstance().SingleStep();

            WaferSubController.GetInstance().WaferTableController.CheckWaferTableAllowMagazineAction();
            WaferSubController.GetInstance().WaferTableController.CheckWaferClampAllowMagazineAction();
            this.CheckWaferPushCylinderAllowMagazineLiftAction();
            MovePara movePara = new MovePara()
                                    {
                                        Vel = this.MagazineBoxModule.MagazineAxisZ.AxisMovePara.AbsoluteMoveSpeed * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage,
                                        Acc = this.MagazineBoxModule.MagazineAxisZ.AxisMovePara.ACC,
                                        Dec = this.MagazineBoxModule.MagazineAxisZ.AxisMovePara.DEC,
                                        Jerk = this.MagazineBoxModule.MagazineAxisZ.AxisMovePara.Jerk,
                                        TargetPosition = MachineCoordinateSystem.GetInstance().NullCoordinateSystem.G0PosToSelf(g0Pos).Z
                                    };
            ExcuteResult ret = this.MagazineBoxModule.MagazineAxisZ.AbsoluteMove(movePara, false, AccuracyMode.HighSpeed);
            if (ret != ExcuteResult.Success)
            {
                //throw new Exception("MagazineAxisZ positioning failure.");
                throw new Exception("上晶圆移动到目标位置失败！");
            }
        }

        /// <summary>
        /// magazine电机是否在安全位置
        /// </summary>
        public void CheckMagazineAxisZAtSafePosition()
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                return;
            }

            if (Math.Abs(this.MagazineAxisZG0Pos.Z - MagazineBoxDevicePara.SafePosition.Z) > this.range)
            {
                //throw new Exception("MagazineAxisZ not at SafePosition.");
                DialogResult res = AKRSMessageBoxExt.Show($"上晶圆不在安全位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.MoveMagazineToSafePosition();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("上晶圆不在安全位置！");
                }
            }
        }

        /// <summary>
        /// 推料气缸允许MagazineLift动作
        /// </summary>
        public void CheckWaferPushCylinderAllowMagazineLiftAction()
        {
            if (!this.MagazineBoxModule.WaferPushCylinderNLimitSensor.CheckStateForNums(true, 3, 200))
            {
                //throw new Exception("Push cylinder is not in negative limit!");
                DialogResult res = AKRSMessageBoxExt.Show($"料片推料气缸不在缩回的位置！" + "\r\n" + "点击OK，即将移动到位，" + "\r\n" + "点击Abort，即将终止！", "Prompt", new string[] { "OK", "Abort" }, new DialogResult[] { DialogResult.OK, DialogResult.Abort });
                switch (res)
                {
                    case DialogResult.OK:
                        this.ResetWaferPushCylinder();
                        return;
                    case DialogResult.Abort:
                        throw new Exception("料片推料气缸不在缩回的位置！");
                }
            }
        }
    }
}
