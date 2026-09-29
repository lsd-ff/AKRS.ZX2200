using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionsTeach
{
    using AKRS.Galaxy2.Infrastructure;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using System.Drawing;

    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;

    /// <summary>
    /// 导航示教--记录顶针中心与晶圆相机中心的相对偏差值
    /// </summary>
    public partial class FrmXYPositionTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// EjectMatchName
        /// </summary>
        private string EjectMatchName => this.currentBankSlotConfig.EjectionConfig.EjectMatchName;

        /// <summary>
        /// 顶针中心与晶圆相机中心的相对偏差
        /// </summary>
        private AKRSPoint3D deviationWithEjectionCenterAndWaferCameraCenter = new AKRSPoint3D();

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

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// FlipChipModule
        /// </summary>
        private FlipModule FlipChipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// FlipChipDevicePara
        /// </summary>
        private FlipTableDevicePara FlipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        #endregion

        /// <summary>
        /// Bond模组控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ejectionBankSlotConfig">ejectionBankSlot</param>
        public FrmXYPositionTeach(EjectionBankSlotConfig ejectionBankSlotConfig)
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
            string strStep1;
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                strStep1 = "调整顶针位置和晶圆相机位置，使晶圆相机聚焦顶针.";
            }
            else
            {
                strStep1 = "调整顶针位置和BondZ轴位置，使Bond相机聚焦顶针（请不要移动BondXY轴位置）.";
            }

            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Focus wafer camera\r\n" + $"Lift needles afterwards and determine position.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; " + strStep1,
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().IsOffLineWork)
                                {
                                    return;
                                }

                                FrmPRImage frmPrImage;
                                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                                {
                                    frmPrImage = new FrmPRImage("晶圆相机");
                                }
                                else
                                {
                                    frmPrImage = new FrmPRImage("BOND相机");
                                }

                                frmPrImage.ShowDialog();
                                if (frmPrImage.DialogResult == DialogResult.OK)
                                {
                                    (double x, double y)result = frmPrImage.GetPixelPos();
                                    this.SpEjectionPixelPosX.Value = Convert.ToDecimal(result.x);
                                    this.SpEjectionPixelPosY.Value = Convert.ToDecimal(result.y);
                                }
                                else
                                {
                                    //AKRSXtraMessageBox.Show("Please select ejection center.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    AKRSXtraMessageBox.Show("请选择顶针中心.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.stepIndex = -1;
                                }
                            },
                        doneAction: () =>
                        {
                        }),

                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Position the crosshairs centrally to the center of the needle kit.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 确定顶针中心位置.",
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
                                // 新版
                                double x = (double)this.SpEjectionPixelPosX.Value;
                                double y = (double)this.SpEjectionPixelPosY.Value;

                                //MatchResult baseAlg = new MatchResult();
                                //baseAlg = (MatchResult)VisionService.Vision(
                                //    "Bond相机识别顶针中心模板",
                                //    System2Domain.GetInstance().BondModuleController.GetHardware(),
                                //    "Bond",
                                //    "",
                                //    false);

                                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                                {
                                    this.deviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().GetOffsetWorld(new AKRSPoint3D(x, y, 0));
                                }
                                else
                                {
                                    AKRSPoint2D point1 = CalibService.GetMachinePosByPixelPos(new AKRSPoint2D(0, 0), new MatchResult(x, y, 0), "BondCameraCoordinateSystem");
                                    this.deviationWithEjectionCenterAndWaferCameraCenter = new AKRSPoint3D(point1.X, point1.Y, 0);
                                }
                                                                
                                this.currentBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = this.deviationWithEjectionCenterAndWaferCameraCenter;
                                this.currentBankSlotConfig.EjectionConfig.EjectionPixelPos = new AKRSPoint3D(x, y, 0);

                                this.currentBankSlotConfig.EjectionConfig.XYPosition.State = AssistantStateEnum.Able;
                                EjectionConfigRepository.GetInstance().Save();
                                this.DialogResult = DialogResult.OK;

                                return;

                                // 旧版
                                (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.EjectMatchName, false, false);
                                if (result.isSucceed)
                                {
                                    this.deviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(result.matchResults)[0];
                                    this.currentBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = this.deviationWithEjectionCenterAndWaferCameraCenter;

                                    this.currentBankSlotConfig.EjectionConfig.XYPosition.State = AssistantStateEnum.Able;
                                    EjectionConfigRepository.GetInstance().Save();
                                    this.DialogResult = DialogResult.OK;
                                }
                                else
                                {
                                    AKRSXtraMessageBox.Show("The ejection is not exist.", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }),
                };

             ucGuideMove = new UcGuideMove("固晶模组", "FrmXYPositionTeach", true, CameraEnum.BondCamera);

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
        /// ESVacuumOnOrOff按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESVacuumOnOrOff_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.OpenEjectionTableVacuum();
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
        /// BtnEditProgram_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                WaferSubController.GetInstance().WaferTableController.EditPr(this.EjectMatchName);
            }
            else
            {
                TUAssistantHelper.EditPrInSystem2(this.EjectMatchName, CameraTypeEnum.BondCamera);
            }                        
        }

        /// <summary>
        /// FrmXYPositionTeach_Load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void FrmXYPositionTeach_Load(object sender, EventArgs e)
        {
            this.SpEjectionPixelPosX.EditValue = this.currentBankSlotConfig.EjectionConfig.EjectionPixelPos.X;
            this.SpEjectionPixelPosY.EditValue = this.currentBankSlotConfig.EjectionConfig.EjectionPixelPos.Y;
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

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void FrmXYPositionTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose();
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }

            this.Dispose(); 
        }
    }
}