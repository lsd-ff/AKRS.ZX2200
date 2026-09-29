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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant.CarrierTeach
{
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// ID设置
    /// </summary>
    public partial class FrmTUIdentificationTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// ID设置
        /// </summary>
        public FrmTUIdentificationTeach()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 传输单元对象
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmTUIdentificationTeach_Load(object sender, EventArgs e)
        {
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            if (this.transportUnit == null)
            {
                this.Close();
                return;
            }
        }
    }
}