using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class EndpointAppendedEventArgs : EndpointEventArgs
    {
        public EndpointAppendedEventArgs(Curve curve, int iEndpointIndex, Point2D logicalPoint) :
            base(curve, iEndpointIndex, logicalPoint)
        { }
    }
}
