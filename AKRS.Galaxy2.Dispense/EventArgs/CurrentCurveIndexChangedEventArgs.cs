using System;

namespace AKRS.Galaxy2.Dispense
{
    public class CurrentCurveIndexChangedEventArgs : EventArgs
    {
        readonly int _iOldIndex;
        readonly int _iNewIndex;

        public CurrentCurveIndexChangedEventArgs(int iOldIndex, int iNewIndex)
        {
            _iOldIndex = iOldIndex;
            _iNewIndex = iNewIndex;
        }

        public int OldIndex
        {
            get { return _iOldIndex; }
        }

        public int NewIndex
        {
            get { return _iNewIndex; }
        }
    }
}
