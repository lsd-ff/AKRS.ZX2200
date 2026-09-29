namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

using System;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using Newtonsoft.Json;

public class SingleInterpolationParam
{
    /// <summary>
    /// 点位名称
    /// </summary>
    public string PointName { get; set; }

    /// <summary>
    /// 目标位置
    /// </summary>
    public AKRSPoint4D TargetPoint { get; set; }

    /// <summary>
    /// 速度
    /// </summary>
    public double Velocity { get; set; }

    /// <summary>
    ///  加速时间
    /// </summary>
    public double AccTime { get; set; }

    /// <summary>
    /// 加加速时间
    /// </summary>
    public double JeckTime { get; set; }

    /// <summary>
    /// 转角系数
    /// </summary>
    public double AheadTime { get; set; } = 0.1;

    /// <summary>
    /// 曲率半径
    /// </summary>
    public double AheadRadiusRatio { get; set; } = 0.5;
}

