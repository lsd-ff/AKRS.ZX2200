namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    using OfficeOpenXml;

    using LicenseContext = OfficeOpenXml.LicenseContext;

    /// <summary>
    ///  BMC测试详细结果
    /// </summary>
    public partial class FrmBMCTestResultDetail : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bMCTestResultList">结果</param>
        public FrmBMCTestResultDetail(List<BMCTestResult> bMCTestResultList)
        {
            this.InitializeComponent();
            this.bMCTestResultList = bMCTestResultList;
            this.InitControl();
        }

        /// <summary>
        ///  测试结果
        /// </summary>
        private List<BMCTestResult> bMCTestResultList;

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitControl()
        {
            this.GcTestResultDetail.DataSource = this.bMCTestResultList;
            this.GcTestResultDetail.Refresh();
            this.GvTestResultDetail.RefreshData();
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        /// <param name="path">路径</param>
        private void SaveCorrectionData(string path)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(path));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Number";

                worksheet.Cells[1, 2].Value = "X(μm)";

                worksheet.Cells[1, 3].Value = "Y(μm)";

                worksheet.Cells[1, 4].Value = "Theta(°)";

                worksheet.Cells[1, 5].Value = "Time";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < this.bMCTestResultList.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value = this.bMCTestResultList[i].Index;

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value = this.bMCTestResultList[i].XResult;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = this.bMCTestResultList[i].YResult;

                worksheet.Cells[lastUsedRow + 1 + i, 4].Value = this.bMCTestResultList[i].AngleResult;

                worksheet.Cells[lastUsedRow + 1 + i, 5].Value =
                    this.bMCTestResultList[i].CreateTime.ToString("MM-dd HH:mm:ss");
            }

            excelPackage.Save();

            // ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
        }

        /// <summary>
        ///  打印
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            Thread importThread = new Thread(new ThreadStart(this.ExportData));
            importThread.SetApartmentState(ApartmentState.STA);
            importThread.Start();

            //this.Invoke(
            //    new Action(this.ExportData));
        }

        /// <summary>
        /// 输出到指定路径
        /// </summary>
        private void ExportData()
        {
            XtraSaveFileDialog fileDialog = new XtraSaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.SaveCorrectionData(fileDialog.FileName);

                    AKRSXtraMessageBox.Show(
                        $"Export  success!",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
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
    }
}