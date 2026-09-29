/* 【擦除点】
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
	/// <summary>
	/// 
	/// </summary>
	class ErasePointHandler : ActionHandler
	{
        const char CHAR_ESC = (char)0x001B;

        const string HINT_TEXT = "按下Shift键，可以反转\"吸附\"模式。";

        float _xOffset;
        float _yOffset;

		int _iCandidateEndpointIndex;

        Curve _curve;

		public ErasePointHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{
			_iCandidateEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;

            _xOffset = epoxyPatternControl.EndpointRectWidth / 2;
            _yOffset = epoxyPatternControl.EndpointRectHeight / 2;

            _curve = null;
		}

		public override void SetMode()
		{
			base.SetMode();

			_iCandidateEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;

            _curve = EpoxyPatternControl.GetCurrentCurve();
		}

		public override void HandleExitActionRequest(EventArgs eventArgs)
		{
            base.HandleExitActionRequest(eventArgs);

            _curve = null;
			EpoxyPatternControl.ClearHighlightedEndpoint();
			EpoxyPatternControl.ResetAction();
		}

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        { }

        public override void HandleScaleChanged(ScaleChangedEventArgs eventArgs)
        { }

        public override void HandlePaintEvent(Graphics graphics)
		{
			//EpoxyPatternControl.ShowAction(graphics, "ErasingPoint");
		}

        public override void HandleMouseDownEvent(MouseEventArgs eventArgs)
		{
            if (eventArgs.Button == MouseButtons.Left)
            {
                Debug.Assert(_curve != null);

                Point2D mousePixelPoint = (Point2D)eventArgs.Location;
                Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
                
                Endpoint endpoint;
                int iEndpointIndex;
                if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                {
                    _iCandidateEndpointIndex = iEndpointIndex;
                }
            }
		}

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{
			if (args.Button == MouseButtons.Left)
            {
                Debug.Assert(_curve != null);

                if (_iCandidateEndpointIndex != EpoxyPatternControl.INVALID_ENDPOINT_INDEX)
                {
                    Point2D mousePixelPoint = (Point2D)args.Location;
                    Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);

                    Endpoint endpoint;
                    int iEndpointIndex;
                    if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                    {
                        if (_iCandidateEndpointIndex == iEndpointIndex)
                        {                            
                            _curve.RemoveEndpoint(iEndpointIndex);

                            EpoxyPatternControl.Cursor = Cursors.Arrow;                            
                            EpoxyPatternControl.ClearHighlightedEndpoint();
                            EpoxyPatternControl.OnEndpointErased(new EndpointDeletedEventArgs(_curve, iEndpointIndex, endpoint.LogicalPoint));
                            EpoxyPatternControl.Invalidate();

                            _iCandidateEndpointIndex = EpoxyPatternControl.INVALID_ENDPOINT_INDEX;
                        }
                    }
                }
            }
		}

        public override void HandleMouseMoveEvent(MouseEventArgs args)
        {
            Debug.Assert(_curve != null);

            Point2D mousePixelPoint = (Point2D)args.Location;
            Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
         
            Endpoint endpoint;
            int iEnpointIndex;
            if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEnpointIndex))
            {
                EpoxyPatternControl.HighlightedEndpointIndex = iEnpointIndex;
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

        public override void HandleKeyPressEvent(KeyPressEventArgs args)
        {
            base.HandleKeyPressEvent(args);

            //if (args.KeyChar == CHAR_ESC)
            //{
            //    EpoxyPatternControl.ClearHighlightedEndpoint();
            //    EpoxyPatternControl.ResetAction();
            //}
        }
    }
}
