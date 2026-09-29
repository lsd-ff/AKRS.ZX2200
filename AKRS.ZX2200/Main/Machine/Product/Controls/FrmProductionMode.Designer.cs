namespace AKRS.ZX2200.Main.Machine.Product.Controls
{
    partial class FrmProductionMode
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
            this.BtnOK = new DevExpress.XtraEditors.SimpleButton();
            this.RgProductionMode = new DevExpress.XtraEditors.RadioGroup();
            ((System.ComponentModel.ISupportInitialize)(this.RgProductionMode.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(133, 284);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(142, 32);
            this.BtnOK.TabIndex = 4;
            this.BtnOK.Text = "OK";
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // RgProductionMode
            // 
            this.RgProductionMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.RgProductionMode.Location = new System.Drawing.Point(0, 0);
            this.RgProductionMode.Name = "RgProductionMode";
            this.RgProductionMode.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "生产模式"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "空跑模式(不带载具)"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "离线模式"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "调试模式"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "单颗循环模式")});
            this.RgProductionMode.Size = new System.Drawing.Size(435, 279);
            this.RgProductionMode.TabIndex = 1;
            // 
            // FrmProductionMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 330);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.RgProductionMode);
            this.Name = "FrmProductionMode";
            this.Text = "工作模式";
            ((System.ComponentModel.ISupportInitialize)(this.RgProductionMode.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.RadioGroup RgProductionMode;
        private DevExpress.XtraEditors.SimpleButton BtnOK;
    }
}