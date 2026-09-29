using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controls.Manual
{         
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 点胶手动界面
    /// </summary>
    public partial class UcDispenseManual : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController DispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcDispenseManual()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.Timer.Tick -= Timer_Tick;
                    this.Timer.Dispose();
                };
        }

        /// <summary>
        /// 打开/关闭预点胶阀
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeDispensing_Click(object sender, EventArgs e)
        {
            if (this.DispenseController.IsDispensingElectricOpen())
            {
                this.DispenseController.CloseDispensingElectric();
            }
            else
            {
                System1Domain.GetInstance().DispenseController.SetDispensePressure(DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum, 0);
                this.DispenseController.OpenDispensingElectric();
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
                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                {
                    return;
                }

                if (this.DispenseController.IsDispensingElectricOpen())
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

                if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigPrintingTool)
                {
                    if (this.DispenseController.IsPrintToolMove())
                    {
                        string message = "停止转动";
                        this.BtPrintEpoxyMove.Text = message;
                        this.BtPrintEpoxyMove.Appearance.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        string message = "蘸胶盘开启转动";
                        this.BtPrintEpoxyMove.Text = message;
                        this.BtPrintEpoxyMove.Appearance.BackColor = Color.White;
                    }
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
            FrmDispensePlateAssistant frmDispensePlateAssistant = new FrmDispensePlateAssistant();
            frmDispensePlateAssistant.ShowDialog();
            System1Domain.GetInstance().DispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();
            System1Domain.GetInstance().DispenseController.MoveToSafePos();
        }

        /// <summary>
        /// 换胶水和胶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeDispenser_Click(object sender, EventArgs e)
        {
            FrmChangeEpoxy frmChangeEpoxy = new FrmChangeEpoxy();
            frmChangeEpoxy.ShowDialog();
            frmChangeEpoxy.Dispose();
        }

        /// <summary>
        /// 清理预点胶板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCleanPreDispensePlate_Click(object sender, EventArgs e)
        {
            System1Domain.GetInstance().System1Program.PreDispensePlateProgram.InitPre();
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
            if (System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off)
            {
                AKRSXtraMessageBox.Show("请在程式中开启预点胶");
                return;
            }

            System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.IsManual = true;

            System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.DoWork();
        }

        /// <summary>
        /// 刮胶盘转动
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SpPrintEpoxyMove_Click(object sender, EventArgs e)
        {
            if (this.DispenseController.IsPrintToolMove())
            {
                this.DispenseController.PrintToolStop();
            }
            else
            {
                this.DispenseController.PrintToolContinueMove();
            }
            
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void UcDispenseManual_Load(object sender, EventArgs e)
        {
            this.BtPrintEpoxyMove.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1ConfigPrintingTool;
        }


        /// <summary>
        /// 示教距离
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAssistantDistance_Click(object sender, EventArgs e)
        {
            FrmAssistantPreDispenseDistance frmAssistantPreDispenseDistance = new FrmAssistantPreDispenseDistance();
            frmAssistantPreDispenseDistance.ShowDialog();
            frmAssistantPreDispenseDistance.Dispose();
        }
    }
}