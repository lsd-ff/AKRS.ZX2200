using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach
{
    using System.Drawing;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineBox;

    /// <summary>
    /// 导航示教--记录晶圆台手动换料位、准备位、扫码位
    /// </summary>
    public partial class FrmWaferMagazineGeoTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        #region NeedSaveParas

        /// <summary>
        /// 晶圆台手动换料位
        /// </summary>
        private AKRSPoint3D manualChangePosition = new AKRSPoint3D();

        /// <summary>
        /// 晶圆台准备位
        /// </summary>
        private AKRSPoint3D readyPosition = new AKRSPoint3D();

        /// <summary>
        /// 晶圆台扫码位
        /// </summary>
        private AKRSPoint3D scanPosition = new AKRSPoint3D();

        #endregion

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 4;

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
        public FrmWaferMagazineGeoTeach()
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
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; ",
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

                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move the wafer table to a \r\n" + "comfortable working position using the \r\n" + "manipulator:-manual wafer changing \r\n" + "position",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 请将晶圆台移动到一个可以手动更换料片的位置.",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                // -------------------------------------------------------------------记录晶圆台的位置-晶圆台的手动换料位
                                this.manualChangePosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please move to the wafer table readyPosition.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 请将晶圆台移动到一个准备位置.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                            {
                            },
                        nextAction: () =>
                            {
                                this.readyPosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                            },
                        doneAction: () =>
                            {
                            }),

                    // 步骤4
                    new AssistantConfig(
                        index: 3,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please move to the wafer table scanPosition.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 请将晶圆台移动到一个可以扫码的位置.",
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
                                this.scanPosition = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                                WaferTableDevicePara.ManualChangePosition = this.manualChangePosition;
                                WaferTableDevicePara.ReadyPosition = this.readyPosition;
                                WaferTableDevicePara.ScanPosition = this.scanPosition;
                                WaferSubDevicePara.GetInstance().Save();

                                WaferSystemDomain.GetInstance().WaferSystemProgram.MagazineProgram.MagazineBoxConfig
                                    .WaferMagazineGeometry.State = AssistantStateEnum.Able;
                                MagazineBoxConfigRepository.GetInstance().Save();
                                this.DialogResult = DialogResult.OK;
                            }),
                };

            ucGuideMove = new UcGuideMove("晶圆台模组", "", true, CameraEnum.WaferCamera);

            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
            this.assistantConfigList[0].NextAction();            
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
        /// FrmWaferMagazineGeoTeach_Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmWaferMagazineGeoTeach_Load(object sender, EventArgs e)
        {
            //// 放置治具提示
            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3045:\r\n" + "Put on the setting gauge\r\n" + "Continue with OK.\r\n" + "Stop assistant with CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //switch (this.res)
            //{
            //    case DialogResult.OK:
            //        break;
            //    case DialogResult.Cancel:
            //        this.DialogResult = DialogResult.Abort;
            //        return;
            //}

            //// 移除治具提示
            //this.res = AKRSXtraMessageBox.Show("System 2: Warning 2.3007:\r\n" + "Remove the setting gauge\r\n" + "Continue with OK.\r\n" + "Stop assistant with CANCEL.", "Prompt", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            //switch (this.res)
            //{
            //    case DialogResult.OK:
            //        break;
            //    case DialogResult.Cancel:
            //        this.DialogResult = DialogResult.Abort;
            //        return;
            //}
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

        private void FrmWaferMagazineGeoTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }

            this.ucGuideMove?.Dispose();
        }
    }
}