using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Parameter;
using AKRS.ZX2200.Main.Machine.MachineSupport;
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

namespace AKRS.ZX2200.Main.Machine.Control
{
    /// <summary>
    ///  设备硬件配置
    /// </summary>
    public partial class FrmMachineHardWareConfig : XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmMachineHardWareConfig()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        private MachineHardwareConfiguration MachineHardwareConfiguration => MachineHardwareConfiguration.GetInstance();

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSave_Click(object sender, EventArgs e)
        {
            #region 系统1配置

            this.MachineHardwareConfiguration.IsSystem1Configrated = this.ChkSystem1Config.Checked;
            this.MachineHardwareConfiguration.IsSystem1ConfigPrintingTool = this.ChkIsPrintConfig.Checked;
            this.MachineHardwareConfiguration.IsSystem1ConfigLaserMh = this.ChkIsSystem1LaserConfig.Checked;

            #endregion

            #region 系统2配置

            this.MachineHardwareConfiguration.IsSystem2Dispense = this.ChkIsSystem2DispenseConfig.Checked;
            this.MachineHardwareConfiguration.IsSystem2ConfigLaserMh = this.ChkIsSystem2LaserConfig.Checked;
            this.MachineHardwareConfiguration.IsIPTConfigrated = this.ChkIsIptConfig.Checked;


            this.MachineHardwareConfiguration.IsFlipModuleConfigrated = this.ChkIsFcConfig.Checked;
            this.MachineHardwareConfiguration.IsSlideFluxerConfigured = this.ChkIsSqueegeeConfig.Checked;
            this.MachineHardwareConfiguration.IsNozzleCleanTableConfigured = this.ChkIsNozzleCleanConfig.Checked;
            this.MachineHardwareConfiguration.IsLaserEncoderConfigured = this.ChkLaserEncoder.Checked;

            System2Configuration.GetInstance().IsActivePickUpCompensate = this.ChkIsPickMarkConfig.Checked;
            System2Configuration.GetInstance().IsToolBankEnable = this.ChkIsNozzleShelfConfig.Checked;

            #endregion

            #region 轨道配置

            this.MachineHardwareConfiguration.LoadConfiguration = (LoadConfigurationEnum)Enum.Parse(typeof(LoadConfigurationEnum), this.LueTransportType.EditValue.ToString());
            this.MachineHardwareConfiguration.IsTransportConfigured = this.ChkIsTransportConfig.Checked;
            this.MachineHardwareConfiguration.IsTransportHeatConfigured = this.ChkIsTransportHeaterConfig.Checked;

            #endregion

            #region 晶圆配置

            this.MachineHardwareConfiguration.IsStaticWaffleConfigrated = this.ChkIsStaticWaffleConfig.Checked;
            this.MachineHardwareConfiguration.IsEjectSystemConfigured = this.ChkIsEjectSystemConfig.Checked;

            #endregion

            #region 通用配置

            this.MachineHardwareConfiguration.LightConfig = (LightConfigEnum)Enum.Parse(typeof(LightConfigEnum), this.LueLightType.EditValue.ToString());
            this.MachineHardwareConfiguration.IsSafeDoorConfigured = this.ChkIsSafeDoorConfig.Checked;
            this.MachineHardwareConfiguration.IsJoyStickConfigured = this.ChkIsJoystickConfig.Checked;

            #endregion

            MachineHardwareConfiguration.Save();
            System2Configuration.GetInstance().Save();
            XtraMessageBox.Show("保存成功");
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMachineHardWareConfig_Load(object sender, EventArgs e)
        {
            #region 系统1配置

            this.ChkSystem1Config.Checked = this.MachineHardwareConfiguration.IsSystem1Configrated;
            this.ChkIsPrintConfig.Checked = this.MachineHardwareConfiguration.IsSystem1ConfigPrintingTool;
            this.ChkIsSystem1LaserConfig.Checked = this.MachineHardwareConfiguration.IsSystem1ConfigLaserMh;

            #endregion

            #region 系统2配置

            this.ChkIsSystem2DispenseConfig.Checked = this.MachineHardwareConfiguration.IsSystem2Dispense;
            this.ChkIsSystem2LaserConfig.Checked = this.MachineHardwareConfiguration.IsSystem2ConfigLaserMh;
            this.ChkIsIptConfig.Checked = this.MachineHardwareConfiguration.IsIPTConfigrated;


            this.ChkIsFcConfig.Checked = this.MachineHardwareConfiguration.IsFlipModuleConfigrated;
            this.ChkIsSqueegeeConfig.Checked = this.MachineHardwareConfiguration.IsSlideFluxerConfigured;
            this.ChkIsNozzleCleanConfig.Checked = this.MachineHardwareConfiguration.IsNozzleCleanTableConfigured;

            this.ChkIsPickMarkConfig.Checked = System2Configuration.GetInstance().IsActivePickUpCompensate;
            this.ChkIsNozzleShelfConfig.Checked = System2Configuration.GetInstance().IsToolBankEnable;
            this.ChkLaserEncoder.Checked = this.MachineHardwareConfiguration.IsLaserEncoderConfigured;

            #endregion

            #region 轨道配置

            this.LueTransportType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LoadConfigurationEnum>();
            this.LueTransportType.EditValue = this.MachineHardwareConfiguration.LoadConfiguration;
            this.ChkIsTransportConfig.Checked = this.MachineHardwareConfiguration.IsTransportConfigured;
            this.ChkIsTransportHeaterConfig.Checked = this.MachineHardwareConfiguration.IsTransportHeatConfigured;
           
            #endregion

            #region 晶圆配置

            this.ChkIsStaticWaffleConfig.Checked = this.MachineHardwareConfiguration.IsStaticWaffleConfigrated;
            this.ChkIsEjectSystemConfig.Checked = this.MachineHardwareConfiguration.IsEjectSystemConfigured;

            #endregion

            #region 通用配置

            this.LueLightType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LightConfigEnum>();
            this.LueLightType.EditValue = this.MachineHardwareConfiguration.LightConfig;
            this.ChkIsSafeDoorConfig.Checked = this.MachineHardwareConfiguration.IsSafeDoorConfigured;
            this.ChkIsJoystickConfig.Checked = this.MachineHardwareConfiguration.IsJoyStickConfigured;

            #endregion
        }

        /// <summary>
        /// 系统1配置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void ChkSystem1Config_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.ChkSystem1Config.Checked)
            {
                this.ChkIsSystem1LaserConfig.Enabled = false;
                this.ChkIsPrintConfig.Enabled = false;
                this.ChkIsSystem1LaserConfig.Checked = false;
                this.ChkIsPrintConfig.Checked = false;
            }
            else
            {
                this.ChkIsSystem1LaserConfig.Enabled = true;
                this.ChkIsPrintConfig.Enabled = true;
            }
        }
    }
}
