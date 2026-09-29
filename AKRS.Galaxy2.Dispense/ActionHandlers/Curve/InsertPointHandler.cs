/* 【插入点】： 即将一条线段分割成两段，由新插入的点连接两段。
 * 鼠标移动到线段上，该线段高亮。鼠标离开后，该线段恢复原样。
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
	class InsertPointHandler : ActionHandler
	{
        const string HINT_TEXT = "按下Shift键，可以反转\"吸附\"模式。";

        float _distance;
        float _xOffset;
        float _yOffset;

        Curve _curve;

		int _iHighlightedSegmentIndex;

		int _iSelectedSegmentIndex;

		Endpoint _mouseEndpoint;

		public InsertPointHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{
            _curve = null;
			_iHighlightedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;
			_iSelectedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;

            _distance = Math.Min(epoxyPatternControl.EndpointRectWidth / 2, epoxyPatternControl.EndpointRectHeight / 2);
            _xOffset = epoxyPatternControl.EndpointRectWidth / 2;
			_yOffset = epoxyPatternControl.EndpointRectHeight / 2;
		}

		public override void SetMode()
		{
            _curve = EpoxyPatternControl.GetCurrentCurve();
			_iHighlightedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;
			_iSelectedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;

			EpoxyPatternControl.ClearHighlightedEndpoint();
			EpoxyPatternControl.ClearHighlightedSegment();
		}

        public override void HandleExitActionRequest(EventArgs eventArgs)
        {
            base.HandleExitActionRequest(eventArgs);

			if (_iSelectedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
			{
				_iSelectedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;						

				EpoxyPatternControl.Cursor = Cursors.Arrow;
				EpoxyPatternControl.Invalidate();
			}

            _curve = null;
			EpoxyPatternControl.ResetAction();
        }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        { }

        public override void HandleScaleChanged(ScaleChangedEventArgs eventArgs)
        { }

        public override void HandlePaintEvent(Graphics graphics)
		{
			//EpoxyPatternControl.ShowAction(graphics, "InsertingPoint");

            Debug.Assert(_curve != null);

			if (_iSelectedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
			{
				EpoxyPatternControl.PaintCoordinateLines(graphics, _mouseEndpoint);
				PaintSplitLineSegments(graphics, _curve, _iSelectedSegmentIndex, _mouseEndpoint);
			}

			if (_iHighlightedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
			{
				EpoxyPatternControl.HighlightLineSegment(graphics, _curve, _iHighlightedSegmentIndex);
			}

            if (true)
            {
                DrawHintText(graphics, HINT_TEXT);
            }
		}

        /// <summary>
        /// 画曲线上的分割后的线段。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="iSegmentIndex"></param>
        /// <param name="midPoint"></param>
        internal void PaintSplitLineSegments(Graphics graphics, Curve curve, int iSegmentIndex, Endpoint midPoint)
        {
            if (graphics == null)
                throw new ArgumentNullException(nameof(graphics));

            if (curve == null)
                throw new ArgumentNullException(nameof(curve));

            curve.PaintSplitLineSegments(graphics, iSegmentIndex, midPoint, DashPen, HighlightPaintingTool.Brush);
        }

        public override void HandleMouseDownEvent(MouseEventArgs args)
		{
			Point2D mousePixelPoint = (Point2D)args.Location;

			if (args.Button == MouseButtons.Left)
			{
				Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);				

                Debug.Assert(_curve != null);

				if (_iSelectedSegmentIndex == EpoxyPatternControl.INVALID_SEGMENT_INDEX)
				{						
                    int iSegmentInitialEndpointIndex;
					if (_curve.ContainsSegment(mousePixelPoint, _distance, out iSegmentInitialEndpointIndex))
					{
						_iHighlightedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;
						_iSelectedSegmentIndex = iSegmentInitialEndpointIndex;

                        //curve.HideSegment(_iSelectedSegmentIndex);
                        _mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);

						EpoxyPatternControl.Cursor = Cursors.Cross;
						EpoxyPatternControl.Invalidate();
					}
				}
                else
                {
                    // Insert a new point.

                    Endpoint mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);
                    if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
                    {
                        Endpoint endpoint;
                        int iEndpointIndex;
                        if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                        {
                            mouseEndpoint = endpoint;
                        }
                    }

					int iSelectedSegmentDstIndex = _iSelectedSegmentIndex + 1;                    
					_curve.InsertPoint(iSelectedSegmentDstIndex, mouseEndpoint, EpoxyPatternControl.DefaultEndPointInfo);
			
					_iSelectedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;
					_iHighlightedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;

					EpoxyPatternControl.Cursor = Cursors.Arrow;
                    EpoxyPatternControl.OnEndpointInserted(new EndpointInsertedEventArgs(_curve, iSelectedSegmentDstIndex, mouseLogicalPoint));
					EpoxyPatternControl.Invalidate();                    
                }
			}
			else if (args.Button == MouseButtons.Right)
			{
				// Clear the selected segment.
				if (_iSelectedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
				{
					_iSelectedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;						

					EpoxyPatternControl.Cursor = Cursors.Arrow;
					EpoxyPatternControl.Invalidate();
				}
			}
		}

		public override void HandleMouseMoveEvent(MouseEventArgs args)
		{
			Point2D mousePixelPoint = (Point2D)args.Location;

            Debug.Assert(_curve != null);

			if (_iSelectedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
			{
				Point2D mouseLogicalPoint = EpoxyPatternControl.ConvertToLogicalPoint(mousePixelPoint);
				Endpoint mouseEndpoint = EpoxyPatternControl.CreateEndpoint(mouseLogicalPoint);

				if ((Attachable && Control.ModifierKeys != Keys.Shift) || (!Attachable && Control.ModifierKeys == Keys.Shift))
                {
					Endpoint endpoint;
					int iEndpointIndex;
					if (_curve.ContainsEndpoint(mouseLogicalPoint, _xOffset, _yOffset, out endpoint, out iEndpointIndex))
                    {
						mouseEndpoint = endpoint;
                    }
                }
				
				if (mouseEndpoint.LogicalPoint != _mouseEndpoint.LogicalPoint)
				{
					_mouseEndpoint = mouseEndpoint;
					EpoxyPatternControl.Invalidate();
				}
			}
			else
			{
				int iSegmentInitialEndpointIndex;
				
				if (_curve.ContainsSegment(mousePixelPoint, _distance, out iSegmentInitialEndpointIndex))
				{
					_iHighlightedSegmentIndex = iSegmentInitialEndpointIndex;

					EpoxyPatternControl.Invalidate();
				}
				else
				{
					if (_iHighlightedSegmentIndex != EpoxyPatternControl.INVALID_SEGMENT_INDEX)
					{
						_iHighlightedSegmentIndex = EpoxyPatternControl.INVALID_SEGMENT_INDEX;
						EpoxyPatternControl.Invalidate();
					}
				}
			}
		}

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{ }
	}
}
