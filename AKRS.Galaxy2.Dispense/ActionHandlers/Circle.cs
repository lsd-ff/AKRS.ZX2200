using System.Drawing;

namespace AKRS.Galaxy2.Dispense
{
    class Circle
    {
        private PointF[] _polygonPoints;

        private float _circleRadius;
        private Point2D _circleCenter;

        private RectangleF _circumscribedRectangle;

        public Circle()
        {
            _polygonPoints = new PointF[4];
        }

        /// <summary>
        /// 通过一个矩形来设置圆，圆作为矩形的外接圆。
        /// </summary>
        /// <param name="rectMinX"></param>
        /// <param name="rectMinY"></param>
        /// <param name="rectMaxX"></param>
        /// <param name="rectMaxY"></param>
        public void Set(float rectMinX, float rectMinY, float rectMaxX, float rectMaxY)
        {
            _polygonPoints[0] = new PointF(rectMinX, rectMinY);
            _polygonPoints[1] = new PointF(rectMaxX, rectMinY);
            _polygonPoints[2] = new PointF(rectMaxX, rectMaxY);
            _polygonPoints[3] = new PointF(rectMinX, rectMaxY);


            double circleDiameter = Utility.CalculateLineSegmentLength(new Point2D(rectMinX, rectMinY), new Point2D(rectMaxX, rectMaxY));
            _circleRadius = (float)(circleDiameter / 2);

            float circleCenterX = (rectMaxX + rectMinX) / 2;
            float circleCenterY = (rectMaxY + rectMinY) / 2;
            _circleCenter = new Point2D(circleCenterX, circleCenterY);

            float rectLeft = circleCenterX - _circleRadius;
            float rectTop = circleCenterY - _circleRadius;
            //float rectLeft = rectMinX;
            //float rectTop = rectMinY;
            float rectWidth = (float)circleDiameter;
            float rectHeight = rectWidth;
            _circumscribedRectangle = new RectangleF(rectLeft, rectTop, rectWidth, rectHeight);
        }

        /// <summary>
        /// 获取 原始的矩形的4个点的数组。
        /// </summary>
        public PointF[] OriginalRectanglePoints
        {
            get { return _polygonPoints; }
        }

        /// <summary>
        /// 获取 半径。
        /// </summary>
        public float Radius
        {
            get { return _circleRadius; }
        }

        /// <summary>
        /// 获取 圆心。
        /// </summary>
        public Point2D Center
        {
            get { return _circleCenter; }
        }

        /// <summary>
        /// 获取 圆的外切矩形。
        /// </summary>
        public RectangleF CircumscribedRectangle
        {
            get { return _circumscribedRectangle; }
        }

        /// <summary>
        /// 获取 限制在圆内部的水平坐标轴和垂直坐标轴的起点和终点。
        /// </summary>
        /// <param name="horizontalVectorInitialPoint"></param>
        /// <param name="horizontalVectorTerminalPoint"></param>
        /// <param name="verticalVectorInitialPoint"></param>
        /// <param name="verticalVectorTerminalPoint"></param>
        public void GetCoordinateAxies(
            out Point2D horizontalVectorInitialPoint,
            out Point2D horizontalVectorTerminalPoint,
            out Point2D verticalVectorInitialPoint,
            out Point2D verticalVectorTerminalPoint
            )
        {
            horizontalVectorInitialPoint = _circleCenter.Translate(-1 * _circleRadius, 0);
            horizontalVectorTerminalPoint = _circleCenter.Translate(_circleRadius, 0);

            verticalVectorInitialPoint = _circleCenter.Translate(0, _circleRadius);
            verticalVectorTerminalPoint = _circleCenter.Translate(0, -1 * _circleRadius);
        }
    }
}
