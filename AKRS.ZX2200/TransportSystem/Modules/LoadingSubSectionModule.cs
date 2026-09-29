#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 12:57:45
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
    /// 描述：上料载台
    /// </summary>
    public class LoadingSubSectionModule : BaseSubSectionModule
    {
        #region 未配置上下料

        /// <summary>
        /// 后端物料传感器
        /// </summary>
        public Sensor MaterialExistEndSensor => HardwareRepositoryService.GetHardware<Sensor>("入料区有料检测右");

        /// <summary>
        /// 皮带X轴
        /// </summary>
        public override Axis BeltAxisX => HardwareRepositoryService.GetHardware<Axis>("进料工作台X");

        #endregion


        #region 联机

        /// <summary>
        /// 前端物料传感器
        /// </summary>
        public Sensor MaterialExistFrontSensor => HardwareRepositoryService.GetHardware<Sensor>("入料区入料检测（左）");

        /// <summary>
        /// 进料载台要料信号
        /// </summary>
        public Electric LoadingTableNeedTabletSignal => HardwareRepositoryService.GetHardware<Electric>("上游SMEMA要料信号");

        #endregion

        /// <summary>
        /// 前段是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialOnFront()
        {
            return this.MaterialExistFrontSensor.GetInputValue();
        }

        /// <summary>
        /// 载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public override bool HasMaterialInModule()
        {
            return this.MaterialExistEndSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 设置需料信号
        /// </summary>
        /// <param name="isNeedTablet">是否要料</param>
        public void SetLoadingTableNeedTabletSignal(bool isNeedTablet)
        {
            this.LoadingTableNeedTabletSignal.SetOutputValue(isNeedTablet);
        }
    }
}
