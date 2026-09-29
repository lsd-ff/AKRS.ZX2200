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
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;

    using DevExpress.Utils.Extensions;

    /// <summary>
    /// Magazine System 设置
    /// </summary>
    public partial class FrmMagazineFilling : DevExpress.XtraEditors.XtraForm
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
        /// 中间MagazineAllocationsSetting
        /// </summary>
        private MagazineAllocationsConfig allocationsTemp = new MagazineAllocationsConfig();

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
        public FrmMagazineFilling()
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
            this.timer1.Enabled = true;
        }

        /// <summary>
        /// closing
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmMagazineFilling_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            if (!MagazineAllocationsConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == WaferSystemProgram.GetInstance().MagazineAllocationsProgram.Name))
            {
                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.Name = string.Empty;
                WaferSystemProgram.GetInstance().Save();
                Thread.Sleep(100);
            }

            this.CmbMagazineAllocationsRepository.EditValue = WaferSystemProgram.GetInstance().MagazineAllocationsProgram.Name;
            this.RgTabletType.EditValue = 0;
            this.RefreshAllocationsTemp();
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
                FrmRepository<MagazineAllocationsConfig> frmRepository = new FrmRepository<MagazineAllocationsConfig>(MagazineAllocationsConfigRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    MagazineAllocationsConfig temp = (MagazineAllocationsConfig)frmRepository.DsSetting;

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
            this.RefreshAllocationsTemp();
            this.RefreshTrees();
        }

        /// <summary>
        /// 向左填充按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFill_Click(object sender, EventArgs e)
        {
            string name;
            int index = int.Parse(this.list[this.selectNodeParentIndex].Name.Split(':')[0].Substring(4)) - 1;
            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;
            if (this.allocationsTemp.TabletArray[index] is WaferTablet)
            {
                if (this.GvComponent.GetFocusedRow() is CarrierWithWaferConfig)
                {
                    WaferTablet wt = new WaferTablet() { Index = index, Name = dsSetting.Name };
                    this.allocationsTemp.TabletArray[index] = wt;
                }

                if (this.GvComponent.GetFocusedRow() is AdapterConfig)
                {
                    AdapterTablet adt = new AdapterTablet() { Index = index, Name = dsSetting.Name };
                    this.allocationsTemp.TabletArray[index] = adt;
                }
            }
            else if (this.allocationsTemp.TabletArray[index] is AdapterTablet)
            {
                if (this.selectNodeChildrenIndex > 0)
                {
                    if (this.GvComponent.GetFocusedRow() is CarrierWithWaffleConfig)
                    {
                        name = this.list[this.selectNodeParentIndex].Name.Split(':')[1].Substring(1);
                        AdapterConfig temp = (AdapterConfig)AdapterConfigRepository.GetInstance().Find(name);
                        if (!temp.WaffleArray[this.selectNodeChildrenIndex - 1].Name.Equals(dsSetting.Name))
                        {
                            temp.WaffleArray[this.selectNodeChildrenIndex - 1].Name = dsSetting.Name;
                            temp.WaffleArray[this.selectNodeChildrenIndex - 1].CarrierWithWaffleConfig
                                .ComponentTransportUnitGeometry.State = AssistantStateEnum.UnAble;
                            CarrierConfigRepository.GetInstance().Save();
                            AdapterConfigRepository.GetInstance().Save();
                        }
                    }
                }
                else
                {
                    if (this.GvComponent.GetFocusedRow() is CarrierWithWaferConfig)
                    {
                        WaferTablet wt = new WaferTablet() { Index = index, Name = dsSetting.Name };
                        this.allocationsTemp.TabletArray[index] = wt;
                    }

                    if (this.GvComponent.GetFocusedRow() is AdapterConfig)
                    {
                        AdapterTablet adt = new AdapterTablet() { Index = index, Name = dsSetting.Name };
                        this.allocationsTemp.TabletArray[index] = adt;
                    }
                }
            }
            else if (this.allocationsTemp.TabletArray[index] is NullTablet)
            {
                if (this.GvComponent.GetFocusedRow() is CarrierWithWaferConfig)
                {
                    WaferTablet wt = new WaferTablet() { Index = index, Name = dsSetting.Name };
                    this.allocationsTemp.TabletArray[index] = wt;
                }

                if (this.GvComponent.GetFocusedRow() is AdapterConfig)
                {
                    AdapterTablet adt = new AdapterTablet() { Index = index, Name = dsSetting.Name };
                    this.allocationsTemp.TabletArray[index] = adt;
                }
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
            if (this.selectNodeChildrenIndex == 0)
            {
                int index = int.Parse(this.list[this.selectNodeParentIndex].Name.Split(':')[0].Substring(4)) - 1;
                this.allocationsTemp.TabletArray[index] = new NullTablet() { Index = index };
            }
            else
            {
                string name = this.list[this.selectNodeParentIndex].Name.Split(':')[1].Substring(1);
                AdapterConfig temp = AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                temp.WaffleArray[this.selectNodeChildrenIndex - 1].Name = string.Empty;
                AdapterConfigRepository.GetInstance().Save();
            }

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
            if (this.GvComponent.GetFocusedRow() is CarrierWithWaferConfig)
            {
                for (int i = 0; i < this.allocationsTemp.MaxUseLayerCount; i++)
                {
                    this.allocationsTemp.TabletArray[i] = new WaferTablet() { Index = i, Name = dsSetting.Name };
                }
            }

            if (this.GvComponent.GetFocusedRow() is AdapterConfig)
            {
                for (int i = 0; i < this.allocationsTemp.MaxUseLayerCount; i++)
                {
                    this.allocationsTemp.TabletArray[i] = new AdapterTablet() { Index = i, Name = dsSetting.Name };
                }
            }

            this.RefreshTrees();
        }

        private void BtnAllDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.allocationsTemp.TabletArray.Length; i++)
            {
                this.allocationsTemp.TabletArray[i] = new NullTablet() { Index = i };
            }
            
            this.RefreshTrees();
        }

        /// <summary>
        /// Ok按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.RefreshTrees();

            string name = this.CmbMagazineAllocationsRepository.Text;
            if (name != string.Empty)
            {
                WaferSystemDomain.WaferSystemProgram.MagazineAllocationsProgram.Name = this.CmbMagazineAllocationsRepository.EditValue.ToString();
                WaferSystemProgram.GetInstance().Save();

                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.MaxUseLayerCount = this.allocationsTemp.MaxUseLayerCount;
                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray = this.allocationsTemp.TabletArray;
                MagazineAllocationsConfigRepository.GetInstance().Save();
            }

            WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.SetAllSlotState(SlotStatuEnum.Good);

            MagazineBoxDevicePara.SetNeedReCreateStateSignal();
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
            this.GcTemp.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text);
            this.BtnFill.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text) && this.treeList1.FocusedNode != null && this.GvComponent.GetFocusedRow() != null;
            this.BtnAllFill.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text) && this.GvComponent.GetFocusedRow() != null;
            this.BtnRemove.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text) && this.treeList1.FocusedNode != null;
            this.BtnOk.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text);
            this.BtnCancel.Enabled = !string.IsNullOrWhiteSpace(this.CmbMagazineAllocationsRepository.Text);

            this.GpComponent.Enabled = (int)this.RgTabletType.EditValue == (int)TabletTypeEnum.Componet;
            this.GcAdapter.Enabled = (int)this.RgTabletType.EditValue == (int)TabletTypeEnum.Adapter;

            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;

            if (dsSetting == null)
            {
                return;
            }

            if ((int)this.RgTabletType.EditValue == (int)TabletTypeEnum.Componet)
            {
                if (CarrierConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == dsSetting.Name))
                {
                    if (CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == dsSetting.Name) is CarrierWithWaferConfig)
                    {
                        //this.LcCarrierType.Text = $"Wafer TransportUnit type: {CarrierTypeEnum.Wafer}";
                        this.LcCarrierType.Text = $"载具类型: {CarrierTypeEnum.Wafer}";
                    }
                    else if (CarrierConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == dsSetting.Name) is CarrierWithWaffleConfig)
                    {
                        //this.LcCarrierType.Text = $"Wafer TransportUnit type: {CarrierTypeEnum.Waffle}";
                        this.LcCarrierType.Text = $"载具类型: {CarrierTypeEnum.Waffle}";
                    }
                }
                else
                {
                    //this.LcCarrierType.Text = $"Wafer TransportUnit type:";
                    this.LcCarrierType.Text = $"载具类型:";
                }
            }
            else if ((int)this.RgTabletType.EditValue == (int)TabletTypeEnum.Adapter)
            {
                if (AdapterConfigRepository.GetInstance().BaseDsSettingList.Exists(item => item.Name == dsSetting.Name))
                {
                    AdapterConfig temp = AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == dsSetting.Name);
                    this.LcAdapterName.Text = temp.Name;
                }
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
        /// 料片类刷新事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void RgTabletType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshGcCompnent();
        }

        /// <summary>
        /// adapter信息刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LcAdapterName_TextChanged(object sender, EventArgs e)
        {
            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;
            if (dsSetting != null)
            {
                string name = dsSetting.Name;
                AdapterConfig temp = AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                this.SpWaffleCount.EditValue = temp.MaxUseWaffleCount;
                AdapterConfigRepository.GetInstance().Save();
                this.RefreshTrees();
            }
        }

        /// <summary>
        /// 修改Adapter模板信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpWaffleCount_EditValueChanged(object sender, EventArgs e)
        {
            BaseDsSetting dsSetting = this.GvComponent.GetFocusedRow() as BaseDsSetting;
            if (dsSetting != null)
            {
                string name = dsSetting.Name;
                AdapterConfig temp = AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                temp.MaxUseWaffleCount = (int)this.SpWaffleCount.Value;
                AdapterConfigRepository.GetInstance().Save();
                this.RefreshTrees();
            }
        }

        /// <summary>
        /// 修改temp最大使用层数信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpNumberOfSlots_EditValueChanged(object sender, EventArgs e)
        {
            this.allocationsTemp.MaxUseLayerCount = (int)this.SpNumberOfSlots.Value;
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

            this.treeList1.Appearance.FocusedCell.BackColor = Color.DarkGray;
        }

        /// <summary>
        /// 刷新树结构
        /// </summary>
        private void RefreshTrees()
        {
            this.list.Clear();
            int index = 0;
            int parentIndex = 0;
            for (int i = 0; i < this.allocationsTemp.MaxUseLayerCount; i++)
            {
                parentIndex = index;
                if (this.allocationsTemp.TabletArray[i] is WaferTablet)
                {
                    WaferTablet wt = (WaferTablet)this.allocationsTemp.TabletArray[i];
                    if (CarrierConfigRepository.GetInstance().IsExists(wt.Name))
                    {
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}: {wt.Name}" });
                        index++;
                    }
                    else
                    {
                        this.allocationsTemp.TabletArray[i] = new NullTablet() { Index = i };
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}:" });
                        index++;
                    }
                }
                else if (this.allocationsTemp.TabletArray[i] is AdapterTablet)
                {
                    AdapterTablet ad = (AdapterTablet)this.allocationsTemp.TabletArray[i];
                    if (AdapterConfigRepository.GetInstance().IsExists(ad.Name))
                    {
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}: {ad.Name}" });
                        index++;

                        for (int j = 0; j < ad.AdapterSetting.MaxUseWaffleCount; j++)
                        {
                            if (CarrierConfigRepository.GetInstance().IsExists(ad.AdapterSetting.WaffleArray[j].Name))
                            {
                                this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"{index}- {ad.AdapterSetting.WaffleArray[j].Name}" });
                                index++;
                            }
                            else
                            {
                                ad.AdapterSetting.WaffleArray[j].Name = string.Empty;
                                this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"{index}- {ad.AdapterSetting.WaffleArray[j].Name}" });
                                index++;
                            }
                        }
                    }
                    else
                    {
                        this.allocationsTemp.TabletArray[i] = new NullTablet() { Index = i };
                        this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}:" });
                        index++;
                    }
                }
                else if (this.allocationsTemp.TabletArray[i] is NullTablet)
                {
                    this.list.Add(new Node() { Id = index, ParentId = parentIndex, Name = $"Slot{i + 1}:" });
                    index++;
                }
            }

            this.GcAllocations.Controls.Clear();
            this.GcAllocations.Refresh();
            this.GcAllocations.Controls.Add(this.treeList1);
        }

        /// <summary>
        /// 刷新料片方法
        /// </summary>
        private void RefreshGcCompnent()
        {
            if (this.RgTabletType.SelectedIndex == (int)TabletTypeEnum.Componet)
            {
                this.GcComponent.DataSource = CarrierConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Filter(a => a.CarrierType != CarrierTypeEnum.StaticWaffle);
            }
            else if (this.RgTabletType.SelectedIndex == (int)TabletTypeEnum.Adapter)
            {
                this.GcComponent.DataSource = AdapterConfigRepository.GetInstance().GetDataSourceByCurrentRecipe().Filter(a => a.AdapterType == AdapterTypeEnum.WaferTable);
            }
            else
            {
            }
        }

        /// <summary>
        /// 刷新AllocationsTemp
        /// </summary>
        private void RefreshAllocationsTemp()
        {
            string name = this.CmbMagazineAllocationsRepository.Text;
            if (name != string.Empty)
            {
                MagazineAllocationsConfig t = MagazineAllocationsConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == name);
                this.allocationsTemp = JsonFormatHelper<MagazineAllocationsConfig>.DeepGenericCopy<MagazineAllocationsConfig>(t);               
            }

            this.SpNumberOfSlots.Text = this.allocationsTemp.MaxUseLayerCount.ToString();
        }

        /// <summary>
        /// 添加 Wafer 或者waffle
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtAddWafer_Click(object sender, EventArgs e)
        {
            FrmNewCreateComponent frmNewCreateComponent = new FrmNewCreateComponent();

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
            this.CmbMagazineAllocationsRepository.Properties.Items.Clear();
            List<string> nameList = MagazineAllocationsConfigRepository.GetInstance().Filter(string.Empty, false).Select(a => a.Name).ToList();
            this.CmbMagazineAllocationsRepository.Properties.Items.AddRange(nameList);
            if (nameList.Count == 0)
            {
                this.CmbMagazineAllocationsRepository.Text = string.Empty;
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

            this.RefreshAllocationsTemp();
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
            FrmNewCreateAdapter frmNewCreateAdapter = new FrmNewCreateAdapter();
            if (frmNewCreateAdapter.ShowDialog() == DialogResult.OK)
            {
                this.RefreshTrees();
                this.RefreshGcCompnent();
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

            this.RefreshAllocationsTemp();
            this.RefreshTrees();
            this.RefreshGcCompnent();
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

        private void FrmMagazineFilling_FormClosing(object sender, FormClosingEventArgs e)
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
