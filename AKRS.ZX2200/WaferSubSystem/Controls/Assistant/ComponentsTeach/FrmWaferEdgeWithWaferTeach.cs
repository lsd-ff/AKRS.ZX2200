using System;
using System.Collections.Generic;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using System.Drawing;

    using AKRS.Galaxy2.Machine.Models;
    using AKRS.WM;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
    using AKRS.ZX2200.WaferSubSystem.Services;

    /// <summary>
    /// 导航示教-记录晶圆限位中心和半径
    /// </summary>
    public partial class FrmWaferEdgeWithWaferTeach : DevExpress.XtraEditors.XtraForm
    {
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 第一点
        /// </summary>
        private AKRSPoint3D firstPoint = new AKRSPoint3D();

        /// <summary>
        /// 第二点
        /// </summary>
        private AKRSPoint3D secondPoint = new AKRSPoint3D();

        /// <summary>
        /// 第三点
        /// </summary>
        private AKRSPoint3D thirdPoint = new AKRSPoint3D();

        /// <summary>
        /// 载具名称
        /// </summary>
        private CarrierWithWaferConfig waferCarrierWithWaferConfig;

        /// <summary>
        /// switch当前索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 3;

        /// <summary>
        /// 提示窗口
        /// </summary>
        private DialogResult res;

        /// <summary>
        /// AssistantConfigList
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        #region 模组与模组参数

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// EjectModule
        /// </summary>
        private EjectModule EjectModule => WaferSubModule.GetInstance().Eject;

        /// <summary>
        /// FlipChipModule
        /// </summary>
        private FlipModule FlipChipModule => WaferSubModule.GetInstance().FlipModule;

        /// <summary>
        /// WaferTableDevicePara
        /// </summary>
        private WaferTableDevicePara WaferTableDevicePara => WaferSubDevicePara.GetInstance().WaferTableDevicePara;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// FlipChipDevicePara
        /// </summary>
        private FlipTableDevicePara FlipChipDevicePara => WaferSubDevicePara.GetInstance().FlipChipDevicePara;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="waferCarrierConfig">carrierName</param>
        public FrmWaferEdgeWithWaferTeach(CarrierWithWaferConfig waferCarrierWithWaferConfig)
        {
            this.InitializeComponent();
            this.waferCarrierWithWaferConfig = waferCarrierWithWaferConfig;
            Block.GetInstance().SetCurrentCarrier(waferCarrierWithWaferConfig);
            this.InitControl();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            TUAssistantHelper.SetColor(this.TileBarTeach);
            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    // 步骤1
                    new AssistantConfig(
                        index: 0,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to 1st wafer edge position (first point of the wafer circle).",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请移动到晶圆边界的第一个点.",
                        isShowTitle: true,
                        isShowBack: false,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.firstPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),
                 
                    // 步骤2
                    new AssistantConfig(
                        index: 1,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to 2nd wafer edge position (second point of the wafer circle).",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请移动到晶圆边界的第二个点.",
                        isShowTitle: true,
                        isShowBack: true,
                        isShowNext: true,
                        isShowDone: false,
                        backAction: () =>
                        {
                        },
                        nextAction: () =>
                        {
                            this.secondPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;
                        },
                        doneAction: () =>
                        {
                        }),

                    // 步骤3
                    new AssistantConfig(
                        index: 2,
                        //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Move to 3rd wafer edge position(third point of the wafer circle).",
                        descritpion: $"步 {this.stepIndex++ + 1}/{stepCount}; 请移动到晶圆边界的第三个点.",
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
                            this.thirdPoint = WaferSubController.GetInstance().WaferTableController.WaferTableG0Pos;

                            AKRSPoint3D p1 = this.firstPoint;
                            AKRSPoint3D p2 = this.secondPoint;
                            AKRSPoint3D p3 = this.thirdPoint;
                            double x = 0, y = 0;
                            if (((p1.X == p2.X && p2.X == p3.X) || (p1.Y == p2.Y && p2.Y == p3.Y))  && !MachineStateModel.GetInstance().IsOffLineWork)
                            {
                                //AKRSXtraMessageBox.Show("Point selection did not pass, please re-select point!");
                                AKRSXtraMessageBox.Show("点位选择有误, 请重新选择点位!");
                                return;
                            }

                            x = (((p2.X * p2.X - p1.X * p1.X + p2.Y * p2.Y - p1.Y * p1.Y) / 2) * (p3.Y - p1.Y) - ((p3.X * p3.X - p1.X * p1.X + p3.Y * p3.Y - p1.Y * p1.Y) / 2) * (p2.Y - p1.Y)) / ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));
                            y = (((p3.X * p3.X - p1.X * p1.X + p3.Y * p3.Y - p1.Y * p1.Y) / 2) * (p2.X - p1.X) - ((p2.X * p2.X - p1.X * p1.X + p2.Y * p2.Y - p1.Y * p1.Y) / 2) * (p3.X - p1.X)) / ((p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y));

                            double radius = Math.Sqrt((x - p1.X) * (x - p1.X) + (y - p1.Y) * (y - p1.Y));
                            if (radius > WaferTableDevicePara.WaferTableRadius)
                            {
                                AKRSXtraMessageBox.Show("点位选择有误, 计算半径大于环限位半径，请重新选择点位!");
                                return;
                            }

                            this.waferCarrierWithWaferConfig.WaferCenter = new AKRSPoint3D(x, y, 0);
                            this.waferCarrierWithWaferConfig.WaferRadius = radius;

                            this.waferCarrierWithWaferConfig.WaferEdge.State = AssistantStateEnum.Able;
                            CarrierConfigRepository.GetInstance().Save();

                            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToG0Pos(this.waferCarrierWithWaferConfig.WaferCenter, true);

                            this.DialogResult = DialogResult.OK;
                        }),
                };

            ucGuideMove = new UcGuideMove("晶圆台模组", "FrmWaferEdgeWithWaferTeach", true, CameraEnum.WaferCamera);
            this.PnlControl.Controls.Add(ucGuideMove);
            ucGuideMove.Dock = DockStyle.Fill;

            // 首个步骤的UI设置
            this.stepIndex = 0;
        }

        /// <summary>
        /// 取消操作
        /// </summary>
        private void CancelOperation()
        {
            //this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "End assistant?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            this.res = AKRSXtraMessageBox.Show("Question 2.279:\r\n" + "是否结束示教?", "Prompt", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (this.res)
            {
                case DialogResult.Yes:
                    this.Close();
                    break;
                case DialogResult.No:
                    break;
            }
        }

        /// <summary>
        /// Back操作
        /// </summary>
        private void BackOperation()
        {
            this.stepIndex--;
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.BackAction();
        }

        /// <summary>
        /// Next操作
        /// </summary>
        private void NextOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.NextAction();
            this.stepIndex++;
        }

        /// <summary>
        /// Done操作
        /// </summary>
        private void DoneOperation()
        {
            AssistantConfig assistantConfig = this.assistantConfigList[this.stepIndex];
            assistantConfig.DoneAction();
            this.Close();
        }

        /// <summary>
        /// Back按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.BackOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Next按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.NextOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Done按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDone_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.DoneOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Cancel按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CancelOperation();
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btn.Appearance.BackColor = default;
                btn.Enabled = true;
            }
        }

        /// <summary>
        /// Timer1_Tick
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup2, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmWaferEdgeWithWaferTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ucGuideMove.Dispose();

            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }
        }
    }
}