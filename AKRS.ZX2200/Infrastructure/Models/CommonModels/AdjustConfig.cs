#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/8/31 19:49:58
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using System;

    /// <summary>
    /// 描述：定位配置
    /// </summary>
    [Serializable]
    public class AdjustConfig
    {
        /// <summary>
        /// P1 PR名称
        /// </summary>
        public string P1PRName { get; set; } = string.Empty;

        /// <summary>
        /// P1视觉位置(焊后保存相对Module坐标)
        /// </summary>
        public AKRSPoint3D P1VisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P1模板拍照角度,只有1个角度，P2也用这个
        /// </summary>
        public double PRVisionAngle { get; set; }
        
        /// <summary>
        /// 视觉中心和芯片实际中心的距离
        /// </summary>
        public AKRSPoint3D DistanceForVisionCenterAndComponentCenter { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 视觉中心和芯片实际中心的角度
        /// </summary>
        public double AngleForVisionCenterAndComponentCenter { get; set; } = 0;


        /// <summary>
        /// P1模板定位点(焊后保存相对Module坐标)
        /// </summary>
        public AKRSPoint3D P1VisionMatchResultInModule { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P1 Reference名称
        /// </summary>
        public string P1ReferName { get; set; } = string.Empty;

        /// <summary>
        /// P1 Reference视觉位置(焊后保存相对Module坐标)
        /// </summary>
        public AKRSPoint3D P1ReferVisionPos { get; set; } = new AKRSPoint3D();
        
        /// <summary>
        /// P1 Reference模板定位结果(焊后保存相对Module坐标)
        /// </summary>
        public AKRSPoint3D P1ReferVisionMatchResultInModule { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P2 PR名称
        /// </summary>
        public string P2PRName { get; set; }  = string.Empty;

        /// <summary>
        /// P2视觉位置
        /// </summary>
        public AKRSPoint3D P2VisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P2模板定位结果G0
        /// </summary>
        public AKRSPoint3D P2VisionMatchResultInModule { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P2 Reference名称
        /// </summary>
        public string P2ReferName { get; set; } = string.Empty;

        /// <summary>
        /// P2 ReferenceG0视觉位置   (焊后保存相对位置)
        /// </summary>
        public AKRSPoint3D P2ReferVisionPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P2 Reference模板定位结果G0
        /// </summary>
        public AKRSPoint3D P2ReferVisionMatchResultInModule { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// P1 PR名称
        /// </summary>
        public string BacsideCrackDetectPRName { get; set; } = string.Empty;

        /// <summary>
        /// 识别CodeVision的PR名称
        /// </summary>
        public string CodeVisionPRName { get; set; } = string.Empty;

        /// <summary>
        /// 识别CodeVision的视觉位置
        /// </summary>
        public AKRSPoint3D CodeVisionPRPos { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 定位1使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig1 { get; set; } = new LocateUseConfig();

        /// <summary>
        /// 定位2使用情况
        /// </summary>
        public LocateUseConfig LocateUseConfig2 { get; set; } = new LocateUseConfig();
    }
}
