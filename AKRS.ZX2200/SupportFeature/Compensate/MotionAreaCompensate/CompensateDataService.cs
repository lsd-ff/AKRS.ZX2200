using System;
using System.Collections.Generic;
using System.Linq;
using AKRS.ZX2200.Main.Machine.Product;
using AKRS.ZX2200.SupportFeature.Statistics;

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 轮次数据分组实体
    /// </summary>
    public class RoundGroup
    {
        public int TuIndex { get; set; }
        public int SubIndex { get; set; }
        public DateTime RoundTime { get; set; }
        public int PointCount { get; set; }
        public List<DefectStatisticsEntity> DataList { get; set; }
    }

    /// <summary>
    /// 补偿数据查询服务
    /// </summary>
    public class CompensateDataService
    {
        private readonly int _pointPerRound;
        // 补偿计算对应的缺陷名称，和统计界面下拉框里的名称保持一致
        private const string DefectName = "贴片精度"; // ← 替换成你实际的缺陷项名称

        public CompensateDataService()
        {
            var config = ProductDomain.GetInstance().ProductConfig.SubstrateConfig;
            _pointPerRound = config.RowCount * config.ColumnCount;
        }

        /// <summary>
        /// 获取指定时间点之后的所有轮次分组
        /// </summary>
        public List<RoundGroup> GetRoundsAfterTime(DateTime startTime)
        {
            // 直接复用项目现有统计查询方法，和统计界面数据完全同源
            List<DefectStatisticsEntity> allData = StatisticsDomain.GetInstance().QueryDefect(
                DefectName,
                startTime,
                DateTime.MaxValue); // 结束时间设为最大值，即查开始时间之后的所有数据

            if (allData == null || allData.Count == 0)
                return new List<RoundGroup>();

            // 按 TuIndex + SubIndex + 日期 分组，识别每一轮
            return allData
                .GroupBy(x => new { x.TuIndex, x.SubIndex, x.DateTime.Date })
                .Select(g => new RoundGroup
                {
                    TuIndex = g.Key.TuIndex,
                    SubIndex = g.Key.SubIndex,
                    RoundTime = g.Min(x => x.DateTime),
                    PointCount = g.Count(),
                    DataList = g.OrderBy(x => x.ModuleIndex).ToList()
                })
                .OrderBy(x => x.RoundTime)
                .ToList();
        }

        /// <summary>
        /// 获取单轮数据点数
        /// </summary>
        public int GetPointPerRound() => _pointPerRound;
    }
}
