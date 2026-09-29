namespace AKRS.ZX2200.SupportFeature.Statistics
{
    partial class UcBondPositionInfo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.DeQurtyEndTime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            this.DeQurtyStartTime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.BtQuery = new DevExpress.XtraEditors.SimpleButton();
            this.BtExportData = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.TxName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeQurtyEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeQurtyStartTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.TxName);
            this.panelControl3.Controls.Add(this.DeQurtyEndTime);
            this.panelControl3.Controls.Add(this.DeQurtyStartTime);
            this.panelControl3.Controls.Add(this.labelControl3);
            this.panelControl3.Controls.Add(this.BtQuery);
            this.panelControl3.Controls.Add(this.BtExportData);
            this.panelControl3.Controls.Add(this.labelControl6);
            this.panelControl3.Controls.Add(this.labelControl7);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(1062, 68);
            this.panelControl3.TabIndex = 10;
            // 
            // DeQurtyEndTime
            // 
            this.DeQurtyEndTime.EditValue = null;
            this.DeQurtyEndTime.Location = new System.Drawing.Point(371, 17);
            this.DeQurtyEndTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DeQurtyEndTime.Name = "DeQurtyEndTime";
            this.DeQurtyEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeQurtyEndTime.Properties.MaskSettings.Set("mask", "f");
            this.DeQurtyEndTime.Size = new System.Drawing.Size(167, 20);
            this.DeQurtyEndTime.TabIndex = 20;
            // 
            // DeQurtyStartTime
            // 
            this.DeQurtyStartTime.EditValue = null;
            this.DeQurtyStartTime.Location = new System.Drawing.Point(69, 17);
            this.DeQurtyStartTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DeQurtyStartTime.Name = "DeQurtyStartTime";
            this.DeQurtyStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DeQurtyStartTime.Properties.MaskSettings.Set("mask", "f");
            this.DeQurtyStartTime.Size = new System.Drawing.Size(195, 20);
            this.DeQurtyStartTime.TabIndex = 19;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(578, 20);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "焊点名称";
            // 
            // BtQuery
            // 
            this.BtQuery.Location = new System.Drawing.Point(846, 16);
            this.BtQuery.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtQuery.Name = "BtQuery";
            this.BtQuery.Size = new System.Drawing.Size(82, 23);
            this.BtQuery.TabIndex = 15;
            this.BtQuery.Text = "查询";
            this.BtQuery.Click += new System.EventHandler(this.BtQuery_Click);
            // 
            // BtExportData
            // 
            this.BtExportData.Location = new System.Drawing.Point(934, 16);
            this.BtExportData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtExportData.Name = "BtExportData";
            this.BtExportData.Size = new System.Drawing.Size(82, 23);
            this.BtExportData.TabIndex = 11;
            this.BtExportData.Text = "导出";
            this.BtExportData.Click += new System.EventHandler(this.BtExport_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(5, 20);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(48, 14);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "开始时间";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(308, 20);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(48, 14);
            this.labelControl7.TabIndex = 7;
            this.labelControl7.Text = "结束时间";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 68);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1062, 535);
            this.gridControl1.TabIndex = 11;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // TxName
            // 
            this.TxName.Location = new System.Drawing.Point(632, 17);
            this.TxName.Name = "TxName";
            this.TxName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxName.Properties.Appearance.Options.UseFont = true;
            this.TxName.Size = new System.Drawing.Size(126, 20);
            this.TxName.TabIndex = 45;
            // 
            // UcBondPositionInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.panelControl3);
            this.Name = "UcBondPositionInfo";
            this.Size = new System.Drawing.Size(1062, 603);
            this.Load += new System.EventHandler(this.UcBondPositionInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DeQurtyEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DeQurtyStartTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.DateTimeOffsetEdit DeQurtyEndTime;
        private DevExpress.XtraEditors.DateTimeOffsetEdit DeQurtyStartTime;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton BtQuery;
        private DevExpress.XtraEditors.SimpleButton BtExportData;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.TextEdit TxName;
    }
}
