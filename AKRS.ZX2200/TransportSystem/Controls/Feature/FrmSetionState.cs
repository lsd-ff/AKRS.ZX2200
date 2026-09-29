using System;
using System.Drawing;
using System.Windows.Forms;
using AKRS.ZX2200.TransportSystem.Models;
using AKRS.ZX2200.TransportSystem.Models.Programs;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.TransportSystem.Controls.Feature
{
    /// <summary>
    /// 流道状态显示
    /// </summary>
    public partial class FrmSetionState : XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmSetionState()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 扫描轨道的状态
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void TimerState_Tick(object sender, EventArgs e)
        {
            this.SetSectionState(TransportProgram.GetInstance().LoadingSubSectionProgram, this.MpLoadingTable);
            this.SetSectionState(TransportProgram.GetInstance().DispenseSubSectionProgram, this.MpDispenseTable);
            this.SetSectionState(TransportProgram.GetInstance().BondSubSectionProgram, this.MpWorkTable);
            this.SetSectionState(TransportProgram.GetInstance().WaitingUnloadSubSectionProgram, this.MpWaitingUnloadTable);
            this.SetSectionState(TransportProgram.GetInstance().UnloadingSubSectionProgram, this.MpUnloadingTable);
        }

        /// <summary>
        /// 设置轨道状态
        /// </summary>
        /// <param name="bsp">流道程式</param>
        /// <param name="mpb">流道状态显示控件</param>
        private void SetSectionState(BaseSubSectionProgram bsp, MarqueeProgressBarControl mpb)
        {
            if (bsp.SubSectionTransferState == SubSectionStateTransferEnum.Transfering)
            {
                mpb.Properties.MarqueeWidth = 100;
                mpb.Properties.Appearance.BackColor = Color.Gray;
                mpb.Properties.Stopped = false;
            }
            else
            {
                mpb.Properties.MarqueeWidth = 0;
                mpb.Properties.Paused = true;

                if (bsp.SubSectionTransferState == SubSectionStateTransferEnum.Alarm)
                {
                    mpb.Properties.Appearance.BackColor = Color.Red;
                }
                else if (bsp.SubSectionTransferState == SubSectionStateTransferEnum.Ready)
                {
                     mpb.Properties.Appearance.BackColor = Color.Gray;

                    if (bsp.SubSectionState == SubSectionStateEnum.HasMaterial)
                    {
                        mpb.Properties.Appearance.BackColor = Color.DarkGreen;
                    }
                    else
                    {
                        mpb.Properties.MarqueeWidth = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Load 事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmSetionState_Load(object sender, EventArgs e)
        {
            this.TimerState.Start();
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void FrmSetionState_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.TimerState.Stop();
            this.TimerState.Tick -= TimerState_Tick;
            this.TimerState.Dispose();
            this.TimerState = null;
        }

        /// <summary>
        /// 设置Load 有料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtLoadingHasMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().LoadingSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().LoadingSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
            TransportProgram.GetInstance().Save();  
        }

        /// <summary>
        /// 设置Load 无料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtLoadingNoMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().LoadingSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().LoadingSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置点胶台 有料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDispenseHasMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置点胶台 无料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtDispenseNoMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().DispenseSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置工作台 有料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBondHasMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().BondSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置工作台 无料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtBondNoMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().BondSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().BondSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置等待下料台 有料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtWaitUnloadHasMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置等待下料台 无料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtWaitUnloadNoMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().WaitingUnloadSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置下料台 有料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUnloadingHasMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().UnloadingSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().UnloadingSubSectionProgram.SubSectionState = SubSectionStateEnum.HasMaterial;
            TransportProgram.GetInstance().Save();
        }

        /// <summary>
        /// 设置下料台 无料
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtUnloadingNoMaterial_Click(object sender, EventArgs e)
        {
            TransportProgram.GetInstance().UnloadingSubSectionProgram.SubSectionTransferState =
                SubSectionStateTransferEnum.Ready;
            TransportProgram.GetInstance().UnloadingSubSectionProgram.SubSectionState = SubSectionStateEnum.NoMaterial;
            TransportProgram.GetInstance().Save();
        }
    }
}