using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    using System.IO;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DataAnalysis.Acquisition;

    using OfficeOpenXml;

    public partial class FrmXYAccurayTest : DevExpress.XtraEditors.XtraForm
    {
        public FrmXYAccurayTest()
        {
            InitializeComponent();
        }

        private AKRSPoint3D startPos = new AKRSPoint3D();

        private AKRSPoint3D endPos = new AKRSPoint3D();


        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycles;

        /// <summary>
        /// 定位后移到相机中心
        /// </summary>
        private bool isMoveToCameraCenterAfterVision = false;

        /// <summary>
        /// 拍照次数
        /// </summary>
        private int visionTime = 1;

        /// <summary>
        /// BMC测试线程
        /// </summary>
        private Task accuracyTestTask;


        /// <summary>
        /// BMC测试线程
        /// </summary>
        private Task upLookTestTask;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private UpLookController upLookController = new UpLookController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        private void BtnSetStartPos_Click(object sender, EventArgs e)
        {
            this.startPos = this.bondModuleController.GetG0RealPosition();
            this.SpStartPosX.EditValue = this.startPos.X;
            this.SpStartPosY.EditValue = this.startPos.Y;
            this.SpStartPosZ.EditValue = this.startPos.Z;
        }

        private void BtnSetEndPos_Click(object sender, EventArgs e)
        {
            this.endPos = this.bondModuleController.GetG0RealPosition();
            this.SpEndPosX.EditValue = this.endPos.X;
            this.SpEndPosY.EditValue = this.endPos.Y;
            this.SpEndPosZ.EditValue = this.endPos.Z;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.cycles = (int)this.SpCycles.Value;

            this.visionTime = (int)this.SpVisionTime.Value;

            this.isMoveToCameraCenterAfterVision = this.ChkMoveToCameraCenterAfterVision.Checked;

            // 开启测试线程
            this.StartTask();
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        /// <returns>结果</returns>
        private bool StartTask()
        {
            // 防止线程多次启动
            if (this.accuracyTestTask == null
                || this.accuracyTestTask.Status != TaskStatus.Running)
            {
                this.BtnStart.Enabled = false;
                this.BtnStart.BackColor = Color.Yellow;
                this.accuracyTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("XY轴精度实验线程");
                            this.XYAccuracyTest();
                        });
            }

            return true;
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        /// <returns>结果</returns>
        private bool StartTask2()
        {
            // 防止线程多次启动
            if (this.upLookTestTask == null
                || this.upLookTestTask.Status != TaskStatus.Running)
            {
                this.BtnUpLookTest.Enabled = false;
                this.BtnUpLookTest.BackColor = Color.Yellow;
                this.upLookTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("上视实验线程");
                            this.UplookTest();
                        });
            }

            return true;
        }

        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool XYAccuracyTest()
        {
            try
            {
                DialogResult dialog;
                AKRSPoint3D newVisionPos = new AKRSPoint3D();

                ExcuteResult upLookP1Result;
                ExcuteResult upLookP2Result;

                MatchResult upLookRes1 = null, upLookRes2 = null;
                AKRSPoint3D uplookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D uplookP2ResInAxis = new AKRSPoint3D();

                MatchResult matchResult1 = null;
                MatchResult matchResult2 = null;

                AKRSPoint3D downlookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D downlookP2ResInAxis = new AKRSPoint3D();
                this.bondHeadController.MoveBondZToSafePos();

                this.bondModuleController.MoveToG0Pos(this.startPos);

                for (int i = 0; i < this.cycles; i++)
                {
                    this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    this.bondModuleController.MoveBondXYZWithoutSafe(this.bondModuleController.ConvertG0ToMachinePos(this.startPos));

                // P1定位
                retryP1:
                    matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                       null,
                         CalibrateRunPara.GetInstance().BmcPRName,
                        this.visionTime,true);

                    if (matchResult1 == null)
                    {
                        dialog = AKRSMessageBoxExt.Show(
                            $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto retryP1;

                            case DialogResult.Abort:

                                throw new Exception($"{""}  adjust  failed!");

                            case DialogResult.Ignore:
                                matchResult1 = new MatchResult();
                                break;
                        }
                    }

                    //if (this.isMoveToCameraCenterAfterVision)
                    //{
                    //    // 移动到相机中心再拍一次
                    //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), matchResult1);

                    //    matchResult1 =
                    //        (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "", this.visionTime);
                    //}

                    this.bondModuleController.MoveBondXYZWithoutSafe(this.bondModuleController.ConvertG0ToMachinePos(this.endPos));

                    // P1定位
                    retryP2:
                    matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                       null,
                         CalibrateRunPara.GetInstance().BmcPRName,
                        this.visionTime);

                    if (matchResult2 == null)
                    {
                        dialog = AKRSMessageBoxExt.Show(
                            $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto retryP1;

                            case DialogResult.Abort:

                                throw new Exception($"{""}  adjust  failed!");

                            case DialogResult.Ignore:
                                matchResult1 = new MatchResult();
                                break;
                        }
                    }

                    //if (this.isMoveToCameraCenterAfterVision)
                    //{
                    //    // 移动到相机中心再拍一次
                    //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), matchResult1);

                    //    matchResult1 =
                    //        (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "", this.visionTime);
                    //}

                    this.SaveCurrentData(matchResult1,matchResult2,this.startPos,this.endPos);
                }

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"BMC  test  failed:{e.ToString()}!",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BeginInvoke(
                    () =>
                    {
                        this.BtnStart.Enabled = true;
                        this.BtnStart.BackColor = default;
                    });


                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"实验结束!",
                    "信息",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return true;
        }


        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool UplookTest()
        {
            string fileName = $"D:\\精度实验2025\\上视定位验证实验{DateTime.Now:HHmmss}";

            IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"上视定位验证实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");
            try
            {


                DialogResult dialog;
                AKRSPoint3D newVisionPos = new AKRSPoint3D();

                ExcuteResult upLookP1Result;
                ExcuteResult upLookP2Result;

                MatchResult upLookRes1 = null, upLookRes2 = null;
                AKRSPoint3D uplookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D uplookP2ResInAxis = new AKRSPoint3D();

                MatchResult matchResult1 = null;
                MatchResult matchResult2 = null;

                AKRSPoint3D downlookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D downlookP2ResInAxis = new AKRSPoint3D();
                this.bondHeadController.MoveBondZToSafePos();

                for (int i = 0; i < this.cycles; i++)
                {
                    this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    dataLog.AddData($"时间", new { 时间= $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}"});

                // P1定位
                retryP1:
                    matchResult1  = (MatchResult)this.system2Controller.UpLookCameraVision(
                        this.startPos,
                            CalibrateRunPara.GetInstance().UpLookPRName,
                            this.visionTime,
                            i != 0);

                    if (matchResult1 == null)
                    {
                        dialog = AKRSMessageBoxExt.Show(
                            $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto retryP1;

                            case DialogResult.Abort:

                                throw new Exception($"{""}  adjust  failed!");

                            case DialogResult.Ignore:
                                matchResult1 = new MatchResult();
                                break;
                        }
                    }


                    AKRSPoint3D p1Res = this.system2Controller.GetBondVisionResultPos(matchResult1);

                    dataLog.AddData($"上视M1结果", new { X = matchResult1.CenterX, Y = matchResult1.CenterY });
                    dataLog.AddData($"上视M1结果-转到G0坐标", new { X = p1Res.X, Y = p1Res.Y });

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), matchResult1);

                //    matchResult1 =
                //        (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "", this.visionTime);
                //}

                // P1定位
                retryP2:
                 

                    matchResult2 = (MatchResult)this.system2Controller.UpLookCameraVision(
                        this.endPos,
                        CalibrateRunPara.GetInstance().UpLookPRName,
                        this.visionTime,
                        true);

                    if (matchResult2 == null)
                    {
                        dialog = AKRSMessageBoxExt.Show(
                            $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto retryP1;

                            case DialogResult.Abort:

                                throw new Exception($"{""}  adjust  failed!");

                            case DialogResult.Ignore:
                                matchResult1 = new MatchResult();
                                break;
                        }
                    }

                    //if (this.isMoveToCameraCenterAfterVision)
                    //{
                    //    // 移动到相机中心再拍一次
                    //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), matchResult1);

                    //    matchResult1 =
                    //        (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "", this.visionTime);
                    //}

                    AKRSPoint3D p2Res = this.system2Controller.GetBondVisionResultPos(matchResult2);

                    dataLog.AddData($"上视M2结果", new { X = matchResult2.CenterX, Y = matchResult2.CenterY });
                    dataLog.AddData($"上视M2结果-转到G0坐标", new { X = p2Res.X, Y = p2Res.Y });

                    dataLog.CommitData();
                    if (i % 5 == 0 && i > 1)
                    {
                        dataLog.Flush();
                    }
                }

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"BMC  test  failed:{e.ToString()}!",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                dataLog.Flush();

                this.BeginInvoke(
                    () =>
                    {
                        this.BtnUpLookTest.Enabled = true;
                        this.BtnUpLookTest.BackColor = default;
                    });


                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"实验结束!",
                    "信息",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            return true;
        }

        /// <summary>
        /// 保存定位结果
        /// </summary>
        /// <param name="uplookP1">上视P1</param>
        /// <param name="uplookP2">上视p2</param>
        /// <param name="downlookP1">下视p1</param>
        /// <param name="downlookP2">下视p2</param>
        private void SaveCurrentData(MatchResult startMatchResult, MatchResult endMatchResult, AKRSPoint3D startPos, AKRSPoint3D endPos)
        {
            string fileName = $"D:\\精度实验2025\\XY轴重复定位精度验证实验{DateTime.Now:yyyyMMdd}";

            // 添加一个工作表
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName+@".xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 20; i++)
            {
                worksheet.Column(i).Width = 20;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "时间";

                worksheet.Cells[1, 2].Value = "起点定位结果像素坐标-X";

                worksheet.Cells[1, 3].Value = "起点定位结果像素坐标-Y";

                worksheet.Cells[1, 4].Value = "起点定位结果像素坐标-Angle";

                worksheet.Cells[1, 5].Value = "终点定位结果像素坐标-X";

                worksheet.Cells[1, 6].Value = "终点定位结果像素坐标-Y";

                worksheet.Cells[1, 7].Value = "终点定位结果像素坐标-Angle";

                worksheet.Cells[1, 8].Value = "起点轴坐标-X";

                worksheet.Cells[1, 9].Value = "起点轴坐标-Y";

                worksheet.Cells[1, 10].Value = "起点轴坐标-Z";

                worksheet.Cells[1, 11].Value = "终点轴坐标-X";

                worksheet.Cells[1, 12].Value = "终点轴坐标-Y";

                worksheet.Cells[1, 13].Value = "终点轴坐标-Z";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = startMatchResult?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = startMatchResult?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 4].Value = startMatchResult?.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = endMatchResult?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 6].Value = endMatchResult?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 7].Value = endMatchResult?.Angle;

            worksheet.Cells[lastUsedRow + 1, 8].Value = startPos?.X;

            worksheet.Cells[lastUsedRow + 1, 9].Value = startPos?.Y;

            worksheet.Cells[lastUsedRow + 1, 10].Value = startPos?.Z;

            worksheet.Cells[lastUsedRow + 1, 11].Value = endPos?.X;

            worksheet.Cells[lastUsedRow + 1, 12].Value = endPos?.Y;

            worksheet.Cells[lastUsedRow + 1, 13].Value = endPos?.Z;


            excelPackage.Save();
        }

        /// <summary>
        /// 上视测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnUpLookTest_Click(object sender, EventArgs e)
        {
            this.cycles = (int)this.SpCycles.Value;

            this.visionTime = (int)this.SpVisionTime.Value;

            this.isMoveToCameraCenterAfterVision = this.ChkMoveToCameraCenterAfterVision.Checked;
            StartTask2();
        }
    }
}