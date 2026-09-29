namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.FlatLevelMeasure.System1Related
{
    partial class FrmSystem1FlatLevelMeasure
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSystem1FlatLevelMeasure));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ImgCmbSystem1FlatLevelMeasure = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.BtnTeach = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCmbSystem1FlatLevelMeasure.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(397, 182);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(114, 30);
            this.BtnStart.TabIndex = 15;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.SpTimes);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.ImgCmbSystem1FlatLevelMeasure);
            this.groupControl1.Location = new System.Drawing.Point(29, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(533, 139);
            this.groupControl1.TabIndex = 14;
            this.groupControl1.Text = "Option";
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(177, 92);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Properties.MaxValue = new decimal(new int[] {
            5000,
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
            // ImgCmbSystem1FlatLevelMeasure
            // 
            this.ImgCmbSystem1FlatLevelMeasure.Location = new System.Drawing.Point(177, 50);
            this.ImgCmbSystem1FlatLevelMeasure.Name = "ImgCmbSystem1FlatLevelMeasure";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.ImgCmbSystem1FlatLevelMeasure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.ImgCmbSystem1FlatLevelMeasure.Size = new System.Drawing.Size(333, 24);
            this.ImgCmbSystem1FlatLevelMeasure.TabIndex = 8;
            this.ImgCmbSystem1FlatLevelMeasure.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.ImgCmbSystem1FlatLevelMeasure_ButtonClick);
            // 
            // BtnTeach
            // 
            this.BtnTeach.Location = new System.Drawing.Point(246, 182);
            this.BtnTeach.Name = "BtnTeach";
            this.BtnTeach.Size = new System.Drawing.Size(114, 30);
            this.BtnTeach.TabIndex = 17;
            this.BtnTeach.Text = "Teach";
            this.BtnTeach.Click += new System.EventHandler(this.BtnTeach_Click);
            // 
            // FrmSystem1FlatLevelMeasure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 228);
            this.Controls.Add(this.BtnTeach);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmSystem1FlatLevelMeasure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System1 Flat Level Measure";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCmbSystem1FlatLevelMeasure.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ImageComboBoxEdit ImgCmbSystem1FlatLevelMeasure;
        private DevExpress.XtraEditors.SimpleButton BtnTeach;
    }
}