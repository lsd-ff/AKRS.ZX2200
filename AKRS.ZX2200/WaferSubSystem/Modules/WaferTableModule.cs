namespace AKRS.ZX2200.WaferSubSystem.Modules
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;

    using Newtonsoft.Json;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using System.Xml.Linq;
    using System.Threading;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using DevExpress.XtraEditors;
    using System.Windows.Forms;
    using DevExpress.Charts.Native;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Controllers;

    /// <summary>
    /// 晶圆台模组
    /// </summary>
    public class WaferTableModule
    {
        /// <summary>
        /// 热吹风
        /// </summary>        
        [JsonIgnore]
        public Electric HotBlower { get; set; } = HardwareRepositoryService.GetHardware<Electric>("扩晶热吹风");

        /// <summary>
        /// 晶圆夹持气缸
        /// </summary>        
        [JsonIgnore]
        public Electric BlockCylinder { get; set; } = HardwareRepositoryService.GetHardware<Electric>("晶圆夹持气缸电磁阀");

        /// <summary>
        /// 晶圆夹持气缸负限位
        /// </summary>        
        [JsonIgnore]
        public Sensor BlockCylinderNLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("晶圆夹持气缸原点松开检测");

        /// <summary>
        /// 晶圆夹持气缸正限位
        /// </summary>        
        [JsonIgnore]
        public Sensor BlockCylinderPLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("晶圆夹持气缸动点夹紧检测");

        /// <summary>
        /// 晶圆台X
        /// </summary>        
        [JsonIgnore]
        public Axis WaferTableAxisX { get; set; } = HardwareRepositoryService.GetHardware<Axis>("晶圆台X");

        /// <summary>
        /// 晶圆台Y
        /// </summary>        
        [JsonIgnore]
        public Axis WaferTableAxisY { get; set; } = HardwareRepositoryService.GetHardware<Axis>("晶圆台Y");

        /// <summary>
        /// 晶圆台扩晶Z
        /// </summary>        
        [JsonIgnore]
        public Axis ExpandAxisZ { get; set; } = HardwareRepositoryService.GetHardware<Axis>("晶圆台扩晶Z");

        /// <summary>
        /// 晶圆环检测
        /// </summary>        
        [JsonIgnore]
        public Sensor WaferSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("晶圆环检测");

        /// <summary>
        /// 晶圆夹Y
        /// </summary>        
        [JsonIgnore]
        public Axis WaferClampAxisY { get; set; } = HardwareRepositoryService.GetHardware<Axis>("晶圆夹Y");

        /// <summary>
        /// 晶圆夹气缸
        /// </summary>        
        [JsonIgnore]
        public Electric WaferClampCylinder { get; set; } = HardwareRepositoryService.GetHardware<Electric>("上晶圆夹子气缸电磁阀");

        /// <summary>
        /// 晶圆夹子气缸到位检测（正限位）
        /// </summary>        
        [JsonIgnore]
        public Sensor WaferClampPLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆夹子气缸动点夹紧检测");

        /// <summary>
        /// 晶圆夹子物料检测
        /// </summary>        
        [JsonIgnore]
        public Sensor WaferClampCheckSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆夹子有料检测");

        /// <summary>
        /// 晶圆相机
        /// </summary>
        [JsonIgnore]
        public AKRSCamera WaferCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("晶圆相机");

        /// <summary>
        /// AmbientLight
        /// </summary>
        [JsonIgnore]
        public Light RingLight => HardwareRepositoryService.GetHardware<Light>("下视环光");

        /// <summary>
        /// SpotLightGreen
        /// </summary>
        [JsonIgnore]
        public Light GreenSpotLight => HardwareRepositoryService.GetHardware<Light>("晶圆三色点光-绿");

        /// <summary>
        /// SpotLightRed
        /// </summary>
        [JsonIgnore]
        public Light RedSpotLight => HardwareRepositoryService.GetHardware<Light>("晶圆三色点光-红");

        /// <summary>
        /// SpotLightBlue
        /// </summary>
        [JsonIgnore]
        public Light BlueSpotLight => HardwareRepositoryService.GetHardware<Light>("晶圆三色点光-蓝");

        /// <summary>
        /// 晶圆相机Z
        /// </summary>        
        [JsonIgnore]
        public Axis WaferCameraAxisZ { get; set; } = HardwareRepositoryService.GetHardware<Axis>("晶圆相机Z");

        /// <summary>
        /// 静态华夫盒真空
        /// </summary>        
        [JsonIgnore]
        public Electric StaticWaffleVacuum => HardwareRepositoryService.GetHardware<Electric>("静态华夫盒真空电磁阀");
        
        /// <summary>
        /// 静态华夫盒真空检测
        /// </summary>        
        [JsonIgnore]
        public Sensor StaticWaffleVacuumCheck => HardwareRepositoryService.GetHardware<Sensor>("静态华夫盒真空检测");

        /// <summary>
        /// 晶圆台Tray盘真空电磁阀
        /// </summary>        
        [JsonIgnore]
        public Electric WaffleVacuum => HardwareRepositoryService.GetHardware<Electric>("晶圆台Tray盘真空电磁阀");

        /// <summary>
        /// 晶圆台Tray盘负压检测
        /// </summary>        
        [JsonIgnore]
        public Sensor WaffleVacuumCheck => HardwareRepositoryService.GetHardware<Sensor>("晶圆台Tray盘负压检测");

        /// <summary>
        /// 获取灯光集合
        /// </summary>
        /// <returns>结果</returns>
        public List<Light> GetLights()
        {
            return new List<Light>() { this.BlueSpotLight, this.GreenSpotLight, this.RedSpotLight, this.RingLight };
        }
    }
}
