using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.DispenseSystem.Services;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models;
using log4net.Core;
using VM.PlatformSDKCS;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseUpAction
{
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 基板定位
    /// </summary>
    public class S1SubstrateVisionActionNode : ActionNode
    {
        /// <summary>
        /// 点胶动作
        /// </summary>
        /// <returns>执行的结果</returns>
        public override ExcuteResult DoWork()
        {
            this.WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 获取当前的carrier对象
                Substrate substrate = System1Domain.GetInstance().ActionNodeController.CurrentSubstrate;

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    substrate.SubstrateInfo.IsVisioned = true;
                }

                DispenseRunTimeProvider.RecordTime("系统1基板定位", $"基板{substrate.Index}定位开始");

                // 执行定位操作
                System1Domain.GetInstance().System1MatterVision(substrate);

                DispenseRunTimeProvider.RecordTime("系统1基板定位", $"基板{substrate.Index}定位结束");

                // 返回结果
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

            // Substrate点位是否开启
            if (substrate.Config.LocateConfig.AdjustType == AdjustTypeEnum.None)
            {
                return false;
            }

            // 被屏蔽则不做
            if (substrate.MatterProductState == MatterProductState.Disable
                || substrate.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // 定位过则不再定位
            if (substrate.BaseInfo.IsVisioned)
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

        /// <summary>
        /// 是否进行该动作
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsAlarm()
        {
            // 获取当前的carrier对象
            Substrate substrate = System1Domain.GetInstance().ActionNodeController.CurrentSubstrate;

            if (substrate == null)
            {
                Machine.GetInstance().Stop();
                AKRSMessageBoxExt.Show(
                    $"Don not exist substrate , Please check TransportUnit Program\r\n" + $"{this.Name}",
                    "Alarm",
                    new string[] { "Yes" },
                    new DialogResult[] { DialogResult.Abort },
                    AlarmLevel.SecondLevel);
                return false;
            }

            return true;
        }
    }
}
