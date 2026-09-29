using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Manual
{
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.XtraEditors.Controls;
    using log4net.Core;
    using OfficeOpenXml;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// 手动
    /// </summary>
    public partial class UcEjectBank : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// 上晶圆程式
        /// </summary>
        private WaferSystemDomain WaferSystemDomain => WaferSystemDomain.GetInstance() ?? new WaferSystemDomain();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcEjectBank()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= this.Timer1_Tick;
                    this.timer1.Dispose();
                };

            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 绑定顶针
            this.CmbEjectionName.Properties.Items.Clear();

            if (WaferSystemProgram.GetInstance().GetDistinctEjectionSettings().Count != 0)
            {
                foreach (var item in WaferSystemProgram.GetInstance().GetDistinctEjectionSettings())
                {
                    this.CmbEjectionName.Properties.Items.Add(item.Name);
                }

                if (EjectDevicePara.CurrentSlotConfig != null)
                {
                    this.CmbEjectionName.EditValue = EjectDevicePara.CurrentSlotConfig.Name;
                }

                this.RefreshControl();
            }            
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            this.GcEjectionBank.DataSource = WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities;
            this.GcEjectionBank.Refresh();
            this.GvEjectionBank.RefreshData();
        }

        /// <summary>
        /// BtnChangeTool
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnChangeTool_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {           
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                string slotName = this.CmbEjectionName.Text;
                EjectionBankSlotConfig slotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.FirstOrDefault(a => a.Name == slotName);

                if (slotConfig != null)
                {
                    WaferSubController.GetInstance().EjectController.ChangeEjection(slotConfig.Index, true); 
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            {
                this.RefreshControl();
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnMaintenance
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton; 
            try
            {                
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                // 归还顶针
                WaferSubController.GetInstance().EjectController.ReturnEjection();                
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.RefreshControl();
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnMeasurement_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMeasurement_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                string slotName = this.CmbEjectionName.Text;
                EjectionBankSlotConfig slotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.FirstOrDefault(a => a.Name == slotName);

                if (slotConfig != null)
                {
                    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

                    // 自动换Touchdown
                    if (System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
                    {
                        // 顶针安装精度实验
                        FrmEjectMeasurementTeach frmEjectMeasurementTeach = new FrmEjectMeasurementTeach(slotConfig);
                        frmEjectMeasurementTeach.ShowDialog();
                        frmEjectMeasurementTeach.Dispose();

                        System2Domain.GetInstance().System2Controller.PutbackNozzleAssitance();
                    }
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 界面加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void UcEjectBank_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 雨刷器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.BtnChangeTool.Enabled = Machine.GetInstance().IsWorking() == false && !string.IsNullOrEmpty(this.CmbEjectionName.Text);
            this.BtnMaintenance.Enabled = Machine.GetInstance().IsWorking() == false && !string.IsNullOrEmpty(this.CmbEjectionName.Text);
            this.BtnMeasurement.Enabled = Machine.GetInstance().IsWorking() == false;
        }
    }
}
