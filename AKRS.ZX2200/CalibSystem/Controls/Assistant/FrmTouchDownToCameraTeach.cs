using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Controls;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.Drawing;

    using AKRS.ZX2200.CalibSystem.Models;

    /// <summary>
    /// 吸嘴标定示教窗体,矩形吸嘴和圆形吸嘴标定方法相同
    /// </summary>
    public partial class FrmTouchDownToCameraTeach : DevExpress.XtraEditors.XtraForm
    {

        /// <summary>
        /// 运行参数
        /// </summary>
        private CalibrateRunPara CalibrateRunPara => CalibrateRunPara.GetInstance();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nozzle">吸嘴对象</param>
        public FrmTouchDownToCameraTeach()
        {
            this.InitializeComponent();
            this.InitControl();
            this.InitMovement();
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
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 点击Start传进来的吸嘴对象
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 吸嘴定位结果(机械坐标)
        /// </summary>
        private AKRSPoint2D wordPos1, wordPos2;

        /// <summary>
        /// 轴是否准备好
        /// </summary>
        private bool isReady;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmTouchDownToCameraTeach");

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: "Step1/2:将相机中心对齐TouchDown中心。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiAlignCorner2;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录当前坐标
                                this.wordPos1 = this.bondModuleController.Get2DRealPosition();

                                try
                                {
                                    double angle = this.bondHeadController.GetAxisTRealPos();

                                    if (this.bondHeadController.GetAxisTRealPos() <= 0)
                                    {
                                        // T轴转180度
                                        this.bondHeadController.RotateAxisT(angle + 180);
                                    }
                                    else
                                    {
                                        // T轴转-180度
                                        this.bondHeadController.RotateAxisT(angle - 180);
                                    }
                                }
                                catch (Exception e)
                                {
                                    DialogResult dialog = XtraMessageBox.Show(
                                        $"T轴旋转失败:" + e.ToString(),
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    throw e;
                                }
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: "Step2/2:再次将相机中心对齐TouchDown中心",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            // T轴往回转180度
                            this.bondHeadController.RelativeRotateAxisT(-180);

                            this.TileBarTeach.SelectedItem = this.TbiAlignCorner1;
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

                            // 记录当前坐标
                                this.wordPos2 = this.bondModuleController.Get2DRealPosition();

                                // 计算结果
                                CalibrateRunPara.BondCenterToTouchDownOffset.X = (this.wordPos2.X - this.wordPos1.X) / 2;
                                CalibrateRunPara.BondCenterToTouchDownOffset.Y = (this.wordPos2.Y - this.wordPos1.Y) / 2;

                                CalibrateRunPara.Save();
                        }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiAlignCorner1;

            // 方向盘设置模组名称
            ucGuideMove1.ChangeModuleName("上视模组", true);

            this.Size = new Size(749, 900);
        }

        /// <summary>
        /// 初始运动
        /// </summary>
        private void InitMovement()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // T轴转到0度
            this.bondHeadController.RotateAxisT(0);

            // 移动到上视旋转中心
            this.bondModuleController.MoveToUpLookPos();

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
        /// 关闭窗体后事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleAlignmentTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.isMove)
            {
                // 关闭安全门
                return;
            }
        }

        /// <summary>
        /// 关闭窗体时事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void FrmNozzleAlignmentTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 如果轴正运动，不可取消
            if (this.isMove)
            {
                e.Cancel = true;
            }
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
           // this.nozzle.ToolAlignment.State = AssistantStateEnum.Able;
           // NozzleRepository.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = XtraMessageBox.Show(
                $"是否结束示教?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        /// <summary>
        ///  编辑模版
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            string name = this.nozzle.NozzleShape == NozzleShapeEnum.Round ? "圆搜索" : "矩形搜索";

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(name);
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }

        /// <summary>
        /// 自动搜索中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string name = this.nozzle.NozzleShape == NozzleShapeEnum.Round ? "圆搜索" : "矩形搜索";

            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(name);
            if (prEntity == null)
            {
                DialogResult dialog = XtraMessageBox.Show(
                    $"请先编辑视觉模板!",
                    "Prompt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

        Retry:

            // 执行定位
            MatchResult matchResult = (MatchResult)this.system2Controller.UpLookCameraVision(null, name, true);

            if (matchResult == null)
            {
                DialogResult dialog = XtraMessageBox.Show(
                    $"上视相机定位失败!",
                    "Error",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Error);

                switch (dialog)
                {
                    case DialogResult.Retry:
                        goto Retry;

                    case DialogResult.Cancel:
                        return;

                    default: return;
                }
            }

            AKRSPoint3D posInG0 = this.system2Controller.GetUplookVisionResultPos(matchResult, null);

            AKRSPoint3D pos = this.bondModuleController.ConvertG0ToMachinePos(posInG0);

            // 距离检查
            if (Math.Abs(pos.X - this.bondModuleController.GetAxisXRealPos()) > 3
                || Math.Abs(pos.Y - this.bondModuleController.GetAxisYRealPos()) > 3)
            {
                DialogResult dialog = XtraMessageBox.Show(
                    $"移动距离超过3mm,请重试!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // 移动到模版中心
            this.bondModuleController.MoveBondXY(pos.X, pos.Y);
        }

        /// <summary>
        /// 自动对焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }
    }
}