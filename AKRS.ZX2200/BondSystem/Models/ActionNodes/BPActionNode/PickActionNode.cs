using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Module = AKRS.ZX2200.TransportUnitSystem.Module.Matter.Module;


namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Programs;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.XtraPrinting.Native;

    /// <summary>
    /// 取料流程
    /// </summary>
    [Serializable]
    public partial class PickActionNode : ActionNode
    {
        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

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
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        ///  翻转模组
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// Pick动作节点帮助类
        /// </summary>
        private PickActionService pickActionProvider = new PickActionService();

        /// <summary>
        /// 程式
        /// </summary>
        private FlipModuleProgram flipModuleProgram => WaferSystemProgram.GetInstance().FlipModuleProgram;

        /// <summary>
        /// ULM运动点位
        /// </summary>
        private ULMPara uLMPara => BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 翻转台设备参数
        /// </summary>
        private FlipTableDevicePara flipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        /// <summary>
        /// 下一颗芯片名称
        /// </summary>
        private string nextComponentName;

        /// <summary>
        /// 读取片力值(LVDT)
        /// </summary>
        private bool isReadPickupForce = false;

        /// <summary>
        ///  顶针顶起过程中LVDT值采集
        /// </summary>
        private List<(DateTime time, double lvdtVal)> lvdtValueList = new();

        /// <summary>
        /// Bond程式
        /// </summary>
        private BondProgram bondProgram => BondProgram.GetInstance();

        /// <summary>
        /// 芯片
        /// </summary>
        private BaseCarrierConfig component =>
            actionNodesService.GetCurrentComponent().component;

        /// <summary>
        /// 焊点
        /// </summary>
        private BondPosition bp;

        /// <summary>
        ///  是否跳过此芯片
        /// </summary>
        private bool isSkipComponent = false;

        /// <summary>
        /// 当前吸嘴
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 初始速度（取片前的速度）
        /// </summary>
        private double initialSpeed;

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            System2RunTimeProvider.RecordTime("PickAction", "Start ----------------");

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return ExcuteResult.Success;
            }

            try
            {
                this.PrepareBeforePickAction();

                // 换吸嘴
                ExcuteResult ret = this.JudgeAndChangeNozzle();
                if (ret != ExcuteResult.Success)
                {
                    return ret;
                }

                // 擦拭吸嘴
                this.JudgeAndCleanNozzle();

                // 空跑
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
                {
                    return this.PickActionDryRun();
                }

                // 取片温漂补偿
                if (!TpMarkCompensate.PickUpCompensationVision(true))
                {
                    return ExcuteResult.Abort;
                }


                AKRSPoint3D pickCompensation = TpMarkCompensate.GetPickCompensation();

            #region 定位并计算取料位

            NextDie:

                // 静态华夫盒芯片定位结果
                MatchResult matchResult = null;

                // 芯片定位角度
                double componentAngle = default;

                System2RunTimeProvider.RecordTime("PickAction", $"判断吸嘴有无芯片完成 - 开始等待晶圆允许取料信号,当前芯片名称{this.component.Name}");

                if (!MachineStateModel.GetInstance().IsCompensateWork)
                {
                    // 允许取料信号判断（从信号池获取）,这个信号会自动复位
                    if (System2RunTimeProvider.WaitAllowBondPickSignal(component) != ExcuteResult.Success)
                    {
                        return ExcuteResult.Abort;
                    }
                }

                System2RunTimeProvider.RecordTime("PickAction", $"开始等待晶圆允许取料信号 -- 等待晶圆允许取料信号完成,当前芯片名称{this.component.Name}");

                // 芯片Total数加一
                component.AddCountOfTotal();

                // 静态华夫盒：定位并获取芯片定位角度
                if (component.CarrierType == CarrierTypeEnum.StaticWaffle)
                {
                    // 静态华夫盒定位
                    (ExcuteResult res, MatchResult matchResult) result = this.StaticWaffleVision(this.isSkipComponent);

                    switch (result.res)
                    {
                        case ExcuteResult.Abort:
                            return ExcuteResult.Abort;

                        case ExcuteResult.Success:

                            // 保存结果
                            matchResult = result.matchResult;
                            componentAngle = result.matchResult.Angle;
                            break;

                        case ExcuteResult.Retry:

                            // 下一颗
                            goto NextDie;

                        default:
                            return result.res;
                    }
                }
                else
                {
                    if (component.SearchCamera == SearchCameraEnum.BondCamera)
                    {
                       ExcuteResult excuteResult = this.MoveToSearchPosAndWaitSignal(component.BondCameraSearchPosition);

                       if (excuteResult != ExcuteResult.Success)
                       {
                           return excuteResult;
                       }
                    }

                    // 获取芯片定位角度
                    componentAngle = WaferSystemDomain.GetInstance().Block.GetBondSpinAngle();
                }

                bp.BondPositionInfo.ComponentAngleOnWafer = componentAngle;

                // 计算最终取晶位
                AKRSPoint4D curPickPos = this.system2Controller.CalculatePickPos(component, componentAngle, matchResult);

                #endregion

                #region 去取料位

                // 取片前准备，主要是真空
                this.system2Controller.PrepareBeforePick(component);

                // 预取片位
                AKRSPoint4D prePickPos = new AKRSPoint4D()
                {
                    X = curPickPos.X + pickCompensation.X,
                    Y = curPickPos.Y + pickCompensation.Y,
                    Z = component.IsActivateSlowTravelBeforePickup
                                                         ? curPickPos.Z + component.SlowTravelDistanceBeforePickup
                                                         : curPickPos.Z,
                    T = curPickPos.T
                };

                // 获取安全高度
                double liftSafeLevel = this.bondModuleController.GetBondheadSafeLevel(prePickPos);

                // 去预取片位
                ExcuteResult res = this.MoveToPrePickPos(
                    prePickPos,
                    liftSafeLevel,
                    component.CarrierType,
                    component.SearchCamera);

                if (res != ExcuteResult.Success)
                {
                    return res;
                }

                #endregion

                // 开始设置力控实时曲线
                this.StartSetBondForceRealTimeCurve();

                // 保存取片力值
                this.StartCollectPickupForce();

                // 取片前检查
                ExcuteResult checkRes = this.CheckBeforePick(component);
                if (checkRes != ExcuteResult.Success)
                {
                    return checkRes;
                }

                #region 取料

                // 取料类型
                PickTypeEnum pickType;

                if (component.IsUseFlipTable)
                {
                    pickType = PickTypeEnum.FlipTable;
                }
                else
                {
                    // 判断芯片类型
                    pickType = component is CarrierWithWaferConfig ? PickTypeEnum.CarrierWithWafer : PickTypeEnum.CarrierWithWaffle;
                }

            Repick:

                // 执行Pick方法
                ExcuteResult pickRes = this.Pick(curPickPos.Z, pickType);

                switch (pickRes)
                {
                    case ExcuteResult.Success:
                        Static.IsComponentOnFlipTool = false;
                        break;

                    case ExcuteResult.Abort:
                        return ExcuteResult.Abort;

                    case ExcuteResult.Exception:

                        // 顶针缩回
                        WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();

                        return ExcuteResult.Exception;
                }

                //this.StopSetBondForceRealTimeCurve();

                #endregion

                // 被顶起的芯片+1
                component.AddCountOfUseable();
                WaferSubController.GetInstance().EjectController.AddCountOfUseable();

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
                            // 真空报警芯片+1
                            component.AddCountOfVacuumError();

                            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                                $"取片失败:检测到漏精！请选择如何处理! \r\n",
                                "真空报警",
                                new string[] { "重新检测", "退出", "忽略", "重取", "下一颗" },
                                new DialogResult[]
                                    {
                                        DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore,
                                        DialogResult.Cancel, DialogResult.Yes
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
                                    goto Repick;

                                // 下一颗
                                case DialogResult.Yes:

                                    this.bondModuleController.ThrowAction();          

                                    // 给翻转台发信号
                                    if (component.IsUseFlipTable)
                                    {
                                        string nextComponentName = actionNodesService.GetNextComponentName();

                                        // 判断是不是最后一颗芯片
                                        if (nextComponentName == null)
                                        {
                                            if (this.component.CarrierType == CarrierTypeEnum.Wafer)
                                            {
                                                // 顶针缩回
                                                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                                            }


                                            // 发要料信号
                                            // 刷新Map信息
                                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                                            // 芯片名称传给晶圆台
                                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                                            // 给晶圆台发要料信号
                                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                                            Static.RecordTime("取片信号交互", $"取片完成，给晶圆发要料信号，芯片名称{component.Name}");
                                        }

                                        SignalPool.GetInstance().IsBondPickSucceedSignal.Set();
                                    }
                                    else
                                    {
                                        // 刷新Map信息
                                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                                        // 芯片名称传给晶圆台
                                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                                        // 给晶圆台发要料信号
                                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();
                                    }

                                    System2RunTimeProvider.IsRepickComponent = true;

                                        System2RunTimeProvider.RecordTime("取片信号交互", $"取片失败，给晶圆发要料信号，芯片名称{component.Name}");

                                    goto NextDie;
                            }
                        }
                    }
                }

                #endregion

                System2RunTimeProvider.IsMaterialOnBondhead = true;

                Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("提前设置上视/下视灯光");

                            this.SetAcurracyCameraLight();
                        });

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

                                               if (!MachineStateModel.GetInstance().IsCompensateWork)
                                               {
                                                   this.SendSignalToWaferTable();
                                               }
                                           });
                }
                else
                {
                    Task.Run(
                                           () =>
                                           {
                                               CommonUtil.SetCurrentThreadName("给翻转台发取料成功信号线程");

                                               this.SendSignalToFlipTable();
                                           });
                }

                System2RunTimeProvider.RecordTime("PickAction", "Action 完成");
                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Info, $"流程{this.Name}运行故障", ex, LogCategory.Bond);
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
        /// 是否报警
        /// 这个一般是程序中出现空指针的时候会采用这个方法
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 获取当前基岛，
            Module curIslandAcupoint = this.actionNodesService.GetCurrentModule();

            if (curIslandAcupoint == null)
            {
                MachineStateModel.GetInstance().MachineState = Galaxy2.Machine.Enums.MachineStateEnum.Stop;
                AKRSMessageBoxExt.Show(
                    $"找不到当前基岛，请检查流道程式\r\n" + $"{this.Name}",
                    "Alarm",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                return false;
            }

            // 获取当前芯片对象是否成功
            if (!System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().Item1)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"获取当前芯片失败!",
                    "Warn",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Yes },
                    AlarmLevel.SecondLevel);

                return false;
            }

            return true;
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
                if (MachineStateModel.GetInstance().IsCompensateWork)
                {
                    return ExcuteResult.Success;
                }

                System2RunTimeProvider.RecordTime("Pick", "开始Pick动作 ");

                PickActionParameter pickActionParameter =
                    this.pickActionProvider.GetPickActionParameter(component, pickType);

                // 取片准备位
                double pickPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceBeforePickup;

                // 预备抬起位
                double liftPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceAfterPickup;

                this.pickActionProvider.SlowDownToPickLevel(pickLevel, pickActionParameter);

                System2RunTimeProvider.RecordTime(
                    "Pick",
                    $"取片动作--焊头移动到取料高度: {pickPreLevel}完成, Speed: {pickActionParameter.SlowTravelSpeedBeforePickup}");

                // 吸嘴吸真空打开
                this.bondHeadController.OpenToolVaccum();

                System2RunTimeProvider.RecordTime("Pick", $"打开吸嘴真空完成");

                // 关平台真空
                pickActionParameter.VacuumElectric?.SetOutputValue(false);

                System2RunTimeProvider.RecordTime("Pick", $"关平台真空完成");

                // 翻转台吹气
                if (pickType == PickTypeEnum.FlipTable && this.flipModuleProgram.CurrentFlipTool.BlowDelay > 0)
                {
                    this.flipTableController.Blow(
                        this.flipModuleProgram.CurrentFlipTool.BlowProportion,
                        this.flipModuleProgram.CurrentFlipTool.BlowDelay);

                    System2RunTimeProvider.RecordTime("Pick", $"翻转台以{this.flipModuleProgram.CurrentFlipTool.BlowProportion}的比例吹气{this.flipModuleProgram.CurrentFlipTool.BlowDelay}ms 完成");
                }

                // 记录取片力值
                this.RecordLastPickForce();

                System2RunTimeProvider.RecordTime("Pick", $"读取并记录取片压力完成");

                // 顶针动作
                if (pickType == PickTypeEnum.CarrierWithWafer)
                {
                    this.EjectionLift();

                    System2RunTimeProvider.RecordTime(
                        "Pick",
                        $"顶针顶起完成,顶针G0坐标{WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos.Z}");
                }

                // 拾取延迟
                DelayHelper.Delay(pickActionParameter.PickDelay);

                System2RunTimeProvider.RecordTime("Pick", $"拾取延时{pickActionParameter.PickDelay} ms 完成");

                // 结束力控读取
                UcMainSystem.ActiveReadBondForce(false);

                // 晶圆芯片同步顶的情况下不执行低速上抬动作
                if ((component is CarrierWithWaferConfig wafer && wafer.IsActivateSynchronousEjection) == false)
                {
                    // 焊头低速上抬
                    this.SlowUpToLiftLevel(liftPreLevel, pickActionParameter);

                    System2RunTimeProvider.RecordTime("Pick", $"焊头低速上抬结束");
                }

                System2RunTimeProvider.RecordTime("Pick", $"取片动作完成");
            }
            catch (NullReferenceException e)
            {
                LogHelper.Post(Level.Error, $"获取力控配置参数失败", e, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"取片动作-获取力控配置参数失败，将退出自动工作! \r\n" + e.ToString(),
                    "取片报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"取料失败", ex, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"取片失败，将退出自动工作! \r\n" + ex.ToString(),
                    "取片报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 记录最终贴片力
        /// </summary>
        private void RecordLastPickForce()
        {
            if (this.component.PickupForceMode != ForceModeEnum.Force)
            {
                return;
            }

            // etel机台没办法读模拟量
            if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
            {
                return;
            }

            double angle = this.bondHeadController.GetAxisTRealPos();

            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.PickupForce);

            double readForce = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

            double forecIncrement = readForce - System2RunTimeProvider.BondHeadInitialVal;

            // 记录力值
            this.bp.BondPositionInfo.ActualPickForce =
                ForceCalibrationService.ForceIncrementToActualForce(forecIncrement, isSmallForce, angle);
        }
    }
}
