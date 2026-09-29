namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.Consumables;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Product.Statistics;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Mapping;
    using AKRS.ZX2200.WaferSubSystem.Controls;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using DevExpress.XtraBars;
    using DevExpress.XtraRichEdit.Model;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// 描述: DataAdministrator
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 统计页面
        /// </summary>
        private FrmStatistics statistics;

        /// <summary>
        /// 耗材界面
        /// </summary>
        private FrmConsumables frmConsumables;

        /// <summary>
        /// 力控实时曲线
        /// </summary>
        private FrmForceRealTimeCurve frmForceRealTimeCurve;

        /// <summary>
        /// LVDT实时曲线
        /// </summary>
        private FrmLVDTRealTimeCurve frmLVDTRealTimeCurve;

        /// <summary>
        /// 系统1传输单元界面
        /// </summary>
        private FrmTransportUnitMapping frmTransportUnitMappingSystem1;

        /// <summary>
        /// 系统2传输单元界面
        /// </summary>
        private FrmTransportUnitMapping frmTransportUnitMappingSystem2;

        /// <summary>
        /// 生产统计
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtStatics_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.statistics == null || this.statistics.IsDisposed)
            {
                this.statistics = new FrmStatistics();
            }

            this.statistics.TopLevel = false;
            this.statistics.TopLevel = true;
            this.statistics.WindowState = FormWindowState.Normal;
            this.statistics.Show();
        }

        /// <summary>
        /// 系统1的载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarSystem1Tu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmTransportUnitMappingSystem1 == null || this.frmTransportUnitMappingSystem1.IsDisposed)
            {
                this.frmTransportUnitMappingSystem1 = new FrmTransportUnitMapping(
                    TransportProgram.GetInstance().DispenseSubSectionProgram);
            }


            this.frmTransportUnitMappingSystem1.TopLevel = false;
            this.frmTransportUnitMappingSystem1.TopLevel = true;
            this.frmTransportUnitMappingSystem1.WindowState = FormWindowState.Normal;
            this.frmTransportUnitMappingSystem1.Show();
        }

        /// <summary>
        /// 系统2的载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarSystem2Tu_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmTransportUnitMappingSystem2 == null || this.frmTransportUnitMappingSystem2.IsDisposed)
            {
                this.frmTransportUnitMappingSystem2 = new FrmTransportUnitMapping(
                    TransportProgram.GetInstance().BondSubSectionProgram);
            }

            this.frmTransportUnitMappingSystem2.TopLevel = false;
            this.frmTransportUnitMappingSystem2.TopLevel = true;
            this.frmTransportUnitMappingSystem2.WindowState = FormWindowState.Normal;
            this.frmTransportUnitMappingSystem2.Show();
        }

        /// <summary>
        /// 耗材
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtConsumables_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmConsumables == null || this.frmConsumables.IsDisposed)
            {
                List<TimeConsumable> epoxyConsumables = System1Program.GetInstance().GetEpoxyConsumable();

                epoxyConsumables.AddRange(BondProgram.GetInstance().GetConsumable());

                this.frmConsumables = new FrmConsumables(
                    BondProgram.GetInstance().NozzleShelfProgram.GetToolConsumable(),
                    WaferSystemProgram.GetInstance().EjectionBankProgram.GetEjectFrequencyList(),
                    epoxyConsumables,
                    BondProgram.GetInstance().SlideFluxerProgram.FluxConsumable);
            }

            this.frmConsumables.TopLevel = false;
            this.frmConsumables.TopLevel = true;
            this.frmConsumables.WindowState = FormWindowState.Normal;
            this.frmConsumables.Show();
        }

        /// <summary>
        /// 开始/停止读焊头力
        /// </summary>
        /// <param name="isActive">是否开启</param>
        private void ReadBondForce(bool isActive)
        {
            FrmForceRealTimeCurve.ReadBondForce(isActive);
        }

        /// <summary>
        /// 设置限定线
        /// </summary>
        /// <param name="force">力</param>
        private void SetConstantLine(double force)
        {
            FrmForceRealTimeCurve.SetForceReferenceValue(force);
        }


        /// <summary>
        ///  力控实时曲线
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtBondForceRealTimeCurve_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmForceRealTimeCurve == null || this.frmForceRealTimeCurve.IsDisposed)
            {
                this.frmForceRealTimeCurve = new FrmForceRealTimeCurve();
            }

            this.frmForceRealTimeCurve.Show();
        }

        /// <summary>
        ///  LVDT实时曲线
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtLVDTRealTimeCurve_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmLVDTRealTimeCurve == null || this.frmLVDTRealTimeCurve.IsDisposed)
            {
                this.frmLVDTRealTimeCurve = new FrmLVDTRealTimeCurve();
            }

            this.frmLVDTRealTimeCurve.Show();
        }

        /// <summary>
        /// 焊后实时曲线
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtPostBondRes_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmPostBondResCurve == null || this.frmPostBondResCurve.IsDisposed)
            {
                this.frmPostBondResCurve = new FrmPostBondResCurve();
            }

            this.frmPostBondResCurve.Show();

            //this.ShowForm<FrmPostBondData>();
        }

        /// <summary>
        /// 顶针实时状态曲线
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtEjectionState_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmEjectionRealtimeState == null || this.frmEjectionRealtimeState.IsDisposed)
            {
                this.frmEjectionRealtimeState = new FrmEjectionRealtimeState();
            }

            this.frmEjectionRealtimeState.Show();
        }
    }
}
