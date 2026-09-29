using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class EndpointChangedEventArgs : EndpointEventArgs
    {
        public EndpointChangedEventArgs(Curve curve, int iEndpointIndex, Point2D logicalPoint)
            : base(curve, iEndpointIndex, logicalPoint)
        { }
    }
}
