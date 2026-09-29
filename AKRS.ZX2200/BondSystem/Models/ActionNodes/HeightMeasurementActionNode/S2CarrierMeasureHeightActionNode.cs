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
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.UserManager.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;

    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 载具测高
    /// </summary>
    public class S2CarrierMeasureHeightActionNode : ActionNode
    {
        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController S2ActionNodeController => System2Domain.GetInstance().ActionNodesService;

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
                System2RunTimeProvider.RecordTime("系统2载具测高", $"载具测高开始");

                TransportUnit transportUnit =
                    TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;
                ExcuteResult dialog = System2Domain.GetInstance().MatterHeightMeasurePoints(transportUnit);

                System2RunTimeProvider.RecordTime("系统2载具测高", $"载具测高结束");

                return dialog;
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
            // 产品对象
            AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 被屏蔽则不做
            if (transportUnit.MatterProductState == MatterProductState.Disable
                || transportUnit.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 内部工序全部完成了也不需要执行
            if (transportUnit.IsTuProcessFinishedInSystem2)
            {
                return false;
            }

            // 测高完成之后也不需要执行
            if (transportUnit.TransportUnitInfo.IsMeasureHeight)
            {
                return false;
            }

            if (transportUnit.Config.HeightMeasurementPoints.Count == 0)
            {
                return false;
            }

            if (!transportUnit.Config.MeasureHeightInSystem2)
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
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return false;
            }

            // 获取Bond的TU对象
            TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

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
