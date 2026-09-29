using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class MousePointChangedEventArgs : EventArgs
    {    
        readonly Point2D _mouseLogicalPoint;
        readonly Point2D _mousePixelPoint;

        public MousePointChangedEventArgs(Point2D mouseLogicalPoint, Point2D mousePixelPoint)
		{
			_mouseLogicalPoint = mouseLogicalPoint;
            _mousePixelPoint = mousePixelPoint;
		}

        public Point2D MouseLogicalPoint
        {
            get { return _mouseLogicalPoint; }
        }

        public Point2D MousePixelPoint
        {
            get { return _mousePixelPoint; }
        }
    }
}
