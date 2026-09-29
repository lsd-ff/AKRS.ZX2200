using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
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
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Carrier;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
using DataAnalysis.Acquisition;
using DevExpress.XtraEditors;
using LanguageExt.TypeClasses;
using log4net.Core;
using SqlSugar;

namespace AKRS.ZX2200.BondSystem.Controllers;

using System.ComponentModel;
using System.Linq;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.GT;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.UserManager.Models;
using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Main.Machine.Process;
using AKRS.ZX2200.SupportFeature.Statistics;
using DevExpress.CodeParser;
using TransportUnit = TransportUnit;

/// <summary>
/// 系统2控制器
/// </summary>
public partial class System2Controller
{
    /// <summary>
    /// 焊头
    /// </summary>
    private BondHead bondHead => System2Module.GetInstance().BondModule.BondHead;

    /// <summary>
    /// 焊头设备参数
    /// </summary>
    private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;

    /// <summary>
    /// 上视控制器
    /// </summary>
    private UpLookController upLookController => System2Domain.GetInstance().UpLookController;

    /// <summary>
    /// 焊头控制器
    /// </summary>
    private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

    /// <summary>
    /// Bond模组控制器
    /// </summary>
    private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

    /// <summary>
    /// 焊头控制器
    /// </summary>
    private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

    /// <summary>
    /// 刮胶盘控制器
    /// </summary>
    private SlideFluxerController slideFluxerController => System2Domain.GetInstance().SlideFluxerController;

    /// <summary>
    /// 中转台控制器
    /// </summary>
    private IPTController iPTController => System2Domain.GetInstance().IPTController;

    /// <summary>
    /// 系统2点胶系
    /// </summary>
    private S2DispenseController s2DispenseController => System2Domain.GetInstance().S2DispenseController;


    /// <summary>
    /// Bond程式
    /// </summary>
    private BondProgram bondProgram => BondProgram.GetInstance();

    /// <summary>
    /// 设备参数
    /// </summary>
    private BondDevicePara bondDevicePara => BondDevicePara.GetInstance();

    /// <summary>
    /// 设备参数
    /// </summary>
    private System2Configuration system2Configuration => System2Configuration.GetInstance();

    /// <summary>
    /// 当前传输单元/载具 
    /// </summary>
    private TransportUnit transportUnit =>
        TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

    /// <summary>
    /// DipAction Provider
    /// </summary>
    private DipActionService dipActionProvider = new DipActionService();

    /// <summary>
    /// 从吸嘴架取指定吸嘴
    /// </summary>
    /// <param name="nozzleName">吸嘴名</param>
    /// <returns>结果</returns>
    public bool TakeNozzle(string nozzleName)
    {
        try
        {
            if (this.bondHeadController.GetCurrentNozzleName() == nozzleName)
            {
                return true;
            }

            bool ret;
            int changeNozzleRetryTimes = 0;

            // 当前吸嘴架
            NozzleShelf nozzleShelf = (NozzleShelf)NozzleShelfRepository.GetInstance()
                .Find(BondProgram.GetInstance().NozzleShelfProgram.NozzleShelfName);

            // 获取吸嘴
            Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(nozzleName);

            if (!this.system2Configuration.IsToolBankEnable)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                    $"吸嘴架未启用，请手动更换吸嘴!\r\n",
                    "提示",
                    new string[] { "确认", "取消" },
                    new DialogResult[] { DialogResult.OK, DialogResult.Cancel },
                    AlarmLevel.SecondLevel);

                if (dialogResult == DialogResult.OK)
                {
                    // 刷新吸嘴槽状态
                    nozzleShelf.NozzleShelfSlots[nozzle.SlotIdentification - 1].NozzleState =
                        NozzleStateEnum.OnBondHead;

                    // 刷新焊头上的吸嘴
                    this.bondHeadController.SetCurrentNozzleName(nozzleName);

                    this.bondHeadController.OpenBondHeadVaccum();

                    NozzleShelfRepository.GetInstance().Save();

                    return true;
                }

                return false;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle)
            {
                if (this.system2Configuration.IsActiveToolDetection)
                {
                    // 硬件检查焊头上是否有吸嘴
                    ret = this.bondHeadController.IsToolOnBondheadPhy();
                    this.bondHeadController.CloseBondHeadVaccum();
                    if (ret)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"焊头上检测到仍有吸嘴，此时去取吸嘴有撞机风险!请选择如何处理！",
                            "换吸嘴报警",
                            new string[] { "忽略", "取消" },
                            new DialogResult[] { DialogResult.Ignore, DialogResult.Cancel },
                            AlarmLevel.SecondLevel);

                        if (dialogResult == DialogResult.Cancel)
                        {
                            return false;
                        }
                    }
                }
            }

        Recheck:
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                // 程式检查
                ret = this.nozzleShelfController.IsConfiguredOnToolBank(nozzleName);
            }
            else
            {
                // 检查此吸嘴是否在吸嘴架上,程式检查+硬件检查
                ret = this.nozzleShelfController.IsConfiguredOnToolBank(nozzleName)
                      && this.nozzleShelfController.IsSlotHaveNozzle(nozzle.SlotIdentification);
            }

            if (!ret)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"所需要的吸嘴:{nozzleName}不在吸嘴架上！\r\n请检查吸嘴架配置!\r\n"
                    + "重试:重新检查\r\n继续:手动将此吸嘴放到焊头上并继续工作\r\n退出:退出工作",
                    "换吸嘴报警",
                    new string[] { "重试", "继续", "退出" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                switch (dialogResult)
                {
                    case DialogResult.Retry:
                        goto Recheck;

                    case DialogResult.Ignore:
                        goto Continue;

                    case DialogResult.Abort:
                        return false;
                }

                return false;
            }

            // Z轴回到安全位
            this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();

            ExcuteResult excuteResult = ExcuteResult.Fail;

            // 吸嘴架移动到换吸嘴架位
            Task nozzleShelfMoveTask = Task.Run(() =>
                {
                    excuteResult = this.nozzleShelfController.MoveShelfToChangeNozzlePos();
                });

            // 打印
            LogHelper.Post(Level.Info, $"换吸嘴流程-吸嘴架移动到换吸嘴架位，此时Y轴坐标：{this.nozzleShelfController.GetYAxisPos()} ",
                LogCategory.Bond);

            // 获取取吸嘴位置
            AKRSPoint3D point3D = this.bondProgram.NozzleShelfProgram.NozzleShelf.GetPickNozzleSlotPos(nozzleName);
            double angle = this.bondProgram.NozzleShelfProgram.NozzleShelf.GetPickNozzleSlotAngle(nozzleName);

            // 移动到预取位置
            this.bondHeadController.RotateAxisT(angle);

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-移动到预取位置，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            AKRSPoint3D pickPos = this.bondModuleController.ConvertG0ToMachinePos(point3D);

            if (this.IsNozzleShelfAxisYSafe())
            {
                this.bondModuleController.MoveBondXYZWithoutSafe(pickPos);
            }
            else
            {
                this.bondModuleController.MoveBondXY(pickPos.X, pickPos.Y);
                this.bondHeadController.MoveZAxis(pickPos.Z);
            }


            // 防呆
            if (Math.Abs(this.bondHeadController.GetAxisTRealPos() - angle) > 0.1)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"T轴运动到指令位置失败！",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return false;
            }

            // 等吸嘴架移动到位
            nozzleShelfMoveTask.Wait();
            if (excuteResult != ExcuteResult.Success || this.nozzleShelfController.IsShelfAtChangeNozzlePos() == false)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架移动到换吸嘴架位失败！",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                LogHelper.Post(Level.Error, $"取吸嘴流程-吸嘴架移动到换吸嘴架位失败 ", LogCategory.Bond);

                throw new("取吸嘴流程-吸嘴架移动到换吸嘴架位失败！");
            }

            LogHelper.Post(Level.Info, $"放回吸嘴流程 等吸嘴架到位 ", LogCategory.Bond);

            double targetPosition = this.bondHeadController.GetAxisZRealPos()
                                    - BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceZ;

            MovePara runPara1 = new()
            {
                TargetPosition = targetPosition,
                Acc = this.bondHead.AxisZ.AxisMovePara.ACC,
                Dec = this.bondHead.AxisZ.AxisMovePara.DEC,
                Jerk = this.bondHead.AxisZ.AxisMovePara.Jerk,
                Vel = BondDevicePara.GetInstance().NozzleShelfParam.SlowTravelDownSpeed
            };

        RetryChangeNozzle:

            // 低速往下压
            ExcuteResult ret1 = this.bondHeadController.MoveAxisZ(runPara1);

            if (ret1 != ExcuteResult.Success)
            {
                throw new("取吸嘴流程Z轴慢速下压到指令位置失败！");
            }

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-Z轴低速往下压，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            // 开焊头吸附
            this.bondHeadController.OpenBondHeadVaccum();

            // 获放吸嘴高度
            AKRSPoint3D point = this.bondProgram.NozzleShelfProgram.NozzleShelf.GetPlaceNozzleSlotPos(nozzleName);
            double pointInBond = this.bondModuleController.ConvertG0ToMachinePos(point).Z;
            MovePara runPara2 = new()
            {
                TargetPosition = pointInBond,
                Acc = this.bondHead.AxisZ.AxisMovePara.ACC,
                Dec = this.bondHead.AxisZ.AxisMovePara.DEC,
                Jerk = this.bondHead.AxisZ.AxisMovePara.Jerk,
                Vel = BondDevicePara.GetInstance().NozzleShelfParam.SlowTravelUpSpeed
            };

            // 低速上抬到放吸嘴高度
            ExcuteResult ret2 = this.bondHeadController.MoveAxisZ(runPara2);

            if (ret2 != ExcuteResult.Success)
            {
                throw new("取吸嘴流程Z轴慢速上抬到指令位置失败！");
            }

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-低速上抬到放吸嘴高度，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            if (this.system2Configuration.IsActiveToolDetection)
            {
            CheckBondhead:
                // 判断焊头有没有吸上来
                if (this.bondHeadController.IsToolOnBondheadPhy() == false)
                {
                    if (this.system2Configuration.IsRetryAfterChangeNozzleFailed &&
                        this.system2Configuration.ChangeNozzleRetryTimesLimit > changeNozzleRetryTimes)
                    {
                        // 关焊头吸附
                        this.bondHeadController.CloseBondHeadVaccum();

                        //// Z轴回到安全位
                        //this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();

                        // 先上抬使吸嘴脱离
                        this.bondHeadController.MoveAxisZ(pickPos.Z);
                        changeNozzleRetryTimes++;
                        goto RetryChangeNozzle;
                    }

                    // 报警
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $"取吸嘴流程-焊头吸附吸嘴失败，请检查焊头！",
                        "换吸嘴报警",
                        new string[] { "重试", "忽略", "终止" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    switch (dialogResult)
                    {
                        case DialogResult.Retry:
                            goto CheckBondhead;

                        case DialogResult.Ignore:
                            break;

                        case DialogResult.Abort:
                            return false;

                        default:
                            return false;
                    }
                }
            }

            if (this.system2Configuration.IsActiveBlowDuringChangeNozzle)
            {
                // 这里开吹气是防止吸嘴憋气
                Task.Run(
                    () =>
                    {
                        CommonUtil.SetCurrentThreadName("换吸嘴吹气线程");

                        System2RunTimeProvider.IsChangeToolBlowFinish = false;

                        LogHelper.Post(Level.Info,
                            $" 换吸嘴流程-焊头准备设置吹气比例"
                            , LogCategory.Bond);

                        this.bondHeadController.SetBlowProportion(this.bondDevicePara.BondHeadParam
                            .ChangeToolBlowProportion);

                        LogHelper.Post(Level.Info,
                            $" 换吸嘴流程-焊头设置吹气比例完成"
                            , LogCategory.Bond);

                        // 开吹气
                        this.bondHeadController.OpenToolBlowEle();
                        Thread.Sleep(this.bondDevicePara.BondHeadParam.ChangeToolBlowDelay);

                        LogHelper.Post(Level.Info,
                            $" 换吸嘴流程-焊头开吹气完成"
                            , LogCategory.Bond);

                        // 关吹气
                        this.bondHeadController.CloseToolBlowEle();

                        LogHelper.Post(Level.Info,
                            $" 换吸嘴流程-焊头关吹气完成"
                            , LogCategory.Bond);

                        System2RunTimeProvider.IsChangeToolBlowFinish = true;
                    });
            }


            targetPosition = this.bondModuleController.GetAxisYRealPos()
                             - BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceY;

            MovePara runPara3 = new()
            {
                TargetPosition = targetPosition,
                Acc = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.ACC,
                Dec = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.DEC,
                Jerk = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.Jerk,
                Vel = this.bondDevicePara.NozzleShelfParam.SlowTravelAwayFromSlotSpeed
            };

            // Y轴低速向外移动一段距离,将吸嘴取出
            ExcuteResult res = this.bondModuleController.MoveAxisY(runPara3);

            if (res != ExcuteResult.Success)
            {
                throw new("取吸嘴流程Y轴慢速运动到指令位置失败！");
            }

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-Y轴低速向外移动一段距离,将吸嘴取出，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

        Continue:

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle)
            {
                // 如果没有吸上来就报警
                if (this.system2Configuration.IsActiveToolDetection)
                {
                Retry:
                    if (!this.bondHeadController.IsToolOnBondheadPhy())
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"取吸嘴失败!请选择如何处理:\r\n重试:重新检查吸嘴架\r\n忽略:忽略并继续工作\r\n退出:退出工作",
                            "换吸嘴报警",
                            new string[] { "重试", "忽略", "退出" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                            AlarmLevel.SecondLevel);

                        switch (dialogResult)
                        {
                            case DialogResult.Retry:
                                goto Retry;

                            case DialogResult.Ignore:
                                break;

                            case DialogResult.Abort:
                                return false;
                        }
                    }
                }
            }

            // 刷新吸嘴槽状态
            nozzleShelf.NozzleShelfSlots[nozzle.SlotIdentification - 1].NozzleState = NozzleStateEnum.OnBondHead;

            // 刷新焊头上的吸嘴
            this.bondHeadController.SetCurrentNozzleName(nozzleName);

            // 保存
            NozzleShelfRepository.GetInstance().Save();

            return true;
        }
        catch (Exception e)
        {
            AKRSXtraMessageBox.Show(
                $"取吸嘴: {nozzleName} 失败!" + e.ToString(),
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);

            LogHelper.Post(Level.Error, $"取吸嘴失败", e, LogCategory.Bond);

            // 防撞，直接抛异常
            throw e;
            //return false;
        }
    }

    /// <summary>
    /// 放回当前焊头上的吸嘴
    /// </summary>
    /// <returns>结果</returns>
    public bool PutbackNozzle()
    {
        if (!this.system2Configuration.IsToolBankEnable)
        {
            DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                $"吸嘴架未启用，请将焊头上的吸嘴手动放回吸嘴架！\r\n",
                "提示",
                new string[] { "确认", "取消" },
                new DialogResult[] { DialogResult.OK, DialogResult.Cancel },
                AlarmLevel.SecondLevel);

            if (dialogResult == DialogResult.OK)
            {
                // 刷新焊头上的吸嘴
                this.bondHeadController.SetCurrentNozzleName(string.Empty);

                return true;
            }

            return false;
        }

        try
        {
            // 当前吸嘴架
            NozzleShelf nozzleShelf = (NozzleShelf)NozzleShelfRepository.GetInstance()
                .Find(BondProgram.GetInstance().NozzleShelfProgram.NozzleShelfName);

            // 当前焊头上的吸嘴
            Nozzle nozzle = System2Domain.GetInstance().NozzleRepository
                .GetNozzle(this.bondHeadController.GetCurrentNozzleName());

            if (nozzle == null)
            {
                return true;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle)
            {
                // 程式检查：此吸嘴是否配置
                if (this.nozzleShelfController.IsConfiguredOnToolBank(this.bondHeadController.GetCurrentNozzleName())
                    == false)
                {
                ReAlarm:
                    this.bondHeadController.CloseBondHeadVaccum();
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $" 吸嘴： {nozzle.Name} 未配置到吸嘴架，放回吸嘴失败！\r\n请检查吸嘴架配置!\r\n",
                        "换吸嘴报警",
                        new string[] { "手动拿走", "终止" },
                        new DialogResult[] { DialogResult.OK, DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    if (dialogResult == DialogResult.OK)
                    {
                        // 硬件检查焊头上是否有吸嘴
                        bool res = this.bondHeadController.IsToolOnBondheadPhy();
                        if (res)
                        {
                            goto ReAlarm;
                        }

                        this.bondHeadController.CloseBondHeadVaccum();

                        // 设置焊头上无吸嘴
                        this.bondHeadController.SetCurrentNozzleName(string.Empty);
                        return true;
                    }

                    return false;
                }


            Recheck:
                // 硬件检查此穴位上是否有吸嘴
                bool ret = this.nozzleShelfController.IsSlotHaveNozzle(nozzle.SlotIdentification);
                if (ret)
                {
                    DialogResult dialogResult = AKRSMessageBoxExt.Show(
                        $" 检测到吸嘴架槽： {nozzle.SlotIdentification}已有吸嘴，放回吸嘴失败！\r\n请检查吸嘴架!\r\n"
                        + "重试:重新检测\r\n" + "退出:退出工作",
                        "换吸嘴报警",
                        new string[] { "重试", "退出" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Abort },
                        AlarmLevel.SecondLevel);

                    if (dialogResult == DialogResult.Retry)
                    {
                        goto Recheck;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            // 获取当前吸嘴原来在哪个穴位
            int num = nozzle.SlotIdentification;

            // 找到原来穴位的位置
            AKRSPoint3D point3D = BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[num - 1];
            double angle = BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzleAngle[num - 1];

            // 计算还吸嘴位置
            AKRSPoint3D prePos = new(
                point3D.X,
                BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[num - 1].Y -
                BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceY,
                point3D.Z);

            AKRSPoint3D prePosInBond = this.bondModuleController.ConvertG0ToMachinePos(prePos);

            // 防呆
            if (prePosInBond.Z < this.bondDevicePara.BMCDevicePara.MeasureHeightResult)
            {
                // 还吸嘴高度低于BMC高度直接抛异常
                throw new("还吸嘴高度过低，请先示教吸嘴架！");
            }

            // 先抬起到预放回高度
            this.bondHeadController.MoveZAxis(prePosInBond.Z);

            // 防撞
            if (!this.IsNozzleShelfAxisYSafe())
            {
                this.bondModuleController.MoveToChangeNozzleSafePos();
            }

            ExcuteResult excuteResult = ExcuteResult.Fail;

            // 吸嘴架移动到换吸嘴架位
            Task nozzleShelfMoveTask = Task.Run(() =>
                {
                    excuteResult = this.nozzleShelfController.MoveShelfToChangeNozzlePos();
                });

            // 打印
            LogHelper.Post(Level.Info, $"换吸嘴流程-吸嘴架移动到换吸嘴架位，此时Y轴坐标：{this.nozzleShelfController.GetYAxisPos()} ",
                LogCategory.Bond);

            // 打印
            LogHelper.Post(Level.Info, $"换吸嘴流程-准备移动到预放回位 X：{prePos.X},Y:{prePos.Y},Z:{prePos.Z} ", LogCategory.Bond);

            // T轴转一定角度
            this.bondHeadController.RotateAxisT(angle);
            this.bondHeadController.MoveZAxis(prePosInBond.Z);
            this.bondModuleController.MoveBondXY(prePosInBond.X, prePosInBond.Y);

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-移动到预放回位，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            // 关焊头吸附
            this.bondHeadController.CloseBondHeadVaccum();

            // 打印
            LogHelper.Post(Level.Info, $"放回吸嘴流程 关焊头吸附 ", LogCategory.Bond);

            // 等吸嘴架到位
            nozzleShelfMoveTask.Wait();

            if (excuteResult != ExcuteResult.Success || this.nozzleShelfController.IsShelfAtChangeNozzlePos() == false)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"吸嘴架移动到换吸嘴架位失败！",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);
                LogHelper.Post(Level.Error, $"放回吸嘴流程-吸嘴架移动到换吸嘴架位失败 ", LogCategory.Bond);
                throw new("放回吸嘴流程-吸嘴架移动到换吸嘴架位失败！");
            }

            LogHelper.Post(Level.Info, $"放回吸嘴流程 等吸嘴架到位 ", LogCategory.Bond);

            // 防呆
            if (Math.Abs(this.bondHeadController.GetAxisTRealPos() - angle) > 0.1)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"T轴转到指令位置失败！",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return false;
            }

            double targetPosition = this.bondModuleController.ConvertG0ToMachinePos(point3D).Y;

            MovePara runPara1 = new()
            {
                TargetPosition = targetPosition,
                Acc = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.ACC,
                Dec = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.DEC,
                Jerk = System2Module.GetInstance().BondModule.BondAxisY.AxisMovePara.Jerk,
                Vel = this.bondDevicePara.NozzleShelfParam.SlowTravelToSlotSpeed
            };

            // Y轴低速向正方向移动一段距离
            ExcuteResult ret1 = this.bondModuleController.MoveAxisY(runPara1);

            if (ret1 != ExcuteResult.Success)
            {
                throw new("放回吸嘴流程Y轴慢速运动到指令位置失败！");
            }

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-Y轴低速向正方向移动3mm完成，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            targetPosition = this.bondHeadController.GetAxisZRealPos() + 3;

            MovePara runPara2 = new()
            {
                TargetPosition = targetPosition,
                Acc = this.bondHead.AxisZ.AxisMovePara.ACC,
                Dec = this.bondHead.AxisZ.AxisMovePara.DEC,
                Jerk = this.bondHead.AxisZ.AxisMovePara.Jerk,
                Vel = 100
            };

            // z轴低速向上移动一段距离:3mm,使吸嘴脱离
            ExcuteResult ret2 = this.bondHeadController.MoveAxisZ(runPara2);

            if (ret2 != ExcuteResult.Success)
            {
                throw new("放回吸嘴流程Z轴慢速上抬到指令位置失败！");
            }

            LogHelper.Post(Level.Info,
                $" 换吸嘴流程-Z轴低速向上移动3mm，此时各轴坐标：" +
                $" X：{this.bondModuleController.GetAxisXRealPos()}," +
                $" Y: {this.bondModuleController.GetAxisYRealPos()}" +
                $" T: {this.bondHeadController.GetAxisTRealPos()}," +
                $" Z: {this.bondHeadController.GetAxisZRealPos()} "
                , LogCategory.Bond);

            // 回到安全位置,这之前应该要关真空
            this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();

            // 打印
            LogHelper.Post(Level.Info, $"换吸嘴流程完成 Z轴回到安全位置 ", LogCategory.Bond);

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle)
            {
                // 如果没有放好就报警
                if (this.system2Configuration.IsActiveToolDetection)
                {
                Retry:
                    bool res = this.bondHeadController.IsToolOnBondheadPhy();
                    this.bondHeadController.CloseBondHeadVaccum();
                    if (res)
                    {
                        DialogResult dialogResult = AKRSMessageBoxExt.Show(
                            $"吸嘴 :{nozzle.Name}放回吸嘴架失败！\r\n请检查吸嘴是否已放在吸嘴架槽： {num} ! \r\n",
                            "换吸嘴报警",
                            new string[] { "重新检测", "忽略", "退出" },
                            new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                            AlarmLevel.SecondLevel);

                        switch (dialogResult)
                        {
                            case DialogResult.Retry:

                                goto Retry;

                            case DialogResult.Abort:

                                return false;

                            case DialogResult.Ignore:

                                break;
                        }
                    }
                }
            }

            // 刷新吸嘴槽状态
            nozzleShelf.NozzleShelfSlots[nozzle.SlotIdentification - 1].NozzleState = NozzleStateEnum.OnSlot;

            // 刷新焊头上的吸嘴
            this.bondHeadController.SetCurrentNozzleName(string.Empty);

            // 保存
            NozzleShelfRepository.GetInstance().Save();

            return true;
        }
        catch (Exception e)
        {
            AKRSXtraMessageBox.Show(
                $"放回吸嘴:{this.bondHeadController.GetCurrentNozzleName()}  失败!" + e.ToString(),
                "Warn",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            LogHelper.Post(Level.Error, $"放回当前焊头上的吸嘴失败", e, LogCategory.Bond);

            // 防撞，直接抛异常
            throw e;
            //return false;
        }
    }

    /// <summary>
    /// 换吸嘴方法
    /// </summary>
    /// <param name="nozzleName">吸嘴名称</param>
    /// <returns>结果</returns>
    public bool ChangeNozzle(string nozzleName)
    {
        // 检查吸嘴架是否启用
        if (!this.system2Configuration.IsToolBankEnable)
        {
            this.bondHeadController.CloseBondHeadVaccum();

            DialogResult dialogResult = AKRSMessageBoxExt.ShowWarn(
                $"吸嘴架未启用，请手动更换吸嘴：{nozzleName}!\r\n",
                "提示",
                new string[] { "确认", "取消" },
                new DialogResult[] { DialogResult.OK, DialogResult.Cancel },
                AlarmLevel.SecondLevel);

            if (dialogResult == DialogResult.OK)
            {
                // 刷新焊头上的吸嘴
                this.bondHeadController.SetCurrentNozzleName(nozzleName);

                this.bondHeadController.OpenBondHeadVaccum();

                return true;
            }

            return false;
        }

        bool isToolOnHead;

        // 1. 判断焊头上有没有吸嘴， 如果有和tool bank的配置是否对应 如果不对应则报警，如果对应则先放回去。
        bool isToolOnHeadLogic = this.bondHeadController.IsToolOnBondheadLogic();
        bool isToolOnHeadPhy = this.bondHeadController.IsToolOnBondheadPhy();

        this.bondHeadController.CloseBondHeadVaccum();

        if (this.system2Configuration.IsActiveToolDetection)
        {
            isToolOnHead = isToolOnHeadPhy;
            if (isToolOnHeadPhy == true && isToolOnHeadLogic == false)
            {
                // 报警
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"焊头上检测到有吸嘴，与程式冲突！换吸嘴失败！\r\n",
                    "换吸嘴报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return false;
            }
            else if (isToolOnHeadPhy == false && isToolOnHeadLogic == true)
            {
                // 如果焊头检测没有，但记忆中有吸嘴
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"记忆中有吸嘴但焊头上未检测到吸嘴！\r\n OK:默认此时焊头上没有吸嘴\r\n Ignore:忽略并继续工作\r\nAbort:退出工作",
                    "换吸嘴报警",
                    new string[] { "确认", "忽略", "终止" },
                    new DialogResult[] { DialogResult.OK, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                switch (dialogResult)
                {
                    case DialogResult.OK:

                        this.bondHeadController.SetCurrentNozzleName(string.Empty);
                        isToolOnHead = this.bondHeadController.IsToolOnBondheadPhy();
                        break;

                    case DialogResult.Ignore:
                        break;

                    case DialogResult.Abort:

                        return false;

                    default: return false;
                }
            }
        }
        else
        {
            isToolOnHead = isToolOnHeadLogic;
        }

        if (isToolOnHead)
        {
            // 放回当前焊头上的吸嘴
            if (!this.PutbackNozzle())
            {
                return false;
            }
        }

        // 取吸嘴
        if (!this.TakeNozzle(nozzleName))
        {
            return false;
        }

        // 吸嘴架回原
        this.nozzleShelfController.MoveShelfToHome();

        return true;
    }

    /// <summary>
    /// 判断并换吸嘴
    /// </summary>
    /// <param name="nozzleName">吸嘴名称</param>
    /// <returns>结果</returns>
    public bool JudgeAndChangeNozzle(string nozzleName)
    {
        bool isNeedChange = this.IsNeedToChangeNozzle(nozzleName);
        if (isNeedChange)
        {
           return  this.ChangeNozzle(nozzleName);
        }

        return true;
    }

    /// <summary>
    /// 切换TouchDown
    /// </summary>
    /// <returns>结果</returns>
    public bool SwitchTouchDown()
    {
        if (this.bondHeadController.GetCurrentNozzleName() == "TouchDown")
        {
            return true;
        }

        return this.ChangeNozzle("TouchDown");
    }

    /// <summary>
    /// 判断是否需要换吸嘴
    /// </summary>
    /// <param name="nozzleName">吸嘴名</param>
    /// <returns>是否需要换吸嘴</returns>
    public bool IsNeedToChangeNozzle(string nozzleName)
    {
        // 不一样就要换
        return this.bondHeadController.GetCurrentNozzleName() != nozzleName;
    }

    /// <summary>
    /// 吸嘴架Y轴是否安全
    /// </summary>
    /// <returns>结果</returns>
    public bool IsNozzleShelfAxisYSafe()
    {
        double pos = this.bondModuleController.GetG0RealPosition().Y;

        //GeneralCoordinateSystem toolBankCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "ToolBankCoordinateSystem");

        //if (toolBankCoordinateSystem == null)
        //{
        //    DialogResult dialog = XtraMessageBox.Show(
        //        $"吸嘴架坐标系为空，请先示教吸嘴架!",
        //        "报警",
        //        MessageBoxButtons.OK,
        //    MessageBoxIcon.Warning);

        //    return false;
        //}

        // 预还吸嘴位
        double prePutBackPos = BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[0].Y -
                               BondDevicePara.GetInstance().NozzleShelfParam.ChangeToolDistanceY;

        return this.bondModuleController.GetG0RealPosition().Y
               <= prePutBackPos;
    }

    /// <summary>
    /// 获取流道安全高度，机械坐标
    /// </summary>
    /// <returns>高度</returns>
    public double GetTUMachineSafeHeight()
    {
        // 计算抬起高度
        double safeHeight = this.bondModuleController.ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, 0)).Z;

        if (this.transportUnit != null)
        {
            safeHeight = this.transportUnit.CoordinateSystem.SelfPosToG0(new()).Z
                         + ProductConfiguration.GetInstance().TransportUnitConfig.SafeHeight
                         + (this.bondHeadController.GetCurrentNozzle()?.MeasureHeightOffset ?? 0);
        }

        safeHeight = this.bondModuleController.ConvertG0ToMachinePos(new(0, 0, safeHeight)).Z;

        return safeHeight;
    }

    /// <summary>
    /// 获取清洁的位置(G0)
    /// </summary>
    /// <returns>点位</returns>
    public AKRSPoint3D GetCleanPos()
    {
        this.InitCleanTable();

        if (bondProgram.CleanNozzleProgram.Count >= (bondProgram.CleanNozzleProgram.CleanArea.Length * bondProgram.CleanNozzleProgram.HeatActivatedFilmUseLimit))
        {
            AKRSMessageBoxExt.Show(
                     "吸嘴清洁布达到使用次数上限，请更换清洁布!",
                     "报警",
                     new string[] { "确定" },
                     new DialogResult[] { DialogResult.Yes });
            bondProgram.CleanNozzleProgram.Count = 0;
        }

        int column = 0;
        int row = 0;

        // 所有的点都点完
        int count = bondProgram.CleanNozzleProgram.Count / bondProgram.CleanNozzleProgram.HeatActivatedFilmUseLimit;

        if (bondProgram.CleanNozzleProgram.Row != 0 && bondProgram.CleanNozzleProgram.Column != 0)
        {
            column = count / bondProgram.CleanNozzleProgram.Row;
            row = count - column * bondProgram.CleanNozzleProgram.Row;
        }

        bondProgram.CleanNozzleProgram.Count++;

        AKRSPoint3D point3D = bondProgram.CleanNozzleProgram.CleanArea[row, column];

        BondProgram.GetInstance().Save();

        return point3D;
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void InitCleanTable()
    {
        // 计算行列间距
        double rowSpacing = (this.bondHeadParam.NozzleCleanTableLeftTopPos.Y
                                                                        - this.bondHeadParam.NozzleCleanTableRightBottomPos.Y)
                                                                       / this.bondProgram.CleanNozzleProgram.Row;

        double columnSpacing = (this.bondHeadParam.NozzleCleanTableRightBottomPos.X
                                                                            - this.bondHeadParam.NozzleCleanTableLeftTopPos.X)
                                                                           / this.bondProgram.CleanNozzleProgram.Column;

        bondProgram.CleanNozzleProgram.CleanArea = new AKRSPoint3D[this.bondProgram.CleanNozzleProgram.Row, this.bondProgram.CleanNozzleProgram.Column];

        if (this.bondProgram.CleanNozzleProgram.Column <= 0)
        {
            throw new Exception("吸嘴清洁台列数为负数！");
        }

        if (this.bondProgram.CleanNozzleProgram.Row <= 0)
        {
            throw new Exception("吸嘴清洁台行数为负数！");
        }

        for (int i = 0; i < this.bondProgram.CleanNozzleProgram.Row; i++)
        {
            for (int j = 0; j < this.bondProgram.CleanNozzleProgram.Column; j++)
            {
                bondProgram.CleanNozzleProgram.CleanArea[i, j] = new AKRSPoint3D(
                    this.bondHeadParam.NozzleCleanTableLeftTopPos.X + j * columnSpacing,
                    this.bondHeadParam.NozzleCleanTableLeftTopPos.Y - i * rowSpacing,
                    this.bondHeadParam.NozzleCleanTableLeftTopPos.Z);
            }
        }

        BondProgram.GetInstance().Save();
    }

    /// <summary>
    /// 吸嘴擦拭动作
    /// </summary>
    public void CleanNozzle()
    {
        if (MachineHardwareConfiguration.GetInstance().IsNozzleCleanTableConfigured == false)
        {
            throw new Exception("硬件配置错误，吸嘴清洁失败，请先配置清洁台！");
        }

        Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
        AKRSPoint3D cleanPos = this.bondModuleController.ConvertG0ToMachinePos(this.GetCleanPos());

        double cleanHeight = cleanPos.Z + nozzle.MeasureHeightOffset;
        double force = this.bondProgram.CleanNozzleProgram.CleanNozzleBeforePickupForce;

        // 去清洁位
        this.bondHeadController.MoveBondZToSafePos();
        this.bondModuleController.MoveSafeBondXY(cleanPos.X, cleanPos.Y);
        this.bondHeadController.RotateAxisT(nozzle.AlignAngle);

        // 高速运动到预清洁位
        this.bondHeadController.MoveAxisZ(cleanHeight + 2);

        for (int i = 0; i < this.bondProgram.CleanNozzleProgram.CleanNozzleBeforePickupTimes; i++)
        {
            // 距离、力控
            if (this.bondProgram.CleanNozzleProgram.CleanNozzleBeforePickupForceMode == ForceModeEnum.Distance)
            {
                // 低速运动到预清洁位
                this.bondHeadController.MoveAxisZ(cleanHeight, 100, AccuracyMode.HighAccuracy, false);

                // 焊头抬起
                this.bondHeadController.MoveAxisZ(cleanHeight + 2, 100,AccuracyMode.HighAccuracy, false);
            }
            else
            {
                // 低速运动到预清洁位
                this.bondHeadController.ForceControlSet(force, cleanHeight, 100, 10,2);

                this.bondHeadController.ForceControlReset(cleanHeight + 2, 100);
            }
        }

        // 去安全高度
        this.bondHeadController.MoveBondZToSafePos();

        // 吹气
        this.bondHeadController.OpenToolBlowEle();
        Thread.Sleep(50);
        this.bondHeadController.CloseToolBlowEle();
    }

    /// <summary>
    ///  获取焊头压力表模拟量
    /// </summary>
    /// <returns>值</returns>
    public double GetBondheadForceValue()
    {
        if (HardwareRepositoryService.GetAxisCard(1).MotionController.MotionControllerDrive is GTController)
        {
            // 56号机是通道1
            short rtn = GTN.mc.GTN_GetAuAdc((short)this.bondHead.AxisZ.CardNum, (short)bondHead.ReadBondheadForce.SensorIO, out double pValue, 1, out UInt32 p2Clock);

            double read = Math.Round((pValue / 32767) * ForceConfig.GetInstance().BondheadMaxPress, 6) * 1000.0;

            return read;
        }
        else
        {
            int[] read = ModbusService.GetInstance().ReadBondForce();

            return read[0] / 10.0;
        }
    }

    /// <summary>
    ///  获取校正台压力表模拟量
    /// </summary>
    /// <returns>值</returns>
    public double GetCalibrateTableForceValue()
    {
        if (HardwareRepositoryService.GetAxisCard(1).MotionController.MotionControllerDrive is GTController)
        {
            // 6号机通道是2，5号机通道是7
            short rtn = GTN.mc.GTN_GetAuAdc((short)this.bondHead.AxisZ.CardNum, (short)this.bondHead.ReadCalibrateTableForce.SensorIO, out double pValue, 1, out UInt32 p3Clock);

            double read = Math.Round((pValue) * ForceConfig.GetInstance().CalibrateTableMaxPress, 6) * 100.0;

            return read;
        }
        else
        {
            int[] read = ModbusService.GetInstance().ReadManometer();

            return read[0] / 10.0;
        }
    }

    /// <summary>
    /// 蘸胶动作
    /// </summary>
    /// <param name="dipLevel">蘸胶高度</param>
    /// <param name="component">芯片</param>
    /// <returns>结果</returns>
    public ExcuteResult DipFlux(double dipLevel, BaseCarrierConfig component)
    {
        System2RunTimeProvider.RecordTime("蘸胶", "蘸胶  开始");

        try
        {
            DipActionParamter dipActionParameter =
                this.dipActionProvider.GetDipActionParameter(component);

            // 预备抬起位
            double liftPreLevel = dipLevel + dipActionParameter.SlowTravelDistanceAfterDip;

            // 当前速度
            double speed = this.bondHeadController.GetAxisZAbsoluteSpeed();

            if (component.BondingForceMode == ForceModeEnum.Distance)
            {
                if (component.IsActivateSlowTravelBeforeBonding)
                {
                    // 低速移动到蘸胶高度  如果未开启二段速 则在前一步Jump的时候已经到达了蘸胶高度
                    this.bondHeadController.MoveAxisZ(dipLevel, dipActionParameter.SlowTravelSpeedBeforeDip, AccuracyMode.HighAccuracy, false);

                    System2RunTimeProvider.RecordTime("蘸胶", "低速移动到固晶高度完成");
                }
            }
            else
            {
                System2RunTimeProvider.RecordTime("蘸胶", "准备力控模式下压");

                double curLevel = this.bondHeadController.GetAxisZRealPos();

                // 力控模式直接从当前位置进入力控模式
                this.bondHeadController.ForceControlSet(
                    dipActionParameter.DipForce,
                    curLevel - 0.01,
                    speed,
                    component.ForceControlOutTime,
                    dipActionParameter.SlowTravelSpeedBeforeDip);

                System2RunTimeProvider.RecordTime("蘸胶", $"力控下压完成  Speed: {speed}");
            }

            int dipDelay = dipActionParameter.DipDelay;

            // 延迟
            DelayHelper.Delay(dipDelay);

            System2RunTimeProvider.RecordTime("蘸胶", $"蘸胶延时: {dipActionParameter.DipDelay}ms");

            if (component.BondingForceMode == ForceModeEnum.Distance)
            {
                if (component.IsActivateSlowTravelAfterDip)
                {
                    // 低速上抬到预固晶位
                    this.bondHeadController.MoveAxisZ(
                        liftPreLevel,
                        dipActionParameter.SlowTravelSpeedAfterDip,
                        AccuracyMode.HighSpeed,false);
                }
            }
            else
            {
                if (component.IsActivateSlowTravelAfterDip)
                {
                    // 加这句是为了防止Z轴抬起时没有恢复正常速度
                    this.bondHeadController.SetAxisZSpeed(dipActionParameter.SlowTravelSpeedAfterDip);

                    liftPreLevel = this.bondHeadController.GetAxisZRealPos()
                                   + dipActionParameter.SlowTravelDistanceAfterDip;

                    // 力控模式低速速上抬
                    this.bondHeadController.ForceControlReset(
                        liftPreLevel,
                        dipActionParameter.SlowTravelSpeedAfterDip);

                    System2RunTimeProvider.RecordTime(
                        "蘸胶",
                        $"力控低速上抬完成，速度{dipActionParameter.SlowTravelSpeedAfterDip}");
                }
                else
                {
                    // 原地退出力控模式
                    this.bondHeadController.ForceControlReset(this.bondHeadController.GetAxisZRealPos(), speed);
                }
            }

            System2RunTimeProvider.RecordTime("蘸胶", $"蘸胶  完成");
        }
        catch (Exception ex)
        {
            LogHelper.Post(Level.Error, $"蘸胶失败", ex, LogCategory.Bond);

            DialogResult dialogResult = AKRSMessageBoxExt.Show(
                $"蘸胶失败! \r\n" + ex.ToString(),
                "蘸胶报警",
                new string[] { "确认" },
                new DialogResult[] { DialogResult.OK },
                AlarmLevel.SecondLevel);

            return ExcuteResult.Exception;
        }

        return ExcuteResult.Success;
    }

    /// <summary>
    /// BLT检测
    /// </summary>
    /// <param name="measureHeightPosList">测高点位(基于焊头)</param>
    /// <returns>结果</returns>
    public List<double> LaserMeasureHeight(List<AKRSPoint3D> measureHeightPosList)
    {
        // 测高结果
        List<double> res = new List<double>();

        System2RunTimeProvider.RecordTime($"BLT检测", $"准备激光测高");

        foreach (var g0Pos in measureHeightPosList)
        {
            // 移动到该位置
            this.s2DispenseController.MeasureHeightMoveToG0Pos(g0Pos);

            System2RunTimeProvider.RecordTime($"BLT检测", $"轴移动到测高位置完成{g0Pos}");

            // 打开测高气缸
            System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();

            System2RunTimeProvider.RecordTime($"BLT检测", $"打开测高气缸完成");

            // 到位等待
            Thread.Sleep(this.bondDevicePara.S2DispenseDevicePara.LaserMhDelayTime);

            System2RunTimeProvider.RecordTime(
                $"BLT检测",
                $"结束到位延迟完成，延时为{this.bondDevicePara.S2DispenseDevicePara.LaserMhDelayTime}");

        RetryCommand:

            // 测高
            double heightInMachine = this.s2DispenseController.LaserMeasureHeight();

            System2RunTimeProvider.RecordTime($"BLT检测", $"测高完成，高度为{heightInMachine}");

            // 高度预警
            if (Math.Abs(heightInMachine) > this.bondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"测高数值为{heightInMachine}mm，超出设置阈值{this.bondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance}\r\n"
                    + $"请选择如何处理",
                    "测高高度过大报警",
                    new string[] { "重试", "忽略", "终止" },
                    new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort },
                    AlarmLevel.FirstLevel);

                switch (dialogResult)
                {
                    case DialogResult.Retry:
                        goto RetryCommand;
                    case DialogResult.Ignore:
                        break;
                    case DialogResult.Abort:
                        throw new Exception(
                            $"测高数值为{heightInMachine}mm，超出设置阈值{this.bondDevicePara.S2DispenseDevicePara.MeasureHeightAlarmDistance}");
                }
            }

            res.Add(heightInMachine);
        }

        System2RunTimeProvider.RecordTime($"BLT检测", $"激光测高完成");

        return res;
    }

    /// <summary>
    /// 获取硬件集合
    /// </summary>
    /// <returns>结果</returns>
    public List<string> GetHardWareNames()
    {
        List<string> list = new List<string>();

        #region BondModule

        list.Add("BondZ");
        list.Add("焊头T");
        list.Add("焊头吸芯片负压源检测");
        list.Add("焊头吸吸嘴检测");
        list.Add("焊头吹气电磁阀");
        list.Add("吹气比例阀设置");
        list.Add("焊头取片真空电磁阀");

        list.Add("焊头气浮正压检测");

        list.Add("焊头吸吸嘴真空电磁阀");
        list.Add("焊头压力传感器清零");
        //list.Add("标定台压力传感器清零");

        list.Add("校正台压力表模拟量读取");
        list.Add("焊头压力表模拟量读取");
        list.Add("焊头LVDT");

        list.Add("BondX");
        list.Add("BondY");

        if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
        {
            list.Add("邦头点胶测高气缸电磁阀");
            list.Add("邦头点胶测高气缸原点升起检测");
            list.Add("邦头点胶测高气缸动点下降检测");

            list.Add("点胶控制2");
            list.Add("点胶器2");
        }

        if (MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh)
        {
            list.Add("Bond激光测高");
        }

        list.Add("BOND相机");
        //strings.Add("BondZ标定相机");

        if (MachineHardwareConfiguration.GetInstance().LightConfig == LightConfigEnum.MonochromaticLight)
        {
            list.Add("邦头三色点光-红");
            list.Add("邦头三色环光-红");
        }
        else
        {
            list.Add("邦头三色点光-红");
            list.Add("邦头三色点光-绿");
            list.Add("邦头三色点光-蓝");
            list.Add("点胶三色环光-红");
            list.Add("邦头三色环光-绿");
            list.Add("邦头三色环光-蓝");
        }

        #endregion

        #region UplookModule

        list.Add("上视相机");
        list.Add("上视环光");
        list.Add("上视点光");

        #endregion

        #region SlideFluxer

        if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
        {
            list.Add("刮胶盘气缸");
            list.Add("刮胶盘正限位");
            list.Add("刮胶盘负限位");
        }

        #endregion

        #region IPTModule

        if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated)
        {
            list.Add("中转台真空电磁阀");
            list.Add("中转台吹气");
            //strings.Add("中转台旋转轴");
        }

        #endregion

        #region NozzleShelfModule

        list.Add("焊头放置架Y");
        list.Add("焊头吸嘴从右到左1检测");
        list.Add("焊头吸嘴从右到左2检测");
        list.Add("焊头吸嘴从右到左3检测");
        list.Add("焊头吸嘴从右到左4检测");
        list.Add("焊头吸嘴从右到左5检测");
        list.Add("焊头吸嘴从右到左6检测");
        list.Add("焊头吸嘴从右到左7检测");

        #endregion

        list.Add("BMC真空电磁阀");


        return list;
    }
}