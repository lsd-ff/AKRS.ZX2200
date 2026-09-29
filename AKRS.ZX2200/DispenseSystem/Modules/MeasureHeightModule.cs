using AKRS.Galaxy2.LogicHardware.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.DispenseSystem.Modules
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.HardWares.LaserMeasureHeightControllers;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using System.Threading;
    using System.Windows.Forms;

    /// <summary>
    /// 点胶测高模组
    /// </summary>
    public class MeasureHeightModule
    {
        /// <summary>
        /// isCancelWait
        /// </summary>
        private bool isCancelWait;

        /// <summary>
        /// 点胶Z轴
        /// </summary>
        public Axis DispenseAxisZ => HardwareRepositoryService.GetHardware<Axis>("点胶Z");

        /// <summary>
        /// 激光测高
        /// </summary>
        public LaserMeasureHeight LaserMeasureHeight => HardwareRepositoryService.GetHardware<LaserMeasureHeight>("点胶激光测高");

        /// <summary>
        /// 点胶测高传感器
        /// </summary>        
        public Sensor DispenseAltimemetrySensor => HardwareRepositoryService.GetHardware<Sensor>("点胶测高传感器");

        /// <summary>
        /// 点胶测高气缸
        /// </summary>        
        public Electric DispenseAltimemetryCyclinder => HardwareRepositoryService.GetHardware<Electric>("点胶测高气缸电磁阀");
        
        /// <summary>
        /// 预点胶板压力传感器
        /// </summary>
        public Sensor PreDispensePressureSensor => HardwareRepositoryService.GetHardware<Sensor>("预点胶平台检测");

        /// <summary>
        /// 点胶测高正限位
        /// </summary>        
        public Sensor DispenseAltimemetryCyclinderPLimit => HardwareRepositoryService.GetHardware<Sensor>("点胶测高气缸动点下降检测");

        /// <summary>
        /// 点胶测高原点
        /// </summary>        
        public Sensor DispenseAltimemetryCyclinderNLimit => HardwareRepositoryService.GetHardware<Sensor>("点胶测高气缸原点升起检测");

        /// <summary>
        /// 打开点胶气缸
        /// </summary>
        public void OpenDispenseAltimemetryCyclinder()
        {
            if (this.DispenseAltimemetryCyclinder == null)
            {
                throw new Exception("未找到点胶测高气缸电磁阀，请检查硬件后重试");
            }

            this.DispenseAltimemetryCyclinder.SetOutputValue(true);
        }

        /// <summary>
        /// 关闭点胶气缸
        /// </summary>
        public void CloseDispenseAltimemetryCyclinder()
        {
            if (this.DispenseAltimemetryCyclinder == null)
            {
                throw new Exception("未找到点胶测高气缸电磁阀，请检查硬件后重试");
            }

            this.DispenseAltimemetryCyclinder.SetOutputValue(false);
        }

        /// <summary>
        /// 点胶气缸是否已经打开
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispenseAltimemetryCyclinderOpened()
        {
            if (this.DispenseAltimemetryCyclinderPLimit == null)
            {
                throw new Exception("未找到点胶测高气缸动点下降检测，请检查硬件后重试");
            }

            return this.DispenseAltimemetryCyclinderPLimit.GetInputValue();
        }

        /// <summary>
        /// 点胶气缸是否已经打开
        /// </summary>
        /// <returns>结果</returns>
        public bool IsDispenseAltimemetryCyclinderClosed()
        {
            if (this.DispenseAltimemetryCyclinderNLimit == null)
            {
                throw new Exception("未找到点胶测高气缸原点升起检测，请检查硬件后重试");
            }

            return this.DispenseAltimemetryCyclinderNLimit.GetInputValue();
        }
    }
}
