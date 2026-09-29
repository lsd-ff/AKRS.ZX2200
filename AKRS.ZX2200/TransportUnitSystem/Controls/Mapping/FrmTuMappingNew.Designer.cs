namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    partial class FrmTuMappingNew
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.ChkModule = new DevExpress.XtraEditors.CheckEdit();
            this.LueWorkSortType = new DevExpress.XtraEditors.LookUpEdit();
            this.ChkAutoReturn = new DevExpress.XtraEditors.CheckEdit();
            this.ChkSubstrate = new DevExpress.XtraEditors.CheckEdit();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.BtClearAll = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueWorkSortType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkAutoReturn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkSubstrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl3);
            this.panelControl1.Controls.Add(this.BtClearAll);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(241, 785);
            this.panelControl1.TabIndex = 0;
            // 
            // groupControl3
            // 
            this.groupControl3.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl3.AppearanceCaption.Options.UseFont = true;
            this.groupControl3.Controls.Add(this.ChkModule);
            this.groupControl3.Controls.Add(this.LueWorkSortType);
            this.groupControl3.Controls.Add(this.ChkAutoReturn);
            this.groupControl3.Controls.Add(this.ChkSubstrate);
            this.groupControl3.Controls.Add(this.pictureEdit1);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(5, 151);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(227, 270);
            this.groupControl3.TabIndex = 26;
            this.groupControl3.Text = "工作顺序";
            // 
            // ChkModule
            // 
            this.ChkModule.Location = new System.Drawing.Point(147, 26);
            this.ChkModule.Name = "ChkModule";
            this.ChkModule.Properties.Caption = "基岛";
            this.ChkModule.Properties.RadioGroupIndex = 1;
            this.ChkModule.Size = new System.Drawing.Size(75, 20);
            this.ChkModule.TabIndex = 10;
            this.ChkModule.TabStop = false;
            this.ChkModule.CheckedChanged += new System.EventHandler(this.ChkModule_CheckedChanged);
            // 
            // LueWorkSortType
            // 
            this.LueWorkSortType.Location = new System.Drawing.Point(5, 61);
            this.LueWorkSortType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LueWorkSortType.Name = "LueWorkSortType";
            this.LueWorkSortType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueWorkSortType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "")});
            this.LueWorkSortType.Properties.DisplayMember = "Display";
            this.LueWorkSortType.Properties.NullText = "";
            this.LueWorkSortType.Properties.ValueMember = "Value";
            this.LueWorkSortType.Size = new System.Drawing.Size(217, 20);
            this.LueWorkSortType.TabIndex = 8;
            this.LueWorkSortType.EditValueChanged += new System.EventHandler(this.LueWorkSortType_EditValueChanged);
            // 
            // ChkAutoReturn
            // 
            this.ChkAutoReturn.Location = new System.Drawing.Point(4, 225);
            this.ChkAutoReturn.Name = "ChkAutoReturn";
            this.ChkAutoReturn.Properties.Caption = "是否自动转向";
            this.ChkAutoReturn.Size = new System.Drawing.Size(144, 20);
            this.ChkAutoReturn.TabIndex = 2;
            this.ChkAutoReturn.CheckedChanged += new System.EventHandler(this.ChkAutoReturn_CheckedChanged);
            // 
            // ChkSubstrate
            // 
            this.ChkSubstrate.EditValue = true;
            this.ChkSubstrate.Location = new System.Drawing.Point(5, 26);
            this.ChkSubstrate.Name = "ChkSubstrate";
            this.ChkSubstrate.Properties.Caption = "基板";
            this.ChkSubstrate.Properties.RadioGroupIndex = 1;
            this.ChkSubstrate.Size = new System.Drawing.Size(75, 20);
            this.ChkSubstrate.TabIndex = 9;
            this.ChkSubstrate.CheckedChanged += new System.EventHandler(this.ChkSubstrate_CheckedChanged);
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = global::AKRS.ZX2200.Properties.Resources.ToRightUp;
            this.pictureEdit1.Location = new System.Drawing.Point(4, 87);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.StretchHorizontal;
            this.pictureEdit1.Size = new System.Drawing.Size(218, 132);
            this.pictureEdit1.TabIndex = 1;
            // 
            // BtClearAll
            // 
            this.BtClearAll.Appearance.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtClearAll.Appearance.Options.UseFont = true;
            this.BtClearAll.Location = new System.Drawing.Point(5, 722);
            this.BtClearAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtClearAll.Name = "BtClearAll";
            this.BtClearAll.Size = new System.Drawing.Size(227, 52);
            this.BtClearAll.TabIndex = 23;
            this.BtClearAll.Text = "清除所有记忆";
            this.BtClearAll.Click += new System.EventHandler(this.BtClearAll_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(98, 11);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(24, 14);
            this.labelControl8.TabIndex = 22;
            this.labelControl8.Text = "屏蔽";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelControl7.Appearance.Options.UseBackColor = true;
            this.labelControl7.Location = new System.Drawing.Point(12, 11);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(64, 14);
            this.labelControl7.TabIndex = 21;
            this.labelControl7.Text = "                ";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(98, 108);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 20;
            this.labelControl6.Text = "只贴片";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.BackColor = System.Drawing.Color.Crimson;
            this.labelControl5.Appearance.Options.UseBackColor = true;
            this.labelControl5.Location = new System.Drawing.Point(12, 108);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 14);
            this.labelControl5.TabIndex = 19;
            this.labelControl5.Text = "                ";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(98, 74);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 18;
            this.labelControl4.Text = "只点胶";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.BackColor = System.Drawing.Color.Cyan;
            this.labelControl3.Appearance.Options.UseBackColor = true;
            this.labelControl3.Location = new System.Drawing.Point(12, 74);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(64, 14);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "                ";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(98, 44);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "正常";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Lime;
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 44);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(64, 14);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "                ";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(241, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1197, 785);
            this.panelControl2.TabIndex = 1;
            // 
            // FrmTuMappingNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 785);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmTuMappingNew";
            this.Text = "框架Mapping图";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmTuMappingNew_FormClosed);
            this.Load += new System.EventHandler(this.FrmTransportUnitMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ChkModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueWorkSortType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkAutoReturn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkSubstrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.CheckEdit ChkModule;
        private DevExpress.XtraEditors.LookUpEdit LueWorkSortType;
        private DevExpress.XtraEditors.CheckEdit ChkAutoReturn;
        private DevExpress.XtraEditors.CheckEdit ChkSubstrate;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton BtClearAll;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}