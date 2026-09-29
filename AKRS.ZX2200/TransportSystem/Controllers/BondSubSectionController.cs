#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/23 18:06:05
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using LanguageExt.UnitsOfMeasure;
    using Newtonsoft.Json;
    using System.Diagnostics;

    /// <summary>
    /// 描述： Bond载台控制器
    /// </summary>
    public class BondSubSectionController : BaseSubSectionController
    {
        /// <summary>
        /// 上料载台模组
        /// </summary>
        private BondSubSectionModule bondSubSectionModule => TransportModule.GetInstance().BondSubSectionModule;

        /// <summary>
        /// 点胶载台模组
        /// </summary>
        private DispenseSubSectionModule dispenseSubSectionModule => TransportModule.GetInstance().DispenseSubSectionModule;

        /// <summary>
        /// 上料程式
        /// </summary>
        public BondSubSectionProgram bondSubSectionProgram => TransportProgram.GetInstance().BondSubSectionProgram;


        /// <summary>
        /// 硬件模组
        /// </summary>
        public BondMaxSubSectionModule Module { get; set; } = new BondMaxSubSectionModule();


        /// <summary>
        /// 构造函数
        /// </summary>
        public BondSubSectionController()
        {
            this.SubSectionModule = TransportModule.GetInstance().BondSubSectionModule;
            this.SubSectionProgram = TransportProgram.GetInstance().BondSubSectionProgram;
        }

        /// <summary>
        /// 是否加热完成
        /// </summary>
        [JsonIgnore]
        public bool IsHeatingComplete { get; set; } = false;

        /// <summary>
        /// 挡料气缸升起 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinderUpAndDelay(int afterDelay)
        {
            this.bondSubSectionModule.BlockCylinderUp();
            Thread.Sleep(afterDelay);

            // Todo 判断限位
        }

        /// <summary>
        /// 挡料气缸降下 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinderDownAndDelay(int afterDelay)
        {
            this.bondSubSectionModule.BlockCylinderDown();
            Thread.Sleep(afterDelay);

            // Todo 判断限位
        }

        /// <summary>
        /// 轨道气缸顶起
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void TrackLiftCylinderUpAndDelay(int afterDelay)
        {
            this.bondSubSectionModule.TrackLiftCylinderUp();
            Thread.Sleep(afterDelay);

            // Todo 判断限位
        }

        /// <summary>
        /// 轨道气缸降落
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void TrackLiftCylinderDownAndDelay(int afterDelay)
        {
            this.bondSubSectionModule.TrackLiftCylinderDown();
            Thread.Sleep(afterDelay);

            // Todo 判断限位
        }

        /// <summary>
        /// 夹紧
        /// </summary>
        /// <exception cref="Exception">夹紧夹爪后，气缸限位传感器没能处于正确的状态</exception>
        public void Clamp()
        {
            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                if (this.bondSubSectionProgram.TransportBeltSetting == null)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"传输系统皮带设置为空，夹紧夹爪失败!  \r\n请先设置传输皮带参数！",
                        "报警",
                        new string[] { "确认" },
                        new DialogResult[] { DialogResult.OK });

                    return;
                }

            RetryCommand:
                switch (this.bondSubSectionProgram.TransportBeltSetting.ClampMode)
                {
                    case ClampModeEnum.Lowering:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                        break;

                    case ClampModeEnum.Vacuum:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                        break;

                    case ClampModeEnum.LoweringAndVacuum:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                        break;

                    case ClampModeEnum.VacuumAndLowering:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                        break;
                }

                Thread.Sleep(500);
                bool ret1 = this.bondSubSectionModule.TrackFrontCylinder1PLSensor.GetInputValue();
                bool ret2 = this.bondSubSectionModule.TrackFrontCylinder2PLSensor == null
                            || this.bondSubSectionModule.TrackFrontCylinder2PLSensor.GetInputValue();
                bool ret3 = this.bondSubSectionModule.TrackBackCylinder1PLSensor == null
                            || this.bondSubSectionModule.TrackBackCylinder1PLSensor.GetInputValue();

                if ((ret1 && ret2 && ret3) == false)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"固晶工作台夹紧夹爪失败，请检查传感器状态!  \r\n"
                        + "重试：重新松开夹爪\r\n"
                        + "忽略：忽略这个报警\r\n"
                        + "退出：退出 \r\n",
                        "报警",
                        new string[] { "重试", "忽略", "退出" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort, });
                    switch (dialog)
                    {
                        case DialogResult.Retry:
                            goto RetryCommand;

                        // 终止
                        case DialogResult.Abort:
                            Machine.GetInstance().Stop();
                            return;

                        // 忽略
                        case DialogResult.Ignore:
                            break;
                    }
                }
            }
            else
            {
                this.OpenVacuum();
            }
        }

        /// <summary>
        /// 松开夹爪
        /// </summary>
        /// <exception cref="Exception">松开夹爪后，气缸限位传感器没能处于正确的状态</exception>
        public void UnClamp()
        {
            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                if (this.bondSubSectionProgram.TransportBeltSetting == null)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"传输系统皮带设置为空，松开夹爪失败!  \r\n请先设置传输皮带参数！",
                        "报警",
                        new string[] { "确认" },
                        new DialogResult[] { DialogResult.OK });

                    return;
                }

            RetryCommand:
                switch (this.bondSubSectionProgram.TransportBeltSetting.UnClampMode)
                {
                    case UnClampModeEnum.Rising:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                        break;

                    case UnClampModeEnum.Vacuum:
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                        break;

                    case UnClampModeEnum.RisingAndVacuum:
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);

                        break;

                    case UnClampModeEnum.VacuumAndRising:
                        this.bondSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                        this.bondSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);
                        Thread.Sleep(this.bondSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                        break;
                }

                Thread.Sleep(500);

                bool ret1 = this.bondSubSectionModule.TrackFrontCylinder1NLSensor.GetInputValue();
                bool ret2 = this.bondSubSectionModule.TrackFrontCylinder2NLSensor == null
                            || this.bondSubSectionModule.TrackFrontCylinder2NLSensor.GetInputValue();
                bool ret3 = this.bondSubSectionModule.TrackBackCylinder1NLSensor == null
                            || this.bondSubSectionModule.TrackBackCylinder1NLSensor.GetInputValue();

                if ((ret1 && ret2 && ret3) == false)
                {
                    DialogResult dialog = AKRSMessageBoxExt.Show(
                        $"固晶工作台松开夹爪失败，请检查传感器状态!  \r\n"
                        + "重试：重新松开夹爪\r\n"
                        + "忽略：忽略这个报警\r\n"
                        + "退出：退出 \r\n",
                        "报警",
                        new string[] { "重试", "忽略", "退出" },
                        new DialogResult[] { DialogResult.Retry, DialogResult.Ignore, DialogResult.Abort, });

                    switch (dialog)
                    {
                        case DialogResult.Retry:
                            goto RetryCommand;

                        // 终止
                        case DialogResult.Abort:
                            Machine.GetInstance().Stop();
                            return;

                        // 忽略
                        case DialogResult.Ignore:
                            break;
                    }

                    //throw new Exception("固晶工作台松开夹爪失败，请检查传感器状态");
                }
            }
            else
            {
                this.CloseVacuum();
            }
        }

        /// <summary>
        /// 手动关闭真空
        /// </summary>
        public void ManualCloseVacuum()
        {
            this.Module.BondMaxSubSectionVacuumMiddle?.SetOutputValue(false);
            this.Module.BondMaxSubSectionVacuumFront?.SetOutputValue(false);
            this.Module.BondMaxSubSectionVacuumRight?.SetOutputValue(false);
        }

        /// <summary>
        ///  搜索皮带2系统2
        /// </summary>
        /// <returns>
        /// true: 搜索到并传料成功，
        /// false：没搜索到或者搜索到了后传料失败
        /// 里面不涉及任何状态参数的修改
        /// </returns>
        public override bool SearchBelt()
        {
            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                retry:
                // bond夹爪归位，bond气缸升起
                this.UnClamp();

                // 点胶载台皮带先进行倒转
                double transportDistance =
                    this.bondSubSectionProgram.TransportBeltSetting.BackwardTransportDistance;

                this.bondSubSectionModule.SendRelativeMoveCommandBelt(
                    -transportDistance,
                    this.bondSubSectionProgram.TransportBeltSetting.BackwardVelRate);

                // 入料感应器是否感应到料
                bool isSensorTrigger = false;

                // 检测传感器 判断状态
                while (this.bondSubSectionModule.IsMoving())
                {
                    // 如果点胶出料口有料
                    if (this.dispenseSubSectionModule.HasMaterialInOutlet())
                    {
                        isSensorTrigger = true;

                        // 停止倒转
                        this.StopMove();
                        this.dispenseSubSectionModule.StopMove();

                        Thread.Sleep(100);
                        break;
                    }
                }

                // 如果没感应到
                if (isSensorTrigger == false)
                {
                    return false;
                }

                Thread.Sleep(50);

                // Bond挡料气缸1顶起
                this.BlockCylinderUpAndDelay(50);

                // 检测到了 正转
                this.bondSubSectionModule.RelativeMoveBelt(
                    this.bondSubSectionProgram.TransportBeltSetting.ForwardTransportDistance,
                    this.bondSubSectionProgram.TransportBeltSetting.ForwardVelRate);

                // 此时如果点胶出料口有料 则是卡料 报警
                if (this.dispenseSubSectionModule.HasMaterialInOutlet())
                {
                    // 报警：入料卡料
                    DialogResult dr = AKRSMessageBoxExt.Show(
                        @"固晶工作台卡料，请选择如何处理
                              点击
                              重试 : 重新搜索
                              忽略 : 忽略",
                        $"报警",
                        new[] { "重试", "忽略" },
                        new[] { DialogResult.Retry, DialogResult.Ignore });

                    switch (dr)
                    {
                        case DialogResult.Retry:
                            goto retry;

                        case DialogResult.Ignore:
                            return false;
                    }
                }

                // 传输成功,夹紧夹爪
                this.Clamp();

                return true;
            }
            else
            {
                try
                {
                    this.OpenVacuumAndCheck();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 搜索载具
        /// </summary>
        /// <param name="autoWork">是否是在自动工作的时候</param>
        public override void MapBelt(bool autoWork)
        {
            if (MachineHardwareConfiguration.GetInstance().IsTransportConfigured)
            {
                // 只有当流道不反找且是自动工作且轨道上有料的时候才不反照
                if (!TransportDevicePara.GetInstance().IsReverseSearch
                    && autoWork
                    && this.bondSubSectionProgram.TransportUnit != null)
                {
                    return;
                }

                bool hasMaterial = this.SearchBelt();

                // 如果检测到有料
                if (hasMaterial)
                {
                    // 当缓存中没有料 
                    if (this.bondSubSectionProgram.TransportUnit == null)
                    {
                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                            @" 设备记忆中系统2没有载具，此次搜索发现了载具，请确认是否真的存在载具。
                              有:  有载具, 设备将记录此载具
                              没有: 没有载具",
                            $"警告",
                            new[] { "有", "没有" },
                            new[] { DialogResult.Yes, DialogResult.No });

                        switch (dr)
                        {
                            case DialogResult.Yes:

                                this.bondSubSectionProgram.TransportUnit =
                                    new TransportUnit(CurrentMachineSystemEnum.System2);

                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                                break;

                            case DialogResult.No:
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                                break;
                        }
                    }
                    else   // 如果缓存中有料
                    {
                        this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
                        this.bondSubSectionProgram.TransportUnit.Refresh();
                    }
                }
                else   // 没有搜索到料
                {
                    if (this.bondSubSectionProgram.TransportUnit != null)
                    {
                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                            @" 设备记忆中系统2有载具，此次收索没有发现载具，请确认是否真的存在载具。
                              有:  有载具
                              没有: 没有载具, 设备将清空此载具记忆",
                            $"警告",
                            new[] { "有", "没有" },
                            new[] { DialogResult.Yes, DialogResult.No });

                        switch (dr)
                        {
                            case DialogResult.Yes:
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                                break;

                            case DialogResult.No:
                                this.bondSubSectionProgram.TransportUnit = null;
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                                break;
                        }
                    }
                    else   // 如果没料
                    {
                        this.bondSubSectionProgram.TransportUnit = null;
                        this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                    }
                }
            }
            else
            {
                // 只有当流道不反找且是自动工作且轨道上有料的时候才不反照
                if (!TransportDevicePara.GetInstance().IsReverseSearch
                    && autoWork
                    && this.bondSubSectionProgram.TransportUnit != null)
                {
                    return;
                }

            RetryCommand:

                bool isSuccess = this.OpenVacuumAndCheck();

                if (isSuccess)
                {
                    if (this.bondSubSectionProgram.TransportUnit == null)
                    {
                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                            @" 设备记忆中系统2没有载具，此次搜索发现了载具，请确认是否真的存在载具。
                              有:  有载具, 设备将记录此载具
                              没有: 没有载具",
                            $"警告",
                            new[] { "有", "没有" },
                            new[] { DialogResult.Yes, DialogResult.No });

                        switch (dr)
                        {
                            case DialogResult.Yes:

                                this.bondSubSectionProgram.TransportUnit =
                                    new TransportUnit(CurrentMachineSystemEnum.System2);

                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                                break;

                            case DialogResult.No:
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                                break;
                        }
                    }
                    else
                    {
                        this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
                    }
                }
                else
                {
                    if (this.bondSubSectionProgram.TransportUnit != null)
                    {
                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                            @" 设备记忆中系统2有载具，此次收索没有发现载具，请确认是否真的存在载具。
                              有:  有载具
                              没有: 没有载具, 设备将清空此载具记忆",
                            $"警告",
                            new[] { "有", "没有" },
                            new[] { DialogResult.Yes, DialogResult.No });

                        switch (dr)
                        {
                            case DialogResult.Yes:
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                                break;

                            case DialogResult.No:
                                this.bondSubSectionProgram.TransportUnit = null;
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                                break;
                        }
                    }
                    else
                    {
                        DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                            @" 未工作台上未发现产品，请确证产品是否存在漏真空",
                            $"警告",
                            new[] { "重试", "退出" },
                            new[] { DialogResult.Retry, DialogResult.No });

                        switch (dr)
                        {
                            case DialogResult.Retry:
                                goto RetryCommand;

                            case DialogResult.No:
                                this.bondSubSectionProgram.TransportUnit = null;
                                this.bondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                                this.CloseVacuum();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 转动皮带不等待到位
        /// </summary>
        public override void MoveBeltNoWaitArrive()
        {
            // 按时间传送， 时间配置在参数里
            TransportBeltSetting transportBeltSetting = this.bondSubSectionProgram.TransportBeltSetting;

            if (transportBeltSetting == null)
            {
                throw new Exception("固晶工作台没有设置传输距离数据集");
            }

            double transportDistance = transportBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.bondSubSectionModule.SendRelativeMoveCommandBelt(transportDistance, transportBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 转动皮带等待到位
        /// </summary>
        public override void MoveBeltWaitArrive()
        {
            // 按时距离送
            TransportBeltSetting transportBeltSetting = this.bondSubSectionProgram.TransportBeltSetting;

            if (transportBeltSetting == null)
            {
                throw new Exception("固晶工作台没有设置传输距离数据集");
            }

            double transportDistance = transportBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.bondSubSectionModule.RelativeMoveBelt(transportDistance, transportBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return this.bondSubSectionModule.HasMaterialInOutlet();
        }


        #region 5号机载台

        /// <summary>
        /// 打开真空
        /// </summary>
        public void OpenVacuum()
        {
            if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.Middle)
            {
                this.OpenVacuumMiddle();
            }
            else if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.MiddleAndFront)
            {
                this.OpenVacuumMiddle();
                this.OpenVacuumFront();
            }
            else if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.MaxRight)
            {
                this.OpenVacuumMiddle();
                this.OpenVacuumRight();
            }
            else if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.All)
            {
                this.OpenVacuumMiddle();
                this.OpenVacuumFront();
                this.OpenVacuumRight();
            }
        }

        /// <summary>
        /// 检测真空
        /// </summary>
        /// <returns>结果</returns>
        public bool CheckVacuum()
        {
            if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.Middle)
            {
                return this.CheckVacuum1();
            }
            else if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.MaxRight)
            {
                return this.CheckVacuum1() && this.CheckVacuum2();
            }
            else if (this.bondSubSectionProgram.Size == BondMaxSubSectionSizeEnum.MiddleAndFront)
            {
                return this.CheckVacuum1() && this.CheckVacuum3();
            }
            else
            {
                return this.CheckVacuum1() && this.CheckVacuum2() && this.CheckVacuum3();
            }
        }

        /// <summary>
        /// 关闭真空
        /// </summary>
        public void CloseVacuum()
        {
            if (!TransportDevicePara.GetInstance().IsManuallyReleaseVacuum)
            {
                this.Module.BondMaxSubSectionVacuumMiddle?.SetOutputValue(false);
                this.Module.BondMaxSubSectionVacuumFront?.SetOutputValue(false);
                this.Module.BondMaxSubSectionVacuumRight?.SetOutputValue(false);
            }
        }

        /// <summary>
        /// 打开真空然后检测
        /// </summary>
        /// <returns>结果</returns>
        public bool OpenVacuumAndCheck()
        {
            this.OpenVacuum();

            Stopwatch sw = Stopwatch.StartNew();

            while (sw.ElapsedMilliseconds < this.bondSubSectionProgram.CheckVacuumTime)
            {
                if (this.CheckVacuum())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 打开真空通道中
        /// </summary>
        private void OpenVacuumMiddle()
        {
            this.Module.BondMaxSubSectionVacuumMiddle?.SetOutputValue(true);
        }

        /// <summary>
        /// 打开真空通道前
        /// </summary>
        private void OpenVacuumFront()
        {
            this.Module.BondMaxSubSectionVacuumFront?.SetOutputValue(true);
        }

        /// <summary>
        /// 打开真空通道右边
        /// </summary>
        private void OpenVacuumRight()
        {
            this.Module.BondMaxSubSectionVacuumRight?.SetOutputValue(true);
        }


        /// <summary>
        /// 检测真空通道中
        /// </summary>
        /// <returns>结果</returns>
        private bool CheckVacuum1()
        {
            if (this.Module.BondMaxSubSectionVacuumCheckMiddle == null)
            {
                return true;
            }
            return this.Module.BondMaxSubSectionVacuumCheckMiddle.GetInputValue();
        }

        /// <summary>
        /// 检测真空通道前
        /// </summary>
        /// <returns>结果</returns>
        private bool CheckVacuum2()
        {
            if (this.Module.BondMaxSubSectionVacuumCheckFront == null)
            {
                return true;
            }
            return this.Module.BondMaxSubSectionVacuumCheckFront.GetInputValue();
        }

        /// <summary>
        /// 检测真空通道右
        /// </summary>
        /// <returns>结果</returns>
        private bool CheckVacuum3()
        {
            if (this.Module.BondMaxSubSectionVacuumCheckRight == null)
            {
                return true;
            }
            return this.Module.BondMaxSubSectionVacuumCheckRight.GetInputValue();
        }

        #endregion
        #region 加热

        /// <summary>
        /// 打开加热
        /// </summary>
        public void OpenHeater()
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }
                Thread.Sleep(50);

                this.Module.BondMaxSubHeaterElectric.SetOutputValue(true);
            }

           
        }

        /// <summary>
        /// 关闭加热
        /// </summary>
        public void CLoseHeater()
        {

            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }
                Thread.Sleep(50);

                this.Module.BondMaxSubHeaterElectric.SetOutputValue(false);
            }
        }

        /// <summary>
        /// 是否在加热
        /// </summary>
        public bool IsHeaterOpen()
        {
            lock (obLock)
            {
                if (!RuntimeProvider.ThreadFlag)
                {
                    return false;
                }    

                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return true;
                }

                Thread.Sleep(50);
                return this.Module.BondMaxSubHeaterElectric.GetOutputValue();
            }
        }

        /// <summary>
        /// 设置温度
        /// </summary>
        /// <param name="temperature">温度</param>
        public void SetHeaterTemperature(int temperature)
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }
                Thread.Sleep(50);

                this.Module.BondMaxSubHeater.SetTemperature(temperature);
            }


          
        }

        private static readonly object obLock = new object();

        /// <summary>
        /// 获取温度
        /// </summary>
        /// <returns>温度</returns>
        public int GetHeaterTemperature()
        {
            lock (obLock)
            {
                Thread.Sleep(50);

                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return 100;
                }
                return this.Module.BondMaxSubHeater.GetTemperature();
            }

        }

        /// <summary>
        /// 获取补偿温度
        /// </summary>
        /// <returns>结果</returns>
        public double GetCompensateValue()
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return 10;
                }

                Thread.Sleep(50);
                return this.Module.BondMaxSubHeater.GetOffset();
            }

           
        }

        /// <summary>
        /// 设置补偿温度
        /// </summary>
        /// <param name="value">补偿值</param>
        public void SetCompensateValue(double value)
        {

            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }

                Thread.Sleep(50);
                this.Module.BondMaxSubHeater.SetOffset(value);
            }

          
        }

        /// <summary>
        /// 获取补偿斜率
        /// </summary>
        /// <returns>结果</returns>
        public double GetCompensateSlope()
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return 1;
                }
                Thread.Sleep(50);

                return this.Module.BondMaxSubHeater.GetSlope();
            }

            
        }

        /// <summary>
        /// 设置补偿斜率
        /// </summary>
        /// <param name="value">补偿值</param>
        public void SetCompensateSlope(double value)
        {

            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return;
                }

                Thread.Sleep(50);
                this.Module.BondMaxSubHeater.SetSlope(value);
            }

           
        }

        /// <summary>
        /// 是否报警
        /// </summary>
        /// <returns>结果</returns>
        public bool Alarm()
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return false;
                }

                Thread.Sleep(50);
                return Math.Abs(this.GetHeaterTemperature() - this.bondSubSectionProgram.HeaterTemperature) > this.bondSubSectionProgram.AlarmTemperature;
            }

           
        }

        /// <summary>
        /// 是否加热成功
        /// </summary>
        /// <returns>结果</returns>
        public bool IsHeatingSucceed()
        {
            lock (obLock)
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    return true;
                }

                Thread.Sleep(50);
                return Math.Abs(this.GetHeaterTemperature() - this.bondSubSectionProgram.HeaterTemperature) <= 1;
            }

           
        }

        #endregion
    }
}
