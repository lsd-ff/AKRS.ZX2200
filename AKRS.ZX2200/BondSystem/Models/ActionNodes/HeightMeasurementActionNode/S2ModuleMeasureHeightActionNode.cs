using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Models;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.HeightMeasurementActionNode
{
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.Services;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 基岛测高
    /// </summary>
    public class S2ModuleMeasureHeightActionNode : ActionNode
    {
        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

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
                Module module = System2Domain.GetInstance().ActionNodesService.GetCurrentModule();

                System2RunTimeProvider.RecordTime("系统2基岛测高", $"{module.Name}测高开始");
                 
                ExcuteResult result = System2Domain.GetInstance().MatterHeightMeasurePoints(module);

                System2RunTimeProvider.RecordTime("系统2基岛测高", $"{module.Name}测高结束");

                return result;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程{this.Name}运行故障！", ex, LogCategory.Dispense);
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
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }

            // 获取当前的carrier对象
            Module module = this.system2Domain.ActionNodesService.GetCurrentModule();

            // 被屏蔽则不做
            if (module.MatterProductState == MatterProductState.Disable
                || module.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (module.IsModuleProcessFinishedInSystem2)
            {
                return false;
            }

            // 测高完成之后也不需要执行
            if (module.ModuleInfo.IsMeasureHeight)
            {
                return false;
            }

            if (module.Config.HeightMeasurementPoints.Count == 0)
            {
                return false;
            }

            if (!module.Config.MeasureHeightInSystem2)
            {
                return false;
            }

            return true;
        }
    }
}
