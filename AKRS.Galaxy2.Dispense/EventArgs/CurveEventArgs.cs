using System;

namespace AKRS.Galaxy2.Dispense
{
    public abstract class CurveEventArgs : EventArgs
    {
        readonly int _iCurveIndex;
        readonly Curve _curve;

        public CurveEventArgs(int iCurveIndex, Curve curve)
        {
            if (curve == null)
                throw new ArgumentNullException(nameof(curve));

            _iCurveIndex = iCurveIndex;
            _curve = curve;
        }

        public int CurveIndex
        {
            get { return _iCurveIndex; }
        }

        public Curve Curve
        {
            get { return _curve; }
        }
    }
}
