using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    
    public struct Point2D
    {
        public static readonly Point2D Empty;

        static Point2D()
        {
            Point2D.Empty = new Point2D(0, 0);
        }

        public static explicit operator Point2D(Point point)
        {
            return new Point2D(point.X, point.Y);
        }

        public static explicit operator Point2D(PointF point)
        {
            return new Point2D(point.X, point.Y);
        }

        public static explicit operator PointF(Point2D point2D)
        {
            return new PointF(point2D.X, point2D.Y);
        }

        public static explicit operator Point(Point2D point2D)
        {
            return new Point((int)point2D.X, (int)point2D.Y);
        }

        public static bool operator ==(Point2D point1, Point2D point2)
        {
            return (point1._x == point2._x) && (point1._y == point2._y);
        }

        public static bool operator !=(Point2D point1, Point2D point2)
        {
            return !(point1 == point2);
        }


        //---------------------------------------------------------------------
        //---------------------------------------------------------------------
        //---------------------------------------------------------------------

        float _x;
        float _y;

        public Point2D(float x, float y)
        {
            _x = x;
            _y = y;
        }

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }

        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(Point2D))
                return false;

            return this == (Point2D)obj;
        }

        public override int GetHashCode()
        {
            return (int)(_x + _y);
        }

        public override string ToString()
        {
            return String.Format($"({_x},{_y})");
        }

        public Point2D Translate(float xIncrement, float yIncrement)
        {
            return new Point2D(_x + xIncrement, _y + yIncrement);
        }
    }
}
