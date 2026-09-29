namespace AKRS.ZX2200.WaferSubSystem.Controls.Manual
{
    partial class UcEjectBank
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GcEjectionBank = new DevExpress.XtraGrid.GridControl();
            this.GvEjectionBank = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridColumn100 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CmbEjectionName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.BtnMeasurement = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMaintenance = new DevExpress.XtraEditors.SimpleButton();
            this.BtnChangeTool = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GcEjectionBank
            // 
            this.GcEjectionBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcEjectionBank.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionBank.Location = new System.Drawing.Point(2, 23);
            this.GcEjectionBank.MainView = this.GvEjectionBank;
            this.GcEjectionBank.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionBank.Name = "GcEjectionBank";
            this.GcEjectionBank.Size = new System.Drawing.Size(395, 178);
            this.GcEjectionBank.TabIndex = 3;
            this.GcEjectionBank.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvEjectionBank});
            // 
            // GvEjectionBank
            // 
            this.GvEjectionBank.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.GridColumn100,
            this.gridColumn3});
            this.GvEjectionBank.DetailHeight = 272;
            this.GvEjectionBank.GridControl = this.GcEjectionBank;
            this.GvEjectionBank.Name = "GvEjectionBank";
            this.GvEjectionBank.OptionsBehavior.Editable = false;
            this.GvEjectionBank.OptionsBehavior.ReadOnly = true;
            this.GvEjectionBank.OptionsCustomization.AllowSort = false;
            this.GvEjectionBank.OptionsDetail.EnableMasterViewMode = false;
            this.GvEjectionBank.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "槽号";
            this.gridColumn1.FieldName = "EjectionBankSlotConfig.SlotNum";
            this.gridColumn1.MinWidth = 22;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 87;
            // 
            // GridColumn100
            // 
            this.GridColumn100.Caption = "槽状态";
            this.GridColumn100.FieldName = "SlotStateStr";
            this.GridColumn100.MinWidth = 22;
            this.GridColumn100.Name = "GridColumn100";
            this.GridColumn100.Visible = true;
            this.GridColumn100.VisibleIndex = 1;
            this.GridColumn100.Width = 102;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "顶针名称";
            this.gridColumn3.FieldName = "EjectionBankSlotConfig.Name";
            this.gridColumn3.MinWidth = 22;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 246;
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.GcEjectionBank);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(3, 3);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(399, 203);
            this.groupControl2.TabIndex = 17;
            // 
            // groupControl1
            // 
            this.groupControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl1.Controls.Add(this.CmbEjectionName);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(3, 212);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(399, 82);
            this.groupControl1.TabIndex = 17;
            this.groupControl1.Tag = "Current tool";
            this.groupControl1.Text = "当前顶针";
            // 
            // CmbEjectionName
            // 
            this.CmbEjectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbEjectionName.Location = new System.Drawing.Point(5, 38);
            this.CmbEjectionName.Name = "CmbEjectionName";
            this.CmbEjectionName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbEjectionName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbEjectionName.Size = new System.Drawing.Size(382, 20);
            this.CmbEjectionName.TabIndex = 42;
            // 
            // groupControl3
            // 
            this.groupControl3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl3.Controls.Add(this.BtnMeasurement);
            this.groupControl3.Controls.Add(this.BtnMaintenance);
            this.groupControl3.Controls.Add(this.BtnChangeTool);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(3, 300);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(399, 130);
            this.groupControl3.TabIndex = 17;
            // 
            // BtnMeasurement
            // 
            this.BtnMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMeasurement.Location = new System.Drawing.Point(5, 84);
            this.BtnMeasurement.Name = "BtnMeasurement";
            this.BtnMeasurement.Size = new System.Drawing.Size(382, 23);
            this.BtnMeasurement.TabIndex = 2;
            this.BtnMeasurement.Tag = "Measurement";
            this.BtnMeasurement.Text = "顶针测试";
            this.BtnMeasurement.Click += new System.EventHandler(this.BtnMeasurement_Click);
            // 
            // BtnMaintenance
            // 
            this.BtnMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnMaintenance.Location = new System.Drawing.Point(5, 55);
            this.BtnMaintenance.Name = "BtnMaintenance";
            this.BtnMaintenance.Size = new System.Drawing.Size(382, 23);
            this.BtnMaintenance.TabIndex = 1;
            this.BtnMaintenance.Tag = "Maintenance";
            this.BtnMaintenance.Text = "归还顶针";
            this.BtnMaintenance.Click += new System.EventHandler(this.BtnMaintenance_Click);
            // 
            // BtnChangeTool
            // 
            this.BtnChangeTool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnChangeTool.Location = new System.Drawing.Point(5, 26);
            this.BtnChangeTool.Name = "BtnChangeTool";
            this.BtnChangeTool.Size = new System.Drawing.Size(382, 23);
            this.BtnChangeTool.TabIndex = 0;
            this.BtnChangeTool.Tag = "Change tool";
            this.BtnChangeTool.Text = "更换顶针";
            this.BtnChangeTool.Click += new System.EventHandler(this.BtnChangeTool_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "UcEjectBank";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // UcEjectBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl2);
            this.Name = "UcEjectBank";
            this.Size = new System.Drawing.Size(405, 442);
            this.Load += new System.EventHandler(this.UcEjectBank_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraGrid.GridControl GcEjectionBank;
        private DevExpress.XtraGrid.Views.Grid.GridView GvEjectionBank;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn GridColumn100;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton BtnMaintenance;
        private DevExpress.XtraEditors.SimpleButton BtnChangeTool;
        private DevExpress.XtraEditors.ComboBoxEdit CmbEjectionName;
        private DevExpress.XtraEditors.SimpleButton BtnMeasurement;
        private System.Windows.Forms.Timer timer1;
    }
}
