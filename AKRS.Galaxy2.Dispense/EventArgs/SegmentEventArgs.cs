using System;
using System.Collections.Generic;

namespace AKRS.Galaxy2.Dispense
{
	public class SegmentEventArgs : EventArgs
	{
		public SegmentEventArgs(int iSrcIndex, int iDstIndex)
		{
			this.SrcIndex = iSrcIndex;
			this.DstIndex = iDstIndex;

			this.SrcPoint = new EPEPointData();
			this.DstPoint = new EPEPointData();
		}

		public SegmentEventArgs(int iOrgSrcIndex, EPEPointData orgSrcData, int iOrgDstIndex, EPEPointData orgDstData)
		{
			this.SrcIndex = iOrgSrcIndex;
			this.DstIndex = iOrgDstIndex;
			this.SrcPoint = orgSrcData;
			this.DstPoint = orgDstData;
		}


		public int SrcIndex
		{
			get;
			private set;
		}

		public int DstIndex
		{
			get;
			private set;
		}

		public EPEPointData SrcPoint
		{
			get;
			private set;
		}

		public EPEPointData DstPoint
		{
			get;
			private set;
		}
	}
}
