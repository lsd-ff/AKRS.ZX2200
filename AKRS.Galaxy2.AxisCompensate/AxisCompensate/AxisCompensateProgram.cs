using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalCalibration.GlobalCalibration
{
    using AKRS.Galaxy2.AxisCompensate.AxisCompensate;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.MachineSupport.Config;
    using MathNet.Numerics;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// 全局标定程式
    /// </summary>
    public class AxisCompensateProgram : Singleton<AxisCompensateProgram>
    {
        /// <summary>
        /// 补偿的集合
        /// </summary>
        public List<BaseCompensate> CompensateList { get; set; } = new List<BaseCompensate>();

        /// <summary>
        /// 轴补偿方法是否开启
        /// </summary>
        public bool IsAxisCompensateOpen { get; set; } = false;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static AxisCompensateProgram()
        {
            AxisCompensateProgram.FilePath = Path.Combine(PathConfig.DeviceDirPath, "AxisCompensateProgram.json");
        }

        /// <summary>
        /// 获取补偿值
        /// </summary>
        /// <param name="axisName">轴名称</param>
        /// <param name="axisPos">轴的点位</param>
        /// <returns>结果</returns>
        public double GetOffset(string axisName, double axisPos)
        {
            BaseCompensate baseCompensate = this.CompensateList.Find(it => it.Name == axisName);

            if (baseCompensate == null)
            {
                throw new Exception("获取轴补偿值失败，请重试");
            }

            if (axisPos < (baseCompensate.StartPos + 0.5) || axisPos > (baseCompensate.EndPos - 0.5))
            {
                return axisPos;
            }

            double pos = baseCompensate.GetOffset(axisPos);

            if (Math.Abs(pos - axisPos) > 0.5)
            {
                throw new Exception("轴补偿值过大");
            }

            return pos;
        }

        /// <summary>
        /// 添加一个轴的补偿
        /// </summary>
        /// <param name="axisName">轴名称</param>
        /// <param name="realPosList">轴真实位置的值</param>
        /// <param name="axisPosList">轴的指令值</param>
        public void AddAxisCompensate(string axisName, List<double> realPosList, List<double> axisPosList)
        {
            try
            {
                // 数据处理
                double[] realAverageList = realPosList.ToArray();
                double[] axisAverageList = axisPosList.ToArray();
                
                int orderCount = Math.Min(axisAverageList.Length, realAverageList.Length) - 1; 

                double[] calibrationCoefficient = Fit.Polynomial(realAverageList, axisAverageList, orderCount);

                LsmCompensate lsmCompensate = new LsmCompensate(
                    axisName,
                    calibrationCoefficient,
                    axisAverageList.Min(),
                    axisAverageList.Max());

                BaseCompensate baseCompensate = this.CompensateList.Find(it => it.Name == axisName);

                if (baseCompensate != null)
                {
                    this.CompensateList.Remove(baseCompensate);
                }

                this.CompensateList.Add(lsmCompensate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        /// <summary>
        /// 添加一个轴的补偿
        /// </summary>
        /// <param name="axisName">轴名称</param>
        /// <param name="realPosList">轴点位的集合</param>
        /// <param name="axisPosList">轴真实位置的集合</param>
        public void AddAxisCompensate(string axisName, List<List<double>> realPosList, List<List<double>> axisPosList)
        {
            try
            {
                // 数据处理
                double[] realAverageList = MathService.GetListAverageValue(realPosList);
                double[] axisAverageList = MathService.GetListAverageValue(axisPosList);

                int orderCount = Math.Min(axisAverageList.Length, realAverageList.Length) - 1;

                if (orderCount > 30)
                {
                    orderCount = 15;
                }

                // 获取余数
                double[] calibrationCoefficient = Fit.Polynomial(realAverageList, axisAverageList, orderCount);

                LsmCompensate lsmCompensate = new LsmCompensate(
                    axisName,
                    calibrationCoefficient,
                    realAverageList.Min(),
                    realAverageList.Max());

                BaseCompensate baseCompensate = this.CompensateList.Find(it => it.Name == axisName);

                if (baseCompensate != null)
                {
                    this.CompensateList.Remove(baseCompensate);
                }

                this.CompensateList.Add(lsmCompensate);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
