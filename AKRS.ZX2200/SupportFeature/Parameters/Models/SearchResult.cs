using System.Collections.Generic;
using AKRS.ZX2200.SupportFeature.Parameters.Model;

namespace AKRS.ZX2200.SupportFeature.Parameters.Models;

/// <summary>
/// 搜索结果模型
/// </summary>
public class SearchResult
{
    /// <summary>
    /// 搜索结果索引
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// 组节点类型
    /// </summary>
    public TreeGroupChildNodesEnum GroupNodeType { get; set; }

    /// <summary>
    /// 参数组节点下的参数详细模型列表
    /// </summary>
    public List<ParameterDetailModel> ParamDetailNodes { get; set; }

    /// <summary>
    /// 当前选中的节点
    /// </summary>
    public ParameterDetailModel CurrentSelectedNode { get; set; }

    /// <summary>
    /// 所属集合类型参数的元素
    /// </summary>
    public object Element { get; set; }

    /// <summary>
    /// 展开
    /// </summary>
    /// <param name="originResult"></param>
    /// <returns></returns>
    public static List<SearchResult> Flatten(List<SearchResult> originResult)
    {
        List<SearchResult> result = new();
        int index = 1;
        foreach (SearchResult sr in originResult)
        {
            if (sr.ParamDetailNodes != null)
            {
                foreach (ParameterDetailModel node in sr.ParamDetailNodes)
                {
                    result.Add(new SearchResult
                    {
                        Index = index++,
                        GroupNodeType = sr.GroupNodeType,
                        ParamDetailNodes = sr.ParamDetailNodes,
                        CurrentSelectedNode = node,
                        Element = sr.Element
                    });
                }
            }
        }

        return result;
    }
}