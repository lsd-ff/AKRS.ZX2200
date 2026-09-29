namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    partial class FrmForceControlInspection
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
            DevExpress.XtraCharts.SwiftPlotDiagram swiftPlotDiagram2 = new DevExpress.XtraCharts.SwiftPlotDiagram();
            DevExpress.XtraCharts.ConstantLine constantLine2 = new DevExpress.XtraCharts.ConstantLine();
            DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY swiftPlotDiagramSecondaryAxisY2 = new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView3 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView4 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle2 = new DevExpress.XtraCharts.ChartTitle();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChartModbusRTUForce = new DevExpress.XtraCharts.ChartControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView4)).BeginInit();
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
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1053, 67);
            this.groupControl1.TabIndex = 37;
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(234, 24);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(113, 26);
            this.BtnClear.TabIndex = 39;
            this.BtnClear.Text = "清空";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(805, 24);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(113, 26);
            this.BtSave.TabIndex = 38;
            this.BtSave.Text = "数据保存";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(60, 25);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(113, 26);
            this.BtnStart.TabIndex = 34;
            this.BtnStart.Text = "开始采集";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChartModbusRTUForce
            // 
            swiftPlotDiagram2.AxisX.VisibleInPanesSerializable = "-1";
            constantLine2.AxisValueSerializable = "1";
            constantLine2.Name = "标准值";
            swiftPlotDiagram2.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
            constantLine2});
            swiftPlotDiagram2.AxisY.VisibleInPanesSerializable = "-1";
            swiftPlotDiagramSecondaryAxisY2.AxisID = 0;
            swiftPlotDiagramSecondaryAxisY2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            swiftPlotDiagramSecondaryAxisY2.Name = "Secondary AxisY 1";
            swiftPlotDiagramSecondaryAxisY2.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram2.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY[] {
            swiftPlotDiagramSecondaryAxisY2});
            this.ChartModbusRTUForce.Diagram = swiftPlotDiagram2;
            this.ChartModbusRTUForce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartModbusRTUForce.Location = new System.Drawing.Point(0, 67);
            this.ChartModbusRTUForce.Name = "ChartModbusRTUForce";
            series3.Name = "Force";
            series3.View = swiftPlotSeriesView3;
            series4.Name = "LVDT";
            swiftPlotSeriesView4.AxisYName = "Secondary AxisY 1";
            swiftPlotSeriesView4.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series4.View = swiftPlotSeriesView4;
            this.ChartModbusRTUForce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3,
        series4};
            this.ChartModbusRTUForce.Size = new System.Drawing.Size(1053, 474);
            this.ChartModbusRTUForce.TabIndex = 38;
            chartTitle2.Text = "力值: g";
            this.ChartModbusRTUForce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle2});
            // 
            // FrmForceControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 541);
            this.Controls.Add(this.ChartModbusRTUForce);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmForceControlInspection";
            this.Text = "力控测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmForceControlTest_FormClosing);
            this.Load += new System.EventHandler(this.FrmForceControlTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
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
