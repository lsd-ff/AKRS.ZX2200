using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
using AKRS.ZX2200.WaferSubSystem.Modules;
using AKRS.ZX2200.WaferSubSystem.Services;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.Model;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    using System.Threading;
    using AKRS.Galaxy2.Infrastructure.CommonServices.PropertyChanged;
    using AKRS.Galaxy2.Infrastructure.CustomControls.Components;

    /// <summary>
    ///  用于存放系统2示教会用到的方法
    /// </summary>
    public partial class System2Controller
    {
        /// <summary>
        /// 取片测试
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public bool PickupFromWaferTable(BaseCarrierConfig component)
        {
            // 去避让位
            this.bondModuleController.MoveToSafePos();

            System2RunTimeProvider.RecordTime("取片测试", "Bond模组去安全位");

            #region 吸嘴堵塞判断

            if (component.IsActiveComponentDetection
                && this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
            {
                System2RunTimeProvider.RecordTime("取片测试", "吸嘴检测到有芯片残留");

                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"吸嘴检测到有芯片残留!\r\n",
                    "报警",
                    new string[] { "忽略", "抛料" },
                    new DialogResult[] { DialogResult.Yes, DialogResult.No },
                    AlarmLevel.SecondLevel);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        break;

                    case DialogResult.No:
                        this.bondModuleController.ThrowAction();
                        break;
                    default:
                        break;
                }
            }

            // 关闭吸嘴真空
            this.bondHeadController.CloseToolVaccum();

            System2RunTimeProvider.RecordTime("取片测试", "关闭吸嘴真空");

            #endregion

            // 开搜晶线程
            WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();

            System2RunTimeProvider.RecordTime("取片测试", " 启动晶圆线程");

        #region 取料

        NextDie:

            if (component.SearchCamera == SearchCameraEnum.WaferCamera
          || component.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
            // 晶圆上料准备判断（从信号池获取）,这个信号会自动复位
            if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
            {
                System2RunTimeProvider.RecordTime("取片测试", " 等晶圆台允许取料信号失败，即将退出取片流程！");

                    DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"等晶圆台允许取料信号失败，即将退出取片流程！!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }
        }

            // 获取当前吸嘴
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            #region 获取芯片定位角度

            double componentAngle = 0;
            MatchResult matchResult = null;
            if (component.CarrierType == CarrierTypeEnum.StaticWaffle)
            {
                if (component.IsMatchWithVacuum == false)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();

                    System2RunTimeProvider.RecordTime("取片测试", " 关静态华夫盒真空完成");
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();

                    System2RunTimeProvider.RecordTime("取片测试", " 开静态华夫盒真空完成");
                }

                // 是否跳过
                bool isSkip = false;

            Skip:

                // 获取拍照位
                AKRSPoint3D componentVisionPosInG0 = Block.GetInstance().GetResultDieG0Pos();
                AKRSPoint3D componentVision3DPos =
                    this.bondModuleController.ConvertG0ToMachinePos(componentVisionPosInG0);

                // 去拍照位
                this.bondModuleController.MoveSafeBondXYZ(componentVision3DPos);

                System2RunTimeProvider.RecordTime("取片测试", " 去静态华夫盒拍照位完成");

                // Bond定位
                matchResult = (MatchResult)this.BondCameraVision(
                    null,
                    component.DieMatchName);

                System2RunTimeProvider.RecordTime("取片测试", $" Bond定位完成");

                PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(component.DieMatchName);

                // 结果判断
                if (matchResult == null)
                {
                    System2RunTimeProvider.RecordTime("取片测试", $" 测试取料视觉定位失败");

                    (DialogResult dialog, BaseAlgResult baseAlgResult) result = UcMainSystem.VisionAlarmFunc(
                        pREntity,
                        component.Name,
                        "测试取料视觉定位失败");

                    if (result.dialog == DialogResult.OK)
                    {
                        matchResult = (MatchResult)result.baseAlgResult;
                    }
                    // 跳过
                    else if (result.dialog == DialogResult.Ignore)
                    {
                        // 刷新Map信息
                        // 调试模式下传true!
                        WaferSystemDomain.GetInstance().Block.RefreshMap(true);

                        // 芯片名称传给晶圆台
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                        // 给晶圆台发要料信号
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                        System2RunTimeProvider.RecordTime("取片信号交互", $"定位报警，给晶圆发要料信号，芯片名称{component.Name}");

                        isSkip = true;

                        goto NextDie;
                    }
                    else if (result.dialog == DialogResult.Abort)
                    {
                        return false;
                    }
                    else
                    {
                        return false;
                    }
                }

                // 角度
                componentAngle = matchResult.Angle;
            }
            else
            {
                if (component.SearchCamera == SearchCameraEnum.BondCamera)
                {
                    ExcuteResult excuteResult = this.MoveToSearchPosAndWaitSignal(component.BondCameraSearchPosition);

                    if (excuteResult != ExcuteResult.Success)
                    {
                        return false;
                    }
                }

                // 获取芯片定位角度
                componentAngle = WaferSystemDomain.GetInstance().Block.GetBondSpinAngle();
            }

            #endregion

            // 计算最终取晶位
            AKRSPoint4D curPickPos = this.CalculatePickPos(component, componentAngle, matchResult);

            // 移动到取晶位
            this.bondModuleController.MoveSafeBondXY(curPickPos.X, curPickPos.Y);

            System2RunTimeProvider.RecordTime("取片测试", $" BondXY移动到取晶位！");

            // 焊头旋转到取料角度
            this.bondHeadController.RotateAxisT(curPickPos.T);

            System2RunTimeProvider.RecordTime("取片测试", $" 焊头旋转到取料角度！");

            this.PrepareBeforePick(component);

            System2RunTimeProvider.RecordTime("取片测试", $" 取片前开关静态华夫盒真空完成！");

            // 判断拾取类型
            PickTypeEnum pickType = component is CarrierWithWaferConfig
                ? PickTypeEnum.CarrierWithWafer
                : PickTypeEnum.CarrierWithWaffle;

            AKRSPoint3D avoidPos = this.bondModuleController.ConvertG0ToMachinePos(
                WaferSubDevicePara.GetInstance().WaferTableDevicePara.BondAvoidStaticWafflePos);

            double nozzleOffset = this.bondHeadController.GetCurrentNozzle() != null
                                      ? this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset
                                      : 0;

            // 静态华夫盒安全高度
            double staticWaffleSafeHeight = avoidPos.Z + nozzleOffset;

            // 抬起安全高度
            double liftSafeLevel = component.CarrierType == CarrierTypeEnum.StaticWaffle
                                       ? staticWaffleSafeHeight
                                       : BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

        IgnoreCurDie:
            // 执行Pick方法
            bool ret = this.bondHeadController.PickAction(
                curPickPos.Z,
                component,
               liftSafeLevel,
                pickType);

            // 如果晶圆台当前料片为晶圆料片，则顶针回预顶起位
            if (component is CarrierWithWaferConfig)
            {
                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
            }

            if (ret == false)
            {
                return false;
            }

        #region 漏晶检测

        ReCheckVacuum:

            if (component.IsActiveComponentDetection)
            {
                // 检测真空
                if (!this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                        $"取片失败! \r\n" + "重试：重新检测真空\r\n"
                                      + "终止：退出工作\r\n"
                                      + "忽略：忽略此异常\r\n"
                                      + "重取: 重取此颗芯片\r\n"
                                      + "下一颗:取下一颗芯片\r\n",
                        "报警",
                        new string[] { "重试", "终止", "忽略", "重取", "下一颗" },
                        new DialogResult[]
                        {
                        DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.Cancel,
                        DialogResult.Yes
                        },
                        AlarmLevel.SecondLevel);

                    switch (dialogResult)
                    {
                        case DialogResult.Retry:
                            goto ReCheckVacuum;

                        case DialogResult.Abort:
                            return false;

                        case DialogResult.Ignore:
                            break;

                        case DialogResult.Cancel:
                            goto IgnoreCurDie;

                        case DialogResult.Yes:

                            // 去避让位
                            this.bondModuleController.MoveToSafePos();

                            // 刷新Map信息
                            WaferSystemDomain.GetInstance().Block.RefreshMap();

                            // 芯片名称传给晶圆台
                            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                            // 给晶圆台发要料信号
                            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                            System2RunTimeProvider.RecordTime("取片信号交互", $"取片失败，给晶圆发要料信号，芯片名称{component.Name}");

                            goto NextDie;
                    }
                }
            }

            #endregion

            #endregion

            // 刷新Map信息
            WaferSystemDomain.GetInstance().Block.RefreshMap(true);

            // 芯片转正
            this.bondHeadController.RotateAxisT(nozzle.AlignAngle + component.RotaryPosition);

            return true;
        }

        /// <summary>
        /// 去搜晶位置并等待信号
        /// </summary>
        /// <param name="posInG0">G0坐标</param>
        /// <returns>结果</returns>
        private ExcuteResult MoveToSearchPosAndWaitSignal(AKRSPoint3D posInG0)
        {
            this.bondModuleController.MoveToG0Pos(posInG0);

            System2RunTimeProvider.RecordTime("取片测试", $" 去BOND相机搜晶位置完成");

            System2RunTimeProvider.IsBondArriveSearchPos = true;

            // 晶圆上料准备判断（从信号池获取）,这个信号会自动复位
            if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
            {
                System2RunTimeProvider.RecordTime("取片测试", $" 等晶圆允许取料信号失败！");

                return ExcuteResult.Abort;
            }

            System2RunTimeProvider.RecordTime("取片测试", $" 等晶圆允许取料信号成功！");

            this.bondHeadController.MoveBondZToSafePos();

            System2RunTimeProvider.RecordTime("取片测试", $" BondZ轴去安全位！");

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 去翻转台取片
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public bool PickupFromFlipTable(BaseCarrierConfig component)
        {
            // 去避让位
            this.bondModuleController.MoveToSafePos();

            #region 吸嘴堵塞判断

            if (component.IsActiveComponentDetection
                && this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
            {
                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"吸嘴检测到有芯片残留!\r\n",
                    "报警",
                    new string[] { "忽略", "抛料" },
                    new DialogResult[] { DialogResult.Yes, DialogResult.No },
                    AlarmLevel.SecondLevel);
                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        break;

                    case DialogResult.No:
                        this.bondModuleController.ThrowAction();
                        break;
                    default:
                        break;
                }
            }

            // 关闭吸嘴真空
            this.bondHeadController.CloseToolVaccum();

            #endregion

            // 判断反转台是否有芯片
            if (Static.IsComponentOnFlipTool == false)
            {
                // 开搜晶线程
                WaferSystemDomain.GetInstance().WaferSubSystemTask.Start();

                // 等晶圆允许取料信号
                if (!SignalPool.GetInstance().IsWaferAllowPickSignal.Wait())
                {
                    return false;
                }

                ExcuteResult ret = WaferSubController.GetInstance().FlipChipAction(component);

                if (ret != ExcuteResult.Success)
                {
                    return false;
                }

                if (component.CarrierType == CarrierTypeEnum.Wafer)
                {
                    // 顶针缩回
                    WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                }
            }

            #region 取料

            // 翻转台位置判断
            if (WaferSubController.GetInstance().FlipController.IsFlipTableAtTransferPos() == false)
            {
                //DialogResult dialog = AKRSXtraMessageBox.Show(
                //    $"翻转台不在交接位置，焊头取片失败!",
                //    "报警",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Warning);

                //return false;

                // 去交接位置
                WaferSubController.GetInstance().FlipController.MoveFlipToTransferPos();
            }

            // 获取当前吸嘴
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            // 获取芯片定位角度
            double componentAngle = WaferSystemDomain.GetInstance().Block.GetBondSpinAngle();

            #endregion

            // 计算取晶位
            // 翻转芯片不需要BOND相机定位，所以最后一个参数传null
            AKRSPoint4D curPickPos = this.CalculatePickPos(component, componentAngle, null);

            // 移动到取晶位
            this.bondModuleController.MoveSafeBondXY(curPickPos.X, curPickPos.Y);

            // 焊头旋转到取料角度
            this.bondHeadController.RotateAxisT(curPickPos.T);

            if (component.PickupForceMode == ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(component.PickupForce);

                this.bondHeadController.ZeroBondhead(isSmallForce);
            }

            // 修改拾取类型
            PickTypeEnum pickType = PickTypeEnum.FlipTable;

        Repick:

            // 执行Pick方法
            bool res = this.bondHeadController.PickAction(
                curPickPos.Z,
                component,
                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                pickType);

        #region 漏晶检测

        ReCheckVacuum:

            if (component.IsActiveComponentDetection)
            {
                // 检测真空
                if (!this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle()))
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                        $"取片失败! \r\n" + "重试：重新检测真空\r\n"
                                      + "终止：退出工作\r\n"
                                      + "忽略：忽略此异常\r\n"
                                      + "重取: 重取此颗芯片\r\n",
                        "报警",
                        new string[] { "重试", "终止", "忽略", "重取" },
                        new DialogResult[]
                        {
                        DialogResult.Retry, DialogResult.Abort, DialogResult.Ignore, DialogResult.Cancel,
                        },
                        AlarmLevel.SecondLevel);

                    switch (dialogResult)
                    {
                        case DialogResult.Retry:
                            goto ReCheckVacuum;

                        case DialogResult.Abort:
                            return false;

                        case DialogResult.Ignore:
                            break;

                        case DialogResult.Cancel:
                            goto Repick;
                    }
                }
            }

            #endregion

            // 芯片转正
            this.bondHeadController.RotateAxisT(nozzle.AlignAngle + component.RotaryPosition);

            return true;
        }

        /// <summary>
        /// 取片前准备
        /// </summary>
        /// <param name="component">芯片</param>
        public void PrepareBeforePick(BaseCarrierConfig component)
        {
            if (component.IsOpenWaferTableVacuumBeforePick)
            {
                // 只控制静态华夫盒真空，晶圆台的真空改到晶圆线程控制
                switch (component.CarrierType)
                {
                    //case CarrierTypeEnum.Wafer:
                    //    WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
                    //    break;
                    case CarrierTypeEnum.StaticWaffle:
                        WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
                        break;
                    //case CarrierTypeEnum.Waffle:
                    //    WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
                    //    break;
                }

                // 开真空延时
                Thread.Sleep(component.OpenWaferTableVacuumBeforePickDelay);
            }
            else
            {
                switch (component.CarrierType)
                {
                    //case CarrierTypeEnum.Wafer:
                    //    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                    //    break;
                    case CarrierTypeEnum.StaticWaffle:
                        WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                        break;
                    //case CarrierTypeEnum.Waffle:
                    //    WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    //    break;
                }
            }

            System2RunTimeProvider.IsBondArriveSearchPos = false;
        }

        /// <summary>
        /// 在刮胶盘蘸胶
        /// </summary>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public ExcuteResult DipFluxOnFluxer(BaseCarrierConfig component)
        {
            this.bondModuleController.MoveToSafePos();

            this.slideFluxerController.SlideFluxerOutWaitArrive();

            // 蘸胶位
            AKRSPoint3D dipFluxPos =
                this.bondModuleController.ConvertG0ToMachinePos(
                    this.bondDevicePara.SlideFluxerParam.SlideFluxerPos);

            // 实际蘸胶位置要加上吸嘴高度和芯片厚度
            dipFluxPos.Z = dipFluxPos.Z + this.bondHeadController.GetCurrentNozzle().MeasureHeightOffset
                                        + component.ComponentThickness;

            // 去刮胶盘
            this.bondModuleController.MoveBondXY(dipFluxPos.X, dipFluxPos.Y);

            // 蘸胶
            this.DipFlux(dipFluxPos.Z, component);

            // 轴抬到安全高度
            this.bondHeadController.MoveBondZToSafePos();

            // 刮胶盘收回
            this.slideFluxerController.SlideFluxerHomeWaitArrive();

            return ExcuteResult.Success;
        }

        /// <summary>
        /// 贴片测试
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="substrateIndex">基板号</param>
        /// <param name="moduleIndex">基岛号</param>
        /// <param name="bondPositionName">焊点名</param>
        /// <param name="bondType">固晶类型</param>
        /// <returns>结果</returns>
        public ExcuteResult PlacementTest(
            BaseCarrierConfig component,
            int substrateIndex,
            int moduleIndex,
            string bondPositionName,
            BondTypeEnum bondType = BondTypeEnum.BondOnTU)
        {
            #region 流道检查

            TransportUnit transportUnit = TUAssistantHelper.JudgeTuExist();

            if (transportUnit == null)
            {
                return ExcuteResult.Abort;
            }

            #endregion

            #region 按照配置执行定位

            Substrate substrate = transportUnit.Substrates.Find(it => it.Index == substrateIndex);

            if (!System2Domain.GetInstance().System2MatterVision(substrate))
            {
                return ExcuteResult.Abort;
            }

            Module module = substrate.Modules.Find(it => it.Index == moduleIndex);

            if (!System2Domain.GetInstance().System2MatterVision(module))
            {
                return ExcuteResult.Abort;
            }

            BondPosition bondPosition =
                module.BondPositions.Find(it => it.Name == bondPositionName);

            if (!System2Domain.GetInstance().System2MatterVision(bondPosition))
            {
                return ExcuteResult.Abort;
            }

            #endregion

            #region 蘸胶

            if ((bondType == BondTypeEnum.DipFluxOnTU || bondType == BondTypeEnum.BondOnTU) && component.DipMode == DipModeEnum.BeforeAccuracyMode)
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                $"请选择此芯片是否需要先去刮胶盘蘸胶？",
                    "提示",
                    new string[] { "是", "否" },
                    new DialogResult[] { DialogResult.Yes, DialogResult.No },
                    AlarmLevel.SecondLevel);

                if (dialog == DialogResult.Yes)
                {
                    // 蘸胶
                    this.DipFluxOnFluxer(component);

                    // 漏晶检测
                    if (component.IsActiveComponentDetection)
                    {
                    ReCheck:
                    bool hasComponent =
                        this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle());

                        if (hasComponent == false)
                        {
                            dialog = AKRSXtraMessageBox.Show(
                  $"蘸胶完成后检测到焊头上没有芯片，请人工确认！\r\nAbort:退出\r\nRetry:重新检测\r\nIgnore:忽略",
                      "提示",
                      MessageBoxButtons.AbortRetryIgnore,
                      MessageBoxIcon.Information);

                            switch (dialog)
                            {
                                case DialogResult.Abort:
                                    return ExcuteResult.Abort;

                                case DialogResult.Retry:
                                    goto ReCheck;

                                case DialogResult.Ignore:
                                    break;
                            }
                        }
                    }
                }
            }

            #endregion

            #region 上视/下视纠偏

            if (component.AccuracyMode != AccuracyModeEnum.Off)
            {
                switch (component.AdjustCamera)
                {
                    // 上视
                    case CameraTypeEnum.UpLookCamera:

                        // 上视流程
                        (ExcuteResult excuteResult, AKRSPoint4D offset) res = this.UplookVisionAction(
                            bondPosition,
                            component);

                        if (res.excuteResult != ExcuteResult.Success)
                        {
                            return res.excuteResult;
                        }

                        break;

                    //// 中转台
                    //case CameraTypeEnum.BondCamera:

                    default:
                        // 暂未开发
                        throw new Exception("模拟贴片仅支持上视纠偏，请检查芯片定位配置！");
                }
            }

            #endregion

            #region 蘸胶

            if ((bondType == BondTypeEnum.DipFluxOnTU || bondType == BondTypeEnum.BondOnTU) && component.DipMode == DipModeEnum.AfterAccuracyMode)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                $"请选择此芯片是否需要先去刮胶盘蘸胶？",
                    "提示",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (dialog == DialogResult.Yes)
                {
                    // 蘸胶
                    this.DipFluxOnFluxer(component);

                    // 漏晶检测
                    if (component.IsActiveComponentDetection)
                    {
                    ReCheck:
                        bool hasComponent = this.bondHeadController.ComponentCheckAfterPickup(this.bondHeadController.GetCurrentNozzle());

                        if (hasComponent == false)
                        {
                            dialog = AKRSXtraMessageBox.Show(
                  $"蘸胶完成后检测到焊头上没有芯片，请人工确认！\r\nAbort:退出\r\nRetry:重新检测\r\nIgnore:忽略",
                      "提示",
                      MessageBoxButtons.AbortRetryIgnore,
                      MessageBoxIcon.Information);

                            switch (dialog)
                            {
                                case DialogResult.Abort:
                                    return ExcuteResult.Abort;

                                case DialogResult.Retry:
                                    goto ReCheck;

                                case DialogResult.Ignore:
                                    break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 贴片

            TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit =
                TransportProgram.GetInstance().BondSubSectionProgram.TransportUnit;

            // 贴片硬补偿
            AKRSPoint3D bondPosOffset = new AKRSPoint3D(bondPosition.BondPositionInfo.ComponentOffSet.X, bondPosition.BondPositionInfo.ComponentOffSet.Y, 0);

            // 焊点位置
            AKRSPoint3D bondPositionInG0 = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            // 硬补偿随角度旋转，焊点位置加硬补偿
            AKRSPoint3D bondPosAddOffset = bondPosition.CoordinateSystem.SelfPosToG0(bondPosOffset);

            // 最终固晶位
            AKRSPoint3D realBondPos = this.bondModuleController.ConvertG0ToMachinePos(bondPosAddOffset + bondPosition.BondPositionInfo.ComponentOffSet);

            // 移动到安全高度
            System2Domain.GetInstance().BondHeadController.MoveBondZToSafePos();

            // 移动XY，暂时不转角度
            this.bondModuleController.MoveSafeBondXY(realBondPos.X, realBondPos.Y);

            // 焊头清零
            if (component.BondingForceMode != ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(component.BondingForce);

                this.bondHeadController.ZeroBondhead(isSmallForce);
            }

            #region 固精

            double bondLevel;

        ReBond:

            // 力控模式不用这个硬补偿
            double bondingDistance =
                component.BondingForceMode == ForceModeEnum.Distance ? component.BondingDistance : 0;

            // 计算最终固晶高度=焊点示教高度+当前吸嘴测高补偿+芯片厚度+焊点贴片补偿
            bondLevel = this.bondModuleController.ConvertG0ToMachinePos(bondPositionInG0).Z
                        + this.bondHead.CurrentNozzle.MeasureHeightOffset + component.ComponentThickness
                        + bondPosition.SingleBondPositionConfig.BondPosOffset.Z + bondingDistance;

            // 执行固晶动作
            bool ret = this.bondHeadController.BondAction(
                bondLevel - 0.01,
                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                component,
                bondType);

            if (ret == false)
            {
                DialogResult dialog = AKRSMessageBoxExt.ShowWarn(
                    $"固晶失败!",
                    "报警",
                    new string[] { "重试", "忽略" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore },
                    AlarmLevel.SecondLevel);

                switch (dialog)
                {
                    // 重新Bond
                    case DialogResult.Retry:
                        goto ReBond;

                    // 忽略
                    case DialogResult.Ignore:
                        break;
                }

                return ExcuteResult.Abort;
            }

            #endregion

            // 焊后回带检测
            if (component.IsActiveComponentDetection && bondType != BondTypeEnum.DipFluxOnTU)
            {
                if (this.bondHeadController.ComponentCheckAfterBonding(this.bondHeadController.GetCurrentNozzle())
                    == false) 
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                        $"吸嘴上检测到仍有芯片残留 !\r\n",
                        "报警",
                        new string[] { "忽略", "抛料" },
                        new DialogResult[] { DialogResult.Yes, DialogResult.No },
                        AlarmLevel.SecondLevel);

                    switch (dialogResult)
                    {
                        case DialogResult.Yes:
                            break;

                        case DialogResult.No:
                            this.bondModuleController.ThrowAction();
                            break;

                        default:
                            break;
                    }
                }
            }

            #endregion

           // 去看蘸胶印子
           this.bondModuleController.CameraMoveToG0Pos(bondPositionInG0);

           return ExcuteResult.Success;
        }

        /// <summary>
        /// 上视定位流程
        /// </summary>
        /// <param name="bondPosition">焊点</param>
        /// <param name="component">芯片</param>
        /// <returns>结果</returns>
        public (ExcuteResult, AKRSPoint4D) UplookVisionAction(BondPosition bondPosition, BaseCarrierConfig component)
        {
            #region 去上视

            // 计算真实拍照位
            double tuDegree = bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI;
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            // 旋转的角度为吸嘴的矫正角度 + 焊点的角度 + 焊点的硬补偿
            double angle = nozzle.AlignAngle + tuDegree + component.RotaryPosition
                           + bondPosition.SingleBondPositionConfig.RotaryPosition;

            // 拍照位和吸嘴绑定，不同的吸嘴高度不同
            AKRSPoint3D p1RealVisionPos = component.UpLookAdjustConfig.P1VisionPos + new AKRSPoint3D(
                                              0,
                                              0,
                                              nozzle.MeasureHeightOffset);

            AKRSPoint3D targetPos = this.bondModuleController.ConvertG0ToMachinePos(p1RealVisionPos);

            AKRSPoint3D targetPos2 = new AKRSPoint3D(CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos.X, CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos.Y, targetPos.Z);

            // 上视的位置
            AKRSPoint3D upLookPos = new AKRSPoint3D(targetPos2.X, targetPos2.Y, targetPos2.Z);

            // 如果不是快速定位就去P1拍照位
            if (!System2Configuration.GetInstance().UpLookQuickPositioning)
            {
                upLookPos = new AKRSPoint3D(targetPos.X, targetPos.Y, targetPos.Z);
            }

            ExcuteResult ret;

            // 去上视
            this.bondModuleController.MoveSafeBondXYZ(upLookPos);
            this.bondHeadController.RotateAxisT(angle);

            #endregion

            // 一点定位
            if (!component.IsTwoPointAdjust)
            {
                //  double tuDegree2 = this.bondPosition.CoordinateSystem.DegreeInG0() * 180 / Math.PI;

                System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第一次定位");

                (ExcuteResult result, MatchResult matchResult) result1 = this.UpLookVisionAsissitant(
                    component,
                    component.UpLookAdjustConfig.P1VisionPos,
                    component.UpLookAdjustConfig.P1PRName);

                System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位第一次定位完成");

                if (result1.result != ExcuteResult.Success)
                {
                    return (result1.result, null);
                }

                // 保存
                bondPosition.BondPositionInfo.UpLookMatchResult1 = result1.matchResult;

                System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第二次定位");

                AKRSPoint3D point3D = this.bondModuleController.ConvertMachineToG0Pos(CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos);

                if (System2Configuration.GetInstance().UpLookQuickPositioning)
                {
                    // 当前芯片的真实角度 = 拍照的结果角度 - 示教时的角度
                    double currentComponentAngle = result1.matchResult.Angle
                                                   - component.UpLookAdjustConfig
                                                       .AngleForVisionCenterAndComponentCenter;

                    // 芯片中心的实际位置 = XY的偏移量 - 芯片中心绕视觉中心旋转之后的位置
                    AKRSPoint3D akrsPoint3D = this.upLookController.ConvertPixelToDistance(result1.matchResult) - MathHelper.RotateCenter(
                                              component.UpLookAdjustConfig
                                                  .DistanceForVisionCenterAndComponentCenter,
                                              new AKRSPoint3D(),
                                              currentComponentAngle * Math.PI / 180.0 * -1.0);

                    // 当前芯片的真实位置（相对于焊头） = 当前的位置 - 上视看到旋转中心的实际位置 + 芯片实际位置
                    AKRSPoint3D currentComponentOffset =
                        this.bondModuleController.Get3DRealPosition()
                        - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos + akrsPoint3D;

                    // 结果保存
                   bondPosition.BondPositionInfo.ComponentAngleOffSet = result1.matchResult.Angle;
                   bondPosition.BondPositionInfo.ComponentOffSet = currentComponentOffset;

                    // 一次定位
                    return (result1.result, new AKRSPoint4D(bondPosition.BondPositionInfo.ComponentOffSet.X, bondPosition.BondPositionInfo.ComponentOffSet.Y, bondPosition.BondPositionInfo.ComponentOffSet.Z, bondPosition.BondPositionInfo.ComponentAngleOffSet));
                }
                else
                {
                    System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第二次定位");

                    // T轴转到定位角度
                    // 旧
                    //this.bondHeadController.RelativeRotateAxisT(result1.matchResult.Angle + tuDegree + bondPosition.SingleBondPositionConfig.RotaryPosition);

                    double bpAngle =bondPosition.IsMirror
                                         ? 180.0 - bondPosition.GetRotaryCompensate()
                                         : bondPosition.GetRotaryCompensate();


                    // T轴转到定位角度
                    this.bondHeadController.RelativeRotateAxisT(result1.matchResult.Angle + tuDegree + bpAngle);

                    System2RunTimeProvider.RecordTime("UpLookAction", $"T轴旋转到焊点角度完成");

                    // 第二次定位
                    (ExcuteResult result, MatchResult matchResult) result2 = this.UpLookVisionAsissitant(
                        component,
                        component.UpLookAdjustConfig.P1VisionPos,
                        component.UpLookAdjustConfig.P1PRName);

                    System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位第二次定位完成");

                    System2RunTimeProvider.RecordTime(
                        "UpLookAction",
                        $"上视旋转角度后定位完成，定位结果:X :{result2.matchResult.CenterX},Y :{result2.matchResult.CenterY},角度 :{result2.matchResult.Angle}");

                    if (result2.result != ExcuteResult.Success)
                    {
                        return (result2.result, null);
                    }

                    // 由于提前转好了角度，则不需要考虐旋转带来的偏移
                    // 芯片距离旋转中的距离为 = 视觉定位结果 + 此时的位置 - 上视的固定位置 - 视觉位置和芯片中心的差值
                   bondPosition.BondPositionInfo.ComponentOffSet =
                        this.upLookController.ConvertPixelToDistance(result2.matchResult)
                        + this.bondModuleController.Get3DRealPosition()
                        - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos
                        - component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter;

                   bondPosition.BondPositionInfo.ComponentAngleOffSet =
                       -(tuDegree + bondPosition.GetRotaryCompensate());
                }
            }
            else
            {
                // P1点定位
                System2RunTimeProvider.RecordTime("UpLookAction", $"上视定位准备开始第一次定位");

                (ExcuteResult result, MatchResult matchResult) result1 = this.UpLookVisionAsissitant(
                    component,
                    component.UpLookAdjustConfig.P1VisionPos,
                    component.UpLookAdjustConfig.P1PRName);

                AKRSPoint3D visionPos1 = this.bondModuleController.Get3DRealPosition();

                System2RunTimeProvider.RecordTime(
                    "UpLookAction",
                    $"上视定位第一次定位完成，定位结果:X :{result1.matchResult.CenterX},Y :{result1.matchResult.CenterY},角度 :{result1.matchResult.Angle}");

                if (result1.result != ExcuteResult.Success || result1.matchResult == null)
                {
                    return (result1.result, null);
                }

                // 去P2拍照位
                AKRSPoint3D p2RealVisionPos = component.UpLookAdjustConfig.P2VisionPos + new AKRSPoint3D(
                                                  0,
                                                  0,
                                                  nozzle.MeasureHeightOffset);

                // 直接移动XY
                this.bondModuleController.MoveBondXYToG0Pos(p2RealVisionPos.X, p2RealVisionPos.Y);

                (ExcuteResult result, MatchResult matchResult) result2 = this.UpLookVisionAsissitant(
                    component,
                    component.UpLookAdjustConfig.P2VisionPos,
                    component.UpLookAdjustConfig.P2PRName);

                AKRSPoint3D visionPos2 = this.bondModuleController.Get3DRealPosition();

                System2RunTimeProvider.RecordTime(
                    "UpLookAction",
                    $"上视定位P2定位完成，定位结果:X :{result2.matchResult.CenterX},Y :{result2.matchResult.CenterY},角度 :{result2.matchResult.Angle}");

                if (result2.result != ExcuteResult.Success || result2.matchResult == null)
                {
                    return (result2.result, null);
                }

                if (System2Configuration.GetInstance().UpLookQuickPositioning)
                {
                    // 当前芯片的真实角度 = 拍照的结果角度 - 示教时的角度
                    double currentComponentAngle = (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0
                                                   - component.UpLookAdjustConfig
                                                       .AngleForVisionCenterAndComponentCenter;

                    // 芯片中心的实际位置 = 芯片中心绕视觉中心旋转之后的位置 + XY的偏移量
                    AKRSPoint3D point3D =
                        (this.upLookController.ConvertPixelToDistance(result1.matchResult)
                         + this.upLookController.ConvertPixelToDistance(result2.matchResult)) / 2.0
                        - MathHelper.RotateCenter(
                            component.UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter,
                            new AKRSPoint3D(),
                            currentComponentAngle * Math.PI / 180.0 * -1.0);

                    // 当前芯片的真实位置（相对于焊头） = 当前的位置 -上视看到旋转中心的实际位置 + 芯片实际位置
                    AKRSPoint3D currentComponentOffset =
                        (visionPos2 + visionPos1) / 2.0
                        - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos + point3D;

                    // 结果保存
                    bondPosition.BondPositionInfo.ComponentAngleOffSet = (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0;
                    bondPosition.BondPositionInfo.ComponentOffSet = currentComponentOffset;
                }
                else
                {
                    // T轴转到定位角度
                    this.bondHeadController.RelativeRotateAxisT(
                        (result1.matchResult.Angle + result2.matchResult.Angle) / 2.0 + tuDegree
                        + bondPosition.SingleBondPositionConfig.RotaryPosition);

                    // 直接移动XY
                    this.bondModuleController.MoveBondXYToG0Pos(p1RealVisionPos.X, p1RealVisionPos.Y);

                    // 第二次定位
                    (ExcuteResult result, MatchResult matchResult) result3 = this.UpLookVisionAsissitant(
                        component,
                        component.UpLookAdjustConfig.P1VisionPos,
                        component.UpLookAdjustConfig.P1PRName);

                    if (result3.result != ExcuteResult.Success)
                    {
                        return (result3.result, null);
                    }

                    visionPos1 = this.bondModuleController.Get3DRealPosition();

                    AKRSPoint3D point3D1 = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(
                        this.bondModuleController.Get3DRealPosition(),
                        result3.matchResult);

                    // 直接移动XY
                    this.bondModuleController.MoveBondXYToG0Pos(p2RealVisionPos.X, p2RealVisionPos.Y);

                    (ExcuteResult result, MatchResult matchResult) result4 = this.UpLookVisionAsissitant(
                        component,
                        component.UpLookAdjustConfig.P2VisionPos,
                        component.UpLookAdjustConfig.P2PRName);

                    if (result4.result != ExcuteResult.Success)
                    {
                        return (result4.result, null);
                    }

                    visionPos2 = this.bondModuleController.Get3DRealPosition();

                    System2RunTimeProvider.RecordTime(
                        "UpLookAction",
                        $"上视旋转角度后定位完成，定位结果:X :{result4.matchResult.CenterX},Y :{result4.matchResult.CenterY},角度 :{result4.matchResult.Angle}");

                    // 由于提前转好了角度，则不需要考虑旋转带来的偏移
                    // 芯片距离旋转中的距离为 = 视觉定位结果 + 此时的位置 - 上视的固定位置 - 视觉位置和芯片中心的差值
                bondPosition.BondPositionInfo.ComponentOffSet =
                        (this.upLookController.ConvertPixelToDistance(result3.matchResult)
                         + this.upLookController.ConvertPixelToDistance(result4.matchResult)) / 2.0
                        + (visionPos1 + visionPos2) / 2.0
                        - CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos - component
                            .UpLookAdjustConfig.DistanceForVisionCenterAndComponentCenter;

                   bondPosition.BondPositionInfo.ComponentAngleOffSet =
                        -(tuDegree + bondPosition.GetRotaryCompensate());
                }
            }

            Task.Run(
                () =>
                {
                    // 上视定位完关闭灯光
                    this.upLookController.CloseLight();
                });

            AKRSPoint4D offset = new AKRSPoint4D(
                bondPosition.BondPositionInfo.ComponentOffSet.X,
                bondPosition.BondPositionInfo.ComponentOffSet.Y,
                bondPosition.BondPositionInfo.ComponentOffSet.Z,
                0);

            System2RunTimeProvider.RecordTime("UplookAction", "Action 完成");

            return (ExcuteResult.Success, offset);
        }

        /// <summary>
        /// 上视单次拍照
        /// </summary>
        /// <param name="component">点位</param>
        /// <param name="visionPoint3D">位置</param>
        /// <param name="name">定位的名称</param>
        /// <returns>定位结果</returns>
        public (ExcuteResult, MatchResult) UpLookVisionAsissitant(BaseCarrierConfig component, AKRSPoint3D visionPoint3D, string name)
        {
            // 寻找Pr模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);

            if (pREntity == null)
            {
                AKRSMessageBoxExt.Show(
                    $" 上视定位流程中发现模板： {name} 不存在,将退出自动工作！\r\n",
                    "异常",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                return (ExcuteResult.Exception, null);
            }

            System2RunTimeProvider.RecordTime("UpLookAction", $"开始设置上视相机硬件");

            // 设置硬件
            this.SetHardware(name, CameraTypeEnum.UpLookCamera);

            System2RunTimeProvider.RecordTime("UpLookAction", $"设置上视相机硬件完成");

            Stopwatch sp = Stopwatch.StartNew();

            sp.Restart();

            // 拍照停留
            DelayHelper.Delay(component.AdjustVisionDelay);

            // 开始定位(默认不频闪)
            ExcuteResult p1Result = pREntity.DoWork();

            sp.Stop();
            long time1 = sp.ElapsedMilliseconds;

            LogHelper.Post(Level.Info, $" 上视{name}定位完成，用时{time1} ms", LogCategory.Bond);

            // 处理拍照完成后的结果
            if (p1Result != ExcuteResult.Success || pREntity.AlgResult.IsSuccess == false)
            {
                // 报警
                (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmFunc(
                    pREntity,
                    "上视定位失败",
                    "上视定位");

                switch (result.dialog)
                {
                    case DialogResult.OK:
                        return (ExcuteResult.Success, (MatchResult)result.matchResult);

                    case DialogResult.Abort:
                        StatisticsDomain.GetInstance().PostBondFailedCount++;
                        Machine.GetInstance().Stop();
                        return (ExcuteResult.Abort, null);

                    case DialogResult.Ignore:
                        return (ExcuteResult.Fail, new MatchResult());

                    // 重取，这里是取下一颗不是当前颗
                    case DialogResult.Retry:

                        System2RunTimeProvider.RecordTime("取片信号交互", $"上视定位失败，点击重取！");

                        if (component.IsUseFlipTable && component.CarrierType == CarrierTypeEnum.Wafer)
                        {
                            // 顶针缩回
                            WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();
                        }

                        // 刷新Map信息
                        WaferSystemDomain.GetInstance().Block.RefreshMap();

                        // 当前种类芯片名称传给晶圆台
                        WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

                        // 给晶圆台发要料信号
                        SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                        System2RunTimeProvider.RecordTime("取片信号交互", $"给晶圆发要料信号，芯片名称{component.Name}");

                        // 重新设置灯光
                        pREntity.SetLight();

                        System2RunTimeProvider.RecordTime("上视流程", $"重新设置灯光完成");

                        this.bondModuleController.ThrowAction();

                        System2RunTimeProvider.RecordTime("上视流程", $"抛料完成");

                        // 抛掉的芯片+1
                        component.AddCountOfReject();

                        System2RunTimeProvider.IsRepickComponent = true;

                        Thread.Sleep(20);

                        return (ExcuteResult.Retry, null);
                }
            }

            //if (System2Configuration.GetInstance().IsActiveSaveImg)
            //{
            //    System2RunTimeProvider.RecordTime("UpLookAction", $"发送存图指令完成");

            //    Task.Run(
            //        () =>
            //        {
            //            sp.Restart();

            //            // 保存结果图片
            //            VisionService.SaveVisionImage(
            //                pREntity.AlgResult.OutPutImg1,
            //                new List<string>() { "Bond", "示教", pREntity.GetName() },
            //                pREntity.GetName(),
            //                pREntity.OriginalBmp);

            //            sp.Stop();
            //            long time2 = sp.ElapsedMilliseconds;

            //            LogHelper.Post(Level.Info, $" 上视{name}模板，存图用时{time2} ms", LogCategory.Bond);
            //        });
            //}

            MatchResult matchResult = (MatchResult)pREntity.AlgResult;

            return (ExcuteResult.Success, matchResult);
        }

        ///// <summary>
        ///// 上视拍照
        ///// </summary>
        ///// <param name="visionPoint3D">点位</param>
        ///// <param name="name">定位的名称</param>
        ///// <returns>定位结果</returns>
        //public (ExcuteResult, MatchResult) UpLookVisionAsissitant(BaseCarrierConfig component, AKRSPoint3D visionPoint3D, string name)
        //{
        //    // 寻找Pr模板
        //    PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);

        //    if (pREntity == null)
        //    {
        //        AKRSMessageBoxExt.Show(
        //            $" 上视定位流程中发现模板： {name} 不存在,将退出自动工作！\r\n",
        //            "异常",
        //            new string[] { "确认" },
        //            new DialogResult[] { DialogResult.Abort },
        //            AlarmLevel.SecondLevel);

        //        return (ExcuteResult.Exception, null);
        //    }

        //    System2RunTimeProvider.RecordTime("UpLookAction", $"开始设置上视相机硬件");

        //    // 设置硬件
        //    this.SetHardware(name, CameraTypeEnum.UpLookCamera);

        //    System2RunTimeProvider.RecordTime("UpLookAction", $"设置上视相机硬件完成");

        //    Stopwatch sp = Stopwatch.StartNew();

        //    sp.Restart();

        //    // 拍照停留
        //    DelayHelper.Delay(component.AdjustVisionDelay);

        //    // 开始定位(默认不频闪)
        //    ExcuteResult p1Result = pREntity.DoWork();

        //    sp.Stop();
        //    long time1 = sp.ElapsedMilliseconds;

        //    LogHelper.Post(Level.Info, $" 上视{name}定位完成，用时{time1} ms", LogCategory.Bond);

        //    // 处理拍照完成后的结果
        //    if (p1Result != ExcuteResult.Success || pREntity.AlgResult.IsSuccess == false)
        //    {
        //        // 报警
        //        (DialogResult dialog, BaseAlgResult matchResult) result = UcMainSystem.VisionAlarmFunc(
        //            pREntity,
        //            "上视定位失败",
        //            "上视定位");

        //        if (result.dialog == DialogResult.Abort)
        //        {
        //            Machine.GetInstance().Stop();
        //            return (ExcuteResult.Abort, null);
        //        }
        //        else if (result.dialog == DialogResult.Ignore)
        //        {
        //            return (ExcuteResult.Fail, new MatchResult());
        //        }
        //        else if (result.dialog == DialogResult.OK)
        //        {
        //            return (ExcuteResult.Success, (MatchResult)result.matchResult);
        //        }
        //    }


        //    MatchResult matchResult = (MatchResult)pREntity.AlgResult;

        //    return (ExcuteResult.Success, matchResult);
        //}

        /// <summary>
        /// 计算取料位
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="componentAngle">芯片角度</param>
        /// <param name="matchResult">芯片定位结果</param>
        /// <returns>取料位</returns>
        public AKRSPoint4D CalculatePickPos(BaseCarrierConfig component, double componentAngle, MatchResult matchResult)
        {
            // 最终取晶位
            AKRSPoint3D curPickPos = new AKRSPoint3D();

            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            double pickAngle;
            if (component.IsUseFlipTable)
            {
                // T轴旋转角度;取片硬补偿+芯片定位角度+吸嘴示教角度
                pickAngle = component.PickAngleOffset + componentAngle
                              + nozzle.AlignAngle;
            }
            else
            {
                // T轴旋转角度;取片硬补偿-芯片定位角度+吸嘴示教角度
                pickAngle = component.PickAngleOffset - componentAngle
                                       + nozzle.AlignAngle;
            }

            // 计算焊头旋转后的吸嘴偏移
            AKRSPoint2D nozzleOffsetRotated =
                this.bondHeadController.GetNozzleOffset(nozzle.Name, pickAngle - nozzle.AlignAngle);

            AKRSPoint2D pickPos = CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC;

            double readyBondPickPosition = 0;

            // 力控模式不用这个硬补偿
            double pickUpDistance =
                component.PickupForceMode == ForceModeEnum.Distance ? component.PickupDistance : 0;

            // 计算取片位
            switch (component.CarrierType)
            {
                case CarrierTypeEnum.Wafer:

                    #region 晶圆

                    // 获取晶圆对象
                    CarrierWithWaferConfig carrierWithWafer =
                        (CarrierWithWaferConfig)CarrierConfigRepository.GetInstance().Find(component.Name);

                    // 获取顶针对象
                    EjectionConfig ejectionConfig =
                        (EjectionConfig)EjectionConfigRepository.GetInstance().Find(carrierWithWafer.EjectionName);

                    // 获取顶针中心和晶圆相机中心的偏移
                    AKRSPoint3D ejectionCenterToWaferCameraCenterDistance = MachineStateModel.GetInstance().IsCompensateWork ? new AKRSPoint3D() : Block.GetInstance().GetPickOffset();

                    if (component.IsUseFlipTable == false)
                    {
                        // 顶针预取料高度（顶针帽测高高度）转Bond坐标系
                        readyBondPickPosition =
                            this.bondModuleController.ConvertG0ToMachinePos(ejectionConfig.ReadyBondPickPosition).Z;

                        // 最终取晶位=Bond取料位-吸嘴偏移+顶针中心和晶圆相机中心的偏差+芯片取料补偿
                        curPickPos.X = pickPos.X - nozzleOffsetRotated.X
                                       + ejectionCenterToWaferCameraCenterDistance.X
                                       + component.PickupOffset.X /*+ nearestOffset.Dx*/;

                        curPickPos.Y = pickPos.Y - nozzleOffsetRotated.Y
                                       + ejectionCenterToWaferCameraCenterDistance.Y
                                       + component.PickupOffset.Y/* + nearestOffset.Dy*/;

                        // 计算最终取晶高度:顶针帽测高高度+顶针台相对高度（顶针台工作位置-顶针台测高高度）+吸嘴相对标准吸嘴补偿+芯片和蓝膜厚度+取料补偿
                        curPickPos.Z = readyBondPickPosition
                                       + (carrierWithWafer.EjectionTableWorkPosition
                                          - ejectionConfig.EjectionTableMeasureHeightPosition).Z
                                       + nozzle.MeasureHeightOffset + carrierWithWafer.ComponentThickness
                                       + carrierWithWafer.CarrierThickness + pickUpDistance;
                    }
                    else
                    {
                        // 获取翻转工具
                        FlipTool flipTool =
                            WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                        // 最终取晶位=翻转台工具示教取片位置-吸嘴偏移+芯片取料补偿
                        // 第三项正负待定
                        curPickPos.X = flipTool.FlipToolPositionXY.X - nozzleOffsetRotated.X
                                       + component.PickupOffset.X;

                        curPickPos.Y = flipTool.FlipToolPositionXY.Y - nozzleOffsetRotated.Y
                                       + component.PickupOffset.Y;

                        // 计算最终取晶高度:翻转台工具示教取片高度+吸嘴相对标准吸嘴补偿+取料补偿+芯片厚度
                        curPickPos.Z = flipTool.FlipToolPositionZ + nozzle.MeasureHeightOffset + pickUpDistance + component.ComponentThickness;
                    }

                    break;

                #endregion

                case CarrierTypeEnum.Waffle:

                    #region 华夫盒

                    // 获取华夫盒对象
                    CarrierWithWaffleConfig carrierWithWaffleConfig =
                        (CarrierWithWaffleConfig)CarrierConfigRepository.GetInstance().Find(component.Name);

                    if (component.IsUseFlipTable == false)
                    {
                        // 华夫盒示教高度转Bond坐标系
                        readyBondPickPosition =
                            this.bondModuleController.ConvertG0ToMachinePos(carrierWithWaffleConfig.ReadyBondPickPosition)
                                .Z;

                        // 最终取晶位=Bond取料位+吸嘴偏移+芯片取料补偿
                        // 计算最终取晶高度=华夫盒示教高度+取料补偿/*+芯片厚度*//*+吸嘴测高补偿*/
                        // 华夫盒用吸嘴测高
                        // 在芯片上测高不加芯片厚度
                        curPickPos.X = pickPos.X - nozzleOffsetRotated.X + component.PickupOffset.X;
                        curPickPos.Y = pickPos.Y - nozzleOffsetRotated.Y + component.PickupOffset.Y;
                        curPickPos.Z = readyBondPickPosition + pickUpDistance
                            //+ carrierWithWaffleConfig.ComponentThickness
                            /* + nozzle.MeasureHeightOffset*/;
                    }
                    else
                    {
                        // 获取翻转工具
                        FlipTool flipTool =
                            WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool;

                        // 最终取晶位=翻转台工具示教取片位置-吸嘴偏移+芯片取料补偿
                        curPickPos.X = flipTool.FlipToolPositionXY.X - nozzleOffsetRotated.X
                                       + component.PickupOffset.X;

                        curPickPos.Y = flipTool.FlipToolPositionXY.Y - nozzleOffsetRotated.Y
                                       + component.PickupOffset.Y;

                        // 计算最终取晶高度:翻转台工具示教取片高度+吸嘴相对标准吸嘴补偿+取料补偿
                        curPickPos.Z = flipTool.FlipToolPositionZ + nozzle.MeasureHeightOffset + pickUpDistance /*+ component.ComponentThickness*/;
                    }

                    break;

                #endregion

                case CarrierTypeEnum.StaticWaffle:

                    #region 静态华夫盒

                    // 获取静态华夫盒对象
                    CarrierWithStaticWaffleConfig carrierWithStaticWaffleConfig =
                        (CarrierWithStaticWaffleConfig)CarrierConfigRepository.GetInstance().Find(component.Name);

                    // 华夫盒示教高度转Bond坐标系
                    readyBondPickPosition =
                        this.bondModuleController.ConvertG0ToMachinePos(carrierWithStaticWaffleConfig.ReadyBondPickPosition)
                            .Z;

                    AKRSPoint2D CalculateOffset()
                    {
                        double angleInRadians = matchResult.Angle * Math.PI / 180;


                        (double x, double y) cal = GeometryHelper.Rotate(0, 0, -matchResult.Angle, component.RelativeDistanceMarkWithCenter.X, component.RelativeDistanceMarkWithCenter.Y);

                        return new AKRSPoint2D(cal.x, cal.y);
                    }

                    AKRSPoint2D dieOffset = CalculateOffset();

                    // 最终取晶位=Bond取料位+吸嘴偏移+芯片取料补偿
                    // 计算最终取晶高度=华夫盒示教高度+取料补偿/*+芯片厚度*/+吸嘴测高补偿
                    // 在芯片上测高不加芯片厚度
                    AKRSPoint3D pickPosInG0 = this.GetBondVisionResultPos(matchResult)
                                              + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;
                    curPickPos.X = this.bondModuleController.ConvertG0ToMachinePos(pickPosInG0).X - nozzleOffsetRotated.X +
                                   component.PickupOffset.X - dieOffset.X;
                    curPickPos.Y = this.bondModuleController.ConvertG0ToMachinePos(pickPosInG0).Y - nozzleOffsetRotated.Y +
                                   component.PickupOffset.Y - dieOffset.Y;
                    curPickPos.Z = readyBondPickPosition + pickUpDistance
                        //+ carrierWithStaticWaffleConfig.ComponentThickness

                        /* + nozzle.MeasureHeightOffset*/;

                    /// yaoyu 8.5
                    //AKRSPoint2D pickPosTest = CalibService.GetMachinePosByPixelPos(this.bondModuleController.Get2DRealPosition(), matchResult, "BondCameraCoordinateSystem");

                    //double PickX = pickPosTest.X - nozzleOffsetRotated.X + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X;
                    //double PickY = pickPosTest.Y - nozzleOffsetRotated.Y + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y;

                    break;

                    #endregion
            }

            return new AKRSPoint4D()
            {
                X = curPickPos.X,
                Y = curPickPos.Y,
                Z = curPickPos.Z,
                T = pickAngle
            };
        }

        /// <summary>
        /// 自动换Touchdown
        /// </summary>
        /// <returns>结果</returns>
        public bool ChangeTouchDownAssistance()
        {
            // 检查Touchdown
            if (!this.bondHeadController.IsTouchDownOnBondhead())
            {
                // Touchdown在吸嘴架上就自动换
                if (this.nozzleShelfController.IsTouchDownOnToolBank() && this.system2Configuration.IsToolBankEnable)
                {
                    return this.SwitchTouchDown();
                }
                else
                {
                    this.bondHeadController.CloseToolVaccum();

                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"请先将touchdown装到焊头上!",
                        "提示",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Information);

                    if (dialog == DialogResult.Cancel)
                    {
                        return false;
                    }

                    this.bondHeadController.SetTouchDownOnBondhead();
                }
            }

            return true;
        }

        /// <summary>
        /// 切换BMC
        /// </summary>
        /// <returns>结果</returns>
        public bool ChangeBMCAssistance()
        {
            return this.ChangeNozzleAssistance("BMC");
        }

        /// <summary>
        /// 示教结束放回当前焊头上的吸嘴
        /// </summary>
        /// <returns>结果</returns>
        public bool PutbackNozzleAssitance()
        {
            try
            {
                string nozzle = this.bondHeadController.GetCurrentNozzleName();

                if (!this.nozzleShelfController.IsConfiguredOnToolBank(nozzle))
                {
                    this.bondHeadController.CloseBondHeadVaccum();

                    this.bondModuleController.MoveToSafePos();
                    this.bondHeadController.RotateAxisTToPreparePos();

                Retry:
                    DialogResult dialogResult = AKRSXtraMessageBox.Show(
                        $"吸嘴 :{this.bondHeadController.GetCurrentNozzleName()}  未配置到吸嘴架，请手动放回到吸嘴架!",
                        "提示",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        if (this.system2Configuration.IsActiveToolDetection)
                        {
                            bool res = this.bondHeadController.IsToolOnBondheadPhy();
                            this.bondHeadController.CloseBondHeadVaccum();

                            // 如果检测到还有吸嘴
                            if (res)
                            {
                                goto Retry;
                            }
                        }

                        this.bondHeadController.SetCurrentNozzleName(string.Empty);
                    }

                    NozzleShelfSlot nozzleShelfSlot = this.nozzleShelfController.GetCurrentToolNozzleShelfSlot();
                    if (nozzleShelfSlot != null)
                    {
                        nozzleShelfSlot.NozzleState = NozzleStateEnum.OnSlot;
                        NozzleShelfRepository.GetInstance().Save();
                    }


                    return true;
                }

                if (!this.system2Configuration.IsToolBankEnable)
                {
                    this.bondHeadController.CloseBondHeadVaccum();

                    this.bondModuleController.MoveToSafePos();

                Retry:
                    DialogResult dialogResult = AKRSXtraMessageBox.Show(
                        $"吸嘴架未启用，请手动放回吸嘴:{this.bondHeadController.GetCurrentNozzleName()}!",
                        "提示",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (dialogResult == DialogResult.OK)
                    {
                        if (this.system2Configuration.IsActiveToolDetection)
                        {
                            bool res = this.bondHeadController.IsToolOnBondheadPhy();
                            this.bondHeadController.CloseBondHeadVaccum();

                            // 如果检测到还有吸嘴
                            if (res)
                            {
                                goto Retry;
                            }
                        }

                        this.bondHeadController.SetCurrentNozzleName(string.Empty);
                    }

                    NozzleShelfSlot nozzleShelfSlot = this.nozzleShelfController.GetCurrentToolNozzleShelfSlot();
                    if (nozzleShelfSlot != null)
                    {
                        nozzleShelfSlot.NozzleState = NozzleStateEnum.OnSlot;
                        NozzleShelfRepository.GetInstance().Save();
                    }

                    return true;
                }
                else
                {
                    this.PutbackNozzle();

                    ExcuteResult ret = this.nozzleShelfController.MoveShelfToHome();

                    if(ret!=ExcuteResult.Success)
                    {
                        throw new Exception("还吸嘴流程：吸嘴架缩回失败，请检查吸嘴架Y轴正限位是否触发！");
                    }
                }

                this.bondModuleController.MoveToSafePos();

                return true;
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show(
                    $"放回吸嘴:{this.bondHeadController.GetCurrentNozzleName()}  失败!" + e.ToString(),
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                LogHelper.Post(Level.Error, $"放回当前焊头上的吸嘴失败", e, LogCategory.Bond);
                return false;
            }
        }

        /// <summary>
        /// 示教页面换吸嘴方法
        /// </summary>
        /// <param name="nozzleName">吸嘴名称</param>
        /// <returns>结果</returns>
        public bool ChangeNozzleAssistance(string nozzleName)
        {
            if (this.bondHeadController.GetCurrentNozzleName() != nozzleName)
            {
                if (this.nozzleShelfController.IsConfiguredOnToolBank(nozzleName) &&
                    this.system2Configuration.IsToolBankEnable)
                {
                    bool res = this.ChangeNozzle(nozzleName);
                    if (!res)
                    {
                        return false;
                    }
                }
                else
                {
                    this.bondHeadController.CloseToolVaccum();

                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"请先更换吸嘴，所需吸嘴 :" + nozzleName,
                        "提示",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Warning);

                    if (dialog == DialogResult.OK)
                    {
                        this.bondHeadController.SetCurrentNozzleName(nozzleName);

                        this.bondHeadController.OpenBondHeadVaccum();
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }


    }
}
