using System;
using System.Windows.Forms;

namespace AKRS.Galaxy2.PR.Controls
{
    /// <summary>
    /// 创建窗体
    /// </summary>
    public partial class FrmName : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造器
        /// </summary>
        public FrmName()
        { 

        }

        /// <summary>
        /// 构造器
        /// </summary>
        public FrmName(string intialName, string labelText = "PR名称",string title = "保存PR模板")
        {
            InitializeComponent();
            txtName.Text = intialName;
            this.labelControl1.Text = labelText;
            this.Text = title;
        }

        /// <summary>
        /// 创建的名称
        /// </summary>
        public string CreateName = string.Empty;

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">传输参数</param>
        private void BtnSure_Click(object sender, EventArgs e)
        {
            CreateName = txtName.Text;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">传输参数</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CreateName = txtName.Text;
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 名称改变事件
        /// </summary>
        /// <param name="sender">触发源</param>
        /// <param name="e">传输参数</param>
        private void TxtName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                this.btnSure.Enabled = false;
            if (string.IsNullOrEmpty(txtName.Text))
                this.btnSure.Enabled = true;
        }
    }
}
