using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Calibration
{
    /// <summary>
    /// 定位类型
    /// </summary>
    public enum LocateTypeEnum
    {
        /// <summary>
        /// p1
        /// </summary>
        P1,

        /// <summary>
        /// p1p2
        /// </summary>
        P1P2定位,

        /// <summary>
        /// p1p2仿射
        /// </summary>
        P1P2仿射,

        /// <summary>
        /// P1P2P3仿射缩放
        /// </summary>
        P1P2P3仿射缩放,

        /// <summary>
        /// P1偏移
        /// </summary>
        P1偏移,

        /// <summary>
        /// P1P2中点偏移
        /// </summary>
        P1P2中点偏移,
    }

    /// <summary>
    /// 获取角度方法枚举
    /// </summary>
    public enum GetAngleEnum
    {
        /// <summary>
        /// 两点连线
        /// </summary>
        两点连线,

        /// <summary>
        /// 多点拟合
        /// </summary>
        多点拟合
    }

}
