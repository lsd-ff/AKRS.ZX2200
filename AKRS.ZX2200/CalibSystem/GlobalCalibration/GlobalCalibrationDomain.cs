using AKRS.ZX2200.Infrastructure.Models.CommonModels;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;

    /// <summary>
    /// 全局标定类
    /// </summary>
    public class GlobalCalibrationDomain : Singleton<GlobalCalibrationDomain>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static GlobalCalibrationDomain()
        {
            GlobalCalibrationDomain.FilePath = ZX2200PathConfig.GlobalCalibrationDomainPath;
        }
        
        /// <summary>
        /// 开始点位
        /// </summary>
        public AKRSPoint3D StartPoint3D { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 列间距
        /// </summary>
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// 行间距
        /// </summary>
        public double RowSpacing { get; set; }

        /// <summary>
        /// 列个数
        /// </summary>
        public int ColumnCount { get; set; }

        /// <summary>
        /// 行个数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// PR的名称
        /// </summary>
        public string PrName { get; set; } = "GlobalCalibrationProgram";
        
        /// <summary>
        /// 拍照的延迟
        /// </summary>
        public int Delay { get; set; }
        
        /// <summary>
        /// 名称
        /// </summary>
        public string TextName { get; set; } = string.Empty;

        /// <summary>
        /// 补偿X
        /// </summary>
        public int[,] CompensateX { get; set; }

        /// <summary>
        /// 补偿Y
        /// </summary>
        public int[,] CompensateY { get; set; }
        
        /// <summary>
        /// 自动拓展
        /// </summary>
        public bool AutoScale { get; set; } = false;

        #region 标定固晶补偿相机

        

        #endregion

        /// <summary>
        /// 获取相机的偏移值
        /// </summary>
        /// <param name="x">x的值</param>
        /// <param name="y">y的值</param>
        /// <returns>结果</returns>
        public (double, double) GetAxisCompensate(double x, double y)
        {
            if (x > this.StartPoint3D.X + this.ColumnSpacing * this.ColumnCount || x < this.StartPoint3D.X
                || y > this.StartPoint3D.Y + this.RowSpacing * this.RowCount || y < this.StartPoint3D.Y)
            {
                throw new Exception("补偿范围超极限");
            }

            // 计算偏移值
            double offsetX = x - this.StartPoint3D.X;
            double offsetY = y - this.StartPoint3D.Y;

            // 计算在哪个区间
            int indexX = (int)(offsetX / this.ColumnSpacing);
            int indexY = (int)(offsetY / this.RowSpacing);

            double offsetDistanceX = offsetX % this.ColumnSpacing;
            double offsetDistanceY = offsetY % this.RowSpacing;

            double x1 = this.CompensateX[indexY, indexX];
            double y1 = this.CompensateY[indexY, indexX];
            double x2 = this.CompensateX[indexY, indexX + 1];
            double y2 = this.CompensateY[indexY, indexX + 1];
            double x3 = this.CompensateX[indexY + 1, indexX];
            double y3 = this.CompensateY[indexY + 1, indexX];
            double x4 = this.CompensateX[indexY + 1, indexX + 1];
            double y4 = this.CompensateY[indexY + 1, indexX + 1];

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

            interpolationX = interpolationX / 10000.0;
            interpolationY = interpolationY / 10000.0;

            if (Math.Abs(interpolationX) > 1 || Math.Abs(interpolationY) > 1)
            {
                throw new Exception("补偿值过大");
            }

            return (interpolationX, interpolationY);
        }

        /// <summary>
        /// 获取焊头的补偿
        /// 相机和焊头之间有一段距离，数据都来自相机，所有焊头要减去偏移值
        /// </summary>
        /// <param name="x">点位X</param>
        /// <param name="y">点位Y</param>
        /// <param name="offset">相机和焊头之间的距离</param>
        /// <returns>结果</returns>
        public AKRSPoint3D GetBondHandCompensate(double x, double y, AKRSPoint3D offset)
        {
            // 获取相机的补偿
            (double, double) camera = this.GetAxisCompensate(x, y);

            // 获取焊头的补偿
            (double, double) bond = this.GetAxisCompensate(x - offset.X, y - offset.Y);

            // 返回差值
            return new AKRSPoint3D(camera.Item1 - bond.Item1, camera.Item2 - bond.Item2, 0);
        }
    }
}
