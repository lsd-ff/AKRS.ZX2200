namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using System;
    using System.ComponentModel;

    using AKRS.Galaxy2.Infrastructure.Helper;

    /// <summary>
    /// 多功能测高界面选择
    /// </summary>
    public partial class FrmMultipleHeightMeasurementType : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FrmMultipleHeightMeasurementType()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 测高方式
        /// </summary>
        public MultipleHeightMeasurementType Type { get; set; } = MultipleHeightMeasurementType.TouchDown;

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void FrmMultipleHeightMeasurementType_Load(object sender, EventArgs e)
        {
           this.LueAdjustType.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<MultipleHeightMeasurementType>();
           this.LueAdjustType.EditValue = MultipleHeightMeasurementType.TouchDown;
        }

        /// <summary>
        /// 测高方式
        /// </summary>
        public enum MultipleHeightMeasurementType
        {
            /// <summary>
            /// TouchDown
            /// </summary>
            [Description("Touch Down")]
            TouchDown,

            /// <summary>
            /// SubstrateCamera
            /// </summary>
            [Description("Substrate Camera")]
            SubstrateCamera
        }

        /// <summary>
        /// 测高方式发生改变
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void LueAdjustType_EditValueChanged(object sender, EventArgs e)
        {
            this.Type = MultipleHeightMeasurementType.TouchDown;
        }

        /// <summary>
        /// 下一步
        /// </summary>
        /// <param name="sender">事件</param>
        /// <param name="e">参数</param>
        private void BtNext_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}