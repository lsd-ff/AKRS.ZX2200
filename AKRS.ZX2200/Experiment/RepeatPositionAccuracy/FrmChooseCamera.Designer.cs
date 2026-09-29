namespace AKRS.ZX2200.ExperimentSystem.RepeatPositionAccuracy
{
    partial class FrmChooseCamera
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
            this.BtnChooseBondCamera = new DevExpress.XtraEditors.SimpleButton();
            this.BtnChooseUpLookCamera = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // BtnChooseBondCamera
            // 
            this.BtnChooseBondCamera.Location = new System.Drawing.Point(49, 53);
            this.BtnChooseBondCamera.Name = "BtnChooseBondCamera";
            this.BtnChooseBondCamera.Size = new System.Drawing.Size(127, 40);
            this.BtnChooseBondCamera.TabIndex = 0;
            this.BtnChooseBondCamera.Text = "BondCamera";
            this.BtnChooseBondCamera.Click += new System.EventHandler(this.BtnChooseBondCamera_Click);
            // 
            // BtnChooseUpLookCamera
            // 
            this.BtnChooseUpLookCamera.Location = new System.Drawing.Point(49, 139);
            this.BtnChooseUpLookCamera.Name = "BtnChooseUpLookCamera";
            this.BtnChooseUpLookCamera.Size = new System.Drawing.Size(127, 40);
            this.BtnChooseUpLookCamera.TabIndex = 1;
            this.BtnChooseUpLookCamera.Text = "UpLookCamera";
            this.BtnChooseUpLookCamera.Click += new System.EventHandler(this.BtnChooseUpLookCamera_Click);
            // 
            // FrmChooseCamera
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 270);
            this.Controls.Add(this.BtnChooseUpLookCamera);
            this.Controls.Add(this.BtnChooseBondCamera);
            this.Name = "FrmChooseCamera";
            this.Text = "FrmChooseCamera";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnChooseBondCamera;
        private DevExpress.XtraEditors.SimpleButton BtnChooseUpLookCamera;
    }
}