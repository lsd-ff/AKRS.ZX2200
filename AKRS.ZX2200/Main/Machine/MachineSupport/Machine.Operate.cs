#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/8/1 15:37:25
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    using AKRS.Galaxy.SoftKey;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.GlobalCalibration;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Controls.Ucmain;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.SensorCheckScan;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Programs;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using ch.etel.edi.dsa.v40;
    using DevExpress.CodeParser;
    using DevExpress.XtraEditors;
    using DevExpress.XtraReports.Native;
    using DevExpress.XtraSplashScreen;
    using log4net.Core;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.LogicHardware.Hardwares.FeederControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LaserInterferometerEncoderControllers;

    /// <summary>
    /// 描述：
    /// </summary>
    public partial class Machine
    {
        /// <summary>
        /// 设备后台线程
        /// </summary>
        [JsonIgnore]
        private Thread BackGroundThreadThread;

        /// <summary>
        /// 检查自动运行环境
        /// </summary>
        /// <returns>是否能够运行</returns>
        public bool CheckOperatingEnvironment()
        {
            // 晶圆域有没有准备好
            if (!WaferSystemDomain.GetInstance().CheckIsReady())
            {
                return false;
            }

            // 固晶域启动自检
            if (!System2Domain.GetInstance().CheckIsReady())
            {
                return false;
            }

            // 点胶域有没有准备好
            if (!System1Domain.GetInstance().CheckIsReady())
            {
                return false;
            }

            // 产品有没有准备好
            if (!ProductConfiguration.GetInstance().IsTransportUnitReady())
            {
                return false;
            }

            // 焊点有没有准备好
            if (!ProductConfiguration.GetInstance().BondPositionConfig.IsBpAssistanceFinish())
            {
                return false;
            }

            // 制程是否准备好
            if (!ProcessDomain.GetInstance().CheckIsReady())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 复位机台所有信号
        /// </summary>
        public void ResetMachineSignal()
        {
            // wafersub
            SignalPool.GetInstance().IsBondNeedChipSignal.ReSet();
            SignalPool.GetInstance().IsWaferAllowPickSignal.ReSet();

            // 流道信号
            SignalPool.GetInstance().AllowDispense1TransferToDispense2Signal.ReSet();
            SignalPool.GetInstance().AllowDispenseTransferToBondSignal.ReSet();
            SignalPool.GetInstance().AllowBondTransferToWaitingUnloadSignal.ReSet();
            SignalPool.GetInstance().AllowWaitingUnloadTransferToOutloadSignal.ReSet();
            SignalPool.GetInstance().TransferToDispense1FinishedSignal.ReSet();
            SignalPool.GetInstance().TransferToDispense2FinishedSignal.ReSet();
            SignalPool.GetInstance().TransferToBondFinishedSignal.ReSet();

            // 上下料
            SignalPool.GetInstance().AllowUnloaderMoveSignal.ReSet();
            SignalPool.GetInstance().AllowLoaderMoveSignal.ReSet();
            SignalPool.GetInstance().LoaderAllowPushSignal.ReSet();
            SignalPool.GetInstance().UnloaderAllowPushSignal.ReSet();

            // 固晶域信号
            SignalPool.GetInstance().IsBondNeedChipSignal.ReSet();
            SignalPool.GetInstance().IsWaferAllowPickSignal.ReSet();
            SignalPool.GetInstance().TransferToBondFinishedSignal.ReSet();
            SignalPool.GetInstance().IsBondPickSucceedSignal.ReSet();
            SignalPool.GetInstance().IsFlipTableAllowPickSignal.ReSet();
            SignalPool.GetInstance().IsAllowDipFluxSignal.ReSet();
            SignalPool.GetInstance().BondDipFluxFinishSignal.ReSet();
            SignalPool.GetInstance().AllowSlideOutSignal.ReSet();
        }

        /// <summary>
        /// 设置日志
        /// </summary>
        public void ReadyToLog()
        {
            // 日志配置
            if (MachineSoftwareConfiguration.GetInstance().IsWorkLog)
            {
                LogsManager.IsLogInfo = true;
            }
            else
            {
                LogsManager.IsLogInfo = false;
            }
        }

        /// <summary>
        /// 启动前检查
        /// </summary>
        /// <returns>结果</returns>
        private bool PrepareMachineReadyToStart()
        {
            this.CleanFile(MachineSoftwareConfiguration.GetInstance().FileSaveDue);

            // 复位信号
            this.ResetMachineSignal();

            // 检测加密狗是否到期
            //if (MachineSoftwareConfiguration.GetInstance().IsDetectingEncryption && !SoftkeyManager.CheckYtSoftKey())
            //{
            //    return false;
            //}

            // 检查所有是否总复位
            if (!this.AllAxisReset)
            {
                AKRSXtraMessageBox.Show("启动前请先总复位");
                return false;
            }

            this.ReadyToLog();

            if (!this.IsAllowTaskClosed())
            {
                AKRSXtraMessageBox.Show($"部分模组仍在工作,不能启动，请关闭后重试");
                return false;
            }

            foreach (var item in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
            {
                item.InitDefectCount();
            }

            if (!this.MachineInitSuccess())
            {
                return false;
            }

            if (!this.IsSafeDoorClose && MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured)
            {
                AKRSXtraMessageBox.Show("请先关好安全门");
                return false;
            }

            if (Machine.GetInstance().IsWorking())
            {
                return false;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                System1Domain.GetInstance().DispenseController.MoveToSafePos();
            }

            // 轴移动到安全位置,放到流道线程里面
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            if (!WaferSubDevicePara.GetInstance().EjectDevicePara.IsShieldEjectModule)
            {
                Axis Axis = HardwareRepositoryService.GetHardware<Axis>("顶针Z");
                Axis.AbsoluteMove(0);
            }


            bool IsSuccess = this.CheckOperatingEnvironment();
            if (IsSuccess == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 程序启动
        /// </summary>
        public void Start()
        {
           // Machine.GetInstance().ChoseAllLight();

            // 启动前的检查工作
            bool isSuccess = this.PrepareMachineReadyToStart();
            if (isSuccess == false)
            {
                return;
            }

            Machine.GetInstance().SetState(MachineStateEnum.Working);

            if (!MachineStateModel.GetInstance().IsCompensateWork)
            {
                TransportDomain.GetInstance().StartTask();
                WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                if (!MachineStateModel.GetInstance().IsCompensateWork)
                {
                    System1Domain.GetInstance().DispenseWorkTask.Start();
                }
            }

            // 先排好序
            System2Domain.GetInstance().ActionNodesService.Init();
            System2Domain.GetInstance().BondTask.Start();
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
            {
                if (!MachineStateModel.GetInstance().IsCompensateWork)
                {
                    System2Domain.GetInstance().SlideFluxerTask.Start();
                }
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                if (!MachineStateModel.GetInstance().IsCompensateWork)
                {
                    WaferSystemDomain.GetInstance().FlipTask.Start();
                }
            }

            WaferSubController.GetInstance().WaferTableController.CheckAndOpenWaffleVacuum();
            WaferSubController.GetInstance().WaferTableController.CheckAndOpenStaticWaffleVacuum();
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            // 改变设备状态
            Machine.GetInstance().SetState(MachineStateEnum.Pause);
        }

        /// <summary>
        /// 继续
        /// </summary>
        public void Continue()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured)
            {
                Machine.GetInstance().SetState(MachineStateEnum.Working);
            }
            else
            {
                if (!this.IsSafeDoorClose)
                {
                    AKRSXtraMessageBox.Show("请先关好安全门");
                }
                else
                {
                    DispenseRunTimeProvider.IsFirstDispense = true;
                    System2RunTimeProvider.IsFirstDispense = true;
                    Machine.GetInstance().SetState(MachineStateEnum.Working);
                }
            }            
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            // 改变设备状态
            Machine.GetInstance().SetState(MachineStateEnum.Stop);

            // 清除弹窗
            AKRSMessageBoxExt.ClearMessageBox();

            // 状态改为停止
            this.SetStopStateAlarm();

            // 退出单步模式
            MachineStateModel.GetInstance().IsSingleStepWork = false;

            // 重置Bond需料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.ReSet();

            // 关要料信号
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.Online)
            {
                TransportDomain.GetInstance().TransportController.LoadingSubSectionController.SetLoadingTableNeedTabletSignal(false);
            }

            // 关闭顶针真空 
            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            // 焊后重新计数
            foreach (PostBondInspection postBondInspection in BondProgram.GetInstance().PostBondProgram.PostBondInspections)
            {
                postBondInspection.CurrentDefectNumber = 0;
            }
        }

        /// <summary>
        /// 急停
        /// </summary>
        public void EmergencyStop()
        {
            // 所有轴停止移动
            List<Axis> axisList = HardwareRepositoryService.GetHardwaresByType<Axis>();

            Parallel.ForEach(
                axisList,
                (axis, sate, i) =>
                {
                    axis.StopMove(false, true);
                });

            // 改变设备状态
            Machine.GetInstance().Stop();

            // TransportDomain.GetInstance().StartTask(); 流道的Task线程不知道怎么结束
            System1Domain.GetInstance().DispenseWorkTask.AbortThread();
            System2Domain.GetInstance().BondTask.AbortThread();
            WaferSystemDomain.GetInstance().WaferSubSystemTask.AbortWaferSubThread();
        }

        /// <summary>
        /// 设备初始化是否成功
        /// </summary>
        /// <returns>结果</returns>
        public bool MachineInitSuccess()
        {
            if (MachineSoftwareConfiguration.GetInstance().IsAxisResetBeforeWork && !this.AllAxisReset)
            {
                AKRSXtraMessageBox.Show("设备轴未复位，请先复位所有轴");

                return false;
            }

            if (!this.PrInitSuccess)
            {
                AKRSXtraMessageBox.Show("设备视觉模板正在初始化，请稍后重试");

                return false;
            }

            if (!this.IsDiskCapacitySufficient())
            {
                return false;
            }

            if (MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured
                && TransportDomain.GetInstance().TransportController.BondSubSectionController.IsHeaterOpen()
                && !this.PreheatingComplete)
            {
                AKRSXtraMessageBox.Show("垫块预热时间未达到，请稍后重试");

                return false;
            }

            //if (this.LaserEncoderCheck() == false) 
            //{
            //    return false;
            //}

            return true;
        }

        /// <summary>
        /// 加热检测线程
        /// </summary>
        public void BackgroundTemperatureAlarm()
        {
            if (!MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured)
            {
                return;
            }

            if (Machine.GetInstance().IsWorking()
              && TransportDomain.GetInstance().TransportController.BondSubSectionController.IsHeaterOpen()
              && TransportDomain.GetInstance().TransportController.BondSubSectionController.Alarm())
            {
                Machine.GetInstance().Stop();
                AKRSMessageBoxExt.Show(
                $"当前温度超出设定超阈值! 请检查加热器 \r\n",
                "温度超阈值报警",
                new string[] { "确认" },
                new DialogResult[] { DialogResult.OK },
                AlarmLevel.SecondLevel);
            }

            if (TransportDomain.GetInstance().TransportController.BondSubSectionController.IsHeaterOpen()
            && TransportDomain.GetInstance().TransportController.BondSubSectionController.IsHeatingSucceed())
            {
                // 判断是否加热完成
                if (!this.HeatingComplete)
                {
                    this.HeatingComplete = true;
                    this.HeatingCompleteTime = DateTime.Now;
                }

                // 预热有没有完成
                if (!this.PreheatingComplete && this.HeatingComplete)
                {
                    if ((DateTime.Now - this.HeatingCompleteTime).Minutes >= TransportProgram.GetInstance().BondSubSectionProgram.PreheatTime)
                    {
                        this.PreheatingComplete = true;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化信息
        /// </summary>
        public void InitTemperatureInfo()
        {
            this.HeatingComplete = false;
            this.HeatingCompleteTime = DateTime.Now;
            this.PreheatingComplete = false;
        }

        /// <summary>
        /// 启动软件回零
        /// </summary>
        /// <returns>回零是否成功</returns>
        public bool AllAxisGoHome()
        {
            try
            {
                if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show("回零前请确保焊头和翻转台不干涉！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dialog != DialogResult.OK)
                    {
                        return false;
                    }
                }
                else
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show("轴即将复位回零，人员请离开设备", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    if (dialog != DialogResult.OK)
                    {
                        return false;
                    }
                }

                // 收回点胶气缸
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
                }

                LogHelper.Post(Level.Info, $"系统2收回点胶气缸成功", LogCategory.Global, ViewType.InFileAndUI);

                // 设备回零
                this.CloseCompensate();

                LogHelper.Post(Level.Info, $"关闭二维补偿成功", LogCategory.Global, ViewType.InFileAndUI);

                if (MachineHardwareConfiguration.GetInstance().IsEjectSystemConfigured)
                {
                    WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
                    WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
                }

                LogHelper.Post(Level.Info, $"顶针系统初始化成功", LogCategory.Global, ViewType.InFileAndUI);

                if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
                {
                    System2Domain.GetInstance().SlideFluxerController.SlideFluxerHomeNoWait();
                }

                LogHelper.Post(Level.Info, $"静态华夫盒初始化成功", LogCategory.Global, ViewType.InFileAndUI);

                using (DevExpress.Utils.WaitDialogForm sdf = new DevExpress.Utils.WaitDialogForm("轴回零", "轴回零中，请稍后"))
                {
                    LogHelper.Post(Level.Info, $"所有轴开始回零", LogCategory.Global, ViewType.InFileAndUI);
                    bool ret = HardwareRepositoryService.OneKeyHome();
                    LogHelper.Post(Level.Info, $"所有轴完成回零", LogCategory.Global, ViewType.InFileAndUI);
                    if (ret)
                    {
                        //WaferSubController.GetInstance().EjectController.MoveEjectionBankToSlotPosition(0);

                        AKRSXtraMessageBox.Show("所有轴回零成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("轴回零失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                this.OpenCompensate();

                Machine.GetInstance().AllAxisReset = true;

                return true;
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 关闭补偿
        /// </summary>
        public void CloseCompensate()
        {
            BondModule bondModule = new BondModule();

            if (bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                DsaDrive moveDriveX = ((ETELAxis)bondModule.BondAxisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)bondModule.BondAxisY.AxisDrive).GetDrive();

                ETelDrive.Close2DCompensate(bondModule.BondAxisX, bondModule.BondAxisY);
            }
            else
            {
                GuGaoDrive.Close2DCompensate();
            }
        }

        /// <summary>
        /// 开启补偿
        /// </summary>
        private void OpenCompensate()
        {
            if (!MachineSoftwareConfiguration.GetInstance().Is2DCompensationOpen)
            {
                return;
            }

            BondModule bondModule = new BondModule();
            if (bondModule.BondAxisX.AxisDrive is ETELAxis)
            {
                DsaDrive moveDriveX = ((ETELAxis)bondModule.BondAxisX.AxisDrive).GetDrive();
                DsaDrive moveDriveY = ((ETELAxis)bondModule.BondAxisY.AxisDrive).GetDrive();

                ETelDrive.Open2DCompensate(
                   bondModule.BondAxisX,
                   bondModule.BondAxisY,
                   GlobalCalibrationDomain.GetInstance().TextName);
            }
            else
            {
                bool isSuccess = GuGaoDrive.Open2DCompensate();
                if (isSuccess == false)
                {
                    AKRSXtraMessageBox.Show("Open Compensate fail, 请联系技术人员处理", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// 自动登出
        /// </summary>
        public void AutoLogOut()
        {
            if (MachineSoftwareConfiguration.GetInstance().AutoLogOut
                && RuntimeProvider.CurrentLoginUser != null
                && (int)RuntimeProvider.CurrentLoginUser.RoleID < 3)
            {
                if ((DateTime.Now - StatisticsService.LastOperaDateTime).TotalMinutes > MachineSoftwareConfiguration.GetInstance().AutoLogOutTime)
                {
                    RuntimeProvider.CurrentLoginUser = null;
                    AKRSMessageBoxExt.ShowWarn(
                        $"用户长时间未操作，自动退出登录\r\n",
                        "提示",
                        new string[] { "确定" },
                        new DialogResult[] { DialogResult.Yes, DialogResult.No },
                        AlarmLevel.SecondLevel);
                }
            }
        }

        /// <summary>
        /// 启动后台线程
        /// </summary>
        public void StartBackGroundThread()
        {
            if (this.BackGroundThreadThread == null || !this.BackGroundThreadThread.IsAlive)
            {
                this.BackGroundThreadThread = new Thread(
                                  () =>
                                  {
                                      while (RuntimeProvider.ThreadFlag)
                                      {
                                          // 检测是否安全门关闭，安全门关闭则允许设备工作
                                          this.StartCheckSafeDoorTask();

                                          // 检测激光干涉尺状态
                                          this.StartLaserEncoderCheckTask();

                                          // 长时间未操作登出高级权限账户
                                          this.AutoLogOut();

                                          // 后台加热检测
                                          this.BackgroundTemperatureAlarm();

                                          // 后台耗材检测
                                          ConsumablesProgram.GetInstance().BackgroundConsumable();

                                          // 后台信号检测
                                          NeedScanSensorPool.GetInstance().StartScan();

                                          // 存图
                                          VisionService.StartSaveThread();

                                          // 存数据库
                                          StatisticsService.StartSaveThread();

                                          Thread.Sleep(100);
                                      }
                                  })
                { Name = "后台监控线程", IsBackground = true };
                this.BackGroundThreadThread.Start();
            }
        }

        /// <summary>
        /// 退出软件
        /// </summary>
        public void ExitSoftware()
        {
            try
            {
                SplashScreenManager.ShowForm(typeof(SplashScreen1));
                SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, "保存数据中。。。");
                Machine.GetInstance().DataSave();

                SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.SetProgress, "关闭硬件中。。。");
                if (MachineHardwareConfiguration.GetInstance().IsTransportHeatConfigured)
                {
                    TransportDomain.GetInstance().TransportController.BondSubSectionController.CLoseHeater();
                }

                if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                {
                    try
                    {
                        HardwareRepositoryService.CloseAllHardware();
                    }
                    catch (Exception)
                    {
                    }
                   
                }

                SplashScreenManager.CloseForm();
            }
            catch (Exception)
            {
            }

            System.Environment.Exit(0);
        }

        /// <summary>
        /// 检查硬件是否存在
        /// </summary>
        public void CheckHardWareByConfig()
        {

        }

        /// <summary>
        /// 等待激光干涉尺预热完成
        /// </summary>
        /// <returns>线程</returns>
        public async Task WaitLaserEncoderWarmupAsync()
        {
            //var sp = Stopwatch.StartNew();
            //while (true)
            //{
            //    await Task.Delay(1000); // 异步等待，不阻塞线程

            //    if (sp.Elapsed.TotalSeconds > 20)
            //    {
            //        break;
            //    }
            //}

            LaserEncoder laserEncoder = HardwareRepositoryService.GetHardware<LaserEncoder>("激光干涉尺");

            // 如果不配置，直接返回
            if (laserEncoder == null)
            {
                return;
            }

            var sp = Stopwatch.StartNew();
            while (!laserEncoder.GetWarmupStatus())
            {
                await Task.Delay(1000); // 异步等待，不阻塞线程

                if (sp.Elapsed.TotalSeconds > 242)
                {
                    throw new TimeoutException("激光干涉尺预热超时！");
                }
            }
        }
    }
}
