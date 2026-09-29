using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    using WindowsFormsApp1.Helper;

    public partial class FrmTest : Form
    {
        public FrmTest()
        {
            InitializeComponent();
        }

        private Movements movements = new Movements();

        /// <summary>
        /// 移到起始位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton4_Click(object sender, EventArgs e)
        {
            movements.MoveToStartPos();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            movements.AbsoluteMove();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            movements.InterpolationMovementWithFourAxis(false);
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            movements.InterpolationMovementWithFourAxis(true);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            movements.MovementWithoutWait();
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            movements.PVTWithFourAxis();
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {
            movements.PTWithFourAxis();
        }
    }
}
