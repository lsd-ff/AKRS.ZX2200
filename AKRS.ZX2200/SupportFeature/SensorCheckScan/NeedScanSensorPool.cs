using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.SensorCheckScan
{
    using System.Threading;
    using System.Windows.Forms;
    using AKRS.Galaxy2.BackgroundWorkThread;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Controllers;
    using AKRS.ZX2200.TransportSystem.Models;

    /// <summary>
    /// 需要检查的传感器扫描池
    /// </summary>
    public class NeedScanSensorPool : SingletonNoSave<NeedScanSensorPool>
    {
        /// <summary>
        /// 线程集合
        /// </summary>
        private readonly List<Sensor> sensors = new List<Sensor>();

        /// <summary>
        /// 加入传感器
        /// </summary>
        public void Init()
        {
            this.sensors.Add(HardwareRepositoryService.GetHardware<Sensor>("总正压检测"));
            this.sensors.Add(HardwareRepositoryService.GetHardware<Sensor>("总负压检测"));
            this.sensors.Add(HardwareRepositoryService.GetHardware<Sensor>("焊头气浮正压检测"));
        }

        /// <summary>
        /// 扫码线程启动
        /// </summary>
        public void StartScan()
        {
            this.ScanSensor();
        }

        /// <summary>
        /// 扫描线程
        /// </summary>
        private void ScanSensor()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == Galaxy2.Machine.Enums.MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if(this.sensors.Count==0)
            {
                this.Init();
            }

            foreach (var sensor in this.sensors)
            {
                if (sensor != null && !sensor.GetInputValue())
                {
                    //AKRSMessageBoxExt.Show(
                    //    $"{sensor.HardwareName}传感器未检测到信号",
                    //    "传感器报警",
                    //    new string[] { "确认" },
                    //    new DialogResult[] { DialogResult.OK },
                    //    AlarmLevel.FirstLevel);
                }
            }

            if (Machine.GetInstance().IsWorking()
                && !MachineHardwareConfiguration.GetInstance().IsTransportConfigured
                && TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit != null
                && !TransportDomain.GetInstance().TransportController.BondSubSectionController.CheckVacuum())
            {
                Machine.GetInstance().Stop();

                AKRSMessageBoxExt.Show(
                    $"产品负压不足报警，请检查是否存在漏真空",
                    "工作台负压报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK },
                    AlarmLevel.FirstLevel);
            }
        }
    }
}
