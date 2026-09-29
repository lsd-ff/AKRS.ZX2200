namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using log4net.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    /// 基岛测高
    /// </summary>
    public class S1ModuleMeasureHeightActionNode : ActionNode
    {
        /// <summary>
        /// 测高动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 获取sub对象
                Module module = System1Domain.GetInstance().ActionNodeController.CurrentModule;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    module.ModuleInfo.IsVisioned = true;
                }

                if (!module.Config.MeasureHeightInSystem1)
                {
                    return ExcuteResult.Success;
                }

                DispenseRunTimeProvider.RecordTime("系统1测高开始", $"{module.Name}测高开始 ----------------");

                return System1Domain.GetInstance().MatterHeightMeasurePoints(module);
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
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

            if (module.BaseInfo.IsMeasureHeight)
            {
                return false;
            }

            // 没有测高功能没有开启，直接返回
            if (!module.Config.MeasureHeightInSystem1)
            {
                return false;
            }

            if (module.MatterProductState == MatterProductState.Disable
                || module.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            if (module.IsModuleProcessFinishedInSystem1)
            {
                return false;
            }

            return true;
        }
    }
}
