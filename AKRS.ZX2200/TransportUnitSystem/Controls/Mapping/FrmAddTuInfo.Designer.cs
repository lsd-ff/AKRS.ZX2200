namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    partial class FrmAddTuInfo
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.TxRecipeName = new DevExpress.XtraEditors.TextEdit();
            this.TxSubstrateLotNumber = new DevExpress.XtraEditors.TextEdit();
            this.TxSubstrateNumber = new DevExpress.XtraEditors.TextEdit();
            this.TxFaceType = new DevExpress.XtraEditors.TextEdit();
            this.BtSure = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TxRecipeName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxSubstrateLotNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxSubstrateNumber.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxFaceType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(57, 44);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 24);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "程式名";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(57, 111);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(100, 24);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "基板批次号";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Location = new System.Drawing.Point(57, 175);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 24);
            this.labelControl3.TabIndex = 2;
            this.labelControl3.Text = "基板号";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Location = new System.Drawing.Point(57, 238);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(40, 24);
            this.labelControl4.TabIndex = 3;
            this.labelControl4.Text = "面别";
            // 
            // TxRecipeName
            // 
            this.TxRecipeName.Location = new System.Drawing.Point(201, 48);
            this.TxRecipeName.Name = "TxRecipeName";
            this.TxRecipeName.Size = new System.Drawing.Size(136, 20);
            this.TxRecipeName.TabIndex = 4;
            // 
            // TxSubstrateLotNumber
            // 
            this.TxSubstrateLotNumber.Location = new System.Drawing.Point(201, 115);
            this.TxSubstrateLotNumber.Name = "TxSubstrateLotNumber";
            this.TxSubstrateLotNumber.Size = new System.Drawing.Size(136, 20);
            this.TxSubstrateLotNumber.TabIndex = 5;
            // 
            // TxSubstrateNumber
            // 
            this.TxSubstrateNumber.Location = new System.Drawing.Point(201, 179);
            this.TxSubstrateNumber.Name = "TxSubstrateNumber";
            this.TxSubstrateNumber.Size = new System.Drawing.Size(136, 20);
            this.TxSubstrateNumber.TabIndex = 6;
            // 
            // TxFaceType
            // 
            this.TxFaceType.Location = new System.Drawing.Point(201, 243);
            this.TxFaceType.Name = "TxFaceType";
            this.TxFaceType.Size = new System.Drawing.Size(136, 20);
            this.TxFaceType.TabIndex = 7;
            // 
            // BtSure
            // 
            this.BtSure.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtSure.Appearance.Options.UseFont = true;
            this.BtSure.Location = new System.Drawing.Point(57, 313);
            this.BtSure.Name = "BtSure";
            this.BtSure.Size = new System.Drawing.Size(280, 66);
            this.BtSure.TabIndex = 8;
            this.BtSure.Text = "确定";
            this.BtSure.Click += new System.EventHandler(this.BtSure_Click);
            // 
            // FrmAddTuInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 449);
            this.Controls.Add(this.BtSure);
            this.Controls.Add(this.TxFaceType);
            this.Controls.Add(this.TxSubstrateNumber);
            this.Controls.Add(this.TxSubstrateLotNumber);
            this.Controls.Add(this.TxRecipeName);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmAddTuInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "输入产品信息";
            this.Load += new System.EventHandler(this.FrmAddTuInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TxRecipeName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxSubstrateLotNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxSubstrateNumber.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxFaceType.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit TxRecipeName;
        private DevExpress.XtraEditors.TextEdit TxSubstrateLotNumber;
        private DevExpress.XtraEditors.TextEdit TxSubstrateNumber;
        private DevExpress.XtraEditors.TextEdit TxFaceType;
        private DevExpress.XtraEditors.SimpleButton BtSure;
    }
}