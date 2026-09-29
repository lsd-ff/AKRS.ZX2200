using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;

    using DevExpress.XtraEditors;

    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 测高的示教界面
    /// </summary>
    public partial class FrmMeasureHeight : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 测高界面构造函数
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="baseConfig">配置文件名称</param>
        /// <param name="coordinateSystem">坐标系</param>
        public FrmMeasureHeight(string name, BaseConfig baseConfig, GeneralCoordinateSystem coordinateSystem)
        {
            this.InitializeComponent();
            this.name = name;
            this.coordinateSystem = coordinateSystem;
            this.config = baseConfig;
        }

        /// <summary>
        /// 名称
        /// </summary>
        private readonly string name;

        /// <summary>
        /// 定位配置对象
        /// </summary>
        private readonly BaseConfig config;

        /// <summary>
        /// 测高选择
        /// </summary>
        private MultipleHeightMeasurementType type;

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 5;

        /// <summary>
        /// 对话步骤1
        /// </summary>
        private string Message1 => $"步骤 {stepIndex}/{stepCount}:示教 {this.name.ToString()}的测高位置\r\n"
                                   + $"移动固晶相机到需要测高的位置\r\n"
                                   + $"点击下一步或完成，设备将自动完成测高";

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 坐标系
        /// </summary>
        private readonly GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 测高点位的集合
        /// </summary>
        private readonly List<AKRSPoint3D> measureHeightPoints = new List<AKRSPoint3D>();


        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 测高1
                          new AssistantConfig(
                              index: 0,
                              descritpion: this.Message1,
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.measureHeightPoints.RemoveAt(this.measureHeightPoints.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      // 记录当前坐标
                                      AKRSPoint3D point3D = TUAssistantHelper.GetPosBySystem(
                                          this.coordinateSystem,
                                          this.type);

                                      // 执行测高，并记录位置
                                      (ExcuteResult result, double height) result =
                                          TUAssistantHelper.AssistantMeasureHeight(this.type);
                                      if (result.result != ExcuteResult.Success)
                                      {
                                          this.stepIndex--;
                                          return;
                                      }

                                      point3D.Z = this.coordinateSystem
                                          .G0PosToSelf(new AKRSPoint3D(0, 0, result.height)).Z;

                                      this.measureHeightPoints.Add(point3D);
                                  },
                              doneAction: () =>
                                  {
                                      // 记录当前坐标
                                      AKRSPoint3D point3D = TUAssistantHelper.GetPosBySystem(
                                          this.coordinateSystem,
                                          this.type);

                                      // 执行测高，并记录位置
                                      (ExcuteResult result, double height) result =
                                          TUAssistantHelper.AssistantMeasureHeight(this.type);
                                      if (result.result != ExcuteResult.Success)
                                      {
                                          this.stepIndex--;
                                          return;
                                      }

                                      point3D.Z = this.coordinateSystem
                                          .G0PosToSelf(new AKRSPoint3D(0, 0, result.height)).Z;

                                      this.measureHeightPoints.Add(point3D);

                                      this.UpdateState();
                                      ProductConfiguration.GetInstance().Save();
                                      this.DialogResult = DialogResult.OK;
                                  }),
                      };

            this.ucGuideMove = new UcGuideMove("FrmMeasureHeight");
            CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(this.ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
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
            this.config.HeightMeasurementPoints = this.measureHeightPoints;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];

            if (this.stepIndex == 0)
            {
                this.BtDone.Visible = true;
                this.BtNext.Visible = true;
                this.BtCancel.Visible = true;
                this.BtBack.Visible = false;
            }
            else if (this.stepIndex == 2)
            {
                this.BtDone.Visible = true;
                this.BtNext.Visible = false;
                this.BtCancel.Visible = true;
                this.BtBack.Visible = true;
            }
            else
            {
                this.BtDone.Visible = true;
                this.BtNext.Visible = true;
                this.BtCancel.Visible = true;
                this.BtBack.Visible = true;
            }

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;

            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
        }

        /// <summary>
        /// 测高加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmMeasureHeight_Load(object sender, EventArgs e)
        {
            // 判断是否需要测高
            if (!this.config.MeasureHeightInSystem1 && !this.config.MeasureHeightInSystem2)
            {
                this.ForBidden();
                this.DialogResult = DialogResult.OK;
                return;
            }
            
            this.type = MultipleHeightMeasurementType.SubstrateCamera;

            this.InitControl();
            this.SetUiControl();
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 自动聚焦
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtAutoFocus_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.AutoFocus(sender, this);
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            this.assistantConfigList[0].NextAction();
            this.stepIndex++;
            this.SetUiControl();
        }

        /// <summary>
        /// 上一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            this.assistantConfigList[0].BackAction();
            this.stepIndex--;
            this.SetUiControl();
        }

        /// <summary>
        /// 更新示教状态
        /// </summary>
        private void UpdateState()
        {
            if (this.name == EntityTypeEnum.TransportUnit.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.Able;
            }
            else if (this.name == EntityTypeEnum.Substrate.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.Able;
            }
            else if (this.name == EntityTypeEnum.Module.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.Able;
            }
            else
            {
                SingleBondPositionConfig singleBondPositionConfig = (SingleBondPositionConfig)this.config;
                singleBondPositionConfig.BondPositionMeasureHeight.State = AssistantStateEnum.Able;
            }
        }

        /// <summary>
        /// 禁止的
        /// </summary>
        private void ForBidden()
        {
            if (this.name == EntityTypeEnum.TransportUnit.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUHeightMeasurement.State = AssistantStateEnum.ForBidden;
            }
            else if (this.name == EntityTypeEnum.Substrate.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.ForBidden;
            }
            else if (this.name == EntityTypeEnum.Module.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.ForBidden;
            }
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void BtEditPr_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPrInSystem2("框架示教找中心模板");
        }

        /// <summary>
        /// 移动到中心
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtMoveToCenter_Click(object sender, EventArgs e)
        {
            (bool success, AKRSPoint3D point) result = TUAssistantHelper.AssistantPR("框架示教找中心模板");

            if (result.success)
            {
                TUAssistantHelper.MoveToPos(result.point - BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset);
            }
            else
            {
                AKRSXtraMessageBox.Show("定位失败，请重置制作模板");
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMeasureHeight_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
        }

        private void PnlControl_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}