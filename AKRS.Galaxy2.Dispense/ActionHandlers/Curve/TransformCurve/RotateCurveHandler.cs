using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class RotateCurveHandler : AffineTransformCurveHandler
    {
        const string HINT_TEXT = "按下鼠标左键，上下拖动鼠标以旋转当前曲线。\r\n同时按下Shift键使得旋转更精细。";

        Point2D _mouseDownLogicalPoint;
        Point2D _previousMouseLogicalPoint;
        Point2D _previousMousePixelPoint;
        bool _bHolding;

        Vector2D _xLogicalVector;
        Vector2D _yLogicalVector;
        Vector2D _xPixelVector;
        Vector2D _yPixelVector;

        Point2D[] _polygonLogicalPoints;
        PointF[] _polygonPixelPoints;

        Pen _vectorPen;
        Brush _vectorBrush;

        Pen _arcPen;
        Brush _arcBrush;

        //float _arcSweepAngle;
        float _accumulatedAngle;

        Point2D[] _additionalLogicalPoints = new Point2D[16];


        public RotateCurveHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _vectorPen = new Pen(Color.Blue);
            _vectorBrush = new SolidBrush(Color.Blue);

            _arcPen = new Pen(Color.Blue, 3);
            _arcBrush = new SolidBrush(Color.Blue);

            _previousMouseLogicalPoint = new Point2D();
            _bHolding = false;

            _polygonLogicalPoints = new Point2D[4];
            _polygonPixelPoints = new PointF[4];

            //_arcSweepAngle = 0;
            _accumulatedAngle = 0;

            RectangleVisible = true;
            CircleVisible = true;
        }

        public override void SetMode()
        {
            base.SetMode();

            _previousMouseLogicalPoint = new Point2D();
            //_arcSweepAngle = 0;
            _accumulatedAngle = 0;            

            InitVectors();
            InitPolygonPoints();

            _bHolding = false;
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if (MinLogicalPoint.X <= mouseLogicalPoint.X &&
                MinLogicalPoint.Y <= mouseLogicalPoint.Y &&
                mouseLogicalPoint.X <= MaxLogicalPoint.X &&
                mouseLogicalPoint.Y <= MaxLogicalPoint.Y
                )
            {
                EpoxyPatternControl.Cursor = Cursors.NoMoveVert;

                _mouseDownLogicalPoint = mouseLogicalPoint;
                _previousMouseLogicalPoint = mouseLogicalPoint;
                _previousMousePixelPoint = mousePixelPoint;

                //InitVectors();
                //InitPolygonPoints();

                //_arcSweepAngle = 0;


                _bHolding = true;

                EpoxyPatternControl.Invalidate();
            }
            else
            {
                EpoxyPatternControl.Cursor = Cursors.Default;
                _bHolding = false;
            }
        }


        private void InitVectors()
        {
            double circleDiameter = Utility.CalculateLineSegmentLength(MinLogicalPoint, MaxLogicalPoint);
            float circleRadius = (float)(circleDiameter / 2);
            float circleCenterX = MinLogicalPoint.X + (MaxLogicalPoint.X - MinLogicalPoint.X) / 2;
            float circleCenterY = MinLogicalPoint.Y + (MaxLogicalPoint.Y - MinLogicalPoint.Y) / 2;
            Point2D rotationCenter = new Point2D(circleCenterX, circleCenterY);

            Point2D initialPoint, terminalPoint;

            //initialPoint = new Point2D(circleCenterX - circleRadius - EpoxyPatternControl.EndpointRectWidth, circleCenterY);
            //terminalPoint = new Point2D(circleCenterX + circleRadius + EpoxyPatternControl.EndpointRectWidth, circleCenterY);
            initialPoint = rotationCenter.Translate(-1 * circleRadius - EpoxyPatternControl.EndpointRectWidth, 0);
            terminalPoint = rotationCenter.Translate(circleRadius + EpoxyPatternControl.EndpointRectWidth, 0);
            _xLogicalVector = new Vector2D(initialPoint, terminalPoint);
            
            //initialPoint = new Point2D(circleCenterX, circleCenterY - circleRadius - EpoxyPatternControl.EndpointRectHeight);
            //terminalPoint = new Point2D(circleCenterX, circleCenterY + circleRadius + EpoxyPatternControl.EndpointRectHeight);
            initialPoint = rotationCenter.Translate(0, -1 * circleRadius - EpoxyPatternControl.EndpointRectHeight);
            terminalPoint = rotationCenter.Translate(0, circleRadius + EpoxyPatternControl.EndpointRectHeight);
            _yLogicalVector = new Vector2D(initialPoint, terminalPoint);

            UpdatePixelVectors();
        }

        private void UpdatePixelVectors()
        {
            _xPixelVector = new Vector2D(
                initialPoint: EpoxyPatternControl.CreateRealPoint(_xLogicalVector.InitialPoint),
                terminalPoint: EpoxyPatternControl.CreateRealPoint(_xLogicalVector.TerminalPoint));

            _yPixelVector = new Vector2D(
                initialPoint: EpoxyPatternControl.CreateRealPoint(_yLogicalVector.InitialPoint),
                terminalPoint: EpoxyPatternControl.CreateRealPoint(_yLogicalVector.TerminalPoint));
        }

        private void InitPolygonPoints()
        {
            float xMin = MinLogicalPoint.X - EpoxyPatternControl.EndpointRectWidth;
            float yMin = MinLogicalPoint.Y - EpoxyPatternControl.EndpointRectHeight;
            float xMax = MaxLogicalPoint.X + EpoxyPatternControl.EndpointRectWidth;
            float yMax = MaxLogicalPoint.Y + EpoxyPatternControl.EndpointRectHeight;
            _polygonLogicalPoints[0] = new Point2D(xMin, yMin);
            _polygonLogicalPoints[1] = new Point2D(xMax, yMin);
            _polygonLogicalPoints[2] = new Point2D(xMax, yMax);
            _polygonLogicalPoints[3] = new Point2D(xMin, yMax);

            UpdatePolygonPixelPoints();
        }

        private void UpdatePolygonPixelPoints()
        {
            for (int i = 0; i < _polygonLogicalPoints.Length; i++)
            {
                _polygonPixelPoints[i] = (PointF)EpoxyPatternControl.CreateRealPoint(_polygonLogicalPoints[i]);
            }
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if (_bHolding)
            {
                HandleMouseMoveEvent_Holding(mousePixelPoint, mouseLogicalPoint);
            }
            else
            {
                if (MinLogicalPoint.X <= mouseLogicalPoint.X &&
                    MinLogicalPoint.Y <= mouseLogicalPoint.Y &&
                    mouseLogicalPoint.X <= MaxLogicalPoint.X &&
                    mouseLogicalPoint.Y <= MaxLogicalPoint.Y
                    )
                {
                    EpoxyPatternControl.Cursor = Cursors.Hand;
                }
                else
                {
                    EpoxyPatternControl.Cursor = Cursors.Default;
                }
            }
        }


        private void HandleMouseMoveEvent_Holding(Point2D mousePixelPoint, Point2D mouseLogicalPoint)
        {
            Debug.Assert(_bHolding);

            float xIncrement = mouseLogicalPoint.X - _previousMouseLogicalPoint.X;
            float yIncrement = mouseLogicalPoint.Y - _previousMouseLogicalPoint.Y;

            double circleDiameter = Utility.CalculateLineSegmentLength(MinLogicalPoint, MaxLogicalPoint);
            float circleRadius = (float)(circleDiameter / 2);
            float circleCenterX = MinLogicalPoint.X + (MaxLogicalPoint.X - MinLogicalPoint.X) / 2;
            float circleCenterY = MinLogicalPoint.Y + (MaxLogicalPoint.Y - MinLogicalPoint.Y) / 2;
            Point2D rotationCenter = new Point2D(circleCenterX, circleCenterY);

            _additionalLogicalPoints[0] = _xLogicalVector.InitialPoint;
            _additionalLogicalPoints[1] = _xLogicalVector.TerminalPoint;

            _additionalLogicalPoints[2] = _yLogicalVector.InitialPoint;
            _additionalLogicalPoints[3] = _yLogicalVector.TerminalPoint;

            int iPointIndex = 4;
            foreach (Point2D point in _polygonLogicalPoints)
            {
                _additionalLogicalPoints[iPointIndex] = point;
                iPointIndex++;
            }

            float yDelta = mousePixelPoint.Y - _previousMousePixelPoint.Y;
            float degree = yDelta;
            if (mouseLogicalPoint.X > 0)
            {
                degree *= -1;
            }

            if (Control.ModifierKeys == Keys.Shift)
                degree /= 10;

            EpoxyPatternControl.RotateCurrentCurve(rotationCenter, degree, ref _additionalLogicalPoints);

            
            _xLogicalVector = new Vector2D(_additionalLogicalPoints[0], _additionalLogicalPoints[1]);
            _yLogicalVector = new Vector2D(_additionalLogicalPoints[2], _additionalLogicalPoints[3]);
            UpdatePixelVectors();

            iPointIndex = 4;
            for (int i = 0; i < _polygonLogicalPoints.Length; i++, iPointIndex++)
            {
                _polygonLogicalPoints[i] = _additionalLogicalPoints[iPointIndex];
            }

            UpdatePolygonPixelPoints();

            //_arcSweepAngle -= degree;
            _accumulatedAngle -= degree;

            _previousMouseLogicalPoint = mouseLogicalPoint;
            _previousMousePixelPoint = mousePixelPoint;

            EpoxyPatternControl.Invalidate();
        }

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {
                _bHolding = false;
                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
            base.HandleLogicalCoordinateSystemOriginChanged(eventArgs);

            UpdatePixelVectors();
            UpdatePolygonPixelPoints();
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        {
            base.HandleScaleChanged(args);

            UpdatePixelVectors();
            UpdatePolygonPixelPoints();
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            base.HandlePaintEvent(graphics);
            
            graphics.DrawPolygon(Pens.Black, _polygonPixelPoints);

                Utility.FillVector(graphics, _xPixelVector, _vectorPen, _vectorBrush);
                Utility.FillVector(graphics, _yPixelVector, _vectorPen, _vectorBrush);

            if (_accumulatedAngle != 0)
            {
                float arcStartAngle = -90;
                graphics.DrawArc(_arcPen, Circle.CircumscribedRectangle, arcStartAngle, _accumulatedAngle);

                if (false)
                {
                    string s = String.Format("{0}°", _accumulatedAngle);
                    SizeF size = graphics.MeasureString(s, EpoxyPatternControl.Font);

                    PointF point;
                    if (_yLogicalVector.TerminalPoint.X > 0)
                    {
                        if (_yLogicalVector.TerminalPoint.Y > 0)
                        {
                            point = (PointF)_yPixelVector.TerminalPoint.Translate(-0.5F * size.Width, -1 * size.Height);
                        }
                        else
                        {
                            point = (PointF)_yPixelVector.TerminalPoint.Translate(-0.5F * size.Width, 1 * size.Height);
                        }
                    }
                    else
                    {
                        if (_yLogicalVector.TerminalPoint.Y > 0)
                        {
                            point = (PointF)_yPixelVector.TerminalPoint.Translate(size.Width, -1 * size.Height);
                        }
                        else
                        {
                            point = (PointF)_yPixelVector.TerminalPoint.Translate(size.Width, 1 * size.Height);
                        }
                    }

                    RectangleF layoutRectangle = new RectangleF(point, size);
                    graphics.DrawString(s, EpoxyPatternControl.Font, _arcBrush, layoutRectangle);
                }
            }            

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }
    }
}
