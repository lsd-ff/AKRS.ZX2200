namespace AKRS.ZX2200.Experiment.Test
{
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

    /// <summary>
    /// CMK测试窗体
    /// </summary>
    public partial class FrmBMCTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmBMCTest()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 循环次数
        /// </summary>
        private int cycles;

        /// <summary>
        /// XY公差界限
        /// </summary>
        private double accuracyXY;

        /// <summary>
        /// 角度公差界限
        /// </summary>
        private double accuracyAngle;

        /// <summary>
        /// 补偿X
        /// </summary>
        private double compensateX;

        /// <summary>
        /// 补偿Y
        /// </summary>
        private double compensateY;

        /// <summary>
        /// 补偿角度
        /// </summary>
        private double compensateAngle;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

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
        private Task bMCTestTask;

        /// <summary>
        /// 是否开启模拟取片
        /// </summary>
        private bool isActiveSimulatePick = false;

        /// <summary>
        /// 是否开启温漂测试
        /// </summary>
        private bool isActiveTemperatrueDriftTest = false;

        /// <summary>
        /// 是否开启温漂补偿
        /// </summary>
        private bool isActiveTemperatrueDriftCompensate = false;

        /// <summary>
        /// 上视是否定位两次(第二次是验证纠偏)
        /// </summary>
        private bool isActiveUplookAjustTwice = false;

        /// <summary>
        /// 是否开启焊后补偿
        /// </summary>
        private bool isActivePostBondCompensateAccordingToFirst = true;

        /// <summary>
        /// 是否开启焊后补偿
        /// </summary>
        private bool isActivePostBondCompensateAccordingToLastTwo = true;

        /// <summary>
        /// 温漂测试循环次数
        /// </summary>
        private int temperatrueDriftTestcycles;

        /// <summary>
        /// 随机角度最大值
        /// </summary>
        private double randomAngleMaxValue;

        /// <summary>
        /// 随机角度最小值
        /// </summary>
        private double randomAngleMinValue;

        /// <summary>
        /// 温漂补偿系数Y
        /// </summary>
        private double temperatrueDriftCompensateKy;

        /// <summary>
        /// 温漂补偿系数X
        /// </summary>
        private double temperatrueDriftCompensateKx;

        /// <summary>
        /// 上视验证数据
        /// </summary>
        private List<AKRSPoint3D> uplookTestResList = new List<AKRSPoint3D>();

        /// <summary>
        /// 下视验证数据
        /// </summary>
        private List<AKRSPoint3D> downlookTestResList = new List<AKRSPoint3D>();

        /// <summary>
        /// 温漂
        /// </summary>
        private List<MatchResult> temperatrueDriftTestRes = new List<MatchResult>();

        /// <summary>
        /// 温漂补偿
        /// </summary>
        private List<AKRSPoint3D> temperatrueDriftOffsetList = new List<AKRSPoint3D>();

        /// <summary>
        /// 温漂补偿枚举
        /// </summary>
        private TemperatrueDriftCompensateEnum temperatrueDriftCompensate;

        /// <summary>
        /// 焊后补偿系数Y
        /// </summary>
        private double postBondCompensateKy;

        /// <summary>
        /// 焊后补偿系数X
        /// </summary>
        private double postBondCompensateKx;

        /// <summary>
        /// 焊后补偿系数阈值x
        /// </summary>
        private double postBondCompensateLimitX;

        /// <summary>
        /// 焊后补偿系数阈值y
        /// </summary>
        private double postBondCompensateLimitY;

        /// <summary>
        /// 定位延时
        /// </summary>
        private int visionDelay;

        /// <summary>
        /// 焊后硬补偿
        /// </summary>
        private AKRSPoint2D postBondOffset = new AKRSPoint2D();

        /// <summary>
        /// 焊后检测补偿
        /// </summary>
        private AKRSPoint3D postBondCompensation = BondDevicePara.GetInstance().BMCDevicePara.PostBondCompensate;

        // 休眠时间
        private double sleepInterval;

        // 是否停止
        private bool isStopTask=false;

        private Stopwatch sp=new Stopwatch();

        /// <summary>
        /// 吹气比列
        /// </summary>
        private int blowProportion;

        /// <summary>
        ///  确认按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            // 换BMC
            if (!this.system2Controller.ChangeNozzleAssistance("BMC"))
            {
                return;
            }

            // 参数保存
            this.Save();

            // 更换治具
            this.bondHeadController.SetBMCOnBondhead();

            isStopTask = false;

            // 开启测试线程
            this.StartTask();

            //this.bMCTestResultList.Clear();
            //this.bMCTestResultList.Add(
            //    new BMCTestResult() { XResult = 2.256631851196289, YResult = -1.5562772750854492, AngleResult = -0.0048610182978165994, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = 0.9391307830810547, YResult = -1.33734941482543, AngleResult = -0.005010660198735195, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = 1.3898611068725586, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = 1.5053749084472656, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = -0.6475448608398438, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = -0.24235248565673828, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = 0.7436275482177734, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = 1.6908645629882812, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = -2.0045042037963867, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });
            //this.bMCTestResultList.Add(new BMCTestResult() { XResult = -0.8744001388549805, YResult = 3, AngleResult = 3, CreateTime = DateTime.Now });

            //Task.Run(
            //    () =>
            //        {

            //            // 打开结果显示窗体
            //            FrmBMCTestResult frmBmcTestResult = new FrmBMCTestResult(
            //                this.accuracyXY,
            //                this.accuracyAngle,
            //                this.bMCTestResultList);
            //            frmBmcTestResult.ShowDialog();

            //        });
        }

        /// <summary>
        ///  BMC测试
        /// </summary>
        /// <returns>结果</returns>
        private bool BMCTest()
        {
            try
            {
                bool isFirstStart = true;
                while (true)
                {
                    if (this.isStopTask)
                    {
                        return false;
                    }

                    if (!isFirstStart) 
                    {
                        if (sp.Elapsed.TotalHours < this.sleepInterval)
                        {
                            Thread.Sleep(500);
                            continue;
                        }
                    }
                   

                    #region CMK测试

                    isFirstStart = false;

                    // 温漂补偿
                    AKRSPoint3D temperatrueDriftOffset = new AKRSPoint3D();

                    // 首次温漂定位结果P1
                    AKRSPoint3D firstTemperatrueDriftResP1 = new AKRSPoint3D();

                    // 首次温漂定位结果P2
                    AKRSPoint3D firstTemperatrueDriftResP2 = new AKRSPoint3D();

                    DialogResult dialog;

                    this.testFinished = false;
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
                        pRName);

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
                        WeakBlowProportion = blowProportion
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

                    // 计算上视拍照位
                    AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                    AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                    AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

                    // 寻找Pr模板
                    PREntity upLookPREntity = (PREntity)VisionEntityRepository.GetInstance()
                        .Find(CalibrateRunPara.GetInstance().UpLookPRName);

                    if (upLookPREntity == null)
                    {
                        AKRSMessageBoxExt.Show(
                            $"未找到PR：{CalibrateRunPara.GetInstance().UpLookPRName} ！\r\n",
                            "Alarm",
                            new string[] { "OK" },
                            new DialogResult[] { DialogResult.Abort },
                            AlarmLevel.SecondLevel);

                        return false;
                    }

                    // 循环测试
                    for (int i = 1; i <= this.cycles; i++)
                    {
                        if (this.isStopTask)
                        {
                            return false;
                        }

                        this.BeginInvoke(new Action(() => { this.LbCycle.Text = "Cycle:" + i; }));

                        #region 上视定位

                        // 运动到上视P1拍照位
                        this.bondModuleController.MoveSafeBondXYZ(upLookVisionPos1);

                        Thread.Sleep(this.visionDelay);

                        MatchResult upLookRes1, upLookRes2;

                    // P1定位
                    RetryP1:

                        // 开始定位
                        ExcuteResult upLookP1Result = upLookPREntity.DoWork();

                        upLookRes1 = (MatchResult)upLookPREntity.AlgResult;

                        // 处理拍照完成后的结果
                        if (upLookP1Result != ExcuteResult.Success)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                                $"上视PR：{CalibrateRunPara.GetInstance().UpLookPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
                                new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                                AlarmLevel.SecondLevel);

                            switch (dialog)
                            {
                                case DialogResult.Retry:

                                    goto RetryP1;

                                case DialogResult.Abort:

                                    throw new Exception($"{CalibrateRunPara.GetInstance().UpLookPRName}  adjust  failed!");

                                case DialogResult.Ignore:
                                    upLookRes1 = new MatchResult();
                                    break;
                            }
                        }

                        AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes1);

                        // 保存结果图片
                        Bitmap bitmap =
                           upLookPREntity.AlgResult.OutPutImg1;

                        // 运动到P2拍照位
                        // Z轴安全直接移动XY轴
                        System2Module.GetInstance().BondModule.MoveBondXY(upLookVisionPos2.X, upLookVisionPos2.Y);

                        Thread.Sleep(this.visionDelay);

                    // P2定位
                    RetryP2:

                        // 开始定位
                        ExcuteResult upLookP2Result = upLookPREntity.DoWork();

                        upLookRes2 = (MatchResult)upLookPREntity.AlgResult;

                        // 处理拍照完成后的结果
                        if (upLookP1Result != ExcuteResult.Success)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                               $"上视PR：{CalibrateRunPara.GetInstance().UpLookPRName} 定位失败!\r\n",
                               "Alarm",
                               new string[] { "重试", "退出", "忽略" },
                               new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                               AlarmLevel.SecondLevel);

                            switch (dialog)
                            {
                                case DialogResult.Retry:

                                    goto RetryP2;

                                case DialogResult.Abort:

                                    throw new Exception($"{CalibrateRunPara.GetInstance().UpLookPRName}  adjust  failed!");

                                case DialogResult.Ignore:
                                    matchResult1 = new MatchResult();
                                    break;
                            }
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        // 保存结果图片
                        // bitmap.Save(path);

                        #endregion

                        #region 上视纠偏计算

                        // 计算角度
                        double uplookOffSetAngle = CalibService.CalculateAngle(
                            new AKRSPoint2D(point3D1.X, point3D1.Y),
                            new AKRSPoint2D(point3D2.X, point3D2.Y));

                        double placeAngle = uplookOffSetAngle - 45 - this.compensateAngle;

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

                        this.SaveRotateOffsetData(offsetAfterRotate, placeAngle);

                        // 贴片的角度
                        double bondAngle = this.bondHeadController.GetAxisTRealPos() - placeAngle;

                        #endregion

                        #region 上视二次定位

                        // 提前转
                        this.bondHeadController.RelativeRotateAxisT(-placeAngle);

                        //if (this.isActiveUplookAjustTwice)
                        if (i == 1)
                        {
                            // p1
                            AKRSPoint3D test1 = upLookVisionPos1 + new AKRSPoint3D(
                                                    offsetAfterRotate.X,
                                                    offsetAfterRotate.Y,
                                                    0);

                            // 运动到上视P1拍照位
                            // Z轴安全直接移动XY轴
                            this.bondModuleController.MoveBondXY(test1.X, test1.Y);

                            Thread.Sleep(this.visionDelay);

                            // 定位
                            upLookP1Result = upLookPREntity.DoWork();

                            upLookRes1 = (MatchResult)upLookPREntity.AlgResult;

                            point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                                this.bondModuleController.Get3DRealPosition(),
                                (MatchResult)upLookPREntity.AlgResult);

                            // p2
                            AKRSPoint3D test2 = upLookVisionPos2 + new AKRSPoint3D(
                                                    offsetAfterRotate.X,
                                                    offsetAfterRotate.Y,
                                                    0);

                            // 运动到上视P2拍照位
                            // Z轴安全直接移动XY轴
                            System2Module.GetInstance().BondModule.MoveBondXY(test2.X, test2.Y);

                            Thread.Sleep(this.visionDelay);

                            // 定位
                            upLookP2Result = upLookPREntity.DoWork();

                            upLookRes2 = (MatchResult)upLookPREntity.AlgResult;

                            point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                                this.bondModuleController.Get3DRealPosition(),
                                (MatchResult)upLookPREntity.AlgResult);

                            // 小标定片中心
                            AKRSPoint3D centerAfterTest = (point3D1 + point3D2) / 2;

                            this.uplookTestResList.Add(centerAfterTest);

                            // 计算角度
                            uplookOffSetAngle = CalibService.CalculateAngle(
                               new AKRSPoint2D(point3D1.X, point3D1.Y),
                               new AKRSPoint2D(point3D2.X, point3D2.Y));

                            placeAngle = uplookOffSetAngle - 45 - this.compensateAngle;

                            // 小标定片中心
                            glassUpLookCenter = (point3D1 + point3D2) / 2;

                            // 相对旋转中心的偏移
                            offsetCenter = new AKRSPoint2D()
                            {
                                X = centerAfterTest.X - bondHeadCenter.X,
                                Y = centerAfterTest.Y - bondHeadCenter.Y,
                            };

                            // 旋转之后的偏移
                            offsetAfterRotate = this.bondHeadController.GetNozzleOffset(offsetCenter, placeAngle);

                            this.SaveRotateOffsetData(offsetAfterRotate, placeAngle);
                        }

                        #endregion

                        #region 旋转中心计算

                        MatchResult matchRrotateCenterResult = new MatchResult();

                        AKRSPoint3D rotatePos = new AKRSPoint3D();

                        if (i % this.temperatrueDriftTestcycles == 0)
                        {
                            this.bondModuleController.MoveBondXY(
                                CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos.X, CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos.Y);

                            int[] angleArray = new int[] { -180, -90, 0, 90, 180 };
                            List<AKRSPoint2D> circlePointList = new List<AKRSPoint2D>();
                            List<AKRSPoint2D> matchResultList2D = new List<AKRSPoint2D>();

                            AKRSPoint2D curMachinePos2D = this.bondModuleController.Get2DRealPosition();

                            foreach (int angle in angleArray)
                            {
                                // T轴旋转一定角度
                                this.bondHeadController.RotateAxisT(angle);

                                Thread.Sleep(100);

                            RetryCommand:
                                ExcuteResult res = upLookPREntity.DoWork();

                                // 处理拍照完成后的结果
                                if (res != ExcuteResult.Success)
                                {
                                    dialog = AKRSMessageBoxExt.Show(
                                $"上视PR：{CalibrateRunPara.GetInstance().UpLookPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
                                new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore },
                                AlarmLevel.SecondLevel);

                                    switch (dialog)
                                    {
                                        case DialogResult.Retry:

                                            goto RetryCommand;

                                        case DialogResult.Abort:

                                            throw new Exception($"{CalibrateRunPara.GetInstance().UpLookPRName}  adjust  failed!");

                                        case DialogResult.Ignore:
                                            matchRrotateCenterResult = new MatchResult();
                                            break;
                                    }
                                }
                                else
                                {
                                    matchRrotateCenterResult = (MatchResult)upLookPREntity.AlgResult;
                                }

                                matchResultList2D.Add(new AKRSPoint2D(matchRrotateCenterResult.CenterX, matchRrotateCenterResult.CenterY));

                                AKRSPoint2D circlePoint = CalibService.GetMachinePosByPixelPos(
                                    curMachinePos2D,
                                    matchRrotateCenterResult,
                                    "UpLookCameraCoordinateSystem");

                                circlePointList.Add(circlePoint);
                            }

                            CalibService.FitCircle(matchResultList2D, out double circleCenterPixelX, out double circleCenterPixelY, out double circleRadiusPixel);

                            matchRrotateCenterResult.CenterX = circleCenterPixelX;
                            matchRrotateCenterResult.CenterY = circleCenterPixelY;
                            this.calibController.MoveToCamCenter(CalibController.CamCoordinateType.UpLook, new MatchResult(circleCenterPixelX, circleCenterPixelY, 0));
                            rotatePos = this.bondModuleController.Get3DRealPosition();

                            // 再重新转到贴片角度
                            this.bondHeadController.RotateAxisT(bondAngle);
                        }

                        #endregion

                        #region 上视温漂测试,改到上视拍完去温漂点

                        if (this.isActiveTemperatrueDriftTest
                            && this.temperatrueDriftCompensate == TemperatrueDriftCompensateEnum.UpLookMark)
                        {
                            if (i % this.temperatrueDriftTestcycles == 0)
                            {
                                this.bondModuleController.MoveSafeBondXYZ(
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
                                    dialog = AKRSMessageBoxExt.Show(
                               $"下视PR：{CalibrateRunPara.GetInstance().BmcPRName} 定位失败!\r\n",
                               "Alarm",
                               new string[] { "Retry", "Abort" },
                               new DialogResult[] { DialogResult.Retry, DialogResult.Abort},
                               AlarmLevel.SecondLevel);                   

                                    switch (dialog)
                                    {
                                        case DialogResult.Retry:

                                            goto RetryUplookMark;

                                        case DialogResult.Abort:

                                            throw new Exception($"上视温漂测试Mark  adjust  failed!");
                                    }
                                }

                                if (this.isActiveTemperatrueDriftCompensate)
                                {
                                    if (i == 1)
                                    {
                                        firstTemperatrueDriftResP1 =
                                            this.system2Controller.GetBondVisionResultPos(driftTestMatchResult);
                                    }

                                    temperatrueDriftOffset.X =
                                        (this.system2Controller.GetBondVisionResultPos(driftTestMatchResult)
                                         - firstTemperatrueDriftResP1).X * this.temperatrueDriftCompensateKx;

                                    temperatrueDriftOffset.Y =
                                        (this.system2Controller.GetBondVisionResultPos(driftTestMatchResult)
                                         - firstTemperatrueDriftResP1).Y * this.temperatrueDriftCompensateKy;

                                    this.temperatrueDriftOffsetList.Add(temperatrueDriftOffset);
                                }

                                this.bondHeadController.MoveBondZToSafePos();

                                this.temperatrueDriftTestRes.Add(driftTestMatchResult);

                                // 打印
                                this.SaveTempratrueDriftData(driftTestMatchResult, temperatrueDriftOffset);

                                SaveDriftAndRotateCenterTestData(driftTestMatchResult, matchRrotateCenterResult, rotatePos);
                            }
                        }

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
                        MatchResult matchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                            p1MarkPos,
                            CalibrateRunPara.GetInstance().BmcPRName);

                        if (matchResult2 == null)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                                $"下视模板：{CalibrateRunPara.GetInstance().BmcPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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
                                $"下视模板：{CalibrateRunPara.GetInstance().BmcPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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

                        // 打印
                        this.SaveVisionResData(upLookRes1, upLookRes2, matchResult2, matchResult3);

                        #endregion

                        #region BMC温漂计算

                        if (this.isActiveTemperatrueDriftTest
                            && this.temperatrueDriftCompensate == TemperatrueDriftCompensateEnum.BMC)
                        {
                            if (i == 1)
                            {
                                firstTemperatrueDriftResP1 = p1VisionPos;
                                firstTemperatrueDriftResP2 = p2VisionPos;
                            }

                            // 温漂补偿是P1P2的温漂取平均值
                            AKRSPoint3D average = ((p1VisionPos - firstTemperatrueDriftResP1)
                                                   + (p2VisionPos - firstTemperatrueDriftResP2)) / 2;

                            if (this.isActiveTemperatrueDriftCompensate)
                            {
                                temperatrueDriftOffset.X = average.X * this.temperatrueDriftCompensateKx;

                                temperatrueDriftOffset.Y = average.Y * this.temperatrueDriftCompensateKy;
                            }
                            else
                            {
                                temperatrueDriftOffset = new AKRSPoint3D();
                            }

                            this.temperatrueDriftOffsetList.Add(temperatrueDriftOffset);

                            this.SaveTempratrueDriftData(matchResult2, matchResult3, temperatrueDriftOffset, this.postBondCompensation);
                        }

                        #endregion

                        #region 计算贴片位

                        // 计算放片位
                        AKRSPoint3D placePos = (p1VisionPos + p2VisionPos) / 2
                                               + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                        // 最终贴片位，暂时不动态补偿
                        AKRSPoint2D finalPlacePos = new AKRSPoint2D()
                        {
                            X = placePos.X + offsetAfterRotate.X + this.compensateX
                                                                - temperatrueDriftOffset.X - this.postBondCompensation.X,
                            Y = placePos.Y + offsetAfterRotate.Y + this.compensateY
                            - temperatrueDriftOffset.Y - this.postBondCompensation.Y
                        };

                        this.downlookTestResList.Add(placePos);

                        #endregion

                        #region 贴片

                        // 运动到放片位
                        this.bondModuleController.MoveToG0Pos(finalPlacePos.X, finalPlacePos.Y);

                        // 放片参数,标定片厚度：2
                        double placeLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;
                        liftLevel = CalibrateRunPara.GetInstance().GlassCenterCurVisionMachinePos.Z;

                    // 放片
                    ReBond:
                        bool ret = this.bondHeadController.BondAction(placeLevel, liftLevel , component, BondTypeEnum.BondOnBMC);

                        if (ret == false)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                                $"放片失败!",
                                "Alarm",
                                new string[] { "重试", "忽略" },
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
                                $"下视模板：{CalibrateRunPara.GetInstance().BmcPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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

                        AKRSPoint3D bMCVisionPos1 = this.system2Controller.GetBondVisionResultPos(bMCMatchResult1);

                    RetryPostBondGlassP1:
                        // 小标定片P1定位
                        MatchResult glassMatchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                            postBondP1Pos,
                            CalibrateRunPara.GetInstance().GlassPRName,
                            true);

                        if (glassMatchResult1 == null)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                                $"下视模板：{CalibrateRunPara.GetInstance().GlassPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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
                                $"下视模板：{CalibrateRunPara.GetInstance().BmcPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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

                        AKRSPoint3D bMCVisionPos2 = this.system2Controller.GetBondVisionResultPos(bMCMatchResult2);

                    RetryPostBondGlassP2:

                        // 小标定片P2定位
                        MatchResult glassMatchResult2 = (MatchResult)this.system2Controller.BondCameraVision(
                            postBondP2Pos,
                            CalibrateRunPara.GetInstance().GlassPRName,
                            true);

                        if (glassMatchResult2 == null)
                        {
                            dialog = AKRSMessageBoxExt.Show(
                                $"下视模板：{CalibrateRunPara.GetInstance().GlassPRName} 定位失败!\r\n",
                                "Alarm",
                                new string[] { "重试", "退出", "忽略" },
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

                        // 计算大标定片中心点
                        AKRSPoint3D bMCCenterPos = (postBondP1Pos + postBondP2Pos) / 2
                                                   + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                        double bMCAngle = CalibService.CalculateAngle(
                            new AKRSPoint2D(bMCVisionPos1.X, bMCVisionPos1.Y),
                            new AKRSPoint2D(bMCVisionPos2.X, bMCVisionPos2.Y));

                        // 计算小标定片中心点
                        AKRSPoint3D glassCenterPos = (glassVisionPos1 + glassVisionPos2) / 2
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

                        #endregion

                        #region 焊后补偿计算

                        if (i == 2)
                        {
                            this.postBondOffset.X = postBondRes.X;
                            this.postBondOffset.Y = postBondRes.Y;
                        }

                        // 第二次的焊后检测结果补偿到贴片位
                        if (this.isActivePostBondCompensateAccordingToFirst)
                        {
                            this.postBondCompensation.X = this.postBondOffset.X;
                            this.postBondCompensation.Y = this.postBondOffset.Y;
                        }

                        // 焊后补偿=前两次焊后结果平方和除以再开方
                        if (i > 2 && this.isActivePostBondCompensateAccordingToLastTwo)
                        {
                            double lastPoint1X =
                                Math.Abs(this.bMCTestResultList[i - 1].XResult) < this.postBondCompensateLimitX
                                    ? this.bMCTestResultList[i - 1].XResult
                                    : 0;

                            double lastPoint2X = Math.Abs(this.bMCTestResultList[i - 2].XResult) < this.postBondCompensateLimitX
                                                     ? this.bMCTestResultList[i - 2].XResult
                                                     : 0;

                            double lastPoint1Y =
                                Math.Abs(this.bMCTestResultList[i - 1].YResult) < this.postBondCompensateLimitY
                                    ? this.bMCTestResultList[i - 1].YResult
                                    : 0;

                            double lastPoint2Y = Math.Abs(this.bMCTestResultList[i - 2].YResult) < this.postBondCompensateLimitY
                                                     ? this.bMCTestResultList[i - 2].YResult
                                                     : 0;

                            this.postBondCompensation.X += (lastPoint1X + lastPoint2X) / 2000.0 * this.postBondCompensateKx;

                            this.postBondCompensation.Y += (lastPoint1Y + lastPoint2Y) / 2000.0 * this.postBondCompensateKy;
                        }

                        #endregion

                        #region 取片

                        // 运动到取片位
                        this.bondModuleController.MoveToG0Pos(glassCenterPos.X, glassCenterPos.Y);
                        this.bondHeadController.RotateAxisT(0);

                        // 计算参数
                        pickLevel = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightResult + 1.7;

                        // 取片
                        this.bondHeadController.PickAction(pickLevel, component, liftLevel + 5, PickTypeEnum.BMC);

                        if (this.isActiveSimulatePick)
                        {
                            // 取片后旋转随机角度
                            double randomAngle = GeometryService.NextDouble(
                                this.randomAngleMaxValue,
                                this.randomAngleMinValue);
                            this.bondHeadController.RelativeRotateAxisT(randomAngle);
                        }

                        this.SavePostBondCompensateData();

                        #endregion
                    }

                    #region 放回小标定片

                    // 移动到放片位
                    this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                    pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                    // 放片
                    this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                    this.bondModuleController.MoveToSafePos();

                #endregion

                End:

                    if (this.bMCTestResultList.Count != 0)
                    {
                        this.SaveUplookTestData();
                    }

                    BondDevicePara.GetInstance().Save();

                    #endregion

                    sp.Restart();           
                }
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"BMC测试失败！\r\n{e.ToString()}",
                    "Exception",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                // 打开结果显示窗体
                FrmBMCTestResult frmBmcTestResult = new FrmBMCTestResult(
                    this.accuracyXY,
                    this.accuracyAngle,
                    this.bMCTestResultList);
                frmBmcTestResult.ShowDialog();

                frmBmcTestResult.Dispose();
            }

            return false;
        }

        /// <summary>
        /// 参数保存
        /// </summary>
        private void Save()
        {
            this.cycles = (int)this.SpCycles.Value;
            this.accuracyXY = this.bMCDevicePara.AccuracyXY = (double)this.SpXYAccuracy.Value;
            this.accuracyAngle = this.bMCDevicePara.AccuracyAngle = (double)this.SpAngleAccuracy.Value;

            this.compensateX = this.bMCDevicePara.CompensateX = (double)this.SpCompensateX.Value;
            this.compensateY = this.bMCDevicePara.CompensateY = (double)this.SpCompensateY.Value;
            this.compensateAngle = this.bMCDevicePara.CompensateAngle = (double)this.SpCompensateAngle.Value;

            this.randomAngleMaxValue = (double)this.SpMaxValue.Value;
            this.randomAngleMinValue = (double)this.SpMinValue.Value;
            this.temperatrueDriftTestcycles = (int)this.SpTemperatrueDriftTestCycle.Value;
            this.temperatrueDriftCompensateKy = (double)this.SpKyValue.Value;
            this.temperatrueDriftCompensateKx = (double)this.SpKxValue.Value;

            this.postBondCompensateKx = (double)this.SpPostBondCompensateKx.Value;
            this.postBondCompensateKy = (double)this.SpPostBondCompensateKy.Value;

            this.postBondCompensateLimitX = (double)this.SpPostBondCompensateLimitX.Value;
            this.postBondCompensateLimitY = (double)this.SpPostBondCompensateLimitY.Value;

            this.visionDelay = (int)this.SpVisionDelay.Value;

            this.isActiveSimulatePick = this.ChkSimulatePick.Checked;
            this.isActiveTemperatrueDriftTest = this.ChkTemperatrueDriftTest.Checked;
            this.isActiveTemperatrueDriftCompensate = this.ChkActiveTemperatrueDriftCompensate.Checked;
            this.isActiveUplookAjustTwice = this.ChkUplookAdjustTwice.Checked;
            this.isActivePostBondCompensateAccordingToFirst = this.ChkCompensateAccordingToFirst.Checked;
            this.isActivePostBondCompensateAccordingToLastTwo = this.ChkCompensateAccordingToLastTwo.Checked;

            this.temperatrueDriftCompensate = (TemperatrueDriftCompensateEnum)this.LueCompensateType.EditValue;

            this.sleepInterval = (double)SpSleepInterval.Value;

            this.blowProportion = this.bMCDevicePara.BlowProportion = (int)this.SpBlowProportion.Value;

            this.blowProportion = (int)this.SpBlowProportion.Value;

            BondDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 页面初始化
        /// </summary>
        private void InitControl()
        {
            this.SpXYAccuracy.EditValue = this.bMCDevicePara.AccuracyXY;
            this.SpAngleAccuracy.EditValue = this.bMCDevicePara.AccuracyAngle;

            this.SpCompensateX.EditValue = this.bMCDevicePara.CompensateX;
            this.SpCompensateY.EditValue = this.bMCDevicePara.CompensateY;
            this.SpCompensateAngle.EditValue = this.bMCDevicePara.CompensateAngle;

            this.TxtCompensateX.Text = this.postBondCompensation.X.ToString();
            this.TxtCompensateY.Text = this.postBondCompensation.Y.ToString();

            // 下拉框绑定
            this.LueCompensateType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<TemperatrueDriftCompensateEnum>();
            this.LueCompensateType.EditValue = TemperatrueDriftCompensateEnum.BMC;

            this.SpBlowProportion.EditValue= this.bMCDevicePara.BlowProportion;

            this.LbTip.Visible = false;
        }

        /// <summary>
        ///  取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 开启线程
        /// </summary>
        /// <returns>结果</returns>
        private bool StartTask()
        {
            // 防止线程多次启动
            if (this.bMCTestTask == null
                || this.bMCTestTask.Status != TaskStatus.Running)
            {
                this.bMCTestResultList.Clear();
                this.bMCTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("BMC测试线程");
                            this.BMCTest();
                        });
            }

            return true;
        }

        /// <summary>
        /// 视觉窗口
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnVision_Click(object sender, EventArgs e)
        {
            UcMainSystem.VmVisionShow();
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        private void SaveUplookTestData()
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--UplookTestDetail.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "uplookTestResX";

                worksheet.Cells[1, 2].Value = "uplookTestResY";

                worksheet.Cells[1, 3].Value = "downlookTestResX";

                worksheet.Cells[1, 4].Value = "downlookTestResY";

                worksheet.Cells[1, 5].Value = "time";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < this.uplookTestResList.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value = this.uplookTestResList[i]?.X;

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value = this.uplookTestResList[i]?.Y;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = this.downlookTestResList[i]?.X;

                worksheet.Cells[lastUsedRow + 1 + i, 4].Value = this.downlookTestResList[i]?.Y;

                worksheet.Cells[lastUsedRow + 1 + i, 5].Value =
                    DateTime.Now.ToString("MM-dd HH:mm:ss");
            }

            excelPackage.Save();
        }

        /// <summary>
        ///  保存测试数据
        /// </summary>
        private void SaveDriftAndRotateCenterTestData(MatchResult driftResult, MatchResult rotateCenterResult,AKRSPoint3D pos)
        {
            // 温漂结果转到G0坐标
            AKRSPoint3D driftG0Result = this.bondModuleController.ConvertPixelToG0Pos(
                BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1,
                driftResult);

            // 温漂结果转到轴坐标
            AKRSPoint3D driftAxisResult = this.bondModuleController.ConvertPixelToG0Pos(
                BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1,
                driftResult);

            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--DriftAndRotateCenterTestDetail.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "driftMatchResultX";

                worksheet.Cells[1, 2].Value = "driftMatchResultY";

                worksheet.Cells[1, 3].Value = "driftAxisResultX";

                worksheet.Cells[1, 4].Value = "driftAxisResultY";

                worksheet.Cells[1, 5].Value = "rotate center match result X";

                worksheet.Cells[1, 6].Value = "rotate center match result Y";

                worksheet.Cells[1, 7].Value = "rotate center X";

                worksheet.Cells[1, 8].Value = "rotate center Y";

                worksheet.Cells[1, 9].Value = "time";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                worksheet.Cells[lastUsedRow + 1, 1].Value = driftResult?.CenterX;

                worksheet.Cells[lastUsedRow + 1, 2].Value = driftResult?.CenterY;

                worksheet.Cells[lastUsedRow + 1, 3].Value = driftAxisResult?.X;

                worksheet.Cells[lastUsedRow + 1, 4].Value = driftAxisResult?.Y;

            worksheet.Cells[lastUsedRow + 1 , 5].Value = rotateCenterResult?.CenterX;

                worksheet.Cells[lastUsedRow + 1 , 6].Value = rotateCenterResult?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 7].Value = pos?.X;

            worksheet.Cells[lastUsedRow + 1, 8].Value = pos?.Y;

            worksheet.Cells[lastUsedRow + 1, 9].Value =
                    DateTime.Now.ToString("MM-dd HH:mm:ss");


            excelPackage.Save();
        }

        /// <summary>
        ///  保存温漂数据
        /// </summary>
        /// <param name="testMatchResult">定位结果</param>
        /// <param name="temperatrueDriftOffset">温漂补偿</param>
        private void SaveTempratrueDriftData(
            MatchResult testMatchResult,
            AKRSPoint3D temperatrueDriftOffset)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--UplookMarkTempratrueDriftTest.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "MatchResultX";

                worksheet.Cells[1, 3].Value = "MatchResultY";

                worksheet.Cells[1, 4].Value = "MatchResultAngle";

                worksheet.Cells[1, 5].Value = "temperatrueDriftCompensationX(μm)";

                worksheet.Cells[1, 6].Value = "temperatrueDriftCompensationY(μm)";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = testMatchResult?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = testMatchResult?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 4].Value = testMatchResult?.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = temperatrueDriftOffset?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 6].Value = temperatrueDriftOffset?.Y * 1000;

            excelPackage.Save();
        }

        /// <summary>
        ///  保存焊后补偿数据
        /// </summary>
        private void SavePostBondCompensateData()
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--PostBondCompensateData.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "postBondCompensationX(μm)";

                worksheet.Cells[1, 3].Value = "postBondCompensationY(μm)";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = this.postBondCompensation?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 3].Value = this.postBondCompensation?.Y * 1000;

            excelPackage.Save();
        }

        /// <summary>
        ///  保存吸嘴旋转之后的偏移
        /// </summary>
        /// <param name="offsetAfterRotate">偏移</param>
        private void SaveRotateOffsetData(AKRSPoint2D offsetAfterRotate,double angle)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--OffsetAfterRotateData.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "OffsetAfterRotateX(微米)";

                worksheet.Cells[1, 3].Value = "OffsetAfterRotateY(微米)";

                worksheet.Cells[1, 3].Value = "Angle";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = offsetAfterRotate?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 3].Value = offsetAfterRotate?.Y * 1000;

            worksheet.Cells[lastUsedRow + 1, 4].Value = angle;

            excelPackage.Save();
        }

        /// <summary>
        /// 保存定位结果
        /// </summary>
        /// <param name="uplookP1">上视P1</param>
        /// <param name="uplookP2">上视p2</param>
        /// <param name="downlookP1">下视p1</param>
        /// <param name="downlookP2">下视p2</param>
        private void SaveVisionResData(MatchResult uplookP1, MatchResult uplookP2, MatchResult downlookP1, MatchResult downlookP2)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--BMCVisionResData.xlsx"));

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
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "uplookP1-X";

                worksheet.Cells[1, 3].Value = "uplookP1-Y";

                worksheet.Cells[1, 4].Value = "uplookP1-Angle";

                worksheet.Cells[1, 5].Value = "uplookP2-X";

                worksheet.Cells[1, 6].Value = "uplookP2-Y";

                worksheet.Cells[1, 7].Value = "uplookP2-Angle";

                worksheet.Cells[1, 8].Value = "downlookP1-X";

                worksheet.Cells[1, 9].Value = "downlookP1-Y";

                worksheet.Cells[1, 10].Value = "downlookP1-Angle";

                worksheet.Cells[1, 11].Value = "downlookP2-X";

                worksheet.Cells[1, 12].Value = "downlookP2-Y";

                worksheet.Cells[1, 13].Value = "downlookP2-Angle";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = uplookP1?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = uplookP1?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 4].Value = uplookP1?.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = uplookP2?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 6].Value = uplookP2?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 7].Value = uplookP2?.Angle;

            worksheet.Cells[lastUsedRow + 1, 8].Value = downlookP1?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 9].Value = downlookP1?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 10].Value = downlookP1?.Angle;

            worksheet.Cells[lastUsedRow + 1, 11].Value = downlookP2?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 12].Value = downlookP2?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 13].Value = downlookP2?.Angle;

            excelPackage.Save();
        }

        /// <summary>
        ///  保存温漂数据(BMC)
        /// </summary>
        /// <param name="testMatchResult1">定位结果</param>
        /// <param name="testMatchResult2">定位结果</param>
        /// <param name="temperatrueDriftOffset">温漂补偿</param>
        /// <param name="postBondCompensation">焊后补偿</param>
        private void SaveTempratrueDriftData(
            MatchResult testMatchResult1,
            MatchResult testMatchResult2,
            AKRSPoint3D temperatrueDriftOffset,
            AKRSPoint3D postBondCompensation)
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "--BMCTempratrueDriftTest.xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "MatchResultY";

                worksheet.Cells[1, 3].Value = "MatchResultY";

                worksheet.Cells[1, 4].Value = "MatchResultAngle";

                worksheet.Cells[1, 5].Value = "MatchResult2X";

                worksheet.Cells[1, 6].Value = "MatchResult2Y";

                worksheet.Cells[1, 7].Value = "MatchResult2Angle";

                worksheet.Cells[1, 8].Value = "temperatrueDriftCompensationX(μm)";

                worksheet.Cells[1, 9].Value = "temperatrueDriftCompensationY(μm)";

                worksheet.Cells[1, 10].Value = "postBondCompensationX(μm)";

                worksheet.Cells[1, 11].Value = "postBondCompensationY(μm)";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            worksheet.Cells[lastUsedRow + 1, 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

            worksheet.Cells[lastUsedRow + 1, 2].Value = testMatchResult1?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 3].Value = testMatchResult1?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 4].Value = testMatchResult1?.Angle;

            worksheet.Cells[lastUsedRow + 1, 5].Value = testMatchResult2?.CenterX;

            worksheet.Cells[lastUsedRow + 1, 6].Value = testMatchResult2?.CenterY;

            worksheet.Cells[lastUsedRow + 1, 7].Value = testMatchResult2?.Angle;

            worksheet.Cells[lastUsedRow + 1, 8].Value = temperatrueDriftOffset?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 9].Value = temperatrueDriftOffset?.Y * 1000;

            worksheet.Cells[lastUsedRow + 1, 10].Value = postBondCompensation?.X * 1000;

            worksheet.Cells[lastUsedRow + 1, 11].Value = postBondCompensation?.Y * 1000;

            excelPackage.Save();
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkSimulatePick_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkSimulatePick.Checked)
            {
                this.SpMaxValue.Enabled = true;
                this.SpMinValue.Enabled = true;
            }
            else
            {
                this.SpMaxValue.Enabled = false;
                this.SpMinValue.Enabled = false;
            }
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkTemperatrueDriftTest_CheckedChanged(object sender, EventArgs e)
        {
            this.SpTemperatrueDriftTestCycle.Enabled = this.ChkTemperatrueDriftTest.Checked;
            this.LueCompensateType.Enabled = this.ChkTemperatrueDriftTest.Checked;
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActiveTemperatrueDriftCompensate_CheckedChanged(object sender, EventArgs e)
        {
            this.SpKxValue.Enabled = this.ChkActiveTemperatrueDriftCompensate.Checked;
            this.SpKyValue.Enabled = this.ChkActiveTemperatrueDriftCompensate.Checked;
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkCompensateAccordingToLastTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkCompensateAccordingToLastTwo.Checked)
            {
                this.ChkCompensateAccordingToFirst.Checked = false;
            }

            this.SpPostBondCompensateKx.Enabled = this.ChkCompensateAccordingToLastTwo.Checked;
            this.SpPostBondCompensateKy.Enabled = this.ChkCompensateAccordingToLastTwo.Checked;
            this.SpPostBondCompensateLimitX.Enabled = this.ChkCompensateAccordingToLastTwo.Checked;
            this.SpPostBondCompensateLimitY.Enabled = this.ChkCompensateAccordingToLastTwo.Checked;
        }

        /// <summary>
        /// 勾选改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkCompensateAccordingToFirst_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkCompensateAccordingToFirst.Checked)
            {
                this.ChkCompensateAccordingToLastTwo.Checked = false;
            }
        }

        /// <summary>
        /// 清除
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            // 提示
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"Clear  post  bond  compensation?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.No)
            {
                return;
            }

            this.postBondCompensation = new AKRSPoint3D();
            BondDevicePara.GetInstance().Save();
            this.InitControl();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.TxtCompensateX.Text = this.postBondCompensation.X.ToString();
            this.TxtCompensateY.Text = this.postBondCompensation.Y.ToString();

            if (this.bMCTestTask != null)
            {
                if (this.bMCTestTask.Status == TaskStatus.Running || this.bMCTestTask.IsCompleted == false)
                {
                    this.LbTip.Visible = true;
                }
                else
                {
                    this.LbTip.Visible = false;
                }
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmBMCTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.bMCTestTask != null && this.bMCTestTask.IsCompleted == false) 
            {
                // 提示
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"Thread is not end ,please stop thread first!",
                    "Question",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                e.Cancel = true;
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            this.isStopTask = !this.isStopTask;
        }
    }

    /// <summary>
    /// BMC测试结果
    /// </summary>
    public class BMCTestResult
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// X
        /// </summary>
        public double XResult { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public double YResult { get; set; }

        /// <summary>
        ///  角度
        /// </summary>
        public double AngleResult { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 温漂补偿枚举
    /// </summary>
    public enum TemperatrueDriftCompensateEnum
    {
        /// <summary>
        /// 上视Mark
        /// </summary>
        UpLookMark,

        /// <summary>
        /// BMC
        /// </summary>
        BMC
    }
}