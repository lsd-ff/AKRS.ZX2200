namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmRotateCenterAccuracyTwo
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
            this.BtnStop = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStartRotateCenterTest = new DevExpress.XtraEditors.SimpleButton();
            this.spIntervalTimes = new DevExpress.XtraEditors.SpinEdit();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.spIntervalTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(227, 137);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(105, 28);
            this.BtnStop.TabIndex = 11;
            this.BtnStop.Text = "Stop";
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(36, 95);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(81, 14);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "Interval Times:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(85, 56);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 14);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "Cycle:";
            // 
            // BtnStartRotateCenterTest
            // 
            this.BtnStartRotateCenterTest.Location = new System.Drawing.Point(101, 137);
            this.BtnStartRotateCenterTest.Name = "BtnStartRotateCenterTest";
            this.BtnStartRotateCenterTest.Size = new System.Drawing.Size(105, 28);
            this.BtnStartRotateCenterTest.TabIndex = 6;
            this.BtnStartRotateCenterTest.Text = "Start";
            this.BtnStartRotateCenterTest.Click += new System.EventHandler(this.BtnStartRotateCenterTest_Click);
            // 
            // spIntervalTimes
            // 
            this.spIntervalTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spIntervalTimes.Location = new System.Drawing.Point(144, 92);
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
            this.spIntervalTimes.TabIndex = 10;
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(144, 53);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Size = new System.Drawing.Size(148, 20);
            this.SpTimes.TabIndex = 8;
            // 
            // FrmRotateCenterAccuracyTwo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 218);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.spIntervalTimes);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.SpTimes);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnStartRotateCenterTest);
            this.Name = "FrmRotateCenterAccuracyTwo";
            this.Text = "FrmRotateCenterAccuracyTwo";
            ((System.ComponentModel.ISupportInitialize)(this.spIntervalTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStop;
        private DevExpress.XtraEditors.SpinEdit spIntervalTimes;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnStartRotateCenterTest;
    }
}