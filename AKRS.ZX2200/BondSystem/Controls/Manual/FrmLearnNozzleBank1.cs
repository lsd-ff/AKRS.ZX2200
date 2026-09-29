using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;


namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.Galaxy2.PR.Controls;
    using AKRS.Galaxy2.PR.Models.CommonModels;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.Infrastructure.Utils;
    using DevExpress.Utils.Extensions;

    /// <summary>
    /// 吸嘴架示教窗体
    /// 新治具示教吸嘴架，待测试
    /// </summary>
    public partial class FrmLearnNozzleBank1 : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLearnNozzleBank1()
        {
            this.InitializeComponent();
            this.InitControl();
            this.InitMovement();
        }

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 6;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private System2Controller system2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 吸嘴架参数
        /// </summary>
        private NozzleShelfParam nozzleShelfPara => BondDevicePara.GetInstance().NozzleShelfParam;

        /// <summary>
        /// 槽位7拍照位,G0
        /// </summary>
        private AKRSPoint3D slot7VisionPos = new AKRSPoint3D();

        /// <summary>
        /// 槽位1拍照位,G0
        /// </summary>
        private AKRSPoint3D slot1VisionPos = new AKRSPoint3D();

        /// <summary>
        /// 吸嘴架换吸嘴位
        /// </summary>
        private double toolChangePositionInAxis;

        /// <summary>
        /// 换吸嘴角度
        /// </summary>
        private double changeNozzleAngle;

        /// <summary>
        /// 换吸嘴高度
        /// </summary>
        private double changeNozzleLevel;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove = new UcGuideMove("FrmLearnNozzleBank1");

        /// <summary>
        ///  PR名称
        /// </summary>
        private string prName = "吸嘴架示教圆搜索";

        /// <summary>
        ///  是否弹出页面
        /// </summary>
        /// <returns>结果</returns>
        public bool IsShowDialog()
        {
            DialogResult dia;

        Retry1:
            bool isNozzleShelfEmpty = this.nozzleShelfController.IsNozzleShelfEmpty();
            if (isNozzleShelfEmpty==false)
            {
                dia = AKRSXtraMessageBox.Show(
                   "示教吸嘴架之前请先将所有吸嘴移出吸嘴架！",
                   "Error",
                   MessageBoxButtons.OKCancel,
                   MessageBoxIcon.Error);

                if (dia == DialogResult.OK)
                {
                    goto Retry1;
                }

                return false;
            }

            this.bondHeadController.CloseBondHeadVaccum();

        Retry2:
            dia = AKRSXtraMessageBox.Show(
               "示教吸嘴架之前请先将吸嘴架配套标定治具分别放到吸嘴架槽1和焊头上！",
               "Info",
               MessageBoxButtons.OKCancel,
               MessageBoxIcon.Information);

            if (dia == DialogResult.OK)
            {
                bool isSlot1Empty = this.nozzleShelfController.IsSlotHaveNozzle(1);

                if (isSlot1Empty == false)
                {
                    goto Retry2;
                }
            }
            else
            {
                return false;
            }

            this.bondHeadController.OpenBondHeadVaccum();

            return true;
        }

        /// <summary>
        /// 初始运动
        /// </summary>
        private void InitMovement()
        {
            // T轴回0
            this.bondHeadController.MoveTAxisToHome();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.Size = new Size(749, 950);
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(ucGuideMove);

            // 方向盘设置模组名称
            this.ucGuideMove.ChangeModuleName("吸嘴架模组");

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                   // 示教吸嘴架Y轴
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/{this.stepCount}: 将吸嘴架Y轴移动到换吸嘴位置。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiMeasureHeight;

                                // 记录吸嘴架伸出位置
                                this.toolChangePositionInAxis = this.nozzleShelfController.GetYAxisPos();

                                // 方向盘设置模组名称
                                this.ucGuideMove.ChangeModuleName("固晶模组");
                            },
                       doneAction: () =>
                       {
                       }),

                    // 测高
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{this.stepCount}: 将焊头上的测高治具移到吸嘴槽1里的治具正上方5mm进行测高。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiChangePositionY;
                            },
                        nextAction: () =>
                            {
                                if (this.nozzleShelfController.IsSlotHaveNozzle(1) == false)
                                {
                                    AKRSXtraMessageBox.Show(
                                        "吸嘴架槽1未检测到治具！请将治具放入槽内！",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    this.stepCount--;
                                    return;
                                }

                                double liftLevel = this.bondModuleController.Get3DRealPosition().Z;

                                // 测高
                                var ret = this.bondHeadController.MeasureHeight(
                                    liftLevel,
                                    HeightMeasurementFunctionEnum.WithTDSensor);

                                if (ret.Ret != ExcuteResult.Success)
                                {
                                    this.stepCount--;
                                    return;
                                }

                                double safeLevel = ret.HeightValue + 7.9;

                                // 安全位置在测高基础上加7.9mm,参考Plus机台
                                BondDevicePara.GetInstance().BondHeadParam.ChangeNozzleSafePos.Z =
                                    safeLevel;

                                // 检查安全位置是否超过Z轴软极限
                                bool res = this.bondHeadController.CheckZAxisSoftLimitAssitance(
                                    safeLevel);

                                if (res == false)
                                {
                                    AKRSXtraMessageBox.Show(
                                        $"换吸嘴安全高度:{safeLevel} 超过Z轴软限位,请检查限位后重新示教！",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    this.DialogResult = DialogResult.Cancel;
                                    return;
                                }

                                AKRSPoint3D pos = new AKRSPoint3D()
                                                      {
                                                          X = this.bondModuleController.Get3DRealPosition().X,
                                                          Y = this.bondModuleController.Get3DRealPosition().Y,
                                                          Z = ret.HeightValue
                                                      };

                                // 吸嘴架高度
                                this.nozzleShelfPara.NozzleShelfHeight =
                                    this.bondModuleController.ConvertMachineToG0Pos(pos);

                                // 换吸嘴高度=治具测高高度-固定值
                                this.changeNozzleLevel =
                                    this.nozzleShelfPara.NozzleShelfHeight.Z
                                    - this.nozzleShelfPara.TCCaliToolHeightOffset;

                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot1Pos;
                            },
                        doneAction: () =>
                            {
                            }),

                       // 示教槽1拍照位
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"3/{this.stepCount}: 将焊头移至吸嘴架槽1，用Bond相机十字线对准治具的中心。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            // 返回测高
                            this.TileBarTeach.SelectedItem = this.TbiMeasureHeight;

                            this.ucGuideMove.ChangeModuleName("固晶模组");
                        },
                        nextAction: () =>
                        {
                            this.slot1VisionPos = this.bondModuleController.GetG0RealPosition();

                            // 去上视
                              this.bondHeadController.MoveTAxisToHome();
                              this.bondModuleController.MoveToUpLookPos();

                                this.TileBarTeach.SelectedItem = this.TbiFindCenter;
                        },
                        doneAction: () =>
                        {
                        }),

                   // 上视定位
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"4/{this.stepCount}: 将上视相机十字线对准治具中心，点“下一步”。",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachSlot1Pos;
                        },
                        nextAction: () =>
                        {
                            // 保存
                            this.nozzleShelfPara.CaliToolToBondheadCenterOffset =
                                BondDevicePara.GetInstance().CameraDevicePara.UpLookPos
                                - this.bondModuleController.Get3DRealPosition();

                            // 根据治具的尺寸去偏移
                            // todo:治具机械结构还不确定
                              //AKRSPoint2D pos = this.bondModuleController.Get2DG0RealPosition()
                              //                  + this.nozzleShelfPara.TCCaliToolGeometry;
                              //this.bondModuleController.MoveToG0Pos(pos.X, pos.Y);

                                this.TileBarTeach.SelectedItem = this.TbiRotateAngle;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 确定换吸嘴角度
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"5/{this.stepCount}: 旋转T轴使治具凸起部分与相机线平行。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiFindCenter;
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachSlot7Pos;

                            // 保存换吸嘴角度
                            this.changeNozzleAngle = this.bondHeadController.GetAxisTRealPos() - 90;

                            // 测高位置换算轴坐标
                            AKRSPoint3D pos = this.bondModuleController.ConvertG0ToMachinePos(this.slot1VisionPos
                                + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);

                            // 去测高位置
                            this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();
                            this.bondModuleController.MoveBondXY(pos.X, pos.Y);
                        },
                        doneAction: () =>
                        {
                        }),

                    // Slot 7 拍照位
                    new AssistantConfig(
                        index: 5,
                        descritpion: $"6/{this.stepCount}:将吸嘴槽1的治具放入吸嘴槽7，再将焊头移至吸嘴架槽7，用Bond相机十字线对准治具的中心。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                //// 测高位置换算轴坐标
                                //AKRSPoint3D pos = this.bondModuleController.ConvertG0ToMachinePos(this.slot1VisionPos
                                //    + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);

                                //// 去测高位置
                                //this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();
                                //this.bondModuleController.MoveBondXY(pos.X, pos.Y);

                                this.TileBarTeach.SelectedItem = this.TbiRotateAngle;
                            },
                        nextAction: () =>
                            {
                            },
                        doneAction: () =>
                            {
                                if (this.nozzleShelfController.IsSlotHaveNozzle(7) == false)
                                {
                                    AKRSXtraMessageBox.Show(
                                        "吸嘴架槽7未检测到治具！请将治具放入槽内！",
                                        "Error",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);

                                    this.stepCount--;
                                    return;
                                }

                                this.slot7VisionPos = this.bondModuleController.GetG0RealPosition();

                                this.SaveSlotPos();

                                // 去安全位
                                this.bondModuleController.MoveToChangeNozzleSafePos();
                            })
                        {
                    },
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiChangePositionY;

            // 方向盘设置模组名称
            this.ucGuideMove.ChangeModuleName("吸嘴架模组", true);
        }

        /// <summary>
        /// 计算槽位位置
        /// </summary>
        private void SaveSlotPos()
        {
            // 间距
            double xPitch = (this.slot1VisionPos.X
                             - this.slot7VisionPos.X) / 6;
            double yPitch = (this.slot1VisionPos.Y
                             - this.slot7VisionPos.Y) / 6;

            // 顺时针旋转一定角度后治具和焊头中心的偏移量
            AKRSPoint3D toolOffsetRotated = MathHelper.RotateCenter(
                this.nozzleShelfPara.CaliToolToBondheadCenterOffset,
                new AKRSPoint3D(),
                this.changeNozzleAngle);

            // 放吸嘴高度
            for (int i = 0; i < 7; i++)
            {
                // 放吸嘴位置XY=拍照位+焊头中心和相机偏移-治具中心和焊头中心的偏移-槽间距
                this.nozzleShelfPara.PlaceNozzlePos[i].X =
                    this.slot1VisionPos.X + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.X
                    - toolOffsetRotated.X - i * xPitch;

                this.nozzleShelfPara.PlaceNozzlePos[i].Y =
                    this.slot1VisionPos.Y + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset.Y
                    - toolOffsetRotated.Y - i * yPitch;

                this.nozzleShelfPara.PlaceNozzlePos[i].Z = this.changeNozzleLevel;

                this.nozzleShelfPara.PlaceNozzleAngle[i] = this.changeNozzleAngle;

                // 取吸嘴位置XY=放吸嘴位置XY
                this.nozzleShelfPara.PickNozzlePos[i].X = this.nozzleShelfPara.PlaceNozzlePos[i].X;
                this.nozzleShelfPara.PickNozzlePos[i].Y = this.nozzleShelfPara.PlaceNozzlePos[i].Y;

                // 取吸嘴高度=放吸嘴高度-固定差值
                this.nozzleShelfPara.PickNozzlePos[i].Z =
                    this.changeNozzleLevel - this.nozzleShelfPara.PlacePickPosHeightOffset;

                this.nozzleShelfPara.PickNozzleAngle[i] = this.changeNozzleAngle;
            }

            #region 创建吸嘴架坐标系

            AKRSPoint3D upPoint = BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[0];

            AKRSPoint3D downPoint = new AKRSPoint3D(
                upPoint.X,
               this.toolChangePositionInAxis,
                upPoint.Z);

            MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("ToolBankCoordinateSystem", "G0", true, CoordinateSystemTypeEnum.General);

            GeneralCoordinateSystem toolBankCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "ToolBankCoordinateSystem");
            toolBankCoordinateSystem.Init(upPoint, downPoint, 0);

            // 保存吸嘴架换吸嘴位从轴坐标换算到G0
            BondDevicePara.GetInstance().NozzleShelfParam.ToolChangePosition =
                toolBankCoordinateSystem.SelfPosToG0(downPoint);

            #endregion

            // X方向间距
            BondDevicePara.GetInstance().NozzleShelfParam.SlotPitch = xPitch;

            this.nozzleShelfPara.Slot1VisionPos = this.slot1VisionPos;
            this.nozzleShelfPara.Slot7VisionPos = this.slot7VisionPos;

            BondDevicePara.GetInstance().Save();
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
                this.Close();
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
            BondDevicePara.GetInstance().Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 计时器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            bool ret = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor1.GetInputValue();
            this.BtnSlot1.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor1.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot2.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor2.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot3.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor3.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot4.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor4.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot5.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor5.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot6.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor6.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSlot7.BackColor = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor7.GetInputValue() ? Color.Yellow : Color.Transparent;

            this.BtnSwitchBondheadVaccum.BackColor =
                this.bondHeadController.GetBondheadVacuumState() ? Color.Yellow : Color.Transparent;
        }

        /// <summary>
        /// 焊头真空
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSwitchBondheadVaccum_Click_1(object sender, EventArgs e)
        {
            if (!this.bondHeadController.GetBondheadVacuumState())
            {
                // 打开焊头真空
                this.bondHeadController.OpenBondHeadVaccum();
                this.BtnSwitchBondheadVaccum.Appearance.BackColor = Color.Yellow;
            }
            else
            {
                // 关焊头真空
                this.bondHeadController.CloseBondHeadVaccum();
                this.BtnSwitchBondheadVaccum.Appearance.BackColor = Color.Transparent;
            }
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditProgram_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            TUAssistantHelper.EditPrInSystem2("吸嘴架示教治具模板", CameraTypeEnum.BondCamera);
        }

        private void FrmLearnNozzleBank_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmLearnNozzleBank_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }

        /// <summary>
        ///  圆搜索
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSearchCircle_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);
            if (prEntity == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先编辑视觉模板!",
                    "Prompt",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            Retry:

            // 执行定位
            MatchResult matchResult = (MatchResult)this.system2Controller.BondCameraVision(null, prName);

            if (matchResult == null)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"Bond相机定位失败!",
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
            if (Math.Abs(pos.X - this.bondModuleController.GetAxisXRealPos()) > 5
                || Math.Abs(pos.Y - this.bondModuleController.GetAxisYRealPos()) > 5)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"移动距离超过5mm,请重试!",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // 移动到模版中心
            this.bondModuleController.MoveBondXY(pos.X, pos.Y);
        }

        /// <summary>
        ///  编辑PR
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditPR_Click(object sender, EventArgs e)
        {
            PREntity prEntity = (PREntity)VisionEntityRepository.GetInstance().Find(prName);
            BaseVisionEntity visionEntity = null;
            if (prEntity != null)
            {
                visionEntity = prEntity;
            }
            else
            {
                prEntity = new PREntity(prName);
                prEntity.Alg.AlgBeLong = AlgBeLongEnum.Calibration;
                visionEntity = prEntity;
                VisionEntityRepository.GetInstance().AddVisionEntity(visionEntity);
            }

            FrmPREditor editor = new FrmPREditor((PREntity)visionEntity, false);
            editor.ShowDialog();
            VisionEntityRepository.GetInstance().Save();
        }
    }
}