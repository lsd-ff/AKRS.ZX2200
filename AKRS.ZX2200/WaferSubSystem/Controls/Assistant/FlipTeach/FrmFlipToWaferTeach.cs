using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using AKRS.Galaxy2.Machine.Enums;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.FlipTool;

using DevExpress.XtraEditors;


namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.FlipTeach
{
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;

    /// <summary>
    /// 示教顶针和翻转工具接触位置
    /// </summary>
    public partial class FrmFlipToWaferTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseCarrierConfig">芯片</param>
        public FrmFlipToWaferTeach(BaseCarrierConfig baseCarrierConfig)
        {
            this.InitializeComponent();
            this.baseCarrierConfig = baseCarrierConfig;
            this.InitControl();
        }

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 11;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 芯片名称
        /// </summary>
        private BaseCarrierConfig baseCarrierConfig;

        /// <summary>
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmFlipToWaferTeach");

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);

            this.PnlControl.Controls.Add(this.ucGuideMove1);

            this.TbiEject.Text = this.baseCarrierConfig.CarrierType == CarrierTypeEnum.Wafer ? "顶针接触" : "华夫盒接触";

            this.BtnESUpOrDown.Enabled = this.baseCarrierConfig.CarrierType == CarrierTypeEnum.Wafer;

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 步骤1
                                               new AssistantConfig(
                                                   index: 0,
                                                   descritpion:
                                                   this.baseCarrierConfig.CarrierType == CarrierTypeEnum.Wafer
                                                       ? $"1/1:将翻转工具移向顶针，使二者轻轻接触，点“确定”。"
                                                       : $"1/1:将翻转工具移向华夫盒，使二者轻轻接触，点“确定”。",
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
                                                           FlipTool flipTool = (FlipTool)FlipToolRepository
                                                               .GetInstance().Find(
                                                                   WaferSystemProgram.GetInstance().FlipModuleProgram
                                                                       .FlipToolName);

                                                           if (MachineStateModel.GetInstance().MachineWorkMode
                                                               == MachineWorkModeEnum.OffLineWork)
                                                           {
                                                               //flipTool.FlipToolAssistant.State = AssistantStateEnum.Able;
                                                               this.DialogResult = DialogResult.OK;

                                                               return;
                                                           }

                                                           // 记录接触位置
                                                           flipTool.FlipToolWaferHeight =
                                                               this.flipTableController.GetTRealPos();

                                                           this.flipTableController.MoveFlipTAxis(0);

                                                           if (this.baseCarrierConfig.CarrierType
                                                               == CarrierTypeEnum.Wafer)
                                                           {
                                                               // 归还顶针
                                                               WaferSubController.GetInstance().EjectController
                                                                   .ReturnEjection();
                                                           }

                                                           this.baseCarrierConfig.FlipToWafer.State =
                                                               AssistantStateEnum.Able;

                                                           this.DialogResult = DialogResult.OK;
                                                       }),
                                           };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiEject;

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtDone.Visible = assistantConfig.IsShowDone;
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
            this.SetUIControl(this.stepIndex);
            FlipToolRepository.GetInstance().Save();
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }


        /// <summary>
        ///  顶针上下
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESUpOrDown_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
        
            try
            {
                if (this.baseCarrierConfig is CarrierWithWaferConfig carrierWithWafer)
                {
                    btn.Enabled = false;
                    if (btn.Appearance.BackColor == Color.Yellow)
                    {
                        WaferSubController.GetInstance().EjectController.ReturnEjection();
                        btn.Appearance.BackColor = default;
                    }
                    else
                    {
                        if (WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Exists(item => item.Name == carrierWithWafer.EjectionName) && carrierWithWafer.EjectionName != string.Empty)
                        {
                            EjectionBankSlotConfig tarBankSlotConfig = (EjectionBankSlotConfig)WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.Find(item => item.Name == carrierWithWafer.EjectionName);
                            Block.GetInstance().SetCurrentCarrier(this.baseCarrierConfig);
                            WaferSubController.GetInstance().EjectController.ChangeEjection(tarBankSlotConfig.Index);

                            // 顶针顶起到预顶位
                            WaferSubController.GetInstance().EjectController.MoveEjectToReadyLiftPosition();

                            btn.Appearance.BackColor = Color.Yellow;
                        }
                        else
                        {
                            throw new Exception($"该芯片使用的顶针在当前顶针架中不存在!");
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Enabled = true;
            }
        }
    }
}