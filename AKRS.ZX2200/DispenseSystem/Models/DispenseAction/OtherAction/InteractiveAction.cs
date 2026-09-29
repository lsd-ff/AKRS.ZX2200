using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Models;
using log4net.Core;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.OtherAction
{
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    /// <summary>
    /// 交互动作
    /// 产品是分离的时候和流道的一个交互动作
    /// </summary>
    public class InteractiveAction : ActionNode
    {
        /// <summary>
        /// 预点胶开始工作
        /// </summary>
        /// <returns>是否成功执行动作</returns>
        public override ExcuteResult DoWork()
        {
            // 如果进入到这个动作，那么产品一定是分离的，这个动作里面不需要再去判断
            WorkStart?.Invoke();
            this.State = RunStateEnum.Running;
            try
            {
                // 如果产品是分段的，告诉流道可以传输的信号
                if (ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateProcessing == SubstrateProcessingEnum.Divide)
                {
                    // 如果不在第一段则不需要传送料
                    if (!TransportDomain.GetInstance().TransportProgram.DispenseSubSectionProgram.IsTuInDispense1)
                    {
                        return ExcuteResult.Success;
                    }

                    DispenseRunTimeProvider.RecordTime("系统1分段传送", $"系统1分段传送开始 ----------------");

                    // 移动到安全位置
                    System1Domain.GetInstance().DispenseController.MoveZToSafePos();

                    DispenseRunTimeProvider.RecordTime("系统1分段传送", $"系统1移动到安全位置 ----------------");

                    // 告诉流道前半段已经做完了，可以进行传料了
                    SignalPool.GetInstance().AllowDispense1TransferToDispense2Signal.Set();

                    // 等待流道完成信号
                    SignalPool.GetInstance().TransferToDispense2FinishedSignal.Wait();

                    DispenseRunTimeProvider.IsFirstDispense = true;

                    DispenseRunTimeProvider.RecordTime("系统1分段传送", $"系统1收到信号，分段传送结束 ----------------");
                }

                return ExcuteResult.Success;
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
                WorkStop?.Invoke();
            }
        }
    }
}
