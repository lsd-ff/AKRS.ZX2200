using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace AKRS.Galaxy2.Dispense
{
	public class ActionChangedEventArgs : EventArgs
	{
        ActionEnum oldActionEnum;
        ActionEnum newActionEnum;

        public ActionChangedEventArgs(ActionEnum oldActionEnum, ActionEnum newActionEnum)
		{
            Debug.Assert(oldActionEnum != newActionEnum);

            this.oldActionEnum = oldActionEnum;
            this.newActionEnum = newActionEnum;
		}
        
        public ActionEnum OldActionEnum
        {
            get { return this.oldActionEnum; }
        }

        public ActionEnum NewActionEnum
        {
            get { return this.newActionEnum; }
        }
    }
}
