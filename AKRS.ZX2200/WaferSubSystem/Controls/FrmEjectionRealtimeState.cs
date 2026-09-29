using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.Galaxy2.LogicHardware.Repository;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Service;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using OfficeOpenXml;
using Axis = AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers.Axis;
using LicenseContext = OfficeOpenXml.LicenseContext;


namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    /// <summary>
    /// 顶针实时状态显示
    /// </summary>
    public partial class FrmEjectionRealtimeState : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmEjectionRealtimeState()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause = false;

        /// <summary>
        /// 是否关闭
        /// </summary>
        private bool isClose = false;

        /// <summary>
        /// 测量运行时间
        /// </summary>
        private Stopwatch sp = new Stopwatch();

        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> cmdVelList = new BindingList<DataPoint> { };

        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> realVelList = new BindingList<DataPoint> { };

        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> cmdPosList = new BindingList<DataPoint> { };

        /// <summary>
        /// 使用 BindingList 存储 DataPoint
        /// </summary>
        private BindingList<DataPoint> realPosList = new BindingList<DataPoint> { };

        /// <summary>
        /// 打印的数据
        /// </summary>
        private List<(DateTime time, double cmdVel, double realVel, double cmdPos, double realPos)> pointSource = new();

        /// <summary>
        /// 顶针Z
        /// </summary>        
        private Axis ejectionAxisZ => HardwareRepositoryService.GetHardware<Axis>("顶针Z");

        /// <summary>
        ///  取消线程
        /// </summary>
        private CancellationTokenSource cts = null;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmEjectionRealtimeState_Load(object sender, EventArgs e)
        {
            // this.Init();
        }

        /// <summary>
        ///  初始化界面
        /// </summary>
        private void InitControl()
        {
            this.ChartEjection.Series[0].DataSource = this.realPosList;
            this.ChartEjection.Series[1].DataSource = this.cmdPosList;
            this.ChartEjection.Series[2].DataSource = this.realVelList;
            this.ChartEjection.Series[3].DataSource = this.cmdPosList;

            // 设置 X 轴为数值类型，绑定 X 数据字段
            this.ChartEjection.Series[0].ArgumentScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[0].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartEjection.Series[0].ValueScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[0].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.ChartEjection.Series[1].ArgumentScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[1].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartEjection.Series[1].ValueScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[1].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.ChartEjection.Series[2].ArgumentScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[2].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartEjection.Series[2].ValueScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[2].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            this.ChartEjection.Series[3].ArgumentScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[3].ArgumentDataMember = "X"; // 绑定 X 轴（索引）
            this.ChartEjection.Series[3].ValueScaleType = ScaleType.Numerical;
            this.ChartEjection.Series[3].ValueDataMembers.AddRange(new string[] { "Y" }); // 绑定 Y 轴（索引）

            // 设置用RTU连接时XY轴的范围
            ChartService.SetYRange(this.ChartEjection, 0, 100);
            ChartService.SetSecondaryYRange((SwiftPlotDiagram)this.ChartEjection.Diagram, 0, 5);
        }

        ///// <summary>
        ///// 初始化
        ///// </summary>
        //private void Init()
        //{
        //    this.sp = new Stopwatch();
        //    this.sp.Start();

        //    // 设置用RTU连接时XY轴的范围
        //    ChartService.SetYRange(this.ChartEjection, 0, 100);
        //    ChartService.SetSecondaryYRange((SwiftPlotDiagram)this.ChartEjection.Diagram, 0, 5);
        //    this.AutoSetXWholeRange(this.ChartEjection, 1000, 1000);

        //    this.task = Task.Run(() =>
        //        {
        //            while (this.isClose == false)
        //            {
        //                if (this.isPause == false)
        //                {
        //                    // 指令速度
        //                    double cmdVel = this.ejectionAxisZ.GetVel();
        //                    this.cmdVelList.Add(new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = cmdVel });

        //                    // 实际速度
        //                    double realVel = this.ejectionAxisZ.GetRealVel();
        //                    this.realVelList.Add(new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = realVel });

        //                    // 指令位置
        //                    double cmdPos = this.ejectionAxisZ.GetCmdPosition();
        //                    this.cmdPosList.Add(new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = cmdPos });

        //                    // 实际位置
        //                    double realPos = this.ejectionAxisZ.GetRealPosition();
        //                    this.realPosList.Add(new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = realPos });

        //                    this.pointSource.Add((DateTime.Now,
        //                                             cmdVel,
        //                                             realVel,
        //                                             cmdPos,
        //                                             realPos));

        //                    if (this.pointSource.Count > 1000)
        //                    {
        //                        this.pointSource.Select(
        //                   (tuple, index) =>
        //                   {
        //                       return new
        //                       {
        //                           时间 = tuple.time,
        //                           指令速度 = tuple.cmdVel,
        //                           真实速度 = tuple.realVel,
        //                           指令位置 = tuple.cmdPos,
        //                           真实位置 = tuple.realPos
        //                       };
        //                   }).ExportToXlsx($"D:\\顶针实时状态{DateTime.Now.Day}.xlsx");

        //                        this.pointSource.Clear();
        //                    }

        //                    SeriesPoint p1 = new SeriesPoint(
        //                                       this.sp.ElapsedMilliseconds,
        //                                       cmdVel);

        //                    SeriesPoint p2 = new SeriesPoint(
        //                        this.sp.ElapsedMilliseconds,
        //                        realVel);

        //                    SeriesPoint p3 = new SeriesPoint(
        //                        this.sp.ElapsedMilliseconds,
        //                        cmdPos);

        //                    SeriesPoint p4 = new SeriesPoint(
        //                        this.sp.ElapsedMilliseconds,
        //                        realPos);

        //                    this.BeginInvoke(
        //                        new Action(
        //                            () =>
        //                                {
        //                                    // 加点
        //                                    ChartService.AddPoint(this.ChartEjection.Series[0], p4);

        //                                    ChartService.AddPoint(this.ChartEjection.Series[1], p3);

        //                                    ChartService.AddPoint(this.ChartEjection.Series[2], p2);

        //                                    ChartService.AddPoint(this.ChartEjection.Series[3], p1);

        //                                    // 自动设置x轴范围
        //                                    this.AutoSetXWholeRange(this.ChartEjection, 1000, 9000);
        //                                }));
        //                }

        //                Thread.Sleep(20);
        //            }
        //        });
        //}

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
                            (int)this.sp.ElapsedMilliseconds + retain - xRange,
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
        /// 开始采集
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            //if (this.isPause == true)
            //{
            //    // 开始采集
            //    this.ChartEjection.Series[0].Points.Clear();
            //    this.ChartEjection.Series[1].Points.Clear();
            //    this.ChartEjection.Series[2].Points.Clear();
            //    this.ChartEjection.Series[3].Points.Clear();
            //    this.SetXWholeRange(this.ChartEjection, 0, 10000);
            //    this.sp.Restart();
            //    this.isPause = false;
            //    this.BtnStart.Text = @"暂停采集";
            //}
            //else
            //{
            //    // 暂停采集
            //    this.isPause = true;
            //    this.BtnStart.Text = @"开始采集";
            //}


            if (this.cts != null)
            {
                this.cts.Cancel();
                this.sp.Stop();
                this.BtnStart.Text = @"开始采集";
                return;
            }

            this.BtnStart.Text = @"暂停采集";
            this.cts = new CancellationTokenSource();

            Action<DataPoint, DataPoint, DataPoint, DataPoint> action = (cmdVel, realVel, cmdPos, realPos) =>
                {
                    // 加点
                    this.cmdVelList.Add(cmdVel);
                    this.realVelList.Add(realVel);
                    this.cmdPosList.Add(cmdPos);
                    this.realPosList.Add(realPos);

                    // 如果数据点太多，可以选择移除一些旧数据（可选）
                    if (this.cmdVelList.Count > 500) // 只保留最近100个数据点
                    {
                        this.cmdVelList.RemoveAt(0);
                        this.realVelList.RemoveAt(0);
                        this.cmdPosList.RemoveAt(0);
                        this.realPosList.RemoveAt(0);
                    }

                    //// 刷新图表以显示新的数据
                    //this.ChartEjection.RefreshData();
                };

            this.sp.Start();

            Task.Run(async () =>
                {
                    CommonUtil.SetCurrentThreadName("顶针数据实时采集线程");

                    while (!this.cts.IsCancellationRequested) 
                    {
                        // 轴运动的时候才采集
                        if (ejectionAxisZ.IsInCommandPosition() == false)
                        {
                            this.sp.Start();

                            // 指令速度
                            double cmdVel = this.ejectionAxisZ.GetVel();

                            DataPoint cmdVelData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = cmdVel };

                            // 实际速度
                            double realVel = this.ejectionAxisZ.GetRealVel();
                            DataPoint realVelData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = realVel };

                            // 指令位置
                            double cmdPos = this.ejectionAxisZ.GetCmdPosition();
                            DataPoint cmdPosData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = cmdPos };

                            // 实际位置
                            double realPos = this.ejectionAxisZ.GetRealPosition();
                            DataPoint realPosData = new DataPoint() { X = this.sp.ElapsedMilliseconds, Y = realPos };

                            this.BeginInvoke(action, cmdVelData, realVelData, cmdPosData, realPosData);

                            this.pointSource.Add((DateTime.Now,
                                                     cmdVel,
                                                     realVel,
                                                     cmdPos,
                                                     realPos));

                            // 数据大于6000就打印一次
                            if (this.pointSource.Count > 6000)
                            {
                                this.pointSource.Select(
                                    (tuple, index) =>
                                    {
                                        return new
                                        {
                                            时间 = tuple.time,
                                            指令速度 = tuple.cmdVel,
                                            真实速度 = tuple.realVel,
                                            指令位置 = tuple.cmdPos,
                                            真实位置 = tuple.realPos
                                        };
                                    }).ExportToXlsx($"D:\\顶针实时状态{DateTime.Now.Day}.xlsx");

                                this.pointSource.Clear();
                            }

                            await Task.Delay(1).ConfigureAwait(false);
                        }
                        else
                        {
                            this.sp.Stop();
                        }
                    }

                    this.cts = null;
                }, this.cts.Token);
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmEjectionRealtimeState_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (cts != null)
            {
                this.cts.Cancel();
            }

            //  this.isClose = true;
        }

        /// <summary>
        ///  保存数据
        /// </summary>
        private void SaveCorrectionData()
        {
            // 添加一个工作表
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage excelPackage = new ExcelPackage(
                new FileInfo(
                    @"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "顶针实时状态显示"
                    + ".xlsx"));

            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                           ? excelPackage.Workbook.Worksheets[0]
                                           : excelPackage.Workbook.Worksheets.Add("DataSheet");

            // 设置列宽
            for (int i = 1; i <= 7; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Time";

                worksheet.Cells[1, 2].Value = "指令速度mm/s";

                worksheet.Cells[1, 3].Value = "实际速度mm/s";

                worksheet.Cells[1, 4].Value = "指令位置mm";

                worksheet.Cells[1, 5].Value = "实际位置mm";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < this.pointSource.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value =
                    this.pointSource[i].time.ToString("MM-dd HH:mm:ss");

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value = this.pointSource[i].cmdVel;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = this.pointSource[i].realVel;

                worksheet.Cells[lastUsedRow + 1 + i, 4].Value = this.pointSource[i].cmdPos;

                worksheet.Cells[lastUsedRow + 1 + i, 5].Value = this.pointSource[i].realPos;
            }

            excelPackage.Save();
        }

        /// <summary>
        ///  保存当前数据
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            Task.Run(
                () =>
                    {
                        this.pointSource.Select(
                            (tuple, index) =>
                                {
                                    return new
                                    {
                                        时间 = tuple.time,
                                        指令速度 = tuple.cmdVel,
                                        真实速度 = tuple.realVel,
                                        指令位置 = tuple.cmdPos,
                                        真实位置 = tuple.realPos
                                    };
                                }).ExportToXlsx($"D:\\顶针实时状态{DateTime.Now.Day}.xlsx");

                        this.pointSource.Clear();
                    });
        }

        /// <summary>
        ///  清空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            this.realVelList.Clear();
            this.realPosList.Clear();
            this.cmdVelList.Clear();
            this.cmdPosList.Clear();
            this.sp.Restart();
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

