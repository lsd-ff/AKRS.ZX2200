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
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    /// <summary>
    /// IPT 示教
    /// </summary>
    public partial class FrmLearnIPT : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLearnIPT(IPTTypeEnum iptType)
        {
            this.iptType = iptType;
            this.InitializeComponent();
           this.InitControl();
        }

        /// <summary>
        /// 中转台类型
        /// </summary>
        private IPTTypeEnum iptType = IPTTypeEnum.LeftIPT;

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

             ucGuideMove = new UcGuideMove("固晶模组", "FrmLearnIPT", true, CameraEnum.BondCamera);
             this.PnlControl.Controls.Add(ucGuideMove);

             string info = this.iptType == IPTTypeEnum.LeftIPT ? "左中转台" : "右中转台";

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"Step1/2:移动焊头使Bond相机看到{info}中心圆。",
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

                                AKRSPoint3D posInG0 = this.bondModuleController.GetG0RealPosition();

                                AKRSPoint3D IPTPos = new AKRSPoint3D();

                                if (this.iptType == IPTTypeEnum.LeftIPT)
                                {
                                    // 保存拍照位
                                    BondDevicePara.GetInstance().IPTDevicePara.IPTVisionPos = posInG0;

                                    // 加上焊头偏移
                                    IPTPos =
                                        posInG0 + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                    // 保存
                                    BondDevicePara.GetInstance().IPTDevicePara.IPTPos.X = IPTPos.X;
                                    BondDevicePara.GetInstance().IPTDevicePara.IPTPos.Y = IPTPos.Y;
                                    BondDevicePara.GetInstance().IPTDevicePara.IPTPos.Z = IPTPos.Z;
                                }
                                else
                                {
                                    // 保存拍照位
                                    BondDevicePara.GetInstance().IPTDevicePara.RightIPTVisionPos = posInG0;

                                    // 加上焊头偏移
                                    IPTPos =
                                        posInG0 + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                    // 保存
                                    BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos.X = IPTPos.X;
                                    BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos.Y = IPTPos.Y;
                                    BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos.Z = IPTPos.Z;
                                }

                                // 移动到测高位
                                AKRSPoint3D measureHeight =
                                    IPTPos + new AKRSPoint3D(0, 0, 5);
                                this.bondModuleController.MoveToG0Pos(measureHeight);

                                this.TileBarTeach.SelectedItem = this.TbiMeasureHeight;
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"Step2/2:在{info}上测高。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiDetermineCenter;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                        {
                            // 执行测高
                            (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                HeightMeasurementFunctionEnum.WithTDSensor);

                            if (res.Ret == ExcuteResult.Success)
                            {
                                if (this.iptType == IPTTypeEnum.LeftIPT)
                                {
                                    // 保存测高结果
                                    BondDevicePara.GetInstance().IPTDevicePara.IPTPos.Z =
                                        this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);
                                }
                                else
                                {
                                    // 保存测高结果
                                    BondDevicePara.GetInstance().IPTDevicePara.RightIPTPos.Z =
                                        this.bondModuleController.ConvertMachineToG0Pos(res.HeightValue);
                                }

                                this.DialogResult = DialogResult.OK;
                            }
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
        private void FrmLearnIPT_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 自动归还
            this.system2Controller.PutbackNozzleAssitance();
            this.ucGuideMove.Dispose();
        }

        /// <summary>
        /// 移动到模板中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToCameraCenter_Click(object sender, EventArgs e)
        {
            // 判断一下是否做了模板
            PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find("IPTCenterCircle");
            if (pREntity == null)
            {
                DialogResult dialog1 = AKRSXtraMessageBox.Show(
                    $"请先编辑视觉模板!",
                    "Warn",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);

                return;
            }

            // 执行定位
            MatchResult result = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                null,
                "IPT teach",
                "IPTCenterCircle",
                false,
                CameraTypeEnum.BondCamera);

            if (result == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"视觉定位失败，请检查PR!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // 获取G0坐标
            AKRSPoint3D posInG0 = System2Module.GetInstance().BondModule.ConvertPixelToG0Pos(
                this.bondModuleController.Get3DRealPosition(),
                result);

            // 去相机中心
            this.bondModuleController.MoveToG0Pos(posInG0.X, posInG0.Y);
        }
    }
}