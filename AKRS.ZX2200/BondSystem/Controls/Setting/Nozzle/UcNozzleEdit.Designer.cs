namespace AKRS.ZX2200.BondSystem.Controls.Setting.Nozzle
{
    partial class UcNozzleEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LueNozzleShape = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.LueNozzleHeightMeasurementFunction = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.LbSlotIdentification = new DevExpress.XtraEditors.LabelControl();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.RgZDeterminationMethod = new DevExpress.XtraEditors.RadioGroup();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.RgXYDeterminationMethod = new DevExpress.XtraEditors.RadioGroup();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpVacummCheckDelay = new DevExpress.XtraEditors.SpinEdit();
            this.SpVacuumCheckAfterPickUp = new DevExpress.XtraEditors.SpinEdit();
            this.BtGetCurrentVacuumValue = new DevExpress.XtraEditors.SimpleButton();
            this.BtGetAfterBondingVacuumValue = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl7 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.SpVacuumCheckDelayAfterBonding = new DevExpress.XtraEditors.SpinEdit();
            this.SpVacuumCheckValueAfterBonding = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueNozzleShape.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueNozzleHeightMeasurementFunction.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RgZDeterminationMethod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RgXYDeterminationMethod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacummCheckDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckAfterPickUp.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).BeginInit();
            this.groupControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckDelayAfterBonding.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckValueAfterBonding.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.LueNozzleShape);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(501, 22);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(375, 109);
            this.groupControl2.TabIndex = 3;
            this.groupControl2.Text = "吸嘴形状";
            // 
            // LueNozzleShape
            // 
            this.LueNozzleShape.Location = new System.Drawing.Point(34, 45);
            this.LueNozzleShape.Name = "LueNozzleShape";
            this.LueNozzleShape.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueNozzleShape.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueNozzleShape.Properties.DisplayMember = "Display";
            this.LueNozzleShape.Properties.DropDownRows = 5;
            this.LueNozzleShape.Properties.NullText = "";
            this.LueNozzleShape.Properties.PopupSizeable = false;
            this.LueNozzleShape.Properties.PopupWidth = 299;
            this.LueNozzleShape.Properties.PopupWidthMode = DevExpress.XtraEditors.PopupWidthMode.ContentWidth;
            this.LueNozzleShape.Properties.ValueMember = "Value";
            this.LueNozzleShape.Size = new System.Drawing.Size(299, 24);
            this.LueNozzleShape.TabIndex = 30;
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.LueNozzleHeightMeasurementFunction);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(30, 19);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(375, 112);
            this.groupControl3.TabIndex = 3;
            this.groupControl3.Text = "测高模式";
            // 
            // LueNozzleHeightMeasurementFunction
            // 
            this.LueNozzleHeightMeasurementFunction.Location = new System.Drawing.Point(37, 48);
            this.LueNozzleHeightMeasurementFunction.Name = "LueNozzleHeightMeasurementFunction";
            this.LueNozzleHeightMeasurementFunction.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueNozzleHeightMeasurementFunction.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueNozzleHeightMeasurementFunction.Properties.DisplayMember = "Display";
            this.LueNozzleHeightMeasurementFunction.Properties.DropDownRows = 5;
            this.LueNozzleHeightMeasurementFunction.Properties.NullText = "";
            this.LueNozzleHeightMeasurementFunction.Properties.ValueMember = "Value";
            this.LueNozzleHeightMeasurementFunction.Size = new System.Drawing.Size(299, 24);
            this.LueNozzleHeightMeasurementFunction.TabIndex = 29;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.LbSlotIdentification);
            this.groupControl5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl5.Location = new System.Drawing.Point(30, 147);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(375, 72);
            this.groupControl5.TabIndex = 6;
            this.groupControl5.Text = "所在槽位";
            // 
            // LbSlotIdentification
            // 
            this.LbSlotIdentification.Location = new System.Drawing.Point(141, 31);
            this.LbSlotIdentification.Name = "LbSlotIdentification";
            this.LbSlotIdentification.Size = new System.Drawing.Size(81, 18);
            this.LbSlotIdentification.TabIndex = 7;
            this.LbSlotIdentification.Text = "labelControl1";
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.RgZDeterminationMethod);
            this.groupControl6.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl6.Location = new System.Drawing.Point(30, 242);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(375, 165);
            this.groupControl6.TabIndex = 6;
            this.groupControl6.Text = "确定Z向高度";
            // 
            // RgZDeterminationMethod
            // 
            this.RgZDeterminationMethod.Location = new System.Drawing.Point(15, 40);
            this.RgZDeterminationMethod.Name = "RgZDeterminationMethod";
            this.RgZDeterminationMethod.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "手动"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "Mini BMC"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "力传感器")});
            this.RgZDeterminationMethod.Size = new System.Drawing.Size(342, 105);
            this.RgZDeterminationMethod.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.RgXYDeterminationMethod);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(30, 427);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(375, 140);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "确定XY偏移";
            // 
            // RgXYDeterminationMethod
            // 
            this.RgXYDeterminationMethod.Location = new System.Drawing.Point(15, 40);
            this.RgXYDeterminationMethod.Name = "RgXYDeterminationMethod";
            this.RgXYDeterminationMethod.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "基板相机"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "上视相机")});
            this.RgXYDeterminationMethod.Size = new System.Drawing.Size(342, 80);
            this.RgXYDeterminationMethod.TabIndex = 0;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.labelControl2);
            this.groupControl4.Controls.Add(this.labelControl1);
            this.groupControl4.Controls.Add(this.SpVacummCheckDelay);
            this.groupControl4.Controls.Add(this.SpVacuumCheckAfterPickUp);
            this.groupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl4.Location = new System.Drawing.Point(501, 147);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(375, 125);
            this.groupControl4.TabIndex = 31;
            this.groupControl4.Text = "漏晶检测";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(34, 84);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 18);
            this.labelControl2.TabIndex = 4;
            this.labelControl2.Text = "延时";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(31, 34);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(45, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "检测值";
            // 
            // SpVacummCheckDelay
            // 
            this.SpVacummCheckDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacummCheckDelay.Location = new System.Drawing.Point(82, 81);
            this.SpVacummCheckDelay.Name = "SpVacummCheckDelay";
            this.SpVacummCheckDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacummCheckDelay.Size = new System.Drawing.Size(251, 24);
            this.SpVacummCheckDelay.TabIndex = 2;
            // 
            // SpVacuumCheckAfterPickUp
            // 
            this.SpVacuumCheckAfterPickUp.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacuumCheckAfterPickUp.Location = new System.Drawing.Point(82, 31);
            this.SpVacuumCheckAfterPickUp.Name = "SpVacuumCheckAfterPickUp";
            this.SpVacuumCheckAfterPickUp.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacuumCheckAfterPickUp.Size = new System.Drawing.Size(251, 24);
            this.SpVacuumCheckAfterPickUp.TabIndex = 1;
            // 
            // BtGetCurrentVacuumValue
            // 
            this.BtGetCurrentVacuumValue.Location = new System.Drawing.Point(917, 166);
            this.BtGetCurrentVacuumValue.Name = "BtGetCurrentVacuumValue";
            this.BtGetCurrentVacuumValue.Size = new System.Drawing.Size(201, 41);
            this.BtGetCurrentVacuumValue.TabIndex = 32;
            this.BtGetCurrentVacuumValue.Text = "获取当前真空模拟量";
            this.BtGetCurrentVacuumValue.Click += new System.EventHandler(this.BtGetCurrentVacuumValue_Click);
            // 
            // BtGetAfterBondingVacuumValue
            // 
            this.BtGetAfterBondingVacuumValue.Location = new System.Drawing.Point(917, 325);
            this.BtGetAfterBondingVacuumValue.Name = "BtGetAfterBondingVacuumValue";
            this.BtGetAfterBondingVacuumValue.Size = new System.Drawing.Size(201, 41);
            this.BtGetAfterBondingVacuumValue.TabIndex = 34;
            this.BtGetAfterBondingVacuumValue.Text = "获取当前真空模拟量";
            this.BtGetAfterBondingVacuumValue.Click += new System.EventHandler(this.BtGetAfterBondingVacuumValue_Click);
            // 
            // groupControl7
            // 
            this.groupControl7.Controls.Add(this.labelControl3);
            this.groupControl7.Controls.Add(this.labelControl4);
            this.groupControl7.Controls.Add(this.SpVacuumCheckDelayAfterBonding);
            this.groupControl7.Controls.Add(this.SpVacuumCheckValueAfterBonding);
            this.groupControl7.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl7.Location = new System.Drawing.Point(501, 306);
            this.groupControl7.Name = "groupControl7";
            this.groupControl7.Size = new System.Drawing.Size(375, 134);
            this.groupControl7.TabIndex = 33;
            this.groupControl7.Text = "焊后回带检测";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(34, 84);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(30, 18);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "延时";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(34, 31);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 18);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "检测值";
            // 
            // SpVacuumCheckDelayAfterBonding
            // 
            this.SpVacuumCheckDelayAfterBonding.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacuumCheckDelayAfterBonding.Location = new System.Drawing.Point(82, 81);
            this.SpVacuumCheckDelayAfterBonding.Name = "SpVacuumCheckDelayAfterBonding";
            this.SpVacuumCheckDelayAfterBonding.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacuumCheckDelayAfterBonding.Size = new System.Drawing.Size(251, 24);
            this.SpVacuumCheckDelayAfterBonding.TabIndex = 2;
            // 
            // SpVacuumCheckValueAfterBonding
            // 
            this.SpVacuumCheckValueAfterBonding.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpVacuumCheckValueAfterBonding.Location = new System.Drawing.Point(82, 31);
            this.SpVacuumCheckValueAfterBonding.Name = "SpVacuumCheckValueAfterBonding";
            this.SpVacuumCheckValueAfterBonding.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVacuumCheckValueAfterBonding.Size = new System.Drawing.Size(251, 24);
            this.SpVacuumCheckValueAfterBonding.TabIndex = 1;
            // 
            // UcNozzleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtGetAfterBondingVacuumValue);
            this.Controls.Add(this.groupControl7);
            this.Controls.Add(this.BtGetCurrentVacuumValue);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl6);
            this.Controls.Add(this.groupControl5);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Name = "UcNozzleEdit";
            this.Size = new System.Drawing.Size(1175, 678);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueNozzleShape.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueNozzleHeightMeasurementFunction.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            this.groupControl5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RgZDeterminationMethod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RgXYDeterminationMethod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.groupControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacummCheckDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckAfterPickUp.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).EndInit();
            this.groupControl7.ResumeLayout(false);
            this.groupControl7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckDelayAfterBonding.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVacuumCheckValueAfterBonding.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.LabelControl LbSlotIdentification;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.RadioGroup RgZDeterminationMethod;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.RadioGroup RgXYDeterminationMethod;
        private DevExpress.XtraEditors.LookUpEdit LueNozzleHeightMeasurementFunction;
        private DevExpress.XtraEditors.LookUpEdit LueNozzleShape;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SpinEdit SpVacuumCheckAfterPickUp;
        private DevExpress.XtraEditors.SimpleButton BtGetCurrentVacuumValue;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpVacummCheckDelay;
        private DevExpress.XtraEditors.SimpleButton BtGetAfterBondingVacuumValue;
        private DevExpress.XtraEditors.GroupControl groupControl7;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SpinEdit SpVacuumCheckDelayAfterBonding;
        private DevExpress.XtraEditors.SpinEdit SpVacuumCheckValueAfterBonding;
    }
}
