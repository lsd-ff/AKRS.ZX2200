namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;

    /// <summary>
    /// 距离测量结果显示
    /// </summary>
    public partial class FrmDistanceMeasureResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">测量次数</param>
        /// <param name="x">X方向距离</param>
        /// <param name="y">Y方向距离</param>
        public FrmDistanceMeasureResult(int count,double x,double y)
        {
            this.count = count;
            this.x = x;
            this.y = y;
            this.InitializeComponent();
          this.Init();
        }

        /// <summary>
        /// 测量次数
        /// </summary>
        private int count;

        /// <summary>
        /// X方向距离
        /// </summary>
        private double x;

        /// <summary>
        /// Y方向距离
        /// </summary>
        private double y;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.LbTimes.Text = this.count.ToString();
            this.LbDistanceX.Text = this.x.ToString();
            this.LbDistanceY.Text = this.y.ToString();
            this.LbDistance.Text = Math.Sqrt(this.x * this.x + this.y * this.y).ToString();
        }
    }
}