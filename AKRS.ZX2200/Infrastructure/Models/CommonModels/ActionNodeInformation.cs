namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    /// ActionNode信息类
    /// </summary>
    public class ActionNodeInformation
    {
        /// <summary>
        /// 传输单元
        /// </summary>
        public int TransportUnit { get; set; }

        /// <summary>
        /// 基板
        /// </summary>
        public int Substrate { get; set; }

        /// <summary>
        /// 基岛
        /// </summary>
        public int Module { get; set; }

        /// <summary>
        /// 焊点
        /// </summary>
        public int BondPosition { get; set; }
    }
}
