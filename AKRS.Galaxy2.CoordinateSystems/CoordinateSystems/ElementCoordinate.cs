namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    using System;
    using System.Drawing.Drawing2D;
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    /// <summary>
    /// 构建坐标系的最小元素
    /// </summary>
    [Serializable]
    public class ElementCoordinate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="degree">角度</param>
        /// <param name="point">点位</param>
        public ElementCoordinate(double degree = 0, AKRSPoint3D point = null)
        {
            if (point == null)
            {
                point = new AKRSPoint3D();
            }

            this.Degree = degree;
            this.Point = point;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ElementCoordinate()
        {
        }

        /// <summary>
        /// 角度
        /// </summary>
        public double Degree { get; set; }

        /// <summary>
        /// 位置
        /// </summary>
        public AKRSPoint3D Point { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 两元素相加
        /// </summary>
        /// <param name="elementCoordinate1">元素1</param>
        /// <param name="elementCoordinate2">元素2</param>
        /// <returns>结果</returns>
        public static ElementCoordinate operator +(
            ElementCoordinate elementCoordinate1,
            ElementCoordinate elementCoordinate2)
        {
            return new ElementCoordinate(
                elementCoordinate1.Degree + elementCoordinate2.Degree,
                elementCoordinate1.Point + elementCoordinate2.Point);
        }
    }
}
