using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    partial class ScaleCurveHandler : AffineTransformCurveHandler
    {
        const string HINT_TEXT = "拖动矩形的边框以缩放当前曲线。\r\n同时按下Shift键使得水平方向和垂直方向一起缩放。";

        bool _bHolding;

        Point2D _previousMouseLogicalPoint;

        PointAndRectangleRelation _relation;

        
        Point2D _outerRectangleMinPoint;
        Point2D _outerRectangleMaxPoint;

        Point2D[] _polygonLogicalPoints;
        PointF[] _polygonPixelPoints;

        bool _bMinifiable;

        Point2D[] _additionalLogicalPoints;

        Point2D _curveCenterLogicalPoint;

        float _xDelta;
        float _yDelta;

        public ScaleCurveHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _polygonLogicalPoints = new Point2D[4];
            _polygonPixelPoints = new PointF[4];

            _additionalLogicalPoints = new Point2D[16];

            _xDelta = epoxyPatternControl.EndpointRectWidth / 2;
            _yDelta = epoxyPatternControl.EndpointRectHeight / 2;

            RectangleVisible = true;
            CircleVisible = true;
        }

        private void RefreshOuterRectangle()
        {
            float xMin = MinLogicalPoint.X - EpoxyPatternControl.EndpointRectWidth;
            float yMin = MinLogicalPoint.Y - EpoxyPatternControl.EndpointRectHeight;
            _outerRectangleMinPoint = new Point2D(xMin, yMin);

            float xMax = MaxLogicalPoint.X + EpoxyPatternControl.EndpointRectWidth;
            float yMax = MaxLogicalPoint.Y + EpoxyPatternControl.EndpointRectHeight;
            _outerRectangleMaxPoint = new Point2D(xMax, yMax);
        }

        public override void SetMode()
        {
            base.SetMode();

            RefreshOuterRectangle();

            _bMinifiable = true;
            _bHolding = false;
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
            Debug.Assert(_bHolding == false);

            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
            _relation = Utility.CheckPointOnRectangleEdge(_outerRectangleMinPoint, _outerRectangleMaxPoint, mouseLogicalPoint, _xDelta, _yDelta);

            switch (_relation)
            {
            case PointAndRectangleRelation.LeftEdge:
            case PointAndRectangleRelation.RightEdge:
                EpoxyPatternControl.Cursor = Cursors.SizeWE;
                break;

            case PointAndRectangleRelation.TopEdge:
            case PointAndRectangleRelation.BottomEdge:
                EpoxyPatternControl.Cursor = Cursors.SizeNS;
                break;

            //case PointAndRectangleRelation.TopLeftCorner:
            //case PointAndRectangleRelation.BottomRightCorner:
            //    EpoxyPatternControl.Cursor = Cursors.SizeNESW;
            //    break;

            //case PointAndRectangleRelation.TopRightCorner:
            //case PointAndRectangleRelation.BottomLeftCorner:
            //    EpoxyPatternControl.Cursor = Cursors.SizeNWSE;
            //    break;

            case PointAndRectangleRelation.Unkown:
            default:
                EpoxyPatternControl.Cursor = Cursors.Default;
                break;
            }

            if (_relation != PointAndRectangleRelation.Unkown)
            {
                _previousMouseLogicalPoint = mouseLogicalPoint;

                float xMin = MinLogicalPoint.X - EpoxyPatternControl.EndpointRectWidth;
                float yMin = MinLogicalPoint.Y - EpoxyPatternControl.EndpointRectHeight;
                float xMax = MaxLogicalPoint.X + EpoxyPatternControl.EndpointRectWidth;
                float yMax = MaxLogicalPoint.Y + EpoxyPatternControl.EndpointRectHeight;
                _polygonLogicalPoints[0] = new Point2D(xMin, yMin);
                _polygonLogicalPoints[1] = new Point2D(xMax, yMin);
                _polygonLogicalPoints[2] = new Point2D(xMax, yMax);
                _polygonLogicalPoints[3] = new Point2D(xMin, yMax);

                for (int i = 0; i < _polygonLogicalPoints.Length; i++)
                {
                    _polygonPixelPoints[i] = (PointF)EpoxyPatternControl.CreateRealPoint(_polygonLogicalPoints[i]);
                }

                _curveCenterLogicalPoint = new Point2D((xMin + xMax) / 2, (yMin + yMax) / 2);

                _bMinifiable = true;
                _bHolding = true;
            }
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if (_bHolding)
            {
                Debug.Assert(_relation != PointAndRectangleRelation.Unkown);

                Curve curve = EpoxyPatternControl.GetCurrentCurve();
                if (curve != null && curve.EndpointCount > 0)
                {
                    float xScale = 1;
                    float yScale = 1;

                    if ((_relation & PointAndRectangleRelation.VerticalEdges) != 0)
                    {
                        xScale = Math.Abs(mouseLogicalPoint.X / _previousMouseLogicalPoint.X);
                    }
                    else
                    {
                        if ((_relation & PointAndRectangleRelation.HorizontalEdges) != 0)
                        {
                            yScale = Math.Abs(mouseLogicalPoint.Y / _previousMouseLogicalPoint.Y);
                        }
                    }


                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        if (xScale == 1 && yScale != 1)
                        {
                            xScale = yScale;
                        }
                        else if (xScale != 1 && yScale == 1)
                        {
                            yScale = xScale;
                        }
                        else
                        { }
                    }


                    _previousMouseLogicalPoint = mouseLogicalPoint;

                    bool bScalable = true;
                    if (!_bMinifiable)
                    {
                        if (xScale < 1 || yScale < 1)
                            bScalable = false;
                    }

                    if (bScalable)
                    {
                        try
                        {
                            int iPointIndex = 0;
                            foreach (Point2D point in _polygonLogicalPoints)
                            {
                                _additionalLogicalPoints[iPointIndex] = point;
                                iPointIndex++;
                            }

                            EpoxyPatternControl.ScaleCurrentCurve(_curveCenterLogicalPoint, xScale, yScale, ref _additionalLogicalPoints);

                            iPointIndex = 0;
                            for (int i = 0; i < _polygonLogicalPoints.Length; i++, iPointIndex++)
                            {
                                _polygonLogicalPoints[i] = _additionalLogicalPoints[iPointIndex];
                                _polygonPixelPoints[i] = (PointF)EpoxyPatternControl.CreateRealPoint(_polygonLogicalPoints[i]);
                            }

                            float polygonWidth = Math.Abs(_polygonLogicalPoints[0].X - _polygonLogicalPoints[1].X);
                            float polygonHeight = Math.Abs(_polygonLogicalPoints[0].Y - _polygonLogicalPoints[2].Y);
                            if (polygonWidth <= EpoxyPatternControl.EndpointRectWidth * 2 || polygonHeight <= EpoxyPatternControl.EndpointRectHeight * 2)
                            {
                                _bMinifiable = false;
                            }
                            else
                            {
                                _bMinifiable = true;
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            _bHolding = false;
                        }

                        EpoxyPatternControl.Invalidate();
                    }
                }
            }
            else
            {
                var relation = Utility.CheckPointOnRectangleEdge(_outerRectangleMinPoint, _outerRectangleMaxPoint, mouseLogicalPoint, _xDelta, _yDelta);

                switch (relation)
                {
                case PointAndRectangleRelation.LeftEdge:
                case PointAndRectangleRelation.RightEdge:
                    EpoxyPatternControl.Cursor = Cursors.SizeWE;
                    break;

                case PointAndRectangleRelation.TopEdge:
                case PointAndRectangleRelation.BottomEdge:
                    EpoxyPatternControl.Cursor = Cursors.SizeNS;
                    break;

                //case PointAndRectangleRelation.TopLeftCorner:
                //case PointAndRectangleRelation.BottomRightCorner:
                //    EpoxyPatternControl.Cursor = Cursors.SizeNESW;
                //    break;

                //case PointAndRectangleRelation.TopRightCorner:
                //case PointAndRectangleRelation.BottomLeftCorner:
                //    EpoxyPatternControl.Cursor = Cursors.SizeNWSE;
                //    break;

                case PointAndRectangleRelation.Unkown:
                default:
                    EpoxyPatternControl.Cursor = Cursors.Default;
                    break;
                }
            }
        }

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {
                Curve curve = EpoxyPatternControl.GetCurrentCurve();
                Debug.Assert(curve != null && curve.EndpointCount > 0);

                RefreshCurveBounds(curve);
                RefreshOuterRectangle();

                _bHolding = false;

                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            base.HandlePaintEvent(graphics);

            if (_bHolding)
            {
                graphics.DrawPolygon(Pens.Black, _polygonPixelPoints);

                //foreach (Vector2D vector in _realVectors)
                //{
                //    Utility.FillVector(graphics, vector, _vectorPen, _vectorBrush);
                //}
            }

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }
    }
}
