namespace AKRS.ZX2200.SupportFeature.LevelMeasurementSystem.Controls.BondLevelMeasure
{
    partial class FrmBondLevel
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
            this.BtnTeach = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnTeach
            // 
            this.BtnTeach.Location = new System.Drawing.Point(247, 118);
            this.BtnTeach.Name = "BtnTeach";
            this.BtnTeach.Size = new System.Drawing.Size(104, 33);
            this.BtnTeach.TabIndex = 0;
            this.BtnTeach.Text = "Teach";
            this.BtnTeach.Click += new System.EventHandler(this.BtnTeach_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.SpTimes);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(563, 89);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Option";
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(186, 38);
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
            this.SpTimes.Size = new System.Drawing.Size(272, 20);
            this.SpTimes.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(17, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(137, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Measurement frequency:";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(390, 118);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(104, 33);
            this.BtnStart.TabIndex = 3;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // FrmBondLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 163);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.BtnTeach);
            this.Name = "FrmBondLevel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bond Head Level";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnTeach;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
    }
}