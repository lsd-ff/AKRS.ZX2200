using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.Path;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 载具单元补偿
    /// </summary>
    public class TransportUnitCompensate : Singleton<TransportUnitCompensate>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static TransportUnitCompensate()
        {
            TransportUnitCompensate.FilePath = ZX2200PathConfig.TransportUnitCompensatePath;
        }

        /// <summary>
        /// 补偿移动区域X
        /// </summary>
        public double[,] TransportUnitCompensateX { get; set; }

        /// <summary>
        /// 补偿移动区域Y
        /// </summary>
        public double[,] TransportUnitCompensateY { get; set; }

        /// <summary>
        /// 开始点
        /// </summary>
        public AKRSPoint3D StartPoint { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 列间距
        /// </summary>
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// 行间距
        /// </summary>
        public double RowSpacing { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// 获取补偿值
        /// </summary>
        /// <param name="x">点位X</param>
        /// <param name="y">点位Y</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetBondCompensate(double x, double y)
        {
            if (!MachineSoftwareConfiguration.GetInstance().IsBondCompensate)
            {
                return new AKRSPoint3D();
            }

            if (this.TransportUnitCompensateX == null || this.TransportUnitCompensateY == null)
            {
                return new AKRSPoint3D();
            }

            // 计算偏移值
            double offsetX = x - this.StartPoint.X;
            double offsetY = y - this.StartPoint.Y;

            // 计算在哪个区间
            int indexX = (int)(offsetX / this.ColumnSpacing);
            int indexY = (int)(offsetY / this.RowSpacing);

            double offsetDistanceX = offsetX % this.ColumnSpacing;
            double offsetDistanceY = offsetY % this.RowSpacing;

            int xAdd = 1;
            int yAdd = 1;

            if (indexX < 0)
            {
                indexX = 0;
            } 
            else if (indexX >= this.TransportUnitCompensateX.GetLength(1) - 1)
            {
                indexX = this.TransportUnitCompensateX.GetLength(1) - 1;
                xAdd = 0;
            }

            if (indexY < 0)
            {
                indexY = 0;
            }
            else if (indexY >= this.TransportUnitCompensateX.GetLength(0) - 1)
            {
                indexY = this.TransportUnitCompensateX.GetLength(0) - 1;
                yAdd = 0;
            }

            double x1 = this.TransportUnitCompensateX[indexY, indexX];
            double y1 = this.TransportUnitCompensateY[indexY, indexX];
            double x2 = this.TransportUnitCompensateX[indexY, indexX + xAdd];
            double y2 = this.TransportUnitCompensateY[indexY, indexX + xAdd];
            double x3 = this.TransportUnitCompensateX[indexY + yAdd, indexX];
            double y3 = this.TransportUnitCompensateY[indexY + yAdd, indexX];
            double x4 = this.TransportUnitCompensateX[indexY + yAdd, indexX + xAdd];
            double y4 = this.TransportUnitCompensateY[indexY + yAdd, indexX + xAdd];

            // 插值法
            double interpolationX1 = (this.ColumnSpacing - offsetDistanceX) / this.ColumnSpacing * x1
                                     + offsetDistanceX / this.ColumnSpacing * x2;

            double interpolationY1 = (this.ColumnSpacing - offsetDistanceX) / this.ColumnSpacing * y1
                                     + offsetDistanceX / this.ColumnSpacing * y2;

            double interpolationX2 = (this.ColumnSpacing - offsetDistanceX) / this.ColumnSpacing * x3
                                     + offsetDistanceX / this.ColumnSpacing * x4;

            double interpolationY2 = (this.ColumnSpacing - offsetDistanceX) / this.ColumnSpacing * y3
                                     + offsetDistanceX / this.ColumnSpacing * y4;

            double interpolationX = (this.RowSpacing - offsetDistanceY) / this.RowSpacing * interpolationX1
                                    + offsetDistanceY / this.RowSpacing * interpolationX2;

            double interpolationY = (this.RowSpacing - offsetDistanceY) / this.RowSpacing * interpolationY1
                                    + offsetDistanceY / this.RowSpacing * interpolationY2;

            if (Math.Abs(interpolationX) > 1 || Math.Abs(interpolationY) > 1)
            {
                throw new Exception("贴片补偿过大，有问题");
            }

            return new AKRSPoint3D(interpolationX, interpolationY, 0);
        }
    }
}
