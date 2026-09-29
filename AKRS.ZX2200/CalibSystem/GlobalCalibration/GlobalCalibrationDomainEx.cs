using System.Linq;

using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Service;

using Newtonsoft.Json;

namespace AKRS.ZX2200.CalibSystem.GlobalCalibration
{
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    /// <summary>
    /// 全局标定类
    /// </summary>
    public class GlobalCalibrationDomainEx : Singleton<GlobalCalibrationDomainEx>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static GlobalCalibrationDomainEx()
        {
            GlobalCalibrationDomainEx.FilePath = ZX2200PathConfig.GlobalCalibrationDomainPathEx;
        }

        /// <summary>
        /// 开始点位
        /// </summary>
        public AKRSPoint3D StartPoint3D { get; set; } = new AKRSPoint3D();

        /// <summary>
        /// 列间距
        /// </summary>
        public double ColumnSpacing { get; set; }

        /// <summary>
        /// 行间距Y
        /// </summary>
        public double RowSpacing { get; set; }

        /// <summary>
        /// 左侧列数
        /// </summary>
        public double LeftColCount { get; set; }

        /// <summary>
        /// 右侧列数
        /// </summary>
        public double RightColCount { get; set; }

        /// <summary>
        /// 上方行数
        /// </summary>
        public double UpRowCount { get; set; }

        /// <summary>
        /// 下方行数
        /// </summary>
        public double DownRowCount { get; set; }

        /// <summary>
        /// PR的名称
        /// </summary>
        public string PrName { get; set; } = "GlobalCalibrationProgram";

        /// <summary>
        /// 拍照的延迟
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// 扫描点位列表
        /// </summary>
        public List<GlobalCalibrationData> ScanData { get; set; } = new List<GlobalCalibrationData>();

        /// <summary>
        /// 扫描完成标志
        /// </summary>
        [JsonIgnore]
        public bool ScanCompletedFlag => this.ScanData
            .SelectMany(d => new double[] { d.ScanAxisX, d.ScanAxisY }).Count(d => d == 0) < 3;

        /// <summary>
        /// 补偿器
        /// </summary>
        public BilinearCompensator Compensator { get; set; } = new BilinearCompensator();
    }
}
