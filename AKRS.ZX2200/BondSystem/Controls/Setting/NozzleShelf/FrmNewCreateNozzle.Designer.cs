namespace AKRS.ZX2200.BondSystem.Controls.Setting.NozzleShelf
{
    partial class FrmNewCreateNozzle
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
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LueToolShape = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.LueToolSize = new DevExpress.XtraEditors.LookUpEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GcTool = new DevExpress.XtraGrid.GridControl();
            this.GvTool = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueToolShape.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueToolSize.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcTool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTool)).BeginInit();
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
            this.groupControl1.Size = new System.Drawing.Size(440, 87);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "吸嘴名称";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(45, 42);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(350, 24);
            this.TxtName.TabIndex = 1;
            this.TxtName.EditValueChanged += new System.EventHandler(this.TxtName_EditValueChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.LueToolShape);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 87);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(440, 87);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "吸嘴形状";
            // 
            // LueToolShape
            // 
            this.LueToolShape.Location = new System.Drawing.Point(45, 40);
            this.LueToolShape.Name = "LueToolShape";
            this.LueToolShape.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueToolShape.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueToolShape.Properties.DisplayMember = "Display";
            this.LueToolShape.Properties.DropDownRows = 5;
            this.LueToolShape.Properties.NullText = "";
            this.LueToolShape.Properties.ValueMember = "Value";
            this.LueToolShape.Size = new System.Drawing.Size(350, 24);
            this.LueToolShape.TabIndex = 29;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.LueToolSize);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(0, 174);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(440, 87);
            this.groupControl3.TabIndex = 3;
            this.groupControl3.Text = "吸嘴尺寸";
            // 
            // LueToolSize
            // 
            this.LueToolSize.Location = new System.Drawing.Point(45, 41);
            this.LueToolSize.Name = "LueToolSize";
            this.LueToolSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueToolSize.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueToolSize.Properties.DisplayMember = "Display";
            this.LueToolSize.Properties.DropDownRows = 5;
            this.LueToolSize.Properties.NullText = "";
            this.LueToolSize.Properties.ValueMember = "Value";
            this.LueToolSize.Size = new System.Drawing.Size(350, 24);
            this.LueToolSize.TabIndex = 30;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 261);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(440, 474);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GcTool);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(438, 442);
            this.xtraTabPage1.Text = "Dataset";
            // 
            // GcTool
            // 
            this.GcTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcTool.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcTool.Location = new System.Drawing.Point(0, 0);
            this.GcTool.MainView = this.GvTool;
            this.GcTool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcTool.Name = "GcTool";
            this.GcTool.Size = new System.Drawing.Size(438, 442);
            this.GcTool.TabIndex = 0;
            this.GcTool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvTool});
            // 
            // GvTool
            // 
            this.GvTool.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
            this.GvTool.DetailHeight = 450;
            this.GvTool.GridControl = this.GcTool;
            this.GvTool.Name = "GvTool";
            this.GvTool.OptionsDetail.EnableMasterViewMode = false;
            this.GvTool.OptionsView.ShowColumnHeaders = false;
            this.GvTool.OptionsView.ShowGroupPanel = false;
            this.GvTool.OptionsView.ShowIndicator = false;
            this.GvTool.OptionsView.ShowVerticalLines = DevExpress.Utils.DefaultBoolean.False;
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
            // tablePanel1
            // 
            this.tablePanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Controls.Add(this.BtnGenerate);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 673);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(440, 62);
            this.tablePanel1.TabIndex = 22;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 1);
            this.BtCancel.Location = new System.Drawing.Point(258, 12);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(144, 37);
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
            this.BtnGenerate.Size = new System.Drawing.Size(144, 37);
            this.BtnGenerate.TabIndex = 6;
            this.BtnGenerate.Text = "确认";
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // FrmNewCreateNozzle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 735);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Location = new System.Drawing.Point(25, 0);
            this.Name = "FrmNewCreateNozzle";
            this.Text = "新建数据集";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueToolShape.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueToolSize.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.TextEdit TxtName;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.LookUpEdit LueToolShape;
        private DevExpress.XtraEditors.LookUpEdit LueToolSize;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl GcTool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvTool;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}