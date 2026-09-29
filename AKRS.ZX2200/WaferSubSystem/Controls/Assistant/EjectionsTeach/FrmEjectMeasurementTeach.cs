using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach
{
    using System.Drawing;
    using System.IO;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using log4net.Core;
    using OfficeOpenXml;

    /// <summary>
    /// 导航示教--记录顶针中心与晶圆相机中心的相对偏差值
    /// </summary>
    public partial class FrmEjectMeasurementTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// 当前顶针槽位
        /// </summary>
        private EjectionBankSlotConfig currentBankSlotConfig;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// EjectMatchName
        /// </summary>
        private string EjectMatchName => this.currentBankSlotConfig.EjectionConfig.EjectMatchName;

        /// <summary>
        /// 顶针中心与晶圆相机中心的相对偏差
        /// </summary>
        private AKRSPoint3D deviationWithEjectionCenterAndWaferCameraCenter = new AKRSPoint3D();

        /// <summary>
        /// axisZPosition
        /// </summary>
        private double axisZPosition;

        /// <summary>
        /// measureTimes
        /// </summary>
        private int measureTimes;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlot</param>
        public FrmEjectMeasurementTeach(EjectionBankSlotConfig ejectionBankSlotConfig)
        {
            this.InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            this.currentBankSlotConfig = ejectionBankSlotConfig;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Position the tool approx. 5 mm above the eject cap, about to pre measure height!",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 请将吸嘴移动到靠近顶针帽5mm以内的位置，即将进行预测高!",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.axisZPosition = this.bondHeadController.GetAxisZRealPos();

                                (ExcuteResult Ret, double HeightValue) result = this.bondHeadController.MeasureHeight(
                                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                HeightMeasurementFunctionEnum.WithTDSensor);

                                if (result.Ret != ExcuteResult.Success)
                                {
                                    this.bondHeadController.CloseBondHeadVaccum();
                                    //AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Measure  height  failed!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "测高失败!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = -1;
                                }
                            },
                        doneAction: () =>
                        {
                        }),

                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; About to measurement.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 即将测高.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                            {
                                ExcelPackage package = new ExcelPackage(new FileInfo($"C:\\Thimble ejection consistency detection.xlsx"));

                                // 添加一个工作表
                                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(DateTime.Now + "sheet");

                                worksheet.Cells[1, 1].Value = "number";
                                worksheet.Cells[1, 2].Value = "x";
                                worksheet.Cells[1, 3].Value = "y";
                                worksheet.Cells[1, 4].Value = "z";
                                worksheet.Cells[1, 5].Value = "time";

                                for (int i = 0; i < this.measureTimes; i++)
                                {
                                    // 偏差
                                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                                    WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index, true);
                                    WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();

                                    (bool isSucceed, MatchResult[] matchResults) resultPr = Block.GetInstance().MatchResult(this.EjectMatchName, true, false);
                                    if (resultPr.isSucceed)
                                    {
                                        this.deviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(resultPr.matchResults)[0];
                                    }
                                    else
                                    {
                                        //AKRSXtraMessageBox.Show("The ejection is not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        AKRSXtraMessageBox.Show("pr未识别到顶针中心.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.DialogResult = DialogResult.Abort;
                                        return;
                                    }

                                    // 测高
                                    System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
                                    this.bondHeadController.MoveZAxis(this.axisZPosition);
                                    this.bondHeadController.CloseBondHeadVaccum();
                                    (ExcuteResult Ret, double HeightValue) resultMeasureHeight = this.bondHeadController.MeasureHeight(
                                        BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                        HeightMeasurementFunctionEnum.WithTDSensor);

                                    if (resultMeasureHeight.Ret != ExcuteResult.Success)
                                    {
                                        this.bondHeadController.CloseBondHeadVaccum();
                                        //AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Measure  height  failed!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "测高失败!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        this.DialogResult = DialogResult.Abort;
                                        return;
                                    }

                                    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

                                    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                                    WaferSubController.GetInstance().EjectController.ReturnEjection();

                                    // 整理结果xyz
                                    LogHelper.Post(Level.Info, $"x = {this.deviationWithEjectionCenterAndWaferCameraCenter.X}, y = {this.deviationWithEjectionCenterAndWaferCameraCenter.Y}, z = {resultMeasureHeight.HeightValue}", LogCategory.Global, ViewType.InUI);

                                    worksheet.Cells[i + 2, 1].Value = i + 1;
                                    worksheet.Cells[i + 2, 2].Value = this.deviationWithEjectionCenterAndWaferCameraCenter.X;
                                    worksheet.Cells[i + 2, 3].Value = this.deviationWithEjectionCenterAndWaferCameraCenter.Y;
                                    worksheet.Cells[i + 2, 4].Value = resultMeasureHeight.HeightValue;
                                    worksheet.Cells[i + 2, 5].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                                    // 保存Excel文件
                                    package.Save();
                                }
                            }),
                };

             ucGuideMove = new UcGuideMove("晶圆台模组", "FrmEjectMeasurementTeach", true, CameraEnum.WaferCamera);
            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void CancelOperation()
        {
            //this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "是否结束示教?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// Back操作
        /// </summary>
        private void BackOperation()
        {
            this.stepIndex--;
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
        }

        /// <summary>
        /// Next操作
        /// </summary>
        private void NextOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
        }

        /// <summary>
        /// Done操作
        /// </summary>
        private void DoneOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.Close();
        }

        /// <summary>
        /// Back按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.BackOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Next按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.NextOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.DoneOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CancelOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Autofocus按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutofocus_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.Autofocus();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// BtnEditProgram_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().WaferTableController.EditPr(this.EjectMatchName);
        }

        /// <summary>
        /// FrmEjectMeasurementTeach_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void FrmEjectMeasurementTeach_Load(object sender, EventArgs e)
        {
            //var result = XtraInputBox.Show("Please input measure times:", "MeasureTimes", "1", MessageBoxButtons.OKCancel);
            var result = XtraInputBox.Show("请输入测量次数:", "MeasureTimes", "1", MessageBoxButtons.OKCancel);
            if (string.IsNullOrEmpty(result))
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }

            this.measureTimes = int.Parse(result);
            if (this.measureTimes < 1)
            {
                //AKRSXtraMessageBox.Show("Please Input int >= 1!", "Promit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AKRSXtraMessageBox.Show("请输入 >= 1 的整数!", "Promit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Abort;
                return;
            }

            WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index, true);
            System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup2, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmEjectMeasurementTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }

            this.ucGuideMove.Dispose();
        }
    }
}