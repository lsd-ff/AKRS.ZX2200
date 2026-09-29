using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach
{
    /// <summary>
    /// 夹料设置界面--记录重复夹与慢速夹相关参数
    /// </summary>
    public partial class FrmRegripSet : DevExpress.XtraEditors.XtraForm
    {
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
        /// 构造函数
        /// </summary>
        public FrmRegripSet()
        {
            this.InitializeComponent();
            this.Binding();
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void Binding()
        {
            this.CeIsNeedRegrip.CheckState = WaferTableDevicePara.IsNeedRegrip ? CheckState.Checked : CheckState.Unchecked;
            this.SpFirstPullDistance.Text = WaferTableDevicePara.FirstPullDistance.ToString();
            this.SpRegripDistance.Text = WaferTableDevicePara.RegripDistance.ToString();
            this.SpRegripFeedrate.Text = WaferTableDevicePara.RegripFeedrate.ToString();

            this.CeIsNeedSlowTravel.CheckState = WaferTableDevicePara.IsNeedSlowTravel ? CheckState.Checked : CheckState.Unchecked;
            this.SpSlowDistance.Text = WaferTableDevicePara.SlowDistance.ToString();
            this.SpSlowFeedrate.Text = WaferTableDevicePara.SlowFeedrate.ToString();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            WaferTableDevicePara.IsNeedRegrip = this.CeIsNeedRegrip.CheckState is CheckState.Checked;
            WaferTableDevicePara.FirstPullDistance = (double)this.SpFirstPullDistance.Value;
            WaferTableDevicePara.RegripDistance = (double)this.SpRegripDistance.Value;
            WaferTableDevicePara.RegripFeedrate = (double)this.SpRegripFeedrate.Value;

            WaferTableDevicePara.IsNeedSlowTravel = this.CeIsNeedSlowTravel.CheckState is CheckState.Checked;
            WaferTableDevicePara.SlowDistance = (double)this.SpSlowDistance.Value;
            WaferTableDevicePara.SlowFeedrate = (double)this.SpSlowFeedrate.Value;
            WaferSubDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// Next按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNext_Click(object sender, EventArgs e)
        {
            this.SaveData();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            //this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "是否结束示教?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    this.DialogResult = DialogResult.Cancel;
                    break;
                case DialogResult.No:
                    break;
            }
        }

        #region 同步块

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpRegripFeedrate_EditValueChanged(object sender, EventArgs e)
        {
            this.TbRegripFeedrate.EditValue = this.SpRegripFeedrate.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbRegripFeedrate_EditValueChanged(object sender, EventArgs e)
        {
            this.SpRegripFeedrate.EditValue = this.TbRegripFeedrate.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSlowFeedrate_EditValueChanged(object sender, EventArgs e)
        {
            this.TbSlowFeedrate.EditValue = this.SpSlowFeedrate.EditValue;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TbSlowFeedrate_EditValueChanged(object sender, EventArgs e)
        {
            this.SpSlowFeedrate.EditValue = this.TbSlowFeedrate.EditValue;
        }

        #endregion

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshTwiceRegrip()
        {
            if (this.CeIsNeedRegrip.CheckState == CheckState.Checked)
            {
                this.SpFirstPullDistance.Enabled = true;
                this.SpRegripDistance.Enabled = true;
                this.SpRegripFeedrate.Enabled = true;
                this.TbRegripFeedrate.Enabled = true;
            }
            else
            {
                this.SpFirstPullDistance.Enabled = false;
                this.SpRegripDistance.Enabled = false;
                this.SpRegripFeedrate.Enabled = false;
                this.TbRegripFeedrate.Enabled = false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshSlowRegrip()
        {
            if (this.CeIsNeedSlowTravel.CheckState == CheckState.Checked)
            {
                this.SpSlowDistance.Enabled = true;
                this.SpSlowFeedrate.Enabled = true;
                this.TbSlowFeedrate.Enabled = true;
            }
            else
            {
                this.SpSlowDistance.Enabled = false;
                this.SpSlowFeedrate.Enabled = false;
                this.TbSlowFeedrate.Enabled = false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshTwiceRegrip();
            this.RefreshSlowRegrip();
        }

        private void FrmRegripSet_FormClosing(object sender, FormClosingEventArgs e)
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
