using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    // **************************** 产能统计,半小时的产量 ******************* //

    /// <summary>
    /// 产能统计模型
    /// </summary>
    [SugarTable("CapacityLog")]
    public class CapacityLogEntity
    {
        ///// <summary>
        ///// 索引
        ///// </summary>
        //[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        //public int Index { get; set; }

        /// <summary>
        /// 统计开始时间
        /// </summary>
        [SugarColumn(ColumnName = "StartTime", IsNullable = false)]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 总共生产的数量
        /// </summary>
        [SugarColumn(ColumnName = "TotalNumber", IsNullable = false)]
        public int TotalNumber { get; set; } = 0;

        /// <summary>
        /// 无参构造
        /// </summary>
        public CapacityLogEntity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="totalNumber">贴片总数量</param>
        public CapacityLogEntity(DateTime startTime, int totalNumber)
        {
            this.StartTime = startTime;
            this.TotalNumber = totalNumber;
        }
    }
}
