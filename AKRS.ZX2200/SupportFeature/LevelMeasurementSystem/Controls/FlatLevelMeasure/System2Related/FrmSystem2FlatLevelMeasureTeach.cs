namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System2Related
{
    using System;
    using System.Collections.Generic;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.LevelMeasurementSystem.Models.FlatLevelMeasure;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.FlatLevelMeasure.System2Related;

    /// <summary>
    /// 导航示教
    /// </summary>
    public partial class FrmSystem2FlatLevelMeasureTeach : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 10;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 系统2水平测试点位参数
        /// </summary>
        private System2FlatLevelMeasureSetting system2FlatLevelMeasureSetting;

        /// <summary>
        /// 缓存
        /// </summary>
        private List<AKRSPoint2D> flatLevelMeasurePointList = new List<AKRSPoint2D>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="levelMeasureName">测水平的名称</param>
        public FrmSystem2FlatLevelMeasureTeach(string levelMeasureName)
        {
            this.InitializeComponent();
            this.InitControl();
            this.BtBack.Visible = false;
            this.BtDone.Visible = false;
            this.TileBarTeach.SelectedItem = this.TbiPoint1;

            this.system2FlatLevelMeasureSetting =
                (System2FlatLevelMeasureSetting)System2FlatLevelMeasureRepository.GetInstance().Find(levelMeasureName);
        }

        /// <summary>
        /// Bond模组控制器
        /// </summary>
        private BondModuleController BondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          // 点1
                          new AssistantConfig(
                              index: 0,
                              descritpion: $"Step 1/{this.stepCount}; Use the camera to view a point, click next to save, click done to end ",
                              isShowTitle: true,
                              isShowBack: false,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint2;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点2
                          new AssistantConfig(
                              index: 1,
                              descritpion: $"Step 2/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint1;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint3;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点3
                          new AssistantConfig(
                              index: 2,
                              descritpion: $"Step 3/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint2;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint4;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点4
                          new AssistantConfig(
                              index: 3,
                              descritpion: $"Step 4/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint3;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint5;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点5
                          new AssistantConfig(
                              index: 4,
                              descritpion: $"Step 5/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint4;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint6;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点6
                          new AssistantConfig(
                              index: 5,
                              descritpion: $"Step 6/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint5;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint7;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点7
                          new AssistantConfig(
                              index: 6,
                              descritpion: $"Step 7/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint6;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint8;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点8
                          new AssistantConfig(
                              index: 7,
                              descritpion: $"Step 8/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint7;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint9;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点9
                          new AssistantConfig(
                              index: 8,
                              descritpion: $"Step 9/{this.stepCount};Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint8;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint10;
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),

                          // 点10
                          new AssistantConfig(
                              index: 9,
                              descritpion: $"Step 10/{this.stepCount}; Use the camera to view a point, click next to save, click done to end",
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      this.TileBarTeach.SelectedItem = this.TbiPoint9;
                                      this.flatLevelMeasurePointList.RemoveAt(
                                          this.flatLevelMeasurePointList.Count - 1);
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      this.flatLevelMeasurePointList.Add(this.BondModuleController.Get2DG0RealPosition());

                                      this.system2FlatLevelMeasureSetting.FlatLevelMeasurePointList =
                                          this.flatLevelMeasurePointList;

                                      System2FlatLevelMeasureRepository.GetInstance().Save();
                                      this.Close();
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmSystem2FlatLevelMeasureTeach");
            ucGuideMove.ChangeModuleName("固晶模组");
            this.PnlControl.Controls.Add(ucGuideMove);

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
            this.Dispose(); 
        }

        private void PnlControl_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmSystem2FlatLevelMeasureTeach_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            this.Dispose();
        }
    }
}