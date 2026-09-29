namespace AKRS.ZX2200.WaferSubSystem.Modules
{
    using System;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.XtraEditors;

    using Newtonsoft.Json;

    /// <summary>
    /// 料架模组
    /// </summary>
    public class MagazineBoxModule
    {
        /// <summary>
        /// 上晶圆Z
        /// </summary>        
        [JsonIgnore]
        public Axis MagazineAxisZ { get; set; } = HardwareRepositoryService.GetHardware<Axis>("上晶圆Z");

        /// <summary>
        /// 晶圆盒挡料气缸
        /// </summary>        
        [JsonIgnore]
        public Electric FixedCylinder { get; set; } = HardwareRepositoryService.GetHardware<Electric>("上晶圆提篮夹紧气缸电磁阀");

        /// <summary>
        /// 晶圆盒挡料盒气缸负限位
        /// </summary>        
        [JsonIgnore]
        public Sensor FixedCylinderNLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆提篮夹紧气缸原点松开检测");

        /// <summary>
        /// 晶圆盒挡料盒气缸正限位
        /// </summary>        
        [JsonIgnore]
        public Sensor FixedCylinderPLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆提篮夹紧气缸动点夹紧检测");
        
        /// <summary>
        /// 晶圆推料气缸
        /// </summary>        
        [JsonIgnore]
        public Electric WaferPushCylinder { get; set; } = HardwareRepositoryService.GetHardware<Electric>("上晶圆推料气缸电磁阀");

        /// <summary>
        /// 晶圆推料气缸负限位
        /// </summary>        
        [JsonIgnore]
        public Sensor WaferPushCylinderNLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆推料气缸原点退回检测");

        /// <summary>
        /// 晶圆推料气缸正限位
        /// </summary>        
        [JsonIgnore]
        public Sensor WaferPushCylinderPLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆推料气缸动点推出检测");

        /// <summary>
        /// 槽位感应感应器
        /// </summary>        
        [JsonIgnore]
        public Sensor SlotScanSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆提篮料层检测");


        /// <summary>
        /// 上晶圆提篮检测
        /// </summary>        
        [JsonIgnore]
        public Sensor MagazineCheckSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("上晶圆提篮检测");
    }
}
