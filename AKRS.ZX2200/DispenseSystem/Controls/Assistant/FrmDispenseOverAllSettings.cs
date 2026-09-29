using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Action;

namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 点胶头点胶材料选择
    /// </summary>
    public partial class FrmDispenseOverAllSettings : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public FrmDispenseOverAllSettings()
        {
            this.InitializeComponent();

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                BondProgram.GetInstance().S2DispenserProgram.DispenserName = this.CmbDispenser.Text;
                BondProgram.GetInstance().S2EpoxyMaterialProgram.EpoxyMaterialName = this.CmbDispensingMeterialReference.Text;

                BondProgram.GetInstance().Save();
            }
            else
            {
                this.CmbDispenser.Text = System1Program.GetInstance().DispenserProgram.DispenserName;
                this.CmbDispensingMeterialReference.Text = System1Program.GetInstance().EpoxyMaterialProgram.EpoxyMaterialName;
            }
            
            this.RefreshCmbItems();
        }

        /// <summary>
        /// 选择Dispenser
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbDispenser_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<Dispenser> frmRepository = new FrmRepository<Dispenser>(DispenserRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    Dispenser dispenser = (Dispenser)frmRepository.DsSetting;

                    if (dispenser != null)
                    {
                        ((ComboBoxEdit)sender).Text = dispenser.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 选择胶水
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbDispensingMeterialReference_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Index == 1)
            {
                FrmRepository<EpoxyMaterial> frmRepository = new FrmRepository<EpoxyMaterial>(EpoxyMaterialRepository.GetInstance());

                if (frmRepository.ShowDialog() == DialogResult.OK)
                {
                    EpoxyMaterial epoxyMaterial = (EpoxyMaterial)frmRepository.DsSetting;

                    if (epoxyMaterial != null)
                    {
                        ((ComboBoxEdit)sender).Text = epoxyMaterial.Name;
                    }
                }

                this.RefreshCmbItems();
            }
        }

        /// <summary>
        /// 刷新下拉框
        /// </summary>
        private void RefreshCmbItems()
        {
            this.CmbDispenser.Properties.Items.Clear();
            List<string> nameList = DispenserRepository.GetInstance().GetDataSourceByCurrentRecipe().Select(a => a.Name).ToList();
            this.CmbDispenser.Properties.Items.AddRange(nameList);

            this.CmbDispensingMeterialReference.Properties.Items.Clear();
            nameList = EpoxyMaterialRepository.GetInstance().GetDataSourceByCurrentRecipe().Select(a => a.Name).ToList();
            this.CmbDispensingMeterialReference.Properties.Items.AddRange(nameList);
        }

        /// <summary>
        /// 保存参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                BondProgram.GetInstance().S2DispenserProgram.DispenserName = this.CmbDispenser.Text;
                BondProgram.GetInstance().S2EpoxyMaterialProgram.EpoxyMaterialName = this.CmbDispensingMeterialReference.Text;

                BondProgram.GetInstance().Save();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                System1Domain.GetInstance().System1Program.DispenserProgram.DispenserName = this.CmbDispenser.Text;
                System1Domain.GetInstance().System1Program.EpoxyMaterialProgram.EpoxyMaterialName = this.CmbDispensingMeterialReference.Text;

                System1Domain.GetInstance().System1Program.Save();

                this.DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
