namespace AKRS.ZX2200.DispenseSystem.Models.Enums
{
    using System.ComponentModel;

    /// <summary>
    /// 所用的点胶头
    /// 目前只有一个，对于后续增加做准备的
    /// </summary>
    public enum DispenserTypeEnum
    {
        /// <summary>
        /// 点胶
        /// </summary>
        [Description("点胶头")]
        Dispenser,

        [Description("蘸胶头")]
        PrintingTool,

        //[Description("喷胶")]
        //Jetter
    }

    /// <summary>
    /// 点胶测高的方式
    /// </summary>
    public enum DispenseDeterminationZTypeEnum
    {
        [Description("压力测高")]
        BondForceSensor,

        [Description("手动测高")]
        Manual
    }

    /// <summary>
    /// 点胶头和相机中心的距离
    /// </summary>
    public enum MeasureDistanceDispenserAndVisionTypeEnum
    {
        /// <summary>
        /// 手动计算
        /// </summary>
        Manual,

        /// <summary>
        /// 自动计算
        /// </summary>
        AutoMeasure,
    }

    /// <summary>
    /// 点胶搜索方式
    /// </summary>
    public enum DispenseSearchMethodEnum
    {
        [Description("中心搜索")]
        CenterSearch,

        [Description("基准搜索")]
        FiducialSearch,

        [Description("循环搜索")]
        CircleSearch,

        [Description("手动移动")]
        ManualDetermination
    }

    /// <summary>
    /// 预点胶的模式
    /// </summary>
    public enum PreDispensingModeEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Off,

        /// <summary>
        /// 周期性预点胶，时间后续设置
        /// </summary>
        [Description("周期性预点胶")]
        Periodic,
    }


    /// <summary>
    /// 预点胶的时机
    /// </summary>
    public enum PreDispensingTimingEnum
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Off,

        /// <summary>
        /// 周期性预点胶，时间后续设置
        /// </summary>
        [Description("进料后第一次点胶前")]
        BeforeComingFirstDispense,

        /// <summary>
        /// 周期性预点胶，时间后续设置
        /// </summary>
        [Description("每次点胶前")]
        BeforeEveryDispense,
    }

    /// <summary>
    /// 预点胶的位置
    /// 一般为预点胶板
    /// </summary>
    public enum PreDispensePositionEnum
    {
        [Description("预点胶板")]
        Plate,

        [Description("条，这个不知道")]
        Strips,

        [Description("插座，这个不知道")]
        Receptacle
    }

    /// <summary>
    /// 清洁类型
    /// </summary>
    public enum CleaningTypeEnum
    {
        [Description("关闭")]
        Off,

        [Description("在预点胶之前")]
        BeforePreDispense
    }

    /// <summary>
    /// 不知道什么意思
    /// </summary>
    public enum DispensingTypeEnum
    {
        /// <summary>
        /// 胶水应用策略
        /// </summary>
        [Description("胶水应用策略")]
        EpoxyApplication,

        /// <summary>
        /// 预点胶
        /// </summary>
        [Description("预点胶")]
        PreDispense
    }

    /// <summary>
    /// 点胶策略
    /// </summary>
    public enum EpoxyApplicationTypeEnum
    {
        /// <summary>
        /// 点胶
        /// </summary>
        [Description("单点点胶")]
        SingleDotOnly,

        /// <summary>
        /// 画胶
        /// </summary>
        [Description("画胶")]
        Drawing,

        /// <summary>
        /// 蘸胶
        /// </summary>
        [Description("蘸胶")]
        Printting,
    }

    /// <summary>
    /// 胶水的应用类型
    /// </summary>
    public enum EpoxyUseTypeEnum
    {
        [Description("点胶头出胶")]
        Epoxy,

        [Description("刷胶")]
        Flux
    }

    public enum RefillCriteriaEnum
    {
        [Description("时间")]
        Time,

        [Description("周期")]
        Cycle
    }

    public enum EpoxyLifetimeEntryMethodEnum
    {
        [Description("参数")]
        Parameter,

        [Description("手动输入")]
        ManualInput,

        [Description("扫码")]
        BarcodeScanner,

        [Description("远程输入")]
        SecsRemoteCommand
    }

    /// <summary>
    /// 系统1测高的传感器，不同的传感器有不同的计算方法
    /// </summary>
    public enum System1MeasHeightToolEnum
    {
        /// <summary>
        /// 测高针
        /// </summary>
        HeightSensor,

        /// <summary>
        /// 点胶头
        /// </summary>
        Dispenser
    }
}
