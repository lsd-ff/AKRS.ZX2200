using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Log;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VM.PlatformSDKCS;

namespace AKRS.ZX2200.DispenseSystem.Models.DispenseAction.IdentityActionNode
{
    /// <summary>
    /// 焊点ID识别
    /// </summary>
    public class S1BondPositionIdentityActionNode : ActionNode
    {
        /// <summary>
        /// 当前焊点
        /// </summary>
        private BondPosition BondPosition => System1Domain.GetInstance().ActionNodeController.CurrentBondPosition;

        /// <summary>
        /// 基板ID识别动作
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
                    this.BondPosition.BaseInfo.IsIdentity = true;
                    return ExcuteResult.Success;
                }

                return System1Domain.GetInstance().MatterIdentity(BondPosition);
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
        /// 完成动作的条件是否满足
        /// </summary>
        /// <returns>结果</returns>
        public override bool IsDoWork()
        {
            // 成功或者失败了都不会进入该制程
            if (this.BondPosition.EntityState != EntityState.Process)
            {
                return false;
            }

            // 焊点点位是否开启
            if (this.BondPosition.Config.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                return false;
            }

            // 被屏蔽则不做
            if (this.BondPosition.MatterProductState == MatterProductState.Disable
                || BondPosition.MatterProductState == MatterProductState.EnableInSystem2)
            {
                return false;
            }

            // 定位过则不再定位
            if (this.BondPosition.BaseInfo.IsIdentity)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (this.BondPosition.IsFinishedInSystem1)
            {
                return false;
            }

            return true;
        }
    }
}
