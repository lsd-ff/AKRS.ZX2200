namespace AKRS.ZX2200.Experiment.Test
{
    using AKRS.ZX2200.CalibSystem.Test;

    /// <summary>
    /// 三点一线测试结果
    /// </summary>
    public partial class FrmBondToCamResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="result">结果</param>
        public FrmBondToCamResult(BondToCamResult result)
        {
            this.InitializeComponent();
            this.gridControl1.DataSource = result;
        }
    }
}