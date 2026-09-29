using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Main.Controls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using NozzleShelf = AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf.NozzleShelf;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    /// <summary>
    /// 吸嘴架调试窗体
    /// </summary>
    public partial class FrmNozzleBankManual : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNozzleBankManual()
        {
            this.InitializeComponent();
            this.RefreshControl();
        }

        /// <summary>
        /// 方向盘
        /// </summary>
        private FrmBear frmBear;

        /// <summary>
        /// 当前使用的吸嘴架
        /// </summary>
        private NozzleShelf NozzleShelf =>
            System2Domain.GetInstance().BondProgram.NozzleShelfProgram.NozzleShelf ?? new NozzleShelf();

        /// <summary>
        /// 吸嘴架程式
        /// </summary>
        private NozzleShelfProgram nozzleShelfProgram => System2Domain.GetInstance().BondProgram.NozzleShelfProgram;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 还吸嘴线程
        /// </summary>
        private Task putbackNozzleTask;

        /// <summary>
        /// 取吸嘴线程
        /// </summary>
        private Task takeNozzleTask;

        /// <summary>
        /// 吸嘴架Y轴伸缩线程
        /// </summary>
        private Task toolBankMaintenanceTask;

        /// <summary>
        /// 取指定吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFetchNozzle_Click(object sender, EventArgs e)
        {
            // 吸嘴架当前选中的槽
            NozzleShelfSlot toolBankSlot = (NozzleShelfSlot)this.GvNozzleBank.GetFocusedRow();

            // 界面防呆
            this.BtnRemoveNozzle.Enabled = false;
            this.BtnFetchNozzle.Enabled = false;
            this.BtnMaintenance.Enabled = false;

            // 防止线程多次启动
            if (this.takeNozzleTask == null || this.takeNozzleTask.IsCompleted == true)
            {
                this.takeNozzleTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("取吸嘴调试线程");

                            // 如果和当前吸嘴是同一个就直接返回
                            if (this.bondHeadController.GetCurrentNozzleName() == toolBankSlot.NozzleName)
                            {
                                return;
                            }

                            // 取吸嘴
                            bool ret = this.system2Controller.ChangeNozzle(toolBankSlot.NozzleName);

                            if (ret == true)
                            {
                                this.bondModuleController.MoveToSafePos();
                            }

                            this.BeginInvoke(new Action(this.RefreshControl));
                        });
            }
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            this.GcNozzleBank.DataSource = this.NozzleShelf.NozzleShelfSlots;
            this.GcNozzleBank.Refresh();
            this.GvNozzleBank.RefreshData();

            // 吸嘴架当前选中的槽
            NozzleShelfSlot toolBankSlot =
                ((NozzleShelfSlot)this.GvNozzleBank.GetFocusedRow()) ?? new NozzleShelfSlot();

            // 当前焊头上的吸嘴名
            string curNozzleName = this.bondHeadController.GetCurrentNozzleName();

            this.LbSelectedNozzle.Text = toolBankSlot.NozzleName;
            this.LbCurNozzle.Text = curNozzleName;
            this.LbCurNozzle2.Text = curNozzleName;

            // 界面防呆
            this.BtnDetails.Enabled = !string.IsNullOrEmpty(curNozzleName);
            this.BtnRemoveNozzle.Enabled = !string.IsNullOrEmpty(curNozzleName);
            this.BtnFetchNozzle.Enabled = !string.IsNullOrEmpty(toolBankSlot.NozzleName);
            this.BtnMaintenance.ButtonStyle = BorderStyles.Flat;
            this.BtnMaintenance.Appearance.Options.UseBackColor = true;
            this.BtnMaintenance.Enabled = true;

            this.Size = new System.Drawing.Size() { Width = 520, Height = 653 };
            // this.panelControl1.Size = new System.Drawing.Size() { Width = 520, Height = 653 };

            this.panelControl1.Dock = DockStyle.Fill;
            this.gCBondHead.Size = new System.Drawing.Size() { Width = 0, Height = 653 };

            // 保存
            NozzleShelfRepository.GetInstance().Save();

            // 设置权限
            if (Machine.GetInstance().CurrentRole == RoleEnum.Admin)
            {
                this.GvNozzleBank.Columns[1].OptionsColumn.AllowEdit = true;
            }
            else
            {
                this.GvNozzleBank.Columns[1].OptionsColumn.AllowEdit = false;
            }
        }

        /// <summary>
        /// 归还当前焊头上的吸嘴
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRemoveNozzle_Click(object sender, EventArgs e)
        {
            // 界面防呆
            this.BtnRemoveNozzle.Enabled = false;
            this.BtnFetchNozzle.Enabled = false;
            this.BtnMaintenance.Enabled = false;

            // 防止线程多次启动
            if (this.putbackNozzleTask == null || this.putbackNozzleTask.IsCompleted == true)
            {
                this.putbackNozzleTask = Task.Run(
                    () =>
                    {
                        CommonUtil.SetCurrentThreadName("还吸嘴调试线程");

                        // 放回当前吸嘴
                        this.system2Controller.PutbackNozzle();

                        this.nozzleShelfController.MoveShelfToHome();

                        this.bondModuleController.MoveToChangeNozzleSafePos();

                        // Z轴去待机位
                        this.bondHeadController.MoveBondZToSafePos();

                        this.BeginInvoke(new Action(
                            () =>
                                {
                                    this.RefreshControl();
                                }));
                    });
            }
        }

        /// <summary>
        /// 吸嘴架调试按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMaintenance_Click(object sender, EventArgs e)
        {
            // 界面防呆
            this.BtnRemoveNozzle.Enabled = false;
            this.BtnFetchNozzle.Enabled = false;
            this.BtnMaintenance.Enabled = false;

            // 防止线程多次启动
            if (this.toolBankMaintenanceTask == null || this.toolBankMaintenanceTask.IsCompleted == true)
            {
                ExcuteResult res;
                this.toolBankMaintenanceTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("吸嘴架伸出缩回线程");

                            double pos = this.nozzleShelfController.GetYAxisPos();
                            if (pos >= -1.5)
                            {
                                // 防撞
                                if (!system2Controller.IsNozzleShelfAxisYSafe())
                                {
                                    this.bondModuleController.MoveToSafePos();
                                }

                                // 吸嘴架Y到换吸嘴位
                                res = System2Domain.GetInstance().NozzleShelfController.MoveShelfToChangeNozzlePos();

                                if (res != ExcuteResult.Success)
                                {
                                    AKRSXtraMessageBox.Show("吸嘴架伸出失败，请检查吸嘴架Y轴！","错误");
                                }

                                this.BeginInvoke(new Action(
                                   () =>
                                   {
                                       this.BtnMaintenance.Appearance.BackColor = Color.Yellow;
                                   }));
                            }
                            else
                            {
                                // 吸嘴架Y回到原位
                              ExcuteResult res=  System2Module.GetInstance().NozzleShelfModule.MoveShelfToHome();

                                if (res != ExcuteResult.Success)
                                {
                                    AKRSXtraMessageBox.Show("吸嘴架缩回失败，请检查吸嘴架Y轴！","错误");
                                }

                                this.BeginInvoke(new Action(
                                   () =>
                                   {
                                       this.BtnMaintenance.Appearance.BackColor = Color.Transparent;
                                   }));

                            }

                            this.BeginInvoke(new Action(
                                () =>
                                    {
                                        this.RefreshControl();
                                    }));
                         });
            }
        }

        /// <summary>
        /// 吸嘴信息展示按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDetails_Click(object sender, EventArgs e)
        {
            //if (this.gCBondHead.Size.Width == 0)
            //{
            //    this.Size = new System.Drawing.Size() { Width = 920, Height = 653 };
            //    this.panelControl1.Size = new System.Drawing.Size() { Width = 500, Height = 653 };
            //    this.gCBondHead.Size = new System.Drawing.Size() { Width = 413, Height = 653 };

            //    // 清空文本
            //    this.MeCurNozzleMes.Text = string.Empty;

            //    // 当前焊头上的吸嘴名
            //    string curNozzleName = this.bondHeadController.GetCurrentNozzleName();

            //    // 当前吸嘴对象
            //    Nozzle nozzle = NozzleRepository.GetInstance().GetNozzle(curNozzleName);

            //    //Models.Repositories.Nozzle.Nozzle nozzle = new Models.Repositories.Nozzle.Nozzle()
            //    //{
            //    //    PREntityName = "1234",
            //    //    NozzleOffset = new AKRSPoint2D()
            //    //    {
            //    //        X = 0.11,
            //    //        Y = 0.22
            //    //    }
            //    //};

            //    // 获取属性值
            //    this.MeCurNozzleMes.Text = nozzle.ToString();
            //}
            //else
            //{
            //    //this.panelControl1.Size = new System.Drawing.Size() { Width = 500, Height = 743 };
            //    //this.gCBondHead.Size = new System.Drawing.Size() { Width = 0, Height = 743 };
            //    //this.Size = new System.Drawing.Size() { Width = 520, Height = 743 };
            //}

            if (this.frmBear == null || this.frmBear.IsDisposed == true)
            {
                this.frmBear = new FrmBear();
                // 弹出方向盘
                ucGuideMove = new UcGuideMove("FrmNozzleBankManual") { Dock = DockStyle.Fill };

                frmBear.Size = ucGuideMove.Size;
                frmBear.Controls.Add(ucGuideMove);
                frmBear.Show();
            }
            else
            {
                frmBear.Show();
            }
        }

        /// <summary>
        /// 选中行改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvNozzleBank_FocusedRowChanged_1(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            this.RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.nozzleShelfProgram.SelfCheck();
            string nozzleName = this.nozzleShelfProgram.GetToolOnBondhead();
            this.bondHeadController.SetCurrentNozzleName(nozzleName);
            this.RefreshControl();
            BondDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsSingleStepWork)
            {
                this.LbSingleStep.ForeColor = Color.Yellow;
            }
            else
            {
                this.LbSingleStep.ForeColor = Color.Gray;
            }
        }
       
        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void UcNozzleBank_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this.takeNozzleTask != null && this.takeNozzleTask.IsCompleted == false)
                || (this.putbackNozzleTask != null && this.putbackNozzleTask.IsCompleted == false)
                || (this.toolBankMaintenanceTask != null && this.toolBankMaintenanceTask.IsCompleted == false))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"当前动作未执行完，请等动作执行完再关闭窗体！",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                e.Cancel = true;
                return;
            }

            // 退出单步
            MachineStateModel.GetInstance().IsSingleStepWork = false;
            this.ucGuideMove?.Dispose();
            this.timer1.Stop();
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleBank_Load(object sender, EventArgs e)
        {
            // 父窗体
            Form frmForm = (Form)this.ParentForm;

            if (frmForm != null)
            {
                frmForm.KeyPreview = true;
                frmForm.KeyDown += this.FrmNozzleBank_KeyDown;
            }
        }

        /// <summary>
        /// 按键触发
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleBank_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F9)
            //{
            //    if (MachineStateModel.GetInstance().IsSingleStepWork)
            //    {
            //        // 单步执行
            //        SignalPool.GetInstance().System2SingleStepSignal.Set();
            //    }
            //}

            //if (Control.ModifierKeys == Keys.Shift)
            //{
            //    switch (e.KeyCode)
            //    {
            //        case Keys.F9:
            //            // 切换单步
            //            MachineStateModel.GetInstance().IsSingleStepWork =
            //                !MachineStateModel.GetInstance().IsSingleStepWork;

            //            break;
            //        case Keys.F10:
            //            break;
            //        case Keys.F11:
            //            break;
            //        case Keys.F12:
            //            break;
            //    }
            //}
        }
    }
}