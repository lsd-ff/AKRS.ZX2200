using System;

namespace AKRS.Galaxy2.Dispense
{
    public class LogicalCoordinateSystemOriginChangedEventArgs : EventArgs
    {
        readonly Point2D _oldValue;
        readonly Point2D _newValue;

        public LogicalCoordinateSystemOriginChangedEventArgs(Point2D oldValue, Point2D newValue)
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
