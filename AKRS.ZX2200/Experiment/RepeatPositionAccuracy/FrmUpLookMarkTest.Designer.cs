namespace AKRS.ZX2200.Experiment.RepeatPositionAccuracy
{
    partial class FrmUpLookMarkTest
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
            this.SpCycles = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.SpDistance = new DevExpress.XtraEditors.SpinEdit();
            this.LbCycle = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.SpVisionDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.ChkIsActiveZMove = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveZMove.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpCycles
            // 
            this.SpCycles.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Location = new System.Drawing.Point(152, 26);
            this.SpCycles.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SpCycles.Name = "SpCycles";
            this.SpCycles.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCycles.Properties.IsFloatValue = false;
            this.SpCycles.Properties.MaskSettings.Set("mask", "N00");
            this.SpCycles.Properties.MaxValue = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.SpCycles.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpCycles.Size = new System.Drawing.Size(134, 20);
            this.SpCycles.TabIndex = 4;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(56, 30);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 14);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "cycles";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(56, 75);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "Z  distance";
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(324, 75);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(20, 14);
            this.labelControl21.TabIndex = 36;
            this.labelControl21.Text = "mm";
            // 
            // SpDistance
            // 
            this.SpDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpDistance.Location = new System.Drawing.Point(152, 71);
            this.SpDistance.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SpDistance.Name = "SpDistance";
            this.SpDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDistance.Size = new System.Drawing.Size(134, 20);
            this.SpDistance.TabIndex = 35;
            // 
            // LbCycle
            // 
            this.LbCycle.Location = new System.Drawing.Point(307, 173);
            this.LbCycle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbCycle.Name = "LbCycle";
            this.LbCycle.Size = new System.Drawing.Size(32, 14);
            this.LbCycle.TabIndex = 37;
            this.LbCycle.Text = "Cycle:";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(152, 207);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(82, 26);
            this.BtnStart.TabIndex = 38;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(56, 119);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(67, 14);
            this.labelControl3.TabIndex = 39;
            this.labelControl3.Text = "Vision  delay";
            // 
            // SpVisionDelay
            // 
            this.SpVisionDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVisionDelay.Location = new System.Drawing.Point(152, 117);
            this.SpVisionDelay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SpVisionDelay.Name = "SpVisionDelay";
            this.SpVisionDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVisionDelay.Properties.IsFloatValue = false;
            this.SpVisionDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpVisionDelay.Size = new System.Drawing.Size(134, 20);
            this.SpVisionDelay.TabIndex = 40;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(324, 121);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(15, 14);
            this.labelControl4.TabIndex = 41;
            this.labelControl4.Text = "ms";
            // 
            // ChkIsActiveZMove
            // 
            this.ChkIsActiveZMove.Location = new System.Drawing.Point(56, 157);
            this.ChkIsActiveZMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChkIsActiveZMove.Name = "ChkIsActiveZMove";
            this.ChkIsActiveZMove.Properties.Caption = "Is  active  Z  move";
            this.ChkIsActiveZMove.Size = new System.Drawing.Size(178, 20);
            this.ChkIsActiveZMove.TabIndex = 42;
            // 
            // FrmUpLookMarkTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 252);
            this.Controls.Add(this.ChkIsActiveZMove);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.SpVisionDelay);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.LbCycle);
            this.Controls.Add(this.labelControl21);
            this.Controls.Add(this.SpDistance);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.SpCycles);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmUpLookMarkTest";
            this.Text = "FrmUpLookMarkTest";
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkIsActiveZMove.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit SpCycles;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.SpinEdit SpDistance;
        private DevExpress.XtraEditors.LabelControl LbCycle;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit SpVisionDelay;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.CheckEdit ChkIsActiveZMove;
    }
}