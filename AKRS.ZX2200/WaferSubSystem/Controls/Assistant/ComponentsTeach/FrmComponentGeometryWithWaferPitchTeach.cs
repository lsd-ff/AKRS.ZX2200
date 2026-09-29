using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Machine;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.CommonModels;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.XtraEditors;
    using System.Drawing;
    using System.Threading;

    /// <summary>
    /// 芯片跳转间距示教
    /// </summary>
    public partial class FrmComponentGeometryWithWaferPitchTeach : DevExpress.XtraEditors.XtraForm
    {
        UcGuideMove ucGuideMove;

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
        /// 列间距
        /// </summary>
        public double pitchCol;

        /// <summary>
        /// 行间距
        /// </summary>
        public double pitchRow;

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
        public FrmComponentGeometryWithWaferPitchTeach(CarrierWithWaferConfig carrierWithWaferConfig)
        {
            this.InitializeComponent();
            this.carrierWithWaferConfig = carrierWithWaferConfig;
            this.InitControl();            
        }

        private CarrierWithWaferConfig carrierWithWaferConfig;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            Machine.GetInstance().OpenAlarm();
            TUAssistantHelper.SetColor(this.TileBarTeach);

            string tip = this.carrierWithWaferConfig.SearchCamera == SearchCameraEnum.BondCamera
                             ? "在不移动BondXY轴的情况下将焊头Z轴移动看清芯片的高度再"
                             : string.Empty;

            this.assistantConfigList = new List<AssistantConfig>
                                           {
                                               // 步骤1
                                               new AssistantConfig(
                                                   index: 0,
                                                   //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Please replace the wafer diagram.",
                                                   descritpion:
                                                   $"Step {this.stepIndex++ + 1}/{stepCount}; {tip}将晶圆台对准某一颗芯片的左上角（需调整扩晶环和晶圆相机位置，才能看清芯片）."
                                                   ,
                                                   isShowTitle: true,
                                                   isShowBack: false,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.firstPoint = WaferSubController.GetInstance()
                                                               .WaferTableController.WaferTableG0Pos;
                                                       },
                                                   doneAction: () =>
                                                       {
                                                       }),

                                               // 步骤2
                                               new AssistantConfig(
                                                   index: 1,
                                                   //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Set the actual location of reference point 1. ",
                                                   descritpion:
                                                   $"Step {this.stepIndex++ + 1}/{stepCount}; 请将晶圆台对准右边芯片的左上角. ",
                                                   isShowTitle: true,
                                                   isShowBack: true,
                                                   isShowNext: true,
                                                   isShowDone: false,
                                                   backAction: () =>
                                                       {
                                                       },
                                                   nextAction: () =>
                                                       {
                                                           this.secondPoint = WaferSubController.GetInstance()
                                                               .WaferTableController.WaferTableG0Pos;
                                                       },
                                                   doneAction: () =>
                                                       {
                                                       }),

                                               // 步骤4
                                               new AssistantConfig(
                                                   index: 2,
                                                   //descritpion: $"Step {this.stepIndex++ + 1}/{stepCount}; Click Next will move to the starting point and continue the thread.",
                                                   descritpion:
                                                   $"Step {this.stepIndex++ + 1}/{stepCount}; 请将晶圆台对准下边芯片的左上角.",
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
                                                           this.thirdPoint = WaferSubController.GetInstance()
                                                               .WaferTableController.WaferTableG0Pos;

                                                           pitchCol = Math.Abs(this.firstPoint.X - this.secondPoint.X);
                                                           pitchRow = Math.Abs(this.thirdPoint.Y - this.secondPoint.Y);

                                                           this.DialogResult = DialogResult.OK;
                                                       }),
                                           };

            if (this.carrierWithWaferConfig.SearchCamera == SearchCameraEnum.BondCamera)
            {
                ucGuideMove = new UcGuideMove("固晶模组", "FrmComponentGeometryWithWaferPitchTeach", true, CameraEnum.WaferCamera);
                ucGuideMove.ChangeCamera(CameraEnum.BondCamera);
                ucGuideMove.IsSetCameraParematerByModule = false;
            }
            else
            {
                ucGuideMove = new UcGuideMove("晶圆台模组", "FrmComponentGeometryWithWaferPitchTeach", true, CameraEnum.WaferCamera);
            }

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
                    this.DialogResult = DialogResult.Abort;
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

                Thread.Sleep(500);
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
            WaferSubController.GetInstance().SetUIControl(this.TileBarTeach, this.tileBarGroup5, this.assistantConfigList, this.stepIndex, this.BtBack, this.BtNext, this.BtDone, this.LbDescription);
        }

        private void FrmComponentGeometryWithWaferPitchTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.timer1 != null)
            {
                this.timer1.Stop();
                this.timer1.Tick -= this.Timer1_Tick;
                this.timer1.Dispose();
                this.timer1 = null;
            }

            Machine.GetInstance().ResetAlarm();

            this.ucGuideMove.Dispose();
        }

        private void BtnBlockCyc_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.ResetBlockCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.SetBlockCylinder();
                    btn.Appearance.BackColor = Color.Yellow;
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