using System;
using System.Collections.Generic;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using DevExpress.XtraCharts;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.SupportFeature.Statistics;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate;

    using DevExpress.ExpressApp.Utils;
    using DevExpress.XtraEditors;

    using Task = System.Threading.Tasks.Task;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 焊后检测结果显示
    /// </summary>
    public partial class FrmPostBondResCurve : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmPostBondResCurve()
        {
            this.InitializeComponent();
            this.InitControl();
            DefectCompensate.RePostBondInspection += this.AddDefectData;
        }

        /// <summary>
        /// 当前程式的焊后检测集合
        /// </summary>
        private List<PostBondInspection> PostBondInspections =>
            BondProgram.GetInstance().PostBondProgram.PostBondInspections;

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitControl()
        {
            // 绑定数据源
            this.CmbPostBond.Properties.Items.Clear();
            foreach (var bp in this.PostBondInspections)
            {
                this.CmbPostBond.Properties.Items.Add(bp.Name);
            }

            if (this.CmbPostBond.Properties.Items.Count != 0)
            {
                this.CmbPostBond.SelectedItem = this.PostBondInspections[0].Name;
                this.CmbPostBond.Text = this.PostBondInspections[0].Name;
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="defectStatisticsEntity">数据</param>
        private void AddDefectData(DefectStatisticsEntity defectStatisticsEntity)
        {
            Task.Run(
                () =>
                    {
                        if (!this.Created || this.Disposing)
                        {
                            return;
                        }

                        this.Invoke(this.ReFreshData);
                    });
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void ReFreshData()
        {
            List<DefectStatisticsEntity> list = this.GetDataSource();
            this.GcPostBondRes.DataSource = list;
            this.GcPostBondRes.RefreshDataSource();
            this.RefreshLine(list);
        }
        
        /// <summary>
        /// 刷新曲线
        /// </summary>
        /// <param name="list">名称</param>
        private void RefreshLine(List<DefectStatisticsEntity> list)
        {
            if (list == null)
            {
                this.ChartPostBondResX.Series.Clear();
                this.ChartPostBondResX.Series.Clear();
                this.ChartPostBondResAngle.Series.Clear();
                return;
            }

            this.ChartPostBondResX.Series.Clear();
            this.ChartPostBondResX.Series.Clear();
            this.ChartPostBondResAngle.Series.Clear();
            DevExpress.XtraCharts.Series s1 = new DevExpress.XtraCharts.Series("X精度", ViewType.Line);
            DevExpress.XtraCharts.Series s2 = new DevExpress.XtraCharts.Series("Y精度", ViewType.Line);

            DevExpress.XtraCharts.Series s5 = new DevExpress.XtraCharts.Series("X温漂补偿值", ViewType.Line);
            DevExpress.XtraCharts.Series s6 = new DevExpress.XtraCharts.Series("Y温漂补偿值", ViewType.Line);
            DevExpress.XtraCharts.Series s3 = new DevExpress.XtraCharts.Series("角度精度", ViewType.Line);

            for (int i = 0; i < list.Count; i++)
            {
                s1.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetX ));
                s2.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetY ));
                s3.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetAngle));

                s5.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].TpOffsetX * 1000));
                s6.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].TpOffsetY * 1000));
            }

            this.ChartPostBondResX.Series.Add(s1);
            this.ChartPostBondResX.Series.Add(s2);
            this.ChartPostBondResX.Series.Add(s5);
            this.ChartPostBondResX.Series.Add(s6);
            this.ChartPostBondResAngle.Series.Add(s3);
        }

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <returns>结果</returns>
        private List<DefectStatisticsEntity> GetDataSource()
        {
            if (this.CmbPostBond.SelectedItem == null)
            {
                return null;
            }

            string defectName = this.CmbPostBond.SelectedItem.ToString();

            if (DefectCompensate.DefectDataS.ContainsKey(defectName))
            {
                return DefectCompensate.DefectDataS[defectName];
            }

            return null;
        }

        /// <summary>
        /// 窗体出现时发生
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmPostBondResCurve_Shown(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 当选择发生变化时发生
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbPostBond_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ReFreshData();
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
                    this.GvPostBondRes.ExportToXls(fileDialog.FileName);
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
        /// 清空结果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearResult_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("清除数据焊后补偿将失效，是否执行", "提示", MessageBoxButtons.OKCancel);

            if (dialogResult == DialogResult.OK)
            {
                DefectCompensate.DefectDataS.Clear();
                this.ReFreshData();
            }
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmPostBondResCurve_FormClosed(object sender, FormClosedEventArgs e)
        {
            DefectCompensate.RePostBondInspection -= this.AddDefectData;
        }
    }
}