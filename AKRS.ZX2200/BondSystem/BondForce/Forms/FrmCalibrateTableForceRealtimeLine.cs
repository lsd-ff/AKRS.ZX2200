using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.ControlServices;

using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 矫正台实时力控曲线
    /// </summary>
    public partial class FrmCalibrateTableForceRealtimeLine : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测量运行时间
        /// </summary>
        private Stopwatch sp;

        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool isPause = false;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService modbusService => ModbusService.GetInstance();

        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 初始化
        /// </summary>
        public FrmCalibrateTableForceRealtimeLine()
        {
            // 控件初始化
            this.InitializeComponent();
        }

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmModbus_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            if (!this.modbusService.ConnectManometer())
            {
                return;
            }

            this.sp = new Stopwatch();
            this.sp.Start();

            // 设置用RTU连接时XY轴的范围
            ChartService.SetYRange(this.ChartModbusRTUForce, 0, 800);
            this.AutoSetXWholeRange(this.ChartModbusRTUForce, 3000, 1000);

            this.task = Task.Run(() =>
                {
                    while (this.isClose == false)
                    {
                        if (this.isPause == false)
                        {
                            try
                            {
                                //int[] holdRegisters = this.modbusService.ReadManometer();

                                double read = this.system2Controller.GetCalibrateTableForceValue();

                                this.BeginInvoke(
                                    new Action(
                                        () =>
                                            {
                                                //for (int i = 0; i < read.Length; i++)
                                                {
                                                    // 力值显示
                                                    this.SpRTUBondForceRead.EditValue = read;

                                                    SeriesPoint p = new SeriesPoint(
                                                        this.sp.ElapsedMilliseconds,
                                                        read);

                                                    ChartService.AddPoint(this.ChartModbusRTUForce.Series[0], p);
                                                }

                                                // 自动设置x轴范围
                                                this.AutoSetXWholeRange(this.ChartModbusRTUForce, 2000, 9000);
                                            }));
                            }
                            catch (Exception e)
                            {
                                AKRSXtraMessageBox.Show($"读取标定台力出错：{e.ToString()}");
                              return;
                            }
                        }

                        Thread.Sleep(20);
                    }
                });
        }

        /// <summary>
        /// 在运行过程中自动设置X轴的区间范围
        /// </summary>
        /// <param name="chartControl">图表控件</param>
        /// <param name="retain">X轴保留几分钟的空白</param>
        /// <param name="xRange">范围</param>
        public void AutoSetXWholeRange(ChartControl chartControl, int retain, int xRange = 0)
        {
            try
            {
                WholeRange dia = ((SwiftPlotDiagram)chartControl.Diagram).AxisX.WholeRange;

                int minValue = dia.MinValue == null ? 0 : Convert.ToInt32(dia.MinValue);
                int maxValue = dia.MaxValue == null ? 0 : Convert.ToInt32(dia.MaxValue);

                // 如果当前时间>= 轴的最大时间
                if (this.sp.ElapsedMilliseconds >= maxValue)
                {
                    this.SetXWholeRange(chartControl, minValue, (int)this.sp.ElapsedMilliseconds + xRange);
                }

                // 如果X轴的最大时间 大于当前时间不足 2分钟
                if ((maxValue - this.sp.ElapsedMilliseconds) < retain)
                {
                    if (xRange == 0)
                    {
                        this.SetXWholeRange(chartControl, minValue, (int)this.sp.ElapsedMilliseconds + retain);
                    }
                    else
                    {
                        this.SetXWholeRange(
                            chartControl,
                            (int)this.sp.ElapsedMilliseconds - retain,
                            (int)this.sp.ElapsedMilliseconds + retain);
                    }
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 设置X轴范围
        /// </summary>
        /// <param name="chartControl1">图表控件</param>
        /// <param name="minValue">范围最小值</param>
        /// <param name="maxValue">范围最大值</param>
        public void SetXWholeRange(ChartControl chartControl1, int minValue, int maxValue)
        {
            WholeRange dia = ((SwiftPlotDiagram)chartControl1.Diagram).AxisX.WholeRange;
            dia.SetMinMaxValues(minValue, maxValue);
        }

        /// <summary>
        /// 开始/暂停
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (this.isPause == true)
            {
                // 开始采集
                this.ChartModbusRTUForce.Series[0].Points.Clear();
                this.SetXWholeRange(this.ChartModbusRTUForce, 0, 10000);
                this.sp.Restart();
                this.isPause = false;
                this.BtnStart.Text = @"暂停采集";
            }
            else
            {
                // 暂停采集
                this.isPause = true;
                this.BtnStart.Text = @"开始采集";
            }
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmModbus_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.isClose = true;
            if (this.task != null)
            {
                this.task.Wait();
            }
        }
    }
}
