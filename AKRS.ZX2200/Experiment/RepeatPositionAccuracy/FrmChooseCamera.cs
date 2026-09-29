using System;

namespace AKRS.ZX2200.ExperimentSystem.RepeatPositionAccuracy
{
    using AKRS.ZX2200.BondSystem.Models.Enums;
    using AKRS.ZX2200.CalibSystem.TestControls;

    public partial class FrmChooseCamera : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 当前选择相机类型
        /// </summary>
        public CameraTypeEnum CurCameraType;

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
            InitializeComponent();
            this.isStaticTest = isStatic;
        }

        /// <summary>
        /// 选择bond相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseBondCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraTypeEnum.BondCamera;
            if (this.isStaticTest == true)
            {
                FrmSetting frmBondCameraSetting = new FrmSetting(this.CurCameraType);
                frmBondCameraSetting.ShowDialog();
            }
        }

        /// <summary>
        /// 选择uplook相机
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnChooseUpLookCamera_Click(object sender, EventArgs e)
        {
            this.CurCameraType = CameraTypeEnum.UpLookCamera;
            if (this.isStaticTest == true)
            {
                FrmSetting frmUpLookCameraSetting = new FrmSetting(this.CurCameraType);
                frmUpLookCameraSetting.ShowDialog();
            }
        }

    }
}