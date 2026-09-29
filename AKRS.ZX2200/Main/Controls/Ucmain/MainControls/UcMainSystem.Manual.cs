#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/30 12:34:30
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/
#endregion << 版 本 注 释 >>

using AKRS.ZX2200.DispenseSystem.Controls.Manual;

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using System;
    using System.Windows.Forms;

    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.BondForce.Forms;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportSystem.Controls.Manual;
    using AKRS.ZX2200.WaferSubSystem.Controls.Manual;

    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 描述: 调试
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// 测力曲线
        /// </summary>
        private UcBondForceMeasurement frmBondForceMeasurement;

        /// <summary>
        /// 方向盘
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtManipulator_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //this.ShowPanel(
            //    "ucGuideMove",
            //    () =>
            //    {
            //        UcGuideMove ucGuideMove = new UcGuideMove() { Dock = DockStyle.Fill };
            //        return ucGuideMove;
            //    });

            this.GuideMoveOpen();
        }

        /// <summary>
        /// 吸嘴库
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtPPToolBank_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                if ((GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
                        .Find(it => it.Name == "ToolBankCoordinateSystem") == null)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"请先示教吸嘴架!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }
            }

  
            if (this.frmNozzleBank == null || this.frmNozzleBank.IsDisposed)
            {
                this.frmNozzleBank = new FrmNozzleBankManual();
            }
            frmNozzleBank.StartPosition = FormStartPosition.CenterScreen;
            this.frmNozzleBank.Show();
        }

        /// <summary>
        /// 顶针库
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtESToolBank_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcEjectBank",
                () =>
                {
                    UcEjectBank ucEjectBank = new UcEjectBank(){ Dock = DockStyle.Fill };
                    return ucEjectBank;
                });
        }

        /// <summary>
        /// Test Run
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtTestRun_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 防呆，退出单步
            MachineStateModel.GetInstance().IsSingleStepWork = false;

            if ((GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems
              .Find(it => it.Name == "ToolBankCoordinateSystem") == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                       $"Please set  up  PP tool  bank  first!",
                       "Warn",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);

                return;
            }

            FrmToolsTestRun frmToolsTestRun = new FrmToolsTestRun();
            frmToolsTestRun.StartPosition = FormStartPosition.CenterScreen; 
            frmToolsTestRun.ShowDialog();
        }

        /// <summary>
        /// Component Handing
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtComponentHanding_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcComponentHandling",
                () =>
                {
                    UcComponentHandling ucComponentHandling = new UcComponentHandling() { Dock = DockStyle.Fill };
                    return ucComponentHandling;
                });
        }

        /// <summary>
        /// Epoxy application
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDispenseManual_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcDispenseManual",
                () =>
                    {
                        UcDispenseManual ucDispenseManual = new UcDispenseManual();
                        return ucDispenseManual;
                    });
        }

        /// <summary>
        /// Measure tool
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtMeasureTool_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcMeasuringTool",
                () =>
                {
                    UcMeasuringTool ucMeasuringTool = new UcMeasuringTool() { Dock = DockStyle.Fill };
                    return ucMeasuringTool;
                });
        }

        /// <summary>
        /// Vacuum
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtVacuum_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcVacuum",
                () =>
                {
                    UcVacuum ucVacuum = new UcVacuum() { Dock = DockStyle.Fill };
                    return ucVacuum;
                });
        }

        /// <summary>
        /// Bondforce 曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtBondForceMeasurement_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcBondForceMeasurement",
                () =>
                {
                    UcBondForceMeasurement ucBondForceMeasurement = new UcBondForceMeasurement() { Dock = DockStyle.Fill };
                    return ucBondForceMeasurement;
                });
        }

        /// <summary>
        /// 方向键
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtManipulator_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.ShowPanel(
            //               "UcGuideMove",
            //               () =>
            //               {
            //                   UcGuideMove ucGuideMove = new UcGuideMove() { Dock = DockStyle.Fill };
            //                   return ucGuideMove;
            //               });

            this.GuideMoveOpen();
        }

        /// <summary>
        /// 上下料调试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtLoaderManual_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().LoadConfiguration != LoadConfigurationEnum.LoaderBin)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"Auto loader an unloader is not configured!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            this.ShowPanel(
                "UcLoaderAndUnloaderManual",
                () =>
                    {
                        UcLoaderAndUnloaderManual ucLoaderAndUnloaderManual = new UcLoaderAndUnloaderManual();
                        return ucLoaderAndUnloaderManual;
                    });
        }
    }
}
