using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Akrs.ChangeJson
{
    public partial class FrmMain : DevExpress.XtraEditors.XtraForm
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            UcChangeNameSpace ucChangeNameSpace = new UcChangeNameSpace(){Dock =DockStyle.Fill};  
            this.xtraTabPage1.Controls.Add(ucChangeNameSpace);  
        }
    }
}
