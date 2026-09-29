using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace AKRS.Galaxy2.Dispense
{
    class AppendPointHandler : ActionHandler
	{
        const string HINT_TEXT = "按下Shift键，可以反转\"吸附\"模式。";

        float _xOffset;
		float _yOffset;

        Curve _curve;

		bool _bMouseOnControl;

		Endpoint _mouseEndpoint;
        Endpoint _tailEndpoint;


		public AppendPointHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{
            _curve = null;
			_bMouseOnControl = false;
            _mouseEndpoint = new Endpoint();

            _xOffset = epoxyPatternControl.EndpointRectWidth / 2;
			_yOffset = epoxyPatternControl.EndpointRectHeight / 2;
        }

		public override void SetMode()
		{
			base.SetMode();

            _curve = EpoxyPatternControl.GetCurrentCurve();
            _tailEndpoint = _curve.GetEndpoint(_curve.EndpointCount - 1);

			EpoxyPatternControl.Cursor = Cursors.Cross;

			_bMouseOnControl = false;
			_mouseEndpoint = new Endpoint();
		}

        public override void HandleExitActionRequest(EventArgs eventArgs)
        {
            base.HandleExitActionRequest(eventArgs);

            _curve = null;
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        { }

        public override void HandleScaleChanged(ScaleChangedEventArgs eventArgs)
        {
			_mouseEndpoint = EpoxyPatternControl.CreateEndpoint(_mouseEndpoint.LogicalPoint);
        }

        public override void HandlePaintEvent(Graphics graphics)
		{
			//EpoxyPatternControl.ShowAction(graphics, "AppendingPoint");

			if (_bMouseOnControl)
			{
                //EpoxyPatternControl.PaintCoordinateLines(graphics, _currentEndpoint);
                
                FillArrowDashLineSegment(graphics, _tailEndpoint.RealPoint, _mouseEndpoint.RealPoint);
			}

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
        }


		public override void HandleMouseDownEvent(MouseEventArgs args)
		{
            if (args.Button == MouseButtons.Left)
            {
                Point2D mousePixelPoint = (Point2D)args.Location;
                Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

                if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
                {
				    Endpoint endpoint;
				    int iEndpointIndex;
				    if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                    {
					    if (iEndpointIndex != _curve.EndpointCount - 1)
                        {
                            mouseLogicalPoint = endpoint.LogicalPoint;
                        }
                    }
                }

			    _tailEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);                
                int iTailEndpointIndex = _curve.AppendPoint(_tailEndpoint, EpoxyPatternControl.DefaultEndPointInfo);

                EpoxyPatternControl.OnEndpointAppended(new EndpointAppendedEventArgs(_curve, iTailEndpointIndex, _tailEndpoint.LogicalPoint));
                EpoxyPatternControl.Invalidate();
            }
		}

		public override void HandleMouseMoveEvent(MouseEventArgs args)
		{
			Debug.Assert(_bMouseOnControl);

			Point2D mousePixelPoint = (Point2D)args.Location;
			Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
            {
				Endpoint endpoint;
				int iEndpointIndex;
				if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                {
					if (iEndpointIndex != _curve.EndpointCount - 1)
                    {
                        mouseLogicalPoint = endpoint.LogicalPoint;
                    }
                }
            }

            if (mouseLogicalPoint != _mouseEndpoint.LogicalPoint)
            {
                _mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);

                EpoxyPatternControl.Invalidate();
            }
		}

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{

		}

        public override void HandleMouseEnterEvent(EventArgs args)
        {
			_bMouseOnControl = true;
            EpoxyPatternControl.Invalidate();
        }

		public override void HandleMouseLeaveEvent(EventArgs args)
		{
			_bMouseOnControl = false;
			EpoxyPatternControl.Invalidate();
		}

    }
}
