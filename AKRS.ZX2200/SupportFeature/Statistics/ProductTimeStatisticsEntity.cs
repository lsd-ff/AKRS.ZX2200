using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    /// <summary>
    /// 统计时长耗时
    /// </summary>
    public class ProductTimeStatisticsEntity
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "StartTime", IsNullable = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "EndTime", IsNullable = false)]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Tu的序号
        /// </summary>
        [SugarColumn(ColumnName = "ProductTime", IsNullable = false)]
        public double ProductTime { get; set; }


        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ProductTimeStatisticsEntity()
        {
        }

        /// <summary>
        /// 自动工作开始的统计
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="productTime">工作时间</param>
        public ProductTimeStatisticsEntity(DateTime startTime, DateTime endTime, double productTime)
        {
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.ProductTime = productTime;
        }
    }
}
