namespace AKRS.ZX2200.Main.Machine.Product.Controls
{
    using System;
    using System.Windows.Forms;

    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;

    /// <summary>
    /// 工作模式选择窗体
    /// </summary>
    public partial class FrmProductionMode : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmProductionMode()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtnOK_Click(object sender, EventArgs e)
        {
            MachineStateModel.GetInstance().MachineWorkMode = (MachineWorkModeEnum)this.RgProductionMode.SelectedIndex;
            MachineWorkModeEnum eum = MachineStateModel.GetInstance().MachineWorkMode;
            MachineStateModel.GetInstance().Save();

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        private void InitControl()
        {
            MachineWorkModeEnum eum = MachineStateModel.GetInstance().MachineWorkMode;
            this.RgProductionMode.SelectedIndex = (int)MachineStateModel.GetInstance().MachineWorkMode;
        }
    }
}