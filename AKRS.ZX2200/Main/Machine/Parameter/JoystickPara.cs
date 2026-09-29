using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.Parameter
{
    /// <summary>
    ///  摇杆参数
    /// </summary>
    public class JoystickPara
    {
        /// <summary>
        /// 摇杆X方向初始值
        /// </summary>
        public double XInitialVal { get; set; } = 2500;

        /// <summary>
        /// 摇杆X方向上限
        /// </summary>
        public double XUpperLimit { get; set; }

        /// <summary>
        /// 摇杆Y方向初始值
        /// </summary>
        public double YInitialVal { get; set; } = 2500;

        /// <summary>
        /// 摇杆Y方向上限
        /// </summary>
        public double YUpperLimit { get; set; }

        /// <summary>
        /// 摇杆T方向初始值
        /// </summary>
        public double TInitialVal { get; set; } = 2450;

        /// <summary>
        /// 摇杆T方向上限
        /// </summary>
        public double TUpperLimit { get; set; }

        /// <summary>
        /// 盲区
        /// </summary>
        public double BlindRange { get; set; } = 50;
    }
}
