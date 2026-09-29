using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using System;
using System.Collections.Generic;

namespace AKRS.ZX2200.TransportSystem.Controls.Assistance
{
    using System.Windows.Forms;

    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportSystem.Controllers;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.DatasetModels.StockBin;
    using AKRS.ZX2200.TransportSystem.Models.Programs;
    using DevExpress.Xpo.DB.Helpers;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 自动上料步骤
    /// </summary>
    public partial class FrmUnLoadAssistance : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 自动下料示教步骤
        /// </summary>
        public FrmUnLoadAssistance()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 上料控制器
        /// </summary>
        private UnLoaderBinController unLoadControl = new UnLoaderBinController();

        /// <summary>
        /// 上料控制器
        /// </summary>
        private UnLoaderBinProgram UnLoaderBinProgram => TransportProgram.GetInstance().UnLoaderBinProgram;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 开始位置
        /// </summary>
        private double startPos;

        /// <summary>
        /// 结束位置
        /// </summary>
        private double endPos;

        /// <summary>
        /// 左边位置
        /// </summary>
        private double leftPos;

        /// <summary>
        /// 右边位置
        /// </summary>
        private double rightPos;

        /// <summary>
        /// 层数
        /// </summary>
        private int floodCount;

        /// <summary>
        /// 间距
        /// </summary>
        private double floodSpacing;

        /// <summary>
        /// 推料位
        /// </summary>
        private double pushPos;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmLoadingAssistance_Load(object sender, EventArgs e)
        {
            // 上料模组全部移动到安全位置
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 左边料仓的位置
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion: "Step 1/5;请移到料盒A的位置",
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                         this.leftPos = this.unLoadControl.GetYPos();
                                                       },
                                                   doneAction: () => { }),

                                               // 右边料仓的位置
                                               new AssistantConfig(
                                                   index: 1,
                                                   descritpion: "Step 2/5;请移到料盒B的位置",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiLastLayer;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.rightPos = this.unLoadControl.GetYPos();
                                                       },
                                                   doneAction: () => { }),

                                               // 料仓起点
                                               new AssistantConfig(
                                                   index: 2,
                                                   descritpion: "Step 3/5; 请移动Z轴到第一层。",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () => { },
                                                   nextAction: () =>
                                                       {
                                                           this.startPos = this.unLoadControl.GetZPos();
                                                       },
                                                   doneAction: () => { }),

                                               // 料仓结束点
                                               new AssistantConfig(
                                                   index: 3,
                                                   descritpion: "Step 4/5; 请移动Z轴到最后一层。",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiRotaryPoint1;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.endPos = this.unLoadControl.GetZPos();

                                                           // 输入列数
                                                           if (!int.TryParse(XtraInputBox.Show("请输入层数。", "层数", "2"), out this.floodCount))
                                                           {
                                                               if (this.floodCount <= 1)
                                                               {
                                                                   AKRSXtraMessageBox.Show("参数错误, 请重新输入！");
                                                                   this.stepIndex--;
                                                                   return;
                                                               }
                                                           }

                                                           // 计算间距
                                                           this.floodSpacing =
                                                               (this.endPos - this.startPos) / (this.floodCount - 1);

                                                           this.TileBarTeach.SelectedItem = this.TbiMovePusRod;
                                                       },
                                                   doneAction: () =>
                                                       {
                                                          

                                                       }),

                                                // 料仓结束点
                                               new AssistantConfig(
                                                   index: 4,
                                                   descritpion: "Step 5/5; 请移动推杆到推料位。",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: false,
                                                   isShowDone: true,
                                                   backAction: () =>
                                                       {
                                                           this.TileBarTeach.SelectedItem = this.TbiLayerNum;
                                                       },
                                                   nextAction: () =>
                                                       {
                                                       },
                                                   doneAction: () =>
                                                       {
                                                           this.pushPos = this.unLoadControl.GetPushPos();
                                                       }),
                                           };

            ucGuideMove = new UcGuideMove("FrmUnLoadAssistance");
            ucGuideMove.ChangeModuleName("自动下料模组");
            this.PnlControl.Controls.Add(ucGuideMove);
            this.SetUiControl(0);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;
            TUAssistantHelper.SetColor(this.TileBarTeach);
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
        /// cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;    
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
            TransportProgram.GetInstance().UnLoaderAssistantState.State = AssistantStateEnum.Able;
            this.unLoadControl.MovePushRodHome();
            this.DialogResult = DialogResult.OK;
            this.Save();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            this.UnLoaderBinProgram.UnloaderBin.FirstTabletLevel = this.startPos;
            this.UnLoaderBinProgram.UnloaderBin.LastTabletLevel = this.endPos;
            this.UnLoaderBinProgram.UnloaderBin.LayerNum = this.floodCount;
            this.UnLoaderBinProgram.UnloaderBin.BinAPosY = this.leftPos;
            this.UnLoaderBinProgram.UnloaderBin.BinBPosY = this.rightPos;
            this.UnLoaderBinProgram.UnloaderBin.TabletPitch = this.floodSpacing;
            this.UnLoaderBinProgram.UnloaderBin.PushPos = this.pushPos;
            TransportProgram.GetInstance().Save();
            UnLoaderBinRepository.GetInstance().Save();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmUnLoadAssistance_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ucGuideMove.Dispose();
        }
    }
}