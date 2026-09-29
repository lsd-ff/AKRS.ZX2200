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
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;

    /// <summary>
    /// 基岛定位
    /// </summary>
    public class S2ModuleVisionActionNode : ActionNode
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
        /// 基岛定位
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 获取当前的carrier对象
                Module module = System2Domain.GetInstance().ActionNodesService.GetCurrentModule();

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    module.ModuleInfo.IsVisioned = true;
                    return ExcuteResult.Success;
                }

                System2RunTimeProvider.RecordTime("系统2基岛定位", $"基岛{module.Index}定位开始");

                System2Domain.GetInstance().System2ObjectVision(module);

                System2RunTimeProvider.RecordTime("系统2基岛定位", $"基岛{module.Index}定位结束");

                return ExcuteResult.Success;
            }
            catch (VmException ex)
            {
                string message = "\r\n";
                AKRSXtraMessageBox.Show(ex.errorMessage + message + ex.Message + message + ex.errorCode);
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
            // 获取当前的carrier对象
            Module module = this.system2Domain.ActionNodesService.GetCurrentModule();
            
            // 焊点点位是否开启
            if (module.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            // 被屏蔽则不做
            if (module.MatterProductState == MatterProductState.Disable
                || module.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 定位过则不再定位
            if (module.BaseInfo.IsVisioned)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (module.IsModuleProcessFinishedInSystem2)
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
            Module island = this.system2Domain.ActionNodesService.GetCurrentModule();

            if (island == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist module , Please check TransportUnit Program\r\n" + $"{this.Name}",
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
