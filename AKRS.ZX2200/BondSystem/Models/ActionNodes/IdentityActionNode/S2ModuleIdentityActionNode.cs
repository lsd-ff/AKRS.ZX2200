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

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.IdentityActionNode
{
    /// <summary>
    /// 基岛身份识别
    /// </summary>
    public class S2ModuleIdentityActionNode : ActionNode
    {
        /// <summary>
        /// 当前基岛
        /// </summary>
        private Module module => System2Domain.GetInstance().ActionNodesService.GetCurrentModule();

        /// <summary>
        /// 基岛ID识别动作
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
                    this.module.ModuleInfo.IsIdentity = true;
                    return ExcuteResult.Success;
                }

                return System2Domain.GetInstance().MatterIdentity(this.module);
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
            if (this.module.EntityState != EntityState.Process)
            {
                return false;
            }

            // 焊点点位是否开启
            if (this.module.Config.IdentityConfig.Identification == IdentificationEnum.Off)
            {
                return false;
            }

            // 被屏蔽则不做
            if (this.module.MatterProductState == MatterProductState.Disable
                || module.MatterProductState == MatterProductState.EnableInSystem1)
            {
                return false;
            }

            // 定位过则不再定位
            if (this.module.BaseInfo.IsIdentity)
            {
                return false;
            }

            // module里面的制程都完成了则不需要去做
            if (this.module.IsModuleProcessFinishedInSystem2)
            {
                return false;
            }

            return true;
        }
    }
}
