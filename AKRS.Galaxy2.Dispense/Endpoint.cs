using System;
using System.Drawing;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 端点，即线段两端的点。
    /// </summary>
    
	public struct Endpoint
	{
        /// <summary>
        /// 逻辑坐标。
        /// </summary>
        public Point2D LogicalPoint { get; set; }

        /// <summary>
        /// 实际坐标。
        /// </summary>
		public Point2D RealPoint;

        /// <summary>
        /// realRectangle
        /// </summary>
		public RectangleF RealRectangle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logicalPoint">逻辑坐标</param>
		public Endpoint(Point2D logicalPoint)
        {
            this.LogicalPoint = logicalPoint;
			this.RealPoint = new Point2D();
			this.RealRectangle = new RectangleF();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logicalPoint">逻辑坐标</param>
        /// <param name="realPoint">实际坐标</param>
        /// <param name="realRectangle">realRectangle</param>
        public Endpoint(Point2D logicalPoint, Point2D realPoint, RectangleF realRectangle)
        {
            this.LogicalPoint = logicalPoint;
			this.RealPoint = realPoint;
			this.RealRectangle = realRectangle;
		}
    }
}
