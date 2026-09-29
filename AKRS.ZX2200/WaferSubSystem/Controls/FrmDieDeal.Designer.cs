namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    partial class FrmDieDeal
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
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            this.BtnSkip = new DevExpress.XtraEditors.SimpleButton();
            this.LbString = new DevExpress.XtraEditors.LabelControl();
            this.BtnRetry = new DevExpress.XtraEditors.SimpleButton();
            this.BtnChange = new DevExpress.XtraEditors.SimpleButton();
            this.BtnAbort = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).BeginInit();
            this.SuspendLayout();
            // 
            // PnlControl
            // 
            this.PnlControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PnlControl.Location = new System.Drawing.Point(0, 51);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(748, 495);
            this.PnlControl.TabIndex = 0;
            // 
            // BtnSkip
            // 
            this.BtnSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnSkip.Location = new System.Drawing.Point(28, 756);
            this.BtnSkip.Name = "BtnSkip";
            this.BtnSkip.Size = new System.Drawing.Size(122, 42);
            this.BtnSkip.TabIndex = 1;
            this.BtnSkip.Text = "Skip";
            this.BtnSkip.Click += new System.EventHandler(this.BtnSkip_Click);
            // 
            // LbString
            // 
            this.LbString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LbString.Location = new System.Drawing.Point(28, 585);
            this.LbString.Name = "LbString";
            this.LbString.Size = new System.Drawing.Size(70, 14);
            this.LbString.TabIndex = 2;
            this.LbString.Text = "labelControl1";
            // 
            // BtnRetry
            // 
            this.BtnRetry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnRetry.Location = new System.Drawing.Point(188, 756);
            this.BtnRetry.Name = "BtnRetry";
            this.BtnRetry.Size = new System.Drawing.Size(122, 42);
            this.BtnRetry.TabIndex = 3;
            this.BtnRetry.Text = "Retry";
            this.BtnRetry.Click += new System.EventHandler(this.BtnRetry_Click);
            // 
            // BtnChange
            // 
            this.BtnChange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnChange.Location = new System.Drawing.Point(348, 756);
            this.BtnChange.Name = "BtnChange";
            this.BtnChange.Size = new System.Drawing.Size(122, 42);
            this.BtnChange.TabIndex = 4;
            this.BtnChange.Text = "Change";
            this.BtnChange.Click += new System.EventHandler(this.BtnChange_Click);
            // 
            // BtnAbort
            // 
            this.BtnAbort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BtnAbort.Location = new System.Drawing.Point(508, 756);
            this.BtnAbort.Name = "BtnAbort";
            this.BtnAbort.Size = new System.Drawing.Size(122, 42);
            this.BtnAbort.TabIndex = 5;
            this.BtnAbort.Text = "Abort";
            this.BtnAbort.Click += new System.EventHandler(this.BtnAbort_Click);
            // 
            // FrmDieDeal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 842);
            this.Controls.Add(this.BtnAbort);
            this.Controls.Add(this.BtnChange);
            this.Controls.Add(this.BtnRetry);
            this.Controls.Add(this.LbString);
            this.Controls.Add(this.BtnSkip);
            this.Controls.Add(this.PnlControl);
            this.Name = "FrmDieDeal";
            this.Text = "FrmDieDeal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmDieDeal_FormClosing);
            this.Load += new System.EventHandler(this.FrmDieDeal_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PnlControl;
        private DevExpress.XtraEditors.SimpleButton BtnSkip;
        private DevExpress.XtraEditors.LabelControl LbString;
        private DevExpress.XtraEditors.SimpleButton BtnRetry;
        private DevExpress.XtraEditors.SimpleButton BtnChange;
        private DevExpress.XtraEditors.SimpleButton BtnAbort;
    }
}