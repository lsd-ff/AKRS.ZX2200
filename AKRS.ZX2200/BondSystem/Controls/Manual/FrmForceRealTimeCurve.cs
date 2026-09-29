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
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.BondForce.Services;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using PostSharp.Aspects.Advices;

    /// <summary>
    /// 力控实时曲线
    /// </summary>
    public partial class FrmForceRealTimeCurve : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmForceRealTimeCurve()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 测量运行时间
        /// </summary>
        private Stopwatch sp = new Stopwatch();

        /// <summary>
        /// 锁
        /// </summary>
        private readonly object printSourceLock = new object();

        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 是否暂停
        /// </summary>
        private static bool IsPause = true;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        ///  力控标准值
        /// </summary>
        private static double ForceReferenceValue;

        /// <summary>
        /// 应变片力值
        /// </summary>
        private BindingList<DataPoint> headForceValList = new();

        /// <summary>
        /// 标定台力值
        /// </summary>
        private BindingList<DataPoint> caliTableForceValList = new();

        /// <summary>
        /// Z轴位置值
        /// </summary>
        private BindingList<DataPoint> zPosValList = new();

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double forceVal)> printSource = new();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        ///  取消线程
        /// </summary>
        private CancellationTokenSource cts = null;

        /// <summary>
        /// 力控曲线读取时机
        /// </summary>
        public static ForceReadTimingEnum ForceReadTiming = ForceReadTimingEnum.PickAndPlace;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 当前力值
        /// </summary>
        private double force = 0;

        /// <summary>
        /// 设置力控标准值
        /// </summary>
        /// <param name="force">力</param>
        public static void SetForceReferenceValue(double force)
        {
            ForceReferenceValue = force;
        }

        /// <summary>
        /// 开始/停止读焊头力
        /// true:开始读取  false:停止读取
        /// </summary>
        /// <param name="isActive">是否开启</param>
        public static void ReadBondForce(bool isActive)
        {
            IsPause = !isActive;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //isClose = false;
            this.cts = new CancellationTokenSource();

            // 控件绑定数据源
            this.ChartModbusRTUForce.Series[0].DataSource = this.caliTableForceValList;
            this.ChartModbusRTUForce.Series[1].DataSource = this.headForceValList;
            this.ChartModbusRTUForce.Series[2].DataSource = this.zPosValList;


            // 设置 X 轴为数值类型，绑定 X 数据字段
            this.ChartModbusRTUForce.Series[0].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[0].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.ChartModbusRTUForce.Series[1].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[1].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[1].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[1].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.ChartModbusRTUForce.Series[2].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[2].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[2].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[2].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            // 设置用RTU连接时XY轴的范围
            ((SwiftPlotDiagram)ChartModbusRTUForce.Diagram).AxisY.ConstantLines[0].AxisValue = ForceReferenceValue;

            ChartService.SetYRange(this.ChartModbusRTUForce, 0, ForceReferenceValue + 5);

            this.LueReadTiming.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<ForceReadTimingEnum>();
            this.LueReadTiming.EditValue = ForceReadTiming;

            this.BtnStart.Text = IsPause ? "开始采集" : "暂停采集";

            this.task = Task.Run(
                async () =>
                    {
                        CommonUtil.SetCurrentThreadName("力控实时曲线");
                        await this.SetChartControlAsync();
                    },
                this.cts.Token);
        }

        /// <summary>
        /// 数据源加点
        /// </summary>
        private void AddDataPoint()
        {
            if (ForceReadTiming == ForceReadTimingEnum.TestOnCaliTable)
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(ForceReferenceValue);

                double forceCurrentVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                //double forceCurrentVal = (int)GeometryService.NextDouble(30, 40);

                DataPoint cmdPosData = new() { X = this.sp.ElapsedMilliseconds, Y = forceCurrentVal };

                // 应变片力
                this.headForceValList.Add(cmdPosData);

                double caliTableForceVal = this.system2Controller.GetCalibrateTableForceValue();

                //double caliTableForceVal= (int)GeometryService.NextDouble(40, 50);

                DataPoint cmdPosData2 = new() { X = this.sp.ElapsedMilliseconds, Y = caliTableForceVal };

                // 标定台力
                this.caliTableForceValList.Add(cmdPosData2);
            }
            else
            {
                bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(ForceReferenceValue);

                double forceCurrentVal = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                double forecIncrement = forceCurrentVal - System2RunTimeProvider.BondHeadInitialVal;

                double angle = this.bondHeadController.GetAxisTRealPos();

                // 力值转换
               force = ForceCalibrationService.ForceIncrementToActualForce(
                   forecIncrement,
                   isSmallForce,
                   angle);

                //force = (int)GeometryService.NextDouble(60, 70);

                DataPoint cmdPosData2 = new() { X = this.sp.ElapsedMilliseconds, Y = force };

                this.caliTableForceValList.Add(cmdPosData2);
            }

            double zPosVal = this.bondHeadController.GetAxisZRealPos();

            //double zPosVal = (int)GeometryService.NextDouble(150, 300);

            DataPoint cmdPosData3 = new() { X = this.sp.ElapsedMilliseconds, Y = zPosVal };

            // Z轴位置
            this.zPosValList.Add(cmdPosData3);
        }

        /// <summary>
        /// 异步设置图表
        /// </summary>
        /// <returns></returns>
        private async Task SetChartControlAsync()
        {
            try
            {
                while (!this.cts.IsCancellationRequested)
                {
                    if (!IsPause)
                    {
                       this.sp.Start();

                        //double force = this.AddDataPoint();

                        //DataPoint cmdPosData = new() { X = this.sp.ElapsedMilliseconds, Y = force };

                        // 更新UI绑定的数据源（必须在UI线程）
                        this.BeginInvoke(
                            new Action(
                                () =>
                                    {
                                        //// 批量存储数据点
                                        //this.forceValList.Add(cmdPosData);

                                        this.AddDataPoint();

                                        // 满200个就开始移除最前面的数据
                                        if (this.headForceValList.Count > 200)
                                        {
                                            this.headForceValList.RemoveAt(0);
                                        }

                                        if (this.caliTableForceValList.Count > 200)
                                        {
                                            this.caliTableForceValList.RemoveAt(0);
                                        }

                                        if (this.zPosValList.Count > 200)
                                        {
                                            this.zPosValList.RemoveAt(0);
                                        }
                                    }));

                        // 存储打印数据（使用线程安全集合或锁）
                        lock (printSourceLock) 
                        {
                            // 添加打印数据
                            printSource.Add((DateTime.Now, force));

                            // 检查是否需要导出
                            if (printSource.Count > 5000)
                            {
                                var dataToExport = printSource.ToList(); // 快照
                                printSource.Clear();

                                // 异步写入 Excel
                                _ = Task.Run(() =>
                                    {
                                        dataToExport.Select(tuple => new
                                        {
                                            时间 = tuple.time,
                                            力值 = tuple.forceVal
                                        }).ExportToXlsx($"D:\\力控实时曲线{DateTime.Now:yyyy-MM-dd}.xlsx");
                                    });
                            }

                            // 每隔5个刷新一次
                            if (this.printSource.Count % 5 == 0)
                            {
                                this.BeginInvoke(new Action(() =>
                                    {
                                        // 设置Y轴范围
                                        this.AdjustYAxisRange();
                                        this.AdjustSecondYAxisRange();

                                        // 更新 ConstantLines 值
                                        ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.ConstantLines[0]
                                            .AxisValue = ForceReferenceValue;
                                    }));
                            }
                        }

                        //// 更新UI
                        //await Task.Run(() =>
                        //    {
                        //        this.BeginInvoke(new Action(() =>
                        //            {
                        //                // 每隔5个刷新一次
                        //                if (this.printSource.Count % 5 == 0)
                        //                {
                        //                    // 设置Y轴范围
                        //                    this.AdjustYAxisRange();

                        //                    // 更新 ConstantLines 值
                        //                    ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.ConstantLines[0]
                        //                        .AxisValue = ForceReferenceValue;
                        //                }
                        //            }));
                        //    });
                    }
                    else
                    {
                        if (this.sp.IsRunning)
                        {
                            this.sp.Stop();
                        }
                    }

                    // 不用 Thread.Sleep
                    await Task.Delay(1).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    ex.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 自动调节Y轴范围
        /// </summary>
        private void AdjustYAxisRange()
        {
            if (this.caliTableForceValList.Count == 0)
            {
                return;
            }

            var list = this.headForceValList.ToList();
            list.AddRange(caliTableForceValList);

            // Y轴上下界
            var maxY = list.Max(p => p.Y);
            var minY = list.Min(p => p.Y);
            var upperLimit = Math.Max(ForceReferenceValue + 5, maxY + 1);
            var lowerLimit = Math.Min(0, minY - 1);

            ChartService.SetYRange(this.ChartModbusRTUForce, lowerLimit, upperLimit);
        }


        /// <summary>
        /// 自动调节第二坐标系Y轴范围
        /// </summary>
        private void AdjustSecondYAxisRange()
        {
            if (this.zPosValList.Count == 0)
            {
                return;
            }

            var list = this.zPosValList.ToList();

            // Y轴上下界
            var maxY = list.Max(p => p.Y);
            var minY = list.Min(p => p.Y);
            var upperLimit = maxY + 5;
            var lowerLimit = minY - 5;

            ChartService.SetSecondaryYRange((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram, lowerLimit, upperLimit);
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmForceRealTimeCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
            //isClose = true;
            //if (this.task != null)
            //{
            //    this.task.Wait();
            //}

            this.cts?.Cancel();
            this.cts?.Dispose();
        }

        /// <summary>
        /// 开始/暂停
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (IsPause == true)
            {
                this.sp.Start();
                IsPause = false;
                BtnStart.Text = @"暂停采集";
            }
            else
            {
                // 暂停采集
                IsPause = true;
                BtnStart.Text = @"开始采集";
            }
        }

        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.headForceValList.Clear();
            this.caliTableForceValList.Clear();
            this.zPosValList.Clear();
            this.sp.Restart();
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        if (this.printSource.Count != 0)
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
                    });
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmForceRealTimeCurve_Load(object sender, EventArgs e)
        {
            this.Init();
        }

        /// <summary>
        /// 下拉框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueType_EditValueChanged(object sender, EventArgs e)
        {
            ForceReadTiming = (ForceReadTimingEnum)this.LueReadTiming.EditValue;
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