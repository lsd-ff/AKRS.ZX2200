using System;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 参数节点可见性配置特性，构造函数中输入关联的bool类型属性名称
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class ParamVisibleConfigAttribute : Attribute
{
    /// <summary>
    /// 关联的bool类型属性名称
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="propertyName"></param>
    public ParamVisibleConfigAttribute(string propertyName)
    {
        this.PropertyName = propertyName;
    }
}