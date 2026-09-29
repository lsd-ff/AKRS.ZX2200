using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DispensePara;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using Newtonsoft.Json;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Services;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;

    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 预点胶板示教窗体
    /// </summary>
    public partial class FrmDispensePlateAssistant : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FrmDispensePlateAssistant()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 总共的步数
        /// </summary>
        private int stepCount = 3;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 测高方式
        /// </summary>
        private MultipleHeightMeasurementType type;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 设备参数
        /// </summary>
        [JsonIgnore]
        private DispenseDevicePara DispenseDevicePara => DispenseDevicePara.GetInstance();

        /// <summary>
        /// 起始点位
        /// </summary>
        private AKRSPoint3D preDispensePlateStartPos = new AKRSPoint3D();

        /// <summary>
        /// 列结束点位
        /// </summary>
        private AKRSPoint3D preDispensePlateEndPos = new AKRSPoint3D();

        /// <summary>
        /// 列数
        /// </summary>
        private int columns;

        /// <summary>
        /// 行数
        /// </summary>
        private int rows;

        /// <summary>
        /// 预点胶板的高度
        /// </summary>
        private double height;

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            string message1 = $"预点胶板测高\r\n" + $"移动点胶测高针到预点胶板上方5mm处，点击下一步进行测高\r\n";

            // 如果是系统1配置了激光测高，则提示使用激光测高
            if (MachineHardwareConfiguration.GetInstance().IsSystem1ConfigLaserMh)
            {
                message1 = $"预点胶板测高\r\n" + $"将预点胶板镜面朝下（粗糙面朝上）,移动点胶激光测高到预点胶板上方。\r\n激光测高数值在0附近时，点击下一步进行测高\r\n";
            }

            string message2 = $"步骤 2/{stepCount}: 移动点胶相机到预点胶板左上角落（切记在预点胶板范围内） \r\n,"
                              + $" 这个点将会作为预点胶的开始点\r\n ";

            string message3 = $"步骤 3/{stepCount}: 移动点胶相机到预点胶板右下角落（切记在预点胶板范围内） \r\n,"
                              + $" 这个点将会作为预点胶的结束点\r\n ";



            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 确定开始位置
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
                                                    // 点胶测高
                                                            (ExcuteResult result, double height) result =
                                                                TUAssistantHelper.AssistantMeasureHeight(this.type);

                                                           if (result.result == ExcuteResult.Success)
                                                           {
                                                               this.height = result.height;
                                                               DispenseRunTimeProvider.RecordTime($"系统1示教", $"点胶头测高完成，高度: {this.height}");
                                                           }
                                                           else
                                                           {
                                                               this.DialogResult = DialogResult.Abort;
                                                           }

                                                           AKRSPoint3D point3D = TUAssistantHelper.GetPosBySystem(null,MultipleHeightMeasurementType.TouchDown);

                                                            point3D.Z = this.height;

                                                           AKRSPoint3D visionpos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point3D);

                                                            try
                                                            {
                                                                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionpos);
                                                                DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到预点胶板平面完成");
                                                            }
                                                            catch(Exception ex)
                                                            {
                                                                AKRSXtraMessageBox.Show("相机移动到预点胶板平面失败，请手动移动");
                                                                DispenseRunTimeProvider.RecordTime($"系统1示教", $"相机移动到预点胶板平面失败");
                                                            }
                                                   },
                                                   doneAction: () => { }),

                                               // 确定开始位置
                                               new AssistantConfig(
                                                   index: 1,
                                                   descritpion: message2,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                            // 获取开始点的位置
                                                            preDispensePlateStartPos =
                                                                TUAssistantHelper.GetPosBySystem(null);
                                                            DispenseRunTimeProvider.RecordTime($"系统1示教", $"预点胶板开始位置设置: {preDispensePlateStartPos}");
                                                       },
                                                   doneAction: () => { }),

                                               // 确定结束位置
                                               new AssistantConfig(
                                                   index: 2,
                                                   descritpion: message3,
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
                                                            // 获取结束点的位置
                                                            preDispensePlateEndPos =
                                                                TUAssistantHelper.GetPosBySystem(null);
                                                            DispenseRunTimeProvider.RecordTime($"系统1示教", $"预点胶板结束位置设置: {preDispensePlateEndPos}");

                                                            // 输入列数
                                                            RetryCommandColumns:
                                                            if (!int.TryParse(
                                                                    XtraInputBox.Show(
                                                                        "请输入列数",
                                                                        "列数",
                                                                        "10"),
                                                                    out this.columns))
                                                            {
                                                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                                                goto RetryCommandColumns;
                                                            }

                                                            // 非负判断
                                                            this.columns = Math.Abs(this.columns);

                                                            // 输入列数
                                                            RetryCommandRows:
                                                            if (!int.TryParse(
                                                                    XtraInputBox.Show(
                                                                        "请输入行数",
                                                                        "行数",
                                                                        "5"),
                                                                    out this.rows))
                                                            {
                                                                AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                                                goto RetryCommandRows;
                                                            }

                                                            // 非负判断
                                                            this.rows = Math.Abs(this.rows);

                                                            this.Save();
                                                            DispenseRunTimeProvider.RecordTime($"系统1示教", $"保存预点胶板参数: Start {preDispensePlateStartPos}, End {preDispensePlateEndPos}, Rows {this.rows}, Columns {this.columns}");

                                                            // 关闭测高气缸
                                                            System1Domain.GetInstance().DispenseMeasureHeightController.CloseDispenseHeightMeasurementCylinder();

                                                            // 回到安全位置
                                                            System1Domain.GetInstance().DispenseController.MoveToSafePos();
                                                            DispenseRunTimeProvider.RecordTime($"系统1示教", $"移动到安全位置");

                                                            // 刷新与点胶板子的状态
                                                            System1Domain.GetInstance().System1Program.PreDispensePlateProgram.InitPre();
                                                            DispenseRunTimeProvider.RecordTime($"系统1示教", $"刷新预点胶板状态完成");
                                                       })
                                                       
                                           };

             ucGuideMove = new UcGuideMove("FrmDispensePlateAssistant");

            // 现在没有点胶,默认为系统1点胶
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System1);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.DispenseDevicePara.PreDispensePlatePara.PreDispensePlateStartPos = this.preDispensePlateStartPos;
            this.DispenseDevicePara.PreDispensePlatePara.PreDispensePlateEndPos = this.preDispensePlateEndPos;

            this.DispenseDevicePara.PreDispensePlatePara.PreDispensePlateStartPos.Z = this.height;
            this.DispenseDevicePara.PreDispensePlatePara.PreDispensePlateEndPos.Z = this.height;
        
            this.DispenseDevicePara.PreDispensePlatePara.Rows = this.rows;
            this.DispenseDevicePara.PreDispensePlatePara.Columns = this.columns;
            this.DispenseDevicePara.PreDispensePlatePara.AssistanceFinish = true;
            this.DispenseDevicePara.Save();
        }

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
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// Done
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.Close();
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 预点胶板示教
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmDispensePlateAssistant_Load(object sender, EventArgs e)
        {
            // 由于目前点胶只有一个预点胶板，状态直接改成系统1
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;

            // 默认改为测高针去寻找位置
            this.type = MultipleHeightMeasurementType.TouchDown;

            if (this.type == MultipleHeightMeasurementType.TouchDown)
            {
                System1Domain.GetInstance().DispenseController.MoveToSafePos();
                System1Domain.GetInstance().DispenseMeasureHeightController.OpenDispenseHeightMeasurementCylinder();
            }

            this.InitControl();
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmDispensePlateAssistant_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}