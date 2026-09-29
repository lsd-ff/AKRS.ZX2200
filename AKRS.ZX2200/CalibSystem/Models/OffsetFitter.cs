using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.CalibSystem.Models;
using AKRS.ZX2200.Infrastructure.Models.Path;
using MathNet.Numerics.LinearRegression;
using System;
using System.Collections.Generic;

/// <summary>
/// 偏移模型拟合工具（线性/二次多项式拟合）
/// </summary>
public class OffsetFitter : Singleton<OffsetFitter>
{
    // 线性回归系数：a=斜率，b=截距
    public (double A, double B) DxCoeffs { get; private set; } // dx = A*ZCam + B

    public (double A, double B) DyCoeffs { get; private set; } // dy = A*ZCam + B

    public List<(double, double, double)> calibData { get; private set; } = new List<(double, double, double)>();

    public OffsetFitter()
    {
        Singleton<OffsetFitter>.FilePath = ZX2200PathConfig.WaferCameraAndEjectorCalibratePara;
    }

    /// <summary>
    /// 手动计算线性回归系数（对dx和dy分别拟合）
    /// </summary>
    /// <param name="calibData">标定数据：(ZCam, Dx, Dy)</param>
    public void Fit(List<(double ZCam, double Dx, double Dy)> calibData)
    {
        // 1. 验证数据有效性
        if (calibData == null || calibData.Count < 2)
            throw new ArgumentException("标定点数量不足，至少需要2组数据（n≥2）");

        // 提取x（ZCam）、y1（Dx）、y2（Dy）的数组
        int n = calibData.Count;
        double[] x = new double[n];
        double[] y1 = new double[n]; // dx数组
        double[] y2 = new double[n]; // dy数组
        for (int i = 0; i < n; i++)
        {
            x[i] = calibData[i].ZCam;
            y1[i] = calibData[i].Dx;
            y2[i] = calibData[i].Dy;
        }

        // 2. 分别拟合dx和dy的线性系数
        DxCoeffs = CalculateCoeffs(x, y1, "Dx");
        DyCoeffs = CalculateCoeffs(x, y2, "Dy");

        // 输出拟合结果
        Console.WriteLine("基于最小二乘法的线性回归拟合完成：");
        Console.WriteLine($"dx = {DxCoeffs.A:F6} * ZCam + {DxCoeffs.B:F6}");
        Console.WriteLine($"dy = {DyCoeffs.A:F6} * ZCam + {DyCoeffs.B:F6}");
    }

    /// <summary>
    /// 核心：手动计算单组（x,y）的线性系数（a=斜率，b=截距）
    /// </summary>
    /// <param name="x">自变量数组（ZCam）</param>
    /// <param name="y">因变量数组（Dx/Dy）</param>
    /// <param name="type">拟合类型（日志标识）</param>
    /// <returns>线性系数（a, b）</returns>
    private (double A, double B) CalculateCoeffs(double[] x, double[] y, string type)
    {
        int n = x.Length;
        double sumX = 0, sumY = 0;
        double sumXY = 0, sumX2 = 0;

        // 3. 计算核心求和项（避免重复遍历数组，提升效率）
        for (int i = 0; i < n; i++)
        {
            sumX += x[i];          // Σx
            sumY += y[i];          // Σy
            sumXY += x[i] * y[i];  // Σxy
            sumX2 += x[i] * x[i];  // Σx²
        }

        // 4. 计算x和y的均值
        double avgX = sumX / n;   // x̄ = Σx / n
        double avgY = sumY / n;   // ȳ = Σy / n

        // 5. 计算协方差（cov(x,y)）和x的方差（var(x)）
        double covXY = sumXY - n * avgX * avgY;  // cov(x,y) = Σxy - n*x̄*ȳ
        double varX = sumX2 - n * avgX * avgX;   // var(x) = Σx² - n*x̄²

        // 6. 避免除以零（x全为同一个值时，varX=0，无法拟合）
        if (Math.Abs(varX) < 1e-10)
            throw new InvalidOperationException($"[{type}] 自变量x（ZCam）无波动，无法拟合线性模型（varX≈0）");

        // 7. 求解斜率a和截距b
        double a = covXY / varX;                  // a = cov(x,y) / var(x)
        double b = avgY - a * avgX;               // b = ȳ - a*x̄

        // 输出单组拟合结果
        Console.WriteLine($"[{type}] 线性系数计算完成：斜率a={a:F6}，截距b={b:F6}");
        return (a, b);
    }

    /// <summary>
    /// 基于拟合系数，预测指定x（ZCam）对应的y（Dx/Dy）值
    /// </summary>
    public (double Dx, double Dy) Predict(double zCam)
    {
        double dx = DxCoeffs.A * zCam + DxCoeffs.B;
        double dy = DyCoeffs.A * zCam + DyCoeffs.B;
        return (dx / 1000.0, dy / 1000.0);
    }

    /// 根据指定的ZCam值，从标定数据列表中查找ZCam最接近的记录，返回其Dx和Dy
    /// </summary>
    /// <param name="zCam">目标ZCam值</param>
    /// <returns>最接近ZCam值对应的(Dx, Dy)  mm</returns>
    /// <exception cref="InvalidOperationException">当标定数据列表为空时抛出</exception>
    public (double Dx, double Dy) GetNearestOffset(double zCam)
    {
        // 验证标定数据是否存在
        if (calibData == null || calibData.Count == 0)
            throw new InvalidOperationException("标定数据列表为空，无法查找最接近的偏移值");

        // 初始化最小差值和对应的结果
        double minDiff = double.MaxValue;
        (double Dx, double Dy) nearestOffset = (0, 0);

        // 遍历所有标定数据，寻找ZCam差值最小的记录
        foreach (var data in calibData)
        {
            double currentDiff = Math.Abs(data.Item1 - zCam); // 计算当前ZCam与目标值的绝对差值
            if (currentDiff < minDiff)
            {
                minDiff = currentDiff;
                nearestOffset = (data.Item2/1000.0, data.Item3 / 1000.0); // 更新最接近的Dx和Dy
            }
        }

        Console.WriteLine($"找到与ZCam={zCam}最接近的标定数据，差值：{minDiff:F6}，对应Dx={nearestOffset.Dx:F6}，Dy={nearestOffset.Dy:F6}");
        return nearestOffset;
    }

    /// <summary>
    /// 验证拟合精度：计算预测值与真实值的平均绝对误差
    /// </summary>
    public (double DxAvgError, double DyAvgError) ValidateAccuracy(List<(double ZCam, double DxReal, double DyReal)> testData)
    {
        if (testData == null || testData.Count == 0)
            throw new ArgumentException("测试数据不能为空");

        double dxTotalError = 0, dyTotalError = 0;
        int testCount = testData.Count;

        foreach (var data in testData)
        {
            // 预测当前ZCam对应的偏移
            var (dxPred, dyPred) = Predict(data.ZCam);
            // 累计绝对误差（避免正负误差抵消）
            dxTotalError += Math.Abs(dxPred - data.DxReal);
            dyTotalError += Math.Abs(dyPred - data.DyReal);
        }

        // 计算平均误差
        double dxAvgError = dxTotalError / testCount;
        double dyAvgError = dyTotalError / testCount;

        Console.WriteLine($"\n拟合精度验证结果：");
        Console.WriteLine($"Dx平均绝对误差：{dxAvgError:F6} mm");
        Console.WriteLine($"Dy平均绝对误差：{dyAvgError:F6} mm");
        return (dxAvgError, dyAvgError);
    }
}