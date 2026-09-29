using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.FlipTeach
{
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    /// <summary>
    /// 翻转工具示教窗体
    /// </summary>
    public partial class FrmFlipToolTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tool">翻转工具</param>
        public FrmFlipToolTeach(FlipTool tool)
        {
            InitializeComponent();
            this.flipTool = tool;
            this.InitControl();
        }

        /// <summary>
        /// 传进来的对象
        /// </summary>
        private FlipTool flipTool;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 3;

        /// <summary>
        /// 判断当前是否在运动，后续应该搞成通用的防呆方法
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
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 参与计算的点位
        /// </summary>
        private AKRSPoint3D[] points = new AKRSPoint3D[2] { new AKRSPoint3D(), new AKRSPoint3D() };

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmFlipToolTeach");

        /// <summary>
        /// 是否弹出
        /// </summary>
        /// <returns>结果</returns>
        public bool IsShowDialog()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return true;
            }

            if (string.IsNullOrEmpty(this.flipTool.MeasureHeightNozzle))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先选择测高吸嘴！",
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return false;
            }

            //if (string.IsNullOrEmpty(this.flipTool.MeasureHeightEjection))
            //{
            //    DialogResult dialog = AKRSXtraMessageBox.Show(
            //        $"请先选择测高顶针！",
            //        "警告",
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Warning);

            //    return false;
            //}

            this.flipTableController.FlipTableGoHome();

            // 换吸嘴
            return this.system2Controller.ChangeNozzleAssistance(this.flipTool.MeasureHeightNozzle);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 确定位置第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{stepCount}: 把Bond相机十字框中心移到吸嘴中心，点“确定”，或者把Bond相机十字框中心移到吸左上角，点“下一步”",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: true,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiFlipToolPosition2;

                                if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                {
                                    return;
                                }

                           // 记录第一点位置
                           points[0] = this.bondModuleController.Get3DRealPosition();

                            },
                       doneAction: () =>
                       {
                           this.TileBarTeach.SelectedItem = this.TbiNozzleTouch;

                           this.stepIndex += 2;

                           if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                           {
                               return;
                           }

                           // 计算吸嘴交接位置XY
                           this.flipTool.FlipToolPositionXY.X =
                               this.bondModuleController.Get2DRealPosition().X
                               + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X;

                           this.flipTool.FlipToolPositionXY.Y =
                               this.bondModuleController.Get2DRealPosition().Y
                               + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y;

                           // 焊头移动到吸嘴交接位
                           AKRSPoint3D touchPos = new AKRSPoint3D()
                                                      {
                                                          X = (this.flipTool.FlipToolPositionXY
                                                               - bondHeadController.GetCurrentNozzle().NozzleOffset).X,
                                                          Y = (this.flipTool.FlipToolPositionXY
                                                               - bondHeadController.GetCurrentNozzle().NozzleOffset).Y,
                                                          Z = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z
                                                      };

                           this.bondHeadController.RotateAxisT(this.bondHeadController.GetCurrentNozzle().AlignAngle);
                           this.bondModuleController.MoveSafeBondXYZ(touchPos);
                       }),
                 
                    // 确定位置第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{stepCount}:把Bond相机十字框中心移到吸嘴右下角，点“下一步”",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiFlipToolPosition1;
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiNozzleTouch;

                            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                            {
                                return;
                            }

                            // 记录第2点位置
                            points[1] = this.bondModuleController.Get3DRealPosition();

                            // 两点距离大报警（0.1mm）
                            if (Math.Abs(points[0].X - points[1].X) > 5)
                            {
                                DialogResult dialog = AKRSXtraMessageBox.Show(
                                    $"两点距离过大.\r\nOK：从头开始示教\r\nCancel:结束示教.",
                                    "Warn",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning);

                                if (dialog == DialogResult.OK)
                                {
                                    // 返回到第一步
                                    // 因为点next会自动加1，所以这里是-1
                                    this.stepIndex -= 1;
                                    this.TileBarTeach.SelectedItem = this.TbiRotationMeasurement2;
                                    return;
                                }
                                else
                                {
                                    // 清除数据
                                    this.points = new AKRSPoint3D[2];
                                    DialogResult = DialogResult.Cancel;
                                }
                            }

                            // 计算吸嘴交接位置XY
                            this.flipTool.FlipToolPositionXY.X =
                                (points[0].X + points[1].X) / 2
                                + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X;
                            this.flipTool.FlipToolPositionXY.Y =
                                (points[0].Y + points[1].Y) / 2
                                + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y;

                             // 焊头移动到吸嘴交接位
                             AKRSPoint3D touchPos = new AKRSPoint3D()
                                                        {
                                                            X = (this.flipTool.FlipToolPositionXY
                                                                 - bondHeadController.GetCurrentNozzle().NozzleOffset)
                                                                .X,
                                                            Y = (this.flipTool.FlipToolPositionXY
                                                                 - bondHeadController.GetCurrentNozzle().NozzleOffset)
                                                                .Y,
                                                            Z = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z
                                                        };

                          this.bondHeadController.RotateAxisT(this.bondHeadController.GetCurrentNozzle().AlignAngle);
                          this.bondModuleController.MoveSafeBondXYZ(touchPos);
                        },
                        doneAction: () =>
                        {
                        }),

                    // 吸嘴接触
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"3/{stepCount}: 将焊头吸嘴移向翻转台，微调翻转台角度使翻转吸嘴和焊头吸嘴在同一垂直线上，在翻转吸嘴正上方5mm处点“确定”进行测高。\r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiFlipToolPosition2;
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

                                Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

                                (ExcuteResult excuteResult, double height) ret = this.bondHeadController.MeasureHeight(
                                    BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z,
                                    HeightMeasurementFunctionEnum.WithTDSensor);

                                if (ret.excuteResult != ExcuteResult.Success)
                                {
                                    this.stepIndex--;
                                    return;
                                }

                                // 吸嘴交接高度
                                this.flipTool.FlipToolPositionZ =
                                    ret.height - nozzle.MeasureHeightOffset;

                                // 翻转吸嘴交接角度
                                this.flipTool.FlipToolTransferPos = this.flipTableController.GetTRealPos();

                                this.bondModuleController.MoveToSafePos();

                                //// 方向盘设置模组名称
                                //ucGuideMove1.ChangeModuleName("翻转台模组", true);

                                //this.TileBarTeach.SelectedItem = this.TbiEjectionTouch;

                                this.flipTool.FlipToolAssistant.State = AssistantStateEnum.Able;

                                this.flipTableController.FlipTableGoHome();

                                this.DialogResult = DialogResult.OK;
                            }),

                    //// 顶针接触
                    //new AssistantConfig(
                    //    index: 3,
                    //    descritpion: $"4/{stepCount}:将翻转工具移向顶针，使翻转吸嘴和顶针重合，如果是华夫盒芯片或者不需要顶针，直接点“确定”结束示教。\r\n",
                    //    isShowTitle: true,
                    //    isShowBack: true,
                    //    isShowNext: false,
                    //    isShowDone: true,
                    //    backAction: () =>
                    //        {
                    //            this.TileBarTeach.SelectedItem = this.TbiNozzleTouch;
                    //        },
                    //    nextAction: () =>
                    //        {
                    //        },
                    //    doneAction: () =>
                    //        {
                    //            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                    //            {
                    //                this.flipTool.FlipToolAssistant.State = AssistantStateEnum.Able;
                    //                this.DialogResult = DialogResult.OK;

                    //                return;
                    //            }

                    //            // 记录接触位置
                    //            this.flipTool.FlipToolWaferHeight = this.flipTableController.GetTRealPos();

                    //            this.flipTableController.MoveFlipTAxis(0);

                    //            // 归还顶针
                    //            WaferSubController.GetInstance().EjectController.ReturnEjection();

                    //            this.flipTool.FlipToolAssistant.State = AssistantStateEnum.Able;

                    //            this.DialogResult = DialogResult.OK;
                    //        }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiFlipToolPosition1;

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
                "Question",
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
            this.SetUIControl(this.stepIndex);
            FlipToolRepository.GetInstance().Save();
        }

        /// <summary>
        /// 自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            this.bondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
        }
    }
}