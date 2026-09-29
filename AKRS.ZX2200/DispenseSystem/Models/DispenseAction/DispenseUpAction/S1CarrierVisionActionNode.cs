using System;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.ZX2200.Models;
using log4net.Core;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.DispenseUpAction
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using VM.PlatformSDKCS;

    /// <summary>
    /// 基板定位
    /// </summary>
    public class S1CarrierVisionActionNode : ActionNode
    {
        /// <summary>
        /// 产品对象
        /// </summary>
        private TransportUnit TransportUnit =>
            TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.TransportUnit;

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
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    // 修改当前焊点的状态为点胶完成
                    TransportUnit.TransportUnitInfo.IsVisioned = true;
                }

                // 如果是板子分成两块的情况下，定位应该是分开定位的，后续要单独写成两个actionNode
                if (ProductDomain.GetInstance().ProductConfig.TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
                {
                    return ExcuteResult.Success;
                }

                DispenseRunTimeProvider.RecordTime("系统1载具定位", $"载具定位开始");

                System1Domain.GetInstance().System1MatterVision(TransportUnit);

                DispenseRunTimeProvider.RecordTime("系统1载具定位", $"载具定位结束");

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
            if (TransportUnit == null)
            {
                throw new Exception("获取当前载具失败！请检查当前载具是否已经被删除或者工作步骤配置有问题。\r\n 请清空载具后重试");
            }

            // 被屏蔽则不做
            if (TransportUnit.MatterProductState == MatterProductState.Disable
                || TransportUnit.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // 定位过则不再定位
            if (TransportUnit.BaseInfo.IsVisioned)
            {
                return false;
            }

            // sub里面的工艺制作完成了也不做
            if (TransportUnit.IsTuProcessFinishedInSystem1)
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
            if (TransportUnit == null)
            {
                AKRSMessageBoxExt.Show(
                    $"Don not exist TransportUnit , Please check TransportUnit Program\r\n" + $"{this.Name}",
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
