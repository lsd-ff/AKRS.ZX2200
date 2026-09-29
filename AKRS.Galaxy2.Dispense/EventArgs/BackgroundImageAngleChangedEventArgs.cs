using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    class BackgroundImageAngleChangedEventArgs : EventArgs
    {
        readonly float _oldValue;
        readonly float _newValue;

        public BackgroundImageAngleChangedEventArgs(float oldValue, float newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public float OldValue
        {
            get { return _oldValue; }
        }

        public float NewValue
        {
            get { return _newValue; }
        }
    }
}
