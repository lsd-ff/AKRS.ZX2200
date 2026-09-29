using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure;

    using DevExpress.XtraCharts;

    /// <summary>
    /// 温度曲线图
    /// </summary>
    public partial class FrmAxisTemperature : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 轴温度曲线
        /// </summary>
        public FrmAxisTemperature()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 温度控制器
        /// </summary>
        private AirCooledController AirCooledController => AirCooledController.GetInstance();

        /// <summary>
        /// 线程加载
        /// </summary>
        private Task task;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAxisTemperature_Load(object sender, EventArgs e)
        {
            this.chartControl1.Series[0].Points.Clear();
            this.chartControl1.Series[0].Name = "X温度曲线";
            this.chartControl1.Series[1].Points.Clear();
            this.chartControl1.Series[1].Name = "Y温度曲线";

            this.SpKp.EditValue = this.AirCooledController.Kp;
            this.SpKi.EditValue = this.AirCooledController.Ki;
            this.SpKd.EditValue = this.AirCooledController.Kd;
            this.SpMax.EditValue = this.AirCooledController.OutputMax;
            this.SpMin.EditValue = this.AirCooledController.OutputMin;
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        private void StartAcquisitionCurves()
        {
            this.chartControl1.Series[0].Points.Clear();
            this.chartControl1.Series[1].Points.Clear();
            this.task.Dispose();

            this.task = Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("温度采集曲线");

                        while (this.BtStart.Checked && !this.IsDisposed && !this.Disposing)
                        {
                            // 读取温度
                            this.chartControl1.Series[0].Points.Add(
                                new SeriesPoint(this.chartControl1.Series[0].Points.Count * 100, 100));

                            // 读取温度
                            this.chartControl1.Series[1].Points.Add(
                                new SeriesPoint(this.chartControl1.Series[0].Points.Count * 100, 100));
                            Thread.Sleep(100);
                        }
                    });
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAxisTemperature_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.task.Dispose();
        }

        /// <summary>
        /// 开始采集温度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStart_CheckedChanged(object sender, EventArgs e)
        {
            if (this.BtStart.Checked)
            {
                this.StartAcquisitionCurves();
            }
        }

        /// <summary>
        /// 开启风冷控温
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkTemperatureControl_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkTemperatureControl.Checked)
            {
                this.AirCooledController.IsWorking = true;
                this.AirCooledController.Start();
            }
            else
            {
                this.AirCooledController.IsWorking = true;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            this.AirCooledController.Kp = (double)this.SpKp.Value;
            this.AirCooledController.Ki = (double)this.SpKi.Value;
            this.AirCooledController.Kd = (double)this.SpKd.Value;
            this.AirCooledController.OutputMax = (double)this.SpMax.Value;
            this.AirCooledController.OutputMin = (double)this.SpMin.Value;
            this.AirCooledController.Save();
        }
    }
}