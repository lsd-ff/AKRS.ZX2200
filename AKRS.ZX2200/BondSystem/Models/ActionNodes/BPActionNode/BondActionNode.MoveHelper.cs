using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using ch.etel.edi.dsa.v40;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    /// <summary>
    /// 用于存放BondAction运动相关方法
    /// </summary>
    public partial class BondActionNode
    {
        /// <summary>
        ///  Bond模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        /// <summary>
        /// 绝对运动到贴片位
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToBondPos(AKRSPoint4D point4D)
        {
            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velY = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velZ = this.bondModule.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.bondModule.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 当前z的位置
            double curZPos = this.bondModule.BondHead.AxisZ.GetRealPosition();

            // TU安全高度
            //double safeLevel = this.system2Controller.GetTUMachineSafeHeight();

            // 获取安全高度
            double safeLevel = this.bondModuleController.GetBondheadSafeLevel(point4D);

            Stopwatch sp = new Stopwatch();

            if (curZPos < safeLevel)
            {
                // Z轴先去安全高度
                this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(safeLevel, velZ);

                sp = Stopwatch.StartNew();
                while (true)
                {
                    double curZ = this.bondModule.BondHead.AxisZ.GetRealPosition();

                    // 达到安全高度
                    if (Math.Abs(curZ - safeLevel) < 0.1)
                    {
                        System2RunTimeProvider.RecordTime("绝对运动到贴片位", $"Z轴抬升到安全高度{safeLevel}完成");

                        // 开始运动
                        this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                        this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                        this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);

                        break;
                    }

                    Thread.Sleep(1);

                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        System2RunTimeProvider.RecordTime("绝对运动到贴片位", $"Z轴抬升到安全高度{safeLevel}超时,当前高度{curZ}");
                        return ExcuteResult.Exception;
                    }
                }
            }
            else
            {
                // 高于安全高度直接开始运动
                this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);
            }       

            sp.Restart();
            while (true)
            {
                if (Math.Abs(this.bondModuleController.GetAxisYRealPos() - point4D.Y) < 0.5
                    && Math.Abs(this.bondModuleController.GetAxisXRealPos() - point4D.X) < 0.5)
                {
                    System2RunTimeProvider.RecordTime("绝对运动到贴片位", $"XY接近贴片位{point4D.X},{point4D.Y} 准备移动Z轴");

                    // 接近贴片位开始移动Z轴
                    this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);
                    break;
                }

                Thread.Sleep(1);

                // 超时
                if (sp.Elapsed.TotalSeconds > 10)
                {
                    System2RunTimeProvider.RecordTime("绝对运动到贴片位", $"等待XY接近贴片位{point4D.X},{point4D.Y} 超时");
                    return ExcuteResult.Exception;
                }
            }

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, point4D.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, point4D.Y, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisZ, true, point4D.Z, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisT, true, point4D.T, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// Bond动作空跑
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult BondActionDryRun()
        {
            // 获取当前芯片
            BaseCarrierConfig component = System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().component;

            AKRSPoint3D bondPositionInG0 = this.bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            // 空跑固晶位往上抬3mm
            AKRSPoint3D bondPosInAxis = this.bondModuleController.ConvertG0ToMachinePos(bondPositionInG0)
                                        + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + component.ComponentThickness + +this.bondPosition.GetPositionCompensate().Z + component.BondingDistance + new AKRSPoint3D(0, 0, 5);

            this.bondModuleController.MoveSafeBondXYZ(bondPosInAxis);

            // 延时
            DelayHelper.Delay(component.PlacementDelay);

            // 抬起
            this.bondHeadController.MoveBondZToSafePos();

            // 结束当前制程
            this.bondPosition.S2FinishedStep(this.actionNodesService.GetCurrentProcessStepName());

            UcMainSystem.ReFreshSystem2TuAction();

            return ExcuteResult.Success;
        }


        /// <summary>
        /// 去预固晶位
        /// </summary>
        /// <param name="preBondPos">贴片位</param>
        /// <param name="component">芯片</param>
        /// <param name="liftLevel">抬起高度，TU安全高度</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToPreBondPos(AKRSPoint4D preBondPos, BaseCarrierConfig component, double liftLevel)
        {
            System2RunTimeProvider.RecordTime("BondAction", $"准备运动到预固晶位");

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    //this.bondModuleController.MoveSafeBondXYZT(preBondPos);

                    ExcuteResult ret = this.AbsMoveToBondPos(preBondPos);
                    if (ret != ExcuteResult.Success)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                    $"运动到预固晶位失败！\r\n",
                                    "报警",
                                    new string[] { "确认" },
                                    new DialogResult[] { DialogResult.OK },
                                    AlarmLevel.FirstLevel);

                        return ret;
                    }

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    return this.JumpToBondPos(preBondPos, component, liftLevel);
            }

            System2RunTimeProvider.RecordTime("BondAction", $"运动到预固晶位结束");
            return ExcuteResult.Success;
        }

        /// <summary>
        /// 去贴片位
        /// </summary>
        /// <param name="bondPos">贴片位</param>
        /// <param name="component1">芯片</param>
        /// <param name="liftLevel">抬起高度，TU安全高度</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToBondPos(AKRSPoint4D bondPos, BaseCarrierConfig component1, double liftLevel)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(bondPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                double startLevel = this.bondHeadController.GetAxisZRealPos();

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("去贴片位T轴单独运动线程");

                            if (this.component.AdjustCamera == CameraTypeEnum.BondCamera
                                || this.component.AccuracyMode == AccuracyModeEnum.Off)
                            {
                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    if (Math.Abs(this.bondHeadController.GetAxisZRealPos() - startLevel) >= 0.5)
                                    {
                                        this.bondHeadController.RotateAxisT(bondPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 20000)
                                    {
                                        throw new Exception("去贴片位T轴单独运动超时！");
                                    }
                                }
                            }
                            else
                            {
                                // 如果是上视纠偏T轴直接动
                                this.bondHeadController.RotateAxisT(bondPos.T);
                            }
                        });
                }

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

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("Bond", $"Jump到贴片位结束,速度:{vel},加速时间:{this.ULmPara.JumpToBondPosAccelerationTime},加加速时间{this.ULmPara.JumpToBondPosJerkTime}，");

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

        /// <summary>
        /// 二段速去固精高度
        /// </summary>
        /// <param name="bondActionParameter">固精参数</param>
        private void SlowDownToBondLevel(BondActionParameter bondActionParameter)
        {
            if (component.BondingForceMode == ForceModeEnum.Distance)
            {
                if (component.IsActivateSlowTravelBeforeBonding)
                {
                    System2RunTimeProvider.RecordTime("Bond", "准备距离模式下压");

                    // 低速移动到固晶高度  如果未开启二段速 则在前一步Jump的时候已经到达了固晶高度
                    this.bondHeadController.MoveAxisZ(bondLevel, bondActionParameter.SlowTravelSpeedBeforePlace, AccuracyMode.HighAccuracy, false);
                }
            }
            else
            {
                System2RunTimeProvider.RecordTime("Bond", "准备力控模式下压");

                double curLevel = this.bondHeadController.GetAxisZRealPos();

                // 力控模式直接从当前位置进入力控模式
                this.bondHeadController.ForceControlSet(
                    bondActionParameter.BondForce,
                    curLevel - 0.01,
                    speed,
                    component.ForceControlOutTime,
                    bondActionParameter.SlowTravelDistanceBeforePlace);
            }
        }

        /// <summary>
        /// 焊头低速上抬
        /// </summary>
        /// <param name="liftPreLevel">抬起高度</param>
        /// <param name="bondActionParameter">固精参数</param>
        private void SlowUpToLiftLevel(double liftPreLevel, BondActionParameter bondActionParameter)
        {
            if (component.BondingForceMode == ForceModeEnum.Distance)
            {
                if (component.IsActivateSlowTravelAfterBonding)
                {
                    // 低速上抬到预固晶位
                    this.bondHeadController.MoveAxisZ(
                        liftPreLevel,
                        bondActionParameter.SlowTravelSpeedAfterPlace,
                        AccuracyMode.HighSpeed,
                        false);

                    System2RunTimeProvider.RecordTime(
                        "Bond",
                        $"距离模式低速上抬完成，速度{bondActionParameter.SlowTravelSpeedAfterPlace}");
                }
            }
            else
            {
                if (component.IsActivateSlowTravelAfterBonding)
                {
                    // 加这句是为了防止Z轴抬起时没有恢复正常速度
                    this.bondHeadController.SetAxisZSpeed(bondActionParameter.SlowTravelSpeedAfterPlace);

                    liftPreLevel = this.bondHeadController.GetAxisZRealPos()
                                   + bondActionParameter.SlowTravelDistanceAfterPlace;

                    // 力控模式低速速上抬
                    this.bondHeadController.ForceControlReset(
                        liftPreLevel,
                        bondActionParameter.SlowTravelSpeedAfterPlace,
                        component.ForceControlOutTime);

                    System2RunTimeProvider.RecordTime(
                        "Bond",
                        $"力控模式低速上抬完成，速度{bondActionParameter.SlowTravelSpeedAfterPlace}");
                }
                else
                {
                    // 原地退出力控模式
                    this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos(), speed);
                }
            }
        }
    }
}
