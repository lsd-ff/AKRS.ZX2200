/* 【移动点】
 * 鼠标移动到线段端点上，高亮该端点。离开该端点后，该端点恢复原样。
 * 鼠标移动到线段端点上，再按下鼠标左键，此时鼠标持有该端点。松开左键，鼠标释放该端点。
 * 当鼠标持有端点时，鼠标移动带动端点。
 * 右击鼠标，结束当前模式。
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
	class MovePointHandler : ActionHandler
	{
        const string HINT_TEXT = "按下Shift键，可以反转\"吸附\"模式。";

        float _xOffset;
        float _yOffset;

        Curve _curve;	
		
        bool _bHolding;
        int _iMouseEndpointIndex;

		public MovePointHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{
            _curve = null;
            _iMouseEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;
			_bHolding = false;

            _xOffset = epoxyPatternControl.EndpointRectWidth / 2;
			_yOffset = epoxyPatternControl.EndpointRectHeight / 2;
		}

		public override void SetMode()
		{
            _curve = EpoxyPatternControl.GetCurrentCurve();
            _iMouseEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;
			_bHolding = false;
		}

        public override void HandleExitActionRequest(EventArgs args)
        {
			EpoxyPatternControl.ResetAction();

            _curve = null;
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
        }

        public override void HandleScaleChanged(ScaleChangedEventArgs eventArgs)
        {
        }

        public override void HandlePaintEvent(Graphics graphics)
		{
			//EpoxyPatternControl.ShowAction(graphics, "MovingPoint");

			if (_bHolding)
			{
                Debug.Assert(_curve != null);

                EpoxyPatternControl.HighlightCurveEndpoint(graphics, _curve, _iMouseEndpointIndex);
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
                Debug.Assert(_curve != null);

				Point2D mousePixelPoint = (Point2D)args.Location;
				Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

				Endpoint endpoint;
				int iEndpointIndex;
				if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
				{
					_iMouseEndpointIndex = iEndpointIndex;
                    _bHolding = true;

					EpoxyPatternControl.Cursor = Cursors.Hand;
                    EpoxyPatternControl.ClearHighlightedEndpoint();
					EpoxyPatternControl.Invalidate();
				}
			}
		}

		public override void HandleMouseMoveEvent(MouseEventArgs args)
		{  
			Point2D mousePixelPoint = (Point2D)args.Location;
			Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

            Debug.Assert(_curve != null);
			
			if (_bHolding)
			{
                if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
                {
				    Endpoint endpoint;
				    int iEndpointIndex;
                    if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
				    {
                        mouseLogicalPoint = endpoint.LogicalPoint;
				    }
                }

				Endpoint mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);
                _curve.ModifyEndpoint(_iMouseEndpointIndex, mouseEndpoint);

                EpoxyPatternControl.OnEndpointChanged(new EndpointChangedEventArgs(_curve, _iMouseEndpointIndex, mouseLogicalPoint));
                EpoxyPatternControl.Invalidate();
			}
			else
			{			
				Endpoint endpoint;
				int iEndpointIndex;				
                if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
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
		}

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left)
			{
				_bHolding = false;
				EpoxyPatternControl.Cursor = Cursors.Default;
			}
		}
	}
}
