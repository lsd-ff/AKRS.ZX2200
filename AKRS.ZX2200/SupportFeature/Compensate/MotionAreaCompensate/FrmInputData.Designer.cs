namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    partial class FrmInputData
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
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtInput = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(890, 599);
            this.panelControl1.TabIndex = 0;
            // 
            // BtInput
            // 
            this.BtInput.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtInput.Appearance.Options.UseFont = true;
            this.BtInput.Location = new System.Drawing.Point(715, 605);
            this.BtInput.Name = "BtInput";
            this.BtInput.Size = new System.Drawing.Size(170, 98);
            this.BtInput.TabIndex = 1;
            this.BtInput.Text = "导入";
            this.BtInput.Click += new System.EventHandler(this.BtInput_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCancel.Appearance.Options.UseFont = true;
            this.BtCancel.Location = new System.Drawing.Point(12, 605);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(170, 98);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // FrmInputData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 711);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtInput);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmInputData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据导入";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmInputData_FormClosed);
            this.Load += new System.EventHandler(this.FrmInputData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtInput;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
    }
}