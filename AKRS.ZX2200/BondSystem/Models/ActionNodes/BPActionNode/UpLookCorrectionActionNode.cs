
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
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Services;
using ch.etel.edi.dsa.v40;
using log4net.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    /// 上视流程
    /// </summary>
    [Serializable]
    public class UpLookCorrectionActionNode : ActionNode
    {
        /// <summary>
        /// BondDomain
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 反转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 上视控制器
        /// </summary>
        private UpLookController upLookController => System2Domain.GetInstance().UpLookController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 刮胶盘控制器
        /// </summary>
        private SlideFluxerController slideFluxerController => System2Domain.GetInstance().SlideFluxerController;

        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition bondPosition => this.system2Domain.ActionNodesService.GetCurrentBondPosition();

        /// <summary>
        /// 当前所需要的芯片
        /// </summary>
        private BaseCarrierConfig component => System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().component;

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara uLMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// 下一颗芯片名称
        /// </summary>
        private string nextComponentName;

        /// <summary>
        ///  Bond模组
        /// </summary>
        private BondModule bondModule = new BondModule();

        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;

            System2RunTimeProvider.RecordTime("UpLookAction", "Start ----------------");

            try
            {
                this.nextComponentName = this.actionNodesService.GetNextComponentName();

                #region 基础条件判断

                // 判断纠偏相机类型
                if (this.component.AccuracyMode == AccuracyModeEnum.Off
                    || this.component.AdjustCamera == CameraTypeEnum.BondCamera)
                {
                    return ExcuteResult.Success;
                }

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    return ExcuteResult.Success;
                }

                #endregion

                // 空跑模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.UplookActionDryRun();
                }

                #region 去上视

                // 计算真实拍照位
                double tuDegree = this.bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI;
                Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
                double angle = this.component.UpLookAdjustConfig.PRVisionAngle + tuDegree
                                                                               + this.bondPosition.GetRotaryCompensate()
                                                                               + nozzle.AlignAngle;

                if (System2Configuration.GetInstance().UpLookQuickPositioning)
                {
                    // 两次定位时，T轴直接转到需要的角度
                    // 旋转的角度 = 吸嘴的示教角度 + 焊点的角度 + 焊点的硬补偿
                    angle = angle - tuDegree - this.bondPosition.GetRotaryCompensate();
                }

                // 拍照位和吸嘴绑定，不同的吸嘴高度不同
                AKRSPoint3D p1RealVisionPos = this.component.UpLookAdjustConfig.P1VisionPos + new AKRSPoint3D(
                                                  0,
                                                  0,
                                                  nozzle.MeasureHeightOffset);

                // 转到轴坐标
                AKRSPoint3D p1RealVisionPosInAxis = this.bondModuleController.ConvertG0ToMachinePos(p1RealVisionPos);

                AKRSPoint4D upLookPos = new AKRSPoint4D(
                    p1RealVisionPosInAxis.X,
                    p1RealVisionPosInAxis.Y,
                    p1RealVisionPosInAxis.Z,
                    angle);

                // 去上视位置
                ExcuteResult ret = this.MoveToUplookPos(this.component, upLookPos);

                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                #endregion

                #region 上视定位

                if (!this.component.IsTwoPointAdjust)
                {
                    // 一点定位
                    System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第一次定位");

                    (ExcuteResult result, MatchResult matchResult) result1 = this.UpLookVision(
                        this.component.UpLookAdjustConfig.P1VisionPos,
                        this.component.UpLookAdjustConfig.P1PRName,true);

                    if (result1.result != ExcuteResult.Success)
                    {
                        return result1.result;
                    }

                    System2RunTimeProvider.RecordTime(
                        "UpLookAction",
                        $"上视定位第一次定位完成，定位结果:X :{result1.matchResult.CenterX},Y :{result1.matchResult.CenterY},角度 :{result1.matchResult.Angle}");

                    // 保存
                    this.bondPosition.BondPositionInfo.UpLookMatchResult1 = result1.matchResult;

                    if (System2Configuration.GetInstance().UpLookQuickPositioning)
                    {
                        // 当前芯片的真实角度 = 拍照的结果角度 - 示教时的角度
                        double currentComponentAngle = result1.matchResult.Angle
                                                       - this.component.UpLookAdjustConfig
                                                           .AngleForVisionCenterAndComponentCenter;

                        // 芯片中心的实际位置 = XY的偏移量 - 芯片中心绕视觉中心旋转之后的位置
                        AKRSPoint3D point3D = this.upLookController.ConvertPixelToDistance(result1.matchResult) - MathHelper.RotateCenter(
                                                  this.component.UpLookAdjustConfig
                                                      .DistanceForVisionCenterAndComponentCenter,
                                                  new AKRSPoint3D(),
                                                  currentComponentAngle * Math.PI / 180.0 * -1.0);

                        // 当前芯片的真实位置（相对于焊头） = 当前的位置 - 上视看到旋转中心的实际位置 + 芯片实际位置
                        AKRSPoint3D currentComponentOffset =
                            this.bondModule.Get3DRealPosition()
                            - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos + point3D;

                        // 结果保存
                        this.bondPosition.BondPositionInfo.ComponentAngleOffSet = result1.matchResult.Angle;
                        this.bondPosition.BondPositionInfo.ComponentOffSet = currentComponentOffset;

                        // 这个地方是有问题
                    }
                    else
                    {
                        System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第二次定位");

                        double bpAngle = this.bondPosition.IsMirror
                                             ? 180.0 - this.bondPosition.GetRotaryCompensate()
                                             : this.bondPosition.GetRotaryCompensate();

                        // T轴转到定位角度
                        this.bondHeadController.RelativeRotateAxisT(result1.matchResult.Angle + tuDegree + bpAngle);

                        System2RunTimeProvider.RecordTime("UpLookAction", $"T轴旋转到焊点角度完成");

                        // 第二次定位
                        (ExcuteResult result, MatchResult matchResult) result2 = this.UpLookVision(
                            this.component.UpLookAdjustConfig.P1VisionPos,
                            this.component.UpLookAdjustConfig.P1PRName,true);

                        if (result2.result != ExcuteResult.Success)
                        {
                            return result2.result;
                        }

                        // 保存
                        this.bondPosition.BondPositionInfo.UpLookMatchResult2 = result2.matchResult;

                        System2RunTimeProvider.RecordTime(
                            "UpLookAction",
                            $"上视旋转角度后定位完成，定位结果:X :{result2.matchResult.CenterX},Y :{result2.matchResult.CenterY},角度 :{result2.matchResult.Angle}");

                        // 由于提前转好了角度，则不需要考虐旋转带来的偏移
                        // 芯片距离旋转中的距离为 = 视觉定位结果 + 此时的位置 - 上视的固定位置 - 视觉位置和芯片中心的差值
                        this.bondPosition.BondPositionInfo.ComponentOffSet =
                            this.upLookController.ConvertPixelToDistance(result2.matchResult)
                            + this.bondModule.Get3DRealPosition()
                            - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos
                            - this.component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter;

                        this.bondPosition.BondPositionInfo.ComponentAngleOffSet =
                            -(tuDegree + this.bondPosition.GetRotaryCompensate());
                    }
                }
                else
                {
                    // P1点定位
                    System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第一次定位");

                    (ExcuteResult result, MatchResult matchResult) result1 = this.UpLookVision(
                        this.component.UpLookAdjustConfig.P1VisionPos,
                        this.component.UpLookAdjustConfig.P1PRName, true);

                    if (result1.result != ExcuteResult.Success)
                    {
                        return result1.result;
                    }

                    AKRSPoint3D visionPos1 = this.bondModule.Get3DRealPosition();

                    System2RunTimeProvider.RecordTime(
                        "UpLookAction",
                        $"上视定位第一次定位完成，定位结果:X :{result1.matchResult.CenterX},Y :{result1.matchResult.CenterY},角度 :{result1.matchResult.Angle}");

                    System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始P2定位");

                    // 去P2拍照位
                    AKRSPoint3D p2RealVisionPos = this.component.UpLookAdjustConfig.P2VisionPos + new AKRSPoint3D(
                                                      0,
                                                      0,
                                                      nozzle.MeasureHeightOffset);

                    // 直接移动XY
                    this.bondModuleController.MoveBondXYToG0Pos(p2RealVisionPos.X, p2RealVisionPos.Y);

                    (ExcuteResult result, MatchResult matchResult) result2 = this.UpLookVision(
                        this.component.UpLookAdjustConfig.P2VisionPos,
                        this.component.UpLookAdjustConfig.P2PRName,false);

                    if (result2.result != ExcuteResult.Success)
                    {
                        return result2.result;
                    }

                    AKRSPoint3D visionPos2 = this.bondModule.Get3DRealPosition();

                    System2RunTimeProvider.RecordTime(
                        "UpLookAction",
                        $"上视定位第二次定位完成，定位结果:X :{result2.matchResult.CenterX},Y :{result2.matchResult.CenterY},角度 :{result2.matchResult.Angle}");

                    double agvAngle = (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0;

                    if (System2Configuration.GetInstance().UpLookQuickPositioning)
                    {
                        // 当前芯片的真实角度 = 拍照的结果角度 - 示教时的角度
                        double currentComponentAngle = (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0
                                                       - this.component.UpLookAdjustConfig
                                                           .AngleForVisionCenterAndComponentCenter;

                        // 芯片中心的实际位置 = 芯片中心绕视觉中心旋转之后的位置 + XY的偏移量
                        AKRSPoint3D point3D =
                            (this.upLookController.ConvertPixelToDistance(result1.matchResult)
                             + this.upLookController.ConvertPixelToDistance(result2.matchResult)) / 2.0
                            - MathHelper.RotateCenter(
                                this.component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter,
                                new AKRSPoint3D(),
                                currentComponentAngle * Math.PI / 180.0 * -1.0);

                        // 当前芯片的真实位置（相对于焊头） = 当前的位置 -上视看到旋转中心的实际位置 + 芯片实际位置
                        AKRSPoint3D currentComponentOffset =
                            (visionPos2 + visionPos1) / 2.0
                            - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos + point3D;

                        // 结果保存
                        this.bondPosition.BondPositionInfo.ComponentAngleOffSet = (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0;
                        this.bondPosition.BondPositionInfo.ComponentOffSet = currentComponentOffset;
                    }
                    else
                    {
                        double bpAngle = this.bondPosition.IsMirror
                                             ? 180.0 - this.bondPosition.GetRotaryCompensate()
                                             : this.bondPosition.GetRotaryCompensate();

                        // T轴转到定位角度
                        this.bondHeadController.RelativeRotateAxisT(
                            (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0 + tuDegree
                            + bpAngle);

                        // P1点定位
                        System2RunTimeProvider.RecordTime("UpLookAction", $"T轴旋转到焊点角度完成");

                        // 直接移动XY
                        this.bondModuleController.MoveBondXYToG0Pos(p1RealVisionPos.X, p1RealVisionPos.Y);

                        // 第二次定位
                        (ExcuteResult result, MatchResult matchResult) result3 = this.UpLookVision(
                            this.component.UpLookAdjustConfig.P1VisionPos,
                            this.component.UpLookAdjustConfig.P1PRName,true);

                        if (result3.result != ExcuteResult.Success)
                        {
                            return result3.result;
                        }

                        visionPos1 = this.bondModule.Get3DRealPosition();

                        System2RunTimeProvider.RecordTime(
                            "UpLookAction",
                            $"上视旋转角度后定位完成，定位结果:X :{result3.matchResult.CenterX},Y :{result3.matchResult.CenterY},角度 :{result3.matchResult.Angle}");

                        // 直接移动XY
                        this.bondModuleController.MoveBondXYToG0Pos(p2RealVisionPos.X, p2RealVisionPos.Y);

                        (ExcuteResult result, MatchResult matchResult) result4 = this.UpLookVision(
                            this.component.UpLookAdjustConfig.P2VisionPos,
                            this.component.UpLookAdjustConfig.P2PRName,false);

                        if (result4.result != ExcuteResult.Success)
                        {
                            return result4.result;
                        }

                        visionPos2 = this.bondModule.Get3DRealPosition();

                        System2RunTimeProvider.RecordTime(
                            "UpLookAction",
                            $"上视旋转角度后定位完成，定位结果:X :{result4.matchResult.CenterX},Y :{result4.matchResult.CenterY},角度 :{result4.matchResult.Angle}");

                        // 由于提前转好了角度，则不需要考虑旋转带来的偏移
                        // 芯片距离旋转中的距离为 = 视觉定位结果 + 此时的位置 - 上视的固定位置 - 视觉位置和芯片中心的差值
                        this.bondPosition.BondPositionInfo.ComponentOffSet =
                            (this.upLookController.ConvertPixelToDistance(result3.matchResult)
                             + this.upLookController.ConvertPixelToDistance(result4.matchResult)) / 2.0
                            + (visionPos1 + visionPos2) / 2.0
                            - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos - this.component
                                .UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter;

                        this.bondPosition.BondPositionInfo.ComponentAngleOffSet =
                            -(tuDegree + this.bondPosition.GetRotaryCompensate());
                    }
                }

                // 二维码识别
                if (this.component.IsRecognizeQRCode)
                {
                    (ExcuteResult result, CodeResult matchResult) codeResult = this.UpLookVisionByCode(this.component.UpLookAdjustConfig.CodeVisionPRPos, this.component.UpLookAdjustConfig.CodeVisionPRName, false);
                    if (codeResult.result != ExcuteResult.Success)
                    {
                        return codeResult.result;
                    }

                    this.bondPosition.BondPositionInfo.ComponentCode = codeResult.matchResult.CodeValue == null ? "0" : codeResult.matchResult.CodeValue;
                }

                #endregion

                Task.Run(
                    () =>
                    {
                        // 上视定位完关闭灯光
                        this.CloseLight();
                    });

                Task.Run(
                    () =>
                    {
                        CommonUtil.SetCurrentThreadName("给晶圆发要料信号线程");

                        // 判断是不是最后一颗芯片
                        // 如果使用翻转台，翻转的时候已经发过了
                        // todo:如果为空怎么办？
                        if ((nextComponentName != component.Name && nextComponentName != null) && component.IsUseFlipTable == false)
                        {
                            // 如果是最后一颗就在这里发要料信号
                            // 刷新Map信息
                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                            // 芯片名称传给晶圆台
                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(nextComponentName);

                            // 给晶圆台发要料信号
                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                            // 自动重取次数清零
                            System2RunTimeProvider.UpLookAutoSkipCount = 0;

                            // 跳过次数清零
                            System2RunTimeProvider.StaticWaffleComponentAutoSkipCount = 0;

                            Static.RecordTime("取片信号交互", $"上视定位完成，给晶圆发要料信号，芯片名称{component.Name}");
                        }
                    });

                // 温飘补偿,贴片前去看一下温飘Mark片
                if (!this.system2Controller.System2TemperatureCompensation(false))
                {
                    return ExcuteResult.Abort;
                }

                // 上视温漂补偿
                AKRSPoint3D driftCompensation = TpMarkCompensate.GetTemperatureCompensation();

                // 记录补偿值
                this.bondPosition.BondPositionInfo.TpMarkCompensate = driftCompensation;

                // 添加偏移量
                this.bondPosition.BondPositionInfo.ComponentOffSet += driftCompensation;

                System2RunTimeProvider.RecordTime("UplookAction", "Action 完成");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障", ex, LogCategory.Bond);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
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
            BondPosition bp = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

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
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Yes },
                    AlarmLevel.SecondLevel);

                return false;
            }

            if (component.UpLookAdjustConfig == null)
            {
                return false;
            }

            // 拍照位空指针判断
            if (component.IsTwoPointAdjust)
            {
                if (component.UpLookAdjustConfig.P1VisionPos == null
                    || component.UpLookAdjustConfig.P2VisionPos == null)
                {
                    AKRSMessageBoxExt.Show(
                        $"上视两点定位模式下未找到拍照位！设备将退出自动工作！",
                        "报警",
                        new string[] { "确认" },
                        new DialogResult[] { DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    return false;
                }
            }
            else
            {
                if (component.UpLookAdjustConfig.P1VisionPos == null)
                {
                    AKRSMessageBoxExt.Show(
                        $"上视一点定位模式下未找到拍照位！设备将退出自动工作！",
                        "报警",
                        new string[] { "确认" },
                        new DialogResult[] { DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    return false;
                }
            }

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
        /// 去上视位置
        /// </summary>
        /// <param name="componet">芯片</param>
        /// <param name="upLookPos">上视位置</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToUplookPos(BaseCarrierConfig componet, AKRSPoint4D upLookPos)
        {
            System2RunTimeProvider.RecordTime("UplookAction", $"准备运动到上视拍照位 X:{upLookPos.X},Y:{upLookPos.Y},Z:{upLookPos.Z}");

            ExcuteResult ret = ExcuteResult.Success;

            switch (MachineSoftwareConfiguration.GetInstance().AxisMoveMode)
            {
                case AxisMoveModeEnum.AbsoluteMove:
                    // this.bondModuleController.MoveSafeBondXYZT(upLookPos);

                    // IPT位置转到Bond
                    AKRSPoint3D iPTPosInBond = this.bondModuleController.ConvertG0ToMachinePos(BondDevicePara.GetInstance().IPTDevicePara.IPTPos);
                    double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

                    // 5mm是余量
                    double iPTsafeLevel = iPTPosInBond.Z + nozzleHeightOffset + component.ComponentThickness + 5;

                    double safeLevel = this.component.CarrierType == CarrierTypeEnum.StaticWaffle
                                           ? this.bondModuleController.GetBondheadSafeLevel(upLookPos)
                                           : iPTsafeLevel;

                    if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
                    {
                        if (this.component.IsUseFlipTable)
                        {
                            switch (component.DipMode)
                            {
                                case DipModeEnum.BeforeAccuracyMode:
                                    safeLevel =
                                        this.bondModuleController
                                            .ConvertG0ToMachinePos(BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos).Z
                                        + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 3;
                                    break;

                                case DipModeEnum.Off:
                                    safeLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                                    break;

                                case DipModeEnum.AfterAccuracyMode:
                                    safeLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                                    break;
                            }
                        }
                        else
                        {
                            safeLevel = this.component.CarrierType == CarrierTypeEnum.StaticWaffle
                                            ? this.bondModuleController.GetBondheadSafeLevel(upLookPos)
                                            : BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;
                        }
                    }

                    ret = this.AbsMoveToUplook(upLookPos, safeLevel);
                    if (ret != ExcuteResult.Success)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"运动到上视位失败！\r\n",
                            "报警",
                            new string[] { "确认" },
                            new DialogResult[] { DialogResult.OK },
                            AlarmLevel.FirstLevel);

                        return ret;
                    }

                    break;

                case AxisMoveModeEnum.InterpolationMove:

                    if (this.component.CarrierType == CarrierTypeEnum.StaticWaffle)
                    {
                        ret = this.JumpToUplookFromStaticWaffle(upLookPos);
                    }
                    else
                    {
                        if (this.component.IsUseFlipTable)
                        {
                            if (component.DipMode == DipModeEnum.BeforeAccuracyMode)
                            {
                                double safeHeight = 0;

                                // 两点定位默认是大芯片，用另一种时序
                                if (this.component.IsTwoPointAdjust)
                                {
                                    #region 先去安全高度让刮胶盘缩回

                                    // 蘸胶安全高度
                                    safeHeight =
                                       this.bondModuleController
                                           .ConvertG0ToMachinePos(BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos).Z
                                       + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 5;

                                    this.bondHeadController.MoveAxisZ(safeHeight);

                                    if (component.DipMode != DipModeEnum.Off)
                                    {
                                        // 给刮胶盘发蘸胶完成信号
                                        SignalPool.GetInstance().BondDipFluxFinishSignal.Set();
                                    }

                                    //this.bondModuleController.MoveBondXY(upLookPos.X, upLookPos.Y);

                                    Stopwatch sp = Stopwatch.StartNew();

                                    // 等刮胶盘缩回
                                    while (this.slideFluxerController.IsSlideFluxerAtNLimit() == false)
                                    {
                                        if (sp.Elapsed.TotalMilliseconds > 5000)
                                        {
                                            throw new Exception("刮胶盘去上视：刮胶盘缩回超时！");
                                        }

                                        Thread.Sleep(5);
                                    }

                                    //this.bondHeadController.MoveZAxis(upLookPos.Z);

                                    #endregion
                                }
                                else
                                {
                                    safeHeight =
                                        this.bondModuleController
                                            .ConvertG0ToMachinePos(BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos).Z
                                        + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 3;
                                }


                                // 从刮胶盘去上视
                                ret = this.JumpToUplookFromSlideFluxer(upLookPos, safeHeight);
                            }
                            else
                            {
                                ret = this.JumpToUplookFromFlipTable(upLookPos);
                            }
                        }
                        else
                        {
                            ret = this.JumpToUplookFromWaferTable(upLookPos);
                        }
                    }

                    break;
            }

            System2RunTimeProvider.RecordTime("UplookAction", $"运动到上视拍照位 X:{upLookPos.X},Y:{upLookPos.Y},Z:{upLookPos.Z}结束");
            return ret;
        }

        /// <summary>
        /// 从翻转台去上视位置 ULM运动
        /// </summary>
        /// <param name="upLookPos">上视位置</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToUplookFromFlipTable(AKRSPoint4D upLookPos)
        {
            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated == false)
            {
                throw new Exception("配置异常：翻转台未配置,从翻转台运动到上视失败！");
            }

            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            if (this.bondModuleController.CheckSoftLimit(upLookPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                double safeHeight = this.bondHeadController.GetAxisZRealPos() + 1;

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("翻转台Jump到上视拍照位T轴单独运动线程");

                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    double curPos = this.bondHeadController.GetAxisZRealPos();
                                    if (Math.Abs(curPos - safeHeight) < 0.1 || curPos >= safeHeight)
                                    {
                                        this.bondHeadController.RotateAxisT(upLookPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        throw new Exception("翻转台Jump到上视拍照位T轴单独运动超时！");
                                    }
                                }
                            });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // 第一个点 如果低于安全高度，上抬到TU安全高度
                double tuSafeHeight = this.system2Controller.GetTUMachineSafeHeight();

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToUplookSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToUplookPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToUplookPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                if (this.bondHeadController.GetAxisZRealPos() < tuSafeHeight)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[2] { new SegmentConfig(), new SegmentConfig() };

                    // 先上抬到安全高度
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = tuSafeHeight, T = this.bondHeadController.GetAxisTRealPos() };
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };
                }
                else
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[1];
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };
                }

                foreach (var segmentConfig in interpolationParam.SegmentConfigs)
                {
                    // 加减速度默认*10
                    segmentConfig.Velocity = vel;
                    segmentConfig.Acc = vel * 10.0;
                    segmentConfig.Dec = vel * 10.0;
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToUplookPosTime,
                    RadiusRatio = this.uLMPara.JumpToUplookPosRadiusRatio
                };

                // 获取卡
                AxisCard card = HardwareRepositoryService.GetHardwaresByType<AxisCard>().Find(
                    card =>
                    card.AxisList.Select(axis => axis.AxisDrive).Exists(
                        drive => drive == interpolationParam.AxisDrives[0]));

                System2RunTimeProvider.RecordTime("UplookAction", $"翻转台Jump到上视拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("UplookAction", $"翻转台Jump到上视拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"翻转台Jump到上视拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"翻转台Jump到上视拍照位，UML运动失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 从晶圆台去上视位置 ULM运动
        /// todo:需要兼容FC安全高度
        /// </summary>
        /// <param name="upLookPos">上视位置</param>
        /// <returns>结果</returns>
        public ExcuteResult JumpToUplookFromWaferTable(AKRSPoint4D upLookPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            if (this.bondModuleController.CheckSoftLimit(upLookPos) == false)
            {
                return ExcuteResult.Exception;
            }

            // 吸嘴高度补偿
            double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

            // 当前位置高于环光或者目标位置低于中转台直接抛异常
            if (this.bondHeadController.GetAxisZRealPos()
                > (this.uLMPara.AboveWaferRingLightPos.Z + nozzleHeightOffset))
            {
                Machine.GetInstance().Stop();
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"致命异常:ULM运动到上视位置前Z轴低于安全高度! 设备将退出自动工作!\r\n",
                    "异常",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                LogHelper.Post(Level.Info, $"上视流程:运动到上视拍照位位运行故障！", LogCategory.Bond);
                return ExcuteResult.Exception;
            }

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated && this.flipTableController.IsFlipTableAtHome() == false)
            {
                Machine.GetInstance().Stop();
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"翻转台不在0位，有撞机风险!即将退出自动工作！\r\n",
                    "异常",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                LogHelper.Post(Level.Info, $"上视流程:运动到上视拍照位位运行故障！", LogCategory.Bond);
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                // 晶圆环光上面
                double aboveRingLightHeight = this.uLMPara.AboveWaferRingLightPos.Z + nozzleHeightOffset;

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("晶圆台Jump到上视拍照位T轴单独运动线程");

                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    if (this.bondHeadController.GetAxisZRealPos() >= aboveRingLightHeight - 5)
                                    {
                                        this.bondHeadController.RotateAxisT(upLookPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        throw new Exception("晶圆台Jump到上视拍照位T轴单独运动超时！");
                                    }
                                }
                            });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // T轴旋转间距
                double rotatePitch = (upLookPos.T - curTPos) / 2;

                // 中转台右上位置
                AKRSPoint3D iPtRightPos = new AKRSPoint3D()
                {
                    X = this.uLMPara.AboveIPTRightPos.X,
                    Z = this.uLMPara.AboveIPTRightPos.Z + nozzleHeightOffset,
                };

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToUplookSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToUplookPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToUplookPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                if (upLookPos.Z < iPtRightPos.Z)
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[4] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                    // 第一个点 应该抬起到晶圆环光上面，T轴不转
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = aboveRingLightHeight, T = this.bondHeadController.GetAxisTRealPos() };

                    // 第二段，运动到中转台右上方 X: -40  Y:  upLookPos.Y
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = iPtRightPos.X, Y = upLookPos.Y, Z = iPtRightPos.Z, T = curTPos + rotatePitch };

                    // 第三个点 运动到上视相机正上方
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = iPtRightPos.Z, T = upLookPos.T };

                    // 第4段,Z运动到上视位置高度
                    interpolationParam.SegmentConfigs[3].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };
                }
                else
                {
                    interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                    // 第一个点 应该抬起到晶圆环光上面，T轴不转
                    interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = aboveRingLightHeight, T = this.bondHeadController.GetAxisTRealPos() };

                    // 第二段，运动到中转台右上方 X: -40  Y:  upLookPos.Y 
                    interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = iPtRightPos.X, Y = upLookPos.Y, Z = iPtRightPos.Z, T = curTPos + rotatePitch };

                    // 第3段,Z运动到上视位置高度
                    interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };
                }

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToUplookPosTime,
                    RadiusRatio = this.uLMPara.JumpToUplookPosRadiusRatio
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

                System2RunTimeProvider.RecordTime("UplookAction", $"晶圆台运动到上视拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("UplookAction", $"晶圆台运动到上视拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"晶圆台运动到上视拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"晶圆台运动到上视拍照位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 从静态华夫盒去上视位置 ULM运动
        /// </summary>
        /// <param name="upLookPos">上视位置</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToUplookFromStaticWaffle(AKRSPoint4D upLookPos)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            if (this.bondModuleController.CheckSoftLimit(upLookPos) == false)
            {
                return ExcuteResult.Exception;
            }


            Task axisTMoveTask = default;

            try
            {
                // 轴安全高度
                double safeHeight = this.bondModuleController.GetBondheadSafeLevel(upLookPos);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("静态华夫盒Jump到上视拍照位T轴单独运动线程");

                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    if (this.bondHeadController.GetAxisZRealPos() >= safeHeight)
                                    {
                                        this.bondHeadController.RotateAxisT(upLookPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        throw new Exception("静态华夫盒Jump到上视拍照位T轴单独运动超时！");
                                    }
                                }
                            });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // 吸嘴高度补偿
                double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

                InterpolationParam interpolationParam = new InterpolationParam();

                // 全局速度百分比
                double vel = this.uLMPara.JumpToUplookSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToUplookPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToUplookPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                // 第一段，Z上抬到安全高度
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = safeHeight, T = this.bondHeadController.GetAxisTRealPos() };

                // 第二段,XYT运动到上视位置  
                interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = safeHeight, T = upLookPos.T };

                // 第3段,Z运动到上视位置高度
                interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToUplookPosTime,
                    RadiusRatio = this.uLMPara.JumpToUplookPosRadiusRatio
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


                System2RunTimeProvider.RecordTime("UpLookAction", $"静态华夫盒Jump到上视拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("UpLookAction", $"静态华夫盒Jump到上视拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"静态华夫盒运动到上视拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"静态华夫盒运动到上视拍照位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 从刮胶盘去上视位置 ULM运动
        /// </summary>
        /// <param name="upLookPos">上视位置</param>
        /// <returns>结果</returns>
        private ExcuteResult JumpToUplookFromSlideFluxer(AKRSPoint4D upLookPos, double safeHeight)
        {
            // 单步工作
            if (!System2Domain.GetInstance().WaitSingleStep())
            {
                return ExcuteResult.Abort;
            }

            if (this.bondModuleController.CheckSoftLimit(upLookPos) == false)
            {
                return ExcuteResult.Exception;
            }

            Task axisTMoveTask = default;

            try
            {
                // 起始点
                double startPos = this.bondHeadController.GetAxisZRealPos();

                //// 蘸胶安全高度
                //double safeHeight =
                //    this.bondModuleController
                //        .ConvertG0ToMachinePos(BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos).Z
                //    + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset + 5;

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("从刮胶盘Jump到上视拍照位T轴单独运动线程");

                                System2RunTimeProvider.AxisTMoveTaskStopwatch.Restart();
                                while (true)
                                {
                                    double curPos = this.bondHeadController.GetAxisZRealPos();
                                    if (Math.Abs(curPos - startPos) >= 1)
                                    {
                                        this.bondHeadController.RotateAxisT(upLookPos.T);
                                        break;
                                    }

                                    // 超时抛异常
                                    if (System2RunTimeProvider.AxisTMoveTaskStopwatch.ElapsedMilliseconds > 5000)
                                    {
                                        throw new Exception("从刮胶盘Jump到上视拍照位T轴单独运动超时！");
                                    }
                                }
                            });
                }

                // T轴当前坐标
                double curTPos = this.bondHeadController.GetAxisTRealPos();

                // 吸嘴高度补偿
                double nozzleHeightOffset = this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset;

                // 全局速度百分比
                double vel = this.uLMPara.JumpToUplookSpeed
                             * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                InterpolationParam interpolationParam = new InterpolationParam();
                interpolationParam.ListNo = 2;
                interpolationParam.GrpCrd = 2;
                interpolationParam.Vel = vel;
                interpolationParam.Acc = this.uLMPara.JumpToUplookPosAccelerationTime;
                interpolationParam.AccAcc = this.uLMPara.JumpToUplookPosJerkTime;

                interpolationParam.AxisDrives = this.bondModuleController.GetBondIpolAxis();

                interpolationParam.SegmentConfigs = new SegmentConfig[3] { new SegmentConfig(), new SegmentConfig(), new SegmentConfig() };

                // 第一段，Z上抬到安全高度
                interpolationParam.SegmentConfigs[0].Point = new AKRSPoint4D() { X = this.bondModuleController.GetAxisXRealPos(), Y = this.bondModuleController.GetAxisYRealPos(), Z = safeHeight, T = this.bondHeadController.GetAxisTRealPos() };

                // 第二段,XYT运动到上视位置  
                interpolationParam.SegmentConfigs[1].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = safeHeight, T = upLookPos.T };

                // 第3段,Z运动到上视位置高度
                interpolationParam.SegmentConfigs[2].Point = new AKRSPoint4D() { X = upLookPos.X, Y = upLookPos.Y, Z = upLookPos.Z, T = upLookPos.T };

                interpolationParam.AheadParam = new AheadParam()
                {
                    Time = this.uLMPara.JumpToUplookPosTime,
                    RadiusRatio = this.uLMPara.JumpToUplookPosRadiusRatio
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


                System2RunTimeProvider.RecordTime("UpLookAction", $"从刮胶盘Jump到上视拍照位开始");

                card.MotionController.ContinueInterpolationMove(interpolationParam);

                if (MachineHardwareConfiguration.GetInstance().IsBondAxisTSingleMove)
                {
                    axisTMoveTask.Wait();
                }

                System2RunTimeProvider.RecordTime("UpLookAction", $"从刮胶盘Jump到上视拍照位结束");

                return ExcuteResult.Success;
            }
            catch (DsaException exc)
            {
                LogHelper.Post(Level.Error, $"从刮胶盘运动到上视拍照位，UML运动失败！", exc, LogCategory.Bond);
                throw;
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Error, $"从刮胶盘运动到上视拍照位失败！", e, LogCategory.Bond);
                throw;
            }
        }

        /// <summary>
        /// 关闭上视灯光
        /// </summary>
        private void CloseLight()
        {
            // 寻找Pr模板
            PREntity pREntity1 = (PREntity)VisionEntityRepository.GetInstance().Find(this.component.UpLookAdjustConfig.P1PRName);
            PREntity pREntity2 = (PREntity)VisionEntityRepository.GetInstance().Find(this.component.UpLookAdjustConfig.P2PRName);

            // 两点定位就以P2的为准
            bool isFlash = this.component.IsTwoPointAdjust ? pREntity2.IsFlash : pREntity1.IsFlash;

            if (isFlash)
            {
                // 关闭灯光
                this.upLookController.CloseLight();
            }
        }

        /// <summary>
        /// 上视拍照
        /// </summary>
        /// <param name="visionPoint3D">点位</param>
        /// <param name="name">定位的名称</param>
        /// <returns>定位结果</returns>
        public (ExcuteResult, MatchResult) UpLookVision(AKRSPoint3D visionPoint3D, string name,bool isFirstPosVision)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);

            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $" {this.Name}定位流程中发现模板： {name} 不存在,将退出自动工作！\r\n",
                    "异常",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                return (ExcuteResult.Exception, null);
            }

            System2RunTimeProvider.RecordTime("UpLookAction", $"开始设置上视相机硬件");

            // 设置硬件
            this.system2Controller.SetHardware(name, CameraTypeEnum.UpLookCamera);

            System2RunTimeProvider.RecordTime("UpLookAction", $"设置上视相机硬件完成");

            // 拍照停留
            DelayHelper.Delay(this.component.AdjustVisionDelay);

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视拍照位延时{this.component.AdjustVisionDelay}ms");

            // 开始定位(默认不频闪)
            ExcuteResult p1Result = pREntity.DoWork(null, false);

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视拍照完成");

            // 处理拍照完成后的结果
            if (p1Result != ExcuteResult.Success || pREntity.AlgResult.IsSuccess == false)
            {
                // 自动抛料重取
                if (component.UpLookAutoSkipTimes != 0
                    && System2RunTimeProvider.UpLookAutoSkipCount < component.UpLookAutoSkipTimes)
                {
                    System2RunTimeProvider.RecordTime("取片信号交互", $"上视定位失败，自动抛料重取！");

                    this.nextComponentName = this.actionNodesService.GetNextComponentName();

                    // 判断是不是最后一颗芯片
                    // 如果使用翻转台，翻转的时候已经发过了
                    if ((nextComponentName == null || nextComponentName != component.Name) /*&& component.IsUseFlipTable == false*/)
                    {
                        // 2025年12月30日 新增
                        if (component.IsUseFlipTable && this.component.CarrierType == CarrierTypeEnum.Wafer)
                        {
                            // 顶针缩回
                            WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                        }

                        // 如果是最后一颗就在这里发要料信号
                        // 刷新Map信息
                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                        // 当前种类芯片名称传给晶圆台
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                        // 给晶圆台发要料信号
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                        System2RunTimeProvider.RecordTime("取片信号交互", $"给晶圆发要料信号，芯片名称{component.Name}");
                    }

                    // 重新设置灯光
                    pREntity.SetLight();

                    this.bondModuleController.ThrowAction();

                    // 抛掉的芯片+1
                    this.component.AddCountOfReject();

                    // 重置前面的动作节点,重新取贴
                    List<ActionNode> actionNodeList;

                    if (component.DipMode == DipModeEnum.BeforeAccuracyMode)
                    {
                        actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.DipFluxActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                    }
                    else
                    {
                        actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                    }

                    this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                    System2RunTimeProvider.IsRepickComponent = true;

                    Thread.Sleep(20);

                    // 自动重取次数+1
                    System2RunTimeProvider.UpLookAutoSkipCount++;

                    return (ExcuteResult.Retry, null);
                }

                // 自动重取次数清零
                System2RunTimeProvider.UpLookAutoSkipCount = 0;

                (DialogResult dialogResult, BaseAlgResult match) result = UcMainSystem.VisionAlarmFunc(
                    pREntity,
                    name,
                    $"上视定位失败");

                switch (result.dialogResult)
                {
                    case DialogResult.Abort:

                        // 退出
                        return (ExcuteResult.Abort, null);

                    // 重取，这里是取下一颗不是当前颗
                    case DialogResult.Retry:

                        System2RunTimeProvider.RecordTime("取片信号交互", $"上视定位失败，点击重取！");

                        // 判断是不是最后一颗芯片
                        // 如果使用翻转台，翻转的时候已经发过了
                        if ((nextComponentName == null
                             || nextComponentName != component.Name) /*&& component.IsUseFlipTable == false*/)
                        {
                            // 2025年12月30日 新增
                            if (component.IsUseFlipTable && this.component.CarrierType == CarrierTypeEnum.Wafer)
                            {
                                // 顶针缩回
                                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                            }

                            // 如果是最后一颗就在这里发要料信号
                            // 刷新Map信息
                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                            // 当前种类芯片名称传给晶圆台
                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                            // 给晶圆台发要料信号
                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                            System2RunTimeProvider.RecordTime("取片信号交互", $"给晶圆发要料信号，芯片名称{component.Name}");
                        }

                        // 重新设置灯光
                        pREntity.SetLight();

                        System2RunTimeProvider.RecordTime("上视流程", $"重新设置灯光完成");

                        this.bondModuleController.ThrowAction();

                        System2RunTimeProvider.RecordTime("上视流程", $"抛料完成");

                        // 抛掉的芯片+1
                        this.component.AddCountOfReject();

                        System2RunTimeProvider.RecordTime("上视流程", $"抛掉的芯片+1");

                        if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured && component.DipMode == DipModeEnum.BeforeAccuracyMode)
                        {
                            // 大芯片和小芯片时序不同
                            if (component.IsTwoPointAdjust)
                            {
                                // 给刮胶盘发允许伸出信号
                                SignalPool.GetInstance().BondDipFluxFinishSignal.Set();
                            }
                            else
                            {
                                // 给刮胶盘发蘸胶完成信号
                                SignalPool.GetInstance().BondDipFluxFinishSignal.Set();

                                // 给刮胶盘发允许伸出信号
                                SignalPool.GetInstance().AllowSlideOutSignal.Set();
                            }
                        }

                        // 重置前面的动作节点,重新取贴
                        List<ActionNode> actionNodeList;

                        if (component.DipMode == DipModeEnum.BeforeAccuracyMode)
                        {
                            actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.DipFluxActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                        }
                        else
                        {
                            actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                        }

                        this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                        System2RunTimeProvider.IsRepickComponent = true;

                        Thread.Sleep(20);

                        return (ExcuteResult.Retry, null);

                    case DialogResult.OK:
                        return (ExcuteResult.Success, (MatchResult)result.match);
                }
            }

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位完成");

            //VisionService.SaveVisionImage(
            //    pREntity.AlgResult.OutPutImg1,
            //   new List<string>() { "Bond", TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit?.Name, pREntity.GetName() },
            //   System2Domain.GetInstance().ActionNodesService.CurrentMatter(),
            //   pREntity.OriginalBmp);

            LocateUseConfig locateUseConfig = isFirstPosVision ? this.component.UpLookAdjustConfig.LocateUseConfig1 : this.component.UpLookAdjustConfig.LocateUseConfig2;

            MatchResult matchResult = System2RunTimeProvider.GetMatchResultByConfig((MatchResult)pREntity.AlgResult, locateUseConfig);

            return (ExcuteResult.Success, matchResult);
        }

        /// <summary>
        /// 上视拍照
        /// </summary>
        /// <param name="visionPoint3D">点位</param>
        /// <param name="name">定位的名称</param>
        /// <returns>定位结果</returns>
        public (ExcuteResult, CodeResult) UpLookVisionByCode(AKRSPoint3D visionPoint3D, string name, bool isFirstPosVision)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);

            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $" {this.Name}定位流程中发现模板： {name} 不存在,将退出自动工作！\r\n",
                    "异常",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                return (ExcuteResult.Exception, null);
            }

            System2RunTimeProvider.RecordTime("UpLookAction", $"开始设置上视相机硬件");

            // 设置硬件
            this.system2Controller.SetHardware(name, CameraTypeEnum.UpLookCamera);

            System2RunTimeProvider.RecordTime("UpLookAction", $"设置上视相机硬件完成");

            // 拍照停留
            DelayHelper.Delay(this.component.AdjustVisionDelay);

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视拍照位延时{this.component.AdjustVisionDelay}ms");

            // 开始定位(默认不频闪)
            ExcuteResult p1Result = pREntity.DoWork(null, false);

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视拍照完成");

            // 处理拍照完成后的结果
            if (p1Result != ExcuteResult.Success || pREntity.AlgResult.IsSuccess == false)
            {
                // 自动抛料重取
                if (component.UpLookAutoSkipTimes != 0
                    && System2RunTimeProvider.UpLookAutoSkipCount < component.UpLookAutoSkipTimes)
                {
                    System2RunTimeProvider.RecordTime("取片信号交互", $"上视定位失败，自动抛料重取！");

                    this.nextComponentName = this.actionNodesService.GetNextComponentName();

                    // 判断是不是最后一颗芯片
                    // 如果使用翻转台，翻转的时候已经发过了
                    if ((nextComponentName == null || nextComponentName != component.Name) /*&& component.IsUseFlipTable == false*/)
                    {
                        // 2025年12月30日 新增
                        if (component.IsUseFlipTable && this.component.CarrierType == CarrierTypeEnum.Wafer)
                        {
                            // 顶针缩回
                            WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                        }

                        // 如果是最后一颗就在这里发要料信号
                        // 刷新Map信息
                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                        // 当前种类芯片名称传给晶圆台
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                        // 给晶圆台发要料信号
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                        System2RunTimeProvider.RecordTime("取片信号交互", $"给晶圆发要料信号，芯片名称{component.Name}");
                    }

                    // 重新设置灯光
                    pREntity.SetLight();

                    this.bondModuleController.ThrowAction();

                    // 抛掉的芯片+1
                    this.component.AddCountOfReject();

                    // 重置前面的动作节点,重新取贴
                    List<ActionNode> actionNodeList;

                    if (component.DipMode == DipModeEnum.BeforeAccuracyMode)
                    {
                        actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.DipFluxActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                    }
                    else
                    {
                        actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                    }

                    this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                    System2RunTimeProvider.IsRepickComponent = true;

                    Thread.Sleep(20);

                    // 自动重取次数+1
                    System2RunTimeProvider.UpLookAutoSkipCount++;

                    return (ExcuteResult.Retry, null);
                }

                // 自动重取次数清零
                System2RunTimeProvider.UpLookAutoSkipCount = 0;

                (DialogResult dialogResult, BaseAlgResult match) result = UcMainSystem.VisionAlarmFunc(
                    pREntity,
                    name,
                    $"上视定位失败");

                switch (result.dialogResult)
                {
                    case DialogResult.Abort:

                        // 退出
                        return (ExcuteResult.Abort, null);

                    // 重取，这里是取下一颗不是当前颗
                    case DialogResult.Retry:

                        System2RunTimeProvider.RecordTime("取片信号交互", $"上视定位失败，点击重取！");

                        // 判断是不是最后一颗芯片
                        // 如果使用翻转台，翻转的时候已经发过了
                        if ((nextComponentName == null
                             || nextComponentName != component.Name) /*&& component.IsUseFlipTable == false*/)
                        {
                            // 2025年12月30日 新增
                            if (component.IsUseFlipTable && this.component.CarrierType == CarrierTypeEnum.Wafer)
                            {
                                // 顶针缩回
                                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                            }

                            // 如果是最后一颗就在这里发要料信号
                            // 刷新Map信息
                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                            // 当前种类芯片名称传给晶圆台
                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                            // 给晶圆台发要料信号
                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                            System2RunTimeProvider.RecordTime("取片信号交互", $"给晶圆发要料信号，芯片名称{component.Name}");
                        }

                        // 重新设置灯光
                        pREntity.SetLight();

                        System2RunTimeProvider.RecordTime("上视流程", $"重新设置灯光完成");

                        this.bondModuleController.ThrowAction();

                        System2RunTimeProvider.RecordTime("上视流程", $"抛料完成");

                        // 抛掉的芯片+1
                        this.component.AddCountOfReject();

                        System2RunTimeProvider.RecordTime("上视流程", $"抛掉的芯片+1");

                        if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured && component.DipMode == DipModeEnum.BeforeAccuracyMode)
                        {
                            // 大芯片和小芯片时序不同
                            if (component.IsTwoPointAdjust)
                            {
                                // 给刮胶盘发允许伸出信号
                                SignalPool.GetInstance().BondDipFluxFinishSignal.Set();
                            }
                            else
                            {
                                // 给刮胶盘发蘸胶完成信号
                                SignalPool.GetInstance().BondDipFluxFinishSignal.Set();

                                // 给刮胶盘发允许伸出信号
                                SignalPool.GetInstance().AllowSlideOutSignal.Set();
                            }
                        }

                        // 重置前面的动作节点,重新取贴
                        List<ActionNode> actionNodeList;

                        if (component.DipMode == DipModeEnum.BeforeAccuracyMode)
                        {
                            actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.DipFluxActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                        }
                        else
                        {
                            actionNodeList = new List<ActionNode>
                                                          {
                                                              this.system2Domain.BondActionNodeRepository.PickActionNode,
                                                              this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode
                                                          };
                        }

                        this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                        System2RunTimeProvider.IsRepickComponent = true;

                        Thread.Sleep(20);

                        return (ExcuteResult.Retry, null);

                    case DialogResult.OK:
                        return (ExcuteResult.Success, (CodeResult)result.match);
                }
            }

            System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位完成");

            LocateUseConfig locateUseConfig = isFirstPosVision ? this.component.UpLookAdjustConfig.LocateUseConfig1 : this.component.UpLookAdjustConfig.LocateUseConfig2;

            return (ExcuteResult.Success, (CodeResult)pREntity.AlgResult);
        }

        /// <summary>
        /// 通过判断Z轴位置安全移动XYZT轴到指定位置（去上视位会用到）
        /// </summary>
        /// <param name="point4D">位置</param>
        /// <param name="safeZ">Z轴安全位置</param>
        /// <returns>结果</returns>
        private ExcuteResult AbsMoveToUplook(AKRSPoint4D point4D, double safeZ)
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

            System2RunTimeProvider.RecordTime("UpLookAction", $"绝对运动到上视-curZPos：{curZPos}");

            Stopwatch sp = new Stopwatch();

            if (point4D.Z > safeZ)
            {
                if (curZPos < point4D.Z)
                {
                    // Z轴直接去目标高度
                    this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);

                    System2RunTimeProvider.RecordTime("UpLookAction", $"Z轴直接去目标高度");
                }

                sp.Start();
                while (true)
                {
                    double curLevel = this.bondModule.BondHead.AxisZ.GetRealPosition();
                    if (Math.Abs(curLevel - safeZ) < 0.01 || curLevel > safeZ)
                    {
                        // 开始运动
                        this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                        this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                        this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);

                        System2RunTimeProvider.RecordTime("UpLookAction", $"开始运动XYT");
                        break;
                    }

                    Thread.Sleep(1);

                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        return ExcuteResult.Exception;
                    }
                }
            }
            else
            {
                if (curZPos < safeZ)
                {
                    // Z轴先去安全高度
                    this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(safeZ, velZ);

                    System2RunTimeProvider.RecordTime("UpLookAction", $"Z轴先去安全高度{safeZ}");
                }

                sp.Start();
                while (true)
                {
                    curZPos = this.bondModule.BondHead.AxisZ.GetRealPosition();
                    if (curZPos >= safeZ || Math.Abs(curZPos - safeZ) < 0.1)
                    {
                        // 开始运动
                        this.bondModule.BondAxisX.SendAbsoluteMoveCommand(point4D.X, velX);
                        this.bondModule.BondAxisY.SendAbsoluteMoveCommand(point4D.Y, velY);
                        this.bondModule.BondHead.AxisT.SendAbsoluteMoveCommand(point4D.T, velT);

                        System2RunTimeProvider.RecordTime("UpLookAction", $"开始运动XYT");
                        break;
                    }

                    Thread.Sleep(1);

                    if (sp.Elapsed.TotalSeconds > 10)
                    {
                        return ExcuteResult.Exception;
                    }
                }

                sp.Restart();
                while (true)
                {
                    if (Math.Abs(this.bondModule.BondAxisX.GetRealPosition() - point4D.X) < 5
                        && Math.Abs(this.bondModule.BondAxisY.GetRealPosition() - point4D.Y) < 5)
                    {
                        // XY轴安全开始移动Z轴
                        this.bondModule.BondHead.AxisZ.SendAbsoluteMoveCommand(point4D.Z, velZ);

                        System2RunTimeProvider.RecordTime("UpLookAction", $"XY轴安全开始移动Z轴");
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


            // 等轴到位
            MotionService.WaitAxesArrival(
                (this.bondModule.BondAxisX, true, point4D.X, AccuracyMode.HighAccuracy),
                (this.bondModule.BondAxisY, true, point4D.Y, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisZ, true, point4D.Z, AccuracyMode.HighAccuracy),
                (this.bondModule.BondHead.AxisT, true, point4D.T, AccuracyMode.HighAccuracy));

            System2RunTimeProvider.RecordTime("UpLookAction", $"绝对运动到上视结束");

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 上视动作空跑
        /// </summary>
        /// <returns>结果</returns>
        private ExcuteResult UplookActionDryRun()
        {
            // 获取当前焊点
            BondPosition bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 若到这里没有退出，直接至少为一点定位
            AKRSPoint3D p1VisionPos = this.component.UpLookAdjustConfig.P1VisionPos + new AKRSPoint3D(
                                          0,
                                          0,
                                          this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset);

            // 轴移动到拍照位
            this.bondModuleController.MoveToG0Pos(p1VisionPos);

            Thread.Sleep(component.AdjustVisionDelay);

            // 判断是否两点定位
            if (this.component.IsTwoPointAdjust)
            {
                // 两点定位
                AKRSPoint3D p2VisionPos =
                    this.component.UpLookAdjustConfig.P2VisionPos + new AKRSPoint3D(
                        0,
                        0,
                        this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset);

                // 直接移动XY
                this.bondModuleController.MoveBondXYToG0Pos(p2VisionPos.X, p2VisionPos.Y);

                Thread.Sleep(component.AdjustVisionDelay);
            }

            Task.Run(
                  () =>
                  {
                      CommonUtil.SetCurrentThreadName("给晶圆发要料信号线程");

                      // 判断是不是最后一颗芯片
                      // 如果使用翻转台，翻转的时候已经发过了
                      // todo:如果为空怎么办？
                      if ((nextComponentName != component.Name && nextComponentName != null) && component.IsUseFlipTable == false)
                      {
                          // 如果是最后一颗就在这里发要料信号
                          // 刷新Map信息
                          WaferSystemDomain.GetInstance().Block.RefreshMap();

                          // 芯片名称传给晶圆台
                          WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(nextComponentName);

                          // 给晶圆台发要料信号
                          SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                          // 自动重取次数清零
                          System2RunTimeProvider.UpLookAutoSkipCount = 0;

                          // 跳过次数清零
                          System2RunTimeProvider.StaticWaffleComponentAutoSkipCount = 0;

                          Static.RecordTime("取片信号交互", $"上视定位完成，给晶圆发要料信号，芯片名称{component.Name}");
                      }
                  });

            return ExcuteResult.Success;
        }
    }
}
