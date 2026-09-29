using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    using System.Threading;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Models.Path;

    using Newtonsoft.Json;

    /// <summary>
    /// 风冷控制器
    /// </summary>
    public class AirCooledController : Singleton<AirCooledController>
    {
        /// <summary>
        /// 风冷存储参数
        /// </summary>
        public AirCooledController()
        {
            Singleton<AirCooledController>.FilePath = ZX2200PathConfig.AirCooledControllerPath;
        }

        /// <summary>
        /// 比例
        /// </summary>
        public double Kp { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public double Ki { get; set; }

        /// <summary>
        /// 微分
        /// </summary>
        public double Kd { get; set; }

        /// <summary>
        /// 反馈量
        /// </summary>
        private double prevError, integralError;

        /// <summary>
        /// 最小值
        /// </summary>
        public double OutputMin { get; set; }

        /// <summary>
        /// 最大值
        /// </summary>
        public double OutputMax { get; set; }

        /// <summary>
        /// 间隔时间
        /// </summary>
        public int IntervalTime { get; set; } = 100;

        /// <summary>
        /// 目标值
        /// </summary>
        public double TargetValue { get; set; }

        /// <summary>
        /// 暂停
        /// </summary>
        [JsonIgnore]
        public bool IsWorking { get; set; }

        /// <summary>
        /// 计算输出值
        /// </summary>
        /// <param name="target">目标值</param>
        /// <param name="feedback">返回值</param>
        /// <param name="deltaTime">时间</param>
        /// <returns>结果</returns>
        public double Calculate(double target, double feedback, double deltaTime)
        {
            double error = target - feedback;
            double derivativeError = (error - this.prevError) / deltaTime;
            double output = this.Kp * error + this.Ki * this.integralError + this.Kd * derivativeError;

            // 输出限制
            if (output > this.OutputMax)
            {
                output = this.OutputMax;
            }
            else if (output < this.OutputMin)
            {
                output = this.OutputMin;
            }

            // 更新反馈变量
            this.prevError = error;
            this.integralError += error * deltaTime;
            return output;
        }

        /// <summary>
        /// 重置PID控制器
        /// </summary>
        public void Reset()
        {
            this.prevError = 0;
            this.integralError = 0;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            // 获取轴温度传感器的值
            double feedback = 0;

            CommonUtil.SetCurrentThreadName("风冷控温线程");

            while (this.IsWorking)
            {
                // 计算PID计算后的参数
                double output = this.Calculate(this.TargetValue, feedback, this.IntervalTime);

                feedback += output;

                // 输入给风冷模拟量
                Thread.Sleep(this.IntervalTime);
            }

            this.prevError = 0;
            this.integralError = 0;
        }
    }
}
