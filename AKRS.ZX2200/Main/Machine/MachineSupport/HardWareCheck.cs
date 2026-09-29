using AKRS.Galaxy2.LogicHardware.Hardwares;
using AKRS.Galaxy2.LogicHardware.Hardwares.Cameras;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Main.Machine.MachineSupport
{
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.LogicHardware.Hardwares.LightControllers;
    using AKRS.Galaxy2.LogicHardware.HardWares.LaserMeasureHeightControllers;

    /// <summary>
    /// 硬件自检类
    /// </summary>
    public class HardWareCheck
    {
        /// <summary>
        /// 硬件自检
        /// </summary>
        /// <param name="hardwareBase">硬件名称</param>
        /// <param name="name">名称</param>
        public HardWareCheck(string name) 
        {
            this.HardwareBase = HardwareRepositoryService.GetHardware<HardwareBase>(name);

            this.IsExist = HardwareBase != null;

            this.Name = name;

            if (this.HardwareBase != null)
            {
                this.HardwareType = this.HardwareBase.GetType().Name;

                // 轴和相机通过drive来判断是否初始化成功
                if (this.HardwareBase is Axis axis)
                {
                    this.IsInit = axis.AxisDrive != null;
                }
                else if (this.HardwareBase is AKRSCamera camera)
                {
                    this.IsInit = camera.AkrsCamera != null;
                }
                else if (this.HardwareBase is Light light)
                {
                    // 通过光源控制器判断
                    this.IsInit = true;
                }
                else if (this.HardwareBase is Sensor || this.HardwareBase is Electric)
                {
                    // 开卡成功即初始化成功,开卡失败无法进入软件
                    this.IsInit = true;
                }
                else if (this.HardwareBase is Alarmer alarmer)
                {
                    // 三色灯蜂鸣器
                    List<Alarmer> alarmerList = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
                    if (alarmerList == null || alarmerList.Any<Alarmer>())
                    {
                        this.IsInit = false;
                    }
                    else
                    {
                        this.IsInit = true;
                    }
                }
                else
                {
                    // 点胶控制器、激光测高控制器、光源控制器
                    this.IsInit = this.HardwareBase.InitState;
                }
            }
        }

        /// <summary>
        /// 硬件配置
        /// </summary>
        public HardwareBase HardwareBase { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 存在
        /// </summary>
        public bool IsExist { get; set; }

        /// <summary>
        /// 是否初始化成功
        /// </summary>
        public bool IsInit { get; set; } = false;

        /// <summary>
        /// 硬件类型
        /// </summary>
        public string HardwareType { get; set; } = string.Empty;
    }
}
