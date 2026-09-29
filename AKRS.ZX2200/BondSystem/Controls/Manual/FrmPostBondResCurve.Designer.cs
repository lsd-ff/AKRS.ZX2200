namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class FrmPostBondResCurve
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
            DevExpress.XtraCharts.XYDiagram xyDiagram3 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series3 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView3 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle3 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.XYDiagram xyDiagram4 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series4 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView4 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle4 = new DevExpress.XtraCharts.ChartTitle();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GcPostBondRes = new DevExpress.XtraGrid.GridControl();
            this.GvPostBondRes = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.ChartPostBondResAngle = new DevExpress.XtraCharts.ChartControl();
            this.ChartPostBondResX = new DevExpress.XtraCharts.ChartControl();
            this.BtExport = new DevExpress.XtraEditors.SimpleButton();
            this.CmbPostBond = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtClearResult = new DevExpress.XtraEditors.SimpleButton();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcPostBondRes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPostBondRes)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResAngle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPostBond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 82);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(1280, 747);
            this.xtraTabControl1.TabIndex = 37;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GcPostBondRes);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1278, 715);
            this.xtraTabPage1.Text = "结果表格";
            // 
            // GcPostBondRes
            // 
            this.GcPostBondRes.Location = new System.Drawing.Point(0, 0);
            this.GcPostBondRes.MainView = this.GvPostBondRes;
            this.GcPostBondRes.Name = "GcPostBondRes";
            this.GcPostBondRes.Size = new System.Drawing.Size(1278, 714);
            this.GcPostBondRes.TabIndex = 0;
            this.GcPostBondRes.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvPostBondRes});
            // 
            // GvPostBondRes
            // 
            this.GvPostBondRes.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.GvPostBondRes.GridControl = this.GcPostBondRes;
            this.GvPostBondRes.Name = "GvPostBondRes";
            this.GvPostBondRes.OptionsBehavior.ReadOnly = true;
            this.GvPostBondRes.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "X(um)";
            this.gridColumn2.FieldName = "OffsetX";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 94;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Y(um)";
            this.gridColumn3.FieldName = "OffsetY";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 94;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Angle(°)";
            this.gridColumn4.FieldName = "OffsetAngle";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 94;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.tablePanel1);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1278, 715);
            this.xtraTabPage2.Text = "结果曲线";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.ChartPostBondResAngle);
            this.tablePanel1.Controls.Add(this.ChartPostBondResX);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 30F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 30F)});
            this.tablePanel1.Size = new System.Drawing.Size(1278, 715);
            this.tablePanel1.TabIndex = 36;
            // 
            // ChartPostBondResAngle
            // 
            this.tablePanel1.SetColumn(this.ChartPostBondResAngle, 0);
            xyDiagram3.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram3.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram3.AxisY.VisualRange.Auto = false;
            xyDiagram3.AxisY.VisualRange.MaxValueSerializable = "1.5";
            xyDiagram3.AxisY.VisualRange.MinValueSerializable = "-1.5";
            xyDiagram3.AxisY.WholeRange.Auto = false;
            xyDiagram3.AxisY.WholeRange.MaxValueSerializable = "1.5";
            xyDiagram3.AxisY.WholeRange.MinValueSerializable = "-1.5";
            this.ChartPostBondResAngle.Diagram = xyDiagram3;
            this.ChartPostBondResAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartPostBondResAngle.Location = new System.Drawing.Point(3, 362);
            this.ChartPostBondResAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartPostBondResAngle.Name = "ChartPostBondResAngle";
            this.tablePanel1.SetRow(this.ChartPostBondResAngle, 1);
            series3.Name = "角度(°)";
            series3.View = lineSeriesView3;
            this.ChartPostBondResAngle.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series3};
            this.ChartPostBondResAngle.Size = new System.Drawing.Size(1272, 349);
            this.ChartPostBondResAngle.TabIndex = 37;
            chartTitle3.Text = "焊后曲线角度";
            this.ChartPostBondResAngle.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle3});
            // 
            // ChartPostBondResX
            // 
            this.tablePanel1.SetColumn(this.ChartPostBondResX, 0);
            xyDiagram4.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram4.AxisY.MinorCount = 1;
            xyDiagram4.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram4.AxisY.VisualRange.Auto = false;
            xyDiagram4.AxisY.VisualRange.MaxValueSerializable = "20";
            xyDiagram4.AxisY.VisualRange.MinValueSerializable = "-20";
            xyDiagram4.AxisY.WholeRange.Auto = false;
            xyDiagram4.AxisY.WholeRange.MaxValueSerializable = "20";
            xyDiagram4.AxisY.WholeRange.MinValueSerializable = "-20";
            this.ChartPostBondResX.Diagram = xyDiagram4;
            this.ChartPostBondResX.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ChartPostBondResX.Location = new System.Drawing.Point(3, 4);
            this.ChartPostBondResX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChartPostBondResX.Name = "ChartPostBondResX";
            this.tablePanel1.SetRow(this.ChartPostBondResX, 0);
            series4.Name = "X(μm)";
            series4.View = lineSeriesView4;
            this.ChartPostBondResX.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series4};
            this.ChartPostBondResX.Size = new System.Drawing.Size(1272, 350);
            this.ChartPostBondResX.TabIndex = 35;
            chartTitle4.Text = "焊后曲线XY";
            this.ChartPostBondResX.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle4});
            // 
            // BtExport
            // 
            this.BtExport.Location = new System.Drawing.Point(1021, 31);
            this.BtExport.Name = "BtExport";
            this.BtExport.Size = new System.Drawing.Size(149, 31);
            this.BtExport.TabIndex = 3;
            this.BtExport.Text = "打印";
            this.BtExport.Click += new System.EventHandler(this.BtExport_Click);
            // 
            // CmbPostBond
            // 
            this.CmbPostBond.Location = new System.Drawing.Point(265, 31);
            this.CmbPostBond.Name = "CmbPostBond";
            this.CmbPostBond.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbPostBond.Size = new System.Drawing.Size(186, 24);
            this.CmbPostBond.TabIndex = 2;
            this.CmbPostBond.SelectedIndexChanged += new System.EventHandler(this.CmbPostBond_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(155, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "焊后检测";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtClearResult);
            this.groupControl1.Controls.Add(this.BtExport);
            this.groupControl1.Controls.Add(this.CmbPostBond);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1280, 82);
            this.groupControl1.TabIndex = 38;
            // 
            // BtClearResult
            // 
            this.BtClearResult.Location = new System.Drawing.Point(834, 31);
            this.BtClearResult.Name = "BtClearResult";
            this.BtClearResult.Size = new System.Drawing.Size(149, 31);
            this.BtClearResult.TabIndex = 4;
            this.BtClearResult.Text = "清空数据";
            this.BtClearResult.Click += new System.EventHandler(this.BtClearResult_Click);
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "X(μm)";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 94;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "X(μm)";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 94;
            // 
            // FrmPostBondResCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 829);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmPostBondResCurve";
            this.Text = "焊后结果显示";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmPostBondResCurve_FormClosed);
            this.Shown += new System.EventHandler(this.FrmPostBondResCurve_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcPostBondRes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPostBondRes)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(xyDiagram3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResAngle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChartPostBondResX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbPostBond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraCharts.ChartControl ChartPostBondResX;
        private DevExpress.XtraGrid.GridControl GcPostBondRes;
        private DevExpress.XtraGrid.Views.Grid.GridView GvPostBondRes;
        private DevExpress.XtraEditors.SimpleButton BtExport;
        private DevExpress.XtraEditors.ComboBoxEdit CmbPostBond;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraCharts.ChartControl ChartPostBondResAngle;
        private DevExpress.XtraEditors.SimpleButton BtClearResult;
    }
}