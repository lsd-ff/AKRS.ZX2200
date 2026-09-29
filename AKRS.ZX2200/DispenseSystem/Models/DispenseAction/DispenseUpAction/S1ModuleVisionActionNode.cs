
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.Models;
using log4net.Core;
using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using VM.PlatformSDKCS;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseUpAction
{
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 基岛定位
    /// </summary>
    public class S1ModuleVisionActionNode : ActionNode
    {
        /// <summary>
        /// 点胶动作
        /// </summary>
        /// <returns>执行的结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 获取当前的carrier对象
                Module module = System1Domain.GetInstance().ActionNodeController.CurrentModule;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    module.ModuleInfo.IsVisioned = true;
                }

                DispenseRunTimeProvider.RecordTime("系统1基岛定位", $"基岛{module.Index}定位开始");

                System1Domain.GetInstance().System1MatterVision(module);


                DispenseRunTimeProvider.RecordTime("系统1基岛定位", $"基岛{module.Index}定位开始");

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
        /// 是否执行该动作
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 获取当前的carrier对象
            Module module = System1Domain.GetInstance().ActionNodeController.CurrentModule;

            if (module == null)
            {
                throw new Exception("获取当前基岛失败！请检查当前基岛是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // Module点位是否开启
            if (module.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            if (module.MatterProductState == MatterProductState.Disable
                || module.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // 定位过则不再定位
            if (module.BaseInfo.IsVisioned)
            {
                return false;
            }

            if (module.IsModuleProcessFinishedInSystem1)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 是否进行该动作
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 获取当前的carrier对象
            Module module = System1Domain.GetInstance().ActionNodeController.CurrentModule;

            if (module == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist module , Please check TransportUnit Program\r\n" + $"{this.Name}",
                    "Alarm",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);
                return false;
            }

            return true;
        }
    }
}
