using System;
using System.Drawing;

namespace AKRS.Galaxy2.Dispense
{
    public class DieSizeChangedEventArgs : EventArgs
    {
        readonly SizeF _newSize;
        readonly SizeF _oldSize;

        public DieSizeChangedEventArgs(SizeF newSize, SizeF oldSize)
        {
            _newSize = newSize;
            _oldSize = oldSize;
        }

        public SizeF NewSize
        {
            get { return _newSize; }
        }

        public SizeF OldSize
        {
            get { return _oldSize; }
        }
    }
}
