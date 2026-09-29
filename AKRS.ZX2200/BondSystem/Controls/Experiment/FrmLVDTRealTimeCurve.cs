using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using DevExpress.XtraCharts;

namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    /// <summary>
    ///  LVDT实时显示窗体
    /// </summary>
    public partial class FrmLVDTRealTimeCurve : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLVDTRealTimeCurve()
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
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> lvdtValList = new();

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double lvdtVal)> printSource = new();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        ///  取消线程
        /// </summary>
        private CancellationTokenSource cts = null;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 当前力值
        /// </summary>
        private double lvdtVal = 0;

        /// <summary>
        /// 开始/停止读LVDT
        /// true:开始读取  false:停止读取
        /// </summary>
        /// <param name="isActive">是否开启</param>
        public static void ReadLVDT(bool isActive)
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
            this.ChartModbusRTUForce.Series[0].DataSource = this.lvdtValList;

            // 设置 X 轴为数值类型，绑定 X 数据字段
            this.ChartModbusRTUForce.Series[0].ArgumentScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartModbusRTUForce.Series[0].ValueScaleType = ScaleType.Numerical;
            this.ChartModbusRTUForce.Series[0].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.BtnStart.Text = IsPause ? "开始采集" : "暂停采集";

            this.task = Task.Run(
                async () =>
                {
                    CommonUtil.SetCurrentThreadName("LVDT实时曲线");
                    await this.SetChartControlAsync();
                },
                this.cts.Token);
        }

        /// <summary>
        /// 数据源加点
        /// </summary>
        private void AddDataPoint()
        {
            this.lvdtVal = this.bondHeadController.GetLVDTVal();

            //this.lvdtVal = (int)GeometryService.NextDouble(60, 70);

            DataPoint cmdPosData = new() { X = this.sp.ElapsedMilliseconds, Y = this.lvdtVal };

            this.lvdtValList.Add(cmdPosData);
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

                        // 更新UI绑定的数据源（必须在UI线程）
                        this.BeginInvoke(
                            new Action(
                                () =>
                                {
                                    this.AddDataPoint();

                                    // 满200个就开始移除最前面的数据
                                    if (this.lvdtValList.Count > 200)
                                    {
                                        this.lvdtValList.RemoveAt(0);
                                    }
                                }));

                        // 存储打印数据（使用线程安全集合或锁）
                        lock (printSourceLock)
                        {
                            // 添加打印数据
                            printSource.Add((DateTime.Now, this.lvdtVal));

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
                                        力值 = tuple.lvdtVal
                                    }).ExportToXlsx($"D:\\LVDT实时曲线{DateTime.Now:yyyy-MM-dd}.xlsx");
                                });
                            }

                            // 每隔5个刷新一次
                            if (this.printSource.Count % 5 == 0)
                            {
                                this.BeginInvoke(new Action(() =>
                                {
                                    // 设置Y轴范围
                                    this.AdjustYAxisRange();
                                }));
                            }
                        }
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
            if (this.lvdtValList.Count == 0)
            {
                return;
            }

            var list = this.lvdtValList.ToList();

            // Y轴上下界
            var maxY = list.Max(p => p.Y);
            var minY = list.Min(p => p.Y);
            var upperLimit = maxY + 1;
            var lowerLimit = Math.Min(0, minY - 1);

            ChartService.SetYRange(this.ChartModbusRTUForce, lowerLimit, upperLimit);
        }

        /// <summary>
        /// 窗口关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmLVDTRealTimeCurve_FormClosing(object sender, FormClosingEventArgs e)
        {
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
            this.lvdtValList.Clear();
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
                              力值 = tuple.lvdtVal,
                          };
                      }).ExportToXlsx($"D:\\LVDT曲线{DateTime.Now.Day}.xlsx");

                        this.printSource.Clear();
                    }
                });
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmLVDTRealTimeCurve_Load(object sender, EventArgs e)
        {
            this.Init();
        }
    }
}