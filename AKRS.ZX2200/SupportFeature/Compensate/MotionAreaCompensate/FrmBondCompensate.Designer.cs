namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    partial class FrmBondCompensate
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
            this.BtInput = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.GcCompensateData = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtClear = new DevExpress.XtraEditors.SimpleButton();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcCompensateData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // BtInput
            // 
            this.BtInput.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtInput.Appearance.Options.UseFont = true;
            this.BtInput.Location = new System.Drawing.Point(771, 663);
            this.BtInput.Name = "BtInput";
            this.BtInput.Size = new System.Drawing.Size(170, 98);
            this.BtInput.TabIndex = 0;
            this.BtInput.Text = "导入";
            this.BtInput.Click += new System.EventHandler(this.BtInput_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.GcCompensateData);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(953, 657);
            this.panelControl1.TabIndex = 1;
            // 
            // GcCompensateData
            // 
            this.GcCompensateData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcCompensateData.Location = new System.Drawing.Point(2, 2);
            this.GcCompensateData.MainView = this.gridView1;
            this.GcCompensateData.Name = "GcCompensateData";
            this.GcCompensateData.Size = new System.Drawing.Size(949, 653);
            this.GcCompensateData.TabIndex = 0;
            this.GcCompensateData.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gridView1.GridControl = this.GcCompensateData;
            this.gridView1.IndicatorWidth = 50;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "补偿X";
            this.gridColumn1.FieldName = "item1";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "补偿Y";
            this.gridColumn2.FieldName = "item2";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // BtClear
            // 
            this.BtClear.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtClear.Appearance.Options.UseFont = true;
            this.BtClear.Location = new System.Drawing.Point(12, 663);
            this.BtClear.Name = "BtClear";
            this.BtClear.Size = new System.Drawing.Size(170, 98);
            this.BtClear.TabIndex = 2;
            this.BtClear.Text = "清空数据";
            this.BtClear.Click += new System.EventHandler(this.BtClear_Click);
            // 
            // BtSave
            // 
            this.BtSave.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtSave.Appearance.Options.UseFont = true;
            this.BtSave.Location = new System.Drawing.Point(357, 663);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(170, 98);
            this.BtSave.TabIndex = 3;
            this.BtSave.Text = "保存";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // FrmBondCompensate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 773);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.BtClear);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.BtInput);
            this.Name = "FrmBondCompensate";
            this.Text = "位置补偿列表";
            this.Load += new System.EventHandler(this.FrmBondCompensate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcCompensateData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtInput;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtClear;
        private DevExpress.XtraGrid.GridControl GcCompensateData;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton BtSave;
    }
}