using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach
{
    using System.Drawing;
    using System.Linq;
    using System.Threading;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.Utils.Extensions;
    using DevExpress.XtraEditors;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmChangeTabletTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 是否第一次加载
        /// </summary>
        private bool isfirstLoad = true;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// FlipChipModule
        /// </summary>
        private FlipModule FlipChipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// FlipChipDevicePara
        /// </summary>
        private FlipTableDevicePara FlipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        #endregion

        /// <summary>
        /// WaferSubModule
        /// </summary>
        private WaferSubModule WaferSubModule => WaferSubModule.GetInstance();

        /// <summary>
        /// BaseTablet
        /// </summary>
        private BaseTablet tempTablet;

        /// <summary>
        /// BaseTablet[]
        /// </summary>
        private BaseTablet[] tabletArray;

        private string tabletName;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmChangeTabletTeach(string tabletName = null)
        {
            this.InitializeComponent();
            this.tabletName = tabletName;
            if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift)
            {
                AKRSMessageBoxExt.Show($"更换料片的方式不是手动模式！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                this.Close();
                return;
            }

            this.tempTablet =
                (BaseTablet)JsonFormatHelper<BaseTablet>.DeepCopy(this.WaferTableDevicePara.CurrentTablet);
            this.tabletArray = new BaseTablet[] { this.tempTablet };
            this.Grc1.DataSource = this.tabletArray;
            this.BindingAdapterData();
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            try
            {
                Machine.GetInstance().OpenAlarm();

                // 无所需芯片提示
                this.res = AKRSMessageBoxExt.Show($"当前料片不存在Bond需要的芯片 -{WaferSystemDomain.GetInstance().WaferSubSystemTask.GetCurrentNeedChipName()} , 请更换料片！\r\n" + "晶圆台将移动到手动换料位！\r\n" + "继续请点击 OK.\r\n" + "停止请点击 CANCEL.", "Prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
                switch (this.res)
                {
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                        return;
                }

                // 顶针
                WaferSubController.GetInstance().EjectController.ReturnEjection();

                // 扩晶到低位
                WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();

                if (WaferTableDevicePara.IsUseWaferTableWaffleVacuum)
                {
                    this.res = AKRSMessageBoxExt.Show($"是否降下晶圆夹持气缸？\r\n" + "降下请点击 OK.\r\n" + "不降请点击 CANCEL.", "Prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
                    switch (this.res)
                    {
                        case DialogResult.OK:
                            WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                            break;
                        case DialogResult.Cancel:
                            break;
                    }
                }
                else
                {
                    // 放下晶圆夹持气缸
                    WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                }

                // 晶圆夹去安全位
                WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();

                // 晶圆台到达手动换料位
                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToManualChangePosition();

                if (WaferTableDevicePara.IsUseWaferTableWaffleVacuum & WaferSubModule.GetInstance().WaferTable.WaffleVacuum != null)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                }

                // 将晶圆台中的料片拿走
                this.res = AKRSMessageBoxExt.Show($"请从晶圆台移除料片！\r\n" + "继续请点击 OK.\r\n" + "停止请点击 CANCEL.", "Prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
                switch (this.res)
                {
                    case DialogResult.OK:
                        break;
                    case DialogResult.Cancel:
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                        return;
                }

                // Save
                WaferTableDevicePara.CurrentTablet = new NullTablet();
                WaferSubDevicePara.GetInstance().Save();

            // 放置新的料片在晶圆台中
            Prompt:
                this.res = AKRSMessageBoxExt.Show($"请放置一片新料到晶圆台！\r\n" + "继续请点击 OK.\r\n" + "停止请点击 CANCEL.", "Prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
                switch (this.res)
                {
                    case DialogResult.OK:
                        if (!WaferSubController.GetInstance().WaferTableController.IsWaferSensor)
                        {
                            goto Prompt;
                        }

                        break;
                    case DialogResult.Cancel:
                        this.DialogResult = DialogResult.Abort;
                        this.Close();
                        return;
                }

                AKRSMessageBoxExt.Show($"请选择料片！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                this.ShowDialog();
            }
            catch (Exception e)
            {
                if (Machine.GetInstance().IsWorking())
                {
                    throw new Exception(e.Message);
                }
                else 
                {
                    AKRSMessageBoxExt.Show(e.Message, "Error", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                }                               
            }           
        }

        /// <summary>
        /// OK按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (this.CmbTabletName.EditValue == null)
            {
                AKRSMessageBoxExt.Show("请选择料片！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                return;
            }

            this.Save();
            AKRSMessageBoxExt.Show("晶圆台即将移动！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });

            // 晶圆夹持气缸抬起
            WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
            if (WaferTableDevicePara.CurrentTablet is WaferTablet wt)
            {
                // 晶圆台到准备位
                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

                // 扩晶到扩晶位
                WaferSubController.GetInstance().WaferTableController.MoveExpandToUpPosition(wt.CarrierConfigWithWafer);
            }

            WaferSubController.GetInstance().WaferTableController.CheckAndOpenWaffleVacuum();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Abort按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        /// <summary>
        /// FrmChangeTabletTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmChangeTabletTeach_Load(object sender, EventArgs e)
        {
            // 绑定料片名称
            this.CmbTabletName.Properties.Items.Clear();
            foreach (var item in CarrierConfigRepository.GetInstance().BaseDsSettingList)
            {
                if (item is CarrierWithWaferConfig)
                {
                    if (CarrierConfigRepository.GetInstance().IsExists(item.Name))
                    {
                        this.CmbTabletName.Properties.Items.Add(item.Name);
                    }
                }
            }

            foreach (AdapterConfig item in AdapterConfigRepository.GetInstance().BaseDsSettingList)
            {
                if (AdapterConfigRepository.GetInstance().IsExists(item.Name))
                {
                    this.CmbTabletName.Properties.Items.Add(item.Name);
                }
            }

            if (this.tabletName == null)
            {
                if (CarrierConfigRepository.GetInstance().Find(WaferSystemDomain.GetInstance().WaferSubSystemTask.GetCurrentNeedChipName()) is CarrierWithWaferConfig carrier)
                {
                    this.CmbTabletName.EditValue = carrier.Name;
                }
                else
                {
                    if (this.tempTablet is NullTablet)
                    {
                        this.CmbTabletName.EditValue = null;
                    }
                    else
                    {
                        if (this.tempTablet.Name == null || this.tempTablet.Name == string.Empty || this.tempTablet.Name == "")
                        {
                            this.CmbTabletName.EditValue = this.tempTablet.Name;
                        }
                        
                        this.Grc1.DataSource = this.tempTablet;
                    }
                }
            }
            else
            {
                this.CmbTabletName.EditValue = this.tabletName;
            }
        }

        /// <summary>
        /// Grv1_DataSourceChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Grv1_DataSourceChanged(object sender, EventArgs e)
        {
            this.BindingAdapterData();
        }

        /// <summary>
        /// CmbTabletName_EditValueChanged
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTabletName_EditValueChanged(object sender, EventArgs e)
        {
            //if (this.isfirstLoad)
            //{
            //    this.isfirstLoad = false;
            //    return;
            //}

            if (this.CmbTabletName.EditValue == null)
            {
                return;
            }

            CarrierWithWaferConfig carrierWithWaferConfigWithWafer = CarrierConfigRepository.GetInstance().BaseDsSettingList
                                                    .Find(item => item.Name == this.CmbTabletName.EditValue.ToString()) as CarrierWithWaferConfig;
            if (carrierWithWaferConfigWithWafer != null)
            {
                this.tempTablet = new WaferTablet() { Name = carrierWithWaferConfigWithWafer.Name };
                this.tabletArray = new BaseTablet[] { this.tempTablet };
                this.Grc1.DataSource = this.tabletArray;
                return;
            }

            AdapterConfig adapterSetting = AdapterConfigRepository.GetInstance().BaseDsSettingList
                                                .Find(item => item.Name == this.CmbTabletName.EditValue.ToString()) as AdapterConfig;
            if (adapterSetting != null)
            {
                this.tempTablet = new AdapterTablet() { Name = adapterSetting.Name };
                AdapterTablet temp = (AdapterTablet)this.tempTablet;
                temp.CreateAdapterEntity();
                this.tabletArray = new BaseTablet[] { this.tempTablet };
                this.Grc1.DataSource = this.tabletArray;

                this.RefreshPncCarrierMapping();
                return;
            }

            AKRSMessageBoxExt.Show("请检查该料片的芯片在库中是否存在?", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
        }

        /// <summary>
        /// Save
        /// </summary>
        private void Save()
        {
            WaferTableDevicePara.CurrentTablet = this.tempTablet;
            WaferSubDevicePara.GetInstance().Save();

            this.RefreshPncCarrierMapping();
        }

        /// <summary>
        /// BindingAdapterData
        /// </summary>
        private void BindingAdapterData()
        {
            AdapterTablet ad = this.Grv1.GetFocusedRow() as AdapterTablet;
            if (ad == null)
            {
                this.Grc2.DataSource = null;
            }
            else
            {
                if (ad.AdapterState == null)
                {
                    this.Grc2.DataSource = null;
                }
                else
                {
                    this.Grc2.DataSource = ad.AdapterState.WafflePlateSlotsState;
                }
            }

            this.Grv2.RefreshData();
        }

        /// <summary>
        /// 聚焦事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Grv2_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.RefreshPncCarrierMapping();
        }

        /// <summary>
        /// BtnGrv1SetState_ButtonClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGrv1SetState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            WaferTablet waferTablet = this.Grv1.GetFocusedRow() as WaferTablet;
            if (waferTablet != null)
            {
                waferTablet.SlotState = SlotStatuEnum.Good;
                waferTablet.InitTabletInfo();
            }

            AdapterTablet adapterTablet = this.Grv1.GetFocusedRow() as AdapterTablet;
            if (adapterTablet != null)
            {
                adapterTablet.SlotState = SlotStatuEnum.Good;
                adapterTablet.AdapterState.SetAllSlotState(SlotStatuEnum.Good);
            }

            this.Grv1.RefreshData();
            this.BindingAdapterData();
        }

        private void BtnGrv1ResetState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            WaferTablet waferTablet = this.Grv1.GetFocusedRow() as WaferTablet;
            if (waferTablet != null)
            {
                waferTablet.SlotState = SlotStatuEnum.None;
                waferTablet.InitTabletInfo();
            }

            AdapterTablet adapterTablet = this.Grv1.GetFocusedRow() as AdapterTablet;
            if (adapterTablet != null)
            {
                adapterTablet.SlotState = SlotStatuEnum.None;
                adapterTablet.AdapterState.SetAllSlotState(SlotStatuEnum.None);
            }

            this.Grv1.RefreshData();
            this.BindingAdapterData();
        }

        /// <summary>
        /// BtnGrv2SetState_ButtonClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGrv2SetState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AdapterTablet ad = this.Grv1.GetFocusedRow() as AdapterTablet;
            if (ad == null)
            {
                return;
            }

            AdapterSlotEntity adapterSlotEntity = this.Grv2.GetFocusedRow() as AdapterSlotEntity;
            ad.AdapterState.SetSlotState(adapterSlotEntity, SlotStatuEnum.Good);

            this.BindingAdapterData();
        }

        private void BtnGrv2ResetState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            AdapterTablet ad = this.Grv1.GetFocusedRow() as AdapterTablet;
            if (ad == null)
            {
                return;
            }

            AdapterSlotEntity adapterSlotEntity = this.Grv2.GetFocusedRow() as AdapterSlotEntity;
            ad.AdapterState.SetSlotState(adapterSlotEntity, SlotStatuEnum.None);

            this.BindingAdapterData();
        }

        /// <summary>
        /// Grv1_CustomDrawRowIndicator
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Grv1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        /// <summary>
        /// Grv2_CustomDrawRowIndicator
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Grv2_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }

        private UcCarrierMapping ucCarrierMapping;

        /// <summary>
        /// RefreshPncCarrierMapping
        /// </summary>
        private void RefreshPncCarrierMapping()
        {
            AdapterSlotEntity adapterSlotEntity = this.Grv2.GetFocusedRow() as AdapterSlotEntity;
            if (adapterSlotEntity == null)
            {
                this.PncCarrierMapping.Controls.Clear();
            }
            else
            {
                this.PncCarrierMapping.Controls.Clear();

                if (ucCarrierMapping == null)
                {
                    ucCarrierMapping = new UcCarrierMapping(adapterSlotEntity);
                }
                else
                {
                    ucCarrierMapping.SetCarrierMapping(adapterSlotEntity);
                }

                //UcCarrierMapping ucCarrierMapping = new UcCarrierMapping(adapterSlotEntity);
                this.PncCarrierMapping.AddControl(ucCarrierMapping);
            }
        }

        private void FrmChangeTabletTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ucCarrierMapping != null)
            {
                ucCarrierMapping.Dispose();
            }

            Machine.GetInstance().ResetAlarm();
        }

        private void BtnWaffleVacuum_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }
    }
}