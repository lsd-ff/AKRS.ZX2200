using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach
{
    using System.Drawing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.XtraRichEdit.Fields;
    using LanguageExt.ClassInstances.Pred;

    /// <summary>
    /// 导航示教--记录Bond接触顶针帽的高度
    /// </summary>
    public partial class FrmHeightMeasurementTeach : DevExpress.XtraEditors.XtraForm
    {
       private UcGuideMove ucGuideMove;
        
        /// <summary>
        /// Bond预取料位置
        /// </summary>
        private AKRSPoint3D readyBondPickPosition = new AKRSPoint3D();

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 1;

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
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlot</param>
        public FrmHeightMeasurementTeach(EjectionBankSlotConfig ejectionBankSlotConfig)
        {
            this.InitializeComponent();
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
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Position the touchdown tool approx.\r\n" + "5 mm above the needle cap of the ejection system (not above the hole).\r\n" + "DONE ... carry out touchdown.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 将TouchDown移动到距离顶针帽上方5mm内.\r\n" + "点击确定 ... 将进行测高.",
                        isShowTitle: true,
                        isShowBack: false,
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
                            if (!MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    (ExcuteResult Ret, double HeightValue) result = (ExcuteResult.Success, this.bondHeadController.GetAxisZRealPos());
                                    ReMeasureHeight:
                                   
                                    // 测高
                                    result = this.bondHeadController.MeasureHeight(
                                            BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                            HeightMeasurementFunctionEnum.WithTDSensor);
                                    
                                    //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Remove touchdown tool", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "请移除吸嘴", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                    switch (this.res)
                                    {
                                        case DialogResult.OK:
                                            break;
                                        case DialogResult.Cancel:
                                            return;
                                    }

                                    if (result.Ret != ExcuteResult.Success)
                                    {
                                        //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "Measure  height  failed!\r\n ReMeasureHeight with  OK\r\n End assistant  with  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                        this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + "测高失败!\r\n 重新测高请点击  OK\r\n 结束请点击  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                                        switch (this.res)
                                        {
                                            case DialogResult.OK:
                                                goto ReMeasureHeight;
                                            case DialogResult.Cancel:
                                                return;
                                        }
                                    }

                                    this.readyBondPickPosition = MachineCoordinateSystem.GetInstance().BondCoordinateSystem.ConvertMachineZToG0Pos(result.HeightValue);

                                    this.currentBankSlotConfig.EjectionConfig.ReadyBondPickPosition = this.readyBondPickPosition;
                                }

                                this.currentBankSlotConfig.EjectionConfig.EjectionTableMeasureHeightPosition = WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos;

                                this.currentBankSlotConfig.EjectionConfig.HeightMeasurement.State = AssistantStateEnum.Able;
                                EjectionConfigRepository.GetInstance().Save();
                                this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("固晶模组", "FrmHeightMeasurementTeach", false, CameraEnum.WaferCamera);
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
        /// ESUpOrDown按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESUpOrDown_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index, true);
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// ESAirSupplyOnOrOff按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESAirSupplyOnOrOff_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.CloseEjectionTableBlow();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.OpenEjectionTableBlow();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// IniNeedleSystem按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnIniNeedleSystem_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                EjectModule.EjectionAxisZ.GoHome();
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
        /// BtnMoveToCameraCenter按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToCameraCenter_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.bondModuleController.MoveToPickupPos();
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
        /// FrmHeightMeasurementTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmHeightMeasurementTeach_Load(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + $"Current MeasureHeight mode:{this.bondHeadController.GetCurrentNozzle().NozzleHeightMeasurementFunction.GetDescription()}\r\n Continue with  OK\r\n End assistant  with  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.2264:\r\n" + $"当前测高方式:{this.bondHeadController.GetCurrentNozzle().NozzleHeightMeasurementFunction.GetDescription()}\r\n 继续请点击  OK\r\n 结束请点击  Cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            switch (this.res)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    this.DialogResult = DialogResult.Abort;
                    return;
            }
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

        private void FrmHeightMeasurementTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose();

            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }
    }
}