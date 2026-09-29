namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    partial class FrmCalibrationRelationship
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.LbTip = new DevExpress.XtraEditors.LabelControl();
            this.SpForceKeepDelay = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.SpLargeForceInitialIncrement = new DevExpress.XtraEditors.SpinEdit();
            this.SpSmallForceInitialIncrement = new DevExpress.XtraEditors.SpinEdit();
            this.SpLargeForceAngleInterval = new DevExpress.XtraEditors.SpinEdit();
            this.SpSmallForceAngleInterval = new DevExpress.XtraEditors.SpinEdit();
            this.SpLargeForceCaliInterval = new DevExpress.XtraEditors.SpinEdit();
            this.SpSmallForceCaliInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.BtnExport = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.CmbCaliMode = new System.Windows.Forms.ComboBox();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.CmbForceRange = new System.Windows.Forms.ComboBox();
            this.BtnStop = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCaliAndTest = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceKeepDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceInitialIncrement.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceInitialIncrement.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceAngleInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceAngleInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceCaliInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceCaliInterval.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(50, 690);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(166, 46);
            this.BtnStart.TabIndex = 1;
            this.BtnStart.Text = "开始标定";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click_1);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.LbTip);
            this.panelControl1.Controls.Add(this.SpForceKeepDelay);
            this.panelControl1.Controls.Add(this.labelControl4);
            this.panelControl1.Controls.Add(this.SpLargeForceInitialIncrement);
            this.panelControl1.Controls.Add(this.SpSmallForceInitialIncrement);
            this.panelControl1.Controls.Add(this.SpLargeForceAngleInterval);
            this.panelControl1.Controls.Add(this.SpSmallForceAngleInterval);
            this.panelControl1.Controls.Add(this.SpLargeForceCaliInterval);
            this.panelControl1.Controls.Add(this.SpSmallForceCaliInterval);
            this.panelControl1.Controls.Add(this.labelControl9);
            this.panelControl1.Controls.Add(this.labelControl8);
            this.panelControl1.Controls.Add(this.labelControl6);
            this.panelControl1.Controls.Add(this.labelControl7);
            this.panelControl1.Controls.Add(this.labelControl5);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.BtnExport);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.CmbCaliMode);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.CmbForceRange);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(715, 651);
            this.panelControl1.TabIndex = 2;
            // 
            // LbTip
            // 
            this.LbTip.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTip.Appearance.ForeColor = System.Drawing.Color.Red;
            this.LbTip.Appearance.Options.UseFont = true;
            this.LbTip.Appearance.Options.UseForeColor = true;
            this.LbTip.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LbTip.Location = new System.Drawing.Point(93, 240);
            this.LbTip.Name = "LbTip";
            this.LbTip.Size = new System.Drawing.Size(489, 102);
            this.LbTip.TabIndex = 20;
            this.LbTip.Text = "标定中。。。。。。";
            this.LbTip.Visible = false;
            // 
            // SpForceKeepDelay
            // 
            this.SpForceKeepDelay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpForceKeepDelay.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpForceKeepDelay.Location = new System.Drawing.Point(320, 509);
            this.SpForceKeepDelay.Name = "SpForceKeepDelay";
            this.SpForceKeepDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForceKeepDelay.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.SpForceKeepDelay.Size = new System.Drawing.Size(139, 24);
            this.SpForceKeepDelay.TabIndex = 19;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(147, 512);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(75, 18);
            this.labelControl4.TabIndex = 18;
            this.labelControl4.Text = "力保持延时";
            // 
            // SpLargeForceInitialIncrement
            // 
            this.SpLargeForceInitialIncrement.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpLargeForceInitialIncrement.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLargeForceInitialIncrement.Location = new System.Drawing.Point(320, 456);
            this.SpLargeForceInitialIncrement.Name = "SpLargeForceInitialIncrement";
            this.SpLargeForceInitialIncrement.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLargeForceInitialIncrement.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.SpLargeForceInitialIncrement.Size = new System.Drawing.Size(139, 24);
            this.SpLargeForceInitialIncrement.TabIndex = 17;
            // 
            // SpSmallForceInitialIncrement
            // 
            this.SpSmallForceInitialIncrement.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpSmallForceInitialIncrement.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSmallForceInitialIncrement.Location = new System.Drawing.Point(320, 396);
            this.SpSmallForceInitialIncrement.Name = "SpSmallForceInitialIncrement";
            this.SpSmallForceInitialIncrement.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSmallForceInitialIncrement.Properties.MaxValue = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.SpSmallForceInitialIncrement.Size = new System.Drawing.Size(139, 24);
            this.SpSmallForceInitialIncrement.TabIndex = 16;
            // 
            // SpLargeForceAngleInterval
            // 
            this.SpLargeForceAngleInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpLargeForceAngleInterval.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLargeForceAngleInterval.Location = new System.Drawing.Point(320, 336);
            this.SpLargeForceAngleInterval.Name = "SpLargeForceAngleInterval";
            this.SpLargeForceAngleInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLargeForceAngleInterval.Properties.MaxValue = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.SpLargeForceAngleInterval.Size = new System.Drawing.Size(139, 24);
            this.SpLargeForceAngleInterval.TabIndex = 15;
            // 
            // SpSmallForceAngleInterval
            // 
            this.SpSmallForceAngleInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpSmallForceAngleInterval.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSmallForceAngleInterval.Location = new System.Drawing.Point(320, 274);
            this.SpSmallForceAngleInterval.Name = "SpSmallForceAngleInterval";
            this.SpSmallForceAngleInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSmallForceAngleInterval.Properties.MaxValue = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.SpSmallForceAngleInterval.Size = new System.Drawing.Size(139, 24);
            this.SpSmallForceAngleInterval.TabIndex = 14;
            // 
            // SpLargeForceCaliInterval
            // 
            this.SpLargeForceCaliInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpLargeForceCaliInterval.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLargeForceCaliInterval.Location = new System.Drawing.Point(320, 213);
            this.SpLargeForceCaliInterval.Name = "SpLargeForceCaliInterval";
            this.SpLargeForceCaliInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLargeForceCaliInterval.Properties.MaxValue = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.SpLargeForceCaliInterval.Size = new System.Drawing.Size(139, 24);
            this.SpLargeForceCaliInterval.TabIndex = 13;
            // 
            // SpSmallForceCaliInterval
            // 
            this.SpSmallForceCaliInterval.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SpSmallForceCaliInterval.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSmallForceCaliInterval.Location = new System.Drawing.Point(320, 154);
            this.SpSmallForceCaliInterval.Name = "SpSmallForceCaliInterval";
            this.SpSmallForceCaliInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSmallForceCaliInterval.Properties.MaxValue = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.SpSmallForceCaliInterval.Size = new System.Drawing.Size(139, 24);
            this.SpSmallForceCaliInterval.TabIndex = 12;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(126, 460);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(120, 18);
            this.labelControl9.TabIndex = 11;
            this.labelControl9.Text = "大力标定初始增量";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(107, 338);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(150, 18);
            this.labelControl8.TabIndex = 10;
            this.labelControl8.Text = "大力转多少角度标一次";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(126, 399);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(120, 18);
            this.labelControl6.TabIndex = 9;
            this.labelControl6.Text = "小力标定初始增量";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(107, 276);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(150, 18);
            this.labelControl7.TabIndex = 8;
            this.labelControl7.Text = "小力转多少角度标一次";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(147, 216);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(90, 18);
            this.labelControl5.TabIndex = 7;
            this.labelControl5.Text = "大力标定间隔";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(147, 156);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(90, 18);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "小力标定间隔";
            // 
            // BtnExport
            // 
            this.BtnExport.Location = new System.Drawing.Point(255, 577);
            this.BtnExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(166, 46);
            this.BtnExport.TabIndex = 4;
            this.BtnExport.Text = "数据导出";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(167, 94);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "标定模式";
            // 
            // CmbCaliMode
            // 
            this.CmbCaliMode.FormattingEnabled = true;
            this.CmbCaliMode.Items.AddRange(new object[] {
            "单角度",
            "多角度"});
            this.CmbCaliMode.Location = new System.Drawing.Point(320, 93);
            this.CmbCaliMode.Name = "CmbCaliMode";
            this.CmbCaliMode.Size = new System.Drawing.Size(140, 26);
            this.CmbCaliMode.TabIndex = 2;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(167, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "标定范围";
            // 
            // CmbForceRange
            // 
            this.CmbForceRange.FormattingEnabled = true;
            this.CmbForceRange.Items.AddRange(new object[] {
            "大力",
            "小力",
            "大力和小力"});
            this.CmbForceRange.Location = new System.Drawing.Point(320, 30);
            this.CmbForceRange.Name = "CmbForceRange";
            this.CmbForceRange.Size = new System.Drawing.Size(140, 26);
            this.CmbForceRange.TabIndex = 0;
            // 
            // BtnStop
            // 
            this.BtnStop.Location = new System.Drawing.Point(479, 690);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(166, 46);
            this.BtnStop.TabIndex = 3;
            this.BtnStop.Text = "停止";
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnCaliAndTest
            // 
            this.BtnCaliAndTest.Location = new System.Drawing.Point(262, 690);
            this.BtnCaliAndTest.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnCaliAndTest.Name = "BtnCaliAndTest";
            this.BtnCaliAndTest.Size = new System.Drawing.Size(166, 46);
            this.BtnCaliAndTest.TabIndex = 4;
            this.BtnCaliAndTest.Text = "开始标定并检验";
            this.BtnCaliAndTest.Click += new System.EventHandler(this.BtnCaliAndTest_Click);
            // 
            // FrmCalibrationRelationship
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 760);
            this.Controls.Add(this.BtnCaliAndTest);
            this.Controls.Add(this.BtnStop);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.BtnStart);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmCalibrationRelationship";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "力控标定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCalibrationRelationship_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceKeepDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceInitialIncrement.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceInitialIncrement.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceAngleInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceAngleInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLargeForceCaliInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSmallForceCaliInterval.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ComboBox CmbForceRange;
        private DevExpress.XtraEditors.SimpleButton BtnStop;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnExport;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ComboBox CmbCaliMode;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit SpLargeForceCaliInterval;
        private DevExpress.XtraEditors.SpinEdit SpSmallForceCaliInterval;
        private DevExpress.XtraEditors.SpinEdit SpLargeForceInitialIncrement;
        private DevExpress.XtraEditors.SpinEdit SpSmallForceInitialIncrement;
        private DevExpress.XtraEditors.SpinEdit SpLargeForceAngleInterval;
        private DevExpress.XtraEditors.SpinEdit SpSmallForceAngleInterval;
        private DevExpress.XtraEditors.SpinEdit SpForceKeepDelay;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl LbTip;
        private DevExpress.XtraEditors.SimpleButton BtnCaliAndTest;
    }
}