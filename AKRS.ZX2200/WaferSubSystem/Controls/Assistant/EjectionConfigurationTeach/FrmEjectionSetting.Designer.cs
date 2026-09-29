namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.EjectionConfigurationTeach
{
    partial class FrmEjectionSetting
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEjectionSetting));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.GcAllocations = new DevExpress.XtraEditors.GroupControl();
            this.CmbEjectionBankRepository = new DevExpress.XtraEditors.ComboBoxEdit();
            this.GcEjectionBank = new DevExpress.XtraGrid.GridControl();
            this.GvEjectionBank = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GridColumn100 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.GcEjectionPool = new DevExpress.XtraGrid.GridControl();
            this.GvEjectionPool = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.GcName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ToolType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.CreateTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtnNozzleManage = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGoLeft = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGoRight = new DevExpress.XtraEditors.SimpleButton();
            this.BtnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.BtOk = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.GcCapDiameter = new DevExpress.XtraEditors.GroupControl();
            this.SpCapDiameter = new DevExpress.XtraEditors.SpinEdit();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).BeginInit();
            this.GcAllocations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionBankRepository.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionPool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionPool)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCapDiameter)).BeginInit();
            this.GcCapDiameter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpCapDiameter.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GcAllocations
            // 
            this.GcAllocations.Controls.Add(this.CmbEjectionBankRepository);
            this.GcAllocations.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcAllocations.Location = new System.Drawing.Point(3, 3);
            this.GcAllocations.Name = "GcAllocations";
            this.GcAllocations.Size = new System.Drawing.Size(435, 62);
            this.GcAllocations.TabIndex = 2;
            this.GcAllocations.Text = "当前顶针架";
            // 
            // CmbEjectionBankRepository
            // 
            this.CmbEjectionBankRepository.Location = new System.Drawing.Point(8, 25);
            this.CmbEjectionBankRepository.Name = "CmbEjectionBankRepository";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbEjectionBankRepository.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbEjectionBankRepository.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbEjectionBankRepository.Size = new System.Drawing.Size(406, 24);
            this.CmbEjectionBankRepository.TabIndex = 5;
            this.CmbEjectionBankRepository.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbEjectionBankRepository_ButtonClick);
            this.CmbEjectionBankRepository.EditValueChanged += new System.EventHandler(this.CmbEjectionBankRepository_EditValueChanged);
            // 
            // GcEjectionBank
            // 
            this.GcEjectionBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcEjectionBank.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionBank.Location = new System.Drawing.Point(2, 23);
            this.GcEjectionBank.MainView = this.GvEjectionBank;
            this.GcEjectionBank.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionBank.Name = "GcEjectionBank";
            this.GcEjectionBank.Size = new System.Drawing.Size(431, 313);
            this.GcEjectionBank.TabIndex = 3;
            this.GcEjectionBank.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvEjectionBank});
            // 
            // GvEjectionBank
            // 
            this.GvEjectionBank.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.GridColumn100});
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
            this.gridColumn1.Caption = "Slot";
            this.gridColumn1.FieldName = "SlotNum";
            this.gridColumn1.MinWidth = 22;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 73;
            // 
            // GridColumn100
            // 
            this.GridColumn100.Caption = "Name";
            this.GridColumn100.FieldName = "Name";
            this.GridColumn100.MinWidth = 22;
            this.GridColumn100.Name = "GridColumn100";
            this.GridColumn100.Visible = true;
            this.GridColumn100.VisibleIndex = 1;
            this.GridColumn100.Width = 162;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.GcEjectionBank);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(9, 91);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(435, 338);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "顶针架配置";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.GcEjectionPool);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(559, 93);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(435, 336);
            this.groupControl2.TabIndex = 6;
            this.groupControl2.Text = "顶针";
            // 
            // GcEjectionPool
            // 
            this.GcEjectionPool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcEjectionPool.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionPool.Location = new System.Drawing.Point(2, 23);
            this.GcEjectionPool.MainView = this.GvEjectionPool;
            this.GcEjectionPool.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEjectionPool.Name = "GcEjectionPool";
            this.GcEjectionPool.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemImageComboBox1});
            this.GcEjectionPool.Size = new System.Drawing.Size(431, 311);
            this.GcEjectionPool.TabIndex = 3;
            this.GcEjectionPool.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvEjectionPool});
            // 
            // GvEjectionPool
            // 
            this.GvEjectionPool.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.GcName,
            this.ToolType,
            this.CreateTime});
            this.GvEjectionPool.DetailHeight = 272;
            this.GvEjectionPool.GridControl = this.GcEjectionPool;
            this.GvEjectionPool.Name = "GvEjectionPool";
            this.GvEjectionPool.OptionsBehavior.Editable = false;
            this.GvEjectionPool.OptionsDetail.EnableMasterViewMode = false;
            this.GvEjectionPool.OptionsView.ShowGroupPanel = false;
            this.GvEjectionPool.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GvEjectionPool_FocusedRowChanged);
            // 
            // GcName
            // 
            this.GcName.Caption = "Name";
            this.GcName.FieldName = "Name";
            this.GcName.MinWidth = 22;
            this.GcName.Name = "GcName";
            this.GcName.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.GcName.OptionsFilter.AllowAutoFilter = false;
            this.GcName.OptionsFilter.AllowFilter = false;
            this.GcName.Visible = true;
            this.GcName.VisibleIndex = 0;
            this.GcName.Width = 82;
            // 
            // ToolType
            // 
            this.ToolType.Caption = "Tool Type";
            this.ToolType.FieldName = "EjectionType";
            this.ToolType.MinWidth = 22;
            this.ToolType.Name = "ToolType";
            this.ToolType.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.ToolType.OptionsFilter.AllowAutoFilter = false;
            this.ToolType.OptionsFilter.AllowFilter = false;
            this.ToolType.Visible = true;
            this.ToolType.VisibleIndex = 1;
            this.ToolType.Width = 82;
            // 
            // CreateTime
            // 
            this.CreateTime.Caption = "Create Time";
            this.CreateTime.FieldName = "CreateTime";
            this.CreateTime.Name = "CreateTime";
            this.CreateTime.OptionsFilter.AllowAutoFilter = false;
            this.CreateTime.OptionsFilter.AllowFilter = false;
            this.CreateTime.Visible = true;
            this.CreateTime.VisibleIndex = 2;
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            this.repositoryItemImageComboBox1.SmallImages = this.imageCollection1;
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "apply_32x32.png");
            this.imageCollection1.Images.SetKeyName(1, "cancel_32x32.png");
            this.imageCollection1.Images.SetKeyName(2, "snapmodifytablecellstyle_32x32.png");
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "NozzleName";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 94;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "gridColumn1";
            this.gridColumn5.FieldName = "SlotName";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 94;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Type";
            this.gridColumn7.FieldName = "NozzleSize";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 94;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "gridColumn2";
            this.gridColumn8.FieldName = "NozzleName";
            this.gridColumn8.MinWidth = 25;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 1;
            this.gridColumn8.Width = 94;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn2";
            this.gridColumn2.FieldName = "NozzleName";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 94;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Maximum tool size";
            this.gridColumn9.FieldName = "MaximumToolSize";
            this.gridColumn9.MinWidth = 25;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 2;
            this.gridColumn9.Width = 94;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Maximum tool size";
            this.gridColumn10.FieldName = "MaximumToolSize";
            this.gridColumn10.MinWidth = 25;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 94;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Maximum tool size";
            this.gridColumn11.FieldName = "MaximumToolSize";
            this.gridColumn11.MinWidth = 25;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 2;
            this.gridColumn11.Width = 94;
            // 
            // BtnNozzleManage
            // 
            this.BtnNozzleManage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnNozzleManage.ImageOptions.Image")));
            this.BtnNozzleManage.Location = new System.Drawing.Point(844, 25);
            this.BtnNozzleManage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnNozzleManage.Name = "BtnNozzleManage";
            this.BtnNozzleManage.Size = new System.Drawing.Size(148, 33);
            this.BtnNozzleManage.TabIndex = 13;
            this.BtnNozzleManage.Text = "顶针管理";
            this.BtnNozzleManage.Click += new System.EventHandler(this.BtnEjectionManage_Click);
            // 
            // BtnGoLeft
            // 
            this.BtnGoLeft.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnGoLeft.ImageOptions.Image")));
            this.BtnGoLeft.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnGoLeft.Location = new System.Drawing.Point(474, 163);
            this.BtnGoLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnGoLeft.Name = "BtnGoLeft";
            this.BtnGoLeft.Size = new System.Drawing.Size(51, 40);
            this.BtnGoLeft.TabIndex = 14;
            this.BtnGoLeft.Click += new System.EventHandler(this.BtnGoLeft_Click);
            // 
            // BtnGoRight
            // 
            this.BtnGoRight.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnGoRight.ImageOptions.Image")));
            this.BtnGoRight.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnGoRight.Location = new System.Drawing.Point(474, 247);
            this.BtnGoRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnGoRight.Name = "BtnGoRight";
            this.BtnGoRight.Size = new System.Drawing.Size(51, 40);
            this.BtnGoRight.TabIndex = 15;
            this.BtnGoRight.Click += new System.EventHandler(this.BtnGoRight_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnAdd.ImageOptions.Image")));
            this.BtnAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnAdd.Location = new System.Drawing.Point(561, 23);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(103, 34);
            this.BtnAdd.TabIndex = 16;
            this.BtnAdd.Text = "增加";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.ImageOptions.Image")));
            this.BtnDelete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnDelete.Location = new System.Drawing.Point(706, 23);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(103, 34);
            this.BtnDelete.TabIndex = 17;
            this.BtnDelete.Text = "删除";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(350, 520);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(94, 32);
            this.BtOk.TabIndex = 18;
            this.BtOk.Text = "确定";
            this.BtOk.Click += new System.EventHandler(this.BtOk_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(559, 520);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(90, 32);
            this.BtCancel.TabIndex = 19;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // GcCapDiameter
            // 
            this.GcCapDiameter.Controls.Add(this.SpCapDiameter);
            this.GcCapDiameter.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcCapDiameter.Location = new System.Drawing.Point(559, 435);
            this.GcCapDiameter.Name = "GcCapDiameter";
            this.GcCapDiameter.Size = new System.Drawing.Size(435, 68);
            this.GcCapDiameter.TabIndex = 20;
            this.GcCapDiameter.Text = "顶针帽直径";
            // 
            // SpCapDiameter
            // 
            this.SpCapDiameter.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCapDiameter.Location = new System.Drawing.Point(5, 31);
            this.SpCapDiameter.Name = "SpCapDiameter";
            this.SpCapDiameter.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCapDiameter.Properties.MaskSettings.Set("mask", "");
            this.SpCapDiameter.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpCapDiameter.Size = new System.Drawing.Size(425, 20);
            this.SpCapDiameter.TabIndex = 10;
            this.SpCapDiameter.EditValueChanged += new System.EventHandler(this.SpCapDiameter_EditValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "FrmEjectionSetting";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmEjectionSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 607);
            this.Controls.Add(this.GcCapDiameter);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtOk);
            this.Controls.Add(this.BtnAdd);
            this.Controls.Add(this.BtnDelete);
            this.Controls.Add(this.BtnGoRight);
            this.Controls.Add(this.BtnGoLeft);
            this.Controls.Add(this.BtnNozzleManage);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.GcAllocations);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmEjectionSetting";
            this.Text = "顶针架配置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEjectionSetting_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).EndInit();
            this.GcAllocations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionBankRepository.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcEjectionPool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEjectionPool)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcCapDiameter)).EndInit();
            this.GcCapDiameter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpCapDiameter.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl GcAllocations;
        private DevExpress.XtraGrid.GridControl GcEjectionBank;
        private DevExpress.XtraGrid.Views.Grid.GridView GvEjectionBank;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn GridColumn100;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl GcEjectionPool;
        private DevExpress.XtraGrid.Views.Grid.GridView GvEjectionPool;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn ToolType;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn GcName;
        private DevExpress.XtraEditors.ComboBoxEdit CmbEjectionBankRepository;
        private DevExpress.XtraEditors.SimpleButton BtnNozzleManage;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        private DevExpress.XtraEditors.SimpleButton BtnGoLeft;
        private DevExpress.XtraEditors.SimpleButton BtnGoRight;
        private DevExpress.XtraEditors.SimpleButton BtnAdd;
        private DevExpress.XtraEditors.SimpleButton BtnDelete;
        private DevExpress.XtraEditors.SimpleButton BtOk;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.GroupControl GcCapDiameter;
        private DevExpress.XtraEditors.SpinEdit SpCapDiameter;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Columns.GridColumn CreateTime;
    }
}
