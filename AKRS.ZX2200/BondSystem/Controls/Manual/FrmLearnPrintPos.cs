using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.BondSystem.Services;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// 蘸胶位置示教
    /// </summary>
    public partial class FrmLearnPrintPos : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLearnPrintPos()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

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
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController = new NozzleShelfController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            ucGuideMove = new UcGuideMove("固晶模组", "FrmLearnPrint", true, CameraEnum.BondCamera);
            this.PnlControl.Controls.Add(ucGuideMove);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"Step1/1:将TouchDown吸嘴移动到蘸胶的位置",
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
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                AKRSPoint3D posInG0 = this.bondModuleController.GetG0RealPosition();

                                // 保存拍照位
                                BondDevicePara.GetInstance().S2DispenseDevicePara.DippingPosition = posInG0;

                                BondDevicePara.GetInstance().Save();
                          this.DialogResult = DialogResult.OK;
                       }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiDetermineCenter;

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
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            BondDevicePara.GetInstance().Save();
        }

        /// <summary>
        /// 相机自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
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
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// 制作PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            TUAssistantHelper.EditPrInSystem2("IPTCenterCircle", CameraTypeEnum.BondCamera);
        }

        /// <summary>
        /// 下一步
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
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void frmLearnPrintPos_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 自动归还
            this.system2Controller.PutbackNozzleAssitance();
            this.ucGuideMove.Dispose();
        }
    }
}