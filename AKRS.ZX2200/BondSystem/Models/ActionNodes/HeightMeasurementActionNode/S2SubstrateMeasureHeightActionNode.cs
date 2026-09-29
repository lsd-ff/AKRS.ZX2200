using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.Process;

    /// <summary>
    /// 基板测高
    /// </summary>
    public class S2SubstrateMeasureHeightActionNode : ActionNode
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
                Substrate substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();

                System2RunTimeProvider.RecordTime("系统2基板测高", $"{substrate.Name}测高开始");

                ExcuteResult result = System2Domain.GetInstance().MatterHeightMeasurePoints(substrate);

                System2RunTimeProvider.RecordTime("系统2基板测高", $"{substrate.Name}测高结束");

                return result;
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
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }

            // 获取当前基板
            Substrate substrate = System2Domain.GetInstance().ActionNodesService.GetCurrentSubstrate();

            // 成功或者失败了都不会进入该制程
            if (substrate.EntityState != EntityState.Process)
            {
                return false;
            }

            // 被屏蔽则不做
            if (substrate.MatterProductState == MatterProductState.Disable
                || substrate.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (substrate.IsSubstrateProcessFinishInSystem2)
            {
                return false;
            }

            // 测高完成之后也不需要执行
            if (substrate.SubstrateInfo.IsMeasureHeight)
            {
                return false;
            }

            if (substrate.Config.HeightMeasurementPoints.Count == 0)
            {
                return false;
            }

            if (!substrate.Config.MeasureHeightInSystem2)
            {
                return false;
            }

            return true;
        }
    }
}
