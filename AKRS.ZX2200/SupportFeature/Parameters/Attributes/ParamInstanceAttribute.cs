using AKRS.ZX2200.SupportFeature.Parameters.Model;
using System;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 标记参数实例的特性
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public sealed class ParamInstanceAttribute : Attribute
{
    /// <summary>
    /// 参数节点所属的TreeGroup枚举。
    /// </summary>
    public TreeGroupChildNodesEnum ParamGroupEnum { get; }

    /// <summary>
    /// 在TreeList上显示的名称。
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// 是否是集合类型
    /// </summary>
    public bool IsCollection { get; }

    /// <summary>
    /// 初始化特性的新实例。
    /// </summary>
    /// <param name="paramGroupEnum">节点所属的TreeGroup枚举。</param>
    /// <param name="displayName">显示的名称。</param>
    /// <param name="isCollection">是否是集合类型</param>
    public ParamInstanceAttribute(TreeGroupChildNodesEnum paramGroupEnum, string displayName, bool isCollection = false)
    {
        this.ParamGroupEnum = paramGroupEnum;
        this.DisplayName = displayName;
        this.IsCollection = isCollection;
    }
}
