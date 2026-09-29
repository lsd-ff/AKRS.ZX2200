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
    using Accord.Math;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using DevExpress.Charts.Native;
    using DevExpress.DashboardWin.Native;
    using DevExpress.DataAccess.Wizard.Presenters;
    using DevExpress.Office.Utils;
    using DevExpress.XtraCharts;
    using log4net.Core;
    using SqlSugar;
    using System.Threading.Tasks.Dataflow;

    using ViewType = DevExpress.XtraCharts.ViewType;

    /// <summary>
    /// 产能统计
    /// </summary>
    public partial class UcCapacityStatistics : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 刷新事件
        /// </summary>
        public static Action<BondPositionLogEntity> RefreshCapacityStatisticsAction { get; set; }

        /// <summary>
        /// 白班统计
        /// </summary>
        private CapacityStatistics[] DaytimeCapacityStatistics = new CapacityStatistics[12];

        /// <summary>
        /// 夜班统计
        /// </summary>
        private CapacityStatistics[] NightCapacityStatistics = new CapacityStatistics[12];

        /// <summary>
        /// 事件加载时发生
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcCapacityStatistics_Load(object sender, EventArgs e)
        {
            this.DateDue.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, StatisticsDomain.GetInstance().DayStart.Hour, StatisticsDomain.GetInstance().DayStart.Minute, 0);

            this.DeQurtyStartTime.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            this.DeQurtyEndTime.EditValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(1);

            this.gridView3.Columns[0].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";

            // 设置表1的行间距
            
            
            RefreshCapacityStatisticsAction = new Action<BondPositionLogEntity>((bondPositionLogEntity) => { this.InputData(bondPositionLogEntity); });

            DevExpress.XtraCharts.XYDiagram xyDiagram = (DevExpress.XtraCharts.XYDiagram)this.chartControl1.Diagram;
            xyDiagram.AxisX.Label.TextPattern = "{A:HH:mm}";
            xyDiagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
            xyDiagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
            xyDiagram.AxisX.DateTimeScaleOptions.MeasureUnitMultiplier = 30;
            xyDiagram.AxisX.DateTimeScaleOptions.GridLayoutMode = GridLayoutMode.GridAndLabelCentered;
            xyDiagram.AxisX.GridLines.Visible = true;

            DevExpress.XtraCharts.XYDiagram xyDiagram2 = (DevExpress.XtraCharts.XYDiagram)this.chartControl2.Diagram;
            xyDiagram2.AxisX.Label.TextPattern = "{A:HH:mm}";
            xyDiagram2.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Minute;
            xyDiagram2.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Hour;
            xyDiagram2.AxisX.DateTimeScaleOptions.MeasureUnitMultiplier = 30;
            xyDiagram2.AxisX.DateTimeScaleOptions.GridLayoutMode = GridLayoutMode.GridAndLabelCentered;
            xyDiagram2.AxisX.GridLines.Visible = true;
        }

        /// <summary>
        /// 产能统计
        /// </summary>
        public UcCapacityStatistics()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 输入数据
        /// </summary>
        /// <param name="bondPositionLogEntity">焊点结果</param>
        private void InputData(BondPositionLogEntity bondPositionLogEntity)
        {
            if (this.IsHandleCreated && !this.Disposing)
            {
                try
                {
                    this.BeginInvoke(
                        new Action(
                            () =>
                            {
                                int[] index1 = this.DaytimeCapacityStatistics.Find(it => it.StartTime <= bondPositionLogEntity.DateTime && it.StartTime.AddHours(1) > bondPositionLogEntity.DateTime);
                                if (index1.Length == 1)
                                {
                                    this.DaytimeCapacityStatistics[index1[0]].TotalNumber++;
                                }

                                int[] index2 = this.NightCapacityStatistics.Find(it => it.StartTime <= bondPositionLogEntity.DateTime && it.StartTime.AddHours(1) > bondPositionLogEntity.DateTime);
                                if (index2.Length == 1)
                                {
                                    this.NightCapacityStatistics[index2[0]].TotalNumber++;
                                }

                                this.RefreshChart();
                            }));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns>数据</returns>
        private void GetCapacityLogEntities()
        {
            DateTime dateTime1 = this.DateDue.DateTimeOffset.DateTime;

            DateTime queryTime = new DateTime(
                dateTime1.Year,
                dateTime1.Month,
                dateTime1.Day,
                dateTime1.Hour,
                dateTime1.Minute,
                0,
                0);

            List<BondPositionLogEntity> listAll = StatisticsDomain.GetInstance()
                .QueryBondPositionLog(queryTime, queryTime.AddHours(24));

            this.DaytimeCapacityStatistics = new CapacityStatistics[12];
            this.NightCapacityStatistics = new CapacityStatistics[12];

            for (int i = 0; i < 12; i++)
            {
                this.DaytimeCapacityStatistics[i] = new CapacityStatistics() { StartTime = queryTime.AddHours(i) };
                this.NightCapacityStatistics[i] = new CapacityStatistics() { StartTime = queryTime.AddHours(i + 12) };
            }

            foreach (BondPositionLogEntity positionLogEntity in listAll)
            {
                int hour = (positionLogEntity.DateTime - queryTime).Hours;

                if (hour < 12)
                {
                    this.DaytimeCapacityStatistics[hour].TotalNumber++;
                }
                else if(hour < 24)
                {
                    this.NightCapacityStatistics[hour - 12].TotalNumber++;
                }
            }
        }

        /// <summary>
        /// 刷新图标
        /// </summary>
        /// <param name="list1">白班</param>
        /// <param name="list2">夜班</param>
        private void RefreshChart()
        {
            this.gridControl1.DataSource = this.DaytimeCapacityStatistics;
            this.gridControl2.DataSource = this.NightCapacityStatistics;

            this.chartControl1.Series.Clear();
            this.chartControl2.Series.Clear();
            DevExpress.XtraCharts.Series s1 = new DevExpress.XtraCharts.Series("白班", ViewType.Bar);
            DevExpress.XtraCharts.Series s2 = new DevExpress.XtraCharts.Series("夜班", ViewType.Bar);
            s1.ArgumentScaleType = ScaleType.DateTime;
            s2.ArgumentScaleType = ScaleType.DateTime;

            for (int i = 0; i < 12; i++)
            {
                s1.Points.Add(new SeriesPoint(this.DaytimeCapacityStatistics[i].StartTime, this.DaytimeCapacityStatistics[i].TotalNumber));
                s2.Points.Add(new SeriesPoint(this.NightCapacityStatistics[i].StartTime, this.NightCapacityStatistics[i].TotalNumber));
            }

            this.chartControl1.Series.Add(s1);
            this.chartControl2.Series.Add(s2);
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.gridControl1.ExportToXls(fileDialog.FileName.Replace(".xls", "白班.xls"));
                    this.gridControl2.ExportToXls(fileDialog.FileName.Replace(".xls", "夜班.xls"));
                    AKRSXtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("数据导出失败", "提示");
                    }
                }
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtExportData_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.gridControl3.ExportToXls(fileDialog.FileName.Replace(".xls", ".xls"));
                    AKRSXtraMessageBox.Show("保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("数据导出失败", "提示");
                    }
                }
            }
        }

        /// <summary>
        /// 查询工作时间
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtQuery_Click(object sender, EventArgs e)
        {
            List<BondPositionLogEntity> listAll = StatisticsDomain.GetInstance()
              .QueryBondPositionLog(this.DeQurtyStartTime.DateTimeOffset.DateTime, this.DeQurtyEndTime.DateTimeOffset.DateTime).OrderBy(it => it.DateTime).ToList();

            List<CapacityStatistics> list = new List<CapacityStatistics>();

            // 根据时间段和间隔统计数量
            int totalHours= (int)((this.DeQurtyEndTime.DateTimeOffset.DateTime - this.DeQurtyStartTime.DateTimeOffset.DateTime).TotalHours / (double)this.SpForceLowerLimit.Value);

            for (int i = 0; i < totalHours; i++)
            {
                list.Add(new CapacityStatistics() { StartTime = this.DeQurtyStartTime.DateTimeOffset.DateTime.AddHours(i * (double)this.SpForceLowerLimit.Value)});
            }

            for (int i = 0; i < listAll.Count; i++)
            {
                CapacityStatistics capacityStatistics = list.Find(it => it.StartTime <= listAll[i].DateTime && it.StartTime.AddMinutes((double)this.SpForceLowerLimit.Value) > listAll[i].DateTime);
                if (capacityStatistics != null)
                {
                    capacityStatistics.TotalNumber++;
                }
            }

            this.gridControl3.DataSource = list;
        }

        /// <summary>
        /// 查询全天数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtQueryWhiteAndNight_Click(object sender, EventArgs e)
        {
            this.GetCapacityLogEntities();
            this.RefreshChart();
        }
    }

    /// <summary>
    /// 生产统计
    /// </summary>
    public class CapacityStatistics()
    {
        /// <summary>
        /// 统计开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 总共生产的数量
        /// </summary>
        public int TotalNumber { get; set; } = 0;
    }
}
