using System;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.Services;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

using log4net.Core;

using VM.PlatformSDKCS;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.AdjustActionNode
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;

    /// <summary>
    /// 焊点定位流程
    /// </summary>
    [Serializable]
    public class BondPositionVisionActionNode : ActionNode
    {
        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// 系统2动作节点排序
        /// </summary>
        private S2ActionNodeController actionNodesService => System2Domain.GetInstance().ActionNodesService;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 执行动作
        /// </summary>
        /// <returns>结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;

            try
            {
                // 获取当前焊点
                BondPosition bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    bondPosition.BondPositionInfo.IsVisioned = true;
                    return ExcuteResult.Success;
                }
                
                System2RunTimeProvider.RecordTime("系统2焊点定位", $"焊点：{bondPosition.Name}定位开始");

                System2Domain.GetInstance().System2ObjectVision(bondPosition);

                System2RunTimeProvider.RecordTime("系统2焊点定位", $"焊点：{bondPosition.Name}定位结束");

                return ExcuteResult.Success;
            }
            catch (VmException ex)
            {
                AKRSXtraMessageBox.Show(ex.errorMessage + "\n" + ex.Message + "\n" + ex.errorCode);
                return ExcuteResult.Exception;
            }
            catch (Exception ex)
            {
                LogHelper.Post(Level.Error, $"流程运行故障！", ex, LogCategory.Dispense);
                AKRSMessageBoxExt.Show(ex.Message, "Exception", new string[] { "Exception" }, new DialogResult[] { DialogResult.Yes });
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
            BondPosition bp = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 焊点点位是否开启
            if (bp.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            // 判断当前工艺制程是否开启
            if (bp.MatterProductState == MatterProductState.Disable
                || bp.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 定位过则不再定位
            if (bp.BaseInfo.IsVisioned)
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
            // 获取当前芯片对象是否成功
            if (!System2Domain.GetInstance().ActionNodesService.GetCurrentComponent().Item1)
            {
                DialogResult dialogResult = AKRSMessageBoxExt.Show(
                    $"Get  current  component  failed!",
                    "Warn",
                    new string[] { "OK" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.SecondLevel);

                return false;
            }

            // 获取当前焊点
            BondPosition bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            if (bondPosition == null)
            {
                return false;
            }

            // 获取当前基岛，
            Module curIslandAcupoint = this.system2Domain.ActionNodesService.GetCurrentModule();

            if (curIslandAcupoint == null)
            {
                return false;
            }

            return true;
        }
    }
}
