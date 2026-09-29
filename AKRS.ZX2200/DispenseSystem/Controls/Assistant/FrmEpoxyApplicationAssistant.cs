namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    using AKRS.ZX2200.DispenseSystem.Models.Repositories.Pattern;
    using AKRS.ZX2200.Infrastructure.Action;

    /// <summary>
    /// 画胶的示教界面
    /// </summary>
    public partial class FrmEpoxyApplicationAssistant : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="epoxyApplication">画胶参数</param>
        public FrmEpoxyApplicationAssistant(EpoxyApplication epoxyApplication)
        {
            this.InitializeComponent();
        }
    }
}