using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.AxisCompensate.AxisCompensate
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// 轴的标定
    /// </summary>
    public class SegmentedCompensation
    {
        /// <summary>
        /// 标定的集合
        /// </summary>
        public List<double>[] CalibrationList { get; set; } = new List<double>[80];

        /// <summary>
        /// 间距
        /// </summary>
        public double Spacing { get; set; } = 3;

        /// <summary>
        /// 起始的位置
        /// </summary>
        public double StartPos { get; set; } = 0;

        /// <summary>
        /// 轴的位置
        /// </summary>
        /// <param name="axisPos">轴目标位置</param>
        /// <returns>轴补偿之后的位置</returns>
        public double GetOffset(double axisPos)
        {
            // 判断X轴是否在此补偿区域
            if (axisPos < this.StartPos || axisPos > this.StartPos + this.Spacing * this.CalibrationList.Length)
            {
                return axisPos;
            }

            // 如果在此补偿区域，将进行补偿
            return axisPos;
        }
    }
}
