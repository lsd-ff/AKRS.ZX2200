using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.BondSystem.Models.DeviceParams;

    /// <summary>
    /// BLT示教窗体
    /// </summary>
    public partial class FrmAssistantBLT : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info">示教信息</param>
        public FrmAssistantBLT(string info)
        {
            InitializeComponent();
            this.info = info;
            this.Text = info + "示教";
        }

        /// <summary>
        /// 示教信息
        /// </summary>
        private string info;

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 4;

        /// <summary>
        /// and
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex;

        /// <summary>
        /// 测高点位集合
        /// </summary>
        public List<AKRSPoint3D> PosList = new List<AKRSPoint3D>();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmAssistantBLT_Load(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;

            if (!MachineHardwareConfiguration.GetInstance().IsSystem2ConfigLaserMh)
            {
                AKRSXtraMessageBox.Show("系统2激光测高未配置！", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Abort;
                return;
            }

            this.InitControl();
            this.SetUiControl();
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
            this.SetUiControl();
        }

        /// <summary>
        /// 设置索引
        /// </summary>
        private void SetUiControl()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
            this.LbDescription.Text = assistantConfig.Descritpion;
            this.LbDescription.Visible = assistantConfig.IsShowTitle;
            this.TileBarTeach.SelectedItem = this.tileBarGroup4.Items[this.stepIndex];
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            string message1 = $"步骤 1/{stepCount}：移动Bond相机将相机中心十字线对准{this.info}测高点1";

            string message2 = $"步骤 2/{stepCount}: 移动Bond相机将相机中心十字线对准{this.info}测高点2";

            string message3 = $"步骤 3/{stepCount}: 移动Bond相机将相机中心十字线对准{this.info}测高点3";

            string message4 = $"步骤 4/{stepCount}:移动Bond相机将相机中心十字线对准{this.info}测高点4";

            this.PosList.Clear();

            this.assistantConfigList
                = new List<AssistantConfig>
                      {
                          new AssistantConfig(
                              index: 1,
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
                                      AKRSPoint3D pos = this.bondModuleController.GetG0RealPosition()
                                                        + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                      this.PosList.Add(pos);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint2;
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          new AssistantConfig(
                              index: 2,
                              descritpion: message2,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      this.PosList.RemoveAt(0);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint1;
                                  },
                              nextAction: () =>
                                  {
                                      AKRSPoint3D pos = this.bondModuleController.GetG0RealPosition()  + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                      this.PosList.Add(pos);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint3;
                                  },
                              doneAction: () => { }),

                          new AssistantConfig(
                              index: 3,
                              descritpion: message3,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: true,
                              isShowDone: false,
                              backAction: () =>
                                  {
                                      // 移除第2点
                                      this.PosList.RemoveAt(1);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint2;
                                  },
                              nextAction: () =>
                                  {
                                      AKRSPoint3D pos = this.bondModuleController.GetG0RealPosition()  + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                      this.PosList.Add(pos);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint4;
                                  },
                              doneAction: () =>
                                  {
                                  }),

                          new AssistantConfig(
                              index: 4,
                              descritpion: message4,
                              isShowTitle: true,
                              isShowBack: true,
                              isShowNext: false ,
                              isShowDone: true,
                              backAction: () =>
                                  {
                                      // 移除第三点
                                      this.PosList.RemoveAt(2);

                                      this.TileBarTeach.SelectedItem = this.TbiMeasureHeightPoint3;
                                  },
                              nextAction: () =>
                                  {
                                  },
                              doneAction: () =>
                                  {
                                      AKRSPoint3D pos = this.bondModuleController.GetG0RealPosition()  + BondDevicePara.GetInstance().BondHeadParam.HeadToCameraOffset;

                                      this.PosList.Add(pos);
                                  }),
                      };

            ucGuideMove = new UcGuideMove("FrmAssistantBLT");
            ucGuideMove.ChangeModuleName("固晶模组", true);
            this.PnlControl.Controls.Add(ucGuideMove);

            // 首个步骤的Description
            this.LbDescription.Text = this.assistantConfigList[0].Descritpion;

            TUAssistantHelper.SetColor(this.TileBarTeach);
        }

        /// <summary>
        /// 引导完成，退出
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.DialogResult = DialogResult.OK;
            MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System2;
        }

        /// <summary>
        /// 引导完成，退出
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
            this.stepIndex--;
            this.SetUiControl();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void FrmAssistantBLT_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove?.Dispose();
        }
    }
}