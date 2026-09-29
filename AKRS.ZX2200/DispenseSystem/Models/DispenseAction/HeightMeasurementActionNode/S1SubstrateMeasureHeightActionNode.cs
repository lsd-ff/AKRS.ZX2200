namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.HeightMeasurementActionNode
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
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
    /// 基板测高
    /// </summary>
    public class S1SubstrateMeasureHeightActionNode : ActionNode
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
                Substrate substrate = System1Domain.GetInstance().ActionNodeController.CurrentSubstrate;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    substrate.SubstrateInfo.IsVisioned = true;
                }

                if (!ProductConfiguration.GetInstance().SubstrateConfig.MeasureHeightInSystem1 && !ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
                {
                    return ExcuteResult.Success;
                }

                DispenseRunTimeProvider.RecordTime("系统1测高开始", $"{substrate.Name}测高开始 ----------------");

                return System1Domain.GetInstance().MatterHeightMeasurePoints(substrate);
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
            // 获取当前的carrier对象
            Substrate substrate = System1Domain.GetInstance().ActionNodeController.CurrentSubstrate;

            if (substrate == null)
            {
                throw new Exception("获取当前基板失败！请检查当前基板是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // 成功或者失败了都不会进入该制程
            if (substrate.EntityState != EntityState.Process)
            {
                return false;
            }

            if (substrate.BaseInfo.IsMeasureHeight)
            {
                return false;
            }

            // 没有测高功能没有开启，直接返回
            if (!substrate.Config.MeasureHeightInSystem1)
            {
                return false;
            }

            // 被屏蔽则不做
            if (substrate.MatterProductState == MatterProductState.Disable
                || substrate.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // sub里面的工艺制作完成了也不做
            if (substrate.IsSubstrateProcessFinishedInSystem1)
            {
                return false;
            }
            
            return true;
        }
    }
}
