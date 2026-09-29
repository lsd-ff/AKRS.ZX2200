#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/10/19 9:50:03
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportSystem.Models.Enums
{
    /// <summary>
    /// 描述：夹紧方式
    /// </summary>
    public enum ClampModeEnum
    {
        /// <summary>
        /// 仅真空
        /// </summary>
        [Description("真空")]
        Vacuum,

        /// <summary>
        /// 仅降低轨道
        /// </summary>
        [Description("降低轨道")]
        Lowering,

        /// <summary>
        /// 真空后降低轨道
        /// </summary>
        [Description("真空后降低轨道")]
        VacuumAndLowering,

        /// <summary>
        /// 降低轨道后真空
        /// </summary>
        [Description("降低轨道后真空")]
        LoweringAndVacuum
    }

    /// <summary>
    /// 描述：松夹方式
    /// </summary>
    public enum UnClampModeEnum
    {
        /// <summary>
        /// 仅真空
        /// </summary>
        [Description("真空")]
        Vacuum,

        /// <summary>
        /// 仅升起轨道
        /// </summary>
        [Description("升起轨道")]
        Rising,

        /// <summary>
        /// 关闭真空后上升轨道
        /// </summary>
        [Description("关闭真空后上升轨道")]
        VacuumAndRising,

        /// <summary>
        /// 升起轨道后关闭真空
        /// </summary>
        [Description("升起轨道后关闭真空")]
        RisingAndVacuum
    }

    /// <summary>
    /// 真空方式
    /// </summary>
    public enum VacuumOptionEnum
    {
        /// <summary>
        /// No vacuum
        /// </summary>
        [Description("No vacuum")]
        NoVacuum,

        /// <summary>
        /// With vacuum
        /// </summary>
        [Description("With vacuum")]
        WithVacuum,

        /// <summary>
        /// Vacuum is checked after clamping the transport unit
        /// </summary>
        [Description("With vacuum and check on activation")]
        WithVacuumAndCheckOnActivation,

        /// <summary>
        /// Vacuum is checked after each processing step
        /// </summary>
        [Description("With vacuum and check on activation")]
        WithVacuumAndContinuousCheck
    }

    /// <summary>
    /// 点胶夹爪状态
    /// </summary>
    public enum DispenseClampStatusEnum
    {
        /// <summary>
        /// 夹紧状态
        /// </summary>
        [Description("夹紧")]
        Clamp,

        /// <summary>
        /// 松夹状态
        /// </summary>
        [Description("松开")]
        UnClamp
    }

    /// <summary>
    /// Bond夹爪状态
    /// </summary>
    public enum BondClampStatusEnum
    {
        /// <summary>
        /// 夹紧状态
        /// </summary>
        [Description("夹紧")]
        Clamp,

        /// <summary>
        /// 松夹状态
        /// </summary>
        [Description("松开")]
        UnClamp
    }

    /// <summary>
    /// 上料模式
    /// </summary>
    public enum FeedingTypeEnum
    {
        /// <summary>
        /// 单步上料模式
        /// </summary>
        [Description("单步上料模式")]
        OneFeeding,

        /// <summary>
        /// 连续上料模式
        /// </summary>
        [Description("连续上料模式")]
        ContinuousFeeding,
    }

    /// <summary>
    /// 料盒枚举
    /// </summary>
    public enum BinTypeEnum
    {
        /// <summary>
        /// 料盒A
        /// </summary>
        [Description("料盒A")]
        BinA,

        /// <summary>
        ///  料盒B
        /// </summary>
        [Description("料盒B")]
        BinB,
    }

    /////// <summary>
    /////// 上料配置枚举
    /////// </summary>
    ////public enum LoderConfigurantionEnum
    ////{
    ////    /// <summary>
    ////    /// 皮带
    ////    /// </summary>
    ////   Belt,

    ////    /// <summary>
    ////    /// 料仓
    ////    /// </summary>
    ////    StockBin,

    ////    ///// <summary>
    ////    ///// 循环上下料
    ////    ///// </summary>
    ////    //CycleLoader,
    ////}
}
