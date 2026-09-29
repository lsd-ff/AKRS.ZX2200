using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using DataAnalysis.Acquisition;
using log4net.Core;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using FileInfo = System.IO.FileInfo;

namespace AKRS.ZX2200.CalibSystem.TestControls;

using DevExpress.Utils.Extensions;
using Galaxy2.CoordinateSystems.CoordinateSystems;
using LanguageExt;
using Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public partial class FrmMoveLocateTest : DevExpress.XtraEditors.XtraForm
{
    /// <summary>
    /// BondModule控制器
    /// </summary>
    private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

    /// <summary>
    /// 焊头控制器
    /// </summary>
    private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

    /// <summary>
    /// 标定控制器
    /// </summary>
    private CalibController calibController = new();

    private IDataLog log;

    private bool isStop = false;

    public FrmMoveLocateTest()
    {
        this.InitializeComponent();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

    }

    /// <summary>
    /// 开始实验
    /// </summary>
    /// <param name="sender">事件</param>
    /// <param name="e">参数</param>
    private void btnStartTest_Click(object sender, EventArgs e)
    {
        this.btnStartTest.Enabled = false;
        Task.Run(() =>
        {
            string fileName = $"D:\\精度实验2025\\移动定位实验{DateTime.Now:HHmmss}";
            this.log = DataLogManager.Instance.CreateDataLog($"移动定位实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");
            try
            {
                this.isStop = false;

                int prTimes = int.Parse(this.spPrTimes.Value.ToString());
                int times = int.Parse(this.spTimes.Value.ToString());
                int prInterval = int.Parse(this.spPrInterval.Value.ToString());

                List<TestData1> testDataList = new();

                this.log.AddData($"移动定位实验开始时间", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                this.log.CommitData();

                this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BMCVisionMachinePos);
                for (int i = 0; i < times; i++)
                {
                    this.bondModuleController.MoveBondXY(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos.X, CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos.Y);

                        List<double> resultXList = new();
                        List<double> resultYList = new();

                        this.log.AddData($"移动定位实验循环次数", $"{i + 1}");

                        for (int k = 0; k < prTimes; k++)
                        {
                            Thread.Sleep(prInterval);
                            MatchResult resultA = this.LocatePosition("大标定片模板", out double timeA1);
                            resultXList.Add(resultA.CenterX);
                            resultYList.Add(resultA.CenterY);

                            this.log.AddData($"移动定位实验第{k + 1}次定位PointA", new { X = resultA.CenterX, Y = resultA.CenterY });
                        }

                        AKRSPoint2D transA = CalibService.GetMachinePosByPixelPos(
                            this.bondModuleController.Get2DRealPosition(),
                            new MatchResult(resultXList.Average(), resultYList.Average(), 0),
                            "BondCameraCoordinateSystem");

                        this.bondModuleController.MoveBondXY(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos.X, CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos.Y);

                        List<double> resultXListAfterMove = new();
                        List<double> resultYListAfterMove = new();

                        for (int k = 0; k < prTimes; k++)
                        {
                            Thread.Sleep(prInterval);
                            MatchResult resultA = this.LocatePosition("大标定片模板", out double timeA1);
                            resultXListAfterMove.Add(resultA.CenterX);
                            resultYListAfterMove.Add(resultA.CenterY);

                            this.log.AddData($"第{k + 1}次定位PointA", new { X = resultA.CenterX, Y = resultA.CenterY });
                        }

                        AKRSPoint2D transB = CalibService.GetMachinePosByPixelPos(
                            this.bondModuleController.Get2DRealPosition(),
                            new MatchResult(resultXListAfterMove.Average(), resultXListAfterMove.Average(), 0),
                            "BondCameraCoordinateSystem");

                    TestData1 testData = new();
                        testData.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        testData.leftPixelPos.X = resultXList.Average();
                        testData.leftPixelPos.Y = resultYList.Average();
                        testData.leftAxisPos.X = transA.X;
                        testData.leftAxisPos.Y = transA.Y;

                        testData.rightPixelPos.X = resultXListAfterMove.Average();
                        testData.rightPixelPos.Y = resultYListAfterMove .Average();
                        testData.rightAxisPos.X = transB.X;
                        testData.rightAxisPos.Y = transB.Y;
                        testDataList.Add(testData);

                        this.log.CommitData();

                        if (this.isStop)
                    {
                        break;
                    }
                }

                ExcelPackage package = new(new FileInfo($"{fileName}.xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "时间";
                worksheet.Cells[1, 2].Value = "左点像素位置X";
                worksheet.Cells[1, 3].Value = "左点像素位置Y";
                worksheet.Cells[1, 4].Value = "左点轴位置X";
                worksheet.Cells[1, 5].Value = "左点轴位置Y";
                worksheet.Cells[1, 6].Value = "右点像素位置X";
                worksheet.Cells[1, 7].Value = "右点像素位置Y";
                worksheet.Cells[1, 8].Value = "右点轴位置X";
                worksheet.Cells[1, 9].Value = "右点轴位置Y";

                // 写入数据
                for (int row = 0; row < testDataList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = testDataList[row].time;
                    worksheet.Cells[row + 2, 2].Value = testDataList[row].leftPixelPos.X;
                    worksheet.Cells[row + 2, 3].Value = testDataList[row].leftPixelPos.Y;
                    worksheet.Cells[row + 2, 4].Value = testDataList[row].leftAxisPos.X;
                    worksheet.Cells[row + 2, 5].Value = testDataList[row].leftAxisPos.Y;
                    worksheet.Cells[row + 2, 6].Value = testDataList[row].rightPixelPos.X;
                    worksheet.Cells[row + 2, 7].Value = testDataList[row].rightPixelPos.Y;
                    worksheet.Cells[row + 2, 8].Value = testDataList[row].rightAxisPos.X;
                    worksheet.Cells[row + 2, 9].Value = testDataList[row].rightAxisPos.Y;
                }

                // 保存Excel文件
                package.Save();

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
            finally
            {
                this.log.Flush();
                this.isStop = false;
                this.Invoke(new Action(() => { this.btnStartTest.Enabled = true; }));
            }
        });
    }

    /// <summary>
    /// 模板定位
    /// </summary>
    /// <param name="patternName">模板名称</param>
    /// <returns>定位结果</returns>
    public MatchResult LocatePosition(string patternName, out double time)
    {
        // 获取Pr实体
        PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

        Stopwatch stopwatch = new();

        stopwatch.Start();

        ExcuteResult excuteResult = pREntity.DoWork();

        stopwatch.Stop();

        time = stopwatch.Elapsed.TotalMilliseconds;

        // 拍照失败，直接返回错误
        if (excuteResult != ExcuteResult.Success)
        {
            throw new ArgumentNullException(patternName, "The" + patternName + " excuteResult is fail.");
        }

        // 获取定位结果
        MatchResult matchResult = (MatchResult)pREntity.AlgResult;

        return matchResult;
    }

    public List<AKRSPoint2D> GeneratePointList(AKRSPoint3D centerPoint, double rowdistance, double coldistance,
        int numRows, int numColumns)
    {
        List<AKRSPoint2D> pointList = new();

        double startX = centerPoint.X - (numColumns - 1) / 2.0 * coldistance; // 左上角点的X坐标
        double startY = centerPoint.Y - (numRows - 1) / 2.0 * rowdistance; // 左上角点的Y坐标

        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numColumns; j++)
            {
                double x = startX + j * coldistance;
                double y = startY + i * rowdistance;
                AKRSPoint2D point = new(x, y);
                pointList.Add(point);
            }
        }

        return pointList;
    }

    private void btnStop_Click(object sender, EventArgs e)
    {
        this.isStop = true;
    }

    AKRSPoint3D initialPos = new AKRSPoint3D();

    private void groupControl1_Paint(object sender, PaintEventArgs e)
    {

    }
}

public class TestData1
{
    public string time;

    public AKRSPoint2D leftPixelPos;

    public AKRSPoint2D leftAxisPos;

    public AKRSPoint2D rightPixelPos;

    public AKRSPoint2D rightAxisPos;

}