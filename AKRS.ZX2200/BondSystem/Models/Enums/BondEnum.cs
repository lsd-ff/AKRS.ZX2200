using System.ComponentModel;

namespace AKRS.ZX2200.BondSystem.Models.Enums
{
    /// <summary>
    /// 吸嘴测高模式枚举
    /// </summary>
    public enum HeightMeasurementFunctionEnum
    {
        /// <summary>
        /// 手动测高
        /// </summary>
        [Description("手动测高")]
        Manual = 0,

        /// <summary>
        /// 用接触传感器邦头测高
        /// </summary>
        [Description("接触测高")]
        WithTDSensor = 1,

        /// <summary>
        /// 用真空检测传感器邦头测高
        /// </summary>
        [Description("真空测高")]
        WithVacuumSensor = 2,


        /// <summary>
        /// 力传感器测高
        /// </summary>
        [Description("真空测高")]
        ForceSensor = 3,
    }

    /// <summary>
    /// 吸嘴测高方式枚举
    /// </summary>
    public enum ZDeterminationMethodEnum
    {
        /// <summary>
        /// 手动(Vacuum sensor)
        /// </summary>
        [Description("手动")]
        Manual = 0,

        /// <summary>
        /// MiniBMC测高（标准吸嘴，TD sensor）
        /// </summary>
        [Description("Mini BMC")]
        MiniBMC = 1,

        /// <summary>
        /// 压力传感器测高（Force  sensor）,这种方式暂时不考虑
        /// </summary>
        [Description("力传感器")]
        BondForceSensor = 2,
    }

    /// <summary>
    /// XY轴测定方法枚举
    /// </summary>
    public enum XYDeterminationMethodEnum
    {
        /// <summary>
        /// 基板相机
        /// </summary>
        [Description("基板相机")]
        SubstrateCamera = 0,

        /// <summary>
        /// 上视相机
        /// </summary>
        [Description("上视相机")]
        UpLookingCamera = 1,
    }

    /// <summary>
    /// 吸嘴类型枚举
    /// </summary>
    public enum NozzleTypeEnum
    {
        /// <summary>
        /// 测高
        /// </summary>
        [Description("标定")]
        Calibrate = 0,

        /// <summary>
        /// 取片/固晶
        /// </summary>
        [Description("取片/固晶")]
        PickAndPlace = 1,
    }

    /// <summary>
    /// 吸嘴尺寸枚举
    /// </summary>
    public enum NozzleSizeEnum
    {
        /// <summary>
        /// 大尺寸
        /// </summary>
        [Description("大尺寸")]
        Large = 0,

        /// <summary>
        /// 小尺寸
        /// </summary>
        [Description("小尺寸")]
        Small = 1,
    }

    /// <summary>
    /// 吸嘴形状枚举
    /// </summary>
    public enum NozzleShapeEnum
    {
        /// <summary>
        /// 圆形
        /// </summary>
        [Description("圆形")]
        Round = 0,

        /// <summary>
        /// 矩形
        /// </summary>
        [Description("矩形")]
        Rectangle = 1,
    }

    ///// <summary>
    ///// 吸嘴几何类型枚举，暂时不用
    ///// </summary>
    //public enum NozzleGeometryTypeEnum
    //{
    //    /// <summary>
    //    /// 普通吸嘴
    //    /// </summary>
    //    [Description("Tool without graduation")]
    //    ToolWithoutGraduation = 0,

    //    /// <summary>
    //    /// 多重吸嘴
    //    /// </summary>
    //    [Description("Multilevel tool")]
    //    MultilevelTool = 1,
    //}

    /// <summary>
    /// 吸嘴槽能容纳的最大吸嘴尺寸
    /// </summary>
    public enum MaximumToolSizeEnum
    {
        /// <summary>
        /// 大吸嘴
        /// </summary>
        [Description("大吸嘴")]
        LargeTool = 0,

        /// <summary>
        /// 小吸嘴
        /// </summary>
        [Description("小吸嘴")]
        SmallTool = 1,
    }

    /// <summary>
    /// 吸嘴槽状态枚举
    /// </summary>
    public enum NozzleStateEnum
    {
        /// <summary>
        /// 穴位上有吸嘴
        /// </summary>
        [Description("有吸嘴")]
        OnSlot = 0,

        /// <summary>
        /// 穴位上无吸嘴
        /// </summary>
        [Description("空")]
        Empty = 1,

        /// <summary>
        /// 穴位上的吸嘴在焊头上
        /// </summary>
        [Description("在焊头上")]
        OnBondHead = 2,
    }

    /// <summary>
    /// 吸嘴力反馈方式枚举
    /// </summary>
    public enum NozzleForceModeEnum
    {
        /// <summary>
        /// 力
        /// </summary>
        [Description("力")]
        Force = 0,

        /// <summary>
        /// ForceFeedback
        /// </summary>
        [Description("力反馈")]
        ForceFeedback = 1,

        /// <summary>
        /// 压力传感器测高（Force  sensor）
        /// </summary>
        [Description("压力传感器")]
        BondForceSensor = 2,
    }

    /// <summary>
    /// 取料/固晶力控模式枚举
    /// </summary>
    public enum ForceModeEnum
    {
        /// <summary>
        /// 距离模式
        /// </summary>
        [Description("距离模式")]
        Distance = 0,

        /// <summary>
        /// 力控模式
        /// </summary>
        [Description("力控模式")]
        Force = 1,
    }

    /// <summary>
    /// 速度模式枚举
    /// </summary>
    public enum SpeedModeEnum
    {
        /// <summary>
        /// 用户自由设置
        /// </summary>
        [Description("自由")]
        Free = 0,

        /// <summary>
        /// 低速5mmm/s
        /// </summary>
        [Description("低速")]
        Low = 1,

        /// <summary>
        /// 中速10mmm/s
        /// </summary>
        [Description("中速")]
        Medium = 2,

        /// <summary>
        /// 高速20mmm/s
        /// </summary>
        [Description("高速")]
        Fast = 3,
    }

    /// <summary>
    /// 视觉搜索点数量枚举
    /// </summary>
    public enum MeasurePointNumberEnum
    {
        /// <summary>
        /// 1点
        /// </summary>
        [Description("1点")]
        OnePoint = 0,

        /// <summary>
        /// 2点
        /// </summary>
        [Description("2点")]
        TwoPoint = 1,
    }
    
    /// <summary>
    /// 焊后检测模式枚举
    /// </summary>
    public enum PostBondInspectionModeEnum
    {
        /// <summary>
        /// 没有参考搜索 
        /// </summary>
        [Description(" 没有参考搜索 ")]
        WithoutReferenceSearch = 0,

        /// <summary>
        /// 有参考搜索
        /// </summary>
        [Description("有参考搜索")]
        WithReferenceSearch = 1,
    }

    /// <summary>
    /// 搜索模式枚举
    /// </summary>
    public enum PostBondInspectionSearchModeEnum
    {
        /// <summary>
        /// 标准搜索 
        /// </summary>
        [Description("焊后检测")]
        PostBondInspection = 0,

        /// <summary>
        /// 胶量检测
        /// </summary>
        [Description("胶量检测")]
        AdhesiveInspection = 1,
    }

    /// <summary>
    /// 焊后应用系统枚举
    /// </summary>
    public enum PostBondApplicationSystemEnum
    {
        /// <summary>
        /// Dispense点胶 
        /// </summary>
        [Description("胶量检测")]
        EpoxyCheck = 0,

        /// <summary>
        /// Bond贴片
        /// </summary>
        [Description("焊后检测")]
        AfterBondCheck = 1,

        /// <summary>
        /// BLT检测
        /// </summary>
        [Description("BLT检测")]
        BLTMeasure = 2,
    }

    /// <summary>
    /// 相机类型枚举
    /// </summary>
    public enum CameraTypeEnum
    {
        /// <summary>
        /// Bond相机 
        /// </summary>
        [Description("Bond相机")]
        BondCamera = 0,

        /// <summary>
        /// 上视相机
        /// </summary>
        [Description("上视相机")]
        UpLookCamera = 1,

        /// <summary>
        /// 上视相机
        /// </summary>
        [Description("点胶相机")]
        DispenseCamera = 2,

        /// <summary>
        /// Z-XY相机
        /// </summary>
        [Description("Z-XY相机")]
        ZWithXYCamera = 3,
    }

    /// <summary>
    /// 拾取类型枚举
    /// </summary>
    public enum PickTypeEnum
    {
        /// <summary>
        /// 晶圆
        /// </summary>
        [Description("晶圆")]
        CarrierWithWafer = 0,

        /// <summary>
        /// 华夫盒
        /// </summary>
        [Description("华夫盒")]
        CarrierWithWaffle = 1,

        /// <summary>
        /// 左IPT平台
        /// </summary>
        [Description("左IPT平台")]
        LeftIPT = 2,

        /// <summary>
        /// 右 IPT平台
        /// </summary>
        [Description("右IPT平台")]
        RightIPT = 3,
        
        /// <summary>
        /// bmc平台
        /// </summary>
        [Description("BMC")]
        BMC = 4,

        /// <summary>
        /// 翻转台
        /// </summary>
        [Description("翻转台")]
        FlipTable = 5,
    }

    /// <summary>
    /// 固晶类型枚举
    /// </summary>
    public enum BondTypeEnum
    {
        /// <summary>
        /// 在TU固晶
        /// </summary>
        [Description("在TU固晶")]
        BondOnTU = 0,

        /// <summary>
        /// 在IPT固晶
        /// </summary>
        [Description("在左IPT固晶")]
        BondOnLeftIPT = 1,

        /// <summary>
        /// 在BMC固晶
        /// </summary>
        [Description("在BMC固晶")]
        BondOnBMC = 2,

        /// <summary>
        /// 在TU蘸胶
        /// </summary>
        [Description("在TU蘸胶")]
        DipFluxOnTU = 3,

        /// <summary>
        /// 在右IPT固晶
        /// </summary>
        [Description("在右IPT固晶")]
        BondOnRightIPT = 4,
    }

    /// <summary>
    /// 重取枚举
    /// </summary>
    public enum RepickTypeEnum
    {
        /// <summary>
        /// 芯片
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// 取当前颗
        /// </summary>
        [Description("Repick")]
        Repick = 1,

        /// <summary>
        /// 下一颗（暂时不用）
        /// </summary>
        [Description("Next die")]
        NextDie = 2,
    }

    /// <summary>
    ///  光源配置枚举
    /// </summary>
    public enum LightConfigEnum
    {
        /// <summary>
        /// 单色光
        /// </summary>
        [Description("单色光")]
        MonochromaticLight = 0,

        /// <summary>
        /// 三色光
        /// </summary>
        [Description("三色光")]
        TrichromaticLight = 1,
    }

    /// <summary>
    ///  芯片蘸胶模式枚举
    /// </summary>
    public enum DipModeEnum
    {
        /// <summary>
        /// 不开启
        /// </summary>
        [Description("不开启")]
        Off = 0,

        /// <summary>
        /// 视觉纠偏前(先蘸胶后上视)
        /// </summary>
        [Description("视觉纠偏前")]
        BeforeAccuracyMode = 1,

        /// <summary>
        /// 视觉纠偏后(先上视后蘸胶)
        /// </summary>
        [Description("视觉纠偏后")]
        AfterAccuracyMode = 2,
    }

    /// <summary>
    /// 力读取类型
    /// </summary>
    public enum ForceReadTimingEnum
    {
        /// <summary>
        /// 贴片
        /// </summary>
        [Description("贴片")]
        Place = 0,

        /// <summary>
        /// 取片
        /// </summary>
        [Description("取片")]
        Pick = 1,

        /// <summary>
        /// 贴片和取片
        /// </summary>
        [Description("贴片和取片")]
        PickAndPlace = 2,

        /// <summary>
        /// 标定台测试
        /// </summary>
        [Description("标定台测试")]
        TestOnCaliTable = 3,
    }

    /// <summary>
    /// 力控标定模式枚举
    /// </summary>
    public enum ForceCaliModeEnum
    {
        /// <summary>
        /// 单角度
        /// </summary>
        [Description("单角度")]
        SingleAngle = 0,

        /// <summary>
        /// 多角度
        /// </summary>
        [Description("多角度")]
        MultipleAngle = 1,
    }

    /// <summary>
    ///  力控标定范围
    /// </summary>
    public enum ForceRangeEnum
    {
        /// <summary>
        /// 大力
        /// </summary>
        [Description("大力")]
        LargeForce=0,

        /// <summary>
        /// 小力
        /// </summary>
        [Description("小力")]
        SmallForce = 1,


        /// <summary>
        /// 小力和大力
        /// </summary>
        [Description("小力和大力")]
        SmallForceAndLargeForce = 2,
    }

    /// <summary>
    /// 力控清零方式
    /// </summary>
    public enum ForceZeroModeEnum
    {
        /// <summary>
        /// IO清零
        /// </summary>
        [Description("IO清零")]
        IO = 0,

        /// <summary>
        /// 小力
        /// </summary>
        [Description("Modbus清零")]
        Modbus = 1,
    }
}
