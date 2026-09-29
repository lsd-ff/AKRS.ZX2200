namespace AKRS.ZX2200.Experiment.Test
{
    using System.Collections.Generic;

    /// <summary>
    /// 旋转中心测试结果
    /// </summary>
    public partial class FrmRotateCenterResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rotateResults">实验结果</param>
        public FrmRotateCenterResult(List<RotateResult> rotateResults)
        {
            this.InitializeComponent();
            this.gridControl1.DataSource = rotateResults;
        }
    }
}