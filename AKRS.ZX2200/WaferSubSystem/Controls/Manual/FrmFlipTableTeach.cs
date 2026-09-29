using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Manual
{
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.WaferSubSystem.Controllers;

    /// <summary>
    /// 示教翻转台
    /// </summary>
    public partial class FrmFlipTableTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmFlipTableTeach()
        {
            InitializeComponent();
            InitControl();
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
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            ucGuideMove = new UcGuideMove("FrmFlipTableTeach");
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System2);
            this.PnlControl.Controls.Add(ucGuideMove);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: "Step1/3:将touchdown移动到翻转台左上位置,点击“下一步”。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerRightBottomPos;

                             if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            WaferSubDevicePara.GetInstance().FlipChipDevicePara.FlipTableLeftTopPos =
                                this.bondModuleController.GetG0RealPosition();
                        },
                        doneAction: () =>
                        {
                        }),

                       new AssistantConfig(
                        index: 1,
                        descritpion: "Step2/3:将touchdown移动到翻转台右下位置,点击“下一步”。",
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
                            this.TileBarTeach.SelectedItem = this.TbiAvoidancePos;
                            
                            // 方向盘设置模组名称
                            ucGuideMove.ChangeModuleName("翻转台模组", true);

                            WaferSubDevicePara.GetInstance().FlipChipDevicePara.FlipTableRightBottomPos  =
                                this.bondModuleController.GetG0RealPosition();

                            // 去安全位避让
                            this.bondModuleController.MoveToSafePos();

                            // 归还顶针，晶圆台去准备位置
                            WaferSubController.GetInstance().EjectController.ReturnEjection();
                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

                            // 弹出视觉窗体
                            UcMainSystem.VmVisionShow();
                            UcMainSystem.ChangeCameraVision("晶圆相机");
                        },
                        doneAction: () =>
                        {
                        }),

                       new AssistantConfig(
                           index: 2,
                           descritpion: "Step3/3:将翻转台翻转到翻转吸嘴刚好没有遮挡到晶圆相机搜晶的极限位置,点击“确定”。",
                           isShowTitle: true,
                           isShowBack: true,
                           isShowNext: false,
                           isShowDone: true,
                           backAction: () =>
                               {
                                   this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerRightBottomPos;
                                   ucGuideMove.ChangeModuleName("固晶模组", true);
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

                                   WaferSubDevicePara.GetInstance().FlipChipDevicePara.SearchComponentAvoidancePos  =
                                       this.flipTableController.GetTRealPos();
                                   WaferSubDevicePara.GetInstance().FlipChipDevicePara.IsCompleted = true;
                               }),
                };


            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiTeachSlideFluxerLeftTopPos;

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
            WaferSubDevicePara.GetInstance().Save();
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
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmFlipTableTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                this.bondModuleController.MoveToSafePos();
                this.flipTableController.FlipTableGoHome();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(
                    $"窗体关闭异常！：{exception.ToString()}",
                    "异常",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            ucGuideMove?.Dispose();  
        }
    }
}