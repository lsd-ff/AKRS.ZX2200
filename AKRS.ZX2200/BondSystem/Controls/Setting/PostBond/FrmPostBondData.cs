using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.SupportFeature.Compensate.DefectCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
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

namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 焊后数据
    /// </summary>
    public partial class FrmPostBondData : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 焊后数据
        /// </summary>
        public FrmPostBondData()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 焊后数据
        /// </summary>
        private PostBondInspection postBondInspection => (PostBondInspection)PostBondInspectionRepository.GetInstance().Find(this.CmbPostBond.SelectedItem?.ToString());

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
                    if (!this.Created || this.Disposing || this.IsDisposed || !this.IsHandleCreated)
                    {
                        return;
                    }

                    try
                    {
                        this.Invoke(this.ReFreshData);
                    }
                    catch{ }

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
            this.RefreshIndicator(list);
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
        /// 刷新曲线
        /// </summary>
        /// <param name="list">名称</param>
        private void RefreshLine(List<DefectStatisticsEntity> list)
        {
            if (list == null)
            {
                this.ChartPostBondResX.Series.Clear();
                this.ChartPostBondResY.Series.Clear();
                this.ChartPostBondResAngle.Series.Clear();
                this.chartControl1.Series.Clear();
                return;
            }

            this.ChartPostBondResX.Series.Clear();
            this.ChartPostBondResY.Series.Clear();
            this.ChartPostBondResAngle.Series.Clear();
            this.chartControl1.Series.Clear();
            Series s1 = new Series("X精度", ViewType.Line);
            Series s2 = new Series("Y精度", ViewType.Line);
            Series s3 = new Series("角度精度", ViewType.Line);
            Series s4 = new Series("面积精度", ViewType.Line);

            for (int i = 0; i < list.Count; i++)
            {
                s1.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetX));
                s2.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetY));
                s3.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].OffsetAngle));
                s4.Points.Add(new SeriesPoint(i, list[list.Count - 1 - i].Area));
            }

            this.ChartPostBondResX.Series.Add(s1);
            this.ChartPostBondResY.Series.Add(s2);
            this.ChartPostBondResAngle.Series.Add(s3);
            this.chartControl1.Series.Add(s4);
        }

        /// <summary>
        /// 刷新曲线
        /// </summary>
        /// <param name="list">名称</param>
        private void RefreshIndicator(List<DefectStatisticsEntity> list)
        {
            if (list == null || this.postBondInspection == null)
            {
                return;
            }

            if (list?.Count == 0)
            {
                #region X指标

                this.LbMaxXValue.Text = string.Empty;
                this.LbMinXValue.Text = string.Empty;
                this.LbAverageXValue.Text = string.Empty;
                this.LbDifferenceXValue.Text = string.Empty;
                this.LbCPKXValue.Text = string.Empty;
                this.LbThreeσXValue.Text = string.Empty;
                this.BtQualifiePercentageXValue.Text = string.Empty;

                #endregion

                #region Y指标

                this.LbMaxYValue.Text = string.Empty;
                this.LbMinYValue.Text = string.Empty;
                this.LbAverageYValue.Text = string.Empty;
                this.LbDifferenceYValue.Text = string.Empty;
                this.LbCPKYValue.Text = string.Empty;
                this.LbThreeσYValue.Text = string.Empty;
                this.BtQualifiePercentageYValue.Text = string.Empty;

                #endregion

                #region 角度指标

                this.LbMaxAngleValue.Text = string.Empty;
                this.LbMinAngleValue.Text = string.Empty;
                this.LbAverageAngleValue.Text = string.Empty;
                this.LbDifferenceAngleValue.Text = string.Empty;
                this.LbCPKAngleValue.Text = string.Empty;
                this.LbThreeσAngleValue.Text = string.Empty;
                this.BtQualifiePercentageAngleValue.Text = string.Empty;

                #endregion
                return;
            }

            double[] listX = list.Select(it => it.OffsetX).ToArray();
            double[] listY = list.Select(it => it.OffsetY).ToArray();
            double[] listAngle = list.Select(it => it.OffsetAngle).ToArray();
            double[] listArea = list.Select(it => it.Area).ToArray();

            #region X指标

            this.LbMaxXValue.Text = JOCPK.Max(listX).ToString("0.00") + " um";
            this.LbMinXValue.Text = JOCPK.Min(listX).ToString("0.00") + " um";
            this.LbAverageXValue.Text = JOCPK.Average(listX).ToString("0.00") + " um";
            this.LbDifferenceXValue.Text = (JOCPK.Max(listX) - JOCPK.Min(listX)).ToString("0.00") + " um";
            this.LbCPKXValue.Text = JOCPK.Cpk(listX, this.postBondInspection.LimitX * 1000.0, -this.postBondInspection.LimitX * 1000.0).ToString("0.00");
            this.LbThreeσXValue.Text = JOCPK.ThreeStDev(listX).ToString("0.00") + " um";
            this.BtQualifiePercentageXValue.Text = (JOCPK.QualifiedPercentage(listX, this.postBondInspection.LimitX * 1000.0, -this.postBondInspection.LimitX * 1000.0) * 100.0).ToString("0.00");

            #endregion

            #region Y指标

            this.LbMaxYValue.Text = JOCPK.Max(listY).ToString("0.00") + " um";
            this.LbMinYValue.Text = JOCPK.Min(listY).ToString("0.00") + " um";
            this.LbAverageYValue.Text = JOCPK.Average(listY).ToString("0.00") + " um";
            this.LbDifferenceYValue.Text = (JOCPK.Max(listY) - JOCPK.Min(listY)).ToString("0.00");
            this.LbCPKYValue.Text = JOCPK.Cpk(listY, this.postBondInspection.LimitY * 1000.0, -this.postBondInspection.LimitY * 1000.0).ToString("0.00");
            this.LbThreeσYValue.Text = JOCPK.ThreeStDev(listY).ToString("0.00") + " um";
            this.BtQualifiePercentageYValue.Text = (JOCPK.QualifiedPercentage(listY, this.postBondInspection.LimitY * 1000.0, -this.postBondInspection.LimitY * 1000.0) * 100.0).ToString("0.00");

            #endregion

            #region 角度指标

            this.LbMaxAngleValue.Text = JOCPK.Max(listAngle).ToString("0.00") + " °";
            this.LbMinAngleValue.Text = JOCPK.Min(listAngle).ToString("0.00") + " °";
            this.LbAverageAngleValue.Text = JOCPK.Average(listAngle).ToString("0.00") + " °";
            this.LbDifferenceAngleValue.Text = (JOCPK.Max(listAngle) - JOCPK.Min(listAngle)).ToString("0.00");
            this.LbCPKAngleValue.Text = JOCPK.Cpk(listAngle, this.postBondInspection.LimitAngle, -this.postBondInspection.LimitAngle).ToString("0.00");
            this.LbThreeσAngleValue.Text = JOCPK.ThreeStDev(listAngle).ToString("0.00") + " °";
            this.BtQualifiePercentageAngleValue.Text =
                (JOCPK.QualifiedPercentage(listAngle, this.postBondInspection.LimitAngle, -this.postBondInspection.LimitAngle) * 100.0).ToString("0.00");

            #endregion

            #region 胶面积指标

            if (this.postBondInspection.ApplicationSystem == Models.Enums.PostBondApplicationSystemEnum.EpoxyCheck)
            {
                this.LbMaxAngleValue.Text = JOCPK.Max(listArea).ToString("0.00") + " 倍";
                this.LbMinAngleValue.Text = JOCPK.Min(listArea).ToString("0.00") + " 倍";
                this.LbAverageAngleValue.Text = JOCPK.Average(listArea).ToString("0.00") + " °";
                this.LbDifferenceAngleValue.Text = (JOCPK.Max(listArea) - JOCPK.Min(listArea)).ToString("0.00");
                this.LbCPKAngleValue.Text = JOCPK.Cpk(listArea, this.postBondInspection.TolerantEpoxyMax, this.postBondInspection.TolerantEpoxyMin).ToString("0.00");
                this.LbThreeσAngleValue.Text = JOCPK.ThreeStDev(listArea).ToString("0.00") + " 倍";
                this.BtQualifiePercentageAngleValue.Text =
                    (JOCPK.QualifiedPercentage(listArea, this.postBondInspection.TolerantEpoxyMax, this.postBondInspection.TolerantEpoxyMin) * 100.0).ToString("0.00");
            }

            #endregion
        }

        /// <summary>
        /// 清空数据
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
        /// 导出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xlsx)|*.xlsx";
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
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmPostBondData_Load(object sender, EventArgs e)
        {
            this.InitControl();
            DefectCompensate.RePostBondInspection += this.AddDefectData;
            this.ReFreshData();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmPostBondData_FormClosed(object sender, FormClosedEventArgs e)
        {
            DefectCompensate.RePostBondInspection -= this.AddDefectData;
        }

        /// <summary>
        /// 选择发生改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbPostBond_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangePostbondType();
            this.ReFreshData();
        }

        /// <summary>
        /// 更改焊后的类型
        /// </summary>
        private void ChangePostbondType()
        {
            if (this.postBondInspection.ApplicationSystem == Models.Enums.PostBondApplicationSystemEnum.EpoxyCheck)
            {
                this.tablePanel1.AddControl(chartControl1);
                this.tablePanel1.Controls.Remove(this.ChartPostBondResAngle);
                this.tablePanel1.SetColumn(this.chartControl1, 0);
                this.gridColumn4.Caption = "面积(倍)";
                this.gridColumn4.FieldName = "Area";
                this.tablePanel1.SetRow(this.chartControl1, 2);
            }
            else
            {
                this.tablePanel1.AddControl(ChartPostBondResAngle);
                this.tablePanel1.Controls.Remove(this.chartControl1);
                this.tablePanel1.SetColumn(this.ChartPostBondResAngle, 0);
                this.gridColumn4.Caption = "角度(°)";
                this.gridColumn4.FieldName = "OffsetAngle";
                this.tablePanel1.SetRow(this.ChartPostBondResAngle, 2);
            }
        }
    }
}