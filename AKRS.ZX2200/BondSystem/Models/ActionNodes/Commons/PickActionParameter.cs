using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.ZX2200.BondSystem.Models.Enums;
using DevExpress.DashboardCommon;

namespace AKRS.ZX2200.BondSystem.Models.ActionNodes.Commons
{
    /// <summary>
    /// 取料参数
    /// </summary>
    public class PickActionParameter
    {
        /// <summary>
        /// 取片前低速段距离
        /// </summary>
        public bool IsActiveSlowTravelBeforePick { get; set; }

        /// <summary>
        /// 取片前低速段距离
        /// </summary>
        public bool IsActiveSlowTravelAfterPick { get; set; }

        /// <summary>
        /// 取片前低速段距离
        /// </summary>
        public double SlowTravelDistanceBeforePickup { get; set; } = 0;

        /// <summary>
        /// 取片前低速段速度
        /// </summary>
        public double SlowTravelSpeedBeforePickup { get; set; }

        /// <summary>
        /// 取片后低速段距离
        /// </summary>
        public double SlowTravelDistanceAfterPickup { get; set; } = 0;

        /// <summary>
        /// 取片后低速段速度
        /// </summary>
        public double SlowTravelSpeedAfterPickup { get; set; }

        /// <summary>
        /// 取片延时
        /// </summary>
        public int PickDelay { get; set; }

        /// <summary>
        /// 吸嘴吸真空延时
        /// </summary>
       // public int VacuumBuildUpDelay { get; set; }

        /// <summary>
        /// 力度
        /// </summary>
        public double PickForce { get; set; }

        /// <summary>
        /// 真空电磁阀
        /// </summary>
        public Electric VacuumElectric { get; set; }

        /// <summary>
        /// 吹气电磁阀
        /// </summary>
        public Electric BlowElectric { get; set; } = null;

        /// <summary>
        /// 平台吹气延时
        /// </summary>
        public int TableBlowDelay { get; set; } = 0;

        /// <summary>
        ///  力控模式
        /// </summary>
        public ForceModeEnum forceMode { get; set; }

        /// <summary>
        /// 力控超时时间
        /// </summary>
        public int ForceControlOutTime { get; set; }

        /// <summary>
        ///  取片提前退出力控
        /// </summary>
        public bool IsResetForceControlAdvanceDuringPickup { get; set; }
    }
}
