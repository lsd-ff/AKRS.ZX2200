namespace AKRS.ZX2200.Experiment.RepeatPositionAccuracy
{
    partial class FrmRepeatPositionAccuracy
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.ChkUplook = new DevExpress.XtraEditors.CheckEdit();
            this.ChkBMC = new DevExpress.XtraEditors.CheckEdit();
            this.SpSpeed = new DevExpress.XtraEditors.SpinEdit();
            this.SpAcc = new DevExpress.XtraEditors.SpinEdit();
            this.SpJerk = new DevExpress.XtraEditors.SpinEdit();
            this.SpTimes = new DevExpress.XtraEditors.SpinEdit();
            this.SpDelay = new DevExpress.XtraEditors.SpinEdit();
            this.ChkTemp = new DevExpress.XtraEditors.CheckEdit();
            this.button1 = new System.Windows.Forms.Button();
            this.BtPickCalibPlate = new DevExpress.XtraEditors.SimpleButton();
            this.BtPutbackCalibPlate = new DevExpress.XtraEditors.SimpleButton();
            this.BtStartTask = new DevExpress.XtraEditors.SimpleButton();
            this.BtStartBMCTask = new DevExpress.XtraEditors.SimpleButton();
            this.BtTestMulLight = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStartShakeTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStopShakeTest = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChkUplook.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBMC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSpeed.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAcc.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJerk.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTemp.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(129, 225);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Times";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(67, 268);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(94, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "After Move Delay";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(366, 271);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(15, 14);
            this.labelControl3.TabIndex = 4;
            this.labelControl3.Text = "ms";
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(132, 344);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(129, 32);
            this.BtnStart.TabIndex = 6;
            this.BtnStart.Text = "Start";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(129, 100);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(35, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "Speed";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(132, 144);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(20, 14);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "Acc";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(132, 189);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(23, 14);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "Jerk";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(366, 103);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(30, 14);
            this.labelControl7.TabIndex = 13;
            this.labelControl7.Text = "mm/s";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(366, 147);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(46, 14);
            this.labelControl8.TabIndex = 14;
            this.labelControl8.Text = "mm/s^2";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(366, 192);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(15, 14);
            this.labelControl9.TabIndex = 15;
            this.labelControl9.Text = "ms";
            // 
            // ChkUplook
            // 
            this.ChkUplook.Location = new System.Drawing.Point(129, 37);
            this.ChkUplook.Name = "ChkUplook";
            this.ChkUplook.Properties.Caption = "Uplook";
            this.ChkUplook.Size = new System.Drawing.Size(75, 20);
            this.ChkUplook.TabIndex = 16;
            // 
            // ChkBMC
            // 
            this.ChkBMC.Location = new System.Drawing.Point(228, 37);
            this.ChkBMC.Name = "ChkBMC";
            this.ChkBMC.Properties.Caption = "BMC";
            this.ChkBMC.Size = new System.Drawing.Size(75, 20);
            this.ChkBMC.TabIndex = 17;
            // 
            // SpSpeed
            // 
            this.SpSpeed.EditValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpSpeed.Location = new System.Drawing.Point(188, 97);
            this.SpSpeed.Name = "SpSpeed";
            this.SpSpeed.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSpeed.Properties.IsFloatValue = false;
            this.SpSpeed.Properties.MaskSettings.Set("mask", "N00");
            this.SpSpeed.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.SpSpeed.Properties.MinValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpSpeed.Size = new System.Drawing.Size(172, 20);
            this.SpSpeed.TabIndex = 18;
            // 
            // SpAcc
            // 
            this.SpAcc.EditValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.SpAcc.Location = new System.Drawing.Point(188, 141);
            this.SpAcc.Name = "SpAcc";
            this.SpAcc.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAcc.Properties.IsFloatValue = false;
            this.SpAcc.Properties.MaskSettings.Set("mask", "N00");
            this.SpAcc.Properties.MaxValue = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.SpAcc.Properties.MinValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpAcc.Size = new System.Drawing.Size(172, 20);
            this.SpAcc.TabIndex = 19;
            // 
            // SpJerk
            // 
            this.SpJerk.EditValue = new decimal(new int[] {
            6,
            0,
            0,
            131072});
            this.SpJerk.Location = new System.Drawing.Point(188, 186);
            this.SpJerk.Name = "SpJerk";
            this.SpJerk.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpJerk.Properties.MaxValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpJerk.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            131072});
            this.SpJerk.Size = new System.Drawing.Size(172, 20);
            this.SpJerk.TabIndex = 20;
            // 
            // SpTimes
            // 
            this.SpTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Location = new System.Drawing.Point(188, 222);
            this.SpTimes.Name = "SpTimes";
            this.SpTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpTimes.Properties.IsFloatValue = false;
            this.SpTimes.Properties.MaskSettings.Set("mask", "N00");
            this.SpTimes.Properties.MaxValue = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.SpTimes.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpTimes.Size = new System.Drawing.Size(172, 20);
            this.SpTimes.TabIndex = 21;
            // 
            // SpDelay
            // 
            this.SpDelay.EditValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpDelay.Location = new System.Drawing.Point(188, 265);
            this.SpDelay.Name = "SpDelay";
            this.SpDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDelay.Properties.IsFloatValue = false;
            this.SpDelay.Properties.MaskSettings.Set("mask", "N00");
            this.SpDelay.Properties.MaxValue = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.SpDelay.Properties.MinValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpDelay.Size = new System.Drawing.Size(172, 20);
            this.SpDelay.TabIndex = 22;
            // 
            // ChkTemp
            // 
            this.ChkTemp.Location = new System.Drawing.Point(321, 37);
            this.ChkTemp.Name = "ChkTemp";
            this.ChkTemp.Properties.Caption = "Temp";
            this.ChkTemp.Size = new System.Drawing.Size(75, 20);
            this.ChkTemp.TabIndex = 23;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(321, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 32);
            this.button1.TabIndex = 24;
            this.button1.Text = "test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtPickCalibPlate
            // 
            this.BtPickCalibPlate.Location = new System.Drawing.Point(422, 207);
            this.BtPickCalibPlate.Name = "BtPickCalibPlate";
            this.BtPickCalibPlate.Size = new System.Drawing.Size(132, 32);
            this.BtPickCalibPlate.TabIndex = 25;
            this.BtPickCalibPlate.Text = "Pickup Calib plate";
            this.BtPickCalibPlate.Click += new System.EventHandler(this.BtPickCalibPlate_Click);
            // 
            // BtPutbackCalibPlate
            // 
            this.BtPutbackCalibPlate.Location = new System.Drawing.Point(422, 253);
            this.BtPutbackCalibPlate.Name = "BtPutbackCalibPlate";
            this.BtPutbackCalibPlate.Size = new System.Drawing.Size(146, 32);
            this.BtPutbackCalibPlate.TabIndex = 26;
            this.BtPutbackCalibPlate.Text = "Putback Calib Plate";
            this.BtPutbackCalibPlate.Click += new System.EventHandler(this.BtPutbackCalibPlate_Click);
            // 
            // BtStartTask
            // 
            this.BtStartTask.Location = new System.Drawing.Point(129, 409);
            this.BtStartTask.Name = "BtStartTask";
            this.BtStartTask.Size = new System.Drawing.Size(129, 32);
            this.BtStartTask.TabIndex = 27;
            this.BtStartTask.Text = "Start Task";
            this.BtStartTask.Click += new System.EventHandler(this.BtStartTask_Click);
            // 
            // BtStartBMCTask
            // 
            this.BtStartBMCTask.Location = new System.Drawing.Point(321, 409);
            this.BtStartBMCTask.Name = "BtStartBMCTask";
            this.BtStartBMCTask.Size = new System.Drawing.Size(129, 32);
            this.BtStartBMCTask.TabIndex = 28;
            this.BtStartBMCTask.Text = "Start BMC  Task";
            this.BtStartBMCTask.Click += new System.EventHandler(this.BtStartBMCTask_Click);
            // 
            // BtTestMulLight
            // 
            this.BtTestMulLight.Location = new System.Drawing.Point(487, 409);
            this.BtTestMulLight.Name = "BtTestMulLight";
            this.BtTestMulLight.Size = new System.Drawing.Size(129, 32);
            this.BtTestMulLight.TabIndex = 29;
            this.BtTestMulLight.Text = "Test Multiple Light";
            this.BtTestMulLight.Click += new System.EventHandler(this.BtTestMulLight_Click);
            // 
            // BtnStartShakeTest
            // 
            this.BtnStartShakeTest.Location = new System.Drawing.Point(129, 464);
            this.BtnStartShakeTest.Name = "BtnStartShakeTest";
            this.BtnStartShakeTest.Size = new System.Drawing.Size(129, 32);
            this.BtnStartShakeTest.TabIndex = 30;
            this.BtnStartShakeTest.Text = "Start Shake Test";
            this.BtnStartShakeTest.Click += new System.EventHandler(this.BtnStartShakeTest_Click);
            // 
            // BtnStopShakeTest
            // 
            this.BtnStopShakeTest.Location = new System.Drawing.Point(321, 464);
            this.BtnStopShakeTest.Name = "BtnStopShakeTest";
            this.BtnStopShakeTest.Size = new System.Drawing.Size(129, 32);
            this.BtnStopShakeTest.TabIndex = 31;
            this.BtnStopShakeTest.Text = "Stop Shake Test";
            this.BtnStopShakeTest.Click += new System.EventHandler(this.BtnStopShakeTest_Click);
            // 
            // FrmRepeatPositionAccuracy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 538);
            this.Controls.Add(this.BtnStopShakeTest);
            this.Controls.Add(this.BtnStartShakeTest);
            this.Controls.Add(this.BtTestMulLight);
            this.Controls.Add(this.BtStartBMCTask);
            this.Controls.Add(this.BtStartTask);
            this.Controls.Add(this.BtPutbackCalibPlate);
            this.Controls.Add(this.BtPickCalibPlate);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ChkTemp);
            this.Controls.Add(this.SpDelay);
            this.Controls.Add(this.SpTimes);
            this.Controls.Add(this.SpJerk);
            this.Controls.Add(this.SpAcc);
            this.Controls.Add(this.SpSpeed);
            this.Controls.Add(this.ChkBMC);
            this.Controls.Add(this.ChkUplook);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Name = "FrmRepeatPositionAccuracy";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Repeat Position Accuracy";
            ((System.ComponentModel.ISupportInitialize)(this.ChkUplook.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkBMC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSpeed.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAcc.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpJerk.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkTemp.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.CheckEdit ChkUplook;
        private DevExpress.XtraEditors.CheckEdit ChkBMC;
        private DevExpress.XtraEditors.SpinEdit SpSpeed;
        private DevExpress.XtraEditors.SpinEdit SpAcc;
        private DevExpress.XtraEditors.SpinEdit SpJerk;
        private DevExpress.XtraEditors.SpinEdit SpTimes;
        private DevExpress.XtraEditors.SpinEdit SpDelay;
        private DevExpress.XtraEditors.CheckEdit ChkTemp;
        private System.Windows.Forms.Button button1;
        private DevExpress.XtraEditors.SimpleButton BtPickCalibPlate;
        private DevExpress.XtraEditors.SimpleButton BtPutbackCalibPlate;
        private DevExpress.XtraEditors.SimpleButton BtStartTask;
        private DevExpress.XtraEditors.SimpleButton BtStartBMCTask;
        private DevExpress.XtraEditors.SimpleButton BtTestMulLight;
        private DevExpress.XtraEditors.SimpleButton BtnStartShakeTest;
        private DevExpress.XtraEditors.SimpleButton BtnStopShakeTest;
    }
}