namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    partial class RealTimeImageForm
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "FrmRealTimeImage";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // RealTimeImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(558, 562);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "RealTimeImageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "相机实时界面";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RealTimeImageForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.RealTimeImageForm_FormClosed);
            this.Load += new System.EventHandler(this.RealTimeImageForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
    }
}