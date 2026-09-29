using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.ZX2200.BondSystem.Modules;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    /// <summary>
    ///  机器硬件
    /// </summary>
    public class MachineModule : SingletonNoSave<MachineModule>
    {
        /// <summary>
        /// 摇杆X方向模拟量读取
        /// </summary>
        public Electric JoystickXRead => HardwareRepositoryService.GetHardware<Electric>("摇杆X方向模拟量读取");

        /// <summary>
        /// 焊头压力表模拟量读取
        /// </summary>
        public Electric JoystickYRead => HardwareRepositoryService.GetHardware<Electric>("摇杆Y方向模拟量读取");

        /// <summary>
        /// 摇杆T方向模拟量读取
        /// </summary>
        public Electric JoystickTRead => HardwareRepositoryService.GetHardware<Electric>("摇杆T方向模拟量读取");

        /// <summary>
        /// 门控检测传感器
        /// </summary>        
        private Sensor SaferDoorSensor => HardwareRepositoryService.GetHardware<Sensor>("安全门");
    }
}
