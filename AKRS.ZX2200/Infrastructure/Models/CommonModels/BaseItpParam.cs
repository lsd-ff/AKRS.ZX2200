namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

/// <summary>
/// 插补参数基类
/// </summary>
public class BaseItpParam
{
    /// <summary>
    /// 速度
    /// </summary>
    public double Speed { get; set; }

    /// <summary>
    /// 加速度
    /// </summary>
    public double AccTime { get; set; }

    /// <summary>
    /// 加加速度
    /// </summary>
    public double JerkTime { get; set; }

    /// <summary>
    /// 转角系数
    /// </summary>
    public double AheadTime { get; set; }

    /// <summary>
    /// 曲率半径
    /// </summary>
    public double AheadRadiusRatio { get; set; }

    /// <summary>
    /// ToString方法
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return
            $"速度：{this.Speed},加速度{this.AccTime},加加速度{this.JerkTime},转角系数{this.AheadTime},曲率半径{this.AheadRadiusRatio}";
    }
}