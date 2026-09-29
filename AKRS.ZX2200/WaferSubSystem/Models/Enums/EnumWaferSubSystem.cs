using System.ComponentModel;

namespace AKRS.ZX2200.WaferSubSystem.Models.Enums
{
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    /// <summary>
    /// 料片类型枚举
    /// </summary>
    public enum TabletTypeEnum
    {
        /// <summary>
        /// Wafer
        /// </summary>
        [Description("Componet")]
        Componet = 0,

        /// <summary>
        /// Adapter
        /// </summary>
        [Description("Adapter")]
        Adapter = 1,

        /// <summary>
        /// Null
        /// </summary>
        [Description("Null")]
        Null = 2
    }

    /// <summary>
    /// 单颗搜索方向
    /// </summary>
    public enum SingleSearchDirection
    {
        /// <summary>
        /// 右下
        /// </summary>
        [Description("从左往右，从上往下")]
        ToRightDown = 0,

        /// <summary>
        /// 左下
        /// </summary>
        [Description("从右往左，从上往下")]
        ToLeftDown = 1,

        /// <summary>
        /// 右上
        /// </summary>
        [Description("从左往右，从下往上")]
        ToRightUp = 2,

        /// <summary>
        /// 左上
        /// </summary>
        [Description("从右往左，从下往上")]
        ToLeftUp = 3,

        /// <summary>
        /// 下右
        /// </summary>
        [Description("从上往下，从左往右")]
        ToDownRight = 4,

        /// <summary>
        /// 下左
        /// </summary>
        [Description("从上往下，从右往左")]
        ToDownLeft = 5,

        /// <summary>
        /// 上右
        /// </summary>
        [Description("从下往上，从左往右")]
        ToUpRight = 6,

        /// <summary>
        /// 上左
        /// </summary>
        [Description("从下往上，从右往左")]
        ToUpLeft = 7,
    }

    /// <summary>
    /// 九颗搜索方向
    /// </summary>
    public enum NineSearchDirection
    {
        /// <summary>
        /// 九点从上往下
        /// </summary>
        [Description("九点从上往下")]
        NineUpToDown = 0,

        /// <summary>
        /// 九点从下往上
        /// </summary>
        [Description("九点从下往上")]
        NineDownToUp = 1,

        /// <summary>
        /// 九点从左往右
        /// </summary>
        [Description("九点从左往右")]
        NineLeftToRight = 2,

        /// <summary>
        /// 九点从右往左
        /// </summary>
        [Description("九点从右往左")]
        NineRightToLeft = 3,
    }

    /// <summary>
    /// 搜寻方式枚举
    /// </summary>
    public enum SearchMode
    {
        /// <summary>
        /// Single
        /// </summary>
        [Description("单颗")]
        Single = 0,

        /// <summary>
        /// Nine
        /// </summary>
        [Description("九颗")] Nine = 1,

        /// <summary>
        /// 晶圆图
        /// </summary>
        [Description("晶圆图")] WaferMap = 2,

        /// <summary>
        /// 华夫盒
        /// </summary>
        [Description("华夫盒")] Box = 3,
    }

    /// <summary>
    /// 行方向枚举
    /// </summary>
    public enum RowDirection
    {
        /// <summary>
        /// 从左到右
        /// </summary>
        [Description("从左到右")] LeftToRight = 0,

        /// <summary>
        /// 从右到左
        /// </summary>
        [Description("从右到左")] RightToLeft = 1
    }

    /// <summary>
    /// 华夫盒状态枚举
    /// </summary>
    public enum StatusEnum
    {
        /// <summary>
        /// 无料
        /// </summary>
        [Description("无料")]
        Null = 0,

        /// <summary>
        /// 有料
        /// </summary>
        [Description("有料")]
        Have = 1,
    }

    /// <summary>
    /// magazine位置方向枚举
    /// </summary>
    public enum MagazineDirectionEnum
    {
        /// <summary>
        /// 从上往下
        /// </summary>
        [Description("从上往下")]
        TopToDrow = 0,

        /// <summary>
        /// 从下往上
        /// </summary>
        [Description("从下往上")]
        DownToTop = 1,
    }

    /// <summary>
    /// 晶圆尺寸枚举
    /// </summary>
    public enum WaferSizeEnum
    {
        /// <summary>
        /// 4寸
        /// </summary>
        [Description("4寸")]
        FourInches = 0,

        /// <summary>
        /// 6寸
        /// </summary>
        [Description("6寸")]
        SixInches = 1,

        /// <summary>
        /// 8寸
        /// </summary>
        [Description("8寸")]
        EightInches = 2,

        /// <summary>
        /// 12寸
        /// </summary>
        [Description("12寸")]
        TwelveInches = 3,
    }

    /// <summary>
    /// 载具类型枚举
    /// </summary>
    public enum CarrierTypeEnum
    {
        /// <summary>
        /// 晶圆
        /// </summary>
        [Description("Wafer")]
        Wafer = 0,

        /// <summary>
        /// 华夫盒
        /// </summary>
        [Description("Waffle pack")]
        Waffle = 1,

        /// <summary>
        /// 静态华夫盒
        /// </summary>
        [Description("Static waffle pack")]
        StaticWaffle = 2,
    }

    /// <summary>
    /// 顶针类型枚举
    /// </summary>
    public enum EjectionTypeEnum
    {
        /// <summary>
        /// Standard
        /// </summary>
        [Description("Standard")]
        Standard = 0,

        /// <summary>
        /// 1 needle mini
        /// </summary>
        [Description("1 needle mini")]
        NeedleMini = 1,

        /// <summary>
        /// Needleless
        /// </summary>
        [Description("Needleless")]
        Needleless = 2,

        /// <summary>
        /// Membrane tool
        /// </summary>
        [Description("Membrane tool")]
        MembraneTool = 3,

        /// <summary>
        /// Multi pin tool
        /// </summary>
        [Description("Multi pin tool")]
        MultiPinTool = 4,
    }

    /// <summary>
    /// 槽位状态枚举
    /// </summary>
    public enum SlotStatuEnum
    {
        /// <summary>
        /// None
        /// </summary>
        [AcupointAttribute("None", "LightSteelBlue")]
        None = 0,

        /// <summary>
        /// Bad
        /// </summary>
        [AcupointAttribute("Bad", "Turquoise")]
        Bad = 1,

        /// <summary>
        /// Good
        /// </summary>
        [AcupointAttribute("Good", "LawnGreen")]
        Good = 2,
    }

    /// <summary>
    /// 顶针槽位状态枚举
    /// </summary>
    public enum EjectSlotStatuEnum
    {
        /// <summary>
        /// Empty
        /// </summary>
        [Description("空")]
        Empty = 0,

        /// <summary>
        /// Using
        /// </summary>
        [Description("使用中")]
        Using = 1,

        /// <summary>
        /// UnUsing
        /// </summary>
        [Description("未使用")]
        UnUsing = 2,
    }

    /// <summary>
    /// 方向枚举
    /// </summary>
    public enum DirectionEnum
    {
        /// <summary>
        /// East
        /// </summary>
        [Description("East")]
        East = 0,

        /// <summary>
        /// South
        /// </summary>
        [Description("South")]
        South = 1,

        /// <summary>
        /// West
        /// </summary>
        [Description("West")]
        West = 2,

        /// <summary>
        /// North
        /// </summary>
        [Description("North")]
        North = 3,

        /// <summary>
        /// Centre
        /// </summary>
        [Description("Centre")]
        Centre = 4,
    }

    /// <summary>
    /// 载具（华夫盒）类型枚举
    /// </summary>
    public enum WaffleTypeEnum
    {
        /// <summary>
        /// 华夫盒 (不带真空)
        /// </summary>
        [Description("华夫盒 (不带真空)")]
        Wafflepack = 0,

        /// <summary>
        /// 华夫盒 (带真空)
        /// </summary>
        [Description("华夫盒 (带真空)")]
        Gelpack = 1,
    }

    /// <summary>
    /// AccuracyMode枚举
    /// </summary>
    public enum AccuracyModeEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Off = 0,

        /// <summary>
        /// 半自动
        /// </summary>
        [Description("半自动")]
        SemiAutomatic = 1,

        /// <summary>
        /// 可配置
        /// </summary>
        [Description("可配置")]
        Configurable = 2,

        /// <summary>
        /// 超边界搜索
        /// </summary>
        [Description("超边界搜索")]
        OutsideEdgeSearch = 3,
    }

    /// <summary>
    /// CarrierShapeTypeEnum
    /// </summary>
    public enum CarrierShapeTypeEnum
    {
        /// <summary>
        /// 圆形
        /// </summary>
        [Description("圆形")]
        Circular = 0,

        /// <summary>
        /// 矩形
        /// </summary>
        [Description("矩形")]
        Rectangular = 1,

        /// <summary>
        /// 圆形和矩形
        /// </summary>
        [Description("圆形和矩形")]
        CircularAndRectangular = 2,
    }

    /// <summary>
    /// CarrierIDReaderModeEnum
    /// </summary>
    public enum CarrierIDReaderModeEnum
    {
        /// <summary>
        /// 不存在
        /// </summary>
        [Description("不存在")]
        None = 0,

        /// <summary>
        /// 屏幕提示
        /// </summary>
        [Description("屏幕提示")]
        ScreenDialogue = 1,

        /// <summary>
        /// 晶圆扫码器
        /// </summary>
        [Description("晶圆扫码器")]
        WaferBarcodeReader = 2,

        /// <summary>
        /// 手持扫码器
        /// </summary>
        [Description("手持扫码器")]
        HandBarcodeReader = 3,

        /// <summary>
        /// 二维码读取器
        /// </summary>
        [Description("二维码读取器")]
        DataMatrixReader = 4,
    }

    /// <summary>
    /// PositionSearchModeEnum/AccuracySearchModeEnum
    /// </summary>
    public enum PositionSearchModeEnum
    {
        /// <summary>
        /// 标准搜索
        /// </summary>
        [Description("标准搜索")]
        StandardSearch = 0,

        /// <summary>
        /// 多点搜索
        /// </summary>
        [Description("多点搜索")]
        MultipleSearch = 1,
    }

    /// <summary>
    /// CarrierGeoSetModeEnum
    /// </summary>
    public enum CarrierGeoSetModeEnum
    {
        /// <summary>
        /// 标准
        /// </summary>
        [Description("标准")]
        Standard = 0,

        /// <summary>
        /// 数字
        /// </summary>
        [Description("数字")]
        Numeric = 1,

        /// <summary>
        /// 自动
        /// </summary>
        [Description("自动")]
        Automatic = 2,
    }

    /// <summary>
    /// CarrierAdjustModeEnum
    /// </summary>
    public enum CarrierAdjustModeEnum
    {
        /// <summary>
        /// 1点校正
        /// </summary>
        [Description("1点校正")]
        Point1Adjust = 0,

        /// <summary>
        /// 2点校正
        /// </summary>
        [Description("2点校正")]
        Point2Adjust = 1,

        /// <summary>
        /// 3点校正
        /// </summary>
        [Description("3点校正")]
        Point3Adjust = 2,
    }

    /// <summary>
    /// PointArrangementModeEnum
    /// </summary>
    public enum PointArrangementModeEnum
    {
        /// <summary>
        /// 任意
        /// </summary>
        [Description("任意")]
        Any = 0,

        /// <summary>
        /// 对称
        /// </summary>
        [Description("对称")]
        Symmetrical = 1,
    }

    /// <summary>
    /// CarrierChangeTypeEnum
    /// </summary>
    public enum TabletChangeTypeEnum
    {
        /// <summary>
        /// 自动更换
        /// </summary>
        [Description("自动更换")]
        AutomaticChange = 0,

        /// <summary>
        /// 手动更换
        /// </summary>
        [Description("手动更换")]
        ManualChange = 1,
    }

    /// <summary>
    /// AutoRunWithDieModeEnum
    /// </summary>
    public enum AutoRunWithDieModeEnum
    {
        /// <summary>
        /// 自动
        /// </summary>
        [Description("自动")]
        Automatic = 0,

        /// <summary>
        /// 手动更换新料片
        /// </summary>
        [Description("手动更换新料片")]
        Manual = 1,

        /// <summary>
        /// 手动更换新料片在产品开始时
        /// </summary>
        [Description("手动更换新料片在产品开始时")]
        ManualAtStart = 2,
    }

    /// <summary>
    /// RotationSearchRangeEnum
    /// </summary>
    public enum RotationSearchRangeEnum
    {
        /// <summary>
        /// 0 degrees or 180 degrees
        /// </summary>
        [Description("0度或180度")]
        Half = 0,

        /// <summary>
        /// 0 度, 90 度, 180 度 或 270 度
        /// </summary>
        [Description("0 度, 90 度, 180 度 或 270 度")]
        All = 1,
    }

    /// <summary>
    /// RotaryPositionDeterminationModeEnum
    /// </summary>
    public enum RotaryPositionDeterminationModeEnum
    {
        /// <summary>
        /// 不存在
        /// </summary>
        [Description("不存在")]
        None = 0,

        /// <summary>
        /// 旋转模式
        /// </summary>
        [Description("旋转模式")]
        RotaryMode = 1,

        /// <summary>
        /// 载具传送校正
        /// </summary>
        [Description("载具传送校正")]
        ComponentCarrierAdjust = 2,

        /// <summary>
        /// 定位搜索
        /// </summary>
        [Description("定位搜索")]
        PositionSearch = 3,
    }

    /// <summary>
    /// RotaryModeSequenceEnum
    /// </summary>
    public enum RotaryModeSequenceEnum
    {
        /// <summary>
        /// 先下后右
        /// </summary>
        [Description("先下后右")]
        DownRight = 0,

        /// <summary>
        /// 仅下
        /// </summary>
        [Description("仅下")]
        DownOnly = 1,

        /// <summary>
        /// 仅右
        /// </summary>
        [Description("仅右")]
        RightOnly = 2,

        /// <summary>
        /// 手动
        /// </summary>
        [Description("手动")]
        Manual = 3,
    }

    /// <summary>
    /// IsOpenEnum
    /// </summary>
    public enum IsOpenEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Off = 0,

        /// <summary>
        /// 打开
        /// </summary>
        [Description("打开")]
        On = 1,
    }

    /// <summary>
    /// 搜寻方向枚举
    /// </summary>
    public enum SearchDirection
    {
        /// <summary>
        /// 按行
        /// </summary>
        ByRow = 0,

        /// <summary>
        /// 按列
        /// </summary>
        ByCol = 1
    }

    /// <summary>
    /// 适配器类型枚举
    /// </summary>
    public enum AdapterTypeEnum
    {
        /// <summary>
        /// 晶圆台
        /// </summary>
        [Description("晶圆台")]
        WaferTable = 0,

        /// <summary>
        /// 静态
        /// </summary>
        [Description("静态")]
        Static = 1,
    }


    /// <summary>
    /// 搜晶相机类型枚举
    /// </summary>
    public enum SearchCameraEnum
    {
        /// <summary>
        /// Bond相机 
        /// </summary>
        [Description("Bond相机")]
        BondCamera = 0,

        /// <summary>
        /// 晶圆相机
        /// </summary>
        [Description("晶圆相机")]
        WaferCamera = 1,
    }

    /// <summary>
    /// 中转台类型枚举
    /// </summary>
    public enum IPTTypeEnum
    {
        /// <summary>
        /// 左中转台
        /// </summary>
        [Description("左中转台")]
        LeftIPT = 0,

        /// <summary>
        /// 右中转台
        /// </summary>
        [Description("右中转台")]
        RightIPT = 1,
    }
}
