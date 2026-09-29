using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach
{
    using System.Drawing;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach;
    using AKRS.ZX2200.WaferSubSystem.Controls.Setting.Waferhandling;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBoxGeo;
    using AKRS.ZX2200.WaferSubSystem.Services;

    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using Newtonsoft.Json;

    /// <summary>
    /// 导航示教-记录晶圆台自动换料位、magazine基准位、基准位与最低槽位的差值、夹爪在Magazine取放料的位置（2、4、6、8）、夹爪在晶圆台取放料的位置（2、4、6、8）、夹爪的安全位置、magazinelift取料与送料的高度差、
    /// </summary>
    public partial class FrmWaferChangeTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        #region NeedSaveParas

        /// <summary>
        /// 晶圆台自动换料位
        /// </summary>
        private AKRSPoint3D autoChangePosition = new AKRSPoint3D();

        /// <summary>
        /// 基准位
        /// </summary> 
        private AKRSPoint3D markPosition = new AKRSPoint3D();

        /// <summary>
        /// 最低槽位与基准位置的距离
        /// </summary>
        private double positionOfLowestSlot;

        /// <summary>
        /// 晶圆夹在晶圆台取放料
        /// </summary>
        private AKRSPoint3D waferClampAtWaferTablePosition = new AKRSPoint3D();

        /// <summary>
        /// 晶圆夹在magazine取放料
        /// </summary>
        private AKRSPoint3D waferClampAtMagazinePosition = new AKRSPoint3D();

        /// <summary>
        /// 晶圆夹取料与放料时magazineLift的位置差
        /// </summary>
        private double magazineLiftDistanceWithClamp;

        /// <summary>
        /// 晶圆夹安全位置
        /// </summary>
        private AKRSPoint3D waferClampSafePosition = new AKRSPoint3D();

        #endregion

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 9;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmWaferChangeTeach()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            string name;
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; set Regrip. ",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 设置重复夹取功能. ",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.stepIndex++;
                        },
                        doneAction: () =>
                        {
                        }),
                    

                    // 步骤2-晶圆台去自动换料位，对准夹爪槽位
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Bring the wafer table automatically to the change position.\r\n" + "Check the position with the wafer gripper.\r\n" + "Caution: The wafer table must be behind the wafer lift.\r\n",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 将晶圆台移动到自动换料的位置，\r\n要求1：晶圆夹可以顺利进出，\r\n要求2：magazine可以顺利上下",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录当前晶圆台的位置-晶圆台自动换料位
                            this.autoChangePosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            // 放置治具提示
                            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3045:\r\n" + "Put on the setting gauge\r\n" + "Continue with OK.\r\n" + "Stop assistant with CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3045:\r\n" + "放置mark治具\r\n" + "继续请点击 OK.\r\n" + "停止示教请点击 CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    break;
                                case DialogResult.Cancel:
                                    this.DialogResult = DialogResult.Abort;
                                    return;
                            }
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤3-夹子对准治具的位置，得到magazine基准位
                    new AssistantConfig(
                        index: 2,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Corrects the position of the wafer \r\n" + "lift so that the wafer gripper can move into \r\n" + "the setting tool.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 校准晶圆台位置 \r\n" + "移动magazine，使晶圆夹能够顺利夹住治具.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录当前magazine上升的位置-magazine基准位
                            this.markPosition = WaferSubController.GetInstance().MagazineController.MagazineAxisZG0Pos;

                            // 移除治具提示
                            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3007:\r\n" + "Remove the setting gauge\r\n" + "Continue with OK.\r\n" + "Stop assistant with CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3007:\r\n" + "请移除治具\r\n" + "继续请点击 OK.\r\n" + "停止示教请点击 CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    break;
                                case DialogResult.Cancel:
                                    this.DialogResult = DialogResult.Abort;
                                    return;
                            }

                            this.stepIndex++;
                            this.NextOperation();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤4
                    new AssistantConfig(
                        index: 3,
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount};  ",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                            this.stepIndex--;
                        },
                        nextAction: () =>
                        {
                            // 填充magazine最低槽位
                            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3008:\r\n" + "Put a magazine with the lowest slot filled on the \r\n" + "waferlift\r\n" + "Continue with OK.\r\n" + "Stop assistant with CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3008:\r\n" + "在magazine最后一层放置一片料" + "继续请点击 OK.\r\n" + "停止示教请点击 CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    break;
                                case DialogResult.Cancel:
                                    this.DialogResult = DialogResult.Abort;
                                    return;
                            }

                            this.stepIndex--;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤5
                    new AssistantConfig(
                        index: 4,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Corrects the position of the wafer \r\n" + "lift so that the wafer gripper can pull out \r\n" + "the wafer as planned. Set the pusher \r\n" + "height if necessary.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 调整晶圆台位置 \r\n" + "调整magazine位置，使晶圆夹可以拉出料片 \r\n" + "调整料片推杆高度 \r\n",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录当前magazine上升的位置-得到基准位与最低槽位的距离（即PositionOfLowestSlot）
                            // -------------------------------------------------------------------调节推杆高度位置
                            this.positionOfLowestSlot = Math.Abs(this.markPosition.Z - WaferSubController.GetInstance().MagazineController.MagazineAxisZG0Pos.Z);

                            // 将晶圆片完全推入magazine
                            //this.res = AKRSXtraMessageBox.Show("System 2: Error 2.2508:\r\n" + "Push the wafer completely into the magazine \r\n" + "OK to continue assistant \r\n" + "(CANCEL ... ends the assistant)", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            this.res = AKRSXtraMessageBox.Show("System 2: Error 2.2508:\r\n" + "将晶圆片完全推入magazine \r\n" + "继续请点击 ok \r\n" + "停止示教请点击 cancel", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                            switch (this.res)
                            {
                                case DialogResult.OK:
                                    break;
                                case DialogResult.Cancel:
                                    this.DialogResult = DialogResult.Abort;
                                    return;
                            }
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤6
                    new AssistantConfig(
                        index: 5,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Pull wafer with gripper to the \r\n" + "clamping position on the wafer table \r\n" + "(possible correction of wafer table \r\n" + "position).",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 控制夹爪夹住料片移动到晶圆台内，直到晶圆感应器有信号.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录当前夹持器的位置-夹爪在晶圆台的位置
                            this.waferClampAtWaferTablePosition = WaferSubController.GetInstance().WaferTableController.WaferClampAxisYG0Pos;

                            // -------------------------------------------------------------------记录当前晶圆台的位置-有可能校正晶圆台自动换料位
                            this.autoChangePosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤7
                    new AssistantConfig(
                        index: 6,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Push the wafer from the wafer table into the magazine, and record the relative value of the decline of the magazine position at this Time.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 夹爪夹住料片，将晶圆料片慢慢推入magazine槽中，过程中可以稍微降低magazine高度.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录magazine lift下降位置的相对值-拉出与推进需要一定的高度差
                            this.magazineLiftDistanceWithClamp = Math.Abs(this.positionOfLowestSlot - Math.Abs(this.markPosition.Z - WaferSubController.GetInstance().MagazineController.MagazineAxisZG0Pos.Z));
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤8
                    new AssistantConfig(
                        index: 7,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Record the position of the current gripper - the position of the gripper in the magazine.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 夹爪在不夹料的情况下闭合，推着料片直到完全进入magazine槽中，记录此时晶圆夹位置.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            // -------------------------------------------------------------------记录当前夹持器的位置-夹爪在magazine的位置
                            this.waferClampAtMagazinePosition = WaferSubController.GetInstance().WaferTableController.WaferClampAxisYG0Pos;

                            // 控制晶圆夹回到安全位，优化人工操作
                            // WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
                            WaferSubModule.GetInstance().WaferTable.WaferClampAxisY.GoHome();
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤9
                    new AssistantConfig(
                        index: 8,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Claw retraction.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 控制夹爪回到安全的位置.",
                        isShowTitle: true,
                        isShowBack: true,
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
                            // -------------------------------------------------------------------记录夹爪安全位置
                            //this.waferClampSafePosition = WaferTableModule.WaferClampAxisYRealPosToG0;

                            WaferTableDevicePara.AutoChangePosition = this.autoChangePosition;
                            MagazineBoxDevicePara.MarkPosition = this.markPosition;
                            WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.PositionOfLowestSlot = this.positionOfLowestSlot;
                            if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.FourInches)
                            {
                                WaferTableDevicePara.WaferClampGetAtWaferTablePositionFourInches = this.waferClampAtWaferTablePosition;
                                WaferTableDevicePara.WaferClampGetAtMagazinePositionFourInches = this.waferClampAtMagazinePosition;
                            }
                            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.SixInches)
                            {
                                WaferTableDevicePara.WaferClampGetAtWaferTablePositionSixInches = this.waferClampAtWaferTablePosition;
                                WaferTableDevicePara.WaferClampGetAtMagazinePositionSixInches = this.waferClampAtMagazinePosition;
                            }
                            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.EightInches)
                            {
                                WaferTableDevicePara.WaferClampGetAtWaferTablePositionEightInches = this.waferClampAtWaferTablePosition;
                                WaferTableDevicePara.WaferClampGetAtMagazinePositionEightInches = this.waferClampAtMagazinePosition;
                            }
                            else if (WaferSystemProgram.GetInstance().MagazineProgram.MagazineBoxConfig.MagazineBoxGeo.WaferSize == WaferSizeEnum.TwelveInches)
                            {
                                WaferTableDevicePara.WaferClampGetAtWaferTablePositionTwelveInches = this.waferClampAtWaferTablePosition;
                                WaferTableDevicePara.WaferClampGetAtMagazinePositionTwelveInches = this.waferClampAtMagazinePosition;
                            }

                            MagazineBoxDevicePara.MagazineLiftDistanceWithClamp = this.magazineLiftDistanceWithClamp;
                            //WaferTableDevicePara.WaferClampSafePosition = this.waferClampSafePosition;

                            MagazineBoxGeoConfigRepository.GetInstance().Save();
                            WaferSubDevicePara.GetInstance().Save();

                            WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig
                                .WaferChange.State = AssistantStateEnum.Able;
                            MagazineBoxConfigRepository.GetInstance().Save();

                            UcMagazineGeometry.RefreshMagazineBoxDataAction();

                            this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("晶圆料盒模组", "FrmWaferChangeTeach", true, CameraEnum.WaferCamera);

            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void CancelOperation()
        {
            //this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "是否结束示教?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// Back操作
        /// </summary>
        private void BackOperation()
        {
            this.stepIndex--;
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
        }

        /// <summary>
        /// Next操作
        /// </summary>
        private void NextOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
        }

        /// <summary>
        /// Done操作
        /// </summary>
        private void DoneOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.Close();
        }

        /// <summary>
        /// Back按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.BackOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Next按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.NextOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.DoneOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CancelOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 是否夹紧夹爪
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnGripperClamping_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.ResetWaferClampCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.SetWaferClampCylinder();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// position
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToSafePosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// FrmWaferChangeTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmWaferChangeTeach_Load(object sender, EventArgs e)
        {
            FrmRegripSet temp = new FrmRegripSet();
            temp.ShowDialog();
            temp.Dispose();
            if (temp.DialogResult == DialogResult.OK)
            {
                temp.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }

            this.assistantConfigList[0].NextAction();
        }

        /// <summary>
        /// 晶圆夹子物料感应器是否有信号
        /// </summary>
        /// <returns>result</returns>
        private bool IsHaveSignalWaferClampCheckSensor()
        {
            if(WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUnCheckTablet)
            {
                return true;
            }

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return false;
            }

            return this.WaferTableModule.WaferClampCheckSensor.CheckStateForNums();
        }

        /// <summary>
        /// 晶圆台料片感应器是否有信号
        /// </summary>
        /// <returns>result</returns>
        private bool IsHaveSignalWaferTableCheckSensor()
        {
            if (WaferSubDevicePara.GetInstance().WaferTableDevicePara.IsUnCheckTablet)
            {
                return true;
            }

            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return false;
            }

            return this.WaferTableModule.WaferSensor.CheckStateForNums();
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.BtnSignalWaferClampCheckSensor.Appearance.BackColor = this.IsHaveSignalWaferClampCheckSensor() ? Color.LightGreen : Color.Red;
            this.BtnSignalWaferTableCheckSensor.Appearance.BackColor = this.IsHaveSignalWaferTableCheckSensor() ? Color.LightGreen : Color.Red;
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup2, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmWaferChangeTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose();
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }

        /// <summary>
        /// 推杆动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMovePusherOut_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.SetWaferPushCylinder();
                    btn.Appearance.BackColor = Color.Yellow;
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// 晶圆夹到安全位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMoveToWaferClampSafePosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }
    }
}