using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class DieSizeHandler : ActionHandler
    {
        bool[] _curveVisibilityArray;

        float _xDelta;
        float _yDelta;

        Point2D _controlCenter;
        SizeF _dieSize;
        float _dieRectMinX;
        float _dieRectMinY;
        float _dieRectMaxX;
        float _dieRectMaxY;

        float _scale;

        bool _bHolding;

        PointAndRectangleRelation _relation;
        Point2D _mouseDownLogicalPoint;
        Point2D _previousMouseLogicalPoint;

        Point2D[] _polygonLogicalPoints;
        PointF[] _polygonPixelPoints;

        public DieSizeHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _curveVisibilityArray = null;

            _xDelta = EpoxyPatternControl.EndpointRectWidth / 2F;
            _yDelta = EpoxyPatternControl.EndpointRectHeight / 2F;

            _polygonLogicalPoints = new Point2D[4];
            _polygonPixelPoints = new PointF[4];
        }

        private void UpdatePolygonPoints()
        {
            _polygonLogicalPoints[0] = new Point2D(_dieRectMinX, _dieRectMinY);
            _polygonLogicalPoints[1] = new Point2D(_dieRectMaxX, _dieRectMinY);
            _polygonLogicalPoints[2] = new Point2D(_dieRectMaxX, _dieRectMaxY);
            _polygonLogicalPoints[3] = new Point2D(_dieRectMinX, _dieRectMaxY);

            for (int i = 0; i < _polygonLogicalPoints.Length; i++)
            {
                _polygonPixelPoints[i] = (PointF)EpoxyPatternControl.CreateRealPoint(_polygonLogicalPoints[i]);
            }
        }

        public override void SetMode()
        {
            base.SetMode();

            EpoxyPatternControl.HideAllCurves(out _curveVisibilityArray);

            _controlCenter = new Point2D(EpoxyPatternControl.Width / 2F, EpoxyPatternControl.Height / 2F);
            _dieSize = EpoxyPatternControl.DieSize;            
            _dieRectMinX = -1 * _dieSize.Width / 2F;
            _dieRectMinY = -1 * _dieSize.Height / 2F;
            _dieRectMaxX = _dieSize.Width / 2F;
            _dieRectMaxY = _dieSize.Height / 2F;

            Console.WriteLine("_dieSize : {0}", _dieSize);
            Console.WriteLine("_dieRectMinX : {0}", _dieRectMinX);
            Console.WriteLine("_dieRectMinY : {0}", _dieRectMinY);
            Console.WriteLine("_dieRectMaxX : {0}", _dieRectMaxX);
            Console.WriteLine("_dieRectMaxY : {0}", _dieRectMaxY);

            UpdatePolygonPoints();

            _scale = EpoxyPatternControl.GetScale();

            _bHolding = false;
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
            Debug.Assert(_bHolding == false);

            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
            _relation = Utility.CheckPointOnRectangleEdge(_dieRectMinX, _dieRectMinY, _dieRectMaxX, _dieRectMaxY, mouseLogicalPoint, _xDelta, _yDelta);

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

            case PointAndRectangleRelation.Unkown:
            default:
                EpoxyPatternControl.Cursor = Cursors.Default;
                break;
            }

            if (_relation != PointAndRectangleRelation.Unkown)
            {
                _mouseDownLogicalPoint = mouseLogicalPoint;
                _previousMouseLogicalPoint = mouseLogicalPoint;

                UpdatePolygonPoints();

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

                if (_relation == PointAndRectangleRelation.LeftEdge || _relation == PointAndRectangleRelation.RightEdge)
                {
                    float xIncrement = mouseLogicalPoint.X - _previousMouseLogicalPoint.X;
                    _dieSize = new SizeF(_dieSize.Width + xIncrement, _dieSize.Height);
                    _dieRectMinX -= xIncrement / 2F;
                    _dieRectMaxX += xIncrement / 2F;
                }
                else
                {
                    Debug.Assert(_relation == PointAndRectangleRelation.TopEdge || _relation == PointAndRectangleRelation.BottomEdge);

                    float yIncrement = mouseLogicalPoint.Y - _previousMouseLogicalPoint.Y;
                    _dieSize = new SizeF(_dieSize.Width, _dieSize.Height + yIncrement);
                    _dieRectMinY -= yIncrement / 2F;
                    _dieRectMaxY += yIncrement / 2F;
                }

                UpdatePolygonPoints();

                _previousMouseLogicalPoint = mouseLogicalPoint;
                
                //EpoxyPatternControl.SetDieSize(_dieSize);
                EpoxyPatternControl.Invalidate();
            }
            else
            {
                var relation = Utility.CheckPointOnRectangleEdge(_dieRectMinX, _dieRectMinY, _dieRectMaxX, _dieRectMaxY, mouseLogicalPoint, _xDelta, _yDelta);

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
                //RefreshDieRectangle();

                EpoxyPatternControl.Cursor = Cursors.Default;
                _bHolding = false;

                EpoxyPatternControl.Invalidate();
            }     
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            if (_dieSize.Width > 0 && _dieSize.Height > 0)
            {
                graphics.DrawPolygon(Pens.Black, _polygonPixelPoints);

                float diePixelWidth = Math.Abs(_polygonPixelPoints[0].X - _polygonPixelPoints[1].X);
                float diePixelHeight = Math.Abs(_polygonPixelPoints[1].Y - _polygonPixelPoints[2].Y);

                
                Font font = EpoxyPatternControl.Font;
                

                {
                    string strDieWidth = String.Format("{0}", _dieSize.Width);
                    SizeF stringSize = graphics.MeasureString(strDieWidth, font);
                    float x = EpoxyPatternControl.Width / 2F - stringSize.Width / 2F;
                    float y = EpoxyPatternControl.Height / 2F - diePixelHeight / 2F - stringSize.Height;
                    RectangleF layoutRectangle = new RectangleF(x, y, stringSize.Width, stringSize.Height);
                    graphics.DrawString(strDieWidth, font, Brushes.Black, layoutRectangle);
                }

                {
                    string strDieHeight = String.Format("{0}", _dieSize.Height);
                    SizeF stringSize = graphics.MeasureString(strDieHeight, font);
                    float x = EpoxyPatternControl.Width / 2F + diePixelWidth / 2F;
                    float y = EpoxyPatternControl.Height / 2F + stringSize.Width / 2F;
                    RectangleF layoutRectangle = new RectangleF(x, y, stringSize.Width, stringSize.Height);
                    graphics.DrawString(strDieHeight, font, Brushes.Black, layoutRectangle);
                }
            }
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        { }

        public override void HandleExitActionRequest(EventArgs args)
        {
            EpoxyPatternControl.ShowCurves(_curveVisibilityArray);

            base.HandleExitActionRequest(args);

            EpoxyPatternControl.SetDieSize(_dieSize.Width, _dieSize.Height);
        }
    }
}
