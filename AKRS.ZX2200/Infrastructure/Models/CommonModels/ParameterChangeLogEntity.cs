using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    ///  参数改变日志
    /// </summary>
    [SugarTable("ParameterChangeLog")]
    public class ParameterChangeLogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        [SugarColumn(ColumnName = "DateTime", IsNullable = false)]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 操作员
        /// </summary>
        [SugarColumn(ColumnName = "Technician", IsNullable = true)]
        public string Technician { get; set; } = string.Empty;

        /// <summary>
        /// 更改信息
        /// </summary>
        [SugarColumn(ColumnName = "Message", IsNullable = true)]
        public string Message { get; set; }

        /// <summary>
        /// 参数名称
        /// </summary>
        [SugarColumn(ColumnName = "ParameterName", IsNullable = true)]
        public string ParameterName { get; set; }

        /// <summary>
        /// 无参构造
        /// </summary>
        public ParameterChangeLogEntity()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTime">发生时间</param>
        /// <param name="technician">操作者</param>
        /// <param name="parameterName">参数名称</param>
        /// <param name="message">信息</param>
        public ParameterChangeLogEntity(DateTime dateTime, string technician, string parameterName,  string message)
        {
            this.DateTime = dateTime;
            this.Message = message;
            this.Technician = technician;
            this.ParameterName = parameterName;
        }
    }
}
