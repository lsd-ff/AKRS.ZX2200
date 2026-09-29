namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

partial class FrmCompensationRegionView
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
            this.GcCompensationRegion = new DevExpress.XtraGrid.GridControl();
            this.GvCompensationRegion = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GcCompensationRegion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvCompensationRegion)).BeginInit();
            this.SuspendLayout();
            // 
            // GcCompensationRegion
            // 
            this.GcCompensationRegion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcCompensationRegion.Location = new System.Drawing.Point(0, 0);
            this.GcCompensationRegion.MainView = this.GvCompensationRegion;
            this.GcCompensationRegion.Name = "GcCompensationRegion";
            this.GcCompensationRegion.Size = new System.Drawing.Size(528, 481);
            this.GcCompensationRegion.TabIndex = 0;
            this.GcCompensationRegion.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvCompensationRegion});
            // 
            // GvCompensationRegion
            // 
            this.GvCompensationRegion.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.GvCompensationRegion.GridControl = this.GcCompensationRegion;
            this.GvCompensationRegion.Name = "GvCompensationRegion";
            this.GvCompensationRegion.OptionsBehavior.Editable = false;
            this.GvCompensationRegion.OptionsBehavior.ReadOnly = true;
            this.GvCompensationRegion.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "轴点位";
            this.gridColumn1.FieldName = "AxisPos";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "补偿值";
            this.gridColumn2.FieldName = "Compensation";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // FrmCompensationRegionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 481);
            this.Controls.Add(this.GcCompensationRegion);
            this.Name = "FrmCompensationRegionView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "补偿区域映射表";
            this.Load += new System.EventHandler(this.FrmCompensationRegionView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GcCompensationRegion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvCompensationRegion)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraGrid.GridControl GcCompensationRegion;
    private DevExpress.XtraGrid.Views.Grid.GridView GvCompensationRegion;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
}