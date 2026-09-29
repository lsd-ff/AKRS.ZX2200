namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System1Related;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System2Related;

    // using AKRS.ZX2200.LevelMeasurementSystem.Controls;
    // using AKRS.ZX2200.LevelMeasurementSystem.Controls;
    // using AKRS.ZX2200.LevelMeasurementSystem.Controls.BondLevelMeasure;
    // using AKRS.ZX2200.LevelMeasurementSystem.Controls.FlatLevelMeasure;

    /// <summary>
    /// 测试工具
    /// </summary>
    public partial class UcMeasuringTool : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 测试工具
        /// </summary>
        public UcMeasuringTool()
        {
            this.InitializeComponent();

            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };
        }

        /// <summary>
        /// 多功能测高
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtMultipleHeightMeasurement_Click(object sender, EventArgs e)
        {
            FrmMultipleHeightMeasurement fmrFrmMultipleHeightMeasurement = new FrmMultipleHeightMeasurement();

            fmrFrmMultipleHeightMeasurement.ShowDialog();

            FrmMeasureHeightPoints frmMeasureHeightPoints = new FrmMeasureHeightPoints();

            frmMeasureHeightPoints.MeasureHeightPoints = fmrFrmMultipleHeightMeasurement.MeasureHeightPoints;

            frmMeasureHeightPoints.Type = fmrFrmMultipleHeightMeasurement.Type;

            if (frmMeasureHeightPoints.MeasureHeightPoints == null || frmMeasureHeightPoints.MeasureHeightPoints.Count <= 0)
            {
                return;
            }

            frmMeasureHeightPoints.Show();
        }

        /// <summary>
        /// 多功能测距
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtDistanceMeasurement_Click(object sender, EventArgs e)
        {
            FrmDistanceMeasurement frmDistanceMeasurement = new FrmDistanceMeasurement();
            frmDistanceMeasurement.ShowDialog();

            frmDistanceMeasurement.Dispose();
        }

        /// <summary>
        /// 焊头水平测量
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondheadStandardMeasurement_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            FrmBondheadStandardMeasurement frmBondheadStandardMeasurement = new FrmBondheadStandardMeasurement();
            DialogResult dialog = frmBondheadStandardMeasurement.ShowDialog();

            frmBondheadStandardMeasurement.Dispose();
        }

        /// <summary>
        /// 焊头水平测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLevelMeasurement_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            FrmBondLevel frmBondLevel = new FrmBondLevel();
            frmBondLevel.ShowDialog();

            frmBondLevel.Dispose();
        }

        /// <summary>
        /// 系统2水平测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSystem2FlatLevelMeasure_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            FrmSystem2FlatLevelMeasure frmSystem2FlatLevelMeasure = new FrmSystem2FlatLevelMeasure();
            frmSystem2FlatLevelMeasure.ShowDialog();

            frmSystem2FlatLevelMeasure.Dispose();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (MachineStateModel.GetInstance().CurrentMachineSystem)
            {
                case CurrentMachineSystemEnum.System1:
                    this.BtnSystem1FlatLevelMeasure.Enabled = true;
                    this.BtnSystem2FlatLevelMeasure.Enabled = false;
                    break;

                case CurrentMachineSystemEnum.System2:
                    this.BtnSystem1FlatLevelMeasure.Enabled = false;
                    this.BtnSystem2FlatLevelMeasure.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// 系统1水平测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSystem1FlatLevelMeasure_Click(object sender, EventArgs e)
        {
            FrmSystem1FlatLevelMeasure frmSystem1FlatLevelMeasure = new FrmSystem1FlatLevelMeasure();
            frmSystem1FlatLevelMeasure.ShowDialog();

            frmSystem1FlatLevelMeasure.Dispose();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcMeasuringTool_Load(object sender, EventArgs e)
        {
            if (!MachineHardwareConfiguration.GetInstance().IsSystem1Configrated)
            {
                this.BtnSystem1FlatLevelMeasure.Enabled = false;
            }
        }
    }
}