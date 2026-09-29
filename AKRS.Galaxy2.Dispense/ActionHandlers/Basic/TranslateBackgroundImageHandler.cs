using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class TranslateBackgroundImageHandler : AffineTransformBackgroundImageHandler
    {
        const string HINT_TEXT = "按下鼠标左键，拖动鼠标以移动图片。\r\n同时按下Shift键使得仅水平方向移动\r\n同时按下Ctrl键使得仅垂直方向移动。";

        bool _bHolding = false;

        Point2D _previousMouseLogicalPoint;

        Vector2D _xLogicalVector;
        Vector2D _yLogicalVector;
        Vector2D _xPixelVector;
        Vector2D _yPixelVector;

        Pen _vectorPen;
        Brush _vectorBrush;

        public TranslateBackgroundImageHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _vectorPen = new Pen(Color.Blue);
            _vectorBrush = new SolidBrush(Color.Blue);
        }

        public override void SetMode()
        {
            base.SetMode();

            _bHolding = false;

            SizeF imageLogicalSize = EpoxyPatternControl.BackgroundImageInfo.Image.Size;

            float imageShortEdgeLength = Math.Min(imageLogicalSize.Width, imageLogicalSize.Height);
            float circleRadius = imageShortEdgeLength / 2;

            _xLogicalVector = new Vector2D(initialPoint: new Point2D(-1 * circleRadius, 0), terminalPoint: new Point2D(circleRadius, 0));
            _yLogicalVector = new Vector2D(initialPoint: new Point2D(0, -1 * circleRadius), terminalPoint: new Point2D(0, circleRadius));

            UpdatePixelVectors();
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            EpoxyPatternControl.Cursor = Cursors.SizeAll;

            _previousMouseLogicalPoint = mouseLogicalPoint;
            _bHolding = true;
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if (_bHolding)
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

                _xLogicalVector = _xLogicalVector.Translate(xIncrement, yIncrement);
                _yLogicalVector = _yLogicalVector.Translate(xIncrement, yIncrement);

                UpdatePixelVectors();

                _previousMouseLogicalPoint = mouseLogicalPoint;

                EpoxyPatternControl.TranslateBackgroundImage(xIncrement, -1 * yIncrement);
                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {
                Point2D mousePixelPoint = (Point2D)args.Location;
                Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

                float xIncrement = mouseLogicalPoint.X - _previousMouseLogicalPoint.X;
                float yIncrement = mouseLogicalPoint.Y - _previousMouseLogicalPoint.Y;
                EpoxyPatternControl.TranslateBackgroundImage(xIncrement, yIncrement);

                EpoxyPatternControl.Cursor = Cursors.Default;
                
                _bHolding = false;
            }
        }

        private void UpdatePixelVectors()
        {
            if (false)
            {
                _xPixelVector = new Vector2D(
                    EpoxyPatternControl.ConvertToPixelPoint(_xLogicalVector.InitialPoint),
                    EpoxyPatternControl.ConvertToPixelPoint(_xLogicalVector.TerminalPoint));
                _yPixelVector = new Vector2D(
                    EpoxyPatternControl.ConvertToPixelPoint(_yLogicalVector.InitialPoint),
                    EpoxyPatternControl.ConvertToPixelPoint(_yLogicalVector.TerminalPoint));
            }
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        {
            base.HandleScaleChanged(args);

            UpdatePixelVectors();

            EpoxyPatternControl.Invalidate();
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            base.HandlePaintEvent(graphics);

            if (false)
            {
                Utility.FillVector(graphics, _xPixelVector, _vectorPen, _vectorBrush);
                Utility.FillVector(graphics, _yPixelVector, _vectorPen, _vectorBrush);
            }

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }
    }
}
