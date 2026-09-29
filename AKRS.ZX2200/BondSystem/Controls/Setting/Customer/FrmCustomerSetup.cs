namespace AKRS.ZX2200.BondSystem.Controls.Setting.Customer
{
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.MotionPlan;
    using AKRS.ZX2200.WaferSubSystem.Controls.Manual;
    using DevExpress.XtraEditors;
    using System;
    using System.Windows.Forms;

    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    /// <summary>
    /// 设备参数示教
    /// </summary>
    public partial class FrmCustomerSetup : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCustomerSetup()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        ///  初始化
        /// </summary>
        private void InitControl()
        {
            this.BtnTeachSlideFluxer.Enabled = MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured;
            this.BtnTeachJoystick.Enabled = MachineHardwareConfiguration.GetInstance().IsJoyStickConfigured;
            this.BtAssistantPrintPos.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem2ConfigPrintingTool;
            this.BtnLearnIPT.Enabled = MachineHardwareConfiguration.GetInstance().IsIPTConfigrated;
            this.BtnTeachRightIPT.Enabled = MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated;
        }

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 吸嘴架示教按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLearnPPToolbank_Click(object sender, EventArgs e)
        {
            //FrmLearnNozzleBank1 frmLearnNozzleBank1 = new FrmLearnNozzleBank1();
            //if (frmLearnNozzleBank1.IsShowDialog())
            //{
            //    frmLearnNozzleBank1.ShowDialog();

            //    frmLearnNozzleBank1.Dispose();
            //}

            FrmLearnToolBank frmLearnToolBank = new FrmLearnToolBank();
            frmLearnToolBank.ShowDialog();

            frmLearnToolBank.Dispose();
        }

        /// <summary>
        /// IPT示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLearnIPT_Click(object sender, EventArgs e)
        {
            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmLearnIPT frmLearnIPT = new FrmLearnIPT(IPTTypeEnum.LeftIPT);
            frmLearnIPT.ShowDialog();

            frmLearnIPT.Dispose();
        }

        /// <summary>
        /// 抛料盒示教
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAssistantBlowOffBox_Click(object sender, EventArgs e)
        {
            FrmBlowOffBoxAssistant frmBlowOffBoxAssistant = new FrmBlowOffBoxAssistant();
            frmBlowOffBoxAssistant.ShowDialog();

            frmBlowOffBoxAssistant.Dispose();
        }

        /// <summary>
        /// 示教ULM运动点位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtLearnULMPositions_Click(object sender, EventArgs e)
        {
            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmTeachULMPositions frmTeachULMPositions = new FrmTeachULMPositions();
            frmTeachULMPositions.ShowDialog();

            frmTeachULMPositions.Dispose();
        }

        /// <summary>
        /// 示教吸嘴擦拭位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNozzleCleanPosTeach_Click(object sender, EventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().IsNozzleCleanTableConfigured == false)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先将在参数界面配置吸嘴清洁台!",
                    "提示",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmNozzleCleanPosTeach frmNozzleCleanPosTeach = new FrmNozzleCleanPosTeach();
            frmNozzleCleanPosTeach.ShowDialog();

            frmNozzleCleanPosTeach.Dispose();
        }

        /// <summary>
        ///  示教刮胶盘
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeachSlideFluxer_Click(object sender, EventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先将在参数界面配置刮胶盘!",
                    "提示",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmSlideFluxerTeach frmSlideFluxerTeach = new FrmSlideFluxerTeach();
            frmSlideFluxerTeach.ShowDialog();

            frmSlideFluxerTeach.Dispose();
        }

        /// <summary>
        /// 示教翻转台
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnTeachFlipTable_Click(object sender, EventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated == false)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先将在参数界面配置翻转台!",
                    "提示",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Information);

                return;
            }

            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmFlipTableTeach frmFlipTableTeach = new FrmFlipTableTeach();
            frmFlipTableTeach.ShowDialog();

            frmFlipTableTeach.Dispose();
        }

        /// <summary>
        /// 示教取片安全区域
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantPickArea_Click(object sender, EventArgs e)
        {
            AKRS.ZX2200.SupportFeature.MotionPlan.FrmAssistantPickArea frm = new FrmAssistantPickArea();
            frm.ShowDialog();
            frm.Dispose();
        }

        /// <summary>
        ///  s示教摇杆
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnTeachJoystick_Click(object sender, EventArgs e)
        {
            FrmTeachJoystick frmTeachJoystick = new FrmTeachJoystick();
            frmTeachJoystick.ShowDialog();
            frmTeachJoystick.Dispose();
        }

        /// <summary>
        /// 示教蘸胶位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantPrintPos_Click(object sender, EventArgs e)
        {
            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmLearnPrintPos frmLearnPrintPos = new FrmLearnPrintPos();
            frmLearnPrintPos.ShowDialog();

            frmLearnPrintPos.Dispose();
        }

        /// <summary>
        /// 示教右中转台
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnTeachRightIPT_Click(object sender, EventArgs e)
        {
            if (!this.system2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmLearnIPT frmLearnIPT = new FrmLearnIPT(IPTTypeEnum.RightIPT);
            frmLearnIPT.ShowDialog();

            frmLearnIPT.Dispose();
        }
    }
}