using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.CommonModels;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.SupportFeature.Calibrate
{
    using System.Linq;
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.PR.Models.Entities;
    using AKRS.Galaxy2.PR.Models.MatchResults;
    using AKRS.Galaxy2.PR.Resipository;
    using AKRS.WM;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;

    /// <summary>
    ///  顶针校准示教
    /// </summary>
    public partial class FrmEjectionCaliTeach : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmEjectionCaliTeach(EjectionBankSlotConfig ejection)
        {
            InitializeComponent();
            this.currentBankSlotConfig = ejection;
            this.InitControl();

            this.Load += new System.EventHandler(FrmEjectionCaliTeach_Load);
        }

        /// <summary>
        ///  构造函数
        /// </summary>
        public FrmEjectionCaliTeach()
        {
            InitializeComponent();
            this.currentBankSlotConfig = WaferSubDevicePara.GetInstance().EjectDevicePara.CurrentSlotConfig;
            this.InitControl();

            this.Load += new System.EventHandler(FrmEjectionCaliTeach_Load);
        }

        /// <summary>
        /// 当前步骤的索引
        /// </summary>
        private int stepIndex = 0;

        /// <summary>
        /// 步数
        /// </summary>
        private int stepCount = 0;

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController BondHeadController => System2Domain.GetInstance().BondHeadController;

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// EjectDevicePara
        /// </summary>
        private EjectDevicePara EjectDevicePara => WaferSubDevicePara.GetInstance().EjectDevicePara;

        /// <summary>
        /// EjectMatchName
        /// </summary>
        private string ejectMatchName => this.currentBankSlotConfig.EjectionConfig.EjectMatchName;

        /// <summary>
        /// 界面配置集
        /// </summary>
        private List<AssistantConfig> assistantConfigList;

        /// <summary>
        ///  方向盘
        /// </summary>
        private UcGuideMove ucGuideMove1 = new UcGuideMove("FrmEjectionCaliTeach");

        /// <summary>
        /// 当前顶针槽位
        /// </summary>
        private EjectionBankSlotConfig currentBankSlotConfig;

        /// <summary>
        /// 是否弹出
        /// </summary>
        /// <returns>结果</returns>
        public bool IsShowDialog()
        {
            //WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            //if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            //{
            //    System2Domain.GetInstance().BondModuleController.MoveToSafePos();
            //}
            //else
            //{
            //    // 归还吸嘴（防撞）
            //    bool putBackRes = System2Domain.GetInstance().System2Controller.PutbackNozzleAssitance();

            //    if (putBackRes == false)
            //    {
            //        return false;
            //    }

            //    this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.X, CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.Y, 0) -
            //                                              new AKRSPoint3D(CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X, CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y, 0));
            //}           

            //WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            //// 确保顶针在安全状态
            //WaferSubController.GetInstance().EjectController.ReturnEjection();
            //WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            //// 判断是否有料片
            //WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            //// 顶针座升起
            //WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index,true);

            return true;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 绑定顶针
            this.CmbEjectionName.Properties.Items.Clear();

            List<EjectionBankSlotConfig> ejectionBankConfigList =
                WaferSystemProgram.GetInstance().GetDistinctEjectionBankSlotConfig();

            if (ejectionBankConfigList.Count != 0)
            {
                foreach (var item in ejectionBankConfigList)
                {
                    this.CmbEjectionName.Properties.Items.Add(item.EjectionConfig.Name);
                }

                if (EjectDevicePara.CurrentSlotConfig != null)
                {
                    this.CmbEjectionName.EditValue = EjectDevicePara.CurrentSlotConfig.EjectionConfig.Name;
                }
            }

            TUAssistantHelper.SetColor(this.TileBarTeach);

            string strStep1;
            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                strStep1 = "调整顶针位置和晶圆相机位置，使晶圆相机聚焦顶针,然后编辑PR";

                this.ucGuideMove1.ChangeCamera(CameraEnum.WaferCamera);

                // 方向盘设置模组名称
                ucGuideMove1.ChangeModuleName("顶针模组", true);
            }
            else
            {
                strStep1 = "调整顶针位置和BondZ轴位置，使Bond相机聚焦顶针（请不要移动BondXY轴位置）,然后编辑PR";

                this.ucGuideMove1.ChangeCamera(CameraEnum.BondCamera);

                // 方向盘设置模组名称
                ucGuideMove1.ChangeModuleName("固晶模组", true);
            }

            this.PnlControl.Controls.Add(ucGuideMove1);

            this.assistantConfigList
                = new List<AssistantConfig>
                {
                    new AssistantConfig(
                        index: 0,
                        descritpion: $"1/1: "+strStep1,
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
                           // 检查PR
                           PREntity pREntity = (PREntity)VisionEntityRepository.GetInstance().Find(this.ejectMatchName);

                           if (pREntity == null)
                           {
                               DialogResult dialog1 = AKRSXtraMessageBox.Show(
                                   $"请先编辑视觉模板!",
                                   "Warn",
                                   MessageBoxButtons.OKCancel,
                                   MessageBoxIcon.Warning);

                               if (dialog1 == DialogResult.Cancel)
                               {
                                   this.DialogResult = DialogResult.Cancel;
                               }

                               return;
                           }

                           if (this.EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
                           {
                               (bool isSucceed, MatchResult[] matchResults) result = Block.GetInstance().MatchResult(this.ejectMatchName, false, false);

                               if (result.isSucceed)
                               {
                                   // 重新计算
                                   this.currentBankSlotConfig.EjectionConfig.DeviationWithEjectionCenterAndWaferCameraCenter = Block.GetInstance().MatchResultToWorld(result.matchResults)[0];
                               }
                               else
                               {
                                   AKRSXtraMessageBox.Show("顶针定位失败！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                   return;
                               }

                               // 保存相机拍照位
                               this.currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos = WaferSubController
                                   .GetInstance().WaferTableController.WaferCameraAxisZG0Pos;
                           }
                           else
                           {
                               // 执行定位
                               MatchResult res = (MatchResult)System2Domain.GetInstance().System2CommonVision(
                                   null,
                                   "EjectionCali",
                                   this.ejectMatchName,
                                   false,
                                   CameraTypeEnum.BondCamera);

                               if (res == null || !res.IsSuccess)
                               {
                                   AKRSXtraMessageBox.Show("定位失败，请检查视觉模板！");
                                   return;
                               }

                               // 重新计算
                               this.currentBankSlotConfig.EjectionConfig
                                       .DeviationWithEjectionCenterAndWaferCameraCenter =
                                   this.bondModuleController.ConvertPixelToG0Pos(
                                       this.bondModuleController.Get3DRealPosition(),
                                       res) - this.bondModuleController.GetG0RealPosition();

                               // 保存相机拍照位
                               this.currentBankSlotConfig.EjectionConfig.EjectionCaliVsionPos =
                                   this.bondModuleController.GetG0RealPosition();
                           }

                           // 顶针校准位置
                           this.currentBankSlotConfig.EjectionConfig.EjectionCaliLevel =
                               WaferSubController.GetInstance().EjectController.EjectionAxisZG0Pos;

                           this.currentBankSlotConfig.EjectionConfig.EjectionPRTeach.State = AssistantStateEnum.Able;

                           this.bondModuleController.MoveToSafePos();
                           WaferSubController.GetInstance().EjectController.ReturnEjection();

                           this.DialogResult = DialogResult.OK;
                       }),
                };

            // 首个步骤的AssistantConfig
            this.SetUIControl(0);

            // 设置高亮
            this.TileBarTeach.SelectedItem = this.TbiEditPr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEjectionCaliTeach_Load(object sender, EventArgs e)
        {
            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                System2Domain.GetInstance().BondModuleController.MoveToSafePos();
            }
            else
            {
                // 归还吸嘴（防撞）
                bool putBackRes = System2Domain.GetInstance().System2Controller.PutbackNozzleAssitance();

                if (putBackRes == false)
                {
                    return;
                }

                this.bondModuleController.MoveSafeBondXYZ(new AKRSPoint3D(CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.X, CalibrateRunPara.GetInstance().BondHeadRotateCenterInWC.Y, 0) -
                                                          new AKRSPoint3D(CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X, CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y, 0));
            }

            WaferSubController.GetInstance().EjectController.CloseEjectionTableVacuum();

            // 确保顶针在安全状态
            WaferSubController.GetInstance().EjectController.ReturnEjection();
            WaferSubController.GetInstance().WaferTableController.MoveWaferTableToReadyPosition();

            // 判断是否有料片
            WaferSubController.GetInstance().WaferTableController.JudgeTabletOnWaferTable(false);

            if (this.currentBankSlotConfig != null)
            {
                // 顶针座升起
                WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index, true);
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
        /// 取消按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                $"是否结束示教?",
                "问题",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialog == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Cancel;
            }
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
            EjectionConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 自动对焦按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnAutoFocus_Click(object sender, EventArgs e)
        {
            if (this.EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                SimpleButton btn = sender as SimpleButton;
                try
                {
                    btn.Enabled = false;
                    btn.Appearance.BackColor = Color.Yellow;

                    WaferSubController.GetInstance().WaferTableController.Autofocus();
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
            else
            {
                this.BondHeadController.AutoFocusAssistance(CameraTypeEnum.BondCamera, sender, this);
            }
        }

        /// <summary>
        /// 编辑模板
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditPR_Click(object sender, EventArgs e)
        {
            if (this.EjectDevicePara.IsUseWaferCameraAssistantEjectCenter)
            {
                WaferSubController.GetInstance().WaferTableController.EditPr(this.ejectMatchName);
            }
            else
            {
                TUAssistantHelper.EditPrInSystem2(this.ejectMatchName, CameraTypeEnum.BondCamera);
            }
        }

        /// <summary>
        ///  顶针升降
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESUpOrDown_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.ReturnEjection();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.ChangeEjection(this.currentBankSlotConfig.Index, true);
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

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmEjectionCaliTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            WaferSubController.GetInstance().EjectController.ReturnEjection();
            this.bondModuleController.MoveToSafePos();
            this.ucGuideMove1?.Dispose();
        }

        /// <summary>
        /// 更换顶针
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnChangeEjection_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                string slotName = this.CmbEjectionName.Text;
                EjectionBankSlotConfig slotConfig = WaferSystemProgram.GetInstance().EjectionBankProgram.CurrentBankConfig.EjectionBankSlots.FirstOrDefault(a => a.Name == slotName);

                if (slotConfig != null)
                {
                    WaferSubController.GetInstance().EjectController.ChangeEjection(slotConfig.Index, true);
                    this.currentBankSlotConfig = slotConfig;
                }
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
    }
}