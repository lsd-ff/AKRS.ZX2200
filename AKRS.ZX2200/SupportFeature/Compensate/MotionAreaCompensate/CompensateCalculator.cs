using System;
using System.Linq;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 鲁棒补偿计算器 完全对齐Excel公式逻辑
    /// </summary>
    public static class CompensateCalculator
    {
        // Huber算法默认参数
        private const double DefaultC = 1.345;
        private const double MadToSigma = 1.4826;
        private const double DefaultAlpha = 0.85; // 修正Excel中alpha=85的笔误
        private const int DefaultSmoothWindow = 5;

        /// <summary>
        /// 执行完整补偿计算
        /// </summary>
        /// <param name="rawOffsets">原始偏移数据 二维数组：[位置索引, 轮次索引]</param>
        /// <param name="rows">基板行数</param>
        /// <param name="cols">基板列数</param>
        /// <param name="alpha">残差修正系数 默认0.85</param>
        /// <param name="windowSize">平滑窗口大小 默认5</param>
        /// <returns>平滑后的二维补偿矩阵 [行,列]</returns>
        public static double[,] Calculate(double[,] rawOffsets, int rows, int cols,
            double alpha = DefaultAlpha, int windowSize = DefaultSmoothWindow)
        {
            int pointCount = rawOffsets.GetLength(0);
            int roundCount = rawOffsets.GetLength(1);

            if (pointCount != rows * cols)
                throw new ArgumentException($"位置数{pointCount}与行列数{rows}×{cols}不匹配");

            // 1. 逐位置Huber估计 得到c_old（对应Excel BYROW(mat, calc_huber)）
            double[] cOld = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                double[] roundData = GetRow(rawOffsets, i);
                cOld[i] = HuberEstimate(roundData);
            }

            // 2. 计算残差矩阵 并逐位置Huber得到公共残差common_resid
            double[,] residMat = new double[pointCount, roundCount];
            double[] commonResid = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
            {
                for (int j = 0; j < roundCount; j++)
                    residMat[i, j] = rawOffsets[i, j] - cOld[i];

                double[] residRow = GetRow(residMat, i);
                commonResid[i] = HuberEstimate(residRow);
            }

            // 3. 系统性偏差修正 c_new_raw = c_old - alpha * common_resid
            double[] cNewRaw = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                cNewRaw[i] = cOld[i] - alpha * commonResid[i];

            // 4. 一维转二维 还原基板阵列布局 [行,列]
            double[,] mat2D = WrapTo2D(cNewRaw, rows, cols);

            // 5. 行方向滑动中位数平滑
            double[,] smoothed = SmoothMedianRowWise(mat2D, windowSize);

            return smoothed;
        }

        /// <summary>
        /// Huber M估计 完全对齐Excel公式逻辑
        /// </summary>
        public static double HuberEstimate(double[] data, double c = DefaultC)
        {
            if (data == null || data.Length == 0) return 0;
            if (data.Length == 1) return data[0];

            double med = Median(data);
            double[] absDev = data.Select(x => Math.Abs(x - med)).ToArray();
            double mad = Median(absDev);
            double sigma = mad == 0 ? 0 : mad * MadToSigma;

            if (sigma == 0) return med;

            double sumW = 0;
            double sumXw = 0;
            foreach (double x in data)
            {
                double u = Math.Abs(x - med) / sigma;
                double w = u <= c ? 1 : c / u;
                sumW += w;
                sumXw += x * w;
            }

            return sumXw / sumW;
        }

        /// <summary>
        /// 中位数
        /// </summary>
        public static double Median(double[] data)
        {
            if (data == null || data.Length == 0) return 0;
            double[] sorted = (double[])data.Clone();
            Array.Sort(sorted);
            int n = sorted.Length;
            return n % 2 == 0 ? (sorted[n / 2 - 1] + sorted[n / 2]) / 2 : sorted[n / 2];
        }

        /// <summary>
        /// 行方向滑动中位数平滑
        /// </summary>
        private static double[,] SmoothMedianRowWise(double[,] mat, int windowSize)
        {
            int rows = mat.GetLength(0);
            int cols = mat.GetLength(1);
            int half = windowSize / 2;
            double[,] result = new double[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int start = Math.Max(0, c - half);
                    int end = Math.Min(cols - 1, c + half);
                    int len = end - start + 1;
                    double[] window = new double[len];
                    for (int k = 0; k < len; k++)
                        window[k] = mat[r, start + k];
                    result[r, c] = Median(window);
                }
            }
            return result;
        }

        // 辅助：取二维数组的一行
        private static double[] GetRow(double[,] mat, int row)
        {
            int cols = mat.GetLength(1);
            double[] res = new double[cols];
            for (int j = 0; j < cols; j++)
                res[j] = mat[row, j];
            return res;
        }

        // 辅助：一维数组按行优先转二维
        private static double[,] WrapTo2D(double[] data, int rows, int cols)
        {
            double[,] res = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    res[i, j] = data[i * cols + j];
            return res;
        }
    }
}
