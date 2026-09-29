namespace AKRS.ZX2200.Experiment.Test
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.CalibSystem.TestControls;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using DevExpress.XtraEditors;

    public partial class FrmChooseCamera : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 当前选择相机类型
        /// </summary>
        public CameraType CurCameraType;

        /// <summary>
        /// 是否为静态实验
        /// </summary>
        public bool isStaticTest = true;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isStatic">是否是静态实验</param>
        public FrmChooseCamera(bool isStatic)
        {
            this.InitializeComponent();
            this.isStaticTest = isStatic;
        }

        /// <summary>
        /// 选择bond相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseBondCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraType.BondCamera;
            if (this.isStaticTest == true)
            {
                FrmSetting frmBondCameraSetting = new FrmSetting(this.CurCameraType);
                frmBondCameraSetting.ShowDialog();
            }
            else
            {
                Task.Run(TestAlg.GetInstance().TestPixelRatioAccuracyBond);
            }
        }

        /// <summary>
        /// 选择uplook相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseUpLookCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraType.UpLookCamera;
            if (this.isStaticTest == true)
            {
                FrmSetting frmUpLookCameraSetting = new FrmSetting(this.CurCameraType);
                frmUpLookCameraSetting.ShowDialog();
            }
            else
            {
                DialogResult dialog2 = AKRSXtraMessageBox.Show(
                    "System2: Warning: \n"
                    + "Remove touchdown tool and Attach BMC tool holder to the bonding head.\r\n",
                    "Alarm",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error);
                if (dialog2 == DialogResult.OK)
                {
                    Task.Run(TestAlg.GetInstance().TestPixelRatioAccuracyUpLook);
                }
            }
        }

        /// <summary>
        /// 选择Dispense相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseDispenseCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraType.DispenseCamera;
            if (this.isStaticTest == true)
            {
            }
            else
            {
                Task.Run(TestAlg.GetInstance().TestPixelRatioAccuracyDispense);
            }
        }

        /// <summary>
        /// 选择Wafer相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseWaferCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraType.WaferCamera;
            if (this.isStaticTest == true)
            {
            }
            else
            {
                Task.Run(TestAlg.GetInstance().TestPixelRatioAccuracyWafer);
            }
        }
    }
}