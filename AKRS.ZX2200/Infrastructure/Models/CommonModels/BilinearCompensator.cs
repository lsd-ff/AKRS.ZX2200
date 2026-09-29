using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.CalibSystem.GlobalCalibration;
using AKRS.ZX2200.Infrastructure.Service;
//using DevExpress.XtraMap.Drawing.DirectD3D9;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

/// <summary>
/// 双线性补偿区域
/// </summary>
public class BilinearCompensationRegion
{
    /// <summary>
    /// 补偿区域(行)
    /// </summary>
    [JsonProperty] private List<List<KeyValuePair<Coords2D, Coords2D>>> compensationRows = new();

    /// <summary>
    /// 补偿区域映射，用于显示
    /// </summary>
    [JsonIgnore]
    public List<KeyValuePair<Coords2D, Coords2D>> CompensationMap => this.compensationRows.SelectMany(r => r).ToList();

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public BilinearCompensationRegion()
    {
        
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="calibrationData">标定数据</param>
    /// <exception cref="ArgumentException">标定数据不足</exception>
    public BilinearCompensationRegion(List<GlobalCalibrationData> calibrationData)
    {
        if (calibrationData == null || calibrationData.Count < 4)
        {
            throw new ArgumentException("标定数据不足");
        }

        this.compensationRows = calibrationData
            .GroupBy(d => Math.Round(d.WorldY, 6))
            .OrderBy(g => g.Key)
            .Select(g => g.OrderBy(d => d.WorldX)
                .Select(d => new KeyValuePair<Coords2D, Coords2D>(
                    new(d.AxisX, d.AxisY),
                    new(d.ScanAxisX - d.AxisX, d.ScanAxisY - d.AxisY)))
                .ToList())
            .ToList();
    }


    private bool FindInterpolationCell(Coords2D pt, out KeyValuePair<Coords2D, Coords2D>[] cellCorners)
    {
        cellCorners = null;

        for (int i = 0; i < this.compensationRows.Count - 1; i++)
        {
            List<KeyValuePair<Coords2D, Coords2D>> rowA = this.compensationRows[i];
            List<KeyValuePair<Coords2D, Coords2D>> rowB = this.compensationRows[i + 1];

            int cols = Math.Min(rowA.Count, rowB.Count);

            for (int j = 0; j < cols - 1; j++)
            {
                KeyValuePair<Coords2D, Coords2D> p11 = rowA[j];
                KeyValuePair<Coords2D, Coords2D> p12 = rowA[j + 1];
                KeyValuePair<Coords2D, Coords2D> p21 = rowB[j];
                KeyValuePair<Coords2D, Coords2D> p22 = rowB[j + 1];

                if (this.IsPointInQuad(pt, p11.Key, p21.Key, p22.Key, p12.Key))
                {
                    cellCorners = new[] { p11, p21, p12, p22 };
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 尝试插值计算
    /// </summary>
    /// <param name="x">目标点位坐标X</param>
    /// <param name="y">目标点位坐标Y</param>
    /// <param name="result">目标点补偿值</param>
    /// <returns>True:计算成功 False:目标点位不在补偿区域中</returns>
    public bool TryInterpolate(double x, double y, out AKRSPoint2D result)
    {
        result = null;
        Coords2D pt = new(x, y);

        if (!this.FindInterpolationCell(pt, out KeyValuePair<Coords2D, Coords2D>[] corners))
        {
            return false;
        }

        result = this.BilinearInterpolation(pt, corners);
        return true;
    }

    /// <summary>
    /// 插值计算
    /// </summary>
    /// <param name="pt">目标点</param>
    /// <param name="corners">包含目标点的网格</param>
    /// <returns>补偿值</returns>
    /// <exception cref="InvalidOperationException">网格顶点异常</exception>
    private AKRSPoint2D BilinearInterpolation(Coords2D pt, KeyValuePair<Coords2D, Coords2D>[] corners)
    {
        KeyValuePair<Coords2D, Coords2D> q11 = corners[0];
        KeyValuePair<Coords2D, Coords2D> q21 = corners[1];
        KeyValuePair<Coords2D, Coords2D> q12 = corners[2];
        KeyValuePair<Coords2D, Coords2D> q22 = corners[3];

        double x1 = q11.Key.X, x2 = q22.Key.X;
        double y1 = q11.Key.Y, y2 = q22.Key.Y;

        double denom = (x2 - x1) * (y2 - y1);
        if (Math.Abs(denom) < 1e-10)
        {
            throw new InvalidOperationException("网格顶点异常");
        }

        double fxy1 = ((x2 - pt.X) * q11.Value.X + (pt.X - x1) * q21.Value.X) / (x2 - x1);
        double fxy2 = ((x2 - pt.X) * q12.Value.X + (pt.X - x1) * q22.Value.X) / (x2 - x1);
        double interpX = ((y2 - pt.Y) * fxy1 + (pt.Y - y1) * fxy2) / (y2 - y1);

        fxy1 = ((x2 - pt.X) * q11.Value.Y + (pt.X - x1) * q21.Value.Y) / (x2 - x1);
        fxy2 = ((x2 - pt.X) * q12.Value.Y + (pt.X - x1) * q22.Value.Y) / (x2 - x1);
        double interpY = ((y2 - pt.Y) * fxy1 + (pt.Y - y1) * fxy2) / (y2 - y1);

        return new(interpX, interpY);
    }

    /// <summary>
    /// 检查点是否在四边形内
    /// </summary>
    /// <param name="pt">目标点</param>
    /// <param name="a">顶点1</param>
    /// <param name="b">顶点2</param>
    /// <param name="c">顶点3</param>
    /// <param name="d">顶点4</param>
    /// <returns>判断结果</returns>
    private bool IsPointInQuad(Coords2D pt, Coords2D a, Coords2D b, Coords2D c, Coords2D d)
    {
        return this.SameSide(pt, a, b, c) && this.SameSide(pt, b, c, d) && this.SameSide(pt, c, d, a) &&
               this.SameSide(pt, d, a, b);
    }

    /// <summary>
    /// 判断点是否在同一侧
    /// </summary>
    /// <param name="p">目标点</param>
    /// <param name="a">点1</param>
    /// <param name="b">点2</param>
    /// <param name="refPoint">参考点</param>
    /// <returns>判断结果</returns>
    private bool SameSide(Coords2D p, Coords2D a, Coords2D b, Coords2D refPoint)
    {
        double cp1 = this.Cross(b - a, p - a);
        double cp2 = this.Cross(b - a, refPoint - a);
        return cp1 * cp2 >= -1e-10;
    }

    /// <summary>
    /// 计算叉积
    /// </summary>
    /// <param name="a">点1</param>
    /// <param name="b">点2</param>
    /// <returns>结果</returns>
    private double Cross(Coords2D a, Coords2D b)
    {
        return a.X * b.Y - a.Y * b.X;
    }
}

/// <summary>
/// 表示一个补偿区域
/// </summary>
public class RegionCompensator
{
    /// <summary>
    /// 补偿区域
    /// </summary>
    public BilinearCompensationRegion Region { get; set; }

    /// <summary>
    /// 补偿区域名称
    /// </summary>
    public string RegionName { get; set; }

    /// <summary>
    /// 补偿区域的优先级
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 检查点是否在区域内
    /// </summary>
    /// <param name="x">x坐标</param>
    /// <param name="y">y坐标</param>
    /// <returns>是否在区域内</returns>
    public bool Contains(double x, double y)
    {
        return this.Region.TryInterpolate(x, y, out AKRSPoint2D _);
    }
}

/// <summary>
/// 双线性补偿器
/// </summary>
public class BilinearCompensator
{
    /// <summary>
    /// 补偿区域列表
    /// </summary>
    [JsonProperty] private List<RegionCompensator> regionCompensators = new();

    /// <summary>
    /// 补偿区域列表
    /// </summary>
    [JsonIgnore]
    public List<RegionCompensator> Regions => this.regionCompensators;

    /// <summary>
    /// 默认构造函数
    /// </summary>
    public BilinearCompensator()
    {
    }

    /// <summary>
    /// 添加补偿区域
    /// </summary>
    /// <param name="regionName">区域名称</param>
    /// <param name="region">补偿区域</param>
    /// <param name="priority">优先级</param>
    public void AddRegion(string regionName, BilinearCompensationRegion region, int priority)
    {
        this.regionCompensators.Add(new()
        {
            RegionName = regionName,
            Region = region,
            Priority = priority
        });

        this.Sort();
    }

    /// <summary>
    /// 对补偿区域进行排序
    /// </summary>
    public void Sort()
    {
        this.regionCompensators = this.regionCompensators
            .OrderByDescending(r => r.Priority)
            .ToList();
    }

    /// <summary>
    /// 移除补偿区域
    /// </summary>
    /// <param name="region"></param>
    public void RemoveRegion(RegionCompensator region)
    {
        this.regionCompensators.Remove(region);
    }

    /// <summary>
    /// 清除所有补偿区域
    /// </summary>
    public void ClearRegions()
    {
        this.regionCompensators.Clear();
    }

    /// <summary>
    /// 插值计算
    /// </summary>
    /// <param name="x">目标点X坐标</param>
    /// <param name="y">目标点Y坐标</param>
    /// <returns>补偿值</returns>
    public AKRSPoint2D Interpolate(double x, double y)
    {
        // 按优先级从高到低依次查找
        foreach (RegionCompensator regionCompensator in this.regionCompensators.Where(regionCompensator =>
                     regionCompensator.Contains(x, y)))
        {
            bool status = regionCompensator.Region.TryInterpolate(x, y, out AKRSPoint2D result);
            return result;
        }

        // 如果未找到合适的补偿区域，返回默认值
        return new(0, 0);
    }
}

/// <summary>
/// 二维坐标点
/// 仅用于二维补偿
/// </summary>
public class Coords2D
{
    /// <summary>
    /// X坐标
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Y坐标
    /// </summary>
    public double Y { get; set; }

    public Coords2D()
    {
    }

    public Coords2D(double x, double y)
    {
        this.X = x;
        this.Y = y;
    }

    public override bool Equals(object obj)
    {
        if (obj is Coords2D coords)
        {
            return Math.Abs(Math.Round(this.X, 6) - Math.Round(coords.X, 6)) < 1e-6 &&
                   Math.Abs(Math.Round(this.Y, 6) - Math.Round(coords.Y, 6)) < 1e-6;
        }

        return false;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int num = 16;

            num += Math.Round(this.X, 3).GetHashCode();
            num += Math.Round(this.Y, 3).GetHashCode();

            return num;
        }
    }

    public static Coords2D operator -(Coords2D a, Coords2D b)
    {
        return new(a.X - b.X, a.Y - b.Y);
    }

    public Vector2 ToVector2()
    {
        return new((float)this.X, (float)this.Y);
    }

    public static explicit operator Coords2D(Vector2 vec)
    {
        return new(vec.X, vec.Y);
    }

    public static implicit operator Coords2D(AKRSPoint2D coords)
    {
        return new(coords.X, coords.Y);
    }

    public override string ToString()
    {
        return $"{this.X},{this.Y}";
    }

    public AKRSPoint2D ToPoint2D()
    {
        return new(this.X, this.Y);
    }
}