using System.ComponentModel;

namespace AKRS.Galaxy2.PR.Models.CommonModels
{
    /// <summary>
    /// 流程名称
    /// </summary>
    public enum AlgFlowTypeEnum
    {
        /// <summary>
        /// 快速匹配
        /// </summary>
        [Description("快速匹配")]
        FastModelAlg,

        /// <summary>
        /// 高精度匹配
        /// </summary>
        [Description("高精度匹配")]
        HighPreModelAlg,

        /// <summary>
        /// 带粗定位高精度匹配
        /// </summary>
        [Description("带粗定位高精度匹配")]
        HighPreModelAlgWithRefer,

        /// <summary>
        /// 带墨点检测的高精度匹配
        /// </summary>
        [Description("带墨点检测的高精度匹配")]
        HighPreModelAlgWithBlob,

        /// <summary>
        /// 灰度匹配
        /// </summary>
        [Description("灰度匹配")]
        NccModelAlg,

        /// <summary>
        /// 轮廓匹配
        /// </summary>
        [Description("轮廓匹配")]
        XldModelAlg,

        /// <summary>
        /// 胶量检测
        /// </summary>
        [Description("胶量检测")]
        EpoxyDetectAlg,

        /// <summary>
        /// 焊后检测
        /// </summary>
        [Description("焊后检测")]
        PostBondDetectAlg,

        /// <summary>
        /// 墨点检测
        /// </summary>
        [Description("墨点检测")]
        InkDotDetectAlg,

        /// <summary>
        /// 缺角检测
        /// </summary>
        [Description("缺角检测")]
        CornerDetectAlg,

        /// <summary>
        /// 防反检测
        /// </summary>
        [Description("防反检测")]
        PreventReverseAlg,

        /// <summary>
        /// 防错检测
        /// </summary>
        [Description("防错检测")]
        PreventWrongAlg,

        /// <summary>
        /// 矩形检测
        /// </summary>
        [Description("矩形检测")]
        RectangleDetectAlg,

        /// <summary>
        /// 圆查找
        /// </summary>
        [Description("圆查找")]
        CircleFindAlg,

        /// <summary>
        /// 清晰度评价
        /// </summary>
        [Description("清晰度评价")]
        FocusMeasureAlg,

        /// <summary>
        /// 中心查找
        /// </summary>
        [Description("中心查找")]
        CenterSearchAlg,

        /// <summary>
        /// 交点查找
        /// </summary>
        [Description("交点查找")]
        CrossSearchAlg,

        /// <summary>
        /// 基准查找
        /// </summary>
        [Description("基准查找")]
        FiducialFindAlg,

        /// <summary>
        /// 线检测
        /// </summary>
        [Description("线检测")]
        LineDetectAlg,

        /// <summary>
        /// 距离测量
        /// </summary>
        [Description("距离测量")]
        DistanceMeasureAlg,

        /// <summary>
        /// 矩形检测2
        /// </summary>
        [Description("矩形检测2")]
        RectangleSecondDetectAlg,

        /// <summary>
        /// 对称模板匹配
        /// </summary>
        [Description("对称模板匹配")]
        SymmetricModeleAlg,

        /// <summary>
        /// 带缺角检测的轮廓匹配
        /// </summary>
        [Description("带缺角检测的轮廓匹配")]
        HighPreModelAlgWithCorner,

        /// <summary>
        /// 组合定位
        /// </summary>
        [Description("组合定位")]
        HybridPositionAlg,

        /// <summary>
        /// FC上视定位
        /// </summary>
        [Description("FC上视定位")]
        FcUplookModelAlg,

        /// <summary>
        /// FC崩边检测
        /// </summary>
        [Description("FC崩边检测")]
        FcEdgeBreakDetectAlg,

        /// <summary>
        /// 表面缺陷检测
        /// </summary>
        [Description("表面缺陷检测")]
        SurfaceDetectAlg,

        /// <summary>
        /// 崩边检测
        /// </summary>
        [Description("崩边检测")]
        EdgeBreakDetectAlg,

        /// <summary>
        /// 二维码识别
        /// </summary>
        [Description("二维码识别")]
        QrCodeDetectAlg,


        /// <summary>
        /// 圆心和角度
        /// </summary>
        [Description("圆心和角度")]
        CircleAndLineAlg,
    }

    /// <summary>
    /// 定位方式
    /// </summary>
    public enum LocateTypeEnum
    {
        /// <summary>
        /// 快速匹配
        /// </summary>
        [Description("快速匹配")]
        FastModel,

        /// <summary>
        /// 高精度匹配
        /// </summary>
        [Description("高精度匹配")]
        HighPreModel,

        /// <summary>
        /// 灰度匹配
        /// </summary>
        [Description("灰度匹配")]
        NccModel,

        /// <summary>
        /// 轮廓匹配
        /// </summary>
        [Description("轮廓匹配")]
        XldModel,
    }

    /// <summary>
    /// 算法归属
    /// </summary>
    public enum AlgBeLongEnum
    {
        /// <summary>
        /// 标定
        /// </summary>
        [Description("标定")]
        Calibration,

        /// <summary>
        /// 晶圆
        /// </summary>
        [Description("晶圆")]
        Component,

        /// <summary>
        /// 上视
        /// </summary>
        [Description("上视")]
        UpLook,

        /// <summary>
        /// 点胶
        /// </summary>
        [Description("点胶")]
        Dispense,

        /// <summary>
        /// 基板
        /// </summary>
        [Description("基板")]
        Substrate,

        /// <summary>
        /// 焊后
        /// </summary>
        [Description("焊后")]
        PostBond,
    }

    /// <summary>
    /// 匹配模板参数
    /// </summary>
    public enum MatchParamEnum
    {
        /// <summary>
        /// 角度
        /// </summary>
        [Description("角度")]
        Angle,

        /// <summary>
        /// 分数
        /// </summary>
        [Description("分数")]
        Score,
    }

    /// <summary>
    /// 边缘极性
    /// </summary>
    public enum EdgePolarityEnum
    {

        /// <summary>
        /// 从黑到白
        /// </summary>
        [Description("从黑到白")]
        BlackToWhite,

        /// <summary>
        /// 从白到黑
        /// </summary>
        [Description("从白到黑")]
        WhiteToBlack,

        /// <summary>
        /// 任意极性
        /// </summary>
        [Description("任意极性")]
        Both,
    }


    /// <summary>
    /// 边缘类型
    /// </summary>
    public enum EdgeTypeEnum
    {

        /// <summary>
        /// 最强
        /// </summary>
        [Description("最强")]
        Best,

        /// <summary>
        /// 第一条
        /// </summary>
        [Description("第一条")]
        First,

        /// <summary>
        /// 最后一条
        /// </summary>
        [Description("最后一条")]
        Last,
    }
}
