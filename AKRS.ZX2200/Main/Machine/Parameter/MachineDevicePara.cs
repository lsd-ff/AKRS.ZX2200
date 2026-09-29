using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.Infrastructure.Models.Path;

namespace AKRS.ZX2200.Main.Machine.Parameter
{
    /// <summary>
    /// 设备参数
    /// </summary>
    public class MachineDevicePara : Singleton<MachineDevicePara>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MachineDevicePara()
        {
            Singleton<MachineDevicePara>.FilePath = ZX2200PathConfig.MachineDeviceParaPath;
        }

        /// <summary>
        ///  主机名
        /// </summary>
        public string MachineName = string.Empty;

        /// <summary>
        ///  MAC地址
        /// </summary>
        public string MacAddress = string.Empty;

        /// <summary>
        ///  摇杆参数
        /// </summary>
        public JoystickPara JoystickPara { get; set; } = new JoystickPara();
    }
}
