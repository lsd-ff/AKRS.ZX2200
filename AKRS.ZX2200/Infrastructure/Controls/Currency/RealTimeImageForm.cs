namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Component.Simple.ImageView;
    using AKRS.Galaxy2.Infrastructure.CommonModel;

    /// <summary>
    /// 实时影像
    /// </summary>
    public partial class RealTimeImageForm : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 类单例饿汉模式，只能创建一个
        /// </summary>
        private static RealTimeImageForm realTimeImageForm = new RealTimeImageForm();

        /// <summary>
        /// 显示
        /// </summary>
        public static void ShowForm()
        {
            if (realTimeImageForm == null || realTimeImageForm.IsDisposed)
            {
                realTimeImageForm = new RealTimeImageForm();
            }

            realTimeImageForm.WindowState = FormWindowState.Normal;
            realTimeImageForm.Show(); 
            realTimeImageForm.BringToFront();
        }

        /// <summary>
        /// 模态显示
        /// </summary>
        public static DialogResult ShowDialogForm()
        {
            return realTimeImageForm.ShowDialog();
        }

        /// <summary>
        /// 实时影像
        /// </summary>
        private RealTimeImageForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视觉窗体
        /// </summary>
        private UcImageView ucImageView;

        /// <summary>
        /// window窗体使能事件
        /// </summary>
        /// <param name="hWnd">事件</param>
        /// <param name="bEnable">参数</param>
        /// <returns>结果</returns>
        [DllImport("User32.dll")]
        public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RealTimeImageForm_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.ucImageView = new UcImageView();
            this.ucImageView.Dock = DockStyle.Fill;
            this.Controls.Add(this.ucImageView);
        }

        /// <summary>
        /// 窗体关闭之后的事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RealTimeImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ucImageView != null)
            {
                this.ucImageView.Dispose();
            }
        }

        /// <summary>
        /// timer
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            EnableWindow(this.Handle, true);
        }

        /// <summary>
        /// 窗体关闭时的事件
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void RealTimeImageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.timer1.Stop();
            this.timer1.Tick -= Timer1_Tick;
            this.timer1.Dispose();
        }

        /// <summary>
        /// 改变相机的名称
        /// </summary>
        /// <param name="cameraName">相机名称</param>
        public static void ChangeCameraVisionByName(string cameraName)
        {
            if (realTimeImageForm != null 
                && realTimeImageForm.IsHandleCreated 
                && !realTimeImageForm.IsDisposed
                && realTimeImageForm.ucImageView != null)
            {
                realTimeImageForm.ucImageView.ClickTitleButton(cameraName);
            }
        }
    }
}