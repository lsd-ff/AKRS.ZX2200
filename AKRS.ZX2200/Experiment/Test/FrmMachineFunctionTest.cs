using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.Experiment.Test
{
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using Accord.IO;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionModule.ETEL;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Calibrate;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using log4net.Core;

    /// <summary>
    ///  整机功能测试界面
    /// </summary>
    public partial class FrmMachineFunctionTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmMachineFunctionTest()
        {
            InitializeComponent();
            this.InitControl();
        }

        #region 线程

        /// <summary>
        /// 换吸嘴重复性测试线程
        /// </summary>
        private Task changePPtoolTestTask;

        /// <summary>
        /// 换顶针重复性测试线程
        /// </summary>
        private Task changeEStoolTestTask;

        /// <summary>
        ///  换晶圆测试线程
        /// </summary>
        private Task changeWaferTestTask;

        /// <summary>
        ///  吸嘴校准线程
        /// </summary>
        private Task nozzleCaliTask;

        /// <summary>
        ///  顶针校准线程
        /// </summary>
        private Task ejectionCaliTask;

        /// <summary>
        ///  取片补偿校准线程
        /// </summary>
        private Task pickOffsetCaliTask;

        /// <summary>
        ///  力控测试线程
        /// </summary>
        private Task forceControlTestTask;

        /// <summary>
        ///  测高测试线程
        /// </summary>
        private Task measureHeightTestTask;

        #endregion

        #region 信号

        /// <summary>
        ///  取消吸嘴校准
        /// </summary>
        private CancellationTokenSource nozzleCaliCts = null;

        /// <summary>
        ///  取消顶针校准
        /// </summary>
        private CancellationTokenSource ejectionCaliCts = null;

        /// <summary>
        ///  取消取片偏移校准
        /// </summary>
        private CancellationTokenSource pickOffsetCaliCts = null;

        /// <summary>
        /// 力控测试取消信号
        /// </summary>
        private CancellationTokenSource forceControlTestCts = null;

        /// <summary>
        /// 测高测试取消信号
        /// </summary>
        private CancellationTokenSource measureHeightTestCts = null;

        /// <summary>
        /// 换吸嘴测试停止信号
        /// </summary>
        private bool isStopPPToolTest = true;

        /// <summary>
        /// 换顶针测试停止信号
        /// </summary>
        private bool stopESToolTest = true;

        #endregion

        /// <summary>
        /// 是否在工作中
        /// </summary>
        private bool isWorking = false;

        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain system2Domain => System2Domain.GetInstance();

        /// <summary>
        /// BondModuleController
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController toolBankController = new NozzleShelfController();

        /// <summary>
        /// EjectionBankSetting
        /// </summary>
        private EjectionBankConfig CurrentBank =>
            EjectionBankConfigRepository.GetInstance().BaseDsSettingList.Find(
                item => item.Name == WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.Name);

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// BondHeadController
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 通讯服务
        /// </summary>
        private ModbusService modbusService => ModbusService.GetInstance();

        /// <summary>
        ///  焊头参数
        /// </summary>
        private BondHeadParam bondHeadParam => BondDevicePara.GetInstance().BondHeadParam;

        /// <summary>
        ///  界面初始化
        /// </summary>
        private void InitControl()
        {
            this.BtnESToolsTestRun.Enabled = MachineHardwareConfiguration.GetInstance().IsEjectSystemConfigured;
            this.BtnPPToolsTestRun.Enabled = System2Configuration.GetInstance().IsToolBankEnable;
            this.BtnWaferChangeTest.Enabled = WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift;
            this.LbResult.Text = string.Empty;
            this.LbResult.Font = new Font("微软雅黑", 15, FontStyle.Bold);
        }

        /// <summary>
        /// 刷新界面信息
        /// </summary>
        /// <param name="message">信息</param>
        private void RefreshInfo(string message)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(
                    () =>
                        {
                            this.LbResult.Text = message;
                        }));
            }
            else
            {
                this.LbResult.Text = message;
            }
        }

        /// <summary>
        /// 自动换晶圆
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnWaferChangeTest_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                // 启动
                if (this.changeWaferTestTask == null || this.changeWaferTestTask.IsCompleted == true)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.BtnESToolsTestRun.Enabled = false;
                    btn.Appearance.BackColor = Color.Yellow;
                    Static.IsWaferChangeTestSignal = true;
                    this.isWorking = true;
                    this.changeWaferTestTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("换晶圆重复性测试线程");

                                Stopwatch sp = Stopwatch.StartNew();

                                while (true)
                                {
                                    Retry:
                                    if (sp.Elapsed.TotalHours > 1 || Static.IsWaferChangeTestSignal == false)
                                    {
                                        return;
                                    }

                                    for (int i = 0;
                                         i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram
                                             .CurrentAllocationsConfig.TabletArray.Length;
                                         i++)
                                    {
                                        this.RefreshInfo($"自动换晶圆测试进行中，剩余：{60 - (int)sp.Elapsed.TotalMinutes}分钟");

                                        #region 换晶圆

                                        WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                                        if (Static.IsWaferChangeTestSignal)
                                        {
                                            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram
                                                    .CurrentAllocationsConfig.TabletArray[i].TabletType
                                                != TabletTypeEnum.Null)
                                            {
                                                WaferSubController.GetInstance().WaferTableController
                                                    .RemoveWaferFromSlot(i, true);
                                            }
                                        }
                                        else
                                        {
                                            return;
                                        }

                                        #endregion

                                        Thread.Sleep(100);
                                    }

                                    goto Retry;
                                }
                            });

                    await changeWaferTestTask;

                    // 料夹去安全位
                    WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();

                    // 料架去安全位
                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();

                    this.isWorking = false;

                    this.RefreshInfo(string.Empty);

                    AKRSXtraMessageBox.Show("重复换晶圆测试完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Static.IsWaferChangeTestSignal = false;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn.Appearance.BackColor = default;
                Static.IsWaferChangeTestSignal = false;
                this.isWorking = false;
                this.RefreshInfo(string.Empty);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                this.BtnESToolsTestRun.Enabled = true;
                this.BtnWaferChangeTest.Enabled = true;
            }
        }

        /// <summary>
        /// 自动换吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnPPToolsTestRun_Click(object sender, EventArgs e)
        {       
            // 启动
            if (this.changePPtoolTestTask == null || this.changePPtoolTestTask.IsCompleted == true)
            {
                if (isWorking)
                {
                    AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                this.BtnPPToolsTestRun.Appearance.BackColor = Color.Yellow;
                this.BtnPPToolsTestRun.Text = @"停止重复换吸嘴测试";
                this.isStopPPToolTest = false;
                this.isWorking = true;

                this.changePPtoolTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("换吸嘴重复性测试线程");
                            this.PPToolsTestRun();
                        });

                await this.changePPtoolTestTask;

                this.isWorking = false;
                this.RefreshInfo(string.Empty);

                AKRSXtraMessageBox.Show("重复换吸嘴测试完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.isStopPPToolTest = true;

                // 停止
                this.BtnPPToolsTestRun.Appearance.BackColor = Color.Transparent;
                this.BtnPPToolsTestRun.Text = @"开始重复换吸嘴测试";
            }
        }

        /// <summary>
        /// PP tool测试方法
        /// </summary>
        private void PPToolsTestRun()
        {
            bool isChangeNozzleSuccess = false;

            Stopwatch sp = Stopwatch.StartNew();

            Stopwatch spChangeNozzleTime = Stopwatch.StartNew();

            List<double> timeList = new List<double>();

            try
            {
                while (true)
                {
                    // 循环吸嘴槽
                    foreach (NozzleShelfSlot slot in this.system2Domain.BondProgram.NozzleShelfProgram.NozzleShelf
                                 .NozzleShelfSlots)
                    {
                        #region 换吸嘴

                        string nozzleName = slot.NozzleName;

                        if (string.IsNullOrEmpty(nozzleName))
                        {
                            continue;
                        }

                        spChangeNozzleTime.Restart();
                        isChangeNozzleSuccess = this.system2Controller.ChangeNozzle(nozzleName);

                        // 取吸嘴
                        if (isChangeNozzleSuccess == false)
                        {
                            throw new Exception();
                        }

                        Task t1 = Task.Run(this.toolBankController.MoveShelfToHome);
                        Task t2 = Task.Run(this.bondModuleController.MoveToChangeNozzleSafePos);
                        t1.Wait(10000);
                        t2.Wait(10000);

                        // 等轴到位
                        if (t1.IsCompleted == false)
                        {
                            throw new Exception("吸嘴架回零失败！");
                        }

                        spChangeNozzleTime.Stop();
                        timeList.Add(spChangeNozzleTime.Elapsed.TotalSeconds);

                        #endregion

                        Thread.Sleep(100);

                        if (this.isStopPPToolTest || sp.Elapsed.TotalHours > 1)
                        {
                            return;
                        }

                        this.RefreshInfo($"自动换吸嘴测试进行中，剩余：{60 - (int)sp.Elapsed.TotalMinutes}分钟");
                    }
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"换吸嘴重复测试失败!\r\n" + ex.ToString(),
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                throw;
            }
            finally
            {
                // 换吸嘴失败不动吸嘴架避免二次撞击
                if (isChangeNozzleSuccess)
                {
                    // 吸嘴架回原
                    this.toolBankController.MoveShelfToHome();
                }

                timeList.Select(it => new { 单次耗时s = it })
                    .ExportToXlsx($"D:\\设备功能测试\\重复换吸嘴耗时数据{DateTime.Now:yyyy-MM-dd}.xlsx");

                this.Invoke(
                    new Action(
                        () =>
                            {
                                this.BtnPPToolsTestRun.Appearance.BackColor = Color.Transparent;
                                this.BtnPPToolsTestRun.Text = @"开始重复换吸嘴测试";
                            }));
            }
        }

        /// <summary>
        /// 自动换顶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnESToolsTestRun_Click(object sender, EventArgs e)
        {
            // 防呆
            if (this.changeWaferTestTask != null)
            {
                if (this.changeWaferTestTask.IsCompleted == false)
                {
                    AKRSXtraMessageBox.Show("请先停止自动换晶圆！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            if (this.changeEStoolTestTask == null || this.changeEStoolTestTask.IsCompleted == true)
            {
                if (isWorking)
                {
                    AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                } 

                this.BtnWaferChangeTest.Enabled = false;
                this.BtnESToolsTestRun.Appearance.BackColor = Color.Yellow;
                this.BtnESToolsTestRun.Text = @"停止重复换顶针测试";
                this.stopESToolTest = false;
                this.isWorking = true;
                this.changeEStoolTestTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("换顶针重复性测试线程");
                            this.ESToolsTestRun();
                        });

                await this.changeEStoolTestTask;
                this.isWorking = false;

                AKRSXtraMessageBox.Show("重复换顶针测试完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                this.BtnESToolsTestRun.Appearance.BackColor = Color.Transparent;
                this.BtnESToolsTestRun.Text = @"开始重复换顶针测试";
                this.stopESToolTest = true;
            }
        }

        /// <summary>
        /// ES tool测试方法
        /// </summary>
        private void ESToolsTestRun()
        {
            Stopwatch sp = Stopwatch.StartNew();

            Stopwatch spChangeToolTime = Stopwatch.StartNew();

            List<double> timeList = new List<double>();

            try
            {
                while (true)
                {
                    for (int i = 0; i < this.CurrentBank.EjectionBankSlots.Length; i++)
                    {
                        this.RefreshInfo($"自动换顶针测试进行中，剩余：{60 - (int)sp.Elapsed.TotalMinutes}分钟");

                        // 超过一个小时自动结束
                        if (sp.Elapsed.TotalHours > 1|| stopESToolTest)
                        {
                            return;
                        }

                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        if (!this.stopESToolTest)
                        {
                            if (WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity
                                    .EjectionBankSlotEntities[i].SlotState != EjectSlotStatuEnum.Empty)
                            {
                                spChangeToolTime.Restart();
                                WaferSubController.GetInstance().EjectController.ChangeEjection(i, true);

                                WaferSubController.GetInstance().EjectController.ReturnEjection();
                                spChangeToolTime.Stop();
                                timeList.Add(spChangeToolTime.Elapsed.TotalSeconds);
                            }
                        }
                        else
                        {
                            return;
                        }

                        Thread.Sleep(100);
                    }
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"重复换顶针失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                throw;
            }
            finally
            {
                this.stopESToolTest = true;
                this.isWorking = false;

                timeList.Select(it => new { 单次耗时s = it })
                    .ExportToXlsx($"D:\\设备功能测试\\重复换顶针耗时数据{DateTime.Now:yyyy-MM-dd}.xlsx");
                this.BeginInvoke(new Action(() =>
                    {
                        this.BtnWaferChangeTest.Enabled = true;
                        this.RefreshInfo(string.Empty);
                        this.BtnESToolsTestRun.Appearance.BackColor = Color.Transparent;
                        this.BtnESToolsTestRun.Text = @"开始重复换顶针测试";
                    }));
            }
        }

        /// <summary>
        /// 窗体关闭事件，停止测试线程
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmMachineFunctionTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 停止测试线程
            //this.isStopPPToolTest = true;
            //this.stopESToolTest = true;
            //Static.IsWaferChangeTestSignal = false;
            //System2RunTimeProvider.IsWaferCheckSucceed = false;

            //this.forceControlTestCts?.Cancel();
            //this.measureHeightTestCts?.Cancel();
            //this.nozzleCaliCts?.Cancel();
            //this.ejectionCaliCts?.Cancel();
            //this.pickOffsetCaliCts?.Cancel();

            Machine.GetInstance().Stop();

            // 等所有线程执行结束
            //this.changePPtoolTestTask?.Wait();
            //this.changeWaferTestTask?.Wait();
            //this.changeEStoolTestTask?.Wait();

            //this.measureHeightTestTask?.Wait();
            //this.forceControlTestTask?.Wait();

            //this.nozzleCaliTask?.Wait();
            //this.changeEStoolTestTask?.Wait();
            //this.changeWaferTestTask?.Wait();

            if (this.changePPtoolTestTask != null && this.changePPtoolTestTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等换吸嘴重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.changeEStoolTestTask != null && this.changeEStoolTestTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等换顶针重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.changeWaferTestTask != null && this.changeWaferTestTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等换晶圆重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.nozzleCaliTask != null && this.nozzleCaliTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等吸嘴校准重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.ejectionCaliTask != null && this.ejectionCaliTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等顶针校准重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.pickOffsetCaliTask != null && this.pickOffsetCaliTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等取片偏移校准重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.measureHeightTestTask != null && this.measureHeightTestTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等测高准重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }

            if (this.forceControlTestTask != null && this.forceControlTestTask.IsCompleted == false)
            {
                AKRSXtraMessageBox.Show($"请先等力控准重复性测试结束！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 窗体加载事件，初始化检查状态
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmMachineFunctionTest_Load(object sender, EventArgs e)
        {
            System2RunTimeProvider.IsWaferCheckSucceed = false;
        }

        /// <summary>
        /// 校准顶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnCaliEjection_Click(object sender, EventArgs e)
        {
            int testTime = (int)this.SpEjectionCaliTime.Value;

            try
            {
                if (this.ejectionCaliTask == null || this.ejectionCaliTask.IsCompleted)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.isWorking = true;
                    List<(string ejection, AKRSPoint3D offset)> data = new();

                    this.ejectionCaliCts = new CancellationTokenSource();
                    this.BtnCaliEjection.Appearance.BackColor = Color.Yellow;

                    this.ejectionCaliTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("顶针重复性校准线程");

                                this.system2Controller.PrepareBeforeCaliAllEjection();

                                for (int j = 0; j < testTime; j++)
                                {
                                    this.RefreshInfo($"顶针校准重复性测试进行中，第{j+1}次");

                                    if (this.ejectionCaliCts.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    for (int i = 0; i < this.CurrentBank.EjectionBankSlots.Length; i++)
                                    {
                                        if (WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity
                                                .EjectionBankSlotEntities[i].SlotState != EjectSlotStatuEnum.Empty)
                                        {
                                            EjectionBankSlotConfig currentBankSlotConfig =
                                                this.CurrentBank.EjectionBankSlots[i];

                                            // 换顶针
                                            WaferSubController.GetInstance().EjectController.ChangeEjection(i, true);

                                            // 顶针到校准高度
                                            WaferSubController.GetInstance().EjectController.MoveEjectionAxisZToG0Pos(
                                                currentBankSlotConfig.EjectionConfig.EjectionCaliLevel);

                                            MatchResult res;

                                            if (WaferSubDevicePara.GetInstance().EjectDevicePara
                                                .IsUseWaferCameraAssistantEjectCenter)
                                            {
                                                // 相机到拍照位
                                                WaferSubController.GetInstance().WaferTableController.MoveWaferCameraAxisZToG0Pos(currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos);

                                                (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(currentBankSlotConfig.EjectionConfig.EjectMatchName, false, false);

                                                if (result.isSucceed == false) 
                                                {
                                                    AKRSXtraMessageBox.Show(
                                                    $"校准顶针:{this.CurrentBank.EjectionBankSlots[i].EjectionConfig.Name} 失败! \r\n ",
                                                    "报警",
                                                    MessageBoxButtons.OK,
                                                    MessageBoxIcon.Warning);

                                                    // 失败直接退出
                                                    return;
                                                }                              

                                                currentBankSlotConfig.EjectionConfig
        .DeviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(result.matchResults[0]);
                                            }
                                            else
                                            {
                                                // 相机到拍照位
                                                // todo:不用每次都抬
                                                 res = (MatchResult)System2Domain.GetInstance()
                                                    .System2CommonVision(
                                                        currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos,
                                                        "EjectionCali",
                                                        currentBankSlotConfig.EjectionConfig.EjectMatchName,
                                                        false,
                                                        CameraTypeEnum.BondCamera);

                                                if (res == null || !res.IsSuccess)
                                                {
                                                    AKRSXtraMessageBox.Show(
                                                        $"校准顶针:{this.CurrentBank.EjectionBankSlots[i].EjectionConfig.Name} 失败! \r\n ",
                                                        "报警",
                                                        MessageBoxButtons.OK,
                                                        MessageBoxIcon.Warning);

                                                    // 失败直接退出
                                                    return;
                                                }

                                                AKRSPoint2D point1 = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(0, 0), res, "BondCameraCoordinateSystem");
                                                currentBankSlotConfig.EjectionConfig
        .DeviationWithEjectionCenterAndWaferCameraCenter = new AKRSPoint3D(point1.X, point1.Y, 0);

        //                                        currentBankSlotConfig.EjectionConfig
        //.DeviationWithEjectionCenterAndWaferCameraCenter = this.bondModuleController.MatchResultToWorld(res);
                                            }                                 
                                            

                                            // 数据收集
                                            data.Add(
                                                new()
                                                    {
                                                        ejection =
                                                           currentBankSlotConfig.EjectionConfig.Name,
                                                        offset = currentBankSlotConfig.EjectionConfig
                                                            .DeviationWithEjectionCenterAndWaferCameraCenter.DeepClone()
                                                    });
                                        }
                                    }
                                }
                            },
                        this.ejectionCaliCts.Token);

                    await this.ejectionCaliTask;

                    this.bondModuleController.MoveToSafePos();
                    WaferSubController.GetInstance().EjectController.ReturnEjection();

                    this.isWorking = false;

                    // 数据处理、界面显示
                    string info = this.HandleEjectionData(data);

                    FrmMachineFunctionTestResult frmResult = new FrmMachineFunctionTestResult(info);
                    frmResult.StartPosition = FormStartPosition.CenterScreen;
                    frmResult.ShowDialog();
                }
                else
                {
                    this.ejectionCaliCts.Cancel();
                }
            }
            catch (Exception ex)
            {
                this.isWorking = false;

                this.bondModuleController.MoveToSafePos();
                WaferSubController.GetInstance().EjectController.ReturnEjection();

                AKRSXtraMessageBox.Show(
                    $"校准顶针失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                this.BtnCaliEjection.Appearance.BackColor = default;
                this.RefreshInfo(string.Empty);        
            }
        }

        /// <summary>
        /// 校准吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private async void BtnCaliNozzle_Click(object sender, EventArgs e)
        {
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

            if (nozzle == null)
            {
                FrmNozzleSelect frmNozzleSelect = new FrmNozzleSelect();
                frmNozzleSelect.StartPosition = FormStartPosition.CenterScreen;
                if (frmNozzleSelect.ShowDialog() == DialogResult.OK)
                {
                    nozzle = (Nozzle)NozzleRepository.GetInstance().Find(frmNozzleSelect.nozzle);
                }
                else
                {
                    return;
                }
            }

            if (nozzle.IsSystemConfiguration)
            {
                AKRSXtraMessageBox.Show($"焊头上检测为治具，请更换成吸嘴再进行测试！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nozzle.IsAssistantSucceed == false)
            {
                AKRSXtraMessageBox.Show($"吸嘴：{nozzle.Name} 自动校准前请先完成示教！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return;
            }

            int testTime = (int)this.SpNozzleCaliTime.Value;

            try
            {
                if (this.nozzleCaliTask == null || this.nozzleCaliTask.IsCompleted)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.isWorking = true;
                    List<AKRSPoint2D> data = new List<AKRSPoint2D>();

                    this.nozzleCaliCts = new CancellationTokenSource();
                    this.BtnCaliNozzle.Appearance.BackColor = Color.Yellow;

                    this.nozzleCaliTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("吸嘴校准线程");

                                for (int i = 0; i < testTime; i++)
                                {
                                    this.RefreshInfo($"吸嘴校准重复性测试进行中，第{i+1}次");

                                    if (nozzleCaliCts.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    // 换吸嘴
                                    bool res = this.system2Controller.ChangeNozzle(nozzle.Name);

                                    if (res == false)
                                    {
                                        // 失败直接退出
                                        return;
                                    }

                                    //nozzle.NozzleOffset.X = i;
                                    //nozzle.NozzleOffset.Y = i;

                                    ExcuteResult ret = this.system2Controller.NozzleCaliAssistance(nozzle);

                                    if (ret != ExcuteResult.Success)
                                    {
                                        AKRSXtraMessageBox.Show(
                                            $"校准吸嘴失败! \r\n Message: {ret.ToString()}",
                                            "报警",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        // 失败直接退出
                                        return;
                                    }

                                    // 数据收集
                                    data.Add(nozzle.NozzleOffset.DeepClone());

                                    Thread.Sleep(100);
                                }
                            },
                        this.nozzleCaliCts.Token);

                    await this.nozzleCaliTask;

                    this.isWorking = false;

                    // 数据处理、界面显示
                    string info = this.HandleNozzleData(nozzle.Name,data);

                    FrmMachineFunctionTestResult frmResult = new FrmMachineFunctionTestResult(info);
                    frmResult.StartPosition = FormStartPosition.CenterScreen;
                    frmResult.ShowDialog();

                    this.RefreshInfo(string.Empty);
                    this.BtnCaliNozzle.Appearance.BackColor = default;
                }
                else
                {
                    this.nozzleCaliCts.Cancel();               
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"校准吸嘴失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.isWorking = false;

                this.RefreshInfo(string.Empty);
                this.BtnCaliNozzle.Appearance.BackColor = default;
            }
        }

        /// <summary>
        /// 吸嘴数据处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        private string HandleNozzleData(string name,List<AKRSPoint2D> data)
        {
            if (data.Any() == false)
            {
                return string.Empty;
            }

            // 均值
            AKRSPoint2D mean = MathService.GetMean(data);

            // 极差
            AKRSPoint2D range = MathService.GetRange(data);

            string info =
                $"吸嘴{this.bondHeadController.GetCurrentNozzle().Name}校准结果：\r\n均值：X={mean.X:F4}mm, Y={mean.Y:F4}mm\r\n极差：X={range.X:F4}mm, Y={range.Y:F4}mm";

            // 日志记录
            LogHelper.Post(Level.Info, info, LogCategory.Global, ViewType.InFileAndUI);

            data.Select(it=>new{X=it.X,Y=it.Y}).ExportToXlsx($"D:\\设备功能测试\\吸嘴{name}偏移校准数据{DateTime.Now:yyyy-MM-dd}.xlsx");

            return info;
        }

        ///// <summary>
        ///// 吸嘴数据处理
        ///// </summary>
        ///// <param name="data">数据</param>
        ///// <returns>结果</returns>
        //private string HandleNozzleData(List<(string ejection, AKRSPoint3D offset)> data)
        //{
        //    string info = default;

        //    var ejectionList = data.Select(it => it.ejection).Distinct();

        //    foreach (var ej in ejectionList)
        //    {
        //        AKRSPoint3D mean = data.Where(it => it.ejection == ej).Select(it => it.offset).ToList().GetMean();

        //        AKRSPoint3D range = data.Where(it => it.ejection == ej).Select(it => it.offset).ToList().GetRange();

        //        info +=
        //            $"顶针：{ej}校准结果：\r\n均值：X={mean.X:F4}mm, Y={mean.Y:F4}mm\r\n极差：X={range.X:F4}mm, Y={range.Y:F4}mm\r\n";
        //    }

        //    // 日志记录
        //    LogHelper.Post(
        //        Level.Info,
        //        info,
        //        LogCategory.Global,
        //        ViewType.InFileAndUI);

        //    return info;
        //}

        /// <summary>
        /// 顶针数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string HandleEjectionData(List<(string ejection, AKRSPoint3D offset)> data)
        {
            if (data.Any() == false)
            {
                return string.Empty;
            }

            var result = data.GroupBy(d => d.ejection).Select(
                g =>
                    {
                        var offsets = g.Select(d => d.offset).ToList();
                        var mean = offsets.GetMean();
                        var range = offsets.GetRange();

                        return
                            $"顶针：{g.Key}校准结果：均值：X={mean.X:F4}mm, Y={mean.Y:F4}mm。极差：X={range.X:F4}mm, Y={range.Y:F4}mm\r\n";
                    });

            string info = string.Concat(result);

            LogHelper.Post(Level.Info, info, LogCategory.Global, ViewType.InFileAndUI);

            var groups = data.GroupBy(item => item.ejection);

            foreach (var group in groups)
            {
                string ejection = group.Key;
                group.Select(tuple => new { 顶针 = tuple.ejection, X = tuple.offset.X, Y = tuple.offset.Y })
                    .ExportToXlsx($"D:\\设备功能测试\\顶针{ejection}偏移校准数据{DateTime.Now:yyyy-MM-dd}.xlsx");
            }

            return info;
        }

        /// <summary>
        /// 吸嘴数据处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        private string HandlePickOffsetData(List<AKRSPoint2D> data)
        {
            if (data.Any() == false)
            {
                return string.Empty;
            }

            // 均值
            AKRSPoint2D mean = MathService.GetMean(data);

            // 极差
            AKRSPoint2D range = MathService.GetRange(data);

            string info =
                $"芯片取片偏移校准结果：\r\n均值：X={mean.X:F4}mm, Y={mean.Y:F4}mm\r\n极差：X={range.X:F4}mm, Y={range.Y:F4}mm";

            // 日志记录
            LogHelper.Post(Level.Info, info, LogCategory.Global, ViewType.InFileAndUI);

            data.Select(it => new { X = it.X, Y = it.Y }).ExportToXlsx($"D:\\设备功能测试\\取片偏移校准数据{DateTime.Now:yyyy-MM-dd}.xlsx");

            return info;
        }

        /// <summary>
        ///  力控测试数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string HandleForceData(List<(double targetForce, double actualForce, double time)> data)
        {
            if (data.Any() == false)
            {
                return string.Empty;
            }

            string info= "力控测试结果：\r\n";
            List<double> error1 = new List<double>();
            List<double> error2 = new List<double>();

            List<double> time1 = new List<double>();
            List<double> time2 = new List<double>();

            double maxForce = data.Max(item => item.targetForce);
            double minForce = data.Min(item => item.targetForce);

            foreach (var item in data)
            {
                if (item.targetForce <= 100)
                {
                    error1.Add(Math.Abs(item.targetForce - item.actualForce));
                    time1.Add(item.time);
                }

                if (item.targetForce > 100)
                {
                    error2.Add(Math.Abs(item.targetForce - item.actualForce) / item.targetForce * 100.0);
                    time2.Add(item.time);
                }
            }

            if (error1.Any())
            {
                info +=
                       $"目标力值{minForce:F2}~100g的误差：{error1.Min():F2}~{error1.Max():F2}g,平均误差：{error1.Average():F2}g。力控耗时：{time1.Min():F0}~{time1.Max():F0}ms,平均力控耗时:{time1.Average():F0}ms\r\n";
            }

            if (error2.Any())
            {
                info +=
                            $"目标力值100~{maxForce:F2}g的误差：\r\n{error2.Min():F2}~{error2.Max():F2}%,,平均误差：{error2.Average():F2}g。力控耗时：{time2.Min():F0}~{time2.Max():F0}ms,平均力控耗时:{time2.Average():F0}ms";
            }
  
            // 日志记录
            LogHelper.Post(Level.Info, info, LogCategory.Global, ViewType.InFileAndUI);
            return info;
        }

        /// <summary>
        /// 吸嘴数据处理
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>结果</returns>
        private string HandleMeasureHeightData(List<double> data)
        {
            if (data.Any() == false)
            {
                return string.Empty;
            }

            double max = data.Max();

            double min = data.Min();

            double range = MathService.GetRange(data);

            string info = $"测高重复性测试结果：\r\n次数：{data.Count}\r\n最大值：{max:F4} mm\r\n最小值：{min:F4} mm\r\n极差：{range:F4} mm";

            // 日志记录
            LogHelper.Post(Level.Info, info, LogCategory.Global, ViewType.InFileAndUI);


            data.ExportToXlsx($"D:\\设备功能测试\\测高重复性测试数据{DateTime.Now:yyyy-MM-dd}.xlsx");

            return info;
        }

        /// <summary>
        /// 焊头力控测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnForceControlTest_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            double testTime = (double)this.SpForceControlTestTime.Value;

            try
            {
                if (this.forceControlTestTask == null || this.forceControlTestTask.IsCompleted)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.forceControlTestCts = new CancellationTokenSource();
                    this.BtnForceControlTest.Appearance.BackColor = Color.Yellow;

                    string info = string.Empty;

                    this.isWorking = true;

                    this.forceControlTestTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("焊头力控测试线程");

                                info = this.ForceControlTest(testTime);
                            },
                        this.forceControlTestCts.Token);

                    await this.forceControlTestTask;

                    FrmMachineFunctionTestResult frmResult = new FrmMachineFunctionTestResult(info);
                    frmResult.StartPosition = FormStartPosition.CenterScreen;
                    frmResult.ShowDialog();

                    this.BtnForceControlTest.Appearance.BackColor = default;
                    this.RefreshInfo(string.Empty);
                }
                else
                {
                    this.forceControlTestCts.Cancel();
                }
            }
            catch (Exception ex)
            {
                this.isWorking = false;
                AKRSXtraMessageBox.Show(
                    $"力控测试异常! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.BtnForceControlTest.Appearance.BackColor = default;
                this.RefreshInfo(string.Empty);
            }
        }

        /// <summary>
        /// 力控测试
        /// </summary>
        /// <param name="testTime">测试时间</param>
        /// <returns>测试结果</returns>
        public string ForceControlTest(double testTime)
        {
            // 打印资源
            List<(DateTime dateTime, double targetForce, double angle, double beforeTouchForce, double posZAfter, double
                touchBondheadForce, double touchForce, double riseForce, double time, double forceReference, double
                touchBondheadForceBefore, double touchForceBefore, double posZBefore)> printList = new();

            List<(double targetForce, double actualForce, double time)> data = new();

            int delay = ForceConfig.GetInstance().ForceKeepDelay;
            double slowTravelDistance = 0.1;
            double minForce = this.bondHeadParam.ForceControlMinVal + 0.1;
            double maxForce = this.bondHeadParam.ForceControlMaxVal - 0.1;
            double forceSpacing = 50;

            try
            {
                // 连接摩尔力设备
                if (!this.modbusService.ConnectManometer())
                {
                    return null;
                }

                this.bondHeadController.MoveBondZToSafePos();

                AKRSPoint3D forceCalibratePos =
                    this.bondModuleController.ConvertG0ToMachinePos(
                        BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos);

                double bondLevel = forceCalibratePos.Z + 0.1;

                double preBondLevel = bondLevel + slowTravelDistance;

                double speed = this.bondHeadController.GetAxisZAbsoluteSpeed()
                               * MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage;

                Stopwatch forceControlStopwatch = Stopwatch.StartNew();

                Stopwatch sp = Stopwatch.StartNew();

                while (true)
                {
                    // 获取力值数据
                    for (double targetForce = minForce;
                         targetForce <= maxForce;
                         targetForce = targetForce + forceSpacing)
                    {
                        for (double angle = -180; angle <= 180; angle = angle + 90)
                        {
                            this.RefreshInfo($"力控测试进行中，还剩：{(testTime - sp.Elapsed.TotalMinutes/60.0):F2}小时");

                            if (this.forceControlTestCts.IsCancellationRequested || sp.Elapsed.TotalHours > testTime)
                            {
                                FileHelper.SaveForceControlData(printList);

                                printList.Clear();

                                return this.HandleForceData(data);
                            }

                            bool isSmallForce = ForceCalibrationService.JudgeIsSmallForce(targetForce);

                            // 移动到标定位
                            this.bondModuleController.MoveToG0Pos(
                                BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.X,
                                BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos.Y);

                            this.bondHeadController.RotateAxisT(angle);

                            //// T轴到位后压力表可能还没稳定
                            //Thread.Sleep(100);

                            if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                if (this.system2Controller.GetBondheadForceValue() < -1)
                                {
                                    this.modbusService.ResetBondhead();
                                }
                            }
                            else
                            {
                                double read = this.system2Controller.GetBondheadForceValue();

                                if (isSmallForce == false && read < -1)
                                {
                                    // 焊头力控清零
                                    this.bondHeadController.ResetBondhead();
                                }
                            }

                            double calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                            // 置零
                            if (Math.Abs(calibrateTableForceInitial) > 3)
                            {
                                this.modbusService.ResetManometer();
                            }

                            calibrateTableForceInitial = this.system2Controller.GetCalibrateTableForceValue();

                            double beforeTouchForce = this.system2Controller.GetBondheadForceValue();

                            double inputForce = targetForce;

                            // 如果是固高轴先运动到预固晶位
                            this.bondHeadController.MoveAxisZ(preBondLevel);

                            if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                // etel的话在预固精位置开启力控
                                bondLevel = forceCalibratePos.Z + 0.1 + slowTravelDistance - 0.01;
                            }

                            // 开始计时
                            forceControlStopwatch.Restart();

                            #region 力控下压,这里为了打印中间数据复制了整个方法

                            this.bondHeadController.CheckZAxisSoftLimit(bondLevel);

                            // 力值转换,Etel的单位是N，固高的单位是g
                            double forceReference;
                            double curAngle = this.bondHeadController.GetAxisTRealPos();

                            // 判断驱动器类型 
                            if (System2Module.GetInstance().BondModule.BondHead.AxisZ.AxisDrive is ETELAxis)
                            {
                                double initialValue = this.bondHeadController.GetBondForceCurrentVal(inputForce);

                                forceReference = ForceCalibrationService.ActualForceToInputForce(
                                    inputForce,
                                    initialValue,
                                    angle);

                                // 这里加速度默认是速度的10倍
                                // 为了和标定统一，改成速度和加速度改成100和500
                                System2Module.GetInstance().BondModule.BondHead.EtelForceControlSet(
                                    forceReference,
                                    bondLevel,
                                    100 /*speed*/,
                                    500 /*speed * 10.0*/,
                                    100);
                            }
                            else
                            {
                                // 判定是否小力
                                bool isSmallforce = ForceCalibrationService.JudgeIsSmallForce(inputForce);

                                // 切换通道、设置压力参数
                                this.bondHeadController.ChangeChannelAndSetForceControlPara(isSmallforce);

                                double initialValue = this.bondHeadController.GetBondForceCurrentVal(inputForce);

                                // 出来的单位是N要换算成g
                                forceReference =
                                    ForceCalibrationService.ActualForceToInputForce(inputForce, initialValue, angle)
                                    * 100.0;

                                this.bondHeadController.GTForceControlSet(forceReference, bondLevel, 0, isSmallforce);
                            }

                            #endregion

                            // 计时结束
                            forceControlStopwatch.Stop();

                            // 读焊头力
                            double touchBondheadForceBefore =
                                this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                            double touchForceBefore = /*ModbusService.GetInstance().ReadManometer()[0] / 10.0*/
                                this.system2Controller.GetCalibrateTableForceValue() - calibrateTableForceInitial;

                            double posZBefore = this.bondHeadController.GetAxisZRealPos();

                            // 读取摩尔力设备读数
                            Thread.Sleep(delay);

                            double touchForce = /*ModbusService.GetInstance().ReadManometer()[0] / 10.0*/
                                this.system2Controller.GetCalibrateTableForceValue() - calibrateTableForceInitial;

                            // 读焊头力
                            double touchBondheadForce = this.bondHeadController.GetBondForceCurrentVal(isSmallForce);

                            double posZAfter = this.bondHeadController.GetAxisZRealPos();

                            // 加这句是为了防止Z轴抬起时没有恢复正常速度
                            this.bondHeadController.SetAxisZSpeed(100);

                            // 抬起
                            this.bondHeadController.ForceControlReset(preBondLevel, speed);

                            this.bondModuleController.MoveToSafePos();

                            // 读焊头力
                            double riseForce = /*this.modbusService.ReadBondForce()[0] / 10.0*/
                                this.system2Controller.GetBondheadForceValue();

                            printList.Add(
                                new()
                                    {
                                        dateTime = DateTime.Now,
                                        targetForce = targetForce,
                                        angle = angle,
                                        beforeTouchForce = beforeTouchForce,
                                        posZAfter = posZAfter,
                                        touchBondheadForce = touchBondheadForce,
                                        touchForce = touchForce,
                                        riseForce = riseForce,
                                        time = forceControlStopwatch.ElapsedMilliseconds,
                                        forceReference = forceReference,
                                        touchBondheadForceBefore = touchBondheadForceBefore,
                                        touchForceBefore = touchForceBefore,
                                        posZBefore = posZBefore,
                                    });

                            data.Add(
                                new()
                                    {
                                        targetForce = targetForce,
                                        actualForce = touchForce,
                                        time = forceControlStopwatch.ElapsedMilliseconds,
                                    });

                            if (printList.Count > 500)
                            {
                                FileHelper.SaveForceControlData(printList);

                                printList.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                this.bondHeadController.MoveBondZToSafePos();
            }
        }

        /// <summary>
        ///  测高重复性测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnMeasureHeight_Click(object sender, EventArgs e)
        {
            if (this.system2Controller.ChangeTouchDownAssistance() == false)
            {
                return;
            }

            double testTime = (double)this.SpMeasureHeightTime.Value;

            try
            {
                if (this.measureHeightTestTask == null || this.measureHeightTestTask.IsCompleted)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.isWorking = true;
                    this.measureHeightTestCts = new CancellationTokenSource();
                    this.BtnMeasureHeight.Appearance.BackColor = Color.Yellow;

                    List<double> data = new List<double>();

                    string info = string.Empty;

                    // 获取BMC测高位置
                    AKRSPoint3D point = CalibrateRunPara.GetInstance().BMCMeasureHeightSearchMachinePos;

                    Stopwatch sp = Stopwatch.StartNew();
                    this.measureHeightTestTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("测高重复性测试线程");

                                while (true)
                                {
                                    this.RefreshInfo($"测高重复性测试进行中，还剩：{(testTime - sp.Elapsed.TotalMinutes / 60.0):F2}小时");

                                    if (this.measureHeightTestCts.IsCancellationRequested
                                        || sp.Elapsed.TotalHours > testTime)
                                    {
                                        return;
                                    }

                                    // 移动到测高位置
                                    this.bondModuleController.MoveSafeBondXYZ(point);

                                    // 执行测高
                                    (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                        BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                        HeightMeasurementFunctionEnum.WithTDSensor);

                                    if (res.Ret == ExcuteResult.Success)
                                    {
                                        // 数据收集
                                        data.Add(res.HeightValue);
                                    }

                                    //// 测试用
                                    //data.Add(sp.ElapsedMilliseconds);

                                    Thread.Sleep(100);
                                }
                            },
                        this.measureHeightTestCts.Token);

                    await this.measureHeightTestTask;

                    this.bondModuleController.MoveToSafePos();

                    this.RefreshInfo($"测高重复性测试数据打印中......");

                    info = this.HandleMeasureHeightData(data);

                    this.RefreshInfo(string.Empty);
                    FrmMachineFunctionTestResult frmResult = new FrmMachineFunctionTestResult(info);
                    frmResult.StartPosition = FormStartPosition.CenterScreen;
                    frmResult.ShowDialog();

                    this.BtnMeasureHeight.Appearance.BackColor = default;
                    this.RefreshInfo(string.Empty);

                    this.isWorking = false;
                }
                else
                {
                    this.measureHeightTestCts.Cancel();
                }
            }
            catch (Exception ex)
            {
                this.isWorking = false;
                AKRSXtraMessageBox.Show(
                    $"测高重复性测试异常! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.BtnMeasureHeight.Appearance.BackColor = default;
                this.RefreshInfo(string.Empty);
            }
        }

        /// <summary>
        /// 取片偏移校准测试
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void BtnCaliPickOffset_Click(object sender, EventArgs e)
        {      
            // todo:暂定
            string componentName = /*"2x2dummy测试"*/"验机芯片";

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(componentName);

            if (component == null)
            {
                AKRSXtraMessageBox.Show($"不存在芯片：{componentName}！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            int testTime = (int)this.SpPickOffsetCaliTime.Value;

            try
            {
                if (this.pickOffsetCaliTask == null || this.pickOffsetCaliTask.IsCompleted)
                {
                    if (isWorking)
                    {
                        AKRSXtraMessageBox.Show("请先等测试结束再启动！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    this.isWorking = true;
                    List<AKRSPoint2D> data = new List<AKRSPoint2D>();

                    this.pickOffsetCaliCts = new CancellationTokenSource();
                    this.BtnCaliPickOffset.Appearance.BackColor = Color.Yellow;

                    this.PrepareBeforePickOffsetAutoCali(component);

                    this.pickOffsetCaliTask = Task.Run(
                        () =>
                            {
                                CommonUtil.SetCurrentThreadName("取片偏移值重复性校准线程");

                                for (int j = 0; j < testTime; j++)
                                {
                                    this.RefreshInfo($"取片偏移值重复性校准测试进行中，第{j+1}次");

                                    if (this.pickOffsetCaliCts.IsCancellationRequested)
                                    {
                                        return;
                                    }

                                    ExcuteResult ret = this.system2Controller.PickOffsetCaliAssistance(component);

                                    if (ret != ExcuteResult.Success)
                                    {
                                        AKRSXtraMessageBox.Show(
                                            $"取片偏移值校准失败! \r\n Message: {ret.ToString()}",
                                            "报警",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        // 失败直接退出
                                        return;
                                    }

                                    // 数据收集
                                    data.Add(component.PickupOffset.DeepClone());

                                    // 刷新Map信息
                                    WaferSystemDomain.GetInstance().Block.RefreshMap();

                                    // 芯片名称传给晶圆台
                                    WaferSystemDomain.GetInstance().WaferSubSystemTask
                                        .SetCurrentNeedChipName(componentName);

                                    // 给晶圆台发要料信号
                                    SignalPool.GetInstance().IsBondNeedChipSignal.Set();

                                    Thread.Sleep(100);
                                }
                            },
                        this.pickOffsetCaliCts.Token);

                    await this.pickOffsetCaliTask;

                    // 数据处理、界面显示
                    string info = this.HandlePickOffsetData(data);

                    FrmMachineFunctionTestResult frmResult = new FrmMachineFunctionTestResult(info);
                    frmResult.StartPosition = FormStartPosition.CenterScreen;
                    frmResult.ShowDialog();

                    Machine.GetInstance().Stop();
                    this.BtnCaliPickOffset.Appearance.BackColor = default;
                    this.RefreshInfo(string.Empty);
                    this.bondModuleController.MoveToSafePos();

                    this.isWorking = false;
                }
                else
                {
                    this.pickOffsetCaliCts.Cancel();
                }
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show(
                    $"取片偏移值校准失败! \r\n Message: {ex.Message}",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.isWorking = false;

                Machine.GetInstance().Stop();
                this.BtnCaliPickOffset.Appearance.BackColor = default;
                this.RefreshInfo(string.Empty);
                this.bondModuleController.MoveToSafePos();
            }
        }

        /// <summary>
        ///  取片偏移自动校准前准备
        /// </summary>
        /// <param name="component">芯片</param>
        private void PrepareBeforePickOffsetAutoCali(BaseCarrierConfig component)
        {
            // 寻找Pr模板
            PREntity pREntity =
                (PREntity)VisionEntityRepository.GetInstance().Find(component.UpLookAdjustConfig.P1PRName);

            if (pREntity == null)
            {
                throw new Exception($"芯片：{component.Name} 未找到上视PR！");
            }

            // 换吸嘴
            if (this.system2Controller.ChangeNozzleAssistance(component.NozzleName) == false)
            {
                throw new Exception($"吸嘴：{component.NozzleName}更换失败！");
            }

            if (!System2RunTimeProvider.IsWaferCheckSucceed)
            {
                // 检查
                if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                {
                    return;
                }

                System2RunTimeProvider.IsWaferCheckSucceed = true;
            }

            // 复位信号
            Machine.GetInstance().ResetMachineSignal();

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

            // 初始化搜精
            Block.GetInstance().StartInit();
        }

        /// <summary>
        ///  吸嘴示教
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTeachNozzle_Click(object sender, EventArgs e)
        {
            Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();
            if (nozzle == null)
            {
                AKRSXtraMessageBox.Show($"焊头上当前吸嘴为空！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(nozzle.IsSystemConfiguration)
            {
                AKRSXtraMessageBox.Show($"焊头上检测为治具，请更换成吸嘴再进行测试！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FrmNozzleCaliTeach frmNozzleCaliTeach = new FrmNozzleCaliTeach(nozzle);
            if (frmNozzleCaliTeach.IsShowDialog())
            {
                DialogResult dialog = frmNozzleCaliTeach.ShowDialog();
                this.bondModuleController.MoveToSafePos();
            }
        }

        /// <summary>
        /// 芯片示教
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTeachComponent_Click(object sender, EventArgs e)
        {
            // todo:暂定
            string componentName = "2x2dummy测试";

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(componentName);

            if (component == null)
            {
                AKRSXtraMessageBox.Show($"不存在芯片：{componentName}！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            FrmComponentAccuracyMode frmComponentAccuracyMode = new FrmComponentAccuracyMode(component, true);
            if (frmComponentAccuracyMode.IsShowDialog())
            {
                DialogResult dialog = frmComponentAccuracyMode.ShowDialog();
            }
        }

        /// <summary>
        ///  示教顶针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTeachEjection_Click(object sender, EventArgs e)
        {
            FrmEjectionCaliTeach frmEjectionCaliTeach = new FrmEjectionCaliTeach();

            if (frmEjectionCaliTeach.IsShowDialog())
            {
                frmEjectionCaliTeach.ShowDialog();
            }
        }
    }
}