using System;
using AKRS.ZX2200.CalibSystem.Models;

namespace AKRS.ZX2200.CalibSystem.Controls
{
    /// <summary>
    /// 标定结果显示界面
    /// </summary>
    public partial class FrmCalibResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 关闭父窗体委托
        /// </summary>
        public event EventHandler CloseParentForm;

        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmCalibResult()
        {
            this.InitializeComponent();
            this.Init();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            this.RefreshData();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            this.GcCameraScale.DataSource = CalibrateTask.GetInstance().CalibCamScales;
            this.GcTrans.DataSource = CalibrateTask.GetInstance().CalibTransResults;

            this.GcCameraScale.Refresh();
            this.GcTrans.Refresh();
            this.gridView1.RefreshData();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.Close();
            this.CloseParentForm?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">事件源</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.CloseParentForm?.Invoke(this, EventArgs.Empty);
        }
    }
}