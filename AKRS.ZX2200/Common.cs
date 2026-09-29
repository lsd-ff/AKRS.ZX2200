using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200
{
    /// <summary>
    /// 坐标
    /// </summary>
    public class Point
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// Y坐标
        /// </summary>
        public float Y { get; set; }
    }

    /// <summary>
    /// 芯片类型和芯片厚度
    /// </summary>
    public class ChipTypeAndThickness
    {
        /// <summary>
        /// 芯片类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 芯片厚度
        /// </summary>
        public float Thickness { get; set; }
    }
}
