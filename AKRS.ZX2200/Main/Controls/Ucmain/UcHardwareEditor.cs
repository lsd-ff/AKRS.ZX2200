namespace AKRS.ZX2200.Main.Controls.Ucmain
{
    using System.Windows.Forms;

    using AKRS.Galaxy2.HardwareEditor.Controls.DebugControls;
    using AKRS.Galaxy2.HardwareEditor.Controls.Interfaces;
    using AKRS.Galaxy2.HardwareEditor.Services;
    using AKRS.Galaxy2.LogicHardware.Hardwares;
    using AKRS.Galaxy2.LogicHardware.Models;
    using AKRS.Galaxy2.LogicHardware.Repository;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    using DevExpress.Utils;
    using DevExpress.XtraBars;

    /// <summary>
    /// 硬件编辑控件
    /// </summary>
    public partial class UcHardwareEditor : BaseControl
    {
        /// <summary>
        /// 调试控件
        /// </summary>
        private UcGroupDebug ucGroupDebug;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcHardwareEditor()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.ucGroupDebug = new UcGroupDebug() { Dock = DockStyle.Fill };
            this.ucGroupDebug.HardwareChangeEvent = this.SelectHardware;
            this.SpcHardwareEditor.Panel1.Controls.Add(this.ucGroupDebug);
        }

        /// <summary>
        /// 开启刷新线程
        /// </summary>
        public void StartRefreshState()
        {
            this.ucGroupDebug.StartRefreshCycleState();
        }

        /// <summary>
        /// 关闭刷新线程
        /// </summary>
        public void StopRefreshState()
        {
            this.ucGroupDebug.StopRefreshCycleState();
        }

        private IDebugUI debugControl = null;

        /// <summary>
        /// 释放
        /// </summary>
        public void DisposeDebugControl()
        {
            if (this.debugControl != null) 
            {
                this.debugControl.DisposeView();
                this.SpcHardwareEditor.Panel2.Controls.Clear();
            }
        }

        /// <summary>
        /// 硬件调试选择硬件
        /// </summary>
        /// <param name="hardware">硬件信息</param>
        private void SelectHardware(HardwareDto hardware)
        {
            this.debugControl?.DisposeView();

            Control view = ViewService.GetDebugView(hardware);

            if (!(view is IDebugUI debugControl))
            {
                return;
            }
            else
            {
                this.debugControl = view as IDebugUI;
            }

            debugControl.LoadView(HardwareRepositoryService.GetHardware<HardwareBase>(hardware.HardwareName));
            view.Dock = DockStyle.Fill;
            this.SpcHardwareEditor.Panel2.Controls.Clear();
            this.SpcHardwareEditor.Panel2.Controls.Add(view);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            HardwareRepositoryService.SaveHardwares();
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
    }
}