using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controls.Manual
{         
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Design;

    /// <summary>
    /// 点胶手动界面
    /// </summary>
    public partial class UcS2DispenseManual : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 点胶控制器
        /// </summary>
        private S2DispenseController S2DispenseController => System2Domain.GetInstance().S2DispenseController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcS2DispenseManual()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.Timer.Tick -= Timer_Tick;
                    this.Timer.Dispose();
                };

            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                this.BtChangeDispensing.Enabled = false;
                this.BtChangeDispenser.Enabled = false;
                this.BtPredispense.Enabled = false;
            }

        }

        /// <summary>
        /// 打开/关闭预点胶阀
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeDispensing_Click(object sender, EventArgs e)
        {
            if (this.S2DispenseController.IsDispensingElectricOpen())
            {
                this.S2DispenseController.CloseDispensingElectric();
            }
            else
            {
                this.S2DispenseController.OpenDispensingElectric();
            }
        }

        /// <summary>
        /// 时钟
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {

                if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
                {
                    return;
                }

                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    return;
                }

                if (this.S2DispenseController.IsDispensingElectricOpen())
                {
                    string message = "关闭挤胶";
                    this.BtChangeDispensing.Text = message;
                    this.BtChangeDispensing.Appearance.BackColor = Color.LightGreen;
                }
                else
                {
                    string message = "打开挤胶";
                    this.BtChangeDispensing.Text = message;
                    this.BtChangeDispensing.Appearance.BackColor = Color.White;
                }
            }
            catch (Exception exception)
            {
                this.Timer.Enabled = false;
                this.Timer.Stop();
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// 示教预点胶板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantPrePlate_Click(object sender, EventArgs e)
        {
            FrmS2DispensePlateAssistant frmS2DispensePlateAssistant = new FrmS2DispensePlateAssistant();
            frmS2DispensePlateAssistant.ShowDialog();
        }

        /// <summary>
        /// 换胶水和胶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeDispenser_Click(object sender, EventArgs e)
        {
            FrmAssistantDispense frmAssistantDispense = new FrmAssistantDispense();
            frmAssistantDispense.ShowDialog();
            frmAssistantDispense.Dispose();
        }

        /// <summary>
        /// 清理预点胶板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCleanPreDispensePlate_Click(object sender, EventArgs e)
        {
            System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.InitPre();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmDispenseManual_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Timer.Enabled = false;
        }

        /// <summary>
        /// 预点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPreDispense_Click(object sender, EventArgs e)
        {
            if (System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off)
            {
                AKRSXtraMessageBox.Show("请在程式中开启预点胶");
                return;
            }

            System2RunTimeProvider.IsPreDispense = true;

            System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.IsManual = true;

            System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.DoWork();
        }

        /// <summary>
        /// 打开关闭蘸胶盘
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStopRotate_Click(object sender, EventArgs e)
        {
            if (this.BtStopRotate.Text == "旋转蘸胶盘")
            {
                System2Domain.GetInstance().S2DispenseController.PrintToolContinueMove();
                this.BtStopRotate.Text = "停止蘸胶盘";
            }
            else
            {
                System2Domain.GetInstance().S2DispenseController.PrintToolStop();
                this.BtStopRotate.Text = "旋转蘸胶盘";
            }
        }
    }
}