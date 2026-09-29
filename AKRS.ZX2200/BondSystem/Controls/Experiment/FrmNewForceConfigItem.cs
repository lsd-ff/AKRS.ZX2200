using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    /// <summary>
    /// 新建力控配置
    /// </summary>
    public partial class FrmNewForceConfigItem : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNewForceConfigItem(ForceConfigItem forceConfigItem)
        {
            InitializeComponent();
            this.forceConfigItem = forceConfigItem;
        }

        private ForceConfigItem forceConfigItem;

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            forceConfigItem.ForceUpperLimit = (double)this.SpForceUpperLimit.Value;
            forceConfigItem.ForceLowerLimit = (double)this.SpForceLowerLimit.Value;
            forceConfigItem.SmoothTime = (short)this.SpSmoothTime.Value;
            forceConfigItem.Kp = (double)this.SpKp.Value;
            forceConfigItem.Ki = (double)this.SpKi.Value;
            forceConfigItem.Kd = (double)this.SpKd.Value;
            forceConfigItem.Acc = (double)this.SpAcc .Value;
            forceConfigItem.Dec  = (double)this.SpDec.Value;
            forceConfigItem.SlowTouchAcc = (double)this.SpSlowTouchAcc.Value;
            forceConfigItem.SlowTouchDec = (double)this.SpSlowTouchDec.Value;
            forceConfigItem.LowSpeedDuringTouchDown = (double)this.SpLowSpeedDuringTouchDown.Value;

            forceConfigItem.ChangeForceLowerLimit = (double)this.SpLowerChangeForce.Value;
            forceConfigItem.ChangeForceUpperLimit = (short)this.SpUpperChangeForce.Value;

            forceConfigItem.ChangeForceK = ((double)this.SpUpperChangeForce.Value - (double)this.SpLowerChangeForce.Value)
                                           / (forceConfigItem.ForceUpperLimit - forceConfigItem.ForceLowerLimit);
            forceConfigItem.ChangeForceB = (double)this.SpUpperChangeForce.Value
                                           - forceConfigItem.ChangeForceK * forceConfigItem.ForceUpperLimit;

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        ///  取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}