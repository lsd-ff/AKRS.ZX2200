using System;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;

namespace AKRS.ZX2200.SupportFeature.Parameters.Models;

using DevExpress.Utils.Extensions;

/// <summary>
/// 参数显示模型
/// </summary>
public class ParameterDetailModel
{
    /// <summary>
    /// 节点ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// 父节点ID
    /// </summary>
    public int? ParentID { get; set; }

    /// <summary>
    /// 节点名称
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// 属性名称
    /// </summary>
    public string PropertyName { get; set; }

    /// <summary>
    /// 节点值
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// 节点值类型
    /// </summary>
    public Type ValueType => this.Value?.GetType();

    /// <summary>
    /// 节点值显示
    /// </summary>
    public string DisplayValue
    {
        get
        {
            if (this.ValueType == null)
            {
                return string.Empty;
            }

            if (this.ValueType.IsEnum)
            {
                return ((Enum)this.Value).GetDescription();
            }
            else if(this.ValueType.IsArray || this.ValueType.GetInterfaces()
                        .Any(i =>
                            i.IsGenericType &&
                            i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return string.Empty;
            }

            return this.Value?.ToString();
        }
    }

    /// <summary>
    /// 节点特性
    /// </summary>
    public TreeProgramListArgsAttribute ArgsAttribute { get; set; }

    /// <summary>
    /// 节点属性信息
    /// </summary>
    public PropertyInfo PropertyInfo { get; set; }

    /// <summary>
    /// 父节点实例
    /// </summary>
    public object ParentInstance { get; set; }

    /// <summary>
    /// 集合元素子节点
    /// </summary>
    public List<ParameterDetailModel> Children { get; set; } = new List<ParameterDetailModel>();

    /// <summary>
    /// 是否为集合类型
    /// </summary>
    public bool IsCollection { get; set; } = false;

    /// <summary>
    /// 集合索引
    /// </summary>
    public int CollectionIndex { get; set; } = -1;

    /// <summary>
    /// 是否有子节点
    /// </summary>
    public bool HasChildren => this.Children.Count > 0;

    /// <summary>
    /// 节点类型
    /// </summary>
    public TreeGroupChildNodesEnum NodeEnum { get; set; }

    /// <summary>
    /// 绑定类型名称
    /// </summary>
    public List<ParamAxesConfig> BindingConfigs { get; set; }

    /// <summary>
    /// 最大值
    /// </summary>
    public double MaxValue { get; set; }

    /// <summary>
    /// 最小值
    /// </summary>
    public double MinValue { get; set; }

    /// <summary>
    /// 单位
    /// </summary>
    public string Unit { get; set; }

    /// <summary>
    /// 节点是否可见
    /// </summary>
    public bool IsNodeVisible
    {
        get
        {
            if (string.IsNullOrEmpty(this.VisibleCheckNodeName) || this.VisibleCheckNode == null)
            {
                return true;
            }

            if (this.VisibleCheckNode.Value == null)
            {
                return true;
            }

            return (bool)this.VisibleCheckNode.Value;
        }
    }

    /// <summary>
    /// 可见性关联节点
    /// </summary>
    public string VisibleCheckNodeName { get; set; }

    /// <summary>
    /// 节点可见性状态检查节点
    /// </summary>
    public ParameterDetailModel VisibleCheckNode { get; set; }

    /// <summary>
    /// 被当前节点管理可见性的节点
    /// </summary>
    public ParameterDetailModel VisibleManageNode { get; set; }

    /// <summary>
    /// 展开子节点
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static List<ParameterDetailModel> Flatten(List<ParameterDetailModel> nodes)
    {
        List<ParameterDetailModel> flatList = new();
        int currentId = 1;
        FlattenNodes(nodes, null, flatList, ref currentId);
        return flatList;
    }

    /// <summary>
    /// 递归展开节点
    /// </summary>
    /// <param name="nodes"></param>
    /// <param name="parentId"></param>
    /// <param name="flatList"></param>
    /// <param name="currentId"></param>
    private static void FlattenNodes(List<ParameterDetailModel> nodes, int? parentId,
        List<ParameterDetailModel> flatList, ref int currentId)
    {
        foreach (ParameterDetailModel node in nodes)
        {
            node.ID = currentId++;
            node.ParentID = parentId;

            flatList.Add(node);

            if (node.Children != null && node.Children.Count > 0)
            {
                FlattenNodes(node.Children, node.ID, flatList, ref currentId);
            }
        }
    }
}