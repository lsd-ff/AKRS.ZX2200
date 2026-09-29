namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmMachineFunctionTest
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
            this.gPAutoChangeTool = new DevExpress.XtraEditors.GroupControl();
            this.BtnESToolsTestRun = new DevExpress.XtraEditors.SimpleButton();
            this.BtnPPToolsTestRun = new DevExpress.XtraEditors.SimpleButton();
            this.BtnWaferChangeTest = new DevExpress.XtraEditors.SimpleButton();
            this.gPThreePointOneLineCali = new DevExpress.XtraEditors.GroupControl();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SpEjectionCaliTime = new DevExpress.XtraEditors.SpinEdit();
            this.SpNozzleCaliTime = new DevExpress.XtraEditors.SpinEdit();
            this.SpPickOffsetCaliTime = new DevExpress.XtraEditors.SpinEdit();
            this.BtnCaliPickOffset = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCaliNozzle = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCaliEjection = new DevExpress.XtraEditors.SimpleButton();
            this.gPForceControl = new DevExpress.XtraEditors.GroupControl();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SpForceControlTestTime = new DevExpress.XtraEditors.SpinEdit();
            this.BtnForceControlTest = new DevExpress.XtraEditors.SimpleButton();
            this.gPMeasureHeight = new DevExpress.XtraEditors.GroupControl();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SpMeasureHeightTime = new DevExpress.XtraEditors.SpinEdit();
            this.BtnMeasureHeight = new DevExpress.XtraEditors.SimpleButton();
            this.LbResult = new DevExpress.XtraEditors.LabelControl();
            this.BtnTeachEjection = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTeachNozzle = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTeachComponent = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.gPAutoChangeTool)).BeginInit();
            this.gPAutoChangeTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gPThreePointOneLineCali)).BeginInit();
            this.gPThreePointOneLineCali.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpEjectionCaliTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpNozzleCaliTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPickOffsetCaliTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPForceControl)).BeginInit();
            this.gPForceControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceControlTestTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPMeasureHeight)).BeginInit();
            this.gPMeasureHeight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpMeasureHeightTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // gPAutoChangeTool
            // 
            this.gPAutoChangeTool.Controls.Add(this.BtnESToolsTestRun);
            this.gPAutoChangeTool.Controls.Add(this.BtnPPToolsTestRun);
            this.gPAutoChangeTool.Controls.Add(this.BtnWaferChangeTest);
            this.gPAutoChangeTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.gPAutoChangeTool.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gPAutoChangeTool.Location = new System.Drawing.Point(0, 0);
            this.gPAutoChangeTool.Name = "gPAutoChangeTool";
            this.gPAutoChangeTool.Size = new System.Drawing.Size(1000, 136);
            this.gPAutoChangeTool.TabIndex = 0;
            this.gPAutoChangeTool.Text = "自动更换";
            // 
            // BtnESToolsTestRun
            // 
            this.BtnESToolsTestRun.Location = new System.Drawing.Point(353, 59);
            this.BtnESToolsTestRun.Name = "BtnESToolsTestRun";
            this.BtnESToolsTestRun.Size = new System.Drawing.Size(213, 39);
            this.BtnESToolsTestRun.TabIndex = 6;
            this.BtnESToolsTestRun.Text = "开始重复换顶针测试";
            this.BtnESToolsTestRun.Click += new System.EventHandler(this.BtnESToolsTestRun_Click);
            // 
            // BtnPPToolsTestRun
            // 
            this.BtnPPToolsTestRun.Location = new System.Drawing.Point(50, 59);
            this.BtnPPToolsTestRun.Name = "BtnPPToolsTestRun";
            this.BtnPPToolsTestRun.Size = new System.Drawing.Size(213, 39);
            this.BtnPPToolsTestRun.TabIndex = 5;
            this.BtnPPToolsTestRun.Text = "开始重复换吸嘴测试";
            this.BtnPPToolsTestRun.Click += new System.EventHandler(this.BtnPPToolsTestRun_Click);
            // 
            // BtnWaferChangeTest
            // 
            this.BtnWaferChangeTest.Location = new System.Drawing.Point(640, 59);
            this.BtnWaferChangeTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnWaferChangeTest.Name = "BtnWaferChangeTest";
            this.BtnWaferChangeTest.Size = new System.Drawing.Size(281, 39);
            this.BtnWaferChangeTest.TabIndex = 4;
            this.BtnWaferChangeTest.Tag = "Wafer change test-run on";
            this.BtnWaferChangeTest.Text = "自动换晶圆测试";
            this.BtnWaferChangeTest.Click += new System.EventHandler(this.BtnWaferChangeTest_Click);
            // 
            // gPThreePointOneLineCali
            // 
            this.gPThreePointOneLineCali.Controls.Add(this.BtnTeachComponent);
            this.gPThreePointOneLineCali.Controls.Add(this.BtnTeachNozzle);
            this.gPThreePointOneLineCali.Controls.Add(this.BtnTeachEjection);
            this.gPThreePointOneLineCali.Controls.Add(this.label3);
            this.gPThreePointOneLineCali.Controls.Add(this.label2);
            this.gPThreePointOneLineCali.Controls.Add(this.label1);
            this.gPThreePointOneLineCali.Controls.Add(this.SpEjectionCaliTime);
            this.gPThreePointOneLineCali.Controls.Add(this.SpNozzleCaliTime);
            this.gPThreePointOneLineCali.Controls.Add(this.SpPickOffsetCaliTime);
            this.gPThreePointOneLineCali.Controls.Add(this.BtnCaliPickOffset);
            this.gPThreePointOneLineCali.Controls.Add(this.BtnCaliNozzle);
            this.gPThreePointOneLineCali.Controls.Add(this.BtnCaliEjection);
            this.gPThreePointOneLineCali.Dock = System.Windows.Forms.DockStyle.Top;
            this.gPThreePointOneLineCali.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gPThreePointOneLineCali.Location = new System.Drawing.Point(0, 136);
            this.gPThreePointOneLineCali.Name = "gPThreePointOneLineCali";
            this.gPThreePointOneLineCali.Size = new System.Drawing.Size(1000, 207);
            this.gPThreePointOneLineCali.TabIndex = 1;
            this.gPThreePointOneLineCali.Text = "三点一线校准";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(47, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 18);
            this.label3.TabIndex = 59;
            this.label3.Text = "次数";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(350, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 18);
            this.label2.TabIndex = 58;
            this.label2.Text = "次数";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(696, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 18);
            this.label1.TabIndex = 57;
            this.label1.Text = "次数";
            // 
            // SpEjectionCaliTime
            // 
            this.SpEjectionCaliTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpEjectionCaliTime.Location = new System.Drawing.Point(121, 40);
            this.SpEjectionCaliTime.Name = "SpEjectionCaliTime";
            this.SpEjectionCaliTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpEjectionCaliTime.Properties.IsFloatValue = false;
            this.SpEjectionCaliTime.Properties.MaskSettings.Set("mask", "N00");
            this.SpEjectionCaliTime.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.SpEjectionCaliTime.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpEjectionCaliTime.Size = new System.Drawing.Size(142, 24);
            this.SpEjectionCaliTime.TabIndex = 56;
            // 
            // SpNozzleCaliTime
            // 
            this.SpNozzleCaliTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpNozzleCaliTime.Location = new System.Drawing.Point(424, 37);
            this.SpNozzleCaliTime.Name = "SpNozzleCaliTime";
            this.SpNozzleCaliTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpNozzleCaliTime.Properties.IsFloatValue = false;
            this.SpNozzleCaliTime.Properties.MaskSettings.Set("mask", "N00");
            this.SpNozzleCaliTime.Properties.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.SpNozzleCaliTime.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpNozzleCaliTime.Size = new System.Drawing.Size(142, 24);
            this.SpNozzleCaliTime.TabIndex = 55;
            // 
            // SpPickOffsetCaliTime
            // 
            this.SpPickOffsetCaliTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpPickOffsetCaliTime.Location = new System.Drawing.Point(770, 37);
            this.SpPickOffsetCaliTime.Name = "SpPickOffsetCaliTime";
            this.SpPickOffsetCaliTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpPickOffsetCaliTime.Properties.IsFloatValue = false;
            this.SpPickOffsetCaliTime.Properties.MaskSettings.Set("mask", "N00");
            this.SpPickOffsetCaliTime.Properties.MaxValue = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.SpPickOffsetCaliTime.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpPickOffsetCaliTime.Size = new System.Drawing.Size(142, 24);
            this.SpPickOffsetCaliTime.TabIndex = 54;
            // 
            // BtnCaliPickOffset
            // 
            this.BtnCaliPickOffset.Location = new System.Drawing.Point(708, 145);
            this.BtnCaliPickOffset.Name = "BtnCaliPickOffset";
            this.BtnCaliPickOffset.Size = new System.Drawing.Size(213, 39);
            this.BtnCaliPickOffset.TabIndex = 53;
            this.BtnCaliPickOffset.Text = "校准取片偏移";
            this.BtnCaliPickOffset.Click += new System.EventHandler(this.BtnCaliPickOffset_Click);
            // 
            // BtnCaliNozzle
            // 
            this.BtnCaliNozzle.Location = new System.Drawing.Point(366, 145);
            this.BtnCaliNozzle.Name = "BtnCaliNozzle";
            this.BtnCaliNozzle.Size = new System.Drawing.Size(213, 39);
            this.BtnCaliNozzle.TabIndex = 52;
            this.BtnCaliNozzle.Text = "校准吸嘴";
            this.BtnCaliNozzle.Click += new System.EventHandler(this.BtnCaliNozzle_Click);
            // 
            // BtnCaliEjection
            // 
            this.BtnCaliEjection.Location = new System.Drawing.Point(59, 145);
            this.BtnCaliEjection.Name = "BtnCaliEjection";
            this.BtnCaliEjection.Size = new System.Drawing.Size(213, 39);
            this.BtnCaliEjection.TabIndex = 51;
            this.BtnCaliEjection.Text = "校准顶针";
            this.BtnCaliEjection.Click += new System.EventHandler(this.BtnCaliEjection_Click);
            // 
            // gPForceControl
            // 
            this.gPForceControl.Controls.Add(this.label6);
            this.gPForceControl.Controls.Add(this.label5);
            this.gPForceControl.Controls.Add(this.SpForceControlTestTime);
            this.gPForceControl.Controls.Add(this.BtnForceControlTest);
            this.gPForceControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.gPForceControl.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gPForceControl.Location = new System.Drawing.Point(0, 343);
            this.gPForceControl.Name = "gPForceControl";
            this.gPForceControl.Size = new System.Drawing.Size(1000, 153);
            this.gPForceControl.TabIndex = 54;
            this.gPForceControl.Text = "力控";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(585, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(16, 18);
            this.label6.TabIndex = 61;
            this.label6.Text = "h";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(354, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 18);
            this.label5.TabIndex = 60;
            this.label5.Text = "时间";
            // 
            // SpForceControlTestTime
            // 
            this.SpForceControlTestTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpForceControlTestTime.Location = new System.Drawing.Point(428, 41);
            this.SpForceControlTestTime.Name = "SpForceControlTestTime";
            this.SpForceControlTestTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForceControlTestTime.Size = new System.Drawing.Size(142, 24);
            this.SpForceControlTestTime.TabIndex = 56;
            // 
            // BtnForceControlTest
            // 
            this.BtnForceControlTest.Location = new System.Drawing.Point(357, 86);
            this.BtnForceControlTest.Name = "BtnForceControlTest";
            this.BtnForceControlTest.Size = new System.Drawing.Size(213, 39);
            this.BtnForceControlTest.TabIndex = 53;
            this.BtnForceControlTest.Text = "焊头力控测试";
            this.BtnForceControlTest.Click += new System.EventHandler(this.BtnForceControlTest_Click);
            // 
            // gPMeasureHeight
            // 
            this.gPMeasureHeight.Controls.Add(this.label7);
            this.gPMeasureHeight.Controls.Add(this.label4);
            this.gPMeasureHeight.Controls.Add(this.SpMeasureHeightTime);
            this.gPMeasureHeight.Controls.Add(this.BtnMeasureHeight);
            this.gPMeasureHeight.Dock = System.Windows.Forms.DockStyle.Top;
            this.gPMeasureHeight.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gPMeasureHeight.Location = new System.Drawing.Point(0, 496);
            this.gPMeasureHeight.Name = "gPMeasureHeight";
            this.gPMeasureHeight.Size = new System.Drawing.Size(1000, 168);
            this.gPMeasureHeight.TabIndex = 55;
            this.gPMeasureHeight.Text = "测高";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(585, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(16, 18);
            this.label7.TabIndex = 62;
            this.label7.Text = "h";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 18);
            this.label4.TabIndex = 60;
            this.label4.Text = "时间";
            // 
            // SpMeasureHeightTime
            // 
            this.SpMeasureHeightTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpMeasureHeightTime.Location = new System.Drawing.Point(424, 46);
            this.SpMeasureHeightTime.Name = "SpMeasureHeightTime";
            this.SpMeasureHeightTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMeasureHeightTime.Properties.MaskSettings.Set("mask", "");
            this.SpMeasureHeightTime.Properties.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpMeasureHeightTime.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpMeasureHeightTime.Size = new System.Drawing.Size(142, 24);
            this.SpMeasureHeightTime.TabIndex = 57;
            // 
            // BtnMeasureHeight
            // 
            this.BtnMeasureHeight.Location = new System.Drawing.Point(357, 98);
            this.BtnMeasureHeight.Name = "BtnMeasureHeight";
            this.BtnMeasureHeight.Size = new System.Drawing.Size(213, 39);
            this.BtnMeasureHeight.TabIndex = 53;
            this.BtnMeasureHeight.Text = "测高重复性测试";
            this.BtnMeasureHeight.Click += new System.EventHandler(this.BtnMeasureHeight_Click);
            // 
            // LbResult
            // 
            this.LbResult.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbResult.Appearance.ForeColor = System.Drawing.Color.Red;
            this.LbResult.Appearance.Options.UseFont = true;
            this.LbResult.Appearance.Options.UseForeColor = true;
            this.LbResult.Location = new System.Drawing.Point(424, 688);
            this.LbResult.Name = "LbResult";
            this.LbResult.Size = new System.Drawing.Size(103, 22);
            this.LbResult.TabIndex = 56;
            this.LbResult.Text = "labelControl1";
            // 
            // BtnTeachEjection
            // 
            this.BtnTeachEjection.Location = new System.Drawing.Point(59, 86);
            this.BtnTeachEjection.Name = "BtnTeachEjection";
            this.BtnTeachEjection.Size = new System.Drawing.Size(213, 39);
            this.BtnTeachEjection.TabIndex = 60;
            this.BtnTeachEjection.Text = "示教";
            this.BtnTeachEjection.Click += new System.EventHandler(this.BtnTeachEjection_Click);
            // 
            // BtnTeachNozzle
            // 
            this.BtnTeachNozzle.Location = new System.Drawing.Point(366, 86);
            this.BtnTeachNozzle.Name = "BtnTeachNozzle";
            this.BtnTeachNozzle.Size = new System.Drawing.Size(213, 39);
            this.BtnTeachNozzle.TabIndex = 61;
            this.BtnTeachNozzle.Text = "示教";
            this.BtnTeachNozzle.Click += new System.EventHandler(this.BtnTeachNozzle_Click);
            // 
            // BtnTeachComponent
            // 
            this.BtnTeachComponent.Location = new System.Drawing.Point(708, 86);
            this.BtnTeachComponent.Name = "BtnTeachComponent";
            this.BtnTeachComponent.Size = new System.Drawing.Size(213, 39);
            this.BtnTeachComponent.TabIndex = 62;
            this.BtnTeachComponent.Text = "示教";
            this.BtnTeachComponent.Click += new System.EventHandler(this.BtnTeachComponent_Click);
            // 
            // FrmMachineFunctionTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 722);
            this.Controls.Add(this.LbResult);
            this.Controls.Add(this.gPMeasureHeight);
            this.Controls.Add(this.gPForceControl);
            this.Controls.Add(this.gPThreePointOneLineCali);
            this.Controls.Add(this.gPAutoChangeTool);
            this.Name = "FrmMachineFunctionTest";
            this.Text = "设备功能测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMachineFunctionTest_FormClosing);
            this.Load += new System.EventHandler(this.FrmMachineFunctionTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gPAutoChangeTool)).EndInit();
            this.gPAutoChangeTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gPThreePointOneLineCali)).EndInit();
            this.gPThreePointOneLineCali.ResumeLayout(false);
            this.gPThreePointOneLineCali.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpEjectionCaliTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpNozzleCaliTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPickOffsetCaliTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPForceControl)).EndInit();
            this.gPForceControl.ResumeLayout(false);
            this.gPForceControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceControlTestTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPMeasureHeight)).EndInit();
            this.gPMeasureHeight.ResumeLayout(false);
            this.gPMeasureHeight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpMeasureHeightTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl gPAutoChangeTool;
        private DevExpress.XtraEditors.GroupControl gPThreePointOneLineCali;
        private DevExpress.XtraEditors.SimpleButton BtnWaferChangeTest;
        private DevExpress.XtraEditors.SimpleButton BtnESToolsTestRun;
        private DevExpress.XtraEditors.SimpleButton BtnPPToolsTestRun;
        private DevExpress.XtraEditors.SimpleButton BtnCaliEjection;
        private DevExpress.XtraEditors.SimpleButton BtnCaliNozzle;
        private DevExpress.XtraEditors.SimpleButton BtnCaliPickOffset;
        private DevExpress.XtraEditors.GroupControl gPForceControl;
        private DevExpress.XtraEditors.SimpleButton BtnForceControlTest;
        private DevExpress.XtraEditors.GroupControl gPMeasureHeight;
        private DevExpress.XtraEditors.SimpleButton BtnMeasureHeight;
        private DevExpress.XtraEditors.SpinEdit SpEjectionCaliTime;
        private DevExpress.XtraEditors.SpinEdit SpNozzleCaliTime;
        private DevExpress.XtraEditors.SpinEdit SpPickOffsetCaliTime;
        private DevExpress.XtraEditors.SpinEdit SpForceControlTestTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private DevExpress.XtraEditors.SpinEdit SpMeasureHeightTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraEditors.LabelControl LbResult;
        private System.Windows.Forms.Label label7;
        private DevExpress.XtraEditors.SimpleButton BtnTeachEjection;
        private DevExpress.XtraEditors.SimpleButton BtnTeachComponent;
        private DevExpress.XtraEditors.SimpleButton BtnTeachNozzle;
    }
}