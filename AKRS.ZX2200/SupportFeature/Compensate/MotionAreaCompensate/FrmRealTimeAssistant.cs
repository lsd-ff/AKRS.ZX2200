using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
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

namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    /// <summary>
    /// 示教矫正系统
    /// </summary>
    public partial class FrmRealTimeAssistant : XtraForm
    {
        /// <summary>
        /// 示教矫正系统
        /// </summary>
        public FrmRealTimeAssistant()
        {
            this.InitializeComponent();
        }

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
        private int stepCount = 2;

        /// <summary>
        /// 开始点
        /// </summary>
        private AKRSPoint3D startPoint3D;

        /// <summary>
        /// 结束点
        /// </summary>
        private AKRSPoint3D endPoint3D;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 编辑的结果
        /// </summary>
        public bool EditResult { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}:移动到轨道标定尺左边的第一个Mark位置";

            string message2 = $"步骤 2/{stepCount}:移动到轨道标定尺右边的最后一个Mark位置";

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
                                      this.startPoint3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();
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
                                      this.endPoint3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

                                      int columnSpacing = XtraInputBox.Show("请输入列间距", "列间距", 2);

                                      if(columnSpacing <= 0)
                                      {
                                          AKRSXtraMessageBox.Show("输入间距有问题，请重新示教");
                                          return;
                                      }

                                      List<AKRSPoint3D> listPoint3D = new List<AKRSPoint3D>();

                                      for (int i = 0; i < int.MaxValue; i++)
                                      {
                                          AKRSPoint3D point3D = new AKRSPoint3D(this.startPoint3D.X + i * columnSpacing,this.startPoint3D.Y,this.startPoint3D.Z);

                                          if(point3D.X > this.endPoint3D.X)
                                          {
                                             break;
                                          }

                                          listPoint3D.Add(point3D);
                                      }

                                      AKRSPoint3D[] point3Ds = listPoint3D.ToArray();

                                      RealTimeCorrection.GetInstance().IsAssistanted = true;

                                      RealTimeCorrection.GetInstance().FirstScanReferencePoints(point3Ds);

                                      System2Domain.GetInstance().BondModuleController.MoveToSafePos();

                                      this.DialogResult = DialogResult.OK;
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmRealTimeAssistant");
            CommonHelper.ChangeUcMove(ucGuideMove, CurrentMachineSystemEnum.System2);
            this.PnlControl.Controls.Add(ucGuideMove);
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
        /// 编辑PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtEditPR_Click(object sender, EventArgs e)
        {
            TUAssistantHelper.EditPrInSystem2(RealTimeCorrection.GetInstance().PrName);
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
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmRealTimeAssistant_Load(object sender, EventArgs e)
        {
            this.InitControl();
        }
    }
}
