namespace AKRS.ZX2200.Experiment.BondAccuracyExperiment
{
    partial class FrmAlignAngleAccuracyTest
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
            this.SpCycles = new DevExpress.XtraEditors.SpinEdit();
            this.spPrInterval = new DevExpress.XtraEditors.SpinEdit();
            this.SpVisionTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lbPrTimes = new DevExpress.XtraEditors.LabelControl();
            this.BtnTest1Start = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTestStop = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTest2Start = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.LbNum = new DevExpress.XtraEditors.LabelControl();
            this.ChkMoveToCameraCenterAfterVision = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SpCycles
            // 
            this.SpCycles.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.SpCycles.Location = new System.Drawing.Point(188, 144);
            this.SpCycles.Name = "SpCycles";
            this.SpCycles.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpCycles.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SpCycles.Size = new System.Drawing.Size(100, 20);
            this.SpCycles.TabIndex = 81;
            // 
            // spPrInterval
            // 
            this.spPrInterval.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spPrInterval.Location = new System.Drawing.Point(188, 110);
            this.spPrInterval.Name = "spPrInterval";
            this.spPrInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spPrInterval.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spPrInterval.Size = new System.Drawing.Size(100, 20);
            this.spPrInterval.TabIndex = 80;
            // 
            // SpVisionTime
            // 
            this.SpVisionTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpVisionTime.Location = new System.Drawing.Point(188, 77);
            this.SpVisionTime.Name = "SpVisionTime";
            this.SpVisionTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpVisionTime.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SpVisionTime.Size = new System.Drawing.Size(100, 20);
            this.SpVisionTime.TabIndex = 79;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(102, 147);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 78;
            this.labelControl6.Text = "循环次数:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(102, 113);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(66, 14);
            this.labelControl5.TabIndex = 77;
            this.labelControl5.Text = "PR定位延迟:";
            // 
            // lbPrTimes
            // 
            this.lbPrTimes.Location = new System.Drawing.Point(102, 80);
            this.lbPrTimes.Name = "lbPrTimes";
            this.lbPrTimes.Size = new System.Drawing.Size(66, 14);
            this.lbPrTimes.TabIndex = 76;
            this.lbPrTimes.Text = "PR重复次数:";
            // 
            // BtnTest1Start
            // 
            this.BtnTest1Start.Location = new System.Drawing.Point(153, 252);
            this.BtnTest1Start.Name = "BtnTest1Start";
            this.BtnTest1Start.Size = new System.Drawing.Size(135, 54);
            this.BtnTest1Start.TabIndex = 82;
            this.BtnTest1Start.Text = "开始1";
            this.BtnTest1Start.Click += new System.EventHandler(this.BtnTest1Start_Click);
            // 
            // BtnTestStop
            // 
            this.BtnTestStop.Location = new System.Drawing.Point(328, 308);
            this.BtnTestStop.Name = "BtnTestStop";
            this.BtnTestStop.Size = new System.Drawing.Size(135, 54);
            this.BtnTestStop.TabIndex = 83;
            this.BtnTestStop.Text = "停止";
            this.BtnTestStop.Click += new System.EventHandler(this.BtnTestStop_Click);
            // 
            // BtnTest2Start
            // 
            this.BtnTest2Start.Location = new System.Drawing.Point(153, 376);
            this.BtnTest2Start.Name = "BtnTest2Start";
            this.BtnTest2Start.Size = new System.Drawing.Size(135, 54);
            this.BtnTest2Start.TabIndex = 90;
            this.BtnTest2Start.Text = "开始2";
            this.BtnTest2Start.Click += new System.EventHandler(this.BtnTest2Start_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(328, 147);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 91;
            this.labelControl1.Text = "当前：";
            // 
            // LbNum
            // 
            this.LbNum.Location = new System.Drawing.Point(370, 147);
            this.LbNum.Name = "LbNum";
            this.LbNum.Size = new System.Drawing.Size(7, 14);
            this.LbNum.TabIndex = 92;
            this.LbNum.Text = "0";
            // 
            // ChkMoveToCameraCenterAfterVision
            // 
            this.ChkMoveToCameraCenterAfterVision.Location = new System.Drawing.Point(102, 189);
            this.ChkMoveToCameraCenterAfterVision.Name = "ChkMoveToCameraCenterAfterVision";
            this.ChkMoveToCameraCenterAfterVision.Properties.Caption = "定位后移动到相机中心";
            this.ChkMoveToCameraCenterAfterVision.Size = new System.Drawing.Size(186, 20);
            this.ChkMoveToCameraCenterAfterVision.TabIndex = 93;
            // 
            // FrmAlignAngleAccuracyTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(886, 636);
            this.Controls.Add(this.ChkMoveToCameraCenterAfterVision);
            this.Controls.Add(this.LbNum);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnTest2Start);
            this.Controls.Add(this.BtnTestStop);
            this.Controls.Add(this.BtnTest1Start);
            this.Controls.Add(this.SpCycles);
            this.Controls.Add(this.spPrInterval);
            this.Controls.Add(this.SpVisionTime);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.lbPrTimes);
            this.Name = "FrmAlignAngleAccuracyTest";
            this.Text = "FrmAlignAngleAccuracyTest";
            ((System.ComponentModel.ISupportInitialize)(this.SpCycles.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpVisionTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMoveToCameraCenterAfterVision.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SpinEdit SpCycles;
        private DevExpress.XtraEditors.SpinEdit spPrInterval;
        private DevExpress.XtraEditors.SpinEdit SpVisionTime;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl lbPrTimes;
        private DevExpress.XtraEditors.SimpleButton BtnTest1Start;
        private DevExpress.XtraEditors.SimpleButton BtnTestStop;
        private DevExpress.XtraEditors.SimpleButton BtnTest2Start;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl LbNum;
        private DevExpress.XtraEditors.CheckEdit ChkMoveToCameraCenterAfterVision;
    }
}