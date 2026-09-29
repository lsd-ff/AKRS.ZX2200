using System;
using System.Threading.Tasks;

namespace AKRS.ZX2200.CalibSystem.TestControls
{
    using AKRS.ZX2200.BondSystem.Controllers;
    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.CalibSystem.Models;
    using AKRS.ZX2200.CalibSystem.Services;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using SqlSugar.Extensions;
    using System.Windows.Forms;

    public partial class UcTestThreePointAlign : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// BondModule控制器
        /// </summary>
        private BondModuleController bondModuleController => System2Domain.GetInstance().BondModuleController;

        /// <summary>
        /// 标定控制器
        /// </summary>
        private CalibController calibController = new CalibController();

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTestThreePointAlign()
        {
            InitializeComponent();

            this.Disposed += (s, e) =>
                {
                    this.timer1.Tick -= timer1_Tick;
                    this.timer1.Dispose();
                };

            UcGuideMove ucGuideMove = new UcGuideMove("点胶标定");
            ucGuideMove.Dock = DockStyle.Fill;
            this.panelControl3.Controls.Add(ucGuideMove);
        }

        /// <summary>
        /// 计算邦头到相机偏移
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnCalcBondToCameraOffset_Click(object sender, EventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().StartBondToCamCalibTask1);
        }

        /// <summary>
        /// 计算晶圆标记点间距
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnCalcWaferDistance_Click(object sender, EventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().StartWaferTableCalibTask);
        }

        /// <summary>
        /// 晶圆左点移动到晶圆相机中心
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnMoveWaferToCenter_Click(object sender, EventArgs e)
        {
            this.calibController.MoveWaferTableToMachinePos(CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos);
        }

        /// <summary>
        /// Bond相机移动到晶圆右点中心
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnMoveBondToRightCenter_Click(object sender, EventArgs e)
        {
            this.bondModuleController.MoveSafeBondXYZ(CalibrateRunPara.GetInstance().WaferTableRightMarkBCVisionMachinePos);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void txtPickPosX_EditValueChanged(object sender, EventArgs e)
        {
            
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshData();
        }

        public void RefreshData()
        {
            if (CalibrateRunPara.GetInstance().WaferTableRightMarkBCVisionMachinePos != null)
            {
                this.txtBondRightMarkPosX.EditValue = CalibrateRunPara.GetInstance().WaferTableRightMarkBCVisionMachinePos.X;
                this.txtBondRightMarkPosY.EditValue = CalibrateRunPara.GetInstance().WaferTableRightMarkBCVisionMachinePos.Y;
            }

            if (CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset != null)
            {
                this.txtBondToBondCameraOffsetX.EditValue = CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.X;
                this.txtBondToBondCameraOffsetY.EditValue = CalibrateRunPara.GetInstance().BondRotateCenterToCamOffset.Y;
            }

            if (CalibrateRunPara.GetInstance().WaferTableMarksDistance != null)
            {
                this.txtWaferMarkDistanceX.EditValue = CalibrateRunPara.GetInstance().WaferTableMarksDistance.X;
                this.txtWaferMarkDistanceY.EditValue = CalibrateRunPara.GetInstance().WaferTableMarksDistance.Y;
            }

            if (CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos != null)
            {
                this.txtWaferLeftMarkCenterX.EditValue = CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos.X;
                this.txtWaferLeftMarkCenterY.EditValue = CalibrateRunPara.GetInstance().WaferTableLeftMarkWCVisionMachinePos.Y;
            }

            this.txtPickPosX.EditValue = this.txtBondRightMarkPosX.EditValue.ObjToDecimal() - this.txtWaferMarkDistanceX.EditValue.ObjToDecimal() + this.txtBondToBondCameraOffsetX.EditValue.ObjToDecimal();
            this.txtPickPosY.EditValue = this.txtBondRightMarkPosY.EditValue.ObjToDecimal() - this.txtWaferMarkDistanceY.EditValue.ObjToDecimal() + this.txtBondToBondCameraOffsetY.EditValue.ObjToDecimal();
        }
    }
}
