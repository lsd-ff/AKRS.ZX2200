using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach
{
    using System.Drawing;

    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 导航示教-记录环限位的中心和半径
    /// </summary>
    public partial class FrmWaferTableCollisionCircleTeach : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 第一点
        /// </summary>
        private AKRSPoint3D firstPoint = new AKRSPoint3D();

        /// <summary>
        /// 第二点
        /// </summary>
        private AKRSPoint3D secondPoint = new AKRSPoint3D();

        /// <summary>
        /// 第三点
        /// </summary>
        private AKRSPoint3D thirdPoint = new AKRSPoint3D();

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 3;

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
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmWaferTableCollisionCircleTeach()
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
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Specify first point.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 指定第一个点.",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.firstPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),
                 
                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Specify second point.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 指定第二个点.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.secondPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Specify third point.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 指定第三个点.",
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
                            this.thirdPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                            AKRSPoint3D p1 = this.firstPoint;
                            AKRSPoint3D p2 = this.secondPoint;
                            AKRSPoint3D p3 = this.thirdPoint;
                            double x = 0, y = 0;
                            if (((p1.X == p2.X && p2.X == p3.X) || (p1.Y == p2.Y && p2.Y == p3.Y)) && !MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                //AKRSXtraMessageBox.Show("Point selection did not pass, please re-select point!");
                                AKRSXtraMessageBox.Show("点位选择未通过, 请重新选择点位!");
                                return;
                            }

                            x = (((p2.X * p2.X - p1.X * p1.X + p2.Y * p2.Y - p1.Y * p1.Y) / 2) * (p3.Y - p1.Y) - ((p3.X * p3.X - p1.X * p1.X + p3.Y * p3.Y - p1.Y * p1.Y) / 2) * (p2.Y - p1.Y)) / ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));
                            y = (((p3.X * p3.X - p1.X * p1.X + p3.Y * p3.Y - p1.Y * p1.Y) / 2) * (p2.X - p1.X) - ((p2.X * p2.X - p1.X * p1.X + p2.Y * p2.Y - p1.Y * p1.Y) / 2) * (p3.X - p1.X)) / ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));

                            WaferTableDevicePara.WaferTableCenter = new AKRSPoint3D(x, y, 0);
                            WaferTableDevicePara.WaferTableRadius = Math.Sqrt((x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y));
                            WaferSubDevicePara.GetInstance().Save();

                            WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig
                                .WaferTableCollisionCircle.State = AssistantStateEnum.Able;
                            MagazineBoxConfigRepository.GetInstance().Save();

                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(WaferTableDevicePara.WaferTableCenter, true);

                            this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("晶圆台模组", "FrmWaferTableCollisionCircleTeach", true, CameraEnum.WaferCamera);
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
        /// FrmWaferTableCollisionCircleTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmWaferTableCollisionCircleTeach_Load(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().IsOffLineWork)
            {
                return;
            }

            //// 移除顶针
            //this.res = AKRSMessageBoxExt.Show("System 2: Warning 2.2264:\r\n" + "Remove touchdown tool", "prompt", new string[] { "OK", "Cancel" }, new DialogResult[] { DialogResult.OK, DialogResult.Cancel });
            //switch (this.res)
            //{
            //    case DialogResult.OK:
            //        break;
            //    case DialogResult.Cancel:
            //        this.DialogResult = DialogResult.Abort;
            //        return;
            //}

            // 提示将做三点
            //this.res = AKRSMessageBoxExt.Show("System 2: Information 2.2449:\r\n" + "Collision circle setup\r\n" + "Use three points on the wafer table to define the circle within which the ejection\r\n" + "tools can be raised without collision.", "prompt", new string[] { "Close" }, new DialogResult[] { DialogResult.OK });
            this.res = AKRSMessageBoxExt.Show("System 2: Information 2.2449:\r\n" + "环限位设置\r\n" + "在晶圆台设置三个点位来限制顶针的活动范围\r\n", "prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
            switch (this.res)
            {
                case DialogResult.OK:
                    break;
            }
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup2, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmWaferTableCollisionCircleTeach_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}