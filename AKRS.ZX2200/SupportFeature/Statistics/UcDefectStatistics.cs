using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using DevExpress.XtraCharts;
    using DevExpress.XtraRichEdit.Model;
    using System.Windows.Forms;

    /// <summary>
    /// 检测的曲线
    /// </summary>
    public partial class UcDefectStatistics : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 刷新事件
        /// </summary>
        public static Action RefreshCapacityStatisticsAction { get; set; }

        /// <summary>
        /// 统计焊后集合
        /// </summary>
        public List<DefectStatisticsEntity> DefectStatisticsEntities { get; set; }

        /// <summary>
        /// 检测
        /// </summary>
        public UcDefectStatistics()
        {
            this.InitializeComponent();
            this.gridView1.Columns[0].DisplayFormat.FormatString = "yyyy-MM-dd HH:mm:ss";
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcDefectStatistics_Load(object sender, EventArgs e)
        {
            RefreshCapacityStatisticsAction += new Action(this.RefreshData);

            List<PostBondInspection> postBondInspections = BondProgram.GetInstance().PostBondProgram.PostBondInspections;

            foreach (PostBondInspection postBondInspection in postBondInspections)
            {
                this.CmbDefectName.Properties.Items.Add(postBondInspection.Name);
            }

            DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            this.DoStartTime.EditValue = dateTime;
            this.DoEndTime.EditValue = dateTime.AddDays(1);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            if (this.CmbDefectName.SelectedItem == null)
            {
                return;
            }

            List<DefectStatisticsEntity> list = StatisticsDomain.GetInstance().QueryDefect(
                this.CmbDefectName.SelectedItem.ToString(),
                this.DoStartTime.DateTimeOffset.DateTime,
                this.DoEndTime.DateTimeOffset.DateTime);


            this.DefectStatisticsEntities = list;
            this.gridControl1.DataSource = list;
            this.RefreshLines(list);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="list">集合</param>
        private void RefreshLines(List<DefectStatisticsEntity> list)
        {
            this.chartControl1.Series.Clear();
            this.chartControl2.Series.Clear();
            this.chartControl3.Series.Clear();
            this.chartControl4.Series.Clear();
            DevExpress.XtraCharts.Series s1 = new DevExpress.XtraCharts.Series("X精度", ViewType.Line);
            DevExpress.XtraCharts.Series s2 = new DevExpress.XtraCharts.Series("Y精度", ViewType.Line);
            DevExpress.XtraCharts.Series s3 = new DevExpress.XtraCharts.Series("角度精度", ViewType.Line);
            DevExpress.XtraCharts.Series s4 = new DevExpress.XtraCharts.Series("面积精度", ViewType.Line);

            for (int i = 0; i < list.Count; i++)
            {
                s1.Points.Add(new SeriesPoint(i, list[i].OffsetX));
                s2.Points.Add(new SeriesPoint(i, list[i].OffsetY));
                s3.Points.Add(new SeriesPoint(i, list[i].OffsetAngle));
                s4.Points.Add(new SeriesPoint(i, list[i].Area));
            }

            this.chartControl1.Series.Add(s1);
            this.chartControl2.Series.Add(s2);
            this.chartControl3.Series.Add(s3);
            this.chartControl4.Series.Add(s4);
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbDefectName_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 开始时间发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoStartTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        /// <summary>
        /// 结束时间发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void DoEndTime_EditValueChanged(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        private void BtExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            fileDialog.FileName = this.CmbDefectName.SelectedItem.ToString();
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.gridControl1.ExportToXls(fileDialog.FileName.Replace(".xls", ".xls"));
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
        /// 导出所有数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtImportAllData_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            fileDialog.FileName = this.CmbDefectName.SelectedItem.ToString();
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    List<DefectStatisticsEntity> list = StatisticsDomain.GetInstance().QueryDefect(
                this.CmbDefectName.SelectedItem.ToString(),
                this.DoStartTime.DateTimeOffset.DateTime,
                this.DoEndTime.DateTimeOffset.DateTime);

                    var properties = typeof(DefectStatisticsEntity).GetProperties();
                    List<List<object>> ob = new List<List<object>>();

                    List<object> listObName = new List<object>();
                    foreach (System.Reflection.PropertyInfo info in properties)
                    {
                        listObName.Add(info.Name);
                    }
                    ob.Add(listObName);

                    foreach (DefectStatisticsEntity defectStatisticsEntity in list)
                    {
                        List<object> listOb = new List<object>();
                        foreach (var property in properties)
                        {
                            listOb.Add(property.GetValue(defectStatisticsEntity));
                        }

                        ob.Add(listOb);
                    }

                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    FileHelper.SaveExcel(ob, fileDialog.FileName.Replace(".xls", ".xls"));
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
    }
}
