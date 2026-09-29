using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram
{
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Programs;

    public partial class UcFlipToolsDescreption : DevExpress.XtraEditors.XtraUserControl
    {
        public UcFlipToolsDescreption()
        {
            InitializeComponent();
            this.RefreshControl();
        }

        /// <summary>
        /// 界面刷新
        /// </summary>
        private void RefreshControl()
        {
            //if (string.IsNullOrEmpty(WaferSystemProgram.GetInstance().FlipModuleProgram.NozzleName))
            //{
            //    this.labelControl1.Text =
            //        $"Assistant for Flip tools \r\n   Further tools are generated int the configuration \r\n   Details can be found in the help";
            //}
            //else
            //{
            //    this.labelControl1.Text =
            //        $"PP tool for flip tool Z height determination: {WaferSystemProgram.GetInstance().FlipModuleProgram.NozzleName}";
            //}
        }
    }
}
