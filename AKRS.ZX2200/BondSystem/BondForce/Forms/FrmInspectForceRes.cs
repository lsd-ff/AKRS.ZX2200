using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.ControlServices;
using AKRS.ZX2200.BondSystem.BondForce.Modbus;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Controls.Manual;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using DevExpress.DashboardWin;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using OfficeOpenXml;

namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    /// <summary>
    /// 力控检测
    /// </summary>
    public partial class FrmInspectForceRes : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmInspectForceRes()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool isStop = false;

        /// <summary>
        /// 采集线程
        /// </summary>
        private Task task;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService ModbusService => ModbusService.GetInstance();

        /// <summary>
        /// 次数
        /// </summary>
        private int times = 2;

        /// <summary>
        ///  力
        /// </summary>
        private int force;

        /// <summary>
        /// 测试结果
        /// </summary>
        private List<double> forceList = new List<double>();

        /// <summary>
        /// 延时
        /// </summary>
        private int delay = 0;


        /// <summary>
        /// 焊头控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        ///  界面初始化
        /// </summary>
        private void InitControl()
        {
            this.SpBondForce.Properties.MaxValue =
                (decimal)BondDevicePara.GetInstance().BondHeadParam.ForceControlMaxVal;
            this.SpBondForce.Properties.MinValue = (decimal)BondDevicePara.GetInstance().BondHeadParam.ForceControlMinVal;

            this.SpDelay.Properties.MaxValue = 5000;

            ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisX.NumericScaleOptions.GridSpacing = 1; // 网格间隔为1
            ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisX.NumericScaleOptions.GridAlignment = NumericGridAlignment.Ones; // 网格对齐到1的倍数


            // 将系列视图转换为折线图视图
            PointSeriesView view = this.ChartModbusRTUForce.Series[0].View as PointSeriesView;
            //if (view != null)
            //{
            //    // 设置线条粗细（单位：像素）
            //    view.PointMarkerOptions.Thickness = 1; // 3像素粗
            //}

            if (view != null)
            {
                // 设置点标记的大小
                view.PointMarkerOptions.Size = 10; // 点的大小为10像素
            }
        }

        /// <summary>
        /// 开始/停止
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (this.task == null || this.task.IsCompleted == true)
            {
                this.force = (int)this.SpBondForce.Value;
                this.times = (int)this.SpTimes.Value;
                this.delay = (int)this.SpDelay.Value;

                // 开始采集
                this.ChartModbusRTUForce.Series[0].Points.Clear();
                this.SetXYRange();
                ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.ConstantLines[0].AxisValue = this.force;
                this.isStop = false;
                this.BtnStart.Text = @"停止";
                this.task = Task.Run(
                        () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控测试线程");
                            this.InspectForce();
                            //this.test();
                        });

                this.task.ContinueWith(t =>
                {
                    if (this.BtnStart.InvokeRequired)
                    {
                        this.BtnStart.Invoke(
                            new Action(
                                () =>
                                    {
                                        this.BtnStart.Text = @"开始";
                                    }));
                    }
                    else
                    {
                        this.BtnStart.Text = @"开始";
                    }
                });
            }
            else
            {
                // 暂停采集
                this.isStop = true;
                this.BtnStart.Text = @"开始";
            }
        }

        /// <summary>
        /// 设置XY轴范围
        /// </summary>
        private void SetXYRange()
        {
            WholeRange diaX = ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisX.WholeRange;
            WholeRange diaY = ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.WholeRange;

            double minValue = this.force - 10 <= 0 ? 0 : this.force;
            double maxValue = this.force + 10;

            // 范围暂定正负10
            diaY.SetMinMaxValues(minValue, maxValue);

            diaX.SetMinMaxValues(1, this.times);
        }

        /// <summary>
        /// 设置Y轴范围
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        private void SetYRange(double minValue, double maxValue)
        {
            WholeRange diaY = ((SwiftPlotDiagram)this.ChartModbusRTUForce.Diagram).AxisY.WholeRange;

            // 范围暂定正负10
            diaY.SetMinMaxValues(minValue, maxValue);
        }

        /// <summary>
        /// 测试
        /// </summary>
        public void test()
        {
            try
            {
                // 获取力值数据
                for (int i = 0; i < this.times; i++)
                {
                    Random rand = new Random();
                    double actualForce = rand.NextDouble()*5;

                    // 把读到的力值添加到力值列表中
                    this.forceList.Add(actualForce);

                    // 绘制图线
                    SeriesPoint p = new SeriesPoint(i + 1, actualForce);
                    ChartService.AddPoint(this.ChartModbusRTUForce.Series[0], p);

                    this.BeginInvoke(
    new Action(
        () =>
        {
            // 设置Y轴范围
            this.SetYRange(this.forceList.Min() - 5, this.forceList.Max() + 5);
        }));

                    Thread.Sleep(50);

                    if (this.isStop)
                    {
                        // 点停止直接退出
                        return;
                    }
                }

                double range = this.forceList.Max() - this.forceList.Min();


                // 标定完成
                AKRSXtraMessageBox.Show("检验完成\n" + "极差: " + range, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {

                AKRSXtraMessageBox.Show($"检验失败:{e.ToString()}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 检验力
        /// </summary>
        public void InspectForce()
        {
            try
            {
                // 连接摩尔力设备
                if (!this.ModbusService.ConnectManometer())
                {
                    return;
                }

                this.bondHeadController.MoveBondZToSafePos();

                AKRSPoint3D forceCalibratePos = BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos;

                // 移动到标定位
                this.bondModuleController.MoveToG0Pos(forceCalibratePos.X, forceCalibratePos.Y);

                AKRSPoint3D forceCalibratePosInAxis =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        forceCalibratePos);

                // 抬起位置
                double liftLevel = forceCalibratePosInAxis.Z + 6;

                double bondLevel = forceCalibratePosInAxis.Z + 1;

                double speed = this.bondHeadController.GetAxisZAbsoluteSpeed()
                               * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                this.forceList.Clear();

                // 获取力值数据
                for (int i = 0; i < this.times; i++)
                {
                    if (this.system2Controller.GetBondheadForceValue() < 0)
                    {
                        // 焊头力控清零
                        this.bondHeadController.ResetBondhead();
                    }

                    // 校正台压力表清零
                    this.ModbusService.ResetManometer();

                    double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                    this.bondHeadController.MoveAxisZ(bondLevel);

                    this.StartForceRealTimeCurve();

                    // 下压
                    this.bondHeadController.ForceControlSet(this.force, bondLevel - 0.01, 100, 500, 1);

                    //double read = this.bondHeadController.GetBondForceInitialVal(this.force);  

                    Thread.Sleep(this.delay);

                    // 读取摩尔力设备读数
                    //int[] holdRegisters = this.ModbusService.ReadManometer();

                    double readCaliTable = this.system2Controller.GetCalibrateTableForceValue();

                    double actualForce = readCaliTable - calibrateTableForceInitial;

                    // 把读到的力值添加到力值列表中
                    this.forceList.Add(actualForce);

                    // 绘制图线
                    SeriesPoint p = new SeriesPoint(i + 1, actualForce);
                    ChartService.AddPoint(this.ChartModbusRTUForce.Series[0], p);

                    this.BeginInvoke(
    new Action(
        () =>
        {
            // 设置Y轴范围
            this.SetYRange(this.forceList.Min() - 5, this.forceList.Max() + 5);
        }));

                    // 加这句是为了防止Z轴抬起时没有恢复正常速度
                    this.bondHeadController.SetAxisZSpeed(2);

                    // 抬起
                    this.bondHeadController.ForceControlReset(liftLevel, speed);

                    this.StopForceRealTimeCurve();

                    //Thread.Sleep(500);

                    if (this.isStop)
                    {
                        this.bondHeadController.MoveBondZToSafePos();

                        // 点停止直接退出
                        return;
                    }
                }

                double range = this.forceList.Max() - this.forceList.Min();

                this.bondHeadController.MoveBondZToSafePos();

                // 标定完成
                AKRSXtraMessageBox.Show("检验完成\n" + "极差: " + range, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                this.bondHeadController.MoveBondZToSafePos();

                AKRSXtraMessageBox.Show($"检验失败:{e.ToString()}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmInspectForceRes_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            if (this.task != null && this.task.IsCompleted == false)
            {
                // 提示
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先结束力值检验!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                e.Cancel = true;
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            // 打印
            this.ExportData();
        }

        /// <summary>
        /// 输出到指定路径
        /// </summary>
        private void ExportData()
        {
            XtraSaveFileDialog fileDialog = new XtraSaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();
                    this.SaveInspectForceRes(fileDialog.FileName);

                    AKRSXtraMessageBox.Show(
                        $"Export  success!",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                       AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                       AKRSXtraMessageBox.Show("数据导出失败！数据量过大，请分别统计再导出！", "提示");
                    }
                }
            }

            fileDialog.Dispose();
        }

        /// <summary>
        /// 打印结果
        /// </summary>
        /// <param name="path">路径</param>
        private void SaveInspectForceRes(string path)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new ExcelPackage(new FileInfo(path));
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

            // 设置列宽
            for (int i = 1; i <= 10; i++)
            {
                worksheet.Column(i).Width = 15;
            }

            // 添加标题行
            if (worksheet.Dimension == null)
            {
                worksheet.Cells[1, 1].Value = "Number";

                worksheet.Cells[1, 2].Value = "Input force(g)";

                worksheet.Cells[1, 3].Value = "Actual  force(g)";
            }

            int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

            for (int i = 0; i < this.forceList.Count; i++)
            {
                worksheet.Cells[lastUsedRow + 1 + i, 1].Value = i + 1;

                worksheet.Cells[lastUsedRow + 1 + i, 2].Value =
                    this.force;

                worksheet.Cells[lastUsedRow + 1 + i, 3].Value = this.forceList[i];
            }

            package.Save();
        }

        /// <summary>
        /// 力控曲线开始读取
        /// </summary>
        private void StartForceRealTimeCurve()
        {
            FrmForceRealTimeCurve.ForceReadTiming = ForceReadTimingEnum.TestOnCaliTable;

            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(true);
        }

        /// <summary>
        /// 力控曲线结束读取
        /// </summary>
        private void StopForceRealTimeCurve()
        {
            // 开始读焊头力
            UcMainSystem.ActiveReadBondForce(false);
        }
    }
}