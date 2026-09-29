using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach
{
    using System.Drawing;
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Component;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 导航示教--芯片及蓝膜测厚
    /// </summary>
    public partial class FrmCarrierHeightMeasureTeach : DevExpress.XtraEditors.XtraForm
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
        private int stepCount = 3;

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

        private CarrierWithWaferConfig CarrierWithWaferConfig;

        /// <summary>
        /// axisZPosition
        /// </summary>
        private double axisZPosition;

        public FrmCarrierHeightMeasureTeach(CarrierWithWaferConfig carrierWithWaferConfig)
        {
            this.InitializeComponent();
            this.CarrierWithWaferConfig = carrierWithWaferConfig;
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
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 即将进行芯片与蓝膜总厚度测高,请将晶圆台移动到芯片处！",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
                            },
                        doneAction: () =>
                            {
                            }),
                 
                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 将吸嘴移动到距离顶针帽上方5mm内.即将进行测高.",
                        isShowTitle: true,
                        isShowBack: true,
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

                                if (result.Ret == ExcuteResult.Success)
                                {
                                    double offset = this.GetCurrentComponentEjectionConfig().EjectionTableMeasureHeightPosition.Z - WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos.Z;
                                    double height = Math.Abs(MachineCoordinateSystem.GetInstance().BondCoordinateSystem
                                                                 .ConvertMachineZToG0Pos(result.HeightValue).Z
                                                             - this.GetCurrentComponentEjectionConfig().ReadyBondPickPosition.Z + offset);

                                    this.CarrierWithWaferConfig.ComponentAndCarrierThickness = double.Parse(height.ToString("f3"));
                                    CarrierConfigRepository.GetInstance().Save();
                                    AKRSXtraMessageBox.Show($"测量芯片 + 蓝膜厚度结果为{this.CarrierWithWaferConfig.ComponentAndCarrierThickness}mm.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    UcComponentEdit.SetLcThicknessAction(this.CarrierWithWaferConfig.ComponentAndCarrierThickness);
                                }
                                else
                                {
                                    this.bondHeadController.CloseBondHeadVaccum();
                                    this.stepIndex--;
                                }
                            },
                        doneAction: () =>
                            {
                            }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 即将进行蓝膜厚度测高,请将晶圆台移动到蓝膜处！点击确定 ... 将进行测高.",
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
                                System2Domain.GetInstance().BondModuleController.MoveToPickupPos();
                                this.bondHeadController.MoveZAxis(this.axisZPosition);
                                this.bondHeadController.CloseBondHeadVaccum();

                                (ExcuteResult Ret, double HeightValue) result = this.bondHeadController.MeasureHeight(
                                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                    HeightMeasurementFunctionEnum.WithTDSensor);

                                if (result.Ret == ExcuteResult.Success)
                                {
                                    double offset = this.GetCurrentComponentEjectionConfig().EjectionTableMeasureHeightPosition.Z - WaferSubController.GetInstance().EjectController.EjectionTableAxisZG0Pos.Z;
                                    double height = Math.Abs(MachineCoordinateSystem.GetInstance().BondCoordinateSystem
                                                                 .ConvertMachineZToG0Pos(result.HeightValue).Z
                                                             - this.GetCurrentComponentEjectionConfig().ReadyBondPickPosition.Z + offset);

                                    this.CarrierWithWaferConfig.CarrierThickness = double.Parse(height.ToString("f3"));;
                                    this.CarrierWithWaferConfig.ComponentThickness = this.CarrierWithWaferConfig.ComponentAndCarrierThickness - this.CarrierWithWaferConfig.CarrierThickness;
                                    CarrierConfigRepository.GetInstance().Save();
                                    AKRSXtraMessageBox.Show($"测量蓝膜厚度结果为{this.CarrierWithWaferConfig.CarrierThickness}mm,经计算芯片厚度为{this.CarrierWithWaferConfig.ComponentThickness}mm", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    UcComponentEdit.SetLcThicknessAction(this.CarrierWithWaferConfig.ComponentAndCarrierThickness);

                                    this.DialogResult = DialogResult.OK;
                                }
                                else
                                {
                                    this.bondHeadController.CloseBondHeadVaccum();
                                    this.DialogResult = DialogResult.Cancel;
                                }
                            }),
                };

            UcGuideMove ucGuideMove = new UcGuideMove("晶圆台模组", "" ,true, CameraEnum.WaferCamera);
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
            
        }

        /// <summary>
        /// 获取芯片所用的顶针的配置
        /// </summary>
        /// <returns>return</returns>
        private EjectionConfig GetCurrentComponentEjectionConfig()
        {
            return (EjectionConfig)EjectionConfigRepository.GetInstance().Find(this.CarrierWithWaferConfig.EjectionName);
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