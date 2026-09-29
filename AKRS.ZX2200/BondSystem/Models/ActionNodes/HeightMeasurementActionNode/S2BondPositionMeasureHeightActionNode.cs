using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;

    /// <summary>
    /// 系统2焊点测高
    /// </summary>
    public class S2BondPositionMeasureHeightActionNode : ActionNode
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
                BondPosition bondPosition = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

                System2RunTimeProvider.RecordTime("系统2焊点测高", $"{bondPosition.Name}测高开始");

                ExcuteResult result = System2Domain.GetInstance().MatterHeightMeasurePoints(bondPosition);

                System2RunTimeProvider.RecordTime("系统2焊点测高", $"{bondPosition.Name}测高结束");

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

            // 焊点
            BondPosition bp = System2Domain.GetInstance().ActionNodesService.GetCurrentBondPosition();

            // 当前焊点如果成功了或者失败了都不会贴片
            if (bp.EntityState != EntityState.Process)
            {
                return false;
            }

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 测高完成之后也不需要执行
            if (bp.BondPositionInfo.IsMeasureHeight)
            {
                return false;
            }

            if (bp.Config.HeightMeasurementPoints.Count == 0)
            {
                return false;
            }

            return true;
        }
    }
}
