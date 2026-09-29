using System.ComponentModel;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;

namespace AKRS.ZX2200.SupportFeature.Parameters.Model;

/// <summary>
///  TreeGroup的子节点枚举
/// </summary>
public enum TreeGroupChildNodesEnum
{
    #region 产品

    /// <summary>
    /// 常规
    /// </summary>
    [Description("常规")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    General,

    /// <summary>
    /// 系统相关
    /// </summary>
    [Description("系统相关")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    Systemrelated,

    /// <summary>
    /// 配置
    /// </summary>
    [Description("配置")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    Configuration,

    /// <summary>
    /// 流程集合
    /// </summary>
    [Description("流程集合")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    Processinglist,

    /// <summary>
    /// 换晶圆
    /// </summary>
    [Description("换晶圆")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    Waferchange,

    /// <summary>
    /// 晶圆盒几何形状
    /// </summary>
    [Description("晶圆盒几何形状")]
    [ParentNode(TreeGroupParentNodesEnum.Product)]
    Wafermagazinegeometry,

    #endregion

    #region 产品框架参数

    /// <summary>
    /// 框架参数
    /// </summary>
    [Description("框架参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    TransportUnitConfig,

    /// <summary>
    /// 基板参数
    /// </summary>
    [Description("基板参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    SubstrateConfig,

    /// <summary>
    /// 基岛参数
    /// </summary>
    [Description("基岛参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    ModuleConfig,

    /// <summary>
    /// 基岛不良数据表
    /// </summary>
    [Description("基岛不良数据表")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    Staticbadmoduletable,

    /// <summary>
    /// 焊点参数
    /// </summary>
    [Description("焊点参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    BondPositionConfig,

    /// <summary>
    /// 焊后/胶后参数
    /// </summary>
    [Description("焊后/胶后参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    Postbondinspection,

    /// <summary>
    /// 测高参数
    /// </summary>
    [Description("测高参数")]
    [ParentNode(TreeGroupParentNodesEnum.Transportunit)]
    Heightmeasurement,

    #endregion

    #region 芯片

    /// <summary>
    /// 芯片
    /// </summary>
    [Description("芯片")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Component,

    /// <summary>
    /// 料盒配置
    /// </summary>
    [Description("料盒配置")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Wafermagazineallocations,

    /// <summary>
    /// 适配器配置
    /// </summary>
    [Description("适配器配置")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Wafflegelpackadapter,

    /// <summary>
    /// 多晶圆
    /// </summary>
    [Description("多晶圆")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Multiwafer,

    /// <summary>
    /// 飞达
    /// </summary>
    [Description("飞达")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Feederbank,

    /// <summary>
    /// 坏芯片表
    /// </summary>
    [Description("坏芯片表")]
    [ParentNode(TreeGroupParentNodesEnum.Component)]
    Badchiptable,

    #endregion

    #region 点胶

    /// <summary>
    /// 胶型
    /// </summary>
    [Description("胶型")]
    [ParentNode(TreeGroupParentNodesEnum.Epoxyapplication)]
    Epoxyapplication,

    /// <summary>
    /// 胶水
    /// </summary>
    [Description("胶水")]
    [ParentNode(TreeGroupParentNodesEnum.Epoxyapplication)]
    EpoxyFlux,

    /// <summary>
    /// 模式
    /// </summary>
    [Description("模式")]
    [ParentNode(TreeGroupParentNodesEnum.Epoxyapplication)]
    Dispensepattern,

    #endregion

    #region 工具

    /// <summary>
    /// 固晶、点胶工具
    /// </summary>
    [Description("固晶、点胶工具")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    PPandepoxytools,

    /// <summary>
    /// 点胶器
    /// </summary>
    [Description("点胶器")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    Dispenser,

    /// <summary>
    /// 顶针系统
    /// </summary>
    [Description("顶针系统")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    Ejectionsystem,

    /// <summary>
    /// 吸嘴架配置
    /// </summary>
    [Description("吸嘴架配置")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    PPtoolbankallocation,

    /// <summary>
    /// 顶针架配置
    /// </summary>
    [Description("顶针架配置")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    EStoolbankallocation,

    /// <summary>
    /// 翻转工具
    /// </summary>
    [Description("翻转工具")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    Fliptool,

    /// <summary>
    /// 刀架
    /// </summary>
    [Description("刀架")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    Toolholder,

    /// <summary>
    /// 吸嘴清洁
    /// </summary>
    [Description("吸嘴清洁")]
    [ParentNode(TreeGroupParentNodesEnum.Tool)]
    NozzleClean,

    #endregion

    #region 辨识

    /// <summary>
    /// 辨识数据
    /// </summary>
    [Description("辨识数据")]
    [ParentNode(TreeGroupParentNodesEnum.Recognition)]
    Adjustdata,

    /// <summary>
    /// 多重搜索
    /// </summary>
    [Description("多重搜索")]
    [ParentNode(TreeGroupParentNodesEnum.Recognition)]
    Multiplesearch,

    /// <summary>
    /// 搜索
    /// </summary>
    [Description("搜索")]
    [ParentNode(TreeGroupParentNodesEnum.Recognition)]
    Search,

    #endregion


    #region 机器

    /// <summary>
    /// 机器硬件配置
    /// </summary>
    [Description("机器硬件配置")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    MachineHardwarecConfiguration,

    /// <summary>
    /// 机器软件配置
    /// </summary>
    [Description("机器软件配置")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    MachineSoftwarecConfiguration,

    /// <summary>
    /// 机器数据
    /// </summary>
    [Description("机器数据")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    MachineData,

    /// <summary>
    /// 模组配置
    /// </summary>
    [Description("模组配置")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Moduleconnfiguration,

    /// <summary>
    /// 模组数据
    /// </summary>
    [Description("模组数据")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Moduledata,

    /// <summary>
    /// 吸嘴架
    /// </summary>
    [Description("吸嘴架")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Toolbank,

    /// <summary>
    /// 统计数据
    /// </summary>
    [Description("统计数据")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Statisticdata,

    /// <summary>
    /// 错误处理
    /// </summary>
    [Description("错误处理")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Errorhandling,

    /// <summary>
    /// 安全区域
    /// </summary>
    [Description("安全区域")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Securitygeometries,

    /// <summary>
    /// 安全区域设置
    /// </summary>
    [Description("安全区域设置")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Movingsecuritygeometries,

    /// <summary>
    /// 电机数据
    /// </summary>
    [Description("电机数据")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Motordata,

    /// <summary>
    /// 相机
    /// </summary>
    [Description("相机")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    Camera,

    /// <summary>
    /// BMC平台
    /// </summary>
    [Description("BMC平台")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    BMC,

    /// <summary>
    /// 中转台
    /// </summary>
    [Description("中转台")]
    [ParentNode(TreeGroupParentNodesEnum.Machine)]
    IPT,

    #endregion

    #region 传输系统

    /// <summary>
    /// 传输基板配置
    /// </summary>
    [Description("传输基板配置")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    TSsubstrateconfiguration,

    /// <summary>
    /// 基板数据
    /// </summary>
    [Description("基板数据")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    IOSubstratedata,

    /// <summary>
    /// 流道数据
    /// </summary>
    [Description("流道数据")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    TUData,

    /// <summary>
    /// 挡料气缸相关设置
    /// </summary>
    [Description("挡料气缸相关设置")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    Bondinsert,

    /// <summary>
    /// 硬件配置
    /// </summary>
    [Description("硬件配置")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    Hardwareconfiguration,

    /// <summary>
    /// part高度map图
    /// </summary>
    [Description("P-part高度map图")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    Ppartheightmap,

    /// <summary>
    /// 上料仓
    /// </summary>
    [Description("上料仓")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    Loader,

    /// <summary>
    /// 下料仓
    /// </summary>
    [Description("下料仓")]
    [ParentNode(TreeGroupParentNodesEnum.Transportsystem)]
    Unloader,

    #endregion

    #region 其它

    /// <summary>
    /// Bin代码
    /// </summary>
    [Description("Bin代码")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Bincode,

    /// <summary>
    /// Eumel
    /// </summary>
    [Description("Eumel")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Eumel,

    /// <summary>
    /// 条形码扫描仪
    /// </summary>
    [Description("条形码扫描仪")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Barcodescanner,

    /// <summary>
    /// 芯片追踪
    /// </summary>
    [Description("芯片追踪")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Singlecomponenttracking,

    /// <summary>
    /// SECS参数
    /// </summary>
    [Description("SECS参数")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    SECSparameters,

    /// <summary>
    /// 文件存档
    /// </summary>
    [Description("文件存档")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Filearchiving,

    /// <summary>
    /// 固晶精度优化
    /// </summary>
    [Description("固晶精度优化")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Bondingoptimization,

    /// <summary>
    /// 主机
    /// </summary>
    [Description("主机")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Host,

    /// <summary>
    /// 数据日志
    /// </summary>
    [Description("数据日志")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    Datalogger,

    /// <summary>
    /// Modbus通讯
    /// </summary>
    [Description("Modbus通讯")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    ModbusConnect,

    /// <summary>
    /// 力控标定
    /// </summary>
    [Description("力控标定")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    ForceCalibrate,

    /// <summary>
    /// 大力力控
    /// </summary>
    [Description("大力力控")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    ForceControl,

    /// <summary>
    /// 小力力控
    /// </summary>
    [Description("小力力控")]
    [ParentNode(TreeGroupParentNodesEnum.Other)]
    SmallForceControl

    #endregion
}