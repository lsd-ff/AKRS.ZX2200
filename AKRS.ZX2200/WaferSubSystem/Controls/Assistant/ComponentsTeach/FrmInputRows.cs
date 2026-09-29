using DevExpress.Charts.Native;
using DevExpress.CodeParser;
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

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;

    using DevExpress.XtraBars.Ribbon.Drawing;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmInputRows : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// 载具名称
        /// </summary>
        private CarrierWithWaffleConfig waffleWithWaffleConfigCarrierWithWaffleConfig;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waffleWithWaffleConfigCarrierWithWaffleConfig">waffle TransportUnit</param>
        public FrmInputRows(CarrierWithWaffleConfig waffleWithWaffleConfigCarrierWithWaffleConfig)
        {
            this.InitializeComponent();
            this.waffleWithWaffleConfigCarrierWithWaffleConfig = waffleWithWaffleConfigCarrierWithWaffleConfig;
        }

        /// <summary>
        /// Back按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDone_Click(object sender, EventArgs e)
        {
            this.RowCount = (int)this.SpRows.Value;
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
                    this.DialogResult = DialogResult.Abort;
                    break;
                case DialogResult.No:
                    break;
            }
        }
    }
}