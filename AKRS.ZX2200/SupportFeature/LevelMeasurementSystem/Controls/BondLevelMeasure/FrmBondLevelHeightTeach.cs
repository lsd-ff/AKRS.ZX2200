namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.LevelMeasurementSystem.Models;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmBondLevelHeightTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 1;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// Bond模组控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmBondLevelHeightTeach()
        {
            this.InitializeComponent();
            this.InitControl();
            this.BtBack.Visible = false;
            this.BtNext.Visible = false;
            this.BtDone.Visible = true;
            this.TileBarTeach.SelectedItem = this.TbiHeight;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 选择Module
                          new AssistantConfig(
                              index: 0,
                              descritpion: $"Step 1/{this.stepCount}; Adjust the bond module to the appropriate height, click and save it",
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
                                      // 获取G0高度
                                      BondLevelMeasureSetting.GetInstance().ZG0Pos =
                                          this.BondModuleController.GetG0RealPosition().Z;

                                      BondLevelMeasureSetting.GetInstance().Save();
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmBondLevelHeightTeach");
            ucGuideMove.ChangeModuleName("固晶模组");
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
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
            this.SetUiControl(this.stepIndex);
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
            this.SetUiControl(this.stepIndex);
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
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void FrmBondLevelHeightTeach_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
        }
    }
}