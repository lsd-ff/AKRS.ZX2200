namespace AKRS.ZX2200.Main.Machine.ObjectLevel
{
    partial class UcObjectLevel
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
            this.LbQualified = new DevExpress.XtraEditors.LabelControl();
            this.LbLevel = new DevExpress.XtraEditors.LabelControl();
            this.BtTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtAssistant = new DevExpress.XtraEditors.SimpleButton();
            this.LbName = new DevExpress.XtraEditors.LabelControl();
            this.LbStandard = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // LbQualified
            // 
            this.LbQualified.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbQualified.Appearance.Options.UseFont = true;
            this.LbQualified.Location = new System.Drawing.Point(419, 25);
            this.LbQualified.Name = "LbQualified";
            this.LbQualified.Size = new System.Drawing.Size(63, 24);
            this.LbQualified.TabIndex = 25;
            this.LbQualified.Text = "不合格";
            // 
            // LbLevel
            // 
            this.LbLevel.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbLevel.Appearance.Options.UseFont = true;
            this.LbLevel.Location = new System.Drawing.Point(175, 25);
            this.LbLevel.Name = "LbLevel";
            this.LbLevel.Size = new System.Drawing.Size(63, 24);
            this.LbLevel.TabIndex = 24;
            this.LbLevel.Text = "水平度";
            // 
            // BtTest
            // 
            this.BtTest.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtTest.Appearance.Options.UseFont = true;
            this.BtTest.Location = new System.Drawing.Point(704, 14);
            this.BtTest.Name = "BtTest";
            this.BtTest.Size = new System.Drawing.Size(120, 46);
            this.BtTest.TabIndex = 23;
            this.BtTest.Text = "测试";
            this.BtTest.Click += new System.EventHandler(this.BtTest_Click);
            // 
            // BtAssistant
            // 
            this.BtAssistant.Appearance.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtAssistant.Appearance.Options.UseFont = true;
            this.BtAssistant.Location = new System.Drawing.Point(584, 14);
            this.BtAssistant.Name = "BtAssistant";
            this.BtAssistant.Size = new System.Drawing.Size(114, 46);
            this.BtAssistant.TabIndex = 22;
            this.BtAssistant.Text = "示教";
            this.BtAssistant.Click += new System.EventHandler(this.BtAssistant_Click);
            // 
            // LbName
            // 
            this.LbName.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbName.Appearance.Options.UseFont = true;
            this.LbName.Location = new System.Drawing.Point(3, 25);
            this.LbName.Name = "LbName";
            this.LbName.Size = new System.Drawing.Size(126, 24);
            this.LbName.TabIndex = 21;
            this.LbName.Text = "焊头水平测试";
            // 
            // LbStandard
            // 
            this.LbStandard.Appearance.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbStandard.Appearance.Options.UseFont = true;
            this.LbStandard.Location = new System.Drawing.Point(311, 25);
            this.LbStandard.Name = "LbStandard";
            this.LbStandard.Size = new System.Drawing.Size(42, 24);
            this.LbStandard.TabIndex = 26;
            this.LbStandard.Text = "标准";
            // 
            // UcObjectLevel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LbStandard);
            this.Controls.Add(this.LbQualified);
            this.Controls.Add(this.LbLevel);
            this.Controls.Add(this.BtTest);
            this.Controls.Add(this.BtAssistant);
            this.Controls.Add(this.LbName);
            this.Name = "UcObjectLevel";
            this.Size = new System.Drawing.Size(836, 71);
            this.Load += new System.EventHandler(this.UcObjectLevel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl LbQualified;
        private DevExpress.XtraEditors.LabelControl LbLevel;
        private DevExpress.XtraEditors.SimpleButton BtTest;
        private DevExpress.XtraEditors.SimpleButton BtAssistant;
        private DevExpress.XtraEditors.LabelControl LbName;
        private DevExpress.XtraEditors.LabelControl LbStandard;
    }
}
