using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.BondSystem.Models.Repositories.NozzleShelf;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Enums;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using System.IO;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.BondForce.Modbus;
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem;
    using OfficeOpenXml;

    /// <summary>
    /// 换Tool测试按钮
    /// </summary>
    public partial class FrmToolsTestRun : XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmToolsTestRun()
        {
            this.InitializeComponent();
            this.InitControl();
        }

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
        private EjectionBankConfig CurrentBank => EjectionBankConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == WaferSystemDomain.GetInstance().WaferSystemProgram.EjectionBankProgram.Name);

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// Bond Head控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// 换吸嘴重复性测试线程
        /// </summary>
        private Task changePPtoolTestTask;

        /// <summary>
        /// 换顶针重复性测试线程
        /// </summary>
        private Task changeEStoolTestTask;

        /// <summary>
        /// 换吸嘴测试停止信号
        /// </summary>
        private bool isStopPPToolTest = true;

        /// <summary>
        /// 换顶针测试停止信号
        /// </summary>
        private bool stopESToolTest = true;

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            // 吸嘴防呆
            if (BondProgram.GetInstance().NozzleShelfProgram.NozzleShelf == null)
            {
                this.LbPPTool.Visible = false;
                this.LbPPTool.Enabled = false;
                this.BtnPPToolsTestRun.Visible = false;
                this.BtnPPToolsTestRun.Enabled = false;
            }

            // 顶针防呆
        }

        /// <summary>
        /// 换PP tool
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnToolsTestRun_Click(object sender, EventArgs e)
        {
            if (System2Configuration.GetInstance().IsToolBankEnable == false)
            {
                AKRSXtraMessageBox.Show(
                    $"吸嘴架未启用！",
                    "报警",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // 启动
            if (this.changePPtoolTestTask == null || this.changePPtoolTestTask.IsCompleted == true) 
            {
                this.BtnPPToolsTestRun.Appearance.BackColor = Color.Yellow;
                this.BtnPPToolsTestRun.Text = @"停止重复换吸嘴测试";
                this.isStopPPToolTest = false;
                this.ChangePPToolsStart();
            }
            else
            {
                this.isStopPPToolTest = true;

                //Stopwatch sp = Stopwatch.StartNew();

                //while (!this.changePPtoolTestTask.IsCompleted)
                //{
                //    Thread.Sleep(100);

                //    if (sp.ElapsedMilliseconds > 100000)
                //    {
                //        MessageBox.Show("Error");
                //        break;
                //    }
                //}

                //this.changePPtoolTestTask.Wait();

                // 停止
                this.BtnPPToolsTestRun.Appearance.BackColor = Color.Transparent;
                this.BtnPPToolsTestRun.Text = @"开始重复换吸嘴测试";
            }
        }

        /// <summary>
        /// 线程启动
        /// </summary>
        /// <returns>结果</returns>
        public bool ChangePPToolsStart()
        {
            this.changePPtoolTestTask = Task.Run(
                () =>
                    {
                        CommonUtil.SetCurrentThreadName("换吸嘴重复性测试线程");
                        this.PPToolsTestRun();
                    });

            return true;
        }

        /// <summary>
        /// PP tool测试方法
        /// </summary>
        private void PPToolsTestRun()
        {
            bool isChangeNozzleSuccess = false;

            try
            {
                while (!this.isStopPPToolTest)
                {
                    // 循环吸嘴槽
                    foreach (NozzleShelfSlot slot in this.system2Domain.BondProgram.NozzleShelfProgram.NozzleShelf
                                 .NozzleShelfSlots)
                    {
                        string nozzleName = slot.NozzleName;

                        if (string.IsNullOrEmpty(nozzleName))
                        {
                            continue;
                        }

                         isChangeNozzleSuccess = this.system2Controller.ChangeNozzle(nozzleName);

                        // 取吸嘴
                        if (isChangeNozzleSuccess == false)
                        {
                            AKRSXtraMessageBox.Show(
                                $"换吸嘴失败！",
                                "报警",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            return;
                        }
  
                        Task t1 = Task.Run(this.toolBankController.MoveShelfToHome);
                        Task t2 = Task.Run(this.bondModuleController.MoveToChangeNozzleSafePos);
                        t1.Wait(10000);
                        t2.Wait(10000);

                        // 等轴到位
                        if (t1.IsCompleted == false)
                        {
                            AKRSXtraMessageBox.Show(
                                $"吸嘴架回零失败!\r\n",
                                "异常",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);

                            return;
                        }

                        Thread.Sleep(100);

                        if (this.isStopPPToolTest)
                        {
                            return;
                        }
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
            }
            finally
            {
                // 换吸嘴失败不动吸嘴架避免二次撞击
                if (isChangeNozzleSuccess == true)
                {
                    // 吸嘴架回原
                    this.toolBankController.MoveShelfToHome();
                }

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
        /// 换ES tool
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESToolsTestRun_Click(object sender, EventArgs e)
        {
            if (this.BtnESToolsTestRun.Text == @"开始重复换顶针测试")
            {
                this.BtnESToolsTestRun.Appearance.BackColor = Color.Yellow;
                this.BtnESToolsTestRun.Text = @"停止重复换顶针测试";
                this.stopESToolTest = false;
                this.ChangeESToolsStart();
            }
            else
            {
                this.BtnESToolsTestRun.Appearance.BackColor = Color.Transparent;
                this.BtnESToolsTestRun.Text = @"开始重复换顶针测试";
                this.stopESToolTest = true;
                this.ChangeESToolsStart();
            }           
        }

        /// <summary>
        /// ChangeESToolsStart
        /// </summary>
        /// <returns>结果</returns>
        public bool ChangeESToolsStart()
        {
            if (this.changeEStoolTestTask == null
              || this.changeEStoolTestTask.Status != TaskStatus.Running)
            {
                this.changeEStoolTestTask = Task.Run(
                  () =>
                  {
                      CommonUtil.SetCurrentThreadName("换顶针重复性测试线程");
                      this.ESToolsTestRun();
                  });
            }
      
            return true;
        }

        /// <summary>
        /// ES tool测试方法
        /// </summary>
        private void ESToolsTestRun()
        {
            try
            {
                while (!this.stopESToolTest)
                {
                    for (int i = 0; i < this.CurrentBank.EjectionBankSlots.Length; i++)
                    {
                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        if (!this.stopESToolTest)
                        {
                            if (WaferSystemProgram.GetInstance().EjectionBankProgram.EjectionBankEntity.EjectionBankSlotEntities[i].SlotState != EjectSlotStatuEnum.Empty)
                            {
                                WaferSubController.GetInstance().EjectController.ChangeEjection(i, true);
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
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmToolsTestRun_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmToolsTestRun_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.changeEStoolTestTask != null)
            {
                if (this.changeEStoolTestTask.IsCompleted == false)
                {
                    e.Cancel = true;
                }
            }
        }

      /// <summary>
      /// 保存力值
      /// </summary>
      /// <param name="val">力</param>
        public void SaveForceVal(int val)
        {
            try
            {
                // 添加一个工作表
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"D:\" + MachineConfigContext.GetInstance().CurrentRecipe.RecipeName + "-换吸嘴过程力值记录.xlsx"));

                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Count > 0
                                               ? excelPackage.Workbook.Worksheets[0]
                                               : excelPackage.Workbook.Worksheets.Add("DataSheet");

                // 设置列宽
                for (int i = 1; i <= 7; i++)
                {
                    worksheet.Column(i).Width = 15;
                }

                // 添加标题行
                if (worksheet.Dimension == null)
                {
                    worksheet.Cells[1, 1].Value = "时间";

                    worksheet.Cells[1, 2].Value = "力值";
                }

                int lastUsedRow = worksheet.Dimension != null ? worksheet.Dimension.End.Row : 0;

                worksheet.Cells[lastUsedRow + 1 , 1].Value = DateTime.Now.ToString("MM-dd HH:mm:ss");

                worksheet.Cells[lastUsedRow + 1, 2].Value = val;

                excelPackage.Save();
            }
            catch (Exception e)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"文件已被打开，保存数据失败!\r\n Retry:关掉文件重新保存\r\nCancel:不保存数据",
                    "报警",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Warning);

                if (dialog == DialogResult.Retry)
                {
                    SaveForceVal(val);
                }
            }
        }
    }
}