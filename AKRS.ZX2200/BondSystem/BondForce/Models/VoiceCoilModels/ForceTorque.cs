namespace AKRS.ZX2200.BondSystem.BondForce.Models.VoiceCoilModels
{
    /// <summary>
    /// 力和转矩
    /// </summary>
    public class ForceTorque
    {
        /// <summary>
        /// 力值(g)
        /// </summary>
        public double Force { get; set; }

        /// <summary>
        /// 转矩值
        /// </summary>
        public int Torque { get; set; }
    }
}
