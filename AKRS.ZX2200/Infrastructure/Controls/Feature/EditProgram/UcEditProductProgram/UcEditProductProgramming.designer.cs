namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram.UcEditProductProgram
{
    partial class UcEditProductProgramming
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcEditProductProgramming));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.ILstAssistantStep = new DevExpress.XtraEditors.ImageListBoxControl();
            this.TreeListProgramming = new DevExpress.XtraTreeList.TreeList();
            this.TcName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.CmbSystem = new DevExpress.XtraEditors.ComboBoxEdit();
            this.NavBarGroupTransportSystem = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem1 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem2 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem3 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem4 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupConfiguration = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem5 = new DevExpress.XtraNavBar.NavBarItem();
            this.navBarItem6 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupPPTools = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem7 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupESTools = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem8 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupDispenser = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem9 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupTransportUnit = new DevExpress.XtraNavBar.NavBarGroup();
            this.NavBarItemTransportUnitGeneral = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupBondPosition = new DevExpress.XtraNavBar.NavBarGroup();
            this.navBarItem11 = new DevExpress.XtraNavBar.NavBarItem();
            this.NavBarGroupComponents = new DevExpress.XtraNavBar.NavBarGroup();
            this.NavBarGroupEpoxyApplication = new DevExpress.XtraNavBar.NavBarGroup();
            this.NavBarGroupPostBond = new DevExpress.XtraNavBar.NavBarGroup();
            this.NavBarGroupProcessingList = new DevExpress.XtraNavBar.NavBarGroup();
            this.PnlContainer = new DevExpress.XtraEditors.PanelControl();
            this.SpiltContainerProgramming = new DevExpress.XtraEditors.SplitContainerControl();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtReset = new DevExpress.XtraEditors.SimpleButton();
            this.BtClose = new DevExpress.XtraEditors.SimpleButton();
            this.BtStart = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ILstAssistantStep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListProgramming)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbSystem.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlContainer)).BeginInit();
            this.PnlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming.Panel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming.Panel2)).BeginInit();
            this.SpiltContainerProgramming.Panel2.SuspendLayout();
            this.SpiltContainerProgramming.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.ILstAssistantStep);
            this.panelControl1.Controls.Add(this.TreeListProgramming);
            this.panelControl1.Controls.Add(this.CmbSystem);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(284, 920);
            this.panelControl1.TabIndex = 0;
            // 
            // ILstAssistantStep
            // 
            this.ILstAssistantStep.Location = new System.Drawing.Point(28, 494);
            this.ILstAssistantStep.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ILstAssistantStep.Name = "ILstAssistantStep";
            this.ILstAssistantStep.Size = new System.Drawing.Size(230, 388);
            this.ILstAssistantStep.TabIndex = 4;
            this.ILstAssistantStep.SelectedIndexChanged += new System.EventHandler(this.LstAssisantStep_SelectedIndexChanged);
            this.ILstAssistantStep.DrawItem += new DevExpress.XtraEditors.ListBoxDrawItemEventHandler(this.LstAssisantStep_DrawItem);
            // 
            // TreeListProgramming
            // 
            this.TreeListProgramming.Appearance.FocusedCell.BackColor = System.Drawing.Color.PowderBlue;
            this.TreeListProgramming.Appearance.FocusedCell.Options.UseBackColor = true;
            this.TreeListProgramming.Appearance.Row.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TreeListProgramming.Appearance.Row.Options.UseFont = true;
            this.TreeListProgramming.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.TcName});
            this.TreeListProgramming.FixedLineWidth = 1;
            this.TreeListProgramming.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TreeListProgramming.Location = new System.Drawing.Point(28, 60);
            this.TreeListProgramming.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TreeListProgramming.MinWidth = 19;
            this.TreeListProgramming.Name = "TreeListProgramming";
            this.TreeListProgramming.BeginUnboundLoad();
            this.TreeListProgramming.AppendNode(new object[] {
            "传输系统"}, -1, 0, 0, 4, "TransportSystem");
            this.TreeListProgramming.AppendNode(new object[] {
            "上料"}, 0, "Input");
            this.TreeListProgramming.AppendNode(new object[] {
            "皮带传输1"}, 0, "BeltSystem1");
            this.TreeListProgramming.AppendNode(new object[] {
            "皮带传输2"}, 0, "BeltSystem2");
            this.TreeListProgramming.AppendNode(new object[] {
            "出料"}, 0, "Output");
            this.TreeListProgramming.AppendNode(new object[] {
            "晶圆系统"}, -1, 0, 0, 15, "WaferHandling");
            this.TreeListProgramming.AppendNode(new object[] {
            "工具/芯片配置"}, -1, 0, 0, 5, "Configuration");
            this.TreeListProgramming.AppendNode(new object[] {
            "系统2"}, 6, "ConfigurationSubNode");
            this.TreeListProgramming.AppendNode(new object[] {
            "系统1"}, 6, "ConfigurationFirNode");
            this.TreeListProgramming.AppendNode(new object[] {
            "吸嘴"}, -1, 0, 0, 6, "PPTools");
            this.TreeListProgramming.AppendNode(new object[] {
            "顶针"}, -1, 0, 0, 7, "ESTools");
            this.TreeListProgramming.AppendNode(new object[] {
            "点胶头"}, -1, 0, 0, 8, "Dispenser");
            this.TreeListProgramming.AppendNode(new object[] {
            "翻转工具"}, -1, 0, 0, 16, "FlipTool");
            this.TreeListProgramming.AppendNode(new object[] {
            "框架"}, -1, 0, 0, 9, "TransportUnit");
            this.TreeListProgramming.AppendNode(new object[] {
            "框架示教"}, 13, "TransportUnitGeneral");
            this.TreeListProgramming.AppendNode(new object[] {
            "焊点"}, -1, 0, 0, 10, "BondingPosition");
            this.TreeListProgramming.AppendNode(new object[] {
            "芯片"}, -1, 0, 0, 11, "Components");
            this.TreeListProgramming.AppendNode(new object[] {
            "胶形"}, -1, 0, 0, 12, "EpoxyApplication");
            this.TreeListProgramming.AppendNode(new object[] {
            "检测"}, -1, 0, 0, 13, "PostBond");
            this.TreeListProgramming.AppendNode(new object[] {
            "工作步骤"}, -1, 0, 0, 14, "ProcessingList");
            this.TreeListProgramming.EndUnboundLoad();
            this.TreeListProgramming.OptionsCustomization.AllowColumnMoving = false;
            this.TreeListProgramming.OptionsCustomization.AllowColumnResizing = false;
            this.TreeListProgramming.OptionsCustomization.AllowFilter = false;
            this.TreeListProgramming.OptionsCustomization.AllowQuickHideColumns = false;
            this.TreeListProgramming.OptionsCustomization.AllowSort = false;
            this.TreeListProgramming.OptionsFind.AllowFindPanel = false;
            this.TreeListProgramming.OptionsView.ShowColumns = false;
            this.TreeListProgramming.OptionsView.ShowHierarchyIndentationLines = DevExpress.Utils.DefaultBoolean.False;
            this.TreeListProgramming.OptionsView.ShowHorzLines = false;
            this.TreeListProgramming.OptionsView.ShowIndicator = false;
            this.TreeListProgramming.OptionsView.ShowVertLines = false;
            this.TreeListProgramming.Size = new System.Drawing.Size(230, 422);
            this.TreeListProgramming.StateImageList = this.imageCollection1;
            this.TreeListProgramming.TabIndex = 0;
            this.TreeListProgramming.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeListProgramming_FocusedNodeChanged);
            // 
            // TcName
            // 
            this.TcName.FieldName = "Name";
            this.TcName.MinWidth = 19;
            this.TcName.Name = "TcName";
            this.TcName.OptionsColumn.AllowEdit = false;
            this.TcName.OptionsColumn.AllowMove = false;
            this.TcName.OptionsColumn.AllowSize = false;
            this.TcName.OptionsColumn.AllowSort = false;
            this.TcName.OptionsFilter.AllowAutoFilter = false;
            this.TcName.OptionsFilter.AllowFilter = false;
            this.TcName.Visible = true;
            this.TcName.VisibleIndex = 0;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "add_16x16.png");
            this.imageCollection1.Images.SetKeyName(1, "suggestion_16x16.png");
            this.imageCollection1.Images.SetKeyName(2, "pictureshapeoutlinecolor_16x16.png");
            this.imageCollection1.Images.SetKeyName(3, "apply_16x16.png");
            this.imageCollection1.Images.SetKeyName(4, "group_16x16.png");
            this.imageCollection1.Images.SetKeyName(5, "build_16x16.png");
            this.imageCollection1.Images.SetKeyName(6, "alignverticalcenter2_16x16.png");
            this.imageCollection1.Images.SetKeyName(7, "printsortasc_16x16.png");
            this.imageCollection1.Images.SetKeyName(8, "filter_16x16.png");
            this.imageCollection1.Images.SetKeyName(9, "cards_16x16.png");
            this.imageCollection1.Images.SetKeyName(10, "bringtofront_16x16.png");
            this.imageCollection1.Images.SetKeyName(11, "brand_16x16.png");
            this.imageCollection1.Images.SetKeyName(12, "stepline_16x16.png");
            this.imageCollection1.Images.SetKeyName(13, "zoom2_16x16.png");
            this.imageCollection1.Images.SetKeyName(14, "listnumbers_16x16.png");
            this.imageCollection1.Images.SetKeyName(15, "contentarrangeinrows_16x16.png");
            this.imageCollection1.Images.SetKeyName(16, "翻转工具.png");
            // 
            // CmbSystem
            // 
            this.CmbSystem.EditValue = "System2";
            this.CmbSystem.Location = new System.Drawing.Point(28, 27);
            this.CmbSystem.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CmbSystem.Name = "CmbSystem";
            this.CmbSystem.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbSystem.Properties.DropDownRows = 2;
            this.CmbSystem.Properties.Items.AddRange(new object[] {
            "System1",
            "System2"});
            this.CmbSystem.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbSystem.Size = new System.Drawing.Size(230, 20);
            this.CmbSystem.TabIndex = 0;
            this.CmbSystem.SelectedIndexChanged += new System.EventHandler(this.CmbSystem_SelectedIndexChanged);
            // 
            // NavBarGroupTransportSystem
            // 
            this.NavBarGroupTransportSystem.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupTransportSystem.Appearance.Options.UseFont = true;
            this.NavBarGroupTransportSystem.Caption = "Transport system";
            this.NavBarGroupTransportSystem.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupTransportSystem.ImageOptions.LargeImage")));
            this.NavBarGroupTransportSystem.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupTransportSystem.ImageOptions.SmallImage")));
            this.NavBarGroupTransportSystem.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem1),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem2),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem3),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem4)});
            this.NavBarGroupTransportSystem.Name = "NavBarGroupTransportSystem";
            // 
            // navBarItem1
            // 
            this.navBarItem1.Caption = "Input";
            this.navBarItem1.Name = "navBarItem1";
            // 
            // navBarItem2
            // 
            this.navBarItem2.Caption = "Belt system1";
            this.navBarItem2.Name = "navBarItem2";
            // 
            // navBarItem3
            // 
            this.navBarItem3.Caption = "Belt system2";
            this.navBarItem3.Name = "navBarItem3";
            // 
            // navBarItem4
            // 
            this.navBarItem4.Caption = "Output";
            this.navBarItem4.Name = "navBarItem4";
            // 
            // NavBarGroupConfiguration
            // 
            this.NavBarGroupConfiguration.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupConfiguration.Appearance.Options.UseFont = true;
            this.NavBarGroupConfiguration.Caption = "Configuration";
            this.NavBarGroupConfiguration.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupConfiguration.ImageOptions.LargeImage")));
            this.NavBarGroupConfiguration.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupConfiguration.ImageOptions.SmallImage")));
            this.NavBarGroupConfiguration.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem5),
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem6)});
            this.NavBarGroupConfiguration.Name = "NavBarGroupConfiguration";
            // 
            // navBarItem5
            // 
            this.navBarItem5.Caption = "navBarItem5";
            this.navBarItem5.Name = "navBarItem5";
            // 
            // navBarItem6
            // 
            this.navBarItem6.Caption = "navBarItem6";
            this.navBarItem6.Name = "navBarItem6";
            // 
            // NavBarGroupPPTools
            // 
            this.NavBarGroupPPTools.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupPPTools.Appearance.Options.UseFont = true;
            this.NavBarGroupPPTools.Caption = "PP tools";
            this.NavBarGroupPPTools.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupPPTools.ImageOptions.LargeImage")));
            this.NavBarGroupPPTools.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupPPTools.ImageOptions.SmallImage")));
            this.NavBarGroupPPTools.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem7)});
            this.NavBarGroupPPTools.Name = "NavBarGroupPPTools";
            this.NavBarGroupPPTools.ShowIcons = DevExpress.Utils.DefaultBoolean.True;
            // 
            // navBarItem7
            // 
            this.navBarItem7.Caption = "navBarItem7";
            this.navBarItem7.Name = "navBarItem7";
            // 
            // NavBarGroupESTools
            // 
            this.NavBarGroupESTools.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupESTools.Appearance.Options.UseFont = true;
            this.NavBarGroupESTools.Caption = "ES Tools";
            this.NavBarGroupESTools.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupESTools.ImageOptions.LargeImage")));
            this.NavBarGroupESTools.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupESTools.ImageOptions.SmallImage")));
            this.NavBarGroupESTools.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem8)});
            this.NavBarGroupESTools.Name = "NavBarGroupESTools";
            // 
            // navBarItem8
            // 
            this.navBarItem8.Caption = "navBarItem8";
            this.navBarItem8.Name = "navBarItem8";
            // 
            // NavBarGroupDispenser
            // 
            this.NavBarGroupDispenser.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupDispenser.Appearance.Options.UseFont = true;
            this.NavBarGroupDispenser.Caption = "Dispenser";
            this.NavBarGroupDispenser.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupDispenser.ImageOptions.LargeImage")));
            this.NavBarGroupDispenser.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupDispenser.ImageOptions.SmallImage")));
            this.NavBarGroupDispenser.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem9)});
            this.NavBarGroupDispenser.Name = "NavBarGroupDispenser";
            // 
            // navBarItem9
            // 
            this.navBarItem9.Caption = "navBarItem9";
            this.navBarItem9.Name = "navBarItem9";
            // 
            // NavBarGroupTransportUnit
            // 
            this.NavBarGroupTransportUnit.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupTransportUnit.Appearance.Options.UseFont = true;
            this.NavBarGroupTransportUnit.Caption = "Transport unit";
            this.NavBarGroupTransportUnit.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupTransportUnit.ImageOptions.LargeImage")));
            this.NavBarGroupTransportUnit.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupTransportUnit.ImageOptions.SmallImage")));
            this.NavBarGroupTransportUnit.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.NavBarItemTransportUnitGeneral)});
            this.NavBarGroupTransportUnit.Name = "NavBarGroupTransportUnit";
            // 
            // NavBarItemTransportUnitGeneral
            // 
            this.NavBarItemTransportUnitGeneral.Caption = "Transport unit general";
            this.NavBarItemTransportUnitGeneral.Name = "NavBarItemTransportUnitGeneral";
            // 
            // NavBarGroupBondPosition
            // 
            this.NavBarGroupBondPosition.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupBondPosition.Appearance.Options.UseFont = true;
            this.NavBarGroupBondPosition.Caption = "Bond position";
            this.NavBarGroupBondPosition.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupBondPosition.ImageOptions.LargeImage")));
            this.NavBarGroupBondPosition.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupBondPosition.ImageOptions.SmallImage")));
            this.NavBarGroupBondPosition.ItemLinks.AddRange(new DevExpress.XtraNavBar.NavBarItemLink[] {
            new DevExpress.XtraNavBar.NavBarItemLink(this.navBarItem11)});
            this.NavBarGroupBondPosition.Name = "NavBarGroupBondPosition";
            // 
            // navBarItem11
            // 
            this.navBarItem11.Caption = "navBarItem11";
            this.navBarItem11.Name = "navBarItem11";
            // 
            // NavBarGroupComponents
            // 
            this.NavBarGroupComponents.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupComponents.Appearance.Options.UseFont = true;
            this.NavBarGroupComponents.Caption = "Components";
            this.NavBarGroupComponents.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupComponents.ImageOptions.SmallImage")));
            this.NavBarGroupComponents.Name = "NavBarGroupComponents";
            // 
            // NavBarGroupEpoxyApplication
            // 
            this.NavBarGroupEpoxyApplication.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupEpoxyApplication.Appearance.Options.UseFont = true;
            this.NavBarGroupEpoxyApplication.Caption = "Epoxy application";
            this.NavBarGroupEpoxyApplication.Expanded = true;
            this.NavBarGroupEpoxyApplication.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupEpoxyApplication.ImageOptions.LargeImage")));
            this.NavBarGroupEpoxyApplication.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupEpoxyApplication.ImageOptions.SmallImage")));
            this.NavBarGroupEpoxyApplication.Name = "NavBarGroupEpoxyApplication";
            // 
            // NavBarGroupPostBond
            // 
            this.NavBarGroupPostBond.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupPostBond.Appearance.Options.UseFont = true;
            this.NavBarGroupPostBond.Caption = "Post-bond";
            this.NavBarGroupPostBond.Expanded = true;
            this.NavBarGroupPostBond.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupPostBond.ImageOptions.LargeImage")));
            this.NavBarGroupPostBond.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupPostBond.ImageOptions.SmallImage")));
            this.NavBarGroupPostBond.Name = "NavBarGroupPostBond";
            // 
            // NavBarGroupProcessingList
            // 
            this.NavBarGroupProcessingList.Appearance.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NavBarGroupProcessingList.Appearance.Options.UseFont = true;
            this.NavBarGroupProcessingList.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.NavBarGroupProcessingList.AppearancePressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.NavBarGroupProcessingList.AppearancePressed.Options.UseBackColor = true;
            this.NavBarGroupProcessingList.Caption = "Processing list";
            this.NavBarGroupProcessingList.Expanded = true;
            this.NavBarGroupProcessingList.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupProcessingList.ImageOptions.LargeImage")));
            this.NavBarGroupProcessingList.ImageOptions.SmallImage = ((System.Drawing.Image)(resources.GetObject("NavBarGroupProcessingList.ImageOptions.SmallImage")));
            this.NavBarGroupProcessingList.Name = "NavBarGroupProcessingList";
            // 
            // PnlContainer
            // 
            this.PnlContainer.Controls.Add(this.SpiltContainerProgramming);
            this.PnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlContainer.Location = new System.Drawing.Point(284, 0);
            this.PnlContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlContainer.Name = "PnlContainer";
            this.PnlContainer.Size = new System.Drawing.Size(1259, 920);
            this.PnlContainer.TabIndex = 1;
            // 
            // SpiltContainerProgramming
            // 
            this.SpiltContainerProgramming.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SpiltContainerProgramming.Horizontal = false;
            this.SpiltContainerProgramming.IsSplitterFixed = true;
            this.SpiltContainerProgramming.Location = new System.Drawing.Point(2, 2);
            this.SpiltContainerProgramming.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.SpiltContainerProgramming.Name = "SpiltContainerProgramming";
            // 
            // SpiltContainerProgramming.Panel1
            // 
            this.SpiltContainerProgramming.Panel1.Text = "Panel1";
            // 
            // SpiltContainerProgramming.Panel2
            // 
            this.SpiltContainerProgramming.Panel2.Controls.Add(this.BtSave);
            this.SpiltContainerProgramming.Panel2.Controls.Add(this.BtReset);
            this.SpiltContainerProgramming.Panel2.Controls.Add(this.BtClose);
            this.SpiltContainerProgramming.Panel2.Controls.Add(this.BtStart);
            this.SpiltContainerProgramming.Panel2.Text = "Panel2";
            this.SpiltContainerProgramming.Size = new System.Drawing.Size(1255, 916);
            this.SpiltContainerProgramming.SplitterPosition = 801;
            this.SpiltContainerProgramming.TabIndex = 2;
            // 
            // BtSave
            // 
            this.BtSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtSave.ImageOptions.Image")));
            this.BtSave.Location = new System.Drawing.Point(195, 35);
            this.BtSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(76, 34);
            this.BtSave.TabIndex = 4;
            this.BtSave.Text = "Save";
            this.BtSave.Click += new System.EventHandler(this.BtSave_Click);
            // 
            // BtReset
            // 
            this.BtReset.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtReset.ImageOptions.Image")));
            this.BtReset.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtReset.Location = new System.Drawing.Point(1084, 35);
            this.BtReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtReset.Name = "BtReset";
            this.BtReset.Size = new System.Drawing.Size(32, 34);
            this.BtReset.TabIndex = 3;
            this.BtReset.Click += new System.EventHandler(this.BtReset_Click);
            // 
            // BtClose
            // 
            this.BtClose.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtClose.ImageOptions.Image")));
            this.BtClose.Location = new System.Drawing.Point(111, 35);
            this.BtClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtClose.Name = "BtClose";
            this.BtClose.Size = new System.Drawing.Size(76, 34);
            this.BtClose.TabIndex = 2;
            this.BtClose.Text = "Close";
            this.BtClose.Click += new System.EventHandler(this.BtClose_Click);
            // 
            // BtStart
            // 
            this.BtStart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtStart.ImageOptions.Image")));
            this.BtStart.Location = new System.Drawing.Point(27, 35);
            this.BtStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.BtStart.Name = "BtStart";
            this.BtStart.Size = new System.Drawing.Size(76, 34);
            this.BtStart.TabIndex = 0;
            this.BtStart.Text = "Start";
            this.BtStart.Click += new System.EventHandler(this.BtStart_Click);
            // 
            // UcEditProductProgramming
            // 
            this.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Appearance.Options.UseFont = true;
            this.Appearance.Options.UseTextOptions = true;
            this.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PnlContainer);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcEditProductProgramming";
            this.Size = new System.Drawing.Size(1543, 920);
            this.Tag = "Product assistant";
            this.Load += new System.EventHandler(this.UcEditProductProgramming_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ILstAssistantStep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TreeListProgramming)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbSystem.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlContainer)).EndInit();
            this.PnlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming.Panel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming.Panel2)).EndInit();
            this.SpiltContainerProgramming.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpiltContainerProgramming)).EndInit();
            this.SpiltContainerProgramming.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupTransportSystem;
        private DevExpress.XtraNavBar.NavBarItem navBarItem1;
        private DevExpress.XtraNavBar.NavBarItem navBarItem2;
        private DevExpress.XtraNavBar.NavBarItem navBarItem3;
        private DevExpress.XtraNavBar.NavBarItem navBarItem4;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupConfiguration;
        private DevExpress.XtraNavBar.NavBarItem navBarItem5;
        private DevExpress.XtraNavBar.NavBarItem navBarItem6;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupPPTools;
        private DevExpress.XtraNavBar.NavBarItem navBarItem7;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupESTools;
        private DevExpress.XtraNavBar.NavBarItem navBarItem8;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupDispenser;
        private DevExpress.XtraNavBar.NavBarItem navBarItem9;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupTransportUnit;
        private DevExpress.XtraNavBar.NavBarItem NavBarItemTransportUnitGeneral;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupBondPosition;
        private DevExpress.XtraNavBar.NavBarItem navBarItem11;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupComponents;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupEpoxyApplication;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupPostBond;
        private DevExpress.XtraNavBar.NavBarGroup NavBarGroupProcessingList;
        private DevExpress.XtraEditors.ComboBoxEdit CmbSystem;
        private DevExpress.XtraEditors.PanelControl PnlContainer;
        private DevExpress.XtraTreeList.TreeList TreeListProgramming;
        private DevExpress.XtraTreeList.Columns.TreeListColumn TcName;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.SplitContainerControl SpiltContainerProgramming;
        private DevExpress.XtraEditors.SimpleButton BtClose;
        private DevExpress.XtraEditors.SimpleButton BtStart;
        private DevExpress.XtraEditors.SimpleButton BtReset;
        private DevExpress.XtraEditors.SimpleButton BtSave;
        private DevExpress.XtraEditors.ImageListBoxControl ILstAssistantStep;
    }
}