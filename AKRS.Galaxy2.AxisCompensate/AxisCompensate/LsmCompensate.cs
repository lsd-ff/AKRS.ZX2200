using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.Galaxy2.AxisCompensate.AxisCompensate
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// 最小二乘法补偿
    /// 主要是将直线全部映射到曲线上面去
    /// </summary>
    public class LsmCompensate : BaseCompensate
    {
        /// <summary>
        /// 补偿构造函数
        /// </summary>
        /// <param name="axisName">轴名称</param>
        /// <param name="calibrationCoefficient">系数数组</param>
        /// <param name="startPos">开始点</param>
        /// <param name="endPos">结束点</param>
        public LsmCompensate(string axisName, double[] calibrationCoefficient, double startPos, double endPos)
        {
            this.Name = axisName;
            this.CalibrationCoefficient = calibrationCoefficient;
            this.StartPos = startPos;
            this.EndPos = endPos;
        }

        /// <summary>
        /// 补偿系数
        /// </summary>
        public double[] CalibrationCoefficient { get; set; }

        /// <summary>
        /// 获取轴补偿之后的位置
        /// </summary>
        /// <param name="axisPos">轴目标位置</param>
        /// <returns>轴补偿之后的位置</returns>
        public override double GetOffset(double axisPos)
        {
            double realPos = MathService.GetPolynomialValue(
                this.CalibrationCoefficient.Length - 1,
                axisPos,
                this.CalibrationCoefficient);

            return realPos;
        }
    }
}
