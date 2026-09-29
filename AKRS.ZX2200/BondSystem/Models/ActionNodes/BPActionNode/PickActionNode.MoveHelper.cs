using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.LogicHardware.Services;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
using ch.etel.edi.dsa.v40;
using log4net.Core;
using PostSharp;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    /// 存放PickActionNode轴移动相关方法
    /// </summary>
    public partial class PickActionNode
    {
        /// <summary>
        ///  Bond模组
        /// </summary>
        private BondModule bondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 去预取片位置
        /// </summary>
        /// <param name="prePickPos">取片位置</param>
        /// <param name="liftSafeLevel">取片抬起安全高度</param>
        /// <param name="carrierType">芯片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToPrePickPos(
            AKRSPoint4D prePickPos,
            double liftSafeLevel,
            CarrierTypeEnum carrierType,
            SearchCameraEnum searchCamera)
        {
            if (component.CarrierType != CarrierTypeEnum.StaticWaffle && MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                if (component.IsUseFlipTable) 
                {
                    if (WaferSubController.GetInstance().FlipController.IsFlipTableAtTransferPos() == false)
                    {
                        throw new Exception("翻转台不在交接位，取片失败！");
                    }
                }
                else
                {
                    if (WaferSubController.GetInstance().FlipController.IsFlipTableAtHome() == false)
                    {
                        throw new Exception("翻转台不在0位，有撞机风险！");
                    }
                }
            }

            System2RunTimeProvider.RecordTime("PickAction", $"准备运动到预取片位 X:{prePickPos.X},Y:{prePickPos.Y},Z:{prePickPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            if (System2RunTimeProvider.IsRepickComponent)
            {
                // 重取用安全的绝对运动
                this.bondModuleController.MoveSafeBondXYZT(prePickPos);

                System2RunTimeProvider.IsRepickComponent = false;
            }
            else
            {
                switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
                {
                    case AxisMoveModeEnum.AbsoluteMove:
                        ret = this.AbsMoveToPickPos(prePickPos, carrierType);
                        if (ret != ExcuteResult.Success)
                        {
                            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                        $"运动到取片位失败！\r\n",
                                        "报警",
                                        new string[] { "确认" },
                                        new DialogResult[] { DialogResult.OK },
                                        AlarmLevel.FirstLevel);

                            return ret;
                        }

                        break;

                    case AxisMoveModeEnum.InterpolationMove:

                        ret = this.JumpToPickupPos(prePickPos, liftSafeLevel, carrierType, searchCamera);
                        break;
                }
            }

            System2RunTimeProvider.RecordTime("PickAction", $"运动到预取片位 X:{prePickPos.X},Y:{prePickPos.Y},Z:{prePickPos.Z} 结束");

            return ret;
        }

        /// <summary>
        /// 去取片位置，插补
        /// </summary>
        /// <param name="pickupPos">取片位置</param>
        /// <param name="liftSafeLevel">取片抬起安全高度</param>
        /// <param name="carrierType">芯片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToPickupPos(AKRSPoint4D pickupPos, double liftSafeLevel, CarrierTypeEnum carrierType,SearchCameraEnum searchCamera)
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
                // TU安全高度
                double tuSafeHeight = this.system2Controller.GetTUMachineSafeHeight();

                double startLevel = this.bondHeadController.GetAxisZRealPos();

                AKRSPoint3D curPos = this.bondModuleController.Get3DRealPosition();

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    // T轴单独运动
                    axisTMoveTask = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("Jump到取片位T轴单独运动线程");

                            System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();

                            if (carrierType == CarrierTypeEnum.StaticWaffle
                                || searchCamera == SearchCameraEnum.BondCamera) 
                            {
                                this.bondHeadController.RotateAxisT(pickupPos.T);
                            }
                            else
                            {
                                while (true)
                                {
                                    if (this.bondHeadController.GetAxisZRealPos() >= tuSafeHeight)
                                    {
                                        this.bondHeadController.RotateAxisT(pickupPos.T);
                                        break;
                                    }
                                    else
                                    {
                                        if (Math.Abs(
                                                this.bondHeadController.GetAxisZRealPos() - startLevel) >= 1)
                                        {
                                            this.bondHeadController.RotateAxisT(pickupPos.T);
                                            break;
                                        }
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 10000)
                                    {
                                        throw new Exception("Jump到取片位T轴单独运动超时！");
                                    }
                                }
                            }
                        });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // T轴旋转间距
                double rotatePitch = (pickupPos.T - curTPos) / 2;

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

                if (carrierType == CarrierTypeEnum.StaticWaffle)
                {
                    // 防呆：如果当前位置低于TU安全高度，直接抛异常
                    if (this.bondHeadController.GetAxisZRealPos() < tuSafeHeight)
                    {
                        Machine.GetInstance().Stop();
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"致命异常:ULM运动到取片位前Z轴低于安全高度! 设备将退出自动工作!\r\n",
                            "致命异常",
                            new string[] { "确认" },
                            new DialogResult[] { DialogResult.OK },
                            AlarmLevel.SecondLevel);

                        Machine.GetInstance().Stop();
                        LogHelper.Post(Level.Info, $"静态华夫盒取片流程:运动到取片位运行故障！", LogCategory.Bond);
                        return ExcuteResult.Exception;
                    }

                    interpolationParam.SegmentConfigs = new SegmentConfig[1] { new SegmentConfig() };
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = pickupPos.X, Y = pickupPos.Y, Z = pickupPos.Z + 0.5, T = pickupPos.T };
                }
                else if (searchCamera == SearchCameraEnum.BondCamera)
                {
                    AKRSPoint3D waferRingLightLeftLimitPos = this.uLMPara.WaferRingLightLeftLimitPos;
                    double safeLevel = waferRingLightLeftLimitPos.Z
                                       + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 5;

                    interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                    // 第一点：上抬安全高度
                    interpolationParam.SegmentConfigs[0].Point =
                        new AKRSPoint4D() { X = curPos.X, Y = curPos.Y, Z = safeLevel, T = curTPos + rotatePitch };

                    // 第二点：XY运动到取片位
                    interpolationParam.SegmentConfigs[1].Point =
                        new AKRSPoint4D() { X = pickupPos.X, Y = pickupPos.Y, Z = safeLevel, T = pickupPos.T };

                    // 第三点：Z轴运动到取片位
                    interpolationParam.SegmentConfigs[2].Point =
                        new AKRSPoint4D() { X = pickupPos.X, Y = pickupPos.Y, Z = pickupPos.Z, T = pickupPos.T };
                }
                else
                {
                    // 低于安全高度就先抬到安全高度
                    if (this.bondHeadController.GetAxisZRealPos() < liftSafeLevel)
                    {
                        interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                        // 第一段，Z上抬到安全高度
                        interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = liftSafeLevel, T = this.bondHeadController.GetAxisTRealPos() };

                        // XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                        interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = pickupPos.X, Y = this.uLMPara.TransportUnitEdgePos.Y, Z = liftSafeLevel, T = curTPos + rotatePitch };

                        // XYT运动到取料位置
                        interpolationParam.SegmentConfigs[2].Point = pickupPos;
                    }
                    else
                    {
                        interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                        // XY运动到轨道外延(-150) 且T 运动到取料角度的 1 / 2 Z不动
                        interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = pickupPos.X, Y = this.uLMPara.TransportUnitEdgePos.Y, Z = liftSafeLevel, T = curTPos + rotatePitch };

                        // XYT运动到取料位置
                        interpolationParam.SegmentConfigs[1].Point = pickupPos;
                    }
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

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到取片位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                this.isReadPickupForce = false;
                LogHelper.Post(Level.Error, $"运动到取片位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                this.isReadPickupForce = false;
                LogHelper.Post(Level.Error, $"运动到取片位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去静态华夫盒视觉位
        /// </summary>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToStaticWaffleVisionPos(AKRSPoint4D visionPos)
        {
            System2RunTimeProvider.RecordTime("PickAction", $"准备去静态华夫盒视觉位 X:{visionPos.X},Y:{visionPos.Y},Z:{visionPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    this.bondModuleController.MoveSafeBondXYZT(visionPos);

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    ret = this.JumpToStaticWaffleVisionPos(visionPos);
                    break;
            }

            System2RunTimeProvider.RecordTime("PickAction", $"去静态华夫盒视觉位  X:{visionPos.X},Y:{visionPos.Y},Z:{visionPos.Z} 结束");

            return ret;
        }

        /// <summary>
        /// 去晶圆台搜晶位
        /// </summary>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToWaferTableSearchPos(AKRSPoint3D visionPos)
        {
            System2RunTimeProvider.RecordTime("PickAction", $"准备去晶圆台搜晶位 X:{visionPos.X},Y:{visionPos.Y},Z:{visionPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            if (System2RunTimeProvider.IsRepickComponent)
            {
                // 重取用安全的绝对运动
                this.bondModuleController.MoveSafeBondXYZ(visionPos);

                System2RunTimeProvider.IsRepickComponent = false;
            }
            else
            {
                switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
                {
                    case AxisMoveModeEnum.AbsoluteMove:
                        this.bondModuleController.MoveSafeBondXYZ(visionPos);

                        break;

                    case AxisMoveModeEnum.InterpolationMove:

                        ret = this.JumpToWaferTableSearchPos(visionPos);
                        break;
                }
            }

            System2RunTimeProvider.RecordTime("PickAction", $"去晶圆台搜晶位  X:{visionPos.X},Y:{visionPos.Y},Z:{visionPos.Z} 结束");

            return ret;
        }

        /// <summary>
        /// 去静态华夫盒视觉位,插补
        /// </summary>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToStaticWaffleVisionPos(AKRSPoint4D visionPos)
        {
            DsaIpolGroup iGroup = null;

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(visionPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                if (this.nozzleShelfController.IsNozzleShelfAtHome() == false) 
                {
                    // 吸嘴架Y回到原位
                    //this.nozzleShelfController.MoveShelfToHome();

                    //System2RunTimeProvider.RecordTime("PickAction", $"去静态华夫盒视觉位-吸嘴架缩回");

                    throw new Exception("去静态华夫盒视觉位失败:检测到吸嘴架未缩回，有撞机风险！");
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // T轴旋转间距
                double rotatePitch = (visionPos.T - curTPos) / 2;

                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                          ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                          : 0;

                // 安全高度设置为静态华夫盒高度
                double safeHeight = avoidPos.Z + nozzleOffset;

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    // T轴单独运动
                    axisTMoveTask = Task.Run(
                      () =>
                      {
                          CommonUtil.SetCurrentThreadName("Jump到上视取片位T轴单独运动线程");

                          this.bondHeadController.RotateAxisT(visionPos.T);
                      });
                }

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToStaticWaffleVisionPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToStaticWaffleVisionPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToStaticWaffleVisionPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                if (this.bondHeadController.GetAxisZRealPos() < safeHeight)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                    // 先上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = safeHeight, T = this.bondHeadController.GetAxisTRealPos() };
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = visionPos.Z, T = visionPos.T };
                }
                else
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[1] { new SegmentConfig() };
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = visionPos.Z, T = visionPos.T };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToStaticWaffleVisionPosTime,
                    RadiusRatio = this.uLMPara.JumpToStaticWaffleVisionPosRadiusRatio
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

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到芯片拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);


                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到芯片拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到芯片拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到芯片拍照位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去晶圆台搜晶位,插补
        /// </summary>
        /// <param name="visionPos">位置</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToWaferTableSearchPos(AKRSPoint3D visionPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            // 限位检测
            if (this.bondModuleController.CheckSoftLimit(visionPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // 安全高度
                double safeHeight = this.bondModuleController.GetBondheadSafeLevel(visionPos);

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToWaferSearchPosSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToWaferSearchPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToWaferSearchPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                double curZ = this.bondHeadController.GetAxisZRealPos();

                if (curZ < safeHeight)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                    // 先上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = safeHeight, T = this.bondHeadController.GetAxisTRealPos() };

                    // XY去目标位
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = safeHeight, T = curTPos };

                    // Z下降
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = visionPos.Z, T = curTPos };
                }
                else
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                    // XY去目标位
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = curZ, T = curTPos };

                    // Z下降
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = visionPos.X, Y = visionPos.Y, Z = visionPos.Z, T = curTPos };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToWaferSearchPosTime,
                    RadiusRatio = this.uLMPara.JumpToWaferSearchPosRadiusRatio
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

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到芯片拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                System2RunTimeProvider.RecordTime("PickAction", $"Jump到芯片拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到芯片拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到芯片拍照位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去取片位
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="carrierType">芯片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToPickPos(AKRSPoint4D point4D, CarrierTypeEnum carrierType)
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
            double safeLevel = this.bondModuleController.GetBondheadSafeLevel(point4D);

            if (carrierType == CarrierTypeEnum.StaticWaffle) 
            {
                AKRSPoint3D avoidPos = this.bondModule.ConvertG0ToMachinePos(
                    WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

                double nozzleOffset = this.bondModule.BondHead.CurrentNozzle != null
                                          ? this.bondModule.BondHead.CurrentNozzle.MeasureHeightOffset
                                          : 0;

                // 若目标位置在静态华夫盒区域，则安全高度设置为避让位高度
                safeLevel = avoidPos.Z + nozzleOffset;
            }

            if (curZPos < safeLevel)
            {
                // Z轴先去安全高度
                this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(safeLevel, velZ);
            }

            Stopwatch sp = Stopwatch.StartNew();
            while (true)
            {
                double curLevel = this.bondHeadController.GetAxisZRealPos();

                // 达到安全高度
                if (Math.Abs(curLevel - safeLevel) < 0.001 || curLevel >= safeLevel)
                {
                    // 开始运动
                    this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                    this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                    this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);
                    break;
                }

                Thread.Sleep(1);

                // 超时
                if (sp.Elapsed.TotalSeconds > 10)
                {
                    return ExcuteResult.Exception;
                }
            }

            // 静态华夫盒
            //if (carrierType == CarrierTypeEnum.StaticWaffle)
            {
                sp.Restart();

                while (true)
                {
                    // XY轴接近目标位时Z开始动
                    if (Math.Abs(this.bondModule.BondAxisX.GetRealPosition() - point4D.X) < 1 && Math.Abs(this.bondModule.BondAxisY.GetRealPosition() - point4D.Y) < 1)
                    {
                        // Y轴安全开始移动Z轴
                        bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);
                        break;
                    }

                    Thread.Sleep(1);

                    // 超时
                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        return ExcuteResult.Exception;
                    }
                }
            }
            //else
            //{
            //    sp.Restart();

            //    while (true)
            //    {
            //       double  posY=this.bondModuleController.Get3DRealPosition().Y;
            //        double posX =this.bondModuleController.Get3DRealPosition().X;

            //        // todo:X暂时写死,后面要改成示教的坐标
            //        if (posY < this.uLMPara.TransportUnitEdgePos.Y
            //            && posX > -16&& posX< this.uLMPara.WaferRingLightRightLimitPos.X)
            //        {
            //            // Y轴安全开始移动Z轴
            //            bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);
            //            break;
            //        }

            //        Thread.Sleep(1);

            //        // 超时
            //        if (sp.Elapsed.TotalSeconds > 10)
            //        {
            //            return ExcuteResult.Exception;
            //        }
            //    }
            //}

            // 等轴到位
            MotionService.WaitAxesArrival(
                (bondModule.BondAxisX, true, point4D.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, point4D.Y, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisZ, true, point4D.Z, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisT, true, point4D.T, AccuracyMode.HighAccuracy));

            return ExcuteResult.Success;
        }

        /// <summary>
        /// Pick动作空跑
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult PickActionDryRun()
        {
            BaseCarrierConfig component = this.actionNodesService.GetCurrentComponent().component;

            // 获取当前焊点
            BondPosition bondPosition = this.actionNodesService.GetCurrentBondPosition();

            AKRSPoint2D bondHeadRotateCenterInWC = CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC;

            // 晶圆环光位置
            double aboveWaferRingLightPos = this.uLMPara.AboveWaferRingLightPos.Z;

            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            // 允许取料信号判断（从信号池获取）,这个信号会自动复位
            if (System2RunTimeProvider.WaitAllowBondPickSignal(component) != ExcuteResult.Success)
            {
                return ExcuteResult.Abort;
            }

            // 静态华夫盒：定位并获取芯片定位角度
            if (component.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                // 获取拍照位
                AKRSPoint3D componentVisionPosInG0 = Block.GetInstance().GetResultDieG0Pos();
                AKRSPoint3D componentVision3DPos =
                    this.bondModuleController.ConvertG0ToMachinePos(componentVisionPosInG0);

                AKRSPoint4D componentVision4DPos = new AKRSPoint4D()
                {
                    X = componentVision3DPos.X,
                    Y = componentVision3DPos.Y,
                    Z = componentVision3DPos.Z,
                    T = nozzle.AlignAngle
                };

                // 去静态华夫盒拍照位
                this.bondModuleController.MoveSafeBondXYZT(componentVision4DPos);
            }
            else
            {
                // todo:FC芯片待验证
                //if (component.IsUseFlipTable)
                //{
                //    // 去翻转台
                //    // 获取翻转工具
                //    FlipTool flipTool =
                //        WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                //    AKRSPoint3D pos = new AKRSPoint3D()
                //                          {
                //                              X = flipTool.FlipToolPositionXY.X + component.PickupOffset.X,
                //                              Y = flipTool.FlipToolPositionXY.Y + component.PickupOffset.Y,
                //                              Z = flipTool.FlipToolPositionZ + nozzle.MeasureHeightOffset
                //                                                             + component.ComponentThickness
                //                          };

                //    this.bondModuleController.MoveSafeBondXYZ(pos);
                //}
                //else
                //{
                    // 移动XY去晶圆相机中心
                    this.bondModuleController.MoveSafeBondXY(bondHeadRotateCenterInWC.X, bondHeadRotateCenterInWC.Y);

                    // 空跑不取料
                    this.bondHeadController.MoveAxisZ(aboveWaferRingLightPos);
                //}
            }


            // 抬起
            this.bondHeadController.MoveBondZToSafePos();

            // 这里阻塞是为了防止点暂停的时候晶圆台移动
            if (Signal.WaitStart() == false)
            {
                return ExcuteResult.Abort;
            }

            // 翻转台芯片不在这这里发信号。会挡晶圆相机视野
            if (component.IsUseFlipTable == false)
            {
                Task.Run(
                                       () =>
                                       {
                                           CommonUtil.SetCurrentThreadName("给晶圆发要料信号线程");

                                           // 判断是不是最后一颗芯片
                                           if ((nextComponentName == component.Name && nextComponentName != null)
                                              || component.AccuracyMode == AccuracyModeEnum.Off)
                                           {
                                               // 如果不是最后一颗就在这里发要料信号
                                               // 盲贴也在这里发
                                               // 刷新Map信息
                                               WaferSystemDomain.GetInstance().Block.RefreshMap();

                                               // 芯片名称传给晶圆台
                                               WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                                               // 给晶圆台发要料信号
                                               SignalPool.GetInstance().IsBondNeedChipSignal.Set();
                                           }
                                       });
            }
            else
            {
                Task.Run(
                                       () =>
                                       {
                                           CommonUtil.SetCurrentThreadName("给翻转台发取料成功信号线程");

                                           // 翻转台左上位置
                                           AKRSPoint3D flipTableLeftTopPos =
                                               this.bondModuleController.ConvertG0ToMachinePos(
                                                   flipDevicePara.FlipTableLeftTopPos);

                                           // 翻转台右下位置
                                           AKRSPoint3D flipTableRightBottomPos =
                                               this.bondModuleController.ConvertG0ToMachinePos(
                                                   flipDevicePara.FlipTableRightBottomPos);

                                           var startTime = DateTime.Now;
                                           while (true)
                                           {
                                               if (MachineStateModel.GetInstance().MachineState == MachineStateEnum.Pause)
                                               {
                                                   startTime = DateTime.Now;
                                                   Thread.Sleep(20);
                                                   continue;
                                               }

                                               if (DateTime.Now - startTime > TimeSpan.FromSeconds(10))
                                               {
                                                   DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                                       $"给翻转台发取料成功信号超时! \r\n",
                                                       "真空报警",
                                                       new string[] { "忽略", "退出" },
                                                       new DialogResult[]
                                                           {
                                                                   DialogResult.Ignore, DialogResult.Abort
                                                           },
                                                       AlarmLevel.SecondLevel);

                                                   switch (dialogResult)
                                                   {
                                                       case DialogResult.Ignore:
                                                           startTime = DateTime.Now;
                                                           break;

                                                       case DialogResult.Abort:
                                                           Machine.GetInstance().Stop();
                                                           return;
                                                   }
                                               }

                                               if (GeometryService.IsBond2DPosInRange(flipTableLeftTopPos, flipTableRightBottomPos))
                                               {
                                                   Thread.Yield();
                                                   continue;
                                               }

                                               // 给翻转台发取料成功信号
                                               SignalPool.GetInstance().IsBondPickSucceedSignal.Set();

                                               break;
                                           }
                                       });
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 二段速上抬
        /// </summary>
        /// <param name="liftPreLevel">抬起高度</param>
        /// <param name="pickActionParameter">取片参数</param>
        public void SlowUpToLiftLevel(double liftPreLevel, PickActionParameter pickActionParameter)
        {
            // 低速上抬动作
                if (component.PickupForceMode == ForceModeEnum.Distance)
                {
                    if (component.IsActivateSlowTravelAfterPickup)
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
                    if (component.IsActivateSlowTravelAfterPickup)
                    {
                        // 加这句是为了防止Z轴抬起时没有恢复正常速度
                        this.bondHeadController.SetAxisZSpeed(pickActionParameter.SlowTravelSpeedAfterPickup);

                        liftPreLevel = this.bondHeadController.GetAxisZRealPos()
                                       + pickActionParameter.SlowTravelDistanceAfterPickup;

                        // 力控模式低速速上抬
                        this.bondHeadController.ForceControlReset(
                            liftPreLevel,
                            pickActionParameter.SlowTravelSpeedAfterPickup,
                            component.ForceControlOutTime);

                        this.bondHeadController.SetAxisZSpeed(this.initialSpeed);
                    }
                    else
                    {
                        if (component.IsResetForceControlAdvanceDuringPickup == false)
                        {
                            // 原地退出力控
                            this.bondHeadController.ForceControlReset(
                                this.bondHeadController.GetAxisZRealPos(),
                                this.initialSpeed);
                        }
                    }
                }
            
        }
    }
}
