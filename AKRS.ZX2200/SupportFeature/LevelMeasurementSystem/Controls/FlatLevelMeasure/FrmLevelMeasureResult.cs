namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 水平测量结果展示
    /// </summary>
    public partial class FrmLevelMeasureResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="levelMeasureName">测高程式的名称</param>
        /// <param name="flatLevelMeasureResultList">测平结果</param>
        public FrmLevelMeasureResult(string levelMeasureName, List<FlatLevelMeasureResult> flatLevelMeasureResultList)
        {
            this.InitializeComponent();
            this.Text = levelMeasureName;   
            this.GcLevelMeasureResult.DataSource = flatLevelMeasureResultList;
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtExport_Click(object sender, EventArgs e)
        {
            XtraSaveFileDialog fileDialog = new XtraSaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.GcLevelMeasureResult.ExportToXls(fileDialog.FileName);
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
                        AKRSXtraMessageBox.Show("数据导出失败！数据量过大，请分别统计再导出！", "提示");
                    }
                }
            }
        }

        /// <summary>
        /// 重测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemeasure_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }

        /// <summary>
        /// 重绘表格
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvLevelMeasureResult_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            int rowHandle = e.RowHandle;

            FlatLevelMeasureResult flatLevelMeasureResult = this.GvLevelMeasureResult.GetRow(rowHandle) as FlatLevelMeasureResult;
            if (flatLevelMeasureResult == null)
            {
                return;
            }

            (double MinValue, double MaxValue) ret = flatLevelMeasureResult.GetMinMaxValue();

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cellInfo = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
            if (cellInfo.IsDataCell && cellInfo.Column.FieldName != "MaxDifference")
            {
                if (Math.Abs(ret.MinValue - double.Parse(cellInfo.CellValue.ToString())) < 0.000001)
                {
                    e.Appearance.BackColor = Color.YellowGreen;
                }
                else if (Math.Abs(ret.MaxValue - double.Parse(cellInfo.CellValue.ToString())) < 0.000001)
                {
                    e.Appearance.BackColor = Color.OrangeRed;
                }
                else
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
            }
        }
    }
}