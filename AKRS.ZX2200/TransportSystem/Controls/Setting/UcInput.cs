using System;
using AKRS.ZX2200.Controls.ToolControls.Programming;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;

namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.ZX2200.Infrastructure.Interface;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 上料皮带速度设置
    /// </summary>
    public partial class UcInput : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 基板流道程式
        /// </summary>
        private TransportProgram transportProgram => TransportProgram.GetInstance() ?? new TransportProgram();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcInput()
        {
            InitializeComponent();

            this.TrackBarForwardVelRate.EditValue =
                transportProgram.LoadingSubSectionProgram.InOutPutBeltSetting?.ForwardVelRate * 100.0;
        }

        /// <summary>
        /// 确认和赋值 并保存
        /// </summary>
        public void Confirm()
        {
            if (this.transportProgram.IsSettingNull())
            {
                return;
            }

            this.transportProgram.LoadingSubSectionProgram.InOutPutBeltSetting.ForwardVelRate
                = Convert.ToDouble(this.TrackBarForwardVelRate.EditValue) / 100.0;

            InOutPutBeltSettingRepository.GetInstance().Save();
        }

        /// <summary>
        /// 拖动条改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TrackBarForwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            this.SpForwardVelRate.EditValue = this.TrackBarForwardVelRate.EditValue;
        }

        /// <summary>
        /// SpinEdit改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpForwardVelRate_EditValueChanged(object sender, EventArgs e)
        {
            this.TrackBarForwardVelRate.EditValue = this.SpForwardVelRate.EditValue;
        }
    }
}
