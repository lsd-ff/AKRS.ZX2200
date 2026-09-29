using AKRS.ZX2200.SupportFeature.Parameters.Model;
using System.Collections.Generic;

namespace AKRS.ZX2200.SupportFeature.Parameters.Models;

/// <summary>
/// 树节点模型
/// </summary>
public class ParamGroupModel
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
    /// 节点值
    /// </summary>
    public object Tag { get; set; }

    /// <summary>
    /// 是否是集合类型
    /// </summary>
    public bool IsCollection { get; set; }

    /// <summary>
    /// 节点类型
    /// </summary>
    public TreeGroupChildNodesEnum? NodeEnum { get; set; }

    /// <summary>
    /// 子节点集合
    /// </summary>
    public List<ParamGroupModel> Children { get; set; } = new List<ParamGroupModel>();

    /// <summary>
    /// 展开子节点
    /// </summary>
    /// <param name="nodes"></param>
    /// <returns></returns>
    public static List<ParamGroupModel> Flatten(List<ParamGroupModel> nodes)
    {
        List<ParamGroupModel> flatList = new();
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
    private static void FlattenNodes(List<ParamGroupModel> nodes, int? parentId, List<ParamGroupModel> flatList, ref int currentId)
    {
        foreach (ParamGroupModel node in nodes)
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