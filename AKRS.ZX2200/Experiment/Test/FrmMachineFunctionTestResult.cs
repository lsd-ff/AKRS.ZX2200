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

namespace AKRS.ZX2200.Experiment.Test
{
    /// <summary>
    /// 设备功能测试结果显示
    /// </summary>
    public partial class FrmMachineFunctionTestResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmMachineFunctionTestResult(string info)
        {
            InitializeComponent();
            this.info = info;
            this.Load += new System.EventHandler(this.FrmMachineFunctionTestResult_Load);
        }

        private string info;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmMachineFunctionTestResult_Load(object sender, EventArgs e)
        {
            this.richTextBox1.Text = this.info;
            this.richTextBox1.Font= new Font("微软雅黑", 10, FontStyle.Bold);
            this.Size = new Size(565, 327);
        }
    }
}