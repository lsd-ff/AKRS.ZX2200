using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;

    /// <summary>
    /// 槽位状态
    /// </summary>
    public partial class FrmSlotsState : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// MagazineAllocationsSetting
        /// </summary>
        private MagazineAllocationsConfig copyCurrentAllocationsConfig;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmSlotsState()
        {
            this.InitializeComponent();
            
            // 生成magazine状态
            if (WaferSubDevicePara.GetInstance().MagazineDevicePara.IsNeedReCreateState/* && WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift*/)
            {
                WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig?.SetAllSlotState(SlotStatuEnum.Good);
                WaferSubDevicePara.GetInstance().MagazineDevicePara.ResetNeedReCreateStateSignal();
            }

            this.copyCurrentAllocationsConfig =
                JsonFormatHelper<MagazineAllocationsConfig>.DeepGenericCopy(WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig);
            this.Grc1.DataSource = this.copyCurrentAllocationsConfig.TabletArray;
        }

        /// <summary>
        /// SaveData
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSave_Click(object sender, EventArgs e)
        {
            this.Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// FillALLSlotState
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFillALLSlotState_Click(object sender, EventArgs e)
        {
            this.FillALLSlotState();
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
                this.Grc2.DataSource = ad.AdapterState.WafflePlateSlotsState;
            }

            this.Grv2.RefreshData();
        }

        /// <summary>
        /// 填充所有槽状态
        /// </summary>
        private void FillALLSlotState()
        {
            this.copyCurrentAllocationsConfig.SetAllSlotState(SlotStatuEnum.Good);

            MagazineBoxDevicePara.ResetNeedReCreateStateSignal();

            this.Grv1.RefreshData();
            this.BindingAdapterData();
            this.Save();
        }

        /// <summary>
        /// Save
        /// </summary>
        private void Save()
        {
            WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray = this.copyCurrentAllocationsConfig.TabletArray;
            MagazineAllocationsConfigRepository.GetInstance().Save();

            this.RefreshPncCarrierMapping();
        }

        /// <summary>
        /// 聚焦事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Grv1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            this.BindingAdapterData();
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
            BaseTablet baseTablet = this.Grv1.GetFocusedRow() as BaseTablet;
            if (baseTablet == null)
            {
                return;
            }

            this.copyCurrentAllocationsConfig.SetSlotState(baseTablet, SlotStatuEnum.Good);

            this.Grv1.RefreshData();
            this.BindingAdapterData();
            this.Save();
        }

        private void BtnGrv1ResetState_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            BaseTablet baseTablet = this.Grv1.GetFocusedRow() as BaseTablet;
            if (baseTablet == null)
            {
                return;
            }

            this.copyCurrentAllocationsConfig.SetSlotState(baseTablet, SlotStatuEnum.None);

            this.Grv1.RefreshData();
            this.BindingAdapterData();
            this.Save();
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
            this.Save();
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
            this.Save();
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

        private void FrmSlotsState_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ucCarrierMapping != null)
            {
                ucCarrierMapping.Dispose();
            }
        }
    }
}