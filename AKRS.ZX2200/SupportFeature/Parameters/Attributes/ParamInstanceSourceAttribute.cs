using System;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 标记参数实例源特性
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ParamInstanceSourceAttribute : Attribute
{
    /// <summary>
    /// 实例提供类型
    /// </summary>
    public Type InstanceProviderType { get; }

    /// <summary>
    /// 提供实例的静态方法名称
    /// </summary>
    public string ProviderMethodName { get; }

    // /// <summary>
    // /// 提供实例的方法参数
    // /// </summary>
    // public object[] ProviderMethodArgs { get; }

    /// <summary>
    /// 初始化类特性，指定实例来源方法
    /// </summary>
    /// <param name="instanceProviderType">提供实例的类类型（静态类或单例类）</param>
    /// <param name="providerMethodName">获取实例的方法名称</param>
    public ParamInstanceSourceAttribute(Type instanceProviderType, string providerMethodName)
    {
        this.InstanceProviderType = instanceProviderType;
        this.ProviderMethodName = providerMethodName;
    }
}