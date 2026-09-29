using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.ZX2200.TransportUnitSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 矩阵或者存在多个的配置文件
    /// </summary>
    public class ArrayConfig : BaseConfig
    {
        /// <summary>
        /// 是否是多个的
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// 如果是多个的情况的下的类型
        /// </summary>
        public MultiplicationEnum Multiplication { get; set; }

        /// <summary>
        /// 基板的数量
        /// </summary>
        public int Count { get; set; } = 1;

        /// <summary>
        /// 基板的位置集合
        /// </summary>
        public List<ElementCoordinate> ElementCoordinates { get; set; } = new List<ElementCoordinate>();

        /// <summary>
        /// 行间距
        /// </summary>
        [TreeProgramListArgs("Row Spacing", (string)null, UnitHelper.mm)]
        public double RowSpacing { get; set; }

        /// <summary>
        /// 列间距
        /// </summary>
        [TreeProgramListArgs("Column Spacing", (string)null, UnitHelper.mm)]
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [TreeProgramListArgs("Row Count", (string)null)]
        public int RowCount { get; set; } = 1;

        /// <summary>
        /// 列数
        /// </summary>
        [TreeProgramListArgs("Column Count", (string)null)]
        public int ColumnCount { get; set; } = 1;

        /// <summary>
        /// 手动输入间距
        /// </summary>
        public bool IsAssistanceDistanceByInput { get; set; } = false;
    }
}
