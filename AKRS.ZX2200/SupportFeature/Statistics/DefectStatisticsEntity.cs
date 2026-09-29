using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    /// <summary>
    /// 精度统计实体对象
    /// </summary>
    public class DefectStatisticsEntity
    {
        /// <summary>
        /// 时间
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "Time", IsNullable = false)]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, ColumnName = "DefectName", IsNullable = false)]
        public string DefectName { get; set; }

        /// <summary>
        /// Tu的序号
        /// </summary>
        [SugarColumn(ColumnName = "TuIndex", IsNullable = false)]
        public int TuIndex { get; set; }

        /// <summary>
        /// Tu的序号
        /// </summary>
        [SugarColumn(ColumnName = "ID", IsNullable = true)]
        public string Id { get; set; }

        /// <summary>
        /// sub的序号
        /// </summary>
        [SugarColumn(ColumnName = "SubIndex", IsNullable = false)]
        public int SubIndex { get; set; }

        /// <summary>
        /// module的序号
        /// </summary>
        [SugarColumn(ColumnName = "ModuleIndex", IsNullable = false)]
        public int ModuleIndex { get; set; }

        /// <summary>
        /// 焊点名称
        /// </summary>
        [SugarColumn(ColumnName = "BondPositionName", IsNullable = false)]
        public string BondPositionName { get; set; }

        /// <summary>
        /// 精度X
        /// </summary>
        [SugarColumn(ColumnName = "OffsetX", IsNullable = false)]
        public double OffsetX { get; set; }

        /// <summary>
        /// 精度Y
        /// </summary>
        [SugarColumn(ColumnName = "OffsetY", IsNullable = false)]
        public double OffsetY { get; set; }

        /// <summary>
        /// 精度角度
        /// </summary>
        [SugarColumn(ColumnName = "OffsetAngle", IsNullable = false)]
        public double OffsetAngle { get; set; }

        /// <summary>
        /// 胶量面积
        /// </summary>
        [SugarColumn(ColumnName = "Area", IsNullable = false)]
        public double Area { get; set; }

        /// <summary>
        /// 温漂偏移值X
        /// </summary>
        [SugarColumn(ColumnName = "TpOffsetX", IsNullable = true)]
        public double TpOffsetX { get; set; }

        /// <summary>
        /// 温漂偏移值Y
        /// </summary>
        [SugarColumn(ColumnName = "TpOffsetY", IsNullable = true)]
        public double TpOffsetY { get; set; }

        /// <summary>
        /// 上视偏移值X
        /// </summary>
        [SugarColumn(ColumnName = "UpLookOffsetX", IsNullable = true)]
        public double UpLookOffsetX { get; set; }

        /// <summary>
        /// 上视偏移值Y
        /// </summary>
        [SugarColumn(ColumnName = "UpLookOffsetY", IsNullable = true)]
        public double UpLookOffsetY { get; set; }

        /// <summary>
        /// 中转台偏移值X
        /// </summary>
        [SugarColumn(ColumnName = "SubstrateCameraCompensateX", IsNullable = true)]
        public double SubstrateCameraCompensateX { get; set; }

        /// <summary>
        /// 中转台偏移值Y
        /// </summary>
        [SugarColumn(ColumnName = "SubstrateCameraCompensateY", IsNullable = true)]
        public double SubstrateCameraCompensateY { get; set; }

        /// <summary>
        /// 焊点的真实贴片位置
        /// </summary>
        [SugarColumn(ColumnName = "BondPositionX", IsNullable = true)]
        public double BondPositionX { get; set; }

        /// <summary>
        /// 焊点的真实贴片位置
        /// </summary>
        [SugarColumn(ColumnName = "BondPositionY", IsNullable = true)]
        public double BondPositionY { get; set; }

        /// <summary>
        /// 焊点偏移值
        /// </summary>
        [SugarColumn(ColumnName = "BpOffsetX", IsNullable = true)]
        public double BpOffsetX { get; set; }

        /// <summary>
        /// 焊点偏移值
        /// </summary>
        [SugarColumn(ColumnName = "BpOffsetY", IsNullable = true)]
        public double BpOffsetY { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <param name="defectName">名称</param>
        /// <param name="id">Tu的编号</param>
        /// <param name="subIndex">基板的索引</param>
        /// <param name="moduleIndex">基岛的索引</param>
        /// <param name="bondPositionName">焊点名称</param>
        /// <param name="offsetX">精度X</param>
        /// <param name="offsetY">精度Y</param>
        /// <param name="offsetAngle">精度角度</param>
        /// <param name="area">精度面积</param>
        /// <param name="tpOffsetX">温漂补偿X</param>
        /// <param name="tpOffsetY">温漂补偿Y</param>
        /// <param name="upLookOffsetX">上视补偿X</param>
        /// <param name="upLookOffsetY">上视补偿Y</param>
        /// <param name="bondPositionX">焊点真实位置X</param>
        /// <param name="bondPositionY">焊点真实位置Y</param>
        /// <param name="bpOffsetX">焊点设置偏移X</param>
        /// <param name="bpOffsetY">焊点设置偏移Y</param>
        /// <param name="substrateCameraCompensateX">中转台补偿X</param>
        /// <param name="substrateCameraCompensateY">中转台补偿Y</param>
        public DefectStatisticsEntity(DateTime dateTime, string defectName, string id, int subIndex, int moduleIndex, string bondPositionName, double offsetX, double offsetY, double offsetAngle, double area, double tpOffsetX, double tpOffsetY, double upLookOffsetX, double upLookOffsetY, double bondPositionX, double bondPositionY, double bpOffsetX, double bpOffsetY,double substrateCameraCompensateX,double substrateCameraCompensateY)
        {
            this.DateTime = dateTime;
            this.DefectName = defectName;
            this.Id = id;
            this.SubIndex = subIndex;
            this.ModuleIndex = moduleIndex;
            this.BondPositionName = bondPositionName;
            this.OffsetX = offsetX;
            this.OffsetY = offsetY;
            this.OffsetAngle = offsetAngle;
            this.Area = area;
            this.TpOffsetX = tpOffsetX;
            this.TpOffsetY = tpOffsetY;
            this.UpLookOffsetX = upLookOffsetX;
            this.UpLookOffsetY = upLookOffsetY;
            this.BondPositionX = bondPositionX;
            this.BondPositionY = bondPositionY;
            this.BpOffsetX = bpOffsetX;
            this.BpOffsetY = bpOffsetY;
            this.SubstrateCameraCompensateX = substrateCameraCompensateX;
            this.SubstrateCameraCompensateY = substrateCameraCompensateY;
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public DefectStatisticsEntity()
        {

        }
    }
}
