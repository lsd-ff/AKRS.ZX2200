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

namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    using System.Threading;

    public partial class FrmTimerTest : DevExpress.XtraEditors.XtraForm
    {
        public FrmTimerTest()
        {
            InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread.Sleep(1000);
        }
    }
}
