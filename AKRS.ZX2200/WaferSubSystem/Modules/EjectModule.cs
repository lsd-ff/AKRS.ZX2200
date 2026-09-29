using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Modules
{
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.XtraEditors;
    using Newtonsoft.Json;

    /// <summary>
    /// 顶针台模组
    /// </summary>
    public class EjectModule
    {
        /// <summary>
        /// 顶针台Z
        /// </summary>        
        [JsonIgnore]
        public Axis EjectionTableAxisZ { get; set; } = HardwareRepositoryService.GetHardware<Axis>("顶针台Z");

        /// <summary>
        /// 顶针Z
        /// </summary>        
        [JsonIgnore]
        public Axis EjectionAxisZ { get; set; } = HardwareRepositoryService.GetHardware<Axis>("顶针Z");

        /// <summary>
        /// 顶针放置架T
        /// </summary>        
        [JsonIgnore]
        public Axis EjectionBankAxisT { get; set; } = HardwareRepositoryService.GetHardware<Axis>("顶针放置架T");

        /// <summary>
        /// 顶针台气缸
        /// </summary>        
        [JsonIgnore]
        public Electric FixedCylinder { get; set; } = HardwareRepositoryService.GetHardware<Electric>("顶针台气缸");

        /// <summary>
        /// 顶针换头气缸负限位
        /// </summary>        
        [JsonIgnore]
        public Sensor FixedCylinderNLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("顶针换头气缸负限位");

        /// <summary>
        /// 顶针换头气缸正限位
        /// </summary>        
        [JsonIgnore]
        public Sensor FixedCylinderPLimitSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("顶针换头气缸正限位");

        /// <summary>
        /// 晶圆顶针台真空
        /// </summary>        
        [JsonIgnore]
        public Electric EjectionTableVacuum { get; set; } = HardwareRepositoryService.GetHardware<Electric>("晶圆顶针台真空电磁阀");

        /// <summary>
        /// 晶圆顶针台吹气
        /// </summary>        
        [JsonIgnore]
        public Electric EjectionTableBlow { get; set; } = HardwareRepositoryService.GetHardware<Electric>("晶圆顶针台吹气电磁阀");

        /// <summary>
        /// 顶针台接触传感器1
        /// </summary>        
        [JsonIgnore]
        public Sensor EjectionTableContactSensor1 { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("顶针台接触传感器1");

        /// <summary>
        /// 顶针台接触传感器2
        /// </summary>        
        [JsonIgnore]
        public Sensor EjectionTableContactSensor2 { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("顶针台接触传感器2");

        // <summary>
        /// 顶针负压检测
        /// </summary>        
        [JsonIgnore]
        public Sensor EjectionVacuumDetectSensor { get; set; } = HardwareRepositoryService.GetHardware<Sensor>("顶针负压检测");
    }
}
