namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmBMCTestResultDetail
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
            this.GcTestResultDetail = new DevExpress.XtraGrid.GridControl();
            this.GvTestResultDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtnExport = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestResultDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTestResultDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // GcTestResultDetail
            // 
            this.GcTestResultDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcTestResultDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcTestResultDetail.Location = new System.Drawing.Point(0, 0);
            this.GcTestResultDetail.MainView = this.GvTestResultDetail;
            this.GcTestResultDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcTestResultDetail.Name = "GcTestResultDetail";
            this.GcTestResultDetail.Size = new System.Drawing.Size(725, 390);
            this.GcTestResultDetail.TabIndex = 14;
            this.GcTestResultDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvTestResultDetail});
            // 
            // GvTestResultDetail
            // 
            this.GvTestResultDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5});
            this.GvTestResultDetail.DetailHeight = 272;
            this.GvTestResultDetail.GridControl = this.GcTestResultDetail;
            this.GvTestResultDetail.Name = "GvTestResultDetail";
            this.GvTestResultDetail.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Number";
            this.gridColumn1.FieldName = "Index";
            this.gridColumn1.MinWidth = 22;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 82;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "X(μm)";
            this.gridColumn2.FieldName = "XResult";
            this.gridColumn2.MinWidth = 22;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 82;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Y(μm)";
            this.gridColumn3.FieldName = "YResult";
            this.gridColumn3.MinWidth = 22;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 82;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Theta(°)";
            this.gridColumn4.FieldName = "AngleResult";
            this.gridColumn4.MinWidth = 22;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 82;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "CreateTime";
            this.gridColumn5.FieldName = "CreateTime";
            this.gridColumn5.MinWidth = 22;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 82;
            // 
            // BtnExport
            // 
            this.BtnExport.Location = new System.Drawing.Point(291, 422);
            this.BtnExport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(116, 30);
            this.BtnExport.TabIndex = 15;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // FrmBMCTestResultDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 489);
            this.Controls.Add(this.BtnExport);
            this.Controls.Add(this.GcTestResultDetail);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmBMCTestResultDetail";
            this.Text = "FrmBMCTestResultDetail";
            ((System.ComponentModel.ISupportInitialize)(this.GcTestResultDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTestResultDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GcTestResultDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView GvTestResultDetail;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton BtnExport;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}