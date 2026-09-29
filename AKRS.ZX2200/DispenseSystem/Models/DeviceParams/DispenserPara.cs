namespace AKRS.ZX2200.DispenseSystem.Models.DeviceParams
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using PropertyChanged;

    /// <summary>
    /// 点胶头模块参数
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public class DispenserPara : PropertyChangeAop
    {
        /// <summary>
        /// 换胶水位置
        /// </summary>
        [TreeProgramListArgs("换胶水的位置", (string)null)]
        public AKRSPoint3D ReplaceGluePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 挤胶水位置
        /// </summary>
        [TreeProgramListArgs("挤胶的位置", (string)null)]
        public AKRSPoint3D ThrustPosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 擦胶的位置
        /// </summary>
        [TreeProgramListArgs("擦胶的位置", (string)null)]
        public AKRSPoint3D ErasePosition { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 标定相机中心和点胶头中心的差值的相机位
        /// </summary>
        [TreeProgramListArgs("相机寻找点胶点的位置", (string)null)]
        public AKRSPoint3D VisionPosForCalibration { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 标定相机中心和点胶头中心的差值的挤胶和测高位
        /// </summary>
        [TreeProgramListArgs("示教点胶头时的测高位置", (string)null)]
        public AKRSPoint3D DispenserMeasureHeightPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 蘸胶位置
        /// </summary>
        [TreeProgramListArgs("蘸胶位置", (string)null)]
        public AKRSPoint3D PrintingToolPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 在示教点胶针的时候，点胶针在预点胶板上的出胶出胶时间
        /// </summary>
        [TreeProgramListArgs("在示教点胶针时的挤胶时间", (string)null, 0, 10000, "ms")]
        public int AssistanceOpenDispenseTime { get; set; } = 500;

        /// <summary>
        /// 挤胶时的气压
        /// </summary>
        [TreeProgramListArgs("挤胶时的气压", (string)null, 30, 500, "千帕")]
        public int AssistanceOpenDispenseVacuum { get; set; } = 100;
    }
}
