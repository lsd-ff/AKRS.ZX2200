using AKRS.ZX2200.BondSystem.BondForce.Controllers;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    using AKRS.Galaxy2.Force.Services;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;
    using OfficeOpenXml;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    /// <summary>
    /// 标定关系
    /// </summary>
    public partial class FrmCalibrationRelationship : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// system2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 闭环力控制器
        /// </summary>
        private ClosedLoopCalibrationController closedLoopCalibrationController = new ClosedLoopCalibrationController();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        ///  焊头参数
        /// </summary>
        private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;

        /// <summary>
        /// 力控配置
        /// </summary>
        private ForceConfig forceConfig => ForceConfig.GetInstance();


        /// <summary>
        /// 测试界面
        /// </summary>
        private FrmBondheadTest fbt = new FrmBondheadTest();

        /// <summary>
        /// 初始化
        /// </summary>
        public FrmCalibrationRelationship()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 开始标定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnStart_Click_1(object sender, EventArgs e)
        {
            this.SavePara();

            this.BtnStart.Enabled = false;
            this.BtnCaliAndTest.Enabled = false;
            this.LbTip.Visible = true;

            try
            {
                await Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("力控标定线程");
                            this.closedLoopCalibrationController.DoWork();
                        });

                AKRSXtraMessageBox.Show(
                    $"标定完成!\r\n最小力：{this.bondHeadParam.ForceControlMinVal}g,最大力{this.bondHeadParam.ForceControlMaxVal}g\r\n耗时{ForceCalibrationData.GetInstance().ForceCaliCostTime}分钟!",
                    "Prompt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"标定异常!\r\n{ex.Message.ToString()}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnStart.Enabled = true;
                this.BtnCaliAndTest.Enabled = true;
                this.LbTip.Visible = false;
            }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public void InitControl()
        {
            this.CmbForceRange.SelectedIndex = (int)ForceConfig.GetInstance().ForceRange;
            this.CmbCaliMode.SelectedIndex = (int)ForceConfig.GetInstance().ForceCaliMode;

            this.SpSmallForceCaliInterval.Value = (decimal)forceConfig.SmallForceCalibrateInterval;
            this.SpLargeForceCaliInterval.Value = (decimal)forceConfig.LargeForceCalibrateInterval;

            this.SpSmallForceAngleInterval.Value = (decimal)forceConfig.SmallForceCaliAngleDistance;
            this.SpLargeForceAngleInterval.Value = (decimal)forceConfig.LargeForceCaliAngleDistance;

            this.SpSmallForceInitialIncrement.Value = (decimal)forceConfig.SmallForceCalibrateInitialIncrement;
            this.SpLargeForceInitialIncrement.Value = (decimal)forceConfig.LargeForceCalibrateInitialIncrement;

            this.SpForceKeepDelay.Value = ForceConfig.GetInstance().ForceKeepDelay;

            this.LbTip.Visible = false;

            //this.BtnCaliAndTest.Enabled = false;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void SavePara()
        {
            ForceConfig.GetInstance().ForceRange = (ForceRangeEnum)this.CmbForceRange.SelectedIndex;
            ForceConfig.GetInstance().ForceCaliMode = (ForceCaliModeEnum)this.CmbCaliMode.SelectedIndex;

            ForceConfig.GetInstance().SmallForceCalibrateInterval = (double)this.SpSmallForceCaliInterval.Value;
            ForceConfig.GetInstance().LargeForceCalibrateInterval = (double)this.SpLargeForceCaliInterval.Value;

            ForceConfig.GetInstance().SmallForceCaliAngleDistance = (double)this.SpSmallForceAngleInterval.Value;
            ForceConfig.GetInstance().LargeForceCaliAngleDistance = (double)this.SpLargeForceAngleInterval.Value;

            ForceConfig.GetInstance().SmallForceCalibrateInitialIncrement =
                (double)this.SpSmallForceInitialIncrement.Value;
            ForceConfig.GetInstance().LargeForceCalibrateInitialIncrement =
                (double)this.SpLargeForceInitialIncrement.Value;

            ForceConfig.GetInstance().ForceKeepDelay = (int)this.SpForceKeepDelay.Value;

            if (ForceConfig.GetInstance().ForceRange != ForceRangeEnum.LargeForce
                && forceConfig.SmallForceConfigItemList.Any() == false)
            {
                throw new Exception("未写入小力参数，不能标定小力！");
            }

            if (ForceConfig.GetInstance().ForceRange != ForceRangeEnum.SmallForce
                && forceConfig.LargeForceConfigItemList.Any() == false)
            {
                throw new Exception("未写入大力参数，不能标定大力！");
            }

            // 如果只用应变片，就先清空小力数据
            if (ForceConfig.GetInstance().ForceRange == ForceRangeEnum.LargeForce)
            {
                ForceCalibrationData.GetInstance().SmallForceRelationList.Clear();
            }

            ForceConfig.GetInstance().Save();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmCalibrationRelationship_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            FrmBondheadTest.IsAutoStartTest = false;
            this.fbt?.Dispose();
            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        ///  停止
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStop_Click(object sender, EventArgs e)
        {
            ClosedLoopCalibrationController.IsStop = true;
            //this.system2Controller.GetCalibrateTableForceValue();
        }

        /// <summary>
        ///  打印结果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            XtraSaveFileDialog fileDialog = new XtraSaveFileDialog();
            fileDialog.Title = "导出Excel";
            fileDialog.Filter = "Excel文件(*.xls)|*.xls";
            DialogResult dialogResult = fileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                try
                {
                    DevExpress.XtraPrinting.XlsExportOptions options = new DevExpress.XtraPrinting.XlsExportOptions();

                    // 打印
                    ForceCalibrationService.ExportCaliData(fileDialog.FileName);

                    AKRSXtraMessageBox.Show(
                        $"打印成功!",
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("正由另一进程使用"))
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！文件正由另一个程序占用！", "提示");
                    }
                    else
                    {
                        AKRSXtraMessageBox.Show("数据导出失败！数据量过大，请分别统计再导出！", "提示");
                    }
                }
            }
        }

        /// <summary>
        /// 力控标定然后测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCaliAndTest_Click(object sender, EventArgs e)
        {
            //this.SavePara();

            //this.BtnStart.Enabled = false;
            //this.BtnCaliAndTest.Enabled = false;
            //this.LbTip.Visible = true;

            //Task.Run(
            //    () =>
            //        {
            //            try
            //            {
            //                this.closedLoopCalibrationController.DoWork();

            //                // 测试
            //                fbt.Show();
            //                fbt.BtnStart_Click(sender, e);
            //            }
            //            finally
            //            {
            //                this.BeginInvoke(
            //                    () =>
            //                        {
            //                            this.BtnStart.Enabled = true;
            //                            this.BtnCaliAndTest.Enabled = true;
            //                            this.LbTip.Visible = false;
            //                        });
            //            }
            //        });

            this.SavePara();

            this.BtnStart.Enabled = false;
            this.BtnCaliAndTest.Enabled = false;
            this.LbTip.Visible = true;

            try
            {
                Task task = Task.Run(
                   () =>
                       {
                           CommonUtil.SetCurrentThreadName("力控标定并检验线程");
                           this.closedLoopCalibrationController.DoWork();

                           this.BeginInvoke(
                               () =>
                                   {
                                       if (fbt.IsDisposed || fbt == null)
                                       {
                                           fbt = new FrmBondheadTest();
                                       }

                                       FrmBondheadTest.IsAutoStartTest = true;
                                       //fbt.BringToFront();

                                       // 测试
                                       fbt.ShowDialog();
                                       fbt.Dispose();
                                   });
                       });

                task.Wait();
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"标定异常!\r\n{ex.Message.ToString()}",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnStart.Enabled = true;
                this.BtnCaliAndTest.Enabled = true;
                this.LbTip.Visible = false;
                FrmBondheadTest.IsAutoStartTest = false;
            }
        }
    }
}