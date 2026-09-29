using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.Infrastructure.Action;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    using Newtonsoft.Json;

    /// <summary>
    /// 示教测高针和焊头之间的距离
    /// </summary>
    public partial class FrmAssistantDistanceMhAndBh : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 示教测高针和焊头之间的距离
        /// </summary>
        public FrmAssistantDistanceMhAndBh()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAssistantDistanceMhAndBh_Load(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                // Z轴移动到安全位置
                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
            }

            this.InitControl();
            this.SetUiControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 点胶模组
        /// </summary>
        private S2DispenseController S2DispenseController => System2Domain.GetInstance().S2DispenseController;

        /// <summary>
        /// 原始位置
        /// </summary>
        private AKRSPoint3D g0Pos;

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl();
        }

        /// <summary>
        /// 设置索引
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}：移动到相机能够清除看清的位置，最好为BMC标定平台";

            string message2 = $"步骤 2/{stepCount}: 将激光测高光斑对准之前相机所看到的位置";

            double height = 0;

            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 准备
                          new AssistantConfig(
                              index: 1,
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
                                      // 相机移动到上方5mm处
                                      AKRSPoint3D current = System2Domain.GetInstance().BondModuleController
                                          .GetG0RealPosition();
                                      current.Z += 5;
                                      this.S2DispenseController.BondHandMoveToVisionPos(current);

                                      // 测高
                                      double left = System2Module.GetInstance().BondModule.BondHead.AxisZ.GetRealPosition();
                                      (ExcuteResult result, double measureHeight) = System2Domain.GetInstance().BondHeadController
                                          .MeasureHeight(left, HeightMeasurementFunctionEnum.WithTDSensor);

                                      if (result != ExcuteResult.Success)
                                      {
                                          AKRSXtraMessageBox.Show("测高失败，请重试");
                                          this.stepIndex--;
                                          return;
                                      }

                                      this.g0Pos = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                                      this.g0Pos.Z = System2Domain.GetInstance().BondModuleController
                                          .ConvertMachineToG0Pos(new AKRSPoint3D(0, 0, measureHeight)).Z;

                                     System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                                       System2Domain.GetInstance().S2DispenseController.OpenDispenseHeightMeasurementCylinder();

                                  },
                              doneAction: () => { }),

                          // 换胶
                          new AssistantConfig(
                              index: 2,
                              descritpion: message2,
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
                                      double measureHeight = this.S2DispenseController.LaserMeasureHeight();
                                      AKRSPoint3D current = System2Domain.GetInstance().BondModuleController
                                          .GetG0RealPosition();


                                      current.Z -= measureHeight;

                                      BondDevicePara.GetInstance().S2DispenseDevicePara.DistanceByMhToBh =
                                          current - this.g0Pos;
                                  BondDevicePara.GetInstance().Save();
                                  this.DialogResult = DialogResult.OK;
                                  this.Close();
                              }),
                      };

            ucGuideMove = new UcGuideMove("FrmAssistantDistanceMhAndBh");
            ucGuideMove.ChangeModuleName("固晶模组", true);
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;

            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 引导完成，退出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.DialogResult = DialogResult.OK;
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
        }

        /// <summary>
        /// 引导完成，退出
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl();
        }


        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmChangeEpoxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose(); 
            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
            System2Domain.GetInstance().BondModuleController.MoveToSafePos();
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
        }
    }
}