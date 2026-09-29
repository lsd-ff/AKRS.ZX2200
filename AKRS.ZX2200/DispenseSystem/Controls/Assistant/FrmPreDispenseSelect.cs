using DevExpress.XtraEditors;
using System;
using System.Windows.Forms;

namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.DispenseSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Controls.Ucmain.MainControls;
    using AKRS.ZX2200.Main.Machine.MachineSupport;

    /// <summary>
    /// 移动到预点胶的位置
    /// </summary>
    public partial class FrmPreDispenseSelect : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 移动到预点胶的位置
        /// </summary>
        public FrmPreDispenseSelect()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 行
        /// </summary>
        private int preDispenseRow;

        /// <summary>
        /// 列
        /// </summary>
        private int preDispenseColumn;

        /// <summary>
        /// 点位
        /// </summary>
        public AKRSPoint3D Point3D { get; set; }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 实时界面
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtVision_Click(object sender, EventArgs e)
        {
            UcMainSystem.VmVisionShow();
            
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                UcMainSystem.ChangeCameraVision(CameraEnum.BondCamera.GetDescription());
            }
            else
            {
                UcMainSystem.ChangeCameraVision(CameraEnum.DispenseCamera.GetDescription());
            }
        }

        /// <summary>
        /// 上升
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnSubstrateMoveUp_Click(object sender, EventArgs e)
        {
            this.preDispenseRow++;

            if (this.IsAllowToMove(this.preDispenseRow, this.preDispenseColumn))
            {
                this.MoveCameraToPreDispense(this.GetPreDispensePos(this.preDispenseRow, this.preDispenseColumn));
            }
            else
            {
                this.preDispenseRow--;
            }
        }

        /// <summary>
        /// 下降
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnSubstrateMoveDown_Click(object sender, EventArgs e)
        {
            this.preDispenseRow--;

            if (this.IsAllowToMove(this.preDispenseRow, this.preDispenseColumn))
            {
                this.MoveCameraToPreDispense(this.GetPreDispensePos(this.preDispenseRow, this.preDispenseColumn));
            }
            else
            {
                this.preDispenseRow++;
            }
        }


        /// <summary>
        /// 向右
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnSubstrateMoveRight_Click(object sender, EventArgs e)
        {
            this.preDispenseColumn++;

            if (this.IsAllowToMove(this.preDispenseRow, this.preDispenseColumn))
            {
                this.MoveCameraToPreDispense(this.GetPreDispensePos(this.preDispenseRow, this.preDispenseColumn));
            }
            else
            {
                this.preDispenseColumn--;
            }
        }

        /// <summary>
        /// 向左
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtnSubstrateMoveLeft_Click(object sender, EventArgs e)
        {
            this.preDispenseColumn--;

            if (this.IsAllowToMove(this.preDispenseRow, this.preDispenseColumn))
            {
                this.MoveCameraToPreDispense(this.GetPreDispensePos(this.preDispenseRow, this.preDispenseColumn));
            }
            else
            {
                this.preDispenseColumn++;
            }
        }

        /// <summary>
        /// 移动到预点胶的位置
        /// </summary>
        /// <param name="point3D">点位</param>
        private void MoveCameraToPreDispense(AKRSPoint3D point3D)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                System2Domain.GetInstance().S2DispenseController.VisionMoveToG0Pos(point3D);
            }
            else
            {
                MachineStateModel.GetInstance().CurrentMachineSystem = CurrentMachineSystemEnum.System1;
                AKRSPoint3D visionPos = System1Domain.GetInstance().DispenseController.GetG0VisionPos(point3D);

                System1Domain.GetInstance().DispenseController.MoveToG0Pos3D(visionPos);
            }

            this.Point3D = point3D;
        }

        /// <summary>
        /// 根据行列获取点位
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <returns>结果</returns>
        private AKRSPoint3D GetPreDispensePos(int row, int column)
        {
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                return System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.EpoxyArea[row, column];
            }
            else
            {
                return System1Domain.GetInstance().System1Program.PreDispensePlateProgram.EpoxyArea[row, column];
            }
        }

        /// <summary>
        /// 是否允许移动
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <returns>结果</returns>
        private bool IsAllowToMove(int row, int column)
        {
            double rowMax = 0;
            double columnMax = 0;
            if (MachineStateModel.GetInstance().CurrentMachineSystem == CurrentMachineSystemEnum.System2
                && MachineHardwareConfiguration.GetInstance().IsSystem2Dispense)
            {
                rowMax = System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.EpoxyArea.GetLength(0);
                columnMax = System2Domain.GetInstance().BondProgram.S2PreDispensePlateProgram.EpoxyArea.GetLength(1);
            }
            else
            {
                rowMax = System1Domain.GetInstance().System1Program.PreDispensePlateProgram.EpoxyArea.GetLength(0);
                columnMax = System1Domain.GetInstance().System1Program.PreDispensePlateProgram.EpoxyArea.GetLength(1);
            }

            if (row < 0 || column < 0 || rowMax < row || columnMax < column)
            {
                AKRSXtraMessageBox.Show("超出移动限制");

                return false;
            }

            return true;
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmPreDispenseSelect_Load(object sender, EventArgs e)
        {
            this.MoveCameraToPreDispense(this.GetPreDispensePos(this.preDispenseRow, this.preDispenseColumn));
        }
    }
}