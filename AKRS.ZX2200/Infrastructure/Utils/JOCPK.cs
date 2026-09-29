using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKRS.ZX2200.Infrastructure.Utils
{
    /// <summary>
    /// 计算CPK公式
    /// </summary>
    public class JOCPK
    {
        /// <summary>
        /// 计算标准偏差
        /// </summary>
        /// <param name="arrData">参数</param>
        /// <returns>结果</returns>
        public static double StDev(double[] arrData)
        {
            double xSum = 0;
            double xAvg = 0;
            double sSum = 0;
            double tmpStDev = 0;
            int arrNum = arrData.Length;
            for (int i = 0; i < arrNum; i++)
            {
                xSum += arrData[i];
            }

            xAvg = xSum / arrNum;
            for (int j = 0; j < arrNum; j++)
            {
                sSum += (arrData[j] - xAvg) * (arrData[j] - xAvg);
            }

            tmpStDev = Math.Sqrt(sSum / (arrNum - 1));
            return tmpStDev;
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="arrData">数据</param>
        /// <returns>结果</returns>
        public static double Average(double[] arrData)
        {
            double tmpSum = 0;
            for (int i = 0; i < arrData.Length; i++)
            {
                tmpSum += arrData[i];
            }

            return tmpSum / arrData.Length;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="arrData">数据</param>
        /// <returns>结果</returns>
        public static double Max(double[] arrData)
        {
            double tmpMax = arrData[0];
            for (int i = 0; i < arrData.Length; i++)
            {
                if (tmpMax < arrData[i])
                {
                    tmpMax = arrData[i];
                }
            }

            return tmpMax;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="arrData">数据</param>
        /// <returns>结果</returns>
        public static double Min(double[] arrData)
        {
            double tmpMin = arrData[0];
            for (int i = 1; i < arrData.Length; i++)
            {
                if (tmpMin > arrData[i])
                {
                    tmpMin = arrData[i];
                }
            }

            return tmpMin;
        }

        /// <summary>
        /// 上线
        /// </summary>
        /// <param name="upperLimit">上线值</param>
        /// <param name="average">均值</param>
        /// <param name="stDev">标准差</param>
        /// <returns>结果</returns>
        public static double CpkU(double upperLimit, double average, double stDev)
        {
            double tmpV = upperLimit - average;
            return tmpV / (3 * stDev);
        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <param name="lowerLimit">上线值</param>
        /// <param name="average">均值</param>
        /// <param name="stDev">标准差</param>
        /// <returns>结果</returns>
        public static double CpkL(double lowerLimit, double average, double stDev)
        {
            double tmpV = average - lowerLimit;
            return tmpV / (3 * stDev);
        }

        /// <summary>
        /// 计算CPK
        /// </summary>
        /// <param name="cpkU">上线</param>
        /// <param name="cpkL">下线</param>
        /// <returns>结果</returns>
        public static double Cpk(double cpkU, double cpkL)
        {
            return Math.Abs(Math.Min(cpkU, cpkL));
        }

        /// <summary>
        /// 计算CPK
        /// </summary>
        /// <param name="array">数据</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns>结果</returns>
        public static double Cpk(double[] array, double max, double min)
        {
            double average = Average(array);
            double stDev = StDev(array);

            double cpkU = CpkU(max, average, stDev);
            double cpkL = CpkL(min, average, stDev);

            return Cpk(cpkU, cpkL);
        }

        /// <summary>
        /// 计算3西格玛
        /// </summary>
        /// <param name="array">数据</param>
        /// <returns>结果</returns>
        public static double ThreeStDev(double[] array)
        {
            return 3 * StDev(array);
        }

        /// <summary>
        /// 合格率
        /// </summary>
        /// <param name="array">数据</param>
        /// <param name="max">最大值</param>
        /// <param name="min">最小值</param>
        /// <returns>结果</returns>
        public static double QualifiedPercentage(double[] array, double max, double min)
        {
            double count = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > max || array[i] < min)
                {
                    count++;
                }
            }

            return 1.0 - count / array.Length;
        }
    }
}
