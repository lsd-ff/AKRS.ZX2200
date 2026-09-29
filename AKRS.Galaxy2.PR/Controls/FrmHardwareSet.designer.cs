namespace AKRS.Galaxy2.PR.Controls
{
    partial class FrmHardwareSet
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
            this.RgCalibAngleType = new DevExpress.XtraEditors.RadioGroup();
            this.SpCalibAngle = new DevExpress.XtraEditors.SpinEdit();
            this.SpOriginalPointXOrY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.LueCamera = new DevExpress.XtraEditors.LookUpEdit();
            this.BsCamera = new System.Windows.Forms.BindingSource(this.components);
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.GcPRLights = new DevExpress.XtraGrid.GridControl();
            this.BsPRLight = new System.Windows.Forms.BindingSource(this.components);
            this.GvPRLights = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RiChkIsUse = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RiLueHardware = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.BsHardware = new System.Windows.Forms.BindingSource(this.components);
            this.RiLueParam = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.BsParam = new System.Windows.Forms.BindingSource(this.components);
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtOk = new DevExpress.XtraEditors.SimpleButton();
            this.CmbModule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LueAxisZ = new DevExpress.XtraEditors.LookUpEdit();
            this.BsAxis = new System.Windows.Forms.BindingSource(this.components);
            this.LueAxisY = new DevExpress.XtraEditors.LookUpEdit();
            this.LueAxisX = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.CheckAxisX = new System.Windows.Forms.CheckBox();
            this.CheckAxisY = new System.Windows.Forms.CheckBox();
            this.CheckAxisZ = new System.Windows.Forms.CheckBox();
            this.BsModule = new System.Windows.Forms.BindingSource(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RgCalibAngleType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCalibAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginalPointXOrY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueCamera.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsCamera)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcPRLights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsPRLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPRLights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiChkIsUse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiLueHardware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsHardware)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiLueParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsModule)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.RgCalibAngleType);
            this.groupControl1.Controls.Add(this.SpCalibAngle);
            this.groupControl1.Controls.Add(this.SpOriginalPointXOrY);
            this.groupControl1.Controls.Add(this.labelControl9);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.LueCamera);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(494, 69);
            this.groupControl1.TabIndex = 6;
            this.groupControl1.Text = "相机和标定";
            // 
            // RgCalibAngleType
            // 
            this.RgCalibAngleType.Location = new System.Drawing.Point(90, 71);
            this.RgCalibAngleType.Name = "RgCalibAngleType";
            this.RgCalibAngleType.Properties.Columns = 2;
            this.RgCalibAngleType.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem("X", "与X夹角"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem("Y", "与Y夹角")});
            this.RgCalibAngleType.Size = new System.Drawing.Size(221, 30);
            this.RgCalibAngleType.TabIndex = 28;
            this.RgCalibAngleType.Visible = false;
            // 
            // SpCalibAngle
            // 
            this.SpCalibAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCalibAngle.Location = new System.Drawing.Point(302, 112);
            this.SpCalibAngle.Name = "SpCalibAngle";
            this.SpCalibAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCalibAngle.Size = new System.Drawing.Size(93, 22);
            this.SpCalibAngle.TabIndex = 27;
            this.SpCalibAngle.Visible = false;
            // 
            // SpOriginalPointXOrY
            // 
            this.SpOriginalPointXOrY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOriginalPointXOrY.Location = new System.Drawing.Point(166, 112);
            this.SpOriginalPointXOrY.Name = "SpOriginalPointXOrY";
            this.SpOriginalPointXOrY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOriginalPointXOrY.Size = new System.Drawing.Size(81, 22);
            this.SpOriginalPointXOrY.TabIndex = 26;
            this.SpOriginalPointXOrY.Visible = false;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(254, 114);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(48, 16);
            this.labelControl9.TabIndex = 24;
            this.labelControl9.Text = "Y角度:";
            this.labelControl9.Visible = false;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(80, 115);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(80, 16);
            this.labelControl8.TabIndex = 23;
            this.labelControl8.Text = "标定原点Y:";
            this.labelControl8.Visible = false;
            // 
            // LueCamera
            // 
            this.LueCamera.Location = new System.Drawing.Point(90, 34);
            this.LueCamera.Name = "LueCamera";
            this.LueCamera.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueCamera.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("HardwareName", "名称", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueCamera.Properties.DataSource = this.BsCamera;
            this.LueCamera.Properties.DisplayMember = "HardwareName";
            this.LueCamera.Properties.NullText = "";
            this.LueCamera.Properties.ValueMember = "HardwareName";
            this.LueCamera.Size = new System.Drawing.Size(304, 22);
            this.LueCamera.TabIndex = 1;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.groupControl3);
            this.groupControl2.Controls.Add(this.BtCancel);
            this.groupControl2.Controls.Add(this.BtOk);
            this.groupControl2.Controls.Add(this.CmbModule);
            this.groupControl2.Controls.Add(this.LueAxisZ);
            this.groupControl2.Controls.Add(this.LueAxisY);
            this.groupControl2.Controls.Add(this.LueAxisX);
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.CheckAxisX);
            this.groupControl2.Controls.Add(this.CheckAxisY);
            this.groupControl2.Controls.Add(this.CheckAxisZ);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(0, 69);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(494, 739);
            this.groupControl2.TabIndex = 7;
            this.groupControl2.Text = "轴";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.GcPRLights);
            this.groupControl3.Location = new System.Drawing.Point(14, 294);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(466, 294);
            this.groupControl3.TabIndex = 41;
            this.groupControl3.Text = "光源";
            // 
            // GcPRLights
            // 
            this.GcPRLights.DataSource = this.BsPRLight;
            this.GcPRLights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcPRLights.Location = new System.Drawing.Point(2, 23);
            this.GcPRLights.MainView = this.GvPRLights;
            this.GcPRLights.Name = "GcPRLights";
            this.GcPRLights.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RiChkIsUse,
            this.RiLueParam,
            this.RiLueHardware});
            this.GcPRLights.Size = new System.Drawing.Size(462, 269);
            this.GcPRLights.TabIndex = 35;
            this.GcPRLights.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvPRLights});
            // 
            // GvPRLights
            // 
            this.GvPRLights.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.GvPRLights.DetailHeight = 400;
            this.GvPRLights.GridControl = this.GcPRLights;
            this.GvPRLights.Name = "GvPRLights";
            this.GvPRLights.OptionsView.ShowGroupPanel = false;
            this.GvPRLights.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GvPRLights_RowCellClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "使用";
            this.gridColumn1.ColumnEdit = this.RiChkIsUse;
            this.gridColumn1.FieldName = "IsUse";
            this.gridColumn1.MinWidth = 23;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 57;
            // 
            // RiChkIsUse
            // 
            this.RiChkIsUse.AutoHeight = false;
            this.RiChkIsUse.Name = "RiChkIsUse";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "光源";
            this.gridColumn2.ColumnEdit = this.RiLueHardware;
            this.gridColumn2.FieldName = "LightName";
            this.gridColumn2.MinWidth = 23;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 158;
            // 
            // RiLueHardware
            // 
            this.RiLueHardware.AutoHeight = false;
            this.RiLueHardware.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RiLueHardware.DataSource = this.BsHardware;
            this.RiLueHardware.DisplayMember = "HardwareName";
            this.RiLueHardware.Name = "RiLueHardware";
            this.RiLueHardware.NullText = "";
            this.RiLueHardware.ValueMember = "HardwareName";
            // 
            // RiLueParam
            // 
            this.RiLueParam.AutoHeight = false;
            this.RiLueParam.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RiLueParam.DataSource = this.BsParam;
            this.RiLueParam.DisplayMember = "Name";
            this.RiLueParam.Name = "RiLueParam";
            this.RiLueParam.NullText = "";
            this.RiLueParam.ValueMember = "ID";
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(291, 617);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(86, 26);
            this.BtCancel.TabIndex = 34;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(106, 617);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(86, 26);
            this.BtOk.TabIndex = 33;
            this.BtOk.Text = "确定";
            this.BtOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // CmbModule
            // 
            this.CmbModule.Location = new System.Drawing.Point(90, 50);
            this.CmbModule.Name = "CmbModule";
            this.CmbModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbModule.Size = new System.Drawing.Size(304, 22);
            this.CmbModule.TabIndex = 31;
            this.CmbModule.SelectedIndexChanged += new System.EventHandler(this.CmbModule_SelectedIndexChanged);
            // 
            // LueAxisZ
            // 
            this.LueAxisZ.Location = new System.Drawing.Point(90, 226);
            this.LueAxisZ.Name = "LueAxisZ";
            this.LueAxisZ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAxisZ.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("HardwareName", "名称", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueAxisZ.Properties.DataSource = this.BsAxis;
            this.LueAxisZ.Properties.DisplayMember = "HardwareName";
            this.LueAxisZ.Properties.NullText = "";
            this.LueAxisZ.Properties.ValueMember = "HardwareName";
            this.LueAxisZ.Size = new System.Drawing.Size(178, 22);
            this.LueAxisZ.TabIndex = 30;
            // 
            // LueAxisY
            // 
            this.LueAxisY.Location = new System.Drawing.Point(90, 179);
            this.LueAxisY.Name = "LueAxisY";
            this.LueAxisY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAxisY.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("HardwareName", "名称", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueAxisY.Properties.DataSource = this.BsAxis;
            this.LueAxisY.Properties.DisplayMember = "HardwareName";
            this.LueAxisY.Properties.NullText = "";
            this.LueAxisY.Properties.ValueMember = "HardwareName";
            this.LueAxisY.Size = new System.Drawing.Size(179, 22);
            this.LueAxisY.TabIndex = 29;
            // 
            // LueAxisX
            // 
            this.LueAxisX.Location = new System.Drawing.Point(90, 133);
            this.LueAxisX.Name = "LueAxisX";
            this.LueAxisX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAxisX.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("HardwareName", "名称", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueAxisX.Properties.DataSource = this.BsAxis;
            this.LueAxisX.Properties.DisplayMember = "HardwareName";
            this.LueAxisX.Properties.NullText = "";
            this.LueAxisX.Properties.ValueMember = "HardwareName";
            this.LueAxisX.Size = new System.Drawing.Size(179, 22);
            this.LueAxisX.TabIndex = 28;
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.ForeColor = System.Drawing.Color.Teal;
            this.labelControl6.Appearance.Options.UseForeColor = true;
            this.labelControl6.Location = new System.Drawing.Point(13, 88);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(728, 16);
            this.labelControl6.TabIndex = 26;
            this.labelControl6.Text = "---------------------------------------------------------------------------------" +
    "----------";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(54, 231);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(32, 16);
            this.labelControl4.TabIndex = 24;
            this.labelControl4.Text = "Z轴:";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(54, 186);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(32, 16);
            this.labelControl3.TabIndex = 23;
            this.labelControl3.Text = "Y轴:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(54, 139);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(32, 16);
            this.labelControl2.TabIndex = 22;
            this.labelControl2.Text = "X轴:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(48, 50);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 16);
            this.labelControl1.TabIndex = 21;
            this.labelControl1.Text = "模组:";
            // 
            // CheckAxisX
            // 
            this.CheckAxisX.AutoSize = true;
            this.CheckAxisX.Checked = true;
            this.CheckAxisX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckAxisX.Location = new System.Drawing.Point(289, 135);
            this.CheckAxisX.Name = "CheckAxisX";
            this.CheckAxisX.Size = new System.Drawing.Size(15, 14);
            this.CheckAxisX.TabIndex = 8;
            this.CheckAxisX.UseVisualStyleBackColor = true;
            // 
            // CheckAxisY
            // 
            this.CheckAxisY.AutoSize = true;
            this.CheckAxisY.Checked = true;
            this.CheckAxisY.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckAxisY.Location = new System.Drawing.Point(289, 182);
            this.CheckAxisY.Name = "CheckAxisY";
            this.CheckAxisY.Size = new System.Drawing.Size(15, 14);
            this.CheckAxisY.TabIndex = 9;
            this.CheckAxisY.UseVisualStyleBackColor = true;
            // 
            // CheckAxisZ
            // 
            this.CheckAxisZ.AutoSize = true;
            this.CheckAxisZ.Checked = true;
            this.CheckAxisZ.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckAxisZ.Location = new System.Drawing.Point(289, 229);
            this.CheckAxisZ.Name = "CheckAxisZ";
            this.CheckAxisZ.Size = new System.Drawing.Size(15, 14);
            this.CheckAxisZ.TabIndex = 10;
            this.CheckAxisZ.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // FrmHardwareSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 808);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.MaximizeBox = false;
            this.Name = "FrmHardwareSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "硬件配置";
            this.Load += new System.EventHandler(this.FormHardwareSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RgCalibAngleType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCalibAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginalPointXOrY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueCamera.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsCamera)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcPRLights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsPRLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPRLights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiChkIsUse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiLueHardware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsHardware)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RiLueParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueAxisX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BsModule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.CheckBox CheckAxisX;
        private System.Windows.Forms.CheckBox CheckAxisY;
        private System.Windows.Forms.CheckBox CheckAxisZ;
        private DevExpress.XtraEditors.LookUpEdit LueCamera;
        private System.Windows.Forms.BindingSource BsAxis;
        private DevExpress.XtraEditors.LookUpEdit LueAxisZ;
        private DevExpress.XtraEditors.LookUpEdit LueAxisY;
        private DevExpress.XtraEditors.LookUpEdit LueAxisX;
        private System.Windows.Forms.BindingSource BsCamera;
        private System.Windows.Forms.BindingSource BsModule;
        private DevExpress.XtraEditors.ComboBoxEdit CmbModule;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtOk;
        private DevExpress.XtraGrid.GridControl GcPRLights;
        private DevExpress.XtraGrid.Views.Grid.GridView GvPRLights;
        private System.Windows.Forms.BindingSource BsPRLight;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RiChkIsUse;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit RiLueHardware;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit RiLueParam;
        private System.Windows.Forms.BindingSource BsHardware;
        private System.Windows.Forms.BindingSource BsParam;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SpinEdit SpCalibAngle;
        private DevExpress.XtraEditors.SpinEdit SpOriginalPointXOrY;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.RadioGroup RgCalibAngleType;
    }
}