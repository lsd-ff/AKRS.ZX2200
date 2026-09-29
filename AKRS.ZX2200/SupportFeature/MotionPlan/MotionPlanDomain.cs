using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.SupportFeature.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.MotionPlan
{
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.ZX2200.Infrastructure.Utils;
    using DevExpress.Office.Utils;
    using DevExpress.XtraCharts.Native;
    using DevExpress.XtraEditors;
    using System.Drawing;

    /// <summary>
    /// 运动规划
    /// </summary>
    public class MotionPlanDomain : Singleton<MotionPlanDomain>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static MotionPlanDomain()
        {
            MotionPlanDomain.FilePath = ZX2200PathConfig.MotionPlanDomainPath;
        }

        /// <summary>
        /// 取片安全点位1
        /// </summary>
        public AKRSPoint3D PickSafeAreaPoint1 { get; set; }

        /// <summary>
        /// 取片安全点位2
        /// </summary>
        public AKRSPoint3D PickSafeAreaPoint2 { get; set; }

        /// <summary>
        /// 取片安全点位3
        /// </summary>
        public AKRSPoint3D PickSafeAreaPoint3 { get; set; }

        /// <summary>
        /// 取片安全点位4
        /// </summary>
        public AKRSPoint3D PickSafeAreaPoint4 { get; set; }

        /// <summary>
        /// 获取半径
        /// </summary>
        /// <param name="startPos">开始点</param>
        /// <param name="endPos">结束点</param>/
        /// <returns>结果</returns>
        public double GetRadius(AKRSPoint3D startPos, AKRSPoint3D endPos)
        {
            List<AKRSPoint3D> listPos = new List<AKRSPoint3D>()
                                            {
                                                this.PickSafeAreaPoint1,
                                                this.PickSafeAreaPoint2,
                                                this.PickSafeAreaPoint3,
                                                this.PickSafeAreaPoint4
                                            };

            if (listPos.Exists(it => it == null))
            {
                throw new Exception("取片安全区域为空，无法计算半径");
            }

            if (endPos.X > listPos.Max(it => it.X) || endPos.X < listPos.Min(it => it.X) 
                || endPos.Y > listPos.Max(it => it.Y) || endPos.Y < listPos.Min(it => it.Y))
            {
                throw new Exception("取片点在安全区域之外，无法计算半径");
            }

            // 获取交点
            AKRSPoint3D intersection = this.GetIntersection(startPos, endPos);

            if (intersection == null)
            {
                throw new Exception("取片点和安全区域无交点，无法计算半径");
            }
            
            // 计算取片点和交点的距离
            double radius = Math.Sqrt(Math.Pow(endPos.X - intersection.X, 2) + Math.Pow(endPos.Y - intersection.Y, 2));

            // 返回两个半径之间的最小值
            return Math.Min(radius, Math.Abs(startPos.Z - endPos.Z));
        }

        /// <summary>
        /// 根据距离获取不同的参数
        /// </summary>
        /// <param name="startPos">开始点</param>
        /// <param name="endPos">结束点</param>
        /// <returns>结果</returns>
        public MovePara GetMoveParaByDistance(AKRSPoint3D startPos, AKRSPoint3D endPos)
        {
            // 两点之间的距离
            double distance = Math.Sqrt(Math.Pow(endPos.X - startPos.X, 2) + Math.Pow(endPos.Y - startPos.Y, 2));

            double acc = 10000.0;
            double jertTime = 0.1;

            if (distance < 10)
            {
                acc = 10000.0;
                jertTime = 0.1;
            }
            else if (distance < 20)
            {
                acc = 11000.0;
                jertTime = 0.09;
            }
            else if (distance < 30)
            {
                acc = 11000.0;
                jertTime = 0.08;
            }
            else if (distance < 40)
            {
                acc = 12000.0;
                jertTime = 0.07;
            }
            else if (distance < 50)
            {
                acc = 12000.0;
                jertTime = 0.06;
            }
            else if (distance < 60)
            {
                acc = 13000.0;
                jertTime = 0.05;
            }
            else if (distance < 70)
            {
                acc = 13000.0;
                jertTime = 0.04;
            }
            else if (distance < 80)
            {
                acc = 14000.0;
                jertTime = 0.03;
            }
            else if (distance < 90)
            {
                acc = 14000.0;
                jertTime = 0.02;
            }
            else if (distance < 100)
            {
                acc = 15000.0;
                jertTime = 0.02;
            }
            else
            {
                acc = 15000.0;
                jertTime = 0.02;
            }

            return new MovePara() { Acc = acc, Dec = acc, Jerk = jertTime };
        }

        /// <summary>
        /// 判断两条线段的交点
        /// </summary>
        /// <param name="p1">线段1点1</param>
        /// <param name="p2">线段1点2</param>
        /// <param name="p3">线段2点1</param>
        /// <param name="p4">线段2点2</param>
        /// <returns>结果</returns>
        private AKRSPoint3D FindIntersection(AKRSPoint3D p1, AKRSPoint3D p2, AKRSPoint3D p3, AKRSPoint3D p4)
        {
            double a1 = p2.Y - p1.Y;
            double b1 = p1.X - p2.X;
            double c1 = a1 * p1.X + b1 * p1.Y;
            double a2 = p4.Y - p3.Y;
            double b2 = p3.X - p4.X;
            double c2 = a2 * p3.X + b2 * p3.Y;
            double det = a1 * b2 - a2 * b1;

            if (det == 0)
            {
                // 两条直线平行或重合，无交点或有无穷多个交点
                return null;
            }

            double x = (b2 * c1 - b1 * c2) / det;
            double y = (a1 * c2 - a2 * c1) / det;

            // 判断交点是否在线段上
            if (x > Math.Max(p1.X, p2.X) || x < Math.Min(p1.X, p2.X))
            {
                return null;
            }

            return new AKRSPoint3D(x, y, p1.Z);
        }

        /// <summary>
        /// 寻找交点
        /// </summary>
        /// <param name="startPos">开始点</param>
        /// <param name="endPos">结束点</param>
        /// <returns>结果</returns>
        private AKRSPoint3D GetIntersection(AKRSPoint3D startPos, AKRSPoint3D endPos)
        {
            AKRSPoint3D point3D = this.FindIntersection(
                startPos,
                endPos,
                this.PickSafeAreaPoint1,
                this.PickSafeAreaPoint2);

            if (point3D != null)
            {
                return point3D;
            }

            point3D = this.FindIntersection(
                startPos,
                endPos,
                this.PickSafeAreaPoint2,
                this.PickSafeAreaPoint3);

            if (point3D != null)
            {
                return point3D;
            }

            point3D = this.FindIntersection(
                startPos,
                endPos,
                this.PickSafeAreaPoint3,
                this.PickSafeAreaPoint4);

            if (point3D != null)
            {
                return point3D;
            }

            point3D = this.FindIntersection(
                startPos,
                endPos,
                this.PickSafeAreaPoint4,
                this.PickSafeAreaPoint1);

            if (point3D != null)
            {
                return point3D;
            }

            return null;
        }


        /// <summary>
        /// 获取运动曲线
        /// </summary>
        /// <param name="startPos">开始点</param>
        /// <param name="endPos">结束点</param>
        /// <returns>结果</returns>
        public List<AKRSPoint3D> GetMoveLine(AKRSPoint3D startPos, AKRSPoint3D endPos)
        {

            List<AKRSPoint3D> listPos = new List<AKRSPoint3D>()
                                            {
                                                this.PickSafeAreaPoint1,
                                                this.PickSafeAreaPoint2,
                                                this.PickSafeAreaPoint3,
                                                this.PickSafeAreaPoint4
                                            };

            if (listPos.Exists(it => it == null))
            {
                throw new Exception("取片安全区域为空，无法计算半径");
            }

            if (endPos.X > listPos.Max(it => it.X) || endPos.X < listPos.Min(it => it.X)
                || endPos.Y > listPos.Max(it => it.Y) || endPos.Y < listPos.Min(it => it.Y))
            {
                throw new Exception("取片点在安全区域之外，无法计算半径");
            }

            // 获取交点
            AKRSPoint3D intersection = this.GetIntersection(startPos, endPos);

            if (intersection == null)
            {
                throw new Exception("取片点和安全区域无交点，无法计算半径");
            }

            AKRSPoint3D distancePoint = intersection - endPos;

            double distance = Math.Sqrt(distancePoint.X * distancePoint.X + distancePoint.Y * distancePoint.Y);

            double ratio = distancePoint.Z / (distance * distance);

            double theta = Math.Atan2(distancePoint.Y, distancePoint.X);

            List<AKRSPoint3D> list = new List<AKRSPoint3D>();

            AKRSPoint3D first = intersection - startPos;

            for (int i = 0; i <= 10; i++)
            {
                list.Add(new AKRSPoint3D(startPos.X + first.X / 10.0 * i, startPos.Y + first.Y / 10.0 * i, startPos.Z));
            }

            for (int i = 0; i <= 10; i++)
            {
                double x = distancePoint.X / 10.0 * i * Math.Cos(theta);

                double y = distancePoint.Y / 10.0 * i * Math.Sin(theta);

                double step = Math.Sqrt(x * x + y * y);

                double z = ratio * step * step;

                list.Add(new AKRSPoint3D(x + intersection.X, -y + intersection.Y, -z + intersection.Z)); 
            }

            return list;
        }
    }
}
