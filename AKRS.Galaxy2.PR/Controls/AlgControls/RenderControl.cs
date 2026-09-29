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
using VMControls.Winform.Release;
using VMControls.WPF;

namespace AKRS.Galaxy2.PR.Controls.AlgControls
{
    /// <summary>
    /// VM流程结果输出界面
    /// </summary>
    public partial class RenderControl : UserControl
    {
        public RenderControl()
        {
            InitializeComponent();
            vmRenderControl1.AutoSize = true;
        }

        public IVmModule  ModuleSoure;

        // 绑定vm流程输出模块
        public IVmModule ModuleSource
        {
            get 
            { 
                return  ModuleSoure; 
            }
            set
            {
                vmRenderControl1.ModuleSource = value;
                ModuleSoure = value;
            }
        }

        // 绘制十字线
        public void AddShape()
        {
            var lineY = new LineEx(new System.Windows.Point(1224, 0), new System.Windows.Point(1224, 2048), stroke: "#4400FF");
            var lineX = new LineEx(new System.Windows.Point(0,1024), new System.Windows.Point(2448,1024), stroke: "#4400FF");
            vmRenderControl1.AddShape(lineX);
            vmRenderControl1.AddShape(lineY);
        }
    }
}
