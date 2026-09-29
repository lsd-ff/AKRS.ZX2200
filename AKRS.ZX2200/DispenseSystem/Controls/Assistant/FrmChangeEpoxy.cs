using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.ZX2200.DispenseSystem.Models;
using AKRS.ZX2200.DispenseSystem.Models.DispensePara;

namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using System.Drawing;
    using System.Threading;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models.Enums;
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Dispenser;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    using DevExpress.XtraEditors;

    using Newtonsoft.Json;
    using DispenseController = Controllers.DispenseController;

    /// <summary>
    /// 换胶水引导界面
    /// </summary>
    public partial class FrmChangeEpoxy : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 无参构造方法
        /// </summary>
        public FrmChangeEpoxy()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmChangeEpoxy_Load(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;

            // 如果没有程式里面没有设置点胶头，直接退出
            if (this.DispenseProgram.DispenserProgram.Dispenser == null)
            {
                string message = "点胶头没有配置，请先配置点胶头";
                AKRSXtraMessageBox.Show(message);
                this.DialogResult = DialogResult.Abort;
            }

            if (MachineStateModel.GetInstance().MachineWorkMode != MachineWorkModeEnum.OffLineWork)
            {
                // Z轴移动到安全位置
                System1Domain.GetInstance().DispenseController.MoveToSafePos();
            }

            this.InitControl();
            this.SetUiControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 7;

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
        private System1Program DispenseProgram => System1Domain.GetInstance().System1Program;

        /// <summary>
        /// 点胶模组
        /// </summary>
        private DispenseController DispenseController => System1Domain.GetInstance().DispenseController;

        /// <summary>
        /// 点胶模组
        /// </summary>
        private readonly DispenseMeasureHeightController dispenseMeasureHeightController = new DispenseMeasureHeightController();
        
        /// <summary>
        /// 设备参数
        /// </summary>
        [JsonIgnore]
        private DispenseDevicePara DispenseDevicePara => DispenseDevicePara.GetInstance();

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

            string message4 = $"步骤 3/{stepCount}: 现在点胶头在擦胶的位置. "
                              + $"如果需要更改擦胶位置请移动方向盘.\r\n "
                              + "擦胶完成之后，点击下一步";

            string message5 = $"步骤 4/{stepCount}: 现在点胶头在蘸胶的位置. "
                              + $"如果需要更改蘸胶位置请移动方向盘.\r\n "
                              + "移动到蘸胶位置之后，点击下一步";

            string message6 = $"步骤 5/{stepCount}: 控制点胶Z轴下降到预点胶板上方5mm处\r\n " 
                              + $"完成后点击下一步，设备将自动测高\r\n ";

            string message7 = $"步骤 6/{stepCount}: 移动相机，使相机十字线与点胶点重合，点击完成. ";

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
                                      // 移动到更换胶水的位置
                                      this.MoveToPos(this.DispenseDevicePara.DispenserPara.ReplaceGluePosition);
                                       DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到更换胶水位置");
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
                                      this.DispenseDevicePara.DispenserPara.ReplaceGluePosition = this.DispenseController.GetG0Pos();
                                      this.DispenseDevicePara.Save();

                                      // 移动到挤胶位置
                                      this.MoveToPos(this.DispenseDevicePara.DispenserPara.ThrustPosition);
                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到挤胶位置");
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
                                      this.DispenseDevicePara.DispenserPara.ThrustPosition = this.DispenseController.GetG0Pos();
                                      this.DispenseDevicePara.Save();

                                       // 擦胶
                                      this.MoveToPos(this.DispenseDevicePara.DispenserPara.ErasePosition);
                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到擦胶位置");
                                  },
                              doneAction: () => { }),

                          // 擦胶
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
                                      // 记录当前的挤胶位置
                                      this.DispenseDevicePara.DispenserPara.ErasePosition = this.DispenseController.GetG0Pos();
                                      this.DispenseDevicePara.Save();

                                      if (this.DispenseProgram.DispenserProgram.Dispenser.DispenserType == DispenserTypeEnum.Dispenser)
                                      {
                                          if (this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos != null 
                                              && !this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.IsEmpty)
                                          {
                                              // 移动到安全高度，让客户自己移动Z轴去测高高度
                                              AKRSPoint3D point3D = new AKRSPoint3D(
                                                  this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.X,
                                                  this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.Y,
                                                  this.DispenseController.ConvertMachineToG0Pos(new AKRSPoint3D()).Z);

                                              this.DispenseController.MoveToG0Pos3D(point3D);
                                              DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动测高胶位置");
                                          }

                                          this.stepIndex++;
                                      }
                                      else
                                      {
                                          this.DispenseController.PrintToolStop();

                                          if (this.DispenseDevicePara.DispenserPara.PrintingToolPos != null
                                              && !this.DispenseDevicePara.DispenserPara.PrintingToolPos.IsEmpty)
                                          {
                                              // 移动到蘸胶位置
                                              AKRSPoint3D point3D = new AKRSPoint3D(
                                                  this.DispenseDevicePara.DispenserPara.PrintingToolPos.X,
                                                  this.DispenseDevicePara.DispenserPara.PrintingToolPos.Y,
                                                  this.DispenseController.ConvertMachineToG0Pos(new AKRSPoint3D()).Z);

                                              this.DispenseController.MoveToG0Pos3D(point3D);
                                              DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动蘸胶胶位置");
                                          }
                                      }
                                  },
                              doneAction: () => { }),

                          // 蘸胶
                          new AssistantConfig(
                              index: 5,
                              descritpion: message5,
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
                                      this.DispenseDevicePara.DispenserPara.PrintingToolPos = this.DispenseController.GetG0Pos();
                                      this.DispenseDevicePara.Save();

                                      if (this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos != null
                                          && !this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.IsEmpty)
                                      {
                                          // 移动到安全高度，让客户自己移动Z轴去测高高度
                                          AKRSPoint3D point3D = new AKRSPoint3D(
                                              this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.X,
                                              this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.Y,
                                              this.DispenseController.ConvertMachineToG0Pos(new AKRSPoint3D()).Z);

                                          this.DispenseController.MoveToG0Pos3D(point3D);
                                          DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动测高胶位置");
                                          this.DispenseController.PrintToolContinueMove();
                                      }
                                  },
                              doneAction: () => { }),

                          // 自动测高加点胶标定点
                          new AssistantConfig(
                              index: 6,
                              descritpion: message6,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos = this.DispenseController.GetG0Pos();
                                      DispenseDevicePara.GetInstance().Save();

                                    (ExcuteResult result, double height1) =
                                          this.dispenseMeasureHeightController.DispenserHeightMeasurementG0(
                                              System1MeasHeightToolEnum.Dispenser,
                                              null,
                                              double.NaN,
                                              5);

                                      if (result != ExcuteResult.Success)
                                      {
                                          this.stepIndex--;
                                          return;
                                      }
                                      else
                                      {
                                          height = height1;
                                      }

                                      DispenseRunTimeProvider.RecordTime($"系统1示教", $"点胶头测高完成，高度: {height}");

                                      // 移动到这个位置
                                      AKRSPoint3D dripElectricPoint3D = new AKRSPoint3D(
                                          this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.X,
                                          this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos.Y,
                                          height + this.DispenseDevicePara.PreDispensePlatePara.DistanceInSensor);
                                      this.DispenseController.MoveToG0Pos3D(dripElectricPoint3D);

                                      if (MachineStateModel.GetInstance().MachineWorkMode
                                          != MachineWorkModeEnum.OffLineWork)
                                      {
                                           System1Domain.GetInstance().DispenseController.SetDispensePressure(DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum, 0);

                                          DispenseController.OpenDispensingElectric();

                                          Thread.Sleep(DispenseDevicePara.DispenserPara.AssistanceOpenDispenseTime);

                                          DispenseController.CloseDispensingElectric();
                                      }

                                       DispenseRunTimeProvider.RecordTime($"系统1示教", $"点胶头点胶完成，高度: {height + this.DispenseDevicePara.PreDispensePlatePara.DistanceInSensor}");

                                      AKRSPoint3D point3DOffset = System1Program.GetInstance().DispenserProgram.Dispenser.DispensingNeedleOffset;

                                      AKRSPoint3D point3D = dripElectricPoint3D + point3DOffset;

                                      point3D.Z = point3D.Z -this.DispenseDevicePara.PreDispensePlatePara.DistanceInSensor;

                                      try
                                      {
                                           this.DispenseController.MoveToG0Pos3D(point3D);

                                          DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到观看点胶点的位置完成");
                                      }
                                      catch(Exception ex)
                                      {
                                           AKRSXtraMessageBox.Show("相机移动到预点胶板平面失败，请手动移动");

                                           DispenseRunTimeProvider.RecordTime($"系统1示教", $"相机移动到预点胶板平面失败");
                                      }

                                      //this.DispenseController.MoveToG0Pos3D(
                                      //   this.DispenseDevicePara.DispenserPara.VisionPosForCalibration);
                                  },
                              doneAction: () => { }),

                          // 寻找胶点
                          new AssistantConfig(
                              index: 7,
                              descritpion: message7,
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
                                          this.DispenseDevicePara.DispenserPara.VisionPosForCalibration = this.DispenseController.GetG0Pos();
                                          
                                          // 计算相机和针头之间的XY距离
                                          this.DispenseProgram.DispenserProgram.Dispenser.DispensingNeedleOffset =
                                              this.DispenseDevicePara.DispenserPara.VisionPosForCalibration
                                              - this.DispenseDevicePara.DispenserPara.DispenserMeasureHeightPos;

                                          // 计算点胶针和测高针之间的距离，获取预点胶板在GO中的高度
                                          double preDispensePlate = DispenseDevicePara.PreDispensePlatePara
                                              .PreDispensePlateStartPos.Z;
                                          
                                          // 计算点胶头和测高针之间的差值
                                          double offsetZ = height - preDispensePlate;

                                          // 点胶头和相机之间的差值 = 两个差值相减
                                          this.DispenseProgram.DispenserProgram.Dispenser.DispensingNeedleOffset.Z =
                                              -offsetZ + DispenseController.GetG0VisionPos(new AKRSPoint3D()).Z;

                                           DispenseRunTimeProvider.RecordTime($"系统1示教", $"相机和针头之间的距离{this.DispenseProgram.DispenserProgram.Dispenser.DispensingNeedleOffset}");

                                          if(!DispenseDevicePara.GetInstance().DispenseModulePara.DispenseOffset.IsEmpty)
                                          {
                                              AKRSPoint3D dispenseOffset = DispenseDevicePara.GetInstance().DispenseModulePara.DispenseOffset;
                                              DialogResult dialogResult = AKRSXtraMessageBox.Show($"当前点胶整体偏移为{dispenseOffset}，是否清空?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                              if (dialogResult == DialogResult.Yes)
                                              {
                                                  DispenseDevicePara.GetInstance().DispenseModulePara.DispenseOffset = new AKRSPoint3D();
                                              }
                                          }

                                          System1Program.GetInstance().Save();
                                          DispenseDevicePara.GetInstance().Save();

                                          this.DispenseProgram.DispenserProgram.Dispenser.DispenserAssistant.State = AssistantStateEnum.Able;
                                          DispenserRepository.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;
                                      }
                                      catch (Exception e)
                                      {
                                          Console.WriteLine(e);
                                          throw;
                                      }
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmChangeEpoxy");
            ucGuideMove.ChangeModuleName("点胶模组", true);
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

            if (this.DispenseController.IsDispensingElectricOpen())
            {
                this.DispenseController.CloseDispensingElectric();
            }
            else
            {
                System1Domain.GetInstance().DispenseController.SetDispensePressure(DispenseDevicePara.GetInstance().DispenserPara.AssistanceOpenDispenseVacuum, 0);
                this.DispenseController.OpenDispensingElectric();
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
            DispenseController.MoveToSafePos();
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
            this.DialogResult= DialogResult.Cancel;
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

            if (DispenseController.IsDispensingElectricOpen())
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
            this.timer1.Tick -= Timer1_Tick;
            this.timer1.Dispose();  
            this.ucGuideMove?.Dispose();
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

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="point3D">点位</param>
        private void MoveToPos(AKRSPoint3D point3D)
        {
            // 如果点位为空，直接返回
            if (point3D == null || point3D.IsEmpty)
            {
                return;
            }

            // 移动
            this.DispenseController.MoveToG0Pos3D(point3D);
        }

        /// <summary>
        /// 清空位置记忆
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClearPosition_Click(object sender, EventArgs e)
        {
            this.DispenseDevicePara.ClearPosition();
        }
    }
}