namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    using AKRS.ZX2200.LevelMeasurementSystem.Models;
    using AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Models.BondLevelMeasure;

    /// <summary>
    /// 焊头水平测量
    /// </summary>
    public partial class FrmBondLevelMeasureResult : DevExpress.XtraEditors.XtraForm
    {
        /// <summary>
        /// 资源对象
        /// </summary>
        private System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBondLevelMeasureResult));

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="resultList">结果</param>
        public FrmBondLevelMeasureResult(List<BondLevelMeasureResult> resultList)
        {
            this.InitializeComponent();
            this.GcBondLevelMeasureResult.DataSource = resultList;
        }

        /// <summary>
        /// 行焦点切换时间
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvBondLevelMeasure_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            BondLevelMeasureResult bondLevelMeasureResult = this.GvBondLevelMeasure.GetFocusedRow() as BondLevelMeasureResult;

            if (bondLevelMeasureResult != null)
            {
                double diff90 = Math.Round(bondLevelMeasureResult.Height90 - bondLevelMeasureResult.Height0, 4);
                double diff180 = Math.Round(bondLevelMeasureResult.Height180 - bondLevelMeasureResult.Height0, 4);
                double diff270 = Math.Round(bondLevelMeasureResult.Height270 - bondLevelMeasureResult.Height0, 4);

                this.Lb90.Text = diff90.ToString();
                this.Lb180.Text = diff180.ToString();
                this.Lb270.Text = diff270.ToString();
                 
                this.Lb90.ImageOptions.Image = diff90 > 0 ? (System.Drawing.Image)this.resources.GetObject("Lb180.ImageOptions.Image") : (System.Drawing.Image)this.resources.GetObject("Lb270.ImageOptions.Image");
                this.Lb180.ImageOptions.Image = diff180 > 0 ? (System.Drawing.Image)this.resources.GetObject("Lb180.ImageOptions.Image") : (System.Drawing.Image)this.resources.GetObject("Lb270.ImageOptions.Image");
                this.Lb270.ImageOptions.Image = diff270 > 0 ? (System.Drawing.Image)this.resources.GetObject("Lb180.ImageOptions.Image") : (System.Drawing.Image)this.resources.GetObject("Lb270.ImageOptions.Image");
            }
            else
            {
                this.Lb90.Text = "0.00";
                this.Lb180.Text = "0.00";
                this.Lb270.Text = "0.00";

                this.Lb90.ImageOptions.Image = (System.Drawing.Image)this.resources.GetObject("Lb0.ImageOptions.Image");
                this.Lb180.ImageOptions.Image = (System.Drawing.Image)this.resources.GetObject("Lb0.ImageOptions.Image");
                this.Lb270.ImageOptions.Image = (System.Drawing.Image)this.resources.GetObject("Lb0.ImageOptions.Image");
            }
        }

        /// <summary>
        /// 重测
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void BtRemeasure_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
        }

        /// <summary>
        /// 绘制单元格
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数封装</param>
        private void GvBondLevelMeasure_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            int rowHandle = e.RowHandle;

            BondLevelMeasureResult bondLevelMeasureResult = this.GvBondLevelMeasure.GetRow(rowHandle) as BondLevelMeasureResult;
            if (bondLevelMeasureResult == null)
            {
                return;
            }

            (double MinValue, double MaxValue) ret = bondLevelMeasureResult.GetMinMaxValue();

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo cellInfo = e.Cell as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridCellInfo;
            if (cellInfo.IsDataCell && cellInfo.Column.FieldName != "MaxDifference")
            {
                if (Math.Abs(ret.MinValue - double.Parse(cellInfo.CellValue.ToString())) < 0.000001)
                {
                    e.Appearance.BackColor = Color.YellowGreen;
                }
                else if (Math.Abs(ret.MaxValue - double.Parse(cellInfo.CellValue.ToString())) < 0.000001)
                {
                    e.Appearance.BackColor = Color.OrangeRed;
                }
                else
                {
                    e.Appearance.BackColor = Color.AliceBlue;
                }
            }
        }
    }
}