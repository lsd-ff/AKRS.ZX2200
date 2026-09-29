using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ActionNode = AKRS.ZX2200.Infrastructure.Models.CommonModels.ActionNode;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate;

    /// <summary>
    /// 固晶动作节点
    /// </summary>
    [Serializable]
    public partial class BondActionNode : ActionNode
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
        /// BondActionProvider
        /// </summary>
        private BondActionService bondActionProvider = new BondActionService();

        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition bondPosition { get; set; }

        /// <summary>
        /// 固晶高度(绝对固晶高度)
        /// </summary>
        private double bondLevel { get; set; }

        /// <summary>
        /// 真实的贴片位(焊点的焊点的位置+焊点硬补偿 - 上视的偏移值-焊后补偿+温漂补偿)
        /// </summary>
        private AKRSPoint3D realBondPositionInG0;

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara ULmPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 当前芯片
        /// </summary>
        private BaseCarrierConfig component;

        /// <summary>
        /// 当前吸嘴
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 固精前速度
        /// </summary>
        private double speed;

        /// <summary>
        /// 抬起高度
        /// </summary>
        private double liftLevel;

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;

            System2RunTimeProvider.RecordTime("BondAction", "Start ----------------");

            try
            {
                // 参数准备
                this.PrepareBeforeBondAction();
                
                // 离线模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    this.bondPosition.S2FinishedStep(this.system2Domain.ActionNodesService.GetCurrentProcessStepName());
                    return ExcuteResult.Success;
                }

                // 空跑模式
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.BondActionDryRun();
                }

                //// 温飘补偿
                //if (!RealTimeCorrection.GetInstance().ScanReferencePoints())
                //{
                //    return ExcuteResult.Abort;
                //}

                // 计算固精位
                AKRSPoint4D bondPos = this.CalculateBondPos();

                // 预固晶位置
                AKRSPoint4D preBondPos = new AKRSPoint4D()
                {
                    X = bondPos.X,
                    Y = bondPos.Y,
                    Z = component.IsActivateSlowTravelBeforeBonding
                                                         ? bondPos.Z + component.SlowTravelDistanceBeforeBonding
                                                         : bondPos.Z,
                    T = bondPos.T
                };


                // 移动到预固晶位置
                ExcuteResult res = this.MoveToPreBondPos(preBondPos, component, liftLevel);
                if (res != ExcuteResult.Success)
                {
                    return res;
                }

                this.SendSignalToSlideFluxer();

                // 记录
                this.bondPosition.BondPositionInfo.RealBondPos =
                    this.bondModuleController.Get3DRealPosition();

                #region 固晶

            ReBond:

                // 开始设置力控曲线
                this.StartSetForceRealTimeCurve();

                // 焊头清零
                this.ZeroBondhead();

                System2RunTimeProvider.RecordTime("BondAction", "焊头清零 完成");

                BondTypeEnum bondType;

                // 判断固晶类型
                if (component.IsDipOnTU
                    && System2RunTimeProvider.WatchFluxCount % component.DipOnTUAfterBondingNum == 0)
                {
                    bondType = BondTypeEnum.DipFluxOnTU;
                }
                else
                {
                    bondType = BondTypeEnum.BondOnTU;
                }

                // 执行固晶动作
                ExcuteResult result = this.Bond(bondType);

                if (result != ExcuteResult.Success)
                {
                    return result;
                }

                //this.StopSetForceRealTimeCurve();

                this.ExportForceData();

                #endregion

                #region 焊后回带检测

                if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle&& bondType != BondTypeEnum.DipFluxOnTU)
                {
                    if (component.IsActiveComponentDetection && component.IsActivateSlowTravelAfterBonding)
                    {
                        if (this.bondHeadController.GetNozzleBlowState())
                        {
                            this.bondHeadController.CloseToolBlowEle();
                        }

                        // 检测
                        bool hasMaterial = !this.bondHeadController.ComponentCheckAfterBonding(
                        this.bondHeadController.GetCurrentNozzle());

                        // 关闭真空
                        Task.Run(this.bondHeadController.CloseToolVaccum);

                        if (hasMaterial)
                        {
                            DialogResult dialog = AKRSMessageBoxExt.Show(
                                $"真空检测到吸嘴上还有芯片，请检查吸嘴!  \r\n" + "重贴：重新贴片\r\n" + "终止：退出工作\r\n" + "忽略：忽略，抛料后继续工作\r\n" + "重新取贴: 抛料重取\r\n",
                                "报警",
                                new string[] { "重贴", "终止", "忽略", "重新取贴" },
                                new DialogResult[] { DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.Cancel });

                            switch (dialog)
                            {
                                // 重新Bond
                                case DialogResult.Retry:

                                    goto ReBond;

                                // 终止
                                case DialogResult.Abort:

                                    StatisticsDomain.GetInstance().BondFailedCount++;
                                    return ExcuteResult.Abort;

                                // 忽略
                                case DialogResult.Ignore:

                                    StatisticsDomain.GetInstance().BondFailedCount++;
                                    this.bondModuleController.ThrowAction();
                                    break;

                                // 取消
                                case DialogResult.Cancel:

                                    StatisticsDomain.GetInstance().BondFailedCount++;

                                    this.bondModuleController.ThrowAction();

                                    // 抛掉的芯片+1
                                    component.AddCountOfReject();

                                    // 重置前面的动作节点,重新取贴
                                    List<ActionNode> actionNodeList = new List<ActionNode>();
                                    actionNodeList.Add(this.system2Domain.BondActionNodeRepository.PickActionNode);
                                    if (component.AccuracyMode != AccuracyModeEnum.Off
                    && component.AdjustCamera == CameraTypeEnum.UpLookCamera)
                                    {
                                        actionNodeList.Add(this.system2Domain.BondActionNodeRepository.UpLookCorrectionActionNode);
                                    }

                                    if (component.AccuracyMode != AccuracyModeEnum.Off
                   && component.AdjustCamera == CameraTypeEnum.BondCamera)
                                    {
                                        actionNodeList.Add(this.system2Domain.BondActionNodeRepository.SubstrateCameraCorrectionActionNode);
                                    }

                                    actionNodeList.Add(this.system2Domain.BondActionNodeRepository.BondActionNode);
                                    this.system2Domain.ActionNodesService.SetActionNodeReWork(actionNodeList);

                                    System2RunTimeProvider.IsRepickComponent = true;

                                    return ExcuteResult.Retry;
                            }
                        }

                        System2RunTimeProvider.RecordTime("BondAction", $"焊后回带检测完成");
                    }
                }

            #endregion

                #region 隔几颗去看一下胶印

                if (component.IsDipOnTU
                    && System2RunTimeProvider.WatchFluxCount % component.DipOnTUAfterBondingNum == 0)
                {
                    System2RunTimeProvider.WatchFluxCount = 0;

                    // 去看胶印
                    ExcuteResult ret = this.DipFluxOnBondPositionVision(component, bondPos);

                    switch (ret)
                    {
                        case ExcuteResult.Success:
                            break;

                        case ExcuteResult.Fail:
                            StatisticsDomain.GetInstance().BondFailedCount++;
                            break;

                        case ExcuteResult.Abort:
                            return ExcuteResult.Abort;

                        case ExcuteResult.Exception:
                            StatisticsDomain.GetInstance().BondFailedCount++;
                            DialogResult dialog = AKRSMessageBoxExt.Show(
                                $" Bond相机识别PR:{component.DipPRName}异常，设备将退出自动工作！",
                                "报警",
                                new string[] { "确认" },
                                new DialogResult[] { DialogResult.OK });

                            return ExcuteResult.Exception;
                    }
                }

                System2RunTimeProvider.WatchFluxCount++;

                #endregion

                // 按快捷键 去看已经贴完的BondPosition
                this.VisionPreviousBondPosition();

                SkipBond:

                System2RunTimeProvider.IsMaterialOnBondhead = false;

                // 修改制程
                this.bondPosition.S2FinishedStep(this.actionNodesService.GetCurrentProcessStepName());

                // 统计固精周期，异步，如果出现卡顿，屏蔽掉
                StatisticsDomain.GetInstance().StatisticsBondPositionInfo(this.bondPosition,this.component);

                System2RunTimeProvider.RecordTime("BondAction", "统计固精周期 完成");

                this.bondHeadController.GetCurrentNozzle().FrequencyConsumables.CurrentUseTimes++;

                // 检查贴片颗数是否超上限 
                this.CheckBondedNumber();

                UcMainSystem.ReFreshSystem2TuAction();

                System2RunTimeProvider.RecordTime("BondAction", "Action 结束");

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

            // 当前焊点如果成功了或者失败了都不会贴片
            if (bp.EntityState != EntityState.Process)
            {
                return false;
            }

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
        /// 焊头清零
        /// </summary>
        private void ZeroBondhead()
        {
            if (this.component.BondingForceMode == ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.BondingForce);

               this.bondHeadController.ZeroBondhead(isSmallForce);
            }
        }

        /// <summary>
        /// 贴片
        /// </summary>
        /// <param name="bondType">贴片类型</param>
        /// <returns>结果</returns>
        private ExcuteResult Bond(BondTypeEnum bondType)
        {
            System2RunTimeProvider.RecordTime("Bond", "Bond  开始");

            try
            {
                BondActionParameter bondActionParameter =
                    this.bondActionProvider.GetBondActionParameter(component, bondType);

                // 预备抬起位
                double liftPreLevel = this.bondLevel + bondActionParameter.SlowTravelDistanceAfterPlace;

                // 二段速去固精高度
                this.SlowDownToBondLevel(bondActionParameter);

                System2RunTimeProvider.RecordTime("Bond", "低速移动到固晶高度完成");

                int placementDelay = (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay) > 0
                            ? (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay)
                            : 0;

                // 固晶延迟
                DelayHelper.Delay(placementDelay);

                System2RunTimeProvider.RecordTime("Bond", $"固精延时: {bondActionParameter.PlacementDelay}ms");

                // 蘸胶不关真空也不弱吹
                if (bondType != BondTypeEnum.DipFluxOnTU)
                {
                    // 关闭吸嘴真空
                    this.bondHeadController.CloseToolVaccum();

                    System2RunTimeProvider.RecordTime("Bond", $"关吸嘴真空");

                    // 异步开弱吹
                    this.BlowAsyn(bondActionParameter);

                    // 关真空延迟
                    DelayHelper.Delay(bondActionParameter.VacuumOffDelay);

                    System2RunTimeProvider.RecordTime("Bond", $" 关真空延时: {bondActionParameter.VacuumOffDelay}ms");
                }

                // 记录最终贴片力
                this.RecordLastBondForce();

                // 结束力控读取
                UcMainSystem.ActiveReadBondForce(false);

                // 二段速低速上抬
                this.SlowUpToLiftLevel(liftPreLevel, bondActionParameter);

                System2RunTimeProvider.RecordTime("Bond", $"Bond  完成");
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"固晶失败", ex, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"固晶失败! \r\n" + ex.ToString(),
                    "固晶报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return ExcuteResult.Exception;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 记录最终贴片力
        /// </summary>
        private void RecordLastBondForce()
        {
            if (this.component.BondingForceMode != ForceModeEnum.Force)
            {
                return;
            }

            // etel机台没办法读模拟量
            if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
            {
                return;
            }

            double angle = this.bondHeadController.GetAxisTRealPos();

            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.BondingForce);

            double readForce = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);
            double forecIncrement = readForce - System2RunTimeProvider.BondHeadInitialVal;

            // 记录力值
            this.bondPosition.BondPositionInfo.ActualBondForce =
                ForceCalibrationService.ForceIncrementToActualForce(forecIncrement, isSmallForce, angle);
        }
    }
}
