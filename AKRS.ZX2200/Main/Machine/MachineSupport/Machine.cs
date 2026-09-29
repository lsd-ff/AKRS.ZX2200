using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    using Accord.IO;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using DevExpress.XtraEditors;
    using LanguageExt.Pipes;
    using log4net.Core;
    using SqlSugar;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// 静态库
    /// </summary>
    public partial class Machine : SingletonNoSave<Machine>
    {
        /// <summary>
        /// 工作步骤
        /// </summary>
        public List<Task> WorkTasks { get; set; }

        /// <summary>
        /// 加密ID
        /// </summary>
        public string AuthenticatorId { get; set; }

        /// <summary>
        /// 加密到期日期
        /// </summary>
        public string AuthenticatorDueDate { get; set; }

        /// <summary>
        /// CurrentRoleLevel
        /// </summary>
        [JsonIgnore]
        public int CurrentRoleLevel => this.GetCurrentRoleLevel();

        /// <summary>
        /// CurrentRole
        /// </summary>
        [JsonIgnore]
        public RoleEnum CurrentRole => (RoleEnum)this.CurrentRoleLevel;

        /// <summary>
        /// 调试停留时间
        /// </summary>
        [JsonIgnore]
        public int DebugPauseTime { get; set; } = 0;

        /// <summary>
        /// 调试模式
        /// </summary>
        [JsonIgnore]
        public DebugModelEnum DebugModel { get; set; }

        /// <summary>
        /// 继续调试
        /// </summary>
        [JsonIgnore]
        public bool ContinueDebug { get; set; }

        /// <summary>
        /// GetCurrentRoleLevel
        /// </summary>
        /// <returns>result</returns>
        private int GetCurrentRoleLevel()
        {
            int level = 3;
            if (RuntimeProvider.CurrentLoginUser != null)
            {
              level = RuntimeProvider.CurrentLoginUser.RoleID; 
            }

            return level;
        }

        /// <summary>
        /// 打开报警器
        /// 考虑到跟业务相关，不设置成静态
        /// </summary>
        /// <param name="alarmLevel">报警等级</param>
        public void OpenAlarm(AlarmLevel alarmLevel = AlarmLevel.SecondLevel)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            if (Machine.GetInstance().IsWorking() || Machine.GetInstance().IsPause())
            {
                List<Alarmer> hardwaresByType = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
                if (hardwaresByType != null && hardwaresByType.Any())
                {
                    Alarmer alarmer = hardwaresByType[0];
                    switch (alarmLevel)
                    {
                        case AlarmLevel.FirstLevel:
                            alarmer.SetFirstLevelAlarm();
                            break;
                        case AlarmLevel.SecondLevel:
                            alarmer.SetSecondLevelAlarm();
                            break;
                        case AlarmLevel.ThirdLevel:
                            alarmer.SetThirdLevelAlarm();
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 重置报警器 运行状态
        /// </summary>
        public void ResetAlarm()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            List<Alarmer> hardwaresByType = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (hardwaresByType == null || hardwaresByType.Any<Alarmer>())
            {
                hardwaresByType[0].SetRunState();
            }
        }

        /// <summary>
        /// 设置报警器为停止状态
        /// </summary>
        public void SetStopStateAlarm()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            List<Alarmer> hardwaresByType = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (hardwaresByType == null || hardwaresByType.Any<Alarmer>())
            {
                hardwaresByType[0].SetStopState();
            }
        }

        /// <summary>
        /// 是否所有线程已经关闭
        /// </summary>
        /// <returns>结果</returns>
        public bool IsAllowTaskClosed()
        {
            this.WorkTasks = new List<Task>();

            // 点胶线程
            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.WorkTasks.Add(System1Domain.GetInstance().DispenseWorkTask.DispenseTask);
            }

            this.WorkTasks.Add(WaferSystemDomain.GetInstance().WaferSubSystemTask.waferSubTask);
            this.WorkTasks.Add(System2Domain.GetInstance().BondTask.BondWorkTask);
            this.WorkTasks.Add(System2Domain.GetInstance().SlideFluxerTask.SlideFluxTask);
            this.WorkTasks.Add(WaferSystemDomain.GetInstance().FlipTask.FlipTableTask);

            #region 流道

            // 送板机传到上料皮带
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferAutoLoaderToLoadingBeltTask);

            // 上料皮带传到点胶
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferLoadBeltToDispense1Task);

            // 上料仓传到点胶
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferLoaderBinToDispense1Task);

            // 点胶1传到点胶2 
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferDispense1ToDispense2Task);

            // 点胶2 传到Bond
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferDispenseToBondTask);

            // Bond传到下料等待
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferBondToWaitingUnloadTask);

            // 下料等待传到下料皮带
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferWaitingUnloadToUnloadBeltTask);

            // 下料等待传到下料仓
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferWaitingUnloadToUnloadBinTask);

            // 下料皮带到送板机
            this.WorkTasks.Add(TransportDomain.GetInstance().transportTask.transferUnloadBeltToAutoUnloaderTask);

            // 上料仓
            this.WorkTasks.Add(TransportDomain.GetInstance().LoaderTask.loaderTask);

            // 下料仓
            this.WorkTasks.Add(TransportDomain.GetInstance().UnloaderTask.unloaderTask);

            #endregion

            foreach (Task task in this.WorkTasks)
            {
                if (task?.Status == TaskStatus.Running)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 关闭所有灯光
        /// </summary>
        public void ChoseAllLight()
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            List<Light> lights = new List<Light>();
            lights.AddRange(System1Domain.GetInstance().DispenseVisionController.GetLights());
            lights.AddRange(new BondModule().GetLights());
            lights.AddRange(new UpLookModule().GetLights());
            lights.AddRange(new WaferTableModule().GetLights());

            foreach (Light light in lights)
            {
                light?.SetIntensity(10);
            }

            foreach (Light light in lights)
            {
                light?.SetIntensity(0);
            }
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        public void DataLoad()
        {
            try
            {
                // WaferSub System
                WaferSubDevicePara.GetInstance();
                AdapterConfigRepository.GetInstance();
                CarrierConfigRepository.GetInstance();
                EjectionBankConfigRepository.GetInstance();
                EjectionConfigRepository.GetInstance();
                MagazineAllocationsConfigRepository.GetInstance();
                MagazineBoxGeoConfigRepository.GetInstance();
                MagazineBoxConfigRepository.GetInstance();
                WaferSystemProgram.GetInstance();

                // Bond system
                BondDevicePara.GetInstance();
                NozzleRepository.GetInstance();
                NozzleShelfRepository.GetInstance();
                PostBondInspectionRepository.GetInstance();
                ProductDomain.GetInstance();
                ForceConfig.GetInstance();
                System2Configuration.GetInstance();
                MachineHardwareConfiguration.GetInstance();
                MachineCoordinateSystem.GetInstance();
                FlipToolRepository.GetInstance();
                RtuConnectConfig.GetInstance();
                ForceCalibrationData.GetInstance();

                // TU system
                ProductConfiguration.GetInstance();
                TransportDevicePara.GetInstance();
                MachineConfigContext.GetInstance();
                TransportProgram.GetInstance();
                LoaderBinRepository.GetInstance();
                UnLoaderBinRepository.GetInstance();
                TransportBeltSettingRepository.GetInstance();
                InOutPutBeltSettingRepository.GetInstance();
                BondinsertSettingRepository.GetInstance();

                // System1
                DispenseDevicePara.GetInstance();
                System1Program.GetInstance();
                EpoxyApplicationRepository.GetInstance();
                EpoxyMaterialRepository.GetInstance();

                PropertyChangeAop.InitSuccess = true;
            }
            catch (Exception exception)
            {
                LogHelper.Post(Level.Error, "加载数据失败", exception, LogCategory.MainSoftWare, ViewType.InFileAndUI);
                AKRSXtraMessageBox.Show("加载数据失败\n" + exception.Message);
            }
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        public void DataSave()
        {
            try
            {
                // WaferSub System
                WaferSubDevicePara.GetInstance().Save();
                AdapterConfigRepository.GetInstance().Save();
                CarrierConfigRepository.GetInstance().Save();
                EjectionBankConfigRepository.GetInstance().Save();
                EjectionConfigRepository.GetInstance().Save();
                MagazineAllocationsConfigRepository.GetInstance().Save();
                MagazineBoxGeoConfigRepository.GetInstance().Save();
                MagazineBoxConfigRepository.GetInstance().Save();
                WaferSystemProgram.GetInstance().Save();
                FlipToolRepository.GetInstance().Save();

                // Bond system
                BondDevicePara.GetInstance().Save();
                NozzleRepository.GetInstance().Save();
                NozzleShelfRepository.GetInstance().Save();
                PostBondInspectionRepository.GetInstance().Save();
                ProductDomain.GetInstance().Save();
                ForceConfig.GetInstance().Save();
                System2Configuration.GetInstance().Save();
                MachineCoordinateSystem.GetInstance().Save();
                BondProgram.GetInstance().Save();
                ForceCalibrationData.GetInstance().Save();

                // TU system
                ProductConfiguration.GetInstance().Save();
                TransportDevicePara.GetInstance().Save();
                MachineConfigContext.GetInstance().SaveMachineConfig();
                TransportProgram.GetInstance().Save();
                LoaderBinRepository.GetInstance().Save();
                UnLoaderBinRepository.GetInstance().Save();
                TransportBeltSettingRepository.GetInstance().Save();
                BondinsertSettingRepository.GetInstance().Save();
                InOutPutBeltSettingRepository.GetInstance().Save();

                // System1
                DispenseDevicePara.GetInstance().Save();
                System1Program.GetInstance().Save();
                DispenserRepository.GetInstance().Save();
                EpoxyApplicationRepository.GetInstance().Save();
                EpoxyMaterialRepository.GetInstance().Save();

                // machine
                MachineHardwareConfiguration.GetInstance().Save();
                MachineSoftwareConfiguration.GetInstance().Save();
                VisionEntityRepository.GetInstance().Save();
                RtuConnectConfig.GetInstance().Save();
                MachineDevicePara.Save();

                PropertyChangeAop.InitSuccess = true;
            }
            catch (Exception exception)
            {
                LogHelper.Post(Level.Error, "加载数据失败", exception, LogCategory.MainSoftWare, ViewType.InFileAndUI);
                AKRSXtraMessageBox.Show("加载数据失败\n" + exception.Message);
            }
        }
    }
}
