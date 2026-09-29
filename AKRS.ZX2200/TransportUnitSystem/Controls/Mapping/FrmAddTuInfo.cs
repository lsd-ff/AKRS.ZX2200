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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 添加载具的信息
    /// </summary>
    public partial class FrmAddTuInfo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 添加载具的信息
        /// </summary>
        /// <param name="transportUnit">载具</param>
        public FrmAddTuInfo(TransportUnit transportUnit)
        {
            this.InitializeComponent();
            this.transportUnit = transportUnit;
        }

        /// <summary>
        /// 载具
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, EventArgs e)
        {
            if (this.transportUnit == null)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }

            this.transportUnit.TransportUnitInfo.RecipeName = this.TxRecipeName.Text;
            this.transportUnit.TransportUnitInfo.SubstrateLotNumber = this.TxSubstrateLotNumber.Text;
            this.transportUnit.TransportUnitInfo.SubstrateNumber = this.TxSubstrateNumber.Text;
            this.transportUnit.TransportUnitInfo.FaceType = this.TxFaceType.Text;

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAddTuInfo_Load(object sender, EventArgs e)
        {
            if (this.transportUnit == null)
            {
                return;
            }

            if (this.transportUnit.TransportUnitInfo.RecipeName == null)
            {
                this.TxRecipeName.Text = MachineConfigContext.GetInstance().RecipeName;
            }
            
            this.TxSubstrateNumber.Text = this.transportUnit.TransportUnitInfo.SubstrateNumber;
            this.TxFaceType.Text = this.transportUnit.TransportUnitInfo.FaceType;
        }
    }
}