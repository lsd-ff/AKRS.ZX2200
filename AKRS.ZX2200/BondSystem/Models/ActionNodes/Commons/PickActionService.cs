using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.ZX2200.BondSystem.BondForce;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Modules;
    using System.ComponentModel;

    /// <summary>
    /// pick动作帮助类
    /// </summary>
    public class PickActionService
    {
        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 获取取片参数
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="pickType">取片类型</param>
        /// <returns>参数</returns>
        /// <exception cref="ArgumentNullException">异常</exception>
        public PickActionParameter GetPickActionParameter(BaseCarrierConfig component, PickTypeEnum pickType)
        {
            if (component == null)
            {
                throw new ArgumentNullException("GetBondActionParameter 不能为空");
            }

            PickActionParameter pickActionParameter = new PickActionParameter();

            if (pickType == PickTypeEnum.LeftIPT|| pickType == PickTypeEnum.RightIPT)
            {
                pickActionParameter.IsActiveSlowTravelBeforePick = component.IsActivateSlowTravelBeforePickOnIPT;

                if (component.IsActivateSlowTravelBeforePickOnIPT)
                {
                    // 取片前低速段距离
                    pickActionParameter.SlowTravelDistanceBeforePickup = component.IPTSlowTravelDistanceBeforePickup;

                    // 取片前低速段速度
                    pickActionParameter.SlowTravelSpeedBeforePickup = component.IPTSlowTravelSpeedBeforePickup;
                }

                pickActionParameter.IsActiveSlowTravelAfterPick = component.IsActivateSlowTravelAfterPickOnIPT;

                if (component.IsActivateSlowTravelAfterPickOnIPT)
                {
                    // 取片后低速段距离
                    pickActionParameter.SlowTravelDistanceAfterPickup = component.IPTSlowTravelDistanceAfterPickup;

                    // 取片后低速段速度
                    pickActionParameter.SlowTravelSpeedAfterPickup = component.IPTSlowTravelSpeedAfterPickup;
                }

                // 取片延时
                pickActionParameter.PickDelay = component.IPTPickupDelay;

                // 力
                pickActionParameter.PickForce = component.IPTPickupForce;

                pickActionParameter.forceMode = component.IPTPickupForceMode                                 ;
            }
            else
            {
                pickActionParameter.IsActiveSlowTravelBeforePick = component.IsActivateSlowTravelBeforePickup;

                if (component.IsActivateSlowTravelBeforePickup)
                {
                    // 取片前低速段距离
                    pickActionParameter.SlowTravelDistanceBeforePickup = component.SlowTravelDistanceBeforePickup;

                    // 取片前低速段速度
                    pickActionParameter.SlowTravelSpeedBeforePickup = component.SlowTravelSpeedBeforePickup;
                }

                pickActionParameter.IsActiveSlowTravelAfterPick = component.IsActivateSlowTravelAfterPickup;

                if (component.IsActivateSlowTravelAfterPickup)
                {
                    // 取片后低速段距离
                    pickActionParameter.SlowTravelDistanceAfterPickup = component.SlowTravelDistanceAfterPickup;

                    // 取片后低速段速度
                    pickActionParameter.SlowTravelSpeedAfterPickup = component.SlowTravelSpeedAfterPickup;
                }

                // 取片延时
                pickActionParameter.PickDelay =component.PickupDelay;

                // 力
                pickActionParameter.PickForce = component.PickupForce;

                pickActionParameter.forceMode = component.PickupForceMode;
            }       

            // 吸嘴吸真空延时
             //pickActionParameter.VacuumBuildUpDelay = pickType == PickTypeEnum.IPT
             //                         ? component.IPTVacuumBuildUpDelay
             //                         : component.NozzleVacuumBuildUpTime;

            switch (pickType)
            {
                case PickTypeEnum.LeftIPT:
                    pickActionParameter.VacuumElectric = System2Module.GetInstance().IPTModule.IPTVacuumElectric;
                  
                    {
                        pickActionParameter.BlowElectric = System2Module.GetInstance().IPTModule.IPTBlowElectric;
                        pickActionParameter.TableBlowDelay = component.IPTBlowingDelayDuringPlaceOnIPT;
                    }

                    break;

                case PickTypeEnum.RightIPT :
                    pickActionParameter.VacuumElectric = System2Module.GetInstance().IPTModule.RightIPTVacuumElectric;

                    {
                        pickActionParameter.BlowElectric = System2Module.GetInstance().IPTModule.RightIPTBlowElectric;
                        pickActionParameter.TableBlowDelay = component.IPTBlowingDelayDuringPlaceOnIPT;
                    }

                    break;

                case PickTypeEnum.BMC:
                    pickActionParameter.VacuumElectric = System2Module.GetInstance().BMCVacuumElectric;
                    break;

                case PickTypeEnum.FlipTable:
                    pickActionParameter.VacuumElectric = WaferSubModule.GetInstance().FlipModule.FlipTableVacuum;
                    pickActionParameter.BlowElectric = WaferSubModule.GetInstance().FlipModule.FlipTableBlowProportionalElectric;
                    pickActionParameter.TableBlowDelay = WaferSystemProgram.GetInstance().FlipModuleProgram.CurrentFlipTool.BlowDelay;
                    break;

                default:
                    pickActionParameter.VacuumElectric = null;
                    break;
            }

            pickActionParameter.ForceControlOutTime = component.ForceControlOutTime;

            // 这个参数只在取Wafer芯片时生效
            pickActionParameter.IsResetForceControlAdvanceDuringPickup =
                pickType == PickTypeEnum.CarrierWithWafer ? component.IsResetForceControlAdvanceDuringPickup : false;

            return pickActionParameter;
        }

        /// <summary>
        /// 二段速运动到取片高度
        /// </summary>
        /// <param name="pickLevel">取片高度</param>
        /// <param name="pickActionParameter">取片参数</param>
        public void SlowDownToPickLevel(double pickLevel, PickActionParameter pickActionParameter)
        {
            // 当前速度
            double speed = this.bondHeadController.GetAxisZAbsoluteSpeed();

            if (pickActionParameter.forceMode == ForceModeEnum.Distance)
            {
                if (pickActionParameter.IsActiveSlowTravelBeforePick)
                {
                    // 低速移动到取料高度
                    this.bondHeadController.MoveAxisZ(
                        pickLevel,
                        pickActionParameter.SlowTravelSpeedBeforePickup,
                        AccuracyMode.HighAccuracy,
                        false);
                }
            }
            else
            {
                double curLevel = this.bondHeadController.GetAxisZRealPos();

                // 力控模式低速下压
                // 这个speed的参数只作用于Etel,作用的距离是减去的这0.01mm
                this.bondHeadController.ForceControlSet(
                    pickActionParameter.PickForce,
                    curLevel - 0.01,
                    speed,
                    pickActionParameter.ForceControlOutTime,
                    pickActionParameter.SlowTravelDistanceBeforePickup);
            }
        }
    }
}
