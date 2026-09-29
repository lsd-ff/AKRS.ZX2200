using AKRS.Galaxy2.Infrastructure.CommonModel;
using System;
using System.Collections.Generic;

namespace AKRS.Galaxy2.CoordinateSystems.CoordinateSystems
{
    /// <summary>
    /// 普通坐标系
    /// </summary>
    
    public class GeneralCoordinateSystem : BaseCoordinateSystem
    {
        /// <summary>
        /// 创建普通坐标系
        /// </summary>
        /// <param name="name">名字</param>
        /// <param name="coordinateSystemType">坐标系类型</param>
        /// <param name="upperCoordinateSystem">上层坐标系</param>
        public GeneralCoordinateSystem(string name, CoordinateSystemTypeEnum coordinateSystemType, BaseCoordinateSystem upperCoordinateSystem)
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
        /// 无参构造函数
        /// </summary>
        public GeneralCoordinateSystem()
        {
        }

        /// <summary>
        /// 将自己的坐标转到G（0）上面去
        /// </summary>
        /// <param name="point3D">点位</param>
        /// <returns>结果</returns>
        public AKRSPoint3D SelfPosToG0(AKRSPoint3D point3D)
        {
            AKRSPoint3D point = new AKRSPoint3D(point3D.X, point3D.Y, point3D.Z);

            BaseCoordinateSystem baseCoordinateSystem = this;
            List<BaseCoordinateSystem> baseCoordinateSystems = new List<BaseCoordinateSystem>();
            while (true)
            {
                if (baseCoordinateSystem.UpperCoordinateSystem != null)
                {
                    baseCoordinateSystems.Add(baseCoordinateSystem);
                    baseCoordinateSystem = baseCoordinateSystem.UpperCoordinateSystem;
                }
                else
                {
                    break;
                }
            }

            for (int i = 0; i < baseCoordinateSystems.Count; i++)
            {
                point = baseCoordinateSystems[i].ForwardConvertCoordinate(point);
            }
           
            return point;
        }

        /// <summary>
        /// 将G（0）的点位转到自己的坐标系
        /// </summary>
        /// <param name="point3D">点位</param>
        /// <returns>结果</returns>
        public AKRSPoint3D G0PosToSelf(AKRSPoint3D point3D)
        {
            AKRSPoint3D point = new AKRSPoint3D(point3D.X, point3D.Y, point3D.Z);

            BaseCoordinateSystem baseCoordinateSystem = this;
            List<BaseCoordinateSystem> baseCoordinateSystems = new List<BaseCoordinateSystem>();
            while (true)
            {
                if (baseCoordinateSystem.UpperCoordinateSystem != null)
                {
                    baseCoordinateSystems.Add(baseCoordinateSystem);
                    baseCoordinateSystem = baseCoordinateSystem.UpperCoordinateSystem;
                }
                else
                {
                    break;
                }
            }
         
            for (int i = baseCoordinateSystems.Count - 1; i >= 0; i--)
            {
                point = baseCoordinateSystems[i].BackConvertCoordinate(point);
            }

            return point;
        }

        /// <summary>
        /// 当前坐标系在G0中的角度
        /// </summary>
        /// <returns>角度</returns>
        public double DegreeInG0()
        {
            BaseCoordinateSystem baseCoordinateSystem = this;
            List<BaseCoordinateSystem> baseCoordinateSystems = new List<BaseCoordinateSystem>();
            while (true)
            {
                if (baseCoordinateSystem.UpperCoordinateSystem != null)
                {
                    baseCoordinateSystems.Add(baseCoordinateSystem);
                    baseCoordinateSystem = baseCoordinateSystem.UpperCoordinateSystem;
                }
                else
                {
                    break;
                }
            }

            double degree = 0;

            for (int i = baseCoordinateSystems.Count - 1; i >= 0; i--)
            {
                degree = degree + baseCoordinateSystems[i].Degree;
            }

            return degree;
        }
    }
}
