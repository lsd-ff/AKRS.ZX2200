namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.ControlServices;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.BondSystem.BondForce;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;

    using DevExpress.XtraCharts;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 力值检查
    /// </summary>
    public partial class FrmForceControlInspection : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测量运行时间
        /// </summary>
        private Stopwatch sp = new Stopwatch();

        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause = true;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        ///  力控标准值
        /// </summary>
        public double ForceReferenceValue;

        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> forceValList = new();


        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> lvdtValList = new();

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double forceVal)> printSource = new();

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService modbusService => ModbusService.GetInstance();

        /// <summary>
        ///  取消线程
        /// </summary>
        private CancellationTokenSource cts = null;

        /// <summary>
        /// lvdt
        /// </summary>
        private Sensor LVDT =>HardwareRepositoryService.GetHardware<Sensor>("LVDT");

        /// <summary>
        /// 焊头控制器
        /// </summary>

        private BondHeadController bondHeadController = new BondHeadController();


        public FrmForceControlInspection(int forceReference)
        {
            this.InitializeComponent();
            this.ForceReferenceValue = forceReference;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (this.IsPause == true)
            {
                this.sp.Start();
                this.IsPause = false;
                this.BtnStart.Text = @"暂停采集";
            }
            else
            {
                // 暂停采集
                this.IsPause = true;
                this.BtnStart.Text = @"开始采集";
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.forceValList.Clear();
            this.lvdtValList.Clear();
            this.sp.Restart();
        }

        private void BtSave_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //this.IsPause = false;

            // 控件绑定数据源
            this.ChartModbusRTUForce.Series[0].DataSource = this.forceValList;
            this.ChartModbusRTUForce.Series[1].DataSource = this.lvdtValList;

            // 设置 X 轴为数值类型，绑定 X 数据字段
            this.ChartModbusRTUForce.Series[0].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[0].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            // 设置 X 轴为数值类型，绑定 X 数据字段
            this.ChartModbusRTUForce.Series[1].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[1].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[1].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[1].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            // 设置用RTU连接时XY轴的范围
            ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.ConstantLines[0].AxisValue = this.ForceReferenceValue;

            ChartService.SetYRange(this.ChartModbusRTUForce, 0, this.ForceReferenceValue + 150);

            ChartService.SetSecondaryYRange((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram, 5000, 12000);

            this.task = Task.Run(() =>
            {
                CommonUtil.SetCurrentThreadName("力控实时曲线");

                this.SetChartControl();
            });

            // this.task = Task.Run(async () =>
            // {
            //     CommonUtil.SetCurrentThreadName("力控实时曲线");
            //
            //     await this.SetChartControlAsync();
            // });
        }

        /// <summary>
        /// 设置折线图
        /// </summary>
        private void SetChartControl()
        {
            try
            {
                List<DataPoint> cmdPosDataRange = new List<DataPoint>();

                while (this.isClose == false)
                {
                    if (this.IsPause == false)
                    {
                        this.sp.Start();

                        bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(this.ForceReferenceValue);

                        double holdRegisters = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                        //int[] holdRegisters = new int[] { 1 };
                        //holdRegisters[0] = (int)GeometryService.NextDouble(0, 800);

     
                        double angle = this.bondHeadController.GetAxisTRealPos();

                        // 力值转换
                        double force = ForceCalibrationService.ForceIncrementToActualForce(
                            holdRegisters,
                            isSmallForce,
                            angle);

                        double lvdtVal = this.LVDT.ReadTxPDO(0, 1);

                        DataPoint forceData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = force };

                        DataPoint lvdtData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = lvdtVal };

                        this.BeginInvoke(
                              new Action(
                                  () =>
                                  {
                                      this.forceValList.Add(forceData);
                                      this.lvdtValList.Add(lvdtData);

                                      if (this.forceValList.Count > 100)
                                      {
                                          this.forceValList.RemoveAt(0);
                                          this.lvdtValList.RemoveAt(0);
                                      }
                                  }));


                        this.printSource.Add(new() { time = DateTime.Now, forceVal = force });

                        // 数据大于5000就打印一次
                        if (this.printSource.Count > 5000)
                        {
                            this.printSource.Select(
                                (tuple, index) =>
                                {
                                    return new
                                    {
                                        时间 = tuple.time,
                                        力值 = tuple.forceVal,
                                    };
                                }).ExportToXlsx($"D:\\力控实时曲线{DateTime.Now.Day}.xlsx");

                            this.printSource.Clear();
                        }
                    }
                    else
                    {
                        this.sp.Stop();
                    }

                    Thread.Sleep(10);
                }
            }
            catch (Exception ex)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    ex.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void FrmForceControlTest_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        private void FrmForceControlTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.cts != null)
            {
                this.cts.Cancel();
            }

             this.isClose = true;

            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// 点
    /// </summary>
    public class DataPoint
    {
        /// <summary>
        /// X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public double Y { get; set; }
    }
}
