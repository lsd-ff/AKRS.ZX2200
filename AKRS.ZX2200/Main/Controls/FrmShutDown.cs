namespace AKRS.ZX2200.Main.Controls
{
    using System.Diagnostics;
    using System.Windows.Forms;

    /// <summary>
    /// 关机/重启窗体
    /// </summary>
    public partial class FrmShutDown : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmShutDown()
        {
            this.InitializeComponent();
            this.LbDescription.Text = @"Do  you  want  to  shut  down  the  machine?";
        }

        /// <summary>
        /// 关机
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnShutDown_Click(object sender, System.EventArgs e)
        {
            // 杀死当前进程
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 重启
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnRebort_Click(object sender, System.EventArgs e)
        {
            // 杀死当前进程，自动重启软件
            Process.Start(Application.StartupPath + "\\AKRS.ZX2200.exe");
            System.Diagnostics.Process.GetCurrentProcess().Kill();
            Application.Exit();
            System.Environment.Exit(0);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, System.EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}