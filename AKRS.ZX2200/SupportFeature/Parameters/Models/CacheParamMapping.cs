using System;
using System.Collections.Generic;
using System.Reflection;
using AKRS.ZX2200.SupportFeature.Parameters.Model;

namespace AKRS.ZX2200.SupportFeature.Parameters.Models;

/// <summary>
/// 缓存参数映射类
/// </summary>
public class CacheParamMapping
{
    /// <summary>
    /// 参数所属实例
    /// </summary>
    public object Instance { get; set; }

    /// <summary>
    /// 参数类型
    /// </summary>
    public Type ParamType { get; set; }

    /// <summary>
    /// 参数类型
    /// </summary>
    public TreeGroupChildNodesEnum ParamGroupEnum { get; set; }

    /// <summary>
    /// 参数所属属性
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }

    /// <summary>
    /// 是否为集合类型
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// 参数显示的名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 参数名称列表
    /// </summary>
    public List<string> ParamNameList { get; set; }
}