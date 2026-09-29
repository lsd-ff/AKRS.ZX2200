namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class FrmForceRealTimeCurve
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
            DevExpress.XtraCharts.ConstantLine constantLine1 = new DevExpress.XtraCharts.ConstantLine();
            DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY swiftPlotDiagramSecondaryAxisY1 = new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView1 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView2 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.SwiftPlotSeriesView swiftPlotSeriesView3 = new DevExpress.XtraCharts.SwiftPlotSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChartModbusRTUForce = new DevExpress.XtraCharts.ChartControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LueReadTiming = new DevExpress.XtraEditors.LookUpEdit();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueReadTiming.Properties)).BeginInit();
            this.SuspendLayout();
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
            constantLine1.AxisValueSerializable = "50";
            constantLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            constantLine1.Name = "参考力值";
            swiftPlotDiagram1.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
            constantLine1});
            swiftPlotDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            swiftPlotDiagramSecondaryAxisY1.AxisID = 0;
            swiftPlotDiagramSecondaryAxisY1.Name = "轴坐标";
            swiftPlotDiagramSecondaryAxisY1.VisibleInPanesSerializable = "-1";
            swiftPlotDiagram1.SecondaryAxesY.AddRange(new DevExpress.XtraCharts.SwiftPlotDiagramSecondaryAxisY[] {
            swiftPlotDiagramSecondaryAxisY1});
            this.ChartModbusRTUForce.Diagram = swiftPlotDiagram1;
            this.ChartModbusRTUForce.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartModbusRTUForce.Location = new System.Drawing.Point(0, 86);
            this.ChartModbusRTUForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartModbusRTUForce.Name = "ChartModbusRTUForce";
            series1.Name = "标定台力值";
            swiftPlotSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            series1.View = swiftPlotSeriesView1;
            series2.Name = "应变片力值";
            swiftPlotSeriesView2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(176)))), ((int)(((byte)(240)))));
            series2.View = swiftPlotSeriesView2;
            series3.Name = "Z轴坐标";
            swiftPlotSeriesView3.AxisYName = "轴坐标";
            swiftPlotSeriesView3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(208)))), ((int)(((byte)(80)))));
            series3.View = swiftPlotSeriesView3;
            this.ChartModbusRTUForce.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2,
        series3};
            this.ChartModbusRTUForce.Size = new System.Drawing.Size(1296, 662);
            this.ChartModbusRTUForce.TabIndex = 33;
            chartTitle1.Text = "力值: g";
            this.ChartModbusRTUForce.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LueReadTiming);
            this.groupControl1.Controls.Add(this.BtnClear);
            this.groupControl1.Controls.Add(this.BtSave);
            this.groupControl1.Controls.Add(this.BtnStart);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1296, 86);
            this.groupControl1.TabIndex = 36;
            // 
            // LueReadTiming
            // 
            this.LueReadTiming.Location = new System.Drawing.Point(519, 36);
            this.LueReadTiming.Name = "LueReadTiming";
            this.LueReadTiming.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueReadTiming.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueReadTiming.Properties.DisplayMember = "Display";
            this.LueReadTiming.Properties.DropDownRows = 5;
            this.LueReadTiming.Properties.NullText = "";
            this.LueReadTiming.Properties.PopupSizeable = false;
            this.LueReadTiming.Properties.PopupWidth = 299;
            this.LueReadTiming.Properties.PopupWidthMode = DevExpress.XtraEditors.PopupWidthMode.ContentWidth;
            this.LueReadTiming.Properties.ValueMember = "Value";
            this.LueReadTiming.Size = new System.Drawing.Size(141, 24);
            this.LueReadTiming.TabIndex = 42;
            this.LueReadTiming.EditValueChanged += new System.EventHandler(this.LueType_EditValueChanged);
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
            // FrmForceRealTimeCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1296, 748);
            this.Controls.Add(this.ChartModbusRTUForce);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmForceRealTimeCurve";
            this.Text = "力控实时曲线";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmForceRealTimeCurve_FormClosing);
            this.Load += new System.EventHandler(this.FrmForceRealTimeCurve_Load);
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagramSecondaryAxisY1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(swiftPlotSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartModbusRTUForce)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueReadTiming.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraCharts.ChartControl ChartModbusRTUForce;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnClear;
        private DevExpress.XtraEditors.SimpleButton BtSave;
        private DevExpress.XtraEditors.LookUpEdit LueReadTiming;
    }
}