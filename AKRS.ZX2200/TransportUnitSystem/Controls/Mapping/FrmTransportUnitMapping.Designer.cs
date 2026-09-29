namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    partial class FrmTransportUnitMapping
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
            this.BtSaveMapping = new DevExpress.XtraEditors.SimpleButton();
            this.BtInPutTuInfo = new DevExpress.XtraEditors.SimpleButton();
            this.BtImportMapping = new DevExpress.XtraEditors.SimpleButton();
            this.BtInit = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtInputID = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.BtInputID);
            this.panelControl1.Controls.Add(this.BtSaveMapping);
            this.panelControl1.Controls.Add(this.BtInPutTuInfo);
            this.panelControl1.Controls.Add(this.BtImportMapping);
            this.panelControl1.Controls.Add(this.BtInit);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1438, 70);
            this.panelControl1.TabIndex = 0;
            // 
            // BtSaveMapping
            // 
            this.BtSaveMapping.Location = new System.Drawing.Point(536, 12);
            this.BtSaveMapping.Name = "BtSaveMapping";
            this.BtSaveMapping.Size = new System.Drawing.Size(137, 46);
            this.BtSaveMapping.TabIndex = 4;
            this.BtSaveMapping.Text = "保存Mapping";
            this.BtSaveMapping.Click += new System.EventHandler(this.BtSaveMapping_Click);
            // 
            // BtInPutTuInfo
            // 
            this.BtInPutTuInfo.Location = new System.Drawing.Point(12, 12);
            this.BtInPutTuInfo.Name = "BtInPutTuInfo";
            this.BtInPutTuInfo.Size = new System.Drawing.Size(129, 46);
            this.BtInPutTuInfo.TabIndex = 3;
            this.BtInPutTuInfo.Text = "新建Mapping";
            this.BtInPutTuInfo.Click += new System.EventHandler(this.BtInPutTuInfo_Click);
            // 
            // BtImportMapping
            // 
            this.BtImportMapping.Location = new System.Drawing.Point(175, 12);
            this.BtImportMapping.Name = "BtImportMapping";
            this.BtImportMapping.Size = new System.Drawing.Size(137, 46);
            this.BtImportMapping.TabIndex = 1;
            this.BtImportMapping.Text = "本地载入Mapping";
            this.BtImportMapping.Click += new System.EventHandler(this.BtImportMapping_Click);
            // 
            // BtInit
            // 
            this.BtInit.Location = new System.Drawing.Point(724, 12);
            this.BtInit.Name = "BtInit";
            this.BtInit.Size = new System.Drawing.Size(95, 46);
            this.BtInit.TabIndex = 0;
            this.BtInit.Text = "清空记忆";
            this.BtInit.Click += new System.EventHandler(this.BtInit_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 70);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1438, 715);
            this.panelControl2.TabIndex = 1;
            // 
            // BtInputID
            // 
            this.BtInputID.Location = new System.Drawing.Point(357, 12);
            this.BtInputID.Name = "BtInputID";
            this.BtInputID.Size = new System.Drawing.Size(137, 46);
            this.BtInputID.TabIndex = 5;
            this.BtInputID.Text = "扫码识别Mapping";
            this.BtInputID.Click += new System.EventHandler(this.BtInputID_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(868, 20);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(127, 24);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "当前产品序号:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(1001, 20);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(120, 24);
            this.labelControl2.TabIndex = 7;
            this.labelControl2.Text = "当前产品序号";
            // 
            // FrmTransportUnitMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1438, 785);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmTransportUnitMapping";
            this.Text = "框架实时图";
            this.Load += new System.EventHandler(this.FrmTransportUnitMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtInit;
        private DevExpress.XtraEditors.SimpleButton BtImportMapping;
        private DevExpress.XtraEditors.SimpleButton BtInPutTuInfo;
        private DevExpress.XtraEditors.SimpleButton BtSaveMapping;
        private DevExpress.XtraEditors.SimpleButton BtInputID;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}