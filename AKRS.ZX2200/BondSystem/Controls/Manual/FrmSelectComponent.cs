using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using AKRS.ZX2200.WaferSubSystem.Models;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    /// <summary>
    /// 选择芯片
    /// </summary>
    public partial class FrmSelectComponent : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="component">芯片</param>
        public FrmSelectComponent()
        {
            InitializeComponent();
            RefreshControl();
        }

        /// <summary>
        /// 选中的芯片
        /// </summary>
        public  string component;

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            List<string> carrierList = WaferSystemProgram.GetInstance().GetCarriers().Select(it => it.Name).ToList();

            this.CmbComponent.Properties.Items.Clear();
            this.CmbComponent.Properties.Items.AddRange(carrierList);
            //this.CmbComponent.SelectedIndex = 0;
            this.CmbComponent.SelectedItem = this.component;
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.component = this.CmbComponent.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}