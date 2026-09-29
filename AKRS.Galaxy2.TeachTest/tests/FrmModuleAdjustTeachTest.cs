using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models.CommonModels;

namespace AKRS.ZX2200.TransportUnit.Controls.Assistant.ModuleTeach
{
    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmModuleAdjustTeachTest : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 3;

        /// <summary>
        /// 载具
        /// </summary>
       // private Galaxy2.Carriers.Models.TransportUnit TransportUnit => MachineConfigContext.GetInstance().CurrentTransportUnit;

        /// <summary>
        /// 定位点1
        /// </summary>
        private AKRSPoint3D adjustPos1;

        /// <summary>
        /// 定位点2
        /// </summary>
        private AKRSPoint3D adjustPos2;

        /// <summary>
        /// 一点定位时定位点
        /// </summary>
        private AKRSPoint3D adjustPos;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmModuleAdjustTeachTest()
        {
            this.InitializeComponent();
            this.InitControl();
            this.BtBack.Visible = false;
            this.BtDone.Visible = false;
            this.TileBarTeach.SelectedItem = this.TbiModuleAndConfirm;

            // 如果是一点定位，则第三步不显示
            //if (this.transportUnit.TransportUnitConfig.LocateConfig.AdjustType == AdjustTypeEnum.OnePoint)
            //{
            //    this.stepCount = 2;
            //    this.assistantConfigList[2].IsShowTitle = false;
            //    this.assistantConfigList[2].IsShowDone = false;
            //    this.assistantConfigList[2].IsShowNext = true;
            //    this.TbiAdjust2.Visible = false;
            //}
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 选择Module
                          new AssistantConfig(
                              index: 0,
                              descritpion: $"Step 1/{stepCount}; Teach-in adjust: Module  \n"
                                           + @" Choose programmed module and confirm with NEXT to start the assistant",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiAdjust1;

                                      // Choose programmed module and confirm
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          // 定位点1
                          new AssistantConfig(
                              index: 1,
                              descritpion: $"Step 2/{stepCount}; Teach-in adjust: Module \n"
                                           + @" Move to Module adjust point 1 and program",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiModuleAndConfirm;
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiAdjust2;

                                      //FrmPRList frmPRList = new FrmPRList();
                                      //frmPRList.ShowDialog();
                                      //this.TransportUnit.ModuleConfig.LocateConfig.P1PRName = frmPRList.PRName;
                                      //this.adjustPos1 = this.ChooseSystem();
                                      //this.adjustPos1 = this.TransportUnit.SubstrateArray[0,0].IslandAcupointArray[0,0].IslandCoordinateSystem.G0PosToSelf(adjustPos1);
                                  },
                              doneAction: () =>
                                  {
                                      // 一点定位
                                       //   FrmPRList frmPRList = new FrmPRList();
                                       //   frmPRList.ShowDialog();
                                       //   this.TransportUnit.ModuleConfig.LocateConfig.P1PRName = frmPRList.PRName;
                                       //   this.adjustPos = this.ChooseSystem();
                                       //this.adjustPos = this.TransportUnit.SubstrateArray[0,0].IslandAcupointArray[0,0].IslandCoordinateSystem.G0PosToSelf(adjustPos);
                                       //   this.assistantConfigList[2].IsShowDone = false;
                                       //   this.assistantConfigList[2].IsShowTitle = false;
                                       //   this.assistantConfigList[2].IsShowNext = true;

                                       //   // 全部完成后把数据传到配置，最后关闭窗口
                                       //   this.TransportUnit.ModuleConfig.LocateConfig.P1VisionRelativePos = this.adjustPos;
                                       //   this.TransportUnit.SubstrateConfig.LocateConfig.AdjustType= AdjustTypeEnum.OnePoint;
                                       //   this.DialogResult = DialogResult.OK;
                                          this.Close();
                                  }),
                 
                          // 定位点2
                          new AssistantConfig(
                              index: 2,
                              descritpion: $"Step 3/{stepCount}; Teach-in adjust: Module \n"
                                           + @" Move to Module adjust point 2 and program",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiAdjust1;
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      //FrmPRList frmPRList = new FrmPRList();
                                      //frmPRList.ShowDialog();
                                      //this.TransportUnit.ModuleConfig.LocateConfig.P1PRName = frmPRList.PRName;
                                      //this.adjustPos2 = this.ChooseSystem();
                                      // this.adjustPos2 = this.TransportUnit.SubstrateArray[0,0].IslandAcupointArray[0,0].IslandCoordinateSystem.G0PosToSelf(adjustPos2);

                                      //// 全部完成后把数据传到配置，最后关闭窗口
                                      //this.TransportUnit.ModuleConfig.LocateConfig.P1VisionRelativePos = this.adjustPos1;
                                      //this.TransportUnit.ModuleConfig.LocateConfig.P2VisionRelativePos = this.adjustPos2;
                                      //this.TransportUnit.SubstrateConfig.LocateConfig.AdjustType= AdjustTypeEnum.TwoPoints;
                                      this.Close();
                                  }),
                      };
            //UcGuideMove ucGuideMove = new UcGuideMove();
            //ucGuideMove.ChangeModuleName("固晶模组");
            //this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUiControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
        }

        /// <summary>
        /// 如果是坐标点，选择系统2为固晶域，系统1为点胶域
        /// </summary>
        /// <returns>坐标点</returns>
        //private AKRSPoint3D ChooseSystem()
        //{
        //    if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
        //    {
        //        AKRSPoint3D akrsPoint3D = BondDomain.GetInstance().this.bondModuleController.Get3DRealPosition();
        //        return akrsPoint3D;
        //    }
        //    else
        //    {
        //        AKRSPoint3D akrsPoint3D = DispenseDomain.GetInstance().DispenseModule.GetPosByG0();
        //        return akrsPoint3D;
        //    }
        //}

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
    }
}