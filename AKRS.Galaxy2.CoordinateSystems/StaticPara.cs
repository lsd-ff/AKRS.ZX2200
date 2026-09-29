using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.CoordinateSystems
{
    using AKRS.Galaxy2.CoordinateSystems.Properties;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// 静态参数
    /// </summary>
    public class StaticPara
    {
        /// <summary>
        /// 文件存放的地址
        /// </summary>
        public static string CoordinateSystemPath { get; set; } = Path.Combine(
            Application.StartupPath,
            "Settings",
            "DeviceParams",
            "MachineCoordinateSystem");

        /// <summary>
        /// MachineCoordinateSystem
        /// </summary>
        public static string MachineCoordinateSystem => Path.Combine(CoordinateSystemPath, "MachineCoordinateSystem.json");
    }
}
