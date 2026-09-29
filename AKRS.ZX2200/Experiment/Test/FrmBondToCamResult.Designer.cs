namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmBondToCamResult
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
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bondX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bondY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upLookX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.upLookY = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DifferenceX = new DevExpress.XtraGrid.Columns.GridColumn();
            this.DifferenceY = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1079, 432);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.bondX,
            this.bondY,
            this.upLookX,
            this.upLookY,
            this.DifferenceX,
            this.DifferenceY});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // bondX
            // 
            this.bondX.Caption = "bondX";
            this.bondX.FieldName = "bondX";
            this.bondX.Name = "bondX";
            this.bondX.Visible = true;
            this.bondX.VisibleIndex = 0;
            // 
            // bondY
            // 
            this.bondY.Caption = "bondY";
            this.bondY.FieldName = "bondY";
            this.bondY.Name = "bondY";
            this.bondY.Visible = true;
            this.bondY.VisibleIndex = 1;
            // 
            // upLookX
            // 
            this.upLookX.Caption = "upLookX";
            this.upLookX.FieldName = "upLookX";
            this.upLookX.Name = "upLookX";
            this.upLookX.Visible = true;
            this.upLookX.VisibleIndex = 2;
            // 
            // upLookY
            // 
            this.upLookY.Caption = "upLookY";
            this.upLookY.FieldName = "upLookY";
            this.upLookY.Name = "upLookY";
            this.upLookY.Visible = true;
            this.upLookY.VisibleIndex = 3;
            // 
            // DifferenceX
            // 
            this.DifferenceX.Caption = "DifferenceX";
            this.DifferenceX.FieldName = "DifferenceX";
            this.DifferenceX.Name = "DifferenceX";
            this.DifferenceX.Visible = true;
            this.DifferenceX.VisibleIndex = 4;
            // 
            // DifferenceY
            // 
            this.DifferenceY.Caption = "DifferenceY";
            this.DifferenceY.FieldName = "DifferenceY";
            this.DifferenceY.Name = "DifferenceY";
            this.DifferenceY.Visible = true;
            this.DifferenceY.VisibleIndex = 5;
            // 
            // FrmBondToCamResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1079, 432);
            this.Controls.Add(this.gridControl1);
            this.Name = "FrmBondToCamResult";
            this.Text = "FrmBondToCamResult";
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn bondX;
        private DevExpress.XtraGrid.Columns.GridColumn bondY;
        private DevExpress.XtraGrid.Columns.GridColumn upLookX;
        private DevExpress.XtraGrid.Columns.GridColumn upLookY;
        private DevExpress.XtraGrid.Columns.GridColumn DifferenceX;
        private DevExpress.XtraGrid.Columns.GridColumn DifferenceY;
    }
}