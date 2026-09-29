namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

using System.Collections.Generic;
using System.Linq;

using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
using AKRS.Galaxy2.Infrastructure.CommonModel;

public class InterpolationParamConfig
{
    /// <summary>
    /// 指令流号 点胶是1 bond是2
    /// </summary>
    public short ListNo { get; set; } = 2;

    /// <summary>
    /// 插补组号 点胶是1 bond是2
    /// </summary>
    public int GrpCrd { get; set; } = 2;

    /// <summary>
    /// 安全高度路线
    /// </summary>
    public List<SingleInterpolationParam> BondZSafeRoute { get; set; } = new();

    /// <summary>
    /// 不安全高度路线
    /// </summary>
    public List<SingleInterpolationParam> BondZUnSafeRoute { get; set; } = new();

    /// <summary>
    /// 插补全局配置
    /// </summary>
    public InterpolationGlobalConfig InterpolationGlobalConfig { get; set; } = new();

    /// <summary>
    /// 是否使用全局配置
    /// </summary>

    private bool isUseGlobalConfig = true;

    /// <summary>
    /// 获取插补参数
    /// todo:判断条件也要传进来
    /// </summary>
    /// <param name="isZSafe"></param>
    /// <returns></returns>
    public InterpolationParam GetInterpolationParam(bool isZSafe, AKRSPoint4D targetPos)
    {
        InterpolationParam interpolationParam = new();
        interpolationParam.ListNo = this.ListNo;
        interpolationParam.GrpCrd = this.GrpCrd;
        List<SingleInterpolationParam> route = isZSafe ? this.BondZSafeRoute : this.BondZUnSafeRoute;
        SingleInterpolationParam firstParagraph = route.FirstOrDefault();
        interpolationParam.Vel = isUseGlobalConfig?this.InterpolationGlobalConfig.Velocity:firstParagraph.Velocity;
        interpolationParam.Acc = isUseGlobalConfig ? this.InterpolationGlobalConfig.AccTime : firstParagraph.AccTime;
        interpolationParam.AccAcc = isUseGlobalConfig ? this.InterpolationGlobalConfig.JeckTime : firstParagraph.JeckTime;

        interpolationParam.AheadParam = new AheadParam
                                            {
                                                Time = isUseGlobalConfig ? this.InterpolationGlobalConfig.AheadTime : firstParagraph.AheadTime,
                                                RadiusRatio = isUseGlobalConfig ? this.InterpolationGlobalConfig.AheadRadiusRatio : firstParagraph.AheadRadiusRatio
                                            };

        return interpolationParam;
    }

    /// <summary>
    /// 这个是干嘛用的?
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    private SegmentConfig[] GetSegmentConfig(List<SingleInterpolationParam> route)
    {
        return route.Select(p => new SegmentConfig
                                     {
                                         Acc = isUseGlobalConfig ? this.InterpolationGlobalConfig.AccTime : p.AccTime,
                                         AccAcc = isUseGlobalConfig ? this.InterpolationGlobalConfig.JeckTime : p.JeckTime,
                                         Velocity = isUseGlobalConfig ? this.InterpolationGlobalConfig.Velocity : p.Velocity,
                                         Point = new AKRSPoint4D(p.TargetPoint.X, p.TargetPoint.Y, p.TargetPoint.Z, p.TargetPoint.X)
                                     }).ToArray();
    }
}

public class InterpolationGlobalConfig
{
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