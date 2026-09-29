using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    /// <summary>
    /// 蘸胶Provider
    /// </summary>
    public class DipActionService
    {
       /// <summary>
       /// 获取蘸胶参数
       /// </summary>
       /// <param name="component">芯片</param>
       /// <returns>参数</returns>
       /// <exception cref="ArgumentNullException">异常</exception>
        public DipActionParamter GetDipActionParameter(BaseCarrierConfig component)
        {
            if (component == null)
            {
                throw new ArgumentNullException("获取蘸胶参数，芯片不能为空");
            }

            DipActionParamter dipActionParameter = new DipActionParamter();

            if (component.IsActivateSlowTravelBeforeDip)
            {
                // 固晶前低速段距离
                dipActionParameter.SlowTravelDistanceBeforeDip = component.SlowTravelDistanceBeforeDipFlux;


                // 固晶前低速段速度
                dipActionParameter.SlowTravelSpeedBeforeDip = component.SlowTravelSpeedBeforeDipFlux;
            }

            if (component.IsActivateSlowTravelAfterDip)
            {
                // 固晶后低速段距离
                dipActionParameter.SlowTravelDistanceAfterDip = component.SlowTravelDistanceAfterDipFlux;

                // 固晶后低速段速度
                dipActionParameter.SlowTravelSpeedAfterDip = component.SlowTravelSpeedAfterDipFlux;
            }

            // 固精延时
            dipActionParameter.DipDelay = component.DipDelay;

            dipActionParameter.DipForce = component.DipFluxForce;

            return dipActionParameter;
        }
    }
}
