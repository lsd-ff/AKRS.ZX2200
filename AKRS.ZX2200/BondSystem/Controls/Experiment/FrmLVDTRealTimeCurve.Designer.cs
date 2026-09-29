namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    partial class FrmLVDTRealTimeCurve
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraCharts.SwiftPlotDiagram swiftPlotDiagram1 = new DevExpress.XtraCharts.SwiftPlotDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChartModbusRTUForce = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnClear);
            this.groupControl1.Controls.Add(this.BtSave);
            this.groupControl1.Controls.Add(this.BtnStart);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1203, 86);
            this.groupControl1.TabIndex = 37;
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(268, 31);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(129, 33);
            this.BtnClear.TabIndex = 39;
            this.BtnClear.Text = "清空";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(920, 31);
            this.BtSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(129, 33);
            this.BtSave.TabIndex = 38;
            this.BtSave.Text = "数据保存";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(69, 32);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(129, 33);
            this.BtnStart.TabIndex = 34;
            this.BtnStart.Text = "暂停采集";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChartModbusRTUForce
            // 
            swiftPlotDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.ChartModbusRTUForce.Diagram = swiftPlotDiagram1;
            this.ChartModbusRTUForce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartModbusRTUForce.Location = new System.Drawing.Point(0, 86);
            this.ChartModbusRTUForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartModbusRTUForce.Name = "ChartModbusRTUForce";
            series1.Name = "LVDT值";
            series1.View = swiftPlotSeriesView1;
            this.ChartModbusRTUForce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.ChartModbusRTUForce.Size = new System.Drawing.Size(1203, 625);
            this.ChartModbusRTUForce.TabIndex = 38;
            chartTitle1.Text = "";
            this.ChartModbusRTUForce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // FrmLVDTRealTimeCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1203, 711);
            this.Controls.Add(this.ChartModbusRTUForce);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmLVDTRealTimeCurve";
            this.Text = "LVDT实时曲线";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLVDTRealTimeCurve_FormClosing);
            this.Load += new System.EventHandler(this.FrmLVDTRealTimeCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnClear;
        private DevExpress.XtraEditors.SimpleButton BtSave;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraCharts.ChartControl ChartModbusRTUForce;
    }
}