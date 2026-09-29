using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.DispenseSystem.Controllers;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;

    /// <summary>
    /// 测高
    /// </summary>
    public partial class FrmMultipleHeightMeasurement : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 点胶控制器
        /// </summary>
       private DispenseController dispenseController = new DispenseController();

        /// <summary>
        /// 点胶测高控制器
        /// </summary>
        private DispenseMeasureHeightController measureHeightController = new DispenseMeasureHeightController();

        /// <summary>
        /// 测高
        /// </summary>
        public FrmMultipleHeightMeasurement()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 测高方式
        /// </summary>
        public MultipleHeightMeasurementType Type { get; set; }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmMultipleHeightMeasurement_Load(object sender, EventArgs e)
        {
            FrmChooseMeasureHeightType frmChooseMeasureHeightType = new FrmChooseMeasureHeightType();
            frmChooseMeasureHeightType.ShowDialog();
            this.Type = frmChooseMeasureHeightType.Type;

            this.InitControl();
            this.SetUiControl();

            // 如果在系统1的情况下，打开测高针
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System1)
            {
                // 移动到安全位置，打开测高针
                this.dispenseController.MoveToSafePos();
                this.measureHeightController.OpenDispenseHeightMeasurementCylinder();
            }
        }

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 步骤数量
        /// </summary>
        private int stepCount;

        /// <summary>
        /// 信息
        /// </summary>
        private string message;

        /// <summary>
        /// 测高点位集合
        /// </summary>
        public List<AKRSPoint3D> MeasureHeightPoints { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            if (this.Type == MultipleHeightMeasurementType.TouchDown)
            {
                this.message = $"Step {this.stepCount}:Teach-in height measurement \r\n"
                          + $"Position the touchdown tool approx 1 mm above \r\n"
                          + $"to teach-in the XY position for the height measurement";
            }
            else
            {
                this.message = $"Step {this.stepCount}:Teach-in height measurement \r\n"
                          + $"Move to the point with the substrate camera\r\n"
                          + $"Where the height measurement will be carried out";
            }

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 示教点位
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: this.message,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: true,
                                                   isShowDone: true,
                                                   backAction: () =>
                                                       {
                                                           // 移除最后一个元素
                                                           this.MeasureHeightPoints.RemoveAt(
                                                               this.MeasureHeightPoints.Count - 1);
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.MeasureHeightPoints.Add(
                                                               TUAssistantHelper.GetPosBySystem(null, this.Type));
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.MeasureHeightPoints.Add(
                                                               TUAssistantHelper.GetPosBySystem(null, this.Type));
                                                       }),
                                           };
            ucGuideMove = new UcGuideMove("FrmMultipleHeightMeasurement");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];

            if (this.MeasureHeightPoints.Count == 0)
            {
                this.BtBack.Visible = false;
            }
            else
            {
                this.BtBack.Visible = true;
            }

            if (this.MeasureHeightPoints.Count == this.tileBarGroup4.Items.Count - 1)
            {
                this.BtNext.Visible = false;
            }
            else
            {
                this.BtNext.Visible = true;
            }

            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.MeasureHeightPoints.Count];
            
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
            AssistantConfig assistantConfig = this.assistantConfigList[0];
            assistantConfig.NextAction();
            this.SetUiControl();
        }

        /// <summary>
        /// 返回上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];
            assistantConfig.BackAction();
            this.SetUiControl();
        }

        /// <summary>
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];
            assistantConfig.DoneAction();
            this.Close();
        }

        /// <summary>
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.MeasureHeightPoints = null;
            this.Close();
        }

        private void FrmMultipleHeightMeasurement_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
        }
    }
}