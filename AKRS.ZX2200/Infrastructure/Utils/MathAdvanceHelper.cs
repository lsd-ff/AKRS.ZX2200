using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Utils
{
    using AKRS.Base;
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    /// <summary>
    /// 高等数学
    /// </summary>
    public static class MathAdvanceHelper
    {
        /// <summary>
        /// 平面差值法
        /// </summary>
        /// <param name="x">输入点X</param>
        /// <param name="y">输入点Y</param>
        /// <param name="planePoint2Ds">片面拟合</param>
        /// <param name="startPos">开始点</param>
        /// <param name="distanceX">间距X</param>
        /// <param name="distanceY">间距Y</param>
        /// <returns>结果</returns>
        public static AKRSPoint2D PlaneInterpolate(double x, double y, AKRSPoint2D[,] planePoint2Ds, AKRSPoint3D startPos, double distanceX, double distanceY)
        {
            // 判断当前点是否在补偿区域
            if (x < startPos.X || x > startPos.X + planePoint2Ds.GetLength(1) * distanceX
                               || y < startPos.Y || y > startPos.Y + planePoint2Ds.GetLength(0) * distanceY)
            {
                return new AKRSPoint2D(x, y);
            }

            int indexX = (int)((x - startPos.X) / distanceX);
            int indexY = (int)((y - startPos.Y) / distanceY);

            AKRSPoint2D q1 = planePoint2Ds[indexY, indexX];
            //AKRSPoint2D q2 = planePoint2Ds[indexY + 1, indexX];
            //AKRSPoint2D q3 = planePoint2Ds[indexY, indexX + 1];
            //AKRSPoint2D q4 = planePoint2Ds[indexY + 1, indexX + 1];

            //double offsetX = ((q1 + q2).X - (q3 + q4).X) / distanceX * (x - startPos.X - indexX * distanceX);
            //double offsetY = ((q1 + q2).Y - (q3 + q4).Y) / distanceY * (y - startPos.Y - indexY * distanceY);

            return new AKRSPoint2D(q1.X, q1.Y);
        }
    }
}
