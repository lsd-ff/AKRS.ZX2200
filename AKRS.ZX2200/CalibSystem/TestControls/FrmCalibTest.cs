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

public partial class FrmCalibTest : DevExpress.XtraEditors.XtraForm
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

    public FrmCalibTest()
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
            string fileName = $"D:\\精度实验2025\\标定精度验证实验{DateTime.Now:HHmmss}";
            this.log = DataLogManager.Instance.CreateDataLog($"标定精度验证实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");
            try
            {
                this.isStop = false;

                int prTimes = int.Parse(this.spPrTimes.Value.ToString());
                int times = int.Parse(this.spTimes.Value.ToString());
                int prInterval = int.Parse(this.spPrInterval.Value.ToString());
                int rows = int.Parse(this.spRows.Value.ToString());
                int cols = int.Parse(this.spCols.Value.ToString());
                double rowDistance = Convert.ToDouble(this.spRowDistance.Value.ToString());
                double colDistance = Convert.ToDouble(this.spColDistance.Value.ToString());

                List<AKRSPoint2D> pointACalibList = new();

                // 生成晶圆台标定点位列表
                pointACalibList = this.GeneratePointList(
                    this.bondModuleController.Get3DRealPosition(),
                    rowDistance,
                    colDistance,
                    rows,
                    cols);

                List<TestData> testDataList = new();


                this.log.AddData($"标定精度实验开始时间", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                this.log.CommitData();


                for (int i = 0; i < times; i++)
                {
                    for (int j = 0; j < pointACalibList.Count; j++)
                    {
                        this.bondModuleController.MoveBondXY(pointACalibList[j].X, pointACalibList[j].Y);

                        List<double> resultXList = new();
                        List<double> resultYList = new();

                        this.log.AddData($"标定精度实验循环次数", $"{i + 1}");
                        this.log.AddData($"标定精度实验移动前定位点位索引", $"{j + 1}");

                        for (int k = 0; k < prTimes; k++)
                        {
                            Thread.Sleep(prInterval);
                            MatchResult resultA = this.LocatePosition("大标定片模板", out double timeA1);
                            resultXList.Add(resultA.CenterX);
                            resultYList.Add(resultA.CenterY);

                            this.log.AddData($"标定精度实验第{k + 1}次定位PointA", new { X = resultA.CenterX, Y = resultA.CenterY });
                        }
                        AKRSPoint2D transA = new();
                        if (this.checkEdit1.Enabled)
                        {
                            AKRSPoint2D cameraCenter = new AKRSPoint2D(1224, 1024);

                            AKRSPoint2D vecToCenterCam = cameraCenter - new AKRSPoint2D(resultXList.Average(), resultYList.Average());
                            AKRSPoint2D vecToCenterAxis = CalibService.TransPoint(vecToCenterCam, Convert.ToDouble(spX1Y1.Value), Convert.ToDouble(spX1Y2.Value), Convert.ToDouble(spX2Y1.Value), Convert.ToDouble(spX2Y2.Value));
                            transA = this.bondModuleController.Get2DRealPosition() + vecToCenterAxis;
                        }
                        else
                        {
                            transA = CalibService.GetMachinePosByPixelPos(
   pointACalibList[j],
   new(resultXList.Average(), resultYList.Average(), 0),
   "BondCameraCoordinateSystem");
                        }


                        this.bondModuleController.MoveBondXY(transA.X, transA.Y);

                        List<double> resultXListAfterMove = new();
                        List<double> resultYListAfterMove = new();

                        this.log.AddData($"标定精度实验移动后定位点位索引", $"{j + 1}");
                        for (int k = 0; k < prTimes; k++)
                        {
                            Thread.Sleep(prInterval);
                            MatchResult resultA = this.LocatePosition("大标定片模板", out double timeA1);
                            resultXListAfterMove.Add(resultA.CenterX);
                            resultYListAfterMove.Add(resultA.CenterY);

                            this.log.AddData($"第{k + 1}次定位PointA", new { X = resultA.CenterX, Y = resultA.CenterY });
                        }

                        TestData testData = new();
                        testData.time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        testData.initialPixelPos = new(resultXList.Average(), resultYList.Average());
                        testData.curAxisPos = new(pointACalibList[j].X, pointACalibList[j].Y);
                        testData.targetPixelPos = new(1224, 1024);
                        testData.moveDistance = new(
                            transA.X - pointACalibList[j].X,
                            transA.Y - pointACalibList[j].Y);
                        testData.targetAxisRealPos = this.bondModuleController.Get2DRealPosition();
                        testData.targetPixelRealPos = new(
                            resultXListAfterMove.Average(),
                            resultYListAfterMove.Average());
                        testDataList.Add(testData);

                        this.log.CommitData();


                    }

                    if (this.isStop)
                    {
                        break;
                    }
                }

                ExcelPackage package = new(new FileInfo($"{fileName}.xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "时间";
                worksheet.Cells[1, 2].Value = "初始像素位置X";
                worksheet.Cells[1, 3].Value = "初始像素位置Y";
                worksheet.Cells[1, 4].Value = "初始轴位置X";
                worksheet.Cells[1, 5].Value = "初始轴位置Y";
                worksheet.Cells[1, 6].Value = "目标像素位置X";
                worksheet.Cells[1, 7].Value = "目标像素位置Y";
                worksheet.Cells[1, 8].Value = "计算出的轴移动量X";
                worksheet.Cells[1, 9].Value = "计算出的轴移动量Y";
                worksheet.Cells[1, 10].Value = "移动后目标轴位置X";
                worksheet.Cells[1, 11].Value = "移动后目标轴位置Y";
                worksheet.Cells[1, 12].Value = "移动后像素位置X";
                worksheet.Cells[1, 13].Value = "移动后像素位置Y";

                // 写入数据
                for (int row = 0; row < testDataList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = testDataList[row].time;
                    worksheet.Cells[row + 2, 2].Value = testDataList[row].initialPixelPos.X;
                    worksheet.Cells[row + 2, 3].Value = testDataList[row].initialPixelPos.Y;
                    worksheet.Cells[row + 2, 4].Value = testDataList[row].curAxisPos.X;
                    worksheet.Cells[row + 2, 5].Value = testDataList[row].curAxisPos.Y;
                    worksheet.Cells[row + 2, 6].Value = testDataList[row].targetPixelPos.X;
                    worksheet.Cells[row + 2, 7].Value = testDataList[row].targetPixelPos.Y;
                    worksheet.Cells[row + 2, 8].Value = testDataList[row].moveDistance.X;
                    worksheet.Cells[row + 2, 9].Value = testDataList[row].moveDistance.Y;
                    worksheet.Cells[row + 2, 10].Value = testDataList[row].targetAxisRealPos.X;
                    worksheet.Cells[row + 2, 11].Value = testDataList[row].targetAxisRealPos.Y;
                    worksheet.Cells[row + 2, 12].Value = testDataList[row].targetPixelRealPos.X;
                    worksheet.Cells[row + 2, 13].Value = testDataList[row].targetPixelRealPos.Y;
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
    /// <summary>
    /// 开始重复标定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnStartRepeatCalib_Click(object sender, EventArgs e)
    {
        this.btnStartRepeatCalib.Enabled = false;
        Task.Run(() =>
        {
            try
            {
                this.isStop = false;
                string fileName = $"D:\\精度实验2025\\重复标定实验{DateTime.Now:HHmmss}";

                this.log = DataLogManager.Instance.CreateDataLog($"重复标定实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");

                int prTimes = int.Parse(this.spPrTimes.Value.ToString());
                int times = int.Parse(this.spTimes.Value.ToString());
                int prInterval = int.Parse(this.spPrInterval.Value.ToString());
                int rows = int.Parse(this.spRows.Value.ToString());
                int cols = int.Parse(this.spCols.Value.ToString());
                double rowDistance = Convert.ToDouble(this.spRowDistance.Value.ToString());
                double colDistance = Convert.ToDouble(this.spColDistance.Value.ToString());

                // Bond相机移动到BMC中心点上方
                // this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BMCVisionMachinePos);

                // MatchResult bigMatchResult = this.LocatePosition("大标定片模板");
                // MatchResult bigMatchResult = this.LocatePosition(CalibrateRunPara.GetInstance().BmcPRName, out double time);

                // this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, bigMatchResult);

                initialPos = this.bondModuleController.Get3DRealPosition();

                List<AKRSPoint2D> bmcCalibPoints = new();

                // 创建BMC标定点位列表
                bmcCalibPoints = this.GeneratePointList(initialPos, rowDistance,
                    colDistance, rows, cols);

                List<List<float>> allTransMatrices = new();
                List<List<double>> allHTransMatrices = new();

                List<AKRSPoint2D> imagePoints = new();
                int marksNum = bmcCalibPoints.Count;
                float[] imageX = new float[marksNum];
                float[] imageY = new float[marksNum];
                float[] worldX = new float[marksNum];
                float[] worldY = new float[marksNum];

                List<List<AKRSPoint3D>> imgPointsList = new();
                List<List<AKRSPoint3D>> realPointsList = new();
                List<List<AKRSPoint3D>> realTimePointsList = new();

                this.log.AddData($"重复标定实验开始时间", $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                this.log.CommitData();
                for (int j = 0; j < times; j++)
                {
                    List<AKRSPoint3D> imgPoints = new();
                    List<AKRSPoint3D> realPoints = new();
                    List<AKRSPoint3D> realTimePoints = new();

                    // Bond相机遍历点位开始定位并标定
                    for (int i = 0; i < bmcCalibPoints.Count; i++)
                    {
                        // 轴移动到位
                        this.bondModuleController.MoveBondXY(bmcCalibPoints[i].X, bmcCalibPoints[i].Y);
                        // this.bondHeadController.MoveAxisZ(CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z);

                        Thread.Sleep(prInterval);
                        // 获取Pr实体
                        // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("大标定片模板");
                        // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance()
                        //     .Find(CalibrateRunPara.GetInstance().BmcPRName);

                        List<AKRSPoint2D> matchResults = new();
                        this.log.AddData($"重复标定实验循环次数", $"{j + 1}");
                        this.log.AddData($"重复标定实验定位点索引", $"{i + 1}");
                        for (int k = 0; k < prTimes; k++)
                        {
                            MatchResult tempMatchResult =
                                this.LocatePosition(CalibrateRunPara.GetInstance().BmcPRName, out double _);
                            matchResults.Add(new(tempMatchResult.CenterX, tempMatchResult.CenterY));

                            this.log.AddData($"第{k + 1}次定位{CalibrateRunPara.GetInstance().BmcPRName}", new { X = tempMatchResult.CenterX, Y = tempMatchResult.CenterY });
                        }

                        this.log.CommitData();

                        MatchResult matchResult = new(matchResults.Average(x => x.X), matchResults.Average(x => x.Y), 0);
                        // // 开始定位
                        // ExcuteResult excuteResult = pREntity.DoWork();
                        //
                        // // 拍照失败，直接返回错误
                        // if (excuteResult != ExcuteResult.Success)
                        // {
                        //     return;
                        // }

                        // 获取定位结果
                        // MatchResult matchResult = (MatchResult)pREntity.AlgResult;

                        imagePoints.Add(new(matchResult.CenterX, matchResult.CenterY));
                        imageX[i] = (float)matchResult.CenterX;
                        imageY[i] = (float)matchResult.CenterY;
                        //worldX[i] = (float)(bmcCalibPoints[i].X - CalibrateRunPara.GetInstance().BMCVisionMachinePos.X);
                        //worldY[i] = (float)(bmcCalibPoints[i].Y - CalibrateRunPara.GetInstance().BMCVisionMachinePos.Y);

                        var axesRealtimePos = this.bondModuleController.Get2DRealPosition();
                        worldX[i] = (float)(axesRealtimePos.X);
                        worldY[i] = (float)(axesRealtimePos.Y);

                        realTimePoints.Add(this.bondModuleController.Get3DRealPosition());
                        imgPoints.Add(new(matchResult.CenterX, matchResult.CenterY, 0));
                        realPoints.Add(new(worldX[i], worldY[i], 0));
                    }

                    realPointsList.Add(realPoints);
                    realTimePointsList.Add(realTimePoints);
                    imgPointsList.Add(imgPoints);

                    // 对比海康和 Halcon 的标定结果
                    List<float> transMatrix = TransformTool.CalcTransMatrix(imgPoints, realPoints);
                    allTransMatrices.Add(transMatrix);

                    List<double> hTransMatrix = CalibService.CalcTransMatrixHalcon(
                        imgPoints.Select(p => new AKRSPoint2D(p.X, p.Y)).ToList(),
                        realPoints.Select(p => new AKRSPoint2D(p.X, p.Y)).ToList());
                    allHTransMatrices.Add(hTransMatrix);

                    if (this.isStop)
                    {
                        break;
                    }
                }
                this.bondModuleController.MoveBondXY(initialPos.X, initialPos.Y);

                ExcelPackage package = new(new FileInfo($"{fileName}矩阵信息.xlsx"));


                realPointsList.ExportToXlsx($"{fileName}轴位置.xlsx",
                    $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} real points sheet");
                realTimePointsList.ExportToXlsx($"{fileName}实际位置.xlsx",
                    $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} real time points sheet");
                imgPointsList.ExportToXlsx($"{fileName}像素坐标.xlsx",
                    $"{DateTime.Now:yyyy MMMM dd HH:mm:ss} img points sheet");

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "VM Matrix Sheet");
                ExcelWorksheet hworksheet = package.Workbook.Worksheets.Add(DateTime.Now + "Halcon Matrix Sheet");

                // 写入数据
                for (int i = 0; i < allTransMatrices.Count; i++)
                {
                    List<float> transMatrix = allTransMatrices[i];

                    // 遍历当前变换矩阵的每个元素
                    for (int j = 0; j < transMatrix.Count; j++)
                    {
                        // 将元素写入到 Excel 工作表中
                        worksheet.Cells[i + 1, j + 1].Value = transMatrix[j];
                    }
                }

                for (int i = 0; i < allHTransMatrices.Count; i++)
                {
                    List<double> transMatrix = allHTransMatrices[i];

                    // 遍历当前变换矩阵的每个元素
                    for (int j = 0; j < transMatrix.Count; j++)
                    {
                        // 将元素写入到 Excel 工作表中
                        hworksheet.Cells[i + 1, j + 1].Value = transMatrix[j];
                    }
                }

                // 保存Excel文件
                package.Save();
                this.log.Flush();

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
            finally
            {
                this.Invoke(new Action(() => { this.btnStartRepeatCalib.Enabled = true; }));
            }
        });
    }
}

public class TestData
{
    public string time;

    public AKRSPoint2D initialPixelPos;

    public AKRSPoint2D curAxisPos;

    public AKRSPoint2D targetPixelPos;

    public AKRSPoint2D moveDistance;

    public AKRSPoint2D targetAxisRealPos;

    public AKRSPoint2D targetPixelRealPos;
}