namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System.Collections.Generic;
    using System.Drawing;

    using DevExpress.XtraCharts;

    /// <summary>
    /// 旋转中心展示界面
    /// </summary>
    public partial class PointShow : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 旋转中心展示界面
        /// </summary>
        public PointShow()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 旋转中心展示界面
        /// </summary>
        /// <param name="list">数据集合</param>
        public PointShow(List<PointF> list)
        {
            this.InitializeComponent();

            this.chartControl1.Series.Clear();

            List<Series> seriesList = new List<Series>();

            Series series = new Series("Rotate Ponit", ViewType.RadarPoint);

           

            for (int i = 0; i < list.Count; i++)
            {
                series.Points.Add(new SeriesPoint(list[i].X, list[i].Y));
            }

            seriesList.Add(series);
            this.chartControl1.Series.AddRange(seriesList.ToArray());
        }

        /// <summary>
        /// 页面展示
        /// </summary>
        /// <param name="list">集合</param>
        public void InitPoint(List<PointF> list)
        {
            this.chartControl1.Series.Clear();

            List<Series> seriesList = new List<Series>();

            Series series = new Series("Rotate Ponit", ViewType.RadarPoint);

            for (int i = 0; i < list.Count; i++)
            {
                series.Points.Add(new SeriesPoint(list[i].X, list[i].Y));
            }

            seriesList.Add(series);
            this.chartControl1.Series.AddRange(seriesList.ToArray());
        }
    }
}