using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Experiment.Test
{
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.HardWares.LaserMeasureHeightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Localization;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using log4net.Core;
    using OfficeOpenXml;
    using System.Diagnostics;
    using System.IO;
    using static DevExpress.Utils.Frames.FrameHelper;

    public partial class FrmBaseTest : DevExpress.XtraEditors.XtraForm
    {
        private bool signalStop = true;

        private bool isSucceedBondZ = false;

        private bool isSucceedBondT = false;

        private LaserMeasureHeight testLM = HardwareRepositoryService.GetHardware<LaserMeasureHeight>("测试激光测高");

        /// <summary>
        /// mark识别位
        /// </summary>
        private AKRSPoint3D markLocation = new AKRSPoint3D();

        /// <summary>
        /// 取料位
        /// </summary>
        private AKRSPoint3D pickLocation = new AKRSPoint3D();

        /// <summary>
        /// pr名称
        /// </summary>
        private string prName => "BondZ下降误差分析pr";

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// Bond模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara bMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        public FrmBaseTest()
        {
            InitializeComponent();

            this.TxtPoint1X.Text = this.Point1.X.ToString();
            this.TxtPoint1Y.Text = this.Point1.Y.ToString();
            this.TxtPoint1Z.Text = this.Point1.Z.ToString();

            this.TxtPoint2X.Text = this.Point2.X.ToString();
            this.TxtPoint2Y.Text = this.Point2.Y.ToString();
            this.TxtPoint2Z.Text = this.Point2.Z.ToString();

            this.TxtPoint3X.Text = this.Point3.X.ToString();
            this.TxtPoint3Y.Text = this.Point3.Y.ToString();
            this.TxtPoint3Z.Text = this.Point3.Z.ToString();

            FormLocalizer.LocalizeForm(this);
        }

        private void BtnBondZStart_Click(object sender, EventArgs e)
        {
            this.groupControl1.Enabled = false;
            this.groupControl2.Enabled = false;
            this.BtnStart.Enabled = false;
            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("BondZ下降误差分析线程");
                        BondZMethod();
                        AKRSXtraMessageBox.Show(
                            "实验已完成，文件保存在'D://BondZ下降误差分析.xlsx'",
                            "提示",
                            MessageBoxButtons.OK);
                    });
        }

        private void BtnSetMarkPos_Click(object sender, EventArgs e)
        {
            this.markLocation = System2Module.GetInstance().BondModule.GetG0RealPosition();
            this.TxtMarkX.Text = this.markLocation.X.ToString();
            this.TxtMarkY.Text = this.markLocation.Y.ToString();
            this.TxtMarkZ.Text = this.markLocation.Z.ToString();
            System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
        }

        private void BtnSetPickPos_Click(object sender, EventArgs e)
        {
            this.pickLocation = System2Module.GetInstance().BondModule.GetG0RealPosition();
            this.TxtPickX.Text = this.pickLocation.X.ToString();
            this.TxtPickY.Text = this.pickLocation.Y.ToString();
            this.TxtPickZ.Text = this.pickLocation.Z.ToString();
        }

        private void BtnBondZPrEdit_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPr(this.prName, CameraTypeEnum.BondCamera);
        }

        private void BtnBondTStart_Click(object sender, EventArgs e)
        {
            this.groupControl1.Enabled = false;
            this.groupControl2.Enabled = false;
            this.BtnStart.Enabled = false;
            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("焊头旋转实验线程");
                        BondTMethod();
                        AKRSXtraMessageBox.Show(
                            "实验已完成，文件保存在'D://Angle.xlsx'",
                            "提示",
                            MessageBoxButtons.OK);
                    });
        }

        private void BondZMethod(bool isContinue = false)
        {
            try
            {
                isSucceedBondZ = false;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\BondZ下降误差分析.xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "markVisionPos_X";
                worksheet.Cells[1, 2].Value = "markVisionPos_Y";
                worksheet.Cells[1, 3].Value = "height";

                this.signalStop = false;
                int testNum = int.Parse(this.SpBondZTestNum.Text);
                int delayPhoto = int.Parse(this.SpDelayPhoto.Text);
                for (int i = 0; i < testNum; i++)
                {
                    if (this.signalStop)
                    {
                        return;
                    }

                    if (!MachineStateModel.GetInstance().IsOffLineWork)
                    {
                        // 去取料位
                        this.bondModuleController.MoveToG0Pos(this.pickLocation);
                    }

                    Thread.Sleep(100);

                    if (!MachineStateModel.GetInstance().IsOffLineWork)
                    {
                        // 去拍照位
                        this.bondModuleController.MoveToG0Pos(this.markLocation);
                    }

                    Thread.Sleep(delayPhoto);

                    MatchResult baseAlg = new MatchResult();
                    if (!MachineStateModel.GetInstance().IsOffLineWork)
                    {
                        // 识别
                        baseAlg = (MatchResult)VisionService.Vision(
                            this.prName,
                            System2Domain.GetInstance().BondModuleController.GetHardware(),
                            "Bond",
                            "Test",
                            false);
                    }

                    Thread.Sleep(100);

                    // 记录数据
                    worksheet.Cells[i + 2, 1].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 2].Value = baseAlg.CenterY;
                    worksheet.Cells[i + 2, 3].Value = this.pickLocation.Z;
                    package.Save();
                }

                isSucceedBondZ = true;
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"BondZ下降误差分析实验失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.signalStop = true;
                if (!isContinue)
                {
                    this.Invoke(
                        () =>
                            {
                                this.groupControl1.Enabled = true;
                                this.groupControl2.Enabled = true;
                                this.BtnStart.Enabled = true;
                            });
                }
            }
        }

        private void BondTMethod()
        {
            try
            {
                isSucceedBondT = false;

                //    // 换BMC
                //    if (!this.system2Controller.ChangeNozzleAssistance("BMC"))
                //    {
                //        return;
                //    }

                //    // 更换治具
                //    this.bondHeadController.SetBMCOnBondhead();
                //    this.bondHeadController.MoveBondZToSafePos();

                //    DialogResult dialog;

                //    #region 取小标定片

                //    this.bondHeadController.RotateAxisT(0);

                //    // 获取标定片拍照位、PR
                //    AKRSPoint3D glassCenterPosition = CalibrateRunPara.GetInstance().GlassVisionMachinePos;
                //    AKRSPoint3D glassCenterPositionInG0 =
                //        this.bondModuleController.ConvertMachineToG0Pos(glassCenterPosition);
                //    string pRName = CalibrateRunPara.GetInstance().GlassPRName;

                //Retry:
                //    // 执行定位
                //    MatchResult matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                //        glassCenterPositionInG0,
                //    pRName,
                //        10);

                //    if (matchResult1 == null)
                //    {
                //        dialog = AKRSXtraMessageBox.Show(
                //            $"PR：{pRName}  vision  failed!Please  check  glass!",
                //        "Warn",
                //            MessageBoxButtons.RetryCancel,
                //            MessageBoxIcon.Warning);

                //        if (dialog == DialogResult.Retry)
                //        {
                //            goto Retry;
                //        }

                //        return;
                //    }

                //    //if (this.isMoveToCameraCenterAfterVision)
                //    //{
                //    //    // 移动到相机中心再拍一次
                //    //    newVisionPos = this.upLookController.ConvertPixelToG0Pos(glassCenterPosition, matchResult1);

                //    //    matchResult1 = (MatchResult)this.system2Controller.BondCameraVision(
                //    //        glassCenterPositionInG0,
                //    //        pRName,
                //    //        this.visionTime);
                //    //}

                //    // 计算取片位(G0)
                //    AKRSPoint3D pickPos = this.system2Controller.GetBondVisionResultPos(matchResult1)
                //                          + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                //    // 移动到取片位
                //    this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                //    // 取放片参数
                //    BaseCarrierConfig component = new BaseCarrierConfig()
                //    {
                //        // 取片
                //        IsActivateSlowTravelBeforePickup = true,
                //        SlowTravelSpeedBeforePickup =
                //                                              this.bMCDevicePara.SlowTravelSpeedBeforePickup,
                //        SlowTravelDistanceBeforePickup =
                //                                              this.bMCDevicePara.SlowTravelDistanceBeforePickup,
                //        IsActivateSlowTravelAfterPickup = true,
                //        SlowTravelSpeedAfterPickup =
                //                                              this.bMCDevicePara.SlowTravelSpeedAfterPickup,
                //        SlowTravelDistanceAfterPickup =
                //                                              this.bMCDevicePara.SlowTravelDistanceAfterPickup,
                //        VacuumOffDelay = this.bMCDevicePara.VacuumOffDelay,
                //        PickupDelay = this.bMCDevicePara.PickupDelay,

                //        // todo:力控暂时没接
                //        PickupForceMode = ForceModeEnum.Distance,

                //        // 放片
                //        IsActivateSlowTravelBeforeBonding = true,
                //        SlowTravelSpeedBeforeBonding =
                //                                              this.bMCDevicePara.SlowTravelSpeedBeforeBonding,
                //        SlowTravelDistanceBeforeBonding =
                //                                              this.bMCDevicePara.SlowTravelDistanceBeforeBonding,
                //        IsActivateSlowTravelAfterBonding = true,
                //        SlowTravelSpeedAfterBonding =
                //                                              this.bMCDevicePara.SlowTravelSpeedAfterBonding,
                //        SlowTravelDistanceAfterBonding =
                //                                              this.bMCDevicePara.SlowTravelDistanceAfterBonding,
                //        BondingBlowDelay = this.bMCDevicePara.BondingBlowDelay,
                //        PlacementDelay = this.bMCDevicePara.PlacementDelay,
                //        IsActiveComponentDetection = true,
                //        BondingForceMode = ForceModeEnum.Distance,
                //        WeakBlowProportion = 10000
                //    };
                //    double pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos;
                //    double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                //    // 关闭吸嘴真空
                //    this.bondHeadController.CloseToolVaccum();

                //    // 取片
                //    this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);

                //    #endregion

                //    AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
                //    this.bondModuleController.MoveToG0Pos(this.bondModuleController.ConvertMachineToG0Pos(upLookVisionCenterPos));

                List<double> realAngleList = new List<double>();
                List<double> imageAngleList = new List<double>();
                List<double> codeAngleList = new List<double>();

                List<AKRSPoint2D> resultList = new List<AKRSPoint2D>();

                int cyclicNum = int.Parse(this.SpAngleCyclicNum.Text);
                int startAngle = int.Parse(this.SpStartAngle.Text);
                int stopAngle = int.Parse(this.SpStopAngle.Text);
                float angleStep = float.Parse(this.SpAngleStep.Text);
                int angleDelayPhoto = int.Parse(this.SpAngleDelayPhoto.Text);

                this.signalStop = false;
                for (int j = 0; j < cyclicNum; j++)
                {
                    for (double i = startAngle; i <= stopAngle; i += angleStep)
                    {
                        if (this.signalStop)
                        {
                            goto Save;
                        }

                        if (!MachineStateModel.GetInstance().IsOffLineWork)
                        {
                            this.BondModule.BondHead.AxisT.AbsoluteMove(i);
                        }

                        Thread.Sleep(angleDelayPhoto);
                        MatchResult resultA = new MatchResult();
                        if (!MachineStateModel.GetInstance().IsOffLineWork)
                        {
                            resultA = LocatePosition("测试", out double time);
                        }

                        resultList.Add(new AKRSPoint2D(resultA.CenterX, resultA.CenterY));
                        realAngleList.Add(i);
                        imageAngleList.Add(resultA.Angle);
                        if (MachineStateModel.GetInstance().IsOffLineWork)
                        {
                            codeAngleList.Add(0);
                        }
                        else
                        {
                            codeAngleList.Add(this.bondHeadController.GetAxisTRealPos());
                        }
                    }
                }

                isSucceedBondT = true;

            Save:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\Angle.xlsx"));
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

                //ReturnGlass:
                //#region 放回小标定片

                //this.bondHeadController.RotateAxisT(0);

                //// 移动到放片位
                //this.bondModuleController.MoveToG0Pos(pickPos.X, pickPos.Y);

                //pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 4;

                //// 放片
                //this.bondHeadController.BondAction(pickLevel, liftLevel, component, BondTypeEnum.BondOnTU);

                //this.bondModuleController.MoveToSafePos();

                //#endregion
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return;
            }
            finally
            {
                this.signalStop = true;
                this.Invoke(
                    () =>
                        {
                            this.groupControl1.Enabled = true;
                            this.groupControl2.Enabled = true;
                            this.BtnStart.Enabled = true;
                        });
            }

            MatchResult LocatePosition(string patternName, out double time)
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
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            this.groupControl1.Enabled = false;
            this.groupControl2.Enabled = false;
            this.BtnStart.Enabled = false;
            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("设备基础实验线程");
                        BondZMethod(true);
                        AKRSXtraMessageBox.Show(
                            "实验已完成，文件保存在'D://BondZ下降误差分析.xlsx'",
                            "提示",
                            MessageBoxButtons.OK);
                    }).ContinueWith(
                a =>
                    {
                        if (this.isSucceedBondZ)
                        {
                            BondTMethod();
                            AKRSXtraMessageBox.Show(
                                "实验已完成，文件保存在'D://Angle.xlsx'",
                                "提示",
                                MessageBoxButtons.OK);
                        }
                        else
                        {
                            this.Invoke(
                                () =>
                                    {
                                        this.groupControl1.Enabled = true;
                                        this.groupControl2.Enabled = true;
                                        this.BtnStart.Enabled = true;
                                    });
                        }
                    });
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            this.signalStop = true;
        }

        private void BtnBondTPrEdit_Click(object sender, EventArgs e)
        {
            //AKRSPoint3D upLookVisionCenterPos = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;
            //CalibrateRunPara.GetInstance().Save();
            //this.bondModuleController.MoveToG0Pos(this.bondModuleController.ConvertMachineToG0Pos(upLookVisionCenterPos));
            EditPr("测试");

            void EditPr(string name, AlgBeLongEnum algBeLong = AlgBeLongEnum.Calibration)
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
        }

        private void BtnSearchOne_Click(object sender, EventArgs e)
        {
            FrmSelectComponent frmSelectComponent = new FrmSelectComponent();
            frmSelectComponent.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dia = frmSelectComponent.ShowDialog();

            if (dia == DialogResult.Cancel)
            {
                return;
            }

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(frmSelectComponent.component);

            if (component == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先选择芯片！",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // 检查
            if (WaferSystemDomain.GetInstance().CheckIsReady(false, false, false, true))
            {
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);
                WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();
                Thread.Sleep(100);
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();
            }
        }

        private void BtnSearchDieContinuePrEdit_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr("芯片位置检测pr");
        }

        private void BtnSearchDieContinue_Click(object sender, EventArgs e)
        {
            FrmSelectComponent frmSelectComponent = new FrmSelectComponent();
            frmSelectComponent.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dia = frmSelectComponent.ShowDialog();

            if (dia == DialogResult.Cancel)
            {
                return;
            }

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(frmSelectComponent.component);

            if (component == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先选择芯片！",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            bool isRecordSearchData = this.IsRecordSearchData.Checked;
            int delay = (int)this.SpDelaySearchDieContinue.Value;

            // 检查
            if (WaferSystemDomain.GetInstance().CheckIsReady(false, true, isRecordSearchData, false))
            {
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);
                WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();
            }
        }

        private void BtnBondGoHomePrEdit_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPr("标定片模板1", CameraTypeEnum.BondCamera);
        }

        private void BtnBondGoHomePr2Edit_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPr("标定片模板2", CameraTypeEnum.BondCamera);
        }

        private void BtnBondGoHomePr3Edit_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPr("标定片模板3", CameraTypeEnum.BondCamera);
        }

        private void BtnBondGoHomeStart_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("Bond回零精度实验线程");
                        this.signalStop = false;
                        BondNoGoHomeMethod("D:\\Bond回零精度实验-不回零反复跑点.xlsx");
                        Thread.Sleep(1000);
                        if (this.signalStop)
                        {
                            return;
                        }

                        BondGoHomeNoZMethod("D:\\Bond回零精度实验-反复回零跑点NoZ.xlsx");
                        Thread.Sleep(1000);
                        if (this.signalStop)
                        {
                            return;
                        }

                        //BondGoHomeWithZMethod("D:\\Bond回零精度实验-反复回零跑点WithZ.xlsx");
                        //Thread.Sleep(1000);
                        //if (this.signalStop)
                        //{
                        //    return;
                        //}

                        if (CheckEditIsTestSixHour.Checked)
                        {
                            Thread.Sleep(3600 * 6 * 1000);

                            BondNoGoHomeMethod("D:\\Bond回零精度实验-不回零反复跑点-6小时后.xlsx");
                            Thread.Sleep(1000);
                            if (this.signalStop)
                            {
                                return;
                            }

                            BondGoHomeNoZMethod("D:\\Bond回零精度实验-反复回零跑点NoZ-6小时后.xlsx");
                            Thread.Sleep(1000);
                            if (this.signalStop)
                            {
                                return;
                            }

                            //BondGoHomeWithZMethod("D:\\Bond回零精度实验-反复回零跑点WithZ-6小时后.xlsx");
                            //Thread.Sleep(1000);
                            //if (this.signalStop)
                            //{
                            //    return;
                            //}
                        }

                        this.signalStop = true;
                        AKRSXtraMessageBox.Show(
                            "实验已完成，文件保存在'D://Bond回零精度实验.xlsx'",
                            "提示",
                            MessageBoxButtons.OK);
                    });
        }

        private void BtnWaferCameraGoHomePrEdit_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr("晶圆相机标定模板");
        }

        private void BtnWaferCameraGoHomeStart_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("晶圆相机回零精度实验线程");
                        this.WaferCameraGoHomeMethod();
                        AKRSXtraMessageBox.Show(
                            "实验已完成，文件保存在'D://晶圆相机回零精度实验.xlsx'",
                            "提示",
                            MessageBoxButtons.OK);
                    });
        }

        /// <summary>
        /// 不回零反复跑点
        /// </summary>
        private void BondNoGoHomeMethod(string path)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo(path));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "markVisionPos1_X";
                worksheet.Cells[1, 2].Value = "markVisionPos1_Y";
                worksheet.Cells[1, 3].Value = "markVisionPos2_X";
                worksheet.Cells[1, 4].Value = "markVisionPos2_Y";
                worksheet.Cells[1, 5].Value = "markVisionPos3_X";
                worksheet.Cells[1, 6].Value = "markVisionPos3_Y";
                //worksheet.Cells[1, 7].Value = "height";

                int testNum = int.Parse(this.SpGoHomeTestNum.Text);
                for (int i = 0; i < testNum; i++)
                {
                    if (this.signalStop)
                    {
                        return;
                    }

                    this.bondModuleController.MoveToG0Pos(this.Point1);
                    Thread.Sleep(100);

                    MatchResult baseAlg = new MatchResult();
                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板1",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    //double value = testLM.GetValue();

                    worksheet.Cells[i + 2, 1].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 2].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point2);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板2",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    worksheet.Cells[i + 2, 3].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 4].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point3);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板3",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    // 记录数据
                    worksheet.Cells[i + 2, 5].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 6].Value = baseAlg.CenterY;
                    //worksheet.Cells[i + 2, 7].Value = value;
                    package.Save();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                this.signalStop = true;
                AKRSXtraMessageBox.Show(
                    $"Bond回零精度实验失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                //this.signalStop = true;
            }
        }

        /// <summary>
        /// 反复回零跑点
        /// </summary>
        private void BondGoHomeNoZMethod(string path)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo(path));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "markVisionPos1_X";
                worksheet.Cells[1, 2].Value = "markVisionPos1_Y";
                worksheet.Cells[1, 3].Value = "markVisionPos2_X";
                worksheet.Cells[1, 4].Value = "markVisionPos2_Y";
                worksheet.Cells[1, 5].Value = "markVisionPos3_X";
                worksheet.Cells[1, 6].Value = "markVisionPos3_Y";
                worksheet.Cells[1, 7].Value = "height";

                int testNum = int.Parse(this.SpGoHomeTestNum.Text);
                for (int i = 0; i < testNum; i++)
                {
                    if (this.signalStop)
                    {
                        return;
                    }

                    //BondModule.BondHead.AxisZ.GoHome();                    
                    BondModule.BondAxisY.GoHome();
                    BondModule.BondAxisX.GoHome();

                    Thread.Sleep(300);

                    double value = 0;
                    try
                    {
                        value = testLM.GetValue();
                    }
                    catch (Exception)
                    {
                        value = 0;
                    }

                    this.bondModuleController.MoveToG0Pos(this.Point1);
                    Thread.Sleep(100);

                    MatchResult baseAlg = new MatchResult();
                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板1",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    worksheet.Cells[i + 2, 1].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 2].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point2);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板2",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    worksheet.Cells[i + 2, 3].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 4].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point3);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板3",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    // 记录数据
                    worksheet.Cells[i + 2, 5].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 6].Value = baseAlg.CenterY;
                    worksheet.Cells[i + 2, 7].Value = value;
                    package.Save();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                this.signalStop = true;
                AKRSXtraMessageBox.Show(
                    $"Bond回零精度实验失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                //this.signalStop = true;
            }
        }

        /// <summary>
        /// 反复回零跑点
        /// </summary>
        private void BondGoHomeWithZMethod(string path)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo(path));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "markVisionPos1_X";
                worksheet.Cells[1, 2].Value = "markVisionPos1_Y";
                worksheet.Cells[1, 3].Value = "markVisionPos2_X";
                worksheet.Cells[1, 4].Value = "markVisionPos2_Y";
                worksheet.Cells[1, 5].Value = "markVisionPos3_X";
                worksheet.Cells[1, 6].Value = "markVisionPos3_Y";

                int testNum = int.Parse(this.SpGoHomeTestNum.Text);
                for (int i = 0; i < testNum; i++)
                {
                    if (this.signalStop)
                    {
                        return;
                    }

                    BondModule.BondHead.AxisZ.GoHome();
                    BondModule.BondAxisX.GoHome();
                    BondModule.BondAxisY.GoHome();

                    this.bondModuleController.MoveToG0Pos(this.Point1);
                    Thread.Sleep(100);

                    MatchResult baseAlg = new MatchResult();
                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板1",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    worksheet.Cells[i + 2, 1].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 2].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point2);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板2",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    worksheet.Cells[i + 2, 3].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 4].Value = baseAlg.CenterY;

                    this.bondModuleController.MoveToG0Pos(this.Point3);
                    Thread.Sleep(100);

                    baseAlg = (MatchResult)VisionService.Vision(
                        "标定片模板3",
                        System2Domain.GetInstance().BondModuleController.GetHardware(),
                        "Bond",
                        "",
                        false);

                    // 记录数据
                    worksheet.Cells[i + 2, 5].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 6].Value = baseAlg.CenterY;
                    package.Save();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                this.signalStop = true;
                AKRSXtraMessageBox.Show(
                    $"Bond回零精度实验失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                //this.signalStop = true;
            }
        }

        private void WaferCameraGoHomeMethod()
        {
            try
            {
                AKRSPoint3D temp = WaferSubController.GetInstance().WaferTableController.WaferCameraAxisZG0Pos;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage package = new ExcelPackage(new FileInfo($"D:\\晶圆相机回零精度实验.xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");
                worksheet.Cells[1, 1].Value = "markVisionPos_X";
                worksheet.Cells[1, 2].Value = "markVisionPos_Y";

                this.signalStop = false;
                int testNum = int.Parse(this.SpGoHomeTestNum.Text);
                for (int i = 0; i < testNum; i++)
                {
                    if (this.signalStop)
                    {
                        return;
                    }

                    WaferSubModule.GetInstance().WaferTable.WaferCameraAxisZ.GoHome();
                    Thread.Sleep(100);

                    WaferSubController.GetInstance().WaferTableController.MoveWaferCameraAxisZToG0Pos(temp);
                    Thread.Sleep(100);

                    MatchResult baseAlg = new MatchResult();
                    baseAlg = (MatchResult)VisionService.Vision(
                        "晶圆相机标定模板",
                        WaferSubController.GetInstance().WaferTableController.GetPrHardware(CarrierTypeEnum.Wafer),
                        "Bond",
                        "",
                        false);

                    // 记录数据
                    worksheet.Cells[i + 2, 1].Value = baseAlg.CenterX;
                    worksheet.Cells[i + 2, 2].Value = baseAlg.CenterY;
                    package.Save();

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"晶圆相机回零精度实验失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.signalStop = true;
            }
        }

        private AKRSPoint3D Point1 => MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint1;
        private AKRSPoint3D Point2 => MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint2;
        private AKRSPoint3D Point3 => MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint3;
        private void BtnPoint1_Click(object sender, EventArgs e)
        {
            MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint1 = System2Module.GetInstance().BondModule.GetG0RealPosition();
            MachineHardwareConfiguration.GetInstance().Save();
            this.TxtPoint1X.Text = this.Point1.X.ToString();
            this.TxtPoint1Y.Text = this.Point1.Y.ToString();
            this.TxtPoint1Z.Text = this.Point1.Z.ToString();
        }

        private void BtnPoint2_Click(object sender, EventArgs e)
        {
            MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint2 = System2Module.GetInstance().BondModule.GetG0RealPosition();
            MachineHardwareConfiguration.GetInstance().Save();
            this.TxtPoint2X.Text = this.Point2.X.ToString();
            this.TxtPoint2Y.Text = this.Point2.Y.ToString();
            this.TxtPoint2Z.Text = this.Point2.Z.ToString();
        }

        private void BtnPoint3_Click(object sender, EventArgs e)
        {
            MachineHardwareConfiguration.GetInstance().BondGoHomeTestPoint3 = System2Module.GetInstance().BondModule.GetG0RealPosition();
            MachineHardwareConfiguration.GetInstance().Save();
            this.TxtPoint3X.Text = this.Point3.X.ToString();
            this.TxtPoint3Y.Text = this.Point3.Y.ToString();
            this.TxtPoint3Z.Text = this.Point3.Z.ToString();
        }

        private void BtnBondCameraSearchEjectionCenterPrEdit_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPr("Bond相机识别顶针中心模板", CameraTypeEnum.BondCamera);
        }

        private void BtnMoveBondCamera_Click(object sender, EventArgs e)
        {
            // 运动到位            
            this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.X, CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.Y, 0) -
                new AKRSPoint3D(CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X, CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y, 0));
        }

        private void BtnSearchEjectionCenter_Click(object sender, EventArgs e)
        {
            MatchResult baseAlg = new MatchResult();
            baseAlg = (MatchResult)VisionService.Vision(
                "Bond相机识别顶针中心模板",
                System2Domain.GetInstance().BondModuleController.GetHardware(),
                "Bond",
                "",
                false);

            AKRSPoint2D point1 = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(0, 0), new MatchResult(baseAlg.CenterX, baseAlg.CenterY, 0), "BondCameraCoordinateSystem");

            // 保存
            WaferSubDevicePara.GetInstance().EjectDevicePara.CurrentSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = new AKRSPoint3D(point1.X, point1.Y,0);
            EjectionConfigRepository.GetInstance().Save();
        }
    }
}