namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Parameter;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    using ch.etel.edi.dsa.v40;

    using DevExpress.Utils;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 自检
    /// </summary>
    public partial class Machine
    {
        /// <summary>
        /// 设备参数
        /// </summary>
        private MachineDevicePara MachineDevicePara => MachineDevicePara.GetInstance();

        /// <summary>
        /// 设备自检
        /// </summary>
        public void SelfCheck()
        {
            try
            {
                BondModule bondModule = new BondModule();

                // 回零之前必须把顶针固定气缸复位，否则顶针升降电机下降时，会拉弯硬件
                //WaferSubController.GetInstance().EjectController.ResetFixedCylinder();

                using (WaitDialogForm sdf = new WaitDialogForm("Self-Check", "Self-Check... Please Wait.", new Size(500, 100)))
                {
                    // 设备回零
                    //DsaDrive moveDriveX = ((ETELAxis)bondModule.BondAxisX.AxisDrive).GetDrive();
                    //DsaDrive moveDriveY = ((ETELAxis)bondModule.BondAxisY.AxisDrive).GetDrive();

                    //DsaDrive[] dsaDrives = new DsaDrive[] { moveDriveX, moveDriveY };

                    //DsaDriveGroup grp = new DsaDriveGroup(dsaDrives);

                    //if (grp.stageMappingIsActivated())
                    //{
                    //    grp.stageMappingDeactivate();
                    //}

                    sdf.Caption = "Axis initialize is self-checking";
                    if (!HardwareRepositoryService.OneKeyHome())
                    {
                        throw new Exception("Axis initialize failed!");
                    }

                    //grp.stageMappingActivate();

                    List<Light> lights = HardwareRepositoryService.GetHardwaresByType<Light>();
                    lights.ForEach(
                        a =>
                        {
                            sdf.Caption = a.HardwareName + " is self-checking";
                            a.SetIntensity(200);
                            Thread.Sleep(2000);
                            a.SetIntensity(0);
                        });

                    #region WaferSubSystem

                    // 气缸动作
                    {
                        // 晶圆夹气缸
                        try
                        {
                            sdf.Caption = "ClampCyc is self-checking";
                            WaferSubController.GetInstance().WaferTableController.SetWaferClampCylinder();
                            WaferSubController.GetInstance().WaferTableController.ResetWaferClampCylinder();
                        }
                        catch (Exception exception)
                        {
                            AKRSXtraMessageBox.Show("ClampCyc action failed!" + exception.Message);
                        }

                        // 晶圆夹持气缸
                        try
                        {
                            sdf.Caption = "BlockCyc is self-checking";
                            WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                            WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("BlockCyc action failed!" + exception.Message);
                        }

                        // 晶圆推料气缸
                        try
                        {
                            sdf.Caption = "PushCyc is self-checking";
                            WaferSubController.GetInstance().MagazineController.SetWaferPushCylinder();
                            WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("PushCyc action failed!" + exception.Message);
                        }

                        // 顶针固定气缸
                        try
                        {
                            sdf.Caption = "FixedCyc is self-checking";
                            WaferSubController.GetInstance().EjectController.SetFixedCylinder();
                            WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("FixedCyc action failed!" + exception.Message);
                        }
                    }

                    // 真空吹气
                    {
                        try
                        {
                            sdf.Caption = "Ejection inhale Elect is self-checking";
                            WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Ejection inhale Elect action failed!" + exception.Message);
                        }

                        try
                        {
                            sdf.Caption = "Ejection blow Elect is self-checking";
                            WaferSubController.GetInstance().EjectController.OpenEjectionTableBlow();
                            WaferSubController.GetInstance().EjectController.CloseEjectionTableBlow();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Ejection blow Elect action failed!" + exception.Message);
                        }
                    }

                    // 电机微运动
                    {
                        // 晶圆台
                        try
                        {
                            sdf.Caption = "Wafer table axisXY is self-checking";
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Wafer table axisXY move failed!" + exception.Message);
                        }

                        // 顶针架旋转
                        try
                        {
                            sdf.Caption = "Eject table axisT is self-checking";
                            WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(1);
                            Thread.Sleep(2000);
                            WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(0);
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Eject table axisT move failed!" + exception.Message);
                        }

                        // 顶针Z
                        try
                        {
                            sdf.Caption = "Eject axisZ is self-checking";
                            WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(WaferSubDevicePara.GetInstance().EjectDevicePara.EjectSafePosition + 2);
                            WaferSubController.GetInstance().EjectController.MoveEjectToSafePosition();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Eject axisZ move failed!" + exception.Message);
                        }

                        // 顶针台升降
                        try
                        {
                            sdf.Caption = "Eject table axisZ is self-checking";
                            WaferSubController.GetInstance().EjectController.MoveEjectionTableAxisZToG0Pos(WaferSubDevicePara.GetInstance().EjectDevicePara.UpDownSafePosition + 20);
                            WaferSubController.GetInstance().EjectController.MoveEjectionTableToSafePosition();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Eject table axisZ move failed!" + exception.Message);
                        }

                        // 晶圆夹Y轴
                        try
                        {
                            sdf.Caption = "Wafer Clamp axisY is self-checking";
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToAutoChangePosition();
                            WaferSubController.GetInstance().WaferTableController.MoveWaferClampToWaitPickPosition();
                            WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Wafer Clamp axisY move failed!" + exception.Message);
                        }

                        // 上晶圆Z轴
                        try
                        {
                            sdf.Caption = "Magazine axisZ is self-checking";
                            WaferSubController.GetInstance().MagazineController.MoveMagazineAxisZToG0Pos(WaferSubDevicePara.GetInstance().MagazineDevicePara.SafePosition + 10);
                            WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Magazine axisZ move failed!" + exception.Message);
                        }
                    }

                    // 相机触发拍照
                    {
                        // 晶圆相机触发拍照
                        try
                        {
                            sdf.Caption = "Wafer camera is self-checking";

                            // Static.Block.MatchResult();
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("Wafer camera action failed!" + exception.Message);
                        }
                    }

                    #endregion

                    #region BondSystem

                    #region 轴测试

                    // BondZ
                    try
                    {
                        sdf.Caption = "System  2  axisZ is self-checking";
                        System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  axisZ  move failed!" + exception.Message);
                    }

                    // 吸嘴架Y
                    try
                    {
                        sdf.Caption = "System  2  tool  bank  axisY is self-checking";
                        System2Domain.GetInstance().NozzleShelfController.MoveShelfToChangeNozzlePos();
                        System2Domain.GetInstance().NozzleShelfController.MoveShelfToHome();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  tool  bank  axisY  move failed!" + exception.Message);
                    }

                    // BondXY
                    try
                    {
                        sdf.Caption = "System  2  axisXY is self-checking";
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  axisXY  move failed!" + exception.Message);
                    }

                    // BondT
                    try
                    {
                        sdf.Caption = "System  2  axisT is self-checking";
                        System2Domain.GetInstance().BondHeadController.RotateAxisT(0);
                        System2Domain.GetInstance().BondHeadController.RotateAxisT(180);
                        System2Domain.GetInstance().BondHeadController.RotateAxisT(0);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  axisT  move failed!" + exception.Message);
                    }

                    #endregion

                    // 测高
                    try
                    {
                        sdf.Caption = "System  2  height  measurement is self-checking";

                        if (System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
                        {
                            // Bond测高
                            AKRSPoint3D measurePos = CalibrateRunPara.GetInstance().BMCMeasureHeightSearchMachinePos;

                            System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();
                            bondModule.MoveBondXY(measurePos.X, measurePos.Y);
                            System2Domain.GetInstance().BondHeadController.MoveAxisZ(measurePos.Z);

                            double liftLevel = System2Domain.GetInstance().BondModuleController.GetBondheadSafeLevel();

                            // 执行测高
                            (ExcuteResult excuteResult, double height) result = System2Domain.GetInstance().BondHeadController.MeasureHeight(
                                liftLevel,
                                HeightMeasurementFunctionEnum.WithTDSensor);

                            // 如果测高不成功
                            if (result.excuteResult != ExcuteResult.Success)
                            {
                                throw new Exception("System  2  measure  height  failed!");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  measure  height  failed!" + exception.Message);
                    }

                    // 真空
                    try
                    {
                        sdf.Caption = "System  2  Vacuum  Elect is self-checking";
                        bondModule.BondHead.VaccumElectric.SetOutputValue(true);
                        bondModule.BondHead.VaccumElectric.SetOutputValue(false);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  Vacuum  Elect action failed!" + exception.Message);
                    }

                    // 吹气
                    try
                    {
                        sdf.Caption = "System  2  Blow  Elect is self-checking";
                        bondModule.BondHead.WeakBlowElectric.SetOutputValue(true);
                        bondModule.BondHead.WeakBlowElectric.SetOutputValue(false);
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("System  2  Blow  Elect action failed!" + exception.Message);
                    }

                    #endregion

                    #region DispenseSystem
                    if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                    {
                        try
                        {
                            sdf.Caption = "System 1 height measurement is self-checking";

                            // 点胶测高
                            AKRSPoint3D measurePos = CalibrateRunPara.GetInstance().DispenseMeasureHeightSearchMachinePos;

                            DispenseModule dispenseModule = new DispenseModule();

                           // dispenseModule.MoveAxis(measurePos);

                            // 执行测高
                            (ExcuteResult excuteResult, double height) result = System1Domain.GetInstance()
                                .DispenseMeasureHeightController.DispenserHeightMeasurementG0(
                                    System1MeasHeightToolEnum.HeightSensor,
                                    null,
                                    double.NaN);

                            // 如果测高不成功
                            if (result.excuteResult != ExcuteResult.Success)
                            {
                                throw new Exception("System 1 measure height failed!");
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new Exception("System 1 measure height failed!" + exception.Message);
                        }
                    }
                    #endregion

                    #region 流道
                    try
                    {
                        sdf.Caption = "TransportModule loading belt moving ...";
                        TransportModule.GetInstance().LoadingSubSectionModule.RelativeMoveBelt(10, 1);

                        sdf.Caption = "TransportModule dispense belt moving ...";
                        TransportModule.GetInstance().DispenseSubSectionModule.RelativeMoveBelt(10, 1);

                        sdf.Caption = "TransportModule dispense area runner block cylinder down";
                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.BlockCylinder1UpAndDelay(1000);
                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.BlockCylinder1DownAndDelay(1000);

                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.BlockCylinder2UpAndDelay(1000);
                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.BlockCylinder2DownAndDelay(1000);

                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.Clamp();
                        Thread.Sleep(1000);
                        TransportDomain.GetInstance().TransportController.DispenseSubSectionController.UnClamp();

                        sdf.Caption = "TransportModule bond belt moving ...";
                        TransportModule.GetInstance().BondSubSectionModule.RelativeMoveBelt(10, 1);

                        sdf.Caption = "TransportModule bond area runner block cylinder down";
                        TransportDomain.GetInstance().TransportController.BondSubSectionController.BlockCylinderUpAndDelay(1000);
                        TransportDomain.GetInstance().TransportController.BondSubSectionController.BlockCylinderDownAndDelay(1000);

                        TransportDomain.GetInstance().TransportController.BondSubSectionController.Clamp();
                        Thread.Sleep(1000);
                        TransportDomain.GetInstance().TransportController.BondSubSectionController.UnClamp();

                        sdf.Caption = "TransportModule waiting unload area belt moving ...";
                        TransportModule.GetInstance().WaitingUnloadSubSectionModule.RelativeMoveBelt(10, 1);

                        sdf.Caption = "TransportModule bond belt moving ...";
                        TransportModule.GetInstance().UnloadingSubSectionModule.RelativeMoveBelt(10, 1);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("System 1 measure height failed!" + ex.Message);
                    }

                    #endregion           
                }

                AKRSXtraMessageBox.Show(
                    "Self-Check success!",
                    "Prompt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message + "\r\n" + "Self-Check failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取硬件集合
        /// </summary>
        /// <returns>结果</returns>
        public List<string> GetHardWareNames()
        {
            List<string> list = new List<string>();
           
            list.Add("总正压检测");
            list.Add("总负压检测");

            if (MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured)
            {
                list.Add("安全门");
            }

             // todo:三色灯、蜂鸣器

            if (MachineHardwareConfiguration.GetInstance().IsJoyStickConfigured)
            {
                list.Add("摇杆X方向模拟量读取");
                list.Add("摇杆Y方向模拟量读取");
                list.Add("摇杆T方向模拟量读取");
            }

            return list;
        }

        /// <summary>
        ///  获取MAC地址
        /// </summary>
        /// <returns></returns>
        public static string GetPrimaryMacAddress()
        {
            // 优先选择：状态为“已连接”的【以太网】网卡
            var targetNic = NetworkInterface.GetAllNetworkInterfaces()
                .FirstOrDefault(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                                       && nic.OperationalStatus == OperationalStatus.Up);

            // 如果没插网线，就找：状态为“已连接”的【无线】网卡
            if (targetNic == null)
            {
                targetNic = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(nic => nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                                           && nic.OperationalStatus == OperationalStatus.Up);
            }

            // 如果都没连接，就随便找一个非虚拟、非回环的网卡
            if (targetNic == null)
            {
                targetNic = NetworkInterface.GetAllNetworkInterfaces()
                    .FirstOrDefault(nic => nic.NetworkInterfaceType != NetworkInterfaceType.Loopback
                                           && !nic.Description.ToLower().Contains("virtual")
                                           && !nic.Description.ToLower().Contains("vmware"));
            }

            // 返回MAC地址，如果没有找到就返回空
            return targetNic?.GetPhysicalAddress().ToString() ?? "00-00-00-00-00-00";
        }

        /// <summary>
        /// 获取主机名
        /// </summary>
        /// <returns></returns>
        public static string GetMachineName()
        {
            return Environment.MachineName;
        }

        /// <summary>
        ///  重置设备信息
        /// </summary>
        public void ReSetMachineInfo()
        {
            MachineDevicePara.MacAddress = GetPrimaryMacAddress();
            MachineDevicePara.MachineName = GetMachineName();
            MachineDevicePara.GetInstance().Save();
        }

        ///// <summary>
        /////  判断数据是否迁移
        ///// </summary>
        ///// <returns></returns>
        //public bool JudgeDataMigrated()
        //{
        //    if (string.IsNullOrEmpty(MachineDevicePara.MacAddress) || string.IsNullOrEmpty(MachineDevicePara.MachineName))
        //    {
        //        MachineDevicePara.MacAddress = GetPrimaryMacAddress();
        //        MachineDevicePara.MachineName = GetMachineName();
        //        MachineDevicePara.GetInstance().Save();
        //        return false;
        //    }

        //    return !(MachineDevicePara.MacAddress == GetPrimaryMacAddress() && MachineDevicePara.MachineName == GetMachineName());
        //}

        /// <summary>
        ///  判断数据是否迁移
        /// </summary>
        /// <returns>true:有迁移</returns>
        public bool JudgeDataMigrated()
        {
            if ( string.IsNullOrEmpty(MachineDevicePara.MachineName))
            {
                MachineDevicePara.MacAddress = GetPrimaryMacAddress();
                MachineDevicePara.MachineName = GetMachineName();
                MachineDevicePara.GetInstance().Save();
                return false;
            }

            return MachineDevicePara.MachineName != GetMachineName();
        }
    }
}
