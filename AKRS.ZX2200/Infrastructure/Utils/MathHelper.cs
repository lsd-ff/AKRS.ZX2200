namespace AKRS.ZX2200.Infrastructure.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Threading;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Service;

    /// <summary>
    /// 数学计算静态帮助类
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// 旋转中心
        /// 逆时针为正
        /// </summary>
        /// <param name="distanceToCenter">到旋转中心的距离</param>
        /// <param name="degree">弧度</param>
        public static AKRSPoint3D RotateCenter(AKRSPoint3D distanceToCenter,double degree)
        {
            AKRSPoint3D point = new AKRSPoint3D();
            point.X = distanceToCenter.X * Math.Cos(degree) - distanceToCenter.Y * Math.Sin(degree);
            point.Y = distanceToCenter.Y * Math.Cos(degree) + distanceToCenter.X * Math.Sin(degree);
            point.Z = distanceToCenter.Z;
            return point;
        }


        /// <summary>
        /// 旋转矫正，顺时针为正
        /// </summary>
        /// <param name="rotatePoint">被旋转点</param>
        /// <param name="centerPoint">旋转中心</param>
        /// <param name="degree">弧度</param>
        /// <returns>旋转之后出来的点位</returns>
        public static AKRSPoint3D RotateCenter(AKRSPoint3D rotatePoint, AKRSPoint3D centerPoint, double degree)
        {
            AKRSPoint3D point = new AKRSPoint3D
            {
                X = (rotatePoint.X - centerPoint.X) * Math.Cos(degree) - (rotatePoint.Y - centerPoint.Y) * Math.Sin(degree) + centerPoint.X,
                Y = (rotatePoint.Y - centerPoint.Y) * Math.Cos(degree) + (rotatePoint.X - centerPoint.X) * Math.Sin(degree) + centerPoint.Y,
                Z = rotatePoint.Z
            };
            return point;
        }

        /// <summary>
        /// 旋转矫正，顺时针为正
        /// </summary>
        /// <param name="rotatePoint">被旋转点</param>
        /// <param name="centerPoint">旋转中心</param>
        /// <param name="degree">弧度</param>
        /// <returns>旋转之后出来的点位</returns>
        public static AKRSPoint2D RotateCenter(AKRSPoint2D rotatePoint, AKRSPoint2D centerPoint, double degree)
        {
            AKRSPoint2D point = new AKRSPoint2D
                                    {
                                        X = (rotatePoint.X - centerPoint.X) * Math.Cos(degree) - (rotatePoint.Y - centerPoint.Y) * Math.Sin(degree) + centerPoint.X,
                                        Y = (rotatePoint.Y - centerPoint.Y) * Math.Cos(degree) + (rotatePoint.X - centerPoint.X) * Math.Sin(degree) + centerPoint.Y,
                                    };
            return point;
        }

        /// <summary>
        /// 旋转中心
        /// 逆时针为正
        /// </summary>
        /// <param name="distanceToCenter">到旋转中心的距离</param>
        /// <param name="degree">弧度</param>
        public static MatchResult RotateCenter(MatchResult rotatePoint, MatchResult centerPoint, double degree)
        {
            MatchResult point = new MatchResult();
            point.CenterX = (rotatePoint.CenterX - centerPoint.CenterX) * Math.Cos(degree) - (rotatePoint.CenterY - centerPoint.CenterY) * Math.Sin(degree) + centerPoint.CenterX;
            point.CenterY = (rotatePoint.CenterY - centerPoint.CenterY) * Math.Cos(degree) + (rotatePoint.CenterX - centerPoint.CenterX) * Math.Sin(degree) + centerPoint.CenterY;
            return point;
        }

        /// <summary>
        /// 一点纠偏
        /// </summary>
        /// <param name="center">旋转中心</param>
        /// <param name="resultPos">点位</param>
        /// <param name="degree">角度</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D OnePointAdjust(double angleDifference, double radius, AKRSPoint3D resultPos, double NewDegree,double rotateDistance)
        {
            // 计算焊头0度的时候所在的位置
            AKRSPoint3D a = RotateCenter(
                new AKRSPoint3D(radius * Math.Cos(angleDifference), radius * Math.Sin(angleDifference), 0),
                NewDegree);

            // 计算XY方向的差值
            AKRSPoint3D b = resultPos - a;

            AKRSPoint3D c = RotateCenter(
                new AKRSPoint3D(radius * Math.Cos(angleDifference), radius * Math.Sin(angleDifference), 0),
                NewDegree - angleDifference);


            return c;
        }

        /// <summary>
        /// 上视矫正
        /// 一下坐标均是基于G0
        /// </summary>
        /// <param name="upLookCenter">旋转中心</param>
        /// <param name="coordinateSystem">上视坐标系</param>
        /// <param name="visionPos">上视拍照位置</param>
        /// <param name="degree">需要旋转的角度</param>
        /// <param name="matchResult">定位结果</param>
        /// <returns>贴片时需要相对移动的值</returns>
        public static AKRSPoint3D UpLookAdjust(AKRSPoint3D upLookCenter, DependentCoordinateSystem coordinateSystem, AKRSPoint3D visionPos, double degree, MatchResult matchResult)
        {
            // 转为弧度去计算
            double realDegree = degree * (Math.PI / 180);

            // 获取轴当前的位置
            AKRSPoint3D point3D = System2Module.GetInstance().BondModule.Get3DRealPosition();


            // 定位出来的结果转到G0
            AKRSPoint3D result = coordinateSystem.SelfPosToG0(
                new AKRSPoint3D(matchResult.CenterX, matchResult.CenterY, 0),
                point3D);

            // 以当前位置拍照位为旋转中心
            AKRSPoint3D center = System2Module.GetInstance().BondModule.BondCoordinateSystem.SelfPosToG0(visionPos);

            // 旋转之后的位置
            AKRSPoint3D rotatePoint = RotateCenter(result, center, realDegree);

            // 相对位移，正负无法确定
            AKRSPoint3D relativeDisplacement = rotatePoint - center;

            // 返回相对值
            return relativeDisplacement;
        }

        public static AKRSPoint3D UpLookTwoPointAdjust(AKRSPoint3D upLookCenter, DependentCoordinateSystem coordinateSystem, AKRSPoint3D visionPos1,AKRSPoint3D visionPos2, double degree, MatchResult matchResult1,MatchResult matchResult2)
        {
            // 转为弧度去计算
            double realDegree = degree * (Math.PI / 180);

            // 定位出来的结果转到G0
            AKRSPoint3D result1 = coordinateSystem.SelfPosToG0(
                new AKRSPoint3D(matchResult1.CenterX, matchResult1.CenterY, 0),
                visionPos1);

            // 定位出来的结果转到G0
            AKRSPoint3D result2 = coordinateSystem.SelfPosToG0(
                new AKRSPoint3D(matchResult2.CenterX, matchResult2.CenterY, 0),
                visionPos2);

            AKRSPoint3D result = (result1 + result2) / 2;

            // 旋转之后的位置
            AKRSPoint3D rotatePoint = RotateCenter(result, upLookCenter, realDegree);

            // 相对位移，正负无法确定
            AKRSPoint3D relativeDisplacement = rotatePoint - upLookCenter;

            // 返回相对值
            return relativeDisplacement;
        }

        /// <summary>
        /// 旋转测试
        /// 围绕旋转中心输出图像
        /// </summary>
        public static void RotateCenterTest()
        {
            //// 上视的位置
            //AKRSPoint3D upLookCenter = CalibrateRunPara.GetInstance().GlassUpLookVisionMachinePos;

            //// 上视在轴坐标中的位置
            //upLookCenter = System2Module.GetInstance().BondModule.BondCoordinateSystem.SelfPosToG0(upLookCenter);

            //// 当前的G0位置
            //AKRSPoint3D point3D = System2Module.GetInstance().BondModule.GetG0RealPosition();

            //// 当前的轴位置
            //AKRSPoint3D point3D1 = System2Module.GetInstance().BondModule.Get3DRealPosition();

            //// 执行定位
            //MatchResult result = System2Domain.GetInstance().System2Controller.AdjustAction("新PR模板377039", "新PR模板377039", BondSystem.Models.Enums.CameraTypeEnum.UpLookCamera);

            //// 上视定位出来的结果
            //AKRSPoint3D realPos = System2Module.GetInstance().UpLookModule.ConvertPixelToG0Pos(point3D1,result);

            //// 旋转之后出来的位置
            // AKRSPoint3D pointAfterRotate = RotateCenter(realPos, point3D,  -result.Angle * (Math.PI / 180));

            //// 移动到相机中心
            // AKRSPoint3D nowCenter = pointAfterRotate;

            //// 如果上述过程时正确的，则移动到这个位置相机中心刚好对准相机中心
            //System2Module.GetInstance().BondModule.MoveToG0Pos(nowCenter);

            //Thread.Sleep(1000);

            //System2Module.GetInstance().BondModule.BondHead.AxisT.AbsoluteMove(0);
        }

        /// <summary>
        /// 旋转补偿，
        /// 如果标准公式算的精度不够将会使用这个算法
        /// </summary>
        /// <param name="list">补偿之后的集合</param>
        /// <param name="degree">芯片此时的角度</param>
        public static void RotationCompensation(List<PointF> list, double degree)
        {
            double[] arrX = new double[list.Count];
            double[] arrY = new double[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                arrX[i] = list[i].X;
                arrY[i] = list[i].Y;
            }

            double[] a =  MultiLine(arrX, arrY, list.Count, 2);

            double c = a[0] + 100 * a[1] + 100 * 100 * a[2];
        }

        ///<summary>
        ///用最小二乘法拟合二元多次曲线
        ///</summary>
        ///<param name="arrX">已知点的x坐标集合</param>
        ///<param name="arrY">已知点的y坐标集合</param>
        ///<param name="length">已知点的个数</param>
        ///<param name="dimension">方程的最高次数</param>

        public static double[] MultiLine(double[] arrX, double[] arrY, int length, int dimension)//二元多次线性方程拟合曲线
        {
            int n = dimension + 1;                  //dimension次方程需要求 dimension+1个 系数
            double[,] Guass = new double[n, n + 1];      //高斯矩阵 例如：y=a0+a1*x+a2*x*x
            for (int i = 0; i < n; i++)
            {
                int j;
                for (j = 0; j < n; j++)
                {
                    Guass[i, j] = SumArr(arrX, j + i, length);
                }
                Guass[i, j] = SumArr(arrX, i, arrY, 1, length);
            }
            return ComputGauss(Guass, n);
        }
        public static double SumArr(double[] arr, int n, int length) //求数组的元素的n次方的和
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if (arr[i] != 0 || n != 0)
                    s = s + Math.Pow(arr[i], n);
                else
                    s = s + 1;
            }
            return s;
        }
        public static double SumArr(double[] arr1, int n1, double[] arr2, int n2, int length)
        {
            double s = 0;
            for (int i = 0; i < length; i++)
            {
                if ((arr1[i] != 0 || n1 != 0) && (arr2[i] != 0 || n2 != 0))
                    s = s + Math.Pow(arr1[i], n1) * Math.Pow(arr2[i], n2);
                else
                    s = s + 1;
            }
            return s;

        }


        public static double[] ComputGauss(double[,] Guass, int n)
        {
            int i, j;
            int k, m;
            double temp;
            double max;
            double s;
            double[] x = new double[n];
            for (i = 0; i < n; i++) x[i] = 0.0;//初始化

            for (j = 0; j < n; j++)
            {
                max = 0;
                k = j;
                for (i = j; i < n; i++)
                {
                    if (Math.Abs(Guass[i, j]) > max)
                    {
                        max = Guass[i, j];
                        k = i;
                    }
                }


                if (k != j)
                {
                    for (m = j; m < n + 1; m++)
                    {
                        temp = Guass[j, m];
                        Guass[j, m] = Guass[k, m];
                        Guass[k, m] = temp;
                    }
                }
                if (0 == max)
                {
                    // "此线性方程为奇异线性方程" 
                    return x;
                }

                for (i = j + 1; i < n; i++)
                {
                    s = Guass[i, j];
                    for (m = j; m < n + 1; m++)
                    {
                        Guass[i, m] = Guass[i, m] - Guass[j, m] * s / (Guass[j, j]);
                    }
                }

            }//结束for (j=0;j<n;j++)

            for (i = n - 1; i >= 0; i--)
            {
                s = 0;
                for (j = i + 1; j < n; j++)
                {
                    s = s + Guass[i, j] * x[j];
                }
                x[i] = (Guass[i, n] - s) / Guass[i, i];
            }
            return x;
        }//返回值是函数的系数


        /// <summary>
        /// 计算四个点组成的对角线的交点
        /// </summary>
        /// <param name="point3D11">第一个线段的第一个点</param>
        /// <param name="point3D12">第一个线段的第二个点</param>
        /// <param name="point3D21">第二个线段的第一个点</param>
        /// <param name="point3D22">第二个线段的第二个点</param>
        /// <returns>结果</returns>
        public static AKRSPoint3D CalculateAdhesive(
            AKRSPoint3D point3D11,
            AKRSPoint3D point3D12,
            AKRSPoint3D point3D21,
            AKRSPoint3D point3D22)
        {
            double x1 = point3D11.X;
            double x2 = point3D12.X;
            double x3 = point3D21.X;
            double x4 = point3D22.X;

            double y1 = point3D11.Y;
            double y2 = point3D12.Y;
            double y3 = point3D21.Y;
            double y4 = point3D22.Y;

            // 根据直线方程两点式求得的交点坐标
            double x = ((x2 - x1) * (x3 - x4) * (y3 - y1) -
                     x3 * (x2 - x1) * (y3 - y4) + x1 * (y2 - y1) * (x3 - x4)) /
                    ((y2 - y1) * (x3 - x4) - (x2 - x1) * (y3 - y4));
            double y = ((y2 - y1) * (y3 - y4) * (x3 - x1) -
                        y3 * (y2 - y1) * (x3 - x4) + y1 * (x2 - x1) * (y3 - y4)) /
                       ((x2 - x1) * (y3 - y4) - (y2 - y1) * (x3 - x4));

            return new AKRSPoint3D(x, y, point3D11.Z);
        }

        /// <summary>
        /// ASCII转字符串
        /// </summary>
        /// <param name="xmlStr">字符串</param>
        /// <returns>ASCII</returns>
        public static byte[] AsciiToString(string xmlStr)
        {
            return Encoding.Default.GetBytes(xmlStr);
        }

        /// <summary>
        /// 字符串转ASCII
        /// </summary>
        /// <param name="buf">ASCII</param>
        /// <returns>字符串</returns>
        public static string Ascii2Str(byte[] buf)
        {
            return System.Text.Encoding.ASCII.GetString(buf);
        }


        public static void Test()
        {
            AKRSPoint3D point3D = new AKRSPoint3D(132.2087,-214,99);
            AKRSPoint3D point = new AKRSPoint3D(69.4759, -261.4913,90.175);
            DispenseMeasureHeightController controller = System1Domain.GetInstance().DispenseMeasureHeightController;
            List<List<double>> list = new List<List<double>>();
            DateTime dateTime = DateTime.Now;

            int index = 0;

            while (true)
            {
                DateTime dateTimeWork = DateTime.Now;

                if (index == 0)
                {
                    // 慢速不抬起测高气缸
                    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                        System1MeasHeightToolEnum.HeightSensor,
                        point3D,
                        double.NaN,
                        double.NaN, false);
                    list.Add(new List<double>() { height, 1 });
                }
                else if(index == 1)
                {
                    // 慢速不抬起测高气缸
                    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                        System1MeasHeightToolEnum.HeightSensor,
                        point3D,
                        double.NaN,
                        double.NaN,
                        false);
                    list.Add(new List<double>() { height, 2 });
                }
   
                //else if ((dateTimeWork - DateTime.Now).Hours < 3 && (dateTimeWork - DateTime.Now).Hours >= 2)
                //{
                //    // 慢速抬起测高气缸
                //    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                //        System1MeasHeightToolEnum.HeightSensor,
                //        point3D,
                //        double.NaN,
                //        true,
                //        1);
                //    list.Add(new List<double>() { height, 3 });
                //}
                //else if ((dateTimeWork - DateTime.Now).Hours < 4 && (dateTimeWork - DateTime.Now).Hours >= 3)
                //{
                //    // 快速不抬起测高气缸
                //    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                //        System1MeasHeightToolEnum.HeightSensor,
                //        point3D,
                //        double.NaN,
                //        true,
                //        10);
                //    list.Add(new List<double>() { height, 4 });
                //}
                //else if ((dateTimeWork - DateTime.Now).Hours < 5 && (dateTimeWork - DateTime.Now).Hours >= 4)
                //{
                //    // 慢速与点胶板测高
                //    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                //        System1MeasHeightToolEnum.Dispenser,
                //        point,
                //        double.NaN,
                //        true,
                //        1);
                //    list.Add(new List<double>() { height, 5 });
                //}
                //else if ((dateTimeWork - DateTime.Now).Hours < 6 && (dateTimeWork - DateTime.Now).Hours >= 5)
                //{
                //    // 慢速与点胶板测高
                //    (ExcuteResult result, double height) = controller.DispenserHeightMeasurementG0(
                //        System1MeasHeightToolEnum.Dispenser,
                //        point,
                //        double.NaN,
                //        true,
                //        10);
                //    list.Add(new List<double>() { height, 6 });
                //}
                else
                {
                    return;
                }

                if (list.Count > 1000)
                {
                    string path = FileHelper.CreateFileByDate("点胶测高测试");
                    FileHelper.SaveDoubleExcel(list, Path.Combine(path, DateTime.Now.ToFileTime().ToString() + ".xlsx"));
                    list.Clear();
                    index = 1;
                }
            }
        }



        /// <summary>
        /// <para>二维：已知圆上三点，求圆心坐标</para>
        /// <para>三个点不在同一直线上</para>
        /// </summary>
        /// <param name="P1">点1</param>
        /// <param name="P2">点2</param>
        /// <param name="P3">点3</param>
        /// <returns></returns>
        public static PointF GetCentre(PointF P1, PointF P2, PointF P3)
        {
            double a13 = P1.X - P3.X;
            double a13_ = P1.X + P3.X;
            double b13 = P1.Y - P3.Y;
            double b13_ = P1.Y + P3.Y;
            double a12 = P1.X - P2.X;
            double a12_ = P1.X + P2.X;
            double b12 = P1.Y - P2.Y;
            double b12_ = P1.Y + P2.Y;

            double a12b12_2 = a12 * a12_ + b12 * b12_;
            double a13b13_2 = a13 * a13_ + b13 * b13_;

            double a13b12 = 2 * a13 * b12;
            double a12b13 = 2 * a12 * b13;


            if (a12b13 - a13b12 == 0)
            {
                return new PointF((P2.X + P1.X) / 2, (P2.Y + P1.Y) / 2);
            }
            
            double af = a12b13 - a13b12;
            double bf = a13b12 - a12b13;
            double az = b13 * a12b12_2 - b12 * a13b13_2;
            double bz = a13 * a12b12_2 - a12 * a13b13_2;
            double a = az / af;
            double b = bz / bf;
            return new PointF((float)a, (float)b);
        }

        /// <summary>
        /// <para>二维：已知圆上三点，求圆心坐标</para>
        /// <para>三个点不在同一直线上</para>
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="p3">点3</param>
        /// <returns>圆心</returns>
        public static AKRSPoint3D GetCentre(AKRSPoint3D p1, AKRSPoint3D p2, AKRSPoint3D p3)
        {
            // 定位
            double z = (p1.Z + p2.Z + p3.Z) / 3;

            PointF center = GetCentre(
                new PointF((float)p1.X, (float)p1.Y),
                new PointF((float)p2.X, (float)p2.Y),
                new PointF((float)p3.X, (float)p3.Y));

            return new AKRSPoint3D(center.X, center.Y, z);
        }

        /// <summary>
        /// 循环次数
        /// </summary>
        /// <param name="totalNumber">总个数</param>
        /// <param name="cycleNumber">循环个数</param>
        /// <returns>结果</returns>
        public static List<List<int>> GetCycleList(int totalNumber, int cycleNumber)
        {
            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < totalNumber; i++)
            {
                List<int> cycleList = new List<int>();
                for (int j = 0; j < cycleNumber; j++)
                {
                    if (i < totalNumber)
                    {
                        cycleList.Add(i);
                        i++;
                    }
                }

                i--;
                list.Add(cycleList);
            }

            return list;
        }
    }
}
