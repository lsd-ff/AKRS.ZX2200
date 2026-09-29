using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

namespace AKRS.ZX2200.BondSystem.Services
{
    using TransportUnit = AKRS.ZX2200.TransportUnitSystem.Module.Matter.TransportUnit;

    /// <summary>
    /// 系统2流道帮助类
    /// </summary>
    public static class System2TUService
    {
        /// <summary>
        /// 获取基板坐标(g0坐标系)
        /// </summary>
        /// <param name="index">基板索引</param>
        /// <returns>基岛坐标</returns>
        public static AKRSPoint3D GetSubstratePos(int index)
        {
           TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            if (transportUnit.GetSubstrate(index) == null)
            {
                return null;
            }

            AKRSPoint3D g0Pos = transportUnit.GetSubstrate(index).CoordinateSystem
                .SelfPosToG0(new AKRSPoint3D(0, 0, 0));

            // 计算视觉位
            AKRSPoint3D visionPos = new AKRSPoint3D()
            {
                X = g0Pos.X - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X,
                Y = g0Pos.Y - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y,
                Z = g0Pos.Z + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Z
            };

            return System2Module.GetInstance().BondModule.ConvertG0ToMachinePos(visionPos);
        }

        /// <summary>
        /// 获取基岛坐标(g0坐标系)
        /// </summary>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <returns>基岛坐标</returns>
        public static AKRSPoint3D GetModulePos(int substrateIndex, int moduleIndex)
        {
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            // 临时添加
            if (transportUnit.GetModule(substrateIndex, moduleIndex) == null)
            {
                return null;
            }

            AKRSPoint3D g0Pos = transportUnit.GetModule(substrateIndex, moduleIndex).CoordinateSystem
                .SelfPosToG0(new AKRSPoint3D(0, 0, 0));

            // 计算视觉位
            AKRSPoint3D visionPos = new AKRSPoint3D()
            {
                X = g0Pos.X - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X,
                Y = g0Pos.Y - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y,
                Z = g0Pos.Z + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Z
            };

            return visionPos;
        }

        /// <summary>
        /// 获取焊点示教视觉坐标(Bond坐标系)
        /// </summary>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <param name="bondPositionName">焊点名</param>
        /// <returns>焊点坐标</returns>
        public static AKRSPoint3D GetBondPositionVisionPos(int substrateIndex, int moduleIndex, string bondPositionName)
        {
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            if (transportUnit.GetModule(substrateIndex, moduleIndex) == null)
            {
                return null;
            }

            // 获取焊点所在基岛
            Module module = transportUnit.GetModule(substrateIndex, moduleIndex);

            if (transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName) == null)
            {
                return null;
            }

            // 获取焊点
            BondPosition bp = transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName);

            // 获取焊点相对基岛的坐标
            AKRSPoint3D bondPoint3D = transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName)
                .SingleBondPositionConfig.ElementCoordinate.Point;

            // 焊点相对基岛的坐标转成G0
            AKRSPoint3D bondToG0Point3D = module.CoordinateSystem.SelfPosToG0(bondPoint3D);

            // 再转到Bond坐标系
            AKRSPoint3D g0ToBondPos = System2Module.GetInstance().BondModule.ConvertG0ToMachinePos(bondToG0Point3D);

            // 最后转到视觉位
            AKRSPoint3D visionPos = g0ToBondPos - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

            return visionPos;
        }

        /// <summary>
        /// 获取焊点示教绝对贴片坐标(Bond坐标系,标准吸嘴)
        /// </summary>
        /// <param name="substrateIndex">基板索引</param>
        /// <param name="moduleIndex">基岛索引</param>
        /// <param name="bondPositionName">焊点名</param>
        /// <returns>焊点坐标</returns>
        public static AKRSPoint3D GetBondPositionTeachPos(int substrateIndex, int moduleIndex, string bondPositionName)
        {
            TransportUnit transportUnit =
                TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            if (transportUnit.GetModule(substrateIndex, moduleIndex) == null)
            {
                return null;
            }

            // 获取焊点所在基岛
            Module module = transportUnit.GetModule(substrateIndex, moduleIndex);

            if (transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName) == null)
            {
                return null;
            }

            // 获取焊点
            BondPosition bp = transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName);

            // 获取焊点相对基岛的坐标
            AKRSPoint3D bondPoint3D = transportUnit.GetBondPosition(substrateIndex, moduleIndex, bondPositionName)
                .SingleBondPositionConfig.ElementCoordinate.Point;

            // 焊点相对基岛的坐标转成G0
            AKRSPoint3D bondToG0Point3D = module.CoordinateSystem.SelfPosToG0(bondPoint3D);

            // 再转到Bond坐标系
            AKRSPoint3D g0ToBondPos = System2Module.GetInstance().BondModule.ConvertG0ToMachinePos(bondToG0Point3D);

            return g0ToBondPos;
        }
    }
}
