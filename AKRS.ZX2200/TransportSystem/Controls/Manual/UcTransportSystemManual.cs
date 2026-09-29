using System;
using System.Threading.Tasks;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Enums;

namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 流道调试菜单
    /// </summary>
    public partial class UcTransportSystemManual : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 基板流道域
        /// </summary>
        private TransportDomain transportDomain => TransportDomain.GetInstance();

        /// <summary>
        /// 流道控制器
        /// </summary>
        private TransportController transportController => transportDomain.TransportController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTransportSystemManual()
        {
            this.InitializeComponent();
            this.ChkContinuousFeeding.Checked = TransportProvider.ContinuousFeeding;

            this.BtTransportUnitIndex.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.BtInitializeTS.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.BtSearchBelt1Sys1.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
        }

        /// <summary>
        /// 不再上料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkEmptyIndexOn_CheckedChanged(object sender, EventArgs e)
        {
            TransportProvider.ContinuousFeeding = this.ChkContinuousFeeding.Checked;
        }

        /// <summary>
        /// 搜索皮带2系统2
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtTransportUnitIndex_Click(object sender, EventArgs e)
        {
            this.transportController.TransportUnitIndex();
        }

        /// <summary>
        /// 初始化流道
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtInitializeTS_Click(object sender, EventArgs e)
        {
            this.transportController.InitializeTS();
        }

        /// <summary>
        /// 夹紧夹爪
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClampTransportUnit_Click(object sender, EventArgs e)
        {
            this.BtUnclampTransportUnit.Enabled = true;
            switch (MachineStateModel.GetInstance().CurrentMachineSystem)
            {
                case CurrentMachineSystemEnum.System1:
                    transportController.DispenseSubSectionController.Clamp(true);
                    break;
                case CurrentMachineSystemEnum.System2:
                    transportController.BondSubSectionController.Clamp();
                    break;
            }

            this.BtClampTransportUnit.Enabled = false;
        }

        /// <summary>
        /// 松开夹爪
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUnclampTransportUnit_Click(object sender, EventArgs e)
        {
            this.BtClampTransportUnit.Enabled = true;
            switch (MachineStateModel.GetInstance().CurrentMachineSystem)
            {
                case CurrentMachineSystemEnum.System1:
                    transportController.DispenseSubSectionController.UnClamp();

                    break;
                case CurrentMachineSystemEnum.System2:
                    transportController.BondSubSectionController.UnClamp();
                    break;
            }

            this.BtUnclampTransportUnit.Enabled = false;
        }


        /// <summary>
        /// 搜索皮带系统,一般用于产品卡料或者没有下压到位使用此功能
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSearchBelt1Sys1_Click(object sender, EventArgs e)
        {
            transportController.DispenseSubSectionController.MapBelt(false);
        }

        /// <summary>
        /// 搜索皮带系统,一般用于产品卡料或者没有下压到位使用此功能
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSearchBelt2Sys2_Click(object sender, EventArgs e)
        {
            transportController.BondSubSectionController.MapBelt(false);
        }

        /// <summary>
        /// 清空点胶台载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClearDispense_Click(object sender, EventArgs e)
        {
            transportController.InitializeTSInDispense();
        }

        /// <summary>
        /// 清空工作台载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClearBond_Click(object sender, EventArgs e)
        {
            transportController.InitializeTSInBond();
        }

        /// <summary>
        /// 手动释放真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtManualOpenVacuum_Click(object sender, EventArgs e)
        {
            this.transportController.BondSubSectionController.ManualCloseVacuum();
        }
    }
}