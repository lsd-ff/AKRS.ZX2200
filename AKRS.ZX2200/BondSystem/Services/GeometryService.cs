using System;
using System.Linq;
// using System.Numerics;
using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.BondSystem.Services
{
    using System.Collections.Generic;
    using System.Drawing;
    using AKRS.ZX2200.BondSystem.Models;

    using PostSharp.Aspects.Advices;

    /// <summary>
    /// 几何帮助
    /// </summary>
    public static class GeometryService
    {
        /// <summary>
        /// 计算平面两线段交点
        /// </summary>
        /// <param name="lineOneStratPoint">线段1起点</param>
        /// <param name="lineOneEndPoint">线段1终点</param>
        /// <param name="lineTwoStratPoint">线段2起点</param>
        /// <param name="lineTwoEndPoint">线段2终点</param>
        /// <returns>交点</returns>
        public static AKRSPoint2D GetIntersectionPoint(
            AKRSPoint2D lineOneStratPoint,
            AKRSPoint2D lineOneEndPoint,
            AKRSPoint2D lineTwoStratPoint,
            AKRSPoint2D lineTwoEndPoint)
        {
            double denom = (lineTwoEndPoint.X - lineTwoStratPoint.X) * (lineOneEndPoint.Y - lineOneStratPoint.Y) -
                           (lineOneEndPoint.X - lineOneStratPoint.X) * (lineTwoEndPoint.Y - lineTwoStratPoint.Y);

            if (denom == 0)
            {
                // 平行的话就取两线段中点的中点
                AKRSPoint2D center1 = (lineOneStratPoint + lineOneEndPoint) / 2;
                AKRSPoint2D center2 = (lineTwoStratPoint + lineTwoEndPoint) / 2;

                return (center1 + center2) / 2;
            }

            double t1 = ((lineTwoStratPoint.X - lineOneStratPoint.X) * (lineTwoStratPoint.Y - lineTwoEndPoint.Y) -
                         (lineTwoStratPoint.Y - lineOneStratPoint.Y) * (lineTwoStratPoint.X - lineTwoEndPoint.X)) /
                        denom;

            return new AKRSPoint2D(lineOneStratPoint.X + t1 * (lineOneEndPoint.X - lineOneStratPoint.X),
                lineOneStratPoint.Y + t1 * (lineOneEndPoint.Y - lineOneStratPoint.Y));
        }

        /// <summary>
        /// 计算空间两线段交点,待完善
        /// </summary>
        /// <param name="lineOneStartPoint">线段1起点</param>
        /// <param name="lineOneEndPoint">线段1终点</param>
        /// <param name="lineTwoStratPoint">线段2起点</param>
        /// <param name="lineTwoEndPoint">线段2终点</param>
        /// <returns>交点</returns>
        public static AKRSPoint3D GetIntersectionPoint(
            AKRSPoint3D lineOneStartPoint,
            AKRSPoint3D lineOneEndPoint,
            AKRSPoint3D lineTwoStratPoint,
            AKRSPoint3D lineTwoEndPoint)
        {
            double denom = (lineTwoEndPoint.X - lineTwoStratPoint.X) * (lineOneEndPoint.Y - lineOneStartPoint.Y) -
                           (lineOneEndPoint.X - lineOneStartPoint.X) * (lineTwoEndPoint.Y - lineTwoStratPoint.Y);

            if (denom == 0)
            {
                // 平行的话就取两线段中点的中点
                AKRSPoint3D center1 = (lineOneStartPoint + lineOneEndPoint) / 2;
                AKRSPoint3D center2 = (lineTwoStratPoint + lineTwoEndPoint) / 2;

                return (center1 + center2) / 2;
            }

            double t1 = ((lineTwoStratPoint.X - lineOneStartPoint.X) * (lineTwoStratPoint.Y - lineTwoEndPoint.Y) -
                         (lineTwoStratPoint.Y - lineOneStartPoint.Y) * (lineTwoStratPoint.X - lineTwoEndPoint.X)) /
                        denom;

            return new AKRSPoint3D(
                lineOneStartPoint.X + t1 * (lineOneEndPoint.X - lineOneStartPoint.X),
                lineOneStartPoint.Y + t1 * (lineOneEndPoint.Y - lineOneStartPoint.Y),
                lineOneStartPoint.Z);
        }

        /// <summary>
        /// 生成随机数
        /// </summary>
        /// <param name="max">a</param>
        /// <param name="min">b</param>
        /// <returns>结果</returns>
        public static double NextDouble(double max, double min)
        {
            Random a = new Random();
            return a.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// Bond轴是否在指定范围内
        /// </summary>
        /// <param name="point1">边界1</param>
        /// <param name="point2">边界2</param>
        /// <returns>结果</returns>
        public static bool IsBond2DPosInRange(AKRSPoint2D point1, AKRSPoint2D point2)
        {
            double xNlimit = Math.Min(point1.X, point2.X);
            double xPlimit = Math.Max(point1.X, point2.X);
            double yNlimit = Math.Min(point1.Y, point2.Y);
            double yPlimit = Math.Max(point1.Y, point2.Y);

            AKRSPoint2D curPos = System2Domain.GetInstance().BondModuleController.Get2DRealPosition();


            return curPos.X >= xNlimit && curPos.X <= xPlimit && curPos.Y >= yNlimit && curPos.Y <= yPlimit;
        }

        /// <summary>
        /// Bond轴是否在指定范围内
        /// </summary>
        /// <param name="point1">边界1</param>
        /// <param name="point2">边界2</param>
        /// <returns>结果</returns>
        public static bool IsBond2DPosInRange(AKRSPoint3D point1, AKRSPoint3D point2)
        {
            double xNlimit = Math.Min(point1.X, point2.X);
            double xPlimit = Math.Max(point1.X, point2.X);
            double yNlimit = Math.Min(point1.Y, point2.Y);
            double yPlimit = Math.Max(point1.Y, point2.Y);

            AKRSPoint2D curPos = System2Domain.GetInstance().BondModuleController.Get2DRealPosition();

            return curPos.X >= xNlimit && curPos.X <= xPlimit && curPos.Y >= yNlimit && curPos.Y <= yPlimit;
        }


        /// <summary>
        /// Bond轴是否在指定范围内
        /// </summary>
        /// <param name="point1">边界1</param>
        /// <param name="point2">边界2</param>
        /// <returns>结果</returns>
        public static bool IsBond3DPosInRange(AKRSPoint3D point1, AKRSPoint3D point2)
        {
            double xNlimit = Math.Min(point1.X, point2.X);
            double xPlimit = Math.Max(point1.X, point2.X);
            double yNlimit = Math.Min(point1.Y, point2.Y);
            double yPlimit = Math.Max(point1.Y, point2.Y);
            double zNlimit = Math.Min(point1.Z, point2.Z);
            double zPlimit = Math.Max(point1.Z, point2.Z);


            AKRSPoint3D curPos = System2Domain.GetInstance().BondModuleController.Get3DRealPosition();


            return curPos.X >= xNlimit && curPos.X <= xPlimit && curPos.Y >= yNlimit && curPos.Y <= yPlimit
                   && curPos.Z >= zNlimit && curPos.Z <= zPlimit;
        }

        /// <summary>
        /// 两点间的横向距离
        /// </summary>
        /// <param name="p1">第一点</param>
        /// <param name="p2">第二点</param>
        /// <returns>横向距离</returns>
        public static double LateralDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X);
        }

        /// <summary>
        /// 两点间的纵向距离
        /// </summary>
        /// <param name="p1">第一点</param>
        /// <param name="p2">第二点</param>
        /// <returns>纵向距离</returns>
        public static double VerticalDistance(Point p1, Point p2)
        {
            return Math.Abs(p1.Y - p2.Y);
        }

        /// <summary>
        /// 四点拟合矩形（自动处理任意顺序的四个角点）
        /// </summary>
        /// <param name="points">四个角点数组</param>
        /// <returns>矩形的长宽（X为宽度，Y为高度，始终返回正值）</returns>
        /// <exception cref="ArgumentNullException">当points为null时抛出</exception>
        /// <exception cref="ArgumentException">当点数不为4时抛出</exception>
        public static AKRSPoint2D GetSizeFormFourPoint(AKRSPoint2D[] points)
        {
            if (points == null)
                throw new ArgumentNullException(nameof(points));
            if (points.Length != 4)
                throw new ArgumentException("输入点的数量必须为4", nameof(points));

            // 去重处理（防止重复点导致排序错误）
            var distinctPoints = points.Distinct().ToArray();
            if (distinctPoints.Length < 4)
                throw new ArgumentException("输入点中存在重复点，无法构成矩形");

            // 计算中心点
            double cx = distinctPoints.Average(p => p.X);
            double cy = distinctPoints.Average(p => p.Y);

            // 计算各点相对于中心的角度并排序（逆时针）
            var sortedPoints = distinctPoints
                .Select(p => new { Point = p, Angle = Math.Atan2(p.Y - cy, p.X - cx) })
                .OrderBy(x => x.Angle)  // 逆时针排序
                .Select(x => x.Point)
                .ToArray();

            // 四条边的长度
            double[] sideLengths = new double[4];
            for (int i = 0; i < 4; i++)
            {
                sideLengths[i] = Distance(sortedPoints[i], sortedPoints[(i + 1) % 4]);
            }

            // 矩形相对的两边长度应该相等，取平均值提高容错性
            double width = (sideLengths[0] + sideLengths[2]) / 2;   // 边0和对边2
            double height = (sideLengths[1] + sideLengths[3]) / 2;  // 边1和对边3

            // 确保宽度不小于高度（可选，根据业务需求）
            // 如果需要宽度始终大于等于高度，取消注释：
            // if (width < height)
            // {
            //     double temp = width;
            //     width = height;
            //     height = temp;
            // }

            // 防止浮点误差导致的负数（理论上不会为负，但保留检查）
            width = Math.Max(0, width);
            height = Math.Max(0, height);

            return new AKRSPoint2D(width, height);
        }

        /// <summary>
        /// 计算两点之间的欧氏距离
        /// </summary>
        private static double Distance(AKRSPoint2D p1, AKRSPoint2D p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// 根据槽位数生成喂料器位置点
        /// </summary>
        public static AKRSPoint3D[] GeneratePointsBetween(AKRSPoint3D startPos, AKRSPoint3D endPos, int slotCount)
        {
            if (slotCount <= 0)
            {
                return new AKRSPoint3D[] { startPos };
            }

            // 创建数组，长度为 slotCount + 1（包含起点和终点）
            AKRSPoint3D[] positions = new AKRSPoint3D[slotCount];

            double stepX = (endPos.X - startPos.X) / (slotCount - 1);
            double stepY = (endPos.Y - startPos.Y) / (slotCount - 1);
            double stepZ = (endPos.Z - startPos.Z) / (slotCount - 1);

            for (int i = 0; i < slotCount; i++)
            {
                positions[i] = new AKRSPoint3D(startPos.X + stepX * i, startPos.Y + stepY * i, startPos.Z + stepZ * i);
            }

            return positions;
        }

        /// <summary>
        /// 根据槽位数生成喂料器位置点
        /// </summary>
        public static double[] GeneratePointsBetween(double startPos, double endPos, int slotCount)
        {
            if (slotCount <= 0)
            {
                return new double[] { startPos };
            }

            // 创建数组，长度为 slotCount + 1（包含起点和终点）
            double[] positions = new double[slotCount];

            double stepX = (endPos - startPos) / (slotCount - 1);

            for (int i = 0; i < slotCount; i++)
            {
                positions[i] = startPos + stepX * i;
            }

            return positions;
        }

        ///// <summary>
        ///// 多点线性拟合圆
        ///// </summary>
        ///// <param name="coords"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentException"></exception>
        //public static ((double X, double Y) centerOfCircle, double radius) LinearFitCircle(
        //    params (double X, double Y )[] coords)
        //{
        //    // 至少需要3个点
        //    if (coords.Length < 3)
        //    {
        //        throw new ArgumentException("至少包含三个点。");
        //    }

        //    int n = coords.Length;
        //    Matrix<double> A = DenseMatrix.OfArray(new double[n, 3]);
        //    Vector<double> b = DenseVector.OfArray(new double[n]);

        //    for (int i = 0; i < n; i++)
        //    {
        //        double x = coords[i].X;
        //        double y = coords[i].Y;
        //        A[i, 0] = 2 * x;
        //        A[i, 1] = 2 * y;
        //        A[i, 2] = 1;
        //        b[i] = x * x + y * y;
        //    }

        //    // 判断行列式是否为0
        //    if (A.TransposeThisAndMultiply(A).Determinant() == 0)
        //    {
        //        throw new InvalidOperationException("矩阵接近奇异，无法求逆");
        //    }

        //    Vector<double> p = A.TransposeThisAndMultiply(A).Inverse() * A.TransposeThisAndMultiply(b);
        //    double centerX = p[0];
        //    double centerY = p[1];
        //    double radius = Math.Sqrt(p[2] + centerX * centerX + centerY * centerY);

        //    return ((centerX, centerY), radius);
        //}

        ///// <summary>
        ///// 多点非线性拟合圆
        ///// </summary>
        ///// <param name="coords"></param>
        ///// <returns></returns>
        ///// <exception cref="ArgumentException"></exception>
        //public static ((double X, double Y) centerOfCircle, double radius) NonlinearFitCircle(
        //    params (double X, double Y)[] coords)
        //{
        //    // 至少需要3个点
        //    if (coords.Length < 3)
        //    {
        //        throw new ArgumentException("至少包含三个点。");
        //    }

        //    int n = coords.Length;
        //    double[,] pointsArray = new double[n, 2];
        //    for (int i = 0; i < n; i++)
        //    {
        //        pointsArray[i, 0] = coords[i].X;
        //        pointsArray[i, 1] = coords[i].Y;
        //    }

        //    DenseMatrix points = DenseMatrix.OfArray(pointsArray);

        //    // 初始猜测：使用点的均值作为初始圆心位置
        //    double xMean = points.Column(0).Average();
        //    double yMean = points.Column(1).Average();
        //    double[] initialGuess = { xMean, yMean };

        //    // 定义残差函数
        //    Func<Vector<double>, Vector<double>, Vector<double>> residuals = (c, observed) =>
        //    {
        //        double xc = c[0];
        //        double yc = c[1];
        //        double[] distances = points.EnumerateRows()
        //            .Select(row => Math.Sqrt(Math.Pow(row[0] - xc, 2) + Math.Pow(row[1] - yc, 2))).ToArray();
        //        double meanDistance = distances.Average();
        //        return Vector<double>.Build.Dense(distances.Select(d => d - meanDistance).ToArray());
        //    };

        //    // 定义观测值
        //    Vector<double> observedX = Vector<double>.Build.Dense(coords.Length, i => i);
        //    Vector<double> observedY = Vector<double>.Build.Dense(coords.Length, i => 0.0);

        //    // 使用最小二乘法优化
        //    LevenbergMarquardtMinimizer optimizer = new LevenbergMarquardtMinimizer();
        //    Vector<double> initialVector = Vector<double>.Build.DenseOfArray(initialGuess);
        //    IObjectiveModel objective = ObjectiveFunction.NonlinearModel(residuals, observedX, observedY);
        //    NonlinearMinimizationResult result = optimizer.FindMinimum(objective, initialVector);

        //    // 计算最终圆心坐标和半径
        //    double centerX = result.MinimizingPoint[0];
        //    double centerY = result.MinimizingPoint[1];
        //    double radius = points.EnumerateRows()
        //        .Select(row => Math.Sqrt(Math.Pow(row[0] - centerX, 2) + Math.Pow(row[1] - centerY, 2))).Average();

        //    return ((centerX, centerY), radius);
        //}
    }
}