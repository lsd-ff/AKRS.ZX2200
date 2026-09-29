using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class RotateBackgroundImageHandler : AffineTransformBackgroundImageHandler
    {
        const string HINT_TEXT = "按下鼠标左键，上下拖动鼠标以旋转图片\r\n同时按下Shift键使旋转角度更精细。";

        bool _bHolding;

        Point2D _previousMouseLogicalPoint;
        Point2D _previousMousePixelPoint;

        Vector2D _xLogicalVector;
        Vector2D _yLogicalVector;

        Vector2D _xPixelVector;
        Vector2D _yPixelVector;

        float _accumulatedAngle;

        Pen _vectorPen;
        Brush _vectorBrush;

        Pen _arcPen;
        Brush _arcBrush;

        Circle _pixelCircle;

        PointF[] _additionalLogicalPoints;
        Matrix _matrix;

        Font _degreeTextFont;
        StringFormat _degreeTextStringFormat;

        public RotateBackgroundImageHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _vectorPen = new Pen(Color.Blue);
            _vectorBrush = new SolidBrush(Color.Blue);

            _arcPen = new Pen(Color.Blue, 3);
            _arcBrush = new SolidBrush(Color.Blue);

            _pixelCircle = new Circle();

            _additionalLogicalPoints = new PointF[16];
            _matrix = new Matrix();

            _degreeTextFont = EpoxyPatternControl.Font;

            _degreeTextStringFormat = new StringFormat();
            _degreeTextStringFormat.Alignment = StringAlignment.Center;
            _degreeTextStringFormat.LineAlignment = StringAlignment.Near;
        }

        public override void SetMode()
        {
            base.SetMode();

            _accumulatedAngle = 0;

            InitVectors();
            InitPixelCircle();
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            _previousMouseLogicalPoint = mouseLogicalPoint;
            _previousMousePixelPoint = mousePixelPoint;

            _bHolding = true;

            EpoxyPatternControl.Cursor = Cursors.NoMoveVert;
            EpoxyPatternControl.Invalidate();
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {
                Point2D mousePixelPoint = (Point2D)args.Location;
                Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

                float yDelta = mousePixelPoint.Y - _previousMousePixelPoint.Y;
                float degree = yDelta;
                if (mouseLogicalPoint.X > 0)
                {
                    degree *= -1;
                }

                if (Control.ModifierKeys == Keys.Shift)
                    degree /= 10;

                _additionalLogicalPoints[0] = (PointF)_xLogicalVector.InitialPoint;
                _additionalLogicalPoints[1] = (PointF)_xLogicalVector.TerminalPoint;
                _additionalLogicalPoints[2] = (PointF)_yLogicalVector.InitialPoint;
                _additionalLogicalPoints[3] = (PointF)_yLogicalVector.TerminalPoint;

                _matrix.Reset();
                _matrix.RotateAt(degree, new PointF(0, 0));
                _matrix.TransformPoints(_additionalLogicalPoints);

                _xLogicalVector = new Vector2D((Point2D)_additionalLogicalPoints[0], (Point2D)_additionalLogicalPoints[1]);
                _yLogicalVector = new Vector2D((Point2D)_additionalLogicalPoints[2], (Point2D)_additionalLogicalPoints[3]);
                UpdatePixelVectors();

                _accumulatedAngle -= degree;

                _previousMouseLogicalPoint = mouseLogicalPoint;
                _previousMousePixelPoint = mousePixelPoint;

                EpoxyPatternControl.RotateBackgroundImage(-1 * degree);
                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
            if (_bHolding)
            {                
                _bHolding = false;

                EpoxyPatternControl.Cursor = Cursors.Default;
                EpoxyPatternControl.Invalidate();
            }
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            base.HandlePaintEvent(graphics);

            Utility.FillVector(graphics, _xPixelVector, _vectorPen, _vectorBrush);
            Utility.FillVector(graphics, _yPixelVector, _vectorPen, _vectorBrush);

            if (_accumulatedAngle != 0)
            {
                float arcStartAngle = -90;
                graphics.DrawArc(_arcPen, _pixelCircle.CircumscribedRectangle, arcStartAngle, _accumulatedAngle);

                string strDegreeText = String.Format("{0}°", _accumulatedAngle);                                               
                RectangleF layoutRectangle = CalculateDegreeTextLayoutRectangle(graphics, strDegreeText);
                //graphics.DrawRectangle(Pens.Black, layoutRectangle.X, layoutRectangle.Y, layoutRectangle.Width, layoutRectangle.Height);
                graphics.DrawString(strDegreeText, _degreeTextFont, _arcBrush, layoutRectangle);
            }

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }

        private RectangleF CalculateDegreeTextLayoutRectangle(Graphics graphics, string strDegreeText)
        {
            SizeF degreeTextSize = graphics.MeasureString(strDegreeText, _degreeTextFont);

            PointF point;
            if (_yLogicalVector.TerminalPoint.X > 0)
            {
                if (_yLogicalVector.TerminalPoint.Y > 0)
                {
                    point = (PointF)_yPixelVector.TerminalPoint.Translate(-0.5F * degreeTextSize.Width, -1 * degreeTextSize.Height);
                }
                else
                {
                    point = (PointF)_yPixelVector.TerminalPoint.Translate(-0.5F * degreeTextSize.Width, 1 * degreeTextSize.Height);
                }
            }
            else
            {
                if (_yLogicalVector.TerminalPoint.Y > 0)
                {
                    point = (PointF)_yPixelVector.TerminalPoint.Translate(degreeTextSize.Width, -1 * degreeTextSize.Height);
                }
                else
                {
                    point = (PointF)_yPixelVector.TerminalPoint.Translate(degreeTextSize.Width, 1 * degreeTextSize.Height);
                }
            }

            return new RectangleF(point, degreeTextSize);
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs eventArgs)
        {
            base.HandleScaleChanged(eventArgs);

            UpdatePixelVectors();
            UpdatePixelCircle();

            EpoxyPatternControl.Invalidate();
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
            base.HandleLogicalCoordinateSystemOriginChanged(eventArgs);

            UpdatePixelVectors();
            UpdatePixelCircle();

            EpoxyPatternControl.Invalidate();
        }

        private void InitVectors()
        {
            //SizeF imagePixelSize = EpoxyPatternControl.BackgroundImageInfo.Image.Size;
            //SizeF imageLogicalSize = EpoxyPatternControl.ConvertToLogicalSize(imagePixelSize);
            SizeF imageLogicalSize = EpoxyPatternControl.BackgroundImageInfo.Image.Size;

            float imageShortEdgeLength = Math.Min(imageLogicalSize.Width, imageLogicalSize.Height);
            float circleRadius = imageShortEdgeLength / 2;


            _xLogicalVector = new Vector2D(initialPoint: new Point2D(-1 * circleRadius, 0),
                                           terminalPoint: new Point2D(circleRadius, 0));            
            _yLogicalVector = new Vector2D(initialPoint: new Point2D(0, -1 * circleRadius),
                                           terminalPoint: new Point2D(0, circleRadius));
            UpdatePixelVectors();
        }

        private void InitPixelCircle()
        {
            UpdatePixelCircle();
        }


        private void UpdatePixelVectors()
        {
            _xPixelVector = new Vector2D(initialPoint: EpoxyPatternControl.CreateRealPoint(_xLogicalVector.InitialPoint),
                                         terminalPoint: EpoxyPatternControl.CreateRealPoint(_xLogicalVector.TerminalPoint));

            _yPixelVector = new Vector2D(initialPoint: EpoxyPatternControl.CreateRealPoint(_yLogicalVector.InitialPoint),
                                         terminalPoint: EpoxyPatternControl.CreateRealPoint(_yLogicalVector.TerminalPoint));
        }

        private void UpdatePixelCircle()
        {
            SizeF imageLogicalSize = EpoxyPatternControl.BackgroundImageInfo.Image.Size;

            float circleLogicalRadius = Math.Min(imageLogicalSize.Width, imageLogicalSize.Height) / 4F;
            //float circleLogicalRadius = Math.Min(imageLogicalSize.Width, imageLogicalSize.Height);

            //float circleLogicalRadius = (float)Utility.CalculateLineSegmentLength(_xLogicalVector.InitialPoint, _xLogicalVector.TerminalPoint) / 2F;

            //Point2D rectMinPoint = new Point2D(-1 * circleLogicalRadius / 2, -1 * circleLogicalRadius / 2);
            //Point2D rectMaxPoint = new Point2D(circleLogicalRadius / 2, circleLogicalRadius / 2);
            Point2D rectMinPoint = new Point2D(-1 * circleLogicalRadius, -1 * circleLogicalRadius);
            Point2D rectMaxPoint = new Point2D(circleLogicalRadius, circleLogicalRadius);

            Point2D rectPixelTopLeft = EpoxyPatternControl.ConvertToPixelPoint(rectMinPoint);
            Point2D rectPixelBottomRight = EpoxyPatternControl.ConvertToPixelPoint(rectMaxPoint);

            float rectMinX = rectPixelTopLeft.X;
            float rectMaxX = rectPixelBottomRight.X;
            float rectMinY = rectPixelBottomRight.Y;
            float rectMaxY = rectPixelTopLeft.Y;
            _pixelCircle.Set(rectMinX, rectMinY, rectMaxX, rectMaxY);
        }
    }
}
