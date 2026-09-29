using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels
{
    /// <summary>
    /// 焊点完成日志模型
    /// </summary>
    [SugarTable("BondPositionLogEntity")]
    public class BondPositionLogEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        [Description("主键")]
        public int Id { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        [SugarColumn(ColumnName = "DateTime", IsNullable = false)]
        [Description("完成时间")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 焊点名称
        /// </summary>
        [SugarColumn(ColumnName = "BondPositionName", IsNullable = false)]
        [Description("焊点名称")]
        public string BondPositionName { get; set; }

        /// <summary>
        /// 基岛名称
        /// </summary>
        [SugarColumn(ColumnName = "ModuleName", IsNullable = false)]
        [Description("基岛名称")]
        public string ModuleName { get; set; }

        /// <summary>
        /// 基板名称
        /// </summary>
        [SugarColumn(ColumnName = "SubstrateName", IsNullable = false)]
        [Description("基板名称")]
        public string SubstrateName { get; set; }

        /// <summary>
        /// 载具名称
        /// </summary>
        [SugarColumn(ColumnName = "TransportUnitName", IsNullable = false)]
        [Description("载具名称")]
        public string TransportUnitName { get; set; }

        #region 焊点的过程参数

        /// <summary>
        /// 芯片名称
        /// </summary>
        [SugarColumn(ColumnName = "ComponentName", IsNullable = false)]
        [Description("芯片名称")]
        public string ComponentName { get; set; }

        /// <summary>
        /// 芯片取片压力
        /// </summary>
        [SugarColumn(ColumnName = "ComponentPickPressure", IsNullable = true)]
        [Description("芯片取片压力")]
        public double ComponentPickPressure { get; set; }

        /// <summary>
        /// 芯片固晶压力
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBondPressure", IsNullable = true)]
        [Description("芯片固晶压力")]
        public double ComponentBondPressure { get; set; }

        /// <summary>
        /// 芯片取片前二段高度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforePickSecondHeight", IsNullable = true)]
        [Description("芯片取片前二段高度")]
        public double ComponentBeforePickSecondHeight { get; set; }

        /// <summary>
        /// 芯片固晶前二段高度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforeBondSecondHeight", IsNullable = true)]
        [Description("芯片固晶前二段高度")]
        public double ComponentBeforeBondSecondHeight { get; set; }

        /// <summary>
        /// 芯片取片前二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforePickSecondSpeed", IsNullable = true)]
        [Description("芯片取片前二段速度")]
        public double ComponentBeforePickSecondSpeed { get; set; }

        /// <summary>
        /// 芯片固晶前二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforeBondSecondSpeed", IsNullable = true)]
        [Description("芯片固晶前二段速度")]
        public double ComponentBeforeBondSecondSpeed { get; set; }

        /// <summary>
        /// 芯片取片停留时间
        /// </summary>
        [SugarColumn(ColumnName = "ComponentPickDelay", IsNullable = true)]
        [Description("芯片取片停留时间 (毫秒)")]
        public int ComponentPickDelay { get; set; }

        /// <summary>
        /// 芯片固晶停留时间
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBondDelay", IsNullable = true)]
        [Description("芯片固晶停留时间 (毫秒)")]
        public int ComponentBondDelay { get; set; }

        /// <summary>
        /// 芯片取片后二段高度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterPickSecondHeight", IsNullable = true)]
        [Description("芯片取片后二段高度")]
        public double ComponentAfterPickSecondHeight { get; set; }

        /// <summary>
        /// 芯片固晶后二段高度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterBondSecondHeight", IsNullable = true)]
        [Description("芯片固晶后二段高度")]
        public double ComponentAfterBondSecondHeight { get; set; }

        /// <summary>
        /// 芯片取片后二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterPickSecondSpeed", IsNullable = true)]
        [Description("芯片取片后二段速度")]
        public double ComponentAfterPickSecondSpeed { get; set; }

        /// <summary>
        /// 芯片固晶后二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterBondSecondSpeed", IsNullable = true)]
        [Description("芯片固晶后二段速度")]
        public double ComponentAfterBondSecondSpeed { get; set; }

        /// <summary>
        /// 芯片蘸胶停留时间
        /// </summary>
        [SugarColumn(ColumnName = "ComponentGlueDelay", IsNullable = true)]
        [Description("芯片蘸胶停留时间 (毫秒)")]
        public int ComponentGlueDelay { get; set; }

        /// <summary>
        /// 芯片蘸胶停留压力
        /// </summary>
        [SugarColumn(ColumnName = "ComponentGluePressure", IsNullable = true)]
        [Description("芯片蘸胶停留压力")]
        public double ComponentGluePressure { get; set; }

        /// <summary>
        /// 芯片蘸胶前二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforeGlueSecondSpeed", IsNullable = true)]
        [Description("芯片蘸胶前二段速度")]
        public double ComponentBeforeGlueSecondSpeed { get; set; }

        /// <summary>
        /// 芯片蘸胶前二段距离
        /// </summary>
        [SugarColumn(ColumnName = "ComponentBeforeGlueSecondDistance", IsNullable = true)]
        [Description("芯片蘸胶前二段距离")]
        public double ComponentBeforeGlueSecondDistance { get; set; }

        /// <summary>
        /// 芯片蘸胶后二段速度
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterGlueSecondSpeed", IsNullable = true)]
        [Description("芯片蘸胶后二段速度")]
        public double ComponentAfterGlueSecondSpeed { get; set; }

        /// <summary>
        /// 芯片蘸胶后二段距离
        /// </summary>
        [SugarColumn(ColumnName = "ComponentAfterGlueSecondDistance", IsNullable = true)]
        [Description("芯片蘸胶后二段距离")]
        public double ComponentAfterGlueSecondDistance { get; set; }

        #endregion

        /// <summary>
        /// 无参构造
        /// </summary>
        public BondPositionLogEntity()
        {
        }

        /// <summary>
        /// 无参构造
        /// </summary>
        /// <param name="transportUnitName">载具名称</param>
        /// <param name="substrateName">基板名称</param>
        /// <param name="moduleName">基岛名称</param>
        /// <param name="bondPositionName">焊点名称</param>
        public BondPositionLogEntity(string transportUnitName, string substrateName, string moduleName, string bondPositionName)
        {
            this.TransportUnitName = transportUnitName;
            this.SubstrateName = substrateName;
            this.ModuleName = moduleName;
            this.BondPositionName = bondPositionName;
            this.DateTime = DateTime.Now;
        }
    }
}
