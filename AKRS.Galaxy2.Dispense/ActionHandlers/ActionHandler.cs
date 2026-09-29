using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AKRS.Galaxy2.Dispense
{
	abstract class ActionHandler
	{
		readonly EpoxyPatternControl _epoxyPatternControl;

        /// <summary>
        /// 提示文本的字体。
        /// </summary>
        Font _hintTextFont;

        /// <summary>
        /// 提示文本的画刷。
        /// </summary>
        Brush _hintTextBrush;

        bool _bAttachable;

        /// <summary>
        /// 虚线的画笔。
        /// </summary>
        Pen _dashPen;

        /// <summary>
        /// 高亮时用的画图工具。
        /// </summary>
        PaintingToolSet _highlightPaintingTool;


        public bool Attachable
        {
            get { return _bAttachable; }
            set { _bAttachable = value; }
        }

        protected Font HintTextFont
        {
            get { return _hintTextFont; }
        }

        protected Brush HintTextBrush
        {
            get { return _hintTextBrush; }
        }

        protected Pen DashPen
        {
            get { return _dashPen; }
        }

        public PaintingToolSet HighlightPaintingTool
        {
            get { return _highlightPaintingTool; }
        }

		public ActionHandler(EpoxyPatternControl epoxyPatternControl)
		{
			Debug.Assert(epoxyPatternControl != null);

			//epoxyPatternControl.ScaleChanged += epoxyPatternControl_ScaleChanged;

			_epoxyPatternControl = epoxyPatternControl;

            _hintTextFont = EpoxyPatternControl.Font;
            _hintTextBrush = new SolidBrush(Color.Blue);

            _dashPen = new Pen(Color.Red);
            _dashPen.DashStyle = DashStyle.Dash;
            _dashPen.DashPattern = new float[] { 10, 3 };

            _highlightPaintingTool = new PaintingToolSet(Color.Red, 3);
		}

		//private void epoxyPatternControl_ScaleChanged(object sender, EventArgs e)
		//{
		//	ActionEnum<EventArgs> handler = HandleScaleChanged;
		//	if (handler != null)
		//		handler.Invoke(e);
		//}

        public virtual void SetMode()
		{
			_epoxyPatternControl.Focus();

            _bAttachable = false;
		}

        public abstract void HandleLogicalCoordinateSystemOriginChanged(LogicalCoordinateSystemOriginChangedEventArgs eventArgs);
        public abstract void HandleScaleChanged(ScaleChangedEventArgs eventArgs);
		public abstract void HandlePaintEvent(Graphics graphics);

        public virtual void HandleExitActionRequest(EventArgs eventArgs)
        {
			EpoxyPatternControl.ResetAction();
        }

		public abstract void HandleMouseDownEvent(MouseEventArgs eventArgs);
		public abstract void HandleMouseMoveEvent(MouseEventArgs eventArgs);
		public abstract void HandleMouseUpEvent(MouseEventArgs eventArgs);
		public virtual void HandleMouseEnterEvent(EventArgs eventArgs)
		{ }
		public virtual void HandleMouseLeaveEvent(EventArgs eventArgs)
		{ }

		public virtual void HandleKeyDownEvent(KeyEventArgs eventArgs)
		{ }

		public virtual void HandleKeyUpEvent(KeyEventArgs eventArgs)
		{ }

		public virtual void HandleKeyPressEvent(KeyPressEventArgs eventArgs)
		{ }
        

		protected EpoxyPatternControl EpoxyPatternControl
		{
			get { return _epoxyPatternControl; }
		}


        protected void DrawHintText(Graphics graphics, string strHintText)
        {
            if (strHintText != null)
            {
                SizeF size = graphics.MeasureString(strHintText, HintTextFont);
                RectangleF layoutRectangle = new RectangleF(EpoxyPatternControl.Width / 2 - size.Width / 2, EpoxyPatternControl.Height - size.Height, size.Width, size.Height);
                graphics.DrawString(strHintText, HintTextFont, HintTextBrush, layoutRectangle);
            }
        }

        /// <summary>
        /// 画末端有实心箭头的虚线段。
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="initialPixelPoint"></param>
        /// <param name="terminalPixelPoint"></param>
        internal void FillArrowDashLineSegment(Graphics graphics, Point2D initialPixelPoint, Point2D terminalPixelPoint)
        {
            GraphicsState state = graphics.Save();
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Utility.FillVector(graphics, initialPixelPoint, terminalPixelPoint, _dashPen, _highlightPaintingTool.Brush);

            graphics.Restore(state);
        }

	}
}
