namespace AKRS.ZX2200.Main.Controls
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.EventBus;
    using AKRS.ZX2200.Infrastructure.Service;
    using AKRS.ZX2200.Main.Controls.Ucmain;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.ObjParameter;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using DevExpress.XtraBars.Docking2010.Views;
    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
    using DevExpress.XtraEditors;
    using DevExpress.XtraSplashScreen;
    using BaseControl = AKRS.ZX2200.Infrastructure.Controls.Currency.BaseControl;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : XtraForm
    {
        /// <summary>
        /// 当前Document
        /// </summary>
        private object current;

        /// <summary>
        /// 流道程式
        /// </summary>
        public TransportProgram TransportProgram => TransportProgram.GetInstance();

        /// <summary>
        /// 获取 ucMainSystem对象
        /// </summary>
        public UcMainSystem UcMainSystem => (UcMainSystem)this.UcMainSystemDocument.Control;

        /// <summary>
        /// UI线程执行的委托
        /// </summary>
        public static Action<Action> SendUiAction;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            IPlatformProvider platform = PlatformProvider.Current;

            SendUiAction += this.PostAction;
        }

        /// <summary>
        /// 执行UI线程的委托
        /// </summary>
        /// <param name="action">委托</param>
        private void PostAction(Action action)
        {
            this.Invoke(action);
        }

        /// <summary>
        /// 根据点击的按钮像是对应的UserControl
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void WindowsUIView_QueryControl(object sender, QueryControlEventArgs e)
        {
            BaseControl module = e.Document.Tag is BaseControl
                                     ? (BaseControl)e.Document.Tag
                                     : Activator.CreateInstance(
                                           typeof(MainForm).Assembly.GetType(e.Document.ControlTypeName)) as BaseControl;

            module?.InitModule(this.barManager1, this.windowsUIView);
            module.StartGroup = this.startGroup;

            if (this.windowsUIView.Tiles.TryGetValue(e.Document, out BaseTile tile))
            {
                TileItemFrame frame = tile.CurrentFrame;
                object data = this.current ?? frame?.Tag;
                //module?.ShowModule(data);
            }

            e.Document.Tag = module;
            e.Control = module;
        }

        /// <summary>
        ///  关闭
        /// </summary>
        /// <param name="e">参数封装</param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            FlyoutAction closeAction = this.CreateCloseAction();
            this.flyout.Action = closeAction;

            e.Cancel = this.windowsUIView.ShowFlyoutDialog(this.flyout) != System.Windows.Forms.DialogResult.Yes;
        }

        /// <summary>
        /// 创建退出弹出框
        /// </summary>
        /// <returns>FlyoutAction</returns>
        private FlyoutAction CreateCloseAction()
        {
            FlyoutAction closeAction = new FlyoutAction();
            closeAction.Caption = this.Text;
            closeAction.Description = "确定要退出软件 ?";
            closeAction.Commands.Add(FlyoutCommand.Yes);
            closeAction.Commands.Add(FlyoutCommand.No);
            return closeAction;
        }

        /// <summary>
        /// 取消自带的ActionBar
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void WindowsUIView_NavigationBarsShowing(object sender, NavigationBarsCancelEventArgs e)
        {
            e.Cancel = true;
        }

        /// <summary>
        /// 退出按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void StartGroup_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Button.Properties.Caption))
            {
                this.Close();
            }
        }

        /// <summary>
        /// 主系统 退出按钮点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void PageGroup_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.Button.Properties.Caption))
            {

            }
        }

        /// <summary>
        /// 硬件编
        /// </summary>
        private UcHardwareEditor ucHardwareEditor = null;

        /// <summary>
        /// Tile 点击
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void windowsUIView_TileClick(object sender, TileClickEventArgs e)
        {
            if (this.ucHardwareEditor != null)
            {
                this.ucHardwareEditor.DisposeDebugControl();
                this.ucHardwareEditor.StopRefreshState();
            }

            Tile tile = e.Tile as Tile;

            if (tile?.Document == null)
            {
                return;
            }

            if (tile.Document.Control is BaseControl module)
            {
                if (module is UcHardwareEditor)
                {
                    this.ucHardwareEditor = module as UcHardwareEditor;
                    this.ucHardwareEditor.StartRefreshState();
                }
                else
                {

                }

                TileItemFrame frame = tile.CurrentFrame;
                object data = frame?.Tag;
                // module.ShowModule(data);
            }
        }

        /// <summary>
        /// 更改数据
        /// </summary>
        private void ChangeData()
        {
            CarrierConfigRepository.GetInstance().BaseDsSettingList.FindAll(a => a.CarrierType == CarrierTypeEnum.Wafer).ForEach(
                b =>
                {
                    if (b is CarrierWithWaferConfig c)
                    {
                        //CarrierWithWaferConfig c = (CarrierWithWaferConfig)b;
                        if (c.EjectionTableWorkPosition.Z == 0 & c.EjectionName != string.Empty & c.EjectionName != null & EjectionConfigRepository.GetInstance().IsExists(c.EjectionName))
                        {
                            c.EjectionTableWorkPosition = ((EjectionConfig)EjectionConfigRepository.GetInstance().Find(c.EjectionName)).EjectionTableWorkPosition;
                        }
                    }
                });

            CarrierConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// Load 事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (!Machine.GetInstance().AllAxisReset && !MachineStateModel.GetInstance().IsOffLineWork)
            {
                this.UcMainSystemTile.Enabled = false;
                return;
            }

            SplashScreenManager.ShowForm(typeof(SplashScreen1));
            SplashScreenManager.Default.SendCommand(SplashScreen1.SplashScreenCommand.InitView, "程序加载中");

            // 数据加载
            Machine.GetInstance().DataLoad();

            this.ChangeData();

            // 参数初始化
            UcProgramArgs.InitParamerer = ParameterManage.InitControl;

            // 连接压力表
            ModbusService.GetInstance().ConnectBondhead();
            ModbusService.GetInstance().ConnectManometer();

            Machine.GetInstance().ChoseAllLight();

            Machine.GetInstance().StartBackGroundThread();

            ExportService.EnsureDirectoryExists(@"D:\\设备功能测试");

            try
            {
                MF = (MainForm)Application.OpenForms[0];
            }
            catch (Exception ex) 
            {
                MF = (MainForm)Application.OpenForms[1];
            }

            SplashScreenManager.CloseForm();
        }

        public static MainForm MF { get; private set; }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Machine.GetInstance().IsStop())
            {
                e.Cancel = true;
                AKRSXtraMessageBox.Show("请先停止设备");
                return;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Machine.GetInstance().ExitSoftware();
        }
    }
}