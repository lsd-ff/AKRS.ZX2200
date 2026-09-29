namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;

    using ch.etel.edi.dsa.v40;

    using DevExpress.XtraCharts;
    using log4net.Core;

    using OfficeOpenXml;

    public partial class BtStop : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 标定参数
        /// </summary>
        private CalibrateRunPara CalibratePara => CalibrateRunPara.GetInstance();

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara uLMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 芯片
        /// </summary>
        private BaseCarrierConfig component => (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find("1-0.9");
        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        public BtStop()
        {
            this.InitializeComponent();
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        /// <summary>
        /// 取片到上视
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtPickToUpLook_Click(object sender, EventArgs e)
        {
            UpLookCorrectionActionNode upLookCorrectionActionNode = new UpLookCorrectionActionNode();

            AKRSPoint4D pickupPos = new AKRSPoint4D(18.9363078492373, -160.992507768887, -84.2138, 0);
            double tuSafeHeight = -20.13;

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Restart();

            ExcuteResult retMove = this.JumpToPickupPos(pickupPos, tuSafeHeight);

            if (retMove != ExcuteResult.Success)
            {
                return;
            }

            LogHelper.Post(Level.Info,
                $"  UpLookToPick 耗时：{stopwatch1.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);

            Thread.Sleep(4000);

            stopwatch1.Restart();

            // 到上视觉位置
            AKRSPoint4D upLookPos = new AKRSPoint4D(-115.298084032822, -142.412989600563, -27.95, 0);


            retMove = upLookCorrectionActionNode.JumpToUplookFromWaferTable(upLookPos);

            LogHelper.Post(Level.Info,
                $"  PickToUpLook 耗时：{stopwatch1.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            for (int i = 0; i < 20; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(@"D:\ExperimentSystem\VibrationTest" +
                                                                 DateTime.Now.ToString("yyMMddhhmmss") + ".xlsx"));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "UpLook定位X";
            worksheet.Cells[1, 3].Value = "UpLook定位Y";
            worksheet.Cells[1, 4].Value = "时间";

            for (int row = 0; row < resultA1List.Count; row++)
            {
                Result ret = resultA1List[row];
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = ret.UpLookCenterX;
                worksheet.Cells[row + 2, 3].Value = ret.UpLookCenterY;
                worksheet.Cells[row + 2, 4].Value = ret.Time;
            }

            // 保存Excel文件
            package.Save();
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

        private BondModuleController bondModuleController = new BondModuleController();
        private BondHeadController bondHeadController = new BondHeadController();
        private BondModule bondModule = new BondModule();

        private void BtDownLookSubstrate_Click(object sender, EventArgs e)
        {
            // 到上视觉位置
            AKRSPoint4D upLookPos = new AKRSPoint4D(-115.298084032822, -142.412989600563, -27.95, 0);

            this.bondModuleController.MoveBondZToTUSafeHeight();
            this.bondModuleController.MoveSafeBondXY(upLookPos.X, upLookPos.Y);

            this.bondHeadController.MoveAxisZ(-27.95);

            // 到模拟芯片位置
            AKRSPoint4D point2 = new AKRSPoint4D(-120.5, -142.412989600563, -27.95, 0);

            this.bondModuleController.MoveBondXY(point2.X, point2.Y);

            Thread.Sleep(4000);

            Stopwatch sw = Stopwatch.StartNew();
            this.bondModuleController.MoveBondXY(upLookPos.X, upLookPos.Y);

            LogHelper.Post(Level.Info,
                $" DownLookSubstrate 耗时：{sw.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            for (int i = 0; i < 20; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition(this.CalibratePara.UpLookPRName);
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx"));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "UpLook定位X";
            worksheet.Cells[1, 3].Value = "UpLook定位Y";
            worksheet.Cells[1, 4].Value = "时间";

            for (int row = 0; row < resultA1List.Count; row++)
            {
                Result ret = resultA1List[row];
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = ret.UpLookCenterX;
                worksheet.Cells[row + 2, 3].Value = ret.UpLookCenterY;
                worksheet.Cells[row + 2, 4].Value = ret.Time;
            }

            // 保存Excel文件
            package.Save();

        }

        /// <summary>
        /// load 事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmVibrationTest_Load(object sender, EventArgs e)
        {

        }

        BondActionNode bondActionNode = new BondActionNode();   

        /// <summary>
        /// 震动测试
        /// </summary>
        /// <param name="xDis">X 距离</param>
        /// <param name="yDis">Y 距离</param>
        private double BondShake(double xDis, double yDis, double speed, double accTime, double jerkTime)
        {
            double liftLevel = System2Domain.GetInstance().System2Controller.GetTUMachineSafeHeight();

            // 1. 先走到起始点
            this.bondHeadController.MoveZAxis(liftLevel);
            this.bondModuleController.MoveBondXY(Convert.ToDouble(SpStartX.EditValue), Convert.ToDouble(SpStartY.EditValue));

            Thread.Sleep(1000);

            // 插补移动到
            Stopwatch sw = Stopwatch.StartNew();
            AKRSPoint4D bondPos = new AKRSPoint4D();

            // 计算运动结束点位
            AKRSPoint3D stratPoint = this.bondModuleController.Get3DRealPosition();
            bondPos.X = stratPoint.X + xDis;
            bondPos.Y = stratPoint.Y + yDis;

            // 限位检查
            this.bondModule.CheckSoftLimit(bondPos.X, bondPos.Y);
            bondPos.Z = liftLevel - 5;

           // AKRSPoint4D bondPos, BaseCarrierConfig component1, double liftLevel

            this.JumpToBondPos(bondPos, speed, accTime, jerkTime);

            long movElapsedMilliseconds = sw.ElapsedMilliseconds;

            this.SpINPTimesX.EditValue = movElapsedMilliseconds;

            Thread.Sleep(Convert.ToInt32(SpDelay.EditValue));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            List<Result> resultA1List = new List<Result>();
            int index = 0;

            for (int i = 0; i < 30; i++)
            {
                Result result = new Result();
                result.Time = stopwatch.ElapsedMilliseconds;
                MatchResult resultUpLook = this.LocatePosition("MoveTestPRSmall");

                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;
                resultA1List.Add(result);
            }

            this.ShowInLine(resultA1List);
            (double inpTime, double range) ret = this.CalcTimes(resultA1List);

            this.SaveExcel(
                resultA1List,
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate2" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx");

            return 0;

            // return 10 * Math.Pow(speed, 2) - 100 * accTime ;  //ret.inpTime / 100 + ret.range * 0.5;
        }

        /// <summary>
        /// 显示曲线
        /// </summary>
        /// <param name="resultA1List">结果集</param>
        private void ShowInLine(List<Result> resultA1List)
        {
            this.chartControl1.Series[0].Points.Clear();
            this.chartControl1.Series[1].Points.Clear();

            SeriesPoint[] spsX = resultA1List.Select(a => new SeriesPoint(a.Time, a.UpLookCenterX)).ToArray();
            this.chartControl1.Series[0].Points.AddRange(spsX);

            SeriesPoint[] spsY = resultA1List.Select(a => new SeriesPoint(a.Time, a.UpLookCenterY)).ToArray();
            this.chartControl1.Series[1].Points.AddRange(spsY);
        }

        /// <summary>
        /// 计算诊定到位时间
        /// </summary>
        /// <param name="resultA1List">结果集合</param>
        private (double inpTime, double range) CalcTimes(List<Result> resultA1List)
        {
            // 诊定窗口
            double window = Convert.ToDouble(this.SpWindow.EditValue);

            // 取最后10 个的平均值 作为诊定基准值
            double lastAvgX = resultA1List.Select(a => a.UpLookCenterX).ToList().Skip(20).Take(10).ToList().Average();
            double lastAvgY = resultA1List.Select(a => a.UpLookCenterY).ToList().Skip(20).Take(10).ToList().Average();

            // 最后 
            this.SpAvgX.EditValue = lastAvgX;
            this.SpAvgY.EditValue = lastAvgY;

            // 与均值的极差
            this.SpMaxAvgX.EditValue = resultA1List.Select(a => Math.Abs(a.UpLookCenterX - lastAvgX)).ToList().Max();
            this.SpMaxAvgY.EditValue = resultA1List.Select(a => Math.Abs(a.UpLookCenterY - lastAvgY)).ToList().Max();

            WholeRange wholeRangeY1 = ((XYDiagram)this.chartControl1.Diagram).AxisY.WholeRange;
            wholeRangeY1.SetMinMaxValues(lastAvgX - 2, lastAvgX + 2);

            WholeRange wholeRangeY2 = ((XYDiagram)this.chartControl1.Diagram).SecondaryAxesY[0].WholeRange;
            wholeRangeY2.SetMinMaxValues(lastAvgY - 2, lastAvgY + 2);

            // 获取诊定到位的耗时
            Result lastOverWindowX = null;
            Result lastOverWindowY = null;

            if (resultA1List.Exists(a => a.UpLookCenterX > lastAvgX + window || a.UpLookCenterX < lastAvgX - window))
            {
                lastOverWindowX = resultA1List.Last(a =>
                    a.UpLookCenterX > lastAvgX + window || a.UpLookCenterX < lastAvgX - window);
            }

            if (resultA1List.Exists(a => a.UpLookCenterY > lastAvgY + window || a.UpLookCenterY < lastAvgY - window))
            {
                lastOverWindowY = resultA1List.Last(a =>
                    a.UpLookCenterY > lastAvgY + window || a.UpLookCenterY < lastAvgY - window);
            }


            if (lastOverWindowX != null)
            {
                int index = resultA1List.IndexOf(lastOverWindowX);
                if (index < 29)
                {
                    this.SpVisionINPTimesX.EditValue = resultA1List[index + 1].Time;
                }
            }
            else
            {
                this.SpVisionINPTimesX.EditValue = 0;
            }

            if (lastOverWindowY != null)
            {
                int index = resultA1List.IndexOf(lastOverWindowY);
                if (index < 29)
                {
                    this.SpVisionINPTimesY.EditValue = resultA1List[index + 1].Time;
                }
            }
            else
            {
                this.SpVisionINPTimesY.EditValue = 0;
            }

            return (
                Math.Max(Convert.ToDouble(this.SpVisionINPTimesX.EditValue),
                    Convert.ToDouble(this.SpVisionINPTimesY.EditValue)),
                Math.Max(Convert.ToDouble(this.SpMaxAvgX.EditValue), 
                    Convert.ToDouble(this.SpMaxAvgY.EditValue)));
        }

        /// <summary>
        /// 保存到Excel
        /// </summary>
        /// <param name="resultA1List"></param>
        /// <param name="fileName"></param>
        private void SaveExcel(List<Result> resultA1List, string fileName)
        {
            ExcelPackage package = new ExcelPackage(new FileInfo(fileName));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "UpLook定位X";
            worksheet.Cells[1, 3].Value = "UpLook定位Y";
            worksheet.Cells[1, 4].Value = "时间";

            for (int row = 0; row < resultA1List.Count; row++)
            {
                Result ret = resultA1List[row];
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = ret.UpLookCenterX;
                worksheet.Cells[row + 2, 3].Value = ret.UpLookCenterY;
                worksheet.Cells[row + 2, 4].Value = ret.Time;
            }

            // 保存Excel文件
            package.Save();
        }

        /// <summary>
        /// Bond
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBond_Click(object sender, EventArgs e)
        {
            double xdis = Convert.ToDouble(this.SpEndX.EditValue);
            double ydis = Convert.ToDouble(this.SpEndY.EditValue);
            double speed = Convert.ToDouble(this.SpSpeed.EditValue);
            double accTime = Convert.ToDouble(this.SpAccTime.EditValue);
            double jerkTime = Convert.ToDouble(this.SpJerkTime.EditValue);

            this.BondShake(xdis, ydis, speed, accTime, jerkTime);
        }

        private ULMPara ULmPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 去贴片位
        /// </summary>
        /// <param name="bondPos">贴片位</param>
        /// <param name="component">芯片</param>
        /// <param name="liftLevel">抬起高度，TU安全高度</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToBondPos(AKRSPoint4D bondPos, double speed, double accTime, double jerkTime)
        {
            double liftLevel = System2Domain.GetInstance().System2Controller.GetTUMachineSafeHeight();

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(bondPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                double startLevel = this.bondHeadController.GetAxisZRealPos();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULmPara.JumpToBondPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULmPara.JumpToBondPosAccelerationTime;
                interpolationParam.AccAcc = this.ULmPara.JumpToBondPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                if (this.bondHeadController.GetAxisZRealPos() < liftLevel)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                    // 第1点, 先上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = liftLevel, T = this.bondHeadController.GetAxisTRealPos() };

                    // 第2点, XYT 移动到贴片位置
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = bondPos.X, Y = bondPos.Y, Z = liftLevel, T = bondPos.T };

                    // 第3点, Z轴下降到贴片位或者二段速贴片位置
                    interpolationParam.SegmentConfigs[2].Point = bondPos;
                }
                else
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                    // 第1点, XYT 移动到贴片位置
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = bondPos.X, Y = bondPos.Y, Z = liftLevel, T = bondPos.T };

                    // 第2点, Z轴下降到贴片位或者二段速贴片位置
                    interpolationParam.SegmentConfigs[1].Point = bondPos;
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULmPara.JumpToBondPosTime,
                    RadiusRatio = this.ULmPara.JumpToBondPosRadiusRatio
                };

                foreach (var segmentConfig in interpolationParam.SegmentConfigs)
                {
                    // 加减速度默认*10
                    segmentConfig.Velocity = vel;
                    segmentConfig.Acc = vel * 10.0;
                    segmentConfig.Dec = vel * 10.0;
                }

                // 获取卡
                AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
                    card =>
                    card.AxisList.Select(axis => axis.AxisDrive).Exists(
                        drive => drive == interpolationParam.AxisDrives[0]));

                System2RunTimeProvider.RecordTime("Bond", $"Jump到贴片位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                System2RunTimeProvider.RecordTime("Bond", $"Jump到贴片位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到贴片位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到贴片位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        ///// <summary>
        ///// 运动到贴片为止
        ///// </summary>
        ///// <param name="bondPos">bond 位置</param>
        ///// <returns>执行结果</returns>
        //private ExcuteResult JumpToBondPos(AKRSPoint4D bondPos, double speed, double accTime, double jerkTime)
        //{
        //    try
        //    {
        //        #region ULM运动

        //        // 设置群组
        //        DsaIpolGroup iGroup = this.bondModuleController.GetXYZIpolGroup();  

        //        // 开始插补
        //        iGroup.ipolBegin();

        //        // 设置为绝对坐标系 ，不设置绝对坐标系
        //        iGroup.ipolSetAbsMode(true, -1);

        //        //// 设置矢量速度  
        //        //iGroup.ipolUSpeed(1.5);

        //        //// 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
        //        //// iGroup.ipolUTime(0.25, 0.1);
        //        //iGroup.ipolUTime(0.2, 0.08);

        //        // 设置矢量速度  
        //        iGroup.ipolUSpeed(speed);

        //        // 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
        //        // iGroup.ipolUTime(0.25, 0.1);
        //        iGroup.ipolUTime(accTime, jerkTime);

        //        //// 设置矢量速度  
        //        //iGroup.ipolUSpeed(1.2);

        //        //// 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
        //        //iGroup.ipolUTime(0.15, 0.07);

        //        // 第2点, XYZ 移动到贴片位置 
        //        AKRSPoint4D point2 = new AKRSPoint4D()
        //        {
        //            X = bondPos.X / 1000.0, Y = bondPos.Y / 1000.0, Z = -bondPos.Z / 1000.0
        //        };

        //        double[] array2 = new[] { point2.X, point2.Y, point2.Z, 0 };
        //        DsaVector vector2 = new DsaVector(array2);
        //        iGroup.ipolULine(vector2);

        //        // 等待插补结束
        //        iGroup.ipolWaitMovement(10000);

        //        // 退出插补模式
        //        iGroup.ipolEnd();

        //        return ExcuteResult.Success;

        //        #endregion
        //    }
        //    catch (DsaException exc)
        //    {
        //        LogHelper.Post(Level.Error, $"运动到贴片位，UML运动失败！", exc, LogCategory.Bond);
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Post(Level.Error, $"运动到贴片位失败！", e, LogCategory.Bond);
        //        throw;
        //    }
        //}

        private void BtBond2_Click(object sender, EventArgs e)
        {
            double xdis = Convert.ToDouble(this.SpEndX2.EditValue);
            double ydis = Convert.ToDouble(this.SpEndY2.EditValue);
            double speed = Convert.ToDouble(this.SpSpeed.EditValue);
            double accTime = Convert.ToDouble(this.SpAccTime.EditValue);
            double jerkTime = Convert.ToDouble(this.SpJerkTime.EditValue);
            this.BondShake(xdis, ydis, speed, accTime, jerkTime);
        }

        private void BtBond3_Click(object sender, EventArgs e)
        {
            double xdis = Convert.ToDouble(this.SpEndX3.EditValue);
            double ydis = Convert.ToDouble(this.SpEndY3.EditValue);
            double speed = Convert.ToDouble(this.SpSpeed.EditValue);
            double accTime = Convert.ToDouble(this.SpAccTime.EditValue);
            double jerkTime = Convert.ToDouble(this.SpJerkTime.EditValue);
            this.BondShake(xdis, ydis, speed, accTime, jerkTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtBondToPick_Click(object sender, EventArgs e)
        {
            for(int i=0;i<20; i++) 
            { 
                this.bondHeadController.MoveZAxis(-20.13);
                this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpStartXX.EditValue),
                    Convert.ToDouble(this.SpStartYY.EditValue));

                Thread.Sleep(50);

                AKRSPoint4D pickupPos = new AKRSPoint4D(30.86, -165, -84.2138, 0);
                double tuSafeHeight = -20.13;
                Stopwatch stopwatch1 = new Stopwatch();
                stopwatch1.Restart();

                ExcuteResult retMove = this.JumpToPickupPos(pickupPos, tuSafeHeight);

                if (retMove != ExcuteResult.Success)
                {
                    return;
                }
                spinEdit1.Value = stopwatch1.ElapsedMilliseconds;

                LogHelper.Post(Level.Info,
                    $"  BondToPick 耗时：{stopwatch1.ElapsedMilliseconds} ms"
                    , LogCategory.MainSoftWare);
            }
        }


        /// <summary>
        /// 去取片位置，插补
        /// </summary>
        /// <param name="pickupPos">取片位置</param>
        /// <param name="liftSafeLevel">取片抬起安全高度</param>
        /// <param name="carrierType">芯片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToPickupPos(AKRSPoint4D pickupPos, double liftSafeLevel)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(pickupPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                // 轴安全高度
                double safeHeight = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                // TU安全高度
                double tuSafeHeight = this.system2Controller.GetTUMachineSafeHeight();
                double startLevel = this.bondHeadController.GetAxisZRealPos();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToPickupPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToPickupPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToPickupPosJerkTime;
                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                    // 低于安全高度就先抬到安全高度
                    if (this.bondHeadController.GetAxisZRealPos() < liftSafeLevel)
                    {
                        interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                        // 第一段，Z上抬到安全高度
                        interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = liftSafeLevel, T = this.bondHeadController.GetAxisTRealPos() };

                        // XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                        interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = pickupPos.X, Y = this.uLMPara.TransportUnitEdgePos.Y, Z = liftSafeLevel, T = 0 };

                        // XYT运动到取料位置
                        interpolationParam.SegmentConfigs[2].Point = pickupPos;
                    }
                    else
                    {
                        interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                        // XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                        interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = pickupPos.X, Y = this.uLMPara.TransportUnitEdgePos.Y, Z = liftSafeLevel, T = 0 };

                        // XYT运动到取料位置
                        interpolationParam.SegmentConfigs[1].Point = pickupPos;
                    }
                

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToPickupPosTime,
                    RadiusRatio = this.uLMPara.JumpToPickupPosRadiusRatio
                };

                foreach (var segmentConfig in interpolationParam.SegmentConfigs)
                {
                    // 加减速度默认*10
                    segmentConfig.Velocity = vel;
                    segmentConfig.Acc = vel * 10.0;
                    segmentConfig.Dec = vel * 10.0;
                }

                // 获取卡
                AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
                    card =>
                    card.AxisList.Select(axis => axis.AxisDrive).Exists(
                        drive => drive == interpolationParam.AxisDrives[0]));

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到取片位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到取片位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        ///// <summary>
        ///// 去取片位置
        ///// </summary>
        ///// <param name="pickupPos">取片位置</param>
        ///// <param name="tuSafeHeight">TU 安全高度</param>
        ///// <returns>结果</returns>
        //public ExcuteResult JumpToPickupPos(AKRSPoint4D pickupPos, double tuSafeHeight)
        //{
        //    DsaIpolGroup iGroup = null;

        //    try
        //    {
        //        #region UML运动

        //        // 设置群组
        //        iGroup = this.bondModuleController.GetXYZIpolGroup();
          
        //        // 开始插补
        //        iGroup.ipolBegin();

        //        // 设置为绝对坐标系 ，不设置绝对坐标系
        //        iGroup.ipolSetAbsMode(true, -1);

        //        // 设置矢量速度  
        //        //iGroup.ipolUSpeed(1.5);

        //        double speed = Convert.ToDouble(this.SpSpeed1.Value);
        //        double acc = Convert.ToDouble(this.SpAcc1.Value);
        //        double jerk = Convert.ToDouble(this.SpJerk1.Value);
        //        // 设置矢量速度  
        //        iGroup.ipolUSpeed(speed);

        //        // 设置加速时间和加加速时间  加速时间 应该大于等于 2 * 加加速时间 
        //        iGroup.ipolUTime(acc, jerk);

        //        // 第一段，Z上抬到TU安全高度
        //        AKRSPoint4D point1 = new AKRSPoint4D()
        //        {
        //            X = this.bondModuleController.GetAxisXRealPos() / 1000.0,
        //            Y = this.bondModuleController.GetAxisYRealPos() / 1000.0,
        //            Z = -tuSafeHeight / 1000.0,
        //            T = this.bondHeadController.GetAxisTRealPos() / 360
        //        };
        //        double[] array1 = new[] { point1.X, point1.Y, point1.Z, point1.T };
        //        DsaVector vector1 = new DsaVector(array1);
        //        iGroup.ipolULine(vector1);

        //        // 第二段,XY运动到轨道外延 (-150) 且T 运动到取料角度的 1/2 Z不动  
        //        double tuEdgeY = this.uLMPara.TransportUnitEdgePos.Y;
        //        AKRSPoint4D point2 = new AKRSPoint4D()
        //        {
        //            X = pickupPos.X / 1000.0,
        //            Y = tuEdgeY / 1000.0,
        //            Z = -tuSafeHeight / 1000.0,
        //            T = pickupPos.T / 2.0 / 360.0
        //        };

        //        double[] array2 = new[] { point2.X, point2.Y, point2.Z, point2.T };
        //        DsaVector vector2 = new DsaVector(array2);
        //        iGroup.ipolULine(vector2);


        //        // 第三段,XYT运动到取料位置
        //        double slowTravelBeforePick = 0;

        //        AKRSPoint4D point3 = new AKRSPoint4D()
        //        {
        //            X = pickupPos.X / 1000.0,
        //            Y = pickupPos.Y / 1000.0,
        //            Z = -(pickupPos.Z + slowTravelBeforePick) / 1000.0,
        //            T = pickupPos.T / 360
        //        };

        //        double[] array3 = new[] { point3.X, point3.Y, point3.Z, point3.T };
        //        DsaVector vector3 = new DsaVector(array3);
        //        iGroup.ipolULine(vector3);

        //        // 等待插补结束
        //        iGroup.ipolWaitMovement(100000);

        //        // 退出插补模式
        //        iGroup?.ipolEnd();

        //        return ExcuteResult.Success;

        //        #endregion
        //    }
        //    catch (DsaException exc)
        //    {
        //        // 退出插补模式
        //        iGroup?.ipolEnd();

        //        LogHelper.Post(Level.Error, $"运动到上视拍照位，UML运动失败！", exc, LogCategory.Global);
        //        throw;
        //    }
        //    catch (Exception e)
        //    {
        //        LogHelper.Post(Level.Error, $"运动到上视拍照位失败！", e, LogCategory.Global);
        //        throw;
        //    }
        //}

        private void BtBondMove_Click(object sender, EventArgs e)
        {
            // 设置群组
            DsaIpolGroup iGroup = this.bondModuleController.GetXYIpolGroup();

            //1. 先走到起始点
            this.bondHeadController.MoveZAxis(-4.96);
            this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpStartX.EditValue),
                Convert.ToDouble(this.SpStartY.EditValue));

            Thread.Sleep(2000);

            Stopwatch stopwatchIp = new Stopwatch();

            stopwatchIp.Restart();

            // 开始插补
            iGroup.ipolBegin();

            // 设置为绝对坐标系 ，不设置绝对坐标系
            iGroup.ipolSetAbsMode(true, -1);

            iGroup.ipolTanVelocity(1);
            iGroup.ipolTanAcceleration(9);
            iGroup.ipolTanDeceleration(9);

            // 设置抖动时间，不知道什么意思
            iGroup.ipolTanJerkTime(0.0015);

            // iGroup.ipolLine((bondAxisX.GetRealPosition() + 3) / 1000.0, bondAxisY.GetRealPosition() / 1000.0);

            iGroup.ipolLine(-59.4164 / 1000.0, -51.6650 / 1000.0);

            // 等待插补结束
            iGroup.ipolWaitMovement(100000);

            // 退出插补模式
            iGroup.ipolEnd();

            LogHelper.Post(Level.Info,
                $" DownLookSubstrate Move 1插补模式 耗时：{stopwatchIp.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);

            ////1. 先走到起始点
            //this.bondHeadController.MoveZAxis(-2.53);
            //this.bondModuleController.MoveBondXY(Convert.ToDouble(SpStartX.EditValue), Convert.ToDouble(SpStartY.EditValue));

            //Thread.Sleep(1000);

            //// 插补移动到
            //Stopwatch sw = Stopwatch.StartNew();
            //AKRSPoint4D bondPos = new AKRSPoint4D();

            //this.bondModuleController.MoveBondXY(Convert.ToDouble(SpEndX.EditValue), Convert.ToDouble(SpEndY.EditValue));

            //LogHelper.Post(Level.Info,
            //     $" DownLookSubstrate Move 1 普通模式 耗时：{sw.ElapsedMilliseconds} ms"
            //     , LogCategory.MainSoftWare);


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            for (int i = 0; i < 40; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition("MoveTestPRSmall");
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            this.SaveExcel(resultA1List,
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate Move 1" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx");
        }

        private void BtBondMove2_Click(object sender, EventArgs e)
        {
            //1. 先走到起始点

            this.bondHeadController.MoveZAxis(-4.96);
            this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpStartX.EditValue),
                Convert.ToDouble(this.SpStartY.EditValue));

            Thread.Sleep(4000);

            // 插补移动到
            Stopwatch sw = Stopwatch.StartNew();
            AKRSPoint4D bondPos = new AKRSPoint4D();

            this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpEndX2.EditValue),
                Convert.ToDouble(this.SpEndY2.EditValue));

            LogHelper.Post(Level.Info,
                $" DownLookSubstrate Move 2 耗时：{sw.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            for (int i = 0; i < 40; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition("MoveTestPRSmall");
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            this.SaveExcel(resultA1List,
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate Move 2" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx");
        }

        private void BtBondMove3_Click(object sender, EventArgs e)
        {
            //1. 先走到起始点

            this.bondHeadController.MoveZAxis(-4.96);
            this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpStartX.EditValue),
                Convert.ToDouble(this.SpStartY.EditValue));

            Thread.Sleep(4000);

            // 插补移动到
            Stopwatch sw = Stopwatch.StartNew();
            AKRSPoint4D bondPos = new AKRSPoint4D();

            this.bondModuleController.MoveBondXY(Convert.ToDouble(this.SpEndX3.EditValue),
                Convert.ToDouble(this.SpEndY3.EditValue));

            LogHelper.Post(Level.Info,
                $" DownLookSubstrate Move 3 耗时：{sw.ElapsedMilliseconds} ms"
                , LogCategory.MainSoftWare);


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            for (int i = 0; i < 40; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition("MoveTestPRSmall");
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            ExcelPackage package = new ExcelPackage(new FileInfo(
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate Move 3" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx"));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            worksheet.Cells[1, 1].Value = "次数";
            worksheet.Cells[1, 2].Value = "UpLook定位X";
            worksheet.Cells[1, 3].Value = "UpLook定位Y";
            worksheet.Cells[1, 4].Value = "时间";

            for (int row = 0; row < resultA1List.Count; row++)
            {
                Result ret = resultA1List[row];
                worksheet.Cells[row + 2, 1].Value = row + 1;
                worksheet.Cells[row + 2, 2].Value = ret.UpLookCenterX;
                worksheet.Cells[row + 2, 3].Value = ret.UpLookCenterY;
                worksheet.Cells[row + 2, 4].Value = ret.Time;
            }

            // 保存Excel文件
            package.Save();
        }



        private void BtnJumpToIPT_Click(object sender, EventArgs e)
        {
            this.BtBondToPick_Click(null, null);

            SubstrateCameraCorrectionActionNode substrateCameraCorrectionActionNode =
                new SubstrateCameraCorrectionActionNode();

            AKRSPoint4D pickupPos = new AKRSPoint4D(18.9363078492373, -160.992507768887, -84.2138, 0);
            double tuSafeHeight = -20.13;

            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Restart();

            // IPT位置转到Bond
            AKRSPoint3D iPTPosInBond =
                this.bondModuleController.ConvertG0ToMachinePos(BondDevicePara.GetInstance().IPTDevicePara.IPTPos);

            // 放片参数
            double placeLevel = iPTPosInBond.Z + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 1;

            // 中转台放片位
            AKRSPoint4D targetPos = new AKRSPoint4D() { X = iPTPosInBond.X, Y = iPTPosInBond.Y, Z = placeLevel, T = 0 };

            Stopwatch stopwatch = Stopwatch.StartNew();
            //substrateCameraCorrectionActionNode.JumpToIPTTest(targetPos);
            this.SpINPTimesX.EditValue = stopwatch.ElapsedMilliseconds;
        }

        private void JumpToIPTVisionPos_Click(object sender, EventArgs e)
        {
            this.BtnJumpToIPT_Click(null, null);

            SubstrateCameraCorrectionActionNode substrateCameraCorrectionActionNode =
                new SubstrateCameraCorrectionActionNode();

            // IPT位置转到Bond
            AKRSPoint3D iPTPosInBond =
                this.bondModuleController.ConvertG0ToMachinePos(BondDevicePara.GetInstance().IPTDevicePara.IPTPos);

            // 计算真实拍照位
            AKRSPoint3D p1RealVisionPos =
                this.bondModuleController.ConvertG0ToMachinePos(this.component.DownLookAdjustConfig.P1VisionPos);
            p1RealVisionPos.Z = -7.4683;

            Stopwatch stopwatch = Stopwatch.StartNew();

            substrateCameraCorrectionActionNode.JumpToVisionPos(p1RealVisionPos);
            this.SpINPTimesX.EditValue = stopwatch.ElapsedMilliseconds;

            List<Result> resultA1List = new List<Result>();
            MatchResult resultUpLook = new MatchResult();
            int index = 0;

            stopwatch.Restart();

            for (int i = 0; i < 30; i++)
            {
                // this.bondModuleController.MoveSafeBondXYZ(this.CalibratePara.WaferTableLeftMarkBCVisionMachinePos);

                resultUpLook = this.LocatePosition("小标定片模板");
                Result result = new Result();
                result.Index = index++;
                result.UpLookCenterX = resultUpLook.CenterX * 1.72;
                result.UpLookCenterY = resultUpLook.CenterY * 1.72;

                result.Time = stopwatch.ElapsedMilliseconds;

                resultA1List.Add(result);
            }

            this.ShowInLine(resultA1List);
            this.CalcTimes(resultA1List);

            this.SaveExcel(resultA1List,
                @"D:\ExperimentSystem\VibrationTest_DownLookSubstrate Move 2" + DateTime.Now.ToString("yyMMddhhmmss") +
                ".xlsx");
        }


        private void BtUplookToBond_Click(object sender, EventArgs e)
        {

        }

        private void BtSpeedCalib_Click(object sender, EventArgs e)
        {
            (double xdis, double ydis)[] xyDisArr =
            {
                (2, 2), (5, 5), (5, 0), (10, 10), (20, 20), (30, 30), (40, 40), (50, 50), (70, 70), (90, 90), 
                (110, 110), (130, 130), (150, 150), (170, 170), (200, 200), (230, 200), (250, 200), (300, 200),
            };

            foreach (var xyDis in xyDisArr)
            {
                PSO pso = new PSO(xyDis.xdis, xyDis.ydis);

                pso.FitnessFunction = this.BondShake;

                (double vmax, double at, double jt, double fitness) ret = pso.Optimize();

                // 保存
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find("MoveTestPRSmall");
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity("MoveTestPRSmall");
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        private void groupControl2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }
    }


    public class PSO
    {
        // 粒子群优化算法的参数

        public double XDis { get; set; }

        public double YDis { get; set; }    

        /// <summary>
        /// 粒子数量
        /// </summary>
        private int numParticles = 30;

        /// <summary>
        /// 最大迭代次数
        /// </summary>
        private int numIterations = 15;

        /// <summary>
        /// vmax 的取值范围
        /// </summary>
        private double[] vmaxRange = { 0.8, 1.7 };

        /// <summary>
        ///  at 的取值范围
        /// </summary>
        private double[] atRange = { 0.02, 0.2 };

        // 惯性权重、个体加速度常数、社会加速度常数
        private double w, c1, c2; 

        // 构造函数，用于初始化粒子群优化算法
        public PSO(double xDis, double yDis)
        {
            this.XDis = xDis;
            this.YDis = yDis;
            /*this.numParticles = numParticles;
            this.numIterations = numIterations;
            this.vmaxRange = vmaxRange;
            this.atRange = atRange;
            this.jtRange = jtRange;*/

            // 设置惯性权重和加速度常数
            this.w = 0.5; // 惯性权重，控制粒子的速度
            this.c1 = 1.5; // 个体加速度常数，控制粒子向自身最优位置的移动
            this.c2 = 1.5; // 社会加速度常数，控制粒子向群体最优位置的移动
        }

        // 优化方法，用于执行粒子群优化过程
        public (double vmax, double at, double jt, double fitness) Optimize()
        {
            // 初始化粒子群
            List<Particle> particles = new List<Particle>();

            for (int i = 0; i < this.numParticles; i++)
            {
                particles.Add(new Particle(this.vmaxRange, this.atRange)); // 创建粒子并随机初始化
            }

            Particle globalBestParticle = null; // 用于保存全局最优粒子

            // 迭代过程
            for (int iter = 0; iter < this.numIterations; iter++)
            {
                foreach (var particle in particles)
                {
                    // 计算当前粒子的适应度
                    double fitness = this.FitnessFunction(this.XDis, this.YDis, particle.Vmax, particle.At, particle.Jt);

                    // 更新个体最优位置和适应度
                    if (fitness < particle.BestFitness)
                    {
                        particle.BestFitness = fitness;
                        particle.BestPosition = (particle.Vmax, particle.At, particle.Jt);
                    }

                    // 更新全局最优粒子
                    if (globalBestParticle == null || fitness < globalBestParticle.BestFitness)
                    {
                        globalBestParticle = particle;
                    }
                }

                // 更新粒子的速度和位置
                foreach (var particle in particles)
                {
                    particle.UpdateVelocity(globalBestParticle.BestPosition, this.w, this.c1, this.c2); // 更新速度
                    particle.UpdatePosition(this.vmaxRange, this.atRange); // 更新位置
                }
            }

            // 返回最优解
            return (globalBestParticle.BestPosition.vmax, globalBestParticle.BestPosition.at,
                globalBestParticle.BestPosition.jt, globalBestParticle.BestFitness);
        }

        public Func<double, double, double, double, double, double> FitnessFunction { get; set; }


        // 用户定义的适应度函数，用于计算适应度值
        //private double FitnessFunction(double vmax, double at, double jt)
        //{

        //    // 例如： return Math.Pow(vmax - 5, 2) + Math.Pow(at - 2, 2) + Math.Pow(jt - 3, 2);
        //    throw new NotImplementedException("请实现您的Fitness函数");
        //}
    }

    // 定义粒子类
    public class Particle
    {
        /// <summary>
        /// 粒子的 vmax 值
        /// </summary>
        public double Vmax { get; set; }

        /// <summary>
        /// 粒子的 at 值
        /// </summary>
        public double At { get; set; }

        /// <summary>
        /// 粒子的 jt 值
        /// </summary>
        public double Jt { get; set; }

        /// <summary>
        /// 粒子的最佳适应度值
        /// </summary>
        public double BestFitness { get; set; }

        /// <summary>
        /// 粒子的最佳位置
        /// </summary>
        public (double vmax, double at, double jt) BestPosition { get; set; }

        /// <summary>
        /// 粒子的速度
        /// </summary>
        private (double vmax, double at, double jt) velocity; 

        // 粒子类的构造函数，用于初始化粒子的位置和速度
        public Particle(double[] vmaxRange, double[] atRange)
        {
            Random rand = new Random();

            // 初始化粒子的位置
            this.Vmax = vmaxRange[0] + rand.NextDouble() * (vmaxRange[1] - vmaxRange[0]);
            this.At = atRange[0] + rand.NextDouble() * (atRange[1] - atRange[0]);
            this.Jt = this.At / 2.0;

            this.BestFitness = double.MaxValue; // 初始化时将最佳适应度设为最大值
            this.BestPosition = (this.Vmax, this.At, this.Jt); // 初始时，最佳位置为当前粒子位置

            // 初始化速度为0
            this.velocity = (0, 0, 0);
        }

        // 更新粒子的速度
        public void UpdateVelocity((double vmax, double at, double jt) globalBestPosition, double w, double c1,
            double c2)
        {
            Random rand = new Random();

            // 速度更新公式
            this.velocity.vmax = w * this.velocity.vmax + c1 * rand.NextDouble() * (this.BestPosition.vmax - this.Vmax) +
                                 c2 * rand.NextDouble() * (globalBestPosition.vmax - this.Vmax);

            this.velocity.at = w * this.velocity.at + c1 * rand.NextDouble() * (this.BestPosition.at - this.At) +
                               c2 * rand.NextDouble() * (globalBestPosition.at - this.At);

            this.velocity.jt = this.velocity.at / 2.0;
        }

        // 更新粒子的位置
        public void UpdatePosition(double[] vmaxRange, double[] atRange)
        {
            // 使用更新后的速度来更新粒子的位置
            this.Vmax += this.velocity.vmax;
            this.At += this.velocity.at;
            this.Jt += this.velocity.jt;

            // 保持粒子的位置在给定范围内
            this.Vmax = Math.Max(vmaxRange[0], Math.Min(vmaxRange[1], this.Vmax));
            this.At = Math.Max(atRange[0], Math.Min(atRange[1], this.At));
        }
    }

    //// 主程序入口
    //public static void Main(string[] args)
    //{
    //    // 定义参数范围
    //    double[] vmaxRange = { 0.0, 10.0 };
    //    double[] atRange = { 0.0, 5.0 };
    //    double[] jtRange = { 0.0, 5.0 };

    //    // 初始化 PSO 算法
    //    PSO pso = new PSO(30, 100, vmaxRange, atRange, jtRange);

    //    // 执行优化
    //    var result = pso.Optimize();

    //    // 输出最优结果
    //    Console.WriteLine(
    //        $"Best vmax: {result.vmax}, Best at: {result.at}, Best jt: {result.jt}, Fitness: {result.fitness}");
    //}
}