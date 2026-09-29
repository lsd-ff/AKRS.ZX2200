using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AKRS.ZX2200.WaferSubSystem.Models;
using DevExpress.XtraEditors;

namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.BondSystem.Models.Programs;

    /// <summary>
    ///  吸嘴选择界面
    /// </summary>
    public partial class FrmNozzleSelect : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmNozzleSelect()
        {
            InitializeComponent();
            this.RefreshControl();
        }

        /// <summary>
        /// 选中的吸嘴
        /// </summary>
        public string nozzle;

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void RefreshControl()
        {
            List<string> nameList = BondProgram.GetInstance().NozzleShelfProgram.GetNozzleNameList();

            this.CmbNozzle.Properties.Items.Clear();
            this.CmbNozzle.Properties.Items.AddRange(nameList);
            //this.CmbComponent.SelectedIndex = 0;
            //this.CmbComponent.SelectedItem = this.component;
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
            this.nozzle = this.CmbNozzle.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}