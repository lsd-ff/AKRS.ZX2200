using System;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// 点胶载台
    /// </summary>
    public partial class UcBeltSystem1 : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 流道控制器
        /// </summary>
        private TransportController TransportController => TransportDomain.GetInstance().TransportController;

        /// <summary>
        /// 基板流道程式
        /// </summary>
        private TransportProgram transportProgram => TransportProgram.GetInstance() ?? new TransportProgram();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcBeltSystem1()
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
            this.TrackBarForwardVelRate.EditValue =
                this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting?.ForwardVelRate * 100.0;

            this.TrackBarBackwardVelRate.EditValue =
                this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting?.BackwardVelRate * 100.0;

            this.LueClampMode.EditValue =
                this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting?.ClampMode;

            this.LueUnClampMode.EditValue =
                this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting?.UnClampMode;

            this.SpDistanceHardstopLeft.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.HardstopLeftDistance;

            this.SpDistanceHardstopRight.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.HardstopRightDistance;

            this.SpDelayBeforeClamping.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.BeforeClampingDelay;

            this.SpDelayAfterClamping.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.AfterClampingDelay;

            this.SpDelayBeforeUnClamping.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.BeforeUnClampingDelay;

            this.SpDelayAfterUnclamping.EditValue
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.AfterUnClampingDelay;

            this.LueVacuumOption.EditValue 
                = this.transportProgram.DispenseSubSectionProgram.BondinsertSetting?.VacuumOption;
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarForwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            SpForwardVelRate.EditValue = TrackBarForwardVelRate.EditValue;
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarBackwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            this.SpBackwardVelRate.EditValue = TrackBarBackwardVelRate.EditValue;
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

            this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting.ForwardVelRate
                = Convert.ToDouble(this.TrackBarForwardVelRate.EditValue) / 100.0;

            this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting.BackwardVelRate
                = Convert.ToDouble(TrackBarBackwardVelRate.EditValue) / 100.0;

            this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting.ClampMode
                = (ClampModeEnum)this.LueClampMode.EditValue;

            this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting.UnClampMode
                = (UnClampModeEnum)this.LueUnClampMode.EditValue;

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.HardstopLeftDistance
                = Convert.ToDouble(SpDistanceHardstopLeft.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.HardstopRightDistance
                = Convert.ToDouble(SpDistanceHardstopRight.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.BeforeClampingDelay 
                = Convert.ToInt32(SpDelayBeforeClamping.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.AfterClampingDelay
                = Convert.ToInt32(SpDelayAfterClamping.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay
                = Convert.ToInt32(this.SpDelayBeforeUnClamping.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.AfterUnClampingDelay
                = Convert.ToInt32(this.SpDelayAfterUnclamping.EditValue);

            this.transportProgram.DispenseSubSectionProgram.BondinsertSetting.VacuumOption
                = (VacuumOptionEnum)this.LueVacuumOption.EditValue;

            TransportBeltSettingRepository.GetInstance().Save();
            BondinsertSettingRepository.GetInstance().Save();
        }

        /// <summary>
        /// Clamp
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClamp_Click(object sender, EventArgs e)
        {
            this.TransportController.DispenseSubSectionController.Clamp(true);
        }

        /// <summary>
        /// UnClamp
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUnClamp_Click(object sender, EventArgs e)
        {
           // UnClampModeEnum unClampMode = this.transportProgram.DispenseSubSectionProgram.TransportBeltSetting.UnClampMode;
            this.TransportController.DispenseSubSectionController.UnClamp();
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpForwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarForwardVelRate.EditValue = SpForwardVelRate.EditValue;
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBackwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarBackwardVelRate.EditValue = SpBackwardVelRate.EditValue;
        }
    }
}
