using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Programs;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;


namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Utils;

    /// <summary>
    ///  刮胶盘示教
    /// </summary>
    public partial class FrmSlideFluxerTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmSlideFluxerTeach()
        {
            InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 判断当前是否在运动，在运动的时候不能取消示教
        /// </summary>
        private bool isMove;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 刮胶盘控制器
        /// </summary>
        private SlideFluxerController slideFluxerController = new SlideFluxerController();

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: "Step1/4:将touchdown移动到蘸胶位置正上方5mm,点击“下一步”进行测高。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerPos =
                                    this.bondModuleController.GetG0RealPosition();

                                double liftLevel = this.bondHeadController.GetAxisZRealPos();

                                // 执行测高
                                (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                    liftLevel,
                                    HeightMeasurementFunctionEnum.WithTDSensor);

                                if (res.Ret == ExcuteResult.Success)
                                {   
                                    // 在G0中的高度
                                    BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerPos.Z =
                                            this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);

                                    // 保存
                                    BondDevicePara.GetInstance().Save();
                                    BondProgram.GetInstance().Save();
                                }
                                else
                                {
                                    this.stepIndex--;
                                    return;
                                }

                                this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerLeftTopPos;
                            },
                       doneAction: () =>
                       {
                       }),

                    new AssistantConfig(
                        index: 1,
                        descritpion: "Step2/4:将touchdown移动到刮胶盘左上位置（超过刮胶盘边缘）,点击“下一步”。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachDipPos;
                        },
                        nextAction: () =>
                        {
                                  this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerRightBottomPos;

                             if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerLeftTopPos =
                                this.bondModuleController.GetG0RealPosition();

                            //// 执行测高
                            //(ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                            //    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                            //    HeightMeasurementFunctionEnum.WithTDSensor);

                            //if (res.Ret == ExcuteResult.Success)
                            //{   
                            //    // 在G0中的高度
                            //    BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerLeftTopPos.Z =
                            //            this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);

                            //    // 保存
                            //    BondDevicePara.GetInstance().Save();
                            //    BondProgram.GetInstance().Save();

                            //    this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerRightBottomPos;
                            //}
                        },
                        doneAction: () =>
                        {  
                        }),

                       new AssistantConfig(
                        index: 2,
                        descritpion: "Step3/4:将touchdown移动到刮胶盘右下位置（超过刮胶盘边缘）,点击“下一步”。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerLeftTopPos;
                        },
                        nextAction: () =>
                        {
                               this.TileBarTeach.SelectedItem = this.TbiMeasureHeight;

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos =
                                this.bondModuleController.GetG0RealPosition();

                            //// 执行测高
                            //(ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                            //    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                            //    HeightMeasurementFunctionEnum.WithTDSensor);

                            //if (res.Ret == ExcuteResult.Success)
                            //{   
                            //    // 在G0中的高度
                            //    BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos.Z =
                            //            this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);

                            //    // 保存
                            //    BondDevicePara.GetInstance().Save();
                            //    BondProgram.GetInstance().Save();

                            //    this.slideFluxerController.SlideFluxerHomeNoWait();
                            //    this.bondModuleController.MoveToSafePos();

                            //    this.DialogResult = DialogResult.OK;
                            //}
                        },
                        doneAction: () =>
                        {
                                    
                        }),

                       new AssistantConfig(
                           index: 3,
                           descritpion: "Step4/4:将touchdown移动到刮胶盘边缘正上方5mm,点击“确定”进行测高。",
                           isShowTitle: true,
                           isShowBack: true,
                           isShowNext: false,
                           isShowDone: true,
                           backAction: () =>
                               {
                                   this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerRightBottomPos;
                               },
                           nextAction: () =>
                               {
                               },
                           doneAction: () =>
                               {
                                   if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                   {
                                       return;
                                   }

                                   double liftLevel = this.bondHeadController.GetAxisZRealPos();

                                   // 执行测高
                                   (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                       liftLevel,
                                       HeightMeasurementFunctionEnum.WithTDSensor);

                                   if (res.Ret == ExcuteResult.Success)
                                   {   
                                       // 在G0中的高度
                                       BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerRightBottomPos.Z =
                                               this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);
                                       BondDevicePara.GetInstance().SlideFluxerParam.SlideFluxerLeftTopPos.Z =
                                           this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);

                                       //// 重新设置安全高度，防止后续步骤运动时碰撞到刮胶盘
                                       //BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z = res.HeightValue + 5;

                                       // 保存
                                       BondDevicePara.GetInstance().Save();
                                       BondProgram.GetInstance().Save();

                                       this.slideFluxerController.SlideFluxerHomeWaitArrive();
                                       this.bondModuleController.MoveToSafePos();

                                       this.DialogResult = DialogResult.OK;
                                   }
                               }),
                };

             ucGuideMove = new UcGuideMove("FrmSlideFluxerTeach");
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System2);
            this.PnlControl.Controls.Add(ucGuideMove);


            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiTeachDipPos;

            // 方向盘设置模组名称
            ucGuideMove.ChangeModuleName("固晶模组", true);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUIControl(this.stepIndex);
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            BondDevicePara.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "问题",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleCleanPosTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            ucGuideMove.Dispose();  
            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 刮胶盘伸出缩回
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnMoveSlideFulxer_Click(object sender, EventArgs e)
        {
            if (this.slideFluxerController.IsSlideFluxerAtNLimit())
            {
                this.slideFluxerController.SlideFluxerOutNoWait();
                this.BtnMoveSlideFulxer.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                this.slideFluxerController.SlideFluxerHomeNoWait();
                this.BtnMoveSlideFulxer.Appearance.BackColor = default;
            }
        }
    }
}