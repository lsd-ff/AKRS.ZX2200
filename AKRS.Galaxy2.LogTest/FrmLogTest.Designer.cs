namespace AKRS.Galaxy2.LogTest
{
    partial class FrmLogTest
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtnAnomalies = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLogs = new DevExpress.XtraEditors.SimpleButton();
            this.GclLog = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GclLog)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnAnomalies);
            this.groupControl1.Controls.Add(this.BtnLogs);
            this.groupControl1.Location = new System.Drawing.Point(12, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(682, 181);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "groupControl1";
            // 
            // BtnAnomalies
            // 
            this.BtnAnomalies.Location = new System.Drawing.Point(76, 94);
            this.BtnAnomalies.Name = "BtnAnomalies";
            this.BtnAnomalies.Size = new System.Drawing.Size(105, 32);
            this.BtnAnomalies.TabIndex = 1;
            this.BtnAnomalies.Text = "Record anomalies";
            this.BtnAnomalies.Click += new System.EventHandler(this.BtnAnomalies_Click);
            // 
            // BtnLogs
            // 
            this.BtnLogs.Location = new System.Drawing.Point(76, 43);
            this.BtnLogs.Name = "BtnLogs";
            this.BtnLogs.Size = new System.Drawing.Size(105, 30);
            this.BtnLogs.TabIndex = 0;
            this.BtnLogs.Text = "record logs";
            this.BtnLogs.Click += new System.EventHandler(this.BtnLogs_Click);
            // 
            // GclLog
            // 
            this.GclLog.Location = new System.Drawing.Point(12, 202);
            this.GclLog.Name = "GclLog";
            this.GclLog.Size = new System.Drawing.Size(682, 355);
            this.GclLog.TabIndex = 1;
            this.GclLog.Text = "Log";
            // 
            // FrmLogTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 569);
            this.Controls.Add(this.GclLog);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmLogTest";
            this.Text = "FrmLogTest";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GclLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnAnomalies;
        private DevExpress.XtraEditors.SimpleButton BtnLogs;
        private DevExpress.XtraEditors.GroupControl GclLog;
    }
}