using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.SupportFeature.Statistics;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static GTN.mc;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.DispenseAction
{
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Main.Machine.Process;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// 系统2点胶动作
    /// </summary>
    public class S2DispenseActionNode : ActionNode
    {
        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController ActionNodesService => System2Domain.GetInstance().ActionNodesService;

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
                System2RunTimeProvider.RecordTime("系统2点胶动作开始执行", "开始 ----------------");

                #region 前提条件判断

                // 单步工作
                if (!System2Domain.GetInstance().WaitSingleStep())
                {
                    return ExcuteResult.Abort;
                }

                // 执行点胶前如果点了停止则退出
                if (!Signal.WaitStart())
                {
                    return ExcuteResult.Fail;
                }

                // 执行预点胶
                if (System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.DoWork() != ExcuteResult.Success)
                {
                    return ExcuteResult.Alarm;
                }

                if (!Signal.WaitStart())
                {
                    return ExcuteResult.Fail;
                }

                #endregion

                System2RunTimeProvider.RecordTime("点胶前置条件通过，准备开始", "开始 ----------------");

                // 焊点
                BondPosition bondPosition = this.ActionNodesService.GetCurrentBondPosition();

                // 获取需要执行的图案
                EpoxyApplication correctApplication = this.ActionNodesService.GetCurrentEpoxyApplication();

                // 焊点在G0中的位置
                AKRSPoint3D point = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D(correctApplication.OffsetX, correctApplication.OffsetY, 0));

                point += bondPosition.GetDispensePositionCompensate();

                // 空跑模式不出胶
                bool isDrip = MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.DryCycle;

                double angle = bondPosition.CoordinateSystem.DegreeInG0() * 180.0 / Math.PI + bondPosition.GetDispenseRotaryCompensate();

                #region 更换蘸胶吸嘴

                if (correctApplication.EpoxyApplicationStrategy == EpoxyApplicationTypeEnum.Printting)
                {
                    Nozzle nozzle = System2Domain.GetInstance().BondProgram.NozzleShelfProgram.GetNozzleList().Find(it => it.Name == correctApplication.PrintNozzleName);
                    if (nozzle == null)
                    {
                        throw new Exception("蘸胶吸嘴为空，请重新选择");
                    }
                    else if (!nozzle.IsAssistantSucceed)
                    {
                        throw new Exception("蘸胶吸嘴未示教完成，请示教完后重试");
                    }

                    // 换吸嘴动作
                    bool ret = System2Domain.GetInstance().System2Controller.JudgeAndChangeNozzle(correctApplication.PrintNozzleName);
                    if (!ret)
                    {
                        return ExcuteResult.Abort;
                    }
                }

                #endregion

                // 执行点胶
                if (System2Domain.GetInstance().S2DispenseController.InterpolationApplication(correctApplication, point, isDrip, false, false, angle) != ExcuteResult.Success)
                {
                    return ExcuteResult.Alarm;
                }

                // 修改当前焊点的状态为点胶完成
                bondPosition.S2FinishedStep(this.ActionNodesService.GetCurrentProcessStepName());

                StatisticsDomain.GetInstance().RefreshDispenseStatistics();

                #region 点胶完之后看点胶效果

                if (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2)
                {
                    System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point);

                    while (MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2)
                    {
                        if (Machine.GetInstance().IsStop())
                        {
                            MachineStateModel.GetInstance().IsVisionPreviousBondPositionInSystem2 = false;
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }

                #endregion

                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.BeforeEveryDispense)
                    {
                        System2RunTimeProvider.IsFirstDispense = true;
                    }
                    else if (BondProgram.GetInstance().S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.BeforeComingFirstDispense)
                    {
                        System2RunTimeProvider.IsFirstDispense = false;
                    }
                }

                DispenseRunTimeProvider.RecordTime("系统2点胶结束", "点胶结束 ----------------");

                System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.LastDispenseTime = DateTime.Now;

                StatisticsDomain.GetInstance().RefreshS2DispenseStatistics();

                return ExcuteResult.Success;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.Bond);
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
            
            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 如果当前焊点已经点过胶则退出
            if (!bp.IsS2NeedStep(this.ActionNodesService.GetCurrentProcessStepName()))
            {
                return false;
            }
            
            return true;
        }
    }
}
