namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
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
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using Newtonsoft.Json;

    using OfficeOpenXml;

    /// <summary>
    /// 旋转中心精度测试
    /// </summary>
    public partial class FrmRotateCenterAccuracyTwo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// bond 模块控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// bond 模块控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara BMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// bond 模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 标定参数
        /// </summary>
        private CalibrateRunPara CalibratePara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 是否继续
        /// </summary>
        private bool isContinue = true;

        private string path => "D:\\rotateAndTemp experiment\\rotate" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx";

        private List<RotateCenterResult> rotateResultList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmRotateCenterAccuracyTwo()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 开始旋转中心测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtnStartRotateCenterTest_Click(object sender, EventArgs e)
        {
            this.isContinue = true;
            Task.Run(this.RotateCenterAccuracyTest);
        }

        /// <summary>
        /// 旋转中心测试
        /// </summary>
        public void RotateCenterAccuracyTest()
        {
            int intervalTimes = Convert.ToInt32(this.spIntervalTimes.Value);
            for (int j = 0; j < intervalTimes; j++)
            {
                int times = Convert.ToInt32(this.SpTimes.Value);

                this.bondHeadController.OpenBondHeadVaccum();

                // 先取标定片 
                this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.GlassVisionMachinePos);

                // 定位小圆并移动至相机中心
                MatchResult smallMatchResult = this.LocatePosition(this.CalibratePara.GlassPRName);

                CalibService.MoveToCamCenterCoo(
                    this.bondModule.BondAxisX,
                    this.bondModule.BondAxisY,
                    smallMatchResult,
                    "BondCameraCoordinateSystem");

                Thread.Sleep(5000);

                this.bondHeadController.RotateAxisT(-180);

                AKRSPoint3D point3D = this.bondModuleController.Get3DRealPosition()
                                      + this.CalibratePara.BondRotateCenterToCamOffset;
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

                    // 真空延时
                    //IPTVacuumOffDelay = 500,
                    //IPTVacuumBuildUpDelay = 500,

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
                double pickLevel = this.CalibratePara.GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                #endregion

                // 取标
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                this.bondHeadController.MoveBondZToSafePos();

                #region 焊点拍照

                // BMC拍照位
                AKRSPoint3D p1MarkPos = this.bondModuleController.ConvertMachineToG0Pos(
                    CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                p1MarkPos.Z = this.bondModuleController
                    .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;
                AKRSPoint3D p2MarkPos = this.bondModuleController.ConvertMachineToG0Pos(
                    CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                p2MarkPos.Z = this.bondModuleController
                    .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;

            RetryBMCP1:
                // P1定位
                MatchResult matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                    p1MarkPos,
                    CalibrateRunPara.GetInstance().BmcPRName);

                DialogResult dialog;

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

                            goto RetryBMCP1;

                        case DialogResult.Abort:

                            throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                        case DialogResult.Ignore:
                            matchResult2 = new MatchResult();
                            break;
                    }
                }

                AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

            RetryBMCP2:
                // P2定位
                MatchResult matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
                    p2MarkPos,
                    CalibrateRunPara.GetInstance().BmcPRName,
                    true);

                if (matchResult3 == null)
                {
                    dialog = AKRSMessageBoxExt.Show(
                        $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P2 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                        "Alarm",
                        new string[] { "Retry", "Abort", "Ignore" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                        AlarmLevel.SecondLevel);

                    switch (dialog)
                    {
                        case DialogResult.Retry:

                            goto RetryBMCP2;

                        case DialogResult.Abort:

                            throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                        case DialogResult.Ignore:
                            matchResult3 = new MatchResult();
                            break;
                    }
                }

                AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);

                #endregion

                #region 计算贴片位

                // 计算放片位
                AKRSPoint3D placePos = (p1VisionPos + p2VisionPos) / 2
                                       + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                #endregion

                #region 贴片

                // 运动到放片位
                this.bondModuleController.MoveToG0Pos(placePos.X, placePos.Y);

                // 放片参数,标定片厚度：2
                double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
                liftLevel = CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos.Z;

            // 放片
            ReBond:
                bool ret = this.bondHeadController.BondAction(placeLevel, liftLevel, component, BondTypeEnum.BondOnBMC);

                if (ret == false)
                {
                    dialog = AKRSMessageBoxExt.Show(
                        $"Component place failed!",
                        "Alarm",
                        new string[] { "Retry", "Ignore" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Ignore },
                        AlarmLevel.SecondLevel);

                    switch (dialog)
                    {
                        // 重新Bond
                        case DialogResult.Retry:

                            goto ReBond;

                        // 终止
                        case DialogResult.Abort:
                            return;

                        // 忽略
                        case DialogResult.Ignore:
                            matchResult2 = new MatchResult();
                            break;
                    }

                    return;
                }

            #endregion

            RetryPostBondGlassP1:
                AKRSPoint3D postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                    CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                postBondP1Pos.Z = this.bondModuleController
                    .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                // 小标定片P1定位
                MatchResult glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                    postBondP1Pos,
                    CalibrateRunPara.GetInstance().GlassPRName,
                    true);

                if (glassMatchResult1 == null)
                {
                    dialog = AKRSMessageBoxExt.Show(
                        $"Downlook  {CalibrateRunPara.GetInstance().GlassPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                        "Alarm",
                        new string[] { "Retry", "Abort", "Ignore" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                        AlarmLevel.SecondLevel);

                    switch (dialog)
                    {
                        case DialogResult.Retry:

                            goto RetryPostBondGlassP1;

                        case DialogResult.Abort:

                            throw new Exception($"{CalibrateRunPara.GetInstance().GlassPRName}  adjust  failed!");

                        case DialogResult.Ignore:
                            glassMatchResult1 = new MatchResult();
                            break;
                    }
                }


                AKRSPoint3D glassVisionPos1 = this.system2Controller.GetBondVisionResultPos(glassMatchResult1);

            RetryPostBondGlassP2:
                // 小标定片P2定位
                AKRSPoint3D postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                    CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                postBondP2Pos.Z = this.bondModuleController
                    .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                MatchResult glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                    postBondP2Pos,
                    CalibrateRunPara.GetInstance().GlassPRName,
                    true);

                if (glassMatchResult2 == null)
                {
                    dialog = AKRSMessageBoxExt.Show(
                        $"Downlook  {CalibrateRunPara.GetInstance().GlassPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                        "Alarm",
                        new string[] { "Retry", "Abort", "Ignore" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                        AlarmLevel.SecondLevel);

                    switch (dialog)
                    {
                        case DialogResult.Retry:

                            goto RetryPostBondGlassP2;

                        case DialogResult.Abort:

                            throw new Exception($"{CalibrateRunPara.GetInstance().GlassPRName}  adjust  failed!");

                        case DialogResult.Ignore:
                            glassMatchResult2 = new MatchResult();
                            break;
                    }
                }

                AKRSPoint3D glassVisionPos2 = this.system2Controller.GetBondVisionResultPos(glassMatchResult2);

                // 计算小标定片中心点
                AKRSPoint3D glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
                                             + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);
                this.bondHeadController.PickAction(placeLevel, component, liftLevel, PickTypeEnum.BMC);

                this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.GlassUpLookVisionMachinePos);

                this.rotateResultList = new List<RotateCenterResult>();

                for (int i = 0; i < times; i++)
                {
                    if (this.isContinue)
                    {
                        RotateCenterResult rotateResult = this.StartMeasure();
                        this.rotateResultList.Add(rotateResult);
                    }
                    else
                    {
                        break;
                    }
                }

                this.bondHeadController.MoveBondZToSafePos();

                // 放回
                this.bondHeadController.RotateAxisT(0);

                this.bondModuleController.MoveSafeBondXYZ(point3D);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 5;

                // 放片
                this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                this.bondModuleController.MoveToSafePos();

                this.SaveCorrectionData();

                FrmRotateCenterResultTwo frmRotateCenterResult = new FrmRotateCenterResultTwo(this.rotateResultList);
                frmRotateCenterResult.ShowDialog();
            }
        }

        /// <summary>
        /// 开始旋转
        /// </summary>
        /// <returns>结果</returns>
        public RotateCenterResult StartMeasure()
        {
            try
            {
                RotateCenterResult rotateResult = new RotateCenterResult();

                List<MatchResult> matchResultList = new List<MatchResult>();

                this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.GlassUpLookVisionMachinePos);

                int[] angleArray = new int[] { -180, -90, 0, 90, 180 };
                List<AKRSPoint2D> circlePointList = new List<AKRSPoint2D>();
                AKRSPoint2D curMachinePos2D = this.bondModuleController.Get2DRealPosition();
                List<AKRSPoint2D> matchResultList2D = new List<AKRSPoint2D>();

                foreach (int angle in angleArray)
                {
                    // T轴旋转一定角度
                    this.bondHeadController.RotateAxisT(angle);
                    MatchResult matchResult = this.LocatePosition(this.CalibratePara.UpLookPRName);

                    matchResultList2D.Add(new AKRSPoint2D(matchResult.CenterX, matchResult.CenterY));

                    AKRSPoint2D circlePoint = CalibService.GetMachinePosByPixelPos(
                        curMachinePos2D,
                        matchResult,
                        "UpLookCameraCoordinateSystem");

                    circlePointList.Add(circlePoint);
                }

                // 拟合像素圆心
                CalibService.FitCircle(matchResultList2D, out double circleCenterPixelX, out double circleCenterPixelY, out double circleRadiusPixel);
                this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, new MatchResult(circleCenterPixelX, circleCenterPixelY, 0));
                AKRSPoint3D curRotateCenterPos = this.bondModuleController.Get3DRealPosition();

                CalibrateTask.GetInstance().StartUpLookMachinePosTask();

                rotateResult.RotateCenterX = curRotateCenterPos.X;
                rotateResult.RotateCenterY = curRotateCenterPos.Y;

                rotateResult.UpLookMarkX = this.CalibratePara.UpLookMarkMachinePos.X;
                rotateResult.UpLookMarkY = this.CalibratePara.UpLookMarkMachinePos.Y;

                return rotateResult;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        ///  保存数据
        /// </summary>
        private void SaveCorrectionData()
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(this.path));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Number";

                worksheet.Cells[1, 2].Value = "RotateCenterX";

                worksheet.Cells[1, 3].Value = "RotateCenterY";

                worksheet.Cells[1, 4].Value = "UpLookX";

                worksheet.Cells[1, 5].Value = "UpLookY";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < this.rotateResultList.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value = i + 1;

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value = this.rotateResultList[i].RotateCenterX;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = this.rotateResultList[i].RotateCenterY;

                worksheet.Cells[lastUsedRow + 1 + i, 4].Value = this.rotateResultList[i].UpLookMarkX;

                worksheet.Cells[lastUsedRow + 1 + i, 5].Value = this.rotateResultList[i].UpLookMarkY;
            }

            excelPackage.Save();

            // ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
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

            Thread.Sleep(500);
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

        /// <summary>
        /// 线程终止
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            this.isContinue = false;
        }
    }

    /// <summary>
    /// 旋转定位结果
    /// </summary>
    public class RotateCenterResult
    {
        /// <summary>
        /// 旋转中心X
        /// </summary>
        [JsonIgnore]
        public double RotateCenterX { get; set; }

        /// <summary>
        /// 旋转中心Y
        /// </summary>
        [JsonIgnore]
        public double RotateCenterY { get; set; }

        /// <summary>
        /// 温漂点X
        /// </summary>
        [JsonIgnore]
        public double UpLookMarkX { get; set; }

        /// <summary>
        /// 温漂点Y
        /// </summary>
        [JsonIgnore]
        public double UpLookMarkY { get; set; }

    }
}