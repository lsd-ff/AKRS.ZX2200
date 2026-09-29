namespace AKRS.Galaxy2.PR.Controls
{
    partial class UcVmResultShow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcVmResultShow));
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.ChkRealTimeRefresh = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkRealTimeRefresh.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtBack
            // 
            this.BtBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtBack.ImageOptions.SvgImage")));
            this.BtBack.Location = new System.Drawing.Point(3, 157);
            this.BtBack.Name = "BtBack";
            this.BtBack.Size = new System.Drawing.Size(43, 23);
            this.BtBack.TabIndex = 0;
            this.BtBack.Visible = false;
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtNext
            // 
            this.BtNext.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtNext.ImageOptions.SvgImage")));
            this.BtNext.Location = new System.Drawing.Point(545, 157);
            this.BtNext.Name = "BtNext";
            this.BtNext.Size = new System.Drawing.Size(43, 23);
            this.BtNext.TabIndex = 1;
            this.BtNext.Visible = false;
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // ChkRealTimeRefresh
            // 
            this.ChkRealTimeRefresh.EditValue = true;
            this.ChkRealTimeRefresh.Location = new System.Drawing.Point(3, 3);
            this.ChkRealTimeRefresh.Name = "ChkRealTimeRefresh";
            this.ChkRealTimeRefresh.Properties.Caption = "实时刷新";
            this.ChkRealTimeRefresh.Size = new System.Drawing.Size(75, 20);
            this.ChkRealTimeRefresh.TabIndex = 2;
            this.ChkRealTimeRefresh.CheckedChanged += new System.EventHandler(this.ChkRealTimeRefresh_CheckedChanged);
            // 
            // UcVmResultShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChkRealTimeRefresh);
            this.Controls.Add(this.BtNext);
            this.Controls.Add(this.BtBack);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcVmResultShow";
            this.Size = new System.Drawing.Size(591, 369);
            ((System.ComponentModel.ISupportInitialize)(this.ChkRealTimeRefresh.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.CheckEdit ChkRealTimeRefresh;
    }
}
