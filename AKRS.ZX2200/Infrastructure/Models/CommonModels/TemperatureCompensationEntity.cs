using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    /// 温度补偿
    /// </summary>
    public class TemperatureCompensationEntity
    {
        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Time", IsNullable = false)]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 结果X
        /// </summary>
        [SugarColumn(ColumnName = "ResultX")]
        public double ResultX { get; set; }

        /// <summary>
        /// 结果Y
        /// </summary>
        [SugarColumn(ColumnName = "ResultY")]
        public double ResultY { get; set; }

        /// <summary>
        /// 结果角度
        /// </summary>
        [SugarColumn(ColumnName = "ResultAngle")]
        public double ResultAngle { get; set; }

        /// <summary>
        /// 结果X
        /// </summary>
        [SugarColumn(ColumnName = "AxisResultX")]
        public double AxisResultX { get; set; }

        /// <summary>
        /// 结果Y
        /// </summary>
        [SugarColumn(ColumnName = "AxisResultY")]
        public double AxisResultY { get; set; }

        /// <summary>
        /// 温度补偿实体
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="resultX">结果X</param>
        /// <param name="resultY">结果Y</param>
        /// <param name="resultAngle">角度</param>
        /// <param name="axisResultX">轴结果X</param>
        /// <param name="axisResultY">轴结果Y</param>
        public TemperatureCompensationEntity(DateTime dateTime, double resultX, double resultY, double resultAngle, double axisResultX, double axisResultY)
        {
            this.DateTime = dateTime;
            this.ResultX = resultX;
            this.ResultY = resultY;
            this.ResultAngle = resultAngle;
            this.AxisResultX = axisResultX;
            this.AxisResultY = axisResultY;
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public TemperatureCompensationEntity()
        {
        }
    }
}
