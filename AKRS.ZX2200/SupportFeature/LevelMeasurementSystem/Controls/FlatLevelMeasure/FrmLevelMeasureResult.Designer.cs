namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure
{
    partial class FrmLevelMeasureResult
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
            this.components = new System.ComponentModel.Container();
            this.GcLevelMeasureResult = new DevExpress.XtraGrid.GridControl();
            this.GvLevelMeasureResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtExport = new DevExpress.XtraEditors.SimpleButton();
            this.xtraOpenFileDialog1 = new DevExpress.XtraEditors.XtraOpenFileDialog(this.components);
            this.BtRemeasure = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GcLevelMeasureResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvLevelMeasureResult)).BeginInit();
            this.SuspendLayout();
            // 
            // GcLevelMeasureResult
            // 
            this.GcLevelMeasureResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcLevelMeasureResult.Location = new System.Drawing.Point(0, 0);
            this.GcLevelMeasureResult.MainView = this.GvLevelMeasureResult;
            this.GcLevelMeasureResult.Name = "GcLevelMeasureResult";
            this.GcLevelMeasureResult.Size = new System.Drawing.Size(1168, 288);
            this.GcLevelMeasureResult.TabIndex = 0;
            this.GcLevelMeasureResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvLevelMeasureResult});
            // 
            // GvLevelMeasureResult
            // 
            this.GvLevelMeasureResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.GvLevelMeasureResult.GridControl = this.GcLevelMeasureResult;
            this.GvLevelMeasureResult.Name = "GvLevelMeasureResult";
            this.GvLevelMeasureResult.OptionsView.ShowGroupPanel = false;
            this.GvLevelMeasureResult.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GvLevelMeasureResult_CustomDrawCell);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Height1";
            this.gridColumn1.FieldName = "Height1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Heght2";
            this.gridColumn2.FieldName = "Height2";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Height3";
            this.gridColumn3.FieldName = "Height3";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Height4";
            this.gridColumn4.FieldName = "Height4";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Height5";
            this.gridColumn5.FieldName = "Height5";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Height6";
            this.gridColumn6.FieldName = "Height6";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Height7";
            this.gridColumn7.FieldName = "Height7";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Height8";
            this.gridColumn8.FieldName = "Height8";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Height9";
            this.gridColumn9.FieldName = "Height9";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Height10";
            this.gridColumn10.FieldName = "Height10";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 9;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Range";
            this.gridColumn11.FieldName = "Range";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            // 
            // BtExport
            // 
            this.BtExport.Location = new System.Drawing.Point(922, 309);
            this.BtExport.Name = "BtExport";
            this.BtExport.Size = new System.Drawing.Size(75, 23);
            this.BtExport.TabIndex = 1;
            this.BtExport.Text = "Export";
            this.BtExport.Click += new System.EventHandler(this.BtExport_Click);
            // 
            // BtRemeasure
            // 
            this.BtRemeasure.Location = new System.Drawing.Point(1047, 309);
            this.BtRemeasure.Name = "BtRemeasure";
            this.BtRemeasure.Size = new System.Drawing.Size(75, 23);
            this.BtRemeasure.TabIndex = 2;
            this.BtRemeasure.Text = "Remeasure";
            this.BtRemeasure.Click += new System.EventHandler(this.BtRemeasure_Click);
            // 
            // FrmLevelMeasureResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1168, 344);
            this.Controls.Add(this.BtRemeasure);
            this.Controls.Add(this.BtExport);
            this.Controls.Add(this.GcLevelMeasureResult);
            this.Name = "FrmLevelMeasureResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Measure Result";
            ((System.ComponentModel.ISupportInitialize)(this.GcLevelMeasureResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvLevelMeasureResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GcLevelMeasureResult;
        private DevExpress.XtraGrid.Views.Grid.GridView GvLevelMeasureResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.SimpleButton BtExport;
        private DevExpress.XtraEditors.XtraOpenFileDialog xtraOpenFileDialog1;
        private DevExpress.XtraEditors.SimpleButton BtRemeasure;
    }
}