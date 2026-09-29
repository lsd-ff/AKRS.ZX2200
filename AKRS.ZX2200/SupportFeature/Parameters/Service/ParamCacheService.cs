using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKRS.ZX2200.SupportFeature.Parameters.Editor;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.SupportFeature.Parameters.Models;
using DevExpress.Office.Utils;

namespace AKRS.ZX2200.SupportFeature.Parameters.Service;

/// <summary>
/// 参数缓存服务
/// </summary>
public class ParamCacheService
{
    #region Private Member

    private static readonly Lazy<ParamCacheService> LazyParamCache = new(() =>
    {
        ParamCacheService instance = new()
        {
            GroupNodes = ParamGroupModel.Flatten(ParamService.LoadParamGroupTreeNodes())
           
        };
        //
        // var nodes = ParamService.LoadParamMappingsNew();
        return instance;
    });


    private ParamCacheService()
    {
    }

    #endregion

    /// <summary>
    /// 单例实例
    /// </summary>
    public static ParamCacheService Instance => LazyParamCache.Value;

    /// <summary>
    /// 编辑器缓存
    /// </summary>
    public Dictionary<Type, IParamEditor> EditorCache { get; private set; }

    /// <summary>
    /// 获取参数组节点列表
    /// </summary>
    public List<ParamGroupModel> GroupNodes { get; internal set; }

    /// <summary>
    /// 根据参数名称模糊搜索参数
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public List<(TreeGroupChildNodesEnum, List<ParameterDetailModel>)> SearchParamByMatchName(string name)
    {
        return default;
    }
}