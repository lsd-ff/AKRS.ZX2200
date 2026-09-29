#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/9/18 15:48:25
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
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;

namespace AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt
{
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 描述：流道皮带设置
    /// </summary>
    [Serializable]
    public class TransportBeltSetting : BaseDsSetting
    {
        /// <summary>
        /// 皮带前进速度百分比
        /// </summary>
        [TreeProgramListArgs("皮带前进速度百分比", "皮带设置", 0.1, 1)]
        public double ForwardVelRate { get; set; } = 0.5;

        /// <summary>
        /// 皮带后退速度百分比
        /// </summary>
        [TreeProgramListArgs("皮带后退速度百分比", "皮带设置", 0.1, 1)]
        public double BackwardVelRate { get; set; } = 0.5;

        /// <summary>
        /// 传送时间: ms
        /// </summary>
        [TreeProgramListArgs("传送时间", "皮带设置", 100, 10000, UnitHelper.mm)]
        public double ForwardTransportDistance { get; set; } = 700;

        /// <summary>
        /// 传送距离: ms
        /// </summary>
        [TreeProgramListArgs("传送距离", "皮带设置", 100, 10000, UnitHelper.mm)]
        public double BackwardTransportDistance { get; set; } = 700;

        /// <summary>
        /// 夹紧模式
        /// </summary>
        public ClampModeEnum ClampMode { get; set; } = ClampModeEnum.VacuumAndLowering;

        /// <summary>
        /// 松夹模式
        /// </summary>
        public UnClampModeEnum UnClampMode { get; set; } = UnClampModeEnum.RisingAndVacuum;
    }
}
