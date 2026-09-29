using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    /// <summary>
    /// 芯片定位配置窗体
    /// </summary>
    public partial class FrmComponentLocateUseInfo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 芯片定位配置
        /// </summary>
        /// <param name="baseCarrierConfig">芯片</param>
        public FrmComponentLocateUseInfo(BaseCarrierConfig baseCarrierConfig)
        {
            InitializeComponent();

            //this.locateConfig = locateConfig;

            //this.locateUseConfig1 = baseCarrierConfig.DieMatchP1LocateUseConfig;

            //this.locateUseConfig2 = baseCarrierConfig.DieMatchP2LocateUseConfig;

            //this.adjustTypeEnum = locateConfig.AdjustType;
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
            this.locateUseConfig2.UseX = this.CkUseX2.Checked;
            this.locateUseConfig2.UseY = this.CkUseY2.Checked;
            this.locateUseConfig2.UseAngle = this.CkUseAngle2.Checked;

            this.DialogResult = DialogResult.OK;

            this.locateConfig.IsAroundLocate = this.CKIsAround.Checked;

            this.locateConfig.AroundLocateDistance = (int)this.SpAroundDistance.Value;

            this.locateConfig.IsCalculateAngleByTwoPoint = this.CkIsCalculateAngelByTwoPoint.Checked;

            this.locateConfig.IsThreePointFittingCircle = this.CkIsCalculateCenterByThreePoint.Checked;

            CarrierConfigRepository.GetInstance().Save();
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
            }

            this.CkUseX1.Checked = this.locateUseConfig1.UseX;
            this.CkUseY1.Checked = this.locateUseConfig1.UseY;
            this.CkUseAngle1.Checked = this.locateUseConfig1.UseAngle;
            this.CkUseX2.Checked = this.locateUseConfig2.UseX;
            this.CkUseY2.Checked = this.locateUseConfig2.UseY;
            this.CkUseAngle2.Checked = this.locateUseConfig2.UseAngle;

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