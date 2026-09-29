namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    partial class PointShow
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.RadarDiagram radarDiagram1 = new DevExpress.XtraCharts.RadarDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.RadarPointSeriesView radarPointSeriesView1 = new DevExpress.XtraCharts.RadarPointSeriesView();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(radarDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(radarPointSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            radarDiagram1.AxisX.VisualRange.Auto = false;
            radarDiagram1.AxisX.VisualRange.MaxValueSerializable = "350";
            radarDiagram1.AxisX.VisualRange.MinValueSerializable = "0";
            radarDiagram1.AxisX.WholeRange.Auto = false;
            radarDiagram1.AxisX.WholeRange.MaxValueSerializable = "360";
            radarDiagram1.AxisX.WholeRange.MinValueSerializable = "0";
            radarDiagram1.AxisY.Interlaced = true;
            radarDiagram1.AxisY.MinorCount = 11;
            radarDiagram1.AxisY.Tickmarks.MinorVisible = false;
            radarDiagram1.AxisY.VisualRange.Auto = false;
            radarDiagram1.AxisY.VisualRange.MaxValueSerializable = "200";
            radarDiagram1.AxisY.VisualRange.MinValueSerializable = "0";
            radarDiagram1.AxisY.WholeRange.Auto = false;
            radarDiagram1.AxisY.WholeRange.MaxValueSerializable = "200";
            radarDiagram1.AxisY.WholeRange.MinValueSerializable = "0";
            this.chartControl1.Diagram = radarDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Legend.MarkerSize = new System.Drawing.Size(1, 1);
            this.chartControl1.Location = new System.Drawing.Point(0, 0);
            this.chartControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chartControl1.Name = "chartControl1";
            series1.Name = "Series 1";
            radarPointSeriesView1.PointMarkerOptions.Size = 7;
            series1.View = radarPointSeriesView1;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControl1.Size = new System.Drawing.Size(1066, 567);
            this.chartControl1.TabIndex = 0;
            // 
            // PointShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 567);
            this.Controls.Add(this.chartControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "PointShow";
            this.Text = "PointShow";
            ((System.ComponentModel.ISupportInitialize)(radarDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(radarPointSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
    }
}