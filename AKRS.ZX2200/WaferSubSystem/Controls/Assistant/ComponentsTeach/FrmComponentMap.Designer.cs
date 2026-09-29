namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    partial class FrmComponentMap
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
            this.TxtMapID = new DevExpress.XtraEditors.TextEdit();
            this.BtnOK = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnSetReferencePoint1 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoadWaferMap = new DevExpress.XtraEditors.SimpleButton();
            this.BtnResetWaferMap = new DevExpress.XtraEditors.SimpleButton();
            this.BtnShowMap = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TxtMapID.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // TxtMapID
            // 
            this.TxtMapID.Location = new System.Drawing.Point(71, 32);
            this.TxtMapID.Name = "TxtMapID";
            this.TxtMapID.Size = new System.Drawing.Size(252, 20);
            this.TxtMapID.TabIndex = 0;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(38, 241);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(75, 23);
            this.BtnOK.TabIndex = 1;
            this.BtnOK.Text = "OK";
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(285, 241);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(16, 35);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(38, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "Map ID";
            // 
            // BtnSetReferencePoint1
            // 
            this.BtnSetReferencePoint1.Location = new System.Drawing.Point(181, 74);
            this.BtnSetReferencePoint1.Name = "BtnSetReferencePoint1";
            this.BtnSetReferencePoint1.Size = new System.Drawing.Size(142, 23);
            this.BtnSetReferencePoint1.TabIndex = 5;
            this.BtnSetReferencePoint1.Text = "Set ReferencePoint1";
            this.BtnSetReferencePoint1.Click += new System.EventHandler(this.BtnSetReferencePoint1_Click);
            // 
            // BtnLoadWaferMap
            // 
            this.BtnLoadWaferMap.Location = new System.Drawing.Point(181, 116);
            this.BtnLoadWaferMap.Name = "BtnLoadWaferMap";
            this.BtnLoadWaferMap.Size = new System.Drawing.Size(142, 23);
            this.BtnLoadWaferMap.TabIndex = 6;
            this.BtnLoadWaferMap.Text = "Load waferMap";
            this.BtnLoadWaferMap.Click += new System.EventHandler(this.BtnLoadWaferMap_Click);
            // 
            // BtnResetWaferMap
            // 
            this.BtnResetWaferMap.Location = new System.Drawing.Point(181, 162);
            this.BtnResetWaferMap.Name = "BtnResetWaferMap";
            this.BtnResetWaferMap.Size = new System.Drawing.Size(142, 23);
            this.BtnResetWaferMap.TabIndex = 7;
            this.BtnResetWaferMap.Text = "Reset waferMap";
            this.BtnResetWaferMap.Click += new System.EventHandler(this.BtnResetWaferMap_Click);
            // 
            // BtnShowMap
            // 
            this.BtnShowMap.Location = new System.Drawing.Point(181, 202);
            this.BtnShowMap.Name = "BtnShowMap";
            this.BtnShowMap.Size = new System.Drawing.Size(142, 23);
            this.BtnShowMap.TabIndex = 8;
            this.BtnShowMap.Text = "Show map";
            this.BtnShowMap.Click += new System.EventHandler(this.BtnShowMap_Click);
            // 
            // FrmComponentMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 317);
            this.Controls.Add(this.BtnShowMap);
            this.Controls.Add(this.BtnResetWaferMap);
            this.Controls.Add(this.BtnLoadWaferMap);
            this.Controls.Add(this.BtnSetReferencePoint1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.TxtMapID);
            this.Name = "FrmComponentMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmComponentMap";
            this.Load += new System.EventHandler(this.FrmComponentMap_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TxtMapID.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit TxtMapID;
        private DevExpress.XtraEditors.SimpleButton BtnOK;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnSetReferencePoint1;
        private DevExpress.XtraEditors.SimpleButton BtnLoadWaferMap;
        private DevExpress.XtraEditors.SimpleButton BtnResetWaferMap;
        private DevExpress.XtraEditors.SimpleButton BtnShowMap;
    }
}