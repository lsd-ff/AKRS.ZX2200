using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.CalibSystem.Services;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.BaseModels;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    using VM.PlatformSDKCS;

    /// <summary>
    /// 吸嘴半径示教窗体
    /// </summary>
    public partial class FrmToolGeometryTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nozzle">吸嘴对象</param>
        public FrmToolGeometryTeach(Nozzle nozzle)
        {
            this.nozzle = nozzle;
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
        /// BondModule控制器
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

        /// <summary>
        /// 点击Start传进来的吸嘴对象
        /// </summary>
        private Nozzle nozzle;

        /// <summary>
        /// 视觉模板库窗体
        /// </summary>
        private FrmPRList frmPRList;

        /// <summary>
        /// 吸嘴标定走的点位
        /// </summary>
        private List<AKRSPoint2D> pointList = new List<AKRSPoint2D>();

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove2 = new UcGuideMove("FrmToolGeometryTeach");

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            this.Size = new Size(749, 850);
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove2);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: this.nozzle.NozzleShape == NozzleShapeEnum.Round ? $"Step1/3:将吸嘴孔圆上一点对准相机中心十字线。" : $"Step1/2:将吸嘴孔左上角对准相机中心十字线。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCenterPoint2;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                                // 记录第一点位置
                                this.pointList.Add(this.bondModuleController.Get2DRealPosition());
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: this.nozzle.NozzleShape == NozzleShapeEnum.Round ? $"Step2/3:将吸嘴孔圆上第二点对准相机中心十字线。" : $"Step2/2:将吸嘴孔右下角对准相机中心十字线。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: this.nozzle.NozzleShape == NozzleShapeEnum.Round,
                        isShowDone: this.nozzle.NozzleShape != NozzleShapeEnum.Round,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiCenterPoint1;
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiCenterPoint3;

                            // 记录第二点位置
                            this.pointList.Add(this.bondModuleController.Get2DRealPosition());
                        },
                        doneAction: () =>
                        {
                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                this.DialogResult = DialogResult.OK;
                                return;
                            }

                            // 记录第二点位置
                            this.pointList.Add(this.bondModuleController.Get2DRealPosition());

                            if (this.nozzle.ToolGeometry == null)
                            {
                                this.nozzle.ToolGeometry = new AKRSPoint3D();
                            }

                            // 计算结果
                            if (this.nozzle.NozzleShape == NozzleShapeEnum.Rectangle)
                            {
                                // 两点确定矩形长宽
                                this.nozzle.ToolGeometry.X = this.pointList[1].X - this.pointList[0].X;
                                this.nozzle.ToolGeometry.Y = this.pointList[1].Y - this.pointList[0].Y;

                                this.nozzle.NozzleOffset.X =
                                    this.nozzle.RectangleNozzleCornerOffset.X - this.nozzle.ToolGeometry.X / 2;
                                this.nozzle.NozzleOffset.Y =
                                    this.nozzle.RectangleNozzleCornerOffset.Y - this.nozzle.ToolGeometry.Y / 2;

                                this.nozzle.EditState = EditStateEnum.Completely;
                                NozzleRepository.GetInstance().Save();
                            }

                            //this.nozzle.NozzleCenterPos.X = (this.pointList[1].X + this.pointList[0].X) / 2;
                            //this.nozzle.NozzleCenterPos.Y = (this.pointList[1].Y + this.pointList[0].Y) / 2;

                            this.DialogResult = DialogResult.OK;
                        }),

                    // 第三点
                    new AssistantConfig(
                        index: 1,
                        descritpion: "Step3/3:将吸嘴孔圆上第三点对准相机中心十字线。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiCenterPoint2;
                            },
                        nextAction: () =>
                            {
                            },
                        doneAction: () =>
                            {
                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    this.DialogResult = DialogResult.OK;
                                    return;
                                }

                                // 记录第三点位置
                                this.pointList.Add(this.bondModuleController.Get2DRealPosition());

                                double centerX;
                                double centerY;

                                try
                                {
                                    // 计算结果,三点找圆
                                    CalibService.FitCircle(this.pointList, out centerX, out centerY, out var radius);

                                    if(this.nozzle.ToolGeometry == null)
                                    {
                                        this.nozzle.ToolGeometry = new AKRSPoint3D();
                                    }

                                    // 记录结果
                                    this.nozzle.ToolGeometry.Z = radius;
                                    this.nozzle.EditState = EditStateEnum.Completely;
                                    NozzleRepository.GetInstance().Save();
                                }
                                catch (Exception e)
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"三点拟合圆形失败!",
                                        "Warn",
                                        MessageBoxButtons.OKCancel,
                                        MessageBoxIcon.Question);

                                    if (dialog == DialogResult.OK)
                                    {
                                        this.stepIndex = 1;
                                        this.TileBarTeach.SelectedItem = this.TbiCenterPoint2;

                                        return;
                                    }

                                    this.DialogResult = DialogResult.Cancel;
                                    return;
                                }

                                this.DialogResult = DialogResult.OK;
                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiCenterPoint1;

            // 方向盘设置模组名称
            ucGuideMove2.ChangeModuleName("上视模组", true);

            this.TbiCenterPoint3.Visible = this.nozzle.NozzleShape == NozzleShapeEnum.Round;
            this.BtnVacuum.Appearance.BackColor = Color.Transparent;
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

            // 判断吸嘴示教状态
            if (this.nozzle.ToolAlignment.State == AssistantStateEnum.Able)
            {
                // 移到吸嘴旋转中心
                this.bondModuleController.MoveToG0Pos(this.nozzle.RotateCenterPos);
            }
            else
            {
                // 移动到上视旋转中心
                this.bondModuleController.MoveToUpLookPos();
            }
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
        private void FrmNozzleGeometryTeach_FormClosed(object sender, FormClosedEventArgs e)
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
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmNozzleGeometryTeach_FormClosing(object sender, FormClosingEventArgs e)
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
            this.nozzle.ToolGeometryAssistant.State = AssistantStateEnum.Able;
            NozzleRepository.GetInstance().Save();
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
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 开关真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnVacuum_Click(object sender, EventArgs e)
        {
            if (!this.bondHeadController.GetNozzleVacuumState())
            {
                // 打开吸嘴真空
               this.bondHeadController.OpenToolVaccum();
                this.BtnVacuum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                // 关吸嘴真空
                this.bondHeadController.CloseToolVaccum();
                this.BtnVacuum.Appearance.BackColor = Color.Transparent;
            }
        }
    }
}