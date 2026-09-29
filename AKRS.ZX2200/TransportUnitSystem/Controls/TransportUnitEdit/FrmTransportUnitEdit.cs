using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using DevExpress.XtraTreeList.Nodes;
using LanguageExt.UnitsOfMeasure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using DevExpress.ExpressApp.Utils;

    /// <summary>
    /// 编辑模板
    /// </summary>
    public partial class FrmTransportUnitEdit : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        private ProductConfiguration ProductConfiguration => ProductConfiguration.GetInstance();

        /// <summary>
        /// 配置控件
        /// </summary>
        private UcObjectFunctionEdit ucObjectFunctionEdit;

        /// <summary>
        /// 配置控件
        /// </summary>
        private UcTransportUnitView ucTransportUnitView;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmTransportUnitEdit()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Init()
        {
            ProductConfiguration.OppositeSexConfiguration.InitOppositeSexConfigIndex();

            List<OppositeSex> list = ProductConfiguration.OppositeSexConfiguration.GetOppositeSexConfigs();

            this.TreeTransportUnit.DataSource = list;

            // 清除可能存在的默认列
            this.TreeTransportUnit.Columns.Clear();

            this.TreeTransportUnit.KeyFieldName = "Id";
            this.TreeTransportUnit.ParentFieldName = "ParentId";
            
            // 创建并配置你希望显示的列
            TreeListColumn nameColumn = new TreeListColumn();
            nameColumn.FieldName = "Name"; // 绑定到数据源的"Name"字段
            nameColumn.Caption = "名称"; // 设置列标题
            nameColumn.VisibleIndex = 0; // 设置显示顺序，0 表示第一列，它将作为树形结构列
            this.TreeTransportUnit.Columns.Add(nameColumn);

            this.TreeTransportUnit.Columns[0].Caption = "载具单元";

            this.TreeTransportUnit.Appearance.FocusedCell.BackColor = System.Drawing.Color.LightSteelBlue;
            this.TreeTransportUnit.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.SteelBlue;
            this.TreeTransportUnit.Appearance.FocusedCell.Options.UseBackColor = true;
            
            this.TreeTransportUnit.SelectImageList = this.imageCollection1;
            
            this.RefreshImage();

            this.TreeTransportUnit.ExpandAll();
        }

        /// <summary>
        /// 树节点发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TreeTransportUnit_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            OppositeSex oppositeSexConfig = (OppositeSex)this.TreeTransportUnit.GetFocusedRow();

            this.BtAdd.Enabled = oppositeSexConfig.MatterTypeEnum != EntityTypeEnum.BondPosition;

            this.BtLock.Text = oppositeSexConfig.IsLock ? "解锁" : "加锁";

            this.BtDelete.Enabled = !oppositeSexConfig.IsLock;

            this.ucObjectFunctionEdit.Init(oppositeSexConfig);
            this.ucTransportUnitView.SelectOppositeSex = oppositeSexConfig;
            this.ucTransportUnitView.Init();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAdd_Click(object sender, EventArgs e)
        {
            FrmAddObject frmAddObject = new FrmAddObject();
            frmAddObject.SelectOppositeSexConfig = (OppositeSex)this.TreeTransportUnit.GetFocusedRow();
            DialogResult dialogResult = frmAddObject.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                frmAddObject.Dispose();
                return;
            }
            
            frmAddObject.Dispose();
            this.Init();

            ProductConfiguration.GetInstance().Save();

            TreeListNode treeNode = this.TreeTransportUnit.FindNodeByKeyID(frmAddObject.OppositeSexConfig.Id);
            this.TreeTransportUnit.SetFocusedNode(treeNode);
            this.ucObjectFunctionEdit.Init(frmAddObject.OppositeSexConfig);
            this.ucTransportUnitView.SelectOppositeSex = frmAddObject.OppositeSexConfig;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDelete_Click(object sender, EventArgs e)
        {
            OppositeSex oppositeSexConfig = (OppositeSex)this.TreeTransportUnit.GetFocusedRow();

            if (oppositeSexConfig == null)
            {
                return;
            }

            if (oppositeSexConfig.IsLock)
            {
                AKRSXtraMessageBox.Show("需要解锁才能删除");
                return;
            }

            DialogResult dialogResult = AKRSXtraMessageBox.Show($"确定要删除{oppositeSexConfig.Name}?", "提示", MessageBoxButtons.OKCancel);

            if (dialogResult != DialogResult.OK)
            {
                return;
            }

            if (oppositeSexConfig != null)
            {
                ProductConfiguration.OppositeSexConfiguration.RemoveOppositeSexConfig(oppositeSexConfig);
            }

            this.Init();

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCopy_Click(object sender, EventArgs e)
        {
            OppositeSex oppositeSexConfig = (OppositeSex)this.TreeTransportUnit.GetFocusedRow();

            if (oppositeSexConfig == null)
            {
                AKRSXtraMessageBox.Show("请先选择需要复制的单元");
                return;
            }

            FrmObjectCopy frmObjectCopy = new FrmObjectCopy(oppositeSexConfig);
            
            if (frmObjectCopy.ShowDialog() != DialogResult.OK)
            {
                frmObjectCopy.Dispose();
                return;
            }

            frmObjectCopy.Dispose();
            OppositeSex oppositeSexConfigUp = ProductConfiguration.OppositeSexConfiguration
                .GetOppositeSexConfigs().Find(it => it.Id == oppositeSexConfig.ParentId);

            oppositeSexConfigUp.DownConfigs.AddRange(frmObjectCopy.NewOppositeSexConfigs);

            foreach (OppositeSex oppositeSexConfigNew in frmObjectCopy.NewOppositeSexConfigs)
            {
                oppositeSexConfigNew.Name =
                    OppositeSex.GetDefaultName(oppositeSexConfig.Name);
            }
            
            this.Init();

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTransportUnitEdit_Load(object sender, EventArgs e)
        {
            this.ucObjectFunctionEdit = new UcObjectFunctionEdit(this.RefreshImage,this.BackAction) { Dock = DockStyle.Fill };
            this.PlFunction.Controls.Add(this.ucObjectFunctionEdit);
            this.ucTransportUnitView = new UcTransportUnitView(this.SelectOppositeSex, this.Init,this.ChangeTape) { Dock = DockStyle.Fill };
            this.xtraTabPage1.Controls.Add(this.ucTransportUnitView);
            this.Init();
        }

        /// <summary>
        /// 返回执行
        /// </summary>
        private void BackAction()
        {
            this.xtraTabControl1.SelectedTabPageIndex = 0;
        }

        /// <summary>
        /// 改变选择
        /// </summary>
        private void ChangeTape()
        {
            this.xtraTabControl1.SelectedTabPageIndex = 1;
        }

        /// <summary>
        /// 选中配置文件
        /// </summary>
        /// <param name="oppositeSexConfig">配置文件</param>
        private void SelectOppositeSex(OppositeSex oppositeSexConfig)
        {
            if (oppositeSexConfig == null)
            {
                return;
            }

            this.TreeTransportUnit.FocusedNodeChanged -= new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeTransportUnit_FocusedNodeChanged);
            TreeListNode treeNode = this.TreeTransportUnit.FindNodeByKeyID(oppositeSexConfig.Id);
            this.TreeTransportUnit.SetFocusedNode(treeNode);
            this.ucObjectFunctionEdit.Init(oppositeSexConfig);
            this.ucTransportUnitView.SelectOppositeSex = oppositeSexConfig;
            this.TreeTransportUnit.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeTransportUnit_FocusedNodeChanged);
        }

        /// <summary>
        /// 刷新图标
        /// </summary>
        private void RefreshImage()
        {
            foreach (TreeListNode treeListNode in this.TreeTransportUnit.GetNodeList())
            {
                OppositeSex oppositeSex = ProductConfiguration.GetInstance().OppositeSexConfiguration
                    .GetOppositeSexConfigs().Find(it => it.Name == treeListNode.GetDisplayText(0).ToString());

                if (oppositeSex != null)
                {
                    treeListNode.ImageIndex = oppositeSex.AssistantSuccess() ? 0 : 1;
                    treeListNode.SelectImageIndex = oppositeSex.AssistantSuccess() ? 0 : 1;
                }
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClear_Click(object sender, EventArgs e)
        {
            this.ProductConfiguration.OppositeSexConfiguration.OppositeSexConfigs.DownConfigs.Clear();
            this.ProductConfiguration.OppositeSexConfiguration.BaseConfigs.Clear();
            this.Init();

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLock_Click(object sender, EventArgs e)
        {
            OppositeSex oppositeSexConfig = (OppositeSex)this.TreeTransportUnit.GetFocusedRow();

            oppositeSexConfig.IsLock = !oppositeSexConfig.IsLock;

            this.BtLock.Text = oppositeSexConfig.IsLock ? "解锁" : "加锁";

            this.BtDelete.Enabled = !oppositeSexConfig.IsLock;
        }
    }
}