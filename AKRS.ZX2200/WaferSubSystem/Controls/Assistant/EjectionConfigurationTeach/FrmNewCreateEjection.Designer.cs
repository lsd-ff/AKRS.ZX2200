namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach
{
    partial class FrmNewCreateEjection
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.TxtName = new DevExpress.XtraEditors.TextEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LueToolType = new DevExpress.XtraEditors.LookUpEdit();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.GcTool = new DevExpress.XtraGrid.GridControl();
            this.GvTool = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueToolType.Properties)).BeginInit();
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
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(385, 68);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Name for Ejection system";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(39, 33);
            this.TxtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(306, 20);
            this.TxtName.TabIndex = 1;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.LueToolType);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 68);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(385, 68);
            this.groupControl2.TabIndex = 2;
            this.groupControl2.Text = "Tool  type";
            // 
            // LueToolType
            // 
            this.LueToolType.Location = new System.Drawing.Point(39, 31);
            this.LueToolType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LueToolType.Name = "LueToolType";
            this.LueToolType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueToolType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 17, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueToolType.Properties.DisplayMember = "Display";
            this.LueToolType.Properties.DropDownRows = 5;
            this.LueToolType.Properties.NullText = "";
            this.LueToolType.Properties.ValueMember = "Value";
            this.LueToolType.Size = new System.Drawing.Size(306, 20);
            this.LueToolType.TabIndex = 29;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 136);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(385, 436);
            this.xtraTabControl1.TabIndex = 4;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.GcTool);
            this.xtraTabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(383, 410);
            this.xtraTabPage1.Text = "Dataset";
            // 
            // GcTool
            // 
            this.GcTool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcTool.Location = new System.Drawing.Point(0, 0);
            this.GcTool.MainView = this.GvTool;
            this.GcTool.Name = "GcTool";
            this.GcTool.Size = new System.Drawing.Size(383, 410);
            this.GcTool.TabIndex = 0;
            this.GcTool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvTool});
            // 
            // GvTool
            // 
            this.GvTool.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1});
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
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 175;
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 524);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(385, 48);
            this.tablePanel1.TabIndex = 22;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 1);
            this.BtCancel.Location = new System.Drawing.Point(226, 9);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(127, 29);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "Cancel";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtnGenerate
            // 
            this.tablePanel1.SetColumn(this.BtnGenerate, 0);
            this.BtnGenerate.Location = new System.Drawing.Point(33, 9);
            this.BtnGenerate.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtnGenerate.Name = "BtnGenerate";
            this.tablePanel1.SetRow(this.BtnGenerate, 0);
            this.BtnGenerate.Size = new System.Drawing.Size(127, 29);
            this.BtnGenerate.TabIndex = 6;
            this.BtnGenerate.Text = "Generate";
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "FrmNewCreateEjection";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmNewCreateEjection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 572);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmNewCreateEjection";
            this.Text = "New    dataset";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmNewCreateEjection_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TxtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueToolType.Properties)).EndInit();
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
        private DevExpress.XtraEditors.LookUpEdit LueToolType;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraGrid.GridControl GcTool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvTool;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.Timer timer1;
    }
}