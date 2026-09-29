using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Infrastructure.Models.Path;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using AKRS.ZX2200.Infrastructure.Utils;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 贴片标定
    /// </summary>
    public class BondCompensateCalibration : Singleton<BondCompensateCalibration>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static BondCompensateCalibration()
        {
            BondCompensateCalibration.FilePath = ZX2200PathConfig.BondCompensateCalibrationPath;
        }

        /// <summary>
        /// 2维标定
        /// </summary>
        public AKRSPoint2D[,] PlanePoint2Ds { get; set; }

        /// <summary>
        /// 2维标定开始点
        /// </summary>
        public AKRSPoint3D StartPoint { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// X方向间距
        /// </summary>
        public double DistanceX { get; set; } = 1;

        /// <summary>
        /// Y方向间距
        /// </summary>
        public double DistanceY { get; set; } = 1;

        /// <summary>
        /// 列的数量
        /// </summary>
        public int ColumnCount { get; set; } = 1;

        /// <summary>
        /// 行的数量
        /// </summary>
        public int RowCount { get; set; } = 1;

        /// <summary>
        /// pR的名字
        /// </summary>
        public string PrName { get; set; }

        /// <summary>
        /// 视觉定位延迟
        /// </summary>
        public int PrDelay { get; set; }

        /// <summary>
        /// 获取贴片补偿
        /// </summary>
        /// <param name="x">x的值</param>
        /// <param name="y">y的值</param>
        /// <returns>结果</returns>
        public AKRSPoint2D GetBondOffset(double x, double y)
        {
            if (this.PlanePoint2Ds == null)
            {
                return new AKRSPoint2D(0, 0);
            }

            return MathAdvanceHelper.PlaneInterpolate(
                x,
                y,
                this.PlanePoint2Ds,
                this.StartPoint,
                this.DistanceX,
                this.DistanceY);
        }


        public void Test()
        {
            this.StartPoint = new AKRSPoint3D(-61.49, -500.52, 0);

            this.ColumnCount = 100;

            this.RowCount = 100;

            this.DistanceX = 2;

            this.DistanceY = 10000;

            string a =
                "4.25 4.55 3.6 4.85 3.95 4.5 4.05 3.95 4.1 2.7 3.05 3.3 3 3.1 3.25 2.9 2.7 2.45 2.4 2.95 2.35 2.55 3 2.45 2.5 2 2.75 2.5 1.9 2.2 2.3 2.6 1.85 1.85 2 2 1.55 1.55 1.25 1.5 1.7 1.05 1.2 1.15 1.45 1.5 1.3 0.9 0.9 1.25 1 1.8 1.4 1.1 1.15 1.25 1.5 1.55 1.5 1.5 2.2 1.6 2.05 2.8 1.65 2.55 1.55 2.25 2 2.4 2.25 2.9 2.5 2.55 2.45 2.7 2.65 2.8 2.8 3.1 2.65 2.2 2.5 2.3 2.7 3.2 2.85 3.5 3.15 3.4 3.2 2.95 3.15 3.75 3.35 3.55 3.55 3.1 3.4 3.95";

            string[] b = a.Split(' ');

            this.PlanePoint2Ds = new AKRSPoint2D[2, 100];

            for (int i = 0; i < 100; i++)
            {
                this.PlanePoint2Ds[0, i] = new AKRSPoint2D(0, Convert.ToDouble(b[i]));
            }

            for (int i = 0; i < 100; i++)
            {
                this.PlanePoint2Ds[1, i] = new AKRSPoint2D(0, Convert.ToDouble(b[i]));
            }

            this.Save();

            AKRSPoint2D c = GetBondOffset(0, 100);
        }


    }
}
