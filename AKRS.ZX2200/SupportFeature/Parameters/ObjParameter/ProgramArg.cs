using System.Collections.Generic;

namespace AKRS.ZX2200.SupportFeature.Parameters.ObjParameter;

/// <summary>
/// 表示用于显示在TreeList中的参数节点。
/// </summary>
public class ProgramArg
{
    /// <summary>
    /// 显示的参数名称。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 显示的参数值。
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// 当前参数是否选中。
    /// </summary>
    public bool IsSelected { get; set; }

    /// <summary>
    /// 关联的数据对象，用于绑定回原始数据（例如ObjAndNodesInfo.CurrentObj）。
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// 子参数集合，支持树形结构。
    /// </summary>
    public List<ProgramArg> Children { get; set; } = new List<ProgramArg>();
}