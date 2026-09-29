using System;
using System.Reflection;
using System.Windows.Forms;
using AKRS.ZX2200.SupportFeature.Parameters.Models;

namespace AKRS.ZX2200.SupportFeature.Parameters.Editor;

/// <summary>
/// 参数编辑器接口
/// </summary>
public interface IParamEditor
{
    /// <summary>
    /// 容器控件
    /// </summary>
    Control EditorContainer { get; set; }

    /// <summary>
    /// 参数名称
    /// </summary>
    string ParamName { get; set; }

    /// <summary>
    /// 是否是值类型或字符串类型集合
    /// </summary>
    bool IsValueTypeCollection { get; set; }

    /// <summary>
    /// 绑定的集合元素索引
    /// </summary>
    int ElementIndex { get; set; }

    /// <summary>
    /// 参数属性信息
    /// </summary>
    PropertyInfo[] PropertyInfo { get; set; }

    /// <summary>
    /// 对象实例
    /// </summary>
    object Instance { get; set; }

    /// <summary>
    /// 显示编辑器到指定容器
    /// </summary>
    void ShowEditor();

    /// <summary>
    /// 隐藏编辑器
    /// </summary>
    void HideEditor();

    /// <summary>
    /// 值更改事件
    /// </summary>
    event Action<object> ValueChanged;

    /// <summary>
    /// 元素节点模型
    /// </summary>
    ParameterDetailModel ParamNodeModel { get; set; }
}