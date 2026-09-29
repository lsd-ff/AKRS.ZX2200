using AKRS.Galaxy2.AutoFocusing;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.MeasureHeight;
using AKRS.Galaxy2.MeasureHeight.IOMeasureHeight;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using ch.etel.edi.dsa.v40;
using DevExpress.XtraEditors;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controllers
{
    /// <summary>
    /// Bond头控制器
    /// </summary>
    public partial class BondHeadController
    {
        /// <summary>
        /// bond head 和顶针的插补
        /// </summary>
        private static DsaIpolGroup zzIGroup;

        /// <summary>
        /// 焊头
        /// </summary>
        private BondHead bondHead = new BondHead();

        /// <summary>
        /// BondHeadParam
        /// </summary>
        private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;

        /// <summary>
        /// 设备参数
        /// </summary>
        private BondDevicePara BondDevicePara => BondDevicePara.GetInstance();

        /// <summary>
        /// 顶针控制器
        /// </summary>
        private EjectController ejectController = new EjectController();

        /// <summary>
        /// Pick动作节点帮助类
        /// </summary>
        private PickActionService pickActionService = new PickActionService();

        /// <summary>
        /// BondAction Provider
        /// </summary>
        private BondActionService bondActionService = new BondActionService();

        /// <summary>
        /// MachineSoftwareConfiguration
        /// </summary>
        private MachineSoftwareConfiguration machineSoftwareConfiguration => MachineSoftwareConfiguration.GetInstance();

        /// <summary>
        /// 拾取方法
        /// </summary>
        /// <param name="pickLevel">拾取高度</param>
        /// <param name="component">芯片</param>
        /// <param name="pickLiftLevel">抬起高度</param>
        /// <param name="pickType">取片类型</param>
        /// <returns>结果</returns>
        public bool PickAction(
            double pickLevel,
     BaseCarrierConfig component,
            double pickLiftLevel,
            PickTypeEnum pickType)
        {
            // 当前速度
            double speed = this.GetAxisZAbsoluteSpeed();

            PickActionParameter pickActionParameter =
                this.pickActionService.GetPickActionParameter(component, pickType);

            try
            {
                // 取片准备位
                double pickPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceBeforePickup;

                // 预备抬起位
                double liftPreLevel = pickLevel + pickActionParameter.SlowTravelDistanceAfterPickup;

                // 吸真空关闭
                this.CloseToolVaccum();

                System2RunTimeProvider.RecordTime("取片动作", $" 吸嘴真空关闭");

                // 设置限定线
                UcMainSystem.SetChartControlConstantLine(component.PickupForce);

                // 开始读焊头力
                UcMainSystem.ActiveReadBondForce(true);

                if (pickActionParameter.forceMode == ForceModeEnum.Distance)
                {
                    if (component.IsActivateSlowTravelBeforePickup)
                    {
                        // 高速移动到预取精位置
                        this.MoveAxisZ(pickPreLevel);

                        System2RunTimeProvider.RecordTime("取片动作", $" 距离模式-高速移动到预取精位置");

                        // 低速移动到取料高度
                        this.MoveAxisZ(
                            pickLevel,
                            pickActionParameter.SlowTravelSpeedBeforePickup,
                            AccuracyMode.HighSpeed,
                            false);

                        System2RunTimeProvider.RecordTime("取片动作", $" 距离模式-低速移动到取料高度");
                    }
                    else
                    {
                        // 高速移动到取晶高度
                        this.MoveAxisZ(pickLevel);

                        System2RunTimeProvider.RecordTime("取片动作", $" 距离模式-高速移动到取晶高度");
                    }
                }
                else
                {
                    if (this.bondHead.AxisZ.AxisDrive is GTAxis)
                    {
                        // 如果是固高轴先运动到目标位
                        this.MoveAxisZ(pickPreLevel);

                        System2RunTimeProvider.RecordTime("取片动作", $" 力控模式-高速移动到预取精位置");
                    }

                    // 力控模式下压
                    this.ForceControlSet(
                        pickActionParameter.PickForce,
                        pickPreLevel,
                        speed,
                        component.ForceControlOutTime,
                        pickActionParameter.SlowTravelDistanceBeforePickup);

                    System2RunTimeProvider.RecordTime("取片动作", $" 力控模式-力控下压");
                }

                // 吸嘴吸真空打开
                this.OpenToolVaccum();

                System2RunTimeProvider.RecordTime("取片动作", $"吸嘴真空打开完成");

                // 关平台真空
                pickActionParameter.VacuumElectric?.SetOutputValue(false);

                System2RunTimeProvider.RecordTime("取片动作", $"关平台真空完成");

                // 按照Datacon的标准这个延时去掉
                //Thread.Sleep(pickActionParameter.VacuumBuildUpDelay);

                if (pickActionParameter.BlowElectric != null && pickActionParameter.TableBlowDelay > 0) 
                {
                    if (pickType == PickTypeEnum.FlipTable)
                    {
                        int proportion = WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool.BlowProportion;
                        WaferSubController.GetInstance().FlipController.Blow(
                        proportion,
                         pickActionParameter.TableBlowDelay);

                        System2RunTimeProvider.RecordTime("取片动作", $"中转台吹气{pickActionParameter.TableBlowDelay}ms");
                    }
                    else
                    {
                        // 开平台吹气
                        pickActionParameter.BlowElectric.SetOutputValue(true);

                        Thread.Sleep(pickActionParameter.TableBlowDelay);

                        // 关平台吹气
                        pickActionParameter.BlowElectric.SetOutputValue(false);

                        System2RunTimeProvider.RecordTime("取片动作", $"{pickActionParameter.BlowElectric.HardwareName}吹气{pickActionParameter.TableBlowDelay}ms");
                    }
                }

                // 顶针顶起线程
                Task ejectTask = null;

                // 顶针动作
                if (pickType == PickTypeEnum.CarrierWithWafer)
                {
                    CarrierWithWaferConfig carrierWithWafer = component as CarrierWithWaferConfig;

                    // 力控模式下没有同步顶，后续看有没有此工艺需求
                    if (carrierWithWafer.IsActivateSynchronousEjection)
                    {
                        // 顶针同步顶
                        this.SynchronousEjection(carrierWithWafer.EjectLiftSpeed);
                    }
                    else
                    {
                        // 这里改成异步是为了提高UPH
                        ejectTask = Task.Run(
                            () =>
                            {
                                System2RunTimeProvider.RecordTime("取片动作", $"顶针准备顶起");

                                // 顶针顶起
                                WaferSubController.GetInstance().EjectController.MoveEjectToLiftPosition();
                            });

                        if (pickActionParameter.IsResetForceControlAdvanceDuringPickup
                            && component.PickupForceMode == ForceModeEnum.Force)
                        {
                            // 这里往上抬一点是为了防止芯片顶裂
                            double pos = this.GetAxisZRealPos()
                                         + carrierWithWafer.ForecResetDistanceDuringForceMode;

                            // 原地退出力控
                            this.ForceControlReset(pos, speed, component.ForceControlOutTime);

                            System2RunTimeProvider.RecordTime("取片动作", $"原地退出力控完成");
                        }
                    }
                }

                // 等顶针顶起到位
                if (ejectTask != null)
                {
                    if (ejectTask.Wait(2000) == false)
                    {
                        DialogResult dialog = AKRSXtraMessageBox.Show(
                            $"顶针到位超时，取片失败！",
                            "Warn",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        // 抬到安全高度
                        if (component.PickupForceMode == ForceModeEnum.Distance)
                        {
                            this.MoveBondZToSafePos();
                        }
                        else
                        {
                            this.ForceControlReset(this.BondDevicePara.BondHeadParam.AxisSafePos.Z, 100);
                        }

                        return false;
                    }

                    System2RunTimeProvider.RecordTime("取片动作", $"顶针顶起完成");
                }

                // 拾取延迟
                DelayHelper.Delay(pickActionParameter.PickDelay);

                System2RunTimeProvider.RecordTime("取片动作", $"拾取延时{pickActionParameter.PickDelay}ms");

                // 晶圆芯片同步顶的情况下不执行低速上抬动作
                if ((component is CarrierWithWaferConfig wafer && wafer.IsActivateSynchronousEjection) == false) 
                {
                    // 低速上抬动作
                    if (pickActionParameter.forceMode == ForceModeEnum.Distance)
                    {
                        if (component.IsActivateSlowTravelAfterPickup)
                        {
                            // 低速移动到预抬起位置
                            this.MoveAxisZ(
                                liftPreLevel,
                                pickActionParameter.SlowTravelSpeedAfterPickup,
                                AccuracyMode.HighSpeed,
                                false);

                            System2RunTimeProvider.RecordTime("取片动作", $"距离模式-低速移动到预抬起位置");
                        }
                    }
                    else
                    {
                        if (component.IsActivateSlowTravelAfterPickup)
                        {
                            // 加这句是为了防止Z轴抬起时没有恢复正常速度
                            this.SetAxisZSpeed(pickActionParameter.SlowTravelSpeedAfterPickup);

                            liftPreLevel = this.GetAxisZRealPos() + pickActionParameter.SlowTravelDistanceAfterPickup;

                            // 力控模式低速速上抬
                            this.ForceControlReset(liftPreLevel, pickActionParameter.SlowTravelSpeedAfterPickup);
                            this.SetAxisZSpeed(speed);

                            System2RunTimeProvider.RecordTime("取片动作", $"力控模式-低速移动到预抬起位置");
                        }
                        else
                        {
                            if (pickActionParameter.IsResetForceControlAdvanceDuringPickup == false) 
                            {
                                // 原地退出力控
                                this.ForceControlReset(this.GetAxisZRealPos(), speed);

                                System2RunTimeProvider.RecordTime("取片动作", $"力控模式-原地退出力控");
                            }
                        }
                    }
                }

                // 高速移动到抬起高度
                this.MoveAxisZ(pickLiftLevel, AccuracyMode.HighSpeed);

                System2RunTimeProvider.RecordTime("取片动作", $"高速移动到抬起高度");
            }
            catch (NullReferenceException e)
            {
                LogHelper.Post(Level.Error, $"获取力控配置参数失败", e, LogCategory.Bond);

                // 顶针缩回
                WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();

                if (pickActionParameter.forceMode == ForceModeEnum.Force)
                {
                    // 退出力控
                    this.ForceControlReset(pickLiftLevel, speed);
                }

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"取片失败! \r\n" + e.ToString(),
                    "取片报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"取料失败", ex, LogCategory.Bond);

                //// 顶针缩回
                //WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPositionAndBlow();

                if (pickActionParameter.forceMode == ForceModeEnum.Force)
                {
                    // 退出力控
                    this.ForceControlReset(pickLiftLevel, 200);
                }

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"取片失败! \r\n" + ex.ToString(),
                    "取片报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw;
            }
            finally 
            {
                UcMainSystem.ActiveReadBondForce(false);
            }

            return true;
        }

        /// <summary>
        /// 固晶动作
        /// </summary>
        /// <param name="bondLevel">固晶高度</param>
        /// <param name="bondLiftLevel">固晶抬起高度</param>
        /// <param name="component">芯片对象</param>
        /// <param name="bondType">固晶类型</param>
        /// <returns>结果</returns>
        public bool BondAction(
            double bondLevel,
            double bondLiftLevel,
BaseCarrierConfig component,
                BondTypeEnum bondType)
        {
            // 当前速度
            double speed = this.GetAxisZAbsoluteSpeed();

            BondActionParameter bondActionParameter =
                this.bondActionService.GetBondActionParameter(component, bondType);

            try
            {
                // 预备抬起位
                double liftPreLevel = bondLevel + bondActionParameter.SlowTravelDistanceAfterPlace;

                // 固精准备位
                double bondPreLevel = bondLevel + bondActionParameter.SlowTravelDistanceBeforePlace;

                // 设置限定线
                UcMainSystem.SetChartControlConstantLine(component.BondingForce);

                UcMainSystem.ActiveReadBondForce(true);

                if (bondActionParameter.forceMode == ForceModeEnum.Distance)
                {
                    if (component.IsActivateSlowTravelBeforeBonding)
                    {
                        // 高速移动到固晶准备位
                        this.MoveAxisZ(bondPreLevel);

                        // 低速移动到固晶高度
                        this.MoveAxisZ(
                            bondLevel,
                            bondActionParameter.SlowTravelSpeedBeforePlace,
                            AccuracyMode.HighAccuracy,
                            false);
                    }
                    else
                    {
                        // 直接高速移动到固晶高度
                        this.MoveAxisZ(bondLevel);
                    }
                }
                else
                {
                    if (this.bondHead.AxisZ.AxisDrive is GTAxis)
                    {
                        // 如果是固高轴先运动到目标位
                        this.MoveAxisZ(component.IsActivateSlowTravelBeforeBonding ? bondPreLevel : bondLevel);
                    }

                    // 力控模式下压
                    this.ForceControlSet(
                        bondActionParameter.BondForce,
                        component.IsActivateSlowTravelBeforeBonding ? bondPreLevel : bondLevel,
                        speed,
                        component.ForceControlOutTime,
                        bondActionParameter.SlowTravelDistanceBeforePlace);
                }

                if (bondType == BondTypeEnum.BondOnLeftIPT || bondType == BondTypeEnum.BondOnRightIPT || bondType == BondTypeEnum.BondOnBMC)
                {
                    // 开平台真空
                    bondActionParameter.VacuumElectric?.SetOutputValue(true);

                    //// 真空延迟
                    //DelayHelper.Delay(component.IPTVacuumBuildUpDelay);
                }

                int placementDelay = (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay) > 0
                                            ? (bondActionParameter.PlacementDelay - bondActionParameter.VacuumOffDelay)
                                            : 0;

                // 固晶延迟
                DelayHelper.Delay(placementDelay);

                // 蘸胶不关真空也不弱吹
                if (bondType != BondTypeEnum.DipFluxOnTU)
                {
                    // 关闭吸嘴真空
                    this.CloseToolVaccum();

                    // 同时开弱吹
                    Task.Run(
                        () =>
                        {
                            // 打开弱吹气
                            if (bondActionParameter.BlowingDelay != 0)
                            {
                                CommonUtil.SetCurrentThreadName("固晶弱吹线程");

                                this.OpenToolBlowEle(component.IPTWeakBlowProportion);

                                // 弱吹气延迟
                                DelayHelper.Delay(bondActionParameter.BlowingDelay);

                                // 关闭弱吹气
                                this.CloseToolBlowEle();
                            }
                        });

                    // 关真空延迟
                    DelayHelper.Delay(bondActionParameter.VacuumOffDelay);
                }

                if (bondActionParameter.forceMode == ForceModeEnum.Distance)
                {
                    if (component.IsActivateSlowTravelAfterBonding)
                    {
                        // 低速上抬到预固晶位
                        this.MoveAxisZ(
                            liftPreLevel,
                            bondActionParameter.SlowTravelSpeedAfterPlace,
                            AccuracyMode.HighSpeed,
                            false);
                    }

                    // 高速抬起
                    this.MoveAxisZ(bondLiftLevel, AccuracyMode.HighSpeed);
                }
                else
                {
                    if (component.IsActivateSlowTravelAfterBonding)
                    {
                        // 加这句是为了防止Z轴抬起时没有恢复正常速度
                        this.SetAxisZSpeed(bondActionParameter.SlowTravelSpeedAfterPlace);

                        liftPreLevel = this.GetAxisZRealPos()
                                       + bondActionParameter.SlowTravelDistanceAfterPlace;

                        // 力控模式低速速上抬
                        this.ForceControlReset(
                            liftPreLevel,
                            bondActionParameter.SlowTravelSpeedAfterPlace);

                        // 高速抬起
                        this.MoveAxisZ(bondLiftLevel, AccuracyMode.HighSpeed);
                    }
                    else
                    {
                        // 高速抬起
                        this.ForceControlReset(bondLiftLevel, speed);
                    }
                }
            }
            catch (Exception ex)
            {
                if (bondActionParameter.forceMode == ForceModeEnum.Force)
                {
                    // 退出力控
                    this.ForceControlReset(bondLiftLevel, speed);
                }

                LogHelper.Post(Level.Error, $"固晶失败", ex, LogCategory.Bond);

                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"固晶失败! \r\n" + ex.ToString(),
                    "固晶报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                throw ex;
            }
            finally
            {
                UcMainSystem.ActiveReadBondForce(false);
            }

            return true;
        }

        /// <summary>
        /// 检查吸嘴上是否有料（漏晶检测）true:无漏晶 false：漏晶
        /// </summary>
        /// <param name="nozzle">吸嘴</param>
        /// <returns>结果</returns>
        public bool ComponentCheckAfterPickup(Nozzle nozzle)
        {
            if (!this.GetNozzleVacuumState())
            {
                // 吸真空
                this.bondHead.VaccumElectric.SetOutputValue(true);
            }

            // 打开真空后的延迟时间
            Thread.Sleep(nozzle.AfterPickupVacuumCheckDelay);

            double vacuumValue = this.ReadVacuumValue();

            // 真空检测
            bool ret = vacuumValue < nozzle.AfterPickupVacuumCheckValue;
            
            return ret;
        }

        /// <summary>
        /// 检查吸嘴上是否有料(回带检测)  true:无回带 false：有回带
        /// </summary>
        /// <param name="nozzle">吸嘴</param>
        /// <returns>结果</returns>
        public bool ComponentCheckAfterBonding(Nozzle nozzle)
        {
            if (!this.GetNozzleVacuumState())
            {
                // 吸真空
                this.bondHead.VaccumElectric.SetOutputValue(true);


                Thread.Sleep(nozzle.AfterBondingVacuumCheckDelay);

            }

            DateTime startTime = DateTime.Now;

            List<double> readList = new List<double> { };

            // 数据采集
            while ((DateTime.Now - startTime) < TimeSpan.FromMilliseconds(5))
            {
                readList.Add(this.ReadVacuumValue());
            }

            // 真空检测
            //bool ret = this.ReadVacuumValue() > nozzle.AfterBondingVacuumCheckValue;


            // 真空检测
            bool ret = readList.Exists(it => it < nozzle.AfterBondingVacuumCheckValue);

            return !ret;
        }

        /// <summary>
        ///  Bond测高方法(从当前位置测高)
        /// </summary>
        /// <param name="liftLevel">抬起高度</param>
        /// <param name="heightMeasurementFunction">测高方式枚举</param>
        /// <param name="searchDistance">搜索距离</param>
        /// <param name="searchVel">搜索速度</param>
        /// <returns>结果，关于Bond</returns>
        public (ExcuteResult Ret, double HeightValue) MeasureHeight(
            double liftLevel,
            HeightMeasurementFunctionEnum heightMeasurementFunction,
            double searchDistance = 10,
            double searchVel = 1)
        {
            double searchPosition = this.bondHead.AxisZ.GetRealPosition();

            return this.MeasureHeight(
                liftLevel,
                heightMeasurementFunction,
                searchPosition,
                searchDistance,
                searchVel);
        }

        /// <summary>
        ///  Bond测高方法(可设置测高搜索距离)
        /// </summary>
        /// <param name="liftLevel">抬起高度</param>
        /// <param name="heightMeasurementFunction">测高方式枚举</param>
        /// <param name="searchPosition">搜索高度</param>
        /// <param name="searchDistance">搜索距离</param>
        /// <param name="searchVel">搜索速度</param>
        /// <returns>结果，关于Bond</returns>
        public (ExcuteResult Ret, double HeightValue) MeasureHeight(double liftLevel, HeightMeasurementFunctionEnum heightMeasurementFunction, double searchPosition, double searchDistance, double searchVel)
        {
            // 测高结果
            (ExcuteResult Ret, double HeightValue) ret;

            if (heightMeasurementFunction == HeightMeasurementFunctionEnum.Manual)
            {
                ret.Ret = ExcuteResult.Abort;
                ret.HeightValue = 999;
            }
            else
            {
                // 测高实体
                MeasureHeightEntity entity;

                IOJudgeCondition condition;

                if (heightMeasurementFunction == HeightMeasurementFunctionEnum.WithTDSensor)
                {
                    condition = new IOJudgeCondition()
                    {
                        MeasureHeightSensorName = "传感器名称",

                        // 默认值
                        IsCalcLVDT = false,

                        LvdtName = "焊头LVDT",

                        // 不接触时的模拟量加1000,弃用
                        LvdtLimit = this.bondHeadParam.LvdtLimit,

                        // 默认值 不使用 lvdt
                        IsUseLVDTJudgeState = true,

                        // 当传感器 的状态为 TriggeIoValue 时 说明已经到达测高位置
                        TriggeIoValue = true
                    };

                    entity = new MeasureHeightEntity()
                    {
                        AxisName = "BondZ",

                        // 测高方向
                        SearchDir = MoveDirection.Negative,

                        // 搜索的位置
                        SearchPosition = searchPosition,

                        // 抬起的位置： 测高完成后 轴抬到此高度
                        LiftPosition = liftLevel,

                        // 是否在停止后等到轴到位
                        IsWaitStopArrive = false,

                        // 测高距离（mm）慢速移动此距离下探直到触发触感器
                        SearchDistance = searchDistance,

                        // 下探的慢速度
                        SearchVel = searchVel,

                        // 超时时间
                        TimeOutMillSeconds = 10000,

                        JudgeArrive = condition,
                    };
                }
               else if (heightMeasurementFunction == HeightMeasurementFunctionEnum.ForceSensor)
                {
                    condition = new IOJudgeCondition()
                    {
                        MeasureHeightSensorName = "传感器名称",

                        // 默认值
                        IsCalcLVDT = false,

                        LvdtName = "校正台压力表模拟量读取",

                        // 不接触时的模拟量加1000
                        LvdtLimit = 1,

                        // 默认值 不使用 lvdt
                        IsUseLVDTJudgeState = true,

                        // 当传感器 的状态为 TriggeIoValue 时 说明已经到达测高位置
                        TriggeIoValue = true
                    };

                    entity = new MeasureHeightEntity()
                    {
                        AxisName = "BondZ",

                        // 测高方向
                        SearchDir = MoveDirection.Negative,

                        // 搜索的位置
                        SearchPosition = searchPosition,

                        // 抬起的位置： 测高完成后 轴抬到此高度
                        LiftPosition = liftLevel,

                        // 是否在停止后等到轴到位
                        IsWaitStopArrive = false,

                        // 测高距离（mm）慢速移动此距离下探直到触发触感器
                        SearchDistance = searchDistance,

                        // 下探的慢速度
                        SearchVel = searchVel,

                        // 超时时间
                        TimeOutMillSeconds = 10000,

                        JudgeArrive = condition,
                    };
                }

                else
                {
                    this.bondHead.VaccumElectric.SetOutputValue(true);

                    condition = new IOJudgeCondition()
                    {
                        // 传感器名称
                        MeasureHeightSensorName = "漏晶检测",

                        // 默认值
                        IsCalcLVDT = false,

                        // 默认值 不使用 lvdt
                        IsUseLVDTJudgeState = false,

                        // 当传感器 的状态为 TriggeIoValue 时 说明已经到达测高位置
                        TriggeIoValue = true
                    };

                    // 测高实体
                    entity = new MeasureHeightEntity()
                    {
                        AxisName = "BondZ",   // 轴名称
                        MeasureHeightAxis = this.bondHead.AxisZ,
                        SearchPosition = searchPosition,
                        LiftPosition = liftLevel,
                        SearchVel = searchVel,
                        MeasureType = MeasureHeightTypeEnum.IOMeasureHeight,
                        SearchDir = MoveDirection.Negative,      // 测高方向
                        SearchDistance = searchDistance,
                        TimeOutMillSeconds = 10000,   // 超时时间
                        JudgeArrive = condition
                    };
                }


            ReMeasureHeight:

                Thread.Sleep(1000);

                // 执行
                ret = entity.DoWork();

                if (ret.Ret != ExcuteResult.Success)
                {
                    DialogResult dialog;

                    if (ret.Ret == ExcuteResult.TimeOut)
                    {
                        // 提示测高失败
                        dialog = AKRSXtraMessageBox.Show(
                            $"测高失败请检查搜索距离！\r\n Retry：重测\r\n Cancel：结束",
                            "测高报警",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning);

                        if (dialog == DialogResult.Retry)
                        {
                            goto ReMeasureHeight;
                        }
                    }
                    else
                    {
                        // 提示测高失败
                        dialog = AKRSXtraMessageBox.Show(
                            $"测高失败！\r\n Retry：重测\r\n Cancel：结束",
                            "测高报警",
                            MessageBoxButtons.RetryCancel,
                            MessageBoxIcon.Warning);

                        if (dialog == DialogResult.Retry)
                        {
                            goto ReMeasureHeight;
                        }
                    }
                }

                if (heightMeasurementFunction == HeightMeasurementFunctionEnum.WithVacuumSensor)
                {
                    this.bondHead.VaccumElectric.SetOutputValue(false);
                }
            }

            return ret;
        }

        /// <summary>
        /// 吸嘴名称切换TouchDown,用于示教
        /// </summary>
        public void SetTouchDownOnBondhead()
        {
            // 开焊头吸附
            this.OpenBondHeadVaccum();

            this.bondHeadParam.CurrentNozzleName = "TouchDown";
            this.BondDevicePara.Save();
        }

        /// <summary>
        /// 吸嘴名称切换BMC,用于示教
        /// </summary>
        public void SetBMCOnBondhead()
        {
            // 开焊头吸附
            this.OpenBondHeadVaccum();

            this.bondHeadParam.CurrentNozzleName = "BMC";
            this.BondDevicePara.Save();
        }

        /// <summary>
        /// 设置当前吸嘴
        /// </summary>
        /// <param name="name">吸嘴名</param>
        public void SetCurrentNozzleName(string name)
        {
            this.bondHeadParam.CurrentNozzleName = name;
            this.BondDevicePara.Save();
        }

        /// <summary>
        /// 获取当前吸嘴名
        /// </summary>
        /// <returns>名称</returns>
        public string GetCurrentNozzleName()
        {
            return this.bondHead.CurrentNozzleName;
        }

        /// <summary>
        /// 焊头上是不是touchdown
        /// </summary>
        /// <returns>结果</returns>
        public bool IsTouchDownOnBondhead()
        {
            return this.GetCurrentNozzleName() == "TouchDown";
        }

        /// <summary>
        /// 焊头上是不是touchdown
        /// </summary>
        /// <returns>结果</returns>
        public bool IsBMCOnBondhead()
        {
            return this.GetCurrentNozzleName() == "BMC";
        }

        /// <summary>
        /// 获取当前吸嘴
        /// </summary>
        /// <returns>吸嘴</returns>
        public Nozzle GetCurrentNozzle()
        {
            return this.bondHead.CurrentNozzle;
        }

        /// <summary>
        /// 获取吸嘴真空状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetNozzleVacuumState()
        {
            return this.bondHead.VaccumElectric.GetOutputValue();
        }

        /// <summary>
        /// 获取吸嘴弱吹状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetNozzleBlowState()
        {
            return this.bondHead.WeakBlowElectric.GetOutputValue();
        }

        /// <summary>
        /// 获取焊头真空状态
        /// </summary>
        /// <returns>结果</returns>
        public bool GetBondheadVacuumState()
        {
            return this.bondHead.BondHeadVaccumElectric?.GetOutputValue()?? false;
        }

        /// <summary>
        /// 获取焊头在一定角度时，吸嘴口的偏移量
        /// </summary>
        /// <param name="nozzleName">吸嘴名</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>偏移值</returns>
        public AKRSPoint2D GetNozzleOffset(string nozzleName, double angle)
        {
            // 获取吸嘴在焊头上的偏移量
            AKRSPoint2D nozzleOffset = NozzleRepository.GetInstance().GetNozzle(nozzleName).NozzleOffset;

            // 顺时针旋转一定角度后的偏移量
            (double xOffset, double yOffset) nozzleOffsetRotated = GeometryHelper.Rotate(0, 0, angle, nozzleOffset.X, nozzleOffset.Y);

            return new AKRSPoint2D(nozzleOffsetRotated.xOffset, nozzleOffsetRotated.yOffset);
        }

        /// <summary>
        /// 获取焊头在一定角度时，吸嘴口的偏移量
        /// </summary>
        /// <param name="offset">相对旋转中心的偏移</param>
        /// <param name="angle">旋转角度</param>
        /// <returns>偏移值</returns>
        public AKRSPoint2D GetNozzleOffset(AKRSPoint2D offset, double angle)
        {
            // 顺时针旋转一定角度后的偏移量
            (double xOffset, double yOffset) nozzleOffsetRotated = GeometryHelper.Rotate(0, 0, angle, offset.X, offset.Y);

            return new AKRSPoint2D(nozzleOffsetRotated.xOffset, nozzleOffsetRotated.yOffset);
        }

        /// <summary>
        /// 自动对焦
        /// 不能用于吸嘴架示教，不安全
        /// </summary>
        /// <param name="cameraTypeEnum">相机类型</param>
        public void AutoFocus(CameraTypeEnum cameraTypeEnum)
        {
            // 正负限位
            double pLimit, nLimit;
            double pos;

            AutoFocusing autofocus = new AutoFocusing();

            UpLookModule uplook = new UpLookModule();
            BondModule bond = new BondModule();
            AKRSCamera camera;

            switch (cameraTypeEnum)
            {
                case CameraTypeEnum.BondCamera:
                    // BMC拍照位
                    pos = CalibrateRunPara.GetInstance().BMCVisionMachinePos.Z;

                    pLimit = pos + 3;
                    nLimit = pos - 3;

                    camera = bond.BondCamera;
                    break;

                case CameraTypeEnum.UpLookCamera:

                    // 上视旋转中心Z坐标作为基准
                    pos = this.BondDevicePara.CameraDevicePara.UpLookPos.Z
                          + (this.GetCurrentNozzle()?.MeasureHeightOffset ?? 0);

                    pLimit = pos + 3;
                    nLimit = pos - 3;

                    camera = uplook.UpLookCamera;

                    break;

                default:
                    throw new Exception($"{cameraTypeEnum.ToString()}自动对焦调用错误！");
            }

            autofocus.AutoFocus(camera, this.bondHead.AxisZ, pLimit, nLimit);
        }

        /// <summary>
        /// 自动对焦,示教界面专用
        /// </summary>
        /// <param name="cameraTypeEnum">相机类型</param>
        /// <param name="sender">控件对象</param>
        /// <param name="form">窗体</param>
        public void AutoFocusAssistance(CameraTypeEnum cameraTypeEnum, object sender, Form form)
        {
            SimpleButton btn = sender as SimpleButton;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            Task autoFocus = Task.Run(
                () =>
                    {
                        try
                        {
                            this.AutoFocus(cameraTypeEnum);
                            form.Invoke(
                                new Action(
                                    () =>
                                        {
                                            btn.Enabled = true;
                                            btn.Appearance.BackColor = Color.White;
                                        }));
                        }
                        catch (Exception exception)
                        {
                            AKRSXtraMessageBox.Show(exception.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        finally
                        {
                            form.Invoke(
                               new Action(
                                   () =>
                                   {
                                       btn.Appearance.BackColor = default;
                                       btn.Enabled = true;
                                   }));
                        }
                    });
        }

        /// <summary>
        ///  同步顶
        /// </summary>
        /// <param name="ejectLiftSpeed">顶针顶出速度</param>
        public void SynchronousEjection(double ejectLiftSpeed)
        {
            try
            {
                double speed = this.GetAxisZAbsoluteSpeed();

                // 顶针
                Axis ejectionAxisZ = HardwareRepositoryService.GetHardware<Axis>("顶针Z");

                // 先退出力控模式
                this.ForceControlReset(this.GetAxisZRealPos(), speed);

                // 设置群组
                DsaIpolGroup iGroup = this.GetZZIpolGroup();

                // 开始插补
                iGroup.ipolBegin();

                // 设置为绝对坐标系 ，不设置绝对坐标系
                iGroup.ipolSetAbsMode(true, -1);

                // 设置速度，加减速度
                iGroup.ipolTanVelocity(ejectLiftSpeed / 1000.0);
                iGroup.ipolTanAcceleration(ejectLiftSpeed * 10 / 1000.0);
                iGroup.ipolTanDeceleration(ejectLiftSpeed * 10 / 1000.0);

                // 获取顶针顶起位置
                AKRSPoint3D ejectPos = ejectController.GetEjectLiftMachinePosition();

                double bondHeaderLiftPos = this.GetAxisZRealPos()
                                           + (ejectPos.Z - ejectionAxisZ.GetRealPosition());

                // 顶针顶起同时焊头抬起
                iGroup.ipolLine(-bondHeaderLiftPos / 1000.0, ejectPos.Z / 1000.0);

                // 等待插补结束
                iGroup.ipolWaitMovement(10000);

                // 退出插补模式
                iGroup.ipolEnd();
            }
            catch (Exception e)
            {
                LogHelper.Post(Level.Info, $"取料流程-顶针同步顶运行故障", e, LogCategory.Bond);

                throw;
            }
        }

        /// <summary>
        /// 获取bond xy 插补组
        /// </summary>
        /// <returns>插补组</returns>
        public DsaIpolGroup GetZZIpolGroup()
        {
            if (zzIGroup == null)
            {
                // BondX轴
                Axis bondAxisZ = HardwareRepositoryService.GetHardware<Axis>("BondZ");

                // 顶针
                Axis ejectionAxisZ = HardwareRepositoryService.GetHardware<Axis>("顶针Z");

                // 获取X，Y轴驱动
                DsaDrive bondZ = ((ETELAxis)bondAxisZ.AxisDrive).GetDrive();
                DsaDrive ejectionZ = ((ETELAxis)ejectionAxisZ.AxisDrive).GetDrive();

                // 获取ETEL轴
                AxisCard card = HardwareRepositoryService.GetHardware<AxisCard>("ETEL");

                // 获取控制器
                DsaMaster dsaMaster = ((ETELController)card.MotionController.MotionControllerDrive).GetMaster();

                // 设置群组
                zzIGroup = new DsaIpolGroup(bondZ, ejectionZ);

                // 设置控制者
                zzIGroup.setMaster(dsaMaster);
            }

            return zzIGroup;
        }

        /// <summary>
        /// 检查吸嘴上是否有料,通过IO
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckComponentByIO()
        {
            if (!this.GetNozzleVacuumState())
            {
                // 吸真空
                this.bondHead.VaccumElectric.SetOutputValue(true);

                Thread.Sleep(50);
            }

            // 真空检测
            bool ret = this.bondHead.CheckVaccumSensor.GetInputValue();

            return ret;
        }
    }
}
