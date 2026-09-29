namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 点
    /// </summary>
    public class EndpointUnit
    {
        /// <summary>
        /// 点的坐标数据。
        /// </summary>
        public Endpoint Endpoint { get; set; }

        /// <summary>
        /// 包含点的其他参数如速度、延时。
        /// </summary>
        public EndpointSetting EndpointInfo { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="endpoint">点的坐标数据</param>
        /// <param name="endpointInfo">包含点的其他参数如速度、延时</param>
        public EndpointUnit(Endpoint endpoint, EndpointSetting endpointInfo)
        {
            Endpoint = endpoint;
            EndpointInfo = endpointInfo;
        }
    }
}
