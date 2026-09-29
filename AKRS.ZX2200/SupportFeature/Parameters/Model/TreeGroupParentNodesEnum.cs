using System.ComponentModel;

namespace AKRS.ZX2200.SupportFeature.Parameters.Model;

/// <summary>
/// TreeGroup的父节点枚举
/// </summary>
public enum TreeGroupParentNodesEnum
{
    /// <summary>
    /// 产品
    /// </summary>
    [Description("产品")] Product,

    /// <summary>
    /// 产品框架参数
    /// </summary>
    [Description("产品框架参数")] Transportunit,

    /// <summary>
    /// 芯片
    /// </summary>
    [Description("芯片")] Component,

    /// <summary>
    /// Epoxy
    /// </summary>
    [Description("点胶")] Epoxyapplication,

    /// <summary>
    /// 工具
    /// </summary>
    [Description("工具")] Tool,

    /// <summary>
    /// 辨识
    /// </summary>
    [Description("辨识")] Recognition,

    /// <summary>
    /// 机器
    /// </summary>
    [Description("机器")] Machine,

    /// <summary>
    /// 传输系统
    /// </summary>
    [Description("传输系统")] Transportsystem,

    /// <summary>
    /// 其它
    /// </summary>
    [Description("其它")] Other
}