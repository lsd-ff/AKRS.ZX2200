namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System2Related
{
    partial class FrmSystem2FlatLevelMeasure
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions2 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSystem2FlatLevelMeasure));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject5 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject6 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject7 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject8 = new DevExpress.Utils.SerializableAppearanceObject();
            this.BtnTeach = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ImgCmbSystem2FlatLevelMeasure = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChkLaserMeasure = new DevExpress.XtraEditors.CheckEdit();
            this.ChkBackCy = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCmbSystem2FlatLevelMeasure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkLaserMeasure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBackCy.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnTeach
            // 
            this.BtnTeach.Location = new System.Drawing.Point(268, 182);
            this.BtnTeach.Name = "BtnTeach";
            this.BtnTeach.Size = new System.Drawing.Size(114, 30);
            this.BtnTeach.TabIndex = 9;
            this.BtnTeach.Text = "Teach";
            this.BtnTeach.Click += new System.EventHandler(this.BtnTeach_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.SpTimes);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.ImgCmbSystem2FlatLevelMeasure);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(534, 139);
            this.groupControl1.TabIndex = 11;
            this.groupControl1.Text = "Option";
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(186, 92);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.SpTimes.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Size = new System.Drawing.Size(333, 20);
            this.SpTimes.TabIndex = 11;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(27, 95);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(135, 14);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "Measurement Frequency";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(85, 55);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(77, 14);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "Level Measure";
            // 
            // ImgCmbSystem2FlatLevelMeasure
            // 
            this.ImgCmbSystem2FlatLevelMeasure.Location = new System.Drawing.Point(186, 50);
            this.ImgCmbSystem2FlatLevelMeasure.Name = "ImgCmbSystem2FlatLevelMeasure";
            editorButtonImageOptions2.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions2.Image")));
            this.ImgCmbSystem2FlatLevelMeasure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions2, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject5, serializableAppearanceObject6, serializableAppearanceObject7, serializableAppearanceObject8, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.ImgCmbSystem2FlatLevelMeasure.Size = new System.Drawing.Size(333, 24);
            this.ImgCmbSystem2FlatLevelMeasure.TabIndex = 8;
            this.ImgCmbSystem2FlatLevelMeasure.SelectedIndexChanged += new System.EventHandler(this.ImgCmbSystem2FlatLevelMeasure_SelectedIndexChanged);
            this.ImgCmbSystem2FlatLevelMeasure.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ImgCmbSystem2FlatLevelMeasure_ButtonClick);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(417, 182);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(114, 30);
            this.BtnStart.TabIndex = 12;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChkLaserMeasure
            // 
            this.ChkLaserMeasure.Location = new System.Drawing.Point(12, 187);
            this.ChkLaserMeasure.Name = "ChkLaserMeasure";
            this.ChkLaserMeasure.Properties.Caption = "激光测高";
            this.ChkLaserMeasure.Size = new System.Drawing.Size(75, 20);
            this.ChkLaserMeasure.TabIndex = 13;
            this.ChkLaserMeasure.CheckedChanged += new System.EventHandler(this.ChkLaserMeasure_CheckedChanged);
            // 
            // ChkBackCy
            // 
            this.ChkBackCy.EditValue = true;
            this.ChkBackCy.Location = new System.Drawing.Point(116, 187);
            this.ChkBackCy.Name = "ChkBackCy";
            this.ChkBackCy.Properties.Caption = "测高时收回气缸";
            this.ChkBackCy.Size = new System.Drawing.Size(112, 20);
            this.ChkBackCy.TabIndex = 14;
            this.ChkBackCy.CheckedChanged += new System.EventHandler(this.ChkBackCy_CheckedChanged);
            // 
            // FrmSystem2FlatLevelMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 228);
            this.Controls.Add(this.ChkBackCy);
            this.Controls.Add(this.ChkLaserMeasure);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.BtnTeach);
            this.Name = "FrmSystem2FlatLevelMeasure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Assistant";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCmbSystem2FlatLevelMeasure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkLaserMeasure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBackCy.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ImageComboBoxEdit ImgCmbSystem2FlatLevelMeasure;
        private DevExpress.XtraEditors.SimpleButton BtnTeach;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.CheckEdit ChkLaserMeasure;
        private DevExpress.XtraEditors.CheckEdit ChkBackCy;
    }
}