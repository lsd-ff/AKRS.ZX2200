using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 固晶设备参数,所有设备参数存的都是关于G0坐标系
    /// </summary>
    public class BondDevicePara : Singleton<BondDevicePara>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BondDevicePara()
        {
            Singleton<BondDevicePara>.FilePath = ZX2200PathConfig.BondDeviceParaPath;
        }

        /// <summary>
        /// 吸嘴架参数
        /// </summary>
        public NozzleShelfParam NozzleShelfParam { get; set; } = new NozzleShelfParam();

        /// <summary>
        /// 焊头参数
        /// </summary>
        public BondHeadParam BondHeadParam { get; set; } = new BondHeadParam();

        /// <summary>
        /// 测高平台参数
        /// </summary>
        public BMCDevicePara BMCDevicePara { get; set; } = new BMCDevicePara();

        /// <summary>
        /// 相机参数
        /// </summary>
        public CameraDevicePara CameraDevicePara { get; set; } = new CameraDevicePara();

        /// <summary>
        /// IPT参数
        /// </summary>
        public IPTDevicePara IPTDevicePara { get; set; } = new IPTDevicePara();

        /// <summary>
        /// ULM运动配置参数
        /// </summary>
        public ULMPara ULMPara { get; set; } = new ULMPara();

        /// <summary>
        /// 系统2点胶参数
        /// </summary>
        public S2DispenseDevicePara S2DispenseDevicePara { get; set; } = new S2DispenseDevicePara();

        /// <summary>
        /// 刮胶盘参数
        /// </summary>
        public SlideFluxerParam SlideFluxerParam { get; set; } = new SlideFluxerParam();
    }
}
