using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AKRS.ZX2200.SupportFeature.Statistics
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.Main.Machine.Product;

    /// <summary>
    /// 每个焊点单独的补偿
    /// </summary>
    public partial class FrmSingleBpData : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 刷新的委托
        /// </summary>
        public static Action ReFreshDataAction { get; set; }

        /// <summary>
        /// 每个焊点的补偿数据
        /// </summary>
        public FrmSingleBpData()
        {
            this.InitializeComponent();
            ReFreshDataAction += this.ReFreshData;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmSingleBpData_Load(object sender, EventArgs e)
        {
            this.gridControl1.DataSource = ProductDomain.GetInstance().ProductConfig.OtherConfig.BpSingleSeparates;
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void ReFreshData()
        {
            if (this.IsDisposed || this.Disposing || !this.Created)
            {
                return;
            }

            this.BeginInvoke(
                () =>
                    {
                        this.gridControl1.DataSource =
                            ProductDomain.GetInstance().ProductConfig.OtherConfig.BpSingleSeparates;
                    });
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmSingleBpData_FormClosed(object sender, FormClosedEventArgs e)
        {
            ReFreshDataAction -= this.ReFreshData;
        }

        /// <summary>
        /// 清除所有补偿
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtClear_Click(object sender, EventArgs e)
        {
            DialogResult result = AKRSXtraMessageBox.Show("确定删除所有补偿?", "提示", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                ProductDomain.GetInstance().ProductConfig.OtherConfig.BpSingleSeparates.Clear();
                ProductDomain.GetInstance().Save();
                this.ReFreshData();
            }
        }
    }
}