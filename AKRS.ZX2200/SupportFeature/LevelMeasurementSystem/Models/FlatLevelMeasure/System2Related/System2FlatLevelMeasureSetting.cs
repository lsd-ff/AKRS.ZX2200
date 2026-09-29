using System.Collections.Generic;

namespace AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Models;
    using System;

    /// <summary>
    /// 系统2平面水平测试点位参数，关于G0
    /// </summary>
    [Serializable]
    public class System2FlatLevelMeasureSetting : BaseDsSetting
    {
        /// <summary>
        /// 系统2平面水平测试点位列表
        /// </summary>
        public List<AKRSPoint2D> FlatLevelMeasurePointList { get; set; } = new List<AKRSPoint2D>();

        /// <summary>
        /// 是否为激光测高
        /// </summary>
        public bool IsLaser { get; set; } = false;

        /// <summary>
        /// 是否收回气缸
        /// </summary>
        public bool IsBackCy { get; set; } = true;

        /// <summary>
        /// 点位测高高度，高度为G0的高度
        /// </summary>
        public double ZG0Pos { get; set; }
    }
}
