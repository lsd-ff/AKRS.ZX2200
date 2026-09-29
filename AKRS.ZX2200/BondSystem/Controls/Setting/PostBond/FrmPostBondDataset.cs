using AKRS.Galaxy2.PR.Models.Algs;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    public partial class FrmPostBondDataset : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 允许的焊后检测X偏差
        /// </summary>
        public double TolerantCenterX { get; set; }

        /// <summary>
        /// 允许的焊后检测Y偏差
        /// </summary>
        public double TolerantCenterY { get; set; }

        /// <summary>
        /// 允许的焊后检测角度偏差
        /// </summary>
        public double TolerantCenterAngle { get; set; }

        /// <summary>
        /// 允许的焊后胶量最小值%
        /// </summary>
        public double TolerantEpoxyMin { get; set; }

        /// <summary>
        /// 允许的焊后胶量最大值%
        /// </summary>
        public double TolerantEpoxyMax { get; set; }

        public FrmPostBondDataset(PostBondInspection postBondInspection)
        {
            
            InitializeComponent();

            //this.SpToleranceXVal.Value = (decimal)postBondInspection.PBIWarningLimit.X * 1000;
            //this.SpToleranceYVal.Value = (decimal)postBondInspection.PBIWarningLimit.Y *1000;
            //this.SpToleranceAngVal.Value = (decimal)postBondInspection.PBIWarningLimit.T;

            this.SpMinEpoxyVal.Value = (decimal)postBondInspection.TolerantEpoxyMin*100;
            this.SpMaxEpoxyVal.Value = (decimal)postBondInspection.TolerantEpoxyMax*100;

            this.TbEpoxyValMin.Value = (int)this.SpMinEpoxyVal.Value;
            this.TbEpoxyValMax.Value = (int)this.SpMaxEpoxyVal.Value;


            if (postBondInspection.ApplicationSystem == PostBondApplicationSystemEnum.AfterBondCheck)
            {
                this.SpMinEpoxyVal.Visible=false;
                this.SpMaxEpoxyVal.Visible=false;

                this.TbEpoxyValMin.Visible = false;
                this.TbEpoxyValMax.Visible = false;

                lbMinEpoxyVal.Visible = false;
                lbMaxEpoxyVal.Visible = false;

                lbpercentMax.Visible = false;
                lbpercentMin.Visible = false;
            }
            else
            {
                lbToleranceAngVal.Visible = false;
                lbToleranceAngDeg.Visible = false;
                this.SpToleranceAngVal.Visible = false;
            }

        }


        private void FrmPostBondDataset_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.TolerantCenterX = (double)this.SpToleranceXVal.Value/1000;
            this.TolerantCenterY = (double) this.SpToleranceYVal.Value/1000;
            this.TolerantCenterAngle=(double)this.SpToleranceAngVal.Value;
            this.TolerantEpoxyMin = (double)this.SpMinEpoxyVal.Value/100;
            this.TolerantEpoxyMax = (double)this.SpMaxEpoxyVal.Value/100;
        }

        private void TbEpoxyValMin_EditValueChanged(object sender, EventArgs e)
        {
            this.SpMinEpoxyVal.Value = this.TbEpoxyValMin.Value;
        }

        private void TbEpoxyValMax_EditValueChanged(object sender, EventArgs e)
        {
            this.SpMaxEpoxyVal.Value = this.TbEpoxyValMax.Value;
        }
    }
}
