using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;

    /// <summary>
    /// 示教ULM 点位
    /// </summary>
    public partial class FrmTeachULMPositions : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmTeachULMPositions()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 点位
        /// </summary>
        private ULMPara ulmPara = BondDevicePara.GetInstance().ULMPara;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 6;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 轴是否准备好
        /// </summary>
        private bool isReady;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmTeachULMPositions");

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove1);

            //if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated == false) 
            //{
            //    this.stepCount--;
            //    this.tileBarGroup4.Items.Remove(this.TbiAboveIPTRightPos);
            //}

            if (MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated == false)
            {
                this.stepCount--;
                this.tileBarGroup4.Items.Remove(this.TbiRightIPTLeftPos);
            }

            int index = 0;

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 流道外延
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ +1}/{this.stepCount}:示教流道外延位置（焊头Y超出流道但不能超出顶针）。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiAboveRingLightPos;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.ulmPara.TransportUnitEdgePos = this.bondModuleController.Get3DRealPosition();
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 示教晶圆环光正上方位置
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ +1}/{this.stepCount}:示教晶圆环光正上方位置(高于环光)。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTransportUnitEdgePos;
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiWaferRingLightLeftLimitPos;

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            this.ulmPara.AboveWaferRingLightPos = this.bondModuleController.Get3DRealPosition();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 晶圆环光左限
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ +1}/{this.stepCount}:示教晶圆环光左极限位置(高于环光)。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiAboveRingLightPos;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiWaferRingLightRightLimitPos;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.ulmPara.WaferRingLightLeftLimitPos = this.bondModuleController.Get3DRealPosition();
                            },
                        doneAction: () =>
                            {
                            }),

                    // 晶圆环光右限
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ +1}/{this.stepCount}:示教晶圆环光右极限位置(高于环光)。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiWaferRingLightLeftLimitPos;
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiAboveIPTRightPos;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.ulmPara.WaferRingLightRightLimitPos = this.bondModuleController.Get3DRealPosition();
                            },
                        doneAction: () =>
                            {
                            })
                };

            // 左中转台
            //if (MachineHardwareConfiguration.GetInstance().IsIPTConfigrated)
            {
                this.assistantConfigList.Add(
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ + 1}/{this.stepCount}:示教左中转台右上方或翻转台正上方位置。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiWaferRingLightRightLimitPos;
                            },
                        nextAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.ulmPara.AboveIPTRightPos = this.bondModuleController.Get3DRealPosition();
                            },
                        doneAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                this.ulmPara.AboveIPTRightPos = this.bondModuleController.Get3DRealPosition();
                            }));
            }

            // 右中转台
            if (MachineHardwareConfiguration.GetInstance().IsRightIPTConfigrated)
            {
                this.assistantConfigList.Add(
                    new AssistantConfig(
                        index: index,
                        descritpion: $"Step{index++ + 1}/{this.stepCount}:示教右中转台左上方位置。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiWaferRingLightRightLimitPos;
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

                                this.ulmPara.AboveRightIPTLeftPos = this.bondModuleController.Get3DRealPosition();
                            }));
            }

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiTransportUnitEdgePos;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("固晶模组", true);
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
                $"结束示教?",
                "Question",
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
        private void FrmTeachULMPositions_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove1?.Dispose();
            this.timer1?.Dispose();
            this.system2Controller.PutbackNozzleAssitance();
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup4, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }
    }
}