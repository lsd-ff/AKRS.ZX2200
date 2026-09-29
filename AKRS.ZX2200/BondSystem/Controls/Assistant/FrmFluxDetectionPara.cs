using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    /// <summary>
    /// 胶印检测参数设置
    /// </summary>
    public partial class FrmFluxDetectionPara : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="component">芯片</param>
        public FrmFluxDetectionPara(BaseCarrierConfig component)
        {
            InitializeComponent();
            this.component = component;
            this.Init();
        }

        /// <summary>
        /// 芯片
        /// </summary>
        private BaseCarrierConfig component;


        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.SpFluxDistanceX.Value = (decimal)(component.FluxDistanceX * 1000.0);
            this.SpFluxDistanceY.Value = (decimal)(component.FluxDistanceY * 1000.0);
            this.SpFluxAngle.Value = (decimal)component.FluxDistanceAngle;


            this.SpFluxLimitX.Value = (decimal)(component.FluxLimitX * 1000.0);
            this.SpFluxLimitY.Value = (decimal)(component.FluxLimitY * 1000.0);
            this.SpFluxLimitAngle.Value = (decimal)component.FluxLimitAngle;
            this.SpFluxMax.Value = (decimal)component.TolerantFluxMax;
            this.SpFluxMin.Value = (decimal)component.TolerantFluxMin;
        }

        private void BtSure_Click(object sender, EventArgs e)
        {
            try
            {
                this.component.FluxDistanceX = (double)this.SpFluxDistanceX.Value / 1000.0;
                this.component.FluxDistanceY = (double)this.SpFluxDistanceY.Value / 1000.0;
                this.component.FluxDistanceAngle = (double)this.SpFluxAngle.Value;

                //// 有问题
                //this.postBondInspection.PostBondEpoxyArea[0] = (double)this.SpPostBondEpoxy.Value;

                this.component.FluxLimitX = (double)this.SpFluxLimitX.Value / 1000.0;
                this.component.FluxLimitY = (double)this.SpFluxLimitY.Value / 1000.0;
                this.component.FluxLimitAngle = (double)this.SpFluxLimitAngle.Value;
                this.component.TolerantFluxMax = (double)this.SpFluxMax.Value;
                this.component.TolerantFluxMin = (double)this.SpFluxMin.Value;

                CarrierConfigRepository.GetInstance().Save();

                AKRSXtraMessageBox.Show("保存成功");
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show($"保存失败,原因 ：{exception.Message}");
            }
        }
    }
}
