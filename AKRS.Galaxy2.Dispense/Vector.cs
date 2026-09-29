using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    struct Vector2D
    {
        readonly Point2D _initialPoint;
        readonly Point2D _terminalPoint;

        public Vector2D(Point2D initialPoint, Point2D terminalPoint)
        {
            _initialPoint = initialPoint;
            _terminalPoint = terminalPoint;
        }

        public Point2D InitialPoint
        {
            get { return _initialPoint; }
        }

        public Point2D TerminalPoint
        {
            get { return _terminalPoint; }
        }

        public Vector2D Translate(float xIncrement, float yIncrement)
        {
            return new Vector2D(_initialPoint.Translate(xIncrement, yIncrement), _terminalPoint.Translate(xIncrement, yIncrement));
        }
    }
}
