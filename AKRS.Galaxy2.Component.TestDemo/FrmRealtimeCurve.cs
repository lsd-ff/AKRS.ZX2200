using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using DevExpress.XtraCharts;

namespace AKRS.Galaxy2.Component.TestDemo
{
    /// <summary>
    /// 压力曲线显示
    /// </summary>
    public partial class FrmRealtimeCurve : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmRealtimeCurve()
        {
            InitializeComponent(); 
            ChartService.InitChartControl(this.chartControl);

            // 设置Y轴的范围
            ChartService.SetYRange(this.chartControl, 0, 20);
            this.timer1.Enabled = true;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rd = new Random();
            int n = rd.Next(10);
            SeriesPoint p = new SeriesPoint(System.DateTime.Now, n);
            ChartService.AddPoint(this.chartControl.Series[0], p);

            n = rd.Next(20);
            System.DateTime dt = System.DateTime.Now;
            SeriesPoint p2 = new SeriesPoint(dt, n);
            ChartService.ClearAixVerConstantLine(this.chartControl);
            ChartService.AddAixVerConstantLine(this.chartControl, "current value", dt, Color.CadetBlue);

            // 自动设置X轴的范围
            ChartService.AutoSetXWholeRange(this.chartControl, 1, 5);
        }
    }
}
