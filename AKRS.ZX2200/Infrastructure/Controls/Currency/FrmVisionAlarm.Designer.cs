namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using VMControls.Winform.Release;

    partial class FrmVisionAlarm
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtOK = new DevExpress.XtraEditors.SimpleButton();
            this.BtAbort = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtEditProgram = new DevExpress.XtraEditors.SimpleButton();
            this.BtSkip = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.BtReDispense = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Location = new System.Drawing.Point(4, 546);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(520, 226);
            this.panelControl1.TabIndex = 1;
            // 
            // BtOK
            // 
            this.BtOK.Location = new System.Drawing.Point(4, 777);
            this.BtOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(100, 40);
            this.BtOK.TabIndex = 3;
            this.BtOK.Text = "确认完成";
            this.BtOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // BtAbort
            // 
            this.BtAbort.Location = new System.Drawing.Point(424, 777);
            this.BtAbort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtAbort.Name = "BtAbort";
            this.BtAbort.Size = new System.Drawing.Size(100, 40);
            this.BtAbort.TabIndex = 4;
            this.BtAbort.Text = "停止";
            this.BtAbort.Click += new System.EventHandler(this.BtAbort_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.labelControl1);
            this.panelControl2.Location = new System.Drawing.Point(4, 442);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(520, 100);
            this.panelControl2.TabIndex = 5;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(4, 4);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(70, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "labelControl1";
            // 
            // BtEditProgram
            // 
            this.BtEditProgram.Location = new System.Drawing.Point(109, 777);
            this.BtEditProgram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtEditProgram.Name = "BtEditProgram";
            this.BtEditProgram.Size = new System.Drawing.Size(100, 40);
            this.BtEditProgram.TabIndex = 6;
            this.BtEditProgram.Text = "编辑模板";
            this.BtEditProgram.Click += new System.EventHandler(this.BtEditProgram_Click);
            // 
            // BtSkip
            // 
            this.BtSkip.Location = new System.Drawing.Point(214, 777);
            this.BtSkip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtSkip.Name = "BtSkip";
            this.BtSkip.Size = new System.Drawing.Size(100, 40);
            this.BtSkip.TabIndex = 7;
            this.BtSkip.Text = "跳过";
            this.BtSkip.Click += new System.EventHandler(this.BtSkip_Click);
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.pictureEdit1);
            this.panelControl3.Location = new System.Drawing.Point(4, 12);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(520, 425);
            this.panelControl3.TabIndex = 8;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.Location = new System.Drawing.Point(2, 2);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEdit1.Size = new System.Drawing.Size(516, 421);
            this.pictureEdit1.TabIndex = 3;
            // 
            // BtReDispense
            // 
            this.BtReDispense.Location = new System.Drawing.Point(319, 777);
            this.BtReDispense.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtReDispense.Name = "BtReDispense";
            this.BtReDispense.Size = new System.Drawing.Size(100, 40);
            this.BtReDispense.TabIndex = 9;
            this.BtReDispense.Text = "补胶";
            this.BtReDispense.Visible = false;
            this.BtReDispense.Click += new System.EventHandler(this.BtReDispense_Click);
            // 
            // FrmVisionAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 826);
            this.Controls.Add(this.BtReDispense);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.BtSkip);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.BtEditProgram);
            this.Controls.Add(this.BtAbort);
            this.Controls.Add(this.BtOK);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmVisionAlarm";
            this.Text = "FrmVisionAlarm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVisionAlarm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmVisionAlarm_FormClosed);
            this.Load += new System.EventHandler(this.FrmVisionAlarm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtOK;
        private DevExpress.XtraEditors.SimpleButton BtAbort;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtEditProgram;
        private DevExpress.XtraEditors.SimpleButton BtSkip;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton BtReDispense;
    }
}