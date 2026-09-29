namespace AKRS.ZX2200.CalibSystem.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DevExpress.XtraEditors;

    using log4net.Core;

    using Newtonsoft.Json;

    using OfficeOpenXml;

    /// <summary>
    /// 项目实验类
    /// </summary>
    public class TestAlg : Singleton<TestAlg>
    {
        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// bond 模块控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// BondModule
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// DispenseModule
        /// </summary>
        private DispenseModule dispenseModule = new DispenseModule();

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara calibratePara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara BMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// Bond相机单点重复性定位路径
        /// </summary>
        private string onlyOnePointBondPath => "D:\\static experiment\\Bond" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";

        /// <summary>
        /// Dispense相机单点重复性定位路径
        /// </summary>
        private string onlyOnePointDispensePath => "D:\\static experiment\\Dispense" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";

        /// <summary>
        /// UpLook相机单点重复性定位路径
        /// </summary>
        private string onlyOnePointUpLookPath => "D:\\static experiment\\Uplook" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";

        /// <summary>
        /// 回零偏移路径
        /// </summary>
        private string backZeroTestPath => "D:\\backZero experiment\\Bond" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";

        /// <summary>
        /// 回零偏移路径点胶
        /// </summary>
        private string backZeroTestDispensePath => "D:\\backZero experiment\\Dispense" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";


        /// <summary>
        /// UpLook相机往复定位实验路径
        /// </summary>
        private string goBackTestUpLookPath = Path.Combine(PathConfig.DeviceDirPath, "GobackExperimentUpLook");


        /// <summary>
        /// Bond开始位置
        /// </summary>
        private AKRSPoint3D bondStartPos = new AKRSPoint3D();

        public TestAlg()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// Bond重复定位测试
        /// </summary>
        /// <param name="cycles">次数</param>
        /// <param name="interval">间隔</param>
        public void OnlyOnePointBond(int cycles, int interval)
        {
            try
            {
                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();
                List<double> resultAList = new List<double>();
                List<string> timeList = new List<string>();

                // 获取Pr实体
                // PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.BmcPRName);
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("大标定片模板");

                // this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);
                //this.bondModuleController.MoveToG0Pos(
                //    BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos);
                //this.bondHeadController.MoveAxisZ(-5.25);

                while (true)
                {
                    Thread.Sleep(interval);
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        // throw new ArgumentNullException("PointA", "The PointA excuteResult is fail.");
                        goto SaveFileDialog;
                    }

                    // 获取定位结果
                    MatchResult resultA = (MatchResult)pREntity.AlgResult;

                    timeList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    resultXList.Add(resultA.CenterX);
                    resultYList.Add(resultA.CenterY);
                    resultAList.Add(resultA.Angle);

                SaveFileDialog:
                    if (resultAList.Count > cycles)
                    {
                        ExcelPackage package = new ExcelPackage(new FileInfo(this.onlyOnePointBondPath));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "时间";
                        worksheet.Cells[1, 2].Value = "次数";
                        worksheet.Cells[1, 3].Value = "a1x";
                        worksheet.Cells[1, 4].Value = "a1y";
                        worksheet.Cells[1, 5].Value = "a1Angle";

                        // 写入数据
                        for (int row = 0; row < resultAList.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = timeList[row];
                            worksheet.Cells[row + 2, 2].Value = (row + 1);
                            worksheet.Cells[row + 2, 3].Value = resultXList[row];
                            worksheet.Cells[row + 2, 4].Value = resultYList[row];
                            worksheet.Cells[row + 2, 5].Value = resultAList[row];
                        }

                        // 保存Excel文件
                        package.Save();
                        break;
                    }
                }


                DialogResult dialog = AKRSXtraMessageBox.Show(
                    "the experiment is over \n"
                    + "document location: \n" + this.onlyOnePointUpLookPath,
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }

        /// <summary>
        /// 上视重复定位测试
        /// </summary>
        /// <param name="cycles">次数</param>
        /// <param name="interval">间隔</param>
        public void OnlyOnePointUpLook(int cycles, int interval, bool isFlash)
        {
            try
            {
                //this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                //// 定位小圆并移动至相机中心
                //MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                //CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

                //this.bondModuleController.MoveSafeBondXYZ(this.bondModuleController.Get3DRealPosition() + this.calibratePara.BondRotateCenterToCamOffset);

                //// Z轴下降
                //this.bondModule.BondHead.AxisZ.AbsoluteMove(this.calibratePara.GlassPickZMachinePos);

                //// 取标
                //this.bondModule.BondHead.VaccumElectric.SetOutputValue(true);

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();
                List<double> resultAList = new List<double>();
                List<string> timeList = new List<string>();

                // 获取Pr实体
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.UpLookPRName);
                pREntity.IsFlash = isFlash;

                while (true)
                {
                    Thread.Sleep(interval);
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        // throw new ArgumentNullException("PointA", "The PointA excuteResult is fail.");
                        goto SaveFileDialog;
                    }

                    // 获取定位结果
                    MatchResult resultA = (MatchResult)pREntity.AlgResult;

                    timeList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    resultXList.Add(resultA.CenterX);
                    resultYList.Add(resultA.CenterY);
                    resultAList.Add(resultA.Angle);

                SaveFileDialog:
                    if (resultAList.Count > cycles)
                    {
                        ExcelPackage package = new ExcelPackage(new FileInfo(this.onlyOnePointUpLookPath));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "时间";
                        worksheet.Cells[1, 2].Value = "次数";
                        worksheet.Cells[1, 3].Value = "a1x";
                        worksheet.Cells[1, 4].Value = "a1y";
                        worksheet.Cells[1, 5].Value = "a1Angle";

                        // 写入数据
                        for (int row = 0; row < resultAList.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = timeList[row];
                            worksheet.Cells[row + 2, 2].Value = (row + 1);
                            worksheet.Cells[row + 2, 3].Value = resultXList[row];
                            worksheet.Cells[row + 2, 4].Value = resultYList[row];
                            worksheet.Cells[row + 2, 5].Value = resultAList[row];
                        }

                        // 保存Excel文件
                        package.Save();
                        break;
                    }
                }

                //DialogResult dialog = XtraMessageBox.Show(
                //    "the experiment is over \n"
                //    + "document location: \n" + this.onlyOnePointUpLookPath,
                //    "Attention",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }
        }

        /// <summary>
        /// Dispense重复定位测试
        /// </summary>
        /// <param name="cycles">次数</param>
        /// <param name="interval">间隔</param>
        public void OnlyOnePointDispense(int cycles, int interval)
        {
            try
            {
                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();
                List<double> resultAList = new List<double>();
                List<string> timeList = new List<string>();

                // 获取Pr实体
                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.calibratePara.DispensePRName);

                this.calibController.MoveDispenseToMachinePos(this.calibratePara.DispenseMarkVisionMachinePos);

                while (true)
                {
                    Thread.Sleep(interval);
                    ExcuteResult excuteResult = pREntity.DoWork();

                    // 拍照失败，直接返回错误
                    if (excuteResult != ExcuteResult.Success)
                    {
                        // throw new ArgumentNullException("PointA", "The PointA excuteResult is fail.");
                        goto SaveFileDialog;
                    }

                    // 获取定位结果

                    MatchResult resultA = (MatchResult)pREntity.AlgResult;

                    timeList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    resultXList.Add(resultA.CenterX);
                    resultYList.Add(resultA.CenterY);
                    resultAList.Add(resultA.Angle);

                SaveFileDialog:
                    if (resultAList.Count > cycles)
                    {
                        ExcelPackage package = new ExcelPackage(new FileInfo(this.onlyOnePointDispensePath));

                        // 添加一个工作表
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                        worksheet.Cells[1, 1].Value = "时间";
                        worksheet.Cells[1, 2].Value = "次数";
                        worksheet.Cells[1, 3].Value = "a1x";
                        worksheet.Cells[1, 4].Value = "a1y";
                        worksheet.Cells[1, 5].Value = "a1Angle";

                        // 写入数据
                        for (int row = 0; row < resultAList.Count; row++)
                        {
                            worksheet.Cells[row + 2, 1].Value = timeList[row];
                            worksheet.Cells[row + 2, 2].Value = (row + 1);
                            worksheet.Cells[row + 2, 3].Value = resultXList[row];
                            worksheet.Cells[row + 2, 4].Value = resultYList[row];
                            worksheet.Cells[row + 2, 5].Value = resultAList[row];
                        }

                        // 保存Excel文件
                        package.Save();
                        break;
                    }
                }


                DialogResult dialog = AKRSXtraMessageBox.Show(
                    "the experiment is over \n"
                    + "document location: \n" + this.onlyOnePointDispensePath,
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }

        /// <summary>
        /// 回零偏移测试
        /// </summary>
        /// <param name="cycles">次数</param>
        public void TestBackOffset(int cycles)
        {
            try
            {
                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();
                List<string> timeList = new List<string>();

                for (int i = 0; i < cycles; i++)
                {
                    //ExcuteResult excuteResult2 = this.bondModule.BondHead.AxisZ.GoHome();
                    //if (excuteResult2 != ExcuteResult.Success)
                    //{
                    //    int num = (int)XtraMessageBox.Show(
                    //        "轴" + this.bondModule.BondHead.AxisZ.HardwareName + " 回零失败",
                    //        "错误",
                    //        MessageBoxButtons.OK,
                    //        MessageBoxIcon.Hand);
                    //}

                    //ExcuteResult excuteResult = this.bondModule.BondAxisX.GoHome();
                    //if (excuteResult != ExcuteResult.Success)
                    //{
                    //    int num = (int)XtraMessageBox.Show(
                    //        "轴" + this.bondModule.BondAxisX.HardwareName + " 回零失败",
                    //        "错误",
                    //        MessageBoxButtons.OK,
                    //        MessageBoxIcon.Hand);
                    //}

                    ExcuteResult excuteResult1 = this.bondModule.BondAxisY.GoHome();
                    if (excuteResult1 != ExcuteResult.Success)
                    {
                        int num = (int)AKRSXtraMessageBox.Show(
                            "轴" + this.bondModule.BondAxisY.HardwareName + " 回零失败",
                            "错误",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Hand);
                    }
                    MatchResult matchResult = this.LocatePosition("GlobalCalibrationRing");

                    this.bondModuleController.MoveBondXY(0, this.calibratePara.BMCVisionMachinePos.Y);
                    //this.bondModuleController.MoveBondXY(this.calibratePara.BMCVisionMachinePos.X, 0);


                    //MatchResult matchResult = this.LocatePosition("GlobalCalibrationRing");

                    timeList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    resultXList.Add(matchResult.CenterX);
                    resultYList.Add(matchResult.CenterY);

                    // Bmc 中心
                    //this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);
                    //this.bondModuleController.MoveBondXY(this.calibratePara.BMCVisionMachinePos.X, this.calibratePara.BMCVisionMachinePos.Y);


                }
                ExcelPackage package = new ExcelPackage(new FileInfo(this.backZeroTestPath));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "时间";
                worksheet.Cells[1, 2].Value = "次数";
                worksheet.Cells[1, 3].Value = "a1x";
                worksheet.Cells[1, 4].Value = "a1y";

                // 写入数据
                for (int row = 0; row < resultXList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = timeList[row];
                    worksheet.Cells[row + 2, 2].Value = (row + 1);
                    worksheet.Cells[row + 2, 3].Value = resultXList[row];
                    worksheet.Cells[row + 2, 4].Value = resultYList[row];
                }

                // 保存Excel文件
                package.Save();

            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(
                    ex.Message,
                    "异常",
                    new string[] { "异常" },
                    new DialogResult[] { DialogResult.Yes });

                return;
            }
        }

        /// <summary>
        /// 点胶回零偏移测试
        /// </summary>
        /// <param name="cycles">次数</param>
        public void TestBackOffsetDispense(int cycles)
        {
            //try
            //{
            //    List<double> resultXList = new List<double>();
            //    List<double> resultYList = new List<double>();
            //    List<string> timeList = new List<string>();

            //    for (int i = 0; i < cycles; i++)
            //    {
            //        this.dispenseModule.DispenseAxisZ.GoHome();

            //        ExcuteResult excuteResult = this.bondModule.BondAxisX.GoHome();
            //        if (excuteResult != ExcuteResult.Success)
            //        {
            //            int num = (int)AKRSXtraMessageBox.Show("轴" + this.dispenseModule.DispenseAxisX.HardwareName + " 回零失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        }


            //        ExcuteResult excuteResult1 = this.bondModule.BondAxisY.GoHome();
            //        if (excuteResult1 != ExcuteResult.Success)
            //        {
            //            int num = (int)AKRSXtraMessageBox.Show("轴" + this.dispenseModule.DispenseAxisY.HardwareName + " 回零失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            //        }

            //        MatchResult matchResult = this.LocatePosition(this.calibratePara.BmcPRName);

            //        timeList.Add(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //        resultXList.Add(matchResult.CenterX);
            //        resultYList.Add(matchResult.CenterY);
            //    }

            //    ExcelPackage package = new ExcelPackage(new FileInfo(this.backZeroTestDispensePath));

            //    // 添加一个工作表
            //    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            //    worksheet.Cells[1, 1].Value = "时间";
            //    worksheet.Cells[1, 2].Value = "次数";
            //    worksheet.Cells[1, 3].Value = "a1x";
            //    worksheet.Cells[1, 4].Value = "a1y";

            //    // 写入数据
            //    for (int row = 0; row < resultXList.Count; row++)
            //    {
            //        worksheet.Cells[row + 2, 1].Value = timeList[row];
            //        worksheet.Cells[row + 2, 2].Value = (row + 1);
            //        worksheet.Cells[row + 2, 3].Value = resultXList[row];
            //        worksheet.Cells[row + 2, 4].Value = resultYList[row];
            //    }

            //    // 保存Excel文件
            //    package.Save();
            //}
            //catch (Exception ex)
            //{
            //    LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
            //    AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

            //    return;
            //}
        }

        /// <summary>
        /// 像素比精度实验(UpLook)
        /// </summary>
        public void TestPixelRatioAccuracyUpLook()
        {
            try
            {
                List<double> resultXBeforeList = new List<double>();
                List<double> resultYBeforeList = new List<double>();

                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();

                // 先取标定片 
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

                AKRSPoint3D point3D = this.bondModuleController.Get3DRealPosition() + this.calibratePara.BondRotateCenterToCamOffset;
                point3D.Z = 0;
                this.bondModuleController.MoveSafeBondXYZ(point3D);

                #region 取放标定片参数

                // 取放片参数
                BaseCarrierConfig component = new BaseCarrierConfig()
                {
                    // 取片
                    IsActivateSlowTravelBeforePickup = true,
                    SlowTravelSpeedBeforePickup =
                        this.BMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                        this.BMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                        this.BMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                        this.BMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.BMCDevicePara.VacuumOffDelay,
                    PickupDelay = this.BMCDevicePara.PickupDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,


                    // 放片
                    IsActivateSlowTravelBeforeBonding = true,
                    SlowTravelSpeedBeforeBonding =
                        this.BMCDevicePara.SlowTravelSpeedBeforeBonding,
                    SlowTravelDistanceBeforeBonding =
                        this.BMCDevicePara.SlowTravelDistanceBeforeBonding,
                    IsActivateSlowTravelAfterBonding = true,
                    SlowTravelSpeedAfterBonding =
                        this.BMCDevicePara.SlowTravelSpeedAfterBonding,
                    SlowTravelDistanceAfterBonding =
                        this.BMCDevicePara.SlowTravelDistanceAfterBonding,
                    BondingBlowDelay = this.BMCDevicePara.BondingBlowDelay,
                    PlacementDelay = this.BMCDevicePara.PlacementDelay,
                    IsActiveComponentDetection = true,
                    BondingForceMode = ForceModeEnum.Distance,
                };
                double pickLevel = this.calibratePara.GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                #endregion
                // 取标
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                this.bondHeadController.MoveBondZToSafePos();

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();
                pointACalibList = CalibService.GeneratePointList(
                    this.calibratePara.GlassUpLookVisionMachinePos,
                    0.5,
                    0.5,
                    3,
                    3);

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < pointACalibList.Count; j++)
                    {
                        this.bondModuleController.MoveSafeBondXY(pointACalibList[j].X, pointACalibList[j].Y);

                        Thread.Sleep(100);

                        MatchResult resultA = this.LocatePosition(this.calibratePara.UpLookPRName);

                        CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, resultA, "UpLookCameraCoordinateSystem");

                        Thread.Sleep(100);

                        MatchResult resultB = this.LocatePosition(this.calibratePara.UpLookPRName);

                        resultXList.Add(resultB.CenterX);
                        resultYList.Add(resultB.CenterY);
                    }
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\PixelRatioAccuracy\UpLookCalibTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";

                worksheet.Cells[1, 2].Value = "坐标X";
                worksheet.Cells[1, 3].Value = "坐标Y";

                for (int row = 0; row < resultXList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = (row + 1);
                    worksheet.Cells[row + 2, 2].Value = resultXList[row];
                    worksheet.Cells[row + 2, 3].Value = resultYList[row];
                }

                package.Save();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }

        }

        /// <summary>
        /// 像素比精度实验(Bond)
        /// </summary>
        public void TestPixelRatioAccuracyBond()
        {
            try
            {
                List<double> resultXBeforeList = new List<double>();
                List<double> resultYBeforeList = new List<double>();

                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();

                // Bmc 中心
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.BmcPRName);

                CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

                this.calibratePara.BMCVisionMachinePos = this.bondModuleController.Get3DRealPosition();

                List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();
                pointACalibList = CalibService.GeneratePointList(
                    this.calibratePara.GlassUpLookVisionMachinePos,
                    0.5,
                    0.5,
                    3,
                    3);

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < pointACalibList.Count; j++)
                    {
                        this.bondModuleController.MoveSafeBondXY(pointACalibList[j].X, pointACalibList[j].Y);

                        Thread.Sleep(100);

                        MatchResult resultA = this.LocatePosition(this.calibratePara.BmcPRName);

                        CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, resultA, "BondCameraCoordinateSystem");

                        Thread.Sleep(100);

                        MatchResult resultB = this.LocatePosition(this.calibratePara.BmcPRName);

                        resultXList.Add(resultB.CenterX);
                        resultYList.Add(resultB.CenterY);
                    }
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\PixelRatioAccuracy\BondCalibTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";

                worksheet.Cells[1, 2].Value = "坐标X";
                worksheet.Cells[1, 3].Value = "坐标Y";

                for (int row = 0; row < resultXList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = (row + 1);
                    worksheet.Cells[row + 2, 2].Value = resultXList[row];
                    worksheet.Cells[row + 2, 3].Value = resultYList[row];
                }

                package.Save();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }
        }

        /// <summary>
        /// 像素比精度实验(Wafer)
        /// </summary>
        public void TestPixelRatioAccuracyWafer()
        {
            try
            {
                List<double> resultXBeforeList = new List<double>();
                List<double> resultYBeforeList = new List<double>();

                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();

                // Bond移开到安全位置
                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BMCVisionMachinePos);

                // 晶圆台左Mark移动到晶圆相机中心
                this.calibController.MoveWaferTableToMachinePos(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos);

                this.calibController.MoveWaferCameraZ(this.calibratePara.WaferTableLeftMarkWCVisionMachinePos.Z);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.WaferPRName);

                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Wafer, smallMatchResult);

                this.calibratePara.WaferTableLeftMarkWCVisionMachinePos = this.calibController.GetWaferTableRealPos();

                // 生成晶圆台标定点位列表
                List<AKRSPoint2D> pointACalibList = CalibService.GeneratePointList(
                    this.calibratePara.WaferTableLeftMarkWCVisionMachinePos,
                    0.4,
                    1,
                    3);

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < pointACalibList.Count; j++)
                    {
                        this.calibController.MoveWaferTableToMachinePos(new AKRSPoint3D(pointACalibList[j].X, pointACalibList[j].Y, 0));

                        Thread.Sleep(500);

                        MatchResult resultA = this.LocatePosition(this.calibratePara.WaferPRName);

                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Wafer, resultA);

                        Thread.Sleep(500);

                        MatchResult resultB = this.LocatePosition(this.calibratePara.WaferPRName);

                        resultXList.Add(resultB.CenterX);
                        resultYList.Add(resultB.CenterY);
                    }
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\PixelRatioAccuracy\WaferCalibTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";

                worksheet.Cells[1, 2].Value = "坐标X";
                worksheet.Cells[1, 3].Value = "坐标Y";

                for (int row = 0; row < resultXList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = (row + 1);
                    worksheet.Cells[row + 2, 2].Value = resultXList[row];
                    worksheet.Cells[row + 2, 3].Value = resultYList[row];
                }

                package.Save();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }
        }

        /// <summary>
        /// 像素比精度实验(Dispense)
        /// </summary>
        public void TestPixelRatioAccuracyDispense()
        {
            try
            {
                List<double> resultXBeforeList = new List<double>();
                List<double> resultYBeforeList = new List<double>();

                List<double> resultXList = new List<double>();
                List<double> resultYList = new List<double>();

                // Bmc 中心
                this.dispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                // 点胶相机移动到点胶标定点中心上方
                this.calibController.MoveDispenseToMachinePos(this.calibratePara.DispenseMarkVisionMachinePos);

                List<AKRSPoint2D> pointACalibList = new List<AKRSPoint2D>();
                pointACalibList = CalibService.GeneratePointList(
                    this.calibratePara.GlassUpLookVisionMachinePos,
                    0.5,
                    0.5,
                    3,
                    3);

                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < pointACalibList.Count; j++)
                    {
                        // 轴移动到位
                        this.calibController.MoveDispenseToMachinePos(new AKRSPoint2D(pointACalibList[j].X, pointACalibList[j].Y));

                        Thread.Sleep(100);

                        MatchResult resultA = this.LocatePosition(this.calibratePara.DispensePRName);

                        this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.Dispense, resultA);

                        Thread.Sleep(100);

                        MatchResult resultB = this.LocatePosition(this.calibratePara.DispensePRName);

                        resultXList.Add(resultB.CenterX);
                        resultYList.Add(resultB.CenterY);
                    }
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\PixelRatioAccuracy\DispenseCalibTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));

                // 添加一个工作表
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";

                worksheet.Cells[1, 2].Value = "坐标X";
                worksheet.Cells[1, 3].Value = "坐标Y";

                for (int row = 0; row < resultXList.Count; row++)
                {
                    worksheet.Cells[row + 2, 1].Value = (row + 1);
                    worksheet.Cells[row + 2, 2].Value = resultXList[row];
                    worksheet.Cells[row + 2, 3].Value = resultYList[row];
                }

                package.Save();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
            }
        }

        /// <summary>
        /// 测试三点一线准确性
        /// </summary>
        public void TestBondToCamOffsetAccuracy()
        {
            this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BondCamUpLookCamMachinePos);

            DialogResult dialog4 = AKRSXtraMessageBox.Show(
                "System2: \n" + "Please put the glass Mark between the Bond camera and UpLook camera.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dialog4 == DialogResult.OK)
            {
                CalibrateTask.GetInstance().StartBondToCamBySameMarkTask();

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.BondCamUpLookCamMachinePos);
            }

            MatchResult bondCamMatchResult = this.LocatePosition("BondCamUpLookCamAlign-Bond");

            MatchResult upLookCamMatchResult = this.LocatePosition("BondCamUpLookCamAlign");

            AKRSPoint2D curMarkBondPos = CalibService.GetMachinePosByPixelPos(
                this.bondModuleController.Get2DRealPosition(),
                bondCamMatchResult,
                "BondCameraCoordinateSystem");

            AKRSPoint2D curMarkBondPos1 = CalibService.GetMachinePosByPixelPos(
                this.bondModuleController.Get2DRealPosition(),
                new MatchResult(1224, 1024, 0),
                "BondCameraCoordinateSystem");

            AKRSPoint2D curMarkUpLookPos = CalibService.GetMachinePosByPixelPos(
                this.bondModuleController.Get2DRealPosition(),
                upLookCamMatchResult,
                "UpLookCameraCoordinateSystem");

            AKRSPoint2D curMarkUpLookPos1 = CalibService.GetMachinePosByPixelPos(
                this.bondModuleController.Get2DRealPosition(),
                new MatchResult(1224, 1024, 0),
                "UpLookCameraCoordinateSystem");

            BondToCamResult result = new BondToCamResult();

            result.bondX = curMarkBondPos.X;
            result.bondY = curMarkBondPos.Y;
            result.upLookX = curMarkUpLookPos.X;
            result.upLookY = curMarkUpLookPos.Y;

            FrmBondToCamResult frmBondToCamResult = new FrmBondToCamResult(result);
            frmBondToCamResult.ShowDialog();
        }

        /// <summary>
        /// 回0偏移上视
        /// </summary>
        /// <param name="cycles">次数</param>
        /// <param name="sleepTime">间隔时间</param>
        /// <param name="speed">轴速</param>
        /// <param name="acceleration">加速度</param>
        /// <param name="Jerk">加加速度</param>
        public void GoBackPosTestUpLook(int cycles, int sleepTime, int speed, int acceleration, double Jerk)
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                this.bondStartPos = this.bondModuleController.Get3DRealPosition();

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition(this.calibratePara.GlassPRName);

                CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassVisionMachinePos + this.calibratePara.BondRotateCenterToCamOffset);

                // Z轴下降
                this.bondModule.BondHead.AxisZ.AbsoluteMove(this.calibratePara.GlassPickZMachinePos);

                // 取标
                this.bondModule.BondHead.VaccumElectric.SetOutputValue(true);

                this.bondModuleController.MoveSafeBondXYZ(this.calibratePara.GlassUpLookVisionMachinePos);

                for (int i = 0; i < cycles; i++)
                {
                    this.bondModule.BondHead.AxisZ.AbsoluteMove(BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z);

                    MovePara movePara = new MovePara();
                    movePara.Vel = speed;
                    movePara.Acc = acceleration;
                    movePara.Dec = acceleration;
                    movePara.Jerk = Jerk;
                    movePara.TargetPosition = this.calibratePara.GlassUpLookVisionMachinePos.X;
                    this.bondModule.BondAxisX.SendAbsoluteMoveCommand(movePara);

                    movePara = new MovePara();
                    movePara.Vel = speed;
                    movePara.Acc = acceleration;
                    movePara.Dec = acceleration;
                    movePara.Jerk = Jerk;
                    movePara.TargetPosition = this.calibratePara.GlassUpLookVisionMachinePos.Y;
                    this.bondModule.BondAxisY.SendAbsoluteMoveCommand(movePara);

                    this.bondModule.BondAxisX.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                    this.bondModule.BondAxisY.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                    this.bondModule.BondHead.AxisZ.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);

                    Thread.Sleep(sleepTime);
                    MatchResult resultA1 = this.LocatePosition(this.calibratePara.UpLookPRName);

                    this.bondModule.BondHead.AxisZ.AbsoluteMove(BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z);

                    movePara = new MovePara();
                    movePara.Vel = speed;
                    movePara.Acc = acceleration;
                    movePara.Dec = acceleration;
                    movePara.Jerk = Jerk;
                    movePara.TargetPosition = this.bondStartPos.X;
                    this.bondModule.BondAxisX.SendAbsoluteMoveCommand(movePara);

                    movePara = new MovePara();
                    movePara.Vel = speed;
                    movePara.Acc = acceleration;
                    movePara.Dec = acceleration;
                    movePara.Jerk = Jerk;
                    movePara.TargetPosition = this.bondStartPos.Y;
                    this.bondModule.BondAxisY.SendAbsoluteMoveCommand(movePara);

                    this.bondModule.BondAxisX.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                    this.bondModule.BondAxisY.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);
                    this.bondModule.BondHead.AxisZ.WaitForArrival(false, 0, AccuracyMode.HighAccuracy);

                    Thread.Sleep(sleepTime);

                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = speed;
                    result.AccSpeed = acceleration;
                    result.Jerk = Jerk;
                    result.SleepTime = sleepTime;
                    //result.CenterX = resultA1.CenterX;
                    //result.CenterY = resultA1.CenterY;
                    resultA1List.Add(result);
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(this.goBackTestUpLookPath + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "定位X";
                worksheet.Cells[1, 7].Value = "定位Y";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                }

                // 保存Excel文件
                package.Save();

                DialogResult dialog = AKRSXtraMessageBox.Show(
                    "the experiment is over \n"
                    + "document location: \n" + this.goBackTestUpLookPath,
                    "Attention",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
        public MatchResult LocatePosition(string patternName)
        {
            // 获取Pr实体
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(patternName);

            // Thread.Sleep(1000);
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
    }

    /// <summary>
    /// 三点一线结果
    /// </summary>
    public class BondToCamResult
    {
        /// <summary>
        /// BondX
        /// </summary>
        public double bondX { get; set; }

        /// <summary>
        /// BondY
        /// </summary>
        public double bondY { get; set; }

        /// <summary>
        /// 上视X
        /// </summary>
        public double upLookX { get; set; }

        /// <summary>
        /// 上视Y
        /// </summary>
        public double upLookY { get; set; }

        /// <summary>
        /// 极差X
        /// </summary>
        [JsonIgnore]
        public double DifferenceX
        {
            get
            {
                List<double> list = new List<double>();
                list.Add(this.bondX);
                list.Add(this.upLookX);

                return list.Max() - list.Min();
            }
        }

        /// <summary>
        /// 极差Y
        /// </summary>
        [JsonIgnore]
        public double DifferenceY
        {
            get
            {
                List<double> list = new List<double>();
                list.Add(this.bondY);
                list.Add(this.upLookY);

                return list.Max() - list.Min();
            }
        }
    }
}
