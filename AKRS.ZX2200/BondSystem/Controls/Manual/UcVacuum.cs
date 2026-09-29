using System;
using System.Drawing;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.WaferSubSystem.Modules;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Localization;
    using AKRS.ZX2200.WaferSubSystem.Controllers;

    using DevExpress.XtraEditors.Senders;

    /// <summary>
    /// 真空手动调试窗体
    /// </summary>
    public partial class UcVacuum : DevExpress.XtraEditors.XtraUserControl, ILanguage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcVacuum()
        {
            InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick; 
                    this.timer1.Dispose();
                };
            this.RefreshControl();

            FormLocalizer.LocalizeForm(this);
        }

        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 顶针台模组
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// 中转台模组
        /// </summary>
        private IPTModule iPTModule => System2Module.GetInstance().IPTModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshControl()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // 判断吸嘴真空状态
            if (this.BondModule.BondHead.VaccumElectric.GetOutputValue())
            {
                this.BtnNozzleVacuum.Text = @"关吸嘴真空";
                this.BtnNozzleVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnNozzleVacuum.Text = @"开吸嘴真空";
                this.BtnNozzleVacuum.Appearance.BackColor = Color.Transparent;
            }

            // 判断吸嘴吹气状态
            if (this.BondModule.BondHead.WeakBlowElectric.GetOutputValue())
            {
                this.BtnWeakBlow.Text = @"关吸嘴吹气";
                this.BtnWeakBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnWeakBlow.Text = @"开吸嘴吹气";
                this.BtnWeakBlow.Appearance.BackColor = Color.Transparent;
            }

            // 判断顶针真空状态
            if (this.EjectModule.EjectionTableVacuum.GetOutputValue())
            {
                this.BtnESVacuum.Text = @"关顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnESVacuum.Text = @"开顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Transparent;
            }

            // 判断顶针吹气状态
            if (this.EjectModule.EjectionTableBlow.GetOutputValue())
            {
                this.BtnESBlow.Text = @"关顶针吹气";
                this.BtnESBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnESBlow.Text = @"开顶针吹气";
                this.BtnESBlow.Appearance.BackColor = Color.Transparent;
            }

            if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated)
            {
                // 判断IPT真空
                if (this.iPTModule.IPTVacuumElectric.GetOutputValue())
                {
                    this.BtnIPTVacuum.Text = @"关中转台真空";
                    this.BtnIPTVacuum.Appearance.BackColor = Color.Yellow;
                }
                else
                {
                    this.BtnIPTVacuum.Text = @"开中转台真空";
                    this.BtnIPTVacuum.Appearance.BackColor = Color.Transparent;
                }
            }
            else
            {
                this.BtnIPTVacuum.Enabled = false;
            }

            // 判断焊头吸附真空
            if (this.BondModule.BondHead.BondHeadVaccumElectric.GetOutputValue())
            {
                this.BtnBondHeadVacuum.Text = @"关焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnBondHeadVacuum.Text = @"开焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Transparent;
            }

            this.TxtVacuumValue.Text = this.bondHeadController.ReadVacuumValue().ToString();

            this.TxtBondheadVacuumValue.Text = this.bondHeadController.ReadBondheadVaccumValue().ToString();

            this.Enabled =! Machine.GetInstance().IsWorking();

            this.BtnFlipTableVacuum.Enabled = MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated;
        }

        /// <summary>
        /// 吸嘴真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnNozzleVacuum_Click(object sender, EventArgs e)
        {
            if (!this.BondModule.BondHead.VaccumElectric.GetOutputValue())
            {
                // 先关吹气电磁阀
                this.BondModule.BondHead.WeakBlowElectric.SetOutputValue(false);

                // 开真空
                this.BondModule.BondHead.VaccumElectric.SetOutputValue(true);
                this.BtnNozzleVacuum.Text = @"关吸嘴真空";
                this.BtnNozzleVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BondModule.BondHead.VaccumElectric.SetOutputValue(false);
                this.BtnNozzleVacuum.Text = @"开吸嘴真空";
                this.BtnNozzleVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 吸嘴弱吹
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnWeakBlow_Click(object sender, EventArgs e)
        {
            if (!this.BondModule.BondHead.WeakBlowElectric.GetOutputValue())
            {
                // 先关真空电磁阀
                this.BondModule.BondHead.VaccumElectric.SetOutputValue(false);

                // 开吹气
                this.BondModule.BondHead.WeakBlowElectric.SetOutputValue(true);
                this.BtnWeakBlow.Text = @"关吸嘴吹气";
                this.BtnWeakBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BondModule.BondHead.WeakBlowElectric.SetOutputValue(false);
                this.BtnWeakBlow.Text = @"开吸嘴吹气";
                this.BtnWeakBlow.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 顶针真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESVacuum_Click(object sender, EventArgs e)
        {
            if (!this.EjectModule.EjectionTableVacuum.GetOutputValue())
            {
                // 先关吹气电磁阀
                this.EjectModule.EjectionTableVacuum.SetOutputValue(false);

                // 开真空
                this.EjectModule.EjectionTableVacuum.SetOutputValue(true);
                this.BtnESVacuum.Text = @"关顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.EjectModule.EjectionTableVacuum.SetOutputValue(false);
                this.BtnESVacuum.Text = @"开顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 顶针吹气
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESBlow_Click(object sender, EventArgs e)
        {
            if (!this.EjectModule.EjectionTableBlow.GetOutputValue())
            {
                // 先关真空电磁阀
                this.EjectModule.EjectionTableBlow.SetOutputValue(false);

                // 开吹气
                this.EjectModule.EjectionTableBlow.SetOutputValue(true);
                this.BtnESBlow.Text = @"关顶针吹气";
                this.BtnESBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.EjectModule.EjectionTableBlow.SetOutputValue(false);
                this.BtnESBlow.Text = @"开顶针吹气";
                this.BtnESBlow.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// IPT真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnIPTVacuum_Click(object sender, EventArgs e)
        {
            if (!this.iPTModule.IPTVacuumElectric.GetOutputValue())
            {
                // 开真空电磁阀
                this.iPTModule.IPTVacuumElectric.SetOutputValue(true);
                this.BtnIPTVacuum.Text = @"关中转台真空";
                this.BtnIPTVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                // 关真空
                this.iPTModule.IPTVacuumElectric.SetOutputValue(false);
                this.BtnIPTVacuum.Text = @"开中转台真空";
                this.BtnIPTVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 焊头吸附
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBondHeadVacuum_Click(object sender, EventArgs e)
        {
            if (this.BondModule.BondHead.BondHeadVaccumElectric.GetOutputValue())
            {
                // 关焊头吸附
                this.BondModule.BondHead.BondHeadVaccumElectric.SetOutputValue(false);
                this.BtnBondHeadVacuum.Text = @"开焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Transparent;
            }
            else
            {
                // 开焊头吸附
                this.BondModule.BondHead.BondHeadVaccumElectric.SetOutputValue(true);
                this.BtnBondHeadVacuum.Text = @"关焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.RefreshControl();
        }

        /// <summary>
        /// 翻转台真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFlipTableVacuum_Click(object sender, EventArgs e)
        {
            if (this.flipTableController.GetFlipTableVacuumState())
            {
                // 关翻转台真空
                this.flipTableController.CloseFlipTableVacuum();
                this.BtnFlipTableVacuum.Text = @"开翻转台真空";
                this.BtnFlipTableVacuum.Appearance.BackColor = Color.Transparent;
            }
            else
            {
                // 开翻转台真空
                this.flipTableController.OpenFlipTableVacuum();
                this.BtnFlipTableVacuum.Text = @"关翻转台真空";
                this.BtnFlipTableVacuum.Appearance.BackColor = Color.Yellow;
            }
        }

        public void GetUI()
        {
            if (this.Parent == null)
            {
                this.Text = LocalizationManager.GetUI(this.GetType().Name, "Title");
            }
            else
            {
                this.Parent.Parent.Text = LocalizationManager.GetUI(this.GetType().Name, "Title");
            }
        }
    }
}