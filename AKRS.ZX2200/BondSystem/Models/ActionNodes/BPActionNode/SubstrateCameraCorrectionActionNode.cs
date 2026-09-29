using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    /// 基板相机矫正
    /// </summary>
    public partial class SubstrateCameraCorrectionActionNode : ActionNode
    {
        /// <summary>
        /// BondDomain
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        ///  中转台控制器
        /// </summary>
        private IPTController iPTController = new IPTController();

        /// <summary>
        /// 当前所需要的芯片
        /// </summary>
        private BaseCarrierConfig component => System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().component;

        /// <summary>
        /// BondAction Provider
        /// </summary>
        private readonly BondActionService bondActionProvider = new BondActionService();

        /// <summary>
        /// Pick动作节点帮助类
        /// </summary>
        private readonly PickActionService pickActionProvider = new PickActionService();

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara ULMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 放片位
        /// </summary>
        private AKRSPoint2D placePos = new AKRSPoint2D();

        /// <summary>
        /// 放片高度
        /// </summary>
        private double placeLevel;

        /// <summary>
        /// 下一颗芯片名称
        /// </summary>
        private string nextComponentName;

        /// <summary>
        /// 初始速度（取片前的速度）
        /// </summary>
        private double initialSpeed;

        /// <summary>
        ///  当前焊点
        /// </summary>
        private BondPosition bondPosition;

        /// <summary>
        ///  当前吸嘴
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
            /// 去中转台前Z速度
            /// </summary>
            private double zSpeed;

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;

            try
            {
                this.PrepareBeforeAction();

                // 判断纠偏相机类型
                if (this.component.AccuracyMode == AccuracyModeEnum.Off
                    || this.component.AdjustCamera == CameraTypeEnum.UpLookCamera)
                {
                    return ExcuteResult.Success;
                }

                // 空跑模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.SubstrateCameraCorrectionActionDryRun();
                }

                #region 去放片位置

                #region  位置计算

                AKRSPoint3D iPTPosInG0 = this.component.IPTType == IPTTypeEnum.LeftIPT
                                             ? BondDevicePara.GetInstance().IPTDevicePara.IPTPos
                                             : BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos;

                // IPT位置转到Bond
                AKRSPoint3D iPTPosInBond = this.bondModuleController.ConvertG0ToMachinePos(iPTPosInG0);

                // 焊头需要旋转的角度 = 焊头放片角度
                double putDownAngle = this.component.DownLookAdjustConfig.PRVisionAngle + this.bondHeadController.GetCurrentNozzle().AlignAngle;

                // 计算焊头旋转后吸嘴相对焊头的偏移
                AKRSPoint2D nozzleOffsetRotated = this.bondHeadController.GetNozzleOffset(
                    nozzle.Name,
                    putDownAngle - nozzle.AlignAngle);

                // 最终放片位为 = IPT平台中心位置 + 放片补偿 - 吸嘴和焊头中心的偏移量;
                // 加减吸嘴的位置有歧义
                this.placePos = new AKRSPoint2D()
                                    {
                                        X = iPTPosInBond.X + this.component.IPTCenterOffset.X - nozzleOffsetRotated.X,
                                        Y = iPTPosInBond.Y + this.component.IPTCenterOffset.Y - nozzleOffsetRotated.Y
                                    };

                // 最终放片高度 = IPT平台高度 + 吸嘴测高高度 + 芯片的厚度
                this.placeLevel = iPTPosInBond.Z + nozzle.MeasureHeightOffset + this.component.ComponentThickness;

                // 中转台预放片位
                AKRSPoint4D prePlacePos = new AKRSPoint4D()
                                              {
                                                  X = this.placePos.X,
                                                  Y = this.placePos.Y,
                                                  Z = this.component.IsActivateSlowTravelBeforePlaceOnIPT
                                                          ? this.placeLevel + this.component.IPTSlowTravelDistanceBeforeBonding
                                                          : this.placeLevel,
                                                  T = putDownAngle
                                              };

                #endregion

                // 去中转台
                ExcuteResult ret = this.MoveToIPT(this.component, prePlacePos);

                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                if (Machine.GetInstance().IsStop())
                {
                    return ExcuteResult.Abort;
                }

                #endregion

                #region 放片

                this.ZeroBondheadBeforePlace();

                System2RunTimeProvider.RecordTime("SubstrateCameraCorrectionActionNode", "焊头清零 完成");

                BondTypeEnum bondType = component.IPTType == IPTTypeEnum.LeftIPT ? BondTypeEnum.BondOnLeftIPT : BondTypeEnum.BondOnRightIPT;

                // 放片动作
                ret = this.Place(bondType);

                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                if (Machine.GetInstance().IsStop())
                {
                    return ExcuteResult.Abort;
                }

                #endregion

                #region 视觉定位

                #region 去视觉位置

                // 计算真实拍照位 = 示教时的位置 - 焊头和相机之间的距离
                AKRSPoint3D p1RealVisionPos =
                    this.bondModuleController.ConvertG0ToMachinePos(this.component.DownLookAdjustConfig.P1VisionPos)
                    - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                this.bondPosition.BondPositionInfo.IPTVisionPos = this.bondModuleController.ConvertMachineToG0Pos(p1RealVisionPos);

                // 去拍照位
                ret = this.MoveToIPTVisionPos(p1RealVisionPos);
                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                if (Machine.GetInstance().IsStop())
                {
                    return ExcuteResult.Abort;
                }

                #endregion

                #region 视觉定位

                // P1定位
                (ExcuteResult result, MatchResult matchResult) result1 = this.DownLookVision(
                    this.component.DownLookAdjustConfig.P1PRName,true);

                if (result1.result != ExcuteResult.Success)
                {
                    return result1.result;
                }

                // 取片位置
                AKRSPoint3D pickPos = new AKRSPoint3D();

                // 取片角度
                double pickAngle = this.bondHeadController.GetAxisTRealPos();

                if (!this.component.IsTwoPointAdjust)
                {
                    // 旋转的弧度
                    double rotaryDegree = (-result1.matchResult.Angle - this.component.IPTDownLookVisionAngle) * Math.PI / 180.0;

                    pickAngle = pickAngle - result1.matchResult.Angle;

                    // 芯片现在所在的真实位置 = 相机所看到的位置 + 芯片中心绕视觉中心旋转后的位置 - 原始位置
                    AKRSPoint3D centerPoint3D = MathHelper.RotateCenter(
                                                    this.component.IPTComponentCenter,
                                                    this.component.DownLookAdjustConfig.P1VisionPos,
                                                    rotaryDegree) - this.component.DownLookAdjustConfig.P1VisionPos
                                                + this.system2Controller.GetBondVisionResultPos(result1.matchResult);

                    // 计算焊头旋转后吸嘴相对焊头的偏移
                    AKRSPoint2D realPickAngleOffset = this.bondHeadController.GetNozzleOffset(
                        nozzle.Name,
                        pickAngle - nozzle.AlignAngle);

                    // 计算取片位置 取片位置 = 芯片所在的位置 + 焊头和相机的距离 + 吸嘴的偏移
                    // 此时芯片中心和吸嘴中心重合
                    pickPos = centerPoint3D + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset
                                - new AKRSPoint3D(realPickAngleOffset.X, realPickAngleOffset.Y, 0);
                    bondPosition.BondPositionInfo.ComponentAngleOffSet = result1.matchResult.Angle;

                    // 中转台芯片偏移=定位结果位置-拍照位
                    this.bondPosition.BondPositionInfo.SubstrateCameraCompensate =
                        this.system2Controller.GetBondVisionResultPos(result1.matchResult)
                        - this.bondModuleController.GetG0RealPosition();
                }
                else
                {
                    AKRSPoint3D realPoint3D1 = this.system2Controller.GetBondVisionResultPos(result1.matchResult);

                    // 计算真实拍照位
                    AKRSPoint3D p2RealVisionPos = this.component.DownLookAdjustConfig.P2VisionPos - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                    this.bondPosition.BondPositionInfo.IPTP2VisionPos = p2RealVisionPos;

                    // 去P2拍照位
                    this.bondModuleController.MoveToG0PosWithoutSafe(p2RealVisionPos);

                    // P2定位
                    (ExcuteResult result, MatchResult matchResult) result2 = this.DownLookVision(
                        this.component.DownLookAdjustConfig.P2PRName, false);

                    if (result2.result != ExcuteResult.Success)
                    {
                        return result2.result;
                    }

                    // 旋转的弧度
                    double rotaryDegree =
                        (-(result1.matchResult.Angle + result2.matchResult.Angle) / 2.0
                         - this.component.IPTDownLookVisionAngle) * Math.PI / 180.0;

                    // 计算角度 取片角度 = 原始放片角度 + 增加的角度
                    pickAngle = pickAngle - (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0;

                    AKRSPoint3D visionCenter = (this.component.DownLookAdjustConfig.P1VisionPos
                                           + this.component.DownLookAdjustConfig.P2VisionPos) / 2.0;

                    AKRSPoint3D realPoint3D2 = this.system2Controller.GetBondVisionResultPos(result2.matchResult);

                    // 芯片现在所在的真实位置 = 相机所看到的位置 + 芯片中心绕视觉中心旋转后的位置 - 原始位置
                    AKRSPoint3D centerPoint3D =
                        MathHelper.RotateCenter(this.component.IPTComponentCenter, visionCenter, rotaryDegree)
                        - visionCenter + (realPoint3D2 + realPoint3D1) / 2.0;

                    // 计算焊头旋转后吸嘴相对焊头的偏移
                    AKRSPoint2D realPickAngleOffset = this.bondHeadController.GetNozzleOffset(
                        nozzle.Name,
                        pickAngle - nozzle.AlignAngle);

                    // 计算取片位置 取片位置 = 芯片所在的位置 + 焊头和相机的距离 + 吸嘴的偏移
                    // 此时芯片中心和吸嘴中心重合
                    pickPos = centerPoint3D + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset
                              - new AKRSPoint3D(realPickAngleOffset.X, realPickAngleOffset.Y, 0);
                    bondPosition.BondPositionInfo.ComponentAngleOffSet =
                        (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0;

                    this.bondPosition.BondPositionInfo.SubstrateCameraCompensate =
                        (this.system2Controller.GetBondVisionResultPos(result1.matchResult)
                         + this.system2Controller.GetBondVisionResultPos(result2.matchResult)) / 2
                        - (this.bondPosition.BondPositionInfo.IPTVisionPos
                           + this.bondPosition.BondPositionInfo.IPTP2VisionPos) / 2;
                }

                //this.bondPosition.BondPositionInfo.SubstrateCameraCompensate = pickPos;

                #endregion

                #endregion

                #region 取片

                // G0坐标转换到轴坐标
                AKRSPoint3D machinePickPos = this.bondModuleController.ConvertG0ToMachinePos(pickPos);

                this.bondPosition.BondPositionInfo.IPTPickPos = this.bondModuleController.ConvertMachineToG0Pos(machinePickPos);

                AKRSPoint4D prePickPos = new AKRSPoint4D()
                {
                    X = machinePickPos.X,
                    Y = machinePickPos.Y,
                    Z = this.component.IsActivateSlowTravelBeforePickOnIPT
                                                         ? this.placeLevel + this.component.IPTSlowTravelDistanceBeforePickup
                                                         : this.placeLevel,
                    T = pickAngle
                };

                // 运动到取料位
                ret = this.MoveToIPTPickPos(prePickPos);

                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                if (Machine.GetInstance().IsStop())
                {
                    return ExcuteResult.Abort;
                }

                this.ZeroBondheadBeforePick();

                System2RunTimeProvider.RecordTime("SubstrateCameraCorrectionActionNode", "焊头清零 完成");

                PickTypeEnum pickType = component.IPTType == IPTTypeEnum.LeftIPT ? PickTypeEnum.LeftIPT : PickTypeEnum.RightIPT;

                // 执行Pick方法
                ret = this.Pick(this.placeLevel, pickType);
                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                #endregion

                #region 漏晶检测
            RePick:
            if (this.component.IsActiveComponentDetection && this.component.IsActivateSlowTravelAfterPickup)
                    {
                        // 检测吸嘴真空
                        if (!this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
                        {
                            DialogResult dialog = AKRSMessageBoxExt.Show(
                                $"中转台取片失败，请检查!  \r\n",
                                "报警",
                                new string[] { "重取", "终止", "忽略", "下一颗" },
                                new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.OK });

                            switch (dialog)
                            {
                                // 重新Pick
                                case DialogResult.Retry:
                                    goto RePick;

                                // 终止
                                case DialogResult.Abort:
                                    return ExcuteResult.Abort;

                                // 忽略
                                case DialogResult.Ignore:
                                    break;

                                // 跳过
                                case DialogResult.OK:

                                    // 抛料
                                    this.bondModuleController.ThrowAction();

                                    // 抛掉的芯片+1
                                    this.component.AddCountOfReject();

                                    // 设备暂停
                                    Machine.GetInstance().Pause();

                                    // 判断是不是最后一颗芯片
                                    if (this.nextComponentName == null || this.nextComponentName != this.component.Name)
                                    {
                                        // 如果是最后一颗就在这里发要料信号
                                        // 刷新Map信息
                                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                                        // 当前种类芯片名称传给晶圆台
                                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(this.component.Name);

                                        // 给晶圆台发要料信号
                                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                                        System2RunTimeProvider.RecordTime("取片信号交互", $"中转台流程，给晶圆发要料信号，芯片名称{this.component.Name}");
                                    }

                                    // 重置前面的动作节点,重新取贴
                                    List<ActionNode> actionNodeList = new List<ActionNode>();
                                    actionNodeList.Add(this.system2Domain.BondActionNodeRepository.PickActionNode);
                                    actionNodeList.Add(this.system2Domain.BondActionNodeRepository.SubstrateCameraCorrectionActionNode);
                                    this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                                    System2RunTimeProvider.IsRepickComponent = true;

                                    return ExcuteResult.Retry;
                            }
                        }
                    }

            #endregion

                Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("给晶圆发要料信号线程");

                            this.SendSignalToWaferTable();
                        });

                // 结束当前制程
                // bondPosition.S2FinishedStep(this.actionNodesService.GetCurrentProcessStepName());

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
        /// 下视拍照
        /// </summary>
        /// <param name="name">定位的名称</param>
        /// <returns>定位结果</returns>
        private (ExcuteResult, MatchResult) DownLookVision(string name,bool isFirstPosVision)
        {
            // 拍照停留
            DelayHelper.Delay(this.component.AdjustVisionDelay);

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

            // 设置硬件
            this.system2Controller.SetHardware(name, CameraTypeEnum.BondCamera);

            Stopwatch sp = Stopwatch.StartNew();

            // 开始定位
            ExcuteResult result = pREntity.DoWork();

            sp.Stop();
            long time1 = sp.ElapsedMilliseconds;

            LogHelper.Post(Level.Info, $" 中转台下视{name}定位完成，用时{time1} ms", LogCategory.Bond);

            // 处理拍照完成后的结果
            if (result != ExcuteResult.Success)
            {
                (DialogResult dialogResult, BaseAlgResult match) result2 = UcMainSystem.VisionAlarmFunc(
                   pREntity,
                   name,
                   $"中转台定位失败");

                switch (result2.dialogResult)
                {
                    case DialogResult.Abort:

                        // 退出
                        return (ExcuteResult.Abort, null);

                    case DialogResult.OK:

                        return (ExcuteResult.Success, (MatchResult)result2.match);

                    case DialogResult.Ignore:

                        #region 取片、抛料

                        // 运动到放料位
                        this.bondModuleController.MoveBondXY(this.placePos.X, this.placePos.Y);

                        PickTypeEnum pickType = component.IPTType == IPTTypeEnum.LeftIPT ? PickTypeEnum.LeftIPT : PickTypeEnum.RightIPT;

                    Repick:
                        this.bondHeadController.PickAction(
                            this.placeLevel,
                            this.component,
                            BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                            pickType);

                        if (this.component.IsActiveComponentDetection)
                        {
                            // 检测吸嘴真空
                            if (!this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
                            {
                                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                    $"中转台芯片去取片失败!  \r\n" + "Repick：重取r\n" + "Abort：退出 \r\n" + "Ignore: 忽略\r\n",
                                    "Alarm",
                                    new string[] { "Repick", "Abort", "Ignore" },
                                    new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.OK });

                                switch (dialogResult)
                                {
                                    // 重新Pick
                                    case DialogResult.Retry:

                                        goto Repick;

                                    // 终止
                                    case DialogResult.Abort:

                                        // 退出
                                        return (ExcuteResult.Abort, null);

                                    // 忽略
                                    case DialogResult.Ignore:
                                        break;
                                }
                            }
                        }

                        #endregion

                        // 抛料
                        this.bondModuleController.ThrowAction();

                        // 判断是不是最后一颗芯片
                        if (this.nextComponentName == null || this.nextComponentName != this.component.Name)
                        {
                            // 如果是最后一颗就在这里发要料信号
                            // 刷新Map信息
                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                            // 当前种类芯片名称传给晶圆台
                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(this.component.Name);

                            // 给晶圆台发要料信号
                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                            System2RunTimeProvider.RecordTime("取片信号交互", $"中转台流程，给晶圆发要料信号，芯片名称{this.component.Name}");
                        }

                        // 抛掉的芯片+1
                        this.component.AddCountOfReject();

                        // 重置前面的动作节点,重新取贴
                        List<ActionNode> actionNodeList = new List<ActionNode>();
                        actionNodeList.Add(this.system2Domain.BondActionNodeRepository.PickActionNode);
                        actionNodeList.Add(this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode);
                        actionNodeList.Add(this.system2Domain.BondActionNodeRepository.SubstrateCameraCorrectionActionNode);
                        this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                        System2RunTimeProvider.IsRepickComponent = true;

                        return (ExcuteResult.Retry, null);

                    default:
                        // 退出
                        return (ExcuteResult.Abort, null);
                }
            }

            //VisionService.SaveVisionImage(
            //    pREntity.AlgResult.OutPutImg1,
            //    new List<string>()
            //        {
            //            "Bond",
            //            TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit?.Name,
            //            pREntity.GetName()
            //        },
            //    System2Domain.GetInstance().ActionNodesService.CurrentMatter(),
            //    pREntity.OriginalBmp);

            LocateUseConfig locateUseConfig = isFirstPosVision ? this.component.DownLookAdjustConfig.LocateUseConfig1 : this.component.DownLookAdjustConfig.LocateUseConfig2;

            MatchResult matchResult = System2RunTimeProvider.GetMatchResultByConfig((MatchResult)pREntity.AlgResult, locateUseConfig);

            return (ExcuteResult.Success, matchResult);
        }

        /// <summary>
        /// 放片
        /// </summary>
        /// <param name="bondType">贴片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult Place(BondTypeEnum bondType)
        {
            System2RunTimeProvider.RecordTime("中转台", "放片开始");

            try
            {
                BondActionParameter bondActionParameter =
                    this.bondActionProvider.GetBondActionParameter(this.component, bondType);

                // 预备抬起位
                double liftPreLevel = this.placeLevel + bondActionParameter.SlowTravelDistanceAfterPlace;

                System2RunTimeProvider.RecordTime("Place", "准备运动到放片高度");

                this.SlowDownToPlaceLevel(bondActionParameter);

                System2RunTimeProvider.RecordTime("Place", "运动到放片高度完成");

                // 开平台真空
                bondActionParameter.VacuumElectric?.SetOutputValue(true);

                int placementDelay = (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay) > 0
                                         ? (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay)
                                         : 0;

                // 固晶延迟
                DelayHelper.Delay(placementDelay);

                System2RunTimeProvider.RecordTime("Place", $"固精延时: {bondActionParameter.PlacementDelay}ms");

                // 关闭吸嘴真空
                this.bondHeadController.CloseToolVaccum();

                System2RunTimeProvider.RecordTime("Place", $"关吸嘴真空");

                // 异步开弱吹气
                this.BlowAsyn(bondActionParameter);

                // 关真空延迟
                DelayHelper.Delay(bondActionParameter.VacuumOffDelay);

                System2RunTimeProvider.RecordTime("Place", $" 关真空延迟: {bondActionParameter.VacuumOffDelay}ms");

                this.SlowUpAfterPlace(liftPreLevel, bondActionParameter);

                System2RunTimeProvider.RecordTime("中转台", $"放片完成");
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"固晶失败", ex, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"中转台流程:放片到中转台失败! \r\n" + ex.ToString(),
                    "Alarm",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw ex;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 取料
        /// </summary>
        /// <param name="pickLevel">取片高度</param>
        /// <param name="pickType">取片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult Pick(
            double pickLevel,
            PickTypeEnum pickType)
        {
            try
            {
                System2RunTimeProvider.RecordTime("中转台流程", "开始Pick动作 ");

                PickActionParameter pickActionParameter =
                    this.pickActionProvider.GetPickActionParameter(this.component, pickType);

                // 取片准备位
                double pickPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceBeforePickup;

                // 预备抬起位
                double liftPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceAfterPickup;

                this.pickActionProvider.SlowDownToPickLevel(pickLevel, pickActionParameter);

                System2RunTimeProvider.RecordTime(
                    "中转台流程",
                    $"运动到取片高度{pickLevel}完成");

                // 吸嘴吸真空打开
                this.bondHeadController.OpenToolVaccum();

                System2RunTimeProvider.RecordTime("中转台流程", $"打开吸嘴真空完成");

                // 关平台真空
                pickActionParameter.VacuumElectric?.SetOutputValue(false);

                System2RunTimeProvider.RecordTime("中转台流程", $"关中转台真空完成");

                // 中转台吹气
                if (component.IPTBlowingDelayDuringPlaceOnIPT > 0)
                {
                    // 开平台吹气
                    pickActionParameter.BlowElectric.SetOutputValue(true);

                    System2RunTimeProvider.RecordTime("中转台流程", $"开平台吹气");

                    Thread.Sleep(pickActionParameter.TableBlowDelay);

                    System2RunTimeProvider.RecordTime("中转台流程", $"平台吹气延时{pickActionParameter.TableBlowDelay}ms");

                    // 关平台吹气
                    pickActionParameter.BlowElectric.SetOutputValue(false);

                    System2RunTimeProvider.RecordTime("中转台流程", $"关平台吹气");
                }

                // 拾取延迟
                DelayHelper.Delay(pickActionParameter.PickDelay);

                System2RunTimeProvider.RecordTime("Pick", $"拾取延时{pickActionParameter.PickDelay} ms 完成");

                // 二段速抬起
                this.SlowUpAfterPick(liftPreLevel, pickActionParameter);

                System2RunTimeProvider.RecordTime("Pick", $"焊头抬起完成，取片结束");
            }
            catch (NullReferenceException e)
            {
                LogHelper.Post(Level.Error, $"获取力控配置参数失败", e, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"中转台流程：取片失败! \r\n" + e.ToString(),
                    "Alarm",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"取料失败", ex, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"中转台流程：取片失败! \r\n" + ex.ToString(),
                    "Alarm",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }

            return ExcuteResult.Success;
        }
    }
}
