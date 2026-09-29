using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.BaseModels;
using AKRS.ZX2200.SupportFeature.Parameters.Editor;
using AKRS.ZX2200.SupportFeature.Parameters.Model;
using AKRS.ZX2200.SupportFeature.Parameters.Models;
using AKRS.ZX2200.SupportFeature.Parameters.Service;
using DevExpress.DataAccess.Native.Json;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;

namespace AKRS.ZX2200.SupportFeature.Parameters;

/// <summary>
/// 参数编辑窗体
/// </summary>
public partial class FrmParamManager : XtraForm
{
    private Dictionary<Type, IParamEditor> editors;
    private IParamEditor currentEditor;
    private TreeListNode groupSelectedNode;
    private ParameterDetailModel paramSelectModel;
    private TreeListNode paramSelectedNode;

    private Color focusedColor = Color.FromArgb(107, 194, 253);
    private Color focusedFontColor = Color.Black;

    /// <summary>
    /// 构造函数
    /// </summary>
    public FrmParamManager()
    {
        this.InitializeComponent();
        this.editors = ParamService.LoadCurrentEditors();
    }

    /// <summary>
    /// Load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void FrmParamManager_Load(object sender, EventArgs e)
    {
        this.barButtonItem1.Visibility = BarItemVisibility.Never;
        await Task.Run(ParamService.ReloadAssembly).ConfigureAwait(false);
        this.Invoke(new Action(() =>
        {
            this.barButtonItem1.Visibility = BarItemVisibility.Always;
            this.InitParamGroupList();
            this.InitParamTreeList();
        }));
   
    }

    /// <summary>
    /// 初始化参数组树列表
    /// </summary>
    private void InitParamGroupList()
    {
        this.TLParamGroupView.OptionsBehavior.AutoPopulateColumns = false;
        this.TLParamGroupView.BeginUpdate();
        this.TLParamGroupView.KeyFieldName = nameof(ParamGroupModel.ID);
        this.TLParamGroupView.ParentFieldName = nameof(ParamGroupModel.ParentID);
        this.TLParamGroupView.Columns.Clear();

        this.TLParamGroupView.OptionsMenu.EnableColumnMenu = false;
        this.TLParamGroupView.OptionsMenu.EnableFooterMenu = false;
        this.TLParamGroupView.OptionsMenu.EnableNodeMenu = false;
        this.TLParamGroupView.OptionsBehavior.ReadOnly = true;
        this.TLParamGroupView.OptionsBehavior.Editable = false;
        this.TLParamGroupView.OptionsCustomization.AllowFilter = false;
        this.TLParamGroupView.OptionsCustomization.AllowSort = false;

        this.TLParamGroupView.OptionsView.ShowVertLines = false;
        this.TLParamGroupView.OptionsView.ShowHorzLines = false;
        this.TLParamGroupView.OptionsView.ShowIndicator = false;

        this.TLParamGroupView.OptionsSelection.EnableAppearanceFocusedRow = true;
        this.TLParamGroupView.OptionsSelection.EnableAppearanceFocusedCell = false;
        this.TLParamGroupView.OptionsView.FocusRectStyle = DrawFocusRectStyle.None;
        this.TLParamGroupView.Appearance.FocusedRow.BackColor = this.focusedColor;
        this.TLParamGroupView.Appearance.FocusedRow.ForeColor = this.focusedFontColor;

        TreeListColumn displayColumn = this.TLParamGroupView.Columns.AddField(nameof(ParamGroupModel.DisplayName));
        displayColumn.Caption = @"参数项";
        displayColumn.Visible = true;
        displayColumn.VisibleIndex = 0;

        TreeListColumn enumColumn = this.TLParamGroupView.Columns.AddField(nameof(ParamGroupModel.NodeEnum));
        enumColumn.Visible = false;

        TreeListColumn tagColumn = this.TLParamGroupView.Columns.AddField(nameof(ParamGroupModel.Tag));
        tagColumn.Visible = false;

        TreeListColumn isCollectionColumn =
            this.TLParamGroupView.Columns.AddField(nameof(ParamGroupModel.IsCollection));
        isCollectionColumn.Visible = false;

        this.TLParamGroupView.DataSource = ParamCacheService.Instance.GroupNodes;

        this.TLParamGroupView.EndUpdate();
    }

    /// <summary>
    /// 初始化参数树列表
    /// </summary>
    private void InitParamTreeList()
    {
        this.TLParamView.OptionsBehavior.AutoPopulateColumns = false;
        this.TLParamView.BeginUpdate();
        this.TLParamView.KeyFieldName = nameof(ParameterDetailModel.ID);
        this.TLParamView.ParentFieldName = nameof(ParameterDetailModel.ParentID);
        this.TLParamView.Columns.Clear();

        this.TLParamView.OptionsMenu.EnableColumnMenu = false;
        this.TLParamView.OptionsMenu.EnableFooterMenu = false;
        this.TLParamView.OptionsMenu.EnableNodeMenu = false;
        this.TLParamView.OptionsBehavior.ReadOnly = true;
        this.TLParamView.OptionsBehavior.Editable = false;
        this.TLParamView.OptionsCustomization.AllowFilter = false;
        this.TLParamView.OptionsCustomization.AllowSort = false;

        this.TLParamView.OptionsView.ShowVertLines = false;
        this.TLParamView.OptionsView.ShowHorzLines = false;
        this.TLParamView.OptionsView.ShowIndicator = false;

        this.TLParamView.OptionsSelection.EnableAppearanceFocusedRow = true;
        this.TLParamView.OptionsSelection.EnableAppearanceFocusedCell = false;

        this.TLParamView.OptionsView.FocusRectStyle = DrawFocusRectStyle.None;
        this.TLParamView.Appearance.FocusedRow.BackColor = this.focusedColor;
        this.TLParamView.Appearance.FocusedRow.ForeColor = this.focusedFontColor;


        TreeListColumn displayColumn = this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.DisplayName));
        displayColumn.Caption = @"参数列表";
        displayColumn.Visible = true;
        displayColumn.VisibleIndex = 0;

        TreeListColumn valueColumn = this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.DisplayValue));
        valueColumn.Caption = @"参数值";
        valueColumn.Visible = true;
        valueColumn.VisibleIndex = 1;

        TreeListColumn objColumn = this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.Value));
        objColumn.Visible = false;

        TreeListColumn propertyInfoColumn =
            this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.PropertyInfo));
        propertyInfoColumn.Visible = false;

        TreeListColumn parentValueColumn =
            this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.ParentInstance));
        parentValueColumn.Visible = false;

        TreeListColumn hasChildren = this.TLParamView.Columns.AddField(nameof(ParameterDetailModel.HasChildren));
        hasChildren.Visible = false;

        this.TLParamView.CustomRowFilter += this.TLParamView_CustomRowFilter;

        this.TLParamView.EndUpdate();
    }

    /// <summary>
    /// 参数树列表行过滤
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TLParamView_CustomRowFilter(object sender, CustomRowFilterEventArgs e)
    {
        if (e.Row is not ParameterDetailModel { IsNodeVisible: false } obj)
        {
            return;
        }

        e.Visible = false;
        e.Handled = true;
    }

    /// <summary>
    /// 参数组树列表节点改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TLParamGroupView_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
    {
        this.ClearEditor();
        this.ClearTLParamView();

        this.RefreshParamTreeList(e.Node);
        this.RefreshParamDataSet(e.Node);
    }

    /// <summary>
    /// 刷新数据源
    /// </summary>
    /// <param name="node"></param>
    private void RefreshParamDataSet(TreeListNode node = null)
    {
        if (node == null)
        {
            return;
        }

        this.groupSelectedNode = node;
        bool isCollection = (bool)this.groupSelectedNode.GetValue(nameof(ParamGroupModel.IsCollection));
        if (!isCollection)
        {
            this.ListBxDataSet.DataSource = null;
            return;
        }

        object dataSource = this.groupSelectedNode.GetValue(nameof(ParamGroupModel.Tag));
        if (dataSource == null)
        {
            return;
        }

        this.ListBxDataSet.DataSource = dataSource;
        this.ListBxDataSet.DisplayMember = nameof(BaseDsSetting.Name);
    }

    /// <summary>
    /// 参数数据源选中项改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ListBxDataSet_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ListBxDataSet.SelectedItem == null)
        {
            return;
        }

        // object selectedNode = this.groupSelectedNode.GetValue(nameof(ParamGroupModel.NodeEnum));
        // if (selectedNode == null)
        // {
        //     return;
        // }
        //
        // List<ParameterDetailModel> dataSource = ParamService.GetTypeParamNodes(
        //     this.ListBxDataSet.SelectedItem.GetType(), this.ListBxDataSet.SelectedItem,
        //     (TreeGroupChildNodesEnum)selectedNode);

        object nodeObj = this.TLParamGroupView.GetDataRecordByNode(this.groupSelectedNode);
        ParamGroupModel paramGroupModel = nodeObj as ParamGroupModel;

        if (paramGroupModel == null)
        {
            return;
        }
        List<ParameterDetailModel> dataSource =
            ParamService.GetDetailNodes(paramGroupModel, this.ListBxDataSet.SelectedItem);
        this.RefreshParamTreeList(dataSource);
    }

    private void RefreshParamTreeList(List<ParameterDetailModel> paramNodes)
    {
        if (paramNodes == null)
        {
            return;
        }

        this.TLParamView.BeginUpdate();
        this.TLParamView.DataSource = new BindingList<ParameterDetailModel>(paramNodes);
        this.TLParamView.RefreshDataSource();
        this.TLParamView.EndUpdate();
    }

    /// <summary>
    /// 刷新参数树列表
    /// </summary>
    /// <param name="node"></param>
    private void RefreshParamTreeList(TreeListNode node = null)
    {
        if (node != null)
        {
            this.groupSelectedNode = node;
        }
        else
        {
            return;
        }

        // object selectedNode = this.groupSelectedNode.GetValue(nameof(ParamGroupModel.NodeEnum));
        // if (selectedNode == null)
        // {
        //     return;
        // }
        //
        // List<ParameterDetailModel> paramViewNodes =
        //     ParamCacheService.Instance.GetParamNodes((TreeGroupChildNodesEnum)selectedNode);

        object nodeObj = this.TLParamGroupView.GetDataRecordByNode(this.groupSelectedNode);
        ParamGroupModel paramGroupModel = nodeObj as ParamGroupModel;

        if (paramGroupModel == null)
        {
            return;
        }

        List<ParameterDetailModel> paramViewNodes = ParamService.GetDetailNodes(paramGroupModel);

        // if (paramViewNodes == null)
        // {
        //     return;
        // }

        this.TLParamView.BeginUpdate();
        this.TLParamView.DataSource = new BindingList<ParameterDetailModel>(paramViewNodes);
        this.TLParamView.RefreshDataSource();
        this.TLParamView.EndUpdate();
    }

    /// <summary>
    /// 参数树列表节点改变事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TLParamView_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
    {
        this.ClearEditor();

        bool hasChildren = (bool)e.Node.GetValue(nameof(ParameterDetailModel.HasChildren));
        if (hasChildren)
        {
            return;
        }

        this.paramSelectModel = this.TLParamView.GetDataRecordByNode(e.Node) as ParameterDetailModel;
        this.paramSelectedNode = e.Node;

        object paramValue = e.Node.GetValue(nameof(ParameterDetailModel.Value));
        string paramName = (string)e.Node.GetValue(nameof(ParameterDetailModel.DisplayName));
        PropertyInfo propertyInfo = e.Node.GetValue(nameof(ParameterDetailModel.PropertyInfo)) as PropertyInfo;
        object parentValue = e.Node.GetValue(nameof(ParameterDetailModel.ParentInstance));

        this.currentEditor = this.ChoseEditor(paramValue, paramName, parentValue, propertyInfo);
        this.currentEditor?.ShowEditor();

        if (this.currentEditor == null)
        {
            this.BarTxtParamRange.Caption = string.Empty;
            return;
        }

        if (this.paramSelectModel != null)
        {
            if (this.paramSelectModel.MaxValue == 0 && this.paramSelectModel.MinValue == 0)
            {
                this.BarTxtParamRange.Caption = string.Empty;
            }
            else
            {
                this.BarTxtParamRange.Caption =
                    $@"{this.paramSelectModel.DisplayName}设定范围：{this.paramSelectModel.MinValue:N0}~{this.paramSelectModel.MaxValue:N0}";
            }
        }

        this.currentEditor.ValueChanged += this.EditorValueChanged;
    }

    /// <summary>
    /// 编辑值更改事件处理
    /// </summary>
    /// <param name="newValue"></param>
    private void EditorValueChanged(object newValue)
    {
        if (this.paramSelectModel == null)
        {
            return;
        }

        Type selectedPropertyType = this.paramSelectModel.PropertyInfo.PropertyType;

        // 首先判断是否是值类型或字符串类型
        if (selectedPropertyType.IsValueType ||
            selectedPropertyType == typeof(string))
        {
            this.paramSelectModel.Value = newValue;
        }
        // 这里判断是否是集合或数组
        else if (typeof(IEnumerable).IsAssignableFrom(selectedPropertyType) && selectedPropertyType != typeof(string))
        {
            Type elementType;

            // 从数组或集合中获取元素类型
            if (selectedPropertyType.IsArray)
            {
                elementType = this.paramSelectModel.PropertyInfo.PropertyType.GetElementType();
            }
            else
            {
                Type enumInterface = selectedPropertyType.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                elementType = enumInterface != null ? enumInterface.GetGenericArguments()[0] : typeof(object);
            }

            if (elementType == null)
            {
                AKRSXtraMessageBox.Show($"获取集合元素类型异常");
                return;
            }

            // 如果是值类型或字符串类型，则直接使用返回值给源元素赋值
            if (elementType.IsValueType || elementType == typeof(string))
            {
                IList listValue = (IList)this.paramSelectModel.PropertyInfo
                    .GetValue(this.paramSelectModel.ParentInstance);
                Type nullableUnderlying = Nullable.GetUnderlyingType(elementType);
                Type targetType = nullableUnderlying ?? elementType;

                object convertedValue;

                if (newValue is IConvertible &&
                    typeof(IConvertible).IsAssignableFrom(targetType))
                {
                    convertedValue = Convert.ChangeType(newValue, targetType);
                }
                else
                {
                    TypeConverter converter = TypeDescriptor.GetConverter(targetType);
                    convertedValue = converter.ConvertFromString(newValue?.ToString());
                }

                listValue[this.currentEditor.ElementIndex] = convertedValue;
                this.paramSelectModel.Value = convertedValue;
            }
        }

        if (this.paramSelectedNode == null)
        {
            return;
        }

        // 更新节点可见性
        if (this.paramSelectModel.VisibleManageNode != null)
        {
            try
            {
                this.TLParamView.FilterNodes(this.TLParamView.Nodes);
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show($"异常:{e.Message}");
            }
        }

        this.TLParamView.RefreshNode(this.paramSelectedNode);
    }

    /// <summary>
    /// 清除当前编辑器
    /// </summary>
    private void ClearEditor()
    {
        if (this.currentEditor == null)
        {
            return;
        }

        this.currentEditor.ValueChanged -= this.EditorValueChanged;
        this.currentEditor.Instance = null;
        this.currentEditor.PropertyInfo = null;
        this.currentEditor.ParamName = null;
        this.currentEditor.HideEditor();
        this.currentEditor = null;
    }

    /// <summary>
    /// 选择编辑器
    /// </summary>
    /// <param name="paramValue">参数值</param>
    /// <param name="paramName">参数名</param>
    /// <param name="parentValue">成员所属实例</param>
    /// <param name="propertyInfo">成员属性信息</param>
    /// <param name="isValueCollection">是否是值类型集合</param>
    /// <param name="elementIndex">集合元素索引</param>
    /// <returns>编辑器</returns>
    private IParamEditor ChoseEditor(object paramValue, string paramName, object parentValue,
        PropertyInfo propertyInfo = null, bool isValueCollection = false, int elementIndex = -1)
    {
        // 防止点击到的是空节点
        if (paramValue == null)
        {
            return null;
        }

        // 首先判断当前节点类型
        Type t = propertyInfo?.PropertyType ?? paramValue.GetType();

        // 如果是值类型或字符串类型，则直接拥有来该节点的父节点对象以及当前节点属性信息传入
        if (t.IsValueType || t == typeof(string))
        {
            bool result = this.editors.TryGetValue(t.IsEnum ? typeof(Enum) : t, out IParamEditor editor);
            if (!result)
            {
                return null;
            }

            editor.EditorContainer = this.PcEditorView;
            editor.Instance = parentValue;
            editor.PropertyInfo = [propertyInfo];
            editor.ParamName = paramName;
            editor.IsValueTypeCollection = isValueCollection;
            editor.ElementIndex = elementIndex;
            editor.ParamNodeModel = this.paramSelectModel;
            return editor;
        }
        // 如果当前节点是数组或者集合，则需要判断元素类型是否为基础类型后再进行编辑器的编辑对象传入
        else if (typeof(IEnumerable).IsAssignableFrom(t) && t != typeof(string))
        {
            Type elementType;

            if (t.IsArray)
            {
                elementType = t.GetElementType();
            }
            else
            {
                Type enumInterface = t.GetInterfaces()
                    .FirstOrDefault(i =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

                elementType = enumInterface != null ? enumInterface.GetGenericArguments()[0] : typeof(object);
            }

            if (elementType != null
                && (elementType.IsValueType || elementType == typeof(string)))
            {
                // 如果当前节点是值类型或字符串类型，则传入当前节点的数组或集合对象以及当前节点的索引信息
                object collectionInstance = propertyInfo?.GetValue(parentValue);
                return this.ChoseEditor(paramValue, paramName, collectionInstance, isValueCollection: true,
                    elementIndex: this.paramSelectModel.CollectionIndex);
            }

            return this.ChoseEditor(paramValue, paramName, parentValue);
        }
        else
        {
            // 如果是自定义类型，则传入该类型实例以及所有公开属性信息
            bool result = this.editors.TryGetValue(t, out IParamEditor editor);
            if (!result)
            {
                return null;
            }

            editor.EditorContainer = this.PcEditorView;
            editor.Instance = paramValue;
            editor.PropertyInfo = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            editor.ParamName = paramName;
            editor.ParamNodeModel = this.paramSelectModel;

            return editor;
        }
    }

    /// <summary>
    /// 上次搜索结果
    /// </summary>
    private List<(TreeGroupChildNodesEnum Group, List<ParameterDetailModel> DataSource, ParameterDetailModel Match,
            object DatasetObj)>
        searchHits = new();

    private int currentSearchIndex = -1;
    private string lastSearchParam = null;

    /// <summary>
    /// 清空参数树列表
    /// </summary>
    private void ClearTLParamView()
    {
        this.TLParamView.BeginUpdate();
        this.TLParamView.DataSource = null;
        this.TLParamView.RefreshDataSource();
        this.TLParamView.EndUpdate();
    }

    /// <summary>
    /// 显示参数搜索结果
    /// </summary>
    /// <param name="paramGroupEnum"></param>
    /// <param name="dataSource"></param>
    /// <param name="matchModel"></param>
    private void ShowParamSearchResult(TreeGroupChildNodesEnum paramGroupEnum, List<ParameterDetailModel> dataSource,
        ParameterDetailModel matchModel)
    {
        TreeGroupChildNodesEnum paramGroup = paramGroupEnum;
        List<ParameterDetailModel> paramNodes = dataSource;
        TreeListNode paramGroupNodeToFocus =
            this.TLParamGroupView.FindNodeByFieldValue(nameof(ParamGroupModel.NodeEnum), paramGroup);
        this.TLParamGroupView.SetFocusedNode(paramGroupNodeToFocus);
        this.TLParamGroupView.MakeNodeVisible(paramGroupNodeToFocus);
        this.TLParamView.BeginUpdate();
        this.TLParamView.DataSource = new BindingList<ParameterDetailModel>(paramNodes);
        this.TLParamView.RefreshDataSource();
        this.TLParamView.EndUpdate();

        TreeListNode paramNodeToFocus = this.TLParamView.FindNodeByKeyID(matchModel.ID);
        if (paramNodeToFocus == null)
        {
            AKRSXtraMessageBox.Show("节点为空");
            return;
        }


        this.TLParamView.SetFocusedNode(paramNodeToFocus);
        this.TLParamView.MakeNodeVisible(paramNodeToFocus);
    }

    /// <summary>
    /// 显示参数搜索结果
    /// </summary>
    /// <param name="paramGroupEnum"></param>
    /// <param name="dataSource"></param>
    /// <param name="matchModel"></param>
    private void ShowParamSearchResult(TreeGroupChildNodesEnum paramGroupEnum, List<ParameterDetailModel> dataSource,
        ParameterDetailModel matchModel, object datasetObj)
    {
        TreeGroupChildNodesEnum paramGroup = paramGroupEnum;
        List<ParameterDetailModel> paramNodes = dataSource;
        TreeListNode paramGroupNodeToFocus =
            this.TLParamGroupView.FindNodeByFieldValue(nameof(ParamGroupModel.NodeEnum), paramGroup);
        this.TLParamGroupView.SetFocusedNode(paramGroupNodeToFocus);
        this.TLParamGroupView.MakeNodeVisible(paramGroupNodeToFocus);
        if (datasetObj != null)
        {
            this.ListBxDataSet.SelectedItem = datasetObj;
        }

        this.TLParamView.BeginUpdate();
        this.TLParamView.DataSource = new BindingList<ParameterDetailModel>(paramNodes);
        this.TLParamView.RefreshDataSource();
        this.TLParamView.EndUpdate();

        TreeListNode paramNodeToFocus = this.TLParamView.FindNodeByKeyID(matchModel.ID);
        if (paramNodeToFocus == null)
        {
            AKRSXtraMessageBox.Show("节点为空");
            return;
        }

        this.TLParamView.SetFocusedNode(paramNodeToFocus);
        this.TLParamView.MakeNodeVisible(paramNodeToFocus);
    }

    /// <summary>
    /// 搜索按钮单击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
    {
        string paramName = ((string)this.TxtParamNameToSearch.EditValue).Trim();
        if (string.IsNullOrEmpty(paramName))
        {
            this.ClearTLParamView();
            this.LblSearchTip.Caption = $"匹配 {0} / {0}";
            this.lastSearchParam = paramName;
            return;
        }

        // 新关键字或还没初始化，就重建 searchHits
        if (this.lastSearchParam != paramName || this.searchHits == null || this.searchHits.Count == 0)
        {
            List<SearchResult> searchResult = ParamService.Search(paramName);
            this.searchHits.Clear();
            searchResult.ForEach(it =>
            {
                this.searchHits.Add((it.GroupNodeType, it.ParamDetailNodes, it.CurrentSelectedNode, it.Element));
            });

            if (this.searchHits.Count == 0)
            {
                AKRSXtraMessageBox.Show($"未找到名为 {paramName} 的参数",
                    "搜索提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.LblSearchTip.Caption = $"匹配 {0} / {0}";
                this.lastSearchParam = paramName;
                return;
            }

            this.lastSearchParam = paramName;
            this.currentSearchIndex = 0;
        }
        else
        {
            // 同一关键字，循环下一个
            this.currentSearchIndex = (this.currentSearchIndex + 1) % this.searchHits.Count;
        }

        // 跳转到当前这一条
        (TreeGroupChildNodesEnum Group, List<ParameterDetailModel> DataSource, ParameterDetailModel Match, object DatasetObj) hit =
            this.searchHits[this.currentSearchIndex];
        this.ShowParamSearchResult(hit.Group, hit.DataSource, hit.Match, hit.DatasetObj);

        // 更新提示
        this.LblSearchTip.Caption = $"匹配 {this.currentSearchIndex + 1} / {this.searchHits.Count}";
    }

    private void PastSearchMethod()
    {
        string paramName = ((string)this.TxtParamNameToSearch.EditValue).Trim();
        if (string.IsNullOrEmpty(paramName))
        {
            this.ClearTLParamView();
            this.LblSearchTip.Caption = $"匹配 {0} / {0}";
            this.lastSearchParam = paramName;
            return;
        }

        // 新关键字或还没初始化，就重建 searchHits
        if (this.lastSearchParam != paramName || this.searchHits == null || this.searchHits.Count == 0)
        {
            // Search 返回的每一项：分组 + 整组里“已经过滤过”的参数列表
            List<(TreeGroupChildNodesEnum, List<ParameterDetailModel>)> searchResult = ParamCacheService.Instance
                .SearchParamByMatchName(paramName);

            // 扁平化 每个列表里的每个 Model 都展开成一条 hit
            // this.searchHits = searchResult
            //     .SelectMany(tuple =>
            //         tuple.Item2
            //             .Select(model =>
            //                 (Group: tuple.Item1,
            //                     DataSource: tuple.Item2,
            //                     Match: model)
            //             )
            //     )
            //     .ToList();

            if (this.searchHits.Count == 0)
            {
                AKRSXtraMessageBox.Show($"未找到名为 {paramName} 的参数",
                    "搜索提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.LblSearchTip.Caption = $"匹配 {0} / {0}";
                this.lastSearchParam = paramName;
                return;
            }

            this.lastSearchParam = paramName;
            this.currentSearchIndex = 0;
        }
        else
        {
            // 同一关键字，循环下一个
            this.currentSearchIndex = (this.currentSearchIndex + 1) % this.searchHits.Count;
        }

        // // 跳转到当前这一条
        // (TreeGroupChildNodesEnum Group, List<ParameterDetailModel> DataSource, ParameterDetailModel Match) hit =
        //     this.searchHits[this.currentSearchIndex];
        // this.ShowParamSearchResult(hit.Group, hit.DataSource, hit.Match);

        // 更新提示
        this.LblSearchTip.Caption = $"匹配 {this.currentSearchIndex + 1} / {this.searchHits.Count}";
    }
}