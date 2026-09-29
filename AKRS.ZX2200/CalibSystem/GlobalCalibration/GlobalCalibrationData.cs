using System;
using System.Collections.Generic;
using System.Linq;
using AKRS.ZX2200.Infrastructure.Service;
using Newtonsoft.Json;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

public class GlobalCalibrationData
{
    #region private fields

    private double worldX;
    private double worldY;
    private double axisX;
    private double axisY;
    private double scanAxisX;
    private double scanAxisY;

    #endregion

    /// <summary>
    /// 世界坐标X（标定板阵列点坐标系）
    /// </summary>
    public double WorldX
    {
        get => this.worldX;
        set => this.worldX = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// 世界坐标Y（标定板阵列点坐标系）
    /// </summary>
    public double WorldY
    {
        get => this.worldY;
        set => this.worldY = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// 轴坐标X（与标定板定位理论对应轴坐标）
    /// </summary>
    public double AxisX
    {
        get => this.axisX;
        set => this.axisX = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// 轴坐标Y（与标定板定位理论对应轴坐标）
    /// </summary>
    public double AxisY
    {
        get => this.axisY;
        set => this.axisY = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// 扫描时轴坐标X（扫描时轴坐标）
    /// </summary>
    public double ScanAxisX
    {
        get => this.scanAxisX;
        set => this.scanAxisX = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// 扫描时轴坐标Y（扫描时轴坐标）
    /// </summary>
    public double ScanAxisY
    {
        get => this.scanAxisY;
        set => this.scanAxisY = RoundToFixedDecimal(value);
    }

    /// <summary>
    /// X方向差值（扫描轴坐标与理论轴坐标之差）
    /// </summary>
    [JsonIgnore]
    public double OffsetX => MathService.DoubleEqual(0, this.ScanAxisX, this.ScanAxisY) ? 0 : RoundToFixedDecimal(this.ScanAxisX - this.AxisX);

    /// <summary>
    /// Y方向差值（扫描轴坐标与理论轴坐标之差）
    /// </summary>
    [JsonIgnore]
    public double OffsetY => MathService.DoubleEqual(0, this.ScanAxisX, this.ScanAxisY) ? 0 : RoundToFixedDecimal(this.ScanAxisY - this.AxisY);

    /// <summary>
    /// 排序，左下角到右上角的顺序，S型路径
    /// </summary>
    /// <param name="dataList"></param>
    /// <returns></returns>
    public static List<GlobalCalibrationData> Sort(List<GlobalCalibrationData> dataList)
    {
        // 按WorldY从小到大排序，再按WorldX从小到大排序
        List<GlobalCalibrationData> sortedList =
            dataList.OrderBy(data => data.WorldY).ThenBy(data => data.WorldX).ToList();
        List<double> yCoordinates = sortedList.Select(data => data.WorldY).Distinct().ToList();
        List<GlobalCalibrationData> sSortedList = new();
        for (int i = 0; i < yCoordinates.Count; i++)
        {
            List<GlobalCalibrationData> currentRow = sortedList.Where(data => data.WorldY == yCoordinates[i]).ToList();
            if (i % 2 == 1)
            {
                currentRow.Reverse();
            }

            sSortedList.AddRange(currentRow);
        }

        return sSortedList;
    }

    /// <summary>
    /// 四舍五入到指定小数位数
    /// </summary>
    /// <param name="value"></param>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    private static double RoundToFixedDecimal(double value, int decimalPlaces = 8)
    {
        return Math.Round(value, decimalPlaces);
    }
}