using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.AxisCompensate.AxisCompensate
{
    /// <summary>
    /// 补偿父类
    /// </summary>
    public abstract class BaseCompensate
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 补偿开始点
        /// </summary>
        public double StartPos { get; set; }

        /// <summary>
        /// 补偿结束点
        /// </summary>
        public double EndPos { get; set; }

        /// <summary>
        /// 获取轴的补偿值
        /// </summary>
        /// <param name="axisPos">轴坐标</param>
        /// <returns>真实轴的位置位置坐标</returns>
        public abstract double GetOffset(double axisPos);
    }
}
