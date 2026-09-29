using AKRS.Galaxy2.Infrastructure.CommonModel;
using System;
using System.Collections.Generic;

namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    /// <summary>
    /// 依赖坐标系
    /// </summary>
    
    public class DependentCoordinateSystem : BaseCoordinateSystem
    {
        /// <summary>
        /// 静态锁
        /// </summary>
        private static object lockObj = new object();    

        /// <summary>
        /// 创建依赖坐标系
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="coordinateSystemType">坐标系类型</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        public DependentCoordinateSystem(string name, CoordinateSystemTypeEnum coordinateSystemType, BaseCoordinateSystem upperCoordinateSystem)
        {
            this.Name = name;
            this.UpperCoordinateSystem = upperCoordinateSystem;

            if (this.UpperCoordinateSystem != null)
            {
                this.UpperName = upperCoordinateSystem.Name;
            }

            this.Type = coordinateSystemType;
        }

        /// <summary>
        /// 将自己的坐标系转换到G0上面去
        /// </summary>
        /// <param name="ownPoint3D">自己的坐标，一般为像素坐标</param>
        /// <param name="upperPoint3D">上层的坐标</param>
        /// <returns>结果</returns>
        public AKRSPoint3D SelfPosToG0(AKRSPoint3D ownPoint3D, AKRSPoint3D upperPoint3D)
        {
            lock (lockObj)
            {
                AKRSPoint3D point = this.ForwardConvertCoordinate(ownPoint3D);
                AKRSPoint3D pointReal = point + upperPoint3D;
                return this.UpperCoordinateSystem.ForwardConvertCoordinate(pointReal);
            }
        }

        /// <summary>
        /// 将G（0）的点位转到自己的坐标系
        /// </summary>
        /// <param name="point3Ds">点位</param>
        /// <returns>结果</returns>
        public AKRSPoint3D G0PosToSelf(List<AKRSPoint3D> point3Ds)
        {
            return new AKRSPoint3D();
        }
    }
}
