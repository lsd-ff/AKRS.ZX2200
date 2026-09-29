namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class UcVacuum
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
            this.components = new System.ComponentModel.Container();
            this.BtnIPTVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.BtnWeakBlow = new DevExpress.XtraEditors.SimpleButton();
            this.BtnNozzleVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.BtnESVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.BtnESBlow = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBondHeadVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.TxtVacuumValue = new DevExpress.XtraEditors.TextEdit();
            this.BtnFlipTableVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.TxtBondheadVacuumValue = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.TxtVacuumValue.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBondheadVacuumValue.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnIPTVacuum
            // 
            this.BtnIPTVacuum.Location = new System.Drawing.Point(84, 367);
            this.BtnIPTVacuum.Name = "BtnIPTVacuum";
            this.BtnIPTVacuum.Size = new System.Drawing.Size(262, 50);
            this.BtnIPTVacuum.TabIndex = 0;
            this.BtnIPTVacuum.Text = "开中转台真空";
            this.BtnIPTVacuum.Click += new System.EventHandler(this.BtnIPTVacuum_Click);
            // 
            // BtnWeakBlow
            // 
            this.BtnWeakBlow.Location = new System.Drawing.Point(84, 278);
            this.BtnWeakBlow.Name = "BtnWeakBlow";
            this.BtnWeakBlow.Size = new System.Drawing.Size(262, 50);
            this.BtnWeakBlow.TabIndex = 1;
            this.BtnWeakBlow.Text = "开吸嘴吹气";
            this.BtnWeakBlow.Click += new System.EventHandler(this.BtnWeakBlow_Click);
            // 
            // BtnNozzleVacuum
            // 
            this.BtnNozzleVacuum.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.BtnNozzleVacuum.Appearance.Options.UseBackColor = true;
            this.BtnNozzleVacuum.Location = new System.Drawing.Point(84, 191);
            this.BtnNozzleVacuum.Name = "BtnNozzleVacuum";
            this.BtnNozzleVacuum.Size = new System.Drawing.Size(262, 50);
            this.BtnNozzleVacuum.TabIndex = 2;
            this.BtnNozzleVacuum.Text = "开吸嘴真空";
            this.BtnNozzleVacuum.Click += new System.EventHandler(this.BtnNozzleVacuum_Click);
            // 
            // BtnESVacuum
            // 
            this.BtnESVacuum.Location = new System.Drawing.Point(84, 455);
            this.BtnESVacuum.Name = "BtnESVacuum";
            this.BtnESVacuum.Size = new System.Drawing.Size(262, 50);
            this.BtnESVacuum.TabIndex = 3;
            this.BtnESVacuum.Text = "开顶针真空";
            this.BtnESVacuum.Click += new System.EventHandler(this.BtnESVacuum_Click);
            // 
            // BtnESBlow
            // 
            this.BtnESBlow.Location = new System.Drawing.Point(84, 543);
            this.BtnESBlow.Name = "BtnESBlow";
            this.BtnESBlow.Size = new System.Drawing.Size(262, 50);
            this.BtnESBlow.TabIndex = 4;
            this.BtnESBlow.Text = "开顶针吹气";
            this.BtnESBlow.Click += new System.EventHandler(this.BtnESBlow_Click);
            // 
            // BtnBondHeadVacuum
            // 
            this.BtnBondHeadVacuum.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.BtnBondHeadVacuum.Appearance.Options.UseBackColor = true;
            this.BtnBondHeadVacuum.Location = new System.Drawing.Point(84, 109);
            this.BtnBondHeadVacuum.Name = "BtnBondHeadVacuum";
            this.BtnBondHeadVacuum.Size = new System.Drawing.Size(262, 50);
            this.BtnBondHeadVacuum.TabIndex = 5;
            this.BtnBondHeadVacuum.Text = "开焊头吸附";
            this.BtnBondHeadVacuum.Click += new System.EventHandler(this.BtnBondHeadVacuum_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 600;
            this.timer1.Tag = "UcVacuum";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(84, 64);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(110, 18);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "吸嘴真空模拟量:";
            // 
            // TxtVacuumValue
            // 
            this.TxtVacuumValue.Enabled = false;
            this.TxtVacuumValue.Location = new System.Drawing.Point(215, 61);
            this.TxtVacuumValue.Name = "TxtVacuumValue";
            this.TxtVacuumValue.Size = new System.Drawing.Size(131, 24);
            this.TxtVacuumValue.TabIndex = 7;
            // 
            // BtnFlipTableVacuum
            // 
            this.BtnFlipTableVacuum.Location = new System.Drawing.Point(84, 620);
            this.BtnFlipTableVacuum.Name = "BtnFlipTableVacuum";
            this.BtnFlipTableVacuum.Size = new System.Drawing.Size(262, 50);
            this.BtnFlipTableVacuum.TabIndex = 8;
            this.BtnFlipTableVacuum.Text = "开翻转台真空";
            this.BtnFlipTableVacuum.Click += new System.EventHandler(this.BtnFlipTableVacuum_Click);
            // 
            // TxtBondheadVacuumValue
            // 
            this.TxtBondheadVacuumValue.Enabled = false;
            this.TxtBondheadVacuumValue.Location = new System.Drawing.Point(215, 19);
            this.TxtBondheadVacuumValue.Name = "TxtBondheadVacuumValue";
            this.TxtBondheadVacuumValue.Size = new System.Drawing.Size(131, 24);
            this.TxtBondheadVacuumValue.TabIndex = 10;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(84, 22);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(110, 18);
            this.labelControl2.TabIndex = 9;
            this.labelControl2.Text = "焊头吸附模拟量:";
            // 
            // UcVacuum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TxtBondheadVacuumValue);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.BtnFlipTableVacuum);
            this.Controls.Add(this.TxtVacuumValue);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnBondHeadVacuum);
            this.Controls.Add(this.BtnESBlow);
            this.Controls.Add(this.BtnESVacuum);
            this.Controls.Add(this.BtnNozzleVacuum);
            this.Controls.Add(this.BtnWeakBlow);
            this.Controls.Add(this.BtnIPTVacuum);
            this.Name = "UcVacuum";
            this.Size = new System.Drawing.Size(439, 690);
            ((System.ComponentModel.ISupportInitialize)(this.TxtVacuumValue.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBondheadVacuumValue.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnIPTVacuum;
        private DevExpress.XtraEditors.SimpleButton BtnWeakBlow;
        private DevExpress.XtraEditors.SimpleButton BtnNozzleVacuum;
        private DevExpress.XtraEditors.SimpleButton BtnESVacuum;
        private DevExpress.XtraEditors.SimpleButton BtnESBlow;
        private DevExpress.XtraEditors.SimpleButton BtnBondHeadVacuum;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit TxtVacuumValue;
        private DevExpress.XtraEditors.SimpleButton BtnFlipTableVacuum;
        private DevExpress.XtraEditors.TextEdit TxtBondheadVacuumValue;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}