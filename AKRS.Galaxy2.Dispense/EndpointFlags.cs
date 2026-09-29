using System;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 结束点动作标志
    /// </summary>
    [Flags]
    public enum EndpointFlags : uint
    {
        /// <summary>
        /// 高度补偿。
        /// </summary>
        /// <remarks>[0000 0001]</remarks>
        AltitudeCompensation = 0x0001,

        /// <summary>
        /// 空过。
        /// </summary>
        /// <remarks>[0000 0010]</remarks>
        Dummied = 0x0002,

        /// <summary>
        /// 测高。
        /// </summary>
        /// <remarks>[0000 0100]</remarks>
        HeightMeasurement = 0x0004,
    }

}
