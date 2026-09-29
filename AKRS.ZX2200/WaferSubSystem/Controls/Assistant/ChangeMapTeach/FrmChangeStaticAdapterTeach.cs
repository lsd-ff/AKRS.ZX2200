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
    using AKRS.ZX2200.BondSystem.Models;
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
    using OfficeOpenXml.Sorting;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmChangeStaticAdapterTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 是否第一次加载
        /// </summary>
        private bool isfirstLoad = true;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// BaseTablet
        /// </summary>
        private BaseTablet tempTablet;

        /// <summary>
        /// BaseTablet[]
        /// </summary>
        private BaseTablet[] tabletArray;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmChangeStaticAdapterTeach()
        {
            this.InitializeComponent();
            this.tempTablet =
                (BaseTablet)JsonFormatHelper<BaseTablet>.DeepCopy(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet);
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
            WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
        }

        /// <summary>
        /// OK按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
            this.Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Abort按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAbort_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        /// <summary>
        /// FrmChangeStaticAdapterTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmChangeStaticAdapterTeach_Load(object sender, EventArgs e)
        {
            Static.IsNeedChangeStaticWaffle = true;
            Machine.GetInstance().OpenAlarm();
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
        /// Save
        /// </summary>
        private void Save()
        {
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet = (AdapterTablet)this.tempTablet;
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
            this.RefreshPncCarrierMapping();
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

        /// <summary>
        /// 创建静态华夫盘状态
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnFillALLSlotState_Click(object sender, EventArgs e)
        {
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CreateStateWithStaticAdapterTablet();
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet.SlotState = SlotStatuEnum.Good;
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet?.AdapterState?.SetAllSlotState(SlotStatuEnum.Good);
            WaferSubDevicePara.GetInstance().WaferTableDevicePara.ResetNeedReCreateStateWithStaticAdapterTabletSignal();

            this.tempTablet =
                (BaseTablet)JsonFormatHelper<BaseTablet>.DeepCopy(WaferSubDevicePara.GetInstance().WaferTableDevicePara.CurrentStaticAdapterTablet);
            this.tabletArray = new BaseTablet[] { this.tempTablet };
            this.Grc1.DataSource = this.tabletArray;

            this.Grv1.RefreshData();
            this.BindingAdapterData();
        }

        private void FrmChangeStaticAdapterTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            Static.IsNeedChangeStaticWaffle = false;
            if (ucCarrierMapping != null)
            {
                ucCarrierMapping.Dispose();
            }

            Machine.GetInstance().ResetAlarm();
        }

        private void BtnStaticWaffleVacuum_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
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