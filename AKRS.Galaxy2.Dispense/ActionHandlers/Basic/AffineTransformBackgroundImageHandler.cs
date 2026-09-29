using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    abstract class AffineTransformBackgroundImageHandler : ActionHandler
    {
        Pen _axisPen;
        Brush _axisBrush;

        Vector2D _horizontalAxisLogicalVector;
        Vector2D _verticalAxisLogicalVector;
        Vector2D _horizontalAxisPixelVector;
        Vector2D _verticalAxisPixelVector;

        public AffineTransformBackgroundImageHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        {
            _axisPen = new Pen(Color.Yellow, 0);
            _axisPen.DashStyle = DashStyle.Solid;

            _axisBrush = new SolidBrush(Color.Yellow);

            _horizontalAxisLogicalVector = new Vector2D();
            _verticalAxisLogicalVector = new Vector2D();

            _horizontalAxisPixelVector = new Vector2D();
            _verticalAxisPixelVector = new Vector2D();
        }

        public override void SetMode()
        {
            base.SetMode();

            SizeF imagePixelSize = EpoxyPatternControl.BackgroundImageInfo.Image.Size;
            SizeF imageLogicalSize = EpoxyPatternControl.ConvertToLogicalSize(imagePixelSize);

            float imageShortEdgeLength = Math.Min(imageLogicalSize.Width, imageLogicalSize.Height);
            float circleRadius = imageShortEdgeLength / 2;

            _horizontalAxisLogicalVector = new Vector2D(
                initialPoint: new Point2D(-1 * circleRadius, 0),
                terminalPoint: new Point2D(circleRadius, 0));
            _verticalAxisLogicalVector = new Vector2D(
                initialPoint: new Point2D(0, -1 * circleRadius),
                terminalPoint: new Point2D(0, circleRadius));

            UpdatePixelVectors();
        }

        private void UpdatePixelVectors()
        {
            _horizontalAxisPixelVector = new Vector2D(
                initialPoint: EpoxyPatternControl.ConvertToPixelPoint(_horizontalAxisLogicalVector.InitialPoint),
                terminalPoint: EpoxyPatternControl.ConvertToPixelPoint(_horizontalAxisLogicalVector.TerminalPoint));

            _verticalAxisPixelVector = new Vector2D(
                initialPoint: EpoxyPatternControl.ConvertToPixelPoint(_verticalAxisLogicalVector.InitialPoint),
                terminalPoint: EpoxyPatternControl.ConvertToPixelPoint(_verticalAxisLogicalVector.TerminalPoint));
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
            UpdatePixelVectors();
            EpoxyPatternControl.Invalidate();
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        {
            UpdatePixelVectors();
            EpoxyPatternControl.Invalidate();
        }

        public override void HandlePaintEvent(Graphics graphics)
        {
            Utility.FillVector(graphics, _horizontalAxisPixelVector, _axisPen, _axisBrush);
            Utility.FillVector(graphics, _verticalAxisPixelVector, _axisPen, _axisBrush);
        }


    }
}
