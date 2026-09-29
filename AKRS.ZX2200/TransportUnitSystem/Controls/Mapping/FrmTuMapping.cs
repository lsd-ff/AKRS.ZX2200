using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.XtraEditors;
    using System.Linq;

    /// <summary>
    /// 产品Mapping图
    /// </summary>
    public partial class FrmTuMapping : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="productConfig">配置文件</param>
        /// <param name="action">保存方法</param>
        public FrmTuMapping(ProductConfiguration productConfig, Action action)
        {
            this.InitializeComponent();
            this.productConfig = productConfig;
            this.transportUnit = new TransportUnit("Mapping图");
            this.actionSave = action;

            if (ProductConfiguration.GetInstance().TransportUnitConfig.LagerNumber)
            {
                this.mappingDraw = new MappingDraw(this.productConfig.OtherConfig.MatterProductInfoMax);
            }
            else
            {
                this.mappingDraw = new MappingDraw(this.productConfig.OtherConfig.MatterProductInfo);
            }

            this.TsmClear.Enabled = true;
            this.transportUnit.Refresh();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="transportUnit">传输单元对象</param>
        public FrmTuMapping(TransportUnit transportUnit)
        {
            this.InitializeComponent();
            this.transportUnit = transportUnit;
        }

        #region 参数

        /// <summary>
        /// 保存动作
        /// </summary>
        private readonly Action actionSave;

        /// <summary>
        /// 绘画工具
        /// </summary>
        private readonly MappingDraw mappingDraw;

        /// <summary>
        /// 被选中的sub集合
        /// </summary>
        private readonly List<Substrate> selectedSubstrateList = new List<Substrate>();

        /// <summary>
        /// 选中sub的索引
        /// </summary>
        private int selectSubstrateIndex;

        /// <summary>
        /// 双击之后选中的sub
        /// </summary>
        private Substrate SelectSubstrate =>
            this.transportUnit.Substrates.Find(it => it.Index == this.selectSubstrateIndex);

        /// <summary>
        /// 被选中的Mo集合
        /// </summary>
        private readonly List<Module> selectedModuleList = new List<Module>();

        /// <summary>
        /// 选中sub的索引
        /// </summary>
        private int selectModuleIndex;

        /// <summary>
        /// 双击之后选中的selectModule
        /// </summary>
        private Module SelectModule => this.SelectSubstrate.Modules.Find(it => it.Index == this.selectModuleIndex);

        /// <summary>
        /// 被选中的BondPosition集合
        /// </summary>
        private readonly List<BondPosition> bondPositions = new List<BondPosition>();

        /// <summary>
        /// 双击之后选中的selectModule
        /// </summary>
        private BondPosition selectBondPosition;

        /// <summary>
        /// 配置对象
        /// </summary>
        private readonly ProductConfiguration productConfig;

        /// <summary>
        /// 传输单元对象
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// 鼠标多选择时起始坐标
        /// </summary>
        private PointF startPointF;

        /// <summary>
        /// 是否多选
        /// </summary>
        private bool isSelectMore = false;

        /// <summary>
        /// 画笔
        /// </summary>
        private Graphics substrateGraphics;

        /// <summary>
        /// 选择的索引
        /// </summary>
        private int SelectIndex => this.GetSelectIndex();

        #endregion

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmTuMapping_Load(object sender, EventArgs e)
        {
            this.substrateGraphics = this.CreateGraphics();

            this.CmbBondPosition.Properties.Items.Add("All");

            foreach (SingleBondPositionConfig singleBondPositionConfig in this.productConfig.BondPositionConfig.SingleBpPositionConfigList)
            {
                this.CmbBondPosition.Properties.Items.Add(singleBondPositionConfig.Name);
            }

            this.CmbBondPosition.SelectedIndex = 0;

            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                //this.TSMTurnOn.Text = string.Empty;
                //this.TsmClear.Text = string.Empty;
                //this.TSMTurnOn.Enabled = false;
                //this.TsmClear.Enabled = false;

                this.contextMenuStrip1.Items.Remove(this.TSMTurnOn);
                this.contextMenuStrip1.Items.Remove(this.TsmClear);
            }

            this.LueWorkSortType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<WorkOrderEnum>();
            this.LueWorkSortType.EditValue = this.productConfig.SubstrateConfig.WorkOrderEnum;
            this.ChkAutoReturn.Checked = this.productConfig.SubstrateConfig.Arrangement == ArrangementEnum.Snake;
        }

        /// <summary>
        /// 绘画Mapping图
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnMapping_Paint(object sender, PaintEventArgs e)
        {
            this.mappingDraw.DrawEntities(
                e.Graphics,
                this.transportUnit.Substrates,
                this.selectedSubstrateList,
                this.PnSubstrateMapping);

            this.RefreshMatterIndex();
        }

        /// <summary>
        /// 获取选择焊点的索引
        /// </summary>
        /// <returns>索引</returns>
        private int GetSelectIndex()
        {
            if (this.CmbBondPosition.SelectedIndex == 0)
            {
                return -1;
            }
            else
            {
                SingleBondPositionConfig single = this.productConfig.BondPositionConfig.SingleBpPositionConfigList.Find(
                        it => it.Name == this.CmbBondPosition.SelectedItem.ToString());
                if (single != null)
                {
                    return this.productConfig.BondPositionConfig.SingleBpPositionConfigList.IndexOf(single) + 1;
                }
            }

            return -1;
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnMapping_MouseMove(object sender, MouseEventArgs e)
        {
            // 判断当前鼠标是否已经按下
            if (e.Button == MouseButtons.None)
            {
                return;
            }

            // ctrl + 鼠标右键不触发事件
            if (e.Button == MouseButtons.Left)
            {
                if ((int)Control.ModifierKeys == (int)Keys.Control)
                {
                    return;
                }
            }

            // 计算起始点和结束点的距离，太小则直接返回
            if (!this.mappingDraw.IsChooseEnable(this.startPointF, e.Location))
            {
                return;
            }

            this.isSelectMore = true;

            this.mappingDraw.MouseMove(
                this.PnMapping,
                this.startPointF,
                e.Location,
                this.transportUnit.Substrates,
                this.selectedSubstrateList);
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnMapping_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectSubstrate != null)
            {
                this.PnMapping.SelectedTabPageIndex = 1;
            }
        }

        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnMapping_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPointF = e.Location;

            Substrate substrate = this.mappingDraw.SelectEntity(
                e,
                this.substrateGraphics,
                this.selectedSubstrateList,
                this.transportUnit.Substrates,
                this.BackColor);

            if (substrate != null)
            {
                this.selectSubstrateIndex = substrate.Index;
            }

            this.PnMapping.Refresh();
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnMapping_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isSelectMore)
            {
                this.PnMapping.Refresh();
                Graphics g = this.PnModuleMapping.CreateGraphics();
                this.mappingDraw.DrawSelectSub(g, this.selectedSubstrateList);
                this.isSelectMore = false;
            }
        }

        /// <summary>
        /// module绘画
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnModuleMapping_Paint(object sender, PaintEventArgs e)
        {
            if (this.SelectSubstrate != null)
            {
                this.mappingDraw.DrawingFrame(e.Graphics, this.PnModuleMapping);
                this.mappingDraw.DrawingSub(e.Graphics, this.SelectSubstrate.Modules, this.PnModuleMapping);
                this.mappingDraw.DrawSelectSub(e.Graphics, this.selectedModuleList);
            }

            this.RefreshMatterIndex();
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnModuleMapping_MouseMove(object sender, MouseEventArgs e)
        {
            // 判断当前鼠标是否已经按下
            if (e.Button == MouseButtons.None)
            {
                return;
            }

            // ctrl + 鼠标右键不触发事件
            if (e.Button == MouseButtons.Left)
            {
                if ((int)Control.ModifierKeys == (int)Keys.Control)
                {
                    return;
                }
            }

            // 计算起始点和结束点的距离，太小则直接返回
            if (!this.mappingDraw.IsChooseEnable(this.startPointF, e.Location))
            {
                return;
            }

            this.isSelectMore = true;

            this.mappingDraw.MouseMove(
                this.PnMapping,
                this.startPointF,
                e.Location,
                this.SelectSubstrate.Modules,
                this.selectedModuleList);
        }

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnModuleMapping_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPointF = e.Location;

            Graphics g = this.PnModuleMapping.CreateGraphics();

            Module module = this.mappingDraw.SelectEntity(
                e,
                g,
                this.selectedModuleList,
                this.SelectSubstrate.Modules,
                this.BackColor);

            if (module != null)
            {
                this.selectModuleIndex = module.Index;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmTuMapping_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.actionSave();
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PnModuleMapping_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isSelectMore)
            {
                this.PnMapping.Refresh();
                Graphics g = this.PnModuleMapping.CreateGraphics();
                this.mappingDraw.DrawSelectSub(g, this.selectedModuleList);
                this.isSelectMore = false;
            }
        }

        /// <summary>
        /// 焊点绘画
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PnBondPositionMapping_Paint(object sender, PaintEventArgs e)
        {
            if (this.SelectModule != null)
            {
                this.mappingDraw.DrawingFrame(e.Graphics, this.PnBondPositionMapping);
                this.mappingDraw.DrawingSub(e.Graphics, this.SelectModule.BondPositions, this.PnBondPositionMapping);
                this.mappingDraw.DrawSelectSub(e.Graphics, this.bondPositions);
            }

            this.RefreshMatterIndex();
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnBondPositionMapping_MouseDown(object sender, MouseEventArgs e)
        {
            this.startPointF = e.Location;

            Graphics g = this.PnBondPositionMapping.CreateGraphics();

            BondPosition bondPosition = this.mappingDraw.SelectEntity(
                e,
                g,
                this.bondPositions,
                this.SelectModule.BondPositions,
                this.BackColor);

            if (bondPosition != null)
            {
                this.selectBondPosition = bondPosition;
                this.bondPositions.Add(bondPosition);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnBondPositionMapping_MouseMove(object sender, MouseEventArgs e)
        {
            // 判断当前鼠标是否已经按下
            if (e.Button == MouseButtons.None)
            {
                return;
            }

            // ctrl + 鼠标右键不触发事件
            if (e.Button == MouseButtons.Left)
            {
                if ((int)Control.ModifierKeys == (int)Keys.Control)
                {
                    return;
                }
            }

            // 计算起始点和结束点的距离，太小则直接返回
            if (!this.mappingDraw.IsChooseEnable(this.startPointF, e.Location))
            {
                return;
            }

            this.isSelectMore = true;

            this.mappingDraw.MouseMove(
                this.PnMapping,
                this.startPointF,
                e.Location,
                this.SelectModule.BondPositions,
                this.bondPositions);
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void PnBondPositionMapping_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isSelectMore)
            {
                this.PnMapping.Refresh();
                Graphics g = this.PnModuleMapping.CreateGraphics();
                this.mappingDraw.DrawSelectSub(g, this.bondPositions);
                this.isSelectMore = false;
            }
        }

        /// <summary>
        /// 双击module
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PnModuleMapping_DoubleClick(object sender, EventArgs e)
        {
            if (this.SelectModule != null)
            {
                this.PnMapping.SelectedTabPageIndex = 2;
            }
        }

        #region 右击按键

        /// <summary>
        /// 获取选中的sub集合
        /// </summary>
        /// <returns>结果</returns>
        private List<int> GetSelectSubs()
        {
            if (this.PnMapping.SelectedTabPageIndex == 0)
            {
                List<int> list = new List<int>();

                foreach (Substrate substrate in this.selectedSubstrateList)
                {
                    list.Add(substrate.Index);
                }

                this.selectedSubstrateList.Clear();

                return list;
            }
            else
            {
                return new List<int>() { this.selectSubstrateIndex };
            }
        }

        /// <summary>
        /// 获取选中的module集合
        /// </summary>
        /// <returns>结果</returns>
        private List<int> GetSelectModules()
        {
            if (this.PnMapping.SelectedTabPageIndex == 0)
            {
                List<int> list = new List<int>();

                for (int i = 0; i < this.productConfig.ModuleConfig.Count; i++)
                {
                    list.Add(i + 1);
                }

                return list;
            }
            else if (this.PnMapping.SelectedTabPageIndex == 1)
            {
                List<int> list = new List<int>();

                foreach (Module module in this.selectedModuleList)
                {
                    list.Add(module.Index);
                }

                this.selectedModuleList.Clear();

                return list;
            }
            else
            {
                return new List<int>() { this.selectModuleIndex };
            }
        }

        /// <summary>
        /// 获取选中的BondPosition集合
        /// </summary>
        /// <returns>结果</returns>
        private List<string> GetSelectBondPositions()
        {
            if (this.PnMapping.SelectedTabPageIndex == 2)
            {
                List<string> name = this.bondPositions.Select(t => t.Name).ToList();
                this.bondPositions.Clear();
                return name;
            }
            else
            {
                if (this.CmbBondPosition.SelectedIndex == 0)
                {
                    List<string> listName = new List<string>();
                    foreach (SingleBondPositionConfig singleBondPosition in this.productConfig.BondPositionConfig.SingleBpPositionConfigList)
                    {
                        listName.Add(singleBondPosition.Name);
                    }

                    return listName;
                }
                else
                {
                    // 返回选中的某一个焊点的集合
                    return new List<string>() { this.CmbBondPosition.SelectedItem.ToString() };
                }
            }
        }

        /// <summary>
        /// 制程打开
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void TsmEnable_Click(object sender, EventArgs e)
        {
            this.mappingDraw.ChangeMatterProduct(
                this.GetSelectSubs(),
                this.GetSelectModules(),
                this.GetSelectBondPositions(),
                MatterProductState.Enable);

            this.transportUnit = new TransportUnit("Mapping图");
            this.PnMapping.Refresh();
        }

        /// <summary>
        /// 屏蔽
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TsmDisable_Click(object sender, EventArgs e)
        {
            this.mappingDraw.ChangeMatterProduct(
                this.GetSelectSubs(),
                this.GetSelectModules(),
                this.GetSelectBondPositions(),
                MatterProductState.Disable);

            OtherConfig otherConfig = ProductConfiguration.GetInstance().OtherConfig;

            this.transportUnit = new TransportUnit("Mapping图");
            this.PnMapping.Refresh();
        }

        /// <summary>
        /// 打开系统1制程
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void TSMEnableInSystem1_Click(object sender, EventArgs e)
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                return;
            }

            this.mappingDraw.ChangeMatterProduct(
                this.GetSelectSubs(),
                this.GetSelectModules(),
                this.GetSelectBondPositions(),
                MatterProductState.EnableInSystem1);

            this.transportUnit = new TransportUnit("Mapping图");
            this.PnMapping.Refresh();
        }

        /// <summary>
        /// 打开系统2制程
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TsmClear_Click(object sender, EventArgs e)
        {
            this.mappingDraw.ChangeMatterProduct(
                this.GetSelectSubs(),
                this.GetSelectModules(),
                this.GetSelectBondPositions(),
                MatterProductState.EnableInSystem2);

            this.transportUnit = new TransportUnit("Mapping图");
            this.PnMapping.Refresh();
        }

        #endregion

        /// <summary>
        /// 清楚所有状态
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearAll_Click(object sender, EventArgs e)
        {
            this.ClearSelect();
            this.productConfig.OtherConfig.ClearInformation();
            this.transportUnit = new TransportUnit("Mapping图");
            this.PnMapping.Refresh();
        }

        /// <summary>
        /// 清楚所有选中
        /// </summary>
        private void ClearSelect()
        {
            this.selectedSubstrateList.Clear();
            this.selectedModuleList.Clear();
            this.bondPositions.Clear();
        }

        /// <summary>
        /// 刷新页面的索引
        /// </summary>
        private void RefreshMatterIndex()
        {
            if (this.PnMapping.SelectedTabPageIndex == 0)
            {
                this.LbSubstrateIndex.Text = string.Empty;
                this.LbModuleIndex.Text = string.Empty;
            }
            else if (this.PnMapping.SelectedTabPageIndex == 1)
            {
                this.LbSubstrateIndex.Text = this.selectSubstrateIndex.ToString();
                this.LbModuleIndex.Text = string.Empty;
            }

            if (this.PnMapping.SelectedTabPageIndex == 2)
            {
                this.LbSubstrateIndex.Text = this.selectSubstrateIndex.ToString();
                this.LbModuleIndex.Text = this.selectModuleIndex.ToString();
            }
        }

        /// <summary>
        /// Top页发生改变时的事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void PnMapping_SelectedPageChanging(object sender, DevExpress.XtraTab.TabPageChangingEventArgs e)
        {
            if (e.Page.TabIndex == 1)
            {
                if (this.SelectSubstrate == null)
                {
                    this.PnMapping.SelectedTabPageIndex = 0;

                    AKRSXtraMessageBox.Show("请先选择基板");
                    e.Cancel = true;
                }
                else
                {
                    this.selectedSubstrateList.Clear();
                    this.selectedModuleList.Clear();
                    this.bondPositions.Clear();
                }
            }
            else if (e.Page.TabIndex == 2)
            {
                if (this.SelectSubstrate == null || this.SelectModule == null)
                {
                    this.PnMapping.SelectedTabPageIndex = 1;

                    AKRSXtraMessageBox.Show("请先选择基岛");
                    e.Cancel = true;
                }
                else
                {
                    this.selectedSubstrateList.Clear();
                    this.selectedModuleList.Clear();
                    this.bondPositions.Clear();
                }
            }
            else
            {
                this.selectedSubstrateList.Clear();
                this.selectedModuleList.Clear();
                this.bondPositions.Clear();
            }
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void LueWorkSortType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.productConfig.SubstrateConfig.WorkOrderEnum = (WorkOrderEnum)this.LueWorkSortType.EditValue;
            }
            else
            {
                this.productConfig.ModuleConfig.WorkOrderEnum = (WorkOrderEnum)this.LueWorkSortType.EditValue;
            }

            this.ChangeImageBySelect((WorkOrderEnum)this.LueWorkSortType.EditValue);
            this.productConfig.Save();
        }

        /// <summary>
        /// 根据选项更改图片
        /// </summary>
        /// <param name="workOrder">工作顺序</param>
        private void ChangeImageBySelect(WorkOrderEnum workOrder)
        {
            switch ((int)workOrder)
            {
                case 0:
                    this.pictureEdit1.EditValue = Properties.Resources.ToRightUp;
                    break;
                case 1:
                    this.pictureEdit1.EditValue = Properties.Resources.ToUpRight;
                    break;
                case 2:
                    this.pictureEdit1.EditValue = Properties.Resources.ToLeftUp;
                    break;
                case 3:
                    this.pictureEdit1.EditValue = Properties.Resources.ToUpLeft;
                    break;
                case 4:
                    this.pictureEdit1.EditValue = Properties.Resources.ToRightDown;
                    break;
                case 5:
                    this.pictureEdit1.EditValue = Properties.Resources.ToDownRight;
                    break;
                case 6: 
                    this.pictureEdit1.EditValue = Properties.Resources.ToLeftDown;
                    break;
                case 7:
                    this.pictureEdit1.EditValue = Properties.Resources.ToDownLeft;
                    break;
            }
        }

        /// <summary>
        /// 是否自动转向
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkAutoReturn_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.productConfig.SubstrateConfig.Arrangement =
                    this.ChkAutoReturn.Checked ? ArrangementEnum.Snake : ArrangementEnum.Normal;
            }
            else 
            {
                this.productConfig.ModuleConfig.Arrangement =
                    this.ChkAutoReturn.Checked ? ArrangementEnum.Snake : ArrangementEnum.Normal;
            }

            this.productConfig.Save();
        }

        /// <summary>
        /// 选择基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkSubstrate_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkSubstrate.Checked)
            {
                this.LueWorkSortType.EditValue = this.productConfig.SubstrateConfig.WorkOrderEnum;
                this.ChkAutoReturn.Checked = this.productConfig.SubstrateConfig.Arrangement == ArrangementEnum.Snake;
            }
        }

        /// <summary>
        /// 选择基板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkModule_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkModule.Checked)
            {
                this.LueWorkSortType.EditValue = this.productConfig.ModuleConfig.WorkOrderEnum;
                this.ChkAutoReturn.Checked = this.productConfig.ModuleConfig.Arrangement == ArrangementEnum.Snake;
            }
        }
    }
}