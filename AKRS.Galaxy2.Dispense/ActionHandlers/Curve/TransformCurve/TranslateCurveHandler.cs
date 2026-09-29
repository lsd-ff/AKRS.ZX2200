using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class TranslateCurveHandler : AffineTransformCurveHandler
    {
        const string HINT_TEXT = "按下鼠标左键，拖动鼠标以移动当前曲线。\r\n同时按下Shift键使得仅水平方向移动。\r\n同时按下Ctrl键使得仅垂直方向移动。";

        bool _bHolding;
        Point2D _mouseDownLogicalPoint;
        Point2D _previousMouseLogicalPoint;

        Point2D _originalMinLogicalPoint;
        Point2D _originalMaxLogicalPoint;

        public TranslateCurveHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _bHolding = false;
            _mouseDownLogicalPoint = new Point2D();
            _previousMouseLogicalPoint = new Point2D();

            RectangleVisible = true;
            CircleVisible = false;
        }

        public override void SetMode()
        {
            base.SetMode();

            _bHolding = false;
            _mouseDownLogicalPoint = new Point2D();
            _previousMouseLogicalPoint = new Point2D();
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
                EpoxyPatternControl.Cursor = Cursors.SizeAll;

                _mouseDownLogicalPoint = mouseLogicalPoint;
                _previousMouseLogicalPoint = mouseLogicalPoint;
                _originalMinLogicalPoint = MinLogicalPoint;
                _originalMaxLogicalPoint = MaxLogicalPoint;
                _bHolding = true;
            }
            else
            {
                EpoxyPatternControl.Cursor = Cursors.Default;
                _bHolding = false;
            }
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if (_bHolding)
            {                
                Curve curve = EpoxyPatternControl.GetCurrentCurve();
                if (curve != null && curve.EndpointCount > 0)
                {
                    float xIncrement = mouseLogicalPoint.X - _previousMouseLogicalPoint.X;
                    float yIncrement = mouseLogicalPoint.Y - _previousMouseLogicalPoint.Y;

                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        yIncrement = 0;
                    }
                    else if (Control.ModifierKeys == Keys.Control)
                    {
                        xIncrement = 0;
                    }
                    else
                    { }

                    EpoxyPatternControl.TranslateCurrentCurve(xIncrement, yIncrement);

                    Point2D newMinLogicalPoint = new Point2D(MinLogicalPoint.X + xIncrement, MinLogicalPoint.Y + yIncrement);
                    Point2D newMaxLogicalPoint = new Point2D(MaxLogicalPoint.X + xIncrement, MaxLogicalPoint.Y + yIncrement);
                    SetMinAndMaxLogicalPoints(newMinLogicalPoint, newMaxLogicalPoint);

                    _previousMouseLogicalPoint = mouseLogicalPoint;

                    EpoxyPatternControl.Invalidate();
                }
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

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {
                _bHolding = false;
                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            base.HandlePaintEvent(graphics);

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }
    }
}
