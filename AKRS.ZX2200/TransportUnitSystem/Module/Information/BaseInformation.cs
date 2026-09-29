using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Information
{
    /// <summary>
    /// 基础信息类
    /// </summary>
    public abstract class BaseInformation
    {
        /// <summary>
        /// 定位参数
        /// </summary>
        public LocateResultInfo LocateResultInfo { get; set; } = new LocateResultInfo();

        /// <summary>
        /// 测高位置结果的集合
        /// </summary>
        public double MeasureHeightResult { get; set; } = 0;

        /// <summary>
        /// 是否定位过
        /// </summary>
        public bool IsVisioned { get; set; } = false;

        /// <summary>
        /// 是否身份识别过
        /// </summary>
        public bool IsIdentity { get; set; } = false;

        /// <summary>
        /// ID号
        /// </summary>
        public string IdentityCode { get; set; }

        /// <summary>
        /// 是否测高过
        /// </summary>
        public bool IsMeasureHeight { get; set; } = false;
    }
}
