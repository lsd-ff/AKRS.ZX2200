namespace AKRS.ZX2200.Main.Controls
{
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (this.components != null)) {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions windowsUIButtonImageOptions1 = new DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.windowsUIView = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView(this.components);
            this.startGroup = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer(this.components);
            this.pageGroup = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup(this.components);
            this.UcMainSystemDocument = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document(this.components);
            this.UcUserManagerDocument = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document(this.components);
            this.UcHardareEditorDocument = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document(this.components);
            this.UcMainSystemTile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile(this.components);
            this.UcUserManagerTile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile(this.components);
            this.UcHardareEditorTile = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile(this.components);
            this.flyout = new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Flyout(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcMainSystemDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcUserManagerDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcHardareEditorDocument)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcMainSystemTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcUserManagerTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcHardareEditorTile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.flyout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.RightToLeftLayout = DevExpress.Utils.DefaultBoolean.False;
            this.documentManager1.ShowToolTips = DevExpress.Utils.DefaultBoolean.False;
            this.documentManager1.View = this.windowsUIView;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.windowsUIView});
            // 
            // windowsUIView
            // 
            this.windowsUIView.AppearanceActionsBar.BackColor = System.Drawing.Color.Red;
            this.windowsUIView.AppearanceActionsBar.Font = new System.Drawing.Font("黑体", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.windowsUIView.AppearanceActionsBar.Options.UseBackColor = true;
            this.windowsUIView.AppearanceActionsBar.Options.UseFont = true;
            this.windowsUIView.AppearanceActionsBarButton.BackColor = System.Drawing.Color.SlateGray;
            this.windowsUIView.AppearanceActionsBarButton.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.windowsUIView.AppearanceActionsBarButton.Options.UseBackColor = true;
            this.windowsUIView.AppearanceActionsBarButton.Options.UseFont = true;
            this.windowsUIView.AppearanceCaption.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.windowsUIView.AppearanceCaption.Options.UseFont = true;
            this.windowsUIView.Caption = "";
            this.windowsUIView.ContentContainers.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.IContentContainer[] {
            this.startGroup,
            this.pageGroup,
            this.flyout});
            this.windowsUIView.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.UcMainSystemDocument,
            this.UcUserManagerDocument,
            this.UcHardareEditorDocument});
            this.windowsUIView.LoadingIndicatorProperties.DescriptionFormat = "{0} loading...";
            this.windowsUIView.PageGroupProperties.ShowPageHeaders = false;
            this.windowsUIView.TileContainerProperties.ItemPadding = new System.Windows.Forms.Padding(0);
            this.windowsUIView.TileProperties.FrameAnimationInterval = 1000;
            this.windowsUIView.Tiles.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.UcMainSystemTile,
            this.UcUserManagerTile,
            this.UcHardareEditorTile});
            this.windowsUIView.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
            this.windowsUIView.UseSnappingEmulation = DevExpress.Utils.DefaultBoolean.True;
            this.windowsUIView.UseSplashScreen = DevExpress.Utils.DefaultBoolean.False;
            this.windowsUIView.TileClick += new DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileClickEventHandler(this.windowsUIView_TileClick);
            this.windowsUIView.NavigationBarsShowing += new DevExpress.XtraBars.Docking2010.Views.WindowsUI.NavigationBarsCancelEventHandler(this.WindowsUIView_NavigationBarsShowing);
            this.windowsUIView.QueryControl += new DevExpress.XtraBars.Docking2010.Views.QueryControlEventHandler(this.WindowsUIView_QueryControl);
            // 
            // startGroup
            // 
            this.startGroup.ActivationTarget = this.pageGroup;
            this.startGroup.AppearanceButton.Normal.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startGroup.AppearanceButton.Normal.Options.UseFont = true;
            this.startGroup.AppearanceGroupText.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startGroup.AppearanceGroupText.Options.UseFont = true;
            this.startGroup.AppearanceItem.Normal.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startGroup.AppearanceItem.Normal.Options.UseFont = true;
            this.startGroup.AppearanceSubtitle.Font = new System.Drawing.Font("黑体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startGroup.AppearanceSubtitle.Options.UseFont = true;
            this.startGroup.AppearanceText.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.startGroup.AppearanceText.Options.UseFont = true;
            this.startGroup.Caption = "ZX2200";
            this.startGroup.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.BaseTile[] {
            this.UcMainSystemTile,
            this.UcUserManagerTile,
            this.UcHardareEditorTile});
            this.startGroup.Name = "startGroup";
            this.startGroup.Properties.ItemPadding = new System.Windows.Forms.Padding(-100, 0, 0, 0);
            this.startGroup.Properties.ItemSize = 180;
            this.startGroup.Properties.Margin = new System.Windows.Forms.Padding(0);
            this.startGroup.Properties.RowCount = 3;
            this.startGroup.Properties.ShowGroupText = DevExpress.Utils.DefaultBoolean.False;
            this.startGroup.Subtitle = "Accuracy ";
            this.startGroup.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.StartGroup_ButtonClick);
            // 
            // pageGroup
            // 
            windowsUIButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("windowsUIButtonImageOptions1.Image")));
            this.pageGroup.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] {
            new DevExpress.XtraBars.Docking2010.WindowsUIButton("", true, windowsUIButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, null, -1, false)});
            this.pageGroup.Caption = "";
            this.pageGroup.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document[] {
            this.UcMainSystemDocument,
            this.UcUserManagerDocument,
            this.UcHardareEditorDocument});
            this.pageGroup.Name = "pageGroup";
            this.pageGroup.Parent = this.startGroup;
            this.pageGroup.Properties.Margin = new System.Windows.Forms.Padding(0);
            this.pageGroup.Properties.ShowCaption = DevExpress.Utils.DefaultBoolean.False;
            this.pageGroup.ButtonClick += new DevExpress.XtraBars.Docking2010.ButtonEventHandler(this.PageGroup_ButtonClick);
            // 
            // UcMainSystemDocument
            // 
            this.UcMainSystemDocument.Caption = "Main System";
            this.UcMainSystemDocument.ControlName = "UcMainSystem";
            this.UcMainSystemDocument.ControlTypeName = "AKRS.ZX2200.Main.Controls.Ucmain.MainControls.UcMainSystem";
            this.UcMainSystemDocument.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.Home1;
            // 
            // UcUserManagerDocument
            // 
            this.UcUserManagerDocument.Caption = "User Manager";
            this.UcUserManagerDocument.ControlName = "UcUserManager";
            this.UcUserManagerDocument.ControlTypeName = "AKRS.ZX2200.Main.Controls.Ucmain.UcUserManager";
            this.UcUserManagerDocument.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.UserManagment;
            // 
            // UcHardareEditorDocument
            // 
            this.UcHardareEditorDocument.Caption = "Hardare Editor";
            this.UcHardareEditorDocument.ControlName = "UcHardareEditor";
            this.UcHardareEditorDocument.ControlTypeName = "AKRS.ZX2200.Main.Controls.Ucmain.UcHardwareEditor";
            this.UcHardareEditorDocument.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.System;
            // 
            // UcMainSystemTile
            // 
            this.UcMainSystemTile.Appearances.Normal.BackColor = System.Drawing.Color.Green;
            this.UcMainSystemTile.Appearances.Normal.BackColor2 = System.Drawing.Color.LightSeaGreen;
            this.UcMainSystemTile.Appearances.Normal.Font = new System.Drawing.Font("黑体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.UcMainSystemTile.Appearances.Normal.Options.UseBackColor = true;
            this.UcMainSystemTile.Appearances.Normal.Options.UseFont = true;
            this.UcMainSystemTile.Document = this.UcMainSystemDocument;
            tileItemElement1.Appearance.Hovered.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement1.Appearance.Hovered.Options.UseFont = true;
            tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tileItemElement1.Appearance.Normal.Options.UseFont = true;
            tileItemElement1.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.Home1;
            tileItemElement1.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "Main System";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.UcMainSystemTile.Elements.Add(tileItemElement1);
            this.UcMainSystemTile.Group = "TileGroup1";
            this.UcMainSystemTile.Name = "UcListingTile";
            this.UcMainSystemTile.Padding = new System.Windows.Forms.Padding(0);
            this.UcMainSystemTile.Properties.AllowCheck = DevExpress.Utils.DefaultBoolean.False;
            this.UcMainSystemTile.Properties.BackgroundImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.UcMainSystemTile.Properties.BackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomOutside;
            this.UcMainSystemTile.Properties.ItemSize = DevExpress.XtraEditors.TileItemSize.Large;
            // 
            // UcUserManagerTile
            // 
            this.UcUserManagerTile.Appearances.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UcUserManagerTile.Appearances.Normal.BackColor2 = System.Drawing.Color.OrangeRed;
            this.UcUserManagerTile.Appearances.Normal.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(66)))), ((int)(((byte)(92)))));
            this.UcUserManagerTile.Appearances.Normal.Options.UseBackColor = true;
            this.UcUserManagerTile.Appearances.Normal.Options.UseBorderColor = true;
            this.UcUserManagerTile.Document = this.UcUserManagerDocument;
            tileItemElement2.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            tileItemElement2.Appearance.Normal.Options.UseFont = true;
            tileItemElement2.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.UserManagment;
            tileItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileItemElement2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Top;
            tileItemElement2.Text = "User Management";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.BottomCenter;
            this.UcUserManagerTile.Elements.Add(tileItemElement2);
            this.UcUserManagerTile.Enabled = false;
            this.UcUserManagerTile.Group = "TileGroup2";
            this.startGroup.SetID(this.UcUserManagerTile, 1);
            this.UcUserManagerTile.Name = "UcUserManagerTile";
            this.UcUserManagerTile.Padding = new System.Windows.Forms.Padding(0);
            this.UcUserManagerTile.Properties.AllowCheck = DevExpress.Utils.DefaultBoolean.False;
            this.UcUserManagerTile.Properties.BackgroundImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.UcUserManagerTile.Properties.BackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomOutside;
            this.UcUserManagerTile.Properties.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            // 
            // UcHardareEditorTile
            // 
            this.UcHardareEditorTile.Appearances.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UcHardareEditorTile.Appearances.Normal.BackColor2 = System.Drawing.Color.Olive;
            this.UcHardareEditorTile.Appearances.Normal.Options.UseBackColor = true;
            this.UcHardareEditorTile.Document = this.UcHardareEditorDocument;
            tileItemElement3.Appearance.Normal.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            tileItemElement3.Appearance.Normal.Options.UseFont = true;
            tileItemElement3.ImageOptions.Image = global::AKRS.ZX2200.Properties.Resources.System;
            tileItemElement3.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            tileItemElement3.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement3.Text = "Hardware Management";
            this.UcHardareEditorTile.Elements.Add(tileItemElement3);
            this.UcHardareEditorTile.Enabled = true;
            this.UcHardareEditorTile.Group = "TileGroup2";
            this.startGroup.SetID(this.UcHardareEditorTile, 2);
            this.UcHardareEditorTile.Name = "UcHardareEditorTile";
            this.UcHardareEditorTile.Padding = new System.Windows.Forms.Padding(0);
            this.UcHardareEditorTile.Properties.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            // 
            // flyout
            // 
            this.flyout.Name = "flyout";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.barDockControlTop.Appearance.Options.UseBackColor = true;
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1296, 0);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 777);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1296, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 777);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1296, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 777);
            // 
            // MainForm
            // 
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseTextOptions = true;
            this.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1296, 777);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.HtmlText = "<p align=\"center\">Accuary</p>";
            this.IconOptions.ShowIcon = false;
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(1000, 600);
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.windowsUIView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pageGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcMainSystemDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcUserManagerDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcHardareEditorDocument)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcMainSystemTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcUserManagerTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UcHardareEditorTile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.flyout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.WindowsUIView windowsUIView;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document UcMainSystemDocument;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document UcUserManagerDocument;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Document UcHardareEditorDocument;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.TileContainer startGroup;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.PageGroup pageGroup;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile UcMainSystemTile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile UcUserManagerTile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Tile UcHardareEditorTile;
        private DevExpress.XtraBars.Docking2010.Views.WindowsUI.Flyout flyout;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
    }
}
