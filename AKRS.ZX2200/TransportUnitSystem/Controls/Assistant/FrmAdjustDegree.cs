using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;

using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Utils;

    /// <summary>
    /// 拉角度
    /// </summary>
    public partial class FrmAdjustDegree : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 拉角度
        /// </summary>
        public FrmAdjustDegree()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 拉角度
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="coordinateSystem">坐标系</param>
        public FrmAdjustDegree(string name, GeneralCoordinateSystem coordinateSystem)
        {
            this.InitializeComponent();
            this.coordinateSystem = coordinateSystem;
            this.editName = name;
            this.Text = this.editName + " Angle";
        }


        private string editName;

        /// <summary>
        /// 坐标系
        /// </summary>
        private GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 防线盘
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
        /// 定位点1
        /// </summary>
        private AKRSPoint3D degreePoint1;

        /// <summary>
        /// 定位点2
        /// </summary>
        private AKRSPoint3D degreePoint2;

        /// <summary>
        /// 真实角度
        /// </summary>
        public double RealDegree { get; set;}

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}: 拉角度左边点\r\n" + $"移动相机到左边的点 \r\n";

            string message2 = $"步骤 2/{stepCount}: 拉角度右边点\r\n" + $"移动相机到右边的点 \r\n";

            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 拉角度1
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
                                      this.degreePoint1 = TUAssistantHelper.GetPosBySystem((GeneralCoordinateSystem)this.coordinateSystem.UpperCoordinateSystem);
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 拉角度2
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
                                      this.degreePoint2 = TUAssistantHelper.GetPosBySystem((GeneralCoordinateSystem)this.coordinateSystem.UpperCoordinateSystem);
                                      
                                      if (MachineStateModel.GetInstance().MachineWorkMode == MachineWorkModeEnum.OffLineWork)
                                      {
                                          this.DialogResult = DialogResult.OK;
                                          return;
                                      }


                                      // 判断两个位置是否差值过小
                                      if (Math.Abs(this.degreePoint1.X - this.degreePoint2.X) < 1)
                                      {
                                          AKRSXtraMessageBox.Show(
                                              "两点之间的距离太短，请重新示教");
                                          this.stepIndex = 0;
                                      }
                                      else
                                      {
                                          this.RealDegree = Math.Atan(
                                              (this.degreePoint1.Y - this.degreePoint2.Y) / (this.degreePoint1.X
                                              - this.degreePoint2.X));

                                          double angle = this.RealDegree * 180.0 / Math.PI;

                                          // 角度太大，返回重做
                                          if (Math.Abs(angle) > 5)
                                          {
                                              AKRSXtraMessageBox.Show(
                                                  "角度太大，请重新示教");
                                              this.stepIndex--;
                                          }
                                          else
                                          {
                                              this.coordinateSystem.Degree = this.RealDegree;
                                              this.DialogResult = DialogResult.OK;
                                              ProductConfiguration.GetInstance().Save();
                                          }
                                      }
                                  }),
                      };
            this.ucGuideMove = new UcGuideMove("载具示教角度");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            this.TileBarTeach.SelectedItem = this.tileBarGroup2.Items[stepIndex];
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

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
            this.SetUiControl(this.stepIndex);
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
            this.SetUiControl(this.stepIndex);
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
            this.SetUiControl(this.stepIndex);
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
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAdjustDegree_Load(object sender, EventArgs e)
        {
            this.InitControl();
            this.SetUiControl(0);
            TUAssistantHelper.SetColor(this.TileBarTeach); ;
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
        /// 关闭
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAdjustDegree_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}