namespace AKRS.ZX2200.SupportFeature.Calibrate
{
    partial class FrmSystem2Calibrate
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
            this.CmbEjectionName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.CmbComponent = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BtnCaliEjection = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCaliPickOffset = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCaliAll = new DevExpress.XtraEditors.SimpleButton();
            this.CmbNozzleName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BtnCaliNozzle = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.CmbComponent1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbComponent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzleName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbComponent1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // CmbEjectionName
            // 
            this.CmbEjectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbEjectionName.Enabled = false;
            this.CmbEjectionName.Location = new System.Drawing.Point(143, 103);
            this.CmbEjectionName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbEjectionName.Name = "CmbEjectionName";
            this.CmbEjectionName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbEjectionName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbEjectionName.Size = new System.Drawing.Size(286, 24);
            this.CmbEjectionName.TabIndex = 42;
            // 
            // CmbComponent
            // 
            this.CmbComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbComponent.Location = new System.Drawing.Point(87, 83);
            this.CmbComponent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbComponent.Name = "CmbComponent";
            this.CmbComponent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbComponent.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbComponent.Size = new System.Drawing.Size(286, 24);
            this.CmbComponent.TabIndex = 46;
            // 
            // BtnCaliEjection
            // 
            this.BtnCaliEjection.Location = new System.Drawing.Point(580, 60);
            this.BtnCaliEjection.Name = "BtnCaliEjection";
            this.BtnCaliEjection.Size = new System.Drawing.Size(154, 58);
            this.BtnCaliEjection.TabIndex = 50;
            this.BtnCaliEjection.Text = "校准顶针";
            this.BtnCaliEjection.Click += new System.EventHandler(this.BtnCaliEjection_Click);
            // 
            // BtnCaliPickOffset
            // 
            this.BtnCaliPickOffset.Location = new System.Drawing.Point(580, 66);
            this.BtnCaliPickOffset.Name = "BtnCaliPickOffset";
            this.BtnCaliPickOffset.Size = new System.Drawing.Size(154, 58);
            this.BtnCaliPickOffset.TabIndex = 52;
            this.BtnCaliPickOffset.Text = "校准取片偏移";
            this.BtnCaliPickOffset.Click += new System.EventHandler(this.BtnCaliPickOffset_Click);
            // 
            // BtnCaliAll
            // 
            this.BtnCaliAll.Location = new System.Drawing.Point(399, 529);
            this.BtnCaliAll.Name = "BtnCaliAll";
            this.BtnCaliAll.Size = new System.Drawing.Size(154, 58);
            this.BtnCaliAll.TabIndex = 53;
            this.BtnCaliAll.Text = "一键全部校准";
            this.BtnCaliAll.Click += new System.EventHandler(this.BtnCaliAll_Click);
            // 
            // CmbNozzleName
            // 
            this.CmbNozzleName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbNozzleName.Location = new System.Drawing.Point(76, 77);
            this.CmbNozzleName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbNozzleName.Name = "CmbNozzleName";
            this.CmbNozzleName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbNozzleName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbNozzleName.Size = new System.Drawing.Size(285, 24);
            this.CmbNozzleName.TabIndex = 44;
            // 
            // BtnCaliNozzle
            // 
            this.BtnCaliNozzle.Location = new System.Drawing.Point(580, 60);
            this.BtnCaliNozzle.Name = "BtnCaliNozzle";
            this.BtnCaliNozzle.Size = new System.Drawing.Size(154, 58);
            this.BtnCaliNozzle.TabIndex = 51;
            this.BtnCaliNozzle.Text = "校准吸嘴";
            this.BtnCaliNozzle.Click += new System.EventHandler(this.BtnCaliNozzle_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(75, 31);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(672, 18);
            this.labelControl4.TabIndex = 54;
            this.labelControl4.Text = "自动校准吸嘴数据，包括高度、偏移量，校准前请先确保吸嘴已经示教完成，并制作吸嘴模板！";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnCaliNozzle);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.CmbNozzleName);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(28, 177);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(813, 145);
            this.groupControl1.TabIndex = 55;
            this.groupControl1.Text = "吸嘴（不适用异形吸嘴、需要两点定位的大吸嘴）";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.CmbComponent1);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.CmbEjectionName);
            this.groupControl2.Controls.Add(this.BtnCaliEjection);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(28, 12);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(813, 145);
            this.groupControl2.TabIndex = 56;
            this.groupControl2.Text = "顶针";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(62, 65);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(30, 18);
            this.labelControl5.TabIndex = 58;
            this.labelControl5.Text = "芯片";
            // 
            // CmbComponent1
            // 
            this.CmbComponent1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbComponent1.Location = new System.Drawing.Point(143, 62);
            this.CmbComponent1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbComponent1.Name = "CmbComponent1";
            this.CmbComponent1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbComponent1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbComponent1.Size = new System.Drawing.Size(286, 24);
            this.CmbComponent1.TabIndex = 57;
            this.CmbComponent1.SelectedIndexChanged += new System.EventHandler(this.CmbComponent1_SelectedIndexChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(62, 106);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(30, 18);
            this.labelControl3.TabIndex = 56;
            this.labelControl3.Text = "顶针";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(21, 31);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(766, 18);
            this.labelControl2.TabIndex = 54;
            this.labelControl2.Text = "自动校准顶针数据(顶针中心和晶圆相机的偏移)，校准前请先设置顶针座不剥离位置和确保晶圆台上有料片！";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.labelControl1);
            this.groupControl3.Controls.Add(this.CmbComponent);
            this.groupControl3.Controls.Add(this.BtnCaliPickOffset);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(28, 348);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(813, 145);
            this.groupControl3.TabIndex = 57;
            this.groupControl3.Text = "芯片";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(143, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(528, 18);
            this.labelControl1.TabIndex = 54;
            this.labelControl1.Text = "自动校准取片偏移，校准前请先给芯片绑定吸嘴和顶针，并制作上视模板！";
            // 
            // FrmSystem2Calibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 615);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.BtnCaliAll);
            this.Name = "FrmSystem2Calibrate";
            this.Text = "系统2校准";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSystem2Calibrate_FormClosing);
            this.Load += new System.EventHandler(this.FrmSystem2Calibrate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbComponent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzleName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbComponent1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit CmbEjectionName;
        private DevExpress.XtraEditors.ComboBoxEdit CmbComponent;
        private DevExpress.XtraEditors.SimpleButton BtnCaliEjection;
        private DevExpress.XtraEditors.SimpleButton BtnCaliPickOffset;
        private DevExpress.XtraEditors.SimpleButton BtnCaliAll;
        private DevExpress.XtraEditors.ComboBoxEdit CmbNozzleName;
        private DevExpress.XtraEditors.SimpleButton BtnCaliNozzle;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.ComboBoxEdit CmbComponent1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}