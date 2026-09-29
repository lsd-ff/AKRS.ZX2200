using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;


namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.BondSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using DevExpress.XtraEditors;
    using System.Linq;

    /// <summary>
    /// 吸嘴架示教窗体
    /// </summary>
    public partial class FrmLearnToolBank : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmLearnToolBank()
        {
            this.InitializeComponent();
            this.InitControl();
            this.InitMovement();
        }

        /// <summary>
        ///  槽位数
        /// </summary>
        private int slotNum;

        /// <summary>
        /// 首槽位取位置
        /// </summary>
        private AKRSPoint3D firstPickPos = new AKRSPoint3D();

        /// <summary>
        /// 末槽位取位置
        /// </summary>
        private AKRSPoint3D lastPickPos = new AKRSPoint3D();

        /// <summary>
        /// 首槽位取吸嘴角度
        /// </summary>
        private double firstPickAngle;

        /// <summary>
        /// 末槽位取吸嘴角度
        /// </summary>
        private double lastPickAngle;

        /// <summary>
        /// 首槽位放吸嘴角度
        /// </summary>
        private double firstPlaceAngle;

        /// <summary>
        /// 末槽位放吸嘴角度
        /// </summary>
        private double lastPlaceAngle;


        /// <summary>
        /// 首槽位放位置
        /// </summary>
        private AKRSPoint3D firstPlacePos = new AKRSPoint3D();

        /// <summary>
        /// 末槽位放位置
        /// </summary>
        private AKRSPoint3D lastPlacePos = new AKRSPoint3D();

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 5;

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
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 吸嘴架控制器
        /// </summary>
        private NozzleShelfController nozzleShelfController => System2Domain.GetInstance().NozzleShelfController;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmLearnToolBank");

        /// <summary>
        /// 初始运动
        /// </summary>
        private void InitMovement()
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            // T轴回0
            this.bondHeadController.MoveTAxisToHome();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.PnlControl.Controls.Add(ucGuideMove1);

            // 方向盘设置模组名称
            this.ucGuideMove1.ChangeModuleName("吸嘴架模组", true);

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
                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot1PlacePos;

                                // 方向盘设置模组名称
                                this.ucGuideMove1.ChangeModuleName("固晶模组", true);
                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // Slot 1 放位置
                    new AssistantConfig(
                        index: 1,
                        descritpion: $"2/{this.stepCount}: 移动焊头将治具放入首槽位示教放吸嘴位置。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiChangePositionY;

                            // 方向盘设置模组名称
                            this.ucGuideMove1.ChangeModuleName("吸嘴架模组", true);
                        },
                        nextAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiTeachSlot1PickPos;

                             #region 创建吸嘴架坐标系

                                AKRSPoint3D upPoint = this.bondModuleController.GetG0RealPosition();
                            
                                AKRSPoint3D downPoint = new AKRSPoint3D(
                                    upPoint.X,
                                    System2Module.GetInstance().NozzleShelfModule.AxisY.GetRealPosition(),
                                    upPoint.Z);

                                MachineCoordinateSystem.GetInstance().CreateCoordinateSystem("ToolBankCoordinateSystem", "G0", true, CoordinateSystemTypeEnum.General);

                                GeneralCoordinateSystem toolBankCoordinateSystem = (GeneralCoordinateSystem)MachineCoordinateSystem.GetInstance().CoordinateSystems.Find(it => it.Name == "ToolBankCoordinateSystem");
                                toolBankCoordinateSystem.Init(upPoint, downPoint, 0);

                                // 保存吸嘴架换吸嘴位
                                BondDevicePara.GetInstance().NozzleShelfParam.ToolChangePosition =
                                    toolBankCoordinateSystem.SelfPosToG0(downPoint);

                             #endregion

                            // 保存槽位位置
                           this.firstPlacePos =
                                this.bondModuleController.GetG0RealPosition();

                            this.firstPlaceAngle =
                               this.bondHeadController.GetAxisTRealPos();

                            double safeLevel = this.bondHeadController.GetAxisZRealPos() + 7;

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

                            // 安全高度默认是放的高度＋10mm
                            BondDevicePara.GetInstance().BondHeadParam.ChangeNozzleSafePos.Z =
                                this.bondHeadController.GetAxisZRealPos() + 6;

                            MachineCoordinateSystem.GetInstance().Save();
                        },
                        doneAction: () =>
                        {
                        }),

                    // Slot 1 取位置
                    new AssistantConfig(
                        index: 2,
                        descritpion: $"3/{this.stepCount}: 移动焊头将治具放入首槽位示教取吸嘴位置",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot1PlacePos;

                                // 方向盘设置模组名称
                                this.ucGuideMove1.ChangeModuleName("固晶模组", true);
                            },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot7PlacePos;

                                // 保存槽位位置
                                this.firstPickPos =
                                    this.bondModuleController.GetG0RealPosition();

                                this.firstPickAngle =
                                    this.bondHeadController.GetAxisTRealPos();
                            },
                        doneAction: () =>
                            {
                            }),

                    // Slot 7  放位置
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"4/{this.stepCount}: 移动焊头将治具放入末槽位示教放吸嘴位置。",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot7PickPos;
                            },
                        nextAction: () =>
                            {
                                // 保存槽位位置
                                this.lastPlacePos =
                                     this.bondModuleController.GetG0RealPosition();

                                this.lastPlaceAngle =
                                      this.bondHeadController.GetAxisTRealPos();

                                // 防呆
                                if (BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[6].X
                                    > BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos[0].X) 
                                {
                                    DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"首槽位在右，末槽位在左，请重新示教！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);

                                    this.stepIndex--;
                                    return;
                                }

                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot7PickPos;
                            },
                        doneAction: () =>
                            {
                            })
                        {
                    },

                    // Slot 7  取位置
                    new AssistantConfig(
                        index: 4,
                        descritpion: $"5/{this.stepCount}: 移动焊头将治具放入末槽位示教取吸嘴位置",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiTeachSlot7PlacePos;
                            },
                        nextAction: () =>
                            {
                            },
                        doneAction: () =>
                        {
                            // 保存槽位位置
                            this.lastPickPos =
                                 this.bondModuleController.GetG0RealPosition();

                            this.lastPickAngle =
                                  this.bondHeadController.GetAxisTRealPos();

                            // 防呆
                                if ( this.lastPlacePos.X
                                    >this.firstPlacePos.X)
                            {
 DialogResult dialog = AKRSXtraMessageBox.Show(
                                        $"首槽位在右，末槽位在左，请重新示教！",
                                        "Warn",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);

                                this.stepIndex--;
                                return;
                            }

                            this.CalculateSlotPos();

                            // 自动去安全位
                            this.bondHeadController.CloseBondHeadVaccum();
                             this.bondHeadController.MoveBondZToChangeNozzleSafeHeight();
                            this.nozzleShelfController.MoveShelfToHome();
                             this.bondModuleController.MoveToSafePos();
                            this.bondHeadController.SetCurrentNozzleName(string.Empty);
                        }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiChangePositionY;

            // 方向盘设置模组名称
            this.ucGuideMove1.ChangeModuleName("吸嘴架模组", true);
        }

        /// <summary>
        /// 计算槽位位置
        /// </summary>
        private void CalculateSlotPos()
        {
            BondDevicePara.GetInstance().NozzleShelfParam.PickNozzlePos = GeometryService.GeneratePointsBetween(
                this.firstPickPos,
                this.lastPickPos,
                this.slotNum);

            BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzlePos = GeometryService.GeneratePointsBetween(
                this.firstPlacePos,
                this.lastPlacePos,
                this.slotNum);

            BondDevicePara.GetInstance().NozzleShelfParam.PickNozzleAngle= GeometryService.GeneratePointsBetween(
                this.firstPickAngle,
                this.lastPickAngle,
                this.slotNum);

            BondDevicePara.GetInstance().NozzleShelfParam.PlaceNozzleAngle = GeometryService.GeneratePointsBetween(
                this.firstPlaceAngle,
                this.lastPlaceAngle,
                this.slotNum);

            BondDevicePara.GetInstance().NozzleShelfParam.SlotPitch =
                (this.lastPickPos.X - this.firstPickPos.X) / (this.slotNum - 1);

            BondDevicePara.GetInstance().NozzleShelfParam.SlotNum = this.slotNum;

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
            bool ret = System2Module.GetInstance().NozzleShelfModule.PickerHolderSensor1.GetInputValue();
            this.BtnSlot1.BackColor = System2Module.GetInstance().NozzleShelfModule.IsSlotHaveNozzle(1) ? Color.Yellow : Color.Transparent;


            this.BtnSlot7.BackColor = System2Module.GetInstance().NozzleShelfModule.IsSlotHaveNozzle(this.slotNum) ? Color.Yellow : Color.Transparent;

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
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmLearnToolBank_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Tick -= timer1_Tick;
            this.timer1.Dispose();
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmLearnToolBank_Load(object sender, EventArgs e)
        {
            Input:
            string inputValue = XtraInputBox.Show("请输入吸嘴架槽位数：", "输入框", "7");

            if (!string.IsNullOrEmpty(inputValue))
            {
                if (int.TryParse(inputValue, out int result))
                {
                    this.slotNum = result;
                }
                else
                {
                    DialogResult res = AKRSXtraMessageBox.Show(
                        "输入无效，请输入一个有效的数字!",
                        "Error",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Error);

                    if (res == DialogResult.OK)
                    {
                        goto Input;
                    }

                    this.DialogResult = DialogResult.Cancel;
                }
            }
        }
    }
}