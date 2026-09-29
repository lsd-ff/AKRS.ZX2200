using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Experiment.Test;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using DataAnalysis.Acquisition;
using DevExpress.XtraEditors;
using OfficeOpenXml;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    /// <summary>
    /// 模拟贴片实验
    /// </summary>
    public partial class FrmSimulateBondTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmSimulateBondTest()
        {
            InitializeComponent();
            this.InitControl();
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

        /// <summary>
        ///  槽里面的取片位
        /// </summary>
        private AKRSPoint3D pickPos = default;

        internal AKRSPoint2D OriginPoint
        {
            get
            {
                return new AKRSPoint2D(Convert.ToDouble(this.SpOriginX.Value), Convert.ToDouble(this.SpOriginY.Value));
            }
        }


        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 测试结束标志
        /// </summary>
        private bool testFinished = false;

        private double uplookAngleLimit;

        /// <summary>
        /// 补偿X
        /// </summary>
        private double compensateX
        {
            get
            {
                this.bMCDevicePara.CompensateX = (double)this.SpCompensateX.Value;
                return (double)this.SpCompensateX.Value;
            }
        }

        /// <summary>
        /// 补偿Y
        /// </summary>
        private double compensateY
        {
            get
            {
                this.bMCDevicePara.CompensateY = (double)this.SpCompensateY.Value;
                return (double)this.SpCompensateY.Value;
            }
        }

        /// <summary>
        /// 补偿角度
        /// </summary>
        private double compensateAngle
        {
            get
            {
                this.bMCDevicePara.CompensateAngle = (double)this.SpCompensateAngle.Value;
                return (double)this.SpCompensateAngle.Value;
            }
        }

        /// <summary>
        /// BMC测试线程
        /// </summary>
        private Task bondTestTask;

        /// <summary>
        ///  控件初始化
        /// </summary>
        private void InitControl()
        {
            // 下拉框绑定
            this.LueOriginAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<TemperatrueDriftCompensateEnum>();
            this.LueOriginAdjustType.EditValue = TemperatrueDriftCompensateEnum.BMC;

            this.SpUplookAngleLimit.Properties.MinValue = 0;
            this.SpUplookAngleLimit.Properties.IsFloatValue = true;
        }

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

            this.uplookAngleLimit = (double)this.SpUplookAngleLimit.Value;

            this.isMoveToCameraCenterAfterVision = this.ChkMoveToCameraCenterAfterVision.Checked;

            BondDevicePara.GetInstance().Save();

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
            if (this.bondTestTask == null
                || this.bondTestTask.Status != TaskStatus.Running)
            {
                this.BtnStart.Enabled = false;
                this.BtnStart.BackColor = Color.Yellow;
                this.bondTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("模拟贴片实验线程");
                            this.BondTest();
                        });
            }

            return true;
        }


        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool BondTest()
        {
            string fileName = $"D:\\精度实验2025\\模拟贴片验证实验{DateTime.Now:HHmmss}";
            IDataLog dataLog = DataLogManager.Instance.CreateDataLog($"模拟贴片验证实验{DateTime.Now:HHmmss}").ConfigureFileOutput($"{fileName}.json");

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

                AKRSPoint3D glassVisionPos1 = default;
                AKRSPoint3D glassVisionPos2 = default;

                // 小标定片P2定位
                MatchResult glassMatchResult1 = default;
                MatchResult glassMatchResult2 = default;


                // 计算上视拍照位
                AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);
                AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                AKRSPoint3D glassCenterPos = default;

                AKRSPoint3D offsetVec = new AKRSPoint3D();

                // 计算参数
                double pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
                double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

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

                this.bondHeadController.MoveBondZToSafePos();

                // 准备动作
                if (this.Prepare() == false)
                {
                    return false;
                }


                // 循环测试
                for (int i = 1; i <= this.cycles; i++)
                {
                    this.Invoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                    #region 小标定片拍照

                    // P1定位
                    AKRSPoint3D postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                    postBondP1Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondGlassP1:
                    // 小标定片P1定位
                    glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP1Pos,
                        CalibrateRunPara.GetInstance().GlassPRName, this.visionTime,
                        i != 1);

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

                        glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                            newVisionPos,
                            CalibrateRunPara.GetInstance().GlassPRName,
                            this.visionTime,
                            true);
                    }

                    glassVisionPos1 = this.system2Controller.GetBondVisionResultPos(glassMatchResult1);

                    dataLog.AddData($"取片前下视定位标定片M1结果", new { X = glassMatchResult1.CenterX, Y = glassMatchResult1.CenterY });
                    //dataLog.AddData($"取片前下视定位标定片M1结果-转到G0坐标", new { X = glassVisionPos1.X, Y = glassVisionPos1.Y });

                    AKRSPoint3D postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                        CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                    postBondP2Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondGlassP2:

                    // 小标定片P2定位
                    glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP2Pos,
                        CalibrateRunPara.GetInstance().GlassPRName, this.visionTime,
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

                        glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                            newVisionPos,
                            CalibrateRunPara.GetInstance().GlassPRName,
                            this.visionTime,
                            true);
                    }

                    glassVisionPos2 = this.system2Controller.GetBondVisionResultPos(glassMatchResult2);

                    #endregion

                    #region 取片

                    dataLog.AddData($"取片前下视定位标定片M2结果", new { X = glassMatchResult1.CenterX, Y = glassMatchResult1.CenterY });
                    //dataLog.AddData($"取片前下视定位标定片M2结果-转到G0坐标", new { X = glassVisionPos2.X, Y = glassVisionPos2.Y });

                    dataLog.AddData($"下视标定片中点", new { X = (glassVisionPos1.X+ glassVisionPos2.X)/2, Y = (glassVisionPos1.Y + glassVisionPos2.Y) / 2 });

                    var pickOffset = ((offsetVec.X == 0) && (offsetVec.Y == 0)) ? BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset : BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset - offsetVec;

                    // 计算小标定片中心点
                    glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
                                                + pickOffset;
                    //double glassAngle = CalibService.CalculateAngle(
                    //    new AKRSPoint2D(glassVisionPos1.X, glassVisionPos1.Y),
                    //    new AKRSPoint2D(glassVisionPos2.X, glassVisionPos2.Y));

                    glassCenterPos.Z = this.bondModuleController.GetG0RealPosition().Z + 3;

                    // 运动到取片位
                    this.bondModuleController.MoveToG0Pos(glassCenterPos);
                    this.bondHeadController.RotateAxisT(0);

                    // 取片
                    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

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
                                matchResult2 = new MatchResult();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, matchResult2);

                        matchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }


                    AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

                    dataLog.AddData($"取片后下视定位BMC M1结果", new { X = matchResult2.CenterX, Y = matchResult2.CenterY });
                    //dataLog.AddData($"取片后下视定位BMC M1结果-转到G0坐标", new { X = p1VisionPos.X, Y = p1VisionPos.Y });

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
                                matchResult3 = new MatchResult();
                                break;
                        }
                    }

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, matchResult3);

                        matchResult3 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }

                    AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);

                    // BMC角度
                    double bmcAngle = CalibService.CalculateAngle(
                        new AKRSPoint2D(p2VisionPos.X, p2VisionPos.Y),
                        new AKRSPoint2D(p1VisionPos.X, p1VisionPos.Y));

                    dataLog.AddData($"取片后下视定位BMC M2结果", new { X = matchResult3.CenterX, Y = matchResult3.CenterY });
                    //dataLog.AddData($"取片后下视定位BMC M2结果-转到G0坐标", new { X = p2VisionPos.X, Y = p2VisionPos.Y });
                    dataLog.AddData($"取片后下视定位BMC中点", new { X =(p1VisionPos.X+ p2VisionPos.X)/2 , Y = (p1VisionPos.Y
                       + p2VisionPos.Y) / 2 });

                    dataLog.AddData($"取片后BMC M1->M2角度", bmcAngle);
                    #endregion

                    #region 上视温漂定位

                    MatchResult driftMatchRes = (MatchResult)this.system2Controller.UpLookCameraVision(
                        upLookVisionPos1InG0,
                        "上视温漂测试Mark",
                        this.visionTime);

                    AKRSPoint3D driftRes = this.system2Controller.GetUplookVisionResultPos(driftMatchRes);

                    dataLog.AddData($"上视温漂Mark定位结果-转到G0坐标", new { X = driftRes.X, Y = driftRes.Y });

                    #endregion

                    #region 上视纠偏
                    int upLookVisionCount = 0;
                    while (upLookVisionCount < 10)
                    {
                        upLookVisionCount++;

                        #region 上视定位

                        // P1定位
                        upLookRes1 = (MatchResult)this.system2Controller.UpLookCameraVision(
                            upLookVisionPos1InG0,
                            CalibrateRunPara.GetInstance().UpLookPRName,
                            this.visionTime,
                            upLookVisionCount != 1);

                        if (this.isMoveToCameraCenterAfterVision)
                        {
                            // 移动到相机中心再拍一次
                            newVisionPos = this.upLookController.ConvertPixelToG0Pos(upLookVisionPos1, upLookRes1);

                            upLookRes1 =
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true);
                        }

                        AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes1);

                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次定位标定片 M1结果", new { X = upLookRes1.CenterX, Y = upLookRes1.CenterY });
                        //dataLog.AddData($"上视角度矫正第{upLookVisionCount}次定位标定片 M1结果-转到G0坐标", new { X = point3D1.X, Y = point3D1.Y });

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
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true);
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次定位标定片 M2结果", new { X = upLookRes2.CenterX, Y = upLookRes2.CenterY });
                        //dataLog.AddData($"上视角度矫正第{upLookVisionCount}次定位标定片 M2结果-转到G0坐标", new { X = point3D2.X, Y = point3D2.Y });

                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次上视标定片中点", new { X =(point3D1.X+ point3D2.X)/2 , Y = (point3D1.Y + point3D2.Y) / 2 });

                        #endregion

                        // 计算角度
                        double uplookOffSetAngle = CalibService.CalculateAngle(
                            new AKRSPoint2D(point3D1.X, point3D1.Y),
                            new AKRSPoint2D(point3D2.X, point3D2.Y));

                        double placeAngle = uplookOffSetAngle - bmcAngle;

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

                        //// 旋转之后的偏移
                        //AKRSPoint2D offsetAfterRotate = this.bondHeadController.GetNozzleOffset(offsetCenter, placeAngle);

                        // 贴片的角度
                        double bondAngle = this.bondHeadController.GetAxisTRealPos() - placeAngle;

                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次  上视定位标定片和吸嘴中心的偏移", new { X = offsetCenter.X, Y = offsetCenter.Y });

                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次  标定片M1->M2角度", uplookOffSetAngle);
                        dataLog.AddData($"上视角度矫正第{upLookVisionCount}次  旋转角度", -placeAngle);

                        if (Math.Abs(placeAngle) < this.uplookAngleLimit)
                        {
                            break;
                        }

                        // 提前转
                        this.bondHeadController.RelativeRotateAxisT(-placeAngle);
                    }

                    #endregion

                    #region 计算贴片位

                    var midPoint = (p1VisionPos + p2VisionPos) / 2;
                    //var placeOffset = this.UpdateCalibData(new AKRSPoint2D(midPoint.X, midPoint.Y));
                    var cameraOffset = this.CalcCameraOffset(new AKRSPoint2D(midPoint.X, midPoint.Y));
                    var placeOffset = CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset + cameraOffset;
                    offsetVec = cameraOffset;

                    dataLog.AddData($"温漂补偿偏移量", new { X = cameraOffset.X, Y = cameraOffset.Y });
                    dataLog.AddData($"BMC中点位置", new { X = midPoint.X, Y = midPoint.Y });
                    // 计算放片位
                    AKRSPoint3D placePos = midPoint
                                           + placeOffset;
                    // 最终贴片位，暂时不动态补偿
                    AKRSPoint3D finalPlacePos = new()
                    {
                        X = placePos.X + this.compensateX  /*+ offsetAfterRotate.X*/,
                        Y = placePos.Y + this.compensateY /*+ offsetAfterRotate.Y*/,
                        Z = this.bondModuleController.GetG0RealPosition().Z + 3
                    };

                    dataLog.AddData($"最终贴片位置", new { X = finalPlacePos.X, Y = finalPlacePos.Y });

                    #endregion

                    #region 贴片

                    // 运动到放片位
                    this.bondModuleController.MoveToG0Pos(finalPlacePos);

                    this.bondHeadController.RelativeRotateAxisT(this.compensateAngle);

                    // 放片参数,标定片厚度：2
                    double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                // 放片
                ReBond:
                    bool ret = this.bondHeadController.BondAction(
                        placeLevel,
                        liftLevel,
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

                    #region 焊后检测

                    // 大标定片P1定位
                    postBondP1Pos = this.bondModuleController.ConvertMachineToG0Pos(
                       CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos);
                    postBondP1Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondBMCP1:
                    MatchResult bMCMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP1Pos,
                        CalibrateRunPara.GetInstance().BmcPRName,
                        this.visionTime, false);

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
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, bMCMatchResult1);

                        bMCMatchResult1 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }


                    AKRSPoint3D bMCVisionPos1 = this.system2Controller.GetBondVisionResultPos(bMCMatchResult1);

                    dataLog.AddData($"焊后检测BMC  M1定位", new { X = bMCMatchResult1.CenterX, Y = bMCMatchResult1.CenterY });

                RetryGlassP1:
                    // 小标定片P1定位
                    glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                       postBondP1Pos,
                       CalibrateRunPara.GetInstance().GlassPRName, this.visionTime,
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

                                goto RetryGlassP1;

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
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, glassMatchResult1);

                        glassMatchResult1 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true);
                    }


                    dataLog.AddData($"焊后检测标定片  M1定位", new { X = glassMatchResult1.CenterX, Y = glassMatchResult1.CenterY });

                    glassVisionPos1 = this.system2Controller.GetBondVisionResultPos(glassMatchResult1);

                    // 大标定片P2定位
                    postBondP2Pos = this.bondModuleController.ConvertMachineToG0Pos(
                       CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos);
                    postBondP2Pos.Z = this.bondModuleController
                        .ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos).Z;

                RetryPostBondBMCP2:
                    MatchResult bMCMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                        postBondP2Pos,
                        CalibrateRunPara.GetInstance().BmcPRName, this.visionTime,
                        true);

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
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, bMCMatchResult2);

                        bMCMatchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }

                    AKRSPoint3D bMCVisionPos2 = this.system2Controller.GetBondVisionResultPos(bMCMatchResult2);

                    dataLog.AddData($"焊后检测BMC  M2定位", new { X = bMCMatchResult2.CenterX, Y = bMCMatchResult2.CenterY });
                    dataLog.AddData($"焊后检测BMC中点", new { X = (bMCVisionPos1.X+ bMCVisionPos2.X)/2, Y = (bMCVisionPos1.Y + bMCVisionPos2.Y) / 2 });

                RetryGlassP2:

                    // 小标定片P2定位
                    glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                       postBondP2Pos,
                       CalibrateRunPara.GetInstance().GlassPRName, this.visionTime,
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

                                goto RetryGlassP2;

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
                        newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, glassMatchResult2);

                        glassMatchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().GlassPRName, this.visionTime, true);
                    }

                    glassVisionPos2 = this.system2Controller.GetBondVisionResultPos(glassMatchResult2);

                    dataLog.AddData($"焊后检测标定片  M2定位", new { X = glassMatchResult2.CenterX, Y = glassMatchResult2.CenterY });
                    dataLog.AddData($"焊后检测标定片中点", new { X = (glassVisionPos1.X+ glassVisionPos2.X)/2, Y = (glassVisionPos1.Y + glassVisionPos2.Y) / 2 });

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

                    // 保存结果
                    this.bMCTestResultList.Add(
                         new BMCTestResult()
                         {
                             Index = i,
                             XResult = postBondRes.X * 1000.0,
                             YResult = postBondRes.Y * 1000.0,
                             AngleResult = postBondAngle,
                             CreateTime = DateTime.Now
                         });

                    dataLog.AddData($"焊后检测结果", new { X = postBondRes.X * 1000, Y = postBondRes.Y * 1000, Angle = postBondAngle });

                    #endregion

                    dataLog.CommitData();

                    if (i % 5 == 0 && i > 1)
                    {
                        dataLog.Flush();
                    }
                }

                #region 取片

                glassCenterPos.Z = this.bondModuleController.GetG0RealPosition().Z + 5;

                // 运动到取片位
                this.bondModuleController.MoveToG0Pos(glassCenterPos);
                this.bondHeadController.RotateAxisT(0);

                // 取片
                this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.BMC);

                #endregion

                #region 放回小标定片

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos);

                double pickLevelInSlot = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;
                double liftLevelInSlot = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // 放片
                this.bondHeadController.BondAction(pickLevelInSlot, liftLevelInSlot, component, BondTypeEnum.BondOnTU);

                #endregion

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"贴片实验失败:{e.ToString()}!",
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
                    $"贴片实验结束!",
                    "信息",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            return true;
        }

        /// <summary>
        /// 准备动作
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool Prepare()
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

            if (this.isMoveToCameraCenterAfterVision)
            {
                // 移动到相机中心再拍一次
                newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

                matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                    glassCenterPositionInG0,
                    pRName,
                    this.visionTime, true);
            }

            // 计算取片位(G0)
            pickPos = this.system2Controller.GetBondVisionResultPos(matchResult1)
                                 + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            pickPos.Z = this.bondModuleController.GetG0RealPosition().Z + 3;

            // 移动到取片位
            this.bondModuleController.MoveToG0Pos(pickPos);

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

            // 计算上视拍照位
            AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
            AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
            AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

            AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
            AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

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
                    (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true);
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
                    (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime, true);
            }

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
                        matchResult2 = new MatchResult();
                        break;
                }
            }

            if (this.isMoveToCameraCenterAfterVision)
            {
                // 移动到相机中心再拍一次
                newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, matchResult2);

                matchResult2 =
                    (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
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
                        matchResult3 = new MatchResult();
                        break;
                }
            }

            if (this.isMoveToCameraCenterAfterVision)
            {
                // 移动到相机中心再拍一次
                newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, matchResult3);

                matchResult3 =
                    (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
            }

            AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);


            downlookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(p2VisionPos);

            #endregion

            #region 计算贴片位


            var midPoint = (p1VisionPos + p2VisionPos) / 2;
            //this.UpdateCalibData(new AKRSPoint2D(midPoint.X, midPoint.Y));

            // 计算放片位
            AKRSPoint3D placePos = midPoint
                                   + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            // 最终贴片位，暂时不动态补偿
            AKRSPoint3D finalPlacePos = new()
            {
                X = placePos.X /*+ offsetAfterRotate.X*/,
                Y = placePos.Y /*+ offsetAfterRotate.Y*/,
                Z = this.bondModuleController.GetG0RealPosition().Z + 3
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
                liftLevel,
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

            return true;
        }

        private void ChkEnableEditor_CheckedChanged(object sender, EventArgs e)
        {
            this.SpCompensateX.Enabled = this.ChkEnableEditor.Checked;
            this.SpCompensateY.Enabled = this.ChkEnableEditor.Checked;
            this.SpCompensateAngle.Enabled = this.ChkEnableEditor.Checked;
        }

        private void FrmSimulateBondTest_Load(object sender, EventArgs e)
        {
            ChkEnableEditor_CheckedChanged(null, null);
        }

        private void BtnSetOrigin_Click(object sender, EventArgs e)
        {
            AKRSPoint3D midPos = default;

            sender.UIActionAsync(() =>
            {
                if ((TemperatrueDriftCompensateEnum)this.LueOriginAdjustType.EditValue == TemperatrueDriftCompensateEnum.BMC)
                {
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
                    var matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                          p1MarkPos,
                          CalibrateRunPara.GetInstance().BmcPRName,
                          this.visionTime);

                    if (matchResult2 == null)
                    {
                        var dialog = AKRSMessageBoxExt.Show(
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

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        var newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkTopLeftVisionMachinePos, matchResult2);

                        matchResult2 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }


                    AKRSPoint3D p1VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult2);

                    RetryBMCP2:
                    // P2定位
                    var matchResult3 = (MatchResult)this.system2Controller.BondCameraVision(
                       p2MarkPos,
                       CalibrateRunPara.GetInstance().BmcPRName,
                       this.visionTime, true);

                    if (matchResult3 == null)
                    {
                        var dialog = AKRSMessageBoxExt.Show(
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

                    if (this.isMoveToCameraCenterAfterVision)
                    {
                        // 移动到相机中心再拍一次
                        var newVisionPos = this.bondModuleController.ConvertPixelToG0Pos(CalibrateRunPara.GetInstance().BMCMarkBotRightVisionMachinePos, matchResult3);

                        matchResult3 =
                            (MatchResult)this.system2Controller.BondCameraVision(newVisionPos, CalibrateRunPara.GetInstance().BmcPRName, this.visionTime, true);
                    }

                    AKRSPoint3D p2VisionPos = this.system2Controller.GetBondVisionResultPos(matchResult3);

                    midPos = (p1VisionPos + p2VisionPos) / 2
                        /*     + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset*/;

                }
                else
                {
                    #region 上视温漂测试
                    {
                        {
                            this.bondModuleController.MoveSafeBondXYZ
                            (
                                BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);

                            // 寻找Pr模板
                            PREntity driftMarkEntity =
                                (PREntity)VisionEntityRepository.GetInstance().Find("上视温漂测试Mark");

                        RetryUplookMark:
                            // 定位
                            ExcuteResult driftTestResult = driftMarkEntity.DoWork();

                            // 温漂定位结果
                            MatchResult driftTestMatchResult = (MatchResult)driftMarkEntity.AlgResult;

                            if (driftTestMatchResult == null)
                            {
                             DialogResult   dialog = AKRSMessageBoxExt.Show(
                                    $"Downlook  {CalibrateRunPara.GetInstance().BmcPRName}  P1 adjust failed!\r\nRetry:retry point  adjust\r\nAbort: Exit program\r\n Ignore: Ignore this failure.\r\n",
                                    "Alarm",
                                    new string[] { "Retry", "Abort" },
                                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                                    AlarmLevel.SecondLevel);

                                switch (dialog)
                                {
                                    case DialogResult.Retry:

                                        goto RetryUplookMark;

                                    case DialogResult.Abort:

                                        throw new Exception($"上视温漂测试Mark  adjust  failed!");
                                }
                            }

                            midPos = this.system2Controller.GetUplookVisionResultPos(driftTestMatchResult);

                        }
                    }

                    #endregion
                }

                this.Invoke((MethodInvoker)(() =>
                                                   {
                                                       this.SpOriginX.EditValue = midPos.X;
                                                       this.SpOriginY.EditValue = midPos.Y;
                                                   }));
                this.bondModuleController.MoveToSafePos();

            });
        }

        private AKRSPoint3D UpdateCalibData(AKRSPoint2D realtimeMidPoint)
        {

            var originOffsetVec = realtimeMidPoint - this.OriginPoint;
            var offsetX = CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X - originOffsetVec.X;
            var offsetY = CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y - originOffsetVec.Y;
            return new AKRSPoint3D(offsetX, offsetY, CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Z);
        }

        /// <summary>
        /// 计算相机偏差，实时位置-> 原点位置
        /// </summary>
        /// <param name="realtimeMidPoint"></param>
        /// <returns></returns>
        private AKRSPoint3D CalcCameraOffset(AKRSPoint2D realtimeMidPoint)
        {
            var originOffsetVec = this.OriginPoint - realtimeMidPoint;
            return new AKRSPoint3D(originOffsetVec.X/2, originOffsetVec.Y/2, 0);
        }
    }
}