using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.ZX2200.Models;
using log4net.Core;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseBPAction
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Product.Statistics;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Information;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using System.Threading;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;

    /// <summary>
    /// 点胶动作节点
    /// </summary>
    public class S1DispenseActionNode : ActionNode
    {
        /// <summary>
        /// 系统1控制
        /// </summary>
        private S1ActionNodeController ActionNodeController => System1Domain.GetInstance().ActionNodeController;

        /// <summary>
        /// 点胶动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                DispenseRunTimeProvider.RecordTime("系统1点胶动作开始执行", "开始 ----------------");

                #region 前提条件判断

                // 焊点
                BondPosition bondPosition = this.ActionNodeController.CurrentBondPosition;

                DispenseRunTimeProvider.RecordTime($"系统1点胶动作开始执行", $"焊点信息 名称:{bondPosition.Name},基板号{bondPosition.SubstrateNum},基岛号{bondPosition.ModuleNum}");

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    bondPosition.S1FinishedStep(this.ActionNodeController.SingleProcessStepName);
                }

                // 单步工作
                if (!System1Domain.GetInstance().WaitSingleStep())
                {
                    return ExcuteResult.Abort;
                }

                #endregion

                // 执行预点胶
                if (System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.DoWork() != ExcuteResult.Success)
                {
                    return ExcuteResult.Alarm;
                }

                // 如果在预点胶的时候点了停止则退出
                if (!Signal.WaitStart())
                {
                    return ExcuteResult.Fail;
                }

                // 获取需要执行的图案
                EpoxyApplication correctApplication = this.ActionNodeController.CurrentEpoxyApplication;

                AKRSPoint3D point = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D(correctApplication.OffsetX, correctApplication.OffsetY, 0));

                DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶的焊点位置为{point}");

                point += bondPosition.GetDispensePositionCompensate() + DispenseDevicePara.GetInstance().DispenseModulePara.DispenseOffset;

                DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶的真实位置为{point}");

                // 空跑模式不出胶
                bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                double angle = bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI + bondPosition.GetDispenseRotaryCompensate();

                DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶的角度为{angle}");

                // 执行点胶
                if (System1Domain.GetInstance().DispenseController.InterpolationApplication(correctApplication, point, isDrip, false, angle) != ExcuteResult.Success)
                {
                    return ExcuteResult.Alarm;
                }

                // 修改当前焊点的状态为点胶完成
                bondPosition.S1FinishedStep(this.ActionNodeController.SingleProcessStepName);

                DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶动作结束");

                StatisticsDomain.GetInstance().RefreshDispenseStatistics();

                #region 点胶完之后看点胶效果

                if (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem1)
                {
                    DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶完之后观看点胶的点位");

                    AKRSPoint3D visionPos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point);

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionPos);

                    while (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem1)
                    {
                        if (Machine.GetInstance().IsStop())
                        {
                            MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem1 = false;

                            DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"点胶完之后观看点胶的点位结束");
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }

                #endregion

                if (System1Program.GetInstance().DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.BeforeEveryDispense)
                {
                    DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"预点胶标志 - 第一次点胶 置为true");
                    DispenseRunTimeProvider.IsFirstDispense = true;
                }
                else 
                {
                    DispenseRunTimeProvider.RecordTime("系统1点胶动作执行", $"预点胶标志 - 第一次点胶 置为false");
                    DispenseRunTimeProvider.IsFirstDispense = false;
                }

                DispenseRunTimeProvider.RecordTime("点胶结束", "点胶结束 ----------------");

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "异常", new string[] { "异常" }, new DialogResult[] { DialogResult.Yes });
                return ExcuteResult.Exception;
            }
            finally
            {
                this.State = RunStateEnum.Stop;
                this.WorkStop?.Invoke();
                UcMainSystem.ReFreshSystem1TuAction();
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

            // 如果当前焊点已经点过胶则退出
            if (!bp.IsS1NeedStep(this.ActionNodeController.SingleProcessStepName))
            {
                return false;
            }



            return true;
        }
    }
}
