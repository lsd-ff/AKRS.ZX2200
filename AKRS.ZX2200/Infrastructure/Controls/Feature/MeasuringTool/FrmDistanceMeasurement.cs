namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Controls.Manual;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;

    using DevExpress.XtraEditors;

    /// <summary>
    /// 距离测量
    /// </summary>
    public partial class FrmDistanceMeasurement : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmDistanceMeasurement()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 2;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 判断当前是否在运动，在运动的时候不能取消示教
        /// </summary>
        private bool isMove;

        /// <summary>
        /// 固晶模组
        /// </summary>
        private BondModule BondModule => System2Module.GetInstance().BondModule;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 视觉模板库窗体
        /// </summary>
        private FrmPRList frmPRList;

        /// <summary>
        /// 测量次数
        /// </summary>
        private int measureCount;

        /// <summary>
        /// 第一点
        /// </summary>
        private AKRSPoint3D point1;

        /// <summary>
        /// 第二点
        /// </summary>
        private AKRSPoint3D point2;

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 第一点
                    new AssistantConfig(
                        index: 0,
                        descritpion: "Step1/2:Move  to  first  piont.",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                            {
                                this.TileBarTeach.SelectedItem = this.TbiPosition2;

                                           if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                                           {
                                               this.point1 = this.bondModuleController.Get3DRealPosition();
                                           }
                                           else
                                {
                                    {
                                        this.point1 = System1Domain.GetInstance().DispenseController.GetG0Pos();
                                    }
                                }


                            },
                       doneAction: () =>
                       {
                       }),
                 
                    // 第二点
                    new AssistantConfig(
                        index: 1,
                        descritpion: "Step2/2:Move  to  second  piont.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: false,
                        isShowDone: true,
                        backAction: () =>
                        {
                            this.TileBarTeach.SelectedItem = this.TbiPosition1;
                        },
                        nextAction: () =>
                        {
                        },
                        doneAction: () =>
                            {
                               if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
                                {
                                   this.point2 = this.bondModuleController.Get3DRealPosition();
                                }
                                else
                                {
                                   this.point2 =System1Domain.GetInstance().DispenseController.GetG0Pos();
                                }

                                this.measureCount++;

                             // 两点距离过短报警（0.1mm）
                            if (Math.Abs(this.point2.X - this.point1.X) < 0.01&&Math.Abs(this.point2.Y - this.point1.Y) < 0.01)
                            {
                                DialogResult dialog = AKRSXtraMessageBox.Show(
                                    $"Distance too small\r\nThe distance between the teached points is too small.Distance should be at least 0.10 mm.\r\nRepeat with OK.\r\nStop assistant with CANCEL.",
                                    "Warn",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Warning);

                                if (dialog == DialogResult.OK)
                                {
                                    // 返回到第一步
                                    this.TileBarTeach.SelectedItem = this.TbiPosition1;
                                        this.stepIndex--;
                                    return;
                                }
                                else
                                {
                                    this.Close();
                                }
                            }
                            else
                                {
                                 FrmDistanceMeasureResult frmDistanceMeasureResult = new FrmDistanceMeasureResult(
                                    this.measureCount,
                                    this.point2.X - this.point1.X,
                                    this.point2.Y - this.point1.Y);

                                 this.ucGuideMove1 = new UcGuideMove("距离测量");

                                 this.Close();
                                    frmDistanceMeasureResult.TopMost = true;
                                    frmDistanceMeasureResult.StartPosition = FormStartPosition.CenterScreen;
                                    frmDistanceMeasureResult.Show();
                                    frmDistanceMeasureResult.BringToFront();
                                }


                            }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiPosition1;

            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2)
            {
                // 方向盘设置模组名称
                this.ucGuideMove1.ChangeModuleName("固晶模组", true);
            }
            else
            {
                // 方向盘设置模组名称
                this.ucGuideMove1.ChangeModuleName("点胶模组", true);
            }

        }

        /// <summary>
        /// 设置UI
        /// </summary>
        /// <param name="stepIndex">步骤索引</param>
        private void SetUIControl(int stepIndex)
        {
            AssistantConfig assistantConfig = this.assistantConfigList[stepIndex];

            this.LbDescription.Text = assistantConfig.Descritpion;
            this.BtBack.Visible = assistantConfig.IsShowBack;
            this.BtNext.Visible = assistantConfig.IsShowNext;
            this.BtDone.Visible = assistantConfig.IsShowDone;
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
            this.SetUIControl(this.stepIndex);
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
            this.SetUIControl(this.stepIndex);
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
        }

        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"End assistant?",
                "Question",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// 视觉校准按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnObjectToCenter_Click(object sender, EventArgs e)
        {
            // 弹出视觉窗体，用于吸嘴对准相机中心十字线
            if (this.frmPRList == null || this.frmPRList.IsDisposed)
            {
                this.frmPRList = new FrmPRList();
                this.frmPRList.Show();
            }
            else
            {
                // 弹出视觉窗体
                this.frmPRList.Show();
            }
        }
    }
}