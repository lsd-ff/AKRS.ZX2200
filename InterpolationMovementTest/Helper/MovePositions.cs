using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.MachineSupport.Config;

namespace WindowsFormsApp1.Helper
{
    /// <summary>
    /// 点
    /// </summary>
    public class MovePositions : Singleton<MovePositions>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MovePositions()
        {
            Singleton<MovePositions>.FilePath = Path.Combine(PathConfig.DeviceDirPath, "PVTMovePositions.json");
        }

        public MovePosition Point1 = new MovePosition();

        public MovePosition Point2 = new MovePosition();

        public MovePosition Point3 = new MovePosition();

        public MovePosition Point4 = new MovePosition();

        public MovePosition Point5 = new MovePosition();

        public AKRSPoint3D StartPos = new AKRSPoint3D();

        public List<MovePosition> GetMovePostions() 
        {
            return new List<MovePosition> { Point1, Point2, Point3, Point4, Point5 };
        }
    }

    public class MovePosition
    {
        /// <summary>
        /// XYZ
        /// </summary>
        public AKRSPoint3D Pos = new AKRSPoint3D();

        /// <summary>
        /// T
        /// </summary>
        public double Angle ;

        /// <summary>
        /// 时间
        /// </summary>
        public double Time ;

        /// <summary>
        /// XYZ速度
        /// </summary>
        public AKRSPoint3D Vel = new AKRSPoint3D();

        /// <summary>
        /// T速度
        /// </summary>
        public double TVel;
    }

}
