namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 测焊头水平窗体
    /// </summary>
    public partial class FrmBondheadStandardMeasurement : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBondheadStandardMeasurement()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 数据源
        /// </summary>
        public List<ResultValue> MeasureHeightResult { get; set; } = new List<ResultValue>();

        /// <summary>
        /// 工作线程
        /// </summary>
        private Task standardMeasurementTask;

        /// <summary>
        /// 安装角度
        /// </summary>
        public double InstallAngle { get; set; }

        /// <summary>
        /// 测量工具长度
        /// </summary>
        public double ToolLenghth { get; set; }

        /// <summary>
        /// 测量次数
        /// </summary>
        public int MeasureTimes { get; set; }

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 刷新页面
        /// </summary>
        private void RefreshControl()
        {
            // 绑定数据
            this.GCMeasureResult.DataSource = this.MeasureHeightResult;

            this.GCMeasureResult.Refresh();

            this.GvMeasureResult.RefreshData();
        }

        /// <summary>
        /// 开始测量
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">方法</param>
        private void BtnStart_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"Please  attach  measuring  tool  on  bondhead!",
                "Question",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Cancel)
            {
                return;
            }

            this.InstallAngle = (double)this.SpInstallAngle.Value;
            this.ToolLenghth = (double)this.SpToolLength.Value;
            this.MeasureTimes = (int)this.SpMeasureTimes.Value;

            this.MeasureHeightResult.Clear();
            this.BtnStart.Appearance.BackColor = Color.Yellow;

            this.standardMeasurementTask = Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("测焊头水平线程");
                        this.MeaureStandard();
                        this.BtnStart.Appearance.BackColor = Color.Transparent;
                    });
        }

        /// <summary>
        /// 测焊头水平
        /// </summary>
        private void MeaureStandard()
        {
            try
            {
                // 测高位置,这个应该是设备参数
                 AKRSPoint3D targetPos = new AKRSPoint3D(168.1554, -274.8245, 106.7341);
                //AKRSPoint3D targetPos = BondDevicePara.GetInstance().BMCDevicePara.MeasureHeightPos;

                for (int i = 0; i < this.MeasureTimes; i++)
                {
                    // 移动到第一点
                    this.bondModuleController.MoveToG0Pos(new AKRSPoint3D(targetPos.X - this.ToolLenghth, targetPos.Y, targetPos.Z));

                    this.bondHeadController.RotateAxisT(this.InstallAngle);

                    // 执行测高
                    (ExcuteResult Ret, double HeightValue) res1 = this.bondHeadController.MeasureHeight(
                        -28,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    Thread.Sleep(1000);

                    // 移动到第二点
                    this.bondModuleController.MoveToG0Pos(new AKRSPoint3D(targetPos.X, targetPos.Y - this.ToolLenghth, targetPos.Z));

                    this.bondHeadController.RotateAxisT(this.InstallAngle + 90);

                    // 执行测高
                    (ExcuteResult Ret, double HeightValue) res2 = this.bondHeadController.MeasureHeight(
                        -28,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    Thread.Sleep(1000);

                    // 移动到第三点
                    this.bondModuleController.MoveToG0Pos(new AKRSPoint3D(targetPos.X + this.ToolLenghth, targetPos.Y, targetPos.Z));

                    this.bondHeadController.RotateAxisT(this.InstallAngle + 180);

                    // 执行测高
                    (ExcuteResult Ret, double HeightValue) res3 = this.bondHeadController.MeasureHeight(
                        -28,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    Thread.Sleep(1000);

                    // 移动到第四点
                    this.bondModuleController.MoveToG0Pos(new AKRSPoint3D(targetPos.X, targetPos.Y + this.ToolLenghth, targetPos.Z));

                    this.bondHeadController.RotateAxisT(this.InstallAngle + 270);

                    // 执行测高
                    (ExcuteResult Ret, double HeightValue) res4 = this.bondHeadController.MeasureHeight(
                        -28,
                        HeightMeasurementFunctionEnum.WithTDSensor);

                    // 计算
                    double resultX = res1.HeightValue - res3.HeightValue;
                    double resultY = res2.HeightValue - res4.HeightValue;

                    ResultValue resultValue = new ResultValue()
                                                  {
                                                      Times = i + 1,
                                                      Point1Value = res1.HeightValue,
                                                      Point2Value = res2.HeightValue,
                                                      Point3Value = res3.HeightValue,
                                                      Point4Value = res4.HeightValue,
                                                      ResultX = resultX,
                                                      ResultY = resultY,
                                                  };

                    this.MeasureHeightResult.Add(resultValue);
                }

                this.bondHeadController.MoveBondZToSafePos();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"Measure  bondhead  standard  failed!\r\n" + e.ToString(),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 用于存储结果
        /// </summary>
        public class ResultValue
        {
            /// <summary>
            /// 次数
            /// </summary>
            public int Times { get; set; }

            /// <summary>
            /// 第一点
            /// </summary>
            public double Point1Value { get; set; }

            /// <summary>
            /// 第二点
            /// </summary>
            public double Point2Value { get; set; }

            /// <summary>
            /// 第三点
            /// </summary>
            public double Point3Value { get; set; }

            /// <summary>
            /// 第四点
            /// </summary>
            public double Point4Value { get; set; }

            /// <summary>
            /// 测量结果X
            /// </summary>
            public double ResultX { get; set; }

            /// <summary>
            /// 测量结果Y
            /// </summary>
            public double ResultY { get; set; }
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">方法</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshControl();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmBondheadStandardMeasurement_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }
    }
}