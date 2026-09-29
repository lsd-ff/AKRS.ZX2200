using DevExpress.XtraEditors;
using System;

namespace AKRS.ZX2200.Infrastructure.Controls.Common
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 检测设置
    /// </summary>
    public partial class UcDefect : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 检测设置
        /// </summary>
        public UcDefect()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 焊后检测对象
        /// </summary>
        private PostBondInspection postBondInspection;

        /// <summary>
        /// 检测设置
        /// </summary>
        /// <param name="postBondInspection">检测对象</param>
        public void PostBondEdit(PostBondInspection postBondInspection)
        {
            this.postBondInspection = postBondInspection;
            this.Init(postBondInspection);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="postBondInspection">焊后检测值</param>
        private void Init(PostBondInspection postBondInspection)
        {
            this.SpPostBondDistanceX.Value = (decimal)(postBondInspection.PostBondDistanceX * 1000.0);
            this.SpPostBondDistanceY.Value = (decimal)(postBondInspection.PostBondDistanceY * 1000.0);
            this.SpPostBondAngle.Value = (decimal)postBondInspection.PostBondDistanceAngle;

            //// 有问题
            //this.SpPostBondEpoxy.Value = (decimal)postBondInspection.PostBondEpoxyArea[0];

            this.SpPostBondLimitX.Value = (decimal)(postBondInspection.LimitX * 1000.0);
            this.SpPostBondLimitY.Value = (decimal)(postBondInspection.LimitY * 1000.0);
            this.SpPostBondLimitAngle.Value = (decimal)postBondInspection.LimitAngle;
            this.SpEpoxyMax.Value = (decimal)postBondInspection.TolerantEpoxyMax;
            this.SpEpoxyMin.Value = (decimal)postBondInspection.TolerantEpoxyMin;

            this.ChkPostBondCompensate.Checked = postBondInspection.IsBondPostCompensation;

            this.CmbPostBondInspectionNumber.Value = (decimal)postBondInspection.BondPostCompensationNumber;

            if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.AfterBondCheck)
            {
                this.SpEpoxyMax.Enabled = false;
                this.SpEpoxyMin.Enabled = false;
                this.SpPostBondEpoxy.Enabled = false;

                this.SpPostBondAngle.Enabled = true;
                this.SpPostBondLimitAngle.Enabled = true;
                this.ChkPostBondCompensate.Enabled = true;
                this.CmbPostBondInspectionNumber.Enabled = true;
            }
            else
            {
                this.SpEpoxyMax.Enabled = true;
                this.SpEpoxyMin.Enabled = true;
                this.SpPostBondEpoxy.Enabled = true;
                this.SpPostBondAngle.Enabled = false;
                this.SpPostBondLimitAngle.Enabled = false;
                this.ChkPostBondCompensate.Enabled = false;
                this.CmbPostBondInspectionNumber.Enabled = false;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, EventArgs e)
        {
            try
            {
                this.postBondInspection.PostBondDistanceX = (double)this.SpPostBondDistanceX.Value / 1000.0;
                this.postBondInspection.PostBondDistanceY = (double)this.SpPostBondDistanceY.Value / 1000.0;
                this.postBondInspection.PostBondDistanceAngle = (double)this.SpPostBondAngle.Value;

                //// 有问题
                //this.postBondInspection.PostBondEpoxyArea[0] = (double)this.SpPostBondEpoxy.Value;

                this.postBondInspection.LimitX = (double)this.SpPostBondLimitX.Value / 1000.0;
                this.postBondInspection.LimitY = (double)this.SpPostBondLimitY.Value / 1000.0;
                this.postBondInspection.LimitAngle = (double)this.SpPostBondLimitAngle.Value;
                this.postBondInspection.TolerantEpoxyMax = (double)this.SpEpoxyMax.Value;
                this.postBondInspection.TolerantEpoxyMin = (double)this.SpEpoxyMin.Value;

                this.postBondInspection.IsBondPostCompensation = this.ChkPostBondCompensate.Checked;

                this.postBondInspection.BondPostCompensationNumber = (double)this.CmbPostBondInspectionNumber.Value;

                PostBondInspectionRepository.GetInstance().Save();

                AKRSXtraMessageBox.Show("保存成功");
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show($"保存失败,原因 ：{exception.Message}");
            }
        }
    }
}
