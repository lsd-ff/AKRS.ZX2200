using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Drawing;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmComponentCarrierAdjustWithWaferTeach : DevExpress.XtraEditors.XtraForm
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
        /// 载具名称
        /// </summary>
        private CarrierWithWaferConfig waferCarrierWithWaferConfig;

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
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// FlipChipModule
        /// </summary>
        private FlipModule FlipChipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// FlipChipDevicePara
        /// </summary>
        private FlipTableDevicePara FlipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waferCarrierConfig">wafer TransportUnit</param>
        public FrmComponentCarrierAdjustWithWaferTeach(CarrierWithWaferConfig waferCarrierWithWaferConfig)
        {
            this.InitializeComponent();
            this.waferCarrierWithWaferConfig = waferCarrierWithWaferConfig;
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to 1st adjust point and teach the search.",
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
                        }),
                };

            ucGuideMove = new UcGuideMove("晶圆台模组", "FrmComponentCarrierAdjustWithWaferTeach", true, CameraEnum.WaferCamera);
            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void CancelOperation()
        {
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            this.SetUIControl(this.stepIndex);
        }

        /// <summary>
        /// Next操作
        /// </summary>
        private void NextOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUIControl(this.stepIndex);
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

        private void FrmComponentCarrierAdjustWithWaferTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
        }
    }
}