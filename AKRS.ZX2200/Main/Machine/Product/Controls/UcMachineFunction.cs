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

namespace AKRS.ZX2200.Main.Machine.Product.Controls
{
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 设备功能
    /// </summary>
    public partial class UcMachineFunction : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 设备功能
        /// </summary>
        public UcMachineFunction()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcMachineFunction_Load(object sender, EventArgs e)
        {
            this.ChkMarkTemperatureOffset.DataBindings.Add(
                "Checked",
                System2Configuration.GetInstance(),
                "IsActiveDriftCompensate");
        }
    }
}
