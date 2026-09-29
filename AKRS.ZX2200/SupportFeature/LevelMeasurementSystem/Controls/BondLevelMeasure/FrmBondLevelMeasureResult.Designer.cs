namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure
{
    partial class FrmBondLevelMeasureResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBondLevelMeasureResult));
            this.GcBondLevelMeasureResult = new DevExpress.XtraGrid.GridControl();
            this.GvBondLevelMeasure = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtRemeasure = new DevExpress.XtraEditors.SimpleButton();
            this.Lb270 = new DevExpress.XtraEditors.LabelControl();
            this.Lb180 = new DevExpress.XtraEditors.LabelControl();
            this.Lb90 = new DevExpress.XtraEditors.LabelControl();
            this.Lb0 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.GcBondLevelMeasureResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvBondLevelMeasure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GcBondLevelMeasureResult
            // 
            this.GcBondLevelMeasureResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcBondLevelMeasureResult.Location = new System.Drawing.Point(0, 0);
            this.GcBondLevelMeasureResult.MainView = this.GvBondLevelMeasure;
            this.GcBondLevelMeasureResult.Name = "GcBondLevelMeasureResult";
            this.GcBondLevelMeasureResult.Size = new System.Drawing.Size(1004, 303);
            this.GcBondLevelMeasureResult.TabIndex = 0;
            this.GcBondLevelMeasureResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvBondLevelMeasure});
            // 
            // GvBondLevelMeasure
            // 
            this.GvBondLevelMeasure.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6});
            this.GvBondLevelMeasure.GridControl = this.GcBondLevelMeasureResult;
            this.GvBondLevelMeasure.Name = "GvBondLevelMeasure";
            this.GvBondLevelMeasure.OptionsView.ShowGroupPanel = false;
            this.GvBondLevelMeasure.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GvBondLevelMeasure_CustomDrawCell);
            this.GvBondLevelMeasure.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GvBondLevelMeasure_FocusedRowChanged);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Height (0°)";
            this.gridColumn2.FieldName = "Height0";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Height (90°)";
            this.gridColumn3.FieldName = "Height90";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Height (180°)";
            this.gridColumn4.FieldName = "Height180";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Height (270°)";
            this.gridColumn5.FieldName = "Height270";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "MaximumDifference";
            this.gridColumn6.FieldName = "MaxDifference";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtRemeasure);
            this.panelControl1.Controls.Add(this.Lb270);
            this.panelControl1.Controls.Add(this.Lb180);
            this.panelControl1.Controls.Add(this.Lb90);
            this.panelControl1.Controls.Add(this.Lb0);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 303);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1004, 239);
            this.panelControl1.TabIndex = 1;
            // 
            // BtRemeasure
            // 
            this.BtRemeasure.Location = new System.Drawing.Point(895, 182);
            this.BtRemeasure.Name = "BtRemeasure";
            this.BtRemeasure.Size = new System.Drawing.Size(75, 28);
            this.BtRemeasure.TabIndex = 4;
            this.BtRemeasure.Text = "Remeasure";
            this.BtRemeasure.Click += new System.EventHandler(this.BtRemeasure_Click);
            // 
            // Lb270
            // 
            this.Lb270.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Lb270.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Lb270.ImageOptions.Image")));
            this.Lb270.Location = new System.Drawing.Point(642, 97);
            this.Lb270.Name = "Lb270";
            this.Lb270.Size = new System.Drawing.Size(69, 36);
            this.Lb270.TabIndex = 3;
            this.Lb270.Text = "0.000";
            // 
            // Lb180
            // 
            this.Lb180.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Lb180.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Lb180.ImageOptions.Image")));
            this.Lb180.Location = new System.Drawing.Point(445, 37);
            this.Lb180.Name = "Lb180";
            this.Lb180.Size = new System.Drawing.Size(69, 36);
            this.Lb180.TabIndex = 2;
            this.Lb180.Text = "0.000";
            // 
            // Lb90
            // 
            this.Lb90.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Lb90.Location = new System.Drawing.Point(269, 97);
            this.Lb90.Name = "Lb90";
            this.Lb90.Size = new System.Drawing.Size(32, 14);
            this.Lb90.TabIndex = 1;
            this.Lb90.Text = "0.000";
            // 
            // Lb0
            // 
            this.Lb0.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.Lb0.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("Lb0.ImageOptions.Image")));
            this.Lb0.Location = new System.Drawing.Point(445, 192);
            this.Lb0.Name = "Lb0";
            this.Lb0.Size = new System.Drawing.Size(37, 36);
            this.Lb0.TabIndex = 0;
            // 
            // FrmBondLevelMeasureResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 542);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.GcBondLevelMeasureResult);
            this.Name = "FrmBondLevelMeasureResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Head Level Measure Reslut";
            ((System.ComponentModel.ISupportInitialize)(this.GcBondLevelMeasureResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvBondLevelMeasure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GcBondLevelMeasureResult;
        private DevExpress.XtraGrid.Views.Grid.GridView GvBondLevelMeasure;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl Lb0;
        private DevExpress.XtraEditors.LabelControl Lb270;
        private DevExpress.XtraEditors.LabelControl Lb180;
        private DevExpress.XtraEditors.LabelControl Lb90;
        private DevExpress.XtraEditors.SimpleButton BtRemeasure;
    }
}