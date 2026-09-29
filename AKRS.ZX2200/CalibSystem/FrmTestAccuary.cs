using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Modules;
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace AKRS.ZX2200.CalibSystem
{
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.LeadShine.E5032;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.LogicHardware.Services;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Models.Services;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Controls.Assistant;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.CalibSystem.TestControls;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using DevExpress.XtraBars.Docking;
    using DevExpress.XtraEditors;
    using log4net.Core;
    using OfficeOpenXml;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using FileInfo = System.IO.FileInfo;
    using LicenseContext = OfficeOpenXml.LicenseContext;

    public partial class FrmTestAccuary : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// Bond模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara bMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;


        /// <summary>
        /// Bond光源控制器
        /// </summary>
        private LightController LightController => HardwareRepositoryService.GetHardware<LightController>("BondZ");

        private Light BondLightRed = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-红");

        private Light BondLightGreen = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-绿");

        private Light BondLightBlue = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-蓝");

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara calibratePara => CalibrateRunPara.GetInstance();

        private DispenseModule dispenseModule = new DispenseModule();

        /// <summary>
        /// A
        /// </summary>
        private AKRSPoint3D pointA;

        private List<AKRSPoint3D> pointAList = new List<AKRSPoint3D>();

        /// <summary>
        /// B
        /// </summary>
        private AKRSPoint3D pointB;

        public FrmTestAccuary()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            //FrmPRList frmPrList = new FrmPRList();
            //frmPrList.Show();

            this.EditPr("测试");

            //MatchResult a = this.LocatePosition("小标定片模板",out double Time);
        }

        private void BtnGetA_Click(object sender, EventArgs e)
        {
            this.pointA = this.bondModuleController.Get3DRealPosition();
            this.pointAList.Add(this.pointA);

            AKRSXtraMessageBox.Show(this.pointAList.Count.ToString());
        }

        private void BtnGetB_Click(object sender, EventArgs e)
        {
            this.pointB = this.bondModuleController.Get3DRealPosition();
            AKRSXtraMessageBox.Show("getB ok");
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();
                List<AKRSPoint3D> pointBList = new List<AKRSPoint3D>();
                List<AKRSPoint3D> pointA2List = new List<AKRSPoint3D>();

                List<MatchResult> resultA1List = new List<MatchResult>();
                List<MatchResult> resultBList = new List<MatchResult>();
                List<MatchResult> resultA2List = new List<MatchResult>();

                List<double> a1TimeList = new List<double>();
                List<double> a2TimeList = new List<double>();
                List<double> a1ToBTimeList = new List<double>();

                int count = int.Parse(this.textEdit1.Text);
                double timeA1 = 0, timeA2 = 0, moveTime = 0;

                //for (int i = 0; i < count; i++)
                //{
                //    this.bondModuleController.MoveSafeBondXYZ(this.pointA, true);
                //    Thread.Sleep(50);
                //    AKRSPoint3D a1 = this.bondModuleController.Get3DRealPosition();
                //    MatchResult resultA1 = this.LocatePosition("PointA", out timeA1);

                //    Stopwatch sw = Stopwatch.StartNew();
                //    sw.Start();
                //    this.bondModuleController.MoveSafeBondXYZ(this.pointB, true);
                //    sw.Stop();
                //    Thread.Sleep(50);
                //    moveTime = sw.Elapsed.TotalMilliseconds;

                //    AKRSPoint3D b = this.bondModuleController.Get3DRealPosition();
                //    // MatchResult resultB = this.LocatePosition("PointB");

                //    this.bondModuleController.MoveSafeBondXYZ(this.pointA, true);
                //    Thread.Sleep(50);
                //    AKRSPoint3D a2 = this.bondModuleController.Get3DRealPosition();
                //    MatchResult resultA2 = this.LocatePosition("PointA", out timeA2);

                //    pointA1List.Add(a1);
                //    pointBList.Add(b);
                //    pointA2List.Add(a2);
                //    resultA1List.Add(resultA1);
                //    // resultBList.Add(resultB);
                //    resultA2List.Add(resultA2);

                //    a1TimeList.Add(timeA1);
                //    a2TimeList.Add(timeA2);
                //    a1ToBTimeList.Add(moveTime);
                //}
                //pointAList.Add(new AKRSPoint3D(11.5121, -19.3211, -20.6726));
                //pointAList.Add(new AKRSPoint3D(12.0733, -80.253, -20.6726));
                //pointAList.Add(new AKRSPoint3D(101.9661, -79.4171, -20.6726));
                //pointAList.Add(new AKRSPoint3D(101.3901, -18.5331, -20.6726));
                //pointAList.Add(new AKRSPoint3D(56.7861, -55.5983, -20.6726));


                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear(); pointBList.Clear();
                    pointA2List.Clear(); resultA1List.Clear();
                    resultA2List.Clear(); a1TimeList.Clear(); a2TimeList.Clear();
                    a1ToBTimeList.Clear();

                    for (int i = 0; i < count; i++)
                    {
                        this.bondModuleController.MoveSafeBondXYZ(pointa);
                        Thread.Sleep(30);
                        AKRSPoint3D a1 = this.bondModuleController.Get3DRealPosition();
                        MatchResult resultA1 = this.LocatePosition("PointA", out timeA1);

                        Stopwatch sw = Stopwatch.StartNew();
                        sw.Start();
                        this.bondModuleController.MoveSafeBondXYZ(this.pointB);
                        sw.Stop();
                        Thread.Sleep(30);
                        moveTime = sw.Elapsed.TotalMilliseconds;

                        AKRSPoint3D b = this.bondModuleController.Get3DRealPosition();
                        // MatchResult resultB = this.LocatePosition("PointB");

                        this.bondModuleController.MoveSafeBondXYZ(pointa);
                        Thread.Sleep(30);
                        AKRSPoint3D a2 = this.bondModuleController.Get3DRealPosition();
                        MatchResult resultA2 = this.LocatePosition("PointA", out timeA2);

                        pointA1List.Add(a1);
                        pointBList.Add(b);
                        pointA2List.Add(a2);
                        resultA1List.Add(resultA1);
                        // resultBList.Add(resultB);
                        resultA2List.Add(resultA2);

                        a1TimeList.Add(timeA1);
                        a2TimeList.Add(timeA2);
                        a1ToBTimeList.Add(moveTime);
                    }


                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\aba.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "A1x";
                    worksheet.Cells[1, 3].Value = "A1y";
                    worksheet.Cells[1, 4].Value = "Bx";
                    worksheet.Cells[1, 5].Value = "By";
                    worksheet.Cells[1, 6].Value = "A2x";
                    worksheet.Cells[1, 7].Value = "A2y";

                    worksheet.Cells[1, 8].Value = "a1x";
                    worksheet.Cells[1, 9].Value = "a1y";
                    worksheet.Cells[1, 10].Value = "a2x";
                    worksheet.Cells[1, 11].Value = "a2y";

                    worksheet.Cells[1, 12].Value = "timeA1";
                    worksheet.Cells[1, 13].Value = "timeA2";
                    worksheet.Cells[1, 14].Value = "timeA1toB";

                    // 写入数据
                    for (int row = 0; row < pointA1List.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = row + 1;
                        worksheet.Cells[row + 2, 2].Value = pointA1List[row].X.ToString();
                        worksheet.Cells[row + 2, 3].Value = pointA1List[row].Y.ToString();
                        worksheet.Cells[row + 2, 4].Value = pointBList[row].X.ToString();
                        worksheet.Cells[row + 2, 5].Value = pointBList[row].Y.ToString();
                        worksheet.Cells[row + 2, 6].Value = pointA2List[row].X.ToString();
                        worksheet.Cells[row + 2, 7].Value = pointA2List[row].Y.ToString();


                        worksheet.Cells[row + 2, 8].Value = resultA1List[row].CenterX.ToString();
                        worksheet.Cells[row + 2, 9].Value = resultA1List[row].CenterY.ToString();
                        worksheet.Cells[row + 2, 10].Value = resultA2List[row].CenterX.ToString();
                        worksheet.Cells[row + 2, 11].Value = resultA2List[row].CenterY.ToString();

                        worksheet.Cells[row + 2, 12].Value = a1TimeList[row];
                        worksheet.Cells[row + 2, 13].Value = a2TimeList[row];
                        worksheet.Cells[row + 2, 14].Value = a1ToBTimeList[row];
                    }

                    // 保存Excel文件
                    package.Save();
                }




            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }

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

            Stopwatch stopwatch = new Stopwatch();

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

        private void BtnStartOnlyA_Click(object sender, EventArgs e)
        {
            try
            {
                // int count = int.Parse(this.textEdit1.Text);
                DateTime currentTime = DateTime.Now;

                DateTime tomorrow = currentTime.AddDays(0);

                DateTime targetTime = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, 9, 0, 0);

                // List<MatchResult> resultAList = new List<MatchResult>();
                List<(string, double, double)> resultAList = new List<(string, double, double)>();

                // 获取Pr实体
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("新PR模板393266");

                //this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().BMCCenterPosition);

                // 记录上次保存图片的时间
                DateTime lastSaveImageTime = DateTime.Now;

                while (DateTime.Now < targetTime)
                {
                    Thread.Sleep(200);
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        //throw new ArgumentNullException("PointA", "The PointA excuteResult is fail.");
                        goto SaveFileDialog;
                    }

                    // 获取定位结果
                    MatchResult resultA = (MatchResult)pREntity.AlgResult;

                    resultAList.Add((DateTime.Now.ToString("yyyyMMddHHmmss"), resultA.CenterX, resultA.CenterY));

                    if ((DateTime.Now - lastSaveImageTime).TotalMinutes >= 1)
                    {
                        // 保存图片
                        string imageFileName = $"D:\\静止定位图片\\{DateTime.Now.ToString("yyyyMMddHHmmss")}.bmp";
                        Directory.CreateDirectory(Path.GetDirectoryName(imageFileName));
                        Bitmap image = ImageHelp.ImageBaseDataV2ToBitmap(resultA.OutPutImg);

                        image.Save(imageFileName);

                        // 更新上次保存图片的时间
                        lastSaveImageTime = DateTime.Now;
                    }
                }

            SaveFileDialog:
                if (resultAList.Count > 200)
                {
                    // ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\温漂数据\\Substrate温漂\\onlyA{DateTime.Now.ToString("yyyyMMddHHmmss")}.xlsx"));
                    ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\静止定位.xlsx"));

                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "时间";
                    worksheet.Cells[1, 2].Value = "a1x";
                    worksheet.Cells[1, 3].Value = "a1y";

                    // 写入数据
                    for (int row = 0; row < resultAList.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = resultAList[row].Item1;
                        worksheet.Cells[row + 2, 2].Value = resultAList[row].Item2;
                        worksheet.Cells[row + 2, 3].Value = resultAList[row].Item3;
                    }

                    // 保存Excel文件
                    package.Save();
                    resultAList.Clear();
                    // break;
                }

                AKRSXtraMessageBox.Show("1");

                //for (int i = 0; i < count; i++)
                //{
                //    MatchResult resultA = this.LocatePosition("PointA", out double time);
                //    resultAList.Add(resultA);
                //}


                //ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\CorrectionData.xlsx"));
                //ExcelWorksheet worksheet1 = excelPackage.Workbook.Worksheets.Count > 0
                //                               ? excelPackage.Workbook.Worksheets[0]
                //                               : excelPackage.Workbook.Worksheets.Add("DataSheet");
                //excelPackage.Save();

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }

        private void BtnStartAngle_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    List<double> realAngleList = new List<double>();
                    List<double> imageAngleList = new List<double>();
                    List<double> codeAngleList = new List<double>();

                    List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();


                    for (int j = 0; j < 2; j++)
                    {
                        for (double i = -175; i <= 175; i += 0.1)
                        {
                            this.BondModule.BondHead.AxisT.AbsoluteMove(i);
                            Thread.Sleep(50);
                            MatchResult resultA = this.LocatePosition("测试", out double time);

                            resultList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                            realAngleList.Add(i);
                            imageAngleList.Add(resultA.Angle);
                            codeAngleList.Add(this.bondHeadController.GetAxisTRealPos());
                        }
                    }

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\Angle.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "realAngle";
                    worksheet.Cells[1, 3].Value = "imageAngle";
                    worksheet.Cells[1, 4].Value = "centerX";
                    worksheet.Cells[1, 5].Value = "centerY";
                    worksheet.Cells[1, 6].Value = "codeAngle";

                    // 写入数据
                    for (int row = 0; row < realAngleList.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = (row + 1).ToString();
                        worksheet.Cells[row + 2, 2].Value = realAngleList[row];
                        worksheet.Cells[row + 2, 3].Value = imageAngleList[row];
                        worksheet.Cells[row + 2, 4].Value = resultList[row].X;
                        worksheet.Cells[row + 2, 5].Value = resultList[row].Y;
                        worksheet.Cells[row + 2, 6].Value = codeAngleList[row];
                    }

                    // 保存Excel文件
                    package.Save();

                    AKRSXtraMessageBox.Show("ok");
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                    return;
                }
            });

        }

        public void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = algBeLong;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.Show();
            VisionEntityRepository.GetInstance().Save();
        }

        private AKRSPoint3D pointStart;

        private AKRSPoint3D pointEnd;

        private void BtnTestTime_Click(object sender, EventArgs e)
        {

        }

        private void BtnGetStart_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();
                List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();

                int count = int.Parse(this.textEdit1.Text);

                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear();

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\DispenseCameraCalibTest.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "实际坐标X";
                    worksheet.Cells[1, 3].Value = "实际坐标Y";

                    worksheet.Cells[1, 4].Value = "转换坐标X";
                    worksheet.Cells[1, 5].Value = "转换坐标Y";

                    //worksheet.Cells[1, 6].Value = "转换坐标X-实际坐标X";
                    //worksheet.Cells[1, 7].Value = "转换坐标Y-实际坐标Y";

                    this.dispenseModule.MoveDispenseXAndY(pointa.X, pointa.Y);
                    this.dispenseModule.AxisZAbsoluteMove(pointa.Z);

                    MatchResult result = this.LocatePosition("PointD", out double time);
                    AKRSPoint2D trans = CalibService.GetMachinePosByPixelPos(
                        new AKRSPoint2D(pointa.X, pointa.Y),
                        result,
                        "DispenseCameraCoordinateSystem");

                    // trans = trans - new AKRSPoint2D(pointa.X, pointa.Y);

                    List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();

                    // 生成晶圆台标定点位列表
                    pointACalibList = this.GeneratePointList(
                        pointa,
                        0.5,
                        0.5,
                        3,
                        3);


                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            this.dispenseModule.MoveDispenseXAndY(pointACalibList[j].X, pointACalibList[j].Y);

                            MatchResult resultA = this.LocatePosition("PointD", out double timeA1);

                            AKRSPoint2D transA = CalibService.GetMachinePosByPixelPos(
                                pointACalibList[j],
                                resultA,
                                "DispenseCameraCoordinateSystem");

                            // resultList.Add(transA - pointACalibList[j]);

                            // 写入数据
                            worksheet.Cells[i * 9 + 2 + j, 1].Value = i * 9 + j;
                            worksheet.Cells[i * 9 + 2 + j, 2].Value = pointACalibList[j].X - pointa.X;
                            worksheet.Cells[i * 9 + 2 + j, 3].Value = pointACalibList[j].Y - pointa.Y;
                            worksheet.Cells[i * 9 + 2 + j, 4].Value = transA.X - trans.X;
                            worksheet.Cells[i * 9 + 2 + j, 5].Value = transA.Y - trans.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 6].Value = transA.X - pointa.X;
                            //worksheet.Cells[i * 9 + 2 + j, 7].Value = transA.Y - pointa.Y;
                        }
                    }

                    // 保存Excel文件
                    package.Save();
                }




            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
        }

        private void BtnGetEnd_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            //AKRSPoint3D point1 = new AKRSPoint3D(0, 0, 0);
            //AKRSPoint3D point2 = new AKRSPoint3D(80, -100, -20);
            //Task.Run(
            //    () =>
            //        {
            //            while (true)
            //            {
            //                this.dispenseModule.MoveAxis(point1);
            //                Thread.Sleep(100);
            //                this.dispenseModule.MoveAxis(point2);
            //                Thread.Sleep(100);

            //            }
            //        });
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            //pointAList.Clear();
            //pointAList.Add(new AKRSPoint3D(28.6035, -32.0204, -15.2241));
            //pointAList.Add(new AKRSPoint3D(28.7987, -68.5628, -15.2241));
            //pointAList.Add(new AKRSPoint3D(88.5655, -32.0204, -15.2241));
            //pointAList.Add(new AKRSPoint3D(88.5655, -68.5628, -15.2241));


            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();
                List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();

                int count = int.Parse(this.textEdit1.Text);

                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear();

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\BondCalibTest.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "实际坐标X";
                    worksheet.Cells[1, 3].Value = "实际坐标Y";

                    worksheet.Cells[1, 4].Value = "转换坐标X";
                    worksheet.Cells[1, 5].Value = "转换坐标Y";

                    //worksheet.Cells[1, 6].Value = "转换坐标X-实际坐标X";
                    //worksheet.Cells[1, 7].Value = "转换坐标Y-实际坐标Y";

                    // this.dispenseModule.MoveDispenseXAndY(pointa.X, pointa.Y);
                    this.bondModuleController.MoveSafeBondXY(pointa.X, pointa.Y);
                    this.BondModule.BondHead.AxisZ.AbsoluteMove(pointa.Z);

                    MatchResult result = this.LocatePosition("PointA", out double time);
                    AKRSPoint2D trans = CalibService.GetMachinePosByPixelPos(
                        new AKRSPoint2D(pointa.X, pointa.Y),
                        result,
                        "BondCameraCoordinateSystem");

                    List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();

                    // 生成晶圆台标定点位列表
                    pointACalibList = this.GeneratePointList(
                        pointa,
                        0.5,
                        0.5,
                        3,
                        3);


                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            // this.dispenseModule.MoveDispenseXAndY(pointACalibList[j].X, pointACalibList[j].Y);
                            this.bondModuleController.MoveSafeBondXY(pointACalibList[j].X, pointACalibList[j].Y);

                            MatchResult resultA = this.LocatePosition("PointA", out double timeA1);

                            AKRSPoint2D transA = CalibService.GetMachinePosByPixelPos(
                                pointACalibList[j],
                                resultA,
                                "BondCameraCoordinateSystem");

                            // resultList.Add(transA - pointACalibList[j]);

                            // 写入数据
                            worksheet.Cells[i * 9 + 2 + j, 1].Value = i * 9 + j;
                            worksheet.Cells[i * 9 + 2 + j, 2].Value = pointACalibList[j].X - pointa.X;
                            worksheet.Cells[i * 9 + 2 + j, 3].Value = pointACalibList[j].Y - pointa.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 4].Value = transA.X - pointACalibList[j].X;
                            //worksheet.Cells[i * 9 + 2 + j, 5].Value = transA.Y - pointACalibList[j].Y;
                            worksheet.Cells[i * 9 + 2 + j, 4].Value = transA.X - trans.X;
                            worksheet.Cells[i * 9 + 2 + j, 5].Value = transA.Y - trans.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 6].Value = transA.X - pointa.X;
                            //worksheet.Cells[i * 9 + 2 + j, 7].Value = transA.Y - pointa.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 6].Value = pointACalibList[j].X - pointa.X - (transA.X - trans.X);
                            //worksheet.Cells[i * 9 + 2 + j, 7].Value = pointACalibList[j].Y - pointa.Y - (transA.Y - trans.Y);

                        }

                    }

                    // 保存Excel文件
                    package.Save();

                }




            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
        }


        public List<AKRSPoint2D> GeneratePointList(AKRSPoint3D centerPoint, double rowdistance, double coldistance, int numRows, int numColumns)
        {
            List<AKRSPoint2D> pointList = new List<AKRSPoint2D>();

            double startX = centerPoint.X - ((numColumns - 1) / 2.0 * coldistance); // 左上角点的X坐标
            double startY = centerPoint.Y - ((numRows - 1) / 2.0 * rowdistance); // 左上角点的Y坐标

            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numColumns; j++)
                {
                    double x = startX + (j * coldistance);
                    double y = startY + (i * rowdistance);
                    AKRSPoint2D point = new AKRSPoint2D(x, y);
                    pointList.Add(point);
                }
            }
            return pointList;
        }

        private void BtnRemoveA_Click(object sender, EventArgs e)
        {
            this.pointAList.RemoveAt(this.pointAList.Count - 1);
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();
                List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();

                int count = int.Parse(this.textEdit1.Text);

                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear();

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\WaferCalibTest.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "实际坐标X";
                    worksheet.Cells[1, 3].Value = "实际坐标Y";

                    worksheet.Cells[1, 4].Value = "转换坐标X";
                    worksheet.Cells[1, 5].Value = "转换坐标Y";

                    //worksheet.Cells[1, 6].Value = "转换坐标X-实际坐标X";
                    //worksheet.Cells[1, 7].Value = "转换坐标Y-实际坐标Y";

                    // this.dispenseModule.MoveDispenseXAndY(pointa.X, pointa.Y);
                    WaferSubModule.GetInstance().WaferTable.WaferTableAxisX.AbsoluteMove(pointa.X);
                    WaferSubModule.GetInstance().WaferTable.WaferTableAxisY.AbsoluteMove(pointa.Y);

                    MatchResult result = this.LocatePosition("PointB", out double timeA);

                    AKRSPoint2D trans = CalibService.GetMachinePosByPixelPos(
                         new AKRSPoint2D(pointa.X, pointa.Y),
                        result,
                        "WaferCameraCoordinateSystem");

                    List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();

                    // 生成晶圆台标定点位列表
                    pointACalibList = this.GeneratePointList(
                        pointa,
                        0.5,
                        0.5,
                        3,
                        3);


                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            WaferSubModule.GetInstance().WaferTable.WaferTableAxisX.AbsoluteMove(pointACalibList[j].X);
                            WaferSubModule.GetInstance().WaferTable.WaferTableAxisY.AbsoluteMove(pointACalibList[j].Y);

                            Thread.Sleep(20);

                            MatchResult resultA = this.LocatePosition("PointB", out double timeA1);

                            AKRSPoint2D transA = CalibService.GetMachinePosByPixelPos(
                                pointACalibList[j],
                                resultA,
                                "WaferCameraCoordinateSystem");

                            // resultList.Add(transA - pointACalibList[j]);

                            // 写入数据
                            worksheet.Cells[i * 9 + 2 + j, 1].Value = i * 9 + j;
                            worksheet.Cells[i * 9 + 2 + j, 2].Value = pointACalibList[j].X - pointa.X;
                            worksheet.Cells[i * 9 + 2 + j, 3].Value = pointACalibList[j].Y - pointa.Y;
                            worksheet.Cells[i * 9 + 2 + j, 4].Value = transA.X - trans.X;
                            worksheet.Cells[i * 9 + 2 + j, 5].Value = transA.Y - trans.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 6].Value = transA.X - pointa.X;
                            //worksheet.Cells[i * 9 + 2 + j, 7].Value = transA.Y - pointa.Y;

                        }

                    }

                    // 保存Excel文件
                    package.Save();

                }


            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            this.pointAList.Add(new AKRSPoint3D(WaferSubModule.GetInstance().WaferTable.WaferTableAxisX.GetRealPosition(), WaferSubModule.GetInstance().WaferTable.WaferTableAxisY.GetRealPosition(), 0));

            AKRSXtraMessageBox.Show(this.pointAList.Count.ToString());
        }

        private void simpleButton6_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();
                List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();

                int count = int.Parse(this.textEdit1.Text);

                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear();

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\UpLookCalibTest.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "实际坐标X";
                    worksheet.Cells[1, 3].Value = "实际坐标Y";

                    worksheet.Cells[1, 4].Value = "转换坐标X";
                    worksheet.Cells[1, 5].Value = "转换坐标Y";

                    //worksheet.Cells[1, 6].Value = "转换坐标X-实际坐标X";
                    //worksheet.Cells[1, 7].Value = "转换坐标Y-实际坐标Y";

                    // this.dispenseModule.MoveDispenseXAndY(pointa.X, pointa.Y);
                    this.bondModuleController.MoveSafeBondXY(pointa.X, pointa.Y);

                    MatchResult result = this.LocatePosition("PointC", out double timeA);

                    AKRSPoint2D trans = CalibService.GetMachinePosByPixelPos(
                        new AKRSPoint2D(pointa.X, pointa.Y),
                        result,
                        "UpLookCameraCoordinateSystem");
                    List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();

                    // 生成晶圆台标定点位列表
                    pointACalibList = this.GeneratePointList(
                        pointa,
                        0.5,
                        0.5,
                        3,
                        3);


                    for (int i = 0; i < count; i++)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            // this.dispenseModule.MoveDispenseXAndY(pointACalibList[j].X, pointACalibList[j].Y);
                            this.bondModuleController.MoveSafeBondXY(pointACalibList[j].X, pointACalibList[j].Y);

                            MatchResult resultA = this.LocatePosition("PointC", out double timeA1);

                            AKRSPoint2D transA = CalibService.GetMachinePosByPixelPos(
                                pointACalibList[j],
                                resultA,
                                "UpLookCameraCoordinateSystem");

                            // resultList.Add(transA - pointACalibList[j]);

                            // 写入数据
                            worksheet.Cells[i * 9 + 2 + j, 1].Value = i * 9 + j;
                            worksheet.Cells[i * 9 + 2 + j, 2].Value = pointACalibList[j].X - pointa.X;
                            worksheet.Cells[i * 9 + 2 + j, 3].Value = pointACalibList[j].Y - pointa.Y;
                            worksheet.Cells[i * 9 + 2 + j, 4].Value = transA.X - trans.X;
                            worksheet.Cells[i * 9 + 2 + j, 5].Value = transA.Y - trans.Y;
                            //worksheet.Cells[i * 9 + 2 + j, 6].Value = transA.X - pointa.X;
                            //worksheet.Cells[i * 9 + 2 + j, 7].Value = transA.Y - pointa.Y;

                        }

                    }

                    // 保存Excel文件
                    package.Save();

                }




            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
        }

        private void simpleButton7_Click(object sender, EventArgs e)
        {

        }

        private void simpleButton8_Click(object sender, EventArgs e)
        {
            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().GlassVisionMachinePos);

            // 定位小圆并移动至相机中心
            MatchResult smallMatchResult = this.LocatePosition("小标定片模板", out double time);

            CalibService.MoveToCamCenterCoo(BondModule.BondAxisX, BondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().GlassVisionMachinePos + CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset);

            // Z轴下降
            BondModule.BondHead.AxisZ.AbsoluteMove(CalibrateRunPara.GetInstance().GlassPickZMachinePos);

            // 取标
            BondModule.BondHead.VaccumElectric.SetOutputValue(true);

            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos);
        }

        private void simpleButton9_Click(object sender, EventArgs e)
        {
            //MatchResult a = this.LocatePosition("大标定片模板", out double time);
            //CalibService.MoveToCamCenterCoo(this.BondModule.BondAxisX, this.BondModule.BondAxisY, a, "BondCameraCoordinateSystem");

            MatchResult result = this.LocatePosition(this.calibratePara.BmcPRName);
            AKRSPoint2D BondTestPos = CalibService.GetMachinePosByPixelPos(this.bondModuleController.Get2DRealPosition(), result, "BondCameraCoordinateSystem");
            //AKRSPoint2D BondTestPos1 = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(200, 200), new MatchResult(2000, 2000, 0), "BondCameraCoordinateSystem");
        }

        private void simpleButton10_Click(object sender, EventArgs e)
        {
            AutoFocusing autofocus = new AutoFocusing();
            AKRSCamera testCamera = HardwareRepositoryService.GetHardware<AKRSCamera>("上视相机");
            List<int> distanceList = new List<int>();
            List<double> zPosList = new List<double>();
            Task.Run(() =>
            {
                for (int i = 5; i < 40; i += 10)
                {
                    for (int j = 0; j < 10; i++)
                    {
                        autofocus.AutoFocus(testCamera, this.BondModule.BondHead.AxisZ, 80, i);
                        double zPos = this.BondModule.BondHead.AxisZ.GetRealPosition();
                        distanceList.Add(80 - i);
                        zPosList.Add(zPos);
                    }
                }


                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\autoFocus.xlsx"));
                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "行程";
                worksheet.Cells[1, 3].Value = "zPos";

                // 写入数据
                for (int row = 0; row < zPosList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = distanceList[row];
                    worksheet.Cells[row + 2, 3].Value = zPosList[row];

                }

                // 保存Excel文件
                package.Save();
            });

        }

        private void simpleButton11_Click(object sender, EventArgs e)
        {
            List<double> timelist = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                Thread.Sleep(10);
                MatchResult a = this.LocatePosition("测试模板", out double time);
                Console.WriteLine("PR总时间:{0}", time);
                timelist.Add(time);
            }


            ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\prtime.xlsx"));
            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "PRtime";

            // 写入数据
            for (int row = 0; row < timelist.Count; row++)
            {
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = timelist[row];

            }

            // 保存Excel文件
            package.Save();
        }

        private void simpleButton12_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();

                List<Result> resultA1List = new List<Result>();

                int count = int.Parse(this.textEdit1.Text);
                double timeA1 = 0, timeA2 = 0, moveTime = 0;

                List<double> speedList = new List<double>() { 1000, 900, 800, 700, 600, 500, 400 };
                List<int> sleepTimeList = new List<int>() { 30, 50, 70, 90, 110 };

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ab.xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                // 添加一个工作表


                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "到位延时";
                worksheet.Cells[1, 4].Value = "定位X";
                worksheet.Cells[1, 5].Value = "定位Y";
                AKRSPoint3D pointA = new AKRSPoint3D(-113.4265, -141.5438, -24.7484);
                AKRSPoint3D pointB = new AKRSPoint3D(22.175, -59.0253, -24.7484);
                int index = 0;
                foreach (double speed in speedList)
                {
                    //this.BondModule.BondAxisX.SetVel(sp);
                    //this.BondModule.BondAxisX.SetAcc(sp * 10);
                    //this.BondModule.BondAxisX.SetDec(sp * 10);

                    //this.BondModule.BondAxisY.SetVel(sp);
                    //this.BondModule.BondAxisY.SetAcc(sp * 10);
                    //this.BondModule.BondAxisY.SetDec(sp * 10);


                    foreach (int sleepTime in sleepTimeList)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            MovePara movePara = new MovePara();
                            movePara.Vel = speed;
                            movePara.Acc = speed * 10;
                            movePara.Dec = speed * 10;
                            movePara.Jerk = 0.02;
                            movePara.TargetPosition = pointA.X;
                            this.BondModule.BondAxisX.SendAbsoluteMoveCommand(movePara);

                            movePara = new MovePara();
                            movePara.Vel = speed;
                            movePara.Acc = speed * 10;
                            movePara.Dec = speed * 10;
                            movePara.Jerk = 0.02;
                            movePara.TargetPosition = pointA.Y;
                            this.BondModule.BondAxisY.SendAbsoluteMoveCommand(movePara);

                            this.BondModule.BondAxisX.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                            this.BondModule.BondAxisY.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);

                            Thread.Sleep(sleepTime);
                            MatchResult resultA1 = this.LocatePosition("PointA", out timeA1);


                            movePara = new MovePara();
                            movePara.Vel = speed;
                            movePara.Acc = speed * 10;
                            movePara.Dec = speed * 10;
                            movePara.Jerk = 0.02;
                            movePara.TargetPosition = pointB.X;
                            this.BondModule.BondAxisX.SendAbsoluteMoveCommand(movePara);

                            movePara = new MovePara();
                            movePara.Vel = speed;
                            movePara.Acc = speed * 10;
                            movePara.Dec = speed * 10;
                            movePara.Jerk = 0.02;
                            movePara.TargetPosition = pointB.Y;
                            this.BondModule.BondAxisY.SendAbsoluteMoveCommand(movePara);

                            this.BondModule.BondAxisX.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                            this.BondModule.BondAxisY.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);

                            Thread.Sleep(sleepTime);

                            Result result = new Result();
                            result.Index = index++;
                            result.Speed = speed;
                            result.SleepTime = sleepTime;
                            //result.CenterX = resultA1.CenterX;
                            //result.CenterY = resultA1.CenterY;
                            resultA1List.Add(result);
                        }
                        // 写入数据
                    }


                    //}
                }

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.SleepTime;
                    //worksheet.Cells[row + 2, 4].Value = ret.CenterX;
                    //worksheet.Cells[row + 2, 5].Value = ret.CenterY;
                }

                // 保存Excel文件
                package.Save();

                AKRSXtraMessageBox.Show("测试完成");

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
        }

        /// <summary>
        /// BMC真空电磁阀
        /// </summary>
        public Electric FixedCyc { get; set; } = HardwareRepositoryService.GetHardware<Electric>("校正台真空");


        private void simpleButton13_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                StartBondToCamCalibTaskWithLocate();
            });
        }

        /// <summary>
        /// 邦头旋转中心到Bond相机中心距离标定线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartBondToCamCalibTask()
        {
            List<(double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 )> resultList = new List<(double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 )>();
            try
            {
                // this.bondHeadController.OpenBondHeadVaccum();

                this.bondHeadController.RotateAxisT(0);

                // Bond移动到小标定片中心点拍照位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition("小标定片模板");

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, smallMatchResult);

                // 更新小圆中心位置
                this.calibratePara.GlassVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                AKRSPoint3D targetPos = new AKRSPoint3D()
                {
                    X = this.calibratePara.GlassVisionMachinePos.X
                                                    + this.calibratePara.BondRotateCenterToCamOffset.X,
                    Y = this.calibratePara.GlassVisionMachinePos.Y
                                                    + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    Z = this.calibratePara.GlassVisionMachinePos.Z
                };

                // 移动到取标位置
                this.bondModuleController.MoveSafeBondXYZ(targetPos);

                this.calibratePara.GlassPickZMachinePos = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult - 3.0;

                // 取放片参数
                BaseCarrierConfig component = new BaseCarrierConfig()
                {
                    // 取片
                    IsActivateSlowTravelBeforePickup = true,
                    SlowTravelSpeedBeforePickup =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,

                    // 真空延时
                    //IPTVacuumOffDelay = 500,
                    //IPTVacuumBuildUpDelay = 500,

                    // 放片
                    IsActivateSlowTravelBeforeBonding = true,
                    SlowTravelSpeedBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                    SlowTravelDistanceBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                    IsActivateSlowTravelAfterBonding = true,
                    SlowTravelSpeedAfterBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                    SlowTravelDistanceAfterBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                    BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                    PlacementDelay = this.bMCDevicePara.PlacementDelay,
                    IsActiveComponentDetection = true,
                    BondingForceMode = ForceModeEnum.Distance,

                    WeakBlowProportion = 1000,
                };

                double pickLevel = this.calibratePara.GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                Thread.Sleep(100);

                // 更新放标位（高度未知）
                this.calibratePara.PlaceToBMCMachinePos = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X - 2, this.calibratePara.BMCVisionMachinePos.Y - 2, BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8);

                AKRSPoint3D targetPos2 = new AKRSPoint3D(
                    this.calibratePara.PlaceToBMCMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                    this.calibratePara.PlaceToBMCMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位(BMC左下角位置，高度暂定)
                this.bondModuleController.MoveSafeBondXY(targetPos2.X, targetPos2.Y);

                // 放片参数,标定片厚度：2
                double placeLevel = this.calibratePara.PlaceToBMCMachinePos.Z;
                liftLevel = CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z;

                // 放片
                bool ret = this.bondHeadController.BondAction(
                    placeLevel,
                    liftLevel,
                    component,
                    BondTypeEnum.BondOnBMC);

                Thread.Sleep(500);

                // 更新当前小标定片中心位置
                this.calibratePara.GlassCenterCurVisionMachinePos = new AKRSPoint3D { X = this.calibratePara.PlaceToBMCMachinePos.X, Y = this.calibratePara.PlaceToBMCMachinePos.Y, Z = -8.2244 };

                // Bond相机移动定位透明片Mark左上点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                this.calibController.AutoFocus(CalibController.CamCoordinateType.Bond, this.calibratePara.BMCVisionMachinePos.Z + 2, this.calibratePara.BMCVisionMachinePos.Z - 3);

                double z = this.bondModuleController.Get3DRealPosition().Z;

                this.calibratePara.GlassCenterCurVisionMachinePos.Z = z;

                MatchResult topLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult);

                // 更新此时左上Mark点坐标
                AKRSPoint3D glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                // Bond相机移动定位透明片Mark右下点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                MatchResult botRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult);

                // 更新此时右下Mark点坐标
                AKRSPoint3D glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();


                this.calibratePara.GlassCenterCurVisionMachinePos.X = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                // 更新取料(放料)位置
                this.calibratePara.PlaceToBMCMachinePos.X = this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X;
                this.calibratePara.PlaceToBMCMachinePos.Y = this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y;
                this.calibratePara.PlaceToBMCMachinePos.Z = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                List<AKRSPoint2D> centerList = new List<AKRSPoint2D>();
                this.bondHeadController.RotateAxisT(0);

                int[] rotateAngle = new int[2];

                foreach (var angle in rotateAngle)
                {
                    // 移动到取料位
                    this.bondModuleController.MoveSafeBondXY(this.calibratePara.PlaceToBMCMachinePos.X, this.calibratePara.PlaceToBMCMachinePos.Y);

                    // 计算参数
                    pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                    // 取片
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                    // Z轴升起
                    this.bondHeadController.MoveBondZToSafePos();

                    // 焊头旋转
                    this.bondHeadController.RotateAxisT(angle);

                    // 放标
                    ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel,
                        component,
                        BondTypeEnum.BondOnBMC);

                    Thread.Sleep(800);

                    // Bond相机移动定位透明片Mark左上点
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                    MatchResult topLeftMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                    // Bond相机移动,Mark点到相机中心
                    this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult1);

                    // 更新此时左上Mark点坐标
                    glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                    // Bond相机移动定位透明片Mark右下点
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                    MatchResult botRightMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                    // Bond相机移动,Mark点到相机中心
                    this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult1);

                    // 更新此时右下Mark点坐标
                    glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();

                    double glassCenterX = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                    double glassCenterY = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                    centerList.Add(new AKRSPoint2D(glassCenterX, glassCenterY));
                }

                this.calibratePara.GlassCenterCurVisionMachinePos.X = centerList[1].X;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = centerList[1].Y;

                int step = 0;

                #region 校正偏移
                int[] rotateAngle2 = new int[20];
                double angleOffset = 0;
                AKRSPoint2D centerOffset = new AKRSPoint2D();
                AKRSPoint2D glassAbsCenter = new AKRSPoint2D();

                List<AKRSPoint2D> centerOffsetList = new List<AKRSPoint2D>();

                foreach (var angle in rotateAngle2)
                {
                    (double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 ) result = default;

                    AKRSPoint3D targetPos3 = new AKRSPoint3D(
                        this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                        this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                        this.calibratePara.PlaceToBMCMachinePos.Z);

                    // 移至放标位
                    this.bondModuleController.MoveSafeBondXY(targetPos3.X, targetPos3.Y);

                    result.pickPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.pickPosY = this.bondModuleController.Get2DRealPosition().Y;

                    // 取标
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                    AKRSPoint3D targetPos4 = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.BMCVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.BMCVisionMachinePos.Z);

                    // 移动到BMC中点位置
                    this.bondModuleController.MoveSafeBondXYZ(targetPos4);

                    // 放标
                    ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel,
                        component,
                        BondTypeEnum.BondOnBMC);

                    Thread.Sleep(100);

                    this.calibratePara.GlassCenterCurVisionMachinePos.X = this.calibratePara.BMCVisionMachinePos.X;
                    this.calibratePara.GlassCenterCurVisionMachinePos.Y = this.calibratePara.BMCVisionMachinePos.Y;

                    this.calibratePara.BMCMarkTopLeftVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                    this.calibratePara.BMCMarkBotRightVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                    // Bond相机移动定位BMC左上点Mark
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkTopLeftVisionMachinePos);
                    this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                    result.mark1AxisPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.mark1AxisPosY = this.bondModuleController.Get2DRealPosition().Y;

                    MatchResult glassTopLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                    MatchResult bMCTopLeftMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                    AKRSPoint2D glassTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), glassTopLeftMatchResult, "BondCameraCoordinateSystem");

                    AKRSPoint2D bMCTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), bMCTopLeftMatchResult, "BondCameraCoordinateSystem");

                    // Bond相机移动定位BMC右下点Mark
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkBotRightVisionMachinePos);
                    this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                    result.mark2AxisPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.mark2AxisPosY = this.bondModuleController.Get2DRealPosition().Y;

                    MatchResult glassBotRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                    MatchResult bMCBotRightMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                    AKRSPoint2D glassBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), glassBotRightMatchResult, "BondCameraCoordinateSystem");

                    AKRSPoint2D bMCBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), bMCBotRightMatchResult, "BondCameraCoordinateSystem");

                    glassAbsCenter = new AKRSPoint2D((glassTopAbs.X + glassBotAbs.X) / 2, (glassTopAbs.Y + glassBotAbs.Y) / 2);
                    AKRSPoint2D bMCAbsCenter = new AKRSPoint2D((bMCTopAbs.X + bMCBotAbs.X) / 2, (bMCTopAbs.Y + bMCBotAbs.Y) / 2);

                    double glassAngle = CalibService.CalculateAngle(glassTopAbs, glassBotAbs);
                    double bmcAngle = CalibService.CalculateAngle(bMCTopAbs, bMCBotAbs);

                    angleOffset = glassAngle - bmcAngle;

                    centerOffset.X = glassAbsCenter.X - bMCAbsCenter.X;
                    centerOffset.Y = glassAbsCenter.Y - bMCAbsCenter.Y;

                    AKRSPoint2D centerOffsetTemp = new AKRSPoint2D(
                        glassAbsCenter.X - bMCAbsCenter.X,
                        glassAbsCenter.Y - bMCAbsCenter.Y);

                    centerOffsetList.Add(centerOffsetTemp);

                    result.matchResultTopMark1X = glassTopLeftMatchResult.CenterX;
                    result.matchResultTopMark1Y = glassTopLeftMatchResult.CenterY;
                    result.matchResultBottomMark1X = bMCTopLeftMatchResult.CenterX;
                    result.matchResultBottomMark1Y = bMCTopLeftMatchResult.CenterY;
                    result.matchResultTopMark2X = glassBotRightMatchResult.CenterX;
                    result.matchResultTopMark2Y = glassBotRightMatchResult.CenterY;
                    result.matchResultBottomMark2X = bMCBotRightMatchResult.CenterX;
                    result.matchResultBottomMark2Y = bMCBotRightMatchResult.CenterY;
                    result.axisT = this.bondHeadController.GetAxisTRealPos();

                    resultList.Add(result);
                }
                #endregion

                AKRSPoint3D targetPos5 = new AKRSPoint3D(this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位
                this.bondModuleController.MoveSafeBondXY(targetPos5.X, targetPos5.Y);

                // 取标
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
            finally
            {
                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\pickDown.xlsx"));
                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                // 写入数据
                for (int row = 0; row < resultList.Count; row++)
                {
                    worksheet.Cells[row + 1, 1].Value = row + 1;
                    worksheet.Cells[row + 1, 2].Value = resultList[row].pickPosX;
                    worksheet.Cells[row + 1, 3].Value = resultList[row].pickPosY;
                    worksheet.Cells[row + 1, 4].Value = resultList[row].mark1AxisPosX;
                    worksheet.Cells[row + 1, 5].Value = resultList[row].mark1AxisPosY;
                    worksheet.Cells[row + 1, 6].Value = resultList[row].mark2AxisPosX;
                    worksheet.Cells[row + 1, 7].Value = resultList[row].mark2AxisPosY;
                    worksheet.Cells[row + 1, 8].Value = resultList[row].matchResultTopMark1X;
                    worksheet.Cells[row + 1, 9].Value = resultList[row].matchResultTopMark1Y;
                    worksheet.Cells[row + 1, 10].Value = resultList[row].matchResultBottomMark1X;
                    worksheet.Cells[row + 1, 11].Value = resultList[row].matchResultBottomMark1X;
                    worksheet.Cells[row + 1, 12].Value = resultList[row].matchResultTopMark2X;
                    worksheet.Cells[row + 1, 13].Value = resultList[row].matchResultTopMark2Y;
                    worksheet.Cells[row + 1, 14].Value = resultList[row].matchResultBottomMark2X;
                    worksheet.Cells[row + 1, 15].Value = resultList[row].matchResultBottomMark2Y;
                    worksheet.Cells[row + 1, 16].Value = resultList[row].axisT;

                }

                // 保存Excel文件
                package.Save();
            }
        }


        /// <summary>
        /// 邦头旋转中心到Bond相机中心距离标定线程
        /// </summary>
        /// <returns>是否成功</returns>
        public bool StartBondToCamCalibTaskWithLocate()
        {
            List<(double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 )> resultList = new List<(double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 )>();
            try
            {
                // this.bondHeadController.OpenBondHeadVaccum();

                this.bondHeadController.RotateAxisT(0);

                // Bond移动到小标定片中心点拍照位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition("小标定片模板");

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, smallMatchResult);

                // 更新小圆中心位置
                this.calibratePara.GlassVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                AKRSPoint3D targetPos = new AKRSPoint3D()
                {
                    X = this.calibratePara.GlassVisionMachinePos.X
                                                    + this.calibratePara.BondRotateCenterToCamOffset.X,
                    Y = this.calibratePara.GlassVisionMachinePos.Y
                                                    + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    Z = this.calibratePara.GlassVisionMachinePos.Z
                };

                // 移动到取标位置
                this.bondModuleController.MoveSafeBondXYZ(targetPos);

                this.calibratePara.GlassPickZMachinePos = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult - 3.0;

                // 取放片参数
                BaseCarrierConfig component = new BaseCarrierConfig()
                {
                    // 取片
                    IsActivateSlowTravelBeforePickup = true,
                    SlowTravelSpeedBeforePickup =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,

                    // 真空延时
                    //IPTVacuumOffDelay = 500,
                    //IPTVacuumBuildUpDelay = 500,

                    // 放片
                    IsActivateSlowTravelBeforeBonding = true,
                    SlowTravelSpeedBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                    SlowTravelDistanceBeforeBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                    IsActivateSlowTravelAfterBonding = true,
                    SlowTravelSpeedAfterBonding =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                    SlowTravelDistanceAfterBonding =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                    BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                    PlacementDelay = this.bMCDevicePara.PlacementDelay,
                    IsActiveComponentDetection = true,
                    BondingForceMode = ForceModeEnum.Distance,

                    WeakBlowProportion = 1000,
                };

                double pickLevel = this.calibratePara.GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                Thread.Sleep(100);

                // 更新放标位（高度未知）
                this.calibratePara.PlaceToBMCMachinePos = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X - 2, this.calibratePara.BMCVisionMachinePos.Y - 2, BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8);

                AKRSPoint3D targetPos2 = new AKRSPoint3D(
                    this.calibratePara.PlaceToBMCMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                    this.calibratePara.PlaceToBMCMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                    this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位(BMC左下角位置，高度暂定)
                this.bondModuleController.MoveSafeBondXY(targetPos2.X, targetPos2.Y);

                // 放片参数,标定片厚度：2
                double placeLevel = this.calibratePara.PlaceToBMCMachinePos.Z;
                liftLevel = CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z;

                // 放片
                bool ret = this.bondHeadController.BondAction(
                    placeLevel,
                    liftLevel,
                    component,
                    BondTypeEnum.BondOnBMC);

                Thread.Sleep(500);

                // 更新当前小标定片中心位置
                this.calibratePara.GlassCenterCurVisionMachinePos = new AKRSPoint3D { X = this.calibratePara.PlaceToBMCMachinePos.X, Y = this.calibratePara.PlaceToBMCMachinePos.Y, Z = -8.2244 };

                // Bond相机移动定位透明片Mark左上点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                this.calibController.AutoFocus(CalibController.CamCoordinateType.Bond, this.calibratePara.BMCVisionMachinePos.Z + 2, this.calibratePara.BMCVisionMachinePos.Z - 3);

                double z = this.bondModuleController.Get3DRealPosition().Z;

                this.calibratePara.GlassCenterCurVisionMachinePos.Z = z;

                MatchResult topLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult);

                // 更新此时左上Mark点坐标
                AKRSPoint3D glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                // Bond相机移动定位透明片Mark右下点
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                MatchResult botRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                // Bond相机移动,Mark点到相机中心
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult);

                // 更新此时右下Mark点坐标
                AKRSPoint3D glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();


                this.calibratePara.GlassCenterCurVisionMachinePos.X = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                // 更新取料(放料)位置
                this.calibratePara.PlaceToBMCMachinePos.X = this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X;
                this.calibratePara.PlaceToBMCMachinePos.Y = this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y;
                this.calibratePara.PlaceToBMCMachinePos.Z = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                List<AKRSPoint2D> centerList = new List<AKRSPoint2D>();
                this.bondHeadController.RotateAxisT(0);

                int[] rotateAngle = new int[] { -180, 0 };

                foreach (var angle in rotateAngle)
                {
                    // 移动到取料位
                    this.bondModuleController.MoveSafeBondXY(this.calibratePara.PlaceToBMCMachinePos.X, this.calibratePara.PlaceToBMCMachinePos.Y);

                    // 计算参数
                    pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.8;

                    // 取片
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                    // Z轴升起
                    this.bondHeadController.MoveBondZToSafePos();

                    // 焊头旋转
                    this.bondHeadController.RotateAxisT(angle);

                    // 放标
                    ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel,
                        component,
                        BondTypeEnum.BondOnBMC);

                    Thread.Sleep(800);

                    // Bond相机移动定位透明片Mark左上点
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkTopLeftCurVisionPos);

                    MatchResult topLeftMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                    // Bond相机移动,Mark点到相机中心
                    this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, topLeftMatchResult1);

                    // 更新此时左上Mark点坐标
                    glassMarkTopLeftAbsPos = this.bondModuleController.Get3DRealPosition();


                    // Bond相机移动定位透明片Mark右下点
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassMarkBotRightCurVisionPos);

                    MatchResult botRightMatchResult1 = this.LocatePosition(this.calibratePara.GlassPRName);

                    // Bond相机移动,Mark点到相机中心
                    this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Bond, botRightMatchResult1);

                    // 更新此时右下Mark点坐标
                    glassMarkBotRightAbsPos = this.bondModuleController.Get3DRealPosition();

                    double glassCenterX = (glassMarkTopLeftAbsPos.X + glassMarkBotRightAbsPos.X) / 2;
                    double glassCenterY = (glassMarkTopLeftAbsPos.Y + glassMarkBotRightAbsPos.Y) / 2;

                    centerList.Add(new AKRSPoint2D(glassCenterX, glassCenterY));

                    this.calibratePara.PlaceToBMCMachinePos.X = glassCenterX;
                    this.calibratePara.PlaceToBMCMachinePos.Y = glassCenterY;

                }
                this.calibratePara.GlassCenterCurVisionMachinePos.X = centerList[1].X;
                this.calibratePara.GlassCenterCurVisionMachinePos.Y = centerList[1].Y;

                int step = 0;

                #region 校正偏移
                int[] rotateAngle2 = new int[20];
                double angleOffset = 0;
                AKRSPoint2D centerOffset = new AKRSPoint2D();
                AKRSPoint2D glassAbsCenter = new AKRSPoint2D();

                List<AKRSPoint2D> centerOffsetList = new List<AKRSPoint2D>();

                foreach (var angle in rotateAngle2)
                {
                    (double pickPosX, double pickPosY, double mark1AxisPosX, double mark1AxisPosY,
                 double mark2AxisPosX, double mark2AxisPosY, double matchResultTopMark1X, double matchResultTopMark1Y, double matchResultBottomMark1X, double matchResultBottomMark1Y, double matchResultTopMark2X, double matchResultTopMark2Y, double matchResultBottomMark2X, double matchResultBottomMark2Y, double axisT
                 ) result = default;

                    AKRSPoint3D targetPos3 = new AKRSPoint3D(
                        this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X,
                        this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y,
                        this.calibratePara.PlaceToBMCMachinePos.Z);

                    // 移至放标位
                    this.bondModuleController.MoveSafeBondXY(targetPos3.X, targetPos3.Y);

                    result.pickPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.pickPosY = this.bondModuleController.Get2DRealPosition().Y;

                    // 取标
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                    AKRSPoint3D targetPos4 = new AKRSPoint3D(this.calibratePara.BMCVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.BMCVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.BMCVisionMachinePos.Z);

                    // 移动到BMC中点位置
                    this.bondModuleController.MoveSafeBondXYZ(targetPos4);

                    // 放标
                    ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel,
                        component,
                        BondTypeEnum.BondOnBMC);

                    Thread.Sleep(100);

                    this.calibratePara.BMCMarkTopLeftVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                    this.calibratePara.BMCMarkBotRightVisionMachinePos.Z = this.calibratePara.GlassCenterCurVisionMachinePos.Z;

                    // Bond相机移动定位BMC左上点Mark
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkTopLeftVisionMachinePos);
                    this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                    result.mark1AxisPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.mark1AxisPosY = this.bondModuleController.Get2DRealPosition().Y;

                    MatchResult glassTopLeftMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                    MatchResult bMCTopLeftMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                    AKRSPoint2D glassTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), glassTopLeftMatchResult, "BondCameraCoordinateSystem");

                    AKRSPoint2D bMCTopAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkTopLeftVisionMachinePos.X, this.calibratePara.BMCMarkTopLeftVisionMachinePos.Y), bMCTopLeftMatchResult, "BondCameraCoordinateSystem");

                    // Bond相机移动定位BMC右下点Mark
                    this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCMarkBotRightVisionMachinePos);
                    this.bondHeadController.MoveAxisZ(this.calibratePara.GlassCenterCurVisionMachinePos.Z);

                    result.mark2AxisPosX = this.bondModuleController.Get2DRealPosition().X;
                    result.mark2AxisPosY = this.bondModuleController.Get2DRealPosition().Y;

                    MatchResult glassBotRightMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                    MatchResult bMCBotRightMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                    AKRSPoint2D glassBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), glassBotRightMatchResult, "BondCameraCoordinateSystem");

                    AKRSPoint2D bMCBotAbs = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(this.calibratePara.BMCMarkBotRightVisionMachinePos.X, this.calibratePara.BMCMarkBotRightVisionMachinePos.Y), bMCBotRightMatchResult, "BondCameraCoordinateSystem");

                    glassAbsCenter = new AKRSPoint2D((glassTopAbs.X + glassBotAbs.X) / 2, (glassTopAbs.Y + glassBotAbs.Y) / 2);
                    AKRSPoint2D bMCAbsCenter = new AKRSPoint2D((bMCTopAbs.X + bMCBotAbs.X) / 2, (bMCTopAbs.Y + bMCBotAbs.Y) / 2);

                    double glassAngle = CalibService.CalculateAngle(glassTopAbs, glassBotAbs);
                    double bmcAngle = CalibService.CalculateAngle(bMCTopAbs, bMCBotAbs);

                    this.calibratePara.GlassCenterCurVisionMachinePos.X = glassAbsCenter.X;
                    this.calibratePara.GlassCenterCurVisionMachinePos.Y = glassAbsCenter.Y;

                    angleOffset = glassAngle - bmcAngle;

                    centerOffset.X = glassAbsCenter.X - bMCAbsCenter.X;
                    centerOffset.Y = glassAbsCenter.Y - bMCAbsCenter.Y;

                    AKRSPoint2D centerOffsetTemp = new AKRSPoint2D(
                        glassAbsCenter.X - bMCAbsCenter.X,
                        glassAbsCenter.Y - bMCAbsCenter.Y);

                    centerOffsetList.Add(centerOffsetTemp);

                    result.matchResultTopMark1X = glassTopLeftMatchResult.CenterX;
                    result.matchResultTopMark1Y = glassTopLeftMatchResult.CenterY;
                    result.matchResultBottomMark1X = bMCTopLeftMatchResult.CenterX;
                    result.matchResultBottomMark1Y = bMCTopLeftMatchResult.CenterY;
                    result.matchResultTopMark2X = glassBotRightMatchResult.CenterX;
                    result.matchResultTopMark2Y = glassBotRightMatchResult.CenterY;
                    result.matchResultBottomMark2X = bMCBotRightMatchResult.CenterX;
                    result.matchResultBottomMark2Y = bMCBotRightMatchResult.CenterY;
                    result.axisT = this.bondHeadController.GetAxisTRealPos();

                    resultList.Add(result);
                }
                #endregion

                AKRSPoint3D targetPos5 = new AKRSPoint3D(this.calibratePara.GlassCenterCurVisionMachinePos.X + this.calibratePara.BondRotateCenterToCamOffset.X, this.calibratePara.GlassCenterCurVisionMachinePos.Y + this.calibratePara.BondRotateCenterToCamOffset.Y, this.calibratePara.PlaceToBMCMachinePos.Z);

                // 移至放标位
                this.bondModuleController.MoveSafeBondXY(targetPos5.X, targetPos5.Y);

                // 取标
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return false;
            }
            finally
            {
                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\pickDownWithLocate.xlsx"));
                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                // 写入数据
                for (int row = 0; row < resultList.Count; row++)
                {
                    worksheet.Cells[row + 1, 1].Value = row + 1;
                    worksheet.Cells[row + 1, 2].Value = resultList[row].pickPosX;
                    worksheet.Cells[row + 1, 3].Value = resultList[row].pickPosY;
                    worksheet.Cells[row + 1, 4].Value = resultList[row].mark1AxisPosX;
                    worksheet.Cells[row + 1, 5].Value = resultList[row].mark1AxisPosY;
                    worksheet.Cells[row + 1, 6].Value = resultList[row].mark2AxisPosX;
                    worksheet.Cells[row + 1, 7].Value = resultList[row].mark2AxisPosY;
                    worksheet.Cells[row + 1, 8].Value = resultList[row].matchResultTopMark1X;
                    worksheet.Cells[row + 1, 9].Value = resultList[row].matchResultTopMark1Y;
                    worksheet.Cells[row + 1, 10].Value = resultList[row].matchResultBottomMark1X;
                    worksheet.Cells[row + 1, 11].Value = resultList[row].matchResultBottomMark1X;
                    worksheet.Cells[row + 1, 12].Value = resultList[row].matchResultTopMark2X;
                    worksheet.Cells[row + 1, 13].Value = resultList[row].matchResultTopMark2Y;
                    worksheet.Cells[row + 1, 14].Value = resultList[row].matchResultBottomMark2X;
                    worksheet.Cells[row + 1, 15].Value = resultList[row].matchResultBottomMark2Y;
                    worksheet.Cells[row + 1, 16].Value = resultList[row].axisT;

                }

                // 保存Excel文件
                package.Save();
            }
        }
        /// <summary>
        /// 模板定位
        /// </summary>
        /// <param name="patternName">模板名称</param>
        /// <returns>定位结果</returns>
        public MatchResult LocatePosition(string patternName)
        {
            // 获取Pr实体
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

            Thread.Sleep(800);
            ExcuteResult excuteResult = pREntity.DoWork();

            // 拍照失败，直接返回错误
            if (excuteResult != ExcuteResult.Success)
            {
                throw new ArgumentNullException(patternName, "The" + patternName + " excuteResult is fail.");
            }

            // 获取定位结果
            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return matchResult;
        }

        private void simpleButton14_Click(object sender, EventArgs e)
        {
            AutoFocusing autofocus = new AutoFocusing();
            Task.Run(
                () => { autofocus.AutoFocus(this.BondModule.BondCamera, this.BondModule.BondHead.AxisZ, -6.9, -11.9); });
        }

        private void simpleButton15_Click(object sender, EventArgs e)
        {
            try
            {
                List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();

                List<MatchResult> resultA1List = new List<MatchResult>();

                List<double> thetaList = new List<double>();


                int count = int.Parse(this.textEdit1.Text);
                double timeA1 = 0, timeA2 = 0, moveTime = 0;

                foreach (AKRSPoint3D pointa in this.pointAList)
                {
                    pointA1List.Clear();
                    resultA1List.Clear();
                    thetaList.Clear();

                    for (int i = 0; i < count; i++)
                    {
                        this.BondModule.BondHead.AxisZ.AbsoluteMove(pointa.Z);

                        MotionService.MoveAxesToTargetPosition(
                            (this.BondModule.BondAxisX, true, pointa.X, AccuracyMode.HighAccuracy),
                            (this.BondModule.BondAxisY, true, pointa.Y, AccuracyMode.HighAccuracy));

                        Thread.Sleep(1000);

                        for (int j = 0; j < 1; j++)
                        {
                            AKRSPoint3D a1 = this.bondModuleController.Get3DRealPosition();
                            double theta = this.BondModule.BondHead.AxisT.GetRealPosition();
                            MatchResult resultA1 = this.LocatePosition("大标定片模板", out timeA1);
                            resultA1List.Add(resultA1);
                            pointA1List.Add(a1);
                            thetaList.Add(theta);
                        }

                        this.BondModule.BondHead.AxisZ.AbsoluteMove(pointB.Z);

                        MotionService.MoveAxesToTargetPosition(
                            (this.BondModule.BondAxisX, true, pointB.X, AccuracyMode.HighAccuracy),
                            (this.BondModule.BondAxisY, true, pointB.Y, AccuracyMode.HighAccuracy));

                        //this.bondModuleController.MoveSafeBondXYZ(this.pointB.X, this.pointB.Y, this.pointB.Z, true);
                        // Thread.Sleep(100);
                    }
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ab.xlsx"));
                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "A1x";
                worksheet.Cells[1, 3].Value = "A1y";
                worksheet.Cells[1, 4].Value = "Theta";

                worksheet.Cells[1, 5].Value = "a1x";
                worksheet.Cells[1, 6].Value = "a1y";
                worksheet.Cells[1, 7].Value = "Angle";


                // 写入数据
                for (int row = 0; row < pointA1List.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = pointA1List[row].X;
                    worksheet.Cells[row + 2, 3].Value = pointA1List[row].Y;
                    worksheet.Cells[row + 2, 4].Value = thetaList[row];

                    worksheet.Cells[row + 2, 5].Value = resultA1List[row].CenterX;
                    worksheet.Cells[row + 2, 6].Value = resultA1List[row].CenterY;
                    worksheet.Cells[row + 2, 7].Value = resultA1List[row].Angle;
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
        }

        private void btnABAuto_Click(object sender, EventArgs e)
        {
            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().GlassVisionMachinePos);

            // 定位小圆并移动至相机中心
            MatchResult smallMatchResult = this.LocatePosition("小标定片模板", out double time);

            CalibService.MoveToCamCenterCoo(BondModule.BondAxisX, BondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().GlassVisionMachinePos + CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset);

            // Z轴下降
            BondModule.BondHead.AxisZ.AbsoluteMove(CalibrateRunPara.GetInstance().GlassPickZMachinePos);

            // 取标
            BondModule.BondHead.VaccumElectric.SetOutputValue(true);

            // 移动到上视相机中心位置
            bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

            List<AKRSPoint3D> pointA1List = new List<AKRSPoint3D>();

            List<MatchResult> resultA1List = new List<MatchResult>();

            List<double> thetaList = new List<double>();

            int count = int.Parse(this.textEdit1.Text);
            double timeA1 = 0, timeA2 = 0, moveTime = 0;

            pointA1List.Clear();
            resultA1List.Clear();
            thetaList.Clear();

            for (int i = 0; i < count; i++)
            {
                // 移动到上视相机中心位置
                bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                Thread.Sleep(1000);

                AKRSPoint3D a1 = this.bondModuleController.Get3DRealPosition();
                double theta = this.BondModule.BondHead.AxisT.GetRealPosition();
                MatchResult resultA1 = this.LocatePosition("上视标定模板", out timeA1);
                resultA1List.Add(resultA1);
                pointA1List.Add(a1);
                thetaList.Add(theta);

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                Thread.Sleep(100);
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ab.xlsx"));
            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "A1x";
            worksheet.Cells[1, 3].Value = "A1y";
            worksheet.Cells[1, 4].Value = "Theta";

            worksheet.Cells[1, 5].Value = "a1x";
            worksheet.Cells[1, 6].Value = "a1y";
            worksheet.Cells[1, 7].Value = "Angle";


            // 写入数据
            for (int row = 0; row < pointA1List.Count; row++)
            {
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = pointA1List[row].X;
                worksheet.Cells[row + 2, 3].Value = pointA1List[row].Y;
                worksheet.Cells[row + 2, 4].Value = thetaList[row];

                worksheet.Cells[row + 2, 5].Value = resultA1List[row].CenterX;
                worksheet.Cells[row + 2, 6].Value = resultA1List[row].CenterY;
                worksheet.Cells[row + 2, 7].Value = resultA1List[row].Angle;
            }

            // 保存Excel文件
            package.Save();

            AKRSPoint3D targetPos3 = new AKRSPoint3D()
            {
                X = this.calibratePara.GlassVisionMachinePos.X
                                                 + this.calibratePara.BondRotateCenterToCamOffset.X,
                Y = this.calibratePara.GlassVisionMachinePos.Y
                                                 + this.calibratePara.BondRotateCenterToCamOffset.Y,
                Z = 0
            };

            // 轴移动至放标位放置标定片
            this.bondModuleController.MoveSafeBondXYZ(targetPos3);

            // BondModule.BondHead.VaccumElectric.SetOutputValue(true);
            //// BondModule.BondHead.AxisZ.AbsoluteMove(0);

            this.BondModule.BondHead.AxisT.AbsoluteMove(0);

            this.BondModule.BondHead.AxisZ.AbsoluteMove(this.calibratePara.GlassPickZMachinePos);

            // 放标
            this.BondModule.BondHead.VaccumElectric.SetOutputValue(false);

            Thread.Sleep(500);

            this.BondModule.BondHead.AxisZ.AbsoluteMove(0);
        }

        private void simpleButton16_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    List<AKRSPoint2D> nCenterList = new List<AKRSPoint2D>();
                    List<AKRSPoint2D> pCenterList = new List<AKRSPoint2D>();

                    List<AKRSPoint2D> nPointList = new List<AKRSPoint2D>();
                    List<AKRSPoint2D> pPointList = new List<AKRSPoint2D>();
                    for (int j = 0; j < 2; j++)
                    {


                        for (double i = -120; i <= 0; i += 40)
                        {
                            this.BondModule.BondHead.AxisT.AbsoluteMove(i);
                            Thread.Sleep(1000);
                            MatchResult resultA = this.LocatePosition("角度测试", out double time);

                            nPointList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                        }
                        //CalibService.FitCircle(nPointList, out double nCenterX, out double nCenterY, out double nRadius);

                        for (double i = 120; i <= 0; i -= 40)
                        {
                            this.BondModule.BondHead.AxisT.AbsoluteMove(i);
                            Thread.Sleep(1000);
                            MatchResult resultA = this.LocatePosition("角度测试", out double time);

                            pPointList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                        }
                        //CalibService.FitCircle(nPointList, out double pCenterX, out double pCenterY, out double pRadius);

                        //nCenterList.Add(new AKRSPoint2D(res, nCenterY));
                        //pCenterList.Add(new AKRSPoint2D(pCenterX, pCenterY));
                    }


                    ExcelPackage package = new ExcelPackage(new FileInfo(@"C:\Angle.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "nCenterX";
                    worksheet.Cells[1, 3].Value = "nCenterY";

                    worksheet.Cells[1, 4].Value = "pCenterX";
                    worksheet.Cells[1, 5].Value = "pCenterY";


                    // 写入数据
                    for (int row = 0; row < nCenterList.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = (row + 1).ToString();
                        worksheet.Cells[row + 2, 2].Value = nPointList[row].X;
                        worksheet.Cells[row + 2, 3].Value = nPointList[row].Y;
                        worksheet.Cells[row + 2, 4].Value = pPointList[row].X;
                        worksheet.Cells[row + 2, 5].Value = pPointList[row].Y;
                    }

                    // 保存Excel文件
                    package.Save();

                    AKRSXtraMessageBox.Show("ok");
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                    return;
                }
            });

        }


        private void simpleButton17_Click(object sender, EventArgs e)
        {
            int time = 1000;
            int[] a = new int[] { 0 }; // 转矩值

            double downPos = 0;
            double upPos = 0;

            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < a.Length; i++)
                {
                    short iret = LTDMC.nmc_torque_move(0, 0, a[j], 0, 100, 0);

                    this.BondModule.BondHead.AxisZ.AbsoluteMove(downPos);
                    Thread.Sleep(time);

                    this.BondModule.BondHead.AxisZ.AbsoluteMove(upPos);
                    Thread.Sleep(time);
                }
            }
        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            TestAlg.GetInstance().OnlyOnePointDispense(10000, 500);
        }

        /// <summary>
        /// 测试光源控制器响应时间
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnTestLightTime_Click(object sender, EventArgs e)
        {
            List<double> mutiList = new List<double>();
            List<double> oneList = new List<double>();

            int redLightIndentity = Convert.ToInt32(this.SpRedLight.EditValue);
            int greenLightIndentity = Convert.ToInt32(this.SpGreenLight.EditValue);
            int blueLightIndentity = Convert.ToInt32(this.SpBlueLight.EditValue);

            Light[] lights = { this.BondLightGreen, this.BondLightRed, this.BondLightBlue };
            int[] indentities = { redLightIndentity, greenLightIndentity, blueLightIndentity };

            for (int i = 0; i < 100; i++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                LightController.SetIntensities(lights.ToList(), indentities.ToList());
                sw.Stop();
                mutiList.Add(sw.ElapsedMilliseconds);

                this.BondLightRed.SetIntensity(0);
                this.BondLightGreen.SetIntensity(0);
                this.BondLightBlue.SetIntensity(0);

                sw.Restart();
                this.BondLightRed.SetIntensity(redLightIndentity);
                this.BondLightGreen.SetIntensity(greenLightIndentity);
                this.BondLightBlue.SetIntensity(blueLightIndentity);
                sw.Stop();

                oneList.Add(sw.ElapsedMilliseconds);
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\LigthControlTime" + DateTime.Now.ToString("HHmmss") + ".xlsx"));

            // 添加一个工作表
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "多通道时间";
            worksheet.Cells[1, 3].Value = "单通道时间";

            // 写入数据
            for (int row = 0; row < mutiList.Count; row++)
            {
                worksheet.Cells[row + 2, 1].Value = (row + 1).ToString();
                worksheet.Cells[row + 2, 2].Value = mutiList[row];
                worksheet.Cells[row + 2, 3].Value = oneList[row];
            }

            // 保存Excel文件
            package.Save();

            AKRSXtraMessageBox.Show("ok");
        }

        private void simpleButton19_Click(object sender, EventArgs e)
        {
            FrmLightnessCalibTeach frmLightnessCalibTeach = new FrmLightnessCalibTeach();
            frmLightnessCalibTeach.Show();
        }

        private void simpleButton20_Click(object sender, EventArgs e)
        {
            this.ShowPanel(
                "UcTestThreePointAlign",
                () =>
                    {
                        UcTestThreePointAlign ucTestThreePointAlign = new UcTestThreePointAlign() { Dock = DockStyle.Fill };
                        return ucTestThreePointAlign;
                    });
        }

        /// <summary>
        /// 显示DockPanel
        /// </summary>
        /// <param name="controlName">组件名称</param>
        /// <param name="createUserControlAction">DockPanel 里包含的组件</param>
        public void ShowPanel(string controlName, Func<XtraUserControl> createUserControlAction)
        {
            DockPanel dockPanel = this.GetDockPanel(controlName);

            if (dockPanel != null)
            {
                Form form = dockPanel.ParentForm;

                if (form != null)
                {
                    form.TopMost = true;
                    form.Show();
                    form.WindowState = FormWindowState.Normal;
                    form.TopMost = false;
                }

                dockPanel.BringToFront();
            }
            else
            {
                XtraUserControl userControl = createUserControlAction();
                Size size = userControl.Size;
                dockPanel = this.dockManager1.AddPanel(userControl, new Point(220, 70), userControl.Text);
                dockPanel.Text = userControl.Text;
                dockPanel.FloatSize = size + new Size(20, 20);
            }
        }

        /// <summary>
        /// 存不存在此DockPanel
        /// </summary>
        /// <param name="controlName">组件名</param>
        /// <returns>是否存在</returns>
        private DockPanel GetDockPanel(string controlName)
        {
            DockPanel dockPanel = null;

            foreach (DockPanel dockManagerPanel in this.dockManager1.Panels)
            {
                Control[] cs = dockManagerPanel.Controls.Find(controlName, true);

                if (cs.Any())
                {
                    dockPanel = dockManagerPanel;
                    break;
                }
            }

            return dockPanel;
        }

        private void btnTestPin_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    List<double> realAngleList = new List<double>();
                    List<double> imageAngleList = new List<double>();
                    List<double> codeAngleList = new List<double>();

                    List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();


                    for (int j = 0; j < 3; j++)
                    {
                        for (double i = 0; i < 100; i += 1)
                        {
                            WaferSubController.GetInstance().EjectController.ChangeEjection(1, true);
                            Thread.Sleep(50);
                            MatchResult resultA = this.LocatePosition("测试", out double time);

                            resultList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                            realAngleList.Add(i);
                            imageAngleList.Add(resultA.Angle);

                        }
                    }

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\顶针.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "realAngle";
                    worksheet.Cells[1, 3].Value = "imageAngle";
                    worksheet.Cells[1, 4].Value = "centerX";
                    worksheet.Cells[1, 5].Value = "centerY";

                    // 写入数据
                    for (int row = 0; row < realAngleList.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = (row + 1).ToString();
                        worksheet.Cells[row + 2, 2].Value = realAngleList[row];
                        worksheet.Cells[row + 2, 3].Value = imageAngleList[row];
                        worksheet.Cells[row + 2, 4].Value = resultList[row].X;
                        worksheet.Cells[row + 2, 5].Value = resultList[row].Y;
                    }

                    // 保存Excel文件
                    package.Save();

                    AKRSXtraMessageBox.Show("ok");
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                    return;
                }
            });
        }

        private void btnTestChangeBond_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    List<double> realAngleList = new List<double>();
                    List<double> imageAngleList = new List<double>();
                    List<double> codeAngleList = new List<double>();

                    List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();


                    for (int j = 0; j < 3; j++)
                    {
                        for (double i = 0; i < 100; i += 1)
                        {
                            this.system2Controller.ChangeNozzle("");

                            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                            Thread.Sleep(50);
                            MatchResult resultA = this.LocatePosition("测试", out double time);

                            resultList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                            realAngleList.Add(i);
                            imageAngleList.Add(resultA.Angle);

                        }
                    }

                    ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\更换焊头重复性.xlsx"));
                    // 添加一个工作表
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                    worksheet.Cells[1, 1].Value = "次数";
                    worksheet.Cells[1, 2].Value = "realAngle";
                    worksheet.Cells[1, 3].Value = "imageAngle";
                    worksheet.Cells[1, 4].Value = "centerX";
                    worksheet.Cells[1, 5].Value = "centerY";

                    // 写入数据
                    for (int row = 0; row < realAngleList.Count; row++)
                    {
                        worksheet.Cells[row + 2, 1].Value = (row + 1).ToString();
                        worksheet.Cells[row + 2, 2].Value = realAngleList[row];
                        worksheet.Cells[row + 2, 3].Value = imageAngleList[row];
                        worksheet.Cells[row + 2, 4].Value = resultList[row].X;
                        worksheet.Cells[row + 2, 5].Value = resultList[row].Y;
                    }

                    // 保存Excel文件
                    package.Save();

                    AKRSXtraMessageBox.Show("ok");
                }
                catch (Exception ex)
                {
                    LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                    return;
                }
            });
        }

        private void FrmTestAccuary_Load(object sender, EventArgs e)
        {

        }
    }

    public class Result
    {
        public int Index { get; set; }

        public double Speed { get; set; }

        public double AccSpeed { get; set; }

        public double Jerk { get; set; }

        public int SleepTime { get; set; }

        public double BmcCenterX { get; set; }

        public double BmcCenterY { get; set; }

        public double UpLookCenterX { get; set; }

        public double UpLookCenterY { get; set; }

        public double TempCenterX { get; set; }

        public double TempCenterY { get; set; }

        public double BmcCenterMachinePosX { get; set; }

        public double BmcCenterMachinePosY { get; set; }

        public double UpLookCenterMachinePosX { get; set; }

        public double UpLookCenterMachinePosY { get; set; }

        public double TempCenterMachinePosX { get; set; }

        public double TempCenterMachinePosY { get; set; }

        public long Time { get; set; }
    }


}