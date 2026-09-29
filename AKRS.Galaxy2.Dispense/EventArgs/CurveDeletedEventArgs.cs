using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    public class CurveDeletedEventArgs : CurveEventArgs
    {
        public CurveDeletedEventArgs(int iCurveIndex, Curve curve)
            : base(iCurveIndex, curve)
        { }
    }
}
