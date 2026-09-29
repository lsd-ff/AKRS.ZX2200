using DevExpress.XtraCharts;
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

namespace AKRS.ZX2200.SupportFeature.RealTimeDisplay
{
    /// <summary>
    /// 实时测高
    /// </summary>
    public partial class UcRealTimeHeight : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 实时测高
        /// </summary>
        public UcRealTimeHeight()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 高度数据
        /// </summary>
        private readonly Queue<double> list = new Queue<double>();

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.ChartPostBondResX.Series.Clear();
            DevExpress.XtraCharts.Series s1 = new DevExpress.XtraCharts.Series("高度(um)", ViewType.Line);

            double[] values = this.list.ToArray();

            for (int i = 0; i < values.Length; i++)
            {
                s1.Points.Add(new SeriesPoint(i, values[i]));
            }
            
            this.ChartPostBondResX.Series.Add(s1);
        }

        /// <summary>
        /// 添加高度数据
        /// </summary>
        /// <param name="height">高度</param>
        public void AddHeightData(double height)
        {
            if (this.Created && !this.IsDisposed && !this.Disposing)
            {
                this.list.Enqueue(height);
                if (this.list.Count > 200)
                {
                    this.list.Dequeue();
                }

                this.Invoke(this.RefreshData);
            }
        }
    }
}
