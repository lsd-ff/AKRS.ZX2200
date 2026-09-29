using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.TemperatureControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    /// <summary>
    /// 大载板
    /// </summary>
    public class BondMaxSubSectionModule
    {
        /// <summary>
        /// 真空检测1
        /// </summary>
        public Sensor BondMaxSubSectionVacuumCheckMiddle => HardwareRepositoryService.GetHardware<Sensor>("工作台负压检测中");

        /// <summary>
        /// 真空检测2
        /// </summary>
        public Sensor BondMaxSubSectionVacuumCheckFront => HardwareRepositoryService.GetHardware<Sensor>("工作台负压检测前");

        /// <summary>
        /// 真空检测3
        /// </summary>
        public Sensor BondMaxSubSectionVacuumCheckRight => HardwareRepositoryService.GetHardware<Sensor>("工作台负压检测右");

        /// <summary>
        /// 真空1
        /// </summary>
        public Electric BondMaxSubSectionVacuumMiddle => HardwareRepositoryService.GetHardware<Electric>("工作台负压中电磁阀");

        /// <summary>
        /// 真空2
        /// </summary>
        public Electric BondMaxSubSectionVacuumFront => HardwareRepositoryService.GetHardware<Electric>("工作台负压前电磁阀");

        /// <summary>
        /// 真空3
        /// </summary>
        public Electric BondMaxSubSectionVacuumRight => HardwareRepositoryService.GetHardware<Electric>("工作台负压右电磁阀");

        /// <summary>
        /// 真空3
        /// </summary>
        public Electric BondMaxSubHeaterElectric => HardwareRepositoryService.GetHardware<Electric>("工作台加热");

        /// <summary>
        /// 加热器
        /// </summary>
        public Heater BondMaxSubHeater => HardwareRepositoryService.GetHardware<Heater>("加热器1");
    }
}
