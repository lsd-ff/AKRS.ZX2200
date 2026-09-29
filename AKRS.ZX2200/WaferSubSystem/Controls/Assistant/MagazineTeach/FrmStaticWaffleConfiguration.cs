using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.MagazineTeach
{
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using System.Drawing;
    using System.Threading;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.DataProcessing;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;

    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 静态华夫盘配置
    /// </summary>
    public partial class FrmStaticWaffleConfiguration : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 声明树组件
        /// </summary>
        private TreeList treeList1 = new TreeList()
                                         {
                                             KeyFieldName = "Id",
                                             ParentFieldName = "ParentId",
                                             OptionsView = { ShowCaption = false, ShowIndicator = true, ShowHorzLines = false, ShowVertLines = false },
                                             OptionsBehavior = { Editable = false },
                                             RowHeight = 20,
                                             Dock = DockStyle.Fill,
                                         };

        /// <summary>
        /// 树组件数据源
        /// </summary>
        private List<Node> list = new List<Node>();

        /// <summary>
        /// 中间静态华夫盘
        /// </summary>
        private AdapterConfig adapterTemp = new AdapterConfig() { AdapterType = AdapterTypeEnum.Static };

        /// <summary>
        /// 当前父Id
        /// </summary>
        private int selectNodeParentIndex;

        /// <summary>
        /// 当前子Id
        /// </summary>
        private int selectNodeChildrenIndex;

        /// <summary>
        /// 上晶圆程式
        /// </summary>
        private WaferSystemDomain WaferSystemDomain => WaferSystemDomain.GetInstance() ?? new WaferSystemDomain();

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmStaticWaffleConfiguration()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmMagazineFilling_Load(object sender, EventArgs e)
        {
            this.RefreshGcCompnent();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            if (!AdapterConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().StaticAdapterProgram.Name))
            {
                WaferSystemProgram.GetInstance().StaticAdapterProgram.Name = string.Empty;
                WaferSystemProgram.GetInstance().Save();
                Thread.Sleep(10);
            }

            this.CmbAdapterRepository.EditValue = WaferSystemProgram.GetInstance().StaticAdapterProgram.Name;
            this.RefreshAdapterTemp();
            this.RefreshCmbItems();
            this.RefreshGcCompnent();
            this.InitializeTreeComponent();
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbMagazineAllocationsRepository_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<AdapterConfig> frmRepository = new FrmRepository<AdapterConfig>(AdapterConfigRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    AdapterConfig temp = (AdapterConfig)frmRepository.DsSetting;

                    if (temp != null)
                    {
                        ((ComboBoxEdit)sender).Text = temp.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 下拉框按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbMagazineAllocationsRepository_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshAdapterTemp();
            this.RefreshTrees();
        }

        /// <summary>
        /// 向左填充按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFill_Click(object sender, EventArgs e)
        {
            int index = int.Parse(this.list[this.selectNodeParentIndex].Name.Split(':')[0].Substring(4)) - 1;
            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;
            if (dsSetting != null)
            {
                this.adapterTemp.WaffleArray[index].Name = dsSetting.Name;
            }

            this.RefreshTrees();
        }

        /// <summary>
        /// 向右去除按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRemove_Click(object sender, EventArgs e)
        {
            int index = int.Parse(this.list[this.selectNodeParentIndex].Name.Split(':')[0].Substring(4)) - 1;
            this.adapterTemp.WaffleArray[index].Name = string.Empty;

            this.RefreshTrees();
        }

        /// <summary>
        /// 向左批量填充按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAllFill_Click(object sender, EventArgs e)
        {
            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;
            if (dsSetting != null)
            {
                for (int i = 0; i < this.adapterTemp.MaxUseWaffleCount; i++)
                {
                    this.adapterTemp.WaffleArray[i].Name = dsSetting.Name;
                }
            }

            this.RefreshTrees();
        }

        private void BtnAllDelete_Click(object sender, EventArgs e)
        {
            this.adapterTemp.WaffleArray.ForEach(a => a.Name = string.Empty);
            this.RefreshTrees();
        }

        /// <summary>
        /// Ok按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            string name = this.CmbAdapterRepository.EditValue.ToString();
            if (name != string.Empty)
            {
                AdapterConfig adapterConfig = (AdapterConfig)AdapterConfigRepository.GetInstance()
                    .Find(name);
                adapterConfig.AdapterType = this.adapterTemp.AdapterType;
                adapterConfig.MaxUseWaffleCount = this.adapterTemp.MaxUseWaffleCount;
                adapterConfig.WaffleArray = this.adapterTemp.WaffleArray;
                AdapterConfigRepository.GetInstance().Save();

                WaferSystemDomain.WaferSystemProgram.StaticAdapterProgram.Name = this.CmbAdapterRepository.EditValue.ToString();
                WaferSystemProgram.GetInstance().Save();
            }

            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CreateStateWithStaticAdapterTablet();
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.SetNeedReCreateStateWithStaticAdapterTabletSignal();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Timer刷新内容
        /// </summary>
        private void RefreshWithTimer()
        {
            this.GcTemp.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);
            this.BtnFill.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);
            this.BtnAllFill.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);
            this.BtnRemove.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);
            this.BtnOk.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);
            this.BtnCancel.Enabled = !string.IsNullOrWhiteSpace(this.CmbAdapterRepository.Text);

            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;

            if (dsSetting == null)
            {
                return;
            }

            if (CarrierConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == dsSetting.Name))
            {
                if (CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == dsSetting.Name) is CarrierWithStaticWaffleConfig)
                {
                    //this.LcCarrierType.Text = $"Component carrier type: {CarrierTypeEnum.StaticWaffle}";
                    this.LcCarrierType.Text = $"载具类型: {CarrierTypeEnum.StaticWaffle}";
                }
                else
                {
                    //this.LcCarrierType.Text = $"Component carrier type:";
                    this.LcCarrierType.Text = $"载具类型:";
                }
            }
            else
            {
                //this.LcCarrierType.Text = $"Component carrier type:";
                this.LcCarrierType.Text = $"载具类型:";
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshWithTimer();
        }

        /// <summary>
        /// 修改temp最大使用层数信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpNumberOfSlots_EditValueChanged(object sender, EventArgs e)
        {
            this.adapterTemp.MaxUseWaffleCount = (int)this.SpNumberOfSlots.Value;
            this.RefreshTrees();
        }

        /// <summary>
        /// 初始化树组件
        /// </summary>
        private void InitializeTreeComponent()
        {
            this.RefreshTrees();

            // 绑定TreeList
            this.treeList1.DataSource = this.list;
            this.treeList1.Columns[0].Caption = "Allocations";
            this.treeList1.Columns[0].OptionsColumn.AllowSort = false;
            this.treeList1.Columns[0].OptionsFilter.AllowFilter = false;
            this.treeList1.ExpandAll();
            this.treeList1.FocusedNodeChanged += this.TreeList1_FocusedNodeChanged;
        }

        /// <summary>
        /// 树结构聚焦事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TreeList1_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        {
            Node node = this.list[e.Node.Id];
            this.selectNodeParentIndex = node.ParentId;
            this.selectNodeChildrenIndex = node.Id - node.ParentId;
        }

        /// <summary>
        /// 刷新树结构
        /// </summary>
        private void RefreshTrees()
        {
            this.list.Clear();
            int index = 0;
            int parentIndex = 0;
            if (AdapterConfigRepository.GetInstance().IsExists(this.adapterTemp?.Name))
            {
                for (int i = 0; i < this.adapterTemp.MaxUseWaffleCount; i++)
                {
                    parentIndex = index;

                    if (CarrierConfigRepository.GetInstance().IsExists(this.adapterTemp.WaffleArray[i].Name))
                    {
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}: {this.adapterTemp.WaffleArray[i].CarrierWithWaffleConfig.Name}" });
                    }
                    else
                    {
                        this.adapterTemp.WaffleArray[i].Name = string.Empty;
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}:" });
                    }

                    index++;
                }
            }

            this.GcAdapter.Controls.Clear();
            this.GcAdapter.Refresh();
            this.GcAdapter.Controls.Add(this.treeList1);
        }

        /// <summary>
        /// 刷新料片方法
        /// </summary>
        private void RefreshGcCompnent()
        {
            List<BaseCarrierConfig> carrierList = new List<BaseCarrierConfig>();
            carrierList.AddRange(CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe()
                .Filter(a => a.CarrierType == CarrierTypeEnum.StaticWaffle));
            this.GcComponent.DataSource = carrierList;
        }

        /// <summary>
        /// 刷新适配器Temp
        /// </summary>
        private void RefreshAdapterTemp()
        {
            string name = this.CmbAdapterRepository.Text;
            if (name != string.Empty)
            {
                AdapterConfig t = AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                this.adapterTemp = JsonFormatHelper<AdapterConfig>.DeepGenericCopy<AdapterConfig>(t);               
            }

            if (this.adapterTemp == null)
            {
                this.CmbAdapterRepository.Text = string.Empty;
                this.RefreshCmbItems();
            }
            else
            {
                this.SpNumberOfSlots.Text = this.adapterTemp.MaxUseWaffleCount.ToString();
            }
        }

        /// <summary>
        /// 添加 Wafer 或者waffle
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddWafer_Click(object sender, EventArgs e)
        {
            FrmNewCreateComponent frmNewCreateComponent = new FrmNewCreateComponent(true);

            if (frmNewCreateComponent.ShowDialog() == DialogResult.OK)
            {
                this.RefreshTrees();
                this.RefreshGcCompnent();
            }

            frmNewCreateComponent.Dispose();
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemoveWafer_Click(object sender, EventArgs e)
        {
            BaseCarrierConfig carrierConfig = this.GvComponent.GetFocusedRow() as BaseCarrierConfig;
            if (carrierConfig != null)
            {
                carrierConfig.RemoveFromBelongRecipeIds();
                CarrierConfigRepository.GetInstance().Save();
                this.RefreshTrees();
                this.RefreshGcCompnent();
            }
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.CmbAdapterRepository.Properties.Items.Clear();
            List<string> nameList = new List<string>();

            // 这里改为直接从库里读取 2026年4月9日
            nameList.AddRange(
                AdapterConfigRepository.GetInstance()./*GetDataSourceByCurrentRecipe()*/BaseDsSettingList
                    .Filter(a => a.AdapterType == AdapterTypeEnum.Static).Filter(a => a.Name != string.Empty)
                    .Select(a => a.Name).ToList());
            this.CmbAdapterRepository.Properties.Items.AddRange(nameList);
            if (nameList.Count == 0)
            {
                this.CmbAdapterRepository.Text = string.Empty;
            }
        }

        /// <summary>
        /// Extrac
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtExtraWafer_Click(object sender, EventArgs e)
        {
            FrmRepository<BaseCarrierConfig> frmRepository = new FrmRepository<BaseCarrierConfig>(CarrierConfigRepository.GetInstance());

            frmRepository.ShowDialog();

            this.RefreshAdapterTemp();
            this.RefreshTrees();
            this.RefreshGcCompnent();
        }  
        
        /// <summary>
        /// 添加 Adpter
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddAdpter_Click(object sender, EventArgs e)
        {
            FrmNewCreateAdapter frmNewCreateAdapter = new FrmNewCreateAdapter(true);
            if (frmNewCreateAdapter.ShowDialog() == DialogResult.OK)
            {
                this.RefreshTrees();
                this.RefreshCmbItems();
            }

            frmNewCreateAdapter.Dispose();
        }

        /// <summary>
        /// Extra Adpter
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtExtraAdpter_Click(object sender, EventArgs e)
        {
            FrmRepository<AdapterConfig> frmRepository = new FrmRepository<AdapterConfig>(AdapterConfigRepository.GetInstance());

            frmRepository.ShowDialog();

            this.RefreshAdapterTemp();
            this.RefreshTrees();
            this.RefreshGcCompnent();
            this.RefreshCmbItems();
        }

        /// <summary>
        /// Remove Adpter
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemoveAdpter_Click(object sender, EventArgs e)
        {
            AdapterConfig adapter = this.GvComponent.GetFocusedRow() as AdapterConfig;
            if (adapter != null)
            {
                adapter.RemoveFromBelongRecipeIds();
                AdapterConfigRepository.GetInstance().Save();
                this.RefreshTrees();
                this.RefreshGcCompnent();
            }
        }

        private void FrmStaticWaffleConfiguration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }
    }
}
