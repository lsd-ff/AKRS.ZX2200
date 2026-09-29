using System;

namespace AKRS.Galaxy2.Dispense
{
    /// <summary>
    /// 点和矩形的关系。
    /// </summary>
    [Flags]
    enum PointAndRectangleRelation : int
    {
        /// <summary>
        /// 未知。
        /// </summary>
        Unkown = 0,

        /// <summary>
        /// 点在矩形的上侧边缘。
        /// </summary>
        /// <remarks>[0001]</remarks>
        TopEdge = 0x01,

        /// <summary>
        /// 点在矩形的下侧边缘。
        /// </summary>
        /// <remarks>[0010]</remarks>
        BottomEdge = 0x02,

        /// <summary>
        /// 点在矩形的左侧边缘。
        /// </summary>
        /// <remarks>[0100]</remarks>
        LeftEdge = 0x04,

        /// <summary>
        /// 点在矩形的右侧边缘。
        /// </summary>
        /// <remarks>[1000]</remarks>
        RightEdge = 0x08,

        /// <summary>
        /// 水平边缘。
        /// </summary>
        /// <remarks>[0011]</remarks>
        HorizontalEdges = 0x03,

        /// <summary>
        /// 垂直边缘。
        /// </summary>
        /// <remarks>[1100]</remarks>
        VerticalEdges = 0x0C,

        /// <summary>
        /// 点在矩形的左上角。
        /// </summary>
        /// <remarks>[0101]</remarks>
        TopLeftCorner = 0x05,

        /// <summary>
        /// 点在矩形的右上角。
        /// </summary>
        /// <remarks>[1001]</remarks>
        TopRightCorner = 0x09,

        /// <summary>
        /// 点在矩形的左下角。
        /// </summary>
        /// <remarks>[0110]</remarks>
        BottomLeftCorner = 0x06,

        /// <summary>
        /// 点在矩形的右下角。
        /// </summary>
        /// <remarks>[1010]</remarks>
        BottomRightCorner = 0x0A,
    }
}
