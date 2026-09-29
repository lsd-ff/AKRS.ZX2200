using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Utils;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using AKRS.Galaxy2.LogicHardware.Hardwares.DispenseControllers;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using System.Drawing;

    /// <summary>
    /// 示教预点胶板与感应器之间的距离
    /// </summary>
    public partial class FrmAssistantPreDispenseDistance : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 示教预点胶板与感应器之间的距离
        /// </summary>
        public FrmAssistantPreDispenseDistance()
        {
            this.InitializeComponent();
            this.InitControl();

            // 自动切换到系统1
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;

            DispenseRunTimeProvider.RecordTime($"系统1示教", $"切换系统完成");
        }

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// 流程
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 编辑的结果
        /// </summary>
        public bool EditResult { get; set; }

        /// <summary>
        /// 第一个位置
        /// </summary>
        private double position1;

        /// <summary>
        /// 第二个位置
        /// </summary>
        private double position2;

        /// <summary>
        /// 点胶模组
        /// </summary>
        private Controllers.DispenseController dispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 测高模组
        /// </summary>
        private readonly MeasureHeightModule measureHeightModule = new MeasureHeightModule();

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}:移动点胶针到预点胶表面。\r\n下降Z轴，使得点胶针刚好与预点胶板表面刚好接触";

            string message2 = $"步骤 2/{stepCount}:下降Z轴，当预点胶板接触信号触发(界面信号按钮刚好变绿)时停止";


            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 定位点1
                          new AssistantConfig(
                              index: 0,
                              descritpion: message1,
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      
                                  },
                              nextAction: () =>
                                  {
                                      this.position1 = this.dispenseController.GetAxisPos().Z;
                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"获取Z轴位置: {this.position1}");
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 定位点2
                          new AssistantConfig(
                              index: 1,
                              descritpion: message2,
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
                                      this.position2 = this.dispenseController.GetAxisPos().Z;

                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"获取Z轴位置: {this.position2}");

                                      double distance = this.position1 - this.position2;

                                      if (distance <= 0)
                                      {
                                          AKRSXtraMessageBox.Show("示教有误，数据不应小于0，请重新示教", "错误");
                                          this.DialogResult = DialogResult.Abort;
                                          return;
                                      }

                                      if (distance > 1)
                                      {
                                          AKRSXtraMessageBox.Show($"预点胶板与传感器之间的距离为{distance}mm，大于1mm，请调整硬件后重新示教", "错误");
                                          this.DialogResult = DialogResult.Abort;
                                          return;
                                      }

                                      DispenseDevicePara.GetInstance().PreDispensePlatePara.DistanceInSensor = distance;
                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"预点胶板平面到信号触发的距离: {distance}");
                                      DispenseDevicePara.GetInstance().Save();
                                      this.DialogResult = DialogResult.OK;
                                      System1Domain.GetInstance().DispenseController.MoveToSafePos();
                                  }),
                      };

            this.ucGuideMove = new UcGuideMove(this.Text);
            CommonHelper.ChangeUcMove(this.ucGuideMove, CurrentMachineSystemEnum.System1);
            this.PnlControl.Controls.Add(this.ucGuideMove);
            this.SetUiControl();
        }
        
        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[this.stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];

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
            this.SetUiControl();
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
            this.SetUiControl();
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
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 信号扫描器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void SensorTime_Tick(object sender, EventArgs e)
        {
            this.BtSensor.Appearance.BackColor = this.measureHeightModule.PreDispensePressureSensor.GetInputValue()
                                                     ? Color.DarkGreen
                                                     : Color.BurlyWood;
        }

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistantPreDispenseDistance_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.SensorTime.Tick -= new System.EventHandler(this.SensorTime_Tick);
            this.SensorTime.Dispose();
        }
    }
}