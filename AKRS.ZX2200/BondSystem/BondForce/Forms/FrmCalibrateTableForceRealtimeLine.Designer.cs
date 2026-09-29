namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    partial class FrmCalibrateTableForceRealtimeLine
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
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.SpRTUBondForceRead = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.ChartModbusRTUForce = new DevExpress.XtraCharts.ChartControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.SpRTUBondForceRead.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // SpRTUBondForceRead
            // 
            this.SpRTUBondForceRead.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRTUBondForceRead.Location = new System.Drawing.Point(135, 41);
            this.SpRTUBondForceRead.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpRTUBondForceRead.Name = "SpRTUBondForceRead";
            this.SpRTUBondForceRead.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRTUBondForceRead.Properties.ReadOnly = true;
            this.SpRTUBondForceRead.Size = new System.Drawing.Size(114, 24);
            this.SpRTUBondForceRead.TabIndex = 21;
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(32, 45);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(74, 18);
            this.labelControl10.TabIndex = 20;
            this.labelControl10.Text = "Bond Force";
            // 
            // ChartModbusRTUForce
            // 
            swiftPlotDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.ChartModbusRTUForce.Diagram = swiftPlotDiagram1;
            this.ChartModbusRTUForce.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ChartModbusRTUForce.Legend.Direction = DevExpress.XtraCharts.LegendDirection.BottomToTop;
            this.ChartModbusRTUForce.Location = new System.Drawing.Point(0, 94);
            this.ChartModbusRTUForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartModbusRTUForce.Name = "ChartModbusRTUForce";
            series1.Name = "Force(g)";
            series1.View = swiftPlotSeriesView1;
            this.ChartModbusRTUForce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.ChartModbusRTUForce.Size = new System.Drawing.Size(1285, 667);
            this.ChartModbusRTUForce.TabIndex = 24;
            chartTitle1.Text = "Bond Force: g";
            this.ChartModbusRTUForce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(302, 33);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(129, 33);
            this.BtnStart.TabIndex = 29;
            this.BtnStart.Text = "暂停采集";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(1190, 717);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(80, 22);
            this.labelControl1.TabIndex = 30;
            this.labelControl1.Text = "Time : ms";
            // 
            // FrmForceRealtimeLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1285, 761);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.ChartModbusRTUForce);
            this.Controls.Add(this.SpRTUBondForceRead);
            this.Controls.Add(this.labelControl10);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmForceRealtimeLine";
            this.Text = "Force line";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmModbus_FormClosed);
            this.Load += new System.EventHandler(this.FrmModbus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpRTUBondForceRead.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SpinEdit SpRTUBondForceRead;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraCharts.ChartControl ChartModbusRTUForce;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}