namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using System.Windows.Forms;

    using AKRS.ZX2200.TransportUnitSystem.Module.Config;

    /// <summary>
    /// 输入行列间距
    /// </summary>
    public partial class FrmInputMatrixInfo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 事件源
        /// </summary>
        /// <param name="arrayConfig">矩阵</param>
        public FrmInputMatrixInfo(ArrayConfig arrayConfig)
        {
            this.InitializeComponent();
            this.arrayConfig = arrayConfig;
        }

        /// <summary>
        /// 距离开始点的X
        /// </summary>
        public double DistanceToStartX { get; set; }

        /// <summary>
        /// 距离开始点的Y
        /// </summary>
        public double DistanceToStartY { get; set; }

        /// <summary>
        /// 矩阵
        /// </summary>
        private readonly ArrayConfig arrayConfig;

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtSure_Click(object sender, System.EventArgs e)
        {
            this.arrayConfig.ColumnCount = (int)this.SpColumnCount.Value;
            this.arrayConfig.ColumnSpacing = (double)this.SpColumnSpacing.Value;
            this.arrayConfig.RowCount = (int)this.SpRows.Value;
            this.arrayConfig.RowSpacing = (double)this.SpRowSpacing.Value;
            this.DistanceToStartX = (double)this.SpStartX.Value;
            this.DistanceToStartY = (double)this.SpStartY.Value;
            this.DialogResult = DialogResult.OK;
        }
    }
}