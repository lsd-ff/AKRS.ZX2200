using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Drawing;
    using AKRS.Galaxy2.AutoFocusing;
    using AKRS.Galaxy2.Log;
    using AKRS.Galaxy2.Machine;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.EjectionBank;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using log4net.Core;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmChangePositionTeach : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 1;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 芯片识别模板名称
        /// </summary>
        private string dieMatchName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dieMatchName">芯片识别模板名称</param>
        public FrmChangePositionTeach(string dieMatchName)
        {
            this.InitializeComponent();
            this.dieMatchName = dieMatchName;
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
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please move wafer table to start position.",
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; 请移动晶圆台到起点位置.",
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
                                bool isInSearchRange = Block.GetInstance().IsInSearchRange(WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos);
                                if (isInSearchRange)
                                {
                                    Block.GetInstance().ReSetChangeCarrierSignal();
                                    this.DialogResult = DialogResult.OK;
                                }
                                else
                                {
                                    AKRSMessageBoxExt.Show("晶圆台当前位置超出了芯片搜索区域, 请重试！", "Prompt", new string[] { "OK" }, new DialogResult[] { DialogResult.OK });
                                }
                               
                                //(bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.dieMatchName);
                                //if (result.isSucceed && isInSearchRange)
                                //{
                                //    Block.GetInstance().ReSetChangeCarrierSignal();
                                //    this.DialogResult = DialogResult.OK;
                                //}
                                //else if (!isInSearchRange)
                                //{
                                //    AKRSXtraMessageBox.Show("Wafer table current position out of wafer search area, please retry!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                                //else if (!result.isSucceed)
                                //{
                                //    AKRSXtraMessageBox.Show("Don't find a die, please retry!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                //}
                            }),
                };

            this.Invoke(new Action(() => 
                {
                 ucGuideMove = new UcGuideMove("晶圆台模组", "FrmChangePositionTeach", true, CameraEnum.WaferCamera);
                 this.PnlControl.Controls.Add(ucGuideMove);
                 ucGuideMove.Dock = DockStyle.Fill;
            }));            

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
                    this.DialogResult = DialogResult.Abort;
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
        /// Load
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmChangePositionTeach_Load(object sender, EventArgs e)
        {
            Machine.GetInstance().OpenAlarm();
            this.InitControl();
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

        private void FrmChangePositionTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }

            this.ucGuideMove.Dispose();
            Machine.GetInstance().ResetAlarm();
        }
    }
}