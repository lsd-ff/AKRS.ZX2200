#region << 版 本 注 释 >>
/*----------------------------------------------------------------
 * 版权所有 (c) 2022  AKRS(艾科瑞思智能装备股份有限公司) 保留所有权利。
 * 公司名称：艾科瑞思
 * 命名空间：
 * 文件名：
 * 创建人： 贺强
 * 创建时间： 2023/11/30 12:49:20
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

namespace AKRS.ZX2200.Main.Controls.Ucmain.MainControls
{
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.WM;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.Main.Machine.Process;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportUnitSystem;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using DevExpress.Utils.Extensions;
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Docking;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using log4net.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using VM.Core;
    using static System.Windows.Forms.VisualStyles.VisualStyleElement;
    using Button = System.Windows.Forms.Button;
    using Machine = AKRS.ZX2200.Main.Machine.MachineSupport.Machine;

    /// <summary>
    /// 描述：Production 标签页
    /// </summary>
    public partial class UcMainSystem
    {
        /// <summary>
        /// Production界面启动按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BarItemStart_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Enabled = false;

            try
            {
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

                // 开始生产后，关闭方向盘
                DockPanel dockPanel = this.GetDockPanel("ucGuidMove");
                if (dockPanel != null)
                {
                    dockPanel.Close();
                }

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
        /// Production界面restart按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BarItemRestart_ItemClick(object sender, ItemClickEventArgs e)
        {
        }

        /// <summary>
        /// 初始化程式节点
        /// </summary>
        private void InitProgramNodes()
        {
            ProductConfiguration.GetInstance().InitProductConfiguration();
            TransportProgram.GetInstance().InitTransportSystemProgram();
            BondProgram.GetInstance().InitBondProgram();
            System1Program.GetInstance().InitSystem1Program();
            WaferSystemProgram.GetInstance().InitWaferSystemProgram();
            ProcessDomain.GetInstance().InitProcessProgram();
        }

        /// <summary>
        /// 重新加载Recipe
        /// </summary>
        private void ReLoadRecipe()
        {
            Machine.GetInstance().PrInitSuccess = false;

            // 防呆
            this.Enabled = false;
            this.LbTip.Visible = true;
            this.LbTip.Location = new Point(this.Size.Width / 2 - 250, this.Size.Height / 2 - 180);
            this.LbTip.BringToFront();

            this.BarLblTaskName.Caption = MachineConfigContext.GetInstance().RecipeName;
            this.dockManager.Clear();
            this.InitProgramNodes();

            try
            {
                if (MachineStateModel.GetInstance().IsOffLineWork)
                {
                    Machine.GetInstance().PrInitSuccess = true;
                    return;
                }

                LogHelper.Post(Level.Info, $"准备加载PR", LogCategory.PR, ViewType.InFileAndUI);

                // 清理旧PR方案
                VmSolution.Instance.CloseSolution();

                LogHelper.Post(Level.Info, $" 清理旧PR方案完成", LogCategory.PR, ViewType.InFileAndUI);

                // 创建新方案
                VmSolution.CreatSolInstance();

                LogHelper.Post(Level.Info, $"创建新方案完成", LogCategory.PR, ViewType.InFileAndUI);

                List<string> lists = System2CommonService.GetCurrentRecipePREntityNameList();

                foreach (string prName in lists)
                {
                    PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);
                    if (prEntity != null)
                    {
                        prEntity.Alg.InitVmProcedure();
                        LogHelper.Post(Level.Info, $"{prEntity.GetName()}:视觉方案加载成功", LogCategory.PR, ViewType.InFileAndUI);
                    }
                    else 
                    {
                        LogHelper.Post(Level.Info, $"视觉方案加载失败，未找到：{prName}", LogCategory.PR, ViewType.InFileAndUI);
                    }
                }

                DispenseRunTimeProvider.LoadDispensePr();

                Machine.GetInstance().PrInitSuccess = true;
            }
            catch (Exception e)
            {
                AKRSXtraMessageBox.Show("设备初始化视觉模板失败，请联系设备人员");
                LogHelper.Post(Level.Error, e.Message, LogCategory.PR);
                throw;
            }
            finally
            {
                this.LbTip.Visible = false;
                this.Enabled = true;
            }

            return;

            // 杀死当前进程，自动重启软件
            Process.Start(Application.StartupPath + "\\AKRS.ZX2200.exe");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
            System.Environment.Exit(0);

            return;
        }
    }
}
