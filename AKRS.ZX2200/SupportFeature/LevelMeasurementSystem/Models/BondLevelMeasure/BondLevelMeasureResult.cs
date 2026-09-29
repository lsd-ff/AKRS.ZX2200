namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    /// <summary>
    /// 测量结果
    /// </summary>
    public class BondLevelMeasureResult
    {
        /// <summary>
        /// 移动到边缘处时，突针上方的点位设置为A点，A点默认是旋转0°的点
        /// </summary>
        [JsonIgnore]
        public double Height0 { get; set; }

        /// <summary>
        /// B点对应的测量高度，B点由A点旋转90°
        /// </summary>
        [JsonIgnore]
        public double Height90 { get; set; }

        /// <summary>
        /// C点对应的测量高度，C点由B点旋转90°
        /// </summary>
        [JsonIgnore]
        public double Height180 { get; set; }

        /// <summary>
        /// D点对应的测量高度，D点由C点旋转90°
        /// </summary>
        [JsonIgnore]
        public double Height270 { get; set; }

        /// <summary>
        /// 极差
        /// </summary>
        [JsonIgnore]
        public double MaxDifference { get; set; }

        /// <summary>
        /// 获取最小值和最大值的索引
        /// </summary>
        /// <returns>最小值索引和最大值索引</returns>
        public (double MinValue, double MaxValue) GetMinMaxValue()
        {
            List<double> list = new List<double> { this.Height0, this.Height90, this.Height180, this.Height270 };

            double minValue = list.Min();
            double maxValue = list.Max();

            return (minValue, maxValue);    
        }
    }
}
