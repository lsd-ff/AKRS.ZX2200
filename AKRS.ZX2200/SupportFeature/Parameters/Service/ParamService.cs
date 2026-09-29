using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
using AKRS.ZX2200.SupportFeature.Parameters.Editor;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.SupportFeature.Parameters.Models;
using DevExpress.DataAccess.Native.DataFederation.QueryBuilder;
using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.Model;
using SearchResult = AKRS.ZX2200.SupportFeature.Parameters.Models.SearchResult;

namespace AKRS.ZX2200.SupportFeature.Parameters.Service;

/// <summary>
/// 参数服务类
/// </summary>
public static class ParamService
{
    private static List<CacheParamMapping> cacheParamMappings;

    static ParamService()
    {
        cacheParamMappings = GetAllParamMappings();
    }

    /// <summary>
    /// 重新加载程序集
    /// </summary>
    public static void ReloadAssembly()
    {
        cacheParamMappings = GetAllParamMappings();
        ParamCacheService.Instance.GroupNodes = ParamGroupModel.Flatten(ParamService.LoadParamGroupTreeNodes());
    }

    private static List<CacheParamMapping> GetAllParamMappings()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        IEnumerable<Type> typesWithProgramArgsSrc = assembly.GetTypes()
            .Where(t => t.GetCustomAttribute<ParamInstanceSourceAttribute>() != null);

        List<CacheParamMapping> cacheMappings = new();
        foreach (Type type in typesWithProgramArgsSrc)
        {
            ParamInstanceSourceAttribute srcAttr = type.GetCustomAttribute<ParamInstanceSourceAttribute>();
            MethodInfo instanceProviderMethod = srcAttr.InstanceProviderType.GetMethod(srcAttr.ProviderMethodName,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            object instance = instanceProviderMethod?.Invoke(null, null);
            if (instance == null)
            {
                continue;
            }

            if (type.GetCustomAttribute<ParamInstanceAttribute>() is { } classAttr)
            {
                CacheParamMapping cacheMapping = new()
                {
                    DisplayName = classAttr.DisplayName,
                    PropertyInfo = null,
                    ParamGroupEnum = classAttr.ParamGroupEnum,
                    IsCollection = classAttr.IsCollection && instance is IEnumerable and not string,
                    Instance = instance,
                    ParamType = type
                };

                cacheMapping.ParamNameList = instance.GetType().GetProperties()
                    .Where(p => p.IsDefined(typeof(TreeProgramListArgsAttribute)))
                    .Select(p =>
                    {
                        if (p.GetCustomAttribute(typeof(TreeProgramListArgsAttribute)) is TreeProgramListArgsAttribute
                            attr)
                        {
                            return attr.ShowText;
                        }

                        return p.Name;
                    }).ToList();
                cacheMappings.Add(cacheMapping);
            }

            foreach (PropertyInfo prop in type.GetProperties()
                         .Where(p => p.IsDefined(typeof(ParamInstanceAttribute))))
            {
                ParamInstanceAttribute propAttr = prop.GetCustomAttribute<ParamInstanceAttribute>();
                object propValue = prop.GetValue(instance);
                CacheParamMapping cache = new()
                {
                    DisplayName = propAttr.DisplayName,
                    PropertyInfo = prop,
                    ParamGroupEnum = propAttr.ParamGroupEnum,
                    IsCollection = propAttr.IsCollection && propValue is IEnumerable and not string,
                    Instance = instance,
                    ParamType = prop.PropertyType,
                    ParamNameList = new List<string>()
                };
                if (cache.IsCollection)
                {
                    if (propValue is IEnumerable enumerableList)
                    {
                        foreach (object element in enumerableList)
                        {
                            List<PropertyInfo> elementPropertiesInfo = element.GetType().GetProperties()
                                .Where(p => p.IsDefined(typeof(TreeProgramListArgsAttribute))).ToList();
                            List<string> elementParamList = elementPropertiesInfo.Select(p =>
                            {
                                if (p.GetCustomAttribute(typeof(TreeProgramListArgsAttribute)) is
                                    TreeProgramListArgsAttribute
                                    attr)
                                {
                                    return attr.ShowText;
                                }

                                return p.Name;
                            }).ToList();
                            cache.ParamNameList.AddRange(elementParamList);
                        }
                    }

                    cacheMappings.Add(cache);
                    continue;
                }

                List<PropertyInfo> propertiesInfo = prop.PropertyType.GetProperties()
                    .Where(p => p.IsDefined(typeof(TreeProgramListArgsAttribute))).ToList();

                cache.ParamNameList = propertiesInfo.Select(p =>
                {
                    if (p.GetCustomAttribute(typeof(TreeProgramListArgsAttribute)) is TreeProgramListArgsAttribute
                        attr)
                    {
                        return attr.ShowText;
                    }

                    return p.Name;
                }).ToList();

                cacheMappings.Add(cache);
            }
        }

        return cacheMappings;
    }

    /// <summary>
    /// 加载参数组节点列表
    /// </summary>
    /// <returns></returns>
    public static List<ParamGroupModel> LoadParamGroupTreeNodes()
    {
        List<ParamGroupModel> rootNodes = new();

        // 加载父节点
        foreach (TreeGroupParentNodesEnum parentEnum in Enum.GetValues(typeof(TreeGroupParentNodesEnum)))
        {
            ParamGroupModel parentNode = new()
            {
                DisplayName = parentEnum.GetDescription(),
                NodeEnum = null,
                Tag = parentEnum,
                ParentID = null
            };

            // 加载子节点
            foreach (TreeGroupChildNodesEnum childEnum in Enum.GetValues(typeof(TreeGroupChildNodesEnum)))
            {
                ParentNodeAttribute parentNodeAttr = childEnum.GetType()
                    .GetMember(childEnum.ToString())[0]
                    .GetCustomAttribute<ParentNodeAttribute>();

                if (parentNodeAttr == null || parentNodeAttr.ParentNodeEnum != parentEnum)
                {
                    continue;
                }

                CacheParamMapping cacheInstance = cacheParamMappings.FirstOrDefault(c => c.ParamGroupEnum == childEnum);
                ParamGroupModel childNode = new()
                {
                    DisplayName = childEnum.GetDescription(),
                    NodeEnum = childEnum,
                    Tag = cacheInstance?.PropertyInfo == null
                        ? cacheInstance?.Instance
                        : cacheInstance.PropertyInfo.GetValue(cacheInstance.Instance),
                    IsCollection = cacheInstance?.IsCollection ?? false
                };

                parentNode.Children.Add(childNode);
            }

            rootNodes.Add(parentNode);
        }

        return rootNodes;
    }

    /// <summary>
    /// 获取指定节点的详细参数列表
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static List<ParameterDetailModel> GetDetailNodes(ParamGroupModel node)
    {
        foreach (CacheParamMapping cacheModel in cacheParamMappings)
        {
            object instance = cacheModel.PropertyInfo == null
                ? cacheModel.Instance
                : cacheModel.PropertyInfo.GetValue(cacheModel.Instance);
            if (cacheModel.ParamGroupEnum != node.NodeEnum)
            {
                continue;
            }

            if (cacheModel.IsCollection)
            {
                return new List<ParameterDetailModel>();
            }

            KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>> pair =
                GetTypeParams(cacheModel.ParamType, instance, cacheModel.ParamGroupEnum);
            return pair.Value;
        }

        return new List<ParameterDetailModel>();
    }

    /// <summary>
    /// 获取List类型节点某个元素的详细参数列表
    /// </summary>
    /// <param name="node"></param>
    /// <param name="instance"></param>
    /// <returns></returns>
    public static List<ParameterDetailModel> GetDetailNodes(ParamGroupModel node, object instance)
    {
        foreach (CacheParamMapping cacheModel in cacheParamMappings)
        {
            if (node.NodeEnum != cacheModel.ParamGroupEnum)
            {
                continue;
            }

            if (!cacheModel.IsCollection)
            {
                return new List<ParameterDetailModel>(); 
            }

            if (cacheModel.Instance == null || instance == null)
            {
                return new List<ParameterDetailModel>(); 
            }

            Type elementType = instance.GetType();
            KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>> result =
                GetTypeParams(elementType, instance, cacheModel.ParamGroupEnum);

            return result.Value;
        }

        return new List<ParameterDetailModel>();
    }

    /// <summary>
    /// 根据参数名称模糊搜索参数
    /// </summary>
    /// <param name="paramName"></param>
    /// <returns></returns>
    public static List<SearchResult> Search(string paramName)
    {
        List<CacheParamMapping> matchingMappings = cacheParamMappings
            .Where(c => c.ParamNameList.Any(name => name.Contains(paramName)))
            .ToList();

        List<SearchResult> results = new();
        var instancePtrCache = new HashSet<IntPtr>();
        foreach (CacheParamMapping mapping in matchingMappings)
        {
            object instance = mapping.PropertyInfo == null
                ? mapping.Instance
                : mapping.PropertyInfo.GetValue(mapping.Instance);

            GCHandle instanceHandle = GCHandle.Alloc(instance, GCHandleType.Normal);
            IntPtr instancePtr = GCHandle.ToIntPtr(instanceHandle);
            if (!instancePtrCache.Add(instancePtr))
            {
                instanceHandle.Free();
                continue;
            }

            if (mapping.IsCollection)
            {
                if (instance is IEnumerable enumerableList)
                {
                    foreach (object o in enumerableList)
                    {
                        SearchResult elementResult = new();
                        elementResult.Element = o;
                        elementResult.GroupNodeType = mapping.ParamGroupEnum;

                        KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>> originNodes =
                            GetTypeParams(o.GetType(), o, mapping.ParamGroupEnum);
                        elementResult.ParamDetailNodes =
                            originNodes.Value.Where(n => n.DisplayName.Contains(paramName)).ToList();
                        results.Add(elementResult);
                    }
                }

                continue;
            }

            SearchResult result = new()
            {
                Element = mapping.Instance,
                GroupNodeType = mapping.ParamGroupEnum
            };

            KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>> originPair =
                GetTypeParams(mapping.ParamType, instance, mapping.ParamGroupEnum);
            result.ParamDetailNodes =
                originPair.Value.Where(n => n.DisplayName.Contains(paramName)).ToList();
            results.Add(result);
        }

        return SearchResult.Flatten(results);
    }

    /// <summary>
    /// 获取类型的参数列表
    /// </summary>
    /// <param name="type"></param>
    /// <param name="instance"></param>
    /// <param name="paramGroupEnum"></param>
    /// <returns></returns>
    private static KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>> GetTypeParams(Type type,
        object instance, TreeGroupChildNodesEnum paramGroupEnum)
    {
        List<ParameterDetailModel> nodes = new();
        foreach (PropertyInfo childProp in type.GetProperties())
        {
            // 过滤掉没有（TreeProgramListArgsAttribute）的属性
            if (!childProp.IsDefined(typeof(TreeProgramListArgsAttribute), false))
            {
                continue;
            }

            // 取出特性实例
            TreeProgramListArgsAttribute childAttr =
                childProp.GetCustomAttribute<TreeProgramListArgsAttribute>();

            object childValue = instance == null ? null : childProp.GetValue(instance);

            // 取出绑定轴配置的特性实例
            List<ParamAxesConfig> bindingConfigs =
                childProp.GetCustomAttributes<PositionParamBindingAxesAttribute>()
                    .Select(posAttr => posAttr.BindingConfig).ToList();
            ParameterDetailModel pdm = new()
            {
                DisplayName = childAttr.ShowText ?? childProp.Name,
                PropertyName = childProp.Name,
                Value = childValue,
                ArgsAttribute = childAttr,
                PropertyInfo = childProp,
                ParentInstance = instance,
                NodeEnum = paramGroupEnum,
                MaxValue = childAttr.MaxValue,
                MinValue = childAttr.MinValue,
                Unit = childAttr.UnitName,
                BindingConfigs = bindingConfigs,
                IsCollection = childValue is IEnumerable and not string
            };
            if (childProp.IsDefined(typeof(ParamVisibleConfigAttribute), false))
            {
                ParamVisibleConfigAttribute attr =
                    childProp.GetCustomAttribute<ParamVisibleConfigAttribute>();
                pdm.VisibleCheckNodeName = attr.PropertyName;
            }

            // 如果是集合类型，则按照索引顺序创建子节点并添加到pdm的Children中
            if (pdm.IsCollection && pdm.Value is IEnumerable enumerable)
            {
                int idx = 0;
                foreach (object item in enumerable)
                {
                    pdm.Children.Add(new ParameterDetailModel
                    {
                        DisplayName = $"{childAttr.ShowText ?? childProp.Name}[{idx + 1}]",
                        PropertyName = childProp.Name,
                        Value = item,
                        ArgsAttribute = childAttr,
                        PropertyInfo = childProp,
                        ParentInstance = instance,
                        NodeEnum = paramGroupEnum,
                        MaxValue = pdm.MaxValue,
                        MinValue = pdm.MinValue,
                        Unit = pdm.Unit,
                        BindingConfigs = bindingConfigs,
                        CollectionIndex = idx
                    });
                    idx++;
                }
            }

            // 添加到列表
            nodes.Add(pdm);
        }

        nodes.Where(n => !string.IsNullOrEmpty(n.VisibleCheckNodeName)).Iter((node) =>
        {
            node.VisibleCheckNode =
                nodes.FirstOrDefault(n => n.PropertyName == node.VisibleCheckNodeName);
            if (node.VisibleCheckNode != null)
            {
                node.VisibleCheckNode.VisibleManageNode = node;
            }
        });

        return new KeyValuePair<TreeGroupChildNodesEnum, List<ParameterDetailModel>>(paramGroupEnum,
            ParameterDetailModel.Flatten(nodes));
    }

    /// <summary>
    /// 加载当前程序集的参数编辑器
    /// </summary>
    /// <returns></returns>
    public static Dictionary<Type, IParamEditor> LoadCurrentEditors()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Dictionary<Type, IParamEditor> editors = new();
        IEnumerable<Type> types = assembly.GetTypes()
            .Where(t => typeof(IParamEditor).IsAssignableFrom(t)
                        && !t.IsInterface
                        && !t.IsAbstract);

        foreach (Type type in types)
        {
            IEnumerable<ParamEditorAttribute> attrs = type.GetCustomAttributes<ParamEditorAttribute>(false);
            foreach (ParamEditorAttribute attr in attrs)
            {
                IParamEditor editor = (IParamEditor)Activator.CreateInstance(type);

                if (editors.ContainsKey(attr.EditorType))
                {
                    continue;
                }

                editors[attr.EditorType] = editor;
            }
        }

        return editors;
    }
}