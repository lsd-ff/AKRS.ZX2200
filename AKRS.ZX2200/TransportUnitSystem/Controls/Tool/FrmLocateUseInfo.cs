using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Tool
{
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Model;

    /// <summary>
    /// 配置定位使用情况
    /// </summary>
    public partial class FrmLocateUseInfo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 定位信息配置
        /// </summary>
        /// <param name="locateConfig">使用信息</param>
        public FrmLocateUseInfo(LocateConfig locateConfig)
        {
            this.InitializeComponent();

            this.locateConfig = locateConfig;

            this.locateUseConfig1 = locateConfig.LocateUseConfig1;

            this.locateUseConfig2 = locateConfig.LocateUseConfig2;

            this.locateUseConfig3 = locateConfig.LocateUseConfig3;

            this.locateUseConfig4 = locateConfig.LocateUseConfig4;

            this.adjustTypeEnum = locateConfig.AdjustType;
        }

        /// <summary>
        /// 定位信息配置
        /// </summary>
        /// <param name="locateConfig">使用信息</param>
        public FrmLocateUseInfo(AdjustConfig locateConfig, AdjustTypeEnum adjustTypeEnum)
        {
            this.InitializeComponent();

            this.locateUseConfig1 = locateConfig.LocateUseConfig1;

            this.locateUseConfig2 = locateConfig.LocateUseConfig2;

            this.adjustTypeEnum = adjustTypeEnum;
        }

        /// <summary>
        /// 定位配置
        /// </summary>
        private readonly LocateConfig locateConfig;

        /// <summary>
        /// 定位配置1
        /// </summary>
        private readonly LocateUseConfig locateUseConfig1;

        /// <summary>
        /// 定位配置2
        /// </summary>
        private readonly LocateUseConfig locateUseConfig2;

        /// <summary>
        /// 定位配置3
        /// </summary>
        private readonly LocateUseConfig locateUseConfig3;

        /// <summary>
        /// 定位配置3
        /// </summary>
        private readonly LocateUseConfig locateUseConfig4;

        /// <summary>
        /// 定位类型
        /// </summary>
        private readonly AdjustTypeEnum adjustTypeEnum;

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            this.locateUseConfig1.UseX = this.CkUseX1.Checked;
            this.locateUseConfig1.UseY = this.CkUseY1.Checked;
            this.locateUseConfig1.UseAngle = this.CkUseAngle1.Checked;
            this.locateUseConfig1.Autofocus = this.CkAutofocus1.Checked;


            this.locateUseConfig2.UseX = this.CkUseX2.Checked;
            this.locateUseConfig2.UseY = this.CkUseY2.Checked;
            this.locateUseConfig2.UseAngle = this.CkUseAngle2.Checked;
            this.locateUseConfig2.Autofocus = this.CkAutofocus2.Checked;


            if (locateUseConfig3 != null)
            {
                this.locateUseConfig3.UseX = this.CkUseX3.Checked;
                this.locateUseConfig3.UseY = this.CkUseY3.Checked;
                this.locateUseConfig3.UseAngle = this.CkUseAngle3.Checked;
                this.locateUseConfig3.Autofocus = this.CkAutofocus3.Checked;
            }

            if (this.locateUseConfig4 != null)
            {
                this.locateUseConfig4.UseX = this.CkUseX4.Checked;
                this.locateUseConfig4.UseY = this.CkUseY4.Checked;
                this.locateUseConfig4.UseAngle = this.CkUseAngle4.Checked;
                this.locateUseConfig4.Autofocus = this.CkAutofocus4.Checked;
            }
         
            this.DialogResult = DialogResult.OK;

            if (this.locateConfig != null)
            {
                this.locateConfig.IsAroundLocate = this.CKIsAround.Checked;

                this.locateConfig.AroundLocateDistance = (int)this.SpAroundDistance.Value;

                this.locateConfig.IsCalculateAngleByTwoPoint = this.CkIsCalculateAngelByTwoPoint.Checked;

                this.locateConfig.IsThreePointFittingCircle = this.CkIsCalculateCenterByThreePoint.Checked;
            }
            else
            {
                this.CKIsAround.Visible = false;
                this.SpAroundDistance.Visible = false;
                this.CkIsCalculateAngelByTwoPoint.Visible = false;
                this.CkIsCalculateCenterByThreePoint.Visible = false;

            }

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 事件加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmLocateUseInfo_Load(object sender, EventArgs e)
        {
            if (this.adjustTypeEnum == AdjustTypeEnum.None)
            {
                this.DialogResult = DialogResult.OK;
            }
            else if (this.adjustTypeEnum == AdjustTypeEnum.OnePoint)
            {
                this.groupControl2.Visible = false;
                this.groupControl3.Visible = false;
                this.groupControl4.Visible = false;
            }
            else if (this.adjustTypeEnum == AdjustTypeEnum.TwoPoints)
            {
                this.groupControl3.Visible = false;
                this.groupControl4.Visible = false;
            }
            else if (this.adjustTypeEnum == AdjustTypeEnum.ThreePoints)
            {
                this.groupControl4.Visible = false;
            }

            this.CkUseX1.Checked = this.locateUseConfig1.UseX;
            this.CkUseY1.Checked = this.locateUseConfig1.UseY;
            this.CkUseAngle1.Checked = this.locateUseConfig1.UseAngle;
            this.CkAutofocus1.Checked = this.locateUseConfig1.Autofocus;

            this.CkUseX2.Checked = this.locateUseConfig2.UseX;
            this.CkUseY2.Checked = this.locateUseConfig2.UseY;
            this.CkUseAngle2.Checked = this.locateUseConfig2.UseAngle;
            this.CkAutofocus2.Checked = this.locateUseConfig2.Autofocus;

            if (this.locateUseConfig3 != null)
            {
                this.CkUseX3.Checked = this.locateUseConfig3.UseX;
                this.CkUseY3.Checked = this.locateUseConfig3.UseY;
                this.CkUseAngle3.Checked = this.locateUseConfig3.UseAngle;
                this.CkAutofocus3.Checked = this.locateUseConfig3.Autofocus;
            }

            if (this.locateUseConfig4 != null)
            {
                this.CkUseX4.Checked = this.locateUseConfig4.UseX;
                this.CkUseY4.Checked = this.locateUseConfig4.UseY;
                this.CkUseAngle4.Checked = this.locateUseConfig4.UseAngle;
                this.CkAutofocus4.Checked = this.locateUseConfig4.Autofocus;
            }

            if (this.locateConfig != null)
            {
                this.CKIsAround.Checked = this.locateConfig.IsAroundLocate;

                this.SpAroundDistance.Value = (int)this.locateConfig.AroundLocateDistance;

                if (this.CKIsAround.Checked)
                {
                    this.SpAroundDistance.Enabled = true;
                }
                else
                {
                    this.SpAroundDistance.Enabled = false;
                }

                // 两点确定角度
                this.CkIsCalculateAngelByTwoPoint.Checked = this.locateConfig.IsCalculateAngleByTwoPoint;

                this.CkIsCalculateCenterByThreePoint.Checked = this.locateConfig.IsThreePointFittingCircle;
            }
            else
            {
                this.CKIsAround.Visible = false;
                this.SpAroundDistance.Visible = false;
                this.CkIsCalculateAngelByTwoPoint.Visible = false;
                this.CkIsCalculateCenterByThreePoint.Visible = false;
                this.labelControl1.Visible = false;
                this.labelControl2.Visible = false;
            }
        }

        /// <summary>
        /// 四周定位是否开启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CKIsAround_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CKIsAround.Checked)
            {
                this.SpAroundDistance.Enabled = true;
            }
            else
            {
                this.SpAroundDistance.Enabled = false;
            }
        }

        /// <summary>
        /// 两点确定角度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CkIsCalculateAngelByTwoPoint_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.locateConfig.AdjustType != AdjustTypeEnum.TwoPoints)
            //{
            //    this.CkIsCalculateAngelByTwoPoint.Checked = false;
            //}
        }
    }
}