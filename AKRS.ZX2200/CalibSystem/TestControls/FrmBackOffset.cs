using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.CalibSystem.TestControls
{
    using AKRS.ZX2200.CalibSystem.Test;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    public partial class FrmBackOffset : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 次数
        /// </summary>
        private int cycles;

        /// <summary>
        /// 当前相机类型
        /// </summary>
        private CameraType curCameraType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cameraType">相机类型</param>
        public FrmBackOffset(CameraType cameraType)
        {
            this.InitializeComponent();
            this.curCameraType = cameraType;
        }

        /// <summary>
        /// 开始回0测试
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (this.txtCycles.Text == null)
            {
                AKRSXtraMessageBox.Show("Please input cycles！");
            }
            else
            {
                this.cycles = Convert.ToInt32(this.txtCycles.Text);
            }

            if (this.curCameraType == CameraType.BondCamera)
            {
                Task.Run(() => { TestAlg.GetInstance().TestBackOffset(this.cycles); });
            }
            else if (this.curCameraType == CameraType.DispenseCamera)
            {
                Task.Run(() => { TestAlg.GetInstance().TestBackOffsetDispense(this.cycles); });
            }

        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}