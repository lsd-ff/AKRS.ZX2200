using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Models;
using log4net.Core;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction
{
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 点胶测高动作
    /// </summary>
    public class S1DispenseMeasureActionNode : ActionNode
    {
        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition BondPosition => System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 测高开始工作动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    BondPosition.BondPositionInfo.IsMeasureHeight = true;
                }

                DispenseRunTimeProvider.RecordTime($"系统1测高动作开始执行", $"焊点信息 名称:{this.BondPosition.Name},基板号{this.BondPosition.SubstrateNum},基岛号{this.BondPosition.ModuleNum}");

                // 获取sub对象
                BondPosition bondPosition = System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

                DispenseRunTimeProvider.RecordTime("系统1测高", $"{bondPosition.Name}测高开始 ----------------");

                ExcuteResult result = System1Domain.GetInstance().MatterHeightMeasurePoints(bondPosition);

                if (result == ExcuteResult.Success)
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高", $"{bondPosition.Name}测高结束 ----------------");

                    return ExcuteResult.Success;
                }
                else
                {
                    DispenseRunTimeProvider.RecordTime("系统1测高", $"{bondPosition.Name}测高失败 ----------------");
                    return ExcuteResult.Abort;
                }
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
            // 焊点
            BondPosition bp = System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

            if (bp == null)
            {
                throw new Exception("获取当前焊点失败！请检查当前焊点是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            if (bp.BondPositionInfo.IsMeasureHeight)
            {
                return false;
            }

            // 没有制程
            if (bp.IsFinishedInSystem1)
            {
                return false;
            }

            if (!bp.Config.MeasureHeightInSystem1)
            {
                return false;
            }

            return true;
        }
    }
}
