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
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;

    using DevExpress.CodeParser;

    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models.Enums;

    /// <summary>
    /// 预点胶板示教窗体
    /// </summary>
    public partial class FrmS2DispensePlateAssistant : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FrmS2DispensePlateAssistant()
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
        private S2DispenseDevicePara S2DispenseDevicePara => BondDevicePara.GetInstance().S2DispenseDevicePara;

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
            string message1;

            if (this.type == MultipleHeightMeasurementType.TouchDown)
            {
                message1 = $"预点胶板测高\r\n" + $"移动TouchDown到预点胶板上方5mm处，点击下一步进行测高\r\n";
            }
            else
            {
                message1 = $"预点胶板测高\r\n"
                                + $"移动点胶相机，使得相机能够看清楚预点胶板表面\r\n"
                                + $"点击下一步进行测高";
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
                                                           try
                                                           {
                                                                BondHeadController bondHeadController = System2Domain.GetInstance().BondHeadController;
            (ExcuteResult Ret, double value) = bondHeadController.MeasureHeight(bondHeadController.GetAxisZRealPos(), HeightMeasurementFunctionEnum.WithTDSensor);

                                                               this.height = System2Domain.GetInstance().BondModuleController.ConvertMachineToG0Pos(new AKRSPoint3D(0,0,value)).Z;

                                                               AKRSPoint3D point3D =  System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                                                               point3D.Z = this.height;

                                                               System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(point3D);

                                                           }
                                                           catch (Exception e)
                                                           {
                                                               AKRSXtraMessageBox.Show("激光测高失败，请重新设置");
                                                               this.stepIndex--;
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
                                                                       "3"),
                                                                   out this.rows))
                                                           {
                                                               AKRSXtraMessageBox.Show("参数错误，请重新输入");
                                                               goto RetryCommandRows;
                                                           }

                                                           // 非负判断
                                                           this.rows = Math.Abs(this.rows);

                                                           this.Save();

                                                           // 关闭测高气缸
                                                           System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

                                                           // 回到安全位置
                                                           System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                                                           // 刷新与点胶板子的状态
                                                           System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.InitPre();
                                                       })
                                           };

            ucGuideMove = new UcGuideMove("FrmS2DispensePlateAssistant");

            // 现在没有点胶,默认为系统1点胶
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System2);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.S2DispenseDevicePara.S2PreDispensePlatePara.PreDispensePlateStartPos = this.preDispensePlateStartPos;
            this.S2DispenseDevicePara.S2PreDispensePlatePara.PreDispensePlateEndPos = this.preDispensePlateEndPos;

            this.S2DispenseDevicePara.S2PreDispensePlatePara.PreDispensePlateStartPos.Z = this.height;
            this.S2DispenseDevicePara.S2PreDispensePlatePara.PreDispensePlateEndPos.Z = this.height;

            this.S2DispenseDevicePara.S2PreDispensePlatePara.Rows = this.rows;
            this.S2DispenseDevicePara.S2PreDispensePlatePara.Columns = this.columns;
            this.S2DispenseDevicePara.S2PreDispensePlatePara.AssistanceFinish = true;
            BondDevicePara.GetInstance().Save();
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
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;

            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();

            this.InitControl();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmS2DispensePlateAssistant_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
            System2Domain.GetInstance().S2DispenseController.CloseDispenseHeightMeasurementCylinder();
        }
    }
}