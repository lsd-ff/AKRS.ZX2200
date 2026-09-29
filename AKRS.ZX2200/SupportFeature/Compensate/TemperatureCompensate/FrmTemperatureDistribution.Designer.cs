namespace AKRS.ZX2200.SupportFeature.Compensate.TemperatureCompensate
{
    partial class FrmTemperatureDistribution
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
            this.PlDraw = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PlDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // PlDraw
            // 
            this.PlDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlDraw.Location = new System.Drawing.Point(0, 0);
            this.PlDraw.Name = "PlDraw";
            this.PlDraw.Size = new System.Drawing.Size(1077, 731);
            this.PlDraw.TabIndex = 0;
            this.PlDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.PlDraw_Paint);
            // 
            // FrmTemperatureDistribution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 731);
            this.Controls.Add(this.PlDraw);
            this.Name = "FrmTemperatureDistribution";
            this.Text = "FrmTemperatureDistribution";
            this.Load += new System.EventHandler(this.FrmTemperatureDistribution_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PlDraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PlDraw;
    }
}