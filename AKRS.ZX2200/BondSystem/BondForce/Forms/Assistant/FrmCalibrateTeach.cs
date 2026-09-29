using System;
using System.Collections.Generic;
using System.Windows.Forms;

using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.DeviceParams;
using AKRS.ZX2200.BondSystem.Models.Enums;

using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.BondForce.Forms.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmCalibrateTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCalibrateTeach()
        {
            this.InitializeComponent();
            this.InitControl();
            this.BtBack.Visible = false;
            this.BtDone.Visible = false;
            this.TileBarTeach.SelectedItem = this.TbiMove;
            this.Disposed += (o, e) => 
            {
                ucGuideMove?.Dispose();
            };
        }

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// 模组控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          new AssistantConfig(
                              index: 0,
                              descritpion: $"Step 1/{this.stepCount}; 移动相机，将相机十字中心对准标定点后点击“下一步”",
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

                                      // 去测高点
                                      this.bondModuleController.MoveToMeasureHeightPos();
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          new AssistantConfig(
                              index: 1,
                              descritpion: $"Step 2/{this.stepCount};确定标定位置：点击“确定”开始测高",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiMove;
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      // 先记录抬起位置
                                      double liftLevel = BondDevicePara.GetInstance().BondHeadParam.AxisSafePos.Z;

                                      // 测高
                                      (ExcuteResult Ret, double HeightValue) res = this.bondHeadController.MeasureHeight(
                                          liftLevel,
                                          HeightMeasurementFunctionEnum.WithTDSensor);

                                      if (res.Ret != ExcuteResult.Success)
                                      {
                                          AKRSXtraMessageBox.Show("测高失败!");
                                          return;
                                      }

                                      // 保存位置
                                      AKRSPoint3D forceCalibratePos = new AKRSPoint3D()
                                                                          {
                                                                              X = this.bondModuleController.GetAxisXRealPos(),
                                                                              Y = this.bondModuleController.GetAxisYRealPos(),
                                                                              Z = res.HeightValue
                                                                          };

                                      BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos =
                                          this.bondModuleController.ConvertMachineToG0Pos(forceCalibratePos);

                                      BondDevicePara.GetInstance().Save();

                                      this.DialogResult = DialogResult.OK;
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmCalibrateTeach");

            // 方向盘设置模组名称
            ucGuideMove.ChangeModuleName("固晶模组", true);

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
            this.stepIndex++;
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
    }
}