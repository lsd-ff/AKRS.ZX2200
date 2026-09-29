namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    partial class FrmPlacementTest
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
            this.SpVisionTime = new DevExpress.XtraEditors.SpinEdit();
            this.LbCycle = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.ChkMoveToCameraCenterAfterVision = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.SpCycles = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpVisionTime
            // 
            this.SpVisionTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpVisionTime.Location = new System.Drawing.Point(187, 97);
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
            this.SpVisionTime.TabIndex = 59;
            // 
            // LbCycle
            // 
            this.LbCycle.Location = new System.Drawing.Point(451, 187);
            this.LbCycle.Name = "LbCycle";
            this.LbCycle.Size = new System.Drawing.Size(39, 18);
            this.LbCycle.TabIndex = 58;
            this.LbCycle.Text = "Cycle:";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(221, 463);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(125, 35);
            this.BtnStart.TabIndex = 57;
            this.BtnStart.Text = "开始";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChkMoveToCameraCenterAfterVision
            // 
            this.ChkMoveToCameraCenterAfterVision.Location = new System.Drawing.Point(55, 184);
            this.ChkMoveToCameraCenterAfterVision.Name = "ChkMoveToCameraCenterAfterVision";
            this.ChkMoveToCameraCenterAfterVision.Properties.Caption = "定位后移动到相机中心";
            this.ChkMoveToCameraCenterAfterVision.Size = new System.Drawing.Size(259, 24);
            this.ChkMoveToCameraCenterAfterVision.TabIndex = 56;
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(77, 100);
            this.labelControl23.Margin = new System.Windows.Forms.Padding(6);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(60, 18);
            this.labelControl23.TabIndex = 55;
            this.labelControl23.Text = "拍照次数";
            // 
            // SpCycles
            // 
            this.SpCycles.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Location = new System.Drawing.Point(187, 32);
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
            this.SpCycles.TabIndex = 54;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(77, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 53;
            this.labelControl1.Text = "循环次数";
            // 
            // FrmPlacementTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 574);
            this.Controls.Add(this.SpVisionTime);
            this.Controls.Add(this.LbCycle);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.ChkMoveToCameraCenterAfterVision);
            this.Controls.Add(this.labelControl23);
            this.Controls.Add(this.SpCycles);
            this.Controls.Add(this.labelControl1);
            this.Name = "FrmPlacementTest";
            this.Text = "放片误差实验";
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit SpVisionTime;
        private DevExpress.XtraEditors.LabelControl LbCycle;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.CheckEdit ChkMoveToCameraCenterAfterVision;
        private DevExpress.XtraEditors.LabelControl labelControl23;
        private DevExpress.XtraEditors.SpinEdit SpCycles;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}