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
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Experiment.Test;

namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    using System.IO;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using DevExpress.XtraEditors;
    using OfficeOpenXml;

    public partial class FrmPlacementTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmPlacementTest()
        {
            InitializeComponent();
        }

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
        /// BMC中心拍照位
        /// todo:待示教
        /// </summary>
        private AKRSPoint3D bmcCenterVisionPos = new AKRSPoint3D(-102.84065, -308.046675, 102.3429);

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

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara bMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        ///  测试结果
        /// </summary>
        private List<BMCTestResult> bMCTestResultList = new List<BMCTestResult>();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        private System2Configuration system2Configuration = new System2Configuration();


        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 测试结束标志
        /// </summary>
        private bool testFinished = false;

        /// <summary>
        /// BMC测试线程
        /// </summary>
        private Task pickTestTask;

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
            if (this.pickTestTask == null
                || this.pickTestTask.Status != TaskStatus.Running)
            {
                this.bMCTestResultList.Clear();
                this.BtnStart.Enabled = false;
                this.BtnStart.BackColor = Color.Yellow;
                this.pickTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("放片误差实验线程");
                            this.PlacementTest();
                        });
            }

            return true;
        }

        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool PlacementTest()
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

                MatchResult matchResult2 = null;
                MatchResult matchResult3 = null;

                AKRSPoint3D downlookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D downlookP2ResInAxis = new AKRSPoint3D();


                AKRSPoint3D glassCenterPos = new AKRSPoint3D();
                this.bondHeadController.MoveBondZToSafePos();

                #region 取小标定片

                this.bondHeadController.RotateAxisT(0);

                // 获取标定片拍照位、PR
                AKRSPoint3D glassCenterPosition = CalibrateRunPara.GetInstance().GlassVisionMachinePos;
                AKRSPoint3D glassCenterPositionInG0 =
                    this.bondModuleController.ConvertMachineToG0Pos(glassCenterPosition);
                string pRName = CalibrateRunPara.GetInstance().GlassPRName;

            Retry:
                // 执行定位
                MatchResult matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                    glassCenterPositionInG0,
                    pRName,
                    this.visionTime);

                if (matchResult1 == null)
                {
                    dialog = AKRSXtraMessageBox.Show(
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

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

                //    matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                //        glassCenterPositionInG0,
                //        pRName,
                //        this.visionTime);
                //}

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
                                                          this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                    SlowTravelDistanceBeforePickup =
                                                          this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                    IsActivateSlowTravelAfterPickup = true,
                    SlowTravelSpeedAfterPickup =
                                                          this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                    SlowTravelDistanceAfterPickup =
                                                          this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                    VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,
                    PickupDelay = this.bMCDevicePara.PickupDelay,

                    // todo:力控暂时没接
                    PickupForceMode = ForceModeEnum.Distance,

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
                    WeakBlowProportion = 10000
                };
                double pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 关闭吸嘴真空
                this.bondHeadController.CloseToolVaccum();

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                #endregion


                #region 上视定位

                // 计算上视拍照位
                AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

                AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                // P1定位
                upLookRes1 = (MatchResult)this.system2Controller.UpLookCameraVision(
                    upLookVisionPos1InG0,
                    CalibrateRunPara.GetInstance().UpLookPRName,
                    this.visionTime);

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(upLookVisionPos1, upLookRes1);

                //    upLookRes1 =
                //        (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                //}

                AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                    this.bondModuleController.Get3DRealPosition(),
                    upLookRes1);

                uplookP1ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D1);

                // 运动到P2拍照位
                // Z轴安全直接移动XY轴
                System2Module.GetInstance().BondModule.MoveBondXY(upLookVisionPos2.X, upLookVisionPos2.Y);

                upLookRes2 = (MatchResult)this.system2Controller.UpLookCameraVision(
                    null,
                    CalibrateRunPara.GetInstance().UpLookPRName,
                    this.visionTime);

                AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                    this.bondModuleController.Get3DRealPosition(),
                    upLookRes2);

                uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                #endregion

                #region 上视纠偏计算

                // 计算角度
                double uplookOffSetAngle = CalibService.CalculateAngle(
                    new AKRSPoint2D(point3D1.X, point3D1.Y),
                    new AKRSPoint2D(point3D2.X, point3D2.Y));

                double placeAngle = uplookOffSetAngle - 45;

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

                // 贴片的角度
                double bondAngle = this.bondHeadController.GetAxisTRealPos() - placeAngle;

                #endregion

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

                this.bondHeadController.MoveBondZToSafePos();

                // P1定位
                 matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                    p1MarkPos,
                    CalibrateRunPara.GetInstance().BmcPRName);

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
                 matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
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

                // 最终贴片位，暂时不动态补偿
                AKRSPoint2D finalPlacePos = new AKRSPoint2D()
                {
                    X = placePos.X + offsetAfterRotate.X,
                    Y = placePos.Y + offsetAfterRotate.Y 
                };

                #endregion

                #region 贴片

                // 运动到放片位
                this.bondModuleController.MoveToG0Pos(finalPlacePos.X, finalPlacePos.Y);

                // 放片参数,标定片厚度：2
                double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
                liftLevel = CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos.Z;

            // 放片
            ReBond:
                bool ret = this.bondHeadController.BondAction(placeLevel, liftLevel + 10, component, BondTypeEnum.BondOnBMC);

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
                            return false;

                        // 忽略
                        case DialogResult.Ignore:
                            matchResult2 = new MatchResult();
                            break;
                    }

                    return false;
                }

                #endregion

                // 循环测试
                for (int i = 1; i <= this.cycles; i++)
                {
                    this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    #region 定位BMC和标定片中心

                    // 拍BMC中心
                    MatchResult bMCCenterMatchResult1 =
                        (MatchResult)this.system2Controller.BondCameraVision(this.bmcCenterVisionPos, "圆搜索", this.visionTime);

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        AKRSPoint3D visionMachinePos = this.bondModuleController.ConvertG0ToMachinePos(this.bmcCenterVisionPos);

                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(visionMachinePos, bMCCenterMatchResult1);

                        bMCCenterMatchResult1 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "圆搜索", this.visionTime);
                    }

                    // 拍标定片中心
                    MatchResult glassCenterMatchResult1 =
                        (MatchResult)this.system2Controller.UpLookCameraVision(this.bmcCenterVisionPos, "圆搜索", this.visionTime);

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        AKRSPoint3D visionMachinePos = this.bondModuleController.ConvertG0ToMachinePos(this.bmcCenterVisionPos);

                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(visionMachinePos, glassCenterMatchResult1);

                        glassCenterMatchResult1 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "圆搜索", this.visionTime);
                    }

                    // 计算差值
                    AKRSPoint2D deviation = new AKRSPoint2D()
                                                {
                                                    X = glassCenterMatchResult1.CenterX - bMCCenterMatchResult1.CenterX,
                                                    Y = glassCenterMatchResult1.CenterY - bMCCenterMatchResult1.CenterY
                                                };

                    #endregion

                    #region 焊点拍照

                    // 大标定片P1定位
                    AKRSPoint3D postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                    postBondP1Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondBMCP1:
                    MatchResult bMCMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP1Pos,
                        CalibrateRunPara.GetInstance().BmcPRName,
                        false);

                    if (bMCMatchResult1 == null)
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

                                goto RetryPostBondBMCP1;

                            case DialogResult.Abort:

                                throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                            case DialogResult.Ignore:
                                bMCMatchResult1 = new MatchResult();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), bMCMatchResult1);

                        bMCMatchResult1 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime);
                    }

                    AKRSPoint3D bMCVisionPos1 = this.system2Controller.GetBondVisionResultPosInMachine(bMCMatchResult1);

                RetryPostBondGlassP1:
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


                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), glassMatchResult1);

                        glassMatchResult1 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().GlassPRName, this.visionTime);
                    }

                    AKRSPoint3D glassVisionPos1 = this.system2Controller.GetBondVisionResultPosInMachine(glassMatchResult1);

                    // 大标定片P2定位
                    AKRSPoint3D postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                    postBondP2Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondBMCP2:
                    MatchResult bMCMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP2Pos,
                        CalibrateRunPara.GetInstance().BmcPRName,
                        false);

                    if (bMCMatchResult2 == null)
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

                                goto RetryPostBondBMCP2;

                            case DialogResult.Abort:

                                throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                            case DialogResult.Ignore:
                                bMCMatchResult2 = new MatchResult();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), bMCMatchResult2);

                        bMCMatchResult2 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime);
                    }

                    AKRSPoint3D bMCVisionPos2 = this.system2Controller.GetBondVisionResultPosInMachine(bMCMatchResult2);

                RetryPostBondGlassP2:

                    // 小标定片P2定位
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

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(this.bondModuleController.Get3DRealPosition(), glassMatchResult2);

                        glassMatchResult2 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().GlassPRName, this.visionTime);
                    }

                    AKRSPoint3D glassVisionPos2 = this.system2Controller.GetBondVisionResultPosInMachine(glassMatchResult2);

                    // 计算大标定片中心点
                    AKRSPoint3D bMCCenterPos = (postBondP1Pos + postBondP2Pos) / 2
                                               + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                    double bMCAngle = CalibService.CalculateAngle(
                        new AKRSPoint2D(bMCVisionPos1.X, bMCVisionPos1.Y),
                        new AKRSPoint2D(bMCVisionPos2.X, bMCVisionPos2.Y));

                    // 计算小标定片中心点
                     glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
                                                 + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                    double glassAngle = CalibService.CalculateAngle(
                        new AKRSPoint2D(glassVisionPos1.X, glassVisionPos1.Y),
                        new AKRSPoint2D(glassVisionPos2.X, glassVisionPos2.Y));

                    // 小标定片中心-大标定片中心点
                    AKRSPoint3D postBondRes = glassCenterPos - bMCCenterPos;
                    double postBondAngle = glassAngle - bMCAngle;

                    #endregion

                    #region 取片

                    // 运动到取片位
                    this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);
                    this.bondHeadController.RotateAxisT(0);

                    // 计算参数
                    pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                    // 取片
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel + 5, PickTypeEnum.BMC);

                    #endregion

                    #region 贴片

                    // 运动到放片位
                    this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);

                    // 放片
               ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel + 10,
                        component,
                        BondTypeEnum.BondOnBMC);

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
                                return false;

                            // 忽略
                            case DialogResult.Ignore:
                                matchResult2 = new MatchResult();
                                break;
                        }

                        return false;
                    }

                    #endregion

                    #region 定位BMC和标定片中心

                    // 拍BMC中心
                    MatchResult bMCCenterMatchResult2 =
                        (MatchResult)this.system2Controller.BondCameraVision(this.bmcCenterVisionPos, "圆搜索", this.visionTime);

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        AKRSPoint3D visionMachinePos = this.bondModuleController.ConvertG0ToMachinePos(this.bmcCenterVisionPos);

                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(visionMachinePos, bMCCenterMatchResult2);

                        bMCCenterMatchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, "圆搜索", this.visionTime);
                    }

                    // 拍标定片中心
                    MatchResult glassCenterMatchResult2 =
                        (MatchResult)this.system2Controller.UpLookCameraVision(this.bmcCenterVisionPos, "圆搜索", this.visionTime);

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        AKRSPoint3D visionMachinePos = this.bondModuleController.ConvertG0ToMachinePos(this.bmcCenterVisionPos);

                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(visionMachinePos, glassCenterMatchResult2);

                        glassCenterMatchResult2 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, "圆搜索", this.visionTime);
                    }

                    // 计算差值
                    AKRSPoint2D deviation2 = new AKRSPoint2D()
                    {
                        X = glassCenterMatchResult2.CenterX - bMCCenterMatchResult2.CenterX,
                        Y = glassCenterMatchResult2.CenterY - bMCCenterMatchResult2.CenterY
                    };

                    #endregion

                    // 保存数据
                    this.SaveCurrentData(
                        deviation,
                        bMCMatchResult1,
                        bMCMatchResult2,
                        CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos,
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos,
                        bMCVisionPos1,
                        bMCVisionPos2,
                        glassMatchResult1,
                        glassMatchResult2,
                        CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos,
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos,
                        glassVisionPos1,
                        glassVisionPos2,
                        deviation2);
                }

                #region 取片

                // 运动到取片位
                this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);
                this.bondHeadController.RotateAxisT(0);

                // 计算参数
                pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel + 5, PickTypeEnum.BMC);

                #endregion

                #region 放回小标定片

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                // 放片
                this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);
                #endregion



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
                    $"放片实验结束!",
                    "信息",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
        private void SaveCurrentData(AKRSPoint2D deviation, MatchResult bmcP1MatchRes, MatchResult bmcP2MatchRes, AKRSPoint3D bmcP1VisionPos, AKRSPoint3D bmcP2VisionPos, AKRSPoint3D bmcP1VisonResInAxis, AKRSPoint3D bmcP2VisonResInAxis, MatchResult glassP1MatchRes, MatchResult glassP2MatchRes, AKRSPoint3D glassP1VisionPos, AKRSPoint3D glassP2VisionPos, AKRSPoint3D glassP1VisonResInAxis, AKRSPoint3D glassP2VisonResInAxis, AKRSPoint2D deviation2)
        {
            string fileName = $"D:\\精度实验2025\\放片误差验证实验{DateTime.Now:yyyyMMdd}";

            // 添加一个工作表
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName + @".xlsx"));

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

                worksheet.Cells[1, 2].Value = "BMC中心和标定片中心差值(前)-X";

                worksheet.Cells[1, 3].Value = "BMC中心和标定片中心差值（前）-Y";

                worksheet.Cells[1, 4].Value = "bmcP1MatchRes-X";

                worksheet.Cells[1, 5].Value = "bmcP1MatchRes-Y";

                worksheet.Cells[1, 6].Value = "bmcP2MatchRes-X";

                worksheet.Cells[1, 7].Value = "bmcP2MatchRes-Y";

                worksheet.Cells[1, 8].Value = "bmcP1拍照位-X";

                worksheet.Cells[1, 9].Value = "bmcP1拍照位-Y";

                worksheet.Cells[1, 10].Value = "bmcP2拍照位-X";

                worksheet.Cells[1, 11].Value = "bmcP2拍照位-Y";

                worksheet.Cells[1, 12].Value = "BMCP1拍照结果转到轴坐标-X";

                worksheet.Cells[1, 13].Value = "BMCP1拍照结果转到轴坐标-Y";

                worksheet.Cells[1, 14].Value = "BMCP2拍照结果转到轴坐标-X";

                worksheet.Cells[1, 15].Value = "BMCP2拍照结果转到轴坐标-Y";

                worksheet.Cells[1, 16].Value = "glassP1MatchRes-X";

                worksheet.Cells[1, 17].Value = "glassP1MatchRes-Y";

                worksheet.Cells[1, 18].Value = "glassP2MatchRes-X";

                worksheet.Cells[1, 19].Value = "glassP2MatchRes-Y";

                worksheet.Cells[1, 20].Value = "glassP1拍照位-X";

                worksheet.Cells[1, 21].Value = "glassP1拍照位-Y";

                worksheet.Cells[1, 22].Value = "glassP2拍照位-X";

                worksheet.Cells[1, 23].Value = "glassP2拍照位-Y";

                worksheet.Cells[1, 24].Value = "glassP1拍照结果转到轴坐标-X";

                worksheet.Cells[1, 25].Value = "glassP1拍照结果转到轴坐标-Y";

                worksheet.Cells[1, 26].Value = "glassP2拍照结果转到轴坐标-X";

                worksheet.Cells[1, 27].Value = "glassP2拍照结果转到轴坐标-Y";

                worksheet.Cells[1, 28].Value = "BMC中心和标定片中心差值(后)-X";

                worksheet.Cells[1, 29].Value = "BMC中心和标定片中心差值（后）-Y";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = deviation?.X;

            worksheet.Cells[lastUsedRow + 1, 3].Value = deviation?.Y;

            worksheet.Cells[lastUsedRow + 1, 4].Value = bmcP1MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 5].Value = bmcP1MatchRes?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 6].Value = bmcP2MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 7].Value = bmcP2MatchRes?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 8].Value = bmcP1VisionPos?.X;

            worksheet.Cells[lastUsedRow + 1, 9].Value = bmcP1VisionPos?.Y;

            worksheet.Cells[lastUsedRow + 1, 10].Value = bmcP2VisionPos?.X;

            worksheet.Cells[lastUsedRow + 1, 11].Value = bmcP2VisionPos?.Y;


            worksheet.Cells[lastUsedRow + 1, 12].Value = bmcP1VisonResInAxis?.X;

            worksheet.Cells[lastUsedRow + 1, 13].Value = bmcP1VisonResInAxis?.Y;

            worksheet.Cells[lastUsedRow + 1, 14].Value = bmcP2VisonResInAxis?.X;

            worksheet.Cells[lastUsedRow + 1, 15].Value = bmcP2VisonResInAxis?.Y;

            worksheet.Cells[lastUsedRow + 1, 16].Value = glassP1MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 17].Value = glassP1MatchRes?.CenterY;


            worksheet.Cells[lastUsedRow + 1, 18].Value = glassP2MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 19].Value = glassP2MatchRes?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 20].Value = glassP1VisionPos?.X;

            worksheet.Cells[lastUsedRow + 1, 21].Value = glassP1VisionPos?.Y;

            worksheet.Cells[lastUsedRow + 1, 22].Value = glassP2VisionPos?.X;

            worksheet.Cells[lastUsedRow + 1, 23].Value = glassP2VisionPos?.Y;

            worksheet.Cells[lastUsedRow + 1, 24].Value = glassP1VisonResInAxis?.X;

            worksheet.Cells[lastUsedRow + 1, 25].Value = glassP1VisonResInAxis?.Y;

            worksheet.Cells[lastUsedRow + 1, 26].Value = glassP2VisonResInAxis?.X;

            worksheet.Cells[lastUsedRow + 1, 27].Value = glassP2VisonResInAxis?.Y;

            worksheet.Cells[lastUsedRow + 1, 28].Value = deviation2?.X;

            worksheet.Cells[lastUsedRow + 1, 29].Value = deviation2?.Y;

            excelPackage.Save();
        }
    }
}
