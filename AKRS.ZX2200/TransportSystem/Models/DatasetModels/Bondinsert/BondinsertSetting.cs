#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/18 15:53:15
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
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;

namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 描述：挡料气缸的相关设置
    /// </summary>
    [Serializable]
    public class BondinsertSetting : BaseDsSetting
    {
        /// <summary>
        /// 最后一个sensor与第一个挡料气缸的距离
        /// Distance between the last sensor before the bonding insert and the first hardstop
        /// </summary>
        [TreeProgramListArgs("最后一个传感器与第一个挡料气缸的距离", "距离", UnitHelper.mm)]
        public double HardstopLeftDistance { get; set; }

        /// <summary>
        /// 最后一个sensor与第二个挡料气缸的距离
        /// Distance between the last sensor before the bonding insert and the second hardstop
        /// </summary>
        [TreeProgramListArgs("最后一个传感器与第二个挡料气缸的距离", "距离", UnitHelper.mm)]
        public double HardstopRightDistance { get; set; }

        /// <summary>
        /// 到达挡料气缸后 延迟多长时间 夹紧传输单元
        /// Delay Time with which the transport unit is clamped after reaching the hard stop
        /// </summary>
        [TreeProgramListArgs("夹紧传输单元后延时", "夹紧", UnitHelper.ms)]
        public int BeforeClampingDelay { get; set; }

        /// <summary>
        /// 关闭真空后延迟多长时间松开夹爪
        /// delay Time with which the transport unit is unclamped after switch off the vacuum
        /// </summary>
        [TreeProgramListArgs("关闭真空后延时", "夹紧", UnitHelper.ms)]
        public int AfterClampingDelay { get; set; }

        /// <summary>
        /// 松开夹爪前延迟
        /// </summary>
        [TreeProgramListArgs("松开夹爪前延时", "松开", UnitHelper.ms)]
        public int BeforeUnClampingDelay { get; set; }

        /// <summary>
        /// 松开夹爪后延迟
        /// </summary>
        [TreeProgramListArgs("松开夹爪后延时", "松开", UnitHelper.ms)]
        public int AfterUnClampingDelay { get; set; }

        /// <summary>
        /// 真空配置选项枚举，此参数暂时没用
        /// </summary>
        //[TreeProgramListArgs("真空配置选项", "夹紧")]
        public VacuumOptionEnum VacuumOption { get; set; }
    }
}
