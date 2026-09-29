namespace AKRS.ZX2200.Infrastructure.Controls.Feature.EditProgram
{
    partial class UcConfiguration2
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
            this.GpPPToolBank = new DevExpress.XtraEditors.GroupControl();
            this.GcPPTool = new DevExpress.XtraGrid.GridControl();
            this.GvPPTool = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.LbFlipToolName = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.LbEpoxyMaterialName = new DevExpress.XtraEditors.LabelControl();
            this.LbDispenserName = new DevExpress.XtraEditors.LabelControl();
            this.LbEpoxyDispenser = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.GcESTool = new DevExpress.XtraGrid.GridControl();
            this.GvESTool = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.TpTools = new DevExpress.XtraTab.XtraTabPage();
            this.TpWaferMagazine = new DevExpress.XtraTab.XtraTabPage();
            this.GcWaferMagazine = new DevExpress.XtraEditors.GroupControl();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GpPPToolBank)).BeginInit();
            this.GpPPToolBank.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcPPTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPPTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcESTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvESTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.TpTools.SuspendLayout();
            this.TpWaferMagazine.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcWaferMagazine)).BeginInit();
            this.SuspendLayout();
            // 
            // GpPPToolBank
            // 
            this.tablePanel1.SetColumn(this.GpPPToolBank, 0);
            this.GpPPToolBank.Controls.Add(this.GcPPTool);
            this.GpPPToolBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GpPPToolBank.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpPPToolBank.Location = new System.Drawing.Point(26, 30);
            this.GpPPToolBank.Margin = new System.Windows.Forms.Padding(26, 30, 26, 30);
            this.GpPPToolBank.Name = "GpPPToolBank";
            this.GpPPToolBank.Padding = new System.Windows.Forms.Padding(0, 13, 0, 13);
            this.tablePanel1.SetRow(this.GpPPToolBank, 0);
            this.GpPPToolBank.Size = new System.Drawing.Size(555, 350);
            this.GpPPToolBank.TabIndex = 0;
            this.GpPPToolBank.Text = "吸嘴架";
            // 
            // GcPPTool
            // 
            this.GcPPTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcPPTool.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcPPTool.Location = new System.Drawing.Point(2, 41);
            this.GcPPTool.MainView = this.GvPPTool;
            this.GcPPTool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcPPTool.Name = "GcPPTool";
            this.GcPPTool.Size = new System.Drawing.Size(551, 294);
            this.GcPPTool.TabIndex = 0;
            this.GcPPTool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvPPTool});
            // 
            // GvPPTool
            // 
            this.GvPPTool.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn7});
            this.GvPPTool.DetailHeight = 450;
            this.GvPPTool.GridControl = this.GcPPTool;
            this.GvPPTool.Name = "GvPPTool";
            this.GvPPTool.OptionsBehavior.ReadOnly = true;
            this.GvPPTool.OptionsView.ShowGroupPanel = false;
            this.GvPPTool.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "槽位";
            this.gridColumn1.FieldName = "SlotNum";
            this.gridColumn1.MinWidth = 23;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 99;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "吸嘴";
            this.gridColumn2.FieldName = "NozzleName";
            this.gridColumn2.MinWidth = 23;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 195;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "吸嘴类型";
            this.gridColumn7.FieldName = "NozzleType";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 257;
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.groupControl2);
            this.tablePanel1.Controls.Add(this.GpPPToolBank);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(34, 39, 34, 39);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F)});
            this.tablePanel1.Size = new System.Drawing.Size(1214, 819);
            this.tablePanel1.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.tablePanel1.SetColumn(this.panelControl1, 1);
            this.panelControl1.Controls.Add(this.LbFlipToolName);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.LbEpoxyMaterialName);
            this.panelControl1.Controls.Add(this.LbDispenserName);
            this.panelControl1.Controls.Add(this.LbEpoxyDispenser);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(610, 4);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 0);
            this.panelControl1.Size = new System.Drawing.Size(601, 402);
            this.panelControl1.TabIndex = 3;
            // 
            // LbFlipToolName
            // 
            this.LbFlipToolName.Location = new System.Drawing.Point(233, 44);
            this.LbFlipToolName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbFlipToolName.Name = "LbFlipToolName";
            this.LbFlipToolName.Size = new System.Drawing.Size(30, 18);
            this.LbFlipToolName.TabIndex = 5;
            this.LbFlipToolName.Text = "------";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(105, 44);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(65, 18);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "翻转工具:";
            // 
            // LbEpoxyMaterialName
            // 
            this.LbEpoxyMaterialName.Location = new System.Drawing.Point(233, 197);
            this.LbEpoxyMaterialName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbEpoxyMaterialName.Name = "LbEpoxyMaterialName";
            this.LbEpoxyMaterialName.Size = new System.Drawing.Size(30, 18);
            this.LbEpoxyMaterialName.TabIndex = 3;
            this.LbEpoxyMaterialName.Text = "------";
            // 
            // LbDispenserName
            // 
            this.LbDispenserName.Location = new System.Drawing.Point(233, 115);
            this.LbDispenserName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbDispenserName.Name = "LbDispenserName";
            this.LbDispenserName.Size = new System.Drawing.Size(30, 18);
            this.LbDispenserName.TabIndex = 2;
            this.LbDispenserName.Text = "------";
            // 
            // LbEpoxyDispenser
            // 
            this.LbEpoxyDispenser.Location = new System.Drawing.Point(125, 197);
            this.LbEpoxyDispenser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbEpoxyDispenser.Name = "LbEpoxyDispenser";
            this.LbEpoxyDispenser.Size = new System.Drawing.Size(45, 18);
            this.LbEpoxyDispenser.TabIndex = 1;
            this.LbEpoxyDispenser.Text = "胶水：";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(120, 115);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(50, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "点胶器:";
            // 
            // groupControl2
            // 
            this.tablePanel1.SetColumn(this.groupControl2, 0);
            this.groupControl2.Controls.Add(this.GcESTool);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(26, 440);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(26, 30, 26, 30);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Padding = new System.Windows.Forms.Padding(0, 13, 0, 13);
            this.tablePanel1.SetRow(this.groupControl2, 1);
            this.groupControl2.Size = new System.Drawing.Size(555, 349);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "顶针架";
            // 
            // GcESTool
            // 
            this.GcESTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcESTool.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcESTool.Location = new System.Drawing.Point(2, 41);
            this.GcESTool.MainView = this.GvESTool;
            this.GcESTool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcESTool.Name = "GcESTool";
            this.GcESTool.Size = new System.Drawing.Size(551, 293);
            this.GcESTool.TabIndex = 0;
            this.GcESTool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvESTool});
            // 
            // GvESTool
            // 
            this.GvESTool.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.gridColumn3,
            this.gridColumn4});
            this.GvESTool.DetailHeight = 450;
            this.GvESTool.GridControl = this.GcESTool;
            this.GvESTool.Name = "GvESTool";
            this.GvESTool.OptionsDetail.EnableMasterViewMode = false;
            this.GvESTool.OptionsView.ShowGroupPanel = false;
            this.GvESTool.OptionsView.ShowIndicator = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "槽位";
            this.gridColumn9.FieldName = "SlotNum";
            this.gridColumn9.MinWidth = 25;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 103;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "顶针";
            this.gridColumn3.FieldName = "Name";
            this.gridColumn3.MinWidth = 23;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 201;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "顶针类型";
            this.gridColumn4.FieldName = "EjectionConfig.EjectionType";
            this.gridColumn4.MinWidth = 23;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 2;
            this.gridColumn4.Width = 247;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Padding = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.xtraTabControl1.SelectedTabPage = this.TpTools;
            this.xtraTabControl1.Size = new System.Drawing.Size(1216, 851);
            this.xtraTabControl1.TabIndex = 3;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TpTools,
            this.TpWaferMagazine});
            // 
            // TpTools
            // 
            this.TpTools.Controls.Add(this.tablePanel1);
            this.TpTools.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpTools.Name = "TpTools";
            this.TpTools.Size = new System.Drawing.Size(1214, 819);
            this.TpTools.Text = " 工具";
            // 
            // TpWaferMagazine
            // 
            this.TpWaferMagazine.Controls.Add(this.GcWaferMagazine);
            this.TpWaferMagazine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpWaferMagazine.Name = "TpWaferMagazine";
            this.TpWaferMagazine.Size = new System.Drawing.Size(1214, 819);
            this.TpWaferMagazine.Text = "   晶圆";
            // 
            // GcWaferMagazine
            // 
            this.GcWaferMagazine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcWaferMagazine.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcWaferMagazine.Location = new System.Drawing.Point(0, 0);
            this.GcWaferMagazine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcWaferMagazine.Name = "GcWaferMagazine";
            this.GcWaferMagazine.Size = new System.Drawing.Size(1214, 819);
            this.GcWaferMagazine.TabIndex = 3;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "State";
            this.gridColumn8.FieldName = "Type";
            this.gridColumn8.MinWidth = 23;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 188;
            // 
            // UcConfiguration2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcConfiguration2";
            this.Size = new System.Drawing.Size(1216, 851);
            ((System.ComponentModel.ISupportInitialize)(this.GpPPToolBank)).EndInit();
            this.GpPPToolBank.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcPPTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPPTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcESTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvESTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.TpTools.ResumeLayout(false);
            this.TpWaferMagazine.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcWaferMagazine)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GpPPToolBank;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl GcESTool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvESTool;
        private DevExpress.XtraGrid.GridControl GcPPTool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvPPTool;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl LbEpoxyMaterialName;
        private DevExpress.XtraEditors.LabelControl LbDispenserName;
        private DevExpress.XtraEditors.LabelControl LbEpoxyDispenser;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage TpTools;
        private DevExpress.XtraTab.XtraTabPage TpWaferMagazine;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraEditors.GroupControl GcWaferMagazine;
        private DevExpress.XtraEditors.LabelControl LbFlipToolName;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
