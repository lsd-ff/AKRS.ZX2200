using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Modules
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;

    /// <summary>
    /// 视觉模组
    /// </summary>
    public class VisionModule
    {
        /// <summary>
        /// 点胶相机
        /// </summary>
        public AKRSCamera DispenseCamera => HardwareRepositoryService.GetHardware<AKRSCamera>("点胶相机");

        /// <summary>
        /// 点光红光
        /// </summary>
        public Light SpotLightRed { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色点光-红");

        /// <summary>
        /// 点光绿光
        /// </summary>
        public Light SpotLightGreed { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色点光-绿");

        /// <summary>
        /// 点光蓝光
        /// </summary>
        public Light SpotLightBlue { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色点光-蓝");

        /// <summary>
        /// 环光红光
        /// </summary>
        public Light RingLightRed { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色环光-红");

        /// <summary>
        /// 环光绿光
        /// </summary>
        public Light RingLightGreed { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色环光-绿");

        /// <summary>
        /// 环光蓝光
        /// </summary>
        public Light RingLightBlue { get; set; } = HardwareRepositoryService.GetHardware<Light>("点胶三色环光-蓝");
    }
}
