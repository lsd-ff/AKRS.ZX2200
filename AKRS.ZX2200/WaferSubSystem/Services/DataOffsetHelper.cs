using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Services
{
    /// <summary>
    /// 数据补偿帮助类
    /// </summary>
    public static class DataOffsetHelper
    {
        /// <summary>
        /// Offset
        /// </summary>
        /// <param name="pointF">pointF</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>result</returns>
        public static PointF Offset(this PointF pointF, float x, float y)
        {
            return new PointF(pointF.X + x, pointF.Y + y);
        }

        /// <summary>
        /// Offset
        /// </summary>
        /// <param name="pointF">pointF</param>
        /// <param name="offset">offset</param>
        /// <returns>result</returns>
        public static PointF Offset(this PointF pointF, PointF offset)
        {
            return new PointF(pointF.X + offset.X, pointF.Y + offset.Y);
        }

        /// <summary>
        /// Offset
        /// </summary>
        /// <param name="sizeF">sizeF</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>result</returns>
        public static SizeF Offset(this SizeF sizeF, float x, float y)
        {
            return new SizeF(sizeF.Width + x, sizeF.Height + y);
        }
    }
}
