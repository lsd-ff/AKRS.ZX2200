using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Action;
    using AKRS.ZX2200.TransportUnitSystem.Model;

    /// <summary>
    /// 示教安全高度
    /// </summary>
    public partial class FrmEditSafeHeight : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmEditSafeHeight()
        {
            this.InitializeComponent();
            this.InitControl();
        }

        /// <summary>
        /// 安全高度
        /// </summary>
        public int SafeHeight { get; set; }

        /// <summary>
        /// 增加安全高度
        /// </summary>
        public int AdditionalSafeHeight { get; set; }

        /// <summary>
        /// 测高方式
        /// </summary>
        public AdditionalSafetyHeightType Type { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitControl()
        {
            string message1 = "请输入焊头XY在载具上平移时的安全高度，此高度的基础为上一步测高时的高度";
            this.labelControl1.Text = message1;

            string message2 =
                "Choose area to use additional safety height.The additional safety height will be used as int as"
                + "the chosen area is not left.Reference is transport unit zero height";
            this.labelControl2.Text = message2;

            this.LueAdditionalSafetyHeight.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<AdditionalSafetyHeightType>();
            this.LueAdditionalSafetyHeight.EditValue = AdditionalSafetyHeightType.Off;
        }

        /// <summary>
        /// 回退
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
        }

        /// <summary>
        /// Next
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            this.Save();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtCancel_Click(object sender, EventArgs e)
        {
            this.Save();
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            this.SafeHeight = (int)this.SpSafetyHeight.Value;
            this.AdditionalSafeHeight = (int)this.SpAdditionalSafetyHeight.Value;
            this.Type = (AdditionalSafetyHeightType)this.LueAdditionalSafetyHeight.EditValue;
        }
    }
}