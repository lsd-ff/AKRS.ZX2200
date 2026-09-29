namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram
{
    using System;

    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// Configuration1,用于显示系统1的Tool配置
    /// </summary>
    public partial class UcConfiguration1 : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public UcConfiguration1()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void UcConfigration_Load(object sender, EventArgs e)
        {
            //this.GvPPTool.DataSource = null;
            //this.GvESTool.DataSource = null;
            //this.GvMagazine.DataSource = null;

            this.LbDispenser.Text = "";
            this.LbEpoxyDispenser.Text = "";
        }

        /// <summary>
        /// 提交
        /// </summary>
        public void Confirm()
        {
        }
    }
}
