#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/15 13:00:11
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
    /// 描述：下料等待载台
    /// </summary>
    public class WaitingUnloadSubSectionModule : BaseSubSectionModule
    {
        /// <summary>
        /// 皮带X轴
        /// </summary>
        public override Axis BeltAxisX => HardwareRepositoryService.GetHardware<Axis>("固晶出料等待区X");

        /// <summary>
        /// 卡料
        /// </summary>
        public Sensor MaterialExistSensor => HardwareRepositoryService.GetHardware<Sensor>("出料等待区有料检测1");

        /// <summary>
        /// 载台是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public override bool HasMaterialInModule()
        {
            return MaterialExistSensor.CheckStateForNums(true, 1, 0);
        }

        /// <summary>
        /// 出料口是否有料
        /// </summary>
        /// <returns>true: 有料， false： 无料</returns>
        public bool HasMaterialInOutlet()
        {
            return MaterialExistSensor.CheckStateForNums(true, 1, 0);
        }
    }
}
