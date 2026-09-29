using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Infrastructure.AOP.Module;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    /// <summary>
    /// 系统2点胶参数
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class S2DispenseDevicePara : PropertyChangeAop
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
        /// 挤胶水位置
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
        /// 在示教点胶针的时候，点胶针在预点胶板上的出胶出胶时间
        /// </summary>
        [TreeProgramListArgs("在示教点胶针时的挤胶时间", (string)null)]
        public double AssistanceOpenDripTime { get; set; } = 500;

        /// <summary>
        /// 预点胶板参数
        /// </summary>
        public S2PreDispensePlatePara S2PreDispensePlatePara { get; set; } = new S2PreDispensePlatePara();

        /// <summary>
        /// 测高针和焊头之间的距离
        /// </summary>
        public AKRSPoint3D DistanceByMhToBh { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 激光测高到位等待时间
        /// </summary>
        [TreeProgramListArgs("激光测高到位等待时间", null, 0, 5000, "ms")]
        public int LaserMhDelayTime { get; set; } = 50;

        /// <summary>
        /// 测高报警距离
        /// </summary>
        [TreeProgramListArgs("测高距离误差报警阈值", null, 0, 5, "mm")]
        public double MeasureHeightAlarmDistance { get; set; } = 1;

        /// <summary>
        /// 蘸胶位置
        /// </summary>
        [TreeProgramListArgs("蘸胶位置", (string)null)]
        public AKRSPoint3D DippingPosition { get; set; }
    }
}
