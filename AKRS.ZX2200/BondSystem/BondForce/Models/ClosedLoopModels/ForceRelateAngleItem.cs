using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels
{
    /// <summary>
    /// 角度绑定力值关系
    /// </summary>
    public class ForceRelateAngleItem
    {
        /// <summary>
        ///  标定时的角度
        /// </summary>
        public double Angle;

        /// <summary>
        /// 理论力和实际力对象存储集合
        /// </summary>
        public List<TheoreticalForceAndActualForce> TheoreticalForceAndActualForceList { get; set; } = new List<TheoreticalForceAndActualForce>();
    }

    /// <summary>
    /// 理论力和实际力(g)
    /// </summary>
    public class TheoreticalForceAndActualForce
    {
        /// <summary>
        /// 理论力值，指令输入的力(模拟量)
        /// </summary>
        public double InputForce { get; set; }

        /// <summary>
        /// 从标定台读到的力g
        /// </summary>
        public double CaliTableForce { get; set; }

        /// <summary>
        /// 从焊头应变片读到的力g
        /// </summary>
        public double BondHeadForce { get; set; }

        /// <summary>
        /// 力控下压前的初始值(模拟量)
        /// </summary>
        public double InitialValue { get; set; }

        /// <summary>
        /// 力控模拟量增量
        /// </summary>
        public double ForceIncrement { get; set; }

        /// <summary>
        /// Z轴位置
        /// </summary>
        public double ZAxisPos { get; set; }
    }
}
