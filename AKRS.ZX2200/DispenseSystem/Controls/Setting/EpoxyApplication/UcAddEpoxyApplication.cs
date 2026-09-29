using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
using AKRS.ZX2200.Infrastructure.Action;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using DevExpress.XtraEditors;


namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Models.Programs;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    using EpoxyApplication = Models.Repositories.Pattern.EpoxyApplication;

    /// <summary>
    /// 新增焊点界面
    /// </summary>
    public partial class UcAddEpoxyApplication : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 树结构
        /// </summary>
        private readonly Action refreshNode;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="refreshNode">树结构</param>
        public UcAddEpoxyApplication(Action refreshNode)
        {
            this.InitializeComponent();
            this.refreshNode = refreshNode;
            this.RefreshControl();
            this.GvEpoxyApplication.OptionsBehavior.Editable = false;
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        private void RefreshControl()
        {
            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                this.GcEpoxyApplication.DataSource =
                    BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplications;
                BondProgram.GetInstance().Save();
            }
            else
            {
                //System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames.Add("dian");

                this.GcEpoxyApplication.DataSource =
                    System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplications;
 

                // 保存数据
                System1Program.GetInstance().Save();
            }

            this.GcEpoxyApplication.Refresh();
            this.GvEpoxyApplication.RefreshData();
        }

        /// <summary>
        /// 新增EpoxyApplication
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            FrmNewCreateEpoxyApplication frmNewCreateEpoxyApplication = new FrmNewCreateEpoxyApplication();

            if (frmNewCreateEpoxyApplication.ShowDialog() == DialogResult.OK)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense 
                    && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                {
                    BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames
                        .Add(frmNewCreateEpoxyApplication.NewEpoxyApplication.Name);
                    BondProgram.GetInstance().Save();
                }
                else
                {
                    System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames
                        .Add(frmNewCreateEpoxyApplication.NewEpoxyApplication.Name);

                    // 保存数据
                    System1Program.GetInstance().Save();
                }
            }

            this.RefreshControl();
            this.refreshNode();
        }

        /// <summary>
        /// 删除选中的焊点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // 获取当前选中的焊点
            EpoxyApplication epoxyApplication = (EpoxyApplication)this.GvEpoxyApplication.GetFocusedRow();

            // 检查是否为空
            if (epoxyApplication == null)
            {
                AKRSXtraMessageBox.Show($"Please select one epoxy application first！", "提示", MessageBoxButtons.OK);
                return;
            }

            DialogResult res = AKRSXtraMessageBox.Show($"Are you sure to remove bonding position：{epoxyApplication.Name}?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (res == DialogResult.Yes)
            {
                if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                    && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                {
                    BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames
                        .Remove(epoxyApplication.Name);
                    BondProgram.GetInstance().Save();
                }
                else
                {
                    System1Program.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames
                        .Remove(epoxyApplication.Name);

                    // 保存数据
                    System1Program.GetInstance().Save();
                }
            }

            this.RefreshControl();
            this.refreshNode.Invoke();
        }

        /// <summary>
        /// 仓库
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEpoxyApplicationRepository_Click(object sender, EventArgs e)
        {
            FrmRepository<EpoxyApplication> frmRepository = new FrmRepository<EpoxyApplication>(EpoxyApplicationRepository.GetInstance(), this.RefreshControl);

            // 获取显示器屏幕宽度,高度
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;
            frmRepository.Location = new Point(xWidth - 700, 250);
            frmRepository.StartPosition = FormStartPosition.Manual;
            frmRepository.ShowDialog();

            EpoxyApplication epoxyApplication = (EpoxyApplication)frmRepository.DsSetting;

            if (epoxyApplication == null)
            {
                return;
            }

            if (MachineHardwareConfiguration.GetInstance().IsSystem2Dispense
                && MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                if (!BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames
                        .Contains(epoxyApplication.Name))
                {
                    BondProgram.GetInstance().EpoxyApplicationProgram.EpoxyApplicationNames.Add(epoxyApplication.Name);
                }
            }
            else
            {
                if (!System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames
                        .Contains(epoxyApplication.Name))
                {
                    System1Domain.GetInstance().System1Program.EpoxyApplicationProgram.EpoxyApplicationNames.Add(epoxyApplication.Name);
                }
            }

            EpoxyApplicationRepository.GetInstance().Save();

            System1Program.GetInstance().Save();
            BondProgram.GetInstance().Save();

            this.RefreshControl();
            this.refreshNode();
        }
    }
}
