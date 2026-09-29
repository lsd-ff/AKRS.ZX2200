using System;
using System.Collections.Generic;
using System.Drawing;

namespace AKRS.Galaxy2.Dispense
{
    public abstract class EndpointEventArgs : EventArgs
	{
        readonly Curve _curve;
        readonly int _endpointIndex;
        readonly Point2D _endpointLogicalPoint;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve">端点所属的曲线。</param>
        /// <param name="endpointIndex">端点的Index。</param>
        /// <param name="endpointLogicalPoint">端点的逻辑位置。</param>
		public EndpointEventArgs(Curve curve, int endpointIndex, Point2D endpointLogicalPoint)
		{
            if (curve == null)
                throw new ArgumentNullException(nameof(curve));

            _curve = curve;
			_endpointIndex = endpointIndex;
			_endpointLogicalPoint = endpointLogicalPoint;
		}

        /// <summary>
        /// 端点所属的曲线。
        /// </summary>
        public Curve Curve
        {
            get { return _curve; }
        }

        /// <summary>
        /// 端点的Index。
        /// </summary>
        public int EndpointIndex
        {
            get { return _endpointIndex; }
        }

        /// <summary>
        /// 端点的逻辑坐标。
        /// </summary>
        public Point2D EndpointLogicalPoint
        {
            get { return _endpointLogicalPoint; }
        }
    }
}
