namespace AKRS.ZX2200.CalibSystem.TestControls
{
    partial class FrmMoveLocateTest
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btnStop = new DevExpress.XtraEditors.SimpleButton();
            this.btnStartTest = new DevExpress.XtraEditors.SimpleButton();
            this.spTimes = new DevExpress.XtraEditors.SpinEdit();
            this.spPrInterval = new DevExpress.XtraEditors.SpinEdit();
            this.spPrTimes = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lbPrTimes = new DevExpress.XtraEditors.LabelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrTimes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(576, 484);
            this.panelControl1.TabIndex = 54;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btnStop);
            this.groupControl1.Controls.Add(this.btnStartTest);
            this.groupControl1.Controls.Add(this.spTimes);
            this.groupControl1.Controls.Add(this.spPrInterval);
            this.groupControl1.Controls.Add(this.spPrTimes);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.lbPrTimes);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(572, 480);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "往复定位精度测试";
            this.groupControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupControl1_Paint);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(309, 236);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(135, 46);
            this.btnStop.TabIndex = 85;
            this.btnStop.Text = "停止";
            // 
            // btnStartTest
            // 
            this.btnStartTest.Location = new System.Drawing.Point(70, 236);
            this.btnStartTest.Name = "btnStartTest";
            this.btnStartTest.Size = new System.Drawing.Size(135, 46);
            this.btnStartTest.TabIndex = 84;
            this.btnStartTest.Text = "开始测试";
            this.btnStartTest.Click += new System.EventHandler(this.btnStartTest_Click);
            // 
            // spTimes
            // 
            this.spTimes.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.spTimes.Location = new System.Drawing.Point(247, 131);
            this.spTimes.Name = "spTimes";
            this.spTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spTimes.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spTimes.Size = new System.Drawing.Size(100, 20);
            this.spTimes.TabIndex = 75;
            // 
            // spPrInterval
            // 
            this.spPrInterval.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.spPrInterval.Location = new System.Drawing.Point(247, 97);
            this.spPrInterval.Name = "spPrInterval";
            this.spPrInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spPrInterval.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spPrInterval.Size = new System.Drawing.Size(100, 20);
            this.spPrInterval.TabIndex = 74;
            // 
            // spPrTimes
            // 
            this.spPrTimes.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spPrTimes.Location = new System.Drawing.Point(247, 64);
            this.spPrTimes.Name = "spPrTimes";
            this.spPrTimes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spPrTimes.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spPrTimes.Size = new System.Drawing.Size(100, 20);
            this.spPrTimes.TabIndex = 73;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(161, 134);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(52, 14);
            this.labelControl6.TabIndex = 72;
            this.labelControl6.Text = "循环次数:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(161, 100);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(66, 14);
            this.labelControl5.TabIndex = 71;
            this.labelControl5.Text = "PR定位延迟:";
            // 
            // lbPrTimes
            // 
            this.lbPrTimes.Location = new System.Drawing.Point(161, 67);
            this.lbPrTimes.Name = "lbPrTimes";
            this.lbPrTimes.Size = new System.Drawing.Size(66, 14);
            this.lbPrTimes.TabIndex = 70;
            this.lbPrTimes.Text = "PR重复次数:";
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 484);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(576, 0);
            this.panelControl2.TabIndex = 55;
            // 
            // FrmMoveLocateTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 333);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmMoveLocateTest";
            this.Text = "FrmCalibTest";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPrTimes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnStop;
        private DevExpress.XtraEditors.SimpleButton btnStartTest;
        private DevExpress.XtraEditors.SpinEdit spTimes;
        private DevExpress.XtraEditors.SpinEdit spPrInterval;
        private DevExpress.XtraEditors.SpinEdit spPrTimes;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl lbPrTimes;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}