using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Infrastructure.Utils;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
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
    /// <summary>
    /// 身份识别
    /// </summary>
    public partial class FrmAssistanceIdentity : XtraForm
    {
        /// <summary>
        /// 身份识别窗体
        /// </summary>
        public FrmAssistanceIdentity(IdentityConfig identityConfig, string name, GeneralCoordinateSystem coordinateSystem)
        {
            this.InitializeComponent();
            this.identityConfig = identityConfig;
            this.identityConfig.Name = name;
            this.coordinateSystem = coordinateSystem;
            this.Text = name + "编辑视觉模板";
        }

        /// <summary>
        /// 窗体加载时间
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistanceIdentity_Load(object sender, EventArgs e)
        {
            this.InitControl();

            this.SetUiControl();
        }

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 定位配置对象
        /// </summary>
        private readonly GeneralCoordinateSystem coordinateSystem;

        /// <summary>
        /// 定位配置对象
        /// </summary>
        private readonly IdentityConfig identityConfig;

        /// <summary>
        /// switch当前索引
        /// </summary>  
        private int stepIndex = 0;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/{1}:编辑身份识别模板:{this.identityConfig.Name}\r\n"
                              + $"移动固晶相机到身份位置做视觉模板";

            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 定位点1
                          new AssistantConfig(
                              index: 0,
                              descritpion: message1,
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
                                      (bool success, AKRSPoint3D point) =
                                          TUAssistantHelper.AssistantPR(this.identityConfig.P1PRName);
                                      if (success)
                                      {
                                          this.identityConfig.P1VisionRelativePos = this.coordinateSystem.G0PosToSelf(point);
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
                      };

            ucGuideMove = new UcGuideMove("示教身份识别");
            CommonHelper.ChangeUcMove(ucGuideMove, MachineStateModel.GetInstance().CurrentMachineSystem);
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
            TUAssistantHelper.EditPrInSystem2(this.identityConfig.P1PRName);
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
            if (this.identityConfig.Name == EntityTypeEnum.TransportUnit.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.TUIdentification.State = AssistantStateEnum.Able;
            }
            else if (this.identityConfig.Name == EntityTypeEnum.Substrate.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.SubstrateIdentification.State = AssistantStateEnum.Able;
            }
            else if (this.identityConfig.Name == EntityTypeEnum.Module.ToString())
            {
                ProductConfiguration.GetInstance().TransportUnitConfig.ModuleIdentification.State = AssistantStateEnum.Able;
            }
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmAssistanceIdentity_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}
