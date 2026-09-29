namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.FlipTool
{
    partial class UcFlipToolEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CmbNozzle = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.SpBlowDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnBlow = new DevExpress.XtraEditors.SimpleButton();
            this.SpBlowProportion = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.BtGetCurrentVacuumValue = new DevExpress.XtraEditors.SimpleButton();
            this.SpVacuumCheckDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.SpVacuumCheckVal = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlowDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlowProportion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckVal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.CmbNozzle);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(16, 15);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(556, 96);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "测高吸嘴";
            // 
            // CmbNozzle
            // 
            this.CmbNozzle.Location = new System.Drawing.Point(59, 41);
            this.CmbNozzle.Name = "CmbNozzle";
            this.CmbNozzle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbNozzle.Size = new System.Drawing.Size(434, 24);
            this.CmbNozzle.TabIndex = 2;
            this.CmbNozzle.SelectedIndexChanged += new System.EventHandler(this.CmbNozzle_SelectedIndexChanged);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.SpBlowDelay);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.BtnBlow);
            this.groupControl2.Controls.Add(this.SpBlowProportion);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(16, 132);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(556, 167);
            this.groupControl2.TabIndex = 45;
            this.groupControl2.Text = "吹气";
            // 
            // SpBlowDelay
            // 
            this.SpBlowDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpBlowDelay.Location = new System.Drawing.Point(134, 116);
            this.SpBlowDelay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpBlowDelay.Name = "SpBlowDelay";
            this.SpBlowDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBlowDelay.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpBlowDelay.Properties.IsFloatValue = false;
            this.SpBlowDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpBlowDelay.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpBlowDelay.Size = new System.Drawing.Size(208, 24);
            this.SpBlowDelay.TabIndex = 56;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(24, 122);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 18);
            this.labelControl2.TabIndex = 55;
            this.labelControl2.Text = "延时";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(24, 59);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 18);
            this.labelControl1.TabIndex = 54;
            this.labelControl1.Text = "比例";
            // 
            // BtnBlow
            // 
            this.BtnBlow.Location = new System.Drawing.Point(377, 49);
            this.BtnBlow.Name = "BtnBlow";
            this.BtnBlow.Size = new System.Drawing.Size(133, 37);
            this.BtnBlow.TabIndex = 53;
            this.BtnBlow.Text = "吹气 开/关";
            this.BtnBlow.Click += new System.EventHandler(this.BtnBlow_Click);
            // 
            // SpBlowProportion
            // 
            this.SpBlowProportion.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpBlowProportion.Location = new System.Drawing.Point(134, 56);
            this.SpBlowProportion.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpBlowProportion.Name = "SpBlowProportion";
            this.SpBlowProportion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBlowProportion.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpBlowProportion.Properties.IsFloatValue = false;
            this.SpBlowProportion.Properties.MaskSettings.Set("mask", "N00");
            this.SpBlowProportion.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpBlowProportion.Size = new System.Drawing.Size(208, 24);
            this.SpBlowProportion.TabIndex = 52;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.BtGetCurrentVacuumValue);
            this.groupControl3.Controls.Add(this.SpVacuumCheckDelay);
            this.groupControl3.Controls.Add(this.labelControl3);
            this.groupControl3.Controls.Add(this.labelControl4);
            this.groupControl3.Controls.Add(this.SpVacuumCheckVal);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(16, 334);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(556, 166);
            this.groupControl3.TabIndex = 57;
            this.groupControl3.Text = "漏晶检测";
            // 
            // BtGetCurrentVacuumValue
            // 
            this.BtGetCurrentVacuumValue.Location = new System.Drawing.Point(377, 47);
            this.BtGetCurrentVacuumValue.Name = "BtGetCurrentVacuumValue";
            this.BtGetCurrentVacuumValue.Size = new System.Drawing.Size(147, 41);
            this.BtGetCurrentVacuumValue.TabIndex = 57;
            this.BtGetCurrentVacuumValue.Text = "获取当前真空模拟量";
            this.BtGetCurrentVacuumValue.Click += new System.EventHandler(this.BtGetCurrentVacuumValue_Click);
            // 
            // SpVacuumCheckDelay
            // 
            this.SpVacuumCheckDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacuumCheckDelay.Location = new System.Drawing.Point(134, 116);
            this.SpVacuumCheckDelay.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpVacuumCheckDelay.Name = "SpVacuumCheckDelay";
            this.SpVacuumCheckDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacuumCheckDelay.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpVacuumCheckDelay.Properties.IsFloatValue = false;
            this.SpVacuumCheckDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpVacuumCheckDelay.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpVacuumCheckDelay.Size = new System.Drawing.Size(208, 24);
            this.SpVacuumCheckDelay.TabIndex = 56;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(24, 119);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(30, 18);
            this.labelControl3.TabIndex = 55;
            this.labelControl3.Text = "延时";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(24, 59);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 54;
            this.labelControl4.Text = "检测值";
            // 
            // SpVacuumCheckVal
            // 
            this.SpVacuumCheckVal.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacuumCheckVal.Location = new System.Drawing.Point(134, 56);
            this.SpVacuumCheckVal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpVacuumCheckVal.Name = "SpVacuumCheckVal";
            this.SpVacuumCheckVal.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacuumCheckVal.Properties.EditValueChangedFiringMode = DevExpress.XtraEditors.Controls.EditValueChangedFiringMode.Default;
            this.SpVacuumCheckVal.Properties.IsFloatValue = false;
            this.SpVacuumCheckVal.Properties.MaskSettings.Set("mask", "N00");
            this.SpVacuumCheckVal.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpVacuumCheckVal.Size = new System.Drawing.Size(208, 24);
            this.SpVacuumCheckVal.TabIndex = 52;
            // 
            // UcFlipToolEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "UcFlipToolEdit";
            this.Size = new System.Drawing.Size(911, 636);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbNozzle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlowDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlowProportion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckVal.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ComboBoxEdit CmbNozzle;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton BtnBlow;
        private DevExpress.XtraEditors.SpinEdit SpBlowProportion;
        private DevExpress.XtraEditors.SpinEdit SpBlowDelay;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SpinEdit SpVacuumCheckDelay;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit SpVacuumCheckVal;
        private DevExpress.XtraEditors.SimpleButton BtGetCurrentVacuumValue;
    }
}
