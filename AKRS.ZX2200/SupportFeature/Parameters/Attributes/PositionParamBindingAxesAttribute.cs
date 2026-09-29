using System;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 位置参数绑定轴属性
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class PositionParamBindingAxesAttribute : Attribute
{
    /// <summary>
    /// 绑定配置
    /// </summary>
    public ParamAxesConfig BindingConfig { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="axisName"></param>
    /// <param name="propertyName"></param>
    public PositionParamBindingAxesAttribute(string axisName, string propertyName)
    {
        this.BindingConfig = new ParamAxesConfig(axisName, propertyName);
    }
}

/// <summary>
/// 位置参数轴配置
/// </summary>
public class ParamAxesConfig
{
    /// <summary>
    /// 轴名称
    /// </summary>
    public string AxisName { get; set; } = string.Empty;

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; set; } = string.Empty;

    public ParamAxesConfig()
    {
        
    }

    public ParamAxesConfig(string axisName, string propertyName)
    {
        this.PropertyName = propertyName;
        this.AxisName = axisName;
    }
}
