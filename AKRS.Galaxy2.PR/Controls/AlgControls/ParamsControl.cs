using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VMControls.Interface;
using VMControls.WPF;

namespace AKRS.Galaxy2.PR.Controls.AlgControls
{
    /// <summary>
    /// VM参数配置控件
    /// </summary>
    public partial class ParamsControl : UserControl
    {
        public ParamsControl()
        {
            InitializeComponent();
            vmParamsConfigWithRenderControl1.AutoSize = true;
        }

        public IVmModule  ModuleSoure;

        // 绑定vm模块
        public IVmModule ModuleSource
        {
            get
            { 
                return  ModuleSoure; 
            }
            set
            {
                vmParamsConfigWithRenderControl1.ModuleSource = value;
                 ModuleSoure = value;
            }
        }

        public void AddShape()
        {
            var lineY = new LineEx(new System.Windows.Point(1224, 0), new System.Windows.Point(1224, 2048), stroke: "#4400FF");
            var lineX = new LineEx(new System.Windows.Point(0, 1024), new System.Windows.Point(2448, 1024), stroke: "#4400FF");
            vmParamsConfigWithRenderControl1.AddShape(lineX);
            vmParamsConfigWithRenderControl1.AddShape(lineY);
        }
    }
}
