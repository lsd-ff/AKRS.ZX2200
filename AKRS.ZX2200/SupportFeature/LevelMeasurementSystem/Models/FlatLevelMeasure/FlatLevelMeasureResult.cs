namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 水平测量结果，关于G0，不保存
    /// </summary>
    public class FlatLevelMeasureResult
    {
        /// <summary>
        /// 点位1测量的高度值
        /// </summary>
        public double Height1 { get; set; }

        /// <summary>
        /// 点位2测量的高度值
        /// </summary>
        public double Height2 { get; set; }

        /// <summary>
        /// 点位3测量的高度值
        /// </summary>
        public double Height3 { get; set; }

        /// <summary>
        /// 点位4测量的高度值
        /// </summary>
        public double Height4 { get; set; }

        /// <summary>
        /// 点位5测量的高度值
        /// </summary>
        public double Height5 { get; set; }

        /// <summary>
        /// 点位6测量的高度值
        /// </summary>
        public double Height6 { get; set; }

        /// <summary>
        /// 点位7测量的高度值
        /// </summary>
        public double Height7 { get; set; }

        /// <summary>
        /// 点位8测量的高度值
        /// </summary>
        public double Height8 { get; set; }

        /// <summary>
        /// 点位9测量的高度值
        /// </summary>
        public double Height9 { get; set; }

        /// <summary>
        /// 点位10测量的高度值
        /// </summary>
        public double Height10 { get; set; }

        /// <summary>
        /// 极差
        /// </summary>
        public double Range { get; set; }

        /// <summary>
        /// 获取最小值和最大值的索引
        /// </summary>
        /// <returns>最小值索引和最大值索引</returns>
        public (double MinValue, double MaxValue) GetMinMaxValue()
        {
            List<double> list = new List<double> { this.Height1, this.Height2, this.Height3, this.Height4, this.Height5, this.Height6, this.Height7, this.Height8, this.Height9, this.Height10 };

            double minValue = list.Min();
            double maxValue = list.Max();

            return (minValue, maxValue);
        }
    }
}
