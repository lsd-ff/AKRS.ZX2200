#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 12:58:49
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Component.Simple.MessageBox;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    /// <summary>
    /// 描述：Bond 载台
    /// </summary>
    public class BondSubSectionModule : BaseSubSectionModule
    {
        /// <summary>
        /// 皮带X轴
        /// </summary>
        public override Axis BeltAxisX => HardwareRepositoryService.GetHardware<Axis>("固晶工作台X");

        /// <summary>
        /// 入料检测传感器
        /// </summary>
        internal Sensor OutMaterialCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区出料检测2");

        /// <summary>
        /// 固晶区挡料气缸
        /// </summary>
        internal Electric BlockCylinderElectric => HardwareRepositoryService.GetHardware<Electric>("固晶区挡料气缸电磁阀");

        /// <summary>
        /// 固晶区压料气缸
        /// </summary>
        internal Electric TrackLiftCylinderElectric => HardwareRepositoryService.GetHardware<Electric>("固晶区压料气缸电磁阀");

        /// <summary>
        /// 基板真空
        /// </summary>
        internal Electric SubstrateVaccumElectric => HardwareRepositoryService.GetHardware<Electric>("固晶区吸料真空电磁阀");

        /// <summary>
        /// 固晶区上压料气缸1到位检测正限位
        /// </summary>
        internal Sensor TrackFrontCylinder1PLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸动点升起检测1");

        /// <summary>
        /// 固晶区上压料气缸1到位检测负限位
        /// </summary>
        internal Sensor TrackFrontCylinder1NLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸原点下降检测1");

        /// <summary>
        /// 固晶区上压料气缸2到位检测正限位
        /// </summary>
        internal Sensor TrackFrontCylinder2PLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸动点升起检测2");

        /// <summary>
        /// 固晶区上压料气缸2到位检测负限位
        /// </summary>
        internal Sensor TrackFrontCylinder2NLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸原点下降检测2");

        /// <summary>
        /// 固晶区下压料气缸1到位检测正限位
        /// </summary>
        internal Sensor TrackBackCylinder1PLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸动点升起检测3");

        /// <summary>
        /// 固晶区下压料气缸1到位检测负限位
        /// </summary>
        internal Sensor TrackBackCylinder1NLSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区压料气缸原点下降检测3");

        /// <summary>
        /// 固晶区负压检测
        /// </summary>
        //internal Sensor 固晶区负压检测 => HardwareRepositoryService.GetHardware<Sensor>("固晶区负压检测");


        /// <summary>
        /// 载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public override bool HasMaterialInModule()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bond夹爪状态
        /// </summary>
        public BondClampStatusEnum BondClampStatus { get; set; }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return OutMaterialCheckSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 气缸1 顶起
        /// </summary>
        public void BlockCylinderUp()
        {
            if (this.BlockCylinderElectric == null)
            {
                return;
            }

            this.BlockCylinderElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 气缸1 降落
        /// </summary>
        public void BlockCylinderDown()
        {
            if (this.BlockCylinderElectric == null)
            {
                return;
            }

            BlockCylinderElectric.SetOutputValue(false);
        }

        /// <summary>
        /// 轨道气缸顶起
        /// </summary>
        public void TrackLiftCylinderUp()
        {
            TrackLiftCylinderElectric.SetOutputValue(true);
        }

        /// <summary>
        /// 轨道气缸降落
        /// </summary>
        public void TrackLiftCylinderDown()
        {
            TrackLiftCylinderElectric.SetOutputValue(false);
        }
    }
}
