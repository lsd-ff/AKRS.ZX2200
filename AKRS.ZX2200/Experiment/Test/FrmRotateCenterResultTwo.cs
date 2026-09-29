namespace AKRS.ZX2200.Experiment.Test
{
    using System.Collections.Generic;

    public partial class FrmRotateCenterResultTwo : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 旋转中心测试结果
        /// </summary>
        /// <param name="rotateResults">结果</param>
        public FrmRotateCenterResultTwo(List<RotateCenterResult> rotateResults)
        {
            this.InitializeComponent();
            this.gridControl1.DataSource = rotateResults;
        }
    }
}