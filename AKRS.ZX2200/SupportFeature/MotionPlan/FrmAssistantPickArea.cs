using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
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

namespace AKRS.ZX2200.SupportFeature.MotionPlan
{
    /// <summary>
    /// 示教取片安全区域
    /// </summary>
    public partial class FrmAssistantPickArea : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 示教取片安全区域
        /// </summary>
        public FrmAssistantPickArea()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 步骤
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            string message1 = $"步骤 1/4: 移动焊头到取片安全区域的左上角落，确保焊头弧线移动时不会和其他物体碰撞";

            string message2 = $"步骤 2/4: 移动焊头到取片安全区域的右上角落，确保焊头弧线移动时不会和其他物体碰撞";

            string message3 = $"步骤 3/4: 移动焊头到取片安全区域的右下角落，确保焊头弧线移动时不会和其他物体碰撞";

            string message4 = $"步骤 4/4: 移动焊头到取片安全区域的左下角落，确保焊头弧线移动时不会和其他物体碰撞";

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
                                      this.SetAreaPosByIndex();
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
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      this.SetAreaPosByIndex();
                                  },
                              doneAction: () =>
                                  {
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
                                       this.SetAreaPosByIndex();
                                  },
                              doneAction: () =>
                                  {
                                    
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
                                      this.SetAreaPosByIndex();
                                      MotionPlanDomain.GetInstance().Save();
                                      this.Close();
                                  }),
                      };

            UcGuideMove ucGuideMove = new UcGuideMove("示教取片安全区域");
            ucGuideMove.ChangeModuleName("固晶模组", false);
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
        /// 根据索引计算安全区域的位置
        /// </summary>
        private void SetAreaPosByIndex()
        {
            AKRSPoint3D point3D = System2Domain.GetInstance().BondModuleController.GetG0RealPosition();

            if (this.stepIndex == 0)
            {
                MotionPlanDomain.GetInstance().PickSafeAreaPoint1 = point3D;
            }
            else if (this.stepIndex == 1)
            {
                MotionPlanDomain.GetInstance().PickSafeAreaPoint2 = point3D;
            }
            else if (this.stepIndex == 2)
            {
                MotionPlanDomain.GetInstance().PickSafeAreaPoint3 = point3D;
            }
            else if (this.stepIndex == 3)
            {
                MotionPlanDomain.GetInstance().PickSafeAreaPoint4 = point3D;
            }
        }
    }
}