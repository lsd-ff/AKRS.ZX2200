using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    using System.IO;
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using OfficeOpenXml;

    public partial class FrmAlignAngleAccuracyTest : DevExpress.XtraEditors.XtraForm
    {
        private bool signalStop = true;

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
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara bMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private UpLookController upLookController = new UpLookController();

        public FrmAlignAngleAccuracyTest()
        {
            InitializeComponent();
        }

        private void BtnTest1Start_Click(object sender, EventArgs e)
        {
            this.cycles = (int)this.SpCycles.Value;
            this.visionTime = (int)this.SpVisionTime.Value;
            //this.isMoveToCameraCenterAfterVision = this.ChkMoveToCameraCenterAfterVision.Checked;
            this.ChkMoveToCameraCenterAfterVision.Enabled = false;
            this.isMoveToCameraCenterAfterVision = false;

            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("角度校正实验1线程");
                        this.Method1();
                    });

            this.BtnTest1Start.Enabled = false;
            this.BtnTest2Start.Enabled = false;
        }

        private void BtnTest2Start_Click(object sender, EventArgs e)
        {
            this.cycles = (int)this.SpCycles.Value;
            this.visionTime = (int)this.SpVisionTime.Value;
            //this.isMoveToCameraCenterAfterVision = this.ChkMoveToCameraCenterAfterVision.Checked;
            this.ChkMoveToCameraCenterAfterVision.Enabled = false;
            this.isMoveToCameraCenterAfterVision = false;

            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("角度校正实验2线程");
                        this.Method2();
                    });

            this.BtnTest1Start.Enabled = false;
            this.BtnTest2Start.Enabled = false;
        }

        private void BtnTestStop_Click(object sender, EventArgs e)
        {
            this.signalStop = true;
        }

        private void Method1()
        {
            try
            {
                // 换BMC
                if (!this.system2Controller.ChangeNozzleAssistance("BMC"))
                {
                    return;
                }

                // 更换治具
                this.bondHeadController.SetBMCOnBondhead();

                double mark1VisionPos_X = 0,
                       mark1VisionPos_Y = 0,
                       mark2VisionPos_X = 0,
                       mark2VisionPos_Y = 0,
                       mark1PixelPos_X = 0,
                       mark1PixelPos_Y = 0,
                       mark2PixelPos_X = 0,
                       mark2PixelPos_Y = 0,
                       mark1CenterPos_X = 0,
                       mark1CenterPos_Y = 0,
                       mark2CenterPos_X = 0,
                       mark2CenterPos_Y = 0,
                       mark1DistancePos_X = 0,
                       mark1DistancePos_Y = 0,
                       mark2DistancePos_X = 0,
                       mark2DistancePos_Y = 0,
                       mark1CenterPos_theory_X = 0,
                       mark1CenterPos_theory_Y = 0,
                       mark2CenterPos_theory_X = 0,
                       mark2CenterPos_theory_Y = 0,
                       theta = 0;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\角度校正实验1.xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "mark1VisionPos_X";
                worksheet.Cells[1, 2].Value = "mark1VisionPos_Y";
                worksheet.Cells[1, 3].Value = "mark2VisionPos_X";
                worksheet.Cells[1, 4].Value = "mark2VisionPos_Y";

                worksheet.Cells[1, 5].Value = "mark1PixelPos_X";
                worksheet.Cells[1, 6].Value = "mark1PixelPos_Y";
                worksheet.Cells[1, 7].Value = "mark2PixelPos_X";
                worksheet.Cells[1, 8].Value = "mark2PixelPos_Y";

                worksheet.Cells[1, 9].Value = "mark1CenterPos_X";
                worksheet.Cells[1, 10].Value = "mark1CenterPos_Y";
                worksheet.Cells[1, 11].Value = "mark2CenterPos_X";
                worksheet.Cells[1, 12].Value = "mark2CenterPos_Y";

                worksheet.Cells[1, 13].Value = "mark1DistancePos_X";
                worksheet.Cells[1, 14].Value = "mark1DistancePos_Y";
                worksheet.Cells[1, 15].Value = "mark2DistancePos_X";
                worksheet.Cells[1, 16].Value = "mark2DistancePos_Y";

                worksheet.Cells[1, 17].Value = "theta";

                worksheet.Cells[1, 18].Value = "mark1CenterPos_theory_X";
                worksheet.Cells[1, 19].Value = "mark1CenterPos_theory_Y";
                worksheet.Cells[1, 20].Value = "mark2CenterPos_theory_X";
                worksheet.Cells[1, 21].Value = "mark2CenterPos_theory_Y";
                worksheet.Cells[1, 22].Value = "HomoMatrix";

                worksheet.Cells[1, 23].Value = "time";

                this.signalStop = false;

                DialogResult dialog;
                AKRSPoint3D newVisionPos = new AKRSPoint3D();

                ExcuteResult upLookP1Result;
                ExcuteResult upLookP2Result;

                MatchResult upLookRes1 = null, upLookRes2 = null;
                AKRSPoint3D uplookP1ResInAxis = new AKRSPoint3D();
                AKRSPoint3D uplookP2ResInAxis = new AKRSPoint3D();

                AKRSPoint3D uplookP1ResInAxis_theory = new AKRSPoint3D();
                AKRSPoint3D uplookP2ResInAxis_theory = new AKRSPoint3D();

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

                    return;
                }

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.upLookController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

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

                // 计算上视拍照位
                AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

                AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                for (int i = 0; i < this.cycles; i++)
                {
                    this.Invoke(() => this.LbNum.Text = (i + 1).ToString());
                    if (!this.signalStop)
                    {
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

                            uplookP1ResInAxis_theory = this.bondModuleController.ConvertG0ToMachinePos(newVisionPos);

                            upLookRes1 =
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
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

                            uplookP2ResInAxis_theory = this.bondModuleController.ConvertG0ToMachinePos(newVisionPos);

                            upLookRes2 =
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                        #endregion

                        mark1VisionPos_X = upLookVisionPos1.X;
                        mark1VisionPos_Y = upLookVisionPos1.Y;
                        mark2VisionPos_X = upLookVisionPos2.X;
                        mark2VisionPos_Y = upLookVisionPos2.Y;

                        mark1PixelPos_X = upLookRes1.CenterX;
                        mark1PixelPos_Y = upLookRes1.CenterY;
                        mark2PixelPos_X = upLookRes2.CenterX;
                        mark2PixelPos_Y = upLookRes2.CenterY;

                        mark1CenterPos_X = uplookP1ResInAxis.X;
                        mark1CenterPos_Y = uplookP1ResInAxis.Y;
                        mark2CenterPos_X = uplookP2ResInAxis.X;
                        mark2CenterPos_Y = uplookP2ResInAxis.Y;

                        mark1DistancePos_X = mark1CenterPos_X - mark1VisionPos_X;
                        mark1DistancePos_Y = mark1CenterPos_Y - mark1VisionPos_Y;
                        mark2DistancePos_X = mark2CenterPos_X - mark2VisionPos_X;
                        mark2DistancePos_Y = mark2CenterPos_Y - mark2VisionPos_Y;

                        mark1CenterPos_theory_X = uplookP1ResInAxis_theory.X;
                        mark1CenterPos_theory_Y = uplookP1ResInAxis_theory.Y;
                        mark2CenterPos_theory_X = uplookP2ResInAxis_theory.X;
                        mark2CenterPos_theory_Y = uplookP2ResInAxis_theory.Y;

                        theta = Math.Atan((mark1CenterPos_Y - mark2CenterPos_Y) / (mark1CenterPos_X - mark2CenterPos_X)) * 180 / Math.PI;

                        worksheet.Cells[i + 2, 1].Value = mark1VisionPos_X;
                        worksheet.Cells[i + 2, 2].Value = mark1VisionPos_Y;
                        worksheet.Cells[i + 2, 3].Value = mark2VisionPos_X;
                        worksheet.Cells[i + 2, 4].Value = mark2VisionPos_Y;

                        worksheet.Cells[i + 2, 5].Value = mark1PixelPos_X;
                        worksheet.Cells[i + 2, 6].Value = mark1PixelPos_Y;
                        worksheet.Cells[i + 2, 7].Value = mark2PixelPos_X;
                        worksheet.Cells[i + 2, 8].Value = mark2PixelPos_Y;

                        worksheet.Cells[i + 2, 9].Value = mark1CenterPos_X;
                        worksheet.Cells[i + 2, 10].Value = mark1CenterPos_Y;
                        worksheet.Cells[i + 2, 11].Value = mark2CenterPos_X;
                        worksheet.Cells[i + 2, 12].Value = mark2CenterPos_Y;

                        worksheet.Cells[i + 2, 13].Value = mark1DistancePos_X;
                        worksheet.Cells[i + 2, 14].Value = mark1DistancePos_Y;
                        worksheet.Cells[i + 2, 15].Value = mark2DistancePos_X;
                        worksheet.Cells[i + 2, 16].Value = mark2DistancePos_Y;

                        worksheet.Cells[i + 2, 17].Value = theta;

                        worksheet.Cells[i + 2, 18].Value = mark1CenterPos_theory_X;
                        worksheet.Cells[i + 2, 19].Value = mark1CenterPos_theory_Y;
                        worksheet.Cells[i + 2, 20].Value = mark2CenterPos_theory_X;
                        worksheet.Cells[i + 2, 21].Value = mark2CenterPos_theory_Y;

                        List<float> ret = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.CCalibTransTool.BasicParam.HomoMatrix;
                        string str_ret = string.Empty;
                        foreach (var a in ret)
                        {
                            str_ret += a + "_";
                        }

                        worksheet.Cells[i + 2, 22].Value = str_ret;

                        worksheet.Cells[i + 2, 23].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        package.Save();
                    }
                    else
                    {
                        goto ReturnGlass;
                    }
                }

                this.isMoveToCameraCenterAfterVision = true;
                for (int i = 0; i < this.cycles; i++)
                {
                    this.Invoke(() => this.LbNum.Text = (i + 1).ToString());
                    if (!this.signalStop)
                    {
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

                            uplookP1ResInAxis_theory = this.bondModuleController.ConvertG0ToMachinePos(newVisionPos);

                            upLookRes1 =
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
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

                            uplookP2ResInAxis_theory = this.bondModuleController.ConvertG0ToMachinePos(newVisionPos);

                            upLookRes2 =
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                        #endregion

                        mark1VisionPos_X = upLookVisionPos1.X;
                        mark1VisionPos_Y = upLookVisionPos1.Y;
                        mark2VisionPos_X = upLookVisionPos2.X;
                        mark2VisionPos_Y = upLookVisionPos2.Y;

                        mark1PixelPos_X = upLookRes1.CenterX;
                        mark1PixelPos_Y = upLookRes1.CenterY;
                        mark2PixelPos_X = upLookRes2.CenterX;
                        mark2PixelPos_Y = upLookRes2.CenterY;

                        mark1CenterPos_X = uplookP1ResInAxis.X;
                        mark1CenterPos_Y = uplookP1ResInAxis.Y;
                        mark2CenterPos_X = uplookP2ResInAxis.X;
                        mark2CenterPos_Y = uplookP2ResInAxis.Y;

                        mark1DistancePos_X = mark1CenterPos_X - mark1VisionPos_X;
                        mark1DistancePos_Y = mark1CenterPos_Y - mark1VisionPos_Y;
                        mark2DistancePos_X = mark2CenterPos_X - mark2VisionPos_X;
                        mark2DistancePos_Y = mark2CenterPos_Y - mark2VisionPos_Y;

                        mark1CenterPos_theory_X = uplookP1ResInAxis_theory.X;
                        mark1CenterPos_theory_Y = uplookP1ResInAxis_theory.Y;
                        mark2CenterPos_theory_X = uplookP2ResInAxis_theory.X;
                        mark2CenterPos_theory_Y = uplookP2ResInAxis_theory.Y;

                        theta = Math.Atan((mark1CenterPos_Y - mark2CenterPos_Y) / (mark1CenterPos_X - mark2CenterPos_X)) * 180 / Math.PI;

                        worksheet.Cells[i + 2, 1].Value = mark1VisionPos_X;
                        worksheet.Cells[i + 2, 2].Value = mark1VisionPos_Y;
                        worksheet.Cells[i + 2, 3].Value = mark2VisionPos_X;
                        worksheet.Cells[i + 2, 4].Value = mark2VisionPos_Y;

                        worksheet.Cells[i + 2, 5].Value = mark1PixelPos_X;
                        worksheet.Cells[i + 2, 6].Value = mark1PixelPos_Y;
                        worksheet.Cells[i + 2, 7].Value = mark2PixelPos_X;
                        worksheet.Cells[i + 2, 8].Value = mark2PixelPos_Y;

                        worksheet.Cells[i + 2, 9].Value = mark1CenterPos_X;
                        worksheet.Cells[i + 2, 10].Value = mark1CenterPos_Y;
                        worksheet.Cells[i + 2, 11].Value = mark2CenterPos_X;
                        worksheet.Cells[i + 2, 12].Value = mark2CenterPos_Y;

                        worksheet.Cells[i + 2, 13].Value = mark1DistancePos_X;
                        worksheet.Cells[i + 2, 14].Value = mark1DistancePos_Y;
                        worksheet.Cells[i + 2, 15].Value = mark2DistancePos_X;
                        worksheet.Cells[i + 2, 16].Value = mark2DistancePos_Y;

                        worksheet.Cells[i + 2, 17].Value = theta;

                        worksheet.Cells[i + 2, 18].Value = mark1CenterPos_theory_X;
                        worksheet.Cells[i + 2, 19].Value = mark1CenterPos_theory_Y;
                        worksheet.Cells[i + 2, 20].Value = mark2CenterPos_theory_X;
                        worksheet.Cells[i + 2, 21].Value = mark2CenterPos_theory_Y;

                        List<float> ret = System2Module.GetInstance().UpLookModule.UpLookCameraCoordinateSystem.CCalibTransTool.BasicParam.HomoMatrix;
                        string str_ret = string.Empty;
                        foreach (var a in ret)
                        {
                            str_ret += a + "_";
                        }

                        worksheet.Cells[i + 2, 22].Value = str_ret;

                        worksheet.Cells[i + 2, 23].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        package.Save();
                    }
                    else
                    {
                        goto ReturnGlass;
                    }
                }

            ReturnGlass:
                #region 放回小标定片

                this.bondHeadController.RotateAxisT(0);

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                // 放片
                this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                this.bondModuleController.MoveToSafePos();

                #endregion
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"角度校正实验1失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.signalStop = true;
                this.Invoke(
                    () =>
                        {
                            this.BtnTest1Start.Enabled = true;
                            this.BtnTest2Start.Enabled = true;
                            this.LbNum.Text = "0";
                        });
            }
        }

        private void Method2()
        {
            try
            {
                // 换BMC
                if (!this.system2Controller.ChangeNozzleAssistance("BMC"))
                {
                    return;
                }

                // 更换治具
                this.bondHeadController.SetBMCOnBondhead();

                double mark1VisionPos_X = 0,
                       mark1VisionPos_Y = 0,
                       mark2VisionPos_X = 0,
                       mark2VisionPos_Y = 0,
                       mark1PixelPos_X = 0,
                       mark1PixelPos_Y = 0,
                       mark2PixelPos_X = 0,
                       mark2PixelPos_Y = 0,
                       mark1CenterPos_X = 0,
                       mark1CenterPos_Y = 0,
                       mark2CenterPos_X = 0,
                       mark2CenterPos_Y = 0,
                       mark1DistancePos_X = 0,
                       mark1DistancePos_Y = 0,
                       mark2DistancePos_X = 0,
                       mark2DistancePos_Y = 0,
                       theta = 0;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\角度校正实验2.xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "mark1VisionPos_X";
                worksheet.Cells[1, 2].Value = "mark1VisionPos_Y";
                worksheet.Cells[1, 3].Value = "mark2VisionPos_X";
                worksheet.Cells[1, 4].Value = "mark2VisionPos_Y";

                worksheet.Cells[1, 5].Value = "mark1PixelPos_X";
                worksheet.Cells[1, 6].Value = "mark1PixelPos_Y";
                worksheet.Cells[1, 7].Value = "mark2PixelPos_X";
                worksheet.Cells[1, 8].Value = "mark2PixelPos_Y";

                worksheet.Cells[1, 9].Value = "mark1CenterPos_X";
                worksheet.Cells[1, 10].Value = "mark1CenterPos_Y";
                worksheet.Cells[1, 11].Value = "mark2CenterPos_X";
                worksheet.Cells[1, 12].Value = "mark2CenterPos_Y";

                worksheet.Cells[1, 13].Value = "mark1DistancePos_X";
                worksheet.Cells[1, 14].Value = "mark1DistancePos_Y";
                worksheet.Cells[1, 15].Value = "mark2DistancePos_X";
                worksheet.Cells[1, 16].Value = "mark2DistancePos_Y";

                worksheet.Cells[1, 17].Value = "theta";

                worksheet.Cells[1, 23].Value = "time";

                this.signalStop = false;




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

                this.bondHeadController.RotateAxisT(-3);

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

                    return;
                }

                //if (this.isMoveToCameraCenterAfterVision)
                //{
                //    // 移动到相机中心再拍一次
                //    newVisionPos = this.upLookController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

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

                // 计算上视拍照位
                AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                AKRSPoint3D upLookVisionPos1 = upLookVisionCenterPos + new AKRSPoint3D(4, -4, 0);
                AKRSPoint3D upLookVisionPos2 = upLookVisionCenterPos + new AKRSPoint3D(-4, 4, 0);

                AKRSPoint3D upLookVisionPos1InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos1);
                AKRSPoint3D upLookVisionPos2InG0 = this.bondModuleController.ConvertMachineToG0Pos(upLookVisionPos2);

                for (int i = 0; i < 36; i++)
                {
                    this.Invoke(() => this.LbNum.Text = $"{(i + 1).ToString()}" + " / 36");
                    this.bondHeadController.RotateAxisT(-3 + i * 0.2);
                    if (!this.signalStop)
                    {
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
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes1);

                        upLookVisionPos1InG0 = new AKRSPoint3D(point3D1.X, point3D1.Y, point3D1.Z);

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
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                        #endregion

                        mark1VisionPos_X = upLookVisionPos1.X;
                        mark1VisionPos_Y = upLookVisionPos1.Y;
                        mark2VisionPos_X = upLookVisionPos2.X;
                        mark2VisionPos_Y = upLookVisionPos2.Y;

                        mark1PixelPos_X = upLookRes1.CenterX;
                        mark1PixelPos_Y = upLookRes1.CenterY;
                        mark2PixelPos_X = upLookRes2.CenterX;
                        mark2PixelPos_Y = upLookRes2.CenterY;

                        mark1CenterPos_X = uplookP1ResInAxis.X;
                        mark1CenterPos_Y = uplookP1ResInAxis.Y;
                        mark2CenterPos_X = uplookP2ResInAxis.X;
                        mark2CenterPos_Y = uplookP2ResInAxis.Y;

                        mark1DistancePos_X = mark1CenterPos_X - mark1VisionPos_X;
                        mark1DistancePos_Y = mark1CenterPos_Y - mark1VisionPos_Y;
                        mark2DistancePos_X = mark2CenterPos_X - mark2VisionPos_X;
                        mark2DistancePos_Y = mark2CenterPos_Y - mark2VisionPos_Y;

                        theta = Math.Atan((mark1CenterPos_Y - mark2CenterPos_Y) / (mark1CenterPos_X - mark2CenterPos_X)) * 180 / Math.PI;

                        upLookVisionPos2 = new AKRSPoint3D(uplookP2ResInAxis.X, uplookP2ResInAxis.Y, uplookP2ResInAxis.Z);

                        worksheet.Cells[i + 2, 1].Value = mark1VisionPos_X;
                        worksheet.Cells[i + 2, 2].Value = mark1VisionPos_Y;
                        worksheet.Cells[i + 2, 3].Value = mark2VisionPos_X;
                        worksheet.Cells[i + 2, 4].Value = mark2VisionPos_Y;

                        worksheet.Cells[i + 2, 5].Value = mark1PixelPos_X;
                        worksheet.Cells[i + 2, 6].Value = mark1PixelPos_Y;
                        worksheet.Cells[i + 2, 7].Value = mark2PixelPos_X;
                        worksheet.Cells[i + 2, 8].Value = mark2PixelPos_Y;

                        worksheet.Cells[i + 2, 9].Value = mark1CenterPos_X;
                        worksheet.Cells[i + 2, 10].Value = mark1CenterPos_Y;
                        worksheet.Cells[i + 2, 11].Value = mark2CenterPos_X;
                        worksheet.Cells[i + 2, 12].Value = mark2CenterPos_Y;

                        worksheet.Cells[i + 2, 13].Value = mark1DistancePos_X;
                        worksheet.Cells[i + 2, 14].Value = mark1DistancePos_Y;
                        worksheet.Cells[i + 2, 15].Value = mark2DistancePos_X;
                        worksheet.Cells[i + 2, 16].Value = mark2DistancePos_Y;

                        worksheet.Cells[i + 2, 17].Value = theta;

                        worksheet.Cells[i + 2, 23].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        package.Save();
                    }
                    else
                    {
                        goto ReturnGlass;
                    }
                }

                this.isMoveToCameraCenterAfterVision = true;
                for (int i = 0; i < 36; i++)
                {
                    this.Invoke(() => this.LbNum.Text = $"{(i + 1).ToString()}" + " / 36");
                    this.bondHeadController.RotateAxisT(-3 + i * 0.2);
                    if (!this.signalStop)
                    {
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
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes1);

                        upLookVisionPos1InG0 = new AKRSPoint3D(point3D1.X, point3D1.Y, point3D1.Z);

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
                                (MatchResult)this.system2Controller.UpLookCameraVision(newVisionPos, CalibrateRunPara.GetInstance().UpLookPRName, this.visionTime);
                        }

                        AKRSPoint3D point3D2 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                            this.bondModuleController.Get3DRealPosition(),
                            upLookRes2);

                        uplookP2ResInAxis = this.bondModuleController.ConvertG0ToMachinePos(point3D2);

                        #endregion

                        mark1VisionPos_X = upLookVisionPos1.X;
                        mark1VisionPos_Y = upLookVisionPos1.Y;
                        mark2VisionPos_X = upLookVisionPos2.X;
                        mark2VisionPos_Y = upLookVisionPos2.Y;

                        mark1PixelPos_X = upLookRes1.CenterX;
                        mark1PixelPos_Y = upLookRes1.CenterY;
                        mark2PixelPos_X = upLookRes2.CenterX;
                        mark2PixelPos_Y = upLookRes2.CenterY;

                        mark1CenterPos_X = uplookP1ResInAxis.X;
                        mark1CenterPos_Y = uplookP1ResInAxis.Y;
                        mark2CenterPos_X = uplookP2ResInAxis.X;
                        mark2CenterPos_Y = uplookP2ResInAxis.Y;

                        mark1DistancePos_X = mark1CenterPos_X - mark1VisionPos_X;
                        mark1DistancePos_Y = mark1CenterPos_Y - mark1VisionPos_Y;
                        mark2DistancePos_X = mark2CenterPos_X - mark2VisionPos_X;
                        mark2DistancePos_Y = mark2CenterPos_Y - mark2VisionPos_Y;

                        theta = Math.Atan((mark1CenterPos_Y - mark2CenterPos_Y) / (mark1CenterPos_X - mark2CenterPos_X)) * 180 / Math.PI;

                        upLookVisionPos2 = new AKRSPoint3D(uplookP2ResInAxis.X, uplookP2ResInAxis.Y, uplookP2ResInAxis.Z);

                        worksheet.Cells[i + 2, 1].Value = mark1VisionPos_X;
                        worksheet.Cells[i + 2, 2].Value = mark1VisionPos_Y;
                        worksheet.Cells[i + 2, 3].Value = mark2VisionPos_X;
                        worksheet.Cells[i + 2, 4].Value = mark2VisionPos_Y;

                        worksheet.Cells[i + 2, 5].Value = mark1PixelPos_X;
                        worksheet.Cells[i + 2, 6].Value = mark1PixelPos_Y;
                        worksheet.Cells[i + 2, 7].Value = mark2PixelPos_X;
                        worksheet.Cells[i + 2, 8].Value = mark2PixelPos_Y;

                        worksheet.Cells[i + 2, 9].Value = mark1CenterPos_X;
                        worksheet.Cells[i + 2, 10].Value = mark1CenterPos_Y;
                        worksheet.Cells[i + 2, 11].Value = mark2CenterPos_X;
                        worksheet.Cells[i + 2, 12].Value = mark2CenterPos_Y;

                        worksheet.Cells[i + 2, 13].Value = mark1DistancePos_X;
                        worksheet.Cells[i + 2, 14].Value = mark1DistancePos_Y;
                        worksheet.Cells[i + 2, 15].Value = mark2DistancePos_X;
                        worksheet.Cells[i + 2, 16].Value = mark2DistancePos_Y;

                        worksheet.Cells[i + 2, 17].Value = theta;

                        worksheet.Cells[i + 2, 23].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        package.Save();
                    }
                    else
                    {
                        goto ReturnGlass;
                    }
                }

            ReturnGlass:
                #region 放回小标定片

                this.bondHeadController.RotateAxisT(-3);

                // 移动到放片位
                this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                // 放片
                this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                this.bondModuleController.MoveToSafePos();

                #endregion
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"角度校正实验2失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.signalStop = true;
                this.Invoke(
                    () =>
                    {
                        this.BtnTest1Start.Enabled = true;
                        this.BtnTest2Start.Enabled = true;
                        this.LbNum.Text = "0";
                    });
            }
        }
    }
}