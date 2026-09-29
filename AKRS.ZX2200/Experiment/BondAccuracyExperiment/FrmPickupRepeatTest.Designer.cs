namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    partial class FrmPickupRepeatTest
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
            this.SpCycles = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.ChkMoveToCameraCenterAfterVision = new DevExpress.XtraEditors.CheckEdit();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.LbCycle = new DevExpress.XtraEditors.LabelControl();
            this.SpVisionTime = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpCycles
            // 
            this.SpCycles.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Location = new System.Drawing.Point(164, 46);
            this.SpCycles.Name = "SpCycles";
            this.SpCycles.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCycles.Properties.IsFloatValue = false;
            this.SpCycles.Properties.MaskSettings.Set("mask", "N00");
            this.SpCycles.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpCycles.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Size = new System.Drawing.Size(291, 24);
            this.SpCycles.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(54, 49);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "循环次数";
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(54, 114);
            this.labelControl23.Margin = new System.Windows.Forms.Padding(6);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(60, 18);
            this.labelControl23.TabIndex = 45;
            this.labelControl23.Text = "拍照次数";
            // 
            // ChkMoveToCameraCenterAfterVision
            // 
            this.ChkMoveToCameraCenterAfterVision.Location = new System.Drawing.Point(32, 198);
            this.ChkMoveToCameraCenterAfterVision.Name = "ChkMoveToCameraCenterAfterVision";
            this.ChkMoveToCameraCenterAfterVision.Properties.Caption = "定位后移动到相机中心";
            this.ChkMoveToCameraCenterAfterVision.Size = new System.Drawing.Size(259, 24);
            this.ChkMoveToCameraCenterAfterVision.TabIndex = 49;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(211, 534);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(125, 35);
            this.BtnStart.TabIndex = 50;
            this.BtnStart.Text = "开始";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LbCycle
            // 
            this.LbCycle.Location = new System.Drawing.Point(428, 201);
            this.LbCycle.Name = "LbCycle";
            this.LbCycle.Size = new System.Drawing.Size(39, 18);
            this.LbCycle.TabIndex = 51;
            this.LbCycle.Text = "Cycle:";
            // 
            // SpVisionTime
            // 
            this.SpVisionTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpVisionTime.Location = new System.Drawing.Point(164, 111);
            this.SpVisionTime.Name = "SpVisionTime";
            this.SpVisionTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVisionTime.Properties.IsFloatValue = false;
            this.SpVisionTime.Properties.MaskSettings.Set("mask", "N00");
            this.SpVisionTime.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpVisionTime.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpVisionTime.Size = new System.Drawing.Size(291, 24);
            this.SpVisionTime.TabIndex = 52;
            // 
            // FrmPickupRepeatTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 631);
            this.Controls.Add(this.SpVisionTime);
            this.Controls.Add(this.LbCycle);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.ChkMoveToCameraCenterAfterVision);
            this.Controls.Add(this.labelControl23);
            this.Controls.Add(this.SpCycles);
            this.Controls.Add(this.labelControl1);
            this.Name = "FrmPickupRepeatTest";
            this.Text = "取片重复性实验";
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit SpCycles;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl23;
        private DevExpress.XtraEditors.CheckEdit ChkMoveToCameraCenterAfterVision;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl LbCycle;
        private DevExpress.XtraEditors.SpinEdit SpVisionTime;
    }
}