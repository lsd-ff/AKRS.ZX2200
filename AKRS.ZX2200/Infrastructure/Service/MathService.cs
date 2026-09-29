using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Models.MatchResults;

using MathNet.Numerics.LinearAlgebra;


namespace AKRS.ZX2200.Infrastructure.Service;

public static class MathService
{
    /// <summary>
    /// 计算标准差
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>标准差</returns>
    public static double Std(this IEnumerable<double> sourceData)
    {
        IEnumerable<double> enumerable = sourceData as double[] ?? sourceData.ToArray();
        double mean = enumerable.Average();
        double sum = enumerable.Sum(d => Math.Pow(d - mean, 2));
        return Math.Sqrt(sum / enumerable.Count());
    }

    /// <summary>
    /// IQR法剔除异常值
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static List<double> RemoveOutliersIQR(List<double> data)
    {
        // 排序数据
        List<double> sortedData = data.OrderBy(x => x).ToList();

        // 计算Q1和Q3
        double q1 = GetPercentile(sortedData, 25);
        double q3 = GetPercentile(sortedData, 75);

        // 计算IQR
        double iqr = q3 - q1;

        // 计算异常值的阈值
        double lowerBound = q1 - 1.5 * iqr;
        double upperBound = q3 + 1.5 * iqr;

        // 去除异常值
        return sortedData.Where(x => x >= lowerBound && x <= upperBound).ToList();
    }

    /// <summary>
    /// 计算指定百分位数的值
    /// </summary>
    /// <param name="sortedData"></param>
    /// <param name="percentile"></param>
    /// <returns></returns>
    private static double GetPercentile(List<double> sortedData, double percentile)
    {
        int index = (int)(percentile / 100 * (sortedData.Count - 1));
        return sortedData[index];
    }

    /// <summary>
    /// 剔除异常数据，基于均值和标准差
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<AKRSPoint2D> RemoveAbnormalDataMeanStd(this IEnumerable<AKRSPoint2D> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomMeanStd(new string[] { "X", "Y" });
    }

    /// <summary>
    /// 剔除异常数据，基于四分位距法
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<AKRSPoint2D> RemoveAbnormalDataIQR(this IEnumerable<AKRSPoint2D> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomIQR(new string[] { "X", "Y" });
    }

    /// <summary>
    /// 剔除异常数据，基于均值和标准差
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<AKRSPoint3D> RemoveAbnormalDataMeanStd(this IEnumerable<AKRSPoint3D> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomMeanStd(new string[] { "X", "Y"});
    }

    /// <summary>
    /// 剔除异常数据，基于四分位距法
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<AKRSPoint3D> RemoveAbnormalDataIQR(this IEnumerable<AKRSPoint3D> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomIQR(new string[] { "X", "Y" });
    }

    /// <summary>
    /// 剔除异常数据
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<MatchResult> RemoveAbnormalDataMeanStd(this IEnumerable<MatchResult> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomMeanStd(new string[] { "CenterX", "CenterY" });
    }

    /// <summary>
    /// 剔除异常数据，基于四分位距法
    /// </summary>
    /// <param name="sourceData">源数据</param>
    /// <returns>剔除结果</returns>
    public static List<MatchResult> RemoveAbnormalDataIQR(this IEnumerable<MatchResult> sourceData)
    {
        return sourceData.RemoveAbnormalDataCustomIQR(new string[] { "CenterX", "CenterY" });
    }

    /// <summary>
    /// 自定义属性名的异常数据过滤（基于均值和标准差）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="sourceData">源数据</param>
    /// <param name="filterPropertiesName">需要过滤的属性值</param>
    /// <returns>去除异常数据后的结果</returns>
    /// <exception cref="ArgumentException">属性名不存在</exception>
    public static List<T> RemoveAbnormalDataCustomMeanStd<T>(this IEnumerable<T> sourceData, string[] filterPropertiesName)
    {
        if (sourceData == null || !sourceData.Any())
        {
            return new List<T>();
        }

        IEnumerable<T> enumerable = sourceData as T[] ?? sourceData.ToArray();
        foreach (string propertyName in filterPropertiesName)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                continue;
            }

            FieldInfo fieldInfo = typeof(T).GetField(propertyName);
            if (fieldInfo == null)
            {
                throw new ArgumentException($"属性名或字段名不存在：{propertyName}");
            }
        }

        // 根据指定的属性名计算均值
        double[] means = filterPropertiesName.Select(propertyName =>
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return enumerable.Average(item => (double)propertyInfo.GetValue(item)!);
            }
            FieldInfo fieldInfo = typeof(T).GetField(propertyName);
            return enumerable.Average(item => (double)fieldInfo.GetValue(item)!);
        }).ToArray();

        // 根据指定的属性名计算标准差
        double[] stds = filterPropertiesName.Select(propertyName =>
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                return enumerable.Select(item => (double)propertyInfo.GetValue(item)!).Std();
            }
            FieldInfo fieldInfo = typeof(T).GetField(propertyName);
            return enumerable.Select(item => (double)fieldInfo.GetValue(item)!).Std();
        }).ToArray();

        return enumerable.Where(item =>
        {
            double[] values = filterPropertiesName.Select(propertyName =>
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                if (propertyInfo != null)
                {
                    return (double)propertyInfo.GetValue(item)!;
                }
                FieldInfo fieldInfo = typeof(T).GetField(propertyName);
                return (double)fieldInfo.GetValue(item)!;
            }).ToArray();

            // 判断是否为异常数据（绝对值小于3倍标准差）
            return values.Select((value, index) => Math.Abs(value - means[index]) < 3 * stds[index]).All(b => b);
        }).ToList();
    }

    /// <summary>
    /// 自定义属性名的异常数据过滤（基于四分位距法）
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <param name="sourceData">源数据</param>
    /// <param name="filterPropertiesName">需要过滤的属性值</param>
    /// <returns>去除异常数据后的结果</returns>
    /// <exception cref="ArgumentException">属性名不存在</exception>
    public static List<T> RemoveAbnormalDataCustomIQR<T>(this IEnumerable<T> sourceData, params string[] filterPropertiesName)
    {
        if (sourceData == null || !sourceData.Any())
        {
            return new List<T>();
        }

        IEnumerable<T> enumerable = sourceData as T[] ?? sourceData.ToArray();
        foreach (string propertyName in filterPropertiesName)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo != null)
            {
                continue;
            }

            FieldInfo fieldInfo = typeof(T).GetField(propertyName);
            if (fieldInfo == null)
            {
                throw new ArgumentException($"属性名或字段名不存在：{propertyName}");
            }
        }

        return enumerable.Where(item =>
        {
            foreach (string propertyName in filterPropertiesName)
            {
                // 获取指定属性/字段的值
                PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
                double value;
                if (propertyInfo != null)
                {
                    value = (double)propertyInfo.GetValue(item)!;
                }
                else
                {
                    FieldInfo fieldInfo = typeof(T).GetField(propertyName);
                    value = (double)fieldInfo.GetValue(item)!;
                }

                // 获取指定属性/字段的值列表
                List<double> values = enumerable.Select(e =>
                {
                    if (propertyInfo != null)
                    {
                        return (double)propertyInfo.GetValue(e)!;
                    }
                    FieldInfo fieldInfo = typeof(T).GetField(propertyName);
                    return (double)fieldInfo.GetValue(e)!;
                }).ToList();

                // 去除异常值, 如果当前值不在去除异常值后的列表中，则返回false
                List<double> filteredValues = RemoveOutliersIQR(values);
                if (!filteredValues.Contains(value))
                {
                    return false;
                }
            }
            return true;
        }).ToList();
    }

    /// <summary>
    /// N组点计算传递矩阵
    /// </summary>
    /// <param name="srcPoints">原坐标系点位列表</param>
    /// <param name="dstPoints">目标坐标系点位列表</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static double[,] FitTransMatrix(List<AKRSPoint2D> srcPoints, List<AKRSPoint2D> dstPoints)
    {
        if (srcPoints.Count != dstPoints.Count || srcPoints.Count < 3)
        {
            throw new ArgumentException("至少设置三组点位或原坐标系点位数量与目标坐标系点位数量不一致！");
        }

        MatrixBuilder<double> matrixBuilder = Matrix<double>.Build;
        VectorBuilder<double> vectorBuilder = Vector<double>.Build;

        int n = srcPoints.Count;
        Matrix<double> extendedA = matrixBuilder.Dense(2 * n, 6);
        Vector<double> Y = vectorBuilder.Dense(2 * n);

        for (int i = 0; i < n; i++)
        {
            extendedA[2 * i, 0] = srcPoints[i].X;
            extendedA[2 * i, 1] = srcPoints[i].Y;
            extendedA[2 * i, 2] = 1;
            extendedA[2 * i + 1, 3] = srcPoints[i].X;
            extendedA[2 * i + 1, 4] = srcPoints[i].Y;
            extendedA[2 * i + 1, 5] = 1;

            Y[2 * i] = dstPoints[i].X;
            Y[2 * i + 1] = dstPoints[i].Y;
        }

        Vector<double> X = extendedA.PseudoInverse() * Y;

        Matrix<double> affineMatrix = matrixBuilder.DenseIdentity(3, 3);
        affineMatrix.SetRow(0, new[] { X[0], X[1], X[2] });
        affineMatrix.SetRow(1, new[] { X[3], X[4], X[5] });
        affineMatrix.SetRow(2, new[] { 0, 0, 1d });

        return affineMatrix.ToArray();
    }

    /// <summary>
    /// 点位变换
    /// </summary>
    /// <param name="point"></param>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static AKRSPoint2D TransformPoint(AKRSPoint2D point, double[,] matrix)
    {
        double x = matrix[0, 0] * point.X + matrix[0, 1] * point.Y + matrix[0, 2];
        double y = matrix[1, 0] * point.X + matrix[1, 1] * point.Y + matrix[1, 2];
        return new AKRSPoint2D(x, y);
    }

    /// <summary>
    /// 判断n个数是否都等于指定数字，精度固定1e-8
    /// </summary>
    /// <param name="num">目标数字</param>
    /// <param name="nums">待比较的数字</param>
    /// <returns></returns>
    public static bool DoubleEqual(double num,params double[] nums)
    {
        return nums.All(n => !(Math.Abs(num - n) > 1e-8));
    }

    /// <summary>
    /// 求极差
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static double GetRange(List<double> list)
    {
        if (list == null || list.Count == 0)
        {
            return 0;
        }

        double min = list.Min();
        double max = list.Max();
        return max - min;
    }

    /// <summary>
    /// 求极差
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static AKRSPoint2D GetRange(List<AKRSPoint2D> list)
    {
        if (list == null || list.Count == 0)
        {
            return new AKRSPoint2D(0, 0);
        }
        double minX = list.Min(p => p.X);
        double maxX = list.Max(p => p.X);
        double minY = list.Min(p => p.Y);
        double maxY = list.Max(p => p.Y);
        return new AKRSPoint2D(maxX - minX, maxY - minY);
    }

    /// <summary>
    /// 求极差
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static AKRSPoint3D GetRange(this List<AKRSPoint3D> list)
    {
        if (list == null || list.Count == 0)
        {
            return new AKRSPoint3D(0, 0,0);
        }
        double minX = list.Min(p => p.X);
        double maxX = list.Max(p => p.X);
        double minY = list.Min(p => p.Y);
        double maxY = list.Max(p => p.Y);
        double minZ = list.Min(p => p.Z);
        double maxZ = list.Max(p => p.Z);
        return new AKRSPoint3D(maxX - minX, maxY - minY, maxZ - minZ);
    }

    /// <summary>
    /// 求均值
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static AKRSPoint2D GetMean(List<AKRSPoint2D> list)
    {
        if (list == null || list.Count == 0)
        {
            return new AKRSPoint2D(0, 0);
        }
        double meanX = list.Average(p => p.X);
        double meanY = list.Average(p => p.Y);
        return new AKRSPoint2D(meanX, meanY);
    }

    /// <summary>
    /// 求均值
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static AKRSPoint3D GetMean(this List<AKRSPoint3D> list)
    {
        if (list == null || list.Count == 0)
        {
            return new AKRSPoint3D(0, 0,0);
        }
        double meanX = list.Average(p => p.X);
        double meanY = list.Average(p => p.Y);
        double meanZ = list.Average(p => p.Z);
        return new AKRSPoint3D(meanX, meanY,meanZ);
    }
}