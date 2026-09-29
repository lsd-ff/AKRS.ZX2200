using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
	class NullHandler : ActionHandler
	{
		public NullHandler(EpoxyPatternControl epoxyPatternControl)
			: base(epoxyPatternControl)
		{ }

		public override void SetMode()
		{

        }

		public override void HandleExitActionRequest(EventArgs args)
		{ }

        public override void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs)
        {
        }

        public override void HandlePaintEvent(Graphics g)
		{ }

        public override void HandleScaleChanged(ScaleChangedEventArgs args)
        { }

        public override void HandleMouseDownEvent(MouseEventArgs args)
		{ }

		public override void HandleMouseMoveEvent(MouseEventArgs args)
		{ }

		public override void HandleMouseUpEvent(MouseEventArgs args)
		{ }

		public override void HandleMouseEnterEvent(EventArgs args)
		{ }

		public override void HandleMouseLeaveEvent(EventArgs args)
		{ }
	}
}
