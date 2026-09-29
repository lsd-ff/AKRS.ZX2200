using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using DevExpress.XtraEditors;
using System;

namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// Bond 区域和下料等待
    /// </summary>
    public partial class UcBeltSystem2 : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 流道模组
        /// </summary>
        private TransportController TransportController => TransportDomain.GetInstance().TransportController;

        /// <summary>
        /// 基板流道程式
        /// </summary>
        private TransportProgram transportProgram => TransportProgram.GetInstance() ?? new TransportProgram();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcBeltSystem2()
        {
            InitializeComponent();

            this.LueClampMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<ClampModeEnum>();
            this.LueUnClampMode.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<UnClampModeEnum>();
            this.LueVacuumOption.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<VacuumOptionEnum>();

            this.InitControl();
        }

        /// <summary>
        /// 初始化控件赋值
        /// </summary>
        private void InitControl()
        {
            this.TrackBarForwardVelRate1.EditValue =
                this.transportProgram.BondSubSectionProgram.TransportBeltSetting?.ForwardVelRate * 100;

            this.TrackBarBackwardVelRate1.EditValue =
                this.transportProgram.BondSubSectionProgram.TransportBeltSetting?.BackwardVelRate * 100;

            this.TrackBarForwardVelRate2.EditValue =
                this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSetting?.ForwardVelRate * 100;

            this.TrackBarBackwardVelRate2.EditValue =
                this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSetting?.BackwardVelRate * 100;

            this.LueClampMode.EditValue =
                this.transportProgram.BondSubSectionProgram.TransportBeltSetting?.ClampMode;

            this.LueUnClampMode.EditValue =
                this.transportProgram.BondSubSectionProgram.TransportBeltSetting?.UnClampMode;

            this.SpDistanceHardstopLeft.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.HardstopLeftDistance;

            this.SpDelayBeforeClamping.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.BeforeClampingDelay;

            this.SpDelayAfterClamping.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.AfterClampingDelay;

            this.SpDelayBeforeUnclamping.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.BeforeUnClampingDelay;

            this.SpDelayAfterUnclamping.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.AfterUnClampingDelay;

            this.LueVacuumOption.EditValue
                = this.transportProgram.BondSubSectionProgram.BondinsertSetting?.VacuumOption;
        }

        /// <summary>
        /// 确认和赋值
        /// </summary>
        public void Confirm()
        {
            if (this.transportProgram.IsSettingNull())
            {
                return;
            }

            if (this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting == null)
            {
                AKRSXtraMessageBox.Show("Save error, Please set InOut PutBelt Setting first");
                return;
            }
            this.transportProgram.BondSubSectionProgram.TransportBeltSetting.ForwardVelRate
                = Math.Round(Convert.ToDouble(this.TrackBarForwardVelRate1.EditValue) / 100, 2);

            this.transportProgram.BondSubSectionProgram.TransportBeltSetting.BackwardVelRate
                = Math.Round(Convert.ToDouble(TrackBarBackwardVelRate1.EditValue) / 100, 2);

            this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSetting.ForwardVelRate
                = Math.Round(Convert.ToDouble(this.TrackBarForwardVelRate2.EditValue) / 100, 2);

            this.transportProgram.WaitingUnloadSubSectionProgram.TransportBeltSetting.BackwardVelRate
                = Math.Round(Convert.ToDouble(this.TrackBarBackwardVelRate2.EditValue) / 100, 2);

            this.transportProgram.BondSubSectionProgram.TransportBeltSetting.ClampMode
                = (ClampModeEnum)this.LueClampMode.EditValue;

            this.transportProgram.BondSubSectionProgram.TransportBeltSetting.UnClampMode
                = (UnClampModeEnum)this.LueUnClampMode.EditValue;

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.HardstopLeftDistance
                = Convert.ToDouble(SpDistanceHardstopLeft.EditValue);

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.BeforeClampingDelay
                = Convert.ToInt32(SpDelayBeforeClamping.EditValue);

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.AfterClampingDelay
                = Convert.ToInt32(SpDelayAfterClamping.EditValue);

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay
                = Convert.ToInt32(this.SpDelayBeforeUnclamping.EditValue);

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.AfterUnClampingDelay
                = Convert.ToInt32(this.SpDelayAfterUnclamping.EditValue);

            this.transportProgram.BondSubSectionProgram.BondinsertSetting.VacuumOption
                = (VacuumOptionEnum)this.LueVacuumOption.EditValue;

            TransportBeltSettingRepository.GetInstance().Save();
            BondinsertSettingRepository.GetInstance().Save();
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarForwardVelRate1_EditValueChanged(object sender, EventArgs e)
        {
            SpForwardVelRate1.EditValue = TrackBarForwardVelRate1.EditValue;
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarBackwardVelRate1_EditValueChanged(object sender, EventArgs e)
        {
            SpBackwardVelRate1.EditValue = TrackBarBackwardVelRate1.EditValue;
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarForwardVelRate2_EditValueChanged(object sender, EventArgs e)
        {
            SpForwardVelRate2.EditValue = TrackBarForwardVelRate2.EditValue;
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarBackwardVelRate2_EditValueChanged(object sender, EventArgs e)
        {
            SpBackwardVelRate2.EditValue = TrackBarBackwardVelRate2.EditValue;
        }

        /// <summary>
        /// Clamp
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClamp_Click(object sender, EventArgs e)
        {
           // ClampModeEnum clampMode = this.transportProgram.BondSubSectionProgram.TransportBeltSetting.ClampMode;
            this.TransportController.BondSubSectionController.Clamp();
        }

        /// <summary>
        /// UnClamp
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUnClamp_Click(object sender, EventArgs e)
        {
            this.TransportController.BondSubSectionController.UnClamp();
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpForwardVelRate1_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarForwardVelRate1.EditValue = this.SpForwardVelRate1.EditValue;
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBackwardVelRate1_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarBackwardVelRate1.EditValue = this.SpBackwardVelRate1.EditValue;
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpForwardVelRate2_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarForwardVelRate2.EditValue = this.SpForwardVelRate2.EditValue;
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBackwardVelRate2_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarBackwardVelRate2.EditValue = this.SpBackwardVelRate2.EditValue;
        }
    }
}
