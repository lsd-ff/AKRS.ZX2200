namespace AKRS.ZX2200.Main.Controls
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls;
    using AKRS.ZX2200.BondSystem.Controls.Experiment;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controls;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach;
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Docking;
    using DevExpress.XtraEditors;
    using System;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;
    using BaseControl = AKRS.ZX2200.Infrastructure.Controls.Currency.BaseControl;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 模块
    /// </summary>
    public partial class UcModule : BaseControl
    {
        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 测高控制器
        /// </summary>
        private DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 点胶控制器
        /// </summary>
        private DispenseController dispenseController = new DispenseController();

        /// <summary>
        /// 模块
        /// </summary>
        public UcModule()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 基板流道右键菜单Click事件
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">事件</param>
        private void SubstrateRunner_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 设置
            if (e.Item.Id == 0)
            {
                //if (this.ucSubstrateRunnerSetPara == null || this.ucSubstrateRunnerSetPara.IsDisposed)
                //{
                //    this.frmPRList = new FrmPRList();
                //    this.frmPRList.Show();
                //}
                //else
                //{
                //    this.frmPRList.Show();
                //}

                //if (this.ucSubstrateRunnerSetPara == null || this.ucSubstrateRunnerSetPara.IsDisposed)
                //{
                //    this.ucSubstrateRunnerSetPara = new UcSubstrateRunnerSetPara();
                //    Machine.GetInstance().Show(this.ucSubstrateRunnerSetPara, "参数设置");
                //}
                //else
                //{
                //    this.ucSubstrateRunnerSetPara.ParentForm.WindowState = FormWindowState.Normal;
                //}
            }
        }

        /// <summary>
        /// 上料器右键菜单Click事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void Feed_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

        }

        private FrmDebug frmDebug;

        private FrmChangeStaticAdapterTeach frmChangeStaticAdapterTeach;

        /// <summary>
        /// 晶圆上料子系统右键菜单Click事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void WaferFeed_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
            //return;

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                // 程式设置
                if (e.Item.Name == "BtnDebug")
                {
                    if (this.frmDebug == null || this.frmDebug.IsDisposed)
                    {
                        this.frmDebug = new FrmDebug();
                    }

                    this.frmDebug.Show();
                }
            }

            if (e.Item.Name == "BtnChangeStaticWaffle")
            {
                if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
                {
                    if (this.frmChangeStaticAdapterTeach == null || this.frmChangeStaticAdapterTeach.IsDisposed)
                    {
                        this.frmChangeStaticAdapterTeach = new FrmChangeStaticAdapterTeach();
                    }

                    this.frmChangeStaticAdapterTeach.Show();
                }
                else
                {
                    AKRSXtraMessageBox.Show("未开启静态华夫盒功能！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /// <summary>
        /// 点胶右击菜单选择
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtDispense_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Caption == "Move to safe pos")
            {
                // 移动到安全位
                this.dispenseController.MoveToSafePos();
            }

            //ProcessStepEdit processStep = new ProcessStepEdit();
            //Machine.GetInstance().Show(processStep, "");

            ////DispenseOverAllSettings DispenseOverAllSettings = new DispenseOverAllSettings() { Dock = DockStyle.Fill };
            ////Machine.GetInstance().Show(DispenseOverAllSettings, "画胶设置");

            //DispenseDomain.GetInstance().Save();
            //MachineCoordinateSystem.GetInstance().Save();
            //MachineCoordinateSystem.GetInstance().BondCoordinateSystem.Degree = Math.Atan2(200, 300);
            ////MachineCoordinateSystem.GetInstance().BondCoordinateSystem
            ////    .ForwardConvertCoordinate(new AKRSPoint3D(100, 100, 100));
            //MachineCoordinateSystem.GetInstance().BondCoordinateSystem.Init(new AKRSPoint3D(0,0,0),new AKRSPoint3D(20,10,1));
            //MachineCoordinateSystem.GetInstance().BondCoordinateSystem
            //    .BackConvertCoordinate(new AKRSPoint3D(600, 200, 100));
            //MachineCoordinateSystem.GetInstance().BondCameraCoordinateSystem.OriginalParameter = new List<AKRSPoint3D>()
            //    {
            //        new AKRSPoint3D(1, 1,1),
            //        new AKRSPoint3D(2, 2, 2),
            //        new AKRSPoint3D(3, 3, 3),
            //        new AKRSPoint3D(4, 4, 4),
            //        new AKRSPoint3D(5, 5, 5),
            //        new AKRSPoint3D(6, 6, 6),
            //        new AKRSPoint3D(7, 7, 7),
            //        new AKRSPoint3D(8, 8, 8),
            //        new AKRSPoint3D(9, 9, 9),
            //    };
            //MachineCoordinateSystem.GetInstance().BondCameraCoordinateSystem.TargetParameter = new List<AKRSPoint3D>()
            //    {
            //        new AKRSPoint3D(1, 1,1),
            //        new AKRSPoint3D(2, 2, 2),
            //        new AKRSPoint3D(3, 3, 3),
            //        new AKRSPoint3D(4, 4, 4),
            //        new AKRSPoint3D(5, 5, 5),
            //        new AKRSPoint3D(6, 6, 6),
            //        new AKRSPoint3D(7, 7, 7),
            //        new AKRSPoint3D(8, 8, 8),
            //        new AKRSPoint3D(9, 9, 9),
            //    };
            //MachineCoordinateSystem.GetInstance().BondCameraCoordinateSystem.Init();
            //IMVSNPointCalibModuTool imvsnPointCalibModuTool =
            //    (IMVSNPointCalibModuTool)VmSolution.Instance["流程1.N点标定1"];

            //imvsnPointCalibModuTool.ModuParams.CalibPathName = "kkkk";
            //imvsnPointCalibModuTool.ModuParams.ImagePoint = new List<PointF>()
            //                                                    {
            //                                                        new PointF(0, 0), 
            //                                                        new PointF(1, 1),
            //                                                        new PointF(2, 2),
            //                                                        new PointF(3, 3),
            //                                                        new PointF(4, 4),
            //                                                        new PointF(5, 5),
            //                                                        new PointF(6, 6),
            //                                                        new PointF(7, 7),
            //                                                        new PointF(8, 8),
            //                                                        new PointF(9, 9),
            //                                                    };
            //imvsnPointCalibModuTool.ModuParams.PhysicalPoint = new List<PointF>()
            //                                                       {
            //                                                           new PointF(1, 1),
            //                                                           new PointF(2, 2),
            //                                                           new PointF(3, 3),
            //                                                           new PointF(4, 4),
            //                                                           new PointF(5, 5),
            //                                                           new PointF(6, 6),
            //                                                           new PointF(7, 7),
            //                                                           new PointF(8, 8),
            //                                                           new PointF(9, 9),
            //                                                           new PointF(10, 10),
            //                                                       };
            //imvsnPointCalibModuTool.Run();


        }



        /// <summary>
        /// 固精系统右键菜单Click事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void Bond_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (e.Item.Caption == "移动到安全位")
            {
                // 移动到安全位
                this.bondModuleController.MoveToSafePos();
            }
            else if (e.Item.Caption == "移动到上视")
            {
                // 移动到上视
                this.bondModuleController.MoveToUpLookPos();
            }
            else if (e.Item.Caption == "移动到抛料位")
            {
                // 移动到抛料
                this.bondModuleController.MoveToThrowPos();
            }
            else if (e.Item.Caption == "抛料")
            {
                // 抛料
                this.bondModuleController.ThrowAction();
            }
            else if (e.Item.Caption == "移动到换吸嘴安全位")
            {
                // 换吸嘴安全位
                this.bondModuleController.MoveToChangeNozzleSafePos();
            }
            else if(e.Item.Caption == "清理吸嘴")
            {
                // 清理吸嘴
                this.bondHeadController.ClearHead();
            }
            else if (e.Item.Caption == "连接焊头压力表")
            {
                // 连接焊头压力表
                bool res = ModbusService.GetInstance().ConnectBondhead();

                if (res = true)
                {
                    AKRSXtraMessageBox.Show(
       "焊头压力表连接成功!",
       "Prompt",
       MessageBoxButtons.OK,
       MessageBoxIcon.Information);
                }
            }
            else if (e.Item.Caption == "焊头压力表清零")
            {
                this.bondHeadController.ResetBondhead();

                double readValue = this.system2Controller.GetBondheadForceValue();

                // 判断清零是否成功
                if (Math.Abs(readValue) > 3)
                {
                    AKRSXtraMessageBox.Show(
                        "焊头压力表清零失败!",
                        "Prompt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    AKRSXtraMessageBox.Show(
                        "焊头压力表清零成功!",
                        "Prompt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else if (e.Item.Caption == "连接校正台压力表")
            {
                // 连接焊头压力表
                bool res = ModbusService.GetInstance().ConnectManometer();

                if (res = true)
                {
                    AKRSXtraMessageBox.Show(
                        "校正台压力表连接成功!",
                        "Prompt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else if (e.Item.Caption == "校正台压力表清零")
            {
                // 校正台压力表清零
               ModbusService.GetInstance().ResetManometer();

                AKRSXtraMessageBox.Show(
                        "校正台压力表清零成功!",
                        "Prompt",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 人工更换料片
        /// </summary>
        /// <returns>是否更换成功</returns>
        public bool ChangeTabletManual()
        {
            try
            {
                //if (WaferTableModule.GetInstance().WaferClampAtSafePosition() && MagazineBoxModule.GetInstance().MagazineAtSafePosition())
                //{
                //    // 晶圆台到换料位
                //    WaferTableModule.GetInstance().MoveXY(WaferTableModule.GetInstance().ChangePosition);

                //    // 气缸落下
                //    Machine.GetInstance().ActionCycToTar(WaferTableModule.GetInstance().BlockCyc, WaferTableModule.GetInstance().BlockCycTarSensor);

                //    // 扩晶到压膜位
                //    WaferTableModule.GetInstance().MoveZ(WaferTableModule.GetInstance().ExpandDownPosition);

                //    XtraMessageBox.Show($"晶圆台已到达换料位，请手动更换！");                    
                //    return true;
                //}

                //XtraMessageBox.Show($"晶圆夹或Magazine电机不在安全位置！");
                
                return false;
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show($"晶圆台到达换料位失败！");                
                return false;
            }
        }


        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            FrmPRList frmPRList = new FrmPRList();
        }

        /// <summary>
        /// 图片双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PeBondModule_DoubleClick(object sender, EventArgs e)
        {
            //this.ShowPanel(
            //    "ucEditProduct",
            //    () =>
            //        {
            //            UcBondModuelSetting ucBondModuelSetting = new UcBondModuelSetting();
            //            return ucBondModuelSetting;
            //        });
        }

        /// <summary>
        /// 显示DockPanel
        /// todo:要不要放到公共类里面？
        /// </summary>
        /// <param name="controlName">组件名称</param>
        /// <param name="createUserControlAction">DockPanel 里包含的组件</param>
        /// <param name="dockingStyle">显示方式</param>
        public void ShowPanel(string controlName, Func<XtraUserControl> createUserControlAction, DockingStyle dockingStyle = DockingStyle.Float)
        {
            DockPanel dockPanel = this.GetDockPanel(controlName);

            if (dockPanel != null)
            {
                //dockPanel.Dock = dockingStyle;

                //Form form = dockPanel.ParentForm;

                //if (form != null)
                //{
                //    form.TopMost = true;
                //    form.Show();
                //    form.WindowState = FormWindowState.Maximized;
                //    form.TopMost = false;
                //}

                //dockPanel.BringToFront();
            }
            else
            {
                XtraUserControl userControl = createUserControlAction();
                Size size = userControl.Size;
                dockPanel = this.dockManager.AddPanel(userControl, new Point(220, 70), userControl.Text);
                dockPanel.Text = userControl.Text;
                dockPanel.Dock = dockingStyle;
                dockPanel.Size = size;
            }
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
        /// 图片双击事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PeDispenseModule_DoubleClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Panel面板点击事件 显示快捷菜单
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void PnlModule_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsSingleStepWork == false) 
            {
                this.ShowPanel(
                   "ucShortcutMenu",
                   () =>
                   {
                       UcShortcutMenu ucShortcutMenu = new UcShortcutMenu();
                       return ucShortcutMenu;
                   },
                   DockingStyle.Right);
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

        private void GpBondModule_DoubleClick(object sender, EventArgs e)
        {
            if (Machine.GetInstance().CurrentRole == RoleEnum.Admin)
            {
                UcBondModuelSetting ucBondModuelSetting = new UcBondModuelSetting() { Dock = DockStyle.Fill };
                FrmBear FrmBear = new FrmBear();
                FrmBear.Size = ucBondModuelSetting.Size;
                FrmBear.Controls.Add(ucBondModuelSetting);
                FrmBear.Show();
            }
        }
    }
}
