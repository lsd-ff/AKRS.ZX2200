using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.WM;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.PostBondInspection;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using DevExpress.DashboardWin.Native;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    public partial class FrmBackSideCrackDetectionTeach : DevExpress.XtraEditors.XtraForm
    {
        public FrmBackSideCrackDetectionTeach(PostBondInspection postBondInspection)
        {
            InitializeComponent();
            this.InitControl();
            this.postBondInspection = postBondInspection;
        }

        /// <summary>
        /// 焊后
        /// </summary>
        private PostBondInspection postBondInspection;

        /// <summary>
        /// System2Controller
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 编辑PR
        /// </summary>
        private FrmPREditor frmPREditor;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 拉角度第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/2: 点击“定位并移动到芯片中心”按钮后，进入下一步。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiEditPR;
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 拉角度第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/2:编辑PR后点击“确认”。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                                      this.TileBarTeach.SelectedItem = this.TbiAdjust;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                            {
                                this.postBondInspection.VisionConfig.BacsideCrackDetectPRName =
                                    this.postBondInspection.Name + "BCD";

                                MatchResult result1 = (MatchResult)this.system2Controller.BondCameraVision(
                                    null,
                                    this.postBondInspection.VisionConfig.P1PRName);

                                if (result1.IsSuccess == false)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"视觉模板：{ this.postBondInspection.VisionConfig.P1PRName}定位失败！请先重新示教定位模板！",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    this.DialogResult = DialogResult.Cancel;
                                }

                                bool isCrack = this.system2Controller.BacksideCrackDetect(
                                   result1,
                                   this.postBondInspection.VisionConfig.BacsideCrackDetectPRName);

                               string ret = isCrack ? "背崩" : "正常";


AKRSXtraMessageBox.Show($" 背崩检测结果为:{ret}！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                            this.DialogResult = DialogResult.OK;
                        })
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiAdjust;

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
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            this.postBondInspection.BacksideCrackDetection.State = AssistantStateEnum.Able;
            assistantConfig.DoneAction();
            CarrierConfigRepository.GetInstance().Save();
            PostBondInspectionRepository.GetInstance().Save();
        }

        /// <summary>
        /// 移动到芯片中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToComponentCenter_Click(object sender, EventArgs e)
        {
            MatchResult result = (MatchResult)this.system2Controller.BondCameraVision(
             null,
           this.postBondInspection.VisionConfig.P1PRName);

            if (result == null)
            {
                AKRSXtraMessageBox.Show($" {this.postBondInspection.VisionConfig.P1PRName}定位失败！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AKRSPoint3D point = this.bondModuleController.ConvertPixelToG0Pos(
            this.bondModuleController.Get3DRealPosition(),
            result);

            // 去芯片中心
            this.bondModuleController.MoveToG0Pos(point);
        }

        /// <summary>
        ///  编辑PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditPr_Click(object sender, EventArgs e)
        {
            string path = this.postBondInspection.Name + "BCD";

            // 先加载库里的prentity,如果存在则加载,如不存在,则创建新的
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(path);
            BaseVisionEntity visionEntity = null;
            if (pREntity == null)
            {
                pREntity = new PREntity(path);
                pREntity.Alg.AlgBeLong = AlgBeLongEnum.PostBond;
                visionEntity = pREntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }
            else
            {
                visionEntity = pREntity;
            }

            // 设置Bond硬件
            pREntity.SetHardware(this.system2Controller.GetHardware(CameraTypeEnum.BondCamera));

            this.frmPREditor = new FrmPREditor((PREntity)visionEntity, false);
            this.frmPREditor.ShowDialog();
            this.frmPREditor.Dispose();
        }
    }
}
