using System;
using AKRS.ZX2200.SupportFeature.Parameters.Model;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 父节点分类
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public sealed class ParentNodeAttribute : Attribute
{
    /// <summary>
    /// 父节点分类
    /// </summary>
    public TreeGroupParentNodesEnum ParentNodeEnum { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="parentNodeEnum"></param>
    public ParentNodeAttribute(TreeGroupParentNodesEnum parentNodeEnum)
    {
        this.ParentNodeEnum = parentNodeEnum;
    }
}