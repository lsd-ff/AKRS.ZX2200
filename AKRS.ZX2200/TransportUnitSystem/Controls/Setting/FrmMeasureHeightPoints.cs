using System.Collections.Generic;

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Setting
{
    using AKRS.Galaxy2.Infrastructure.CommonModel;
    using AKRS.ZX2200.Infrastructure.Controls.Currency;
    using AKRS.ZX2200.TransportUnitSystem.Module.Config;
    using DevExpress.CodeParser;

    /// <summary>
    /// 测高定位界面
    /// </summary>
    public partial class FrmMeasureHeightPoints : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 测高界面
        /// </summary>
        /// <param name="baseConfig">配置</param>
        public FrmMeasureHeightPoints(BaseConfig baseConfig)
        {
            this.baseConfig = baseConfig;
            this.InitializeComponent();
        }

        /// <summary>
        /// 测高数据
        /// </summary>
        private readonly BaseConfig baseConfig;

        /// <summary>
        /// 所有
        /// </summary>
        private int Index => this.CmbPoint.SelectedIndex;

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            this.SpX.EditValueChanged += (s, e) =>
                {
                    if (this.SpX.EditValue is decimal decimalValue)
                    {
                        double intValue = (double)decimalValue; // 手动转换

                        // 更新数据源
                        this.baseConfig.HeightMeasurementPoints[Index].X = intValue;
                    }
                };

            this.SpY.EditValueChanged += (s, e) =>
                {
                    if (this.SpY.EditValue is decimal decimalValue)
                    {
                        double intValue = (double)decimalValue; // 手动转换

                        // 更新数据源
                        this.baseConfig.HeightMeasurementPoints[Index].Y = intValue;
                    }
                };

            this.SpZ.EditValueChanged += (s, e) =>
                {
                    if (this.SpZ.EditValue is decimal decimalValue)
                    {
                        double intValue = (double)decimalValue; // 手动转换

                        // 更新数据源
                        this.baseConfig.HeightMeasurementPoints[Index].Z = intValue;
                    }
                };
        }

        /// <summary>
        /// 初始化选项框
        /// </summary>
        private void RefreshComboBox()
        {
            for (int i = 0; i < this.baseConfig.HeightMeasurementPoints.Count; i++)
            {
                this.CmbPoint.Properties.Items.Add($"测高第{(i + 1).ToString()}个点");
            }

            this.CmbPoint.SelectedIndex = 0;
        }

        /// <summary>
        /// 选项改变
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void CmbPoint_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            this.SpX.Value = (decimal)this.baseConfig.HeightMeasurementPoints[this.Index].X;
            this.SpY.Value = (decimal)this.baseConfig.HeightMeasurementPoints[this.Index].Y;
            this.SpZ.Value = (decimal)this.baseConfig.HeightMeasurementPoints[this.Index].Z;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void FrmMeasureHeightPoints_Load(object sender, System.EventArgs e)
        {
            this.RefreshComboBox();
            this.RefreshData();
        }
    }
}
