namespace AKRS.ZX2200.BondSystem.Controls.Setting.ProcessStep
{
    partial class FrmCreateProcessStep
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
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GcTool = new DevExpress.XtraGrid.GridControl();
            this.GvEpoxyApplication = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.TxtName = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEpoxyApplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnGenerate
            // 
            this.tablePanel1.SetColumn(this.BtnGenerate, 0);
            this.BtnGenerate.Location = new System.Drawing.Point(38, 12);
            this.BtnGenerate.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtnGenerate.Name = "BtnGenerate";
            this.tablePanel1.SetRow(this.BtnGenerate, 0);
            this.BtnGenerate.Size = new System.Drawing.Size(152, 37);
            this.BtnGenerate.TabIndex = 6;
            this.BtnGenerate.Text = "确认";
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 1);
            this.BtCancel.Location = new System.Drawing.Point(266, 12);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(152, 37);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Controls.Add(this.BtnGenerate);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 712);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(455, 62);
            this.tablePanel1.TabIndex = 26;
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GcTool);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(453, 594);
            this.xtraTabPage1.Text = "Dataset";
            // 
            // GcTool
            // 
            this.GcTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcTool.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcTool.Location = new System.Drawing.Point(0, 0);
            this.GcTool.MainView = this.GvEpoxyApplication;
            this.GcTool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcTool.Name = "GcTool";
            this.GcTool.Size = new System.Drawing.Size(453, 594);
            this.GcTool.TabIndex = 0;
            this.GcTool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvEpoxyApplication,
            this.gridView1});
            // 
            // GvEpoxyApplication
            // 
            this.GvEpoxyApplication.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.GvEpoxyApplication.DetailHeight = 450;
            this.GvEpoxyApplication.GridControl = this.GcTool;
            this.GvEpoxyApplication.Name = "GvEpoxyApplication";
            this.GvEpoxyApplication.OptionsDetail.EnableMasterViewMode = false;
            this.GvEpoxyApplication.OptionsView.ShowColumnHeaders = false;
            this.GvEpoxyApplication.OptionsView.ShowGroupPanel = false;
            this.GvEpoxyApplication.OptionsView.ShowIndicator = false;
            this.GvEpoxyApplication.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.MinWidth = 23;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 200;
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 252;
            this.gridView1.GridControl = this.GcTool;
            this.gridView1.Name = "gridView1";
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 86);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(455, 626);
            this.xtraTabControl1.TabIndex = 25;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.TxtName);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(455, 86);
            this.groupControl1.TabIndex = 27;
            this.groupControl1.Text = "名称";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(45, 42);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(350, 24);
            this.TxtName.TabIndex = 1;
            // 
            // FrmCreateProcessStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 774);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmCreateProcessStep";
            this.Text = "新建流程";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEpoxyApplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl GcTool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvEpoxyApplication;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit TxtName;
    }
}