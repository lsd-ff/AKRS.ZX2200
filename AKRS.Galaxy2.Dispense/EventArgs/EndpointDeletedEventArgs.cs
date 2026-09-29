using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class EndpointDeletedEventArgs : EndpointEventArgs
    {
        public EndpointDeletedEventArgs(Curve curve, int iEndpointIndex, Point2D logicalPoint)
            : base(curve, iEndpointIndex, logicalPoint)
        { }
    }
}
