namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    partial class FrmAxisTemperature
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.Series series2 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView2 = new DevExpress.XtraCharts.LineSeriesView();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.label5 = new System.Windows.Forms.Label();
            this.SpMin = new DevExpress.XtraEditors.SpinEdit();
            this.label4 = new System.Windows.Forms.Label();
            this.SpMax = new DevExpress.XtraEditors.SpinEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.SpKd = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.SpKi = new DevExpress.XtraEditors.SpinEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.SpKp = new DevExpress.XtraEditors.SpinEdit();
            this.ChkTemperatureControl = new DevExpress.XtraEditors.CheckEdit();
            this.BtStart = new DevExpress.XtraEditors.CheckEdit();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpMin.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMax.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTemperatureControl.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtStart.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // chartControl1
            // 
            this.tablePanel1.SetColumn(this.chartControl1, 0);
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            this.chartControl1.Diagram = xyDiagram1;
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(3, 156);
            this.chartControl1.Name = "chartControl1";
            this.tablePanel1.SetRow(this.chartControl1, 1);
            series1.Name = "Series 1";
            series1.View = lineSeriesView1;
            series2.Name = "Series 2";
            series2.View = lineSeriesView2;
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1,
        series2};
            this.chartControl1.Size = new System.Drawing.Size(1267, 608);
            this.chartControl1.TabIndex = 0;
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.chartControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 20F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 80F)});
            this.tablePanel1.Size = new System.Drawing.Size(1273, 767);
            this.tablePanel1.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.panelControl1.Controls.Add(this.BtSave);
            this.panelControl1.Controls.Add(this.label5);
            this.panelControl1.Controls.Add(this.SpMin);
            this.panelControl1.Controls.Add(this.label4);
            this.panelControl1.Controls.Add(this.SpMax);
            this.panelControl1.Controls.Add(this.label3);
            this.panelControl1.Controls.Add(this.SpKd);
            this.panelControl1.Controls.Add(this.label2);
            this.panelControl1.Controls.Add(this.SpKi);
            this.panelControl1.Controls.Add(this.label1);
            this.panelControl1.Controls.Add(this.SpKp);
            this.panelControl1.Controls.Add(this.ChkTemperatureControl);
            this.panelControl1.Controls.Add(this.BtStart);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 0);
            this.panelControl1.Size = new System.Drawing.Size(1267, 147);
            this.panelControl1.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(984, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 14);
            this.label5.TabIndex = 15;
            this.label5.Text = "Min";
            // 
            // SpMin
            // 
            this.SpMin.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpMin.Location = new System.Drawing.Point(1029, 71);
            this.SpMin.Name = "SpMin";
            this.SpMin.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMin.Size = new System.Drawing.Size(92, 20);
            this.SpMin.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(796, 74);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 14);
            this.label4.TabIndex = 13;
            this.label4.Text = "Max";
            // 
            // SpMax
            // 
            this.SpMax.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpMax.Location = new System.Drawing.Point(841, 71);
            this.SpMax.Name = "SpMax";
            this.SpMax.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMax.Size = new System.Drawing.Size(92, 20);
            this.SpMax.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(604, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 11;
            this.label3.Text = "Kd";
            // 
            // SpKd
            // 
            this.SpKd.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKd.Location = new System.Drawing.Point(649, 71);
            this.SpKd.Name = "SpKd";
            this.SpKd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKd.Size = new System.Drawing.Size(92, 20);
            this.SpKd.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(405, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 14);
            this.label2.TabIndex = 9;
            this.label2.Text = "Ki";
            // 
            // SpKi
            // 
            this.SpKi.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKi.Location = new System.Drawing.Point(450, 71);
            this.SpKi.Name = "SpKi";
            this.SpKi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKi.Size = new System.Drawing.Size(92, 20);
            this.SpKi.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(199, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 14);
            this.label1.TabIndex = 7;
            this.label1.Text = "Kp";
            // 
            // SpKp
            // 
            this.SpKp.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKp.Location = new System.Drawing.Point(244, 71);
            this.SpKp.Name = "SpKp";
            this.SpKp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKp.Size = new System.Drawing.Size(92, 20);
            this.SpKp.TabIndex = 2;
            // 
            // ChkTemperatureControl
            // 
            this.ChkTemperatureControl.Location = new System.Drawing.Point(5, 63);
            this.ChkTemperatureControl.Name = "ChkTemperatureControl";
            this.ChkTemperatureControl.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkTemperatureControl.Properties.Appearance.Options.UseFont = true;
            this.ChkTemperatureControl.Properties.Caption = "开始恒温控制";
            this.ChkTemperatureControl.Size = new System.Drawing.Size(152, 29);
            this.ChkTemperatureControl.TabIndex = 1;
            this.ChkTemperatureControl.CheckedChanged += new System.EventHandler(this.ChkTemperatureControl_CheckedChanged);
            // 
            // BtStart
            // 
            this.BtStart.Location = new System.Drawing.Point(5, 5);
            this.BtStart.Name = "BtStart";
            this.BtStart.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtStart.Properties.Appearance.Options.UseFont = true;
            this.BtStart.Properties.Caption = "开始采集";
            this.BtStart.Size = new System.Drawing.Size(113, 29);
            this.BtStart.TabIndex = 0;
            this.BtStart.CheckedChanged += new System.EventHandler(this.BtStart_CheckedChanged);
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(1158, 7);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(100, 55);
            this.BtSave.TabIndex = 16;
            this.BtSave.Text = "保存";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // FrmAxisTemperature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1273, 767);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmAxisTemperature";
            this.Text = "FrmAxisTemperature";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAxisTemperature_FormClosed);
            this.Load += new System.EventHandler(this.FrmAxisTemperature_Load);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpMin.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMax.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTemperatureControl.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtStart.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit BtStart;
        private DevExpress.XtraEditors.CheckEdit ChkTemperatureControl;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SpinEdit SpMin;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SpinEdit SpMax;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.SpinEdit SpKd;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.SpinEdit SpKi;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit SpKp;
        private DevExpress.XtraEditors.SimpleButton BtSave;
    }
}