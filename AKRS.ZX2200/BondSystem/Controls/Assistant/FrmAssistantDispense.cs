using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
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

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.BondSystem.Models.Programs;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    using DevExpress.XtraBars.Docking2010.Views.WindowsUI;

    /// <summary>
    /// 系统2示教点胶头
    /// </summary>
    public partial class FrmAssistantDispense : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 系统2示教点胶头
        /// </summary>
        public FrmAssistantDispense()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAssistantDispense_Load(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;

            if (!MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                AKRSXtraMessageBox.Show("系统2点胶硬件未配置");
                this.DialogResult = DialogResult.Abort;
                return;
            }

            if (BondProgram.GetInstance().S2DispenserProgram.Dispenser == null)
            {
                AKRSXtraMessageBox.Show("系统2点胶头为空，请先配置点胶头");
                this.DialogResult = DialogResult.Abort;
                return;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                // Z轴移动到安全位置
                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
                this.S2DispenseController.OpenDispenseHeightMeasurementCylinder();
            }

            this.InitControl();
            this.SetUiControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 6;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 点胶程式
        /// </summary>
        private BondProgram BondProgram => BondProgram.GetInstance();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller System2Controller => System2Domain.GetInstance().System2Controller;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private S2DispenseController S2DispenseController => System2Domain.GetInstance().S2DispenseController;

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;
        
        /// <summary>
        /// 设备参数
        /// </summary>
        private S2DispenseDevicePara S2DispenseDevicePara => BondDevicePara.GetInstance().S2DispenseDevicePara;

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
            string message1 = $"步骤 1/{stepCount}：请点击下一步，点胶头将移动到换胶位置，人员请避开";

            string message2 = $"步骤 2/{stepCount}: 现在点胶头在换胶的位置. "
                              + $"请更换点胶桶.\r\n "
                              + "更换完点胶桶之后点击下一步，点胶头将会移动到挤胶位置";

            string message3 = $"步骤 3/{stepCount}: 现在点胶头在挤胶的位置. "
                              + $"请点击界面上的挤胶按钮，保持一段时间的挤胶.\r\n "
                              + "挤胶完成之后，点击下一步";

            string message4 = $"步骤 4/{stepCount}: 控制点胶Z轴下降到标定台上方5mm处\r\n "
                              + $"完成后点击下一步，设备将自动测高\r\n ";

            string message5 = $"步骤 5/{stepCount}: 移动相机，使相机十字线中点与胶点重合，点击完成. ";
            
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
                                      if (!this.S2DispenseDevicePara.ReplaceGluePosition.IsEmpty
                                          && MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                                      {
                                          this.BondModuleController.MoveToG0Pos(
                                              this.S2DispenseDevicePara.ReplaceGluePosition);
                                      }
                                  },
                              doneAction: () => { }),

                          // 换胶
                          new AssistantConfig(
                              index: 2,
                              descritpion: message2,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      // 记录当前的换胶位置
                                      this.S2DispenseDevicePara.ReplaceGluePosition = this.BondModuleController.GetG0RealPosition();
                                      BondDevicePara.GetInstance().Save();

                                      if (!this.S2DispenseDevicePara.ThrustPosition.IsEmpty
                                          && MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                                      {
                                          // 移动到挤胶位置
                                          this.BondModuleController.MoveToG0Pos(this.S2DispenseDevicePara.ThrustPosition);
                                      }
                                  },
                              doneAction: () => { }),

                          // 挤胶
                          new AssistantConfig(
                              index: 3,
                              descritpion: message3,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      // 记录当前的挤胶位置
                                      this.S2DispenseDevicePara.ThrustPosition = this.BondModuleController.GetG0RealPosition();
                                      BondDevicePara.GetInstance().Save();

                                      BondModule bondModule = System2Module.GetInstance().BondModule;

                                      if (!this.S2DispenseDevicePara.DispenserMeasureHeightPos.IsEmpty
                                          && MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
                                      {
                                          // 移动到安全高度，让客户自己移动Z轴去测高高度
                                          AKRSPoint3D point3D = new AKRSPoint3D(
                                              this.S2DispenseDevicePara.DispenserMeasureHeightPos.X,
                                              this.S2DispenseDevicePara.DispenserMeasureHeightPos.Y,
                                              bondModule.ConvertMachineToG0Pos(new AKRSPoint3D()).Z);

                                          this.BondModuleController.MoveToG0Pos(point3D);
                                          this.S2DispenseController.OpenDispenseHeightMeasurementCylinder();
                                      }
                                  },
                              doneAction: () => { }),

                          // 自动测高加点胶标定点
                          new AssistantConfig(
                              index: 4,
                              descritpion: message4,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      try
                                      {
                                          // 记录当前轴的高度
                                          double left = System2Module.GetInstance().BondModule.BondHead.AxisZ.GetRealPosition();

                                          // 执行测高
                                          RetryCommand:
                                          (ExcuteResult result, double height) = System2Domain.GetInstance().BondHeadController
                                              .MeasureHeight(left, HeightMeasurementFunctionEnum.ForceSensor);

                                          if (result != ExcuteResult.Success)
                                          {
                                              DialogResult dialogResult = AKRSXtraMessageBox.Show(
                                                  "测高失败，是否重试",
                                                  "警告",
                                                  MessageBoxButtons.YesNo);
                                              if (dialogResult == DialogResult.No)
                                              {
                                                  this.stepIndex--;
                                                  return;
                                              }
                                              else
                                              {
                                                  goto RetryCommand;
                                              }
                                          }

                                          System2Module.GetInstance().BondModule.BondHead.AxisZ.AbsoluteMove(height);

                                          this.S2DispenseDevicePara.DispenserMeasureHeightPos =
                                              System2Module.GetInstance().BondModule.GetG0RealPosition();
                                         BondDevicePara.GetInstance().Save();

                                         if (MachineStateModel.GetInstance().MachineWorkMode
                                             != MachineWorkModeEnum.OffLineWork)
                                         {
                                             this.S2DispenseController.OpenDispensingElectric();

                                             Thread.Sleep(500);

                                             this.S2DispenseController.CloseDispensingElectric();
                                         }

                                         if (!this.S2DispenseDevicePara.VisionPosForCalibration.IsEmpty)
                                         {
                                             this.BondModuleController.MoveToG0Pos(
                                                 this.S2DispenseDevicePara.VisionPosForCalibration);
                                         }
                                      }
                                      catch (Exception e)
                                      {
                                          AKRSXtraMessageBox.Show("测高失败，请检查点胶针是否安装好");
                                          this.stepIndex--;
                                          return;
                                      }
                                  },
                              doneAction: () => { }),

                          // 寻找胶点
                          new AssistantConfig(
                              index: 5,
                              descritpion: message5,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () => { },
                              doneAction: () =>
                                  {
                                      try
                                      {
                                          this.S2DispenseDevicePara.VisionPosForCalibration = this.BondModuleController.GetG0RealPosition();
                                          
                                          // 计算相机和针头之间的XY距离
                                          this.BondProgram.S2DispenserProgram.Dispenser.DispensingNeedleOffset =
                                              this.S2DispenseDevicePara.VisionPosForCalibration
                                              - this.S2DispenseDevicePara.DispenserMeasureHeightPos;

                                          // 获取力控标定位
                                          AKRSPoint3D forceCalibratePos = BondDevicePara.GetInstance().BMCDevicePara.ForceCalibratePos;

                                          // 计算高度差
                                          double offsetZ =
                                              -this.S2DispenseDevicePara.DispenserMeasureHeightPos.Z
                                              + forceCalibratePos.Z - BondDevicePara.GetInstance().BondHeadParam
                                                  .HeadToCameraOffset.Z;

                                          this.BondProgram.S2DispenserProgram.Dispenser.DispensingNeedleOffset.Z = offsetZ;
                                          
                                          this.S2DispenseController.CloseDispenseHeightMeasurementCylinder();

                                          System1Program.GetInstance().Save();
                                          DispenseDevicePara.GetInstance().Save();

                                          this.BondProgram.S2DispenserProgram.Dispenser.DispenserAssistant.State = AssistantStateEnum.Able;
                                          DispenserRepository.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;

                                          this.BondModuleController.MoveToSafePos();

                                          if (this.BondProgram.S2EpoxyMaterialProgram.EpoxyMaterial != null )
                                          {
                                             DialogResult dialogResult = AKRSXtraMessageBox.Show("是否重置胶水有效期和点胶头累计次数", "提示", MessageBoxButtons.OKCancel);

                                             if (dialogResult == DialogResult.OK)
                                             {
                                                   this.BondProgram.S2DispenserProgram?.Dispenser?.DispenseTimes?.Clear();
                                                  this.BondProgram.S2EpoxyMaterialProgram?.EpoxyMaterial?.EpoxyMaterialConsumable?.Clear();
                                             }

                                          }
                                      }
                                      catch (Exception e)
                                      {
                                          Console.WriteLine(e);
                                          throw;
                                      }
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmAssistantDispense");
            ucGuideMove.ChangeModuleName("固晶模组", true);
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;

            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 打开/关闭胶阀
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtChangeEpoxy_Click(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (this.S2DispenseController.IsDispensingElectricOpen())
            {
                this.S2DispenseController.CloseDispensingElectric();
            }
            else
            {
                this.S2DispenseController.OpenDispensingElectric();
            }
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
            this.BondModuleController.MoveToSafePos();
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
            this.Close();
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
        /// 判断信号有没有到位
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
            {
                return;
            }

            if (S2DispenseController.IsDispensingElectricOpen())
            {
                this.BtChangeEpoxy.Appearance.BackColor = Color.LightGreen;
            }
            else
            {
                this.BtChangeEpoxy.Appearance.BackColor = Color.White;
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmChangeEpoxy_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Tick -= this.Timer1_Tick;
            this.timer1.Dispose();
            this.timer1 = null;

            this.ucGuideMove?.Dispose();

            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
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