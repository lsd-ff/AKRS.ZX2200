using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Controls.Assistant;
using AKRS.ZX2200.BondSystem.Controls.Manual;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.DispenseSystem.Controllers;
using AKRS.ZX2200.DispenseSystem.Controls.Assistant;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
using AKRS.ZX2200.DispenseSystem.Models.Enums;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Controllers;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.Main.Controls
{
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.LogicHardware.HardWares.Alarmers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Localization;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using OfficeOpenXml;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using ComboBox = DevExpress.XtraEditors.ComboBox;
    //using LicenseContext = System.ComponentModel.LicenseContext;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 快捷菜单
    /// </summary>
    [MethodAop]
    public partial class UcShortcutMenu : DevExpress.XtraEditors.XtraUserControl
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
        /// 点胶控制器
        /// </summary>
        private DispenseController DispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 模组控制器
        /// </summary>
        private IPTController iPTController = new IPTController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 上料仓控制器
        /// </summary>
        private LoaderBinController loaderBinController = new LoaderBinController();

        /// <summary>
        /// 下料仓程式
        /// </summary>
        private UnLoaderBinProgram unLoaderBinProgram = TransportProgram.GetInstance().UnLoaderBinProgram;

        /// <summary>
        /// 上料仓程式
        /// </summary>
        private LoaderBinProgram loaderBinProgram = TransportProgram.GetInstance().LoaderBinProgram;

        /// <summary>
        /// 下料仓控制器
        /// </summary>
        private UnLoaderBinController unLoaderBinController = new UnLoaderBinController();

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
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 刮胶盘控制器
        /// </summary>
        private SlideFluxerController slideFluxerController = new SlideFluxerController();


        /// <summary>
        /// 晶圆检查是否通过
        /// </summary>
        private bool isWaferCheckSucceed = false;

        /// <summary>
        /// 取片线程
        /// </summary>
        private Task pickupTask;

        /// <summary>
        /// 蜂鸣器
        /// </summary>
        private Alarmer alarmer = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcShortcutMenu()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        private void InitControl()
        {
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };

            // 线程未结束不允许关闭窗体
            Form c = (Form)this.Parent;
            if (c != null)
            {
                c.FormClosing += (sender, e) =>
                    {
                        if (this.pickupTask != null && this.pickupTask.IsCompleted == true)
                        {
                            e.Cancel = true;
                        }
                    };
            }

            this.BtSearchBelt1Sys1.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.BtClearDispense.Enabled = MachineHardwareConfiguration.GetInstance().IsSystem1Configrated;
            this.BtVisionBpInDs.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.BtInitializeTS.Enabled = MachineHardwareConfiguration.GetInstance().IsTransportConfigured;
            this.BtnMoveSlideFulxer.Enabled = MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured;

            // 获取蜂鸣器
            List<Alarmer> alarmerList = HardwareRepositoryService.GetHardwaresByType<Alarmer>();
            if (alarmerList == null || alarmerList.Any<Alarmer>())
            {
                alarmer = alarmerList[0];
            }

            this.BtnOpenAlarmer.Enabled = alarmer != null;

            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured)
            {
                this.BtnMoveSlideFulxer.Appearance.BackColor =
                    this.slideFluxerController.IsSlideFluxerAtPLimit() ? Color.Yellow : default;
            }

            this.gPFlip.Enabled = MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated;
        }

        // ---------------------------------流道--------------------------------------------------------

        /// <summary>
        /// 初始化流道
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtInitializeTS_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.transportController.InitializeTS();
        }

        /// <summary>
        /// 清空点胶台载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClearDispense_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.transportController.InitializeTSInDispense();
        }

        /// <summary>
        /// 清空工作台载具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtClearBond_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.transportController.InitializeTSInBond();
        }
      
        /// <summary>
        /// 搜索皮带系统,一般用于产品卡料或者没有下压到位使用此功能
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSearchBelt1Sys1_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.DispenseController.MoveToSafePos();
            transportController.DispenseSubSectionController.MapBelt(false);
        }

        /// <summary>
        /// 搜索皮带系统,一般用于产品卡料或者没有下压到位使用此功能
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSearchBelt2Sys2_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.bondModuleController.MoveToSafePos();

            this.transportController.BondSubSectionController.MapBelt(false);
        }


        // ---------------------------------点胶--------------------------------------------------------

        /// <summary>
        /// 挤/关胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtChangeDispensing_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                if (!System2Domain.GetInstance().S2DispenseController.IsDispensingElectricOpen())
                {
                    AKRSXtraMessageBox.Show("即将移动到挤胶位，人员请离开");

                    System2Domain.GetInstance().BondModuleController.MoveToG0Pos(
                        BondDevicePara.GetInstance().S2DispenseDevicePara.ThrustPosition);

                    System2Domain.GetInstance().S2DispenseController.OpenDispensingElectric();
                    this.BtChangeDispensing.Text = "关闭挤胶";
                    this.BtChangeDispensing.Appearance.BackColor = Color.LightGreen;
                }
                else
                {
                    System2Domain.GetInstance().S2DispenseController.CloseDispensingElectric();
                    this.BtChangeDispensing.Text = "手动挤胶";
                    this.BtChangeDispensing.Appearance.BackColor = Color.White;
                }
            }
            else
            {
                if (!this.DispenseController.IsDispensingElectricOpen())
                {
                    AKRSXtraMessageBox.Show("即将移动到挤胶位，人员请离开");

                    System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(
                        DispenseDevicePara.GetInstance().DispenserPara.ThrustPosition);

                    System1Domain.GetInstance().DispenseController.SetDispensePressure(DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum, 0);
                    this.DispenseController.OpenDispensingElectric();
                    this.BtChangeDispensing.Text = "关闭挤胶";
                    this.BtChangeDispensing.Appearance.BackColor = Color.LightGreen;
                }
                else
                {
                    this.DispenseController.CloseDispensingElectric();
                    this.BtChangeDispensing.Text = "手动挤胶";
                    this.BtChangeDispensing.Appearance.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 换胶水和胶针
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtChangeDispenser_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                FrmAssistantDispense frmAssistantDispense = new FrmAssistantDispense();
                frmAssistantDispense.ShowDialog();
                frmAssistantDispense.Dispose();
            }
            else
            {
                FrmChangeEpoxy frmChangeEpoxy = new FrmChangeEpoxy();
                frmChangeEpoxy.ShowDialog();
                frmChangeEpoxy.Dispose();
            }
        }

        /// <summary>
        /// 预点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPreDispense_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                if (System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off
                    && System2Domain.GetInstance().BondProgram.S2DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
                {
                    AKRSXtraMessageBox.Show("请在程式中开启预点胶");
                    return;
                }

                System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.IsManual = true;

                System2Domain.GetInstance().BondActionNodeRepository.S2PreDispenseActionNode.DoWork();
            }
            else
            {
                if (System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser.PreDispense.PreDispensingMode == PreDispensingModeEnum.Off
                    && System1Domain.GetInstance().System1Program.DispenserProgram.Dispenser.PreDispense.PreDispensingTiming == PreDispensingTimingEnum.Off)
                {
                    AKRSXtraMessageBox.Show("请在程式中开启预点胶");
                    return;
                }


                System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.IsManual = true;

                System1Domain.GetInstance().DispenseActionNodes.PreDispenseAction.DoWork();
            }
        }

        /// <summary>
        /// 清理预点胶板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCleanPreDispensePlate_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.InitPre();
            }
            else
            {
                System1Domain.GetInstance().System1Program.PreDispensePlateProgram.InitPre();
            }
        }

        // ---------------------------------固晶--------------------------------------------------------

        /// <summary>
        /// 焊头吸附
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBondHeadVacuum_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.bondHeadController.GetBondheadVacuumState())
            {
                // 关焊头吸附
                this.bondHeadController.CloseBondHeadVaccum();
                this.BtnBondHeadVacuum.Text = @"开焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Transparent;
            }
            else
            {
                // 开焊头吸附
                this.bondHeadController.OpenBondHeadVaccum();
                this.BtnBondHeadVacuum.Text = @"关焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Yellow;
            }
        }

        /// <summary>
        /// 顶针真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESVacuum_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
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
        /// 抛料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnThrow_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.bondModuleController.ThrowAction();
            this.bondModuleController.MoveToSafePos();
        }

        /// <summary>
        /// 还吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPutBackNozzle_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 去上视
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToUpLook_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.bondModuleController.MoveToUpLookPos();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            this.RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshControl()
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            // 判断顶针真空状态
            if (this.EjectModule.EjectionTableVacuum?.GetOutputValue()?? false)
            {
                this.BtnESVacuum.Text = @"关顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnESVacuum.Text = @"开顶针真空";
                this.BtnESVacuum.Appearance.BackColor = Color.Transparent;
            }

            // 判断焊头吸附真空
            if (this.bondHeadController?.GetBondheadVacuumState()?? false)
            {
                this.BtnBondHeadVacuum.Text = @"关焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnBondHeadVacuum.Text = @"开焊头吸附";
                this.BtnBondHeadVacuum.Appearance.BackColor = Color.Transparent;
            }

            // 判断晶圆台真空状态
            if (WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.WaffleVacuum?.GetOutputValue() ?? false)
            {
                this.BtnWaferTableVacuum.Text = @"关晶圆台真空";
                this.BtnWaferTableVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnWaferTableVacuum.Text = @"开晶圆台真空";
                this.BtnWaferTableVacuum.Appearance.BackColor = Color.Transparent;
            }

            // 判断静态华夫盒真空状态
            if (WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.StaticWaffleVacuum?.GetOutputValue() ?? false)
            {
                this.BtnStaticWaffleVacuum.Text = @"关静态华夫盒真空";
                this.BtnStaticWaffleVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.BtnStaticWaffleVacuum.Text = @"开静态华夫盒真空";
                this.BtnStaticWaffleVacuum.Appearance.BackColor = Color.Transparent;
            }

            //this.TxtVacuumValue.Text = this.bondHeadController.ReadVacuumValue().ToString();

            this.Enabled = Machine.GetInstance().IsStop();

            this.GpUnLoader.Enabled =
                MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin;
            this.GpLoader.Enabled =
                MachineHardwareConfiguration.GetInstance().LoadConfiguration == LoadConfigurationEnum.LoaderBin;

            this.BtnFlipTableGoHome.Enabled = MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated;

            if (MachineHardwareConfiguration.GetInstance().IsFlipModuleConfigrated)
            {
                // 判断焊头吸附真空
                if (this.flipTableController?.GetFlipTableBlowState() ?? false)
                {
                    this.BtnFlipBlow.Appearance.BackColor = Color.Yellow;
                }
                else
                {
                    this.BtnFlipBlow.Appearance.BackColor = Color.Transparent;
                }
            }

            if (alarmer != null)
            {
                this.BtnOpenAlarmer.Appearance.BackColor =
                    this.alarmer.BeepElectric.GetOutputValue() ? Color.Red : default;
            }
        }

        // ------------------上下料仓--------------------------------------------------

        /// <summary>
        /// 从头开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLoaderStartFromFirst_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToBinA();
                this.loaderBinController.MoveToTabletLevel(1);
                this.loaderBinController.ResetLoader();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        ///  下一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLoaderNextTablet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.loaderBinController.IsOutOfTabletLimit(this.loaderBinController.GetCurPlaceLayer() + 1))
            {
                return;
            }

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToNextPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        ///  选择料片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLoaderChooseTablet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            int num;

        // 弹出选择窗体
        RetryCommand:
            string input = XtraInputBox.Show("请输入料片号", "料片号", "1");

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // 输入列数
            if (!int.TryParse(input, out num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"参数错误，请重试！",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                goto RetryCommand;
            }

            this.BtnLoaderChooseTablet.Enabled = false;
            this.BtnLoaderChooseTablet.BackColor = Color.Yellow;

            if (this.loaderBinController.IsOutOfTabletLimit(num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"料片号超限!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                goto RetryCommand;
            }

            try
            {
                // 设置为当前料片
                this.loaderBinController.SetCurPlaceLayer(num);

                // 移动
                this.loaderBinController.MoveToCurrentPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"移动当前料片失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnLoaderChooseTablet.Enabled = true;
                this.BtnLoaderChooseTablet.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 上一个料仓
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnLoaderLastBin_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.loaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        ///  下料仓从第一片开始
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderStartFromFirst_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToBinA();
                this.unLoaderBinController.MoveToTabletLevel(1);
                this.unLoaderBinController.ResetUnloader();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"上料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }

        }

        /// <summary>
        ///  下料仓下一片料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderNextTablet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.unLoaderBinController.IsOutOfTabletLimit())
            {
                return;
            }

            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToNextPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 下料仓选择料片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderChooseTablet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            int num;

        // 弹出选择窗体
        RetryCommand:
            string input = XtraInputBox.Show("请输入料片号", "料片号", "1");

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // 输入列数
            if (!int.TryParse(input, out num))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"参数错误，请重试！",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                goto RetryCommand;
            }

            this.BtnUnLoaderChooseTablet.Enabled = false;
            this.BtnUnLoaderChooseTablet.BackColor = Color.Yellow;

            try
            {
                // 设置为当前料片
                this.unLoaderBinController.SetCurPlaceLayer(num);

                // 移动
                this.unLoaderBinController.MoveToCurrentPlaceLayer();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"移动当前料片失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                this.BtnUnLoaderChooseTablet.Enabled = true;
                this.BtnUnLoaderChooseTablet.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 下料仓去上一个料盒
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnUnLoaderLastBin_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            SimpleButton button = sender as SimpleButton;
            button.BackColor = Color.Yellow;
            button.Enabled = false;
            try
            {
                this.unLoaderBinController.MoveToNextBin();
            }
            catch (Exception exception)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"下料仓移动失败!",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                button.Enabled = true;
                button.BackColor = default;
            }
        }

        /// <summary>
        /// 取芯片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPickComponent_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured
              && !Machine.GetInstance().IsSafeDoorClose)
            {
                AKRSMessageBoxExt.Show(
                    $"安全门未关闭，请关闭安全门后再进行取片测试",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK });

                return;
            }

            FrmSelectComponent frmSelectComponent = new FrmSelectComponent();
            frmSelectComponent.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dia = frmSelectComponent.ShowDialog();

            if (dia == DialogResult.Cancel)
            {
                return;
            }

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(frmSelectComponent.component);

            if (component == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                     $"请先选择芯片！",
                     "Warn",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }

            if (!system2Controller.ChangeNozzleAssistance(component.NozzleName))
            {
                return;
            }

            if (!isWaferCheckSucceed)
            {
                // 检查
                if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                {
                    return;
                }

                isWaferCheckSucceed = true;
            }

            System2RunTimeProvider.RecordTime("取片测试", "晶圆域检查通过");

            // 复位信号
            Machine.GetInstance().ResetMachineSignal();
            System2RunTimeProvider.ResetInformation();

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

            System2RunTimeProvider.RecordTime("取片测试", "给晶圆台发要料信号完成");

            // 防呆
            SimpleButton btn = sender as SimpleButton;
            this.Enabled = false;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            // 防止线程多次启动
            if (this.pickupTask == null || this.pickupTask.IsCompleted == true)
            {
                // 初始化搜精
                Block.GetInstance().StartInit();

                this.pickupTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("取片测试线程");

                            this.Pickup(component.Name);
                        });
            }
        }

        /// <summary>
        /// 取片
        /// </summary>
        private void Pickup(string name)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            try
            {
                BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(name);

                bool ret ;

                if (component.IsUseFlipTable == false)
                {
                    ret = this.system2Controller.PickupFromWaferTable(component);
                }
                else
                {
                    ret = this.system2Controller.PickupFromFlipTable(component);
                }


                System2RunTimeProvider.RecordTime("取片测试", $"取片完成，结果{ret.ToString()}");

                if (!ret)
                {
                    return;
                }

                System2RunTimeProvider.RecordTime("取片测试", $"准备去上视");

                Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

                if (nozzle.ToolAlignment.State != AssistantStateEnum.Able)
                {
                    // 去上视
                    this.bondModuleController.MoveToUpLookPos();
                }
                else
                {
                    // 计算焊头旋转后的吸嘴偏移
                    AKRSPoint2D nozzleOffsetRotated =
                        this.bondHeadController.GetNozzleOffset(nozzle.Name, -nozzle.AlignAngle);

                    AKRSPoint3D pos = new AKRSPoint3D { X = nozzle.RotateCenterPos.X - nozzleOffsetRotated.X, Y = nozzle.RotateCenterPos.Y - nozzleOffsetRotated.Y, Z = nozzle.RotateCenterPos.Z + component.ComponentThickness };

                    // 移到吸嘴旋转中心
                    this.bondModuleController.MoveToG0Pos(pos);
                }

                System2RunTimeProvider.RecordTime("取片测试", "去上视完成");

                // 弹出视觉窗体
                UcMainSystem.VmVisionShow();
                UcMainSystem.ChangeCameraVision("上视相机");
                UcMainSystem.UcGuideMoveOpen("上视模组");
                Machine.GetInstance().Stop();
            }
            catch (Exception ex)
            {
                System2RunTimeProvider.RecordTime("取片测试", $"取片测试异常，{ex.ToString()}");

                AKRSMessageBoxExt.Show(
                            $"取片失败:" + ex.Message,
              "报警",
              new string[] { "确认" },
              new DialogResult[] { DialogResult.OK });
            }
            finally
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            BtnPickComponent.Appearance.BackColor = default;
                            BtnPickComponent.Enabled = true;
                            this.Enabled = true;
                        }));
            }
        }

        /// <summary>
        /// 查看点胶点情况
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVisionBpInDs_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            UcMainSystem.VmVisionShow();
            UcMainSystem.ChangeCameraVision("点胶相机");
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.BondPosition);
            frmModuleSelect.ShowDialog();
        }

        /// <summary>
        /// 查看焊点情况
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVisionBpInBo_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            UcMainSystem.VmVisionShow();
            UcMainSystem.ChangeCameraVision("Bond相机");
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
            FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.BondPosition);
            frmModuleSelect.ShowDialog();
        }

        /// <summary>
        /// 翻转台回0
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnFlipTableGoHome_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.flipTableController.FlipTableGoHome();
        }

        /// <summary>
        /// 手动更换料片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnChangeTabletWithManual_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            FrmChangeTabletTeach frmChangeTablet = new FrmChangeTabletTeach();
            frmChangeTablet.Dispose();
        }

        /// <summary>
        /// 刮胶盘伸出缩回
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnMoveSlideFulxer_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.slideFluxerController.IsSlideFluxerAtNLimit())
            {
                this.slideFluxerController.SlideFluxerOutNoWait();
                this.BtnMoveSlideFulxer.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.slideFluxerController.SlideFluxerHomeNoWait();
                this.BtnMoveSlideFulxer.Appearance.BackColor = default;
            }
        }


        /// <summary>
        /// 翻转台吹气
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnFlipBlow_Click_1(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.flipTableController.GetFlipTableBlowState() == false)
            {
                this.flipTableController.OpenFlipModuleBlow();
                this.BtnFlipBlow.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.flipTableController.CloseFlipModuleBlow();
                this.BtnFlipBlow.Appearance.BackColor = default;
            }
        }

        /// <summary>
        /// 翻转台抛料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnFlipTableThrow_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.flipTableController.IsFlipTableAtHome() == false)
            {
                this.flipTableController.FlipTableGoHome();
            }

            this.flipTableController.Throw();
        }

        /// <summary>
        /// 翻转台取片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnFlipTablePick_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            FrmSelectComponent frmSelectComponent = new FrmSelectComponent();
            frmSelectComponent.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dia = frmSelectComponent.ShowDialog();

            if (dia == DialogResult.Cancel)
            {
                return;
            }

            BaseCarrierConfig component = (BaseCarrierConfig)CarrierConfigRepository.GetInstance().Find(frmSelectComponent.component);

            if (component==null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                     $"请先选择芯片！",
                     "Warn",
                     MessageBoxButtons.OK,
                     MessageBoxIcon.Warning);

                return;
            }

            if (!isWaferCheckSucceed)
            {
                // 检查
                if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                {
                    return;
                }

                isWaferCheckSucceed = true;
            }

            // 复位信号
            Machine.GetInstance().ResetMachineSignal();

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

            // 防呆
            SimpleButton btn = sender as SimpleButton;
            this.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            // 防止线程多次启动
            if (this.pickupTask == null || this.pickupTask.IsCompleted == true)
            {
                // 初始化搜精
                Block.GetInstance().StartInit();

                this.pickupTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("翻转台取片线程");

                            this.FlipFromWafer(component);
                        });
            }
        }

        /// <summary>
        /// 从晶圆台翻转芯片
        /// </summary>
        /// <param name="component">芯片</param>
        private void FlipFromWafer(BaseCarrierConfig component)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            try
            {
                bool ret = WaferSubController.GetInstance().FlipFromWaferTable(component);
            }
            catch (Exception ex)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"翻转台取片失败:" + ex.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                Machine.GetInstance().Stop();
                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            BtnFlipTablePick.Appearance.BackColor = default;
                            this.Enabled = true;
                        }));
            }
        }

        /// <summary>
        /// 显示晶圆图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowWaferMap_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            try
            {
                WaferSystemDomain.GetInstance().Block.ShowMapping();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 显示华夫盒图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShowWaffleMap_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            try
            {
                WaferSystemDomain.GetInstance().Block.ShowMapping(true);
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message);
            }
        }

        /// <summary>
        /// 华夫盒信息设置
        /// </summary>
        private FrmWaffleMap waffleSet;

        /// <summary>
        /// 静态华夫盒信息设置
        /// </summary>
        private FrmWaffleMap StaticWaffleSet;

        /// <summary>
        /// 普通华夫盒设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWaffleSet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.waffleSet = new FrmWaffleMap(false);
            this.waffleSet.ShowDialog();
        }

        /// <summary>
        /// 静态华夫盒设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnStaticWaffleSet_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            this.StaticWaffleSet = new FrmWaffleMap(true);
            this.StaticWaffleSet.ShowDialog();
        }


        /// <summary>
        /// 蜂鸣器开关
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOpenAlarmer_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (this.alarmer.BeepElectric.GetOutputValue())
            {
                this.alarmer.BeepElectric.SetOutputValue(false);
            }
            else
            {
                this.alarmer.BeepElectric.SetOutputValue(true);
            }
        }

        private void BtnWaferTableVacuum_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (!WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.WaffleVacuum.GetOutputValue())
            {
                // 先关吹气电磁阀
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.WaffleVacuum.SetOutputValue(false);

                // 开真空
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.WaffleVacuum.SetOutputValue(true);
                this.BtnWaferTableVacuum.Text = @"关晶圆台真空";
                this.BtnWaferTableVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.WaffleVacuum.SetOutputValue(false);
                this.BtnWaferTableVacuum.Text = @"开晶圆台真空";
                this.BtnWaferTableVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        private void BtnStaticWaffleVacuum_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            if (!WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.StaticWaffleVacuum.GetOutputValue())
            {
                // 先关吹气电磁阀
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.StaticWaffleVacuum.SetOutputValue(false);

                // 开真空
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.StaticWaffleVacuum.SetOutputValue(true);
                this.BtnStaticWaffleVacuum.Text = @"关静态华夫盒真空";
                this.BtnStaticWaffleVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                WaferSystemDomain.GetInstance().WaferSubModule.WaferTable.StaticWaffleVacuum.SetOutputValue(false);
                this.BtnStaticWaffleVacuum.Text = @"开静态华夫盒真空";
                this.BtnStaticWaffleVacuum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 传送到下一个位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtTransportToNext_Click(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                return;
            }
            TransportDomain.GetInstance().TransportController.TransportUnitIndex();
        }
    }
}
