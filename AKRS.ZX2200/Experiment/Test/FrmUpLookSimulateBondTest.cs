namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
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
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DevExpress.XtraEditors;

    using OfficeOpenXml;

    /// <summary>
    /// 上视模拟贴片测试
    /// </summary>
    public partial class FrmUpLookSimulateBondTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmUpLookSimulateBondTest()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 上视模拟贴片测试
        /// </summary>
        private Task testTask;

        ///// <summary>
        /////  测试结果
        ///// </summary>
        //private List<MatchResult> p1MatchResultList = new List<MatchResult>();

        ///// <summary>
        /////  测试结果
        ///// </summary>
        //private List<MatchResult> p2MatchResultList = new List<MatchResult>();

        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycles;

        /// <summary>
        ///  是否放片到BMC
        /// </summary>
        private bool isPlaceGlassOnBMC;

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
        /// System2Domain
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara BMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// 开始按钮
        /// </summary>
        /// <param name="sender">数据源</param>
        /// <param name="e">参数</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // 提示
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"Please  attatch  test  tool  on  bonding  head!",
                "Question",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Cancel)
            {
                return;
            }

            // 参数保存
            this.Save();

            // 更换治具
            this.bondHeadController.SetTouchDownOnBondhead();

            // 开启测试线程
            this.StartTask();
        }

        /// <summary>
        /// 上视模拟贴片
        /// </summary>
        /// <returns>结果</returns>
        private bool UpLookSimulateBondTest()
        {
            try
            {
                AKRSPoint3D placePos = new AKRSPoint3D();

                this.bondHeadController.MoveBondZToSafePos();

                // 待机位是-180
                this.bondHeadController.RotateAxisT(-180);

                #region 取小标定片

                // 获取标定片拍照位、PR
                AKRSPoint3D glassCenterPosition = CalibrateRunPara.GetInstance().GlassVisionMachinePos;
                AKRSPoint3D glassCenterPositionInG0 = this.bondModuleController.ConvertMachineToG0Pos(glassCenterPosition);
                string pRName = CalibrateRunPara.GetInstance().GlassPRName;

            Retry:
                // 执行定位
                MatchResult matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                    glassCenterPositionInG0,
                    pRName);

                if (matchResult1 == null)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"PR：{pRName}  vision  failed!Please  check  glass!",
                        "Warn",
                        MessageBoxButtons.RetryCancel,
                        MessageBoxIcon.Warning);

                    if (dialog == DialogResult.Retry)
                    {
                        goto Retry;
                    }

                    return false;
                }

                // 计算取片位(G0)
                AKRSPoint3D pickPos = this.system2Controller.GetBondVisionResultPos(matchResult1)
                                      + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                // 移动到取片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

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
                double pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;


                #region 吸嘴堵塞判断

                //if (this.bondHeadController.IsComponentOnTool())
                //{
                //    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                //        $"A component is detected on the nozzle !\r\n" + "Yes：Ignore\r\n" + "No：Reject component \r\n",
                //        "Alarm",
                //        new string[] { "Yes", "No" },
                //        new DialogResult[] { DialogResult.Yes, DialogResult.No },
                //        AlarmLevel.SecondLevel);
                //    switch (dialogResult)
                //    {
                //        case DialogResult.Yes:
                //            break;

                //        case DialogResult.No:
                //            this.bondModuleController.ThrowAction();
                //            break;

                //        default:
                //            break;
                //    }
                //}

                // 关闭吸嘴真空
                this.bondHeadController.CloseToolVaccum();

            #endregion

            // 取片
            RePick:
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

            #region 漏晶检测

            ReCheckVacuum:

                //if (component.IsActiveComponentDetection)
                //{
                //    // 吸真空检测延迟
                //    DelayHelper.Delay(100);

                //    // 检测真空
                //    if (!this.bondHeadController.IsComponentOnTool())
                //    {
                //        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                //            $"Pick  component failed! \r\n" + "Recheck：Recheck nozzle vacuum\r\n"
                //                                            + "Cancel：Exit program\r\n"
                //                                            + "Ignore：Ignore  this  exception\r\n"
                //                                            + "RePick: RePick component\r\n"
                //                                            + "NextDie:Pick next die\r\n",
                //            "Alarm",
                //            new string[] { "Recheck", "Cancel", "Ignore", "RePick" },
                //            new DialogResult[]
                //                {
                //                    DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.Cancel
                //                },
                //            AlarmLevel.SecondLevel);

                //        switch (dialogResult)
                //        {
                //            case DialogResult.Retry:
                //                goto ReCheckVacuum;

                //            case DialogResult.Abort:
                //                return false;

                //            case DialogResult.Ignore:
                //                break;

                //            case DialogResult.Cancel:
                //                goto RePick;
                //        }
                //    }
                //}

                #endregion

                #endregion

                for (int i = 1; i <= this.cycles; i++)
                {
                    this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    // 计算上视拍照位
                    AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                    AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);
                    AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);

                    // 寻找Pr模板
                    PREntity upLookPREntity = (PREntity)VisionEntityRepository.GetInstance().Find(CalibrateRunPara.GetInstance().UpLookPRName);

                    if (upLookPREntity == null)
                    {
                        AKRSMessageBoxExt.Show(
                            $"No PR template named {CalibrateRunPara.GetInstance().UpLookPRName} was not  found during BMC  test, about to exit the automatic worker thread\r\n",
                            "Alarm",
                            new string[] { "OK" },
                            new DialogResult[] { DialogResult.Abort },
                            AlarmLevel.SecondLevel);

                        return false;
                    }

                    #region 上视定位

                    // 运动到上视P1拍照位
                    this.bondModuleController.MoveSafeBondXYZ(upLookVisionPos1);

                    MatchResult upLookRes1, upLookRes2, upLookRes3, upLookRes4;

                // P1定位
                RetryP1:

                    // 开始定位
                    ExcuteResult upLookP1Result = upLookPREntity.DoWork();

                    upLookRes1 = (MatchResult)upLookPREntity.AlgResult;

                    // 处理拍照完成后的结果
                    if (upLookP1Result != ExcuteResult.Success)
                    {
                        DialogResult dialog = AKRSMessageBoxExt.Show(
                            $"Uplook  {CalibrateRunPara.GetInstance().UpLookPRName} adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto RetryP1;

                            case DialogResult.Abort:

                                // 退出
                                return false;

                            case DialogResult.Ignore:
                                upLookRes1 = new MatchResult();
                                break;
                        }
                    }

                    AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), upLookRes1);

                    // 保存结果图片
                    Bitmap bitmap =
                      upLookPREntity.AlgResult.OutPutImg1;
                    string path = "D:\\UpLookBitmapData\\" + DateTime.Now.ToString("ddhhmmssfff") + upLookPREntity.GetName() + ".bmp";
                    bitmap.Save(path);

                    // 运动到P2拍照位
                    // Z轴安全直接移动XY轴
                    System2Module.GetInstance().BondModule.MoveBondXY(upLookVisionPos2.X, upLookVisionPos2.Y);

                // P2定位
                RetryP2:

                    // 开始定位
                    ExcuteResult upLookP2Result = upLookPREntity.DoWork();

                    upLookRes2 = (MatchResult)upLookPREntity.AlgResult;

                    // 处理拍照完成后的结果
                    if (upLookP1Result != ExcuteResult.Success)
                    {
                        DialogResult dialog = AKRSMessageBoxExt.Show(
                            $"Uplook  {CalibrateRunPara.GetInstance().UpLookPRName} adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                            "Alarm",
                            new string[] { "Retry", "Abort", "Ignore" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                            AlarmLevel.SecondLevel);

                        switch (dialog)
                        {
                            case DialogResult.Retry:

                                goto RetryP2;

                            case DialogResult.Abort:

                                // 退出
                                return false;

                            case DialogResult.Ignore:
                                matchResult1 = new MatchResult();
                                break;
                        }
                    }

                    AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), upLookRes2);

                    // 保存结果图片
                    bitmap.Save(path);

                    #endregion

                    #region 上视纠偏计算

                    // 计算角度
                    double uplookOffSetAngle = CalibService.CalculateAngle(new AKRSPoint2D(point3D1.X, point3D1.Y), new AKRSPoint2D(point3D2.X, point3D2.Y));

                    double placeAngle = uplookOffSetAngle - 45 /*- this.compensateAngle*/;

                    //AKRSPoint3D pos1 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                    //AKRSPoint3D pos2 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                    // 小标定片中心
                    AKRSPoint3D glassUpLookCenter = (point3D1 + point3D2) / 2;

                    // 焊头旋转中心
                    AKRSPoint3D bondHeadCenter =
                        this.bondModuleController.ConvertMachineToG0Pos(
                            BondDevicePara.GetInstance().CameraDevicePara.UpLookPos);

                    // 相对旋转中心的偏移
                    AKRSPoint2D offsetCenter = new AKRSPoint2D()
                    {
                        X = glassUpLookCenter.X - bondHeadCenter.X,
                        Y = glassUpLookCenter.Y - bondHeadCenter.Y,
                    };

                    // 旋转之后的偏移
                    AKRSPoint2D offsetAfterRotate = this.bondHeadController.GetNozzleOffset(offsetCenter, placeAngle);

                    //// 上视定位偏移
                    //AKRSPoint3D uplookOffSet = (this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1)
                    //                            + this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2)
                    //                            - point3D1 - point3D2) / 2;

                    #endregion

                    #region 上视纠偏验证

                    // this.bondHeadController.MoveBondZToSafePos();

                    // 提前转
                    this.bondHeadController.RelativeRotateAxisT(-placeAngle);

                    // p1
                    AKRSPoint3D test1 = upLookVisionPos1 + new AKRSPoint3D(offsetAfterRotate.X, offsetAfterRotate.Y, 0);

                    // 运动到上视P1拍照位
                    this.bondModuleController.MoveSafeBondXYZ(test1);

                    // 定位
                    upLookP1Result = upLookPREntity.DoWork();

                    upLookRes3 = (MatchResult)upLookPREntity.AlgResult;

                    point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), (MatchResult)upLookPREntity.AlgResult);

                    // p2
                    AKRSPoint3D test2 = upLookVisionPos2 + new AKRSPoint3D(offsetAfterRotate.X, offsetAfterRotate.Y, 0);

                    // 运动到上视P2拍照位
                    System2Module.GetInstance().BondModule.MoveBondXY(test2.X, test2.Y);

                    // 定位
                    upLookP2Result = upLookPREntity.DoWork();

                    upLookRes4 = (MatchResult)upLookPREntity.AlgResult;

                    point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), (MatchResult)upLookPREntity.AlgResult);

                    // 小标定片中心
                    AKRSPoint3D centerAfterTest = (point3D1 + point3D2) / 2;

                    // 打印
                    this.ExportData(upLookRes1, upLookRes2, upLookRes3, upLookRes4, i);

                    #endregion

                    #region 焊点拍照、计算贴片位

                    // BMC拍照位
                    AKRSPoint3D p1MarkPos =
                        this.bondModuleController.ConvertMachineToG0Pos(
                            CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                    p1MarkPos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;
                    AKRSPoint3D p2MarkPos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                    p2MarkPos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().BMCVisionMachinePos).Z;

                    // P1定位
                    MatchResult matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                        p1MarkPos,
                        CalibrateRunPara.GetInstance().BmcPRName);

                    AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

                    // P2定位
                    MatchResult matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
                        p2MarkPos,
                        CalibrateRunPara.GetInstance().BmcPRName);

                    AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);

                    if (i == 1)
                    {
                        // 计算放片位
                        placePos = (p1VisionPos + p2VisionPos) / 2
                                   + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                    }


                    #endregion

                    #region 贴片、取片

                    if (this.isPlaceGlassOnBMC)
                    {
                        #region 贴片

                        // 运动到放片位
                        this.bondModuleController.MoveToG0Pos(placePos.X, placePos.Y);

                        // 放片参数,标定片厚度：2
                        double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
                        liftLevel = CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z;

                        // 放片
                        ReBond:
                        bool ret = this.bondHeadController.BondAction(
                            placeLevel,
                            liftLevel,
                            component,
                            BondTypeEnum.BondOnBMC);

                        if (ret == false)
                        {
                            DialogResult dialog = AKRSMessageBoxExt.Show(
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
                                    return false;

                                // 忽略
                                case DialogResult.Ignore:
                                    break;
                            }

                            return false;
                        }


                        #endregion

                        #region 取片

                        // 运动到取片位
                        this.bondModuleController.MoveToG0Pos(placePos.X , placePos.Y); 
                        //this.bondHeadController.RotateAxisT(-180);

                        // 计算参数
                        pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                        // 取片
                        this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                        #endregion
                    }

                    #endregion
                }

                #region 放回小标定片

                this.bondHeadController.MoveBondZToSafePos();

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 5;

                // 放片
                this.bondHeadController.BondAction(
                    pickLevel,
                    liftLevel,
                    component,
                    BondTypeEnum.BondOnTU);

                this.bondModuleController.MoveToSafePos();

                #endregion

                return true;
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"UpLook  simulate  bond  test  failed:{e.ToString()}!",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                throw e;
            }
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        /// <returns>结果</returns>
        private bool StartTask()
        {
            // 防止线程多次启动
            if (this.testTask == null
                || this.testTask.Status != TaskStatus.Running)
            {
                //this.p1MatchResultList.Clear();
                //this.p2MatchResultList.Clear();
                this.testTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("BMC测试线程");
                            this.UpLookSimulateBondTest();
                        });
            }

            return true;
        }

        /// <summary>
        /// 参数保存
        /// </summary>
        private void Save()
        {
            this.cycles = (int)this.SpCycles.Value;
         
            this.isPlaceGlassOnBMC = this.ChkIsPlaceGlassOnBMC.Checked;
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="p1">p1</param>
        /// <param name="p2">p2</param>
        /// <param name="p1AfterAdjust">p1AfterAdjust</param>
        /// <param name="p2AfterAdjust">p2AfterAdjust</param>
        /// <param name="cycle">cycle</param>
        private void ExportData(MatchResult p1, MatchResult p2, MatchResult p1AfterAdjust, MatchResult p2AfterAdjust, int cycle)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(
                new FileInfo(
                    @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--UpLookSimulateBondTestData.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 25; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Number";

                worksheet.Cells[1, 2].Value = "P1_X";

                worksheet.Cells[1, 3].Value = "P1_Y";

                worksheet.Cells[1, 4].Value = "P1_Angle";

                worksheet.Cells[1, 5].Value = "P2_X";

                worksheet.Cells[1, 6].Value = "P2_Y";

                worksheet.Cells[1, 7].Value = "P2_Angle";

                worksheet.Cells[1, 8].Value = "P1AfterAdjust_X";

                worksheet.Cells[1, 9].Value = "P1AfterAdjust_Y";

                worksheet.Cells[1, 10].Value = "P1AfterAdjust_Angle";

                worksheet.Cells[1, 11].Value = "P2AfterAdjust_X";

                worksheet.Cells[1, 12].Value = "P2AfterAdjust_Y";

                worksheet.Cells[1, 13].Value = "P2AfterAdjust_Angle";

                worksheet.Cells[1, 14].Value = "Time";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = cycle;

            worksheet.Cells[lastUsedRow + 1, 2].Value = p1.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = p1.CenterY;

            worksheet.Cells[lastUsedRow + 1, 4].Value = p1.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = p2.CenterX;

            worksheet.Cells[lastUsedRow + 1, 6].Value = p2.CenterY;

            worksheet.Cells[lastUsedRow + 1, 7].Value = p2.Angle;

            worksheet.Cells[lastUsedRow + 1, 8].Value = p1AfterAdjust.CenterX;

            worksheet.Cells[lastUsedRow + 1, 9].Value = p1AfterAdjust.CenterY;

            worksheet.Cells[lastUsedRow + 1, 10].Value = p1AfterAdjust.Angle;

            worksheet.Cells[lastUsedRow + 1, 11].Value = p2AfterAdjust.CenterX;

            worksheet.Cells[lastUsedRow + 1, 12].Value = p2AfterAdjust.CenterY;

            worksheet.Cells[lastUsedRow + 1, 13].Value = p2AfterAdjust.Angle;

            worksheet.Cells[lastUsedRow + 1, 14].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            excelPackage.Save();
        }
    }
}