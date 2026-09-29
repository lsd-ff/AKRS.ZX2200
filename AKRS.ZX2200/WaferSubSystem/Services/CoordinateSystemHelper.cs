using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.WaferSubSystem.Services
{
    /// <summary>
    /// 坐标系统帮助类
    /// </summary>
    public static class CoordinateSystemHelper
    {
        /// <summary>
        /// 将物理X轴坐标转换为G0坐标
        /// </summary>
        /// <param name="g">g</param>
        /// <param name="axisPos">axisPos</param>
        /// <returns>AxisG0</returns>
        public static AKRSPoint3D ConvertMachineXToG0Pos(this GeneralCoordinateSystem g, double axisPos)
        {
            return g.SelfPosToG0(new AKRSPoint3D(axisPos, 0, 0));
        }

        /// <summary>
        /// 将物理Y轴坐标转换为G0坐标
        /// </summary>
        /// <param name="g">g</param>
        /// <param name="axisPos">axisPos</param>
        /// <returns>AxisG0</returns>
        public static AKRSPoint3D ConvertMachineYToG0Pos(this GeneralCoordinateSystem g, double axisPos)
        {
            return g.SelfPosToG0(new AKRSPoint3D(0, axisPos, 0));
        }

        /// <summary>
        /// 将物理Z轴坐标转换为G0坐标
        /// </summary>
        /// <param name="g">g</param>
        /// <param name="axisPos">axisPos</param>
        /// <returns>AxisG0</returns>
        public static AKRSPoint3D ConvertMachineZToG0Pos(this GeneralCoordinateSystem g, double axisPos)
        {
            return g.SelfPosToG0(new AKRSPoint3D(0, 0, axisPos));
        }
    }
}
