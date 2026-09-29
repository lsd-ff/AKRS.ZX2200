namespace AKRS.ZX2200.SupportFeature.Parameters
{
    partial class UcParamManage
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
            this.TlGroup = new DevExpress.XtraTreeList.TreeList();
            this.TlParams = new DevExpress.XtraTreeList.TreeList();
            ((System.ComponentModel.ISupportInitialize)(this.TlGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TlParams)).BeginInit();
            this.SuspendLayout();
            // 
            // TlGroup
            // 
            this.TlGroup.Location = new System.Drawing.Point(3, 3);
            this.TlGroup.Name = "TlGroup";
            this.TlGroup.Size = new System.Drawing.Size(328, 502);
            this.TlGroup.TabIndex = 0;
            // 
            // TlParams
            // 
            this.TlParams.Location = new System.Drawing.Point(337, 3);
            this.TlParams.Name = "TlParams";
            this.TlParams.Size = new System.Drawing.Size(328, 502);
            this.TlParams.TabIndex = 0;
            // 
            // UcParamManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TlParams);
            this.Controls.Add(this.TlGroup);
            this.Name = "UcParamManage";
            this.Size = new System.Drawing.Size(1115, 718);
            ((System.ComponentModel.ISupportInitialize)(this.TlGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TlParams)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList TlGroup;
        private DevExpress.XtraTreeList.TreeList TlParams;
    }
}
