namespace AKRS.ZX2200.SupportFeature.Statistics
{
    partial class UcOperateLogStatistics
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.CbxOperate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtExport = new DevExpress.XtraEditors.SimpleButton();
            this.DoEndTime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            this.DoStartTime = new DevExpress.XtraEditors.DateTimeOffsetEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.BtQuery = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CbxOperate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DoEndTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DoStartTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.gridControl1);
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 90F)});
            this.tablePanel1.Size = new System.Drawing.Size(1046, 586);
            this.tablePanel1.TabIndex = 11;
            // 
            // gridControl1
            // 
            this.tablePanel1.SetColumn(this.gridControl1, 0);
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Location = new System.Drawing.Point(3, 61);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControl1.Name = "gridControl1";
            this.tablePanel1.SetRow(this.gridControl1, 1);
            this.gridControl1.Size = new System.Drawing.Size(1040, 523);
            this.gridControl1.TabIndex = 10;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.gridView1.DetailHeight = 272;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "时间";
            this.gridColumn1.FieldName = "DateTime";
            this.gridColumn1.MinWidth = 22;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 215;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "操作人员";
            this.gridColumn2.FieldName = "Technician";
            this.gridColumn2.MinWidth = 22;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 203;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "操作信息";
            this.gridColumn3.FieldName = "Message";
            this.gridColumn3.MinWidth = 22;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 511;
            // 
            // panelControl1
            // 
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.panelControl1.Controls.Add(this.BtQuery);
            this.panelControl1.Controls.Add(this.CbxOperate);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.BtExport);
            this.panelControl1.Controls.Add(this.DoEndTime);
            this.panelControl1.Controls.Add(this.DoStartTime);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 2);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 0);
            this.panelControl1.Size = new System.Drawing.Size(1040, 55);
            this.panelControl1.TabIndex = 9;
            // 
            // CbxOperate
            // 
            this.CbxOperate.Location = new System.Drawing.Point(618, 18);
            this.CbxOperate.Name = "CbxOperate";
            this.CbxOperate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CbxOperate.Size = new System.Drawing.Size(107, 20);
            this.CbxOperate.TabIndex = 12;
            this.CbxOperate.SelectedIndexChanged += new System.EventHandler(this.CbxOperate_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(557, 21);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "操作人";
            // 
            // BtExport
            // 
            this.BtExport.Location = new System.Drawing.Point(921, 18);
            this.BtExport.Name = "BtExport";
            this.BtExport.Size = new System.Drawing.Size(112, 23);
            this.BtExport.TabIndex = 10;
            this.BtExport.Text = "导出";
            this.BtExport.Click += new System.EventHandler(this.BtExport_Click);
            // 
            // DoEndTime
            // 
            this.DoEndTime.EditValue = null;
            this.DoEndTime.Location = new System.Drawing.Point(345, 18);
            this.DoEndTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DoEndTime.Name = "DoEndTime";
            this.DoEndTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DoEndTime.Size = new System.Drawing.Size(166, 20);
            this.DoEndTime.TabIndex = 9;
            this.DoEndTime.EditValueChanged += new System.EventHandler(this.DoEndTime_EditValueChanged);
            // 
            // DoStartTime
            // 
            this.DoStartTime.EditValue = null;
            this.DoStartTime.Location = new System.Drawing.Point(82, 19);
            this.DoStartTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DoStartTime.Name = "DoStartTime";
            this.DoStartTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.DoStartTime.Size = new System.Drawing.Size(162, 20);
            this.DoStartTime.TabIndex = 8;
            this.DoStartTime.EditValueChanged += new System.EventHandler(this.DoStartTime_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(274, 21);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "结束时间";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(12, 21);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(48, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "开始时间";
            // 
            // BtQuery
            // 
            this.BtQuery.Location = new System.Drawing.Point(770, 17);
            this.BtQuery.Name = "BtQuery";
            this.BtQuery.Size = new System.Drawing.Size(112, 23);
            this.BtQuery.TabIndex = 13;
            this.BtQuery.Text = "查询";
            this.BtQuery.Click += new System.EventHandler(this.BtQuery_Click);
            // 
            // UcOperateLogStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel1);
            this.Name = "UcOperateLogStatistics";
            this.Size = new System.Drawing.Size(1046, 586);
            this.Load += new System.EventHandler(this.UcOperateLogStatistics_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CbxOperate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DoEndTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DoStartTime.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtExport;
        private DevExpress.XtraEditors.DateTimeOffsetEdit DoEndTime;
        private DevExpress.XtraEditors.DateTimeOffsetEdit DoStartTime;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.ComboBoxEdit CbxOperate;
        private DevExpress.XtraEditors.SimpleButton BtQuery;
    }
}
