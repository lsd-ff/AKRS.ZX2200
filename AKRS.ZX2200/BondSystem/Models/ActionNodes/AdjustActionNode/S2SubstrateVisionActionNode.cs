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
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
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

    /// <summary>
    /// 基板定位
    /// </summary>
    public class S2SubstrateVisionActionNode : ActionNode
    {
        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

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
        /// 基板定位
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                Substrate substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    substrate.SubstrateInfo.IsVisioned = true;
                    return ExcuteResult.Success;
                }

                System2RunTimeProvider.RecordTime("系统2基板定位", $"基板{substrate.Index}定位开始");

                System2Domain.GetInstance().System2ObjectVision(substrate);

                System2RunTimeProvider.RecordTime("系统2基板定位", $"基板{substrate.Index}定位结束");

                return ExcuteResult.Success;
            }
            catch (VmException ex)
            {
                AKRSXtraMessageBox.Show(ex.errorMessage + "\n" + ex.Message + "\n" + ex.errorCode);
                return ExcuteResult.Exception;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.Dispense);
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
            // 获取当前基板
            Substrate substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();

            // 成功或者失败了都不会进入该制程
            if (substrate.EntityState != EntityState.Process)
            {
                return false;
            }

            // 焊点点位是否开启
            if (substrate.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            // 被屏蔽则不做
            if (substrate.MatterProductState == MatterProductState.Disable
                || substrate.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 定位过则不再定位
            if (substrate.BaseInfo.IsVisioned)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (substrate.IsSubstrateProcessFinishInSystem2)
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
            // 获取当前基板
            Substrate substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();

            if (substrate == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist substrate , Please check TransportUnit Program\r\n" + $"{this.Name}",
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
