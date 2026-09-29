using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Infrastructure.Utils;
    using AKRS.ZX2200.TransportUnitSystem.Model;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using System.Windows.Forms;

    /// <summary>
    /// 制作PR
    /// </summary>
    public partial class FrmEditPRs : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

        /// <summary>
        /// 定位示教界面
        /// </summary>
        /// <param name="locateConfig">定位配置</param>
        /// <param name="name">pR的名称</param>
        /// <param name="coordinateSystem">坐标系</param>
        public FrmEditPRs(LocateConfig locateConfig, string name, GeneralCoordinateSystem coordinateSystem)
        {
            this.InitializeComponent();
            this.locateConfig = locateConfig;
            locateConfig.Name = name;
            this.coordinateSystem = coordinateSystem;

            this.Text = name + "编辑视觉模板";
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmEditPRs_Load(object sender, EventArgs e)
        {
            if (this.locateConfig.AdjustType == AdjustTypeEnum.None)
            {
                this.ForBidden();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                return;
            }

            this.stepCount = (int)this.locateConfig.AdjustType;

            this.InitControl();

            this.SetUiControl();
        }

        /// <summary>
        /// 定位配置对象
        /// </summary>
        private readonly GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 定位配置对象
        /// </summary>
        private readonly LocateConfig locateConfig;

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount;

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
            string message1 = $"步骤 1/{stepCount}:编辑模板:{this.locateConfig.Name}\r\n"
                              + $"移动相机 {this.locateConfig.Name} 到第一个参考点然后做模板";

            string message2 = $"步骤 2/{stepCount}:编辑模板:{this.locateConfig.Name}\r\n"
                              + $"移动相机 {this.locateConfig.Name} 到第二个参考点然后做模板";

            string message3 = $"步骤 3/{stepCount}:编辑模板:{this.locateConfig.Name}\r\n"
                              + $"移动相机 {this.locateConfig.Name} 到第三个参考点然后做模板";

            string message4 = $"步骤 4/{stepCount}:编辑模板:{this.locateConfig.Name}\r\n"
                              + $"移动相机 {this.locateConfig.Name} 到第四个参考点然后做模板";

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
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P1PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P1VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                          this.stepIndex--;
                                      }
                                  },
                              doneAction: () =>
                                  {
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P1PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P1VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
                                          this.UpdateState();

                                          ProductConfiguration.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                          this.stepIndex--;
                                      }
                                  }),

                          // 定位点2
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
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P2PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P2VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                      }
                                  },
                              doneAction: () =>
                                  {
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P2PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P2VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
                                          this.UpdateState();
                                          ProductConfiguration.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                      }
                                  }),

                          // 定位点3
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
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P3PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P3VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                      }
                                  },
                              doneAction: () =>
                                  {
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P3PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P3VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
                                          this.UpdateState();
                                          ProductConfiguration.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                      }
                                  }),

                          // 定位点4
                          new AssistantConfig(
                              index: 4,
                              descritpion: message4,
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
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.locateConfig.P4PRName);
                                      if (success)
                                      {
                                          this.locateConfig.P4VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
                                          this.UpdateState();
                                          ProductConfiguration.GetInstance().Save();
                                          this.DialogResult = DialogResult.OK;
                                      }
                                      else
                                      {
                                          AKRSXtraMessageBox.Show("定位失败，请重新制作模板后重试");
                                      }
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmEditPRs");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl();
            this.ChangeTeachByType();
        }

        /// <summary>
        /// 改变示教步骤
        /// </summary>
        public void ChangeTeachByType()
        {
            if (this.locateConfig.AdjustType == AdjustTypeEnum.OnePoint)
            {
                this.assistantConfigList.RemoveAt(3);
                this.assistantConfigList.RemoveAt(2);
                this.assistantConfigList.RemoveAt(1);
                this.assistantConfigList[0].IsShowNext = false;
                this.assistantConfigList[0].IsShowDone = true;
                this.tileBarGroup2.Items.RemoveAt(3);
                this.tileBarGroup2.Items.RemoveAt(2);
                this.tileBarGroup2.Items.RemoveAt(1);
            }
            else if (this.locateConfig.AdjustType == AdjustTypeEnum.TwoPoints)
            {
                this.assistantConfigList.RemoveAt(3);
                this.assistantConfigList.RemoveAt(2);
                this.assistantConfigList[1].IsShowNext = false;
                this.assistantConfigList[1].IsShowDone = true;
                this.tileBarGroup2.Items.RemoveAt(3);
                this.tileBarGroup2.Items.RemoveAt(2);
            }
            else if (this.locateConfig.AdjustType == AdjustTypeEnum.ThreePoints)
            {
                this.assistantConfigList.RemoveAt(3);
                this.assistantConfigList[2].IsShowNext = false;
                this.assistantConfigList[2].IsShowDone = true;
                this.tileBarGroup2.Items.RemoveAt(3);
            }
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
            if (this.stepIndex == 0)
            {
                TUAssistantHelper.EditPrInSystem2(this.locateConfig.P1PRName);
            }
            else if (this.stepIndex == 1)
            {
                TUAssistantHelper.EditPrInSystem2(this.locateConfig.P2PRName);
            }
            else if (this.stepIndex == 2)
            {
                TUAssistantHelper.EditPrInSystem2(this.locateConfig.P3PRName);
            }
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
        /// 更新示教状态
        /// </summary>
        private void UpdateState()
        {
            if (this.locateConfig.Name == EntityTypeEnum.TransportUnit.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUAdjust.State = AssistantStateEnum.Able;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.TransportUnit.ToString() + "Right")
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUAdjust.State = AssistantStateEnum.Able;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.Substrate.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.Able;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.Module.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.Able;
            }
        }

        /// <summary>
        /// 禁止的
        /// </summary>
        private void ForBidden()
        {
            if (this.locateConfig.Name == EntityTypeEnum.TransportUnit.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.TransportUnit.ToString() + "Right")
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.Substrate.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateAdjust.State = AssistantStateEnum.ForBidden;
            }
            else if (this.locateConfig.Name == EntityTypeEnum.Module.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleAdjust.State = AssistantStateEnum.ForBidden;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmEditPRs_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();

            if (this.locateConfig.IsUseCircleAngel())
            {
                this.locateConfig.LocateUseConfig1.UseAngle = false;
                this.locateConfig.LocateUseConfig1.UseAngle = false;
                this.locateConfig.LocateUseConfig1.UseAngle = false;
                this.locateConfig.LocateUseConfig1.UseAngle = false;

                if (this.locateConfig.AdjustType == AdjustTypeEnum.OnePoint)
                {
                    AKRSXtraMessageBox.Show("圆查找模板不能使用角度，已自动关闭，若想矫正角度请手动打开或使用多点定位");
                }

                if (this.locateConfig.AdjustType >= AdjustTypeEnum.TwoPoints)
                {
                    AKRSXtraMessageBox.Show("圆查找模板不能使用角度,已自动打开两点确定角度");

                    this.locateConfig.IsCalculateAngleByTwoPoint = true;
                }

                ProductConfiguration.GetInstance().Save();
            }
        }
    }
}