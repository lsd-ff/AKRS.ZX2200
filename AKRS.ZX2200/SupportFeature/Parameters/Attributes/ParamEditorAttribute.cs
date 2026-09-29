using System;

namespace AKRS.ZX2200.SupportFeature.Parameters.Attributes;

/// <summary>
/// 参数编辑器特性
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public sealed class ParamEditorAttribute : Attribute
{
    /// <summary>
    /// 编辑参数的类型
    /// </summary>
    public Type EditorType { get; }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="type"></param>
    public ParamEditorAttribute(Type type)
    {
        this.EditorType = type;
    }
}