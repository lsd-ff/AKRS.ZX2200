namespace AKRS.ZX2200.Experiment.RepeatPositionAccuracy
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;

    using DevExpress.XtraEditors;

    using log4net.Core;

    using OfficeOpenXml;

    using LicenseContext = OfficeOpenXml.LicenseContext;

    /// <summary>
    /// 重复走位精度实验、震动
    /// </summary>
    public partial class FrmRepeatPositionAccuracy : DevExpress.XtraEditors.XtraForm
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
        /// bond 模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// BMC设备参数
        /// </summary>
        private BMCDevicePara BMCDevicePara => BondDevicePara.GetInstance().BMCDevicePara;

        /// <summary>
        /// 标定参数
        /// </summary>
        private CalibrateRunPara CalibratePara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// 是否定位
        /// </summary>
        private bool isContinue = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmRepeatPositionAccuracy()
        {
            this.InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        /// <summary>
        /// 取标定片的位置
        /// </summary>
        private AKRSPoint3D pickCalibrationPlatePos;

        /// <summary>
        ///  取标定片
        /// </summary>
        private void PickCalibrationPlate()
        {
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

            double pickLevel = this.CalibratePara.GlassPickZMachinePos;
            double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

            // 先取标定片 
            this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.GlassVisionMachinePos);

            // 定位小圆并移动至相机中心
            MatchResult smallMatchResult = this.LocatePosition(this.CalibratePara.GlassPRName);

            CalibService.MoveToCamCenterCoo(this.bondModule.BondAxisX, this.bondModule.BondAxisY, smallMatchResult, "BondCameraCoordinateSystem");

            this.pickCalibrationPlatePos = this.bondModuleController.Get3DRealPosition() + this.CalibratePara.BondRotateCenterToCamOffset;
            this.pickCalibrationPlatePos.Z = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z; 

            this.bondModuleController.MoveSafeBondXYZ(this.pickCalibrationPlatePos);

            // 取标
            this.bondHeadController.PickAction(pickLevel, component, liftLevel, PickTypeEnum.CarrierWithWaffle);
            #endregion
        }

        /// <summary>
        /// 放回标定片
        /// </summary>
        private void PutbackCalibrationPlate()
        {
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

            double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

            if (this.pickCalibrationPlatePos == null || this.pickCalibrationPlatePos.X == 0)
            {
                AKRSXtraMessageBox.Show("无法手动放回标定片");
                return;
            }

            // 放标
            this.bondModuleController.MoveSafeBondXYZ(this.pickCalibrationPlatePos);

            double pickLevel = CalibrateRunPara.GetInstance().GlassPickZMachinePos + 3;

            // 放片
            this.bondHeadController.BondAction(
                pickLevel,
                liftLevel,
                component,
                BondTypeEnum.BondOnTU);
        }

        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatPositionAccuracy()
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                if (isUpLook)
                {
                   this.PickCalibrationPlate();
                }

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);
                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();

                for (int i = 0; i < times; i++)
                {
                    this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                    if (isBMC)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        moveParaX.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.X;
                        moveParaY.TargetPosition = -21.3914;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);

                        moveParaX.TargetPosition = this.CalibratePara.BMCVisionMachinePos.X;
                        moveParaY.TargetPosition = this.CalibratePara.BMCVisionMachinePos.Y;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);
                        this.bondHeadController.MoveAxisZ(this.CalibratePara.BMCVisionMachinePos.Z);

                        Thread.Sleep(delay);
                        BMCMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultBMC = this.LocatePosition(this.CalibratePara.BmcPRName);

                        Bitmap bmcImage = resultBMC.OutPutImg1;

                        bmcImage.Save("D:\\ExperimentSystem\\BMCImage\\" + DateTime.Now.ToString("MMddHHmmss") + "BMC.bmp");
                    }

                    if (isUpLook)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        moveParaX.TargetPosition = this.CalibratePara.BMCVisionMachinePos.X;
                        moveParaY.TargetPosition = -21.3914;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);

                        moveParaX.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.X;
                        moveParaY.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.Y;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);
                        this.bondHeadController.MoveAxisZ(this.CalibratePara.GlassUpLookVisionMachinePos.Z);

                        Thread.Sleep(delay);
                        UpLookMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);

                        Bitmap uplookImage =
                             resultUpLook.OutPutImg1;


                        uplookImage.Save("D:\\ExperimentSystem\\UpLookImage\\" + DateTime.Now.ToString("MMddHHmmss") + "UpLook.bmp");
                    }

                    if (isTemp)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        AKRSPoint3D point = this.bondModuleController.ConvertG0ToMachinePos(
                            BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);
                        moveParaX.TargetPosition = point.X;
                        moveParaY.TargetPosition = point.Y;

                        this.bondModuleController.MoveSafeBondXYZ(point);

                        Thread.Sleep(delay);
                        TempMachinePos = this.bondModuleController.Get2DRealPosition();
                        
                        resultTemp = this.LocatePosition("上视温漂测试Mark");

                        Bitmap tempImage =
                          resultTemp.OutPutImg1;

                        tempImage.Save("D:\\ExperimentSystem\\TempImage\\" + DateTime.Now.ToString("MMddHHmmss") + "Temp.bmp");
                    }

                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultTemp.CenterX;
                    result.TempCenterY = resultTemp.CenterY;
                    result.BmcCenterMachinePosX = BMCMachinePos.X;
                    result.BmcCenterMachinePosY = BMCMachinePos.Y;
                    result.UpLookCenterMachinePosX = UpLookMachinePos.X;
                    result.UpLookCenterMachinePosY = UpLookMachinePos.Y;
                    result.TempCenterMachinePosX = TempMachinePos.X;
                    result.TempCenterMachinePosY = TempMachinePos.Y;
                    // result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }

                if (isUpLook)
                {
                    this.PutbackCalibrationPlate();
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "Temp定位X";
                worksheet.Cells[1, 11].Value = "Temp定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();

                //DialogResult dialog = AKRSXtraMessageBox.Show(
                //    "the experiment is over \n"
                //    + "Result file location: \n" + @"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }


        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatXPositionAccuracy(bool ismove)
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);

                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultIPTLook = new MatchResult();

                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();


                // 移动到安全高度
                this.bondHeadController.MoveBondZToSafePos();


                AKRSPoint3D leftPoint = new AKRSPoint3D(-116.2496, -142.438, -28.2324);

                // 移动到左边拍摄点
                this.bondModule.MoveBondXY(leftPoint.X, leftPoint.Y);

                // Z 轴下降到拍摄点
                this.bondHeadController.MoveAxisZ(leftPoint.Z);


                for (int i = 0; i < times; i++)
                {
                    // isUpLook
                    if (true)
                    {
                        if (ismove)
                        {
                            // 移动到右侧
                            moveParaX.TargetPosition = 122.5;

                            // 移动到右边边拍摄点
                            this.bondModuleController.MoveAxisX(moveParaX);

                            // 移动到左边拍摄点
                            moveParaX.TargetPosition = leftPoint.X;
                            this.bondModuleController.MoveAxisX(moveParaX);
                        }

                        Thread.Sleep(delay);

                        UpLookMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                        Thread.Sleep(200);
                        resultIPTLook = this.LocatePosition("TestIPTPR");

                        Bitmap uplookImage =
                            resultUpLook.OutPutImg1;

                        Bitmap IPTImage =
                            resultUpLook.OutPutImg1;

                        try
                        {
                            uplookImage.Save(
                                "D:\\ExperimentSystem\\UpLookImageX\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss")
                                                                       + "UpLook.bmp");
                            IPTImage.Save(
                                "D:\\ExperimentSystem\\IPTImageX\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss") + "Ipt.bmp");
                        }
                        catch
                        {
                        }
                    }


                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultIPTLook.CenterX;
                    result.TempCenterY = resultIPTLook.CenterY;
                    result.BmcCenterMachinePosX = 0;
                    result.BmcCenterMachinePosY = 0;
                    result.UpLookCenterMachinePosX = 0;
                    result.UpLookCenterMachinePosY = 0;
                    result.TempCenterMachinePosX = 0;
                    result.TempCenterMachinePosY = 0;
                   // result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTestX" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "IPT定位X";
                worksheet.Cells[1, 11].Value = "IPT定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();

                //DialogResult dialog = AKRSXtraMessageBox.Show(
                //    "the experiment is over \n"
                //    + "Result file location: \n" + @"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }


        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatYPositionAccuracy(bool ismove)
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaZ = new MovePara
                                         {
                                             Vel = 50,
                                             Acc = 500,
                                             Dec = 500,
                                             Jerk = 0.06
                                         };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);

                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultIPTLook = new MatchResult();

                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();


                // 移动到安全高度
                this.bondHeadController.MoveBondZToSafePos();


                AKRSPoint3D leftPoint = new AKRSPoint3D(-116.2496, -142.438, -28.2324);  // -21.2

                // 移动到左边拍摄点
                this.bondModule.MoveBondXY(leftPoint.X, leftPoint.Y);

                // Z 轴下降到拍摄点
                this.bondHeadController.MoveAxisZ(leftPoint.Z);


                for (int i = 0; i < times; i++)
                {
                    // isUpLook
                    if (true)
                    {
                        if (ismove)
                        {
                            // 1.缓慢抬起Z   
                            moveParaZ.TargetPosition = -21.2;
                            this.bondHeadController.MoveAxisZ(moveParaZ);

                            // 移动到里侧
                            moveParaY.TargetPosition = 73;
                            this.bondModuleController.MoveAxisY(moveParaY);


                            // 移动到左边拍摄点
                            moveParaY.TargetPosition = leftPoint.Y;
                            this.bondModuleController.MoveAxisY(moveParaY);

                            // Z 轴下降到拍摄点
                            moveParaZ.TargetPosition = leftPoint.Z;
                            this.bondHeadController.MoveAxisZ(moveParaZ);
                        }

                        Thread.Sleep(delay);

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                        Thread.Sleep(200);
                        resultIPTLook = this.LocatePosition("TestIPTPR");

                        Bitmap uplookImage =
                             resultUpLook.OutPutImg1;

                        Bitmap IPTImage =
                            resultIPTLook.OutPutImg1;

                        try
                        {
                        uplookImage.Save("D:\\ExperimentSystem\\UpLookImageY\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss") + "UpLook.bmp");
                        IPTImage.Save("D:\\ExperimentSystem\\IPTImageY\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss") + "Ipt.bmp");
                        }
                        catch
                        {
                        }
                    }


                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultIPTLook.CenterX;
                    result.TempCenterY = resultIPTLook.CenterY;
                    result.BmcCenterMachinePosX = 0;
                    result.BmcCenterMachinePosY = 0;
                    result.UpLookCenterMachinePosX = 0;
                    result.UpLookCenterMachinePosY = 0;
                    result.TempCenterMachinePosX = 0;
                    result.TempCenterMachinePosY = 0;
                   // result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }


                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTestY" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "IPT定位X";
                worksheet.Cells[1, 11].Value = "IPT定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }

        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatZPositionAccuracy(bool ismove)
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);

                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultIPTLook = new MatchResult();

                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();

                // 移动到安全高度
                this.bondHeadController.MoveBondZToSafePos();


                AKRSPoint3D leftPoint = new AKRSPoint3D(-116.2496, -142.438, -28.2324);  // -21.2

                // 移动到左边拍摄点
                this.bondModule.MoveBondXY(leftPoint.X, leftPoint.Y);

                // Z 轴下降到拍摄点
                this.bondHeadController.MoveAxisZ(leftPoint.Z);


                for (int i = 0; i < times; i++)
                {
                    // isUpLook
                    if (true)
                    {
                        if (ismove)
                        {
                            // 1.缓慢抬起Z   
                            this.bondHeadController.MoveZAxis(1);

                            // 升温
                            Thread.Sleep(3000);

                            // Z 轴下降到拍摄点
                            this.bondHeadController.MoveAxisZ(leftPoint.Z);
                        }

                        Thread.Sleep(delay);

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                        Thread.Sleep(200);
                        resultIPTLook = this.LocatePosition("TestIPTPR");

                        Bitmap uplookImage =
                           resultUpLook.OutPutImg1;

                        Bitmap IPTImage =
                           resultIPTLook.OutPutImg1;

                        try
                        {
                            uplookImage.Save(
                                "D:\\ExperimentSystem\\UpLookImageZ\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss")
                                                                       + "UpLook.bmp");
                            IPTImage.Save(
                                "D:\\ExperimentSystem\\IPTImageZ\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss") + "Ipt.bmp");
                        }
                        catch
                        {
                        }
                    }


                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultIPTLook.CenterX;
                    result.TempCenterY = resultIPTLook.CenterY;
                    result.BmcCenterMachinePosX = 0;
                    result.BmcCenterMachinePosY = 0;
                    result.UpLookCenterMachinePosX = 0;
                    result.UpLookCenterMachinePosY = 0;
                    result.TempCenterMachinePosX = 0;
                    result.TempCenterMachinePosY = 0;
                  //  result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }


                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTestZ" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "IPT定位X";
                worksheet.Cells[1, 11].Value = "IPT定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }


        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatZBMCPositionAccuracy(bool ismove)
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);

                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultIPTLook = new MatchResult();

                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();


                // 移动到安全高度
                this.bondHeadController.MoveBondZToSafePos();


                AKRSPoint3D bmcPoint = new AKRSPoint3D(-116.2496, -142.438, -28.2324);  // -21.2

                // 移动到BMC拍摄点
                this.bondModule.MoveBondXY(bmcPoint.X, bmcPoint.Y);

                // Z 轴下降到拍摄点
                this.bondHeadController.MoveAxisZ(bmcPoint.Z);

                for (int i = 0; i < times; i++)
                {
                    // isUpLook
                    if (true)
                    {
                        if (ismove)
                        {
                            // 1.缓慢抬起Z   
                            this.bondHeadController.MoveZAxis(1);

                            // 升温
                            Thread.Sleep(3000);

                            // Z 轴下降到拍摄点
                            this.bondHeadController.MoveAxisZ(bmcPoint.Z);
                        }

                        Thread.Sleep(delay);

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                        Thread.Sleep(200);
                        resultIPTLook = this.LocatePosition("TestIPTPR");

                        Bitmap uplookImage =
                          resultUpLook.OutPutImg1;

                        Bitmap IPTImage =
                           resultIPTLook.OutPutImg1;

                        try
                        {
                            uplookImage.Save(
                                "D:\\ExperimentSystem\\UpLookImageZBMC\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss")
                                                                       + "UpLook.bmp");
                            IPTImage.Save(
                                "D:\\ExperimentSystem\\IPTImageZBMC\\" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("MMddHHmmss") + "Ipt.bmp");
                        }
                        catch
                        {
                        }
                    }


                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultIPTLook.CenterX;
                    result.TempCenterY = resultIPTLook.CenterY;
                    result.BmcCenterMachinePosX = 0;
                    result.BmcCenterMachinePosY = 0;
                    result.UpLookCenterMachinePosX = 0;
                    result.UpLookCenterMachinePosY = 0;
                    result.TempCenterMachinePosX = 0;
                    result.TempCenterMachinePosY = 0;
                   // result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }


                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTestZBMC" + "IsMove" + ismove.ToString() + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "IPT定位X";
                worksheet.Cells[1, 11].Value = "IPT定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }


        public void TestShakePositionAccuracy()
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();
                this.isContinue = true;

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;

                if (isUpLook)
                {
                    this.PickCalibrationPlate();
                }

                this.bondHeadController.MoveBondZToSafePos();
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);

                MatchResult resultUpLook = new MatchResult();

                for (int i = 0; i < times; i++)
                {
                    // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                    if (isUpLook)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        moveParaX.TargetPosition = this.CalibratePara.BMCVisionMachinePos.X;
                        moveParaY.TargetPosition = -21.3914;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);

                        moveParaX.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.X;
                        moveParaY.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.Y;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);
                        this.bondHeadController.MoveAxisZ(this.CalibratePara.GlassUpLookVisionMachinePos.Z);

                        Thread.Sleep(delay);

                        while (this.isContinue)
                        {
                            resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                            Result result = new Result();
                            result.Index = index++;
                            result.Speed = moveParaX.Vel;
                            result.AccSpeed = moveParaX.Acc;
                            result.Jerk = moveParaX.Jerk;
                            result.SleepTime = delay;
                            result.UpLookCenterX = resultUpLook.CenterX;
                            result.UpLookCenterY = resultUpLook.CenterY;

                          //  result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                            resultA1List.Add(result);
                        }
                    }
                }

                if (isUpLook)
                {
                    this.PutbackCalibrationPlate();
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "UpLook定位X";
                worksheet.Cells[1, 7].Value = "UpLook定位Y";
                worksheet.Cells[1, 8].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();

                //DialogResult dialog = AKRSXtraMessageBox.Show(
                //    "the experiment is over \n"
                //    + "Result file location: \n" + @"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);
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
        /// 开始定位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            Task.Run(() => { 
                this.TestRepeatPositionAccuracy();
                Thread.Sleep(7200);
                this.TestRepeatPositionAccuracy();
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() => TestAlg.GetInstance().OnlyOnePointDispense(50000, 500));
        }

        /// <summary>
        /// 取标定片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtPickCalibPlate_Click(object sender, EventArgs e)
        {
            this.PickCalibrationPlate();
        }

        /// <summary>
        /// 放标定片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtPutbackCalibPlate_Click(object sender, EventArgs e)
        {
            this.PutbackCalibrationPlate();
        }

        /// <summary>
        /// 连续实验任务
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtStartTask_Click(object sender, EventArgs e)
        {
           DialogResult dr =  AKRSXtraMessageBox.Show(
                "请安装BMC 吸嘴 并手动取标定片. YES: 已取标定片继续执行   NO: 取消",
                "警告",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

           if (dr == DialogResult.No)
           {
               return;
           }

           Task.Run(
               () =>
                   {
                       this.TestRepeatXPositionAccuracy(false);
                       this.TestRepeatXPositionAccuracy(true);
                       this.TestRepeatXPositionAccuracy(false);

                       this.TestRepeatYPositionAccuracy(true);
                       this.TestRepeatYPositionAccuracy(false);

                       AKRSXtraMessageBox.Show("测试完成");
                       // this.TestRepeatZPositionAccuracy(true);
                       // this.TestRepeatZPositionAccuracy(false);
                   });
            
            
        }



        /// <summary>
        ///  测试重复走位精度实验方法
        /// </summary>
        public void TestRepeatBMCXPositionAccuracy()
        {
            try
            {
                int index = 0;
                List<Result> resultA1List = new List<Result>();

                MovePara moveParaX = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                MovePara moveParaY = new MovePara
                {
                    Vel = Convert.ToDouble(this.SpSpeed.Value),
                    Acc = Convert.ToDouble(this.SpAcc.Value),
                    Dec = Convert.ToDouble(this.SpAcc.Value),
                    Jerk = Convert.ToDouble(this.SpJerk.Value)
                };

                bool isUpLook = this.ChkUplook.Checked;
                bool isBMC = this.ChkBMC.Checked;
                bool isTemp = this.ChkTemp.Checked;

                
                int times = Convert.ToInt32(this.SpTimes.Value);
                int delay = Convert.ToInt32(this.SpDelay.Value);
                MatchResult resultBMC = new MatchResult();
                MatchResult resultUpLook = new MatchResult();
                MatchResult resultTemp = new MatchResult();

                AKRSPoint2D BMCMachinePos = new AKRSPoint2D();
                AKRSPoint2D UpLookMachinePos = new AKRSPoint2D();
                AKRSPoint2D TempMachinePos = new AKRSPoint2D();

                for (int i = 0; i < times; i++)
                {
                    this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                    if (isBMC)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        moveParaX.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.X;
                        moveParaY.TargetPosition = -21.3914;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);

                        moveParaX.TargetPosition = this.CalibratePara.BMCVisionMachinePos.X;
                        moveParaY.TargetPosition = this.CalibratePara.BMCVisionMachinePos.Y;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);
                        this.bondHeadController.MoveAxisZ(this.CalibratePara.BMCVisionMachinePos.Z);

                        Thread.Sleep(delay);
                        BMCMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultBMC = this.LocatePosition(this.CalibratePara.BmcPRName);

                        Bitmap bmcImage =
                            resultBMC.OutPutImg1;

                        bmcImage.Save("D:\\ExperimentSystem\\BMCImage\\" + DateTime.Now.ToString("MMddHHmmss") + "BMC.bmp");
                    }

                    if (isUpLook)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        moveParaX.TargetPosition = this.CalibratePara.BMCVisionMachinePos.X;
                        moveParaY.TargetPosition = -21.3914;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);

                        moveParaX.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.X;
                        moveParaY.TargetPosition = this.CalibratePara.GlassUpLookVisionMachinePos.Y;

                        this.bondModuleController.MoveBondXY(moveParaX, moveParaY);
                        this.bondHeadController.MoveAxisZ(this.CalibratePara.GlassUpLookVisionMachinePos.Z);

                        Thread.Sleep(delay);
                        UpLookMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);

                        Bitmap uplookImage =
                           resultUpLook.OutPutImg1;

                        uplookImage.Save("D:\\ExperimentSystem\\UpLookImage\\" + DateTime.Now.ToString("MMddHHmmss") + "UpLook.bmp");
                    }

                    if (isTemp)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        AKRSPoint3D point = this.bondModuleController.ConvertG0ToMachinePos(
                            BondDevicePara.GetInstance().BondHeadParam.UpLookMarkVisionPos1);
                        moveParaX.TargetPosition = point.X;
                        moveParaY.TargetPosition = point.Y;

                        this.bondModuleController.MoveSafeBondXYZ(point);

                        Thread.Sleep(delay);
                        TempMachinePos = this.bondModuleController.Get2DRealPosition();

                        resultTemp = this.LocatePosition("上视温漂测试Mark");

                        Bitmap tempImage =
                           resultTemp.OutPutImg1;

                        tempImage.Save("D:\\ExperimentSystem\\TempImage\\" + DateTime.Now.ToString("MMddHHmmss") + "Temp.bmp");
                    }

                    Result result = new Result();
                    result.Index = index++;
                    result.Speed = moveParaX.Vel;
                    result.AccSpeed = moveParaX.Acc;
                    result.Jerk = moveParaX.Jerk;
                    result.SleepTime = delay;
                    result.BmcCenterX = resultBMC.CenterX;
                    result.BmcCenterY = resultBMC.CenterY;
                    result.UpLookCenterX = resultUpLook.CenterX;
                    result.UpLookCenterY = resultUpLook.CenterY;
                    result.TempCenterX = resultTemp.CenterX;
                    result.TempCenterY = resultTemp.CenterY;
                    result.BmcCenterMachinePosX = BMCMachinePos.X;
                    result.BmcCenterMachinePosY = BMCMachinePos.Y;
                    result.UpLookCenterMachinePosX = UpLookMachinePos.X;
                    result.UpLookCenterMachinePosY = UpLookMachinePos.Y;
                    result.TempCenterMachinePosX = TempMachinePos.X;
                    result.TempCenterMachinePosY = TempMachinePos.Y;
                   // result.Time = DateTime.Now.ToString("MM-dd HH:mm:ss");

                    resultA1List.Add(result);
                }

                if (isUpLook)
                {
                    this.PutbackCalibrationPlate();
                }

                ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                worksheet.Cells[1, 1].Value = "次数";
                worksheet.Cells[1, 2].Value = "速度";
                worksheet.Cells[1, 3].Value = "加速度";
                worksheet.Cells[1, 4].Value = "加加速度";
                worksheet.Cells[1, 5].Value = "到位延时";
                worksheet.Cells[1, 6].Value = "BMC定位X";
                worksheet.Cells[1, 7].Value = "BMC定位Y";
                worksheet.Cells[1, 8].Value = "UpLook定位X";
                worksheet.Cells[1, 9].Value = "UpLook定位Y";
                worksheet.Cells[1, 10].Value = "Temp定位X";
                worksheet.Cells[1, 11].Value = "Temp定位Y";
                worksheet.Cells[1, 12].Value = "BMC轴X";
                worksheet.Cells[1, 13].Value = "BMC轴Y";
                worksheet.Cells[1, 14].Value = "UpLook轴X";
                worksheet.Cells[1, 15].Value = "UpLook轴Y";
                worksheet.Cells[1, 16].Value = "Temp轴X";
                worksheet.Cells[1, 17].Value = "Temp轴Y";
                worksheet.Cells[1, 18].Value = "时间";

                for (int row = 0; row < resultA1List.Count; row++)
                {
                    Result ret = resultA1List[row];
                    worksheet.Cells[row + 2, 1].Value = row + 1;
                    worksheet.Cells[row + 2, 2].Value = ret.Speed;
                    worksheet.Cells[row + 2, 3].Value = ret.AccSpeed;
                    worksheet.Cells[row + 2, 4].Value = ret.Jerk;
                    worksheet.Cells[row + 2, 5].Value = ret.SleepTime;
                    worksheet.Cells[row + 2, 6].Value = ret.BmcCenterX;
                    worksheet.Cells[row + 2, 7].Value = ret.BmcCenterY;
                    worksheet.Cells[row + 2, 8].Value = ret.UpLookCenterX;
                    worksheet.Cells[row + 2, 9].Value = ret.UpLookCenterY;
                    worksheet.Cells[row + 2, 10].Value = ret.TempCenterX;
                    worksheet.Cells[row + 2, 11].Value = ret.TempCenterY;
                    worksheet.Cells[row + 2, 12].Value = ret.BmcCenterMachinePosX;
                    worksheet.Cells[row + 2, 13].Value = ret.BmcCenterMachinePosY;
                    worksheet.Cells[row + 2, 14].Value = ret.UpLookCenterMachinePosX;
                    worksheet.Cells[row + 2, 15].Value = ret.UpLookCenterMachinePosY;
                    worksheet.Cells[row + 2, 16].Value = ret.TempCenterMachinePosX;
                    worksheet.Cells[row + 2, 17].Value = ret.TempCenterMachinePosY;
                    worksheet.Cells[row + 2, 18].Value = ret.Time;
                }

                // 保存Excel文件
                package.Save();

                this.bondModuleController.MoveToSafePos();

                //DialogResult dialog = AKRSXtraMessageBox.Show(
                //    "the experiment is over \n"
                //    + "Result file location: \n" + @"D:\ExperimentSystem\RepeatPositionAccuracyTest" + DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障", ex, LogCategory.PR);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });

                return;
            }
        }


        private void BtStartBMCTask_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                {
                   
                });
        }

        /// <summary>
        /// 测试灯光
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtTestMulLight_Click(object sender, EventArgs e)
        {
        
             Light AmbientLightRed = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-红");
             Light AmbientLightGreen = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-绿");
             Light AmbientLightBlue = HardwareRepositoryService.GetHardware<Light>("邦头三色环光-蓝");

            Light[] lights = { AmbientLightRed, AmbientLightGreen, AmbientLightBlue };
            int [] indentities = { 100, 100, 100 };
            LightController.SetIntensities(lights.ToList(), indentities.ToList());
        }

        /// <summary>
        /// 测试震动
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnStartShakeTest_Click(object sender, EventArgs e)
        {
            this.TestShakePositionAccuracy();
        }

        /// <summary>
        /// 停止震动采图
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnStopShakeTest_Click(object sender, EventArgs e)
        {
            this.isContinue = false;
        }
    }
}