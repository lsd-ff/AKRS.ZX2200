using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Drive.Common;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using LanguageExt.TypeClasses;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.BPActionNode
{
    /// <summary>
    /// PickActionNode帮助类
    /// </summary>
    public partial class SubstrateCameraCorrectionActionNode
    {
        /// <summary>
        /// 准备动作
        /// </summary>
        private void PrepareBeforeAction()
        {
            this.initialSpeed = this.bondHeadController.GetAxisZAbsoluteSpeed();

            // 获取下一颗芯片
            this.nextComponentName = this.actionNodesService.GetNextComponentName();

            // 获取当前焊点
            this.bondPosition = this.system2Domain.ActionNodesService.GetCurrentBondPosition();

            // 获取当前吸嘴
            this.nozzle = this.bondHeadController.GetCurrentNozzle();

            this.zSpeed = this.bondHeadController.GetAxisZAbsoluteSpeed();
        }

        /// <summary>
        /// 给晶圆台发信号
        /// </summary>
        private void SendSignalToWaferTable()
        {
            // 判断是不是最后一颗芯片
            if (this.nextComponentName != this.component.Name && this.nextComponentName != null)
            {
                // 如果是最后一颗就在这里发要料信号
                // 刷新Map信息
                WaferSystemDomain.GetInstance().Block.RefreshMap();

                // 芯片名称传给晶圆台
                WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(this.nextComponentName);

                // 给晶圆台发要料信号
                SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                // 自动重取次数清零
                System2RunTimeProvider.UpLookAutoSkipCount = 0;

                // 跳过次数清零
                System2RunTimeProvider.StaticWaffleComponentAutoSkipCount = 0;

                System2RunTimeProvider.RecordTime("取片信号交互", $"中转台流程完成，给晶圆发要料信号，芯片名称{this.component.Name}");
            }
        }

        /// <summary>
        /// 异步开吹气
        /// </summary>
        /// <param name="bondActionParameter">固精参数</param>
        private void BlowAsyn(BondActionParameter bondActionParameter)
        {
            if (bondActionParameter.BlowingDelay != 0)
            {
                Task.Run(() =>
                    {
                        this.bondHeadController.OpenToolBlowEle(this.component.IPTWeakBlowProportion);

                        System2RunTimeProvider.RecordTime("Place", $"开弱吹气完成");

                        // 弱吹气延迟
                        DelayHelper.Delay(bondActionParameter.BlowingDelay);

                        System2RunTimeProvider.RecordTime("Place", $" 弱吹气延时: {bondActionParameter.BlowingDelay}ms");

                        // 关闭弱吹气
                        this.bondHeadController.CloseToolBlowEle();

                        System2RunTimeProvider.RecordTime("Place", $"关弱吹气完成");
                    });
            }
        }

        /// <summary>
        /// 焊头清零
        /// </summary>
        private void ZeroBondheadBeforePick()
        {
            if (this.component.IPTPickupForceMode == ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.IPTPickupForce);

                this.bondHeadController.ZeroBondhead(isSmallForce);
            }
        }

        /// <summary>
        /// 焊头清零
        /// </summary>
        private void ZeroBondheadBeforePlace()
        {
            if (this.component.IPTPlacementForceMode == ForceModeEnum.Force)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.component.IPTPlaceForce);

                this.bondHeadController.ZeroBondhead(isSmallForce);
            }
        }
    }
}
