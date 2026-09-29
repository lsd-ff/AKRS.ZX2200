#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2024/3/23 18:03:00
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

using System;
using System.Threading;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
using AKRS.ZX2200.TransportSystem.Models.Enums;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportSystem.Modules;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

namespace AKRS.ZX2200.TransportSystem.Controllers
{
    /// <summary>
    /// 描述：点胶载台控制器
    /// </summary>
    public class DispenseSubSectionController : BaseSubSectionController
    {
        /// <summary>
        /// 点胶载台模组
        /// </summary>
        private DispenseSubSectionModule dispenseSubSectionModule => TransportModule.GetInstance().DispenseSubSectionModule;

        /// <summary>
        /// 点胶程式
        /// </summary>
        private DispenseSubSectionProgram dispenseSubSectionProgram => TransportProgram.GetInstance().DispenseSubSectionProgram;

        /// <summary>
        /// 构造函数
        /// </summary>
        public DispenseSubSectionController()
        {
            this.SubSectionModule = TransportModule.GetInstance().DispenseSubSectionModule;
            this.SubSectionProgram = TransportProgram.GetInstance().DispenseSubSectionProgram;
        }

        /// <summary>
        /// 挡料气缸1升起 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinder1UpAndDelay(int afterDelay)
        {
            this.dispenseSubSectionModule.BlockCylinder1Up();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 挡料气缸1降下 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinder1DownAndDelay(int afterDelay)
        {
            // 如果流道或者系统1没有配置，直接返回
            if ((!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated) || (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured))
            {
                return;
            }

            this.dispenseSubSectionModule.BlockCylinder1Down();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 挡料气缸2升起 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinder2UpAndDelay(int afterDelay)
        {
            this.dispenseSubSectionModule.BlockCylinder2Up();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 挡料气缸2降下 并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void BlockCylinder2DownAndDelay(int afterDelay)
        {
            // 如果流道或者系统1没有配置，直接返回
            if ((!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated) || (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured))
            {
                return;
            }

            this.dispenseSubSectionModule.BlockCylinder2Down();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 轨道气缸顶起  并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void TrackLiftCylinderUpAndDelay(int afterDelay)
        {
            this.dispenseSubSectionModule.TrackLiftCylinderUp();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 轨道气缸降落  并延迟相应时间
        /// </summary>
        /// <param name="afterDelay">打开气缸后的延迟 单位：ms</param>
        public void TrackLiftCylinderDownAndDelay(int afterDelay)
        {
            this.dispenseSubSectionModule.TrackLiftCylinderDown();
            Thread.Sleep(afterDelay);
        }

        /// <summary>
        /// 夹紧气缸
        /// </summary>
        /// <param name="isAdditionVc">附加的真空是否夹紧是否夹紧</param>
        /// <exception cref="Exception">异常</exception>
        public void Clamp(bool isAdditionVc = false)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated == false)
            {
                return;
            }

            if (this.dispenseSubSectionProgram.TransportBeltSetting == null)
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                    $"传输系统皮带设置为空，夹紧夹爪失败!\r\n请先设置传输皮带参数！",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK });

                return;
            }

        RetryCommand:
            switch (this.dispenseSubSectionProgram.TransportBeltSetting.ClampMode)
            {
                case ClampModeEnum.Lowering:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                    break;

                case ClampModeEnum.Vacuum:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);

                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null && isAdditionVc)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(true);
                    }

                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                    break;

                case ClampModeEnum.LoweringAndVacuum:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);
                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null && isAdditionVc)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(true);
                    }
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                    break;

                case ClampModeEnum.VacuumAndLowering:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeClampingDelay);
                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(true);
                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(true);
                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null && isAdditionVc)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(true);
                    }
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterClampingDelay);
                    break;
            }

            Thread.Sleep(500);

            bool ret1 = this.dispenseSubSectionModule.TrackFrontCylinder1PLSensor.GetInputValue();
            bool ret2 = this.dispenseSubSectionModule.TrackFrontCylinder2PLSensor == null
                        || this.dispenseSubSectionModule.TrackFrontCylinder2PLSensor.GetInputValue();
            bool ret3 = this.dispenseSubSectionModule.TrackBackCylinder1PLSensor == null
                        || this.dispenseSubSectionModule.TrackBackCylinder1PLSensor.GetInputValue();

            if ((ret1 && ret2 && ret3) == false)
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                    $"点胶工作台气缸夹紧失败，请检查传感器状态！  \r\n",
                    "Alarm",
                    new string[] { "重试", "忽略", "终止" },
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

               // throw new Exception("点胶夹紧失败, 请检查传感器状态");
            }
        }

        /// <summary>
        ///  松开夹爪
        /// </summary>
        /// <exception cref="Exception">松开夹爪后，气缸限位传感器没能处于正确的状态</exception>
        public void UnClamp()
        {
            // 如果流道或者系统1没有配置，直接返回
            if ((!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
                || (!MachineHardwareConfiguration.GetInstance().IsTransportConfigured)) 
            {
                return;
            }

            if (this.dispenseSubSectionProgram.TransportBeltSetting == null)
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                    $"传输系统皮带设置为空，松开夹爪失败!  \r\n请先设置传输皮带参数！",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK });

                return;
            }

        RetryCommand:
            switch (this.dispenseSubSectionProgram.TransportBeltSetting.UnClampMode)
            {
                case UnClampModeEnum.Rising:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                    break;

                case UnClampModeEnum.Vacuum:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);

                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(false);
                    }

                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                    break;

                case UnClampModeEnum.RisingAndVacuum:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);

                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(false);
                    }

                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                    break;

                case UnClampModeEnum.VacuumAndRising:
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.BeforeUnClampingDelay);
                    this.dispenseSubSectionModule.SubstrateVaccumElectric.SetOutputValue(false);

                    if (this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric != null)
                    {
                        this.dispenseSubSectionModule.SubstrateAdditionVaccumElectric.SetOutputValue(false);
                    }

                    this.dispenseSubSectionModule.TrackLiftCylinderElectric.SetOutputValue(false);
                    Thread.Sleep(this.dispenseSubSectionProgram.BondinsertSetting.AfterUnClampingDelay);
                    break;
            }

            Thread.Sleep(500);

            bool ret1 = this.dispenseSubSectionModule.TrackFrontCylinder1NLSensor.GetInputValue();
            bool ret2 = this.dispenseSubSectionModule.TrackFrontCylinder2NLSensor == null || this.dispenseSubSectionModule.TrackFrontCylinder2NLSensor.GetInputValue();
            bool ret3 = this.dispenseSubSectionModule.TrackBackCylinder1NLSensor == null || this.dispenseSubSectionModule.TrackBackCylinder1NLSensor.GetInputValue();

            if ((ret1 && ret2 && ret3) == false)
            {
                DialogResult dialog = AKRSMessageBoxExt.Show(
                    $"点胶工作台气缸松开失败，请检查传感器状态！ \r\n",
                "Alarm",
                    new string[] { "重试", "忽略", "终止" },
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

        /// <summary>
        ///  搜索皮带1系统1
        /// </summary>
        /// <returns>
        /// true: 搜索到并传料成功，
        /// false：没搜索到或者搜索到了后传料失败
        /// 里面不涉及任何状态参数的修改 此处不修改记忆
        /// </returns>
        public override bool SearchBelt()
        {
        retry:

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                // 点胶夹爪归位，点胶气缸升起
                this.UnClamp();
            } 

            // 搜索到有料之后在打开挡料气缸，提前升起就会卡料，针对于点胶1和点胶2
            //// 点胶挡料气缸1顶起
            this.BlockCylinder1DownAndDelay(50);

            // 点胶载台皮带先进行倒转
            double transportDistance =
                this.dispenseSubSectionProgram.TransportBeltSetting.BackwardTransportDistance;

            this.dispenseSubSectionModule.SendRelativeMoveCommandBelt(
               -transportDistance,
                this.dispenseSubSectionProgram.TransportBeltSetting.BackwardVelRate);

            // 入料感应器是否感应到料
            bool isSensorTrigger = false;

            // 检测传感器 判断状态
            while (this.dispenseSubSectionModule.IsMoving())
            {
                if (this.dispenseSubSectionModule.HasMaterialInFeedingInlet())
                {
                    isSensorTrigger = true;

                    // 停止倒转
                    this.dispenseSubSectionModule.StopMove();
                    Thread.Sleep(20);
                    break;
                }
            }

            // 如果没感应到
            if (isSensorTrigger == false)
            {
                return false;
            }

            //double distance = TransportProgram.GetInstance().DispenseSubSectionProgram.BondinsertSetting
            //    .HardstopRightDistance;

            // 点胶挡料气缸1、2顶起
            this.BlockCylinder1UpAndDelay(50);
            this.BlockCylinder2UpAndDelay(50);

            // 检测到了 正转
            this.dispenseSubSectionModule.RelativeMoveBelt(
                this.dispenseSubSectionProgram.TransportBeltSetting.ForwardTransportDistance,
                this.dispenseSubSectionProgram.TransportBeltSetting.ForwardVelRate);

            // 如果卡料 报警
            if (this.dispenseSubSectionModule.HasMaterialInFeedingInlet())
            {
                // 报警：入料卡料
                DialogResult dr = AKRSMessageBoxExt.Show(
                    @"点胶工作台卡料，请选择如何处理
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

            if (MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                // 传输成功,夹紧夹爪
                this.Clamp(true);
            }

            return true;
        }

        /// <summary>
        /// 搜寻Belt 并和记忆做比较，并修改记忆
        /// </summary>
        public override void MapBelt(bool AutoWork)
        {
            // 只有当流道不反找且是自动工作且轨道上有料的时候才不反照
            if (!TransportDevicePara.GetInstance().IsReverseSearch
                && AutoWork
                && this.dispenseSubSectionProgram.TransportUnit != null)
            {
                return;
            }

            bool hasMaterial = this.SearchBelt();

            // 如果检测到有料
            if (hasMaterial) 
            {
                // 当缓存中没有料 
                if (this.dispenseSubSectionProgram.TransportUnit == null)
                {
                    DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                        @" 设备记忆中系统1没有载具，此次搜索发现了载具，请确认是否真的存在载具。
                              Yes:  有载具, 设备将记录此载具
                              No: 没有载具",
                        $"警告",
                        new[] { "Yes", "No" },
                        new[] { DialogResult.Yes, DialogResult.No });

                    switch (dr)
                    {
                        case DialogResult.Yes:

                            this.dispenseSubSectionProgram.TransportUnit =
                                new TransportUnit(CurrentMachineSystemEnum.System1);

                            this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                            break;

                        case DialogResult.No:
                            this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                            break;
                    }
                }
                else   // 如果有料
                {
                    this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
                    this.dispenseSubSectionProgram.TransportUnit.Refresh();
                }
            }
            else   // 没有搜索到料
            {
                if (this.dispenseSubSectionProgram.TransportUnit != null)
                {
                    DialogResult dr = AKRSMessageBoxExt.ShowWarn(
                        @" 设备记忆中系统1有载具，此次收索没有发现载具，请确认是否真的存在载具。
                              有:  有载具
                              没有: 没有载具, 设备将清空此载具记忆",
                        $"警告",
                        new[] { "有", "没有" },
                        new[] { DialogResult.Yes, DialogResult.No });

                    switch (dr)
                    {
                        case DialogResult.Yes:
                            this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;

                            if (!this.dispenseSubSectionProgram.IsTuInDispense1)
                            {
                                this.dispenseSubSectionProgram.TransportUnit.System1BackMoveDistance();
                            }

                            break;

                        case DialogResult.No:
                            this.dispenseSubSectionProgram.TransportUnit = null;
                            this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                            break;
                    }
                }
                else   // 如果没料
                {
                    this.dispenseSubSectionProgram.TransportUnit = null;
                    this.dispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
                }
            }

            this.dispenseSubSectionProgram.IsTuInDispense1 = true;
        }

        /// <summary>
        /// 转动皮带不等待到位
        /// </summary>
        public override void MoveBeltNoWaitArrive()
        {
            // 按时间传送， 时间配置在参数里
            TransportBeltSetting transportBeltSetting = this.dispenseSubSectionProgram.TransportBeltSetting;

            if (transportBeltSetting == null)
            {
                throw new Exception("dispense transport belt setting is not exist");
            }

            double transportDistance = transportBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.dispenseSubSectionModule.SendRelativeMoveCommandBelt(transportDistance, transportBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 转动皮带等待到位
        /// </summary>
        public override void MoveBeltWaitArrive()
        {
            // 按时距离送
            TransportBeltSetting transportBeltSetting = this.dispenseSubSectionProgram.TransportBeltSetting;

            if (transportBeltSetting == null)
            {
                throw new Exception("dispense transport belt setting is not exist");
            }

            double transportDistance = transportBeltSetting.ForwardTransportDistance;

            // 皮带1相对运动指令
            this.dispenseSubSectionModule.RelativeMoveBelt(transportDistance, transportBeltSetting.ForwardVelRate);
        }

        /// <summary>
        /// 是否有料在入料口
        /// </summary>
        /// <returns>是否有料在入料口</returns>
        public bool HasMaterialInFeedingInlet()
        {
            return this.dispenseSubSectionModule.HasMaterialInFeedingInlet();
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return this.dispenseSubSectionModule.HasMaterialInOutlet();
        }
    }
}
