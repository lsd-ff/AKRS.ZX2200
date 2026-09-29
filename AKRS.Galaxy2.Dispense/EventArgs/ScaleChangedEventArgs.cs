using System;

namespace AKRS.Galaxy2.Dispense
{
    public class ScaleChangedEventArgs : EventArgs
    {
        readonly float _oldValue;
        readonly float _newValue;

        public ScaleChangedEventArgs(float oldValue, float newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public float OldScaleValue
        {
            get { return _oldValue; }
        }

        public float NewScaleValue
        {
            get { return _newValue; }
        }
    }
}
