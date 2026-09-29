using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Machine.Models;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Models.Repositories.Nozzle;
using AKRS.ZX2200.WaferSubSystem.Controllers;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.BondSystem.Models.Parameter;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Infrastructure.Models.Enums;
    using AKRS.ZX2200.Main.Controls;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Parameters;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;

    /// <summary>
    /// 芯片拾取示教窗体
    /// </summary>
    public partial class FrmComponentPickup : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="baseCarrierConfig">传入的芯片对象</param>
        public FrmComponentPickup(BaseCarrierConfig baseCarrierConfig)
        {
            this.component = baseCarrierConfig;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 焊头控制器
        /// </summary>
        private BondHeadController bondHeadController = new BondHeadController();

        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController = new BondModuleController();

        /// <summary>
        /// 系统2控制器
        /// </summary>
        private System2Controller system2Controller = new System2Controller();

        /// <summary>
        /// 翻转台控制器
        /// </summary>
        private FlipTableController flipTableController = new FlipTableController();

        /// <summary>
        /// 取片线程
        /// </summary>
        private Task pickupTask;

        /// <summary>
        /// 点击Start传进来的芯片对象
        /// </summary>
        private BaseCarrierConfig component;

        /// <summary>
        /// 晶圆检查是否通过
        /// </summary>
        private bool isWaferCheckSucceed = false;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            // 先关掉主界面的方向盘
            UcMainSystem.UcGuideMoveClose();

            #region 控件赋值

            this.SpPickupDelay.Value = (decimal)this.component.PickupDelay;
            this.SpPickupForce.Value = (decimal)this.component.PickupForce;
            this.SpPickupDistance.Value = (decimal)this.component.PickupDistance;
            this.SpPickAngleOffset.Value = (decimal)this.component.PickAngleOffset;

            //this.SpVacuumTime.Value = (decimal)this.component.NozzleVacuumBuildUpTime;
            this.SpSlowTravelBeforePickupSpeed.Value = (decimal)this.component.SlowTravelSpeedBeforePickup;
            this.SpSlowTravelAfterPickupSpeed.Value = (decimal)this.component.SlowTravelSpeedAfterPickup;
            this.SpTravelBeforePickupHeight.Value = (decimal)this.component.SlowTravelDistanceBeforePickup;
            this.SpTravelAfterPickupHeight.Value = (decimal)this.component.SlowTravelDistanceAfterPickup;

            this.CmbPickupForceMode.Properties.Items.Clear();
            List<string> list = Enum.GetValues(typeof(ForceModeEnum)).Cast<ForceModeEnum>().Select(x => x.ToString())
                .ToList();
            this.CmbPickupForceMode.Properties.Items.AddRange(list);
            this.CmbPickupForceMode.Text = this.component.PickupForceMode.ToString();

            this.CmbTravelBeforePickupSpeed.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbTravelBeforePickupSpeed.Properties.Items.AddRange(list);
            this.CmbTravelBeforePickupSpeed.SelectedIndex = 0;

            this.CmbTravelAfterPickupSpeed.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbTravelAfterPickupSpeed.Properties.Items.AddRange(list);
            this.CmbTravelAfterPickupSpeed.SelectedIndex = 0;


            this.CmbEjectionSpeedMode.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbEjectionSpeedMode.Properties.Items.AddRange(list);
            this.CmbEjectionSpeedMode.SelectedIndex = 0;

            this.CmbSlowTravelSpeedModeBeforeFlip.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbSlowTravelSpeedModeBeforeFlip.Properties.Items.AddRange(list);
            this.CmbSlowTravelSpeedModeBeforeFlip.SelectedIndex = 0;

            this.CmbSlowTravelSpeedModeAfterFlip.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbSlowTravelSpeedModeAfterFlip.Properties.Items.AddRange(list);
            this.CmbSlowTravelSpeedModeAfterFlip.SelectedIndex = 0;

            this.ChkActivateSlowTravelBeforePickup.Checked = this.component.IsActivateSlowTravelBeforePickup;
            this.ChkActivateSlowTravelAfterPickup.Checked = this.component.IsActivateSlowTravelAfterPickup;

            this.SpPickOffsetX.Value = (decimal)this.component.PickupOffset.X;
            this.SpPickOffsetY.Value = (decimal)this.component.PickupOffset.Y;

            this.ChkActiveSlowTravelBeforeFlip.Checked = this.component.IsActivateSlowTravelBeforeFlip;
            this.ChkActiveSlowTravelAfterFlip.Checked = this.component.IsActivateSlowTravelAfterFlip;

            this.SpSlowTravelDistanceBeforeFlip.Value = (decimal)this.component.SlowTravelDistanceBeforeFlip;
            this.SpSlowTravelSpeedBeforeFlip.Value = (decimal)this.component.SlowTravelSpeedBeforeFlip;

            this.SpSlowTravelDistanceAfterFlip.Value = (decimal)this.component.SlowTravelDistanceAfterFlip;
            this.SpSlowTravelSpeedAfterFlip.Value = (decimal)this.component.SlowTravelSpeedAfterFlip;

            this.SpFlipArmOverTravelDistanceWafer.Value = (decimal)this.component.FlipArmOverTravelDistanceWafer;
            this.SpVaccumAtEndOfProcess.Value = (decimal)this.component.FlipToolVacuumAtEndOfProcess;

            this.SpFlipToolPickupDelay.Value = (decimal)this.component.FlipToolPickupDelay;

            // 判断芯片类型
            if (this.component is CarrierWithWaferConfig carrierWithWafer)
            {
                // 晶圆芯片属性
                this.SpEjectionSpeed.Value = (decimal)carrierWithWafer.EjectLiftSpeed;
                this.CmbEjectionSpeedMode.Text = carrierWithWafer.EjectionSpeedMode.ToString();
                this.ChkSynchronousEjection.Checked = carrierWithWafer.IsActivateSynchronousEjection;
                this.SpEjectionHeight.Value = (decimal)carrierWithWafer.RelativeHeightWithEjectionReadyLiftPosition;

                this.gCEjection.Enabled = true;
            }
            else
            {
                // 华夫盒芯片
                this.gCEjection.Enabled = false;
            }

            #endregion

            #region 界面防呆

            this.LbComponentName.Text = this.component.Name;
            this.LbComponentName.Enabled = false;

            if (this.CmbPickupForceMode.Text == ForceModeEnum.Distance.ToString())
            {
                this.SpPickupDistance.Enabled = true;
                this.SpPickupForce.Enabled = false;
            }
            else
            {
                this.SpPickupDistance.Enabled = false;
                this.SpPickupForce.Enabled = true;
            }

            if (this.ChkActivateSlowTravelBeforePickup.Checked)
            {
                this.CmbTravelBeforePickupSpeed.Enabled = true;
                this.SpSlowTravelBeforePickupSpeed.Enabled = true;
                this.SpTravelBeforePickupHeight.Enabled = true;
            }
            else
            {
                this.CmbTravelBeforePickupSpeed.Enabled = false;
                this.SpSlowTravelBeforePickupSpeed.Enabled = false;
                this.SpTravelBeforePickupHeight.Enabled = false;
            }

            if (this.ChkActivateSlowTravelAfterPickup.Checked)
            {
                this.CmbTravelAfterPickupSpeed.Enabled = true;
                this.SpSlowTravelAfterPickupSpeed.Enabled = true;
                this.SpTravelAfterPickupHeight.Enabled = true;
            }
            else
            {
                this.CmbTravelAfterPickupSpeed.Enabled = false;
                this.SpSlowTravelAfterPickupSpeed.Enabled = false;
                this.SpTravelAfterPickupHeight.Enabled = false;
            }

            this.SpSlowTravelDistanceBeforeFlip.Enabled = this.component.IsActivateSlowTravelBeforeFlip;
            this.SpSlowTravelSpeedBeforeFlip.Enabled = this.component.IsActivateSlowTravelBeforeFlip;

            this.SpSlowTravelDistanceAfterFlip.Enabled = this.component.IsActivateSlowTravelAfterFlip;
            this.SpSlowTravelDistanceAfterFlip.Enabled = this.component.IsActivateSlowTravelAfterFlip;

            if (this.component.IsUseFlipTable == false)
            {
                this.XTabTool.TabPages.Remove(this.XTPFlipTool);
            }

            this.gCFlipToolPickupDelay.Enabled = this.component.IsUseFlipTable;

            this.SpSlowTravelDistanceBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;
            this.CmbSlowTravelSpeedModeBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;
            this.SpSlowTravelSpeedBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;

            this.SpSlowTravelDistanceAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;
            this.CmbSlowTravelSpeedModeAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;
            this.SpSlowTravelSpeedAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;

            ChkSynchronousEjection.Enabled = this.CmbPickupForceMode.SelectedText == ForceModeEnum.Distance.ToString();

            #endregion

           this. SpSlowTravelAfterPickupSpeed.Properties.MinValue =(decimal) 0.01;

            isInit = true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.component.PickupForceMode = (ForceModeEnum)Enum.Parse(
    typeof(ForceModeEnum),
    this.CmbPickupForceMode.Text);

            this.component.PickupDelay = (int)this.SpPickupDelay.Value;
            this.component.PickupForce = (double)this.SpPickupForce.Value;
            this.component.PickupDistance = (double)this.SpPickupDistance.Value;
            this.component.PickAngleOffset = (double)this.SpPickAngleOffset.Value;

            //this.component.NozzleVacuumBuildUpTime = (int)this.SpVacuumTime.Value;
            this.component.SlowTravelSpeedBeforePickup = (double)this.SpSlowTravelBeforePickupSpeed.Value;
            this.component.SlowTravelSpeedAfterPickup = (double)this.SpSlowTravelAfterPickupSpeed.Value;
            this.component.SlowTravelDistanceBeforePickup = (double)this.SpTravelBeforePickupHeight.Value;
            this.component.SlowTravelDistanceAfterPickup = (double)this.SpTravelAfterPickupHeight.Value;

            this.component.IsActivateSlowTravelBeforePickup = this.ChkActivateSlowTravelBeforePickup.Checked;
            this.component.IsActivateSlowTravelAfterPickup = this.ChkActivateSlowTravelAfterPickup.Checked;

            this.component.PickupOffset.X = (double)this.SpPickOffsetX.Value;
            this.component.PickupOffset.Y = (double)this.SpPickOffsetY.Value;

            this.component.IsActivateSlowTravelBeforeFlip = this.ChkActiveSlowTravelBeforeFlip.Checked;
            this.component.IsActivateSlowTravelAfterFlip = this.ChkActiveSlowTravelAfterFlip.Checked;

            this.component.SlowTravelDistanceBeforeFlip = (double)this.SpSlowTravelDistanceBeforeFlip.Value;
            this.component.SlowTravelSpeedBeforeFlip = (double)this.SpSlowTravelSpeedBeforeFlip.Value;

            this.component.SlowTravelDistanceAfterFlip = (double)this.SpSlowTravelDistanceAfterFlip.Value;
            this.component.SlowTravelSpeedAfterFlip = (double)this.SpSlowTravelSpeedAfterFlip.Value;

            this.component.FlipArmOverTravelDistanceWafer = (double)this.SpFlipArmOverTravelDistanceWafer.Value;
            this.component.FlipToolVacuumAtEndOfProcess = (int)this.SpVaccumAtEndOfProcess.Value;

            this.component.FlipToolPickupDelay = (int)this.SpFlipToolPickupDelay.Value;

            // 判断芯片类型
            if (this.component is CarrierWithWaferConfig carrierWithWafer)
            {
                // 晶圆芯片属性
                carrierWithWafer.EjectLiftSpeed = (double)this.SpEjectionSpeed.Value;
                carrierWithWafer.IsActivateSynchronousEjection = this.ChkSynchronousEjection.Checked;
                carrierWithWafer.EjectionSpeedMode = (SpeedModeEnum)Enum.Parse(
                    typeof(SpeedModeEnum),
                    this.CmbEjectionSpeedMode.Text);
                //carrierWithWafer.EjectionHeight = (double)this.SpEjectionHeight.Value;

                carrierWithWafer.RelativeHeightWithEjectionReadyLiftPosition = (double)this.SpEjectionHeight.Value;
            }
            else if (this.component is CarrierWithWaffleConfig carrierWithWaffle)
            {
                // 华夫盒芯片
                BtnESInformation.Enabled = false;
            }

            CarrierConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            // 保存参数
            this.Save();

            this.component.ComponentPickup.State = AssistantStateEnum.Able;
            CarrierConfigRepository.GetInstance().Save();

            // 关闭窗体
            this.DialogResult = DialogResult.OK;
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
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbPickupForceMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbPickupForceMode.SelectedText == ForceModeEnum.Distance.ToString())
            {
                this.SpPickupDistance.Enabled = true;
                this.SpPickupForce.Enabled = false;
            }
            else
            {
                this.SpPickupDistance.Enabled = false;
                this.SpPickupForce.Enabled = true;
            }

            ChkSynchronousEjection.Enabled = this.CmbPickupForceMode.SelectedText == ForceModeEnum.Distance.ToString();
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTravelBeforePickupSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSpinEditValueByCmb(this.CmbTravelBeforePickupSpeed, this.SpSlowTravelBeforePickupSpeed);
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTravelAfterPickupSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSpinEditValueByCmb(this.CmbTravelAfterPickupSpeed, this.SpSlowTravelAfterPickupSpeed);
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbEjectionSpeedMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSpinEditValueByCmb(this.CmbEjectionSpeedMode, this.SpEjectionSpeed);
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActivateSlowTravelBeforePickup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkActivateSlowTravelBeforePickup.Checked)
            {
                this.CmbTravelBeforePickupSpeed.Enabled = true;
                this.SpSlowTravelBeforePickupSpeed.Enabled = true;
                this.SpTravelBeforePickupHeight.Enabled = true;
            }
            else
            {
                this.CmbTravelBeforePickupSpeed.Enabled = false;
                this.SpSlowTravelBeforePickupSpeed.Enabled = false;
                this.SpTravelBeforePickupHeight.Enabled = false;
            }

            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActivateSlowTravelAfterPickup_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkActivateSlowTravelAfterPickup.Checked)
            {
                this.CmbTravelAfterPickupSpeed.Enabled = true;
                this.SpSlowTravelAfterPickupSpeed.Enabled = true;
                this.SpTravelAfterPickupHeight.Enabled = true;
            }
            else
            {
                this.CmbTravelAfterPickupSpeed.Enabled = false;
                this.SpSlowTravelAfterPickupSpeed.Enabled = false;
                this.SpTravelAfterPickupHeight.Enabled = false;
            }

            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 吸嘴信息展示
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPPInformation_Click(object sender, EventArgs e)
        {
            TreeGroupParentNodesEnum treeGroupParentNodesEnum = TreeGroupParentNodesEnum.Tool;
            TreeGroupChildNodesEnum treeGroupChildNodesEnum = TreeGroupChildNodesEnum.PPandepoxytools;
            string dataSetName = this.component.NozzleName;

            if (string.IsNullOrEmpty(this.component.NozzleName))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先为芯片配置吸嘴!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Question);

                return;
            }

            UcProgramArgs ucProgramArgs = new UcProgramArgs();
            ucProgramArgs = ucProgramArgs.FindPage(
                ucProgramArgs,
                treeGroupParentNodesEnum,
                treeGroupChildNodesEnum,
                dataSetName);

            XtraForm xtraForm = new XtraForm() { Width = 1500, Height = 800 };
            ucProgramArgs.SetFocusedNodeByDisplayText(
                ucProgramArgs.treeGroupList,
                ucProgramArgs.TreeGroupChildNodesEnumToNodesDictionary[treeGroupChildNodesEnum.ToString()]);
            ucProgramArgs.SetFocusedNodeByDisplayText(ucProgramArgs.treeDataSetList, dataSetName);

            xtraForm.Controls.Add(ucProgramArgs);
            xtraForm.ShowDialog();

            xtraForm.Dispose();
        }

        /// <summary>
        /// 顶针信息展示
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnESInformation_Click(object sender, EventArgs e)
        {
            CarrierWithWaferConfig carrierWithWaferConfigWithWafer = (CarrierWithWaferConfig)this.component;

            TreeGroupParentNodesEnum treeGroupParentNodesEnum = TreeGroupParentNodesEnum.Tool;
            TreeGroupChildNodesEnum treeGroupChildNodesEnum = TreeGroupChildNodesEnum.Ejectionsystem;
            string dataSetName = carrierWithWaferConfigWithWafer.EjectionName;

            if (string.IsNullOrEmpty(carrierWithWaferConfigWithWafer.EjectionName))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先为芯片配置顶针!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Question);

                return;
            }

            UcProgramArgs ucProgramArgs = new UcProgramArgs();
            ucProgramArgs = ucProgramArgs.FindPage(
                ucProgramArgs,
                treeGroupParentNodesEnum,
                treeGroupChildNodesEnum,
                dataSetName);

            XtraForm xtraForm = new XtraForm() { Width = 1500, Height = 800 };
            ucProgramArgs.SetFocusedNodeByDisplayText(
                ucProgramArgs.treeGroupList,
                ucProgramArgs.TreeGroupChildNodesEnumToNodesDictionary[treeGroupChildNodesEnum.ToString()]);
            ucProgramArgs.SetFocusedNodeByDisplayText(ucProgramArgs.treeDataSetList, dataSetName);

            xtraForm.Controls.Add(ucProgramArgs);
            xtraForm.ShowDialog();

            xtraForm.Dispose();
        }

        /// <summary>
        /// 模拟取料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPickupTest_Click(object sender, EventArgs e)
        {
            // 保存对象属性
            this.Save();

            if (MachineHardwareConfiguration.GetInstance().IsSafeDoorConfigured
                && !Machine.GetInstance().IsSafeDoorClose)
            {
                AKRSMessageBoxExt.Show(
                    $"安全门未关闭，请关闭安全门后再进行取片测试",
                    "报警",
                    new string[] { "确认" },
                    new DialogResult[] { DialogResult.OK });

                return;
            }

            if (!system2Controller.ChangeNozzleAssistance(this.component.NozzleName))
            {
                return;
            }

            if (!isWaferCheckSucceed)
            {
                // 检查
                if (!WaferSystemDomain.GetInstance().CheckIsReady(true))
                {
                    return;
                }

                isWaferCheckSucceed = true;
            }

            // 复位信号
            Machine.GetInstance().ResetMachineSignal();
            System2RunTimeProvider.ResetInformation();

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

            System2RunTimeProvider.RecordTime("取片测试", "给晶圆台发要料信号完成");

            // 防呆
            SimpleButton btn = sender as SimpleButton;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            // 防止线程多次启动
            if (this.pickupTask == null || this.pickupTask.IsCompleted == true)
            {
                // 初始化搜精
                Block.GetInstance().StartInit();

                this.pickupTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("取片测试线程");

                            this.Pickup();
                        });
            }
        }

        /// <summary>
        /// 取片
        /// </summary>
        private void Pickup()
        {
            try
            {
                bool ret;

                if (this.component.IsUseFlipTable == false)
                {
                    ret = this.system2Controller.PickupFromWaferTable(component);
                }
                else
                {
                    ret = this.system2Controller.PickupFromFlipTable(component);
                }

                if (!ret)
                {
                    return;
                }

                Nozzle nozzle = this.bondHeadController.GetCurrentNozzle();

                if (nozzle.ToolAlignment.State != AssistantStateEnum.Able)
                {
                    // 去上视
                    this.bondModuleController.MoveToUpLookPos();
                }
                else
                {
                    // 计算焊头旋转后的吸嘴偏移
                    AKRSPoint2D nozzleOffsetRotated =
                        this.bondHeadController.GetNozzleOffset(nozzle.Name, -nozzle.AlignAngle);

                    AKRSPoint3D pos = new AKRSPoint3D { X = nozzle.RotateCenterPos.X - nozzleOffsetRotated.X, Y = nozzle.RotateCenterPos.Y - nozzleOffsetRotated.X, Z = nozzle.RotateCenterPos.Z + component.ComponentThickness };

                    // 移到吸嘴旋转中心
                    this.bondModuleController.MoveToG0Pos(pos);
                }

                System2RunTimeProvider.RecordTime("取片测试", "去上视完成");
            }
            catch (Exception ex)
            {
                System2RunTimeProvider.RecordTime("取片测试", $"取片测试异常，{ex.ToString()}");

                Machine.GetInstance().Stop();

                AKRSMessageBoxExt.Show(
                                            $"取片失败:" + ex.Message,
                              "报警",
                              new string[] { "确认" },
                              new DialogResult[] { DialogResult.OK });
            }
            finally
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                            {
                                UcMainSystem.UcGuideMoveOpen("上视模组");

                                // 弹出视觉窗体
                                UcMainSystem.VmVisionShow();
                                UcMainSystem.ChangeCameraVision("上视相机");

                                BtnPickupTest.Appearance.BackColor = default;
                                BtnPickupTest.Enabled = true;
                            }));
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentPickup_FormClosed(object sender, FormClosedEventArgs e)
        {
            Machine.GetInstance().Stop();

            UcMainSystem.UcGuideMoveClose();
            //this.frmBear?.Dispose();
            //this.ucGuideMove?.Dispose();
        }

        /// <summary>
        /// 停止取料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStopPick_Click(object sender, EventArgs e)
        {
            Machine.GetInstance().Stop();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentPickup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.pickupTask != null && this.pickupTask.IsCompleted == false)
            {
                e.Cancel = true;
            }

            // 退出单步
            MachineStateModel.GetInstance().IsSingleStepWork = false;
        }

        /// <summary>
        /// 通过lookUpEdit来改变SpinEdit的值
        /// </summary>
        /// <param name="comboBoxEdit">下拉框</param>
        /// <param name="spinEdit">SpinEdit</param>
        private void ChangeSpinEditValueByCmb(ComboBoxEdit comboBoxEdit, SpinEdit spinEdit)
        {
            if (comboBoxEdit.SelectedText == SpeedModeEnum.Free.ToString())
            {
                spinEdit.Enabled = true;
            }
            else if (comboBoxEdit.SelectedText == SpeedModeEnum.Low.ToString())
            {
                spinEdit.Value = 10;
                spinEdit.Enabled = false;
            }
            else if (comboBoxEdit.SelectedText == SpeedModeEnum.Medium.ToString())
            {
                spinEdit.Value = 50;
                spinEdit.Enabled = false;
            }
            else if (comboBoxEdit.SelectedText == SpeedModeEnum.Fast.ToString())
            {
                spinEdit.Value = 100;
                spinEdit.Enabled = false;
            }
        }

        /// <summary>
        /// 回车触发事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentPickup_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.F9)
            //{
            //    if (MachineStateModel.GetInstance().IsSingleStepWork)
            //    {
            //        // 单步执行
            //        SignalPool.GetInstance().System2SingleStepSignal.Set();
            //    }
            //}

            //if (Control.ModifierKeys == Keys.Shift)
            //{
            //    switch (e.KeyCode)
            //    {                                       
            //        case Keys.F9:
            //            MachineStateModel.GetInstance().IsSingleStepWork =
            //                !MachineStateModel.GetInstance().IsSingleStepWork;

            //            break;
            //        case Keys.F10:
            //            break;
            //        case Keys.F11:
            //            break;
            //        case Keys.F12:
            //            break;
            //    }
            //}
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActiveSlowTravelAfterFlip_CheckedChanged(object sender, EventArgs e)
        {
            this.SpSlowTravelDistanceAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;
            this.CmbSlowTravelSpeedModeAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;
            this.SpSlowTravelSpeedAfterFlip.Enabled = this.ChkActiveSlowTravelAfterFlip.Checked;
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActiveSlowTravelBeforeFlip_CheckedChanged(object sender, EventArgs e)
        {
            this.SpSlowTravelDistanceBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;
            this.CmbSlowTravelSpeedModeBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;
            this.SpSlowTravelSpeedBeforeFlip.Enabled = this.ChkActiveSlowTravelBeforeFlip.Checked;
        }

        /// <summary>
        /// 下拉框改改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbSlowTravelSpeedModeBeforeFlip_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSpinEditValueByCmb(this.CmbSlowTravelSpeedModeBeforeFlip, this.SpSlowTravelSpeedBeforeFlip);
        }

        /// <summary>
        /// 下拉框改改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbSlowTravelSpeedModeAfterFlip_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSpinEditValueByCmb(this.CmbSlowTravelSpeedModeAfterFlip, this.SpSlowTravelSpeedAfterFlip);
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpTravelAfterPickupHeight_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSlowTravelAfterPickupSpeed_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpTravelBeforePickupHeight_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSlowTravelBeforePickupSpeed_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpEjectionHeight_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpEjectionSpeed_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpPickupDelay_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.PnlNozzle.Refresh();
        }
    }
}