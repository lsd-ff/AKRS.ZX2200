#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 12:58:29
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

using System.Threading;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    /// <summary>
    /// 描述：点胶载台
    /// </summary>
    public class DispenseSubSectionModule : BaseSubSectionModule
    {
        /// <summary>
        /// 皮带X轴
        /// </summary>
        public override Axis BeltAxisX => HardwareRepositoryService.GetHardware<Axis>("点胶工作台X");

        /// <summary>
        /// 入料检测传感器
        /// </summary>
        internal Sensor FeedMaterialCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区入料检测1");

        /// <summary>
        /// 物料检测传感器
        /// </summary>
        internal Sensor MaterialExistSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区有料检测2");

        /// <summary>
        /// 出料检测传感器
        /// </summary>
        internal Sensor OutMaterialCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("固晶区入料检测1");

        /// <summary>
        /// 轨道顶升气缸
        /// </summary>
        internal Electric TrackLiftCylinderElectric => HardwareRepositoryService.GetHardware<Electric>("点胶区压料气缸电磁阀");

        /// <summary>
        /// 挡料气缸
        /// </summary>   
        internal Electric BlockCylinderElectric1 => HardwareRepositoryService.GetHardware<Electric>("点胶区挡料气缸1电磁阀");

        /// <summary>
        /// 挡料气缸
        /// </summary>
        internal Electric BlockCylinderElectric2 => HardwareRepositoryService.GetHardware<Electric>("点胶区挡料气缸2电磁阀");

        /// <summary>
        /// 基板真空
        /// </summary>
        internal Electric SubstrateVaccumElectric => HardwareRepositoryService.GetHardware<Electric>("点胶区吸料真空电磁阀");

        /// <summary>
        /// 点胶的第二段真空，针对于点胶分段使用的
        /// </summary>
        internal Electric SubstrateAdditionVaccumElectric => HardwareRepositoryService.GetHardware<Electric>("点胶区附加吸料真空");

        /// <summary>
        /// 点胶区上压料气缸1到位检测正限位
        /// </summary>
        internal Sensor TrackFrontCylinder1PLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸动点升起检测1");

        /// <summary>
        /// 点胶区上压料气缸1到位检测负限位
        /// </summary>
        internal Sensor TrackFrontCylinder1NLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸原点下降检测1");

        /// <summary>
        /// 点胶区上压料气缸2到位检测正限位
        /// </summary>
        internal Sensor TrackFrontCylinder2PLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸动点升起检测2");

        /// <summary>
        /// 点胶区上压料气缸2到位检测负限位
        /// </summary>
        internal Sensor TrackFrontCylinder2NLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸原点下降检测2");

        /// <summary>
        /// 点胶区下压料气缸1到位检测正限位
        /// </summary>
        internal Sensor TrackBackCylinder1PLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸动点升起检测3");

        /// <summary>
        /// 点胶区下压料气缸1到位检测负限位
        /// </summary>
        internal Sensor TrackBackCylinder1NLSensor => HardwareRepositoryService.GetHardware<Sensor>("点胶区压料气缸原点下降检测3");

        /// <summary>
        /// 点胶区负压检测
        /// </summary>
        internal Sensor 点胶区负压检测 => HardwareRepositoryService.GetHardware<Sensor>("点胶区负压检测");

        /// <summary>
        /// 点胶载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public override bool HasMaterialInModule()
        {
            return this.MaterialExistSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 入料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInFeedingInlet()
        {
            return this.FeedMaterialCheckSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return this.OutMaterialCheckSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 气缸1 顶起
        /// </summary>
        public void BlockCylinder1Up()
        {
            this.BlockCylinderElectric1.SetOutputValue(true);
        }

        /// <summary>
        /// 气缸1 降落
        /// </summary>
        public void BlockCylinder1Down()
        {
            this.BlockCylinderElectric1.SetOutputValue(false);
        }

        /// <summary>
        /// 气缸2 顶起
        /// </summary>
        public void BlockCylinder2Up()
        {
            this.BlockCylinderElectric2.SetOutputValue(true);
        }

        /// <summary>
        /// 气缸2 降落
        /// </summary>
        public void BlockCylinder2Down()
        {
            this.BlockCylinderElectric2.SetOutputValue(false);
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
