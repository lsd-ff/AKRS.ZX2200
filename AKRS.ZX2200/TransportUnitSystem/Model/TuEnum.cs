using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Model
{
    /// <summary>
    /// 描述：纠偏类型
    /// </summary>
    [Serializable]
    public enum AdjustTypeEnum
    {
        /// <summary>
        /// 不定位
        /// </summary>
        [Description("不定位")]
        None = 0,

        /// <summary>
        /// 单点
        /// </summary>
        [Description("1点定位")]
        OnePoint = 1,

        /// <summary>
        /// 两点
        /// </summary>
        [Description("2点定位")]
        TwoPoints = 2,

        /// <summary>
        /// 3点
        /// </summary>
        [Description("3点定位")]
        ThreePoints = 3,

        /// <summary>
        /// 4点
        /// </summary>
        [Description("4点定位")]
        FourPoints = 4
    }

    /// <summary>
    /// 相对固晶方式枚举
    /// </summary>
    public enum RelativeBondingEnum
    {
        /// <summary>
        /// 不开启
        /// </summary>
        [Description("不开启")]
        Off = 0,

        /// <summary>
        /// 1点
        /// </summary>
        [Description("1点")]
        OneSearch = 1,

        ///// <summary>
        ///// 两点
        ///// </summary>
        //[Description("TwoSearch")]
        //TwoSearch = 2,
    }

    /// <summary>
    /// 相对固晶两点搜索方式枚举
    /// </summary>
    public enum SearchTypeEnum
    {
        /// <summary>
        /// 标准搜索
        /// </summary>
        [Description("标准搜索")]
        StandardSearch = 0,

        /// <summary>
        /// 多重搜索
        /// </summary>
        [Description("多重搜索")]
        MultipleSearch = 1,
    }

    /// <summary>
    /// 焊点点胶应用枚举
    /// </summary>
    public enum EpoxyApplicationEnum
    {
        /// <summary>
        /// 不开启
        /// </summary>
        [Description("不开启")]
        NoEpoxy = 0,

        /// <summary>
        /// 画胶
        /// </summary>
        [Description("画胶")]
        Dispensing = 1,

        /// <summary>
        /// 喷胶
        /// </summary>
        [Description("喷胶")]
        Jetting = 2,
    }

    /// <summary>
    /// 编辑状态枚举
    /// </summary>
    public enum EditStateEnum
    {
        /// <summary>
        /// 新建
        /// </summary>
        NewCreate = 0,

        /// <summary>
        /// 未完成
        /// </summary>
        NotYetFinished = 1,

        /// <summary>
        /// 外部修改
        /// </summary>
        ChangedOutside = 2,

        /// <summary>
        /// 完成
        /// </summary>
        Completely = 3
    }

    /// <summary>
    /// 传输的类型
    /// </summary>
    public enum SubstrateProcessingEnum
    {
        /// <summary>
        /// 完整的
        /// </summary>
        [Description("Complete")]
        Complete,

        /// <summary>
        /// 分离的
        /// </summary>
        [Description("Divide")]
        Divide,

        /// <summary>
        /// 分离的(上下分离的)
        /// </summary>
        [Description("Divide(Inverted)")]
        DivideWithInverted
    }

    /// <summary>
    /// 基板识别类型
    /// </summary>
    public enum IdentificationEnum
    {
        /// <summary>
        /// 关
        /// </summary>
        [Description("关")]
        Off,

        /// <summary>
        /// 传送，这个应该是上游传送过来的
        /// </summary>
        [Description("传送")]
        OnWithTs,

        /// <summary>
        /// 手动扫描或者输入
        /// </summary>
        [Description("手动")]
        Manual,

        /// <summary>
        /// 视觉搜索
        /// </summary>
        [Description("视觉搜索")]
        OnForSearch
    }

    /// <summary>
    /// mapping的识别类型
    /// </summary>
    public enum TuMappingEnum
    {
        /// <summary>
        /// 基板
        /// </summary>
        [Description("基板")]
        Substrate,

        /// <summary>
        /// 基岛
        /// </summary>
        [Description("基岛")]
        Module,
    }

    /// <summary>
    /// 新增安全高度的方式
    /// </summary>
    public enum AdditionalSafetyHeightType
    {
        /// <summary>
        /// 关闭
        /// </summary>
        [Description("关闭")]
        Off,

        /// <summary>
        /// 载具
        /// </summary>
        [Description("载具")]
        TransportUnit,

        /// <summary>
        /// 基板
        /// </summary>
        [Description("基板")]
        Substrate
    }

    /// <summary>
    /// 基板形状
    /// </summary>
    public enum MultiplicationEnum
    {
        /// <summary>
        /// 标准矩形
        /// </summary>
        [Description("标准矩形")]
        Matrix,

        /// <summary>
        /// 自由配置的
        /// </summary>
        [Description("自由配置")]
        Configurable,
    }

    /// <summary>
    /// 实体状态
    /// </summary>
    public enum EntityState
    {
        /// <summary>
        /// 正在制作
        /// </summary>
        Process,

        /// <summary>
        /// 成功
        /// 这个代表所有的制程全部完成
        /// </summary>
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        Fail
    }

    /// <summary>
    /// 实体状态
    /// </summary>
    public enum MatterProductState
    {
        /// <summary>
        /// 正常生产的产品
        /// </summary>
        Enable,

        /// <summary>
        /// 只在系统1生效的产品
        /// 目前是只点胶
        /// </summary>
        EnableInSystem1,

        /// <summary>
        /// 只在系统1生效的产品
        /// 目前是值贴片
        /// </summary>
        EnableInSystem2,

        /// <summary>
        /// 不启用的
        /// 在生产过成中是不会进行任何工艺
        /// </summary>
        Disable,
    }

    /// <summary>
    /// 实体对象的类型
    /// </summary>
    public enum EntityTypeEnum
    {
        /// <summary>
        /// 运输单元
        /// </summary>
        [Description("传输单元")]
        TransportUnit,

        /// <summary>
        /// 基板
        /// </summary>
        [Description("基板")]
        Substrate,

        /// <summary>
        /// 基岛
        /// </summary>
        [Description("基岛")]
        Module,

        /// <summary>
        /// BondPosition
        /// </summary>
        [Description("焊点")]
        BondPosition
    }

    /// <summary>
    /// 焊点状态枚举
    /// </summary>
    public enum BondPositionStateEnum
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Description("Normal")]
        Normal,

        /// <summary>
        /// 正在做
        /// </summary>
        [Description("Working")]
        Working,

        /// <summary>
        /// 成功
        /// </summary>
        [Description("Success")]
        Success,

        /// <summary>
        /// 失败
        /// </summary>
        [Description("Failed")]
        Failed,
    }



    /// <summary>
    /// 焊点制程枚举
    /// </summary>
    public enum System1ProcessEnum
    {
        /// <summary>
        /// 空（制程起点，被屏蔽的情况下制程为空）
        /// </summary>
        [Description("Start")]
        Start,

        /// <summary>
        /// 扫码 ,这个应该属于载具的制程
        /// </summary>
        [Description("Scan")]
        Scan,

        /// <summary>
        /// 点胶测高
        /// </summary>
        [Description("MeasureHeightInS1")]
        MeasureHeightInS1,

        /// <summary>
        /// 系统1定位
        /// </summary>
        [Description("Vision  in  system1")]
        VisionInS1,

        /// <summary>
        /// 点胶
        /// </summary>
        [Description("Dispense")]
        Dispense,

        /// <summary>
        /// 胶型检测
        /// </summary>
        [Description("PostBondInspectionInS1")]
        PostBondInspectionInS1,

        /// <summary>
        /// 系统1完成
        /// 为防止点胶后面还有其他的步骤，
        /// 在这里添加一个步骤
        /// </summary>
        [Description("FinishInSystem1")]
        FinishInSystem1,
    }

    /// <summary>
    /// 排列方式
    /// </summary>
    public enum ArrangementEnum
    {
        /// <summary>
        /// 正常的
        /// </summary>
        [Description("正常")]
        Normal,

        /// <summary>
        /// 蛇形的
        /// </summary>
        [Description("蛇形")]
        Snake
    }

    /// <summary>
    /// 工作顺序
    /// </summary>
    public enum WorkOrderEnum
    {
        /// <summary>
        /// 左下向右
        /// </summary>
        [Description("左下向右")]
        LeftDownToRight,

        /// <summary>
        /// 左下向上
        /// </summary>
        [Description("左下向上")]
        LeftDownToUp,

        /// <summary>
        /// 右下向左
        /// </summary>
        [Description("右下向左")]
        RightDownToLeft,

        /// <summary>
        /// 右下向上
        /// </summary>
        [Description("右下向上")]
        RightDownToUp,

        /// <summary>
        /// 左上向右
        /// </summary>
        [Description("左上向右")]
        LeftUpToRight,

        /// <summary>
        /// 左上向下
        /// </summary>
        [Description("左上向下")]
        LeftUpToDown,

        /// <summary>
        /// 右上向左
        /// </summary>
        [Description("右上向左")]
        RightUpToLeft,

        /// <summary>
        /// 右上向下
        /// </summary>
        [Description("右上向下")]
        RightUpToDown
    }
}
