using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    /// <summary>
    /// tu信息
    /// </summary>
    public class TransportUnitInfo : BaseInformation
    {
        /// <summary>
        /// 是否在板子上去进行工艺
        /// 有可能扫码失败则不会点胶和贴片
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// 是否需要预点胶
        /// </summary>
        public bool IsNeedPreDispense { get; set; } = true;

        /// <summary>
        /// 是否已经开始生产
        /// 如果开始生产，示教的时候就要弹出提示
        /// </summary>
        public bool IsProduct { get; set; } = false;

        /// <summary>
        /// 系统2是否发生偏移
        /// </summary>
        public bool IsSystem1Offset { get; set; } = false;

        /// <summary>
        /// 系统2是否发生偏移
        /// </summary>
        public bool IsSystem2Offset { get; set; } = false;

        /// <summary>
        /// 配置名称
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        /// 基板批次号
        /// </summary>
        public string SubstrateLotNumber { get; set; }

        /// <summary>
        /// 基板号
        /// </summary>
        public string SubstrateNumber { get; set; }

        /// <summary>
        /// 面别
        /// </summary>
        public string FaceType { get; set; } = "A";
    }
}
