namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class FrmToolsTestRun
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
            this.BtnPPToolsTestRun = new DevExpress.XtraEditors.SimpleButton();
            this.BtnESToolsTestRun = new DevExpress.XtraEditors.SimpleButton();
            this.LbPPTool = new DevExpress.XtraEditors.LabelControl();
            this.LbESTool = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // BtnPPToolsTestRun
            // 
            this.BtnPPToolsTestRun.Location = new System.Drawing.Point(176, 34);
            this.BtnPPToolsTestRun.Name = "BtnPPToolsTestRun";
            this.BtnPPToolsTestRun.Size = new System.Drawing.Size(213, 39);
            this.BtnPPToolsTestRun.TabIndex = 0;
            this.BtnPPToolsTestRun.Text = "开始重复换吸嘴测试";
            this.BtnPPToolsTestRun.Click += new System.EventHandler(this.BtnToolsTestRun_Click);
            // 
            // BtnESToolsTestRun
            // 
            this.BtnESToolsTestRun.Location = new System.Drawing.Point(176, 113);
            this.BtnESToolsTestRun.Name = "BtnESToolsTestRun";
            this.BtnESToolsTestRun.Size = new System.Drawing.Size(213, 39);
            this.BtnESToolsTestRun.TabIndex = 1;
            this.BtnESToolsTestRun.Text = "开始重复换顶针测试";
            this.BtnESToolsTestRun.Click += new System.EventHandler(this.BtnESToolsTestRun_Click);
            // 
            // LbPPTool
            // 
            this.LbPPTool.Location = new System.Drawing.Point(41, 45);
            this.LbPPTool.Name = "LbPPTool";
            this.LbPPTool.Size = new System.Drawing.Size(30, 18);
            this.LbPPTool.TabIndex = 2;
            this.LbPPTool.Text = "吸嘴";
            // 
            // LbESTool
            // 
            this.LbESTool.Location = new System.Drawing.Point(41, 124);
            this.LbESTool.Name = "LbESTool";
            this.LbESTool.Size = new System.Drawing.Size(30, 18);
            this.LbESTool.TabIndex = 3;
            this.LbESTool.Text = "顶针";
            // 
            // FrmToolsTestRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 195);
            this.Controls.Add(this.LbESTool);
            this.Controls.Add(this.LbPPTool);
            this.Controls.Add(this.BtnESToolsTestRun);
            this.Controls.Add(this.BtnPPToolsTestRun);
            this.Name = "FrmToolsTestRun";
            this.Text = "工具测试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmToolsTestRun_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmToolsTestRun_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton BtnPPToolsTestRun;
        private DevExpress.XtraEditors.SimpleButton BtnESToolsTestRun;
        private DevExpress.XtraEditors.LabelControl LbPPTool;
        private DevExpress.XtraEditors.LabelControl LbESTool;
    }
}