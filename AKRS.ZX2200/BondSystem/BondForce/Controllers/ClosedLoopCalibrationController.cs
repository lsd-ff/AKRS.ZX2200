using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using DevExpress.XtraEditors;
using LanguageExt.TypeClasses;

namespace AKRS.ZX2200.BondSystem.BondForce.Controllers
{
    using System.Diagnostics;
    using AKRS.Galaxy2.Infrastructure.CustomControls.Components;
    using AKRS.Galaxy2.Log;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Services;
    using log4net.Core;

    /// <summary>
    /// 力闭环控制器
    /// </summary>
    public class ClosedLoopCalibrationController : BaseCalibrationController
    {
        /// <summary>
        /// Bond头
        /// </summary> 
        private BondHead bondHead = new BondHead();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService modbusService => ModbusService.GetInstance();

        /// <summary>
        /// 力控配置参数
        /// </summary>
        private ForceConfig forceConfig => ForceConfig.GetInstance();

        /// <summary>
        ///  焊头参数
        /// </summary>
        private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;

        /// <summary>
        /// Z轴速度
        /// </summary>
        private double speed;

        /// <summary>
        ///  下压高度
        /// </summary>
        private double bondLevel;

        /// <summary>
        /// 抬起高度
        /// </summary>
        private double liftLevel;

        /// <summary>
        ///  是否停止标定
        /// </summary>
        public static bool IsStop = true;

        /// <summary>
        /// 力控标定
        /// </summary>
        public override void DoWork()
        {
            // 判断轴卡类型
            if (this.bondHead.AxisZ.AxisDrive is ETELAxis)
            {
                this.EtelDoWork();
            }
            else
            {
                this.GTDoWork();
            }
        }

        /// <summary>
        /// 力控标定(GT)
        /// </summary>
        public void GTDoWork()
        {
            // 力值对应关系存储列表
            List<TheoreticalForceAndActualForce> smallForceList = new List<TheoreticalForceAndActualForce>();
            List<TheoreticalForceAndActualForce> largeForceList = new List<TheoreticalForceAndActualForce>();

            // 对应角度下的力值关系列表
            List<ForceRelateAngleItem> smallForceRelateAngleList = new List<ForceRelateAngleItem>();
            List<ForceRelateAngleItem> largeForceRelateAngleList = new List<ForceRelateAngleItem>();

            double caliAngleDistance;

            Stopwatch sp = Stopwatch.StartNew();

            try
            {
                // 准备
                this.PrepareBeforeCali();

                // 记录残值
                this.RecordForceInitialValWithAngle();

                switch (ForceConfig.GetInstance().ForceCaliMode)
                {
                    // 单角度
                    case ForceCaliModeEnum.SingleAngle:

                        this.bondHeadController.RotateAxisT(0);

                        if (this.forceConfig.ForceRange == ForceRangeEnum.SmallForce
                            || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                        {
                            // 小力标定
                            smallForceList = this.CaliForceAtSingleAngle(isSmallForce: true);

                            smallForceRelateAngleList.Add(new ForceRelateAngleItem() { Angle = 0, TheoreticalForceAndActualForceList = smallForceList.ToList() });
                        }

                        if (this.forceConfig.ForceRange == ForceRangeEnum.LargeForce
                            || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                        {
                            // 大力标定
                            largeForceList = this.CaliForceAtSingleAngle(isSmallForce: false);

                            largeForceRelateAngleList.Add(new ForceRelateAngleItem() { Angle = 0, TheoreticalForceAndActualForceList = largeForceList.ToList() });
                        }

                        break;

                    // 多角度
                    case ForceCaliModeEnum.MultipleAngle:

                        if (this.forceConfig.ForceRange == ForceRangeEnum.SmallForce
                            || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                        {
                            caliAngleDistance = ForceConfig.GetInstance().SmallForceCaliAngleDistance;
                            for (double angle = -180; angle <= 180; angle += caliAngleDistance)
                            {
                                if (IsStop)
                                {
                                    return;
                                }

                                this.bondHeadController.RotateAxisT(angle);

                                // 小力标定
                                smallForceList = this.CaliForceAtSingleAngle(isSmallForce: true);

                                smallForceRelateAngleList.Add(new ForceRelateAngleItem() { Angle = angle, TheoreticalForceAndActualForceList = smallForceList.ToList() });
                            }
                        }

                        if (this.forceConfig.ForceRange == ForceRangeEnum.LargeForce
                            || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                        {
                            caliAngleDistance = ForceConfig.GetInstance().LargeForceCaliAngleDistance;
                            for (double angle = -180; angle <= 180; angle += caliAngleDistance)
                            {
                                if (IsStop)
                                {
                                    return;
                                }

                                this.bondHeadController.RotateAxisT(angle);

                                // 大力标定
                                largeForceList = this.CaliForceAtSingleAngle(isSmallForce: false);

                                largeForceRelateAngleList.Add(new ForceRelateAngleItem() { Angle = angle, TheoreticalForceAndActualForceList = largeForceList.ToList() });
                            }
                        }

                        break;

                    default: return;
                }
            }
            catch (Exception e)
            {
                LogHelper.Post(
                    Level.Error,
                    $"力控标定过程中出现异常！{e.ToString()}",
                    LogCategory.Global,
                    ViewType.InFileAndUI);

                throw e;
            }
            finally
            {
                ClosedLoopCalibrationController.IsStop = true;

                if (this.forceConfig.ForceRange == ForceRangeEnum.SmallForce
                    || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                {
                    ForceCalibrationData.GetInstance().SmallForceRelationList = smallForceRelateAngleList;
                }

                if (this.forceConfig.ForceRange == ForceRangeEnum.LargeForce
                    || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
                {
                    ForceCalibrationData.GetInstance().LargeForceRelationList = largeForceRelateAngleList;
                }

                ForceCalibrationData.GetInstance().ForceCaliCostTime = sp.Elapsed.TotalMinutes;

                this.Save();

                this.bondHeadController.MoveBondZToSafePos();

                // 临时注释
                //AKRSXtraMessageBox.Show(
                //    $"标定完成!\r\n最小力：{this.bondHeadParam.ForceControlMinVal}g,最大力{this.bondHeadParam.ForceControlMaxVal}g\r\n耗时{sp.Elapsed.TotalMinutes}分钟!",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 力控标定(Etel)
        /// </summary>
        public void EtelDoWork()
        {
            Stopwatch sp = Stopwatch.StartNew();

            try
            {
                this.PrepareBeforeCali();

                double acc = this.bondHead.AxisZ.GetAcc() * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                // Etel退出力控参数
                int option = this.bondHeadParam.ForceResetMode;

                int delay = this.forceConfig.ForceKeepDelay;

                double caliAngleDistance = ForceConfig.GetInstance().LargeForceCaliAngleDistance;
                for (double angle = -180; angle <= 180; angle += caliAngleDistance)
                {
                    List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList = this.GetCaliData();

                    // 先转角度
                    if (forceConfig.ForceCaliMode == ForceCaliModeEnum.MultipleAngle)
                    {
                        this.bondHeadController.RotateAxisT(angle);
                    }
                    else
                    {
                        angle = 0;
                        this.bondHeadController.RotateAxisT(0);
                    }

                    // 获取力值数据
                    foreach (TheoreticalForceAndActualForce theoreticalForceAndActualForce in theoreticalForceAndActualForceList)
                    {
                        if (IsStop)
                        {
                            // 点停止直接退出
                            return;
                        }

                        double forceValue = theoreticalForceAndActualForce.InputForce;

                        // 校正台置零
                        this.modbusService.ResetManometer();

                        double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                        double initialForce = this.system2Controller.GetBondheadForceValue();

                        if (initialForce < 0)
                        {
                            this.bondHeadController.ResetBondhead();
                        }

                        // AB段
                        this.bondHeadController.MoveAxisZ(bondLevel);

                        // 下压
                        this.bondHead.EtelForceControlSet(forceValue / 100.0, bondLevel - 0.01, 100, 500, 100);

                        Thread.Sleep(delay);

                        // 读取摩尔力设备读数
                        double readManometer = this.system2Controller.GetCalibrateTableForceValue()- calibrateTableForceInitial;
                        double readBondForce = this.system2Controller.GetBondheadForceValue();

                        // 保存
                        theoreticalForceAndActualForce.CaliTableForce = readManometer;
                        theoreticalForceAndActualForce.BondHeadForce = readBondForce;
                        theoreticalForceAndActualForce.InitialValue = initialForce;
                        theoreticalForceAndActualForce.ForceIncrement = forceValue - initialForce;

                        // 加这句是为了防止Z轴抬起时没有恢复正常速度
                        this.bondHead.AxisZ.SetVel(100);

                        // 抬起
                        this.bondHead.EtelForceControlReset(liftLevel, speed, acc, option);

                        Thread.Sleep(500);
                    }

                    // 如果是单角度标定，直接退出循环
                    if (forceConfig.ForceCaliMode == ForceCaliModeEnum.SingleAngle)
                    {
                        break;
                    }
                    else
                    {
                        List<TheoreticalForceAndActualForce> list = new List<TheoreticalForceAndActualForce>();
                        list = theoreticalForceAndActualForceList.ToList();
                        ForceCalibrationData.GetInstance().LargeForceRelationList.Add(
                            new ForceRelateAngleItem()
                            {
                                Angle = angle,
                                TheoreticalForceAndActualForceList = list
                            });
                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.Post(
                    Level.Error,
                    $"力控标定过程中出现异常！{e.ToString()}",
                    LogCategory.Global,
                    ViewType.InFileAndUI);

                //AKRSXtraMessageBox.Show(
                //    $"标定失败!\r\n{e.Message.ToString()}",
                //    "Error",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Error);

                throw e;
            }
            finally
            {
                ForceCalibrationData.GetInstance().ForceCaliCostTime = sp.Elapsed.TotalMinutes;

                this.Save();

                this.bondHeadController.MoveBondZToSafePos();

                //AKRSXtraMessageBox.Show(
                //    $"标定完成!\r\n最小力：{this.bondHeadParam.ForceControlMinVal}g,最大力{this.bondHeadParam.ForceControlMaxVal}g\r\n耗时{sp.Elapsed.TotalMinutes}分钟!",
                //    "Prompt",
                //    MessageBoxButtons.OK,
                //    MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 获取标定数据（仅etel）
        /// </summary>
        /// <returns>标定数据</returns>
        public List<TheoreticalForceAndActualForce> GetCaliData()
        {
            List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList = new List<TheoreticalForceAndActualForce>();

            // 力配置集合
            List<ForceConfigItem> forceConfigItemList =
               ForceConfig.GetInstance().LargeForceConfigItemList;

            // 标定间隔
            double calibrateInterval = ForceConfig.GetInstance().LargeForceCalibrateInterval;

            // 算出标定上下界
            double minForce = forceConfigItemList.Select(it => it.ForceLowerLimit).Min();
            double maxForce = forceConfigItemList.Select(it => it.ForceUpperLimit).Max();

            double i = minForce;
            double interval = calibrateInterval;
            while (i <= maxForce)
            {
                TheoreticalForceAndActualForce para = new TheoreticalForceAndActualForce()
                {
                    InputForce = i,
                    CaliTableForce = i,
                    BondHeadForce = i,
                };

                theoreticalForceAndActualForceList.Add(para);

                // 大力超过100 标定间隔除以2
                if (i >= 100)
                {
                    interval = calibrateInterval / 2;
                }
                else
                {
                    interval = calibrateInterval;
                }

                // 标定间隔
                i += interval;
            }

            return theoreticalForceAndActualForceList;
        }

        /// <summary>
        /// 标定前准备
        /// </summary>
        public void PrepareBeforeCali()
        {
            // 连接摩尔力设备
            this.modbusService.ConnectManometer();
            this.modbusService.ConnectBondhead();

            //this.bondHeadController.ResetBondhead();
            this.bondHeadController.MoveBondZToSafePos();

            AKRSPoint3D forceCalibratePos = BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos;

            // 移动到标定位
            this.bondModuleController.MoveToG0Pos(forceCalibratePos.X, forceCalibratePos.Y);

            this.speed = this.bondHeadController.GetAxisZAbsoluteSpeed()
                         * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

            // 换成轴坐标
            AKRSPoint3D forceCalibratePosInAxis =
                this.bondModuleController.ConvertG0ToMachinePos(
                    BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos);

            // 这里为了和测试对应改为0.2
            this.bondLevel = forceCalibratePosInAxis.Z + 0.2;
            this.liftLevel = forceCalibratePosInAxis.Z + 8;

            if (this.forceConfig.ForceRange == ForceRangeEnum.SmallForce
                || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
            {
                ForceCalibrationData.GetInstance().SmallForceRelationList.Clear();
            }

            if (this.forceConfig.ForceRange == ForceRangeEnum.LargeForce
                || this.forceConfig.ForceRange == ForceRangeEnum.SmallForceAndLargeForce)
            {
                ForceCalibrationData.GetInstance().LargeForceRelationList.Clear();
            }

            IsStop = false;
        }

        /// <summary>
        /// 在单个角度下标定
        /// </summary>
        /// <param name="isSmallForce">是否是小力</param>
        /// <returns>结果</returns>
        /// <exception cref="Exception">异常</exception>
        public List<TheoreticalForceAndActualForce> CaliForceAtSingleAngle(bool isSmallForce)
        {
            // 力值对应关系存储列表
            List<TheoreticalForceAndActualForce> theoreticalForceAndActualForceList =
                new List<TheoreticalForceAndActualForce>();

            // 切换通道并设置压力参数
            this.bondHeadController.ChangeChannelAndSetForceControlPara(isSmallForce);

            System2RunTimeProvider.RecordTime("力控标定", $"切换通道并设置压力参数完成，isSmallForce：{isSmallForce}");

            double initialVal = isSmallForce
                                    ? ForceConfig.GetInstance().SmallForceCalibrateInitialIncrement
                                    : ForceConfig.GetInstance().LargeForceCalibrateInitialIncrement;

            double maxIncrement = isSmallForce
                                       ? ForceConfig.GetInstance().SmallForceConfigItemList
                                           .Select(it => it.ForceUpperLimit).Max()
                                       : ForceConfig.GetInstance().LargeForceConfigItemList.Select(it => it.ForceUpperLimit)
                                           .Max();

            double calibrateInterval = isSmallForce
                                           ? ForceConfig.GetInstance().SmallForceCalibrateInterval
                                           : ForceConfig.GetInstance().LargeForceCalibrateInterval;

            double lvdtMaxInputForce = ForceConfig.GetInstance().LVDTMaxInputForce;

            int delay = this.forceConfig.ForceKeepDelay;

            double angle = this.bondHeadController.GetAxisTRealPos();

            // 输入力值递增
            for (double increment = initialVal; increment <= maxIncrement; increment += calibrateInterval)
            {
                if (IsStop)
                {
                    System2RunTimeProvider.RecordTime("力控标定", $"点停止直接退出");

                    // 点停止直接退出
                    return theoreticalForceAndActualForceList;
                }

                double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                System2RunTimeProvider.RecordTime("力控标定", $"下压前读取标定台力：{calibrateTableForceInitial}g");

                // 置零
                if (Math.Abs(calibrateTableForceInitial) > 3)
                {
                    this.modbusService.ResetManometer();

                    System2RunTimeProvider.RecordTime("力控标定", "标定台压力表清零完成");

                    calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                    System2RunTimeProvider.RecordTime("力控标定", $"标定台力刷新：{calibrateTableForceInitial}g");
                }

                double bondheadForceInitial = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                System2RunTimeProvider.RecordTime("力控标定", $"下压前读取焊头模拟量初始值：{bondheadForceInitial}");

                double bondheadForceInitialByAngle = ForceCalibrationService.GetInitialValByAngle(angle);

                double inputForce = bondheadForceInitialByAngle + increment;

                System2RunTimeProvider.RecordTime("力控标定", $"inputForce：{inputForce}=bondheadForceInitial{bondheadForceInitial}+increment{increment}");

                if (isSmallForce && inputForce > lvdtMaxInputForce) 
                {
                    System2RunTimeProvider.RecordTime("力控标定", $"小力标定，LVDT超出量程,标定结束");

                    // LVDT超出量程直接结束标定
                    break;
                }

                // AB段
                this.bondHeadController.MoveAxisZ(bondLevel);

                System2RunTimeProvider.RecordTime("力控标定", $"焊头Z运动到预压位置{bondLevel}");

                this.bondHeadController.GTForceControlSet(inputForce, bondLevel, 0, isSmallForce,true);

                System2RunTimeProvider.RecordTime("力控标定", $"力控下压完成，inputForce：{inputForce}");

                // 延时尽可能和贴片流程同步
                Thread.Sleep(delay);

                System2RunTimeProvider.RecordTime("力控标定", $"延时：{delay}ms");

                // 读取标定台力
                double readCalibrateTable = this.system2Controller.GetCalibrateTableForceValue();

                System2RunTimeProvider.RecordTime("力控标定", $"读标定台力值:{readCalibrateTable}g");

                // 读取应变片力，小力输入是Lvdt,不用读
                double readBondForce = isSmallForce == false ? this.system2Controller.GetBondheadForceValue() : 0;

                // 读焊头模拟量
                double bondHeadVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                System2RunTimeProvider.RecordTime("力控标定", $"读应变片力：{readBondForce}，焊头模拟量{bondHeadVal}");

                // 保存
                theoreticalForceAndActualForceList.Add(
                    new TheoreticalForceAndActualForce()
                    {
                        InputForce = inputForce,
                        CaliTableForce = readCalibrateTable - calibrateTableForceInitial,
                        BondHeadForce = readBondForce,
                        InitialValue = bondheadForceInitial,
                        ForceIncrement = inputForce - bondheadForceInitial,
                        ZAxisPos = this.bondHeadController.GetAxisZRealPos()
                    });

                // 加这句是为了防止Z轴抬起时没有恢复正常速度
                this.bondHead.AxisZ.SetVel(100);

                // 抬起
                this.bondHeadController.GTForceControlReset(liftLevel, speed);

              System2RunTimeProvider.RecordTime("力控标定", $"力控抬起完成，位置{liftLevel}，速度{speed}mm/s");

                Thread.Sleep(500);
            }

            return theoreticalForceAndActualForceList;
        }

        /// <summary>
        /// 记录不同角度下的焊头力初始值
        /// </summary>
        public void RecordForceInitialValWithAngle()
        {
            int count = 0;
        Record:
            ForceCalibrationData.GetInstance().ForceInitialValDic.Clear();
            for (int i = -180; i <= 180; i += 5)
            {
                this.bondHeadController.RotateAxisT(i);

                Thread.Sleep(500);

                double initialVal = this.bondHeadController.GetBondForceCurrentVal(false);

                ForceCalibrationData.GetInstance().ForceInitialValDic.Add(i, initialVal);
            }

            // 判断是否需要清零
            bool isNeedZeroBondForce = ForceCalibrationData.GetInstance()
                .ForceInitialValDic
                .Any(it => it.Value < -4);

            if (isNeedZeroBondForce)
            {
                if (count > 2)
                {
                    throw new Exception("焊头力初始值刷新失败！");
                }

                double minAngle = ForceCalibrationData.GetInstance()
        .ForceInitialValDic
        .OrderBy(kvp => kvp.Value)
        .First()
        .Key;

                this.bondHeadController.RotateAxisT(minAngle);

                // 清零
                this.bondHeadController.ResetBondhead();

                Thread.Sleep(500);

                count++;

                goto Record;
            }
        }
      
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            List<ForceRelateAngleItem> smallForceRelationList =
ForceCalibrationData.GetInstance().SmallForceRelationList;

            List<ForceRelateAngleItem> largeForceRelationList =
                ForceCalibrationData.GetInstance().LargeForceRelationList;

            bool isLargeAny = largeForceRelationList.Any();
            bool isSmallAny = smallForceRelationList.Any();

            if (isSmallAny)
            {
                ForceConfig.GetInstance().LVDTForceControlLowerlimit = smallForceRelationList
                        .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max()
                    ;

                ForceConfig.GetInstance().LVDTForceControlUpperlimit= smallForceRelationList
                    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min()
                    ;
            }

            if (isLargeAny)
            {
                ForceConfig.GetInstance().StrainGaugeForceControlLowerlimit = largeForceRelationList
                        .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max()
                    ;

                ForceConfig.GetInstance().StrainGaugeForceControlUpperlimit = largeForceRelationList
                        .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min()
                    ;
            }

            if (isLargeAny == false && isSmallAny == false)
            {
                return;
            }
            else if (isLargeAny && isSmallAny)
            {
                ForceConfig.GetInstance().ForceBoundary = smallForceRelationList
                       .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min()
                      ;

                BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal =
                    smallForceRelationList
                        .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max()
                       ;


                BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal =
                    largeForceRelationList
                        .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min();
            }
            else if (isSmallAny == false && isLargeAny)
            {
                ForceConfig.GetInstance().ForceBoundary = 0;

                var it = largeForceRelationList
    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max();

                BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal = largeForceRelationList
    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max()
    ;

                BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal = largeForceRelationList
                    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min();
            }
            else
            {
                ForceConfig.GetInstance().ForceBoundary = 0;

                var it = largeForceRelationList
    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max();

                BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal = smallForceRelationList
    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.First()).Select(it => it.CaliTableForce).Max()
    ;

                BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal = smallForceRelationList
                    .Select(it => it.TheoreticalForceAndActualForceList).Select(it => it.Last()).Select(it => it.CaliTableForce).Min();
            }

            ForceCalibrationData.GetInstance().ForceCaliFinishTime = DateTime.Now;

            // 标定完成，保存标定数据
            ForceCalibrationData.GetInstance().Save();
            BondDevicePara.GetInstance().Save();
            ForceConfig.GetInstance().Save();
        }
    }
}
