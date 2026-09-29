using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;

    /// <summary>
    /// 单个配置的父类
    /// 例如TU，焊点
    /// </summary>
    public abstract class SingleConfig : BaseConfig
    {
        /// <summary>
        /// 坐标系构建元素
        /// </summary>
        public ElementCoordinate ElementCoordinate { get; set; } = new ElementCoordinate();
    }
}
