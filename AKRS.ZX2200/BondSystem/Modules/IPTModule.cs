using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Modules
{
    /// <summary>
    /// 中转台模组
    /// </summary>
    public class IPTModule
    {
        /// <summary>
        /// 中转台真空电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric IPTVacuumElectric => HardwareRepositoryService.GetHardware<Electric>("中转台真空电磁阀");

        /// <summary>
        /// 中转台真空电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric IPTBlowElectric => HardwareRepositoryService.GetHardware<Electric>("中转台吹气电磁阀");

        /// <summary>
        /// 右边中转台真空电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric RightIPTVacuumElectric => HardwareRepositoryService.GetHardware<Electric>("右中转台真空电磁阀");

        /// <summary>
        /// 右边中转台真空电磁阀
        /// </summary>
        [JsonIgnore]
        public Electric RightIPTBlowElectric => HardwareRepositoryService.GetHardware<Electric>("右中转台吹气电磁阀");

        /// <summary>
        /// 中转台旋转轴
        /// </summary>
        [JsonIgnore]
        public Axis IPTRotaryAxis => HardwareRepositoryService.GetHardware<Axis>("中转台旋转轴");

        /// <summary>
        /// 中转台负压检测
        /// </summary>
        [JsonIgnore]
        public Sensor 中转台负压检测 => HardwareRepositoryService.GetHardware<Sensor>("中转台负压检测");

        /// <summary>
        /// 中转台负压检测
        /// </summary>
        [JsonIgnore]
        public Sensor 右中转台负压检测 => HardwareRepositoryService.GetHardware<Sensor>("右中转台负压检测");

        /// <summary>
        /// 开IPT平台真空
        /// </summary>
        public void OpenIPTVacuum()
        {
            this.IPTVacuumElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 关IPT平台真空
        /// </summary>
        public void CloseIPTVacuum()
        {
            this.IPTVacuumElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 开IPT平台吹气
        /// </summary>
        public void OpenIPTBlow()
        {
            this.IPTBlowElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 关IPT平台吹气
        /// </summary>
        public void CloseIPTBlow()
        {
            this.IPTBlowElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 中转台旋转
        /// </summary>
        /// <param name="angle">角度</param>
        public void IPTRotary(double angle)
        {
            this.IPTRotaryAxis.AbsoluteMove(angle);
        }
    }
}
