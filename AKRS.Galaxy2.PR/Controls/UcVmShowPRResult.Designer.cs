namespace AKRS.Galaxy2.PR.Controls
{
    partial class UcVmShowPRResult
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.vmRenderControl = new VMControls.Winform.Release.VmRenderControl();
            this.SuspendLayout();
            // 
            // vmRenderControl
            // 
            this.vmRenderControl.BackColor = System.Drawing.Color.Black;
            this.vmRenderControl.CoordinateInfoVisible = true;
            this.vmRenderControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vmRenderControl.ImageSource = null;
            this.vmRenderControl.Location = new System.Drawing.Point(0, 0);
            this.vmRenderControl.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vmRenderControl.ModuleSource = null;
            this.vmRenderControl.Name = "vmRenderControl";
            this.vmRenderControl.Size = new System.Drawing.Size(364, 366);
            this.vmRenderControl.TabIndex = 0;
            // 
            // UcVmShowPRResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.vmRenderControl);
            this.Name = "UcVmShowPRResult";
            this.Size = new System.Drawing.Size(364, 366);
            this.ResumeLayout(false);

        }

        #endregion

        private VMControls.Winform.Release.VmRenderControl vmRenderControl;
    }
}
