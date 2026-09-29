using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Utils;
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

namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Enums;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
    using static AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool.FrmMultipleHeightMeasurementType;

    /// <summary>
    /// 示教中心和大小
    /// </summary>
    public partial class FrmAssistantSizeAndCenter : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 结果
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="IsTransportUnit">是否为tu</param>
        public FrmAssistantSizeAndCenter(string name, bool IsTransportUnit = false)
        {
            this.InitializeComponent();
            this.assistantStateName = name;
            this.InitControl();
            this.IsTransportUnit = IsTransportUnit;
        }

        /// <summary>
        /// 是否为TU
        /// </summary>
        private bool IsTransportUnit = false;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 示教的名称
        /// </summary>
        private readonly string assistantStateName;

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 3;

        /// <summary>
        /// 中心和参考点的偏移
        /// </summary>
        public double OffsetX { get; set; }

        /// <summary>
        /// 中心和参考点的偏移
        /// </summary>
        public double OffsetY { get; set; }

        /// <summary>
        /// 左下角落
        /// </summary>
        public AKRSPoint3D LeftDownPoint3D { get; set; }

        /// <summary>
        /// 右上角落
        /// </summary>
        public AKRSPoint3D RightUpPoint3D { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}:移动固晶相机到{this.assistantStateName}的左下角落边缘\r\n";

            string message2 = $"步骤 2/{stepCount}:移动固晶相机到{this.assistantStateName}的右上角落边缘\r\n";
            
            string message3 = $"步骤 3/{stepCount}:移动固晶相机到{this.assistantStateName}的平面\r\n"
                              + "使得相机能看清除物体表面，点击下一步进行测高";

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
                                      this.LeftDownPoint3D = TUAssistantHelper.GetPosBySystem(null);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 定位点2
                          new AssistantConfig(
                              index: 1,
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
                                      this.RightUpPoint3D = TUAssistantHelper.GetPosBySystem(null);
                                      System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(
                                          (this.LeftDownPoint3D + this.RightUpPoint3D) / 2.0);
                                  },
                              doneAction: () =>
                                  {
                                       this.RightUpPoint3D = TUAssistantHelper.GetPosBySystem(null);
                                      System2Domain.GetInstance().BondModuleController.CameraMoveToG0Pos(
                                          (this.LeftDownPoint3D + this.RightUpPoint3D) / 2.0);
                                      this.DialogResult = DialogResult.OK;
                                  }),

                          // 定位点3
                          new AssistantConfig(
                              index: 3,
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
                                      (ExcuteResult result, double height) =
                                          TUAssistantHelper.AssistantMeasureHeight(MultipleHeightMeasurementType.SubstrateCamera);
                                      if (result == ExcuteResult.Success)
                                      {
                                          this.DialogResult = DialogResult.OK;
                                          this.LeftDownPoint3D.Z = height;
                                          this.RightUpPoint3D.Z = height;
                                      }
                                      else
                                      {
                                          this.stepIndex--;
                                      }
                                  }),
                      };

            this.ucGuideMove = new UcGuideMove("示教物体大小和位置");
            CommonHelper.ChangeUcMove(this.ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
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
        /// 后退
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl();
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl();
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistantSizeAndCenter_Load(object sender, EventArgs e)
        {
            if (!this.IsTransportUnit)
            {
                this.assistantConfigList.RemoveAt(2);
                this.assistantConfigList[1].IsShowDone = true;
                this.assistantConfigList[1].IsShowNext = false;
                this.tileBarGroup2.Items.RemoveAt(2);
            }
        }
    }
}