using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    abstract partial class AffineTransformCurveHandler : ActionHandler
    {
        Point2D _minLogicalPoint;
        Point2D _maxLogicalPoint;

        Pen _pen;
        Pen _outerCirclePen;
        Pen _localCoordinateAxisPen;
        Brush _localCoordinateAxisBrush;

        /// <summary>
        /// 此对象的数据仅在Paint事件中更新和使用的。
        /// </summary>
        Circle _circle;

        bool _bRectangleVisible;
        bool _bCircleVisible;

        public AffineTransformCurveHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _pen = new Pen(Color.DarkGreen, 0);
            _pen.DashStyle = DashStyle.Dot;

            _outerCirclePen = new Pen(Color.DarkGreen, 0);
            _outerCirclePen.DashStyle = DashStyle.Solid;

            _localCoordinateAxisPen = new Pen(Color.Yellow, 0);
            _localCoordinateAxisPen.DashStyle = DashStyle.Solid;

            _localCoordinateAxisBrush = new SolidBrush(Color.Yellow);

            _circle = new Circle();
            _bCircleVisible = true;
            _bRectangleVisible = true;
        }

        protected Circle Circle
        {
            get { return _circle; }
        }

        protected bool CircleVisible
        {
            get { return _bCircleVisible; }
            set { _bCircleVisible = value; }
        }

        protected bool RectangleVisible
        {
            get { return _bRectangleVisible; }
            set { _bRectangleVisible = value; }
        }

        protected Point2D MinLogicalPoint
        {
            get { return _minLogicalPoint; }
        }

        protected Point2D MaxLogicalPoint
        {
            get { return _maxLogicalPoint; }
        }

        protected void SetMinAndMaxLogicalPoints(Point2D minLogicalPoint, Point2D maxLogicalPoint)
        {
            _minLogicalPoint = minLogicalPoint;
            _maxLogicalPoint = maxLogicalPoint;
        }

        public static void GetCurveBounds(Curve curve, out float xMin, out float yMin, out float xMax, out float yMax)
        {
            if (curve == null)
                throw new ArgumentNullException(nameof(curve));
            
            if (curve.EndpointCount == 0)
                throw new InvalidOperationException("the curve does not contain endpoints");

            xMin = float.MaxValue;
            xMax = float.MinValue;
            yMin = float.MaxValue;
            yMax = float.MinValue;
            foreach (EndpointUnit endpointUnit in curve.EndpointUnitList)
            {
                Point2D logicalPoint = endpointUnit.Endpoint.LogicalPoint;

                if (xMin > logicalPoint.X)
                    xMin = logicalPoint.X;

                if (xMax < logicalPoint.X)
                    xMax = logicalPoint.X;

                if (yMin > logicalPoint.Y)
                    yMin = logicalPoint.Y;

                if (yMax < logicalPoint.Y)
                    yMax = logicalPoint.Y;
            }

            Debug.Assert(xMin != float.NaN && xMax != float.NaN);
            Debug.Assert(yMin != float.NaN && yMax != float.NaN);
            Debug.Assert(xMin <= xMax && yMin <= yMax);
        }

        protected void RefreshCurveBounds(Curve curve)
        {
            if (curve == null)
                throw new ArgumentNullException(nameof(curve));           

            Debug.Assert(curve.EndpointCount > 0);

            float xMin, xMax;
            float yMin, yMax;
            GetCurveBounds(curve, out xMin, out yMin, out xMax, out yMax);
            _minLogicalPoint = new Point2D(xMin, yMin);
            _maxLogicalPoint = new Point2D(xMax, yMax);
        }

        public override void SetMode()
        {
            base.SetMode();

            Curve curve = EpoxyPatternControl.GetCurrentCurve();
            if (curve != null && curve.EndpointCount > 0)
            {
                RefreshCurveBounds(curve);
            }
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        { }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        { }

        public override void HandlePaintEvent(Graphics graphics)
        {
            Point2D minLogicalPoint = new Point2D(_minLogicalPoint.X - EpoxyPatternControl.EndpointRectWidth, _minLogicalPoint.Y - EpoxyPatternControl.EndpointRectHeight);
            Point2D maxLogicalPoint = new Point2D(_maxLogicalPoint.X + EpoxyPatternControl.EndpointRectWidth, _maxLogicalPoint.Y + EpoxyPatternControl.EndpointRectHeight);
            Point2D minPixelPoint = EpoxyPatternControl.CreateRealPoint(minLogicalPoint);
            Point2D maxPixelPoint = EpoxyPatternControl.CreateRealPoint(maxLogicalPoint);

            float xMin = minPixelPoint.X;
            float xMax = maxPixelPoint.X;
            float yMin = minPixelPoint.Y;
            float yMax = maxPixelPoint.Y;
            _circle.Set(xMin, yMin, xMax, yMax);

            Point2D logicalCircleCenter = EpoxyPatternControl.ConvertToLogicalPoint(_circle.Center);
            EpoxyPatternControl.PaintCoordinateLines(graphics, logicalCircleCenter, _circle.Center);


            if (_bRectangleVisible)
            {            
                graphics.DrawPolygon(_pen, _circle.OriginalRectanglePoints);
            }

            if (_bCircleVisible)
            {
                graphics.DrawEllipse(_outerCirclePen, _circle.CircumscribedRectangle);

                Point2D hAxisInitialPoint, hAxisTerminalPoint;
                Point2D vAxisInitialPoint, vAxisTerminalPoint;
                _circle.GetCoordinateAxies(out hAxisInitialPoint, out hAxisTerminalPoint, out vAxisInitialPoint, out vAxisTerminalPoint);
                Utility.FillVector(graphics, hAxisInitialPoint, hAxisTerminalPoint, _localCoordinateAxisPen, _localCoordinateAxisBrush);
                Utility.FillVector(graphics, vAxisInitialPoint, vAxisTerminalPoint, _localCoordinateAxisPen, _localCoordinateAxisBrush);
            }
        }


    }
}
