namespace AKRS.ZX2200.Main.Controls.Ucmain
{
    partial class UcHardwareEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcHardwareEditor));
            this.SpcHardwareEditor = new DevExpress.XtraEditors.SplitContainerControl();
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.BtSave = new DevExpress.XtraBars.BarButtonItem();
            this.BarBtShowPageHeader = new DevExpress.XtraBars.BarButtonItem();
            this.BarBtHidePageHeader = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor.Panel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor.Panel2)).BeginInit();
            this.SpcHardwareEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // SpcHardwareEditor
            // 
            this.SpcHardwareEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpcHardwareEditor.Location = new System.Drawing.Point(0, 152);
            this.SpcHardwareEditor.Margin = new System.Windows.Forms.Padding(2);
            this.SpcHardwareEditor.Name = "SpcHardwareEditor";
            // 
            // SpcHardwareEditor.Panel1
            // 
            this.SpcHardwareEditor.Panel1.Text = "Panel1";
            // 
            // SpcHardwareEditor.Panel2
            // 
            this.SpcHardwareEditor.Panel2.Text = "Panel2";
            this.SpcHardwareEditor.Size = new System.Drawing.Size(1241, 618);
            this.SpcHardwareEditor.SplitterPosition = 620;
            this.SpcHardwareEditor.TabIndex = 0;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.BtSave,
            this.BarBtShowPageHeader,
            this.BarBtHidePageHeader});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.PageHeaderItemLinks.Add(this.BarBtShowPageHeader);
            this.ribbonControl1.PageHeaderItemLinks.Add(this.BarBtHidePageHeader);
            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1});
            this.ribbonControl1.Size = new System.Drawing.Size(1241, 152);
            // 
            // BtSave
            // 
            this.BtSave.Caption = "Save";
            this.BtSave.Id = 1;
            this.BtSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtSave.ImageOptions.Image")));
            this.BtSave.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BtSave.ImageOptions.LargeImage")));
            this.BtSave.Name = "BtSave";
            this.BtSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtSave_ItemClick);
            // 
            // BarBtShowPageHeader
            // 
            this.BarBtShowPageHeader.Caption = "Show Page Header";
            this.BarBtShowPageHeader.Id = 2;
            this.BarBtShowPageHeader.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BarBtShowPageHeader.ImageOptions.Image")));
            this.BarBtShowPageHeader.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BarBtShowPageHeader.ImageOptions.LargeImage")));
            this.BarBtShowPageHeader.Name = "BarBtShowPageHeader";
            this.BarBtShowPageHeader.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtShowPageHeader_ItemClick);
            // 
            // BarBtHidePageHeader
            // 
            this.BarBtHidePageHeader.Caption = "Hide Page Header";
            this.BarBtHidePageHeader.Id = 3;
            this.BarBtHidePageHeader.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BarBtHidePageHeader.ImageOptions.Image")));
            this.BarBtHidePageHeader.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("BarBtHidePageHeader.ImageOptions.LargeImage")));
            this.BarBtHidePageHeader.Name = "BarBtHidePageHeader";
            this.BarBtHidePageHeader.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.BarBtHidePageHeader.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarBtHidePageHeader_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Hardware";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.BtSave);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            // 
            // UcHardwareEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SpcHardwareEditor);
            this.Controls.Add(this.ribbonControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UcHardwareEditor";
            this.Size = new System.Drawing.Size(1241, 770);
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor.Panel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor.Panel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpcHardwareEditor)).EndInit();
            this.SpcHardwareEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl SpcHardwareEditor;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.BarButtonItem BtSave;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.BarButtonItem BarBtShowPageHeader;
        private DevExpress.XtraBars.BarButtonItem BarBtHidePageHeader;
    }
}
