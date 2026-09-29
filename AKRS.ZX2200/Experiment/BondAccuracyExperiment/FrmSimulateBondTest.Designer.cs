namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    partial class FrmSimulateBondTest
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
            this.SpUplookAngleLimit = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.ChkEnableEditor = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl20 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.SpCompensateAngle = new DevExpress.XtraEditors.SpinEdit();
            this.SpCompensateX = new DevExpress.XtraEditors.SpinEdit();
            this.SpCompensateY = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.SpOriginY = new DevExpress.XtraEditors.SpinEdit();
            this.SpOriginX = new DevExpress.XtraEditors.SpinEdit();
            this.BtnSetOrigin = new DevExpress.XtraEditors.SimpleButton();
            this.LueOriginAdjustType = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpUplookAngleLimit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkEnableEditor.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueOriginAdjustType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpVisionTime
            // 
            this.SpVisionTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpVisionTime.Location = new System.Drawing.Point(171, 94);
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
            this.LbCycle.Location = new System.Drawing.Point(435, 246);
            this.LbCycle.Name = "LbCycle";
            this.LbCycle.Size = new System.Drawing.Size(39, 18);
            this.LbCycle.TabIndex = 58;
            this.LbCycle.Text = "Cycle:";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(211, 303);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(125, 35);
            this.BtnStart.TabIndex = 57;
            this.BtnStart.Text = "开始";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // ChkMoveToCameraCenterAfterVision
            // 
            this.ChkMoveToCameraCenterAfterVision.Location = new System.Drawing.Point(39, 243);
            this.ChkMoveToCameraCenterAfterVision.Name = "ChkMoveToCameraCenterAfterVision";
            this.ChkMoveToCameraCenterAfterVision.Properties.Caption = "定位后移动到相机中心";
            this.ChkMoveToCameraCenterAfterVision.Size = new System.Drawing.Size(259, 24);
            this.ChkMoveToCameraCenterAfterVision.TabIndex = 56;
            // 
            // labelControl23
            // 
            this.labelControl23.Location = new System.Drawing.Point(62, 96);
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
            this.SpCycles.Location = new System.Drawing.Point(171, 30);
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
            this.labelControl1.Location = new System.Drawing.Point(62, 32);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 53;
            this.labelControl1.Text = "循环次数";
            // 
            // SpUplookAngleLimit
            // 
            this.SpUplookAngleLimit.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpUplookAngleLimit.Location = new System.Drawing.Point(171, 161);
            this.SpUplookAngleLimit.Name = "SpUplookAngleLimit";
            this.SpUplookAngleLimit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpUplookAngleLimit.Properties.MaskSettings.Set("mask", "");
            this.SpUplookAngleLimit.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpUplookAngleLimit.Size = new System.Drawing.Size(291, 24);
            this.SpUplookAngleLimit.TabIndex = 61;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(30, 165);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 18);
            this.labelControl2.TabIndex = 60;
            this.labelControl2.Text = "上视角度矫正阈值";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.ChkEnableEditor);
            this.groupControl1.Controls.Add(this.labelControl21);
            this.groupControl1.Controls.Add(this.labelControl20);
            this.groupControl1.Controls.Add(this.labelControl8);
            this.groupControl1.Controls.Add(this.SpCompensateAngle);
            this.groupControl1.Controls.Add(this.SpCompensateX);
            this.groupControl1.Controls.Add(this.SpCompensateY);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(14, 374);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(640, 170);
            this.groupControl1.TabIndex = 62;
            this.groupControl1.Text = "Compensation  value";
            // 
            // ChkEnableEditor
            // 
            this.ChkEnableEditor.Location = new System.Drawing.Point(530, 31);
            this.ChkEnableEditor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ChkEnableEditor.Name = "ChkEnableEditor";
            this.ChkEnableEditor.Properties.Caption = "编辑补偿值";
            this.ChkEnableEditor.Size = new System.Drawing.Size(104, 24);
            this.ChkEnableEditor.TabIndex = 65;
            this.ChkEnableEditor.CheckedChanged += new System.EventHandler(this.ChkEnableEditor_CheckedChanged);
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(477, 84);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(26, 18);
            this.labelControl21.TabIndex = 34;
            this.labelControl21.Text = "mm";
            // 
            // labelControl20
            // 
            this.labelControl20.Location = new System.Drawing.Point(477, 33);
            this.labelControl20.Name = "labelControl20";
            this.labelControl20.Size = new System.Drawing.Size(26, 18);
            this.labelControl20.TabIndex = 33;
            this.labelControl20.Text = "mm";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(472, 129);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(7, 18);
            this.labelControl8.TabIndex = 8;
            this.labelControl8.Text = "°";
            // 
            // SpCompensateAngle
            // 
            this.SpCompensateAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCompensateAngle.Location = new System.Drawing.Point(175, 129);
            this.SpCompensateAngle.Name = "SpCompensateAngle";
            this.SpCompensateAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCompensateAngle.Size = new System.Drawing.Size(291, 24);
            this.SpCompensateAngle.TabIndex = 12;
            // 
            // SpCompensateX
            // 
            this.SpCompensateX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCompensateX.Location = new System.Drawing.Point(175, 31);
            this.SpCompensateX.Name = "SpCompensateX";
            this.SpCompensateX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCompensateX.Size = new System.Drawing.Size(291, 24);
            this.SpCompensateX.TabIndex = 8;
            // 
            // SpCompensateY
            // 
            this.SpCompensateY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpCompensateY.Location = new System.Drawing.Point(175, 78);
            this.SpCompensateY.Name = "SpCompensateY";
            this.SpCompensateY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCompensateY.Size = new System.Drawing.Size(291, 24);
            this.SpCompensateY.TabIndex = 10;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(37, 131);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(44, 18);
            this.labelControl4.TabIndex = 11;
            this.labelControl4.Text = "Theta ";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(51, 85);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(10, 18);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "Y";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(51, 33);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(9, 18);
            this.labelControl6.TabIndex = 7;
            this.labelControl6.Text = "X";
            // 
            // SpOriginY
            // 
            this.SpOriginY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOriginY.Location = new System.Drawing.Point(319, 598);
            this.SpOriginY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpOriginY.Name = "SpOriginY";
            this.SpOriginY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOriginY.Size = new System.Drawing.Size(155, 24);
            this.SpOriginY.TabIndex = 63;
            // 
            // SpOriginX
            // 
            this.SpOriginX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOriginX.Location = new System.Drawing.Point(153, 598);
            this.SpOriginX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpOriginX.Name = "SpOriginX";
            this.SpOriginX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOriginX.Size = new System.Drawing.Size(155, 24);
            this.SpOriginX.TabIndex = 63;
            // 
            // BtnSetOrigin
            // 
            this.BtnSetOrigin.Location = new System.Drawing.Point(349, 646);
            this.BtnSetOrigin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnSetOrigin.Name = "BtnSetOrigin";
            this.BtnSetOrigin.Size = new System.Drawing.Size(82, 31);
            this.BtnSetOrigin.TabIndex = 64;
            this.BtnSetOrigin.Text = "原点纠正";
            this.BtnSetOrigin.Click += new System.EventHandler(this.BtnSetOrigin_Click);
            // 
            // LueOriginAdjustType
            // 
            this.LueOriginAdjustType.Location = new System.Drawing.Point(156, 650);
            this.LueOriginAdjustType.Name = "LueOriginAdjustType";
            this.LueOriginAdjustType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueOriginAdjustType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "", 19, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueOriginAdjustType.Properties.DisplayMember = "Display";
            this.LueOriginAdjustType.Properties.DropDownRows = 5;
            this.LueOriginAdjustType.Properties.NullText = "";
            this.LueOriginAdjustType.Properties.ValueMember = "Value";
            this.LueOriginAdjustType.Size = new System.Drawing.Size(142, 24);
            this.LueOriginAdjustType.TabIndex = 65;
            // 
            // FrmSimulateBondTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 775);
            this.Controls.Add(this.LueOriginAdjustType);
            this.Controls.Add(this.BtnSetOrigin);
            this.Controls.Add(this.SpOriginX);
            this.Controls.Add(this.SpOriginY);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.SpUplookAngleLimit);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.SpVisionTime);
            this.Controls.Add(this.LbCycle);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.ChkMoveToCameraCenterAfterVision);
            this.Controls.Add(this.labelControl23);
            this.Controls.Add(this.SpCycles);
            this.Controls.Add(this.labelControl1);
            this.Name = "FrmSimulateBondTest";
            this.Text = "模拟贴片实验";
            this.Load += new System.EventHandler(this.FrmSimulateBondTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpUplookAngleLimit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkEnableEditor.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCompensateY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOriginX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueOriginAdjustType.Properties)).EndInit();
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
        private DevExpress.XtraEditors.SpinEdit SpUplookAngleLimit;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.LabelControl labelControl20;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SpinEdit SpCompensateAngle;
        private DevExpress.XtraEditors.SpinEdit SpCompensateX;
        private DevExpress.XtraEditors.SpinEdit SpCompensateY;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.SpinEdit SpOriginY;
        private DevExpress.XtraEditors.SpinEdit SpOriginX;
        private DevExpress.XtraEditors.SimpleButton BtnSetOrigin;
        private DevExpress.XtraEditors.CheckEdit ChkEnableEditor;
        private DevExpress.XtraEditors.LookUpEdit LueOriginAdjustType;
    }
}