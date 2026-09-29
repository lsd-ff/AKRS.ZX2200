using System.Drawing;
using System.Text;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
	/// 点坐标和点的其他参数信息
	/// </summary>
    public struct EPEPointData  
	{
        /// <summary>
        /// 点逻辑坐标 
        /// </summary>
        private Point2D point;

        /// <summary>
        /// 点参数(工艺参数)
        /// </summary>
        private EndpointSetting info;

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="x">x</param>
		/// <param name="y">y</param>
		/// <param name="info">点配置信息</param>
		public EPEPointData(float x, float y, EndpointSetting info)
            : this(new Point2D(x, y), info)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="point">点胶点</param>
        /// <param name="info">点配置信息</param>
        public EPEPointData(Point2D point, EndpointSetting info)
		{
			this.point = point;
		    this.info = info;
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="other">EPEPoint</param>
        public EPEPointData(EPEPointData other)
		{
			this.point = other.point;
			this.info = other.info;
		}

		/// <summary>
		/// X
		/// </summary>
        public double X
        {
            get => this.point.X;
            set => this.point.X = (float)value;
        }

        /// <summary>
        /// Y
        /// </summary>
        public double Y => this.point.Y;

        /// <summary>
        /// 返回Point
        /// </summary>
        public Point2D Point
        {
            get => this.point;
            set => this.point = value;
        }

        /// <summary>
        /// Info
        /// </summary>
        public EndpointSetting Info => this.info;

        /// <summary>
        /// 重载ToString能改变调试模式时鼠标悬停在变量上时显示的数据
        /// </summary>
        /// <returns>格式化字符串</returns>
        public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

            // 这里显示的格式就是form1里上面那个输入框里显示的格式
            sb.AppendFormat("(X={0}, Y={1}", this.point.X, this.point.Y);

			sb.AppendFormat(", AltitudeCompensation={0}", this.info.AltitudeCompensation);
			sb.AppendFormat(", MaxSpeed={0}", this.info.MaxSpeed);
			sb.AppendFormat(", endSpeed={0}", this.info.EndSpeed);
			sb.AppendFormat(", Acc={0}", this.info.Acc);
			sb.AppendFormat(", Time={0}", this.info.Time);
			sb.AppendFormat(", dummied={0}", this.info.Dummied);
            sb.AppendFormat(", heightMeasurement={0}", this.info.HeightMeasurement);
			sb.AppendFormat(", preDelay={0}, postDelay={1}", this.info.PreDelay, this.info.PostDelay);

			sb.Append(")");

			return sb.ToString();
		}

	}
}