using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    using AKRS.ZX2200.BondSystem.BondForce.Controllers;
    using AKRS.ZX2200.BondSystem.BondForce.Forms.Assistant;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 力控测量
    /// </summary>
    public partial class UcBondForceMeasurement : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public UcBondForceMeasurement()
        {
            this.InitializeComponent();

            // 开焊头吸附
            this.bondHeadController.OpenBondHeadVaccum();
        }

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 力控标定控制器
        /// </summary>
        private ClosedLoopCalibrationController closedLoopCalibrationController = new ClosedLoopCalibrationController();

        /// <summary>
        /// 力控标定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBondingForceSensorSetUp_Click(object sender, EventArgs e)
        {
            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            DialogResult dialogResult = DialogResult.OK;

            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否重新力控示教标定位置?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes) 
            {
                // 示教标定位
                FrmCalibrateTeach frmCalibrateTeach = new FrmCalibrateTeach();
                dialogResult = frmCalibrateTeach.ShowDialog();
                frmCalibrateTeach.Dispose();
            }

            if (dialogResult == DialogResult.OK) 
            {
                 dialog = AKRSXtraMessageBox.Show(
                    $"是否准备开始力控标定?",
                    "Question",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dialog == DialogResult.Yes)
                {
                    FrmCalibrationRelationship frmCalibrationRelationship = new FrmCalibrationRelationship();
                    frmCalibrationRelationship.ShowDialog();

                    frmCalibrationRelationship.Dispose();
                }
                else
                {
                    this.system2Controller.PutbackNozzleAssitance();
                }
            }
        }

        /// <summary>
        /// 力控曲线图
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBondingForce_Click(object sender, EventArgs e)
        {
            FrmCalibrateTableForceRealtimeLine frmModbus = new FrmCalibrateTableForceRealtimeLine();
            frmModbus.TopLevel = true;
            frmModbus.Show();
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
                    this.BtnBondingForceSensorSetUp.Enabled = false;
                    break;
                case CurrentMachineSystemEnum.System2:
                    this.BtnBondingForceSensorSetUp.Enabled = true;
                    break;
            }
        }

        /// <summary>
        /// 检验力
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnInspectForce_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            FrmInspectForceRes frmInspectForceRes = new FrmInspectForceRes();
            frmInspectForceRes.ShowDialog();

            frmInspectForceRes.Dispose();
        }

        /// <summary>
        ///  刷新力控初始值
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRefreshForceInitialVal_Click(object sender, EventArgs e)
        {
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            this.closedLoopCalibrationController.RecordForceInitialValWithAngle();

            AKRSXtraMessageBox.Show(
                $"刷新力控初始值成功!",
                "提示",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}