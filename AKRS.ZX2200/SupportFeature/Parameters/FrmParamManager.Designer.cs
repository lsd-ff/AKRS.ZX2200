namespace AKRS.ZX2200.SupportFeature.Parameters
{
    partial class FrmParamManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmParamManager));
            this.TLParamGroupView = new DevExpress.XtraTreeList.TreeList();
            this.TLParamView = new DevExpress.XtraTreeList.TreeList();
            this.PcEditorView = new DevExpress.XtraEditors.PanelControl();
            this.ListBxDataSet = new DevExpress.XtraEditors.ListBoxControl();
            this.BarManagerMain = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.TxtParamNameToSearch = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.LblSearchTip = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.BarTxtParamRange = new DevExpress.XtraBars.BarHeaderItem();
            ((System.ComponentModel.ISupportInitialize)(this.TLParamGroupView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TLParamView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PcEditorView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListBxDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManagerMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // TLParamGroupView
            // 
            this.TLParamGroupView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLParamGroupView.Location = new System.Drawing.Point(0, 0);
            this.TLParamGroupView.Name = "TLParamGroupView";
            this.TLParamGroupView.Size = new System.Drawing.Size(232, 425);
            this.TLParamGroupView.TabIndex = 0;
            this.TLParamGroupView.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TLParamGroupView_FocusedNodeChanged);
            // 
            // TLParamView
            // 
            this.TLParamView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TLParamView.Location = new System.Drawing.Point(0, 0);
            this.TLParamView.Name = "TLParamView";
            this.TLParamView.Size = new System.Drawing.Size(602, 655);
            this.TLParamView.TabIndex = 0;
            this.TLParamView.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TLParamView_FocusedNodeChanged);
            // 
            // PcEditorView
            // 
            this.PcEditorView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.PcEditorView.Dock = System.Windows.Forms.DockStyle.Top;
            this.PcEditorView.Location = new System.Drawing.Point(0, 0);
            this.PcEditorView.Name = "PcEditorView";
            this.PcEditorView.Size = new System.Drawing.Size(231, 146);
            this.PcEditorView.TabIndex = 2;
            // 
            // ListBxDataSet
            // 
            this.ListBxDataSet.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ListBxDataSet.Location = new System.Drawing.Point(0, 425);
            this.ListBxDataSet.Name = "ListBxDataSet";
            this.ListBxDataSet.Size = new System.Drawing.Size(232, 230);
            this.ListBxDataSet.TabIndex = 6;
            this.ListBxDataSet.SelectedIndexChanged += new System.EventHandler(this.ListBxDataSet_SelectedIndexChanged);
            // 
            // BarManagerMain
            // 
            this.BarManagerMain.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar3,
            this.bar1});
            this.BarManagerMain.DockControls.Add(this.barDockControlTop);
            this.BarManagerMain.DockControls.Add(this.barDockControlBottom);
            this.BarManagerMain.DockControls.Add(this.barDockControlLeft);
            this.BarManagerMain.DockControls.Add(this.barDockControlRight);
            this.BarManagerMain.Form = this;
            this.BarManagerMain.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.TxtParamNameToSearch,
            this.barButtonItem1,
            this.LblSearchTip,
            this.barStaticItem2,
            this.BarTxtParamRange});
            this.BarManagerMain.MaxItemId = 6;
            this.BarManagerMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1});
            this.BarManagerMain.StatusBar = this.bar3;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.BarManagerMain;
            this.barDockControlTop.Size = new System.Drawing.Size(1065, 24);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 679);
            this.barDockControlBottom.Manager = this.BarManagerMain;
            this.barDockControlBottom.Size = new System.Drawing.Size(1065, 23);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 24);
            this.barDockControlLeft.Manager = this.BarManagerMain;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 655);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1065, 24);
            this.barDockControlRight.Manager = this.BarManagerMain;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 655);
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BarTxtParamRange)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.TLParamGroupView);
            this.panelControl1.Controls.Add(this.ListBxDataSet);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 24);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(232, 655);
            this.panelControl1.TabIndex = 11;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 3";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.TxtParamNameToSearch),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.LblSearchTip)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawBorder = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Custom 3";
            // 
            // TxtParamNameToSearch
            // 
            this.TxtParamNameToSearch.Caption = "barEditItem1";
            this.TxtParamNameToSearch.Edit = this.repositoryItemTextEdit1;
            this.TxtParamNameToSearch.EditWidth = 200;
            this.TxtParamNameToSearch.Id = 0;
            this.TxtParamNameToSearch.Name = "TxtParamNameToSearch";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "搜索";
            this.barButtonItem1.Id = 1;
            this.barButtonItem1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonItem1.ImageOptions.SvgImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // LblSearchTip
            // 
            this.LblSearchTip.Caption = "匹配 0/0";
            this.LblSearchTip.Id = 3;
            this.LblSearchTip.Name = "LblSearchTip";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Caption = "参数名称";
            this.barStaticItem2.Id = 4;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.TLParamView);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(232, 24);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(602, 655);
            this.panelControl2.TabIndex = 12;
            // 
            // panelControl3
            // 
            this.panelControl3.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl3.Controls.Add(this.PcEditorView);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl3.Location = new System.Drawing.Point(834, 24);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(231, 655);
            this.panelControl3.TabIndex = 13;
            // 
            // BarTxtParamRange
            // 
            this.BarTxtParamRange.Caption = "设定范围：";
            this.BarTxtParamRange.Id = 5;
            this.BarTxtParamRange.Name = "BarTxtParamRange";
            // 
            // FrmParamManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1065, 702);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("FrmParamManager.IconOptions.SvgImage")));
            this.Name = "FrmParamManager";
            this.Text = "参数编辑界面";
            this.Load += new System.EventHandler(this.FrmParamManager_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TLParamGroupView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TLParamView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PcEditorView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListBxDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BarManagerMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList TLParamGroupView;
        private DevExpress.XtraTreeList.TreeList TLParamView;
        private DevExpress.XtraEditors.PanelControl PcEditorView;
        private DevExpress.XtraEditors.ListBoxControl ListBxDataSet;
        private DevExpress.XtraBars.BarManager BarManagerMain;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.BarEditItem TxtParamNameToSearch;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarStaticItem barStaticItem2;
        private DevExpress.XtraBars.BarStaticItem LblSearchTip;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraBars.BarHeaderItem BarTxtParamRange;
    }
}