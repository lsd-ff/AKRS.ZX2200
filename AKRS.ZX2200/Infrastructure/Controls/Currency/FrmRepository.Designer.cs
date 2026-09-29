namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    partial class FrmRepository<T>
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
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.BarBtCreateNew = new DevExpress.XtraBars.BarButtonItem();
            this.BarBtDelete = new DevExpress.XtraBars.BarButtonItem();
            this.BarBtSave = new DevExpress.XtraBars.BarButtonItem();
            this.BarBtExtract = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.ChkIncludeAll = new DevExpress.XtraEditors.CheckEdit();
            this.TxtName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.GcRepository = new DevExpress.XtraGrid.GridControl();
            this.BsRepository = new System.Windows.Forms.BindingSource(this.components);
            this.GvRepository = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.Timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIncludeAll.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvRepository)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BarBtCreateNew,
            this.BarBtDelete,
            this.BarBtSave,
            this.BarBtExtract});
            this.barManager1.MaxItemId = 4;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BarBtCreateNew),
            new DevExpress.XtraBars.LinkPersistInfo(this.BarBtDelete, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.BarBtSave, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.BarBtExtract, true)});
            this.bar1.Text = "Tools";
            // 
            // BarBtCreateNew
            // 
            this.BarBtCreateNew.Caption = "新建(复制选中的)";
            this.BarBtCreateNew.Id = 0;
            this.BarBtCreateNew.Name = "BarBtCreateNew";
            this.BarBtCreateNew.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BarBtCreateNew.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtCreateNew_ItemClick);
            // 
            // BarBtDelete
            // 
            this.BarBtDelete.Caption = "删除";
            this.BarBtDelete.Id = 1;
            this.BarBtDelete.Name = "BarBtDelete";
            this.BarBtDelete.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BarBtDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtDelete_ItemClick);
            // 
            // BarBtSave
            // 
            this.BarBtSave.Caption = "保存";
            this.BarBtSave.Id = 2;
            this.BarBtSave.Name = "BarBtSave";
            this.BarBtSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BarBtSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtSave_ItemClick);
            // 
            // BarBtExtract
            // 
            this.BarBtExtract.Caption = "导入";
            this.BarBtExtract.Hint = "Extract dataset to this recipe";
            this.BarBtExtract.Id = 3;
            this.BarBtExtract.Name = "BarBtExtract";
            this.BarBtExtract.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BarBtExtract.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtExtract_ItemClick);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(904, 21);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 578);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(904, 20);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 21);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 557);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(904, 21);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 557);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Horizontal = false;
            this.splitContainerControl1.IsSplitterFixed = true;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 21);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.ChkIncludeAll);
            this.splitContainerControl1.Panel1.Controls.Add(this.TxtName);
            this.splitContainerControl1.Panel1.Controls.Add(this.labelControl1);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.GcRepository);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(904, 557);
            this.splitContainerControl1.SplitterPosition = 71;
            this.splitContainerControl1.TabIndex = 4;
            // 
            // ChkIncludeAll
            // 
            this.ChkIncludeAll.Location = new System.Drawing.Point(387, 30);
            this.ChkIncludeAll.MenuManager = this.barManager1;
            this.ChkIncludeAll.Name = "ChkIncludeAll";
            this.ChkIncludeAll.Properties.Caption = "包含所有";
            this.ChkIncludeAll.Size = new System.Drawing.Size(120, 20);
            this.ChkIncludeAll.TabIndex = 2;
            this.ChkIncludeAll.CheckedChanged += new System.EventHandler(this.ChkIncludeAll_CheckedChanged);
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(125, 30);
            this.TxtName.MenuManager = this.barManager1;
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(200, 20);
            this.TxtName.TabIndex = 1;
            this.TxtName.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.TxtName_EditValueChanging);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(82, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "搜索";
            // 
            // GcRepository
            // 
            this.GcRepository.DataSource = this.BsRepository;
            this.GcRepository.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcRepository.Location = new System.Drawing.Point(0, 0);
            this.GcRepository.MainView = this.GvRepository;
            this.GcRepository.MenuManager = this.barManager1;
            this.GcRepository.Name = "GcRepository";
            this.GcRepository.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1});
            this.GcRepository.Size = new System.Drawing.Size(904, 476);
            this.GcRepository.TabIndex = 0;
            this.GcRepository.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvRepository});
            // 
            // GvRepository
            // 
            this.GvRepository.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn3,
            this.gridColumn2});
            this.GvRepository.GridControl = this.GcRepository;
            this.GvRepository.Name = "GvRepository";
            this.GvRepository.OptionsDetail.EnableMasterViewMode = false;
            this.GvRepository.OptionsView.ShowGroupPanel = false;
            this.GvRepository.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GvRepository_RowClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "名字";
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 495;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "创建时间";
            this.gridColumn3.FieldName = "CreateTime";
            this.gridColumn3.MinWidth = 22;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 174;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "是否启动";
            this.gridColumn2.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn2.FieldName = "IsEnable";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 208;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // Timer1
            // 
            this.Timer1.Enabled = true;
            this.Timer1.Tag = "FrmRepository";
            this.Timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmRepository
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 598);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FrmRepository";
            this.Text = "Dataset";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRepository_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmRepository_FormClosed);
            this.Load += new System.EventHandler(this.FrmRepository_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            this.splitContainerControl1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChkIncludeAll.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvRepository)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem BarBtCreateNew;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.TextEdit TxtName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.GridControl GcRepository;
        private DevExpress.XtraGrid.Views.Grid.GridView GvRepository;
        private DevExpress.XtraEditors.CheckEdit ChkIncludeAll;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarButtonItem BarBtDelete;
        private DevExpress.XtraBars.BarButtonItem BarBtSave;
        private System.Windows.Forms.BindingSource BsRepository;
        private DevExpress.XtraBars.BarButtonItem BarBtExtract;
        private System.Windows.Forms.Timer Timer1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}