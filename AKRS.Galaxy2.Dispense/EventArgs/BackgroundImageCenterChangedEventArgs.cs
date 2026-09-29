using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    class BackgroundImageCenterChangedEventArgs : EventArgs
    {
        readonly Point2D _oldValue;
        readonly Point2D _newValue;

        public BackgroundImageCenterChangedEventArgs(Point2D oldValue, Point2D newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public Point2D OldValue
        {
            get { return _oldValue; }
        }

        public Point2D NewValue
        {
            get { return _newValue; }
        }
    }
}
