using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using log4net.Core;
using VM.PlatformSDKCS;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.AdjustActionNode
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 载具定位
    /// </summary>
    public class S2CarrierVisionActionNode : ActionNode
    {
        /// <summary>
        /// 基板定位
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 产品对象
                AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit transportUnit =
                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    transportUnit.TransportUnitInfo.IsVisioned = true;
                    return ExcuteResult.Success;
                }

                // 如果是板子分成两块的情况下，定位应该是分开定位的，后续要单独写成两个actionNode
                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
                {
                    return ExcuteResult.Success;
                }

                System2RunTimeProvider.RecordTime("系统2载具定位", $"载具定位开始");

                System2Domain.GetInstance().System2ObjectVision(transportUnit);

                System2RunTimeProvider.RecordTime("系统2载具定位", $"载具定位结束");

                return ExcuteResult.Success;
            }
            catch (VmException ex)
            {
                AKRSXtraMessageBox.Show(ex.errorMessage + "\n" + ex.Message + "\n" + ex.errorCode);
                return ExcuteResult.Exception;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.Bond);
                AKRSMessageBoxExt.Show(
                    ex.Message,
                    "异常",
                    new string[] { "异常" },
                    new DialogResult[] { DialogResult.Yes });
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
            // 产品对象
            AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;
            
            // TU定位是否开启
            if (transportUnit.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            // 被屏蔽则不做
            if (transportUnit.MatterProductState == MatterProductState.Disable
                || transportUnit.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 定位过则不再定位
            if (transportUnit.BaseInfo.IsVisioned)
            {
                return false;
            }

            // sub里面的工艺制作完成了也不做
            if (transportUnit.IsTuProcessFinishedInSystem2)
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
            // 获取Bond的TU对象
            AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            if (transportUnit == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist TransportUnit , Please check TransportUnit Program\r\n" + $"{this.Name}",
                    "Alarm",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);

                Machine.GetInstance().Stop();
                return false;
            }

            return true;
        }
    }
}
