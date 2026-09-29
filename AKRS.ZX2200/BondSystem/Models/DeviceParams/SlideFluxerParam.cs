using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.SupportFeature.Parameters.Model;

namespace AKRS.ZX2200.BondSystem.Models.DeviceParams
{
    /// <summary>
    /// 刮胶盘参数
    /// </summary>
    public class SlideFluxerParam
    {
        /// <summary>
        /// 刮胶盘位置
        /// </summary>
        [TreeProgramListArgs("刮胶盘位置(G0)", (string)null, false, null, RoleEnum.Admin)]
        public AKRSPoint3D SlideFluxerPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 刮胶盘右下位置
        /// </summary>
        [TreeProgramListArgs("刮胶盘右下位置(G0)", (string)null, false, null, RoleEnum.Admin)]
        public AKRSPoint3D SlideFluxerRightBottomPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 刮胶盘左上位置
        /// </summary>
        [TreeProgramListArgs("刮胶盘左上位置(G0)", (string)null, false, null, RoleEnum.Admin)]
        public AKRSPoint3D SlideFluxerLeftTopPos { get; set; } = new AKRSPoint3D();     
    }
}
