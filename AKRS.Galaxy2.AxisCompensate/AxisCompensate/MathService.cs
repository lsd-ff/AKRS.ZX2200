using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.AxisCompensate.AxisCompensate
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    /// <summary>
    /// 数学算数类
    /// </summary>
    public class MathService
    {
        /// <summary>
        /// 获取多项式的值
        /// </summary>
        /// <param name="order">阶数</param>
        /// <param name="x">x坐标</param>
        /// <param name="polynomialArray">多项式系数数组</param>
        /// <returns>Y坐标</returns>
        public static double GetPolynomialValue(int order, double x, double[] polynomialArray) =>
            Enumerable.Range(0, order + 1)
                .Select(t => polynomialArray[t] * Math.Pow(x, t))
                .Sum();

        /// <summary>
        /// 获取集合的集合的平均值
        /// </summary>
        /// <param name="doubleList">平均值</param>
        /// <returns>集合的平均值</returns>
        public static double[] GetListAverageValue(List<List<double>> doubleList)
        {
            List<List<double>> list = new List<List<double>>();

            for (int i = 0; i < doubleList.Count; i++)
            {
                for (int j = 0; j < doubleList[i].Count; j++)
                {
                    if (list.Count < doubleList[i].Count)
                    {
                        list.Add(new List<double>());
                    }

                    list[j].Add(doubleList[i][j]);
                }
            }

            double[] averageList = new double[list.Count];

            for (int i = 0; i < list.Count; i++)
            {
                double average = list[i].Sum() / list[i].Count;
                averageList[i] = average;
            }

            return averageList;
        }


        /// <summary>
        /// 平面上的点拟合曲线
        /// </summary>
        /// <param name="pointFs">点位</param>
        /// <returns>圆心结果</returns>
        private static PointF FitCircle(List<PointF> pointFs)
        {
            Matrix<float> YMat;
            Matrix<float> RMat;
            Matrix<float> AMat;
            List<float> YLit = new List<float>();
            List<float[]> RLit = new List<float[]>();
            //------构建Y矩阵
            foreach (var pointF in pointFs)
                YLit.Add(pointF.X * pointF.X + pointF.Y * pointF.Y);
            float[,] Yarray = new float[YLit.Count, 1];
            for (int i = 0; i < YLit.Count; i++)
                Yarray[i, 0] = YLit[i];
            YMat = CreateMatrix.DenseOfArray<float>(Yarray);

            //构建R矩阵
            foreach (var pointF in pointFs)
                RLit.Add(new float[] { -pointF.X, -pointF.Y, -1 });
            float[,] Rarray = new float[RLit.Count, 3];
            for (int i = 0; i < RLit.Count; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Rarray[i, j] = RLit[i][j];
                }
            }
            RMat = CreateMatrix.DenseOfArray<float>(Rarray);
            Matrix<float> RTMat = RMat.Transpose();
            Matrix<float> RRTInvMat = (RTMat.Multiply(RMat)).Inverse();
            AMat = RRTInvMat.Multiply(RTMat.Multiply(YMat));

            float[,] Aarray = AMat.ToArray();
            float A = Aarray[0, 0];
            float B = Aarray[1, 0];
            float C = Aarray[2, 0];
            float CenterX = A / -2.0f;
            float CenterY = B / -2.0f;
            float CenterR = (float)(Math.Sqrt((A * A + B * B - 4 * C)) / 2.0f);
            return new PointF(CenterX, CenterY);
        }
    }
}
