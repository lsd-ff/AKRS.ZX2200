namespace AKRS.ZX2200.Controls.ToolControls.Programming
{
    partial class UcTransportUnitFrame
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
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.TpTransportUnit = new DevExpress.XtraTab.XtraTabPage();
            this.TpSubstrate = new DevExpress.XtraTab.XtraTabPage();
            this.TpModule = new DevExpress.XtraTab.XtraTabPage();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.TpTransportUnit;
            this.xtraTabControl1.Size = new System.Drawing.Size(1311, 982);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TpTransportUnit,
            this.TpSubstrate,
            this.TpModule});
            // 
            // TpTransportUnit
            // 
            this.TpTransportUnit.Appearance.Header.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TpTransportUnit.Appearance.Header.Options.UseFont = true;
            this.TpTransportUnit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpTransportUnit.Name = "TpTransportUnit";
            this.TpTransportUnit.Size = new System.Drawing.Size(1309, 945);
            this.TpTransportUnit.Text = "载具";
            // 
            // TpSubstrate
            // 
            this.TpSubstrate.Appearance.Header.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TpSubstrate.Appearance.Header.Options.UseFont = true;
            this.TpSubstrate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpSubstrate.Name = "TpSubstrate";
            this.TpSubstrate.Size = new System.Drawing.Size(1309, 945);
            this.TpSubstrate.Text = "基板";
            // 
            // TpModule
            // 
            this.TpModule.Appearance.Header.Font = new System.Drawing.Font("Tahoma", 11F);
            this.TpModule.Appearance.Header.Options.UseFont = true;
            this.TpModule.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TpModule.Name = "TpModule";
            this.TpModule.Size = new System.Drawing.Size(1309, 945);
            this.TpModule.Text = "基岛";
            // 
            // UcTransportUnitFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcTransportUnitFrame";
            this.Size = new System.Drawing.Size(1311, 982);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage TpTransportUnit;
        private DevExpress.XtraTab.XtraTabPage TpSubstrate;
        private DevExpress.XtraTab.XtraTabPage TpModule;
    }
}
