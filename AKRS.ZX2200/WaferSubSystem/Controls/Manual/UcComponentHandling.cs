using AKRS.ZX2200.WaferSubSystem.Models;
using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;
using AKRS.ZX2200.WaferSubSystem.Modules;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.WaferSubSystem.Controls.Manual
{
    using AKRS.Galaxy2.Drive.Common;
    using AKRS.Galaxy2.Drive.Exceptions;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionControllerInterface;
    using AKRS.Galaxy2.Drive.MotionControllerDrive.MotionPara;
    using AKRS.Galaxy2.Infrastructure.ControlServices;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.WM;
    using AKRS.ZX2200.BondSystem.Modules;
    using AKRS.ZX2200.DispenseSystem.Modules;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Localization;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.WaferSubSystem.Controllers;
    using AKRS.ZX2200.WaferSubSystem.Models.Enums;
    using AKRS.ZX2200.WaferSubSystem.Services;
    using DevExpress.Charts.Native;
    using DevExpress.Utils.Extensions;
    using GTN;
    using System.Diagnostics;
    using System.Threading;
    using static GTN.mc;

    /// <summary>
    /// 手动
    /// </summary>
    public partial class UcComponentHandling : DevExpress.XtraEditors.XtraUserControl, ILanguage
    {
        /// <summary>
        /// WaferSubModule
        /// </summary>
        private WaferSubModule WaferSubModule => WaferSubModule.GetInstance();

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private WaferTableModule WaferTableModule => WaferSubModule.GetInstance().WaferTable;

        /// <summary>
        /// WaferTableModule
        /// </summary>
        private MagazineBoxModule MagazineBoxModule => WaferSubModule.GetInstance().MagazineBox;

        /// <summary>
        /// MagazineBoxDevicePara
        /// </summary>
        private MagazineBoxDevicePara MagazineBoxDevicePara => WaferSubDevicePara.GetInstance().MagazineDevicePara;

        /// <summary>
        /// MagazineBox当前属于哪一层（当前层号）
        /// </summary>
        private int CurrentLayerNo { get; set; } = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcComponentHandling()
        {
            this.InitializeComponent();
            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= this.Timer1_Tick;
                    this.timer1.Dispose();
                };

            this.InitControl();

            FormLocalizer.LocalizeForm(this);
        }

        /// <summary>
        /// InitControl
        /// </summary>
        private void InitControl()
        {
            this.BtnWaferChangeTest.Appearance.BackColor = Static.IsWaferChangeTestSignal ? Color.Yellow : default;
            if (!MachineStateModel.GetInstance().IsOffLineWork)
            {
                this.BtnMovePusherOut.Appearance.BackColor = WaferSubController.GetInstance().MagazineController.GetWaferPushCylinder() ? Color.Yellow : default;
                this.BtnCloseGripper.Appearance.BackColor = WaferSubController.GetInstance().WaferTableController.GetWaferClampCylinder() ? Color.Yellow : default;
                this.BtnBlockCyc.Appearance.BackColor = WaferSubController.GetInstance().WaferTableController.GetBlockCylinder() ? Color.Yellow : default;

                if (WaferSubDevicePara.GetInstance().EjectDevicePara.IsUseEjectFixedCylinder)
                {
                    this.BtnFixedCyc.Appearance.BackColor = WaferSubController.GetInstance().EjectController.GetFixedCylinder() ? Color.Yellow : default;
                }
            }
        }

        /// <summary>
        /// BtnMoveWaferMagazineToSlot
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveWaferMagazineToSlot_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                if (this.CeIsActive.Checked)
                {
                    int magazineBoxLayer = (int)this.SpMagazineBoxLayer.Value;
                    if (magazineBoxLayer > MagazineBoxDevicePara.Position.Length || magazineBoxLayer < 1)
                    {
                        throw new Exception($"请输入 1-{MagazineBoxDevicePara.Position.Length} 的整数.");
                    }

                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSlotPosition(magazineBoxLayer - 1);
                    this.CurrentLayerNo = magazineBoxLayer;
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSlotPosition(0);
                    this.CurrentLayerNo = 1;
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

        /// <summary>
        /// BtnMoveToNextSlot
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToNextSlot_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CurrentLayerNo++;
                if (this.CurrentLayerNo > MagazineBoxDevicePara.Position.Length)
                {
                    AKRSXtraMessageBox.Show("这是MagazineBox的最后一层!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.CurrentLayerNo = MagazineBoxDevicePara.Position.Length;                    
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSlotPosition(this.CurrentLayerNo - 1);
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

        /// <summary>
        /// BtnMoveToPreviousSlot
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToPreviousSlot_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                this.CurrentLayerNo--;
                if (this.CurrentLayerNo <= 0)
                {
                    AKRSXtraMessageBox.Show("这是MagazineBox的第一层!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.CurrentLayerNo = 1;                    
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSlotPosition(this.CurrentLayerNo - 1);
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

        /// <summary>
        /// BtnPlaceWaferInSlot
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnPlaceWaferInSlot_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                if (this.CeIsActive.Checked)
                {
                    WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.ActionWaferClampPlaceWaferInSlot();
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

        /// <summary>
        /// BtRemoveWaferFromSlot
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemoveWaferFromSlot_Click(object sender, EventArgs e)
        {                        
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;
                Task.Run(() =>
                {
                    this.Invoke(new Action(() => { btn.Enabled = false; }));
                    if (this.CeIsActive.Checked)
                    {
                        int i = (int)this.SpMagazineBoxLayer.Value - 1;
                        if (MagazineBoxDevicePara.CurrentLayerNo != i + 1)
                        {
                            WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                            WaferSubController.GetInstance().WaferTableController.RemoveWaferFromSlot(i);
                        }
                    }
                    else
                    {
                        WaferSubController.GetInstance().WaferTableController.ActionWaferClampRemoveWaferFromSlot();
                    }

                    this.Invoke(new Action(() => { btn.Enabled = true; }));
                });               
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
        /// BtnSlotScan
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnSlotScan_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().MagazineController.SlotScan();
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
        /// BtnMovePusherOut
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMovePusherOut_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().MagazineController.ResetWaferPushCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.SetWaferPushCylinder();
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
        /// BtnCloseGripper
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCloseGripper_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.ResetWaferClampCylinder();                    
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.SetWaferClampCylinder();
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
        /// BtnBlockCyc_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
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

        /// <summary>
        /// BtnFixedCyc_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnFixedCyc_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.SetFixedCylinder();
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
        /// BtnStaticWaffleVacuum_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnStaticWaffleVacuum_Click(object sender, EventArgs e)
        {            
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseStaticWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenStaticWaffleVacuum();
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
        /// 华夫盒真空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnWaffleVacuum_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().WaferTableController.CloseWaffleVacuum();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().WaferTableController.OpenWaffleVacuum();
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
        /// BtnInitialization
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnInitialization_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSystemDomain.GetInstance().Initialization();
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
        /// 雨刷器
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.LcCurrentLayerNo.Text = $"{this.CurrentLayerNo}";
            this.LcLayerNo.Text = $"{MagazineBoxDevicePara.CurrentLayerNo}";

            // 按钮防呆
            {
                // test/display
                this.BtnMoveToUpDownMarkPosition.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnMoveToExpandDownPosition.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnMoveToAutoChangePosition.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnMoveToMagazineSafePosition.Enabled = !Machine.GetInstance().IsWorking();

                // wafer change
                this.BtnMoveWaferMagazineToSlot.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnMoveToNextSlot.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnMoveToPreviousSlot.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnPlaceWaferInSlot.Enabled = !Machine.GetInstance().IsWorking() && WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift;
                this.BtRemoveWaferFromSlot.Enabled = !Machine.GetInstance().IsWorking() && WaferSubDevicePara.GetInstance().MagazineDevicePara.IsUseMagazineLift;
                this.BtnSlotScan.Enabled = !Machine.GetInstance().IsWorking();

                // signals
                this.BtnMovePusherOut.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnCloseGripper.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnBlockCyc.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnFixedCyc.Enabled = !Machine.GetInstance().IsWorking();
                this.BtnInitialization.Enabled = !Machine.GetInstance().IsWorking();
            }
        }

        /// <summary>
        /// BtnMoveToUpDownMarkPosition_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToUpDownMarkPosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
                    WaferSubController.GetInstance().EjectController.MoveEjectionTableToSafePosition();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().EjectController.ResetFixedCylinder();
                    WaferSubController.GetInstance().EjectController.MoveEjectionTableToMarkPosition();
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
        /// BtnMoveToExpandDownPosition_Click
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnMoveToExpandDownPosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.MoveExpandToDownPosition();
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
        /// BtnWaferChangeTest_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private async void BtnWaferChangeTest_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    Static.IsWaferChangeTestSignal = false;
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    btn.Appearance.BackColor = Color.Yellow;
                    Static.IsWaferChangeTestSignal = true;
                    await Task.Run(
                        () =>
                            {
                                while (Static.IsWaferChangeTestSignal)
                                {
                                    Retry:
                                    for (int i = 0; i < WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray.Length; i++)
                                    {
                                        WaferSubController.GetInstance().WaferTableController.PlaceWaferInSlot();
                                        if (Static.IsWaferChangeTestSignal)
                                        {
                                            if (WaferSystemProgram.GetInstance().MagazineAllocationsProgram.CurrentAllocationsConfig.TabletArray[i].TabletType != TabletTypeEnum.Null)
                                            {
                                                WaferSubController.GetInstance().WaferTableController.RemoveWaferFromSlot(i, true);
                                            }
                                        }
                                        else
                                        {
                                            return;
                                        }

                                        Thread.Sleep(100);
                                    }

                                    goto Retry;
                                }
                            });

                    // 料夹去安全位
                    WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();

                    // 料架去安全位
                    WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
                }
            }
            catch (Exception exception)
            {
                AKRSXtraMessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btn.Appearance.BackColor = default;
                Static.IsWaferChangeTestSignal = false;
            }
        }

        /// <summary>
        /// BtnMoveToAutoChangePosition_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnMoveToAutoChangePosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.MoveWaferTableToAutoChangePosition();
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
        /// BtnMoveToMagazineSafePosition_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnMoveToMagazineSafePosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().MagazineController.MoveMagazineToSafePosition();
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
        /// BtnMoveToWaferClampSafePosition_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnMoveToWaferClampSafePosition_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                btn.Appearance.BackColor = Color.Yellow;

                WaferSubController.GetInstance().WaferTableController.MoveWaferClampToSafePosition();
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
        /// 提篮夹紧气缸动作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnMagazineFixedCyc_Click(object sender, EventArgs e)
        {
            SimpleButton btn = sender as SimpleButton;
            try
            {
                btn.Enabled = false;
                if (btn.Appearance.BackColor == Color.Yellow)
                {
                    WaferSubController.GetInstance().MagazineController.ResetMagazineFixedCylinder();
                    btn.Appearance.BackColor = default;
                }
                else
                {
                    WaferSubController.GetInstance().MagazineController.SetMagazineFixedCylinder();
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

        public void GetUI()
        {
            if (this.Parent == null)
            {
                this.Text = LocalizationManager.GetUI(this.GetType().Name, "Title");
            }
            else
            {
                this.Parent.Parent.Text = LocalizationManager.GetUI(this.GetType().Name, "Title");
            }
        }
    }    
}
