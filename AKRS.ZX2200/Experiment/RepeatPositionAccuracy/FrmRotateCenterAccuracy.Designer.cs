namespace AKRS.ZX2200.ExperimentSystem.RepeatPositionAccuracy
{
    partial class FrmRotateCenterAccuracy
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
            this.BtnStartRotateCenterTest = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.spIntervalTimes = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIntervalTimes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStartRotateCenterTest
            // 
            this.BtnStartRotateCenterTest.Location = new System.Drawing.Point(113, 118);
            this.BtnStartRotateCenterTest.Name = "BtnStartRotateCenterTest";
            this.BtnStartRotateCenterTest.Size = new System.Drawing.Size(105, 28);
            this.BtnStartRotateCenterTest.TabIndex = 0;
            this.BtnStartRotateCenterTest.Text = "Start";
            this.BtnStartRotateCenterTest.Click += new System.EventHandler(this.BtnStartRotateCenterTest_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(54, 37);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Cycle:";
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(113, 34);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Size = new System.Drawing.Size(148, 20);
            this.SpTimes.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(5, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(81, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Interval Times:";
            // 
            // spIntervalTimes
            // 
            this.spIntervalTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spIntervalTimes.Location = new System.Drawing.Point(113, 73);
            this.spIntervalTimes.Name = "spIntervalTimes";
            this.spIntervalTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spIntervalTimes.Properties.IsFloatValue = false;
            this.spIntervalTimes.Properties.MaskSettings.Set("mask", "N00");
            this.spIntervalTimes.Properties.MaxValue = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.spIntervalTimes.Size = new System.Drawing.Size(148, 20);
            this.spIntervalTimes.TabIndex = 4;
            // 
            // FrmRotateCenterAccuracy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(313, 158);
            this.Controls.Add(this.spIntervalTimes);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.SpTimes);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnStartRotateCenterTest);
            this.Name = "FrmRotateCenterAccuracy";
            this.Text = "Rotate Center Accuracy";
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spIntervalTimes.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStartRotateCenterTest;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit spIntervalTimes;
    }
}