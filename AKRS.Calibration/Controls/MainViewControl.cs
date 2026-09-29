namespace AKRS.Calibration.Controls
{
    using System.Windows.Forms;

    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainViewControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainViewControl()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// 加锁
        /// </summary>
        public void Lock()
        {
            this.vmMainViewConfigControl1.LockWorkArea();
        }

        /// <summary>
        /// 解锁
        /// </summary>
        public void Unlock() 
        { 
            this.vmMainViewConfigControl1?.UnlockWorkArea();
        }
    }
}
