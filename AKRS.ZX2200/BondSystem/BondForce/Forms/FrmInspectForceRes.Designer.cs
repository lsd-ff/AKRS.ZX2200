namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    partial class FrmInspectForceRes
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
            DevExpress.XtraCharts.SwiftPlotDiagram swiftPlotDiagram1 = new DevExpress.XtraCharts.SwiftPlotDiagram();
            DevExpress.XtraCharts.ConstantLine constantLine1 = new DevExpress.XtraCharts.ConstantLine();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SeriesPoint seriesPoint1 = new DevExpress.XtraCharts.SeriesPoint(0D, new object[] {
            ((object)(0D))});
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.ChartModbusRTUForce = new DevExpress.XtraCharts.ChartControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.SpBondForce = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.SpDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.BtnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBondForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ChartModbusRTUForce
            // 
            swiftPlotDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            constantLine1.AxisValueSerializable = "1";
            constantLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            constantLine1.Name = "Constant Line 1";
            constantLine1.Title.Text = "Force  standard";
            swiftPlotDiagram1.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
            constantLine1});
            swiftPlotDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.ChartModbusRTUForce.Diagram = swiftPlotDiagram1;
            this.ChartModbusRTUForce.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChartModbusRTUForce.Location = new System.Drawing.Point(0, 86);
            this.ChartModbusRTUForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartModbusRTUForce.Name = "ChartModbusRTUForce";
            series1.CrosshairTextOptions.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            series1.Name = "Force";
            seriesPoint1.ColorSerializable = "#F00000";
            series1.Points.AddRange(new DevExpress.XtraCharts.SeriesPoint[] {
            seriesPoint1});
            swiftPlotSeriesView1.LineStyle.Thickness = 3;
            series1.View = swiftPlotSeriesView1;
            this.ChartModbusRTUForce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.ChartModbusRTUForce.Size = new System.Drawing.Size(1285, 607);
            this.ChartModbusRTUForce.TabIndex = 25;
            chartTitle1.Text = "Bond Force: g";
            this.ChartModbusRTUForce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(1057, 33);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(129, 33);
            this.BtnStart.TabIndex = 32;
            this.BtnStart.Text = "开始";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // SpBondForce
            // 
            this.SpBondForce.EditValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.SpBondForce.Location = new System.Drawing.Point(74, 37);
            this.SpBondForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpBondForce.Name = "SpBondForce";
            this.SpBondForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBondForce.Properties.MaskSettings.Set("mask", "");
            this.SpBondForce.Properties.MaxValue = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.SpBondForce.Size = new System.Drawing.Size(114, 24);
            this.SpBondForce.TabIndex = 31;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(19, 41);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(30, 18);
            this.labelControl10.TabIndex = 30;
            this.labelControl10.Text = "力值";
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(358, 41);
            this.SpTimes.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Properties.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.SpTimes.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Size = new System.Drawing.Size(114, 24);
            this.SpTimes.TabIndex = 34;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(305, 42);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 18);
            this.labelControl1.TabIndex = 33;
            this.labelControl1.Text = "次数";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.SpDelay);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.BtnStart);
            this.groupControl1.Controls.Add(this.SpTimes);
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.SpBondForce);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1285, 86);
            this.groupControl1.TabIndex = 35;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(739, 41);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(20, 18);
            this.labelControl3.TabIndex = 38;
            this.labelControl3.Text = "ms";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(543, 41);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(30, 18);
            this.labelControl4.TabIndex = 36;
            this.labelControl4.Text = "延时";
            // 
            // SpDelay
            // 
            this.SpDelay.EditValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpDelay.Location = new System.Drawing.Point(598, 39);
            this.SpDelay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpDelay.Name = "SpDelay";
            this.SpDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDelay.Properties.IsFloatValue = false;
            this.SpDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpDelay.Properties.MaxValue = new decimal(new int[] {
            700,
            0,
            0,
            0});
            this.SpDelay.Size = new System.Drawing.Size(114, 24);
            this.SpDelay.TabIndex = 37;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(216, 41);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(8, 18);
            this.labelControl2.TabIndex = 35;
            this.labelControl2.Text = "g";
            // 
            // BtnExport
            // 
            this.BtnExport.Location = new System.Drawing.Point(522, 715);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(190, 46);
            this.BtnExport.TabIndex = 36;
            this.BtnExport.Text = "打印";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // FrmInspectForceRes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 819);
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.ChartModbusRTUForce);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmInspectForceRes";
            this.Text = "力值检验";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmInspectForceRes_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBondForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl ChartModbusRTUForce;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.SpinEdit SpBondForce;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnExport;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit SpDelay;
    }
}