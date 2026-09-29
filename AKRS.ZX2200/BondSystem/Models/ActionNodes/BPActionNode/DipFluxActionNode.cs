using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using ch.etel.edi.dsa.v40;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    /// 蘸胶动作节点
    /// </summary>
    public class DipFluxActionNode : ActionNode
    {
        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition bondPosition { get; set; }

        /// <summary>
        /// 芯片
        /// </summary>
        private BaseCarrierConfig component =>
            actionNodesService.GetCurrentComponent().component;


        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara uLMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 设备参数
        /// </summary>
        private WaferSubDevicePara waferSubDevicePara => WaferSubDevicePara.GetInstance();

        /// <summary>
        /// 设备参数
        /// </summary>
        private BondDevicePara bondDevicePara => BondDevicePara.GetInstance();

        /// <summary>
        /// 设备参数
        /// </summary>
        private SlideFluxerProgram slideFluxerProgram => BondProgram.GetInstance().SlideFluxerProgram;

        /// <summary>
        /// SlideFluxerController
        /// </summary>
        private SlideFluxerController slideFluxerController = new SlideFluxerController();

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;

            System2RunTimeProvider.RecordTime("DipFluxActionNode", "Start ----------------");

            try
            {
                if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
                {
                    throw new Exception("未配置刮胶盘硬件！");
                }

                // 空跑模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.DipFluxActionDryRun();
                }

                Substrate substrate = this.system2Domain.ActionNodesService.GetCurrentSubstrate();

                // 获取当前基岛，
                Module curIslandAcupoint = this.system2Domain.ActionNodesService.GetCurrentModule();

                // 获取当前焊点
                this.bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

                System2RunTimeProvider.RecordTime("DipFluxActionNode", "开始等允许蘸胶信号");

                // 等允许蘸胶信号
                if (!SignalPool.GetInstance().IsAllowDipFluxSignal.Wait())
                {
                    return ExcuteResult.Abort;
                }

                System2RunTimeProvider.RecordTime("DipFluxActionNode", "接收到允许蘸胶信号");

                // 防呆;判断刮胶盘有没有在0位
                if (this.slideFluxerController.IsSlideFluxerAtNLimit())
                {
                    // 胶盘伸出
                    this.slideFluxerController.SlideFluxerOutWaitArrive();
                    //throw new Exception("DipFluxActionNode：刮胶盘处于缩回状态！蘸胶失败！");
                }

                // 蘸胶位
                AKRSPoint3D dipFluxPos =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        this.bondDevicePara.SlideFluxerParam.SlideFluxerPos);

                // 力控模式下不用硬补偿
                double distance = component.DipFluxForceMode == ForceModeEnum.Distance ? component.DipDistance : 0;

                // 实际蘸胶位置要加上吸嘴高度和芯片厚度
                dipFluxPos.Z = dipFluxPos.Z + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset
                                            + component.ComponentThickness + distance;

                // 蘸胶位置
                AKRSPoint4D dipFlux4DPos = new AKRSPoint4D()
                                               {
                                                   X = dipFluxPos.X,
                                                   Y = dipFluxPos.Y,
                                                   Z = dipFluxPos.Z,
                                                   T = this.bondHeadController.GetAxisTRealPos()
                                               };

                // 预蘸胶位置
                AKRSPoint4D preDipFlux4DPos = new AKRSPoint4D()
                                                  {
                                                      X = dipFluxPos.X,
                                                      Y = dipFluxPos.Y,
                                                      Z = this.component.IsActivateSlowTravelBeforeDip
                                                              ? dipFluxPos.Z + component.SlowTravelDistanceBeforeDipFlux
                                                              : dipFluxPos.Z,
                                                      T = this.bondHeadController.GetAxisTRealPos()
                                                  };

                // 安全高度
                double safelevel = this.bondModuleController.ConvertG0ToMachinePos(this.bondDevicePara.SlideFluxerParam.SlideFluxerRightBottomPos).Z
                                   + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 5;

                // 去预蘸胶位置
                ExcuteResult ret = this.JumpToSlideFluxer(preDipFlux4DPos, safelevel, component.DipMode);

             if (ret != ExcuteResult.Success)
             {
                 return ret;
             }

             System2RunTimeProvider.RecordTime("DipFluxActionNode", "准备开始蘸胶");

                // 蘸胶
                ret = this.system2Controller.DipFlux(dipFlux4DPos.Z, component);

              if (ret != ExcuteResult.Success)
              {
                  return ret;
              }

                #region 漏晶检测

                // 空跑模式不检测
                if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle)
                {
                ReCheckVacuum:

                    if (component.IsActiveComponentDetection)
                    {
                        // 检测真空
                        if (!this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
                        {
                            // 先安全位置
                            this.bondHeadController.MoveBondZToSafePos();

                            // 真空报警芯片+1
                            component.AddCountOfVacuumError();

                            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                $"蘸胶后检测到漏晶！请选择如何处理! \r\n",
                                "真空报警",
                                new string[] { "重新检测", "退出", "忽略", "重取" },
                                new DialogResult[]
                                    {
                                        DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore,
                                        DialogResult.Cancel
                                    },
                                AlarmLevel.SecondLevel);

                            switch (dialogResult)
                            {
                                case DialogResult.Retry:
                                    goto ReCheckVacuum;

                                case DialogResult.Abort:
                                    return ExcuteResult.Abort;

                                case DialogResult.Ignore:
                                    break;

                                case DialogResult.Cancel:

                                    this.bondModuleController.ThrowAction();

                                    // 抛掉的芯片+1
                                    this.component.AddCountOfReject();

                                    // 重置前面的动作节点,重新取贴
                                    List<ActionNode> actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                                    this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                                    System2RunTimeProvider.IsRepickComponent = true;

                                    return ExcuteResult.Retry;
                                        
                            }
                        }
                    }
                }

                #endregion

                //// 给刮胶盘发蘸胶完成信号
                //// 现在放到去固精位发
                //SignalPool.GetInstance().BondDipFluxFinishSignal.Set();

                System2RunTimeProvider.RecordTime("DipFluxActionNode", "Action 完成");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障", ex, LogCategory.Bond);
                AKRSMessageBoxExt.Show(ex.Message, "Exception", new string[] { "Exception" }, new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
            }
        }

        /// <summary>
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 焊点
            BondPosition bp = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }


            // 如果当前焊点已经点过胶则退出
            if (!bp.IsS2NeedStep(this.actionNodesService.GetCurrentProcessStepName()))
            {
                return false;
            }

            // 不执行蘸胶动作
            if (component.DipMode == DipModeEnum.Off)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 是否报警
        /// 这个一般是程序中出现空指针的时候会采用这个方法
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 获取当前芯片对象是否成功
            if (!System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().Item1)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"获取当前芯片失败!",
                    "Warn",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return false;
            }

            // 获取当前焊点
            BondPosition bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            if (bondPosition == null)
            {
                return false;
            }

            // 获取当前基岛，
            Module curIslandAcupoint = this.system2Domain.ActionNodesService.GetCurrentModule();

            if (curIslandAcupoint == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 去刮胶盘
        /// </summary>
        /// <param name="targetPos">目标位置</param>
        /// <param name="safeLevel">安全高度</param>
        /// <param name="dipModeEnum">蘸胶类型</param>
        /// <returns></returns>
        /// <exception cref="Exception">异常</exception>
        private ExcuteResult JumpToSlideFluxer(AKRSPoint4D targetPos, double safeLevel, DipModeEnum dipModeEnum)
        {
            switch (dipModeEnum)
            {
                case DipModeEnum.AfterAccuracyMode:

                    return this.JumpToSlideFluxerFromUplook(targetPos, safeLevel);
      
                case DipModeEnum.BeforeAccuracyMode:
                    return this.JumpToSlideFluxerFromFlipTable(targetPos, safeLevel);

                default:
                    // todo:晶圆台到刮胶盘

                    throw new Exception("排序错误，运动到刮胶盘失败！");
            }
        }

        /// <summary>
        /// 去刮胶盘
        /// </summary>
        /// <param name="targetPos">取片位置</param>
        /// <param name="safeLevel">取片抬起安全高度</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToSlideFluxerFromFlipTable(AKRSPoint4D targetPos, double safeLevel)
        {
            System2RunTimeProvider.RecordTime("DipFluxActionNode", $"准备Jump到刮胶盘 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z}");

            if (this.bondModuleController.Get2DRealPosition().X <= this.bondDevicePara.CameraDevicePara.UpLookPos.X)
            {
                throw new Exception("Jump到刮胶盘起始点位置异常！");
            }

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
                // 轴安全高度
                double safeHeight = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                double startPos = this.bondHeadController.GetAxisZRealPos();

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("Jump到刮胶盘T轴单独运动线程");

                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    if (Math.Abs(startPos - this.bondHeadController.GetAxisZRealPos()) > 1)
                                    {
                                        this.bondHeadController.RotateAxisT(targetPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        throw new Exception("Jump到刮胶盘T轴单独运动超时！");
                                    }
                                }
                            });
                }


                // T轴当前坐标
                AKRSPoint4D curPos = this.bondModuleController.Get4DRealPosition();

                // T轴旋转间距
                double rotatePitch = (targetPos.T - curPos.T) / 2;

                // 全局速度百分比
                double vel = this.uLMPara.JumpToSlideFluxerSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                InterpolationParam interpolationParam = new InterpolationParam();
                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToSlideFluxerAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToSlideFluxerJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                // 低于安全高度就先抬到安全高度
                if (this.bondHeadController.GetAxisZRealPos() < safeLevel)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[4] { new(), new(), new(), new() };

                    // 第一段，Z上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = curPos.X, Y = curPos.Y, Z = safeLevel, T = curPos.T };

                    // X运动到上视正上方 Y运动到蘸胶位置 且T 运动到取料角度的 1 / 2 Z不动
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D()
                                                                     {
                                                                         X = this.bondDevicePara.CameraDevicePara
                                                                             .UpLookPos.X,
                                                                         Y = (curPos.Y + targetPos.Y) / 2,
                                                                         Z = safeLevel,
                                                                         T = curPos.T + rotatePitch
                                                                     };

                    // XYT运动到蘸胶位置，Z不动
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = safeLevel, T = targetPos.T };

                    // Z下降到蘸胶位置
                    interpolationParam.SegmentConfigs[3].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z, T = targetPos.T };
                }
                else
                {
                    interpolationParam.SegmentConfigs =
                        new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new() };

                    // X运动到上视正上方 Y运动到蘸胶位置 且T 运动到取料角度的 1 / 2 Z不动
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondDevicePara.CameraDevicePara.UpLookPos.X, Y = targetPos.Y, Z = safeLevel, T = curPos.T + rotatePitch };

                    // XYT运动到蘸胶位置
                    interpolationParam.SegmentConfigs[1].Point =
                        new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = safeLevel, T = targetPos.T };

                    // Z运动到蘸胶位置
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z, T = targetPos.T };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToSlideFluxerTime,
                    RadiusRatio = this.uLMPara.JumpToSlideFluxerRadiusRatio
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

                System2RunTimeProvider.RecordTime("DipFluxActionNode", $"Jump到刮胶盘开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("DipFluxActionNode", $"Jump到刮胶盘结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到刮胶盘，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到取片位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 去刮胶盘
        /// </summary>
        /// <param name="targetPos">取片位置</param>
        /// <param name="safeLevel">取片抬起安全高度</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToSlideFluxerFromUplook(AKRSPoint4D targetPos, double safeLevel)
        {
            System2RunTimeProvider.RecordTime("DipFluxActionNode", $"准备Jump到刮胶盘 X:{targetPos.X},Y:{targetPos.Y},Z:{targetPos.Z}");

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
                // 轴安全高度
                double safeHeight = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                double startPos = this.bondHeadController.GetAxisZRealPos();

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("Jump到刮胶盘T轴单独运动线程");

                            System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                            while (true)
                            {
                                if (Math.Abs(startPos - this.bondHeadController.GetAxisZRealPos()) > 1)
                                {
                                    this.bondHeadController.RotateAxisT(targetPos.T);
                                    break;
                                }

                                // 超时抛异常
                                if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                {
                                    throw new Exception("Jump到刮胶盘T轴单独运动超时！");
                                }
                            }
                        });
                }


                // T轴当前坐标
                AKRSPoint4D curPos = this.bondModuleController.Get4DRealPosition();

                // T轴旋转间距
                double rotatePitch = (targetPos.T - curPos.T) / 2;

                InterpolationParam interpolationParam = new InterpolationParam();
                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = this.uLMPara.JumpToSlideFluxerSpeed;
                interpolationParam.Acc = this.uLMPara.JumpToSlideFluxerAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToSlideFluxerJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                // 低于安全高度就先抬到安全高度
                if (this.bondHeadController.GetAxisZRealPos() < safeLevel)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[3] { new(), new(), new() };

                    // 第一段，Z上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = curPos.X, Y = curPos.Y, Z = safeLevel, T = curPos.T };

                    // XYT运动到蘸胶位置，Z不动
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = safeLevel, T = targetPos.T };

                    // Z下降到蘸胶位置
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z, T = targetPos.T };
                }
                else
                {
                    interpolationParam.SegmentConfigs =
                        new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig()};

                    // XYT运动到蘸胶位置
                    interpolationParam.SegmentConfigs[0].Point =
                        new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = safeLevel, T = targetPos.T };

                    // Z下降到蘸胶位置
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = targetPos.X, Y = targetPos.Y, Z = targetPos.Z, T = targetPos.T };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToSlideFluxerTime,
                    RadiusRatio = this.uLMPara.JumpToSlideFluxerRadiusRatio
                };

                foreach (var segmentConfig in interpolationParam.SegmentConfigs)
                {
                    // 加减速度默认*10
                    segmentConfig.Velocity = this.uLMPara.JumpToSlideFluxerSpeed;
                    segmentConfig.Acc = this.uLMPara.JumpToSlideFluxerSpeed * 10.0;
                    segmentConfig.Dec = this.uLMPara.JumpToSlideFluxerSpeed * 10.0;
                }

                // 获取卡
                AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
                    card =>
                    card.AxisList.Select(axis => axis.AxisDrive).Exists(
                        drive => drive == interpolationParam.AxisDrives[0]));

                System2RunTimeProvider.RecordTime("DipFluxActionNode", $"Jump到刮胶盘开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("DipFluxActionNode", $"Jump到刮胶盘结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"运动到刮胶盘，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"运动到取片位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        ///  蘸胶动作空跑
        /// todo:待验证
        /// </summary>
        /// <returns></returns>
        private ExcuteResult DipFluxActionDryRun()
        {
            throw new Exception("DipFluxActionNode：蘸胶空跑动作待验证！");

            // 等允许蘸胶信号
            if (!SignalPool.GetInstance().IsAllowDipFluxSignal.Wait())
            {
                return ExcuteResult.Abort;
            }

            System2RunTimeProvider.RecordTime("DipFluxActionNode", "接收到允许蘸胶信号");

            // 防呆;判断刮胶盘有没有在0位
            if (this.slideFluxerController.IsSlideFluxerAtNLimit())
            {
                // 胶盘伸出
                this.slideFluxerController.SlideFluxerOutWaitArrive();
            }

            // 蘸胶位
            AKRSPoint3D dipFluxPos =
                this.bondModuleController.ConvertG0ToMachinePos(
                    this.bondDevicePara.SlideFluxerParam.SlideFluxerPos);

            // 力控模式下不用硬补偿
            double distance = component.DipFluxForceMode == ForceModeEnum.Distance ? component.DipDistance : 0;

            // 实际蘸胶位置要加上吸嘴高度和芯片厚度
            // 空跑往上抬2mm
            dipFluxPos.Z = dipFluxPos.Z + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset
                                        + component.ComponentThickness + distance + 2;

            // 蘸胶位置
            AKRSPoint4D dipFlux4DPos = new AKRSPoint4D()
            {
                X = dipFluxPos.X,
                Y = dipFluxPos.Y,
                Z = dipFluxPos.Z,
                T = this.bondHeadController.GetAxisTRealPos()
            };

            // 预蘸胶位置
            AKRSPoint4D preDipFlux4DPos = new AKRSPoint4D()
            {
                X = dipFluxPos.X,
                Y = dipFluxPos.Y,
                Z = this.component.IsActivateSlowTravelBeforeDip
                                                          ? dipFluxPos.Z + component.SlowTravelDistanceBeforeDipFlux
                                                          : dipFluxPos.Z,
                T = this.bondHeadController.GetAxisTRealPos()
            };

            // 安全高度
            double safelevel = this.bondModuleController.ConvertG0ToMachinePos(this.bondDevicePara.SlideFluxerParam.SlideFluxerRightBottomPos).Z
                               + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 5;

            // 去预蘸胶位置
            ExcuteResult ret = this.JumpToSlideFluxer(preDipFlux4DPos, safelevel, component.DipMode);

            if (ret != ExcuteResult.Success)
            {
                return ret;
            }

            System2RunTimeProvider.RecordTime("DipFluxActionNode", "准备开始蘸胶");

            // 蘸胶
            this.bondHeadController.MoveAxisZ(dipFlux4DPos.Z);

            // 回安全高度
            this.bondHeadController.MoveAxisZ(safelevel);

            // 刮胶盘缩回
            this.slideFluxerController.SlideFluxerHomeWaitArrive();

            System2RunTimeProvider.RecordTime("DipFluxActionNode", "Action 完成");

            return ExcuteResult.Success;
        }
    }
}
