namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    partial class FrmAddObject
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
            this.BtSure = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.CmbType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.SpOffsetX = new DevExpress.XtraEditors.SpinEdit();
            this.SpOffsetY = new DevExpress.XtraEditors.SpinEdit();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.TxName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.CmbSubstrate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.CmbModule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.SpOffsetZ = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.ChkAbsoluteOffset = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.CmbType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbSubstrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkAbsoluteOffset.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtSure
            // 
            this.BtSure.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtSure.Appearance.Options.UseFont = true;
            this.BtSure.Location = new System.Drawing.Point(256, 426);
            this.BtSure.Name = "BtSure";
            this.BtSure.Size = new System.Drawing.Size(155, 85);
            this.BtSure.TabIndex = 0;
            this.BtSure.Text = "确定";
            this.BtSure.Click += new System.EventHandler(this.BtSure_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(12, 183);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(40, 24);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "名称";
            // 
            // CmbType
            // 
            this.CmbType.Enabled = false;
            this.CmbType.Location = new System.Drawing.Point(139, 6);
            this.CmbType.Name = "CmbType";
            this.CmbType.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbType.Properties.Appearance.Options.UseFont = true;
            this.CmbType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbType.Size = new System.Drawing.Size(204, 30);
            this.CmbType.TabIndex = 2;
            this.CmbType.SelectedIndexChanged += new System.EventHandler(this.CmbType_SelectedIndexChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(12, 12);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(40, 24);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "类型";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(12, 33);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(72, 24);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "偏移值X";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Location = new System.Drawing.Point(12, 67);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(72, 24);
            this.labelControl5.TabIndex = 8;
            this.labelControl5.Text = "偏移值Y";
            // 
            // SpOffsetX
            // 
            this.SpOffsetX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOffsetX.Location = new System.Drawing.Point(138, 28);
            this.SpOffsetX.Name = "SpOffsetX";
            this.SpOffsetX.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpOffsetX.Properties.Appearance.Options.UseFont = true;
            this.SpOffsetX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOffsetX.Size = new System.Drawing.Size(204, 30);
            this.SpOffsetX.TabIndex = 11;
            // 
            // SpOffsetY
            // 
            this.SpOffsetY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOffsetY.Location = new System.Drawing.Point(138, 64);
            this.SpOffsetY.Name = "SpOffsetY";
            this.SpOffsetY.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpOffsetY.Properties.Appearance.Options.UseFont = true;
            this.SpOffsetY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOffsetY.Size = new System.Drawing.Size(204, 30);
            this.SpOffsetY.TabIndex = 12;
            // 
            // BtCancel
            // 
            this.BtCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCancel.Appearance.Options.UseFont = true;
            this.BtCancel.Location = new System.Drawing.Point(12, 426);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(155, 85);
            this.BtCancel.TabIndex = 15;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // TxName
            // 
            this.TxName.Location = new System.Drawing.Point(139, 177);
            this.TxName.Name = "TxName";
            this.TxName.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxName.Properties.Appearance.Options.UseFont = true;
            this.TxName.Size = new System.Drawing.Size(204, 30);
            this.TxName.TabIndex = 16;
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl7.Appearance.Options.UseFont = true;
            this.labelControl7.Location = new System.Drawing.Point(12, 65);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(80, 24);
            this.labelControl7.TabIndex = 18;
            this.labelControl7.Text = "所在基板";
            // 
            // CmbSubstrate
            // 
            this.CmbSubstrate.Enabled = false;
            this.CmbSubstrate.Location = new System.Drawing.Point(139, 59);
            this.CmbSubstrate.Name = "CmbSubstrate";
            this.CmbSubstrate.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbSubstrate.Properties.Appearance.Options.UseFont = true;
            this.CmbSubstrate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbSubstrate.Size = new System.Drawing.Size(204, 30);
            this.CmbSubstrate.TabIndex = 17;
            // 
            // labelControl8
            // 
            this.labelControl8.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl8.Appearance.Options.UseFont = true;
            this.labelControl8.Location = new System.Drawing.Point(12, 126);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(80, 24);
            this.labelControl8.TabIndex = 20;
            this.labelControl8.Text = "所在基岛";
            // 
            // CmbModule
            // 
            this.CmbModule.Enabled = false;
            this.CmbModule.Location = new System.Drawing.Point(139, 120);
            this.CmbModule.Name = "CmbModule";
            this.CmbModule.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbModule.Properties.Appearance.Options.UseFont = true;
            this.CmbModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbModule.Size = new System.Drawing.Size(204, 30);
            this.CmbModule.TabIndex = 19;
            // 
            // SpOffsetZ
            // 
            this.SpOffsetZ.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOffsetZ.Location = new System.Drawing.Point(138, 100);
            this.SpOffsetZ.Name = "SpOffsetZ";
            this.SpOffsetZ.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpOffsetZ.Properties.Appearance.Options.UseFont = true;
            this.SpOffsetZ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOffsetZ.Size = new System.Drawing.Size(204, 30);
            this.SpOffsetZ.TabIndex = 22;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(13, 103);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(71, 24);
            this.labelControl3.TabIndex = 21;
            this.labelControl3.Text = "偏移值Z";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl9);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.SpOffsetY);
            this.groupControl1.Controls.Add(this.SpOffsetZ);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.SpOffsetX);
            this.groupControl1.Enabled = false;
            this.groupControl1.Location = new System.Drawing.Point(2, 283);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(420, 137);
            this.groupControl1.TabIndex = 23;
            this.groupControl1.Text = "偏移值设定";
            // 
            // ChkAbsoluteOffset
            // 
            this.ChkAbsoluteOffset.Location = new System.Drawing.Point(14, 250);
            this.ChkAbsoluteOffset.Name = "ChkAbsoluteOffset";
            this.ChkAbsoluteOffset.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAbsoluteOffset.Properties.Appearance.Options.UseFont = true;
            this.ChkAbsoluteOffset.Properties.Caption = "绝对偏移";
            this.ChkAbsoluteOffset.Size = new System.Drawing.Size(149, 27);
            this.ChkAbsoluteOffset.TabIndex = 25;
            this.ChkAbsoluteOffset.TabStop = false;
            this.ChkAbsoluteOffset.CheckedChanged += new System.EventHandler(this.ChkAbsoluteOffset_CheckedChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(348, 33);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(34, 24);
            this.labelControl6.TabIndex = 23;
            this.labelControl6.Text = "mm";
            // 
            // labelControl9
            // 
            this.labelControl9.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl9.Appearance.Options.UseFont = true;
            this.labelControl9.Location = new System.Drawing.Point(348, 67);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(34, 24);
            this.labelControl9.TabIndex = 24;
            this.labelControl9.Text = "mm";
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Location = new System.Drawing.Point(348, 103);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(34, 24);
            this.labelControl10.TabIndex = 25;
            this.labelControl10.Text = "mm";
            // 
            // FrmAddObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(423, 519);
            this.Controls.Add(this.ChkAbsoluteOffset);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.CmbModule);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.CmbSubstrate);
            this.Controls.Add(this.TxName);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.CmbType);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtSure);
            this.Name = "FrmAddObject";
            this.Text = "新增参数";
            this.Load += new System.EventHandler(this.FrmAddObject_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CmbType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbSubstrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkAbsoluteOffset.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtSure;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit CmbType;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit SpOffsetX;
        private DevExpress.XtraEditors.SpinEdit SpOffsetY;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.TextEdit TxName;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.ComboBoxEdit CmbSubstrate;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.ComboBoxEdit CmbModule;
        private DevExpress.XtraEditors.SpinEdit SpOffsetZ;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit ChkAbsoluteOffset;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}