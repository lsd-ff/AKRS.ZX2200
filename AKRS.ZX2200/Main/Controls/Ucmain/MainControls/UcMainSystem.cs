namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.Galaxy.SoftKey;
    using AKRS.Galaxy2.BackgroundWorkThread;
    using AKRS.Galaxy2.Component.Simple.FileCleaner;
    using AKRS.Galaxy2.CoordinateSystems.Controls;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.ControlServices;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.LogicHardware.Hardwares.MotionControllers;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.Galaxy2.Recipe;
    using AKRS.Galaxy2.UserManager;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.BondForce.Models.ClosedLoopModels;
    using AKRS.ZX2200.BondSystem.BondForce.Models.DevicePara;
    using AKRS.ZX2200.BondSystem.BondForce.Services;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Controls.Setting.Customer;
    using AKRS.ZX2200.BondSystem.Controls.Setting.PostBond;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
    using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.CalibSystem.Controls;
    using AKRS.ZX2200.CalibSystem.Controls.Assistant;
    using AKRS.ZX2200.CalibSystem.GlobalCalibration;
    using AKRS.ZX2200.CalibSystem.GlobalCalibrationEnd.BCOffsetRelationCalib;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.CalibSystem.TestControls;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Controls.Manual;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.EpoxyMaterial;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Experiment.AccuracyExperiment;
    using AKRS.ZX2200.Experiment.BondAccuracyExperiment;
    using AKRS.ZX2200.Experiment.RepeatPositionAccuracy;
    using AKRS.ZX2200.Experiment.SpeedImprovementExperiment;
    using AKRS.ZX2200.Experiment.Test;
    using AKRS.ZX2200.Experiment.TestFunction;
    using AKRS.ZX2200.Infrastructure.AOP.Module;
    using AKRS.ZX2200.Infrastructure.Controls.Common;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram;
    using AKRS.ZX2200.Infrastructure.HeatingSettings;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.Localization;
    using AKRS.ZX2200.Main.Machine.Control;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Parameter;
    using AKRS.ZX2200.Main.Machine.Product;
    using AKRS.ZX2200.Main.Machine.Product.Controls;
    using AKRS.ZX2200.SupportFeature.Calibrate;
    using AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate;
    using AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate;
    using AKRS.ZX2200.SupportFeature.Consumables;
    using AKRS.ZX2200.SupportFeature.Logs;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.ObjParameter;
    using AKRS.ZX2200.SupportFeature.SensorCheckScan;
    using AKRS.ZX2200.SupportFeature.Statistics;
    using AKRS.ZX2200.SupportFeature.TemperatureMonitoring;
    using AKRS.ZX2200.TransportSystem.Controls.Feature;
    using AKRS.ZX2200.TransportSystem.Controls.Manual;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.Bondinsert;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.InOutPut;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.TansportBelt;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Mapping;
    using AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using AKRS.ZX2200.TransportUnitSystem.Service;
    using AKRS.ZX2200.WaferSubSystem.Controls;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
    using DevExpress.Utils;
    using DevExpress.Utils.Controls;
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Docking;
    using DevExpress.XtraEditors;
    using log4net.Core;
    using MathNet.Numerics;
    using SqlSugar;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using static DevExpress.Utils.Frames.FrameHelper;
    using BaseControl = AKRS.ZX2200.Infrastructure.Controls.Currency.BaseControl;
    using Control = System.Windows.Forms.Control;
    using LanguageEnum = Localization.LanguageEnum;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 主系统控件
    /// </summary>
    [MethodAop]
    public partial class UcMainSystem : BaseControl
    {
        /// <summary>
        /// Bond域
        /// </summary>
        private System2Domain System2Domain => System2Domain.GetInstance();

        /// <summary>
        /// Bond域
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller System2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private readonly DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        #region 控件

        /// <summary>
        /// 编辑程式控件
        /// </summary>
        private UcEditProductProgramming ucEditProductProgramming;

        /// <summary>
        /// 组件界面
        /// </summary>       
        private UcModule ucModule;

        #endregion 控件

        /// <summary>
        /// 构造方法
        /// </summary>
        public UcMainSystem()
        {
            this.InitializeComponent();
            this.BarBtHidePageHeader.Visibility = BarItemVisibility.Never;

            this.Init();
            this.Dock = DockStyle.Fill;

            this.LueLanguage.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<LanguageEnum>();
            this.LueLanguage.EditValue = LanguageSettings.GetInstance().Language;

            FormLocalizer.LocalizeForm(this);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.ucModule = new UcModule { Dock = DockStyle.Fill };

            this.NavFrameMain.Controls.Add(this.ucModule);

            this.LbTip.Visible = false;
        }

        /// <summary>
        /// Tile Bar Item 点击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TileBarItem_ItemClick(object sender, TileItemEventArgs e)
        {
            int selectedIndex = e.Item.Id - 1;
            if (selectedIndex != 10)
            {
                this.NavFrameMain.SelectedPageIndex = selectedIndex;
                this.TileBarMain.SelectedItem = e.Item;
            }
            else
            {
                // 展示快捷菜单
                // this.splitContainer1.Panel1Collapsed = false;
            }
        }

        /// <summary>
        /// 初始化按键信息传递
        /// </summary>
        private void InitKeyPreview()
        {
            Form frmForm = (Form)this.ParentForm;

            if (frmForm != null)
            {
                frmForm.KeyPreview = true;
                frmForm.KeyDown += this.UcMainSystem_KeyDown;
            }
        }

        /// <summary>
        /// Load事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void UcMainSystem_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitKeyPreview();

                this.InitAction();

                // 主界面进度条标签显示
                this.TkbMachineSpeed.Properties.ShowLabels = true;
                this.TkbMachineSpeed.CustomLabel += (s, args) =>
                    {
                        args.LabelInfo.Label = $@"{args.LabelInfo.Value}%";
                        args.Handled = true;
                    };

                this.TkbMachineSpeed.RefreshLabels();
                this.TkbMachineSpeed.Value =
               (int)(MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage * 100);

                // 注册界面刷新任务 
                BackgroundThreadService.RegisterTask(
                    Guid.NewGuid(),
                    "刷新UcMain界面状态",
                    () =>
                    {
                        this.BeginInvoke(new System.Action(() =>
                        {
                            // 根据设备状态改变界面
                            this.ChangeUiByState();

                            // 根据配置刷新UI
                            this.ChangeUiByConfiguration();

                            // 长时间未操作登出高级权限账户
                            Machine.GetInstance().AutoLogOut();

                            string detectingEncryption = string.Empty;

                            if (MachineSoftwareConfiguration.GetInstance().IsDetectingEncryption && MachineSoftwareConfiguration.GetInstance().IsPromptEncryption)
                            {
                                detectingEncryption = $"用户号:{Machine.GetInstance().AuthenticatorId},授权到期时间{Machine.GetInstance().AuthenticatorDueDate}";
                            }

                            MainForm.MF.HtmlText =
                                $"<p align=\"center\" ><b>[{detectingEncryption} 当前程式:{MachineConfigContext.GetInstance().RecipeName},当前用户:{RuntimeProvider.CurrentLoginUser?.Name},工作模式:{MachineStateModel.GetInstance().MachineWorkMode.GetDescription()}]</b></p>";
                        }));
                    },
                    1,
                    1000,
                    false);

                BackgroundThreadService.ActivateSingleTaskExecute();

                this.RegisterGlobalHotkeys();

                ExportService.EnsureDirectoryExists(@"D:\\设备功能测试");
            }
            catch (Exception ex)
            {
                AKRSXtraMessageBox.Show($"Main Load 出错：{ex.Message}");
            }
        }

        /// <summary>
        /// 新建产品
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNewProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FrmAddRecipe frmAddRecipe = new FrmAddRecipe(RecipeRepository.GetInstance().RecipeList, TuService.CopyOldRecipePR);
            DialogResult dialog = frmAddRecipe.ShowDialog();

            if (dialog == DialogResult.OK)
            {
                Recipe oldRecipe = MachineConfigContext.GetInstance().CurrentRecipe;

                if (frmAddRecipe.Recipe != null)
                {
                    if (AKRSXtraMessageBox.Show(
                            $"是否将当前程式切换为:{frmAddRecipe.Recipe.RecipeName}?",
                            "提示",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        MachineConfigContext.GetInstance().RecipeID = frmAddRecipe.Recipe.RecipeID;
                        MachineConfigContext.GetInstance().SaveMachineConfig();

                        if (oldRecipe != null)
                        {
                            if (frmAddRecipe.Recipe.RecipeID != oldRecipe.RecipeID)
                            {
                                this.ReLoadRecipe();
                            }
                        }
                        else
                        {
                            this.ReLoadRecipe();
                        }
                    }
                }
            }

            frmAddRecipe.Dispose();

            // 触发一次界面刷新
            BackgroundThreadService.ActivateSingleTaskExecute();

            RecipeRepository.GetInstance().Save();
        }

        /// <summary>
        /// 编辑程式
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtEditProduct_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Machine.GetInstance().IsWorking())
            {
                string message = "设备正在工作，请先停止工作！";
                AKRSXtraMessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.DryCycle)
            {
                string message = "设备处于空跑模式，不允许编辑程式";
                AKRSXtraMessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ProductConfiguration.GetInstance() == null
                || string.IsNullOrEmpty(MachineConfigContext.GetInstance().RecipeName))
            {
                string message = "请先新建程式!";
                AKRSXtraMessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;

            this.ShowPanel(
                "ucEditProductProgramming",
                () =>
                    {
                        this.ucEditProductProgramming = new UcEditProductProgramming();
                        return this.ucEditProductProgramming;
                    });
        }

        /// <summary>
        /// 存不存在此DockPanel
        /// </summary>
        /// <param name="controlName">组件名</param>
        /// <returns>是否存在</returns>
        private DockPanel GetDockPanel(string controlName)
        {
            DockPanel dockPanel = null;

            foreach (DockPanel dockManagerPanel in this.dockManager.Panels)
            {
                Control[] cs = dockManagerPanel.Controls.Find(controlName, true);

                if (cs.Any())
                {
                    dockPanel = dockManagerPanel;
                    break;
                }
            }

            return dockPanel;
        }

        /// <summary>
        /// 显示DockPanel
        /// </summary>
        /// <param name="controlName">组件名称</param>
        /// <param name="createUserControlAction">DockPanel 里包含的组件</param>
        public void ShowPanel(string controlName, Func<XtraUserControl> createUserControlAction)
        {
            DockPanel dockPanel = this.GetDockPanel(controlName);

            if (dockPanel != null)
            {
                Form form = dockPanel.ParentForm;

                if (form != null)
                {
                    form.TopMost = true;
                    form.Show();
                    form.WindowState = FormWindowState.Normal;
                    form.TopMost = false;
                }

                dockPanel.BringToFront();
            }
            else
            {
                XtraUserControl userControl = createUserControlAction();
                Size size = userControl.Size;
                dockPanel = this.dockManager.AddPanel(userControl, new Point(220, 70), userControl.Text);
                dockPanel.Text = userControl.Text;
                dockPanel.FloatSize = size + new Size(20, 20);
            }
        }

        /// <summary>
        /// 关闭DockPanel
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void DockManager_ClosedPanel(object sender, DockPanelEventArgs e)
        {
            this.dockManager.RemovePanel(e.Panel);
            e.Panel.Dispose();
        }

        /// <summary>
        /// 关闭DockPanel
        /// </summary>
        /// <param name="panelName">控件的名称</param>
        private void DockManager_ClosedPanel(string panelName)
        {
            DockPanel dockPanel = this.GetDockPanel(panelName);

            if (dockPanel != null)
            {
                this.dockManager.RemovePanel(dockPanel);
                dockPanel?.Dispose();
            }
        }

        /// <summary>
        /// 系统切换
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarItemCmdSystemType_EditValueChanged(object sender, EventArgs e)
        {
            if (this.BarItemCmdSystemType.EditValue.ToString() == "System 1")
            {
                MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
            }
            else if (this.BarItemCmdSystemType.EditValue.ToString() == "System 2")
            {
                MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
            }
        }

        /// <summary>
        /// 显示PageHeader
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtShowPageHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.StartGroup.ActivationTarget.Properties.ShowCaption = DefaultBoolean.True;
            this.BarBtShowPageHeader.Visibility = BarItemVisibility.Never;
            this.BarBtHidePageHeader.Visibility = BarItemVisibility.Always;
        }

        /// <summary>
        /// 隐藏PageHeader
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtHidePageHeader_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.StartGroup.ActivationTarget.Properties.ShowCaption = DefaultBoolean.False;
            this.BarBtShowPageHeader.Visibility = BarItemVisibility.Always;
            this.BarBtHidePageHeader.Visibility = BarItemVisibility.Never;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtInitialize_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().AllAxisGoHome();
           
        }


        /// <summary>
        /// 参数显示
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtDataPage_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ProductConfiguration.GetInstance() == null)
            {
                string message = "请先新建程式!";
                AKRSXtraMessageBox.Show(message);
                return;
            }

            this.ShowPanel(
               "UcProgramArgs",
               () =>
               {
                   UcProgramArgs ucProgramArgs = new UcProgramArgs();
                   return ucProgramArgs;
               });
        }

        /// <summary>
        /// 视觉结果图片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtVideoWindow_ItemClick(object sender, ItemClickEventArgs e)
        {
            UcMainSystem.VmVisionShow();
        }

        /// <summary>
        /// 视觉结果图片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtShowVisionResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (this.frmVmVisionResult == null || this.frmVmVisionResult.IsDisposed)
            //{
            //    this.frmVmVisionResult = new FrmVmVisionResult();
            //}

            //this.frmVmVisionResult.TopMost = true;
            //this.frmVmVisionResult.Show();
            //this.frmVmVisionResult.TopMost = false;

            this.ShowForm<FrmVmVisionResult>();
        }

        /// <summary>
        /// BtShowWaferMap
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtShowWaferMap_ItemClick(object sender, ItemClickEventArgs e)
        {
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
        /// 槽位状态
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtSlotState_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.ShowPanel(
            //    "UcSlotsState",
            //    () =>
            //        {
            //            FrmSlotsState ucSlotsState = new FrmSlotsState();
            //            return ucSlotsState;
            //        });

            FrmSlotsState ucSlotsState = new FrmSlotsState();
            ucSlotsState.ShowDialog();
            ucSlotsState.Dispose();
        }

        /// <summary>
        /// 产品保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // WaferSub System
                WaferSubDevicePara.GetInstance().Save();
                AdapterConfigRepository.GetInstance().Save();
                CarrierConfigRepository.GetInstance().Save();
                EjectionBankConfigRepository.GetInstance().Save();
                EjectionConfigRepository.GetInstance().Save();
                MagazineAllocationsConfigRepository.GetInstance().Save();
                MagazineBoxGeoConfigRepository.GetInstance().Save();
                MagazineBoxConfigRepository.GetInstance().Save();
                WaferSystemProgram.GetInstance().Save();
                FlipToolRepository.GetInstance().Save();

                // Bond system
                BondDevicePara.GetInstance().Save();
                NozzleRepository.GetInstance().Save();
                NozzleShelfRepository.GetInstance().Save();
                PostBondInspectionRepository.GetInstance().Save();
                ProductDomain.GetInstance().Save();
                ForceConfig.GetInstance().Save();
                System2Configuration.GetInstance().Save();
                MachineCoordinateSystem.GetInstance().Save();
                BondProgram.GetInstance().Save();
                ForceCalibrationData.GetInstance().Save();

                // TU system
                ProductConfiguration.GetInstance().Save();
                TransportDevicePara.GetInstance().Save();
                MachineConfigContext.GetInstance().SaveMachineConfig();
                TransportProgram.GetInstance().Save();
                LoaderBinRepository.GetInstance().Save();
                UnLoaderBinRepository.GetInstance().Save();
                TransportBeltSettingRepository.GetInstance().Save();
                BondinsertSettingRepository.GetInstance().Save();
                InOutPutBeltSettingRepository.GetInstance().Save();

                // System1
                DispenseDevicePara.GetInstance().Save();
                System1Program.GetInstance().Save();
                DispenserRepository.GetInstance().Save();
                EpoxyApplicationRepository.GetInstance().Save();
                EpoxyMaterialRepository.GetInstance().Save();

                // machine
                MachineHardwareConfiguration.GetInstance().Save();
                MachineSoftwareConfiguration.GetInstance().Save();
                VisionEntityRepository.GetInstance().Save();
                RtuConnectConfig.GetInstance().Save();
                MachineDevicePara.GetInstance().Save();

                AKRSXtraMessageBox.Show("保存成功！");
            }
            catch (Exception exception)
            {
                LogHelper.Post(Level.Error, "保存出错", exception, LogCategory.MainSoftWare, ViewType.InFileAndUI);
                AKRSXtraMessageBox.Show("保存失败\n" + exception.Message);
            }
        }

        /// <summary>
        /// BtStart_ItemClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                this.Enabled = false;

                if (MachineSoftwareConfiguration.GetInstance().RecipeWarningBeforeMachineStart)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"请确认当前程式：{MachineConfigContext.GetInstance().RecipeName} 是否正确?\r\nOK:继续工作\r\nCancel:退出",
                        "提示",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Question);

                    if (dialog == DialogResult.Cancel)
                    {
                        return;
                    }
                }

                this.CloseAllDockPanel();

                // 退出单步模式
                MachineStateModel.GetInstance().IsSingleStepWork = false;
                this.BarSwitchSingleStep.Checked = false;

                Machine.GetInstance().Start();
            }
            catch (Exception ex)
            {
                Machine.GetInstance().Stop();
                AKRSXtraMessageBox.Show("启动失败" + ex.Message);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        /// <summary>
        /// 清除所有的DockPanel
        /// </summary>
        private void CloseAllDockPanel()
        {
            List<DockPanel> dockPanels = this.dockManager.Panels.ToList();
            for (int i = 0; i < dockPanels.Count; i++)
            {
                if (dockPanels[i] != null)
                {
                    dockPanels[i].Close();
                }
            }
        }

        /// <summary>
        /// BtCancel_ItemClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtCancel_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Enabled = false;
            this.LbTip.Text = "设备正在停止，请稍后";
            this.LbTip.Visible = true;
            Machine.GetInstance().Stop();
        }

        /// <summary>
        /// BtPause_ItemClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtPause_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().Pause();
        }

        /// <summary>
        /// 流道复位按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtInitializeTS_ItemClick(object sender, ItemClickEventArgs e)
        {
            TransportDomain.GetInstance().TransportController.InitializeTS();
        }

        /// <summary>
        /// BtCustomerSetup_ItemClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtCustomerSetup_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmCustomerSetup frmCustomerSetup = new FrmCustomerSetup();
            frmCustomerSetup.ShowDialog();

            frmCustomerSetup.Dispose();
        }

        /// <summary>
        /// 加载产品
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtLoad_ItemClick(object sender, ItemClickEventArgs e)
        {
            Recipe oldRecipe = MachineConfigContext.GetInstance().CurrentRecipe;
            FrmLoadRecipe frmLoadRecipe = new FrmLoadRecipe();
            DialogResult dr = frmLoadRecipe.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (frmLoadRecipe.Recipe != null)
                {
                    MachineConfigContext.GetInstance().RecipeID = frmLoadRecipe.Recipe.RecipeID;
                    MachineConfigContext.GetInstance().SaveMachineConfig();

                    if (oldRecipe != null)
                    {
                        if (frmLoadRecipe.Recipe.RecipeID != oldRecipe.RecipeID)
                        {
                            this.ReLoadRecipe();
                        }
                    }
                    else
                    {
                        this.ReLoadRecipe();
                    }

                }
                else
                {
                    AKRSXtraMessageBox.Show("切换程式失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                frmLoadRecipe.Dispose();

                // 触发一次界面刷新
                BackgroundThreadService.ActivateSingleTaskExecute();
            }
        }

        /// <summary>
        /// 继续运行
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtContinue_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().Continue();
        }

        /// <summary>
        /// 继续运行
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void LueWorkModel_EditValueChanged(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().MachineWorkMode = (MachineWorkModeEnum)this.LueWorkModel.EditValue;
        }

        /// <summary>
        /// 流道调试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtTransportSystem_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcTransportSystemManual",
                () =>
                    {
                        UcTransportSystemManual ucTransportSystemManual = new UcTransportSystemManual();
                        return ucTransportSystemManual;
                    });
        }

        /// <summary>
        /// 系统1启用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSystem1Enable_Click(object sender, EventArgs e)
        {
            // 取反
            System1Domain.GetInstance().DispenseWorkTask.Enable = !System1Domain.GetInstance().DispenseWorkTask.Enable;
            this.BtnSystem1Enable.Appearance.BackColor = System1Domain.GetInstance().DispenseWorkTask.Enable ? Color.Green : Color.Yellow;
        }

        /// <summary>
        /// 系统2启用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSystem2Enable_Click(object sender, EventArgs e)
        {
            // 取反
            System2Domain.BondTask.Enable = !System2Domain.BondTask.Enable;
            this.BtnSystem2Enable.Appearance.BackColor = System2Domain.BondTask.Enable ? Color.Green : Color.Yellow;
        }

        /// <summary>
        /// 上料启用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUpTu_Click(object sender, EventArgs e)
        {
            // 取反
            TransportDomain.GetInstance().LoaderTask.Enable = !TransportDomain.GetInstance().LoaderTask.Enable;
            this.BtUpTu.Appearance.BackColor = TransportDomain.GetInstance().LoaderTask.Enable ? Color.Green : Color.Yellow;
        }

        /// <summary>
        /// 下料启用
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDownTu_Click(object sender, EventArgs e)
        {
            // 取反
            TransportDomain.GetInstance().UnloaderTask.Enable = !TransportDomain.GetInstance().UnloaderTask.Enable;
            this.BtDownTu.Appearance.BackColor = TransportDomain.GetInstance().UnloaderTask.Enable ? Color.Green : Color.Yellow;
        }

        /// <summary>
        /// Recipe管理
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtRecipeManage_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRecipeManage frmRecipeManage = new FrmRecipeManage(TuService.CopyOldRecipePR, TuService.DeleteOldRecipePR);
            frmRecipeManage.ShowDialog();

            frmRecipeManage.Dispose();

            //VmSolution.Instance.CloseSolution();
            //VmSolution.CreatSolInstance();
            // 触发一次界面刷新
            BackgroundThreadService.ActivateSingleTaskExecute();
        }

        /// <summary>
        /// 工作模式选择
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtProductionMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Machine.GetInstance().IsWorking())
            {
                AKRSXtraMessageBox.Show(
                    $"Machine  is  working! Please  stop  machine  first!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            FrmProductionMode frmProductionMode = new FrmProductionMode();
            frmProductionMode.ShowDialog();

            frmProductionMode.Dispose();
        }

        /// <summary>
        /// 清楚报警
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemoveAlarm_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().ResetAlarm();
        }

        /// <summary>
        /// 单步模式
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarSwitchSingleStep_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            if (this.BarSwitchSingleStep.Checked)
            {
                MachineStateModel.GetInstance().IsSingleStepWork = true;
                this.BarBtNextStep1.Enabled = true;
            }
            else
            {
                // 单步执行
                SignalPool.GetInstance().System2SingleStepSignal.Set();
                MachineStateModel.GetInstance().IsSingleStepWork = false;
                this.BarBtNextStep1.Enabled = false;
            }
        }

        /// <summary>
        /// 单步执行
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtNextStep_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().IsSingleStepWork)
            {
                if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                {
                    // 单步执行
                    SignalPool.GetInstance().System2SingleStepSignal.Set();
                }
                else
                {
                    // 单步执行
                    SignalPool.GetInstance().System1SingleStepSignal.Set();
                }
            }
        }

        /// <summary>
        /// TUMapping图
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtTUMapping_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.UseNewMappingUi)
            {
                FrmTuMappingNew frmTuMappingNew = new FrmTuMappingNew();
                frmTuMappingNew.ShowDialog();

                if (TransportDomain.GetInstance().TransportController.IsExistTu)
                {
                    TransportDomain.GetInstance().TransportController.InitializeTS();
                }
            }
            else
            {
                // mapping图
                FrmTuMapping frmTuMapping = new FrmTuMapping(ProductConfiguration.GetInstance(), ProductConfiguration.GetInstance().Save);
                frmTuMapping.ShowDialog();

                frmTuMapping.Dispose();

                if (TransportDomain.GetInstance().TransportController.IsExistTu)
                {
                    TransportDomain.GetInstance().TransportController.InitializeTS();
                }
            }
        }

        /// <summary>
        /// 急停
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtEmergencyStop_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<Axis> axisList = HardwareRepositoryService.GetHardwaresByType<Axis>();

            Parallel.ForEach(
                axisList,
                (axis, sate, i) =>
                    {
                        axis.StopMove(false, true);
                    });
        }

        /// <summary>
        /// 流道复位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtInitializeTS_ItemClick(object sender, ItemClickEventArgs e)
        {
            TransportDomain.GetInstance().TransportController.InitializeTS();
        }

        /// <summary>
        /// 机器统计界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtMachineStatistics_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        /// <summary>
        /// 生产统计界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtProductionStatistics_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        /// <summary>
        /// 光源
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBrightnessSetting_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcGuideMove",
                () =>
                    {
                        UcLampRegulation ucLampRegulation = new UcLampRegulation();
                        return ucLampRegulation;
                    });
        }

        /// <summary>
        /// 光源调节
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtLampRegulation_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcLampRegulation",
                () =>
                    {
                        UcLampRegulation ucLampRegulation = new UcLampRegulation();
                        return ucLampRegulation;
                    });
        }

        /// <summary>
        /// BtSelfCHeck_ItemClick
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSelfCHeck_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().SelfCheck();
        }

        /// <summary>
        /// 是否继续上料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEmptyIndex_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 连续上料时的判断
            if (TransportProvider.ContinuousFeeding)
            {
                TransportProvider.ContinuousFeeding = false;
                this.BtEmptyIndex.ItemAppearance.Normal.BackColor = Color.Red;
            }
            else
            {
                TransportProvider.ContinuousFeeding = true;
                this.BtEmptyIndex.ItemAppearance.Normal.BackColor = Color.LightGreen;
            }
        }

        /// <summary>
        /// 关机重启按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BarBtShutDown_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmShutDown frmShutDown = new FrmShutDown();
            frmShutDown.TopMost = true;
            frmShutDown.StartPosition = FormStartPosition.CenterScreen;
            frmShutDown.BringToFront();
            frmShutDown.ShowDialog();

            frmShutDown.Dispose();
        }

        /// <summary>
        /// BtCurrentRecipePRList_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtCurrentRecipePRList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmPRList frmPrList = new FrmPRList(System2CommonService.GetCurrentRecipePREntityNameList());
            frmPrList.ShowDialog();

            frmPrList.Dispose();
        }

        /// <summary>
        /// BtLogon_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtLogon_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmLogin frmLogin = new FrmLogin();
            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                StatisticsService.LastOperaDateTime = DateTime.Now;
                RuntimeProvider.CurrentLoginUser = frmLogin.CurrentUser;
            }
        }

        /// <summary>
        /// BtnUserManager_ItemClick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnUserManager_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (RuntimeProvider.CurrentLoginUser != null && Machine.GetInstance().CurrentRole == RoleEnum.Admin)
            {
                this.ShowPanel(
                    "UcUserManager",
                    () =>
                        {
                            Galaxy2.UserManager.CustomControls.UcUserManager ucUserManager = new Galaxy2.UserManager.CustomControls.UcUserManager();
                            return ucUserManager;
                        });
            }
            else
            {
                AKRSXtraMessageBox.Show("权限太低，只有管理员能进入!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 设备坐标系
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMachineCoordinate_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmCoordinateOffset frmCoordinateOffset = new FrmCoordinateOffset();
            frmCoordinateOffset.ShowDialog();

            frmCoordinateOffset.Dispose();
        }

        /// <summary>
        /// 加载程式
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLoadProduct_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        /// <summary>
        /// 位置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtnVisualStatic_ItemClick(object sender, ItemClickEventArgs e)
        {
            // FrmChooseCamera frmChooseCamera = new FrmChooseCamera(true);
            // frmChooseCamera.ShowDialog();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtnGoBackTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRepeatPositionAccuracy frmRepeatPositionAccuracy = new FrmRepeatPositionAccuracy();
            frmRepeatPositionAccuracy.ShowDialog();

            frmRepeatPositionAccuracy.Dispose();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnStaticAccuracy_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmChooseCamera frmChooseCamera = new FrmChooseCamera(true);
            frmChooseCamera.ShowDialog();

            frmChooseCamera.Dispose();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnZeroOffset_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmBackOffset frmBackOffset = new FrmBackOffset(CameraType.BondCamera);
            frmBackOffset.ShowDialog();

            frmBackOffset.Dispose();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnRepeatPosition_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRepeatPositionAccuracy frmRepeatPositionAccuracy = new FrmRepeatPositionAccuracy();
            frmRepeatPositionAccuracy.Show();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnPixelRatio_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmChooseCamera frmChooseCamera = new FrmChooseCamera(false);
            frmChooseCamera.ShowDialog();

            frmChooseCamera.Dispose();
        }

        /// <summary>
        /// 未知
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnRotateCenter_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRotateCenterAccuracy frmRotateCenterAccuracy = new FrmRotateCenterAccuracy();
            frmRotateCenterAccuracy.Show();
        }

        /// <summary>
        /// 自动标定开始
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().AllAxisGoHome();

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("系统1标定线程");
                            CalibrateTask.GetInstance().StartDispenseCalibTask();
                        });
            }
            else
            {
                Task.Run(() =>
                    {
                        CommonUtil.SetCurrentThreadName("系统2标定线程");
                        CalibrateTask.GetInstance().StartAutoCalib();
                    });
            }
        }

        /// <summary>
        /// 自动标定的示教
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnLearn_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool isUsePreviousPos = false;

            Machine.GetInstance().AllAxisGoHome();

            DialogResult dialog = AKRSXtraMessageBox.Show(
                "Question: \n" + "是否使用上一次标定示教点位？\r\n",
                "Prompt",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                isUsePreviousPos = true;
            }

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                FrmDispenseCalibTeach frmDispenseCalibrationTeach = new FrmDispenseCalibTeach(isUsePreviousPos);
                frmDispenseCalibrationTeach.Show();
            }
            else
            {
                FrmBondCalibTeach frmBondCalibrationTeach = new FrmBondCalibTeach(isUsePreviousPos);
                frmBondCalibrationTeach.Show();
            }

        }

        /// <summary>
        /// special calib
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtnStartUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmStartup frmStartup = new FrmStartup();
            frmStartup.Show();
        }

        /// <summary>
        /// bond to camera 关系标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtBCOffsetRelationCalib_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmCalibration frmCalibration = new FrmCalibration();
            frmCalibration.Show();
        }

        /// <summary>
        ///  轨道状态
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BarBtTransportSystemState_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmSetionState frmSetionState = new FrmSetionState();
            frmSetionState.Show();
        }

        private void barButtonItem4_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("未测试过请勿执行,请先关闭excel", "警告", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.No)
            {
                return;
            }

            // 移动到温漂测试点
            AKRSPoint3D point3D = new AKRSPoint3D(CalibrateRunPara.GetInstance().UpLookMarkMachinePos.X, CalibrateRunPara.GetInstance().UpLookMarkMachinePos.Y, CalibrateRunPara.GetInstance().UpLookMarkMachinePos.Z);

            BondModule bondModule = new BondModule();

            point3D = bondModule.ConvertMachineToG0Pos(point3D);

            MatchResult matchResult;
            matchResult = (MatchResult)System2Domain.GetInstance().System2Controller.BondCameraVision(point3D, "上视温漂测试Mark");

            if (matchResult != null)
            {
                FileHelper.AppendExcelData(
                    "D:" + "\\" + "温漂测试点数据" + ".xlsx",
                    new List<object>() { DateTime.Now.ToString("u"), matchResult.CenterX, matchResult.CenterY, });

                AKRSXtraMessageBox.Show("测试成功");

            }
            else
            {
                AKRSXtraMessageBox.Show("测试失败");
            }
        }

        /// <summary>
        /// 测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            //FrmTest frmTest = new FrmTest();
            //frmTest.Show();
        }

        /// <summary>
        /// PR列表
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtPRList_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmPRList frmPrList = new FrmPRList(System2CommonService.GetCurrentRecipePREntityNameList());
            frmPrList.ShowDialog();

            frmPrList.Dispose();
        }

        /// <summary>
        /// 测试列表
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            TransportUnit transportUnit = new TransportUnit("测试");

            transportUnit.SaveDataToExcel(1);
        }

        /// <summary>
        /// 焊点偏移设置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtProductionWindow_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmEntityOffsetChange frmEntityOffsetChange = new FrmEntityOffsetChange();
            frmEntityOffsetChange.ShowDialog();

            frmEntityOffsetChange.Dispose();
        }

        /// <summary>
        /// 报警历史查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAlarmHistoryQuery_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (FrmAlarmHistoryQuery.Instance.Visible)
            {
                FrmAlarmHistoryQuery.Instance.Hide();
            }
            else
            {
                FrmAlarmHistoryQuery.Instance.Show(this);
            }
        }

        /// <summary>
        /// 校准温漂点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtTemperatureMarkAdjust_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("是否重新示教温漂矫正点", "警告", MessageBoxButtons.YesNo);

            AKRSPoint3D point3D = null;

            if (dialogResult == DialogResult.Yes || CalibrateRunPara.GetInstance().UpLookMarkMachinePos == null)
            {
                TUAssistantHelper.EditPrInSystem2("上视温漂测试Mark");
            }
            else
            {
                // 移动到温漂测试点
                point3D = new AKRSPoint3D(CalibrateRunPara.GetInstance().UpLookMarkMachinePos.X, CalibrateRunPara.GetInstance().UpLookMarkMachinePos.Y, CalibrateRunPara.GetInstance().UpLookMarkMachinePos.Z);

                BondModule bondModule = new BondModule();

                point3D = bondModule.ConvertMachineToG0Pos(point3D);
            }
            
            MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                point3D,
                "温漂定位",
                "上视温漂测试Mark");

            if (driftMatchResult == null)
            {
                AKRSXtraMessageBox.Show("温度Mark定位失败失败,请重新设置模板");

                Machine.GetInstance().Stop();

                return;
            }

            AKRSPoint3D point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(driftMatchResult.CenterX, driftMatchResult.CenterY, 0));

            CalibrateRunPara.GetInstance().UpLookMarkMachinePos = System2Domain.GetInstance().BondModuleController.Get3DRealPosition() + point;

            // 另外一个点的Mark
            if (System2Configuration.GetInstance().DriftCompensateMarkNumber == 2)
            {
                if (dialogResult == DialogResult.Yes || CalibrateRunPara.GetInstance().UpLookMarkMachinePos2 == null)
                {
                    AKRSXtraMessageBox.Show("请继续示教对角温漂矫正点", "提示");

                    TUAssistantHelper.EditPrInSystem2("上视温漂测试Mark2");
                }
                else
                {
                    // 移动到温漂测试点
                    point3D = CalibrateRunPara.GetInstance().UpLookMarkMachinePos2;

                    BondModule bondModule = new BondModule();

                    point3D = bondModule.ConvertMachineToG0Pos(point3D);
                }
                

                MatchResult driftMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    point3D,
                    "温漂定位",
                    "上视温漂测试Mark2");

                if (driftMatchResult2 == null)
                {
                    AKRSXtraMessageBox.Show("温度Mark定位失败失败,请重新设置模板");

                    Machine.GetInstance().Stop();

                    return;
                }

                AKRSPoint3D point2 = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                    new AKRSPoint3D(driftMatchResult2.CenterX, driftMatchResult2.CenterY, 0));

                CalibrateRunPara.GetInstance().UpLookMarkMachinePos2 = System2Domain.GetInstance().BondModuleController.Get3DRealPosition() + point2;
            }

            CalibrateRunPara.GetInstance().Save();

            // Bond模组去安全位
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            AKRSXtraMessageBox.Show("温漂矫正成功");
        }

        /// <summary>
        /// 焊头模组重复定位精度实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtAxesAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmXYAccurayTest frmXYAccurayTest = new FrmXYAccurayTest();
            frmXYAccurayTest.Show();
        }

        /// <summary>
        /// 取片实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtPickUpAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmPickupRepeatTest frmPickupRepeatTest = new FrmPickupRepeatTest();
            frmPickupRepeatTest.ShowDialog();

            frmPickupRepeatTest.Dispose();
        }

        private void BarBtCalibAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            //FrmCalibTest frmCalibTest = new FrmCalibTest();
            //frmCalibTest.Show();
        }

        private void BarBtAlignAngleAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmAlignAngleAccuracyTest frmAlignAngleAccuracyTest = new FrmAlignAngleAccuracyTest();
            frmAlignAngleAccuracyTest.ShowDialog();
            frmAlignAngleAccuracyTest.Dispose();
        }

        private void BarBtPlaceAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void BarBtSimBondAccuracyTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmSimulateBondTest frmSimulateBondTest = new FrmSimulateBondTest();
            frmSimulateBondTest.ShowDialog();
            frmSimulateBondTest.Dispose();
        }

        /// <summary>
        /// 测试页面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtFunctionTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            AKRS.ZX2200.Experiment.TestFunction.FrmTest.ShowUi();
        }

        private void barBtnMoveLocateTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmMoveLocateTest frmMoveLocateTest = new();
            frmMoveLocateTest.Show();
        }

        /// <summary>
        /// 系统2点胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtS2Dispense_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowPanel(
                "UcS2DispenseManual",
                () =>
                    {
                        UcS2DispenseManual ucS2DispenseManual = new UcS2DispenseManual();
                        return ucS2DispenseManual;
                    });
        }

        /// <summary>
        /// 全局标定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtGlobalCalibration_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmGlobalCalibration frmGlobal = new FrmGlobalCalibration();
            frmGlobal.Show();
        }

        /// <summary>
        /// 固高力控实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtGTForceTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmGTForceTest1 frmGTForceTest = new FrmGTForceTest1();
            frmGTForceTest.Show();
        }

        private void BarBtnCalib2D_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmGlobalCalibrationEx form = new FrmGlobalCalibrationEx();
            form.Show();
        }

        /// <summary>
        /// 运行速度
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void TkbMachineSpeed_EditValueChanged(object sender, EventArgs e)
        {
            MachineSoftwareConfiguration.GetInstance().MachineMoveSpeedPercentage = this.TkbMachineSpeed.Value / 100.0;
        }

        /// <summary>
        /// 清除焊点补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearBpOffSet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (this.frmSingleBpData == null || this.frmSingleBpData.IsDisposed)
            {
                this.frmSingleBpData = new FrmSingleBpData();
                this.frmSingleBpData.Show();
            }
        }

        /// <summary>
        ///  文件清理工具
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtnFileCleanConfig_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmFileCleaner>();
        }

        /// <summary>
        /// 焊点补偿标定
        /// </summary>
        /// <param name="sender">事件原</param>
        /// <param name="e">封装参数</param>
        private void BarBondCompense_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmBondCompensateCalibration frmBondCompensateCalibration = new FrmBondCompensateCalibration();
            frmBondCompensateCalibration.Show();
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmTest2 frmTest = new FrmTest2();
            frmTest.Show();
        }

        /// <summary>
        /// 调试界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDug_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowFrmDebug();
        }


        /// <summary>
        /// 自动刮胶
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnAutoSlideFlux_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured == false)
            {
                AKRSXtraMessageBox.Show("请先配置刮胶盘！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Machine.GetInstance().CurrentRole != RoleEnum.Admin)
            {
                AKRSXtraMessageBox.Show("请先登录管理员权限！", "Warn", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FrmAutoSlideFlux frmAutoSlideFlux = new FrmAutoSlideFlux();
            frmAutoSlideFlux.ShowDialog();
            frmAutoSlideFlux.Dispose();
        }

        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmLightnessCalibTeach frmLightnessCalibTeach = new FrmLightnessCalibTeach();
            frmLightnessCalibTeach.Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmCompensateTest frmCompensateTest = new FrmCompensateTest();
            frmCompensateTest.ShowDialog();
        }

        /// <summary>
        /// 光源标定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void barBtnLightCalib_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmLightnessCalibTeach frmLightnessCalibTeach = new FrmLightnessCalibTeach();
            frmLightnessCalibTeach.Show();
        }

        /// <summary>
        /// 锁屏API
        /// </summary>
        /// <returns>无</returns>
        [DllImport("user32 ")]
        public static extern bool LockWorkStation();

        /// <summary>
        /// 锁屏
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLock_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                AKRSXtraMessageBox.Show("设备未处于暂停状态，无法锁屏");
                return;
            }

            LockWorkStation();
        }

        /// <summary>
        /// 移动到焊点查看情况
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMoveToBp_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                UcMainSystem.VmVisionShow();
                UcMainSystem.ChangeCameraVision("点胶相机");
                FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.BondPosition);
                frmModuleSelect.ShowDialog();
            }
            else
            {
                UcMainSystem.VmVisionShow();
                UcMainSystem.ChangeCameraVision("Bond相机");
                FrmModuleSelect frmModuleSelect = new FrmModuleSelect(EntityTypeEnum.BondPosition);
                frmModuleSelect.ShowDialog();
            }
        }

        /// <summary>
        /// 精度提升实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAccuracyUp_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmAccuracyExperiment>();
        }

        /// <summary>
        /// 查找最后一次固晶情况
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVisionLastBp_ItemClick(object sender, ItemClickEventArgs e)
        {
            MachineStateModel.GetInstance().VisionPreviousBondPositionSystem2();
        }

        /// <summary>
        /// 授权信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarAuthorization_ItemClick(object sender, ItemClickEventArgs e)
        {
            SoftkeyManager.CheckYtSoftKey();
        }

        /// <summary>
        /// 设备基础实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtBaseTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmBaseTest>();
        }

        /// <summary>
        /// 贴片补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBondCompensate_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmBondCompensate>();
        }

        /// <summary>
        /// 整机复位
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarBtInitialize_ItemClick(object sender, ItemClickEventArgs e)
        {
            Machine.GetInstance().AllAxisGoHome();
        }

        /// <summary>
        /// 速度提升实验
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarUpSpeed_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmSpeedImprovementExperiment>();
        }

        /// <summary>
        /// 实时焊后曲线
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtPostBondDataNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmPostBondData>();
        }

        private void barBtnWaferCameraCalib_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmWaferCameraCalibTeach frmWaferCameraCalibTeach = new FrmWaferCameraCalibTeach();
            frmWaferCameraCalibTeach.Show();
        }

        private void BtEditOppositeSex_ItemClick(object sender, ItemClickEventArgs e)
        {
            //this.ShowForm<FrmTransportUnitEdit>();
        }

        private void LueLanguage_EditValueChanged(object sender, EventArgs e)
        {
            LocalizationManager.ChangeLanguage((LanguageEnum)this.LueLanguage.EditValue);
            foreach (Form form in Application.OpenForms)
            {
                FormLocalizer.LocalizeForm(form);
                form.GetChildControls<XtraUserControl>().ForEach(a =>
                    {
                        FormLocalizer.LocalizeForm(a);
                    });
            }
        }

        /// <summary>
        /// 加热设置
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarHeatingSettings_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (TransportDomain.GetInstance().TransportController.BondSubSectionController.Module.BondMaxSubHeater == null
                && !MachineStateModel.GetInstance().IsOffLineWork)
            {
                AKRSXtraMessageBox.Show("加热器断开链接，请检查是否存在问题");
                return;
            }

            new FrmHeatingSettings().ShowDialog();
        }

        /// <summary>
        /// 取片位置矫正
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarPickUpMarkAdjust_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult dialogResult = AKRSXtraMessageBox.Show("是否修改校准取片Mark点模板", "警告", MessageBoxButtons.YesNo);

            AKRSPoint3D point3D = BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos1;

            if (dialogResult == DialogResult.Yes || point3D == null)
            {
                TUAssistantHelper.EditPrInSystem2("取片参考点Mark1", CameraTypeEnum.BondCamera, AlgFlowTypeEnum.XldModelAlg, AlgBeLongEnum.Calibration);
                point3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
            }
         
            MatchResult driftMatchResult = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                point3D,
                "取片参考点Mark1",
                "取片参考点Mark1");

            if (driftMatchResult == null)
            {
                AKRSXtraMessageBox.Show("取片Mark定位失败失败,请重新设置模板");

                Machine.GetInstance().Stop();

                return;
            }

            AKRSPoint3D point = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(driftMatchResult.CenterX, driftMatchResult.CenterY, 0));

            BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos1 = System2Domain.GetInstance().BondModuleController.GetG0RealPosition() + point;

            // 另外一个点的Mark
            if (System2Configuration.GetInstance().PickUpCompensateMarkNumber == 2)
            {
                point3D = BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos2;

                if (dialogResult == DialogResult.Yes || BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos2 == null)
                {
                    AKRSXtraMessageBox.Show("请继续编辑Mark2", "提示");

                    TUAssistantHelper.EditPrInSystem2("取片参考点Mark2", CameraTypeEnum.BondCamera, AlgFlowTypeEnum.XldModelAlg, AlgBeLongEnum.Calibration);
                    point3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
                }

                MatchResult driftMatchResult2 = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                    point3D,
                    "取片参考点Mark2",
                    "取片参考点Mark2");

                if (driftMatchResult2 == null)
                {
                    AKRSXtraMessageBox.Show("取片Mark定位失败失败,请重新设置模板");

                    Machine.GetInstance().Stop();

                    return;
                }

                AKRSPoint3D point2 = System2Module.GetInstance().BondModule.BondCameraCoordinateSystem.ForwardConvertCoordinate(
                new AKRSPoint3D(driftMatchResult2.CenterX, driftMatchResult2.CenterY, 0));

                BondDevicePara.GetInstance().BondHeadParam.PickMarkVisionPos2 = System2Domain.GetInstance().BondModuleController.GetG0RealPosition() + point2;
            }

            BondDevicePara.GetInstance().Save();

            // Bond模组去安全位
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();

            AKRSXtraMessageBox.Show("取片位置矫正成功");
        }


        private void barBtnSystem2Cali_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// 系统2三点一线校准
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void barBtnThreePointOneLineCali_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmSystem2Calibrate frmSystem2Calibrate = new FrmSystem2Calibrate();
            frmSystem2Calibrate.ShowDialog();
            frmSystem2Calibrate.Dispose();
        }

        /// <summary>
        /// 示教标定尺
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarAssistantRuler_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmRealTimeAssistant frmRealTimeAssistant = new FrmRealTimeAssistant();
            frmRealTimeAssistant.ShowDialog();
            frmRealTimeAssistant.Dispose();
        }

        /// <summary>
        /// 温度监控
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BarTemperatureMonitoring_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmTemperatureMonitoring>();
        }

        /// <summary>
        /// 软件信息
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSoftWareInfo_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmVersionInfo>();
        }

        /// <summary>
        /// 实验解锁
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtUnLock_ItemClick(object sender, ItemClickEventArgs e)
        {
            string message = XtraInputBox.Show("非专业人员不要开启", "密码", "");

            if (message == "123456")
            {
                this.ribbonPageGroup9.Enabled = true;
            }
            else
            {
                this.ribbonPageGroup9.Enabled = false;
            }
        }

        /// <summary>
        /// 电脑锁屏
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void barButtonItem8_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                AKRSXtraMessageBox.Show("设备未处于暂停状态，无法锁屏");
                return;
            }

            LockWorkStation();
        }

        /// <summary>
        /// 检测结果图
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVisionResult_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmDefectResult>();
        }

        /// <summary>
        /// 查看最后一次点胶的视觉结果
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVisionLastDispense_ItemClick(object sender, ItemClickEventArgs e)
        {
            MachineStateModel.GetInstance().VisionPreviousBondPositionSystem1();
        }

        /// <summary>
        ///  设备功能测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnMachineFunctionTest_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ShowForm<FrmMachineFunctionTest>();
        }
    }
}



