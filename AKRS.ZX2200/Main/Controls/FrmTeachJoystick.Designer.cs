namespace AKRS.ZX2200.Main.Controls
{
    partial class FrmTeachJoystick
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
            this.components = new System.ComponentModel.Container();
            this.SpJoystickT = new DevExpress.XtraEditors.SpinEdit();
            this.SpJoystickY = new DevExpress.XtraEditors.SpinEdit();
            this.SpJoystickX = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl22 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl23 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.SpXInitialVal = new DevExpress.XtraEditors.SpinEdit();
            this.SpYInitialVal = new DevExpress.XtraEditors.SpinEdit();
            this.SpTInitialVal = new DevExpress.XtraEditors.SpinEdit();
            this.SpTMaxVal = new DevExpress.XtraEditors.SpinEdit();
            this.SpYMaxVal = new DevExpress.XtraEditors.SpinEdit();
            this.SpXMaxVal = new DevExpress.XtraEditors.SpinEdit();
            this.BtnSetXMaxVal = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSetYMaxVal = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSetTMaxVal = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSetTInitialVal = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSetYInitialVal = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSetXInitialVal = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.BtnSave = new DevExpress.XtraEditors.SimpleButton();
            this.SpBlindRange = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXInitialVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpYInitialVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTInitialVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTMaxVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpYMaxVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXMaxVal.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlindRange.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpJoystickT
            // 
            this.SpJoystickT.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpJoystickT.Enabled = false;
            this.SpJoystickT.Location = new System.Drawing.Point(120, 332);
            this.SpJoystickT.Name = "SpJoystickT";
            this.SpJoystickT.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpJoystickT.Size = new System.Drawing.Size(107, 24);
            this.SpJoystickT.TabIndex = 101;
            // 
            // SpJoystickY
            // 
            this.SpJoystickY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpJoystickY.Enabled = false;
            this.SpJoystickY.Location = new System.Drawing.Point(120, 262);
            this.SpJoystickY.Name = "SpJoystickY";
            this.SpJoystickY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpJoystickY.Size = new System.Drawing.Size(107, 24);
            this.SpJoystickY.TabIndex = 100;
            // 
            // SpJoystickX
            // 
            this.SpJoystickX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpJoystickX.Enabled = false;
            this.SpJoystickX.Location = new System.Drawing.Point(120, 185);
            this.SpJoystickX.Name = "SpJoystickX";
            this.SpJoystickX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpJoystickX.Size = new System.Drawing.Size(107, 24);
            this.SpJoystickX.TabIndex = 99;
            // 
            // labelControl22
            // 
            this.labelControl22.Location = new System.Drawing.Point(52, 337);
            this.labelControl22.Name = "labelControl22";
            this.labelControl22.Size = new System.Drawing.Size(40, 18);
            this.labelControl22.TabIndex = 98;
            this.labelControl22.Text = "摇杆T";
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(52, 265);
            this.labelControl23.Name = "labelControl23";
            this.labelControl23.Size = new System.Drawing.Size(40, 18);
            this.labelControl23.TabIndex = 97;
            this.labelControl23.Text = "摇杆Y";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(52, 188);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(39, 18);
            this.labelControl14.TabIndex = 96;
            this.labelControl14.Text = "摇杆X";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(587, 139);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 102;
            this.labelControl1.Text = "最大值";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(317, 139);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(45, 18);
            this.labelControl2.TabIndex = 103;
            this.labelControl2.Text = "初始值";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(163, 51);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(469, 24);
            this.labelControl3.TabIndex = 104;
            this.labelControl3.Text = "摇杆掰到初始位置设置初始值，掰到底设置最大值!";
            // 
            // SpXInitialVal
            // 
            this.SpXInitialVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpXInitialVal.Location = new System.Drawing.Point(289, 185);
            this.SpXInitialVal.Name = "SpXInitialVal";
            this.SpXInitialVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpXInitialVal.Size = new System.Drawing.Size(110, 24);
            this.SpXInitialVal.TabIndex = 106;
            // 
            // SpYInitialVal
            // 
            this.SpYInitialVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpYInitialVal.Location = new System.Drawing.Point(289, 262);
            this.SpYInitialVal.Name = "SpYInitialVal";
            this.SpYInitialVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpYInitialVal.Size = new System.Drawing.Size(110, 24);
            this.SpYInitialVal.TabIndex = 107;
            // 
            // SpTInitialVal
            // 
            this.SpTInitialVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpTInitialVal.Location = new System.Drawing.Point(289, 331);
            this.SpTInitialVal.Name = "SpTInitialVal";
            this.SpTInitialVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTInitialVal.Size = new System.Drawing.Size(110, 24);
            this.SpTInitialVal.TabIndex = 108;
            // 
            // SpTMaxVal
            // 
            this.SpTMaxVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpTMaxVal.Location = new System.Drawing.Point(564, 331);
            this.SpTMaxVal.Name = "SpTMaxVal";
            this.SpTMaxVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTMaxVal.Size = new System.Drawing.Size(110, 24);
            this.SpTMaxVal.TabIndex = 111;
            // 
            // SpYMaxVal
            // 
            this.SpYMaxVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpYMaxVal.Location = new System.Drawing.Point(564, 262);
            this.SpYMaxVal.Name = "SpYMaxVal";
            this.SpYMaxVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpYMaxVal.Size = new System.Drawing.Size(110, 24);
            this.SpYMaxVal.TabIndex = 110;
            // 
            // SpXMaxVal
            // 
            this.SpXMaxVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpXMaxVal.Location = new System.Drawing.Point(564, 185);
            this.SpXMaxVal.Name = "SpXMaxVal";
            this.SpXMaxVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpXMaxVal.Size = new System.Drawing.Size(110, 24);
            this.SpXMaxVal.TabIndex = 109;
            // 
            // BtnSetXMaxVal
            // 
            this.BtnSetXMaxVal.Location = new System.Drawing.Point(714, 186);
            this.BtnSetXMaxVal.Name = "BtnSetXMaxVal";
            this.BtnSetXMaxVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetXMaxVal.TabIndex = 112;
            this.BtnSetXMaxVal.Text = "设置";
            this.BtnSetXMaxVal.Click += new System.EventHandler(this.BtnSetXMaxVal_Click);
            // 
            // BtnSetYMaxVal
            // 
            this.BtnSetYMaxVal.Location = new System.Drawing.Point(714, 262);
            this.BtnSetYMaxVal.Name = "BtnSetYMaxVal";
            this.BtnSetYMaxVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetYMaxVal.TabIndex = 113;
            this.BtnSetYMaxVal.Text = "设置";
            this.BtnSetYMaxVal.Click += new System.EventHandler(this.BtnSetYMaxVal_Click);
            // 
            // BtnSetTMaxVal
            // 
            this.BtnSetTMaxVal.Location = new System.Drawing.Point(714, 331);
            this.BtnSetTMaxVal.Name = "BtnSetTMaxVal";
            this.BtnSetTMaxVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetTMaxVal.TabIndex = 114;
            this.BtnSetTMaxVal.Text = "设置";
            this.BtnSetTMaxVal.Click += new System.EventHandler(this.BtnSetTMaxVal_Click);
            // 
            // BtnSetTInitialVal
            // 
            this.BtnSetTInitialVal.Location = new System.Drawing.Point(431, 334);
            this.BtnSetTInitialVal.Name = "BtnSetTInitialVal";
            this.BtnSetTInitialVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetTInitialVal.TabIndex = 117;
            this.BtnSetTInitialVal.Text = "设置";
            this.BtnSetTInitialVal.Click += new System.EventHandler(this.BtnSetTInitialVal_Click);
            // 
            // BtnSetYInitialVal
            // 
            this.BtnSetYInitialVal.Location = new System.Drawing.Point(431, 263);
            this.BtnSetYInitialVal.Name = "BtnSetYInitialVal";
            this.BtnSetYInitialVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetYInitialVal.TabIndex = 116;
            this.BtnSetYInitialVal.Text = "设置";
            this.BtnSetYInitialVal.Click += new System.EventHandler(this.BtnSetYInitialVal_Click);
            // 
            // BtnSetXInitialVal
            // 
            this.BtnSetXInitialVal.Location = new System.Drawing.Point(431, 186);
            this.BtnSetXInitialVal.Name = "BtnSetXInitialVal";
            this.BtnSetXInitialVal.Size = new System.Drawing.Size(82, 23);
            this.BtnSetXInitialVal.TabIndex = 115;
            this.BtnSetXInitialVal.Text = "设置";
            this.BtnSetXInitialVal.Click += new System.EventHandler(this.BtnSetXInitialVal_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(155, 139);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 118;
            this.labelControl4.Text = "实时值";
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(302, 492);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(225, 39);
            this.BtnSave.TabIndex = 119;
            this.BtnSave.Text = "保存";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // SpBlindRange
            // 
            this.SpBlindRange.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpBlindRange.Location = new System.Drawing.Point(403, 412);
            this.SpBlindRange.Name = "SpBlindRange";
            this.SpBlindRange.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBlindRange.Size = new System.Drawing.Size(110, 24);
            this.SpBlindRange.TabIndex = 120;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(302, 415);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 18);
            this.labelControl5.TabIndex = 121;
            this.labelControl5.Text = "盲区范围";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmTeachJoystick
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(848, 570);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.SpBlindRange);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.BtnSetTInitialVal);
            this.Controls.Add(this.BtnSetYInitialVal);
            this.Controls.Add(this.BtnSetXInitialVal);
            this.Controls.Add(this.BtnSetTMaxVal);
            this.Controls.Add(this.BtnSetYMaxVal);
            this.Controls.Add(this.BtnSetXMaxVal);
            this.Controls.Add(this.SpTMaxVal);
            this.Controls.Add(this.SpYMaxVal);
            this.Controls.Add(this.SpXMaxVal);
            this.Controls.Add(this.SpTInitialVal);
            this.Controls.Add(this.SpYInitialVal);
            this.Controls.Add(this.SpXInitialVal);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.SpJoystickT);
            this.Controls.Add(this.SpJoystickY);
            this.Controls.Add(this.SpJoystickX);
            this.Controls.Add(this.labelControl22);
            this.Controls.Add(this.labelControl23);
            this.Controls.Add(this.labelControl14);
            this.Name = "FrmTeachJoystick";
            this.Text = "摇杆示教";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTeachJoystick_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJoystickX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXInitialVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpYInitialVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTInitialVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTMaxVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpYMaxVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXMaxVal.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlindRange.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit SpJoystickT;
        private DevExpress.XtraEditors.SpinEdit SpJoystickY;
        private DevExpress.XtraEditors.SpinEdit SpJoystickX;
        private DevExpress.XtraEditors.LabelControl labelControl22;
        private DevExpress.XtraEditors.LabelControl labelControl23;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit SpXInitialVal;
        private DevExpress.XtraEditors.SpinEdit SpYInitialVal;
        private DevExpress.XtraEditors.SpinEdit SpTInitialVal;
        private DevExpress.XtraEditors.SpinEdit SpTMaxVal;
        private DevExpress.XtraEditors.SpinEdit SpYMaxVal;
        private DevExpress.XtraEditors.SpinEdit SpXMaxVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetXMaxVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetYMaxVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetTMaxVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetTInitialVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetYInitialVal;
        private DevExpress.XtraEditors.SimpleButton BtnSetXInitialVal;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton BtnSave;
        private DevExpress.XtraEditors.SpinEdit SpBlindRange;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private System.Windows.Forms.Timer timer1;
    }
}