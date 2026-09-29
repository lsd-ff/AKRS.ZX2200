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

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using AKRS.ZX2200.Infrastructure.Service;
    using DevExpress.CodeParser;
    using LanguageExt.UnitsOfMeasure;

    /// <summary>
    /// 生产统计
    /// </summary>
    public partial class UcProductStatistics : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 生产统计
        /// </summary>
        public UcProductStatistics()
        {
            this.InitializeComponent();
            //this.gridView1.Columns[2].DisplayFormat.FormatString = "HH:mm:ss";
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcProductStatistics_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (this.DoStartTime.DateTimeOffset.DateTime > this.DoEndTime.DateTimeOffset.DateTime)
            {
                return;
            }

            List<ProductTimeStatisticsEntity> list = StatisticsDomain.GetInstance().QueryProductTime(
                this.DoStartTime.DateTimeOffset.DateTime,
                this.DoEndTime.DateTimeOffset.DateTime);
            
            this.gridControl1.DataSource = list;

            this.chartControl1.Series.Clear();

            Series s1 = new DevExpress.XtraCharts.Series($"自动工作", ViewType.Pie);

            int index = 0;

            // 遍历所有集合
            for (int i = 0; i < list.Count; i++)
            {
                // 如果是第一个,判断第一个的开始时间是否超出范围
                if (i == 0)
                {
                    if (list[0].StartTime < this.DoStartTime.DateTimeOffset.DateTime)
                    {
                        double offsetTime = (this.DoStartTime.DateTimeOffset.DateTime - list[0].StartTime).TotalMinutes
                                            / 60.0;

                        s1.Points.Add(new SeriesPoint($"自动工作", list[i].ProductTime - offsetTime));
                        s1.Points[i].Color = Color.LightGreen;
                    }
                    else if (list[0].StartTime > this.DoStartTime.DateTimeOffset.DateTime)
                    {
                        double offsetTime = (this.DoStartTime.DateTimeOffset.DateTime - list[0].StartTime).TotalMinutes
                                            / 60.0;

                        s1.Points.Add(new SeriesPoint("停止工作", list[0].ProductTime - offsetTime));

                        s1.Points[0].Color = Color.DarkGray;
                        index++;

                        s1.Points.Add(new SeriesPoint("自动工作", list[0].ProductTime));

                        s1.Points[index].Color = Color.LightGreen;
                        index++;
                    }
                }
                else
                {
                    s1.Points.Add(new SeriesPoint("自动工作", list[i].ProductTime));
                    s1.Points[index].Color = Color.LightGreen;
                    index++;

                    double hour = (list[i].EndTime - list[i - 1].StartTime).TotalMinutes / 60.0;

                    s1.Points.Add(new SeriesPoint("停止工作", hour));
                    s1.Points[index].Color = Color.DarkGray;
                    index++;
                }
            }

            s1.LegendTextPattern = "{A}";
            
            this.chartControl1.Series.Add(s1);
        }
        
        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoStartTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoEndTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Random random = new Random();

            DateTime dateTime = DateTime.Now;

            for (int i = 0; i < 100; i++)
            {
                double offset = random.Next(100);

                ProductTimeStatisticsEntity productTimeStatisticsEntity = new ProductTimeStatisticsEntity(
                    dateTime,
                    dateTime.AddMinutes(offset),
                    (dateTime.AddMinutes(offset) - dateTime).Minutes / 60.0);

                dateTime = dateTime.AddMinutes(offset);

                DBService.Insert(productTimeStatisticsEntity);
            }
        }
    }
}
