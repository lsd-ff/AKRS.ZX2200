namespace AKRS.ZX2200.SupportFeature.RealTimeDisplay
{
    partial class UcRealTimeHeight
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.ChartPostBondResX = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartPostBondResX
            // 
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.MinorCount = 1;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.ChartPostBondResX.Diagram = xyDiagram1;
            this.ChartPostBondResX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartPostBondResX.Location = new System.Drawing.Point(0, 0);
            this.ChartPostBondResX.Name = "ChartPostBondResX";
            series1.Name = "高度(um)";
            series1.View = lineSeriesView1;
            this.ChartPostBondResX.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.ChartPostBondResX.Size = new System.Drawing.Size(980, 573);
            this.ChartPostBondResX.TabIndex = 36;
            chartTitle1.Text = "测高数据实时显示";
            this.ChartPostBondResX.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // UcRealTimeHeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChartPostBondResX);
            this.Name = "UcRealTimeHeight";
            this.Size = new System.Drawing.Size(980, 573);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResX)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl ChartPostBondResX;
    }
}
