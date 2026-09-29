namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    partial class FrmNewCreatePostBond
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.TxtName = new DevExpress.XtraEditors.TextEdit();
            this.TabControl = new DevExpress.XtraTab.XtraTabControl();
            this.TpDataset = new DevExpress.XtraTab.XtraTabPage();
            this.GcDataset = new DevExpress.XtraGrid.GridControl();
            this.GvDataset = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TpRepository = new DevExpress.XtraTab.XtraTabPage();
            this.GcRepository = new DevExpress.XtraGrid.GridControl();
            this.GvRepository = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl)).BeginInit();
            this.TabControl.SuspendLayout();
            this.TpDataset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcDataset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvDataset)).BeginInit();
            this.TpRepository.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.TxtName);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(445, 87);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "焊后检测名称";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(45, 42);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(350, 24);
            this.TxtName.TabIndex = 1;
            this.TxtName.EditValueChanged += new System.EventHandler(this.TxtName_EditValueChanged);
            // 
            // TabControl
            // 
            this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TabControl.Location = new System.Drawing.Point(0, 87);
            this.TabControl.Name = "TabControl";
            this.TabControl.SelectedTabPage = this.TpDataset;
            this.TabControl.Size = new System.Drawing.Size(445, 695);
            this.TabControl.TabIndex = 5;
            this.TabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TpDataset,
            this.TpRepository});
            // 
            // TpDataset
            // 
            this.TpDataset.Controls.Add(this.GcDataset);
            this.TpDataset.Name = "TpDataset";
            this.TpDataset.Size = new System.Drawing.Size(443, 663);
            this.TpDataset.Text = "Dataset";
            // 
            // GcDataset
            // 
            this.GcDataset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcDataset.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcDataset.Location = new System.Drawing.Point(0, 0);
            this.GcDataset.MainView = this.GvDataset;
            this.GcDataset.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcDataset.Name = "GcDataset";
            this.GcDataset.Size = new System.Drawing.Size(443, 663);
            this.GcDataset.TabIndex = 0;
            this.GcDataset.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvDataset});
            // 
            // GvDataset
            // 
            this.GvDataset.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.GvDataset.DetailHeight = 450;
            this.GvDataset.GridControl = this.GcDataset;
            this.GvDataset.Name = "GvDataset";
            this.GvDataset.OptionsDetail.EnableMasterViewMode = false;
            this.GvDataset.OptionsView.ShowColumnHeaders = false;
            this.GvDataset.OptionsView.ShowGroupPanel = false;
            this.GvDataset.OptionsView.ShowIndicator = false;
            this.GvDataset.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
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
            // TpRepository
            // 
            this.TpRepository.Controls.Add(this.GcRepository);
            this.TpRepository.Name = "TpRepository";
            this.TpRepository.Size = new System.Drawing.Size(443, 663);
            this.TpRepository.Text = "Repository";
            // 
            // GcRepository
            // 
            this.GcRepository.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcRepository.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcRepository.Location = new System.Drawing.Point(0, 0);
            this.GcRepository.MainView = this.GvRepository;
            this.GcRepository.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcRepository.Name = "GcRepository";
            this.GcRepository.Size = new System.Drawing.Size(443, 663);
            this.GcRepository.TabIndex = 1;
            this.GcRepository.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvRepository});
            // 
            // GvRepository
            // 
            this.GvRepository.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn2});
            this.GvRepository.DetailHeight = 450;
            this.GvRepository.GridControl = this.GcRepository;
            this.GvRepository.Name = "GvRepository";
            this.GvRepository.OptionsDetail.EnableMasterViewMode = false;
            this.GvRepository.OptionsView.ShowColumnHeaders = false;
            this.GvRepository.OptionsView.ShowGroupPanel = false;
            this.GvRepository.OptionsView.ShowIndicator = false;
            this.GvRepository.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn1";
            this.gridColumn2.FieldName = "Name";
            this.gridColumn2.MinWidth = 23;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 200;
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 720);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(445, 62);
            this.tablePanel1.TabIndex = 23;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 1);
            this.BtCancel.Location = new System.Drawing.Point(261, 12);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(147, 37);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtnGenerate
            // 
            this.tablePanel1.SetColumn(this.BtnGenerate, 0);
            this.BtnGenerate.Location = new System.Drawing.Point(38, 12);
            this.BtnGenerate.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtnGenerate.Name = "BtnGenerate";
            this.tablePanel1.SetRow(this.BtnGenerate, 0);
            this.BtnGenerate.Size = new System.Drawing.Size(147, 37);
            this.BtnGenerate.TabIndex = 6;
            this.BtnGenerate.Text = "确定";
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // FrmNewCreatePostBond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 782);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.TabControl);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmNewCreatePostBond";
            this.Text = "新建焊后检测";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TabControl)).EndInit();
            this.TabControl.ResumeLayout(false);
            this.TpDataset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcDataset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvDataset)).EndInit();
            this.TpRepository.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit TxtName;
        private DevExpress.XtraTab.XtraTabControl TabControl;
        private DevExpress.XtraTab.XtraTabPage TpDataset;
        private DevExpress.XtraGrid.GridControl GcDataset;
        private DevExpress.XtraGrid.Views.Grid.GridView GvDataset;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraTab.XtraTabPage TpRepository;
        private DevExpress.XtraGrid.GridControl GcRepository;
        private DevExpress.XtraGrid.Views.Grid.GridView GvRepository;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
    }
}