namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System1Related
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;

    /// <summary>
    /// 系统1平面水平测试点位数据集
    /// </summary>
    [Serializable]
    public class System1FlatLevelMeasureSetting : BaseDsSetting
    {
        /// <summary>
        /// 系统1平面水平测试点位列表，关于G0
        /// </summary>
        public List<AKRSPoint2D> FlatLevelMeasurePointList { get; set; } = new List<AKRSPoint2D>();

        /// <summary>
        /// 点位测高高度，关于G0
        /// </summary>
        public double ZG0Pos { get; set; }
    }
}
