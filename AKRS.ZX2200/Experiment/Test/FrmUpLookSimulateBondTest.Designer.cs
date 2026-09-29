namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmUpLookSimulateBondTest
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
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.SpCycles = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.ChkIsPlaceGlassOnBMC = new DevExpress.XtraEditors.CheckEdit();
            this.LbCycle = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsPlaceGlassOnBMC.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(240, 313);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(214, 62);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.ChkIsPlaceGlassOnBMC);
            this.groupControl1.Controls.Add(this.SpCycles);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(760, 221);
            this.groupControl1.TabIndex = 1;
            // 
            // SpCycles
            // 
            this.SpCycles.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Location = new System.Drawing.Point(173, 49);
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
            this.labelControl1.Location = new System.Drawing.Point(50, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(39, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "cycles";
            // 
            // ChkIsPlaceGlassOnBMC
            // 
            this.ChkIsPlaceGlassOnBMC.Location = new System.Drawing.Point(50, 117);
            this.ChkIsPlaceGlassOnBMC.Name = "ChkIsPlaceGlassOnBMC";
            this.ChkIsPlaceGlassOnBMC.Properties.Caption = "Place  glass  on  BMC";
            this.ChkIsPlaceGlassOnBMC.Size = new System.Drawing.Size(257, 24);
            this.ChkIsPlaceGlassOnBMC.TabIndex = 5;
            // 
            // LbCycle
            // 
            this.LbCycle.Location = new System.Drawing.Point(634, 260);
            this.LbCycle.Name = "LbCycle";
            this.LbCycle.Size = new System.Drawing.Size(39, 18);
            this.LbCycle.TabIndex = 21;
            this.LbCycle.Text = "Cycle:";
            // 
            // FrmUpLookSimulateBondTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 463);
            this.Controls.Add(this.LbCycle);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.BtnStart);
            this.Name = "FrmUpLookSimulateBondTest";
            this.Text = "UpLook  Simulate  Bond  Test";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsPlaceGlassOnBMC.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SpinEdit SpCycles;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit ChkIsPlaceGlassOnBMC;
        private DevExpress.XtraEditors.LabelControl LbCycle;
    }
}