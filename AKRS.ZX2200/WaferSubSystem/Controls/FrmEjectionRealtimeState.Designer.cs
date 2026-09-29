namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    partial class FrmEjectionRealtimeState
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.SwiftPlotDiagram swiftPlotDiagram1 = new DevExpress.XtraCharts.SwiftPlotDiagram();
            DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY swiftPlotDiagramSecondaryAxisY1 = new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView2 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView3 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView4 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChartEjection = new DevExpress.XtraCharts.ChartControl();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEjection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView4)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(1117, 693);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 22);
            this.labelControl1.TabIndex = 35;
            this.labelControl1.Text = "Time : ms";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(66, 13);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(129, 33);
            this.BtnStart.TabIndex = 34;
            this.BtnStart.Text = "开始采集";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChartEjection
            // 
            swiftPlotDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram1.AxisY.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            swiftPlotDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            swiftPlotDiagramSecondaryAxisY1.AxisID = 0;
            swiftPlotDiagramSecondaryAxisY1.Name = "Secondary AxisY 1";
            swiftPlotDiagramSecondaryAxisY1.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY[] {
            swiftPlotDiagramSecondaryAxisY1});
            this.ChartEjection.Diagram = swiftPlotDiagram1;
            this.ChartEjection.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChartEjection.Legend.Direction = DevExpress.XtraCharts.LegendDirection.BottomToTop;
            this.ChartEjection.Location = new System.Drawing.Point(0, 57);
            this.ChartEjection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartEjection.Name = "ChartEjection";
            series1.Name = "实际位置（mm）";
            swiftPlotSeriesView1.AxisYName = "Secondary AxisY 1";
            swiftPlotSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.View = swiftPlotSeriesView1;
            series2.Name = "指令位置（mm）";
            swiftPlotSeriesView2.AxisYName = "Secondary AxisY 1";
            swiftPlotSeriesView2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(89)))), ((int)(((byte)(89)))));
            series2.View = swiftPlotSeriesView2;
            series3.Name = "实际速度（mm/s）";
            swiftPlotSeriesView3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series3.View = swiftPlotSeriesView3;
            series4.Name = "指令速度（mm/s）";
            swiftPlotSeriesView4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series4.View = swiftPlotSeriesView4;
            this.ChartEjection.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3,
        series4};
            this.ChartEjection.Size = new System.Drawing.Size(1200, 667);
            this.ChartEjection.TabIndex = 33;
            chartTitle1.Text = "顶针实时状态显示";
            this.ChartEjection.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(893, 4);
            this.BtSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(129, 33);
            this.BtSave.TabIndex = 36;
            this.BtSave.Text = "数据保存";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(261, 13);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(129, 33);
            this.BtnClear.TabIndex = 37;
            this.BtnClear.Text = "清空";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // FrmEjectionRealtimeState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 724);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.ChartEjection);
            this.Name = "FrmEjectionRealtimeState";
            this.Text = "EjectionRealtimeState";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEjectionRealtimeState_FormClosing);
            this.Load += new System.EventHandler(this.FrmEjectionRealtimeState_Load);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartEjection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraCharts.ChartControl ChartEjection;
        private DevExpress.XtraEditors.SimpleButton BtSave;
        private DevExpress.XtraEditors.SimpleButton BtnClear;
    }
}
