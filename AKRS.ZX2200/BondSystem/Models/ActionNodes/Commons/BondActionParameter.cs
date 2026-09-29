using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.ZX2200.BondSystem.Models.Enums;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    /// <summary>
    /// BondAction 参数
    /// </summary>
    public class BondActionParameter
    {
        /// <summary>
        /// 固晶前低速段距离
        /// </summary>
        public double SlowTravelDistanceBeforePlace { get; set; } = 0;

        /// <summary>
        /// 固晶前低速段速度
        /// </summary>
        public double SlowTravelSpeedBeforePlace { get; set; }

        /// <summary>
        /// 固晶后低速段距离
        /// </summary>
        public double SlowTravelDistanceAfterPlace { get; set; } = 0;

        /// <summary>
        /// 固晶后低速段速度
        /// </summary>
        public double SlowTravelSpeedAfterPlace { get; set; } 

        /// <summary>
        /// 固精延时
        /// </summary>
        public int PlacementDelay { get; set; }

        /// <summary>
        /// 吸嘴关真空真空延时
        /// </summary>
        public int VacuumOffDelay { get; set; }

        /// <summary>
        /// 吸嘴弱吹延时
        /// </summary>
        public int BlowingDelay { get; set; }

        /// <summary>
        /// 力度
        /// </summary>
        public double BondForce { get; set; }

        /// <summary>
        /// 真空电磁阀
        /// </summary>
        public Electric VacuumElectric { get; set; }

        /// <summary>
        ///  力控模式
        /// </summary>
        public ForceModeEnum forceMode { get; set; }
    }
}
