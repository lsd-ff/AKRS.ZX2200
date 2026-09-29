using System;
using AKRS.ZX2200.BondSystem.BondForce;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    /// <summary>
    /// bond action 
    /// </summary>
    internal class BondActionService
    {
        /// <summary>
        /// 获取贴片参数
        /// </summary>
        /// <param name="component">芯片</param>
        /// <param name="bondType">贴片类型</param>
        /// <returns>参数</returns>
        /// <exception cref="ArgumentNullException">异常</exception>
        public BondActionParameter GetBondActionParameter(BaseCarrierConfig component, BondTypeEnum bondType)
        {
            if (component == null)
            {
                throw new ArgumentNullException("GetBondActionParameter 不能为空");
            }

            BondActionParameter bondActionParameter = new BondActionParameter();

            if (bondType == BondTypeEnum.BondOnTU || bondType == BondTypeEnum.DipFluxOnTU|| bondType == BondTypeEnum.BondOnBMC)
            {
                if (component.IsActivateSlowTravelBeforeBonding)
                {
                    // 固晶前低速段距离
                    bondActionParameter.SlowTravelDistanceBeforePlace = component.SlowTravelDistanceBeforeBonding;


                    // 固晶前低速段速度
                    bondActionParameter.SlowTravelSpeedBeforePlace = component.SlowTravelSpeedBeforeBonding;
                }

                if (component.IsActivateSlowTravelAfterBonding)
                {
                    // 固晶后低速段距离
                    bondActionParameter.SlowTravelDistanceAfterPlace = component.SlowTravelDistanceAfterBonding;

                    // 固晶后低速段速度
                    bondActionParameter.SlowTravelSpeedAfterPlace = component.SlowTravelSpeedAfterBonding;
                }

                // 固精延时
                bondActionParameter.PlacementDelay = component.PlacementDelay;

                                // 吸嘴关真空真空延时
            bondActionParameter.VacuumOffDelay =component.VacuumOffDelay;
                ;

                // 吸嘴弱吹延时
                bondActionParameter.BlowingDelay = component.BondingBlowDelay;

                // 力度
                bondActionParameter.BondForce = component.BondingForce;

                bondActionParameter.forceMode = component.BondingForceMode;
            }
            else
            {
                if (component.IsActivateSlowTravelBeforePlaceOnIPT)
                {
                    // 固晶前低速段距离
                    bondActionParameter.SlowTravelDistanceBeforePlace = component.IPTSlowTravelDistanceBeforeBonding;


                    // 固晶前低速段速度
                    bondActionParameter.SlowTravelSpeedBeforePlace = component.IPTSlowTravelSpeedBeforeBonding;
                }

                if (component.IsActivateSlowTravelAfterPlaceOnIPT)
                {
                    // 固晶前低速段距离
                    bondActionParameter.SlowTravelDistanceAfterPlace = component.IPTSlowTravelDistanceAfterBonding;


                    // 固晶前低速段速度
                    bondActionParameter.SlowTravelSpeedAfterPlace = component.IPTSlowTravelSpeedAfterBonding;
                }

                // 固精延时
                bondActionParameter.PlacementDelay = 
                                                      component.IPTPlacementDelay;

                // 吸嘴关真空真空延时
                bondActionParameter.VacuumOffDelay = component.NozzleVacuumOffDelayDuringPlaceOnIPT;

                // 吸嘴弱吹延时
                bondActionParameter.BlowingDelay = component.NozzleBlowingDelayDuringPlaceOnIPT;

                // 力度
                bondActionParameter.BondForce = component.IPTPlaceForce;

                bondActionParameter.forceMode = component.IPTPlacementForceMode;
            }

            switch (bondType)
            {
                case BondTypeEnum.BondOnLeftIPT:
                    bondActionParameter.VacuumElectric = System2Module.GetInstance().IPTModule.IPTVacuumElectric;
                    break;

                case BondTypeEnum.BondOnRightIPT:
                    bondActionParameter.VacuumElectric = System2Module.GetInstance().IPTModule.RightIPTVacuumElectric;
                    break;

                case BondTypeEnum.BondOnBMC:
                    bondActionParameter.VacuumElectric = System2Module.GetInstance().BMCVacuumElectric;
                    break;

                default:
                    bondActionParameter.VacuumElectric = null;
                    break;
            }

            return bondActionParameter;
        }
    }
}
