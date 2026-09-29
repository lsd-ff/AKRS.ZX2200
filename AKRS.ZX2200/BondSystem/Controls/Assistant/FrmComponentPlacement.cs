using AKRS.Galaxy2.Infrastructure;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.Infrastructure.Enums;
using AKRS.Galaxy2.Infrastructure.Helper;
using AKRS.Galaxy2.Machine.Models;
using AKRS.Galaxy2.PR.Models.CommonModels;
using AKRS.Galaxy2.PR.Models.Entities;
using AKRS.Galaxy2.PR.Models.MatchResults;
using AKRS.Galaxy2.PR.Resipository;
using AKRS.ZX2200.BondSystem.Controllers;
using AKRS.ZX2200.BondSystem.Models;
using AKRS.ZX2200.BondSystem.Models.Enums;
using AKRS.ZX2200.BondSystem.Modules;
using AKRS.ZX2200.Infrastructure.Controls.Common;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.Infrastructure.Models.Enums;
using AKRS.ZX2200.Main.Controls;
using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
using AKRS.ZX2200.Main.Machine.MachineSupport;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Assistant;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.Entities;
using AKRS.ZX2200.WaferSubSystem.Models.Entities.SearchChip;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWafer;
using AKRS.ZX2200.WaferSubSystem.Models.Repositories.CarrierWithWaffle;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    /// <summary>
    /// 芯片放置示教窗体
    /// </summary>
    public partial class FrmComponentPlacement : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        ///  构造函数
        /// </summary>
        /// <param name="baseCarrierConfig">传入的芯片对象</param>
        public FrmComponentPlacement(BaseCarrierConfig baseCarrierConfig)
        {
            this.component = baseCarrierConfig;
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 方向盘
        /// </summary>
        private UcGuideMove ucGuideMove;

        /// <summary>
        /// 点击Start传进来的芯片对象
        /// </summary>
        private BaseCarrierConfig component;

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
        /// 取片线程
        /// </summary>
        private Task placementTask;

        /// <summary>
        /// 晶圆检查是否通过
        /// </summary>
        private bool isWaferCheckSucceed = false;

        /// <summary>
        /// 取片线程
        /// </summary>
        private Task pickupTask;

        /// <summary>
        /// 方向盘
        /// </summary>
        private FrmBear frmBear;

        /// <summary>
        /// 是否初始化
        /// </summary>
        private bool isInit = false;

        /// <summary>
        /// 初始化
        /// </summary>
        public void InitControl()
        {
            #region 控件赋值

            this.SpSlowTravelBeforeBondingSpeed.Value = (decimal)this.component.SlowTravelSpeedBeforeBonding;
            this.SpBondingDistance.Value = (decimal)this.component.BondingDistance;
            this.SpBondingForce.Value = (decimal)this.component.BondingForce;
            this.SpSlowTravelAfterBondingSpeed.Value = (decimal)this.component.SlowTravelSpeedAfterBonding;
            this.SpTravelAfterBondHeight.Value = (decimal)this.component.SlowTravelDistanceAfterBonding;
            this.SpTravelBeforeBondHeight.Value = (decimal)this.component.SlowTravelDistanceBeforeBonding;
            this.SpBondingDelay.Value = (decimal)this.component.PlacementDelay;
            this.SpBlowDelay.Value = (decimal)this.component.BondingBlowDelay;
            this.SpVacuumOffDelay.Value = (decimal)this.component.VacuumOffDelay;
            this.CmbBondingForceMode.Properties.Items.Clear();
            List<string> list = Enum.GetValues(typeof(ForceModeEnum)).Cast<ForceModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbBondingForceMode.Properties.Items.AddRange(list);
            this.CmbBondingForceMode.Text = this.component.BondingForceMode.ToString();

            this.CmbTravelBeforeBondingSpeed.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbTravelBeforeBondingSpeed.Properties.Items.AddRange(list);
            this.CmbTravelBeforeBondingSpeed.SelectedIndex = 0;

            this.CmbTravelAfterBondingSpeed.Properties.Items.Clear();
            list = Enum.GetValues(typeof(SpeedModeEnum)).Cast<SpeedModeEnum>().Select(x => x.ToString()).ToList();
            this.CmbTravelAfterBondingSpeed.Properties.Items.AddRange(list);
            this.CmbTravelAfterBondingSpeed.SelectedIndex = 0;

            this.ChkActivateSlowTravelBeforeBonding.Checked = this.component.IsActivateSlowTravelBeforeBonding;
            this.ChkActivateSlowTravelAfterBonding.Checked = this.component.IsActivateSlowTravelAfterBonding;

            this.ChkDipOnBondPosition.Checked = this.component.IsDipOnTU;
          this.SpDipOnTUAfterBondingNum.Value = this.component.DipOnTUAfterBondingNum;

            // 判断芯片类型
            if (this.component is CarrierWithWaferConfig carrierWithWafer)
            {
                // 晶圆芯片属性
            }
            else if (this.component is CarrierWithWaffleConfig carrierWithWaffle)
            {
                // 华夫盒芯片
            }

            // 焊点名称集合
            List<string> bondPositionNameList = new List<string>();

            // 获取数据源
            foreach (var item in ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList)
            {
                bondPositionNameList.Add(item.Name);
            }

            this.CmbBondPositions.Properties.Items.Clear();
            this.CmbBondPositions.Properties.Items.AddRange(bondPositionNameList);

            #endregion

            #region 界面防呆

            this.SpSubstrate.Properties.MinValue = 1;
            this.SpModule.Properties.MinValue = 1;
            this.SpSubstrate.Properties.MaxValue = ProductConfiguration.GetInstance().SubstrateConfig.Count;
            this.SpModule.Properties.MaxValue = ProductConfiguration.GetInstance().ModuleConfig.Count;

            this.LbComponentName.Text = this.component.Name;
            this.LbComponentName.Enabled = false;

            if (this.CmbBondingForceMode.Text == ForceModeEnum.Distance.ToString())
            {
                this.SpBondingDistance.Enabled = true;
                this.SpBondingForce.Enabled = false;
            }
            else
            {
                this.SpBondingDistance.Enabled = false;
                this.SpBondingForce.Enabled = true;
            }

            if (this.ChkActivateSlowTravelBeforeBonding.Checked)
            {
                this.CmbTravelBeforeBondingSpeed.Enabled = true;
                this.SpSlowTravelBeforeBondingSpeed.Enabled = true;
            }
            else
            {
                this.CmbTravelBeforeBondingSpeed.Enabled = false;
                this.SpSlowTravelBeforeBondingSpeed.Enabled = false;
            }

            if (this.ChkActivateSlowTravelAfterBonding.Checked)
            {
                this.CmbTravelAfterBondingSpeed.Enabled = true;
                this.SpSlowTravelAfterBondingSpeed.Enabled = true;
            }
            else
            {
                this.CmbTravelAfterBondingSpeed.Enabled = false;
                this.SpSlowTravelAfterBondingSpeed.Enabled = false;
            }

            this.gPDipOnTU.Enabled = MachineHardwareConfiguration.GetInstance().IsSlideFluxerConfigured;

            #endregion

            this.Size = new Size(1400, 800);
            isInit = true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.component.BondingForceMode = (ForceModeEnum)Enum.Parse(
          typeof(ForceModeEnum),
          this.CmbBondingForceMode.Text);

            this.component.SlowTravelSpeedBeforeBonding = (double)this.SpSlowTravelBeforeBondingSpeed.Value;
            this.component.BondingDistance = (double)this.SpBondingDistance.Value;
            this.component.BondingForce = (double)this.SpBondingForce.Value;
            this.component.SlowTravelSpeedAfterBonding = (double)this.SpSlowTravelAfterBondingSpeed.Value;
            this.component.PlacementDelay = (int)this.SpBondingDelay.Value;
            this.component.BondingBlowDelay = (int)this.SpBlowDelay.Value;
            this.component.VacuumOffDelay = (int)this.SpVacuumOffDelay.Value;

            this.component.SlowTravelDistanceBeforeBonding = (double)this.SpTravelBeforeBondHeight.Value;
            this.component.SlowTravelDistanceAfterBonding = (double)this.SpTravelAfterBondHeight.Value;

            this.component.IsActivateSlowTravelBeforeBonding = this.ChkActivateSlowTravelBeforeBonding.Checked;
            this.component.IsActivateSlowTravelAfterBonding = this.ChkActivateSlowTravelAfterBonding.Checked;

            this.component.IsDipOnTU = this.ChkDipOnBondPosition.Checked;
            this.component.DipOnTUAfterBondingNum = (int)this.SpDipOnTUAfterBondingNum.Value;

            // 判断芯片类型
            if (this.component is CarrierWithWaferConfig carrierWithWafer)
            {
                // 晶圆芯片属性
            }
            else if (this.component is CarrierWithWaffleConfig carrierWithWaffle)
            {
                // 华夫盒芯片
            }

            CarrierConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbBondingForceMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.CmbBondingForceMode.SelectedText == ForceModeEnum.Distance.ToString())
            {
                this.SpBondingDistance.Enabled = true;
                this.SpBondingForce.Enabled = false;
            }
            else
            {
                this.SpBondingDistance.Enabled = false;
                this.SpBondingForce.Enabled = true;
            }
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTravelBeforeBondingSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSpinEditValueByCmb(this.CmbTravelBeforeBondingSpeed, this.SpSlowTravelBeforeBondingSpeed);
        }

        /// <summary>
        /// 下拉框选项改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void CmbTravelAfterBondingSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeSpinEditValueByCmb(this.CmbTravelAfterBondingSpeed, this.SpSlowTravelAfterBondingSpeed);
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActivateSlowTravelBeforeBonding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkActivateSlowTravelBeforeBonding.Checked)
            {
                this.CmbTravelBeforeBondingSpeed.Enabled = true;
                this.SpSlowTravelBeforeBondingSpeed.Enabled = true;
            }
            else
            {
                this.CmbTravelBeforeBondingSpeed.Enabled = false;
                this.SpSlowTravelBeforeBondingSpeed.Enabled = false;
            }

            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 勾选状态改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkActivateSlowTravelAfterBonding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ChkActivateSlowTravelAfterBonding.Checked)
            {
                this.CmbTravelAfterBondingSpeed.Enabled = true;
                this.SpSlowTravelAfterBondingSpeed.Enabled = true;
            }
            else
            {
                this.CmbTravelAfterBondingSpeed.Enabled = false;
                this.SpSlowTravelAfterBondingSpeed.Enabled = false;
            }

            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtOK_Click(object sender, EventArgs e)
        {
            // 保存
            this.Save();

            this.component.ComponentPlacement.State = AssistantStateEnum.Able;
            CarrierConfigRepository.GetInstance().Save();
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
                this.Close();
            }
        }

        /// <summary>
        /// 模拟贴片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPlacementTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CmbBondPositions.Text))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先选择焊点!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // 保存参数
            this.Save();

            // 判断固晶类型
            BondTypeEnum bondType = this.ChkDipOnBondPosition.Checked ? BondTypeEnum.DipFluxOnTU : BondTypeEnum.BondOnTU;

            SimpleButton btn = sender as SimpleButton;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            if (this.system2Controller.ChangeNozzleAssistance(this.component.NozzleName) == false)
            {
                return;
            }

            // 防止线程多次启动
            if (this.placementTask == null || this.placementTask.IsCompleted == true)
            {
                this.placementTask = Task.Run(
                    () =>
                        {
                            CommonUtil.SetCurrentThreadName("贴片测试线程");
                            this.PlacementTest(bondType);
                        });
            }
        }

        /// <summary>
        /// 贴片测试
        /// </summary>
        /// <param name="bondType">贴片类型</param>
        private void PlacementTest(BondTypeEnum bondType)
        {
            try
            {
                // 点胶印测试
                ExcuteResult ret = System2Domain.GetInstance().System2Controller.PlacementTest(
                    component,
                    (int)this.SpSubstrate.Value,
                    (int)this.SpModule.Value,
                    this.CmbBondPositions.Text,
                    bondType);

                if (ret != ExcuteResult.Success)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"固晶失败!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                this.BeginInvoke(
                    new Action(
                        () =>
                            {
                                if (this.frmBear == null || this.frmBear.IsDisposed == true)
                                {
                                    this.frmBear = new FrmBear();

                                    // 弹出方向盘
                                    ucGuideMove = new UcGuideMove("FrmComponentPlacement") { Dock = DockStyle.Fill };
                                    this.frmBear.StartPosition = FormStartPosition.Manual;

                                    this.frmBear.Location = new Point(70, 190);
                                    frmBear.Size = ucGuideMove.Size;
                                    frmBear.Controls.Add(ucGuideMove);
                                    frmBear.Show();
                                }
                                else
                                {
                                    frmBear.Show();
                                }

                                // 弹出视觉窗体
                                UcMainSystem.VmVisionShow();
                                UcMainSystem.ChangeCameraVision(CameraEnum.BondCamera.GetDescription());
                            }));
            }
            catch (Exception ex)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"固晶失败:" + ex.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                            {
                                BtnPlacementTest.Appearance.BackColor = default;
                                BtnPlacementTest.Enabled = true;
                            }));
            }
        }

        /// <summary>
        /// 取片后点胶印
        /// </summary>
        /// <param name="bondType">贴片类型</param>
        private void PickAndDip(BondTypeEnum bondType)
        {
            try
            {
                bool res = this.system2Controller.PickupFromFlipTable(this.component);
                if (!res) 
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                      $"取片失败!",
                      "Warn",
                      MessageBoxButtons.OK,
                      MessageBoxIcon.Warning);

                    return;
                }

                // 点胶印测试
                ExcuteResult ret = System2Domain.GetInstance().System2Controller.PlacementTest(
                    component,
                    (int)this.SpSubstrate.Value,
                    (int)this.SpModule.Value,
                    this.CmbBondPositions.Text,
                    bondType);

                if (ret != ExcuteResult.Success)
                {
                    DialogResult dialog = AKRSXtraMessageBox.Show(
                        $"固晶失败!",
                        "Warn",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            if (this.frmBear == null || this.frmBear.IsDisposed == true)
                            {
                                this.frmBear = new FrmBear();

                                // 弹出方向盘
                                ucGuideMove = new UcGuideMove("FrmComponentPlacement") { Dock = DockStyle.Fill };
                                this.frmBear.StartPosition = FormStartPosition.Manual;

                                this.frmBear.Location = new Point(70, 190);
                                frmBear.Size = ucGuideMove.Size;
                                frmBear.Controls.Add(ucGuideMove);
                                frmBear.Show();
                            }
                            else
                            {
                                frmBear.Show();
                            }


                            // 弹出视觉窗体
                            UcMainSystem.VmVisionShow();
                            UcMainSystem.ChangeCameraVision(CameraEnum.BondCamera.GetDescription());
                        }));
            }
            catch (Exception ex)
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"固晶失败:" + ex.Message,
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                this.BeginInvoke(
                    new Action(
                        () =>
                        {
                            BtnPlacementTest.Appearance.BackColor = default;
                            BtnPlacementTest.Enabled = true;
                        }));
            }
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmComponentPlacement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.placementTask != null && this.placementTask.IsCompleted == false) 
            {
                e.Cancel = true;
            }

            // 退出单步
            Machine.GetInstance().Stop();

            this.ucGuideMove?.Dispose();
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
        private void FrmComponentPlacement_KeyDown(object sender, KeyEventArgs e)
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
        ///  取片
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPickComponent_Click(object sender, EventArgs e)
        {
            // 保存对象属性
            this.Save();

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

            // 芯片名称传给晶圆台
            WaferSystemDomain.GetInstance().WaferSubSystemTask.SetCurrentNeedChipName(component.Name);

            // 给晶圆台发要料信号
            SignalPool.GetInstance().IsBondNeedChipSignal.Set();

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

                // 去安全位
                this.bondModuleController.MoveToSafePos();
            }
            catch (Exception ex)
            {
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
                            BtnPickComponent.Appearance.BackColor = default;
                            BtnPickComponent.Enabled = true;
                        }));
            }
        }

        /// <summary>
        ///  设置焊点偏移
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnBondPositionOffset_Click(object sender, EventArgs e)
        {
            FrmEntityOffsetChange frmEntityOffsetChange = new FrmEntityOffsetChange();
            frmEntityOffsetChange.ShowDialog();

            frmEntityOffsetChange.Dispose();
        }

        /// <summary>
        /// 编辑PR
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditPR_Click(object sender, EventArgs e)
        {
            TransportUnit transportUnit =
TransportDomain.GetInstance().TransportProgram.BondSubSectionProgram.TransportUnit;

            Substrate substrate = transportUnit.Substrates.Find(it => it.Index == (int)this.SpSubstrate.Value);

            Module module = substrate.Modules.Find(it => it.Index == (int)this.SpModule.Value);

            BondPosition bondPosition =
      module.BondPositions.Find(it => it.Name == (string)this.CmbBondPositions.SelectedItem);

            if (bondPosition == null)
            {
                AKRSXtraMessageBox.Show("请先选择焊点！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                return;           
            }

            this.component.DipPRName = this.component.Name + "DipFluxMatch";
            TUAssistantHelper.EditPrInSystem2(this.component.DipPRName);

            // 焊点位置
            AKRSPoint3D bondPositionInG0 = bondPosition.CoordinateSystem.SelfPosToG0(new AKRSPoint3D());

            // 保存拍照位（相对焊点的偏移）
            // todo:这里是不是有问题？
            this.component.DipVisionPos = this.bondModuleController.GetG0RealPosition() - bondPositionInG0;

            // 执行定位
            PREntity pREntity = (PREntity)VisionEntityRepository
                .GetInstance().Find(this.component.DipPRName);

            pREntity.DoWork();

            // 设置模板归属
            pREntity.Alg.AlgBeLong = AlgBeLongEnum.Substrate;

            AKRSPoint3D point3D = new AKRSPoint3D();

            // 计算胶心
            if (pREntity.AlgResults.Count > 0)
            {
                this.component.DipOnTUFluxArea =
                    new List<double>();
                this.component.DipOnTUFluxCenterX =
                    new List<double>();
                this.component.DipOnTUFluxCenterY =
                    new List<double>();
                this.component.DipOnTUFluxNum =
                pREntity.AlgResults.Count;

                for (int i = 0; i < pREntity.AlgResults.Count; i++)
                {
                    BlobResult blobResult = (BlobResult)pREntity.AlgResults[i];
                    MatchResult matchResult = new MatchResult(
                        blobResult.CenterX,
                        blobResult.CenterY,
                        blobResult.Area);

                    AKRSPoint3D point = System2Module.GetInstance()
                        .BondModule.BondCameraCoordinateSystem
                        .ForwardConvertCoordinate(
                            new AKRSPoint3D(
                                matchResult.CenterX,
                                matchResult.CenterY,
                                0));

                    this.component.DipOnTUFluxArea.Add(
                        blobResult.Area);
                    this.component.DipOnTUFluxCenterX.Add(
                        point.X);
                    this.component.DipOnTUFluxCenterY.Add(
                        point.Y);
                    point3D = point3D + point;
                }
            }
            else
            {
                AKRSXtraMessageBox.Show("视觉定位失败，请重新编辑模板！");
                return;
            }

            CarrierConfigRepository.GetInstance().Save();
        }

        /// <summary>
        /// 勾选框改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void ChkDipOnBondPosition_CheckedChanged(object sender, EventArgs e)
        {
            this.BtnEditPR.Enabled = this.SpDipOnTUAfterBondingNum.Enabled = this.ChkDipOnBondPosition.Checked;
        }

        /// <summary>
        ///  编辑胶印检测参数
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnEditFluxPara_Click(object sender, EventArgs e)
        {
            FrmFluxDetectionPara frmFluxDetectionPara = new FrmFluxDetectionPara(this.component);
            frmFluxDetectionPara.ShowDialog();
            frmFluxDetectionPara.Dispose();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpTravelBeforeBondHeight_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSlowTravelBeforeBondingSpeed_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBondingDistance_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpTravelAfterBondHeight_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpSlowTravelAfterBondingSpeed_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBondingDelay_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpVacuumOffDelay_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            if (this.SpVacuumOffDelay.Value > this.SpBondingDelay.Value)
            {
                AKRSXtraMessageBox.Show("关闭真空延时不能超过固精延时！", "Warn");
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 数值改变事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void SpBlowDelay_EditValueChanged(object sender, EventArgs e)
        {
            if (this.isInit == false)
            {
                return;
            }

            this.Save();
            this.tablePanel1.Refresh();
        }

        /// <summary>
        /// 取片后点胶印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPickAndDipOnTU_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.CmbBondPositions.Text))
            {
                DialogResult dialog = AKRSXtraMessageBox.Show(
                    $"请先选择焊点!",
                    "Warn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // 保存参数
            this.Save();

            // 判断固晶类型
            BondTypeEnum bondType = BondTypeEnum.DipFluxOnTU;

            SimpleButton btn = sender as SimpleButton;
            btn.Enabled = false;
            btn.Appearance.BackColor = Color.Yellow;

            if (this.system2Controller.ChangeNozzleAssistance(this.component.NozzleName) == false)
            {
                return;
            }

            // 防止线程多次启动
            if (this.placementTask == null || this.placementTask.IsCompleted == true)
            {
                this.placementTask = Task.Run(
                    () =>
                    {
                        CommonUtil.SetCurrentThreadName("贴片测试线程");
                        this.PickAndDip(bondType);
                    });
            }
        }
    }
}