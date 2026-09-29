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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.MachineSupport.Config;
    using AKRS.ZX2200.BondSystem.Controls.Assistant;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;

    using DevExpress.Utils.Extensions;
    using static Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// TU测高界面
    /// </summary>
    public partial class FrmAssistantHeightPoints : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="entityType">名称</param>
        public FrmAssistantHeightPoints(EntityTypeEnum entityType)
        {
            this.InitializeComponent();

            this.entityType = entityType;

            // 判断在哪层做
            this.baseConfig = TUAssistantHelper.SetConfig(entityType);
        }

        private EntityTypeEnum entityType;

        /// <summary>
        /// 实体对象
        /// </summary>
        private readonly BaseConfig baseConfig;

        /// <summary>
        /// 测高选择
        /// </summary>
        private MultipleHeightMeasurementType type;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 5;

        /// <summary>
        /// 对话步骤1
        /// </summary>
        private string message1;

        /// <summary>
        /// 测高点位集合
        /// </summary>
        private readonly List<AKRSPoint3D> points = new List<AKRSPoint3D>();

        /// <summary>
        /// 流道传输对象
        /// </summary>
        private TransportUnit transportUnit;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 流道坐标系
        /// </summary>
        private GeneralCoordinateSystem coordinateSystem;


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
                              descritpion: this.message1,
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.points.RemoveAt(this.points.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      (ExcuteResult result, double height) =
                                          TUAssistantHelper.AssistantMeasureHeight(this.type);

                                      if (result == ExcuteResult.Success)
                                      {
                                          AKRSPoint3D point3D = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                                          point3D.Z = this.coordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, height))
                                              .Z;
                                          this.points.Add(point3D);

                                          if(this.type == MultipleHeightMeasurementType.SubstrateCamera)
                                          {
                                          // 回到一开始的位置，后续回去修改
                                          }
                                      }
                                  },
                              doneAction: () =>
                                  {
                                        (ExcuteResult result, double height) =
                                          TUAssistantHelper.AssistantMeasureHeight(this.type);

                                      if (result == ExcuteResult.Success)
                                      {
                                          AKRSPoint3D point3D = TUAssistantHelper.GetPosBySystem(this.coordinateSystem);
                                          point3D.Z = this.coordinateSystem.G0PosToSelf(new AKRSPoint3D(0, 0, height))
                                              .Z;
                                          this.points.Add(point3D);
                                      }
                                      else
                                      {
                                         this.Close();
                                      }

                                      this.Save();
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmAssistantHeightPoints");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            // 将值传到配置中
            this.baseConfig.HeightMeasurementPoints = this.points;

            this.DialogResult = DialogResult.OK;

            if (this.entityType == EntityTypeEnum.Substrate)
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateHeightMeasurement.State = AssistantStateEnum.Able;
            }
            else if (this.entityType == EntityTypeEnum.Module)
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleHeightMeasurement.State = AssistantStateEnum.Able;
            }

            // 完成
            ProductConfiguration.GetInstance().Save();
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
        /// Done 按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];
            assistantConfig.DoneAction();
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

        /// <summary>
        /// 设置UI
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[0];

            this.BtBack.Visible = this.points.Count > 0;

            this.BtDone.Visible = true;

            this.BtNext.Visible = this.points.Count < this.tileBarGroup4.Items.Count - 1;

            this.LbDescription.Text = this.message1;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;

            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.points.Count];

            if (this.type == MultipleHeightMeasurementType.TouchDown)
            {
                this.message1 = $"Step {this.points.Count + 1}/{this.tileBarGroup4.Items.Count + 1}:Teach-in height measurement {this.baseConfig.EntityType.ToString()}\r\n"
                                + $"Position the touchdown tool approx 5 mm above {this.baseConfig.EntityType.ToString()}\r\n"
                                + $"to teach-in the XY position for the height measurement";
            }
            else
            {
                this.message1 = $"Step {this.points.Count + 1}/{this.tileBarGroup4.Items.Count + 1}:Teach-in height measurement {this.baseConfig.EntityType.ToString()}\r\n"
                                + $"Move to the point with the substrate camera\r\n"
                                + $"Where the height measurement will be carried out";
            }
        }

        /// <summary>
        /// 测高加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAssistantHeightPoints_Load(object sender, EventArgs e)
        {
            // 判断载台上有没有产品
            this.transportUnit = TUAssistantHelper.JudgeTuExist();
            if (this.transportUnit == null)
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            // 这里还需要判断测高功能是否开启，后续增加

            // 选择哪一个示教
          //  this.coordinateSystem = TUAssistantHelper.SetCoordinateSystem(this.baseConfig.EntityType);
            if (this.coordinateSystem == null)
            {
                this.DialogResult = DialogResult.Abort;
                return;
            }

            // 选择测高方式
            FrmChooseMeasureHeightType frm = new FrmChooseMeasureHeightType();

            // 如果没有选择则直接关闭
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.type = frm.Type;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
                return;
            }

            this.InitControl();
            this.SetUiControl();
            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        private void FrmAssistantHeightPoints_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}