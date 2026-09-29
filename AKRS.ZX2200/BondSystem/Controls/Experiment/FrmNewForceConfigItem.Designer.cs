namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    partial class FrmNewForceConfigItem
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.SpForceUpperLimit = new DevExpress.XtraEditors.SpinEdit();
            this.SpUpperChangeForce = new DevExpress.XtraEditors.SpinEdit();
            this.SpForceLowerLimit = new DevExpress.XtraEditors.SpinEdit();
            this.SpLowerChangeForce = new DevExpress.XtraEditors.SpinEdit();
            this.SpLowSpeedDuringTouchDown = new DevExpress.XtraEditors.SpinEdit();
            this.SpKp = new DevExpress.XtraEditors.SpinEdit();
            this.SpKi = new DevExpress.XtraEditors.SpinEdit();
            this.SpKd = new DevExpress.XtraEditors.SpinEdit();
            this.SpSmoothTime = new DevExpress.XtraEditors.SpinEdit();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.SpAcc = new DevExpress.XtraEditors.SpinEdit();
            this.SpDec = new DevExpress.XtraEditors.SpinEdit();
            this.SpSlowTouchDec = new DevExpress.XtraEditors.SpinEdit();
            this.SpSlowTouchAcc = new DevExpress.XtraEditors.SpinEdit();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGenerate = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceUpperLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpUpperChangeForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceLowerLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLowerChangeForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLowSpeedDuringTouchDown.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKd.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmoothTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAcc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTouchDec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTouchAcc.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "上界";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "下界";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(334, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "切换阈值";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(831, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 18);
            this.label4.TabIndex = 3;
            this.label4.Text = "平滑时间";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(433, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 18);
            this.label5.TabIndex = 4;
            this.label5.Text = "B to C vel";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(561, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 18);
            this.label6.TabIndex = 5;
            this.label6.Text = "Kp";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(658, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 18);
            this.label7.TabIndex = 6;
            this.label7.Text = "Ki";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(757, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(25, 18);
            this.label8.TabIndex = 7;
            this.label8.Text = "Kd";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(119, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(68, 18);
            this.label9.TabIndex = 8;
            this.label9.Text = "切换阈值";
            // 
            // SpForceUpperLimit
            // 
            this.SpForceUpperLimit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpForceUpperLimit.Location = new System.Drawing.Point(208, 57);
            this.SpForceUpperLimit.Name = "SpForceUpperLimit";
            this.SpForceUpperLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForceUpperLimit.Size = new System.Drawing.Size(91, 24);
            this.SpForceUpperLimit.TabIndex = 9;
            // 
            // SpUpperChangeForce
            // 
            this.SpUpperChangeForce.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpUpperChangeForce.Location = new System.Drawing.Point(322, 57);
            this.SpUpperChangeForce.Name = "SpUpperChangeForce";
            this.SpUpperChangeForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpUpperChangeForce.Size = new System.Drawing.Size(90, 24);
            this.SpUpperChangeForce.TabIndex = 10;
            // 
            // SpForceLowerLimit
            // 
            this.SpForceLowerLimit.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpForceLowerLimit.Location = new System.Drawing.Point(13, 57);
            this.SpForceLowerLimit.Name = "SpForceLowerLimit";
            this.SpForceLowerLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForceLowerLimit.Size = new System.Drawing.Size(90, 24);
            this.SpForceLowerLimit.TabIndex = 11;
            // 
            // SpLowerChangeForce
            // 
            this.SpLowerChangeForce.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLowerChangeForce.Location = new System.Drawing.Point(109, 57);
            this.SpLowerChangeForce.Name = "SpLowerChangeForce";
            this.SpLowerChangeForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLowerChangeForce.Size = new System.Drawing.Size(90, 24);
            this.SpLowerChangeForce.TabIndex = 12;
            // 
            // SpLowSpeedDuringTouchDown
            // 
            this.SpLowSpeedDuringTouchDown.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLowSpeedDuringTouchDown.Location = new System.Drawing.Point(427, 57);
            this.SpLowSpeedDuringTouchDown.Name = "SpLowSpeedDuringTouchDown";
            this.SpLowSpeedDuringTouchDown.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLowSpeedDuringTouchDown.Size = new System.Drawing.Size(90, 24);
            this.SpLowSpeedDuringTouchDown.TabIndex = 13;
            // 
            // SpKp
            // 
            this.SpKp.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKp.Location = new System.Drawing.Point(529, 57);
            this.SpKp.Name = "SpKp";
            this.SpKp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKp.Size = new System.Drawing.Size(90, 24);
            this.SpKp.TabIndex = 14;
            // 
            // SpKi
            // 
            this.SpKi.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKi.Location = new System.Drawing.Point(628, 57);
            this.SpKi.Name = "SpKi";
            this.SpKi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKi.Size = new System.Drawing.Size(90, 24);
            this.SpKi.TabIndex = 15;
            // 
            // SpKd
            // 
            this.SpKd.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpKd.Location = new System.Drawing.Point(724, 57);
            this.SpKd.Name = "SpKd";
            this.SpKd.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpKd.Size = new System.Drawing.Size(90, 24);
            this.SpKd.TabIndex = 16;
            // 
            // SpSmoothTime
            // 
            this.SpSmoothTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSmoothTime.Location = new System.Drawing.Point(820, 57);
            this.SpSmoothTime.Name = "SpSmoothTime";
            this.SpSmoothTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSmoothTime.Size = new System.Drawing.Size(90, 24);
            this.SpSmoothTime.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(952, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(31, 18);
            this.label10.TabIndex = 18;
            this.label10.Text = "Acc";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1055, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(33, 18);
            this.label11.TabIndex = 19;
            this.label11.Text = "Dec";
            // 
            // SpAcc
            // 
            this.SpAcc.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAcc.Location = new System.Drawing.Point(925, 57);
            this.SpAcc.Name = "SpAcc";
            this.SpAcc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAcc.Size = new System.Drawing.Size(90, 24);
            this.SpAcc.TabIndex = 20;
            // 
            // SpDec
            // 
            this.SpDec.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpDec.Location = new System.Drawing.Point(1031, 57);
            this.SpDec.Name = "SpDec";
            this.SpDec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDec.Size = new System.Drawing.Size(90, 24);
            this.SpDec.TabIndex = 21;
            // 
            // SpSlowTouchDec
            // 
            this.SpSlowTouchDec.EditValue = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.SpSlowTouchDec.Location = new System.Drawing.Point(1232, 57);
            this.SpSlowTouchDec.Name = "SpSlowTouchDec";
            this.SpSlowTouchDec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTouchDec.Size = new System.Drawing.Size(90, 24);
            this.SpSlowTouchDec.TabIndex = 25;
            // 
            // SpSlowTouchAcc
            // 
            this.SpSlowTouchAcc.EditValue = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.SpSlowTouchAcc.Location = new System.Drawing.Point(1126, 57);
            this.SpSlowTouchAcc.Name = "SpSlowTouchAcc";
            this.SpSlowTouchAcc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTouchAcc.Size = new System.Drawing.Size(90, 24);
            this.SpSlowTouchAcc.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1236, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 18);
            this.label12.TabIndex = 23;
            this.label12.Text = "BC段减速度";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(1130, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 18);
            this.label13.TabIndex = 22;
            this.label13.Text = "BC段加速度";
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(789, 165);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(144, 37);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.Location = new System.Drawing.Point(412, 165);
            this.BtnGenerate.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(144, 37);
            this.BtnGenerate.TabIndex = 6;
            this.BtnGenerate.Text = "确认";
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // FrmNewForceConfigItem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1341, 215);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtnGenerate);
            this.Controls.Add(this.SpSlowTouchDec);
            this.Controls.Add(this.SpSlowTouchAcc);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.SpDec);
            this.Controls.Add(this.SpAcc);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.SpSmoothTime);
            this.Controls.Add(this.SpKd);
            this.Controls.Add(this.SpKi);
            this.Controls.Add(this.SpKp);
            this.Controls.Add(this.SpLowSpeedDuringTouchDown);
            this.Controls.Add(this.SpLowerChangeForce);
            this.Controls.Add(this.SpForceLowerLimit);
            this.Controls.Add(this.SpUpperChangeForce);
            this.Controls.Add(this.SpForceUpperLimit);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FrmNewForceConfigItem";
            this.Text = "新建力控配置";
            ((System.ComponentModel.ISupportInitialize)(this.SpForceUpperLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpUpperChangeForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceLowerLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLowerChangeForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLowSpeedDuringTouchDown.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpKd.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmoothTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAcc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTouchDec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTouchAcc.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.SpinEdit SpForceUpperLimit;
        private DevExpress.XtraEditors.SpinEdit SpUpperChangeForce;
        private DevExpress.XtraEditors.SpinEdit SpForceLowerLimit;
        private DevExpress.XtraEditors.SpinEdit SpLowerChangeForce;
        private DevExpress.XtraEditors.SpinEdit SpLowSpeedDuringTouchDown;
        private DevExpress.XtraEditors.SpinEdit SpKp;
        private DevExpress.XtraEditors.SpinEdit SpKi;
        private DevExpress.XtraEditors.SpinEdit SpKd;
        private DevExpress.XtraEditors.SpinEdit SpSmoothTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private DevExpress.XtraEditors.SpinEdit SpAcc;
        private DevExpress.XtraEditors.SpinEdit SpDec;
        private DevExpress.XtraEditors.SpinEdit SpSlowTouchDec;
        private DevExpress.XtraEditors.SpinEdit SpSlowTouchAcc;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnGenerate;
    }
}