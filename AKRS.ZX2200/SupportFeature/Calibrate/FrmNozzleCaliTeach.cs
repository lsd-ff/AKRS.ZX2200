using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;

namespace AKRS.ZX2200.SupportFeature.Calibrate
{
    using AKRS.Galaxy2.Infrastructure.CustomControls.Components;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 吸嘴标定矫正示教
    /// </summary>
    public partial class FrmNozzleCaliTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNozzleCaliTeach(Nozzle nozzle)
        {
            InitializeComponent();
            this.nozzle = nozzle;
            this.InitControl();
        }

        /// <summary>
        ///  吸嘴
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 1;


        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller System2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// UpLook控制器
        /// </summary>
        private UpLookController UpLookController => System2Domain.GetInstance().UpLookController;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmNozzleCaliTeach");

        /// <summary>
        /// 是否弹出
        /// </summary>
        /// <returns>结果</returns>
        public bool IsShowDialog()
        {
            if (this.nozzle.IsAssistantSucceed == false)
            {
                AKRSXtraMessageBox.Show(
                    $"  请先完成吸嘴:{this.nozzle.Name}示教！",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            if (this.nozzle.NozzleToUplookCenterPos.IsEmpty)
            {
                AKRSXtraMessageBox.Show(
                    $"  请重新示教吸嘴:{this.nozzle.Name}！",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return false;
            }

            // 吸嘴检查
            if (!this.System2Controller.ChangeNozzleAssistance(this.nozzle.Name))
            {
                return false;
            }

            // 去旋转中心
          this.BondModuleController.MoveToG0Pos(this.nozzle.NozzleToUplookCenterPos);
          this.BondHeadController.RotateAxisT(this.nozzle.AlignAngle);

            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove1);
            this.ucGuideMove1.ChangeCamera(CameraEnum.UplookCamera);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 拉角度第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount}: 编辑吸嘴PR模板，点“确定”执行定位。",
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
                           // 检查PR
                           PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.nozzle.NozzlePRName);

                           if (pREntity == null)
                           {
                               DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                   $"请先编辑视觉模板!",
                                   "Warn",
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Warning);

                               if (dialog1 == DialogResult.Cancel)
                               {
                                   this.DialogResult = DialogResult.Cancel;
                               }

                               return;
                           }

                           // 执行定位
                           MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                               null,
                               "NozzleCali",
                               this.nozzle.NozzlePRName,
                               false,
                               CameraTypeEnum.UpLookCamera);

                           if (result == null || !result.IsSuccess)
                           {
                               AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                               return;
                           }

                           // 获取G0坐标
                           AKRSPoint3D posInG0 = this.UpLookController.ConvertPixelToG0Pos(
                               this.BondModuleController.Get3DRealPosition(),
                               result);

                           // 刷新拍照位
                           this.nozzle.NozzleToUplookCenterPos = posInG0;

                           this.DialogResult = DialogResult.OK;
                       }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiEditPr;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("上视模组", true);
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
            assistantConfig.DoneAction();
            NozzleRepository.GetInstance().Save();
        }

        /// <summary>
        /// 自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.BondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditPR_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            TUAssistantHelper.EditPrInSystem2(this.nozzle.NozzlePRName, CameraTypeEnum.UpLookCamera);
        }


        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleCaliTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.BondModuleController.MoveToSafePos();
            ucGuideMove1?.Dispose();
        }
    }
}
