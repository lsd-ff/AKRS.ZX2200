namespace AKRS.Calibration.Controls
{
    using System.Windows.Forms;

    using VMControls.Interface;

    /// <summary>
    /// 图像控件
    /// </summary>
    public partial class RenderControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RenderControl()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 图像源
        /// </summary>
        private IVmModule moduleSoure;

        /// <summary>
        /// 绑定
        /// </summary>
        public IVmModule ModuleSource
        {
            get
            {
                return this.moduleSoure;
            }

            set
            {
                this.vmRenderControl1.ModuleSource = value;
                this.moduleSoure = value;
            }
        }

    }
}
