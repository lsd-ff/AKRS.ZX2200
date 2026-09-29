using SqlSugar;
using System;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    /// 报警日志模型
    /// </summary>
    [SugarTable("AlarmLog")]
    public class AlarmLogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)] 
        public int Id { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        [SugarColumn(ColumnName = "StartTime", IsNullable = false)] 
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 报警信息
        /// </summary>
        [SugarColumn(ColumnName = "Message", IsNullable = true)] 
        public string Message { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        [SugarColumn(ColumnName = "HandleTime", IsNullable = false)]  
        public DateTime HandleTime { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        [SugarColumn(ColumnName = "HandleType", IsNullable = true)] 
        public string HandleType { get; set; }

        /// <summary>
        /// 故障代码
        /// </summary>
        [SugarColumn(ColumnName = "AlarmCode", IsNullable = false)] 
        public int AlarmCode { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        [SugarColumn(ColumnName = "Category", IsNullable = true)]
        public string Category { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public AlarmLogEntity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="message"></param>
        /// <param name="handleType"></param>
        /// <param name="handleTime"></param>
        /// <param name="alarmCode"></param>
        public AlarmLogEntity(DateTime startTime, string message, string handleType, DateTime handleTime,
            int alarmCode = -1)
        {
            this.StartTime = startTime;
            this.Message = message;
            this.HandleTime = handleTime;
            this.AlarmCode = alarmCode;
            this.HandleType = handleType;
        }
    }
}