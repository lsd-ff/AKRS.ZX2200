namespace AKRS.ZX2200.Experiment.Test
{
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using OfficeOpenXml;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    ///  结果显示窗体
    /// </summary>
    public partial class FrmBMCTestResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="accuracyXy">XY公差界限</param>
        /// <param name="accuracyAngle">角度公差界限</param>
        /// <param name="bMCTestResultList">测试结果</param>
        public FrmBMCTestResult(double accuracyXy, double accuracyAngle, List<BMCTestResult> bMCTestResultList)
        {
            this.InitializeComponent();
            this.accuracyXY = accuracyXy;
            this.accuracyAngle = accuracyAngle;
            this.bMCTestResultList = bMCTestResultList;
            this.InitControl();
        }

        /// <summary>
        /// XY公差界限
        /// </summary>
        private double accuracyXY;

        /// <summary>
        /// 角度公差界限
        /// </summary>
        private double accuracyAngle;

        /// <summary>
        ///  测试结果
        /// </summary>
        private List<BMCTestResult> bMCTestResultList;

        /// <summary>
        ///  计算结果
        /// </summary>
        private List<BMCTestFinalResult> bMCTestFinalResultList = new List<BMCTestFinalResult>();

        /// <summary>
        /// 计算结果
        /// </summary>
        private void CalculateResult()
        {
            this.bMCTestFinalResultList.Clear();

            if (this.bMCTestResultList.Count == 0)
            {
                return;
            }

            // MaxValue
            double xMax = this.bMCTestResultList.Max(t => t.XResult);
            double yMax = this.bMCTestResultList.Max(t => t.YResult);
            double thetaMax = this.bMCTestResultList.Max(t => t.AngleResult);

           this.bMCTestFinalResultList.Add(new BMCTestFinalResult()
            {
                Name = "Max",
                XRes = xMax,
                YRes = yMax,
                ThetaRes = thetaMax
            });

            // MinValue
            double xMin = this.bMCTestResultList.Min(t => t.XResult);
            double yMin = this.bMCTestResultList.Min(t => t.YResult);
            double thetaMin = this.bMCTestResultList.Min(t => t.AngleResult);
            this. bMCTestFinalResultList.Add(new BMCTestFinalResult()
                                                {
                                                    Name = "Min",
                                                    XRes = xMin,
                                                    YRes = yMin,
                                                    ThetaRes = thetaMin
                                                });

            // Mean
            double xAverage = this.bMCTestResultList.Average(t => t.XResult);
            double yAverage = this.bMCTestResultList.Average(t => t.YResult);
            double thetaAverage = this.bMCTestResultList.Average(t => t.AngleResult);
            this.bMCTestFinalResultList.Add(new BMCTestFinalResult()
                                                {
                                                    Name = "Mean",
                                                    XRes = xAverage,
                                                    YRes = yAverage,
                                                    ThetaRes = thetaAverage
                                                });

            // Difference
            double xDifference = xMax - xMin;
            double yDifference = yMax - yMin;
            double thetaDifference = thetaMax - thetaMin;
            this. bMCTestFinalResultList.Add(new BMCTestFinalResult()
                                                {
                                                    Name = "Difference",
                                                    XRes = xDifference,
                                                    YRes = yDifference,
                                                    ThetaRes = thetaDifference
                                                });

            // 计算方差之和
            double sumDeviationX = this.bMCTestResultList.Select(it => Math.Pow(it.XResult - xAverage, 2)).Sum();
            double sumDeviationY = this.bMCTestResultList.Select(it => Math.Pow(it.YResult - yAverage, 2)).Sum();
            double sumDeviationTheta = this.bMCTestResultList.Select(it => Math.Pow(it.AngleResult - xAverage, 2)).Sum();

            // 3s:这里用的是样本标准差
            double threeSigmaX = 3.0 * Math.Sqrt(sumDeviationX / (double)(this.bMCTestResultList.Count - 1));
            double threeSigmaY = 3.0 * Math.Sqrt(sumDeviationY / (double)(this.bMCTestResultList.Count - 1));
            double threeSigmaTheta = 3.0 * Math.Sqrt(sumDeviationTheta / (double)(this.bMCTestResultList.Count - 1));
            this.bMCTestFinalResultList.Add(new BMCTestFinalResult()
                                           {
                                               Name = "ThreeSigma",
                                               XRes = threeSigmaX,
                                               YRes = threeSigmaY,
                                               ThetaRes = threeSigmaTheta
            });

            // CMK
            double cMKX = Math.Min((this.accuracyXY - xAverage), (xAverage + this.accuracyXY)) / threeSigmaX;
            double cMKY = Math.Min((this.accuracyXY - yAverage), (yAverage + this.accuracyXY)) / threeSigmaY;
            double cMKTheta = Math.Min(
                                  (this.accuracyAngle - thetaAverage),
                                  (thetaAverage + this.accuracyAngle)) / threeSigmaTheta;

            this.bMCTestFinalResultList.Add(new BMCTestFinalResult()
                                               {
                                                   Name = "CMK",
                                                   XRes = cMKX,
                                                   YRes = cMKY,
                                                   ThetaRes = cMKTheta
                                               });
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            this.SpXYAccuracy.EditValue = this.accuracyXY;
            this.SpAngleAccuracy.EditValue = this.accuracyAngle;

            this.CalculateResult();

            // 绑定数据源
            this.GcTestResult.DataSource = this.bMCTestFinalResultList;
            this.GcTestResult.Refresh();
            this.GvTestResult.RefreshData();

            this.SaveCorrectionData();
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        private void SaveCorrectionData()
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--BMCTestDetail.xlsx"));

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
        ///  取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        ///  细节
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDetail_Click(object sender, EventArgs e)
        {
            try
            {
                this.CalculateResult();
                FrmBMCTestResultDetail frmBMCTestResultDetail = new FrmBMCTestResultDetail(this.bMCTestResultList);
                frmBMCTestResultDetail.ShowDialog();

                frmBMCTestResultDetail.Dispose();
            }
            catch(Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message.ToString());

                return;
            }
        }
    }

    /// <summary>
    /// BMC测试最终结果
    /// </summary>
    public class BMCTestFinalResult
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// X结果
        /// </summary>
        public double XRes { get; set; }

        /// <summary>
        /// Y结果
        /// </summary>
        public double YRes { get; set; }

        /// <summary>
        /// Theta结果
        /// </summary>
        public double ThetaRes { get; set; }
    }
}