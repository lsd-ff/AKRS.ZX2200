using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Experiment.Test;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;

using DevExpress.DataProcessing.InMemoryDataProcessor;
using DevExpress.XtraEditors;

using OfficeOpenXml;

using UcMainSystem = AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem;

namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.ZX2200.BondSystem.Models.Parameter;

    using DataAnalysis.Acquisition;

    /// <summary>
    /// 取片测试
    /// </summary>
    public partial class FrmPickupRepeatTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmPickupRepeatTest()
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

        private AKRSPoint3D bmcToolVisionPos = new AKRSPoint3D(-102.5064, -307.67777, 102.6241);


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

        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            // 换BMC
            if (!this.system2Controller.ChangeNozzleAssistance("BMC"))
            {
                return;
            }

            // 更换治具
            this.bondHeadController.SetBMCOnBondhead();

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
                            CommonUtil.SetCurrentThreadName("取片误差实验线程");
                            this.PickTest();
                        });
            }

            return true;
        }

        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool PickTest()
        {
            string fileName = $"D:\\精度实验2025\\取片误差验证实验{DateTime.Now:HHmmss}";
            IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"取片误差验证实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");

            try
            {
              
                DialogResult dialog;
                AKRSPoint3D newVisionPos = new();

                ExcuteResult upLookP1Result;
                ExcuteResult upLookP2Result;

                MatchResult upLookRes1 = null, upLookRes2 = null;
                AKRSPoint3D uplookP1ResInAxis = new();
                AKRSPoint3D uplookP2ResInAxis = new();

                MatchResult matchResult2 = null;
                MatchResult matchResult3 = null;

                MatchResult glassMatchResult1 = null, glassMatchResult2 = null;
                ;

                AKRSPoint3D glassVisionPos2 = new(), glassVisionPos1 = new();

                // BMC Mark1 拍摄位置
                AKRSPoint3D downlookP1ResInAxis = new();

                // BMC Mark2 拍摄位置
                AKRSPoint3D downlookP2ResInAxis = new();
                this.bondHeadController.MoveBondZToSafePos();

                #region BMC治具中心定位

                //// 位置待示教
                //MatchResult bMCToolMatchResult1 =
                //    (MatchResult)this.system2Controller.UpLookCameraVision(this.bmcToolVisionPos, "圆搜索",
                //        this.visionTime);

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    AKRSPoint3D visionMachinePos =
                //        this.bondModuleController.ConvertG0ToMachinePos(this.bmcToolVisionPos);

                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.upLookController.ConvertPixelToG0Pos(visionMachinePos, bMCToolMatchResult1);

                //    bMCToolMatchResult1 =
                //        (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, "圆搜索", this.visionTime,
                //            true);
                //}

                #endregion

                #region 取小标定片

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

                if (this.isMoveToCameraCenterAfterVision)
                {
                    // 移动到相机中心再拍一次
                    newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

                    matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                        newVisionPos,
                        pRName,
                        this.visionTime, true);
                }

                // 计算取片位(G0)
                AKRSPoint3D pickPos = this.system2Controller.GetBondVisionResultPos(matchResult1)
                                      + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                // 移动到取片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                // 取放片参数
                BaseCarrierConfig component = new()
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
                    WeakBlowProportion = 1000
                };
                double pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 关闭吸嘴真空
                this.bondHeadController.CloseToolVaccum();

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                #endregion

                // 计算上视拍照位
                AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

                AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                // 循环测试
                for (int i = 1; i <= this.cycles; i++)
                {
                    this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    #region 上视定位

                    // P1定位
                    upLookRes1 = (MatchResult)this.system2Controller.UpLookCameraVision(
                        upLookVisionPos1InG0,
                        CalibrateRunPara.GetInstance().UpLookPRName,
                        this.visionTime);

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.upLookController.ConvertPixelToG0Pos(upLookVisionPos1, upLookRes1);

                        upLookRes1 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true, dataLog,"上视定位M1");
                    }

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

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.upLookController.ConvertPixelToG0Pos(upLookVisionPos2, upLookRes2);

                        upLookRes2 =
                            (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true, dataLog,"上视定位M2");
                    }

                    AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                        this.bondModuleController.Get3DRealPosition(),
                        upLookRes2);

                    uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                    #endregion

                    #region 上视纠偏计算

                    // 计算角度
                    // double uplookOffSetAngle = CalibService.CalculateAngle(
                    //     new(point3D1.X, point3D1.Y),
                    //     new(point3D2.X, point3D2.Y));

                    // double placeAngle = uplookOffSetAngle - 45;

                    // 小标定片中心
                    AKRSPoint3D glassUpLookCenter = (point3D1 + point3D2) / 2;

                    // 焊头旋转中心
                    AKRSPoint3D bondHeadCenter =
                        this.bondModuleController.ConvertMachineToG0Pos(
                            BondDevicePara.GetInstance().CameraDevicePara.UpLookPos);

                    // 相对旋转中心的偏移
                    AKRSPoint2D offsetCenter = new()
                    {
                        X = glassUpLookCenter.X - bondHeadCenter.X,
                        Y = glassUpLookCenter.Y - bondHeadCenter.Y
                    };

                    // 旋转之后的偏移
                    // AKRSPoint2D offsetAfterRotate = this.bondHeadController.GetNozzleOffset(offsetCenter, placeAngle);

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

                                goto RetryBMCP1;

                            case DialogResult.Abort:

                                throw new Exception($"{CalibrateRunPara.GetInstance().BmcPRName}  adjust  failed!");

                            case DialogResult.Ignore:
                                matchResult2 = new();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                            CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, matchResult2);

                        matchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true, dataLog, "下视定位BMC M1");
                    }


                    AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

                    downlookP1ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(p1VisionPos);

                    RetryBMCP2:
                    // P2定位
                    matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
                        p2MarkPos,
                        CalibrateRunPara.GetInstance().BmcPRName,
                        this.visionTime, true);

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
                                matchResult3 = new();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                            CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, matchResult3);

                        matchResult3 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true, dataLog, "下视定位BMC M2");
                    }

                    AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);


                    downlookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(p2VisionPos);

                    #endregion

                    #region 计算贴片位

                    // 计算放片位
                    AKRSPoint3D placePos = (p1VisionPos + p2VisionPos) / 2
                                           + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                    // 最终贴片位，暂时不动态补偿
                    AKRSPoint3D finalPlacePos = new()
                    {
                        X = placePos.X /*+ offsetAfterRotate.X*/,
                        Y = placePos.Y /*+ offsetAfterRotate.Y*/,
                       Z=  this.bondModuleController.GetG0RealPosition().Z + 3
                };

                    #endregion

                    #region 贴片

                    // 运动到放片位
                    this.bondModuleController.MoveToG0Pos(finalPlacePos);

                    // 放片参数,标定片厚度：2
                    double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                    // 放片
                    ReBond:
                    bool ret = this.bondHeadController.BondAction(
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
                                matchResult2 = new();
                                break;
                        }

                        return false;
                    }

                    #endregion

                    #region 小标定片拍照

                    // P1定位
                    AKRSPoint3D postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                    postBondP1Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                    this.bondHeadController.MoveBondZToSafePos();

                    RetryPostBondGlassP1:
                    // 小标定片P1定位
                    glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP1Pos,
                        CalibrateRunPara.GetInstance().GlassPRName);

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
                                glassMatchResult1 = new();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                            CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, glassMatchResult1);

                        glassMatchResult1 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true, dataLog, "下视定位标定片M1");

                        //AKRSPoint2D axisM1Pos = CalibService.GetMachinePosByPixelPos(
                        //    this.bondModuleController.Get2DRealPosition(),
                        //    glassMatchResult1,
                        //    "BondCameraCoordinateSystem");

                        //newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                        //    CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, glassMatchResult1);

                        //glassMatchResult1 =
                        //    (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                        //        CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true);
                    }


                    glassVisionPos1 = this.system2Controller.GetBondVisionResultPos(glassMatchResult1);

                    AKRSPoint3D postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                    postBondP2Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                    RetryPostBondGlassP2:

                    // 小标定片P2定位
                    glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
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
                                glassMatchResult2 = new();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                            CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, glassMatchResult2);

                        glassMatchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                                CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true, dataLog, "下视定位标定片M2");

                        // AKRSPoint2D axisM2Pos = CalibService.GetMachinePosByPixelPos(
                        //     this.bondModuleController.Get2DRealPosition(),
                        //     glassMatchResult2,
                        //     "BondCameraCoordinateSystem");
                        //
                        // newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(
                        //     CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, glassMatchResult2);
                        //
                        // glassMatchResult2 =
                        //     (MatchResult)this.system2Controller.BondCameraVision(newVisionPos,
                        //         CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true);
                    }

                    dataLog.CommitData();
                    glassVisionPos2 = this.system2Controller.GetBondVisionResultPos(glassMatchResult2);

                    // 计算小标定片中心点
                    AKRSPoint3D glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
                                                 + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                    //double glassAngle = CalibService.CalculateAngle(
                    //    new AKRSPoint2D(glassVisionPos1.X, glassVisionPos1.Y),
                    //    new AKRSPoint2D(glassVisionPos2.X, glassVisionPos2.Y));

                    glassCenterPos.Z = this.bondModuleController.GetG0RealPosition().Z + 3;

                    #endregion

                    #region 取片

                    // 运动到取片位
                    this.bondModuleController.MoveToG0Pos(glassCenterPos);

                    // 计算参数
                    pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                    // 取片
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                    this.SaveCurrentData(null, upLookRes1, upLookRes2, uplookP1ResInAxis,
                        uplookP2ResInAxis, glassMatchResult1, glassMatchResult2, glassVisionPos1, glassVisionPos2, null,
                        upLookVisionPos1, upLookVisionPos2, offsetCenter,downlookP1ResInAxis, downlookP2ResInAxis);

                    #endregion
                }

                #region 放回小标定片

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                // 放片
                this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                #endregion

                #region BMC治具中心定位

                //// 位置待示教
                //MatchResult bMCToolMatchResult2 =
                //    (MatchResult)this.system2Controller.UpLookCameraVision(this.bmcToolVisionPos, "圆搜索");

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    AKRSPoint3D visionMachinePos =
                //        this.bondModuleController.ConvertG0ToMachinePos(this.bmcToolVisionPos);

                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.upLookController.ConvertPixelToG0Pos(visionMachinePos, bMCToolMatchResult2);

                //    bMCToolMatchResult2 =
                //        (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, "圆搜索", true);
                //}

                #endregion

                //this.SaveCurrentData(null, null, null, null, null, null, null, null, null, bMCToolMatchResult2, null,
                //    null, null, null, null);

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
                        this.BtnStart.Enabled = true;
                        this.BtnStart.BackColor = default;
                    });


                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"取片实验结束!",
                    "信息",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return true;
        }

        /// <summary>
        /// 保存定位结果
        /// </summary>
        private void SaveCurrentData(MatchResult bmcToolMatchResultBefore, MatchResult uplookP1MatchRes, MatchResult uplookP2MatchRes, AKRSPoint3D upLookP1AxisRes, AKRSPoint3D upLookP2AxisRes, MatchResult downLookP1MatchRes, MatchResult downLookP2MatchRes, AKRSPoint3D downLookP1AxisRes, AKRSPoint3D downLookP2AxisRes, MatchResult bmcToolMatchResultAfter,AKRSPoint3D uplookP1VisionPos, AKRSPoint3D uplookP2VisionPos,AKRSPoint2D offsetCenter, AKRSPoint3D downLookBMCM1Pos, AKRSPoint3D downLookBMCM2Pos)
        {
            string fileName = $"D:\\精度实验2025\\取片误差验证实验{DateTime.Now:yyyyMMdd}";

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

                worksheet.Cells[1, 2].Value = "开始时BMC治具中心定位结果-X";

                worksheet.Cells[1, 3].Value = "开始时BMC治具中心定位结果-Y";

                worksheet.Cells[1, 4].Value = "Angle03";

                worksheet.Cells[1, 5].Value = "上视P1定位结果-X";

                worksheet.Cells[1, 6].Value = "上视P1定位结果-Y";

                worksheet.Cells[1, 7].Value = "Angle02";

                worksheet.Cells[1, 8].Value = "上视P2定位结果-X";

                worksheet.Cells[1, 9].Value = "上视P2定位结果-Y";

                worksheet.Cells[1, 10].Value = "Angle01";

                worksheet.Cells[1, 11].Value = "上视P1定位结果转到轴坐标-X";

                worksheet.Cells[1, 12].Value = "上视P1定位结果转到轴坐标-Y";

                worksheet.Cells[1, 13].Value = "上视P1定位结果转到轴坐标-Z";

                worksheet.Cells[1, 14].Value = "上视P2定位结果转到轴坐标-X";

                worksheet.Cells[1, 15].Value = "上视P2定位结果转到轴坐标-Y";

                worksheet.Cells[1, 16].Value = "上视P2定位结果转到轴坐标-Z";

                worksheet.Cells[1, 17].Value = "下视P1定位结果-X";

                worksheet.Cells[1, 18].Value = "下视P1定位结果-Y";

                worksheet.Cells[1, 19].Value = "Angle0";

                worksheet.Cells[1, 20].Value = "下视P2定位结果-X";

                worksheet.Cells[1, 21].Value = "下视P2定位结果-Y";

                worksheet.Cells[1, 22].Value = "Angle056";

                worksheet.Cells[1, 23].Value = "下视P1定位结果转到轴坐标-X";

                worksheet.Cells[1, 24].Value = "下视P1定位结果转到轴坐标-Y";

                worksheet.Cells[1, 25].Value = "下视P1定位结果转到轴坐标-Z";

                worksheet.Cells[1, 26].Value = "下视P2定位结果转到轴坐标-X";

                worksheet.Cells[1, 27].Value = "下视P2定位结果转到轴坐标-Y";

                worksheet.Cells[1, 28].Value = "下视P2定位结果转到轴坐标-Z";

                worksheet.Cells[1, 29].Value = "结束时BMC治具中心定位结果-X";

                worksheet.Cells[1, 30].Value = "结束时BMC治具中心定位结果-Y";

                worksheet.Cells[1, 31].Value = "Angle045";

                worksheet.Cells[1, 32].Value = "上视P1拍照位轴坐标-X";

                worksheet.Cells[1, 33].Value = "上视P1拍照位轴坐标-Y";

                worksheet.Cells[1, 34].Value = "上视P1拍照位轴坐标-Z";

                worksheet.Cells[1, 35].Value = "上视P2拍照位轴坐标-X";

                worksheet.Cells[1, 36].Value = "上视P2拍照位轴坐标-Y";

                worksheet.Cells[1, 37].Value = "上视P2拍照位轴坐标-Z";

                worksheet.Cells[1, 38].Value = "下视P1拍照位轴坐标-X";

                worksheet.Cells[1, 39].Value = "下视P1拍照位轴坐标-Y";

                worksheet.Cells[1, 40].Value = "下视P1拍照位轴坐标-Z";

                worksheet.Cells[1, 41].Value = "下视P2拍照位轴坐标-X";

                worksheet.Cells[1, 42].Value = "下视P2拍照位轴坐标-Y";

                worksheet.Cells[1, 43].Value = "下视P2拍照位轴坐标-Z";

                worksheet.Cells[1, 44].Value = "标定片中心和吸嘴中心的偏移-X";

                worksheet.Cells[1, 45].Value = "标定片中心和吸嘴中心的偏移-Y";

                worksheet.Cells[1, 46].Value = "BMC M1 拍摄轴位置-X";

                worksheet.Cells[1, 47].Value = "BMC M1 拍摄轴位置-Y";

                worksheet.Cells[1, 48].Value = "BMC M2 拍摄轴位置-X";

                worksheet.Cells[1, 49].Value = "BMC M2 拍摄轴位置-Y";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = bmcToolMatchResultBefore?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = bmcToolMatchResultBefore?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 4].Value = bmcToolMatchResultBefore?.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = uplookP1MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 6].Value = uplookP1MatchRes?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 7].Value = uplookP1MatchRes?.Angle;

            worksheet.Cells[lastUsedRow + 1, 8].Value = uplookP2MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 9].Value = uplookP2MatchRes?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 10].Value = uplookP2MatchRes?.Angle;

            worksheet.Cells[lastUsedRow + 1, 11].Value = upLookP1AxisRes?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 12].Value = upLookP1AxisRes?.Y * 1000;

            worksheet.Cells[lastUsedRow + 1, 13].Value = upLookP1AxisRes?.Z * 1000;

            worksheet.Cells[lastUsedRow + 1, 14].Value = upLookP2AxisRes?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 15].Value = upLookP2AxisRes?.Y * 1000;

            worksheet.Cells[lastUsedRow + 1, 16].Value = upLookP2AxisRes?.Z * 1000;

            worksheet.Cells[lastUsedRow + 1, 17].Value = downLookP1MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 18].Value = downLookP1MatchRes?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 19].Value = downLookP1MatchRes?.Angle;

            worksheet.Cells[lastUsedRow + 1, 20].Value = downLookP2MatchRes?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 21].Value = downLookP2MatchRes?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 22].Value = downLookP2MatchRes?.Angle;

            worksheet.Cells[lastUsedRow + 1, 23].Value = downLookP1AxisRes?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 24].Value = downLookP1AxisRes?.Y * 1000;

            // worksheet.Cells[lastUsedRow + 1, 25].Value = downLookP1AxisRes?.Z * 1000;

            worksheet.Cells[lastUsedRow + 1, 26].Value = downLookP2AxisRes?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 27].Value = downLookP2AxisRes?.Y * 1000;

            // worksheet.Cells[lastUsedRow + 1, 28].Value = downLookP2AxisRes?.Z * 1000;

            worksheet.Cells[lastUsedRow + 1, 29].Value = bmcToolMatchResultAfter?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 30].Value = bmcToolMatchResultAfter?.CenterY;

            // worksheet.Cells[lastUsedRow + 1, 31].Value = bmcToolMatchResultAfter?.Angle;

            worksheet.Cells[lastUsedRow + 1, 32].Value = uplookP1VisionPos?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 33].Value = uplookP1VisionPos?.Y * 1000;

            // worksheet.Cells[lastUsedRow + 1, 34].Value = uplookP1VisionPos?.Z * 1000;

            worksheet.Cells[lastUsedRow + 1, 35].Value = uplookP2VisionPos?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 36].Value = uplookP2VisionPos?.Y * 1000;

            // worksheet.Cells[lastUsedRow + 1, 37].Value = uplookP2VisionPos?.Z * 1000;


            worksheet.Cells[lastUsedRow + 1, 38].Value = CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos?.X;

            worksheet.Cells[lastUsedRow + 1, 39].Value = CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos?.Y;

            // worksheet.Cells[lastUsedRow + 1, 40].Value = CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos?.Z;

            worksheet.Cells[lastUsedRow + 1, 41].Value = CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos?.X;

            worksheet.Cells[lastUsedRow + 1, 42].Value = CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos?.Y;

            // worksheet.Cells[lastUsedRow + 1, 43].Value = CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos?.Z;


            worksheet.Cells[lastUsedRow + 1, 44].Value = offsetCenter?.X * 1000;
            worksheet.Cells[lastUsedRow + 1, 45].Value = offsetCenter?.Y * 1000;

            worksheet.Cells[1, 46].Value = downLookBMCM1Pos.X * 1000;

            worksheet.Cells[1, 47].Value = downLookBMCM1Pos.Y * 1000;

            worksheet.Cells[1, 48].Value = downLookBMCM2Pos.X * 1000;

            worksheet.Cells[1, 49].Value = downLookBMCM2Pos.Y * 1000;

            excelPackage.Save();
        }


    }
}