using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models;
using log4net.Core;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 载具测高
    /// </summary>
    public class S1CarrierMeasureHeightActionNode : ActionNode
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
                // 获取TU对象
                TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    transportUnit.TransportUnitInfo.IsVisioned = true;
                }

                if (!ProductConfiguration.GetInstance().TransportUnitConfig.MeasureHeightInSystem1)
                {
                    return ExcuteResult.Success;
                }

                DispenseRunTimeProvider.RecordTime("系统1测高开始", $"{transportUnit.Name}测高开始 ----------------");

                return System1Domain.GetInstance().MatterHeightMeasurePoints(transportUnit);
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
        /// 是否进行该动作
        /// 这个只是根据条件去判断
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            TransportUnit transportUnit = TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

            if (transportUnit == null)
            {
                throw new Exception("获取当前载具失败！请检查当前载具是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            if (transportUnit.BaseInfo.IsMeasureHeight)
            {
                return false;
            }

            // 被屏蔽则不做
            if (transportUnit.MatterProductState == MatterProductState.Disable
                || transportUnit.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // sub里面的工艺制作完成了也不做
            if (transportUnit.IsTuProcessFinishedInSystem1)
            {
                return false;
            }

            return true;
        }
    }
}
