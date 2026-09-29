using AKRS.Galaxy2.Infrastructure.CommonModel;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

/// <summary>
/// 插补参数
/// </summary>
public class ItpParam : BaseItpParam
{
    /// <summary>
    /// 点位名称
    /// </summary>
    public string PointName { get; set; }

    /// <summary>
    /// 目标位置
    /// </summary>
    public AKRSPoint3D TargetPoint { get; set; }

    /// <summary>
    /// ToString方法
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{this.PointName}";
    }
}