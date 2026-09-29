using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    /// <summary>
    /// DipAction 参数
    /// </summary>
    public class DipActionParamter
    {
        /// <summary>
        /// 蘸胶前低速段距离
        /// </summary>
        public double SlowTravelDistanceBeforeDip { get; set; } =2;

        /// <summary>
        /// 蘸胶前低速段速度
        /// </summary>
        public double SlowTravelSpeedBeforeDip { get; set; } = 50;

        /// <summary>
        /// 蘸胶后低速段距离
        /// </summary>
        public double SlowTravelDistanceAfterDip { get; set; } = 2;

        /// <summary>
        /// 蘸胶后低速段速度
        /// </summary>
        public double SlowTravelSpeedAfterDip { get; set; } = 50;

        /// <summary>
        /// 蘸胶延时
        /// </summary>
        public int DipDelay { get; set; } = 20;

        /// <summary>
        /// 力度
        /// </summary>
        public double DipForce { get; set; } = 50;
    }
}
