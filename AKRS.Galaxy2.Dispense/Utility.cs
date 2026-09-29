using System;
using System.Collections.Generic;
using System.Drawing;
using AKRS.Galaxy2.Dispense;

namespace AKRS.Galaxy2.Dispense
{
	class Utility
	{
        public static void FillVector(Graphics graphics, Vector2D vector, Pen pen, Brush brush)
        {
            graphics.DrawLine(pen, (PointF)vector.InitialPoint, (PointF)vector.TerminalPoint);
            FillArrow(graphics, vector.InitialPoint, vector.TerminalPoint, brush);
        }

		public static void FillVector(Graphics graphics, Point2D initialPoint, Point2D terminalPoint, Pen pen, Brush brush)
		{
			graphics.DrawLine(pen, (PointF)initialPoint, (PointF)terminalPoint);
            FillArrow(graphics, initialPoint, terminalPoint, brush);
		}

		public static void FillArrow(Graphics graphics, Point2D initialPoint, Point2D terminalPoint, Brush brush)
		{
			Point2D ptM, ptN, ptA;
			if (initialPoint != terminalPoint)
			{
                CalculateArrowPoints(initialPoint, terminalPoint, out ptM, out ptN, out ptA);

				PointF[] points = new PointF[] { (PointF)terminalPoint, (PointF)ptM, (PointF)ptN, (PointF)terminalPoint };
				graphics.FillPolygon(brush, points);
			}
		}

		public static void DrawVector(Graphics graphics, Point2D initialPoint, Point2D terminalPoint, Pen pen)
		{
			graphics.DrawLine(pen, (PointF)initialPoint, (PointF)terminalPoint);
            DrawArrow(graphics, initialPoint, terminalPoint, pen);
		}

		public static void DrawArrow(Graphics g, Point2D srcPoint, Point2D dstPoint, Pen pen)
		{			
			if (srcPoint != dstPoint)
			{
				Point2D ptM, ptN, ptA;
                CalculateArrowPoints(srcPoint, dstPoint, out ptM, out ptN, out ptA);
				g.DrawLines(pen, new PointF[] { (PointF)ptM, (PointF)dstPoint, (PointF)ptN});
			}
		}

		static void CalculateArrowPoints(Point2D srcPt, Point2D dstPt, out Point2D ptM, out Point2D ptN, out Point2D ptA)
		{
			if (srcPt == dstPt)
				throw new ArgumentException("srcPt == dstpt");

			double tan = (double)(dstPt.Y - srcPt.Y) / (double)(dstPt.X - srcPt.X);
			double sita = Math.Atan(tan);

			double sin = Math.Sin(sita);
			double cos = Math.Cos(sita);

			ptA = new Point2D();
			int L = 10;//箭头的长度
			int H = 10;//箭头的宽度
			if (dstPt.X >= srcPt.X)
			{
				ptA.X = (int)(dstPt.X - L * cos + 0.5);
				ptA.Y = (int)(dstPt.Y - L * sin + 0.5);
			}
			else
			{
				ptA.X = (int)(dstPt.X + L * cos + 0.5);
				ptA.Y = (int)(dstPt.Y + L * sin + 0.5);
			}


			ptM = new Point2D();
			ptM.X = (int)(ptA.X - H / 2 * sin + 0.5);
			ptM.Y = (int)(ptA.Y + H / 2 * cos + 0.5);

			ptN = new Point2D();
			ptN.X = (int)(ptA.X + H / 2 * sin + 0.5);
			ptN.Y = (int)(ptA.Y - H / 2 * cos + 0.5);
		}


		//----------------------------------------------------------------------------

        /// <summary>
        /// 计算线段的长度。
        /// </summary>
        /// <param name="segmentInitialPoint"></param>
        /// <param name="segmentTerminalPoint"></param>
        /// <returns></returns>
        public static double CalculateLineSegmentLength(Point2D segmentInitialPoint, Point2D segmentTerminalPoint)
        {
            return GetPointsDistance(segmentInitialPoint, segmentTerminalPoint);
        }

		/// <summary>
		/// 求两点间距离
		/// </summary>
		/// <param name="pt1">点1坐标</param>
		/// <param name="pt2">点2坐标</param>
		/// <returns></returns>
		public static double GetPointsDistance(PointF pt1, PointF pt2)
		{
			return GetPointsDistance(pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		/// <summary>
		/// 求两点间距离
		/// </summary>
		/// <param name="point1"></param>
		/// <param name="point2"></param>
		/// <returns></returns>
		public static double GetPointsDistance(Point2D point1, Point2D point2)
        {
			return GetPointsDistance(point1.X, point1.Y, point2.X, point2.Y);
        }

		/// <summary>
		/// 求两点间距离
		/// </summary>
		/// <param name="x1"></param>
		/// <param name="y1"></param>
		/// <param name="x2"></param>
		/// <param name="y2"></param>
		/// <returns></returns>
		public static double GetPointsDistance(float x1, float y1, float x2, float y2)
		{
			double a = Math.Pow(x1 - x2, 2);
			double b = Math.Pow(y1 - y2, 2);
			return Math.Sqrt(a + b);
		}


		/// <summary>
		/// 获取点与线段间的最短距离。
		/// </summary>
		/// <param name="segmentPoint1"></param>
		/// <param name="segmentPoint2"></param>
		/// <param name="point"></param>
		/// <returns></returns>
		public static double GetMinimumDistanceBetweenPointAndSegment(Point2D segmentPoint1, Point2D segmentPoint2, Point2D point)
		{
			Point2D A = segmentPoint1;
			Point2D B = segmentPoint2;
			Point2D C = point;

			double BC = GetPointsDistance(B, C);
			double AC = GetPointsDistance(A, C);
			double AB = GetPointsDistance(A, B);

			double squaredBC = Math.Pow(BC, 2);
			double squaredAC = Math.Pow(AC, 2);
			double squaredAB = Math.Pow(AB, 2);

			if (squaredBC > squaredAC + squaredAB)
			{
				// 钝角三角形，∠A是钝角。
				return AC;
			}
			else if (squaredAC > squaredBC + squaredAB)
			{
				// 钝角三角形，∠B是钝角。
				return BC;
			}
			else
			{
				// A、B和C组成一个锐角三角形或直角三角形，或者A、B和C在同一直线上。

				// “海伦公式”：由三角形的3条边长计算三角形的面积。
				// 此公式同样支持A、B和C共线的情况。
				double p = (BC + AC + AB) / 2;
				double triangleArea = Math.Sqrt(p * (p - BC) * (p - AC) * (p - AB));
				double triangleHeight = 2 * triangleArea / AB;

				return triangleHeight;
			}
		}

        /// <summary>
        /// 计算点<paramref name="point"/>在经过点<paramref name="linePointA"/>和点<paramref name="linePointB"/>的直线上的投影。
        /// </summary>
		/// <param name="linePointA">直线上的一点。</param>
		/// <param name="linePointB">直线上的另一点。</param>
		/// <param name="point">点</param>
		/// <returns>点<paramref name="point"/>在直线上的投影。</returns>
        public static Point2D CalculatePointProjectionOnLine(Point2D linePointA, Point2D linePointB, Point2D point)
        {
			if (linePointA == linePointB)
				throw new ArgumentException(nameof(linePointB));

            // 直线方程：Ax + By + C = 0
            // 点: (x0, y0)
            // 法线方程：Bx - Ay + (-Bx0 + Ay0) = 0
            // 投影点：( (B * (-B * x0 + A * y0) + A * C) / (-A^2 - B^2), (B * C - A * (-B * x0 + A * y0)) / (-A^2 - B^2) )

            // 经过点A(xA, yB)和点B(xB, yB)的直线的一般方程：
            //   (yB - yA)x + (xA - xB)y + (xB*yA - xA*yB) = 0
            //  A = yB - yA
            //  B = xA - xB
            //  C = xB * yA - xA * yB
            //  
            // 投影点：
            //  x' = (x0 * B^2 - y0 * A * B - A * C) / (A^2 + B^2)
            //  y' = (y0 * A^2 - x0 * A * B - B * C) / (A^2 + B^2)

            Point2D a = linePointA;
            Point2D b = linePointB;
            Point2D c = point;

            double A = b.Y - a.Y;
            double B = a.X - b.X;
            double C = b.X * a.Y - a.X * b.Y;

            double AB = A * B;
            double AA = A * A;
            double BB = B * B;
            double AA_BB = AA + BB;

            double xProj = (c.X * BB - c.Y * AB - A * C) / AA_BB;
            double yProj = (c.Y * AA - c.X * AB - B * C) / AA_BB;

            return new Point2D((float)xProj, (float)yProj);
        }

        /// <summary>
        /// 计算向量的倾斜角。
        /// </summary>
        /// <param name="initialPoint">向量的起点。</param>
        /// <param name="terminalPoint">向量的终点。</param>
        /// <returns>倾斜角，单位是度。</returns>
        public static double CalculateVectorInclication(Point2D initialPoint, Point2D terminalPoint)
        {
            //float I_i = 1;
            //float I_j = 0;

            float V_i = terminalPoint.X - initialPoint.X;
            float V_j = terminalPoint.Y - initialPoint.Y;
			double dotProduct = V_i;	// dotProduct = V_i * I_i + V_j * I_j  ==>  dotProduct = V_i

            double V_magnitude = Math.Sqrt(V_i * V_i + V_j * V_j);
			//double I_magnitude = 1;

            double cos = dotProduct / V_magnitude;	// cos = dotProduct / V_magnitude / I_magnitude;  ==> cos = dotProduct / V_magnitude
            double radius = Math.Acos(cos);
            double degree = radius * 180 / Math.PI;

            return degree;            
        }

        public static PointAndRectangleRelation CheckPointOnRectangleEdge(Point2D rectMinPoint, Point2D rectMaxPoint, Point2D logicalPoint, float xDelta, float yDelta)
        {
            return CheckPointOnRectangleEdge(rectMinPoint.X, rectMinPoint.Y, rectMaxPoint.X, rectMaxPoint.Y, logicalPoint, xDelta, yDelta);
        }

        public static PointAndRectangleRelation CheckPointOnRectangleEdge(float rectMinX, float rectMinY, float rectMaxX, float rectMaxY, Point2D logicalPoint, float xDelta, float yDelta)
        {
            if (xDelta < 0)
                xDelta = 0;

            if (yDelta < 0)
                yDelta = 0;

            PointAndRectangleRelation relation = PointAndRectangleRelation.Unkown;

            if (rectMinX <= logicalPoint.X && logicalPoint.X <= rectMaxX)
            {
                if (rectMinY - yDelta <= logicalPoint.Y && logicalPoint.Y <= rectMinY + yDelta)
                {
                    // 上边缘
                    relation = PointAndRectangleRelation.TopEdge;
                }
                else if (rectMaxY - yDelta <= logicalPoint.Y && logicalPoint.Y <= rectMaxY + yDelta)
                {
                    // 下边缘
                    relation = PointAndRectangleRelation.BottomEdge;
                }
                else
                {
                    // 其他情况。
                }
            }
            else
            {
                if (rectMinY <= logicalPoint.Y && logicalPoint.Y <= rectMaxY)
                {
                    if (rectMinX - xDelta <= logicalPoint.X && logicalPoint.X <= rectMinX + xDelta)
                    {
                        // 左边缘
                        relation |= PointAndRectangleRelation.LeftEdge;
                    }
                    else if (rectMaxX - xDelta <= logicalPoint.X && logicalPoint.X <= rectMaxX + xDelta)
                    {
                        // 右边缘
                        relation |= PointAndRectangleRelation.RightEdge;
                    }
                    else
                    {
                        // 其他情况。                    
                    }
                }
            }

            return relation;
        }

	}
}
