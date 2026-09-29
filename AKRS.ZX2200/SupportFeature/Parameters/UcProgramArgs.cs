using AKRS.Galaxy2.Infrastructure.CommonServices.PropertyChanged;
using AKRS.ZX2200.SupportFeature.Parameters.ObjParameter;

namespace AKRS.ZX2200.SupportFeature.Parameters
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.SupportFeature.Parameters.Attributes;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.SupportFeature.Parameters.Models;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraTreeList;
    using DevExpress.XtraTreeList.Nodes;
    using SqlSugar.Extensions;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 参数
    /// </summary>
    public partial class UcProgramArgs : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 当前对象
        /// </summary>
        private object currentObj;

        /// <summary>
        /// 当前对象的类别
        /// </summary>
        private Type type = null;

        /// <summary>
        /// 上传的对象和节点位置的集合
        /// </summary>
        public List<ObjAndNodesInfo> TreeProgramsArgsList { get; set; } = new List<ObjAndNodesInfo>();

        /// <summary>
        /// 枚举转文字的字典，TreeGroupList的父节点
        /// </summary>
        public Dictionary<string, string> TreeGroupParentNodesEnumToNodesDictionary { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 枚举转文字的字典，TreeGroupList子节点
        /// </summary>
        public Dictionary<string, string> TreeGroupChildNodesEnumToNodesDictionary { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 文字转枚举的字典，TreeGroupList的父节点文字对应枚举
        /// </summary>
        public Dictionary<string, string> TreeGroupParentNodesToEnumDictionary { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// 文字转枚举的字典，TreeGroupList子节点文字对应枚举
        /// </summary>
        public Dictionary<string, string> TreeGroupChildNodesToEnumDictionary { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///  TreeGroup选中的字节点名称
        /// </summary>
        public string TreeGroupFocusNode { get; set; } = null;

        /// <summary>
        ///  TreeDataSet选中的字节点名称
        /// </summary>
        public string TreeDataSetFocusNode { get; set; } = null;

        /// <summary>
        /// 初始化控件内容
        /// </summary>
        public static Action<UcProgramArgs> InitParamerer;

        private List<ObjAndNodesInfo> searchResult;

        private int currentIndex = 0;

        private string searchStr;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcProgramArgs()
        {
            this.InitTreeGroupNodeDic();
            this.InitializeComponent();
            InitParamerer(this);
        }

        /// <summary>
        /// 初始化字典，根据枚举存储TreeGroup的UI文字
        /// </summary>
        public void InitTreeGroupNodeDic()
        {
            #region 枚举转文字
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Product.ToString(), "产品");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Transportunit.ToString(), "产品框架参数");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Component.ToString(), "芯片");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Epoxyapplication.ToString(), "Epoxy application");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Tool.ToString(), "工具");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Recognition.ToString(), "辨识");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Machine.ToString(), "机器");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Transportsystem.ToString(), "传输系统");
            this.TreeGroupParentNodesEnumToNodesDictionary.Add(TreeGroupParentNodesEnum.Other.ToString(), "其他");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.General.ToString(), "常规");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Systemrelated.ToString(), "系统相关");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Configuration.ToString(), "配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Processinglist.ToString(), "流程集合");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Waferchange.ToString(), "换晶圆");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Wafermagazinegeometry.ToString(), "晶圆盒几何形状");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.TransportUnitConfig.ToString(), "框架参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.SubstrateConfig.ToString(), "基板参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.ModuleConfig.ToString(), "基岛参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Staticbadmoduletable.ToString(), "基岛不良数据表");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.BondPositionConfig.ToString(), "焊点参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Postbondinspection.ToString(), "焊后/胶后参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Heightmeasurement.ToString(), "Height measurement");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Component.ToString(), "芯片");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Wafermagazineallocations.ToString(), "料盒配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Wafflegelpackadapter.ToString(), "适配器配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Multiwafer.ToString(), "多晶圆");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Feederbank.ToString(), "飞达");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Badchiptable.ToString(), "坏芯片表");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Epoxyapplication.ToString(), "胶型");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.EpoxyFlux.ToString(), "胶水");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Dispensepattern.ToString(), "模式");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.PPandepoxytools.ToString(), "固晶、点胶工具");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Dispenser.ToString(), "点胶器");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Ejectionsystem.ToString(), "顶针系统");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.PPtoolbankallocation.ToString(), "吸嘴架配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.EStoolbankallocation.ToString(), "顶针架配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Fliptool.ToString(), "翻转工具");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Toolholder.ToString(), "刀架");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Adjustdata.ToString(), "辨识数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Multiplesearch.ToString(), "多重搜索");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Search.ToString(), "搜索");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.NozzleClean.ToString(), "吸嘴清洁");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.MachineHardwarecConfiguration.ToString(), "机器硬件配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.MachineSoftwarecConfiguration.ToString(), "机器软件配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.MachineData.ToString(), "机器数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Moduleconnfiguration.ToString(), "模组配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Moduledata.ToString(), "模组数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Toolbank.ToString(), "吸嘴架");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Statisticdata.ToString(), "统计数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Errorhandling.ToString(), "错误处理");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Securitygeometries.ToString(), "安全区域");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Movingsecuritygeometries.ToString(), "安全区域设置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Motordata.ToString(), "电机数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Camera.ToString(), "相机");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.BMC.ToString(), "BMC平台");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.IPT.ToString(), "中转台");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.TSsubstrateconfiguration.ToString(), "传输基板配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.IOSubstratedata.ToString(), "基板数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.TUData.ToString(), "流道数据");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Bondinsert.ToString(), "挡料气缸相关设置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Hardwareconfiguration.ToString(), "硬件配置");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Ppartheightmap.ToString(), "P-part高度map图");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Loader.ToString(), "上料仓");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Unloader.ToString(), "下料仓");

            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Bincode.ToString(), "Bin代码");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Eumel.ToString(), "Eumel");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Barcodescanner.ToString(), "条形码扫描仪");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Singlecomponenttracking.ToString(), "芯片跟踪");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.SECSparameters.ToString(), "SECS参数");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Filearchiving.ToString(), "文件存档");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Bondingoptimization.ToString(), "固晶精度优化");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Host.ToString(), "主机");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.Datalogger.ToString(), "数据日志");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.ModbusConnect.ToString(), "Modbus通讯");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.ForceCalibrate.ToString(), "力控标定");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.ForceControl.ToString(), "大力力控");
            this.TreeGroupChildNodesEnumToNodesDictionary.Add(TreeGroupChildNodesEnum.SmallForceControl.ToString(), "小力力控");
            #endregion

            #region 文字转枚举
            foreach (KeyValuePair<string, string> pair in this.TreeGroupParentNodesEnumToNodesDictionary)
            {
                this.TreeGroupParentNodesToEnumDictionary.Add(pair.Value, pair.Key);
            }

            foreach (KeyValuePair<string, string> pair in this.TreeGroupChildNodesEnumToNodesDictionary)
            {

                this.TreeGroupChildNodesToEnumDictionary.Add(pair.Value, pair.Key);
            }
            #endregion
        }

        public UcProgramArgs FindPage(UcProgramArgs ucProgramArgs,TreeGroupParentNodesEnum treeGroupParentNodesEnum, TreeGroupChildNodesEnum treeGroupChildNodesEnum, string dataSetName)
        {
            string treeGroupParentNodesEnumStr = treeGroupParentNodesEnum.ToString();
            string treeGroupChildNodesEnumStr = treeGroupChildNodesEnum.ToString();

            bool ParentNodeExist = ucProgramArgs.TreeGroupParentNodesEnumToNodesDictionary.ContainsKey(treeGroupParentNodesEnumStr);
            bool ChildNodeExist = ucProgramArgs.TreeGroupChildNodesEnumToNodesDictionary.ContainsKey(treeGroupChildNodesEnumStr);

            ObjAndNodesInfo objAndNodesInfo = ucProgramArgs.TreeProgramsArgsList.Find(obj => { return obj.GroupTreeCurrentText.ToString() == treeGroupChildNodesEnumStr && obj.TreeDataSetListNode == dataSetName; });

            if (ParentNodeExist && ChildNodeExist && objAndNodesInfo != null)
            {
                this.currentObj = objAndNodesInfo.CurrentObj;
                //ucProgramArgs.SetFocusedNodeByDisplayText(ucProgramArgs.treeGroupList, treeGroupChildNodesEnumStr);
                //ucProgramArgs.SetFocusedNodeByDisplayText(ucProgramArgs.treeDataSetList, dataSetName);

                return ucProgramArgs;
            }
            else
            {
                return null;
            }
        }

        public void SetFocusedNodeByDisplayText(TreeList tr, string displayText)
        {
            if (tr.Nodes.Count == 0) return;
            List<TreeListNode> lstNode = tr.GetNodeList();//获取树的所有节点int i = lstNode.IndexOf(tr.FocusedNode);
            int i = lstNode.IndexOf(tr.FocusedNode);//焦点节点所在的位置
            int beginIndex = i + 1;//开始搜索匹配的位置
            int waitIndex = lstNode.Count - 1 - i; // 开始位置后面需要匹配的节点位数
            bool isFind = false;//是否找到需要的节点
            if (beginIndex == lstNode.Count - 1)// 如果焦点节点是树中的最后一个节点，则从e开始匹配
            {
                beginIndex = 0;
            }
            while (beginIndex < lstNode.Count)
            {
                //从焦点节点到最后一个节点间进行匹配
                if (lstNode[beginIndex].GetDisplayText(tr.Columns[0].FieldName).Contains(displayText))
                {
                    tr.FocusedNode = lstNode[beginIndex];
                    lstNode[beginIndex].Expand();
                    isFind = true;
                    break;
                }

                beginIndex += 1;
            }

            if (isFind == false)//如果在焦点节点到最后一个节点中没有匹配到想要的结果，则匹配从e开始到焦点节点位置
            {
                //匹配开始位置之前的
                for (int j = 0; j < lstNode.Count - waitIndex; j++)
                {
                    if (lstNode[j].GetDisplayText(tr.Columns[0].FieldName).Contains(displayText))
                    {
                        tr.FocusedNode = lstNode[j]; lstNode[j].Expand();
                        break;
                    }
                }
            }
        }



        /// <summary>
        /// treeGroupList的焦点事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void treeGroupList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            this.treeGroupList.Appearance.FocusedCell.BackColor = Color.DarkGray;

            TreeListNode parentNode = e.Node.ParentNode;

            // 当前节点没有父节点，将所有的UI效果隐藏
            if (parentNode == null)
            {
                this.SetControlVisibleFalse();
            }
            else
            {
                //当前节点是子节点
                // 获取当前焦点的节点UI名称
                this.TreeGroupFocusNode = this.treeGroupList.GetRowCellValue(e.Node, "GroupName").ToString();

                // treeDataSetList界面清空
                this.treeDataSetList.Nodes.Clear();

                foreach (ObjAndNodesInfo obj in this.TreeProgramsArgsList)
                {
                    if (obj.CurrentObj != null)
                    {
                        //当前焦点的节点UI名称 与 封装对象的TreeGroup节点作比较
                        if (this.TreeGroupFocusNode.Equals(this.TreeGroupChildNodesEnumToNodesDictionary[obj.GroupTreeCurrentText.ToString()]))
                        {
                            // treeDataSet添加节点，值为封装对象的TreeDataSetListNode属性，此时会自动触发treeDataSetList的FocusedNodeChanged事件
                            this.treeDataSetList.AppendNode(new object[] { obj.TreeDataSetListNode }, null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// treeDataSetList的焦点事件，向treeProgramsArgsList添加Node
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void treeDataSetList_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            this.treeDataSetList.Appearance.FocusedCell.BackColor = Color.DarkGray;

            this.treeProgramsArgsList.Nodes.Clear();

            // treeDataSET节点被清空时会触发该事件，此时e.Node=null
            if (e.Node == null)
            {

            }
            else
            {
                // treeDataSET节点被增加，或者改变了当前的焦点
                // 获取当前焦点的节点UI名称
                this.TreeDataSetFocusNode = this.treeDataSetList.GetRowCellValue(e.Node, "DataSetName").ToString();
                foreach (ObjAndNodesInfo obj in this.TreeProgramsArgsList)
                {
                    // 寻找，treeDataSet当前焦点的节点UI名称和treeGroup的当前节点UI名称 与 封装的对象的两个属性完全相同的对象
                    if (this.TreeDataSetFocusNode == obj.TreeDataSetListNode && this.TreeGroupFocusNode.Equals(this.TreeGroupChildNodesEnumToNodesDictionary[obj.GroupTreeCurrentText.ToString()]))
                    {
                        //指定当前操作的对象 和 对象的类型
                        this.currentObj = obj.CurrentObj;
                        this.type = obj.CurrentObj.GetType();

                        // 将该对象抛到treeProgramArgsList上,用于显示
                        this.InitTreeProgramsArgsList(this.currentObj, this.type);
                    }
                }
            }
        }

        /// <summary>
        /// 将特定对象的属性和值插入treeProgramsArgsList的节点上
        /// </summary>
        /// <param name="clazz">clazz</param>
        /// <param name="type">type</param>
        public void InitTreeProgramsArgsList(Object clazz, Type type)
        {
            // 第一优先级：是否插入根节点,是则先创建根节点
            // 第二优先级：是否显示在UI
            // 第三优先级：是否改界面值
            // 第四优先级，插入节点

            TreeListNode Parentnode = null;

            PropertyInfo[] properties = type.GetProperties();

            //对当前对象的所有属性进行遍历
            foreach (PropertyInfo property in properties)
            {
                Type propertyType = property.PropertyType;
                object[] objs = null;

                //获取自定义注解
                objs = property.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                //不显示在UI界面的标识符
                bool Ignoreflag = false;

                //注解对象
                TreeProgramListArgsAttribute objAttribute = null;

                //如果没有注解
                if (objs.Length == 0)
                {
                    Ignoreflag = true;
                }
                else if (objs[0] != null && objs.Length > 0)
                {
                    //如果注解存在
                    objAttribute = (TreeProgramListArgsAttribute)objs[0];
                    //读取注解中的VisibleFlaseEnum
                    if (objAttribute.VisibleFlaseEnum != null)
                    {
                        //如果该注解与TreeGroup的焦点文字一致
                        foreach (TreeGroupChildNodesEnum visibalFalseEnum in objAttribute.VisibleFlaseEnum)
                        {
                            if (this.TreeGroupChildNodesEnumToNodesDictionary[visibalFalseEnum.ToString()].Equals(this.TreeGroupFocusNode))
                            {
                                Ignoreflag = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Ignoreflag = false;
                    }
                }

                // 没加注解不显示
                if (Ignoreflag)
                {

                }
                else
                {
                    // 如果需要根节点
                    if (objs.Length > 0 && !string.IsNullOrEmpty(objAttribute.ParentNode))
                    {
                        // 注解对象的根节点名称
                        string nodeText = objAttribute.ParentNode;
                        TreeListNode treeListNode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == nodeText; });
                        if (treeListNode != null)
                        {
                            //指定根节点
                            Parentnode = treeListNode;
                            Parentnode.Expanded = true;
                        }
                        else
                        {
                            //创建根节点
                            this.treeProgramsArgsList.AppendNode(new object[] { nodeText, null }, null);
                            Parentnode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == nodeText; });
                            Parentnode.Expanded = true;
                        }
                    }
                    else
                    {
                        Parentnode = null;
                    }

                    if (property.PropertyType.FullName == "System.Boolean")
                    {
                        // UI层，如果是Bool类型显示为开或关
                        string flag = null;
                        if ((bool)property.GetValue(clazz))
                        {
                            flag = "开";
                        }
                        else
                        {
                            flag = "关";
                        }
                        // 插入节点,会触发treeProgramsArgsList的焦点事件
                        this.treeProgramsArgsList.AppendNode(new object[] { objAttribute.ShowText, flag }, Parentnode);
                    }
                    else if (property.PropertyType.BaseType.FullName == "System.Enum")
                    {
                        // 默认为属性值
                        string textValue = property.Name;

                        //有注解并且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        //插入节点,会触发treeProgramsArgsList的焦点事件
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, property.GetValue(clazz) }, Parentnode);
                    }
                    else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D[]")
                    {
                        // 数组的根节点默认为属性值
                        string textValue = property.Name;

                        //数组的根节点有别名且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        //添加根节点
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, null }, Parentnode);

                        //指定根节点
                        TreeListNode selectedNode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == textValue; });

                        //遍历数组中的元素个数
                        AKRSPoint3D[] akrsPoint3Ds = (AKRSPoint3D[])property.GetValue(clazz);

                        //获取数组对象的Description注解，用于显示数组的子节点
                        // object[] descriptions = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        // string description = null;
                        // if (descriptions.Length > 0 && ((DescriptionAttribute)descriptions[0]).Description != null)
                        // {
                        //     description = ((DescriptionAttribute)descriptions[0]).Description;
                        // }
                        //
                        // if (description == null)
                        // {
                        //     //如果没有注解，默认为数组的属性名称
                        //     description = property.Name;
                        // }

                        string description = textValue;

                        for (int i = 0; i < akrsPoint3Ds.Count(); i++)
                        {
                            //向数组的根节点添加数组元素的子节点
                            this.treeProgramsArgsList.AppendNode(new object[] { description + " " + (i + 1), akrsPoint3Ds[i] }, selectedNode);
                        }
                    }
                    else if (property.PropertyType == typeof(ListEx<AKRSPoint3D>))
                    {
                        // 数组的根节点默认为属性值
                        string textValue = property.Name;

                        //数组的根节点有别名且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        //添加根节点
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, null }, Parentnode);

                        //指定根节点
                        TreeListNode selectedNode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == textValue; });

                        //遍历数组中的元素个数
                        AKRSPoint3D[] akrsPoint3Ds = ((ListEx<AKRSPoint3D>)property.GetValue(clazz)).ToArray();

                        // //获取数组对象的Description注解，用于显示数组的子节点
                        // object[] descriptions = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        // string description = null;
                        // if (descriptions.Length > 0 && ((DescriptionAttribute)descriptions[0]).Description != null)
                        // {
                        //     description = ((DescriptionAttribute)descriptions[0]).Description;
                        // }
                        //
                        // if (description == null)
                        // {
                        //     //如果没有注解，默认为数组的属性名称
                        //     description = property.Name;
                        // }
                        string description = textValue;
                        for (int i = 0; i < akrsPoint3Ds.Count(); i++)
                        {
                            //向数组的根节点添加数组元素的子节点
                            this.treeProgramsArgsList.AppendNode(new object[] { description + " " + (i + 1), akrsPoint3Ds[i] }, selectedNode);
                        }
                    }
                    else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D[]")
                    {
                        // 数组的根节点默认为属性值
                        string textValue = property.Name;

                        //数组的根节点有别名且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        //添加根节点
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, null }, Parentnode);

                        //指定根节点
                        TreeListNode selectedNode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == textValue; });

                        //遍历数组中的元素个数
                        AKRSPoint2D[] akrsPoint2Ds = (AKRSPoint2D[])property.GetValue(clazz);

                        // //获取数组对象的Description注解，用于显示数组的子节点
                        // object[] descriptions = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        // string description = null;
                        // if (descriptions.Length > 0 && ((DescriptionAttribute)descriptions[0]).Description != null)
                        // {
                        //     description = ((DescriptionAttribute)descriptions[0]).Description;
                        // }
                        //
                        // if (description == null)
                        // {
                        //     //如果没有注解，默认为数组的属性名称
                        //     description = property.Name;
                        // }
                        string description = textValue;
                        for (int i = 0; i < akrsPoint2Ds.Count(); i++)
                        {
                            //向数组的根节点添加数组元素的子节点
                            this.treeProgramsArgsList.AppendNode(new object[] { description + " " + (i + 1), akrsPoint2Ds[i] }, selectedNode);
                        }
                    }
                    else if (property.PropertyType == typeof(double[]))
                    {
                        // 数组的根节点默认为属性值
                        string textValue = property.Name;

                        // 数组的根节点有别名且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        // 添加根节点
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, null }, Parentnode);

                        // 指定根节点
                        TreeListNode selectedNode = this.treeProgramsArgsList.FindNode((n) => { return n.GetDisplayText("Paramemter") == textValue; });

                        // 遍历数组中的元素个数
                        double[] pointsValue = (double[])property.GetValue(clazz);

                        // // 获取数组对象的Description注解，用于显示数组的子节点
                        // object[] descriptions = property.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                        // string description = null;
                        // if (descriptions.Length > 0 && ((DescriptionAttribute)descriptions[0]).Description != null)
                        // {
                        //     description = ((DescriptionAttribute)descriptions[0]).Description;
                        // }
                        //
                        // if (description == null)
                        // {
                        //     // 如果没有注解，默认为数组的属性名称
                        //     description = property.Name;
                        // }

                        string description = textValue;
                        for (int i = 0; i < pointsValue.Count(); i++)
                        {
                            // 向数组的根节点添加数组元素的子节点
                            this.treeProgramsArgsList.AppendNode(new object[] { description + " " + (i + 1), pointsValue[i] }, selectedNode);
                        }
                    }
                    else
                    {
                        //不做处理，直接向ProgramTree添加节点

                        // 默认为属性值
                        string textValue = property.Name;

                        //有注解并且不为Null
                        if (objs.Length > 0 && objAttribute.ShowText != null)
                        {
                            // 修改文本值
                            textValue = objAttribute.ShowText;
                        }

                        //插入节点,会触发treeProgramsArgsList的焦点事件
                        this.treeProgramsArgsList.AppendNode(new object[] { textValue, property.GetValue(clazz) }, Parentnode);
                    }
                }
            }
        }

        /// <summary>
        /// treeProgramsArgsList的点击事件，会根据所选行的类型属性，在界面右侧弹出修改界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void treeProgramsArgsList_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            this.treeProgramsArgsList.Appearance.FocusedCell.BackColor = Color.DarkGray;

            this.SetControlVisibleFalse();

            // 该节点被清空时会触发，e.node为空 , 或者该节点作为父节点
            if (e.Node == null)
            { 
                this.LabelUnitNameText.Text = this.ParamValueTextEdit.Text = string.Empty;
                this.EditLabel.Text = string.Empty;
            }
            else
            {
                // 该节点被增加，或者改变了当前的焦点，会触发

                //修改值界面的提示文本
                string infoText = string.Empty;

                // 读取所选行的属性和值
                string para = this.treeProgramsArgsList.GetRowCellValue(e.Node, "Paramemter").ToString();
                object paraVal = this.treeProgramsArgsList.GetRowCellValue(e.Node, "Value");

                PropertyInfo[] properties = this.type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //该属性的注解，TreeProgram界面显示的Param值
                    string showText = null;
                    object[] objs = property.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                    TreeProgramListArgsAttribute objAttribute = null;

                    if (objs != null && objs.Length > 0)
                    {
                        objAttribute = (TreeProgramListArgsAttribute)objs[0];
                        showText = objAttribute.ShowText;
                    }

                    //如果该属性的名称等于界面UI值，或者该属性的显示文本注解等于界面UI
                    if (property.Name.Equals(para) || (showText != null && showText.Equals(para)))
                    {
                        Type propertyType = property.PropertyType;

                        // 读取InfoText，修改属性时，作为提示信息显示
                        if (objs.Length > 0 && objAttribute.InfoText != null)
                        {
                            infoText = objAttribute.InfoText;
                        }
                        else
                        {
                            infoText = string.Empty;
                        }

                        this.SetControlEnable(objAttribute);

                        // 该属性为enum类型的UI处理
                        if (property.PropertyType.BaseType.FullName == "System.Enum")
                        {
                            // 获取枚举的所有成员
                            MemberInfo[] members = propertyType.GetMembers();

                            // 遍历枚举的成员
                            foreach (MemberInfo member in members)
                            {
                                // 判断成员是否为枚举值
                                if (member.MemberType == MemberTypes.Field)
                                {
                                    string memberName = member.Name.ToString();

                                    // 读取枚举值的注解
                                    DescriptionAttribute[] enumVal = member.GetCustomAttributes<DescriptionAttribute>(true).ToArray();

                                    //if (enumVal.Length > 0)
                                    //{
                                    //    // 如果添加了注解就改成注解
                                    //    memberName = enumVal[0].Description;

                                    //    if (!memberName.Equals("value__"))
                                    //    {
                                    //        memberName = enumVal[0].Description;

                                    //        // 每一个单元按钮对应的选线item
                                    //        RadioGroupItem item = new RadioGroupItem();

                                    //        // 设置选项的value值
                                    //        item.Value = memberName;

                                    //        // 设置选项的描述值 即 要显示的值
                                    //        item.Description = memberName;

                                    //        // 使选项启用
                                    //        item.Enabled = true;

                                    //        // 将新增的选项添加到radiogroup的Items中
                                    //        this.radioGroup1.Properties.Items.Add(item);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    //    // 如果没有注解就直接用属性名
                                    //    if (!memberName.Equals("value__"))
                                    //    {
                                    //        memberName = member.Name.ToString();

                                    //        // 每一个单元按钮对应的选线item
                                    //        RadioGroupItem item = new RadioGroupItem();

                                    //        // 设置选项的value值
                                    //        item.Value = memberName;

                                    //        // 设置选项的描述值 即 要显示的值
                                    //        item.Description = memberName;

                                    //        // 使选项启用
                                    //        item.Enabled = true;

                                    //        // 将新增的选项添加到radiogroup的Items中
                                    //        this.radioGroup1.Properties.Items.Add(item);
                                    //    }
                                    //}

                                    // 所有枚举遍历都会有"value_"，这个内容不显示
                                    if (memberName.Equals("value__"))
                                    {
                                    }
                                    else
                                    {
                                        // 将枚举的对应值添加到UI中


                                        // 每一个单元按钮对应的选线item
                                        RadioGroupItem item = new RadioGroupItem();

                                        // 设置选项的value值
                                        item.Value = memberName;

                                        // 设置选项的描述值 即 要显示的值
                                        if (enumVal.Length > 0)
                                        {
                                            item.Description = enumVal[0].Description;
                                        }
                                        else
                                        {
                                            item.Description = memberName;
                                        }
                                        
                                        // 使选项启用
                                        item.Enabled = true;

                                        // 将新增的选项添加到radiogroup的Items中
                                        this.radioGroup1.Properties.Items.Add(item);
                                    }
                                }
                            }

                            this.radioGroup1.EditValue = (string)property.GetValue(this.currentObj).ToString();
                            this.radioGrouppanelControl.Width = 250;
                            this.radioGrouppanelControl.Height = 370;
                            this.radioGrouppanelControl.Dock = DockStyle.Top;
                            this.radioGrouppanelControl.Visible = true;
                        }

                        // 该属性为Bool类型的UI处理
                        else if (property.PropertyType.FullName == "System.Boolean")
                        {
                            RadioGroupItem item = new RadioGroupItem();
                            item.Value = "开";
                            item.Description = "开";
                            item.Enabled = true;
                            this.radioGroup1.Properties.Items.Add(item);

                            RadioGroupItem item1 = new RadioGroupItem();
                            item1.Value = "关";
                            item1.Description = "关";
                            item1.Enabled = true;
                            this.radioGroup1.Properties.Items.Add(item1);

                            if ((bool)property.GetValue(this.currentObj))
                            {
                                this.radioGroup1.EditValue = "开";
                            }
                            else
                            {
                                this.radioGroup1.EditValue = "关";
                            }

                            this.radioGrouppanelControl.Width = 250;
                            this.radioGrouppanelControl.Height = 370;
                            this.radioGrouppanelControl.Dock = DockStyle.Top;
                            this.radioGrouppanelControl.Visible = true;
                        }

                        // 该属性为string类型的UI处理
                        else if (property.PropertyType.FullName == "System.String")
                        {
                            this.editPanelControl.Width = 250;
                            this.editPanelControl.Height = 370;
                            this.editPanelControl.Dock = DockStyle.Top;
                            this.editPanelControl.Visible = true;
                            this.EditLabel.Text = infoText;
                            this.LabelUnitNameText.Visible = this.ParamValueTextEdit.Visible = true;

                            if (paraVal == null)
                            {
                                // string类型
                                this.LabelUnitNameText.Text = this.ParamValueTextEdit.Text = string.Empty;
                            }
                            else
                            {
                                this.ParamValueTextEdit.Text = paraVal.ToString();

                                this.LabelUnitNameText.Text = objAttribute.UnitName;
                            }
                        }

                        // 该属性为Point类型的UI处理
                        else if (property.PropertyType.FullName == "System.Windows.Point" || property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D")
                        {
                            this.editPanelControl.Width = 250;
                            this.editPanelControl.Height = 370;
                            this.editPanelControl.Dock = DockStyle.Top;
                            this.editPanelControl.Visible = true;
                            this.EditLabel.Text = infoText;
                            this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;
                            this.LabelUnitNameY.Visible = this.SpPointY.Visible = true;
                            this.labelControlX.Visible = true;
                            this.labelControlY.Visible = true;

                            if (paraVal == null)
                            {
                                // string类型
                                this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                this.LabelUnitNameY.Text = this.SpPointY.Text = string.Empty;
                            }
                            else
                            {
                                string[] strPointArray = paraVal.ToString().Split(',');

                                this.SpPointX.Text = strPointArray[0];
                                this.SpPointY.Text = strPointArray[1];

                                this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objAttribute.UnitName;
                            }
                        }
                        else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D")
                        {
                            this.editPanelControl.Width = 250;
                            this.editPanelControl.Height = 370;
                            this.editPanelControl.Dock = DockStyle.Top;
                            this.editPanelControl.Visible = true;
                            this.EditLabel.Text = infoText;
                            this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;
                            this.LabelUnitNameY.Visible = this.SpPointY.Visible = true;
                            this.LabelUnitNameZ.Visible = this.SpPointZ.Visible = true;
                            this.labelControlX.Visible = true;
                            this.labelControlY.Visible = true;
                            this.labelControlZ.Visible = true;


                            if (paraVal == null)
                            {
                                //string类型
                                this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                this.LabelUnitNameY.Text = this.SpPointY.Text = string.Empty;
                                this.LabelUnitNameZ.Text = this.SpPointZ.Text = string.Empty;
                            }
                            else
                            {
                                string[] strPointArray = paraVal.ToString().Split(',');

                                this.SpPointX.Text = strPointArray[0];
                                this.SpPointY.Text = strPointArray[1];
                                this.SpPointZ.Text = strPointArray[2];

                                this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objAttribute.UnitName;
                            }
                        }
                        else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D[]")
                        {
                            //集合对象的父节点,不显示
                            break;
                        }
                        else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D[]")
                        {
                            //集合对象的父节点,不显示
                            break;
                        }
                        //else if(property.PropertyType == typeof(double[]))
                        //{
                        //    //集合对象的父节点,不显示
                        //    break;
                        //}
                        // 该属性为数字类型的UI处理
                        else if (true)
                        {
                            this.editPanelControl.Width = 250;
                            this.editPanelControl.Height = 370;
                            this.editPanelControl.Dock = DockStyle.Top;
                            this.editPanelControl.Visible = true;
                            this.EditLabel.Text = infoText;
                            this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;

                            if (paraVal == null)
                            {
                                // string类型
                                this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                            }
                            else
                            {
                                this.SpPointX.Text = paraVal.ToString();

                                this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objAttribute.UnitName;
                            }
                        }
                    }
                }

                //数组：由于对象反射，数组的子节点属性不在原对象中，需要单独额外判断
                //当前选中的节点
                TreeListNode selectedNode = this.treeProgramsArgsList.FocusedNode;
                //当前选中的节点的父节点
                TreeListNode parentNodes = selectedNode.ParentNode;

                //如果没有父节点，该节点为数组的根节点，不需要UI操作
                if (parentNodes != null)
                {
                    //获取数组父节点的显示文本
                    string paramValue = parentNodes.GetValue("Paramemter").ToString();

                    //再次反射该对象
                    PropertyInfo[] prop = this.type.GetProperties();
                    foreach (PropertyInfo pro in prop)
                    {
                        TreeProgramListArgsAttribute objattrbute = null;
                        string showtext = null;

                        object[] objs = pro.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                        if (objs != null && objs.Length > 0)
                        {
                            //如果有自定义注解，获取注解对象和注解中的显示文本
                            objattrbute = (TreeProgramListArgsAttribute)objs[0];
                            showtext = objattrbute.ShowText;
                        }

                        //如果对象的属性的名称为界面的UI文本 或 属性的显示文本注解为界面的UI文本
                        if (pro.Name == paramValue || (showtext != null && showtext.Equals(paramValue)))
                        {
                            this.SetControlEnable(objattrbute);

                            if (pro.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D[]")
                            {
                                this.editPanelControl.Width = 250;
                                this.editPanelControl.Height = 370;
                                this.editPanelControl.Dock = DockStyle.Top;
                                this.editPanelControl.Visible = true;
                                this.EditLabel.Text = infoText;
                                this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;
                                this.LabelUnitNameY.Visible = this.SpPointY.Visible = true;
                                this.LabelUnitNameZ.Visible = this.SpPointZ.Visible = true;
                                this.labelControlX.Visible = true;
                                this.labelControlY.Visible = true;
                                this.labelControlZ.Visible = true;


                                if (paraVal == null)
                                {
                                    this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                    this.LabelUnitNameY.Text = this.SpPointY.Text = string.Empty;
                                    this.LabelUnitNameZ.Text = this.SpPointZ.Text = string.Empty;
                                }
                                else
                                {
                                    string[] strPointArray = paraVal.ToString().Split(',');

                                    this.SpPointX.Text = strPointArray[0];
                                    this.SpPointY.Text = strPointArray[1];
                                    this.SpPointZ.Text = strPointArray[2];

                                    this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objattrbute.UnitName;
                                }
                            }
                            else if (pro.PropertyType == typeof(ListEx<AKRSPoint3D>))
                            {
                                this.editPanelControl.Width = 250;
                                this.editPanelControl.Height = 370;
                                this.editPanelControl.Dock = DockStyle.Top;
                                this.editPanelControl.Visible = true;
                                this.EditLabel.Text = infoText;
                                this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;
                                this.LabelUnitNameY.Visible = this.SpPointY.Visible = true;
                                this.LabelUnitNameZ.Visible = this.SpPointZ.Visible = true;
                                this.labelControlX.Visible = true;
                                this.labelControlY.Visible = true;
                                this.labelControlZ.Visible = true;


                                if (paraVal == null)
                                {
                                    this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                    this.LabelUnitNameY.Text = this.SpPointY.Text = string.Empty;
                                    this.LabelUnitNameZ.Text = this.SpPointZ.Text = string.Empty;
                                }
                                else
                                {
                                    string[] strPointArray = paraVal.ToString().Split(',');

                                    this.SpPointX.Text = strPointArray[0];
                                    this.SpPointY.Text = strPointArray[1];
                                    this.SpPointZ.Text = strPointArray[2];

                                    this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objattrbute.UnitName;
                                }
                            }
                            else if (pro.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D[]")
                            {
                                this.editPanelControl.Width = 250;
                                this.editPanelControl.Height = 370;
                                this.editPanelControl.Dock = DockStyle.Top;
                                this.editPanelControl.Visible = true;
                                this.EditLabel.Text = infoText;
                                this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;
                                this.LabelUnitNameY.Visible = this.SpPointY.Visible = true;
                                this.labelControlX.Visible = true;
                                this.labelControlY.Visible = true;


                                if (paraVal == null)
                                {
                                    this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                    this.LabelUnitNameY.Text = this.SpPointY.Text = string.Empty;
                                }
                                else
                                {
                                    string[] strPointArray = paraVal.ToString().Split(',');

                                    this.SpPointX.Text = strPointArray[0];
                                    this.SpPointY.Text = strPointArray[1];

                                    this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objattrbute.UnitName;
                                }
                            }
                            else if (pro.PropertyType == typeof(double[]))
                            {
                                this.editPanelControl.Width = 250;
                                this.editPanelControl.Height = 370;
                                this.editPanelControl.Dock = DockStyle.Top;
                                this.editPanelControl.Visible = true;
                                this.EditLabel.Text = infoText;
                                this.LabelUnitNameX.Visible = this.SpPointX.Visible = true;

                                if (paraVal == null)
                                {
                                    this.LabelUnitNameX.Text = this.SpPointX.Text = string.Empty;
                                }
                                else
                                {
                                    string[] strPointArray = paraVal.ToString().Split(',');

                                    this.SpPointX.Text = strPointArray[0];

                                    this.LabelUnitNameX.Text = this.LabelUnitNameY.Text = this.LabelUnitNameZ.Text = objattrbute.UnitName;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 输入文本修改参数的leave事件，鼠标焦点离开时，通过反射注入对象的值，同时修改所选的Nodes的UI效果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LeaveCheck(object sender, EventArgs e)
        {
            object paraVal = string.Empty;

            TextEdit textEdit = sender as TextEdit;

            //判断哪个控件触发的事件，并获取值
            if (textEdit.Name == "ParamValueTextEdit")
            {
                paraVal = this.ParamValueTextEdit.EditValue;
            }
            else if (textEdit.Name == "SpPointX")
            {
                paraVal = this.SpPointX.EditValue;

            }
            else if (textEdit.Name == "SpPointY")
            {
                paraVal = this.SpPointY.EditValue;
            }
            else if (textEdit.Name == "SpPointZ")
            {
                paraVal = this.SpPointZ.EditValue;
            }

            //获取焦点的节点
            TreeListNode selectedNode = this.treeProgramsArgsList.FocusedNode;
            //获取该节点值
            string selectTreeProgramsArgsListParaName = selectedNode.GetValue("Paramemter").ToString();

            //为空不做处理
            if (paraVal.Equals(string.Empty))
            {
            }
            else
            {
                PropertyInfo[] properties = this.type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    //注解对象
                    TreeProgramListArgsAttribute objAttribute = null;
                    //显示文本
                    string showText = null;

                    object[] objs = property.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                    if (objs != null && objs.Length > 0)
                    {
                        objAttribute = (TreeProgramListArgsAttribute)objs[0];
                        showText = objAttribute.ShowText;
                    }

                    //如果该对象的该属性名等于该节点的显示文本，或该对象该属性注解显示文本等于该节点的显示文本
                    if (property.Name.Equals(selectTreeProgramsArgsListParaName) || (showText != null && showText.Equals(selectTreeProgramsArgsListParaName)))
                    
                    {
                        if (property.PropertyType.FullName == "System.Int32")
                        {
                            if (!int.TryParse(paraVal.ToString(), out int val))
                            {
                                AKRSXtraMessageBox.Show("Input format error, please enter an integer");
                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();
                            }
                            else if (val < objAttribute.MinValue || val > objAttribute.MaxValue)
                            {
                                AKRSXtraMessageBox.Show($"Input value error. value range：{objAttribute.MinValue} - {objAttribute.MaxValue} ");

                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();
                            }
                            else
                            {
                                property.SetValue(this.currentObj, val);
                                selectedNode.SetValue("Value", paraVal.ToString());
                            }
                        }
                        else if (property.PropertyType.FullName == "System.Single")
                        {
                            if (!Single.TryParse(paraVal.ToString(), out Single val))
                            {
                                AKRSXtraMessageBox.Show("Input format error, please enter a number");
                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();
                            }
                            else if (val < objAttribute.MinValue || val > objAttribute.MaxValue)
                            {
                                AKRSXtraMessageBox.Show($"Input value error. value range: {objAttribute.MinValue} - {objAttribute.MaxValue} ");

                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();

                            }
                            else
                            {
                                property.SetValue(this.currentObj, val);
                                selectedNode.SetValue("Value", paraVal.ToString());
                            }
                        }
                        else if (property.PropertyType.FullName == "System.Double")
                        {
                            if (!double.TryParse(paraVal.ToString(), out double val))
                            {
                                AKRSXtraMessageBox.Show("Input format error, please enter a number");
                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();
                            }
                            else if (val < objAttribute.MinValue || val > objAttribute.MaxValue)
                            {
                                AKRSXtraMessageBox.Show($"Input value error. value range: {objAttribute.MinValue} - {objAttribute.MaxValue} ");

                                this.SpPointX.Text = property.GetValue(this.currentObj).ToString();

                            }
                            else
                            {
                                property.SetValue(this.currentObj, val);
                                selectedNode.SetValue("Value", paraVal.ToString());
                            }
                        }
                        else if (property.PropertyType.FullName == "System.String")
                        {
                            if (paraVal == null)
                            {
                                AKRSXtraMessageBox.Show("Input format error, please enter content");
                                this.ParamValueTextEdit.Text = property.GetValue(this.currentObj).ToString();
                            }
                            else
                            {
                                property.SetValue(this.currentObj, paraVal.ToString());
                                selectedNode.SetValue("Value", paraVal.ToString());
                            }
                        }
                        else if (property.PropertyType.FullName == "System.Windows.Point"
                                 || property.PropertyType.FullName ==
                                 "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D")
                        {
                            try
                            {
                                if (property.PropertyType.FullName == "System.Windows.Point")
                                {
                                    System.Windows.Point point = new System.Windows.Point(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue));
                                    property.SetValue(this.currentObj, point);
                                    selectedNode.SetValue("Value", point.ToString());
                                }
                                else
                                {
                                    AKRSPoint2D point = new AKRSPoint2D(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue));
                                    property.SetValue(this.currentObj, point);
                                    selectedNode.SetValue("Value", point.ToString());
                                }
                            }
                            catch (Exception)
                            {
                                AKRSXtraMessageBox.Show("Input format error");
                                string[] strPointArray = property.GetValue(this.currentObj).ToString().Split(',');
                                this.SpPointX.Text = strPointArray[0].ToString();
                                this.SpPointY.Text = strPointArray[1].ToString();
                            }
                        }
                        else if (property.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D")
                        {
                            try
                            {
                                AKRSPoint3D point = new AKRSPoint3D(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue), Convert.ToDouble(this.SpPointZ.EditValue));
                                property.SetValue(this.currentObj, point);
                                selectedNode.SetValue("Value", point.ToString());
                            }
                            catch (Exception)
                            {
                                AKRSXtraMessageBox.Show("Input format error");
                                string[] strPointArray = property.GetValue(this.currentObj).ToString().Split(',');
                                this.SpPointX.Text = strPointArray[0].ToString();
                                this.SpPointY.Text = strPointArray[1].ToString();
                                this.SpPointZ.Text = strPointArray[2].ToString();
                            }
                        }
                    }
                }

                //数组：由于对象反射，数组的子节点属性不在原对象中，需要单独额外判断
                //当前选中的节点的父节点           
                TreeListNode parentNodes = selectedNode.ParentNode;
                if (parentNodes != null)
                {
                    string paramValue = parentNodes.GetValue("Paramemter").ToString();
                    PropertyInfo[] prop = this.type.GetProperties();
                    foreach (PropertyInfo pro in prop)
                    {
                        //注解对象
                        TreeProgramListArgsAttribute objattrbute = null;
                        //显示文本
                        string showtext = null;

                        object[] objs = pro.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                        if (objs != null && objs.Length > 0)
                        {
                            objattrbute = (TreeProgramListArgsAttribute)objs[0];
                            showtext = objattrbute.ShowText;
                        }

                        //如果该对象的该属性等于显示文本 或 该对象的该属性注解显示文本等于UI的显示文本
                        if (pro.Name == paramValue || (showtext != null && showtext.Equals(paramValue)))
                        {
                            if (pro.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint3D[]")
                            {
                                try
                                {
                                    //当前要改变的值
                                    AKRSPoint3D point = new AKRSPoint3D(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue), Convert.ToDouble(this.SpPointZ.EditValue));
                                    //提前修改界面UI
                                    selectedNode.SetValue("Value", point.ToString());

                                    //获取数组根节点的显示文本
                                    string paramVal = parentNodes.GetValue("Paramemter").ToString();

                                    //子节点个数
                                    List<TreeListNode> childNodes = new List<TreeListNode>();

                                    foreach (TreeListNode node in parentNodes.Nodes)
                                    {
                                        childNodes.Add(node);
                                    }

                                    //新的集合
                                    AKRSPoint3D[] point3Ds = new AKRSPoint3D[childNodes.Count];

                                    //再次循环
                                    PropertyInfo[] propertys = this.type.GetProperties();
                                    foreach (PropertyInfo p in propertys)
                                    {
                                        //找注解
                                        object[] objs1 = p.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);
                                        string showText1 = "";
                                        TreeProgramListArgsAttribute objAttribute1 = null;
                                        if (objs1 != null && objs1.Length > 0)
                                        {
                                            objAttribute1 = (TreeProgramListArgsAttribute)objs1[0];
                                            showText1 = objAttribute1.ShowText;
                                        }

                                        //该对象属性名等与集合根节点文本 或  UI显示文本等于该对象的注解的显示文本
                                        if (p.Name.Equals(paramVal) || (showText1 != null && showText1.Equals(paramVal)))
                                        {
                                            point3Ds = (AKRSPoint3D[])p.GetValue(this.currentObj);

                                            for (int i = 0; i < childNodes.Count(); i++)
                                            {
                                                TreeListNode treeListNode = childNodes[i];
                                                string value = "0,0,0";
                                                if (treeListNode.GetValue("Value") != null)
                                                {
                                                    value = (string)treeListNode.GetValue("Value").ToString();
                                                }
                                                string[] strPointArray = value.Split(',');

                                                //新的集合，通过UI的值遍历赋值
                                                point3Ds[i] = new AKRSPoint3D(Convert.ToDouble(strPointArray[0]), Convert.ToDouble(strPointArray[1]), Convert.ToDouble(strPointArray[2]));
                                            }
                                            //将该对象旧的集合用新集合替代
                                            p.SetValue(this.currentObj, point3Ds);
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show(ex.ToString());
                                }
                            }
                            else if (pro.PropertyType == typeof(ListEx<AKRSPoint3D>))
                            {
                                try
                                {
                                    //当前要改变的值
                                    AKRSPoint3D point = new AKRSPoint3D(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue), Convert.ToDouble(this.SpPointZ.EditValue));
                                    //提前修改界面UI
                                    selectedNode.SetValue("Value", point.ToString());

                                    //获取数组根节点的显示文本
                                    string paramVal = parentNodes.GetValue("Paramemter").ToString();

                                    //子节点个数
                                    List<TreeListNode> childNodes = new List<TreeListNode>();

                                    foreach (TreeListNode node in parentNodes.Nodes)
                                    {
                                        childNodes.Add(node);
                                    }

                                    //新的集合
                                    AKRSPoint3D[] point3Ds = new AKRSPoint3D[childNodes.Count];

                                    //再次循环
                                    PropertyInfo[] propertys = this.type.GetProperties();
                                    foreach (PropertyInfo p in propertys)
                                    {
                                        //找注解
                                        object[] objs1 = p.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);
                                        string showText1 = "";
                                        TreeProgramListArgsAttribute objAttribute1 = null;
                                        if (objs1 != null && objs1.Length > 0)
                                        {
                                            objAttribute1 = (TreeProgramListArgsAttribute)objs1[0];
                                            showText1 = objAttribute1.ShowText;
                                        }

                                        //该对象属性名等与集合根节点文本 或  UI显示文本等于该对象的注解的显示文本
                                        if (p.Name.Equals(paramVal) || (showText1 != null && showText1.Equals(paramVal)))
                                        {
                                            point3Ds = ((ListEx<AKRSPoint3D>)p.GetValue(this.currentObj)).ToArray();

                                            for (int i = 0; i < childNodes.Count(); i++)
                                            {
                                                TreeListNode treeListNode = childNodes[i];
                                                string value = "0,0,0";
                                                if (treeListNode.GetValue("Value") != null)
                                                {
                                                    value = (string)treeListNode.GetValue("Value").ToString();
                                                }
                                                string[] strPointArray = value.Split(',');

                                                //新的集合，通过UI的值遍历赋值
                                                point3Ds[i] = new AKRSPoint3D(Convert.ToDouble(strPointArray[0]), Convert.ToDouble(strPointArray[1]), Convert.ToDouble(strPointArray[2]));
                                            }

                                            ListEx<AKRSPoint3D> currentList =
                                                p.GetValue(this.currentObj) as ListEx<AKRSPoint3D>;
                                            ListEx<AKRSPoint3D>.Assign(currentList, point3Ds);
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show(ex.ToString());
                                }
                            }
                            else if (pro.PropertyType.FullName == "AKRS.Galaxy2.Infrastructure.CommonModel.AKRSPoint2D[]")
                            {
                                try
                                {
                                    string paramVal = parentNodes.GetValue("Paramemter").ToString();

                                    AKRSPoint2D point = new AKRSPoint2D(Convert.ToDouble(this.SpPointX.EditValue), Convert.ToDouble(this.SpPointY.EditValue));
                                    selectedNode.SetValue("Value", point.ToString());

                                    List<TreeListNode> childNodes = new List<TreeListNode>();

                                    foreach (TreeListNode node in parentNodes.Nodes)
                                    {
                                        childNodes.Add(node);
                                    }

                                    AKRSPoint2D[] point2Ds = new AKRSPoint2D[childNodes.Count];

                                    PropertyInfo[] propertys = this.type.GetProperties();
                                    foreach (PropertyInfo p in propertys)
                                    {

                                        object[] objs1 = p.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);
                                        string showText1 = "";
                                        TreeProgramListArgsAttribute objAttribute1 = null;
                                        if (objs1 != null && objs1.Length > 0)
                                        {
                                            objAttribute1 = (TreeProgramListArgsAttribute)objs1[0];
                                            showText1 = objAttribute1.ShowText;
                                        }

                                        if (p.Name.Equals(paramVal) || (showText1 != null && showText1.Equals(paramVal)))
                                        {
                                            point2Ds = (AKRSPoint2D[])p.GetValue(this.currentObj);

                                            for (int i = 0; i < childNodes.Count(); i++)
                                            {
                                                TreeListNode treeListNode = childNodes[i];
                                                string value = "0,0";
                                                if (treeListNode.GetValue("Value") != null)
                                                {
                                                    value = (string)treeListNode.GetValue("Value").ToString();
                                                }
                                                string[] strPointArray = value.Split(',');

                                                point2Ds[i] = new AKRSPoint2D(Convert.ToDouble(strPointArray[0]), Convert.ToDouble(strPointArray[1]));
                                            }

                                            p.SetValue(this.currentObj, point2Ds);
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show(ex.ToString());
                                }
                            }
                            else if (pro.PropertyType == typeof(double[]))
                            {
                                try
                                {
                                    string paramVal = parentNodes.GetValue("Paramemter").ToString();

                                    double point = (double)this.SpPointX.Value;
                                    selectedNode.SetValue("Value", point.ToString());

                                    List<TreeListNode> childNodes = new List<TreeListNode>();

                                    foreach (TreeListNode node in parentNodes.Nodes)
                                    {
                                        childNodes.Add(node);
                                    }

                                    double[] point2Ds = new double[childNodes.Count];

                                    PropertyInfo[] propertys = this.type.GetProperties();
                                    foreach (PropertyInfo p in propertys)
                                    {

                                        object[] objs1 = p.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);
                                        string showText1 = "";
                                        TreeProgramListArgsAttribute objAttribute1 = null;
                                        if (objs1 != null && objs1.Length > 0)
                                        {
                                            objAttribute1 = (TreeProgramListArgsAttribute)objs1[0];
                                            showText1 = objAttribute1.ShowText;
                                        }

                                        if (p.Name.Equals(paramVal) || (showText1 != null && showText1.Equals(paramVal)))
                                        {
                                            point2Ds = (double[])p.GetValue(this.currentObj);

                                            for (int i = 0; i < childNodes.Count(); i++)
                                            {
                                                TreeListNode treeListNode = childNodes[i];
                                                if (treeListNode.GetValue("Value") != null)
                                                {
                                                    point2Ds[i] = (double)treeListNode.GetValue("Value").ObjToDecimal();
                                                }
                                            }

                                            p.SetValue(this.currentObj, point2Ds);
                                            break;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AKRSXtraMessageBox.Show(ex.ToString());
                                }

                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 用于枚举和Bool的选择界面，通过反射注入对象的值，同时修改所选的Nodes的UI效果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void radioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            TreeListNode selectedNode = this.treeProgramsArgsList.FocusedNode;
            string selectTreeProgramsArgsListParaName = selectedNode.GetValue("Paramemter").ToString();
            RadioGroup rg = sender as RadioGroup;
            string text = rg.Text;

            PropertyInfo[] properties = this.type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //注解对象
                TreeProgramListArgsAttribute objAttribute = null;
                //显示文本
                string showText = null;

                object[] objs = property.GetCustomAttributes(typeof(TreeProgramListArgsAttribute), true);

                if (objs != null && objs.Length > 0)
                {
                    objAttribute = (TreeProgramListArgsAttribute)objs[0];
                    showText = objAttribute.ShowText;
                }

                //如果该对象的该属性名等于该节点的显示文本，或该对象该属性注解显示文本等于该节点的显示文本
                if (property.Name.Equals(selectTreeProgramsArgsListParaName) || (showText != null && showText.Equals(selectTreeProgramsArgsListParaName)))
                {
                    if (property.PropertyType.BaseType.FullName == "System.Enum")
                    {
                        object val = Enum.Parse(property.PropertyType, text);
                        property.SetValue(this.currentObj, val);
                        selectedNode.SetValue("Value", val.ToString());
                        break;
                    }

                    if (property.PropertyType.FullName == "System.Boolean")
                    {
                        bool b = true;
                        if (text.Equals("开"))
                        {
                            b = true;
                        }

                        if (text.Equals("关"))
                        {
                            b = false;
                        }

                        property.SetValue(this.currentObj, b);
                        selectedNode.SetValue("Value", text);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 将界面的右边界控件全部隐藏掉
        /// </summary>
        public void SetControlVisibleFalse()
        {
            this.editPanelControl.Visible = false;
            this.radioGrouppanelControl.Visible = false;
            this.radioGroup1.Properties.Items.Clear();

            this.LabelUnitNameText.Visible = this.ParamValueTextEdit.Visible = false;

            this.LabelUnitNameX.Visible = this.SpPointX.Visible = false;
            this.LabelUnitNameY.Visible = this.SpPointY.Visible = false;
            this.LabelUnitNameZ.Visible = this.SpPointZ.Visible = false;

            this.labelControlX.Visible = false;
            this.labelControlY.Visible = false;
            this.labelControlZ.Visible = false;
        }

        /// <summary>
        /// 将界面的右边控件enable属性修改
        /// </summary>
        /// <param name=""></param>
        public void SetControlEnable(bool flag) 
        {
            this.editPanelControl.Enabled = flag;
            this.radioGroup1.Enabled = flag;
        }

        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void EditKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                this.LeaveCheck(sender, e);
            }
        }

        /// <summary>
        /// SetControlEnable
        /// </summary>
        /// <param name="objAttribute">objAttribute</param>
        private void SetControlEnable(TreeProgramListArgsAttribute objAttribute)
        {
            if (Machine.GetInstance().IsWorking() || !objAttribute.AllowUserChange)
            {
                this.SetControlEnable(false);
            }
            else
            {
                if (objAttribute.RoleLevel == 0)
                {
                    this.SetControlEnable(true);
                }
                else
                {
                    if (Machine.GetInstance().CurrentRoleLevel > 0 && Machine.GetInstance().CurrentRoleLevel <= objAttribute.RoleLevel)
                    {
                        this.SetControlEnable(true);
                    }
                    else
                    {
                        this.SetControlEnable(false);
                    }
                }
            }
        }

        /// <summary>
        /// UcProgramArgs_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void UcProgramArgs_Load(object sender, EventArgs e)
        {
            this.treeGroupList.Appearance.FocusedCell.BackColor = Color.DarkGray;
        }

        private void TxtSearchName_EditValueChanged(object sender, EventArgs e)
        {
            this.Search();
        }

        public void Search()
        {
            this.treeProgramsArgsList.FindFilterText = this.searchStr = this.TxtSearchName.Text;
            this.treeProgramsArgsList.ActiveFilterString = $"[Paramemter] LIKE '%{this.searchStr}%'";
            this.currentIndex = 0;
            this.searchResult = string.IsNullOrEmpty(this.searchStr) ? new List<ObjAndNodesInfo>() : this.TreeProgramsArgsList.Filter(a => this.IsExistParams(a.CurrentObj)).OrderBy(a => a.GroupTreeCurrentText).ToList();
            this.LbPromit.Text = $@"{this.currentIndex} / {this.searchResult.Count}";
        }

        /// <summary>
        /// 是否存在参数
        /// </summary>
        /// <returns></returns>
        private bool IsExistParams(object instance)
        {
            if (instance == null)
            {
                return false;
            }
            List<ParameterDetailModel> nodes = new();
            foreach (PropertyInfo childProp in instance.GetType().GetProperties())
            {
                // 过滤掉没有（TreeProgramListArgsAttribute）的属性
                if (!childProp.IsDefined(typeof(TreeProgramListArgsAttribute), false))
                {
                    continue;
                }

                // 取出特性实例
                TreeProgramListArgsAttribute childAttr =
                    childProp.GetCustomAttribute<TreeProgramListArgsAttribute>();

                if (childAttr.ShowText.Contains(this.searchStr))
                {
                    return true;
                }
            }

            return false;
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (this.searchResult == null || this.searchResult.Count == 0)
            {
                AKRSXtraMessageBox.Show("未搜索到");
                return;
            }

            if (this.currentIndex > this.searchResult.Count - 1)
            {
                this.currentIndex = 0;
            }

            ObjAndNodesInfo info = this.searchResult[this.currentIndex];
            TreeListNode temp = this.treeGroupList.FindNode(
                a => a.GetDisplayText(this.treeListColum1) == info.GroupTreeCurrentText.GetDescription()
                     & a.ParentNode != null);
            this.Invoke(() => this.treeGroupList.SetFocusedNode(temp));
            temp = this.treeDataSetList.FindNode(a => a.GetDisplayText(this.treeListColumn1) == info.TreeDataSetListNode);
            this.Invoke(() => this.treeDataSetList.SetFocusedNode(temp));
            this.treeProgramsArgsList.ExpandAll();
            this.LbPromit.Text = $@"{this.currentIndex + 1} / {this.searchResult.Count}";

            this.currentIndex++;
        }
    }
}







