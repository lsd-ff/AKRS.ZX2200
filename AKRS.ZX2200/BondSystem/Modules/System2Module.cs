using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using Newtonsoft.Json;

namespace AKRS.ZX2200.BondSystem.Modules
{
    /// <summary>
    /// 系统2模组
    /// </summary>
    public class System2Module : SingletonNoSave<System2Module>
    {
        /// <summary>
        /// 固晶模组
        /// </summary>
        public BondModule BondModule { get; set; } = new BondModule();

        /// <summary>
        /// 上视模组
        /// </summary>
        public UpLookModule UpLookModule { get; set; } = new UpLookModule();

        /// <summary>
        /// 吸嘴架模组
        /// </summary>
        public NozzleShelfModule NozzleShelfModule { get; set; } = new NozzleShelfModule();

        /// <summary>
        /// 中转台
        /// </summary>
        public IPTModule IPTModule { get; set; } = new IPTModule();

        /// <summary>
        ///  BMC真空
        /// </summary>
        public Electric BMCVacuumElectric => HardwareRepositoryService.GetHardware<Electric>("BMC真空电磁阀");

        /// <summary>
        /// 校正台负压检测
        /// </summary>
        [JsonIgnore]
        public Sensor CaliTableVaccumCheck => HardwareRepositoryService.GetHardware<Sensor>("校正台负压检测");
    }
}
