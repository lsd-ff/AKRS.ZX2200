using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace AKRS.Galaxy2.Dispense
{
	public class PaintingToolSet
	{
		Pen _pen;
		SolidBrush _brush;

		Pen _dummiedPen;

		public PaintingToolSet(Color color)
		{
			_pen = new Pen(color);
			_brush = new SolidBrush(color);

			_dummiedPen = new Pen(color);
			_dummiedPen.DashStyle = DashStyle.Dash;
			_dummiedPen.DashPattern = new float[] { 3, 3 };
		}

		public PaintingToolSet(Color color, int iPenWidth)
			: this(color)
		{
			SetPenWidth(iPenWidth);
		}

		public void SetColor(Color color)
		{
			_pen.Color = color;
			_brush.Color = color;
			_dummiedPen.Color = color;
		}

		public void SetPenWidth(int iPenWidth)
		{
			_pen.Width = iPenWidth;
			_dummiedPen.Width = iPenWidth;
		}

		public Pen Pen
		{
			get { return _pen; }
		}

		public Brush Brush
		{
			get { return _brush; }
		}

		public Pen DummiedPen
		{
			get { return _dummiedPen; }
		}
	}
}