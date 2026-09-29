#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 13:00:34
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
using System.Threading.Tasks;
using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
using AKRS.Galaxy2.LogicHardware.Repository;

namespace AKRS.ZX2200.TransportSystem.Modules
{
    /// <summary>
    /// 描述：下料皮带模组
    /// </summary>
    public class UnloadingSubSectionModule : BaseSubSectionModule
    {
        #region 未配置上下料

        /// <summary>
        /// 物料检测传感器 G
        /// </summary>
        public Sensor FeedMaterialCheckSensor => HardwareRepositoryService.GetHardware<Sensor>("下料区入料检测");

        /// <summary>
        /// 物料检测传感器 H
        /// </summary>
        public Sensor MaterialExistSensor => HardwareRepositoryService.GetHardware<Sensor>("下料区有料检测");

        /// <summary>
        /// 皮带X轴
        /// </summary>
        public override Axis BeltAxisX => HardwareRepositoryService.GetHardware<Axis>("出料工作台X");

        #endregion

        #region 联机
        
        /// <summary>
        /// 收板机要料信号
        /// </summary>
        public Sensor AutoUnLoaderNeedMaterialSignal => HardwareRepositoryService.GetHardware<Sensor>("下游Smema要料信号");

        /// <summary>
        /// 下料区后端物料检测
        /// </summary>
        public Sensor MaterialExistInOutletSensor => HardwareRepositoryService.GetHardware<Sensor>("下料区后端物料检测");

        #endregion

        /// <summary>
        /// 载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public override bool HasMaterialInModule()
        {
            return MaterialExistSensor.CheckStateForNums(true, 1, 200);
        }

        /// <summary>
        /// 入料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInFeedingInlet()
        {
            return this.FeedMaterialCheckSensor.CheckStateForNums(true, 1, 200);
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return this.MaterialExistInOutletSensor.CheckStateForNums(true, 1, 200);
        }

       /// <summary>
       /// 获取收板机要料信号
       /// </summary>
       /// <returns>结果</returns>
        public bool GetAutoUnLoaderNeedMaterialSignal()
        {
            return this.AutoUnLoaderNeedMaterialSignal.GetInputValue();
        }
    }
}
