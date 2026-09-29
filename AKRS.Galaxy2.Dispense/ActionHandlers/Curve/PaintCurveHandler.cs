using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Diagnostics;

namespace AKRS.Galaxy2.Dispense
{
	class PaintCurveHandler : ActionHandler
	{
        const string HINT_TEXT = "按下Shift键，可以反转\"吸附\"模式。";

        Curve _curve;
		bool _bMouseOnControl;
		int _iCurveEndpointCount;
		Endpoint _curveLastEndpoint;

		Endpoint _mouseEndpoint;

        bool _bDrawCoordinateLines;

        float _xOffset;
        float _yOffset;


		public PaintCurveHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{
            _curve = null;
			_bMouseOnControl = false;
			_iCurveEndpointCount = 0;

            _bDrawCoordinateLines = false;

            _xOffset = epoxyPatternControl.EndpointRectWidth / 2;
            _yOffset = epoxyPatternControl.EndpointRectHeight / 2;
		}

		public override void SetMode()
		{
			base.SetMode();

			EpoxyPatternControl.Cursor = Cursors.Cross;
			int iNewCurveIndex = EpoxyPatternControl.AddCurve();
            _curve = EpoxyPatternControl.GetCurve(iNewCurveIndex);

			_iCurveEndpointCount = 0;
			_bMouseOnControl = false;
            _bDrawCoordinateLines = true;

            EpoxyPatternControl.OnCurveAdded(new CurveAddedEventArgs(iNewCurveIndex, _curve));
		}

        public override void HandleExitActionRequest(EventArgs args)
        {
			if (_curve.EndpointCount < 2)
			{
				_curve.Clear();
				EpoxyPatternControl.RemoveCurrentCurve();
			}

            _curve = null;

			EpoxyPatternControl.ResetAction();
			EpoxyPatternControl.Invalidate();
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        {
            Debug.Assert(_curve != null);

			if (_curve.EndpointCount > 0)
            {
				_curveLastEndpoint = _curve.GetEndpoint(_curve.EndpointCount - 1);
            }			

			if (_bMouseOnControl)
			{
				_mouseEndpoint = EpoxyPatternControl.CreateEndpoint(_mouseEndpoint.LogicalPoint);

				Point2D cursorPixelPoint = (Point2D)EpoxyPatternControl.PointToClient(Cursor.Position);
				Point2D cursorLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(cursorPixelPoint);

				Point2D newCursorPixelPoint;
				if (cursorLogicalPoint == _mouseEndpoint.LogicalPoint)
				{
					newCursorPixelPoint = _mouseEndpoint.RealPoint;
				}
				else
                {
					Endpoint endpoint = EpoxyPatternControl.CreateEndpoint(cursorLogicalPoint);
					newCursorPixelPoint = endpoint.RealPoint;
                }
			}
        }

        public override void HandlePaintEvent(Graphics graphics)
		{
            if (_bMouseOnControl)
            {
                if (_bDrawCoordinateLines)
                    EpoxyPatternControl.PaintCoordinateLines(graphics, _mouseEndpoint);

                if (_iCurveEndpointCount > 0)
                {
                    if (_curveLastEndpoint.LogicalPoint != _mouseEndpoint.LogicalPoint)
                    {
                        FillArrowDashLineSegment(graphics, _curveLastEndpoint.RealPoint, _mouseEndpoint.RealPoint);
                    }
                }
            }

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
		}
		
		public override void HandleMouseDownEvent(MouseEventArgs args)
		{
			Debug.Assert(_curve != null);

			if (args.Button == MouseButtons.Left)
			{
                Point2D mousePixelPoint = (Point2D)args.Location;
				Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

				Endpoint mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);

				if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
				{
					Endpoint endpoint;
					int iEndPointIndex;
					if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndPointIndex))
					{
						Debug.Assert(_iCurveEndpointCount > 0);
						if (endpoint.LogicalPoint != _curveLastEndpoint.LogicalPoint)
							mouseEndpoint = endpoint;
					}
				}

				int iNewEndpointIndex = _curve.AppendPoint(mouseEndpoint, EpoxyPatternControl.DefaultEndPointInfo);				

				_curveLastEndpoint = mouseEndpoint;
				_mouseEndpoint = mouseEndpoint;
				_iCurveEndpointCount++;

				EpoxyPatternControl.Invalidate();

                EpoxyPatternControl.OnEndpointAppended(new EndpointAppendedEventArgs(_curve, iNewEndpointIndex, mouseLogicalPoint));
			}
		}


		public override void HandleMouseMoveEvent(MouseEventArgs args)
		{
            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
            _mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);

            Console.WriteLine("mousePixelPoint : {0}", mousePixelPoint);
            Console.WriteLine("mouseLogicalPoint : {0}", mouseLogicalPoint);
            Console.WriteLine("_currentEndpoint.RealPoint : {0}", _mouseEndpoint.RealPoint);
            Console.WriteLine("_currentEndpoint.RealRectangle : {0}", _mouseEndpoint.RealRectangle);
            Console.WriteLine();

            if (_iCurveEndpointCount > 0)
			{
				if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
                {								
					Endpoint endpoint;
					int iEndpointIndex;
					if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                    {
						if (endpoint.LogicalPoint != _curveLastEndpoint.LogicalPoint)
                        {
							_mouseEndpoint = endpoint;
                        }
                    }
                }			
			}

            if (_bDrawCoordinateLines || _iCurveEndpointCount > 0)
                EpoxyPatternControl.Invalidate();
		}

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{ }

        public override void HandleMouseEnterEvent(EventArgs args)
        {
			_bMouseOnControl = true;

			if (_bDrawCoordinateLines || _iCurveEndpointCount > 0)
				EpoxyPatternControl.Invalidate();
        }

        public override void HandleMouseLeaveEvent(EventArgs args)
        {
			_bMouseOnControl = false;

			if (_bDrawCoordinateLines || _iCurveEndpointCount > 0)
				EpoxyPatternControl.Invalidate();
        }


    }

}
