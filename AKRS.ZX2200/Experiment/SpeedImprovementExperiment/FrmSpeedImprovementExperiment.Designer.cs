namespace AKRS.ZX2200.Experiment.SpeedImprovementExperiment
{
    partial class FrmSpeedImprovementExperiment
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
            this.BtPickUp = new DevExpress.XtraEditors.SimpleButton();
            this.BtMoveToUpLook = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // BtPickUp
            // 
            this.BtPickUp.Location = new System.Drawing.Point(44, 36);
            this.BtPickUp.Name = "BtPickUp";
            this.BtPickUp.Size = new System.Drawing.Size(125, 75);
            this.BtPickUp.TabIndex = 0;
            this.BtPickUp.Text = "取片";
            this.BtPickUp.Click += new System.EventHandler(this.BtPickUp_Click);
            // 
            // BtMoveToUpLook
            // 
            this.BtMoveToUpLook.Location = new System.Drawing.Point(229, 36);
            this.BtMoveToUpLook.Name = "BtMoveToUpLook";
            this.BtMoveToUpLook.Size = new System.Drawing.Size(125, 75);
            this.BtMoveToUpLook.TabIndex = 1;
            this.BtMoveToUpLook.Text = "去上视";
            this.BtMoveToUpLook.Click += new System.EventHandler(this.BtMoveToUpLook_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(418, 36);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(125, 75);
            this.simpleButton2.TabIndex = 2;
            this.simpleButton2.Text = "去中转台";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(593, 36);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(125, 75);
            this.simpleButton3.TabIndex = 3;
            this.simpleButton3.Text = "贴片";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(44, 193);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(125, 75);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "测试";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(229, 193);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(125, 75);
            this.simpleButton4.TabIndex = 5;
            this.simpleButton4.Text = "测试";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(44, 502);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.Size = new System.Drawing.Size(125, 75);
            this.simpleButton5.TabIndex = 6;
            this.simpleButton5.Text = "门型运动";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(229, 502);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(125, 75);
            this.simpleButton6.TabIndex = 7;
            this.simpleButton6.Text = "飞拍";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(418, 502);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.Size = new System.Drawing.Size(125, 75);
            this.simpleButton7.TabIndex = 8;
            this.simpleButton7.Text = "移动时发送指令";
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // FrmSpeedImprovementExperiment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 669);
            this.Controls.Add(this.simpleButton7);
            this.Controls.Add(this.simpleButton6);
            this.Controls.Add(this.simpleButton5);
            this.Controls.Add(this.simpleButton4);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.BtMoveToUpLook);
            this.Controls.Add(this.BtPickUp);
            this.Name = "FrmSpeedImprovementExperiment";
            this.Text = "速度提升实验";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtPickUp;
        private DevExpress.XtraEditors.SimpleButton BtMoveToUpLook;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
    }
}