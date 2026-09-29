namespace AKRS.Galaxy2.PR.Resipository
{
    partial class FrmPRList
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPRList));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtOk = new DevExpress.XtraEditors.SimpleButton();
            this.GcPRTemps = new DevExpress.XtraGrid.GridControl();
            this.BsPRList = new System.Windows.Forms.BindingSource(this.components);
            this.GvPRTemps = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gc2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.BtAddPR = new DevExpress.XtraBars.BarButtonItem();
            this.BtAddPRFromSelectedPR = new DevExpress.XtraBars.BarButtonItem();
            this.BtRemove = new DevExpress.XtraBars.BarButtonItem();
            this.BtSave = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.BtOther = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.BtAddSkyTemp = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcPRTemps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsPRList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPRTemps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Controls.Add(this.GcPRTemps);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 24);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(692, 649);
            this.panelControl1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.BtOk);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 600);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(688, 47);
            this.panelControl2.TabIndex = 5;
            this.panelControl2.Visible = false;
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(557, 7);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(101, 37);
            this.BtOk.TabIndex = 0;
            this.BtOk.Text = "确定";
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // GcPRTemps
            // 
            this.GcPRTemps.DataSource = this.BsPRList;
            this.GcPRTemps.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcPRTemps.Location = new System.Drawing.Point(2, 2);
            this.GcPRTemps.MainView = this.GvPRTemps;
            this.GcPRTemps.Name = "GcPRTemps";
            this.GcPRTemps.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemSpinEdit1});
            this.GcPRTemps.Size = new System.Drawing.Size(688, 598);
            this.GcPRTemps.TabIndex = 0;
            this.GcPRTemps.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvPRTemps});
            // 
            // GvPRTemps
            // 
            this.GvPRTemps.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gc2,
            this.gridColumn2});
            this.GvPRTemps.DetailHeight = 306;
            this.GvPRTemps.GridControl = this.GcPRTemps;
            this.GvPRTemps.Name = "GvPRTemps";
            this.GvPRTemps.OptionsView.ShowDetailButtons = false;
            this.GvPRTemps.OptionsView.ShowGroupPanel = false;
            this.GvPRTemps.OptionsView.ShowIndicator = false;
            this.GvPRTemps.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GvPRTemps_RowClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "模板名称";
            this.gridColumn1.FieldName = "Alg.Name";
            this.gridColumn1.MinWidth = 17;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.ReadOnly = true;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 222;
            // 
            // gc2
            // 
            this.gc2.Caption = "模板类型";
            this.gc2.FieldName = "Alg.AlgFlowType";
            this.gc2.Name = "gc2";
            this.gc2.OptionsColumn.AllowEdit = false;
            this.gc2.OptionsColumn.ReadOnly = true;
            this.gc2.Visible = true;
            this.gc2.VisibleIndex = 1;
            this.gc2.Width = 222;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "模板归属";
            this.gridColumn2.FieldName = "Alg.AlgBeLong";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 219;
            // 
            // repositoryItemSpinEdit1
            // 
            this.repositoryItemSpinEdit1.AutoHeight = false;
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
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
            this.BtAddPR,
            this.BtRemove,
            this.BtSave,
            this.BtOther,
            this.barButtonItem1,
            this.BtAddSkyTemp,
            this.BtAddPRFromSelectedPR,
            this.barButtonItem2});
            this.barManager1.MaxItemId = 11;
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BtAddPR),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtAddPRFromSelectedPR),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtRemove),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtSave),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Tools";
            // 
            // BtAddPR
            // 
            this.BtAddPR.Caption = "Add new template";
            this.BtAddPR.Id = 0;
            this.BtAddPR.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtAddPR.ImageOptions.Image")));
            this.BtAddPR.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtAddPR.ImageOptions.LargeImage")));
            this.BtAddPR.Name = "BtAddPR";
            this.BtAddPR.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BtAddPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.BtAddPR.VisibleInSearchMenu = false;
            this.BtAddPR.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtAddPR_ItemClick);
            // 
            // BtAddPRFromSelectedPR
            // 
            this.BtAddPRFromSelectedPR.Caption = "Create from old";
            this.BtAddPRFromSelectedPR.Id = 8;
            this.BtAddPRFromSelectedPR.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtAddPRFromSelectedPR.ImageOptions.Image")));
            this.BtAddPRFromSelectedPR.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtAddPRFromSelectedPR.ImageOptions.LargeImage")));
            this.BtAddPRFromSelectedPR.Name = "BtAddPRFromSelectedPR";
            this.BtAddPRFromSelectedPR.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BtAddPRFromSelectedPR.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.BtAddPRFromSelectedPR.VisibleInSearchMenu = false;
            this.BtAddPRFromSelectedPR.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtAddPRFromSelectedPR_ItemClick);
            // 
            // BtRemove
            // 
            this.BtRemove.Caption = "删除";
            this.BtRemove.Id = 1;
            this.BtRemove.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtRemove.ImageOptions.Image")));
            this.BtRemove.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtRemove.ImageOptions.LargeImage")));
            this.BtRemove.Name = "BtRemove";
            this.BtRemove.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BtRemove.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtRemove_ItemClick);
            // 
            // BtSave
            // 
            this.BtSave.Caption = "保存";
            this.BtSave.Id = 2;
            this.BtSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtSave.ImageOptions.Image")));
            this.BtSave.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtSave.ImageOptions.LargeImage")));
            this.BtSave.Name = "BtSave";
            this.BtSave.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BtSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtSave_ItemClick);
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Id = 10;
            this.barButtonItem2.Name = "barButtonItem2";
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
            this.barDockControlTop.Size = new System.Drawing.Size(692, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 673);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(692, 20);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 649);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(692, 24);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 649);
            // 
            // BtOther
            // 
            this.BtOther.Caption = "备用";
            this.BtOther.Id = 3;
            this.BtOther.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtOther.ImageOptions.Image")));
            this.BtOther.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtOther.ImageOptions.LargeImage")));
            this.BtOther.Name = "BtOther";
            this.BtOther.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.BtOther.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 5;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // BtAddSkyTemp
            // 
            this.BtAddSkyTemp.Id = 7;
            this.BtAddSkyTemp.Name = "BtAddSkyTemp";
            // 
            // FrmPRList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 693);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.MaximizeBox = false;
            this.Name = "FrmPRList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRList";
            this.Load += new System.EventHandler(this.FrmPRList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcPRTemps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsPRList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPRTemps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraGrid.GridControl GcPRTemps;
        private DevExpress.XtraGrid.Views.Grid.GridView GvPRTemps;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem BtAddPR;
        private DevExpress.XtraBars.BarButtonItem BtRemove;
        private DevExpress.XtraBars.BarButtonItem BtSave;
        private DevExpress.XtraBars.BarButtonItem BtOther;
        private System.Windows.Forms.BindingSource BsPRList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtOk;
        private DevExpress.XtraBars.BarButtonItem BtAddSkyTemp;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraGrid.Columns.GridColumn gc2;
        private DevExpress.XtraBars.BarButtonItem BtAddPRFromSelectedPR;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
    }
}