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
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using ch.etel.edi.dsa.v40;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    ///  SubstrateCameraCorrectionActionNode移动帮助类
    /// </summary>
    public partial class SubstrateCameraCorrectionActionNode
    {
        /// <summary>
        ///  Bond模组
        /// </summary>
        private readonly BondModule bondModule = new BondModule();

        /// <summary>
        /// 去中转台
        /// </summary>
        /// <param name="componet">芯片</param>
        /// <param name="targetPos">中转台位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToIPT(BaseCarrierConfig componet, AKRSPoint4D targetPos)
        {
            System2RunTimeProvider.RecordTime("中转台动作节点", $"准备运动到中转台预放片位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    //this.bondModuleController.MoveSafeBondXYZT(targetPos);

                    double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;
                    double safeLevel;
                    if (this.component.CarrierType == CarrierTypeEnum.StaticWaffle)
                    {
                        safeLevel = this.bondModuleController.GetBondheadSafeLevel(targetPos);
                    }
                    else
                    {
                        safeLevel = this.ULMPara.AboveIPTRightPos.Z + nozzleHeightOffset;
                    }

                    ret = this.AbsMoveToIPT(targetPos, safeLevel);
                    if (ret != ExcuteResult.Success)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                    $"运动中转台到失败！\r\n",
                                    "报警",
                                    new string[] { "确认" },
                                    new DialogResult[] { DialogResult.OK },
                                    AlarmLevel.FirstLevel);

                        return ret;
                    }

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    // 去中转台放片位
                    if (this.component.CarrierType == CarrierTypeEnum.StaticWaffle)
                    {
                        // 静态华夫盒
                        ret = this.JumpToIPTFromStaticWaffle(targetPos);
                    }
                    else
                    {
                        // 晶圆台
                        ret = this.JumpToIPTFromWaferWithULMT(targetPos);
                    }

                    break;
            }

            System2RunTimeProvider.RecordTime("中转台动作节点", $"运动到中转台预放片位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z} 结束");
            return ret;
        }

        /// <summary>
        /// 晶圆台去中转台 ULM运动(默认四轴同时插补)
        /// </summary>
        /// <param name="targetPos">上视位置</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToIPTFromWaferWithULMT(AKRSPoint4D targetPos)
        {
            DsaIpolGroup iGroup = null;

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(targetPos) == false)
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
                            CommonUtil.SetCurrentThreadName("去中转台T轴单独运动线程");

                            System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                            while (true)
                            {
                                if (Math.Abs(this.bondHeadController.GetAxisZRealPos() - startLevel) >= 2)
                                {
                                    this.bondHeadController.RotateAxisT(targetPos.T);
                                    break;
                                }

                                // 超时抛异常
                                if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 20000)
                                {
                                    throw new Exception("去中转台T轴单独运动超时！");
                                }
                            }
                        });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // T轴旋转间距
                double rotatePitch = (targetPos.T - curTPos) / 2;

                // TU安全高度
                double tuSafeHeight = this.system2Controller.GetTUMachineSafeHeight();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULMPara.JumpToIPTSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULMPara.JumpToIPTAccelerationTime;
                interpolationParam.AccAcc = this.ULMPara.JumpToIPTJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                // 吸嘴高度补偿
                double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

                // 当前位置高于环光直接抛异常
                if (this.bondHeadController.GetAxisZRealPos()
                    > (this.ULMPara.AboveWaferRingLightPos.Z + nozzleHeightOffset))
                {
                    Machine.GetInstance().Stop();
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $"致命异常:当前Z轴高度错误! 设备将退出自动工作!\r\n",
                        "异常",
                        new string[] { "确认" },
                        new DialogResult[] { DialogResult.OK },
                        AlarmLevel.SecondLevel);

                    Machine.GetInstance().Stop();
                    LogHelper.Post(Level.Info, $"中转台流程:运动到上视拍照位位运行故障！", LogCategory.Bond);
                    return ExcuteResult.Exception;
                }

                interpolationParam.SegmentConfigs = new SegmentConfig[4] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                if (MachineHardwareConfiguration.GetInstance().IsNozzleCleanTableConfigured == false)
                {
                    // 第一个点 应该抬起到晶圆环光上面,T轴不转
                    double aboveRingLightHeight = this.ULMPara.AboveWaferRingLightPos.Z + nozzleHeightOffset;
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = aboveRingLightHeight, T = this.bondHeadController.GetAxisTRealPos() };

                    // 第二段，运动到中转台右上方 X: -40  Y:  upLookPos.Y / 1000.0
                    // 中转台和晶圆台之间的过渡点
                    AKRSPoint3D betweenWaferAndIPTPos = this.component.IPTType == IPTTypeEnum.LeftIPT
                                                            ? new AKRSPoint3D()
                                                            {
                                                                X = this.ULMPara.AboveIPTRightPos.X,
                                                                Z = this.ULMPara.AboveIPTRightPos.Z
                                                                          + nozzleHeightOffset,
                                                            }:new AKRSPoint3D()
                                                                  {
                                                                      X = this.ULMPara.AboveRightIPTLeftPos.X,
                                                                      Z = this.ULMPara.AboveRightIPTLeftPos.Z
                                                                          + nozzleHeightOffset,
                                                                  }
                                                            ;

                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = betweenWaferAndIPTPos.X, Y = targetPos.Y, Z = betweenWaferAndIPTPos.Z, T = curTPos + rotatePitch };

                    // 第三个点 运动到中转台放片位正上方0.5mm,这个点是防止芯片剐蹭
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z + 0.5, T = targetPos.T };


                    // 第四个点 运动到放片位，只动Z轴
                    interpolationParam.SegmentConfigs[3].Point = targetPos;
                }
                else
                {
                    // 第一个点 应该抬起到晶圆环光上面,T轴不转
                    double aboveRingLightHeight = this.ULMPara.AboveWaferRingLightPos.Z + nozzleHeightOffset;
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = aboveRingLightHeight, T = this.bondHeadController.GetAxisTRealPos() };

                    // 第二段，运动到中转台右上方 X: -40  Y:  upLookPos.Y / 1000.0
                    // 中转台右上位置
                    AKRSPoint3D iPTRightPos = this.component.IPTType == IPTTypeEnum.LeftIPT? new AKRSPoint3D()
                    {
                        X = this.ULMPara.AboveIPTRightPos.X,
                        Z = this.ULMPara.AboveIPTRightPos.Z + nozzleHeightOffset,
                    }: new AKRSPoint3D()
                    {
                        X = this.ULMPara.AboveRightIPTLeftPos.X,
                        Z = this.ULMPara.AboveRightIPTLeftPos.Z + nozzleHeightOffset,
                    };

                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = iPTRightPos.X, Y = targetPos.Y, Z = iPTRightPos.Z, T = curTPos + rotatePitch };

                    // 如果有清洁台走要保证不与清洁台干涉
                    // 第三个点 运动到中转台放片位正上方，只动XYT
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = iPTRightPos.Z, T = targetPos.T };


                    // 第四个点 运动到放片位，只动Z轴
                    interpolationParam.SegmentConfigs[3].Point = targetPos;
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULMPara.JumpToIPTTime,
                    RadiusRatio = this.ULMPara.JumpToIPTRadiusRatio
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

                System2RunTimeProvider.RecordTime("中转台流程 ", $"晶圆台Jump到中转台放片位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("中转台流程 ", $"晶圆台Jump到中转台放片位结束");


                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"晶圆台Jump到中转台放片位 ，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"晶圆台Jump到中转台放片位！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 静态华夫盒去中转台 ULM运动(默认四轴同时插补)
        /// </summary>
        /// <param name="targetPos">上视位置</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToIPTFromStaticWaffle(AKRSPoint4D targetPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(targetPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                        {
                            this.bondHeadController.RotateAxisT(targetPos.T);
                        });
                }

                // 安全高度
                double safeHeight = this.bondModuleController.GetBondheadSafeLevel(targetPos);

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULMPara.JumpToIPTSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULMPara.JumpToIPTAccelerationTime;
                interpolationParam.AccAcc = this.ULMPara.JumpToIPTJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                // 先上抬到安全高度
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = safeHeight, T = this.bondHeadController.GetAxisTRealPos() };

                // 第二段，XYT运动到中转台正上方 
                interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = safeHeight, T = targetPos.T };

                // 第3段，Z运动到放片位，只动Z轴
                interpolationParam.SegmentConfigs[2].Point = targetPos;

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULMPara.JumpToIPTTime,
                    RadiusRatio = this.ULMPara.JumpToIPTRadiusRatio
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

                System2RunTimeProvider.RecordTime("中转台流程 ", $"静态华夫盒Jump到中转台放片位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("中转台流程 ", $"静态华夫盒Jump到中转台放片位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到中转台，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到中转台失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去中转台拍照位
        /// </summary>
        /// <param name="targetPos">目标位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToIPTVisionPos(AKRSPoint3D targetPos)
        {
            System2RunTimeProvider.RecordTime("中转台流程", $"准备去中转台拍照位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    this.AbsMoveToVisionPos(targetPos);

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    ret = this.JumpToVisionPos(targetPos);
                    break;
            }

            System2RunTimeProvider.RecordTime("中转台流程", $"去中转台拍照位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z} 结束");

            return ret;
        }

        /// <summary>
        /// 去中转台拍照位（ULM）
        /// </summary>
        /// <param name="targetPos">目标位置</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToVisionPos(AKRSPoint3D targetPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(targetPos) == false)
            {
                return ExcuteResult.Exception;
            }

            try
            {
                // 吸嘴高度补偿
                double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

                // Z轴当前位置
                AKRSPoint3D curPos = this.bondModuleController.Get3DRealPosition();

                double tPos = this.bondHeadController.GetAxisTRealPos();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULMPara.JumpToIPTVisionPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULMPara.JumpToIPTVisionPosAccelerationTime;
                interpolationParam.AccAcc = this.ULMPara.JumpToIPTVisionPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                // 第一个点 Z轴抬起 0.5mm
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = curPos.Z + 0.5, T = this.bondHeadController.GetAxisTRealPos() };

                // 第二段，xyz运动到拍照位
                interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z, T = tPos };

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULMPara.JumpToIPTVisionPosTime,
                    RadiusRatio = this.ULMPara.JumpToIPTVisionPosRadiusRatio
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

                System2RunTimeProvider.RecordTime("中转台流程 ", $"Jump到中转台拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                System2RunTimeProvider.RecordTime("中转台流程 ", $"Jump到中转台拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"Jump到中转台拍照位 ，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"Jump到中转台拍照位 ，UML运动失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去中转台取片位
        /// </summary>
        /// <param name="targetPos">目标位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToIPTPickPos(AKRSPoint4D targetPos)
        {
            System2RunTimeProvider.RecordTime("中转台流程", $"准备去中转台取片位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    this.AbsMoveToIPTPickPos(targetPos);

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    ret = this.JumpToPickIPTPos(targetPos);
                    break;
            }

            System2RunTimeProvider.RecordTime("中转台流程", $"去中转台取片位 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z} 结束");

            return ret;
        }

        /// <summary>
        /// 去中转台预取片位（ULM）
        /// </summary>
        /// <param name="pickPos">目标位置</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToPickIPTPos(AKRSPoint4D pickPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(pickPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("Jump到中转台取片位T轴单独运动线程");
                            this.bondHeadController.RotateAxisT(pickPos.T);
                        });
                }

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.ULMPara.JumpToIPTPickPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.ULMPara.JumpToIPTPickPosAccelerationTime;
                interpolationParam.AccAcc = this.ULMPara.JumpToIPTPickPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();


                interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                double slowTravelBeforePick = this.component.IsActivateSlowTravelBeforePickup
                                                  ? this.component.IPTSlowTravelDistanceBeforePickup
                                                  : 0;

                // 第一个点 到取片位正上方1mm
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D()
                {
                    X = pickPos.X,
                    Y = pickPos.Y,
                    Z = pickPos.Z /*+ slowTravelBeforePick*/ + 1,
                    T = pickPos.T
                };

                // 第2个点 到取片位
                interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D()
                {
                    X = pickPos.X,
                    Y = pickPos.Y,
                    Z = pickPos.Z /*+ slowTravelBeforePick*/,
                    T = pickPos.T
                };

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.ULMPara.JumpToIPTPickPosTime,
                    RadiusRatio = this.ULMPara.JumpToIPTPickPosRadiusRatio
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

                System2RunTimeProvider.RecordTime("中转台流程 ", $"Jump到中转台取片位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask?.Wait();
                }

                System2RunTimeProvider.RecordTime("中转台流程 ", $"Jump到中转台取片位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"去中转台取料位 ，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"去中转台取料位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去中转台（绝对运动）
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeZ">Z轴安全位置</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToIPT(AKRSPoint4D point4D, double safeZ)
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

            // Z轴先去安全高度
            this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(safeZ, velZ);

            Stopwatch sp = Stopwatch.StartNew();
            //if (this.Component.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                while (true)
                {
                    double curLvel = this.bondModule.BondHead.AxisZ.GetRealPosition();

                    // 到安全高度开始动xy
                    if (Math.Abs(curLvel - safeZ) < 0.01)
                    {
                        // 开始运动
                        this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                        this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                        this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);
                        break;
                    }

                    Thread.Sleep(1);

                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        return ExcuteResult.Exception;
                    }
                }

                while (true)
                {
                    if (Math.Abs(this.bondModule.BondAxisX.GetRealPosition() - point4D.X) < 0.1
                       && Math.Abs(this.bondModule.BondAxisY.GetRealPosition() - point4D.Y) < 0.1)
                    {
                        this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);
                        break;
                    }

                    Thread.Sleep(1);

                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        return ExcuteResult.Exception;
                    }
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
        /// 绝对运动到视觉位置（绝对运动）
        /// </summary>
        /// <param name="targetPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToVisionPos(AKRSPoint3D targetPos)
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

            // Z轴先去安全高度
            this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(targetPos.Z, velZ);

            Stopwatch sp = Stopwatch.StartNew();
            while (true)
            {
                // z轴先抬高1mm
                if (this.bondModule.BondHead.AxisZ.GetRealPosition() >= curZPos + 1)
                {
                    // 开始运动
                    this.bondModule.BondAxisX.SendAbsoluteMoveCommand(targetPos.X, velX);
                    this.bondModule.BondAxisY.SendAbsoluteMoveCommand(targetPos.Y, velY);
                    break;
                }

                Thread.Sleep(1);

                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, targetPos.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, targetPos.Y, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisZ, true, targetPos.Z, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 绝对运动到中转台取片位（绝对运动）
        /// </summary>
        /// <param name="targetPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToIPTPickPos(AKRSPoint4D targetPos)
        {
            double velX = this.bondModule.BondAxisX.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velY = this.bondModule.BondAxisY.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;


            double velZ = this.bondModule.BondHead.AxisZ.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            double velT = this.bondModule.BondHead.AxisT.AxisMovePara.AbsoluteMoveSpeed
                          * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 开始运动
            this.bondModule.BondAxisX.SendAbsoluteMoveCommand(targetPos.X, velX);
            this.bondModule.BondAxisY.SendAbsoluteMoveCommand(targetPos.Y, velY);
            this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(targetPos.T, velT);

            Stopwatch sp = Stopwatch.StartNew();
            while (true)
            {
                if (Math.Abs(this.bondModuleController.GetAxisYRealPos() - targetPos.Y) < 0.1 && Math.Abs(this.bondModuleController.GetAxisXRealPos() - targetPos.X) < 0.1)
                {
                    // 接近取片位开始移动Z轴
                    this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(targetPos.Z, velZ);
                    break;
                }

                Thread.Sleep(1);

                // 超时
                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, targetPos.X, AccuracyMode.HighSpeed),
                (this.bondModule.BondAxisY, true, targetPos.Y, AccuracyMode.HighSpeed),
                (this.bondModule.BondHead.AxisZ, true, targetPos.Z, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisT, true, targetPos.T, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 中转台动作空跑
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult SubstrateCameraCorrectionActionDryRun()
        {
            // 获取下一颗芯片
            this.nextComponentName = this.actionNodesService.GetNextComponentName();

            // 获取当前焊点
            BondPosition bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 获取当前吸嘴
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            // 判断是哪个中转台
            AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                         ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                         : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

            // IPT位置转到Bond
            AKRSPoint3D iPTPosInBond = this.bondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

            this.placeLevel = iPTPosInBond.Z + nozzle.MeasureHeightOffset + this.component.ComponentThickness + 5;

            // Z轴去安全位
            this.bondHeadController.MoveBondZToSafePos();

            // 运动到IPT平台
            this.bondModuleController.MoveSafeBondXY(iPTPosInBond.X, iPTPosInBond.Y);

            // 模拟放片
            this.bondHeadController.MoveAxisZ(placeLevel);

            Thread.Sleep(this.component.IPTPlacementDelay);

            // 抬起
            this.bondHeadController.MoveBondZToSafePos();

            // 计算真实拍照位
            AKRSPoint3D p1RealVisionPos = this.bondModuleController.ConvertG0ToMachinePos(this.component.DownLookAdjustConfig.P1VisionPos) - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            // 轴移动到拍照位
            this.bondModuleController.MoveSafeBondXYZ(p1RealVisionPos);

            // 拍照停留
            DelayHelper.Delay(this.component.AdjustVisionDelay);

            // 判断是否两点定位
            if (this.component.IsTwoPointAdjust)
            {
                // 两点定位
                AKRSPoint3D p2VisionPos =
                    this.component.UpLookAdjustConfig.P2VisionPos;

                // 轴移动到拍照位
                this.bondModuleController.MoveToG0Pos(this.component.UpLookAdjustConfig.P2VisionPos);

                // 拍照停留
                DelayHelper.Delay(this.component.AdjustVisionDelay);
            }

            // 抬起
            this.bondHeadController.MoveBondZToSafePos();

            // 运动到IPT平台
            this.bondModuleController.MoveSafeBondXY(iPTPosInBond.X, iPTPosInBond.Y);

            // 模拟取片
            this.bondHeadController.MoveAxisZ(placeLevel);

            Thread.Sleep(this.component.IPTPickupDelay);

            // 抬起
            this.bondHeadController.MoveBondZToSafePos();

            Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("给晶圆发要料信号线程");

                        this.SendSignalToWaferTable();
                    });

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 二段速上抬
        /// </summary>
        /// <param name="liftPreLevel">抬起高度</param>
        /// <param name="pickActionParameter">取片参数</param>
        private void SlowUpAfterPick(double liftPreLevel, PickActionParameter pickActionParameter)
        {
            if (pickActionParameter.forceMode == ForceModeEnum.Distance)
            {
                if (this.component.IsActivateSlowTravelAfterPickOnIPT)
                {
                    // 低速移动到预抬起位置
                    this.bondHeadController.MoveAxisZ(
                        liftPreLevel,
                        pickActionParameter.SlowTravelSpeedAfterPickup,
                        AccuracyMode.HighSpeed,
                        false);
                }
            }
            else
            {
                System2RunTimeProvider.RecordTime("中转台流程", $"Pick 开始抬起");

                if (this.component.IsActivateSlowTravelAfterPickOnIPT)
                {
                    // 加这句是为了防止Z轴抬起时没有恢复正常速度
                    this.bondHeadController.SetAxisZSpeed(pickActionParameter.SlowTravelSpeedAfterPickup);

                    liftPreLevel = this.bondHeadController.GetAxisZRealPos() + pickActionParameter.SlowTravelDistanceAfterPickup;

                    // 力控模式低速速上抬
                    this.bondHeadController.ForceControlReset(liftPreLevel, pickActionParameter.SlowTravelSpeedAfterPickup);
                    this.bondHeadController.SetAxisZSpeed(this.initialSpeed);

                    System2RunTimeProvider.RecordTime("中转台流程", $"力控上抬完成");
                }
                else
                {
                    // 原地退出力控
                    this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos(), this.initialSpeed, component.ForceControlOutTime);
                    System2RunTimeProvider.RecordTime("中转台流程", $"力控高速抬起完成");
                }
            }
        }

        /// <summary>
        /// 二段速去放片高度
        /// </summary>
        /// <param name="bondActionParameter">固精参数</param>
        private void SlowDownToPlaceLevel(BondActionParameter bondActionParameter)
        {
            if (bondActionParameter.forceMode == ForceModeEnum.Distance)
            {
                if (this.component.IsActivateSlowTravelBeforePlaceOnIPT)
                {
                    // 低速移动到固晶高度  如果未开启二段速 则在前一步Jump的时候已经到达了固晶高度
                    this.bondHeadController.MoveAxisZ(this.placeLevel, bondActionParameter.SlowTravelSpeedBeforePlace, AccuracyMode.HighAccuracy, false);
                }
            }
            else
            {
                // 力控模式低速下压
                // 这里不减0.01会一直往上走
                this.bondHeadController.ForceControlSet(
                    bondActionParameter.BondForce,
                    this.placeLevel - 0.01,
                    this.zSpeed,
                    this.component.ForceControlOutTime,
                    bondActionParameter.SlowTravelDistanceBeforePlace);
            }
        }

        /// <summary>
        /// 焊头低速上抬
        /// </summary>
        /// <param name="liftPreLevel">抬起高度</param>
        /// <param name="bondActionParameter">固精参数</param>
        private void SlowUpAfterPlace(double liftPreLevel, BondActionParameter bondActionParameter)
        {
            if (bondActionParameter.forceMode == ForceModeEnum.Distance)
            {
                if (this.component.IsActivateSlowTravelAfterPlaceOnIPT)
                {
                    // 低速上抬到预固晶位
                    this.bondHeadController.MoveAxisZ(
                        liftPreLevel,
                        bondActionParameter.SlowTravelSpeedAfterPlace,
                        AccuracyMode.HighSpeed,
                        false);
                }
            }
            else
            {
                if (this.component.IsActivateSlowTravelAfterPlaceOnIPT)
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
                        "Place",
                        $"力控低速上抬完成，速度{bondActionParameter.SlowTravelSpeedAfterPlace}");
                }
                else
                {
                    // 原地退出力控模式
                    this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos(), this.zSpeed);
                }
            }
        }
    }
}
