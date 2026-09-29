using System;
using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.ZX2200.CalibSystem.Controls.Assistant;
using AKRS.ZX2200.CalibSystem.Models;

namespace AKRS.ZX2200.CalibSystem.Controls
{
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.ZX2200.BondSystem.Models;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    /// <summary>
    /// 单个标定
    /// </summary>
    public partial class FrmStartup : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmStartup()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Bond Camera标定测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void btnOnlyBondCamera_Click(object sender, EventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyBondCameraCalib);
        }

        /// <summary>
        /// UpLook Camera标定测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void btnOnlyUpLookCamera_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                "System2: \n"
                + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dialog == DialogResult.OK)
            {
                Task.Run(CalibrateTask.GetInstance().OnlyUpLookCameraCalib);
            }
        }

        /// <summary>
        /// Wafer Camera标定测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void btnOnlyWaferCamera_Click(object sender, EventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyWaferCameraCaib);
        }

        /// <summary>
        /// Dispense Camera标定测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void btnOnlyDispenseCamera_Click(object sender, EventArgs e)
        {
            Task.Run(CalibrateTask.GetInstance().OnlyDispenseCameraCalib);
        }

        /// <summary>
        /// Bond Camera标定测试
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        private void btnBDCalib_Click(object sender, EventArgs e)
        {
            if (!System2Domain.GetInstance().System2Controller.ChangeTouchDownAssistance())
            {
                return;
            }

            FrmTransportTeach frm = new FrmTransportTeach();
            frm.ShowDialog();
        }

        /// <summary>
        /// Bond到相机偏移标定测试
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondToCamera_Click(object sender, EventArgs e)
        {
            DialogResult dialog = AKRSXtraMessageBox.Show(
                "System2: \n"
                + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                "Prompt",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Information);
            if (dialog == DialogResult.OK)
            {
                Task.Run(CalibrateTask.GetInstance().OnlyBondToCameraOffsetCaib);
            }
        }

        /// <summary>
        /// Bond到相机偏移标定（上视标定片）
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnBondToCameraHighAccuracy_Click(object sender, EventArgs e)
        {
            FrmBondToCameraOffset frmBondToCameraOffset = new FrmBondToCameraOffset();
            frmBondToCameraOffset.Show();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FrmTestAccuary frmTestAccuary = new FrmTestAccuary();
            frmTestAccuary.Show();  
        }

        /// <summary>
        /// 激光测高标定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtLaserCalibration_Click(object sender, EventArgs e)
        {
            FrmAssistantDistanceMhAndBh frmAssistantDistanceMhAndBh = new FrmAssistantDistanceMhAndBh();
            frmAssistantDistanceMhAndBh.ShowDialog();
        }
    }
}