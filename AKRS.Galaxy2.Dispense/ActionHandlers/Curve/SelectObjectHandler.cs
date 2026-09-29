using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
    class SelectObjectHandler : ActionHandler
    {
        public SelectObjectHandler(EpoxyPatternControl epoxyPatternControl)
            : base(epoxyPatternControl)
        { }

        public override void SetMode()
        {
            base.SetMode();
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
        {
        }

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
			Point2D mousePixelPoint = (Point2D)args.Location;
			Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
			
			Curve curve = EpoxyPatternControl.GetCurrentCurve();
						
			float xOffset = EpoxyPatternControl.EndpointRectWidth / 2;
			float yOffset = EpoxyPatternControl.EndpointRectHeight / 2;
			Endpoint endpoint;
			int iEndpointIndex;				
            if (curve.ContainsEndpoint(mouseLogicalPoint, xOffset, yOffset, out endpoint, out iEndpointIndex))
			{
				EpoxyPatternControl.HighlightedEndpointIndex = iEndpointIndex;
                EpoxyPatternControl.Cursor = Cursors.Hand;
				EpoxyPatternControl.Invalidate();
			}
			else
			{
				if (EpoxyPatternControl.HighlightedEndpointIndex != EpoxyPatternControl.INVALID_ENDPOINT_INDEX)
				{
					EpoxyPatternControl.ClearHighlightedEndpoint();
                    EpoxyPatternControl.Cursor = Cursors.Arrow;
					EpoxyPatternControl.Invalidate();
				}
			}
		}

        public override void HandleMouseUpEvent(MouseEventArgs args)
        {
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
        }

        public override void HandlePaintEvent(Graphics g)
        {
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        {
        }
    }
}
