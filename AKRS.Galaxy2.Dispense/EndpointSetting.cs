namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 端点信息。
    /// </summary>
    public struct EndpointSetting
	{
        /// <summary>
        /// 最大速度，即目标速度。
        /// </summary>
        public int MaxSpeed { get; set; }

        /// <summary>
        /// 结束速度，即运动到线段的终点时的速度。
        /// </summary>
        public int EndSpeed { get; set; }

        /// <summary>
        /// 加速度，或加速的时间，或加速的百分比，根据控制卡而定。
        /// </summary>
        public double Acc { get; set; }

        /// <summary>
        /// 线段表示的路径的行走耗时。
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// 点胶前延迟时间
        /// </summary>
        public int PreDelay { get; set; }

        /// <summary>
        /// 点胶后延迟时间
        /// </summary>
        public int PostDelay { get; set; }

        /// <summary>
        /// 高度补偿。
        /// </summary>
        public int AltitudeCompensation;

        /// <summary>
        /// dummied
        /// </summary>
        private EndpointFlags flags;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxSpeed">最大速度</param>
        /// <param name="endSpeed">结束速度</param>
        /// <param name="acc">加速度</param>
        /// <param name="altitudeCompensation">高度补偿</param>
        /// <param name="time">线段表示的路径的行走耗时</param>
        /// <param name="dummied">dummied</param>
        /// <param name="preDelay">点胶前延迟时间</param>
        /// <param name="postDelay">点胶后延迟时间</param>
		public EndpointSetting(int maxSpeed, int endSpeed, double acc, int altitudeCompensation, int time, bool dummied, int preDelay, int postDelay)
		{
			this.MaxSpeed = maxSpeed;
			this.EndSpeed = endSpeed;
			this.Acc = acc;
			this.AltitudeCompensation = altitudeCompensation;
			this.Time = time;

			this.PreDelay = preDelay;
			this.PostDelay = postDelay;

            this.flags = EndpointFlags.AltitudeCompensation;

            if (dummied)
            {
                this.flags |= EndpointFlags.Dummied;
            }
        }

        /// <summary>
        /// 获取或设置测高标志。
        /// </summary>
        public bool HeightMeasurement
        {
            get => (this.flags & EndpointFlags.HeightMeasurement) != 0;

            set
            {
                if (value)
                {
                    this.flags |= EndpointFlags.HeightMeasurement;
                }
                else
                {
                    this.flags &= ~EndpointFlags.HeightMeasurement;
                }
            }
        }

        /// <summary>
        /// 获取或设置空过标志。
        /// </summary>
        public bool Dummied
        {
            get => (this.flags & EndpointFlags.Dummied) != 0;

            set
            {
                if (value)
                {
                    this.flags |= EndpointFlags.Dummied;
                }
                else
                {
                    this.flags &= ~EndpointFlags.Dummied;
                }
            }
        }
    }
}
