namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmTest
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.BtTestMeasureHeight = new DevExpress.XtraEditors.SimpleButton();
            this.BtTestMeasureHeight2 = new DevExpress.XtraEditors.SimpleButton();
            this.BtSaveUplookImage = new DevExpress.XtraEditors.SimpleButton();
            this.BtReadBondLvdt = new DevExpress.XtraEditors.SimpleButton();
            this.SpLvdt = new DevExpress.XtraEditors.SpinEdit();
            this.BtMessageBox = new DevExpress.XtraEditors.SimpleButton();
            this.BtReadBondVaccum = new DevExpress.XtraEditors.SimpleButton();
            this.SpBondVaccum = new DevExpress.XtraEditors.SpinEdit();
            this.BtReadBondVacuumManul = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.SpLvdt.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBondVaccum.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtTestMeasureHeight
            // 
            this.BtTestMeasureHeight.Location = new System.Drawing.Point(123, 162);
            this.BtTestMeasureHeight.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtTestMeasureHeight.Name = "BtTestMeasureHeight";
            this.BtTestMeasureHeight.Size = new System.Drawing.Size(130, 57);
            this.BtTestMeasureHeight.TabIndex = 0;
            this.BtTestMeasureHeight.Text = "探针测高测试";
            this.BtTestMeasureHeight.Click += new System.EventHandler(this.BtTestMeasureHeight_Click);
            // 
            // BtTestMeasureHeight2
            // 
            this.BtTestMeasureHeight2.Location = new System.Drawing.Point(303, 162);
            this.BtTestMeasureHeight2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtTestMeasureHeight2.Name = "BtTestMeasureHeight2";
            this.BtTestMeasureHeight2.Size = new System.Drawing.Size(129, 57);
            this.BtTestMeasureHeight2.TabIndex = 1;
            this.BtTestMeasureHeight2.Text = "探针测高测试2";
            this.BtTestMeasureHeight2.Click += new System.EventHandler(this.BtTestMeasureHeight2_Click);
            // 
            // BtSaveUplookImage
            // 
            this.BtSaveUplookImage.Location = new System.Drawing.Point(579, 155);
            this.BtSaveUplookImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtSaveUplookImage.Name = "BtSaveUplookImage";
            this.BtSaveUplookImage.Size = new System.Drawing.Size(129, 64);
            this.BtSaveUplookImage.TabIndex = 2;
            this.BtSaveUplookImage.Text = "上视存图";
            this.BtSaveUplookImage.Click += new System.EventHandler(this.BtSaveUplookImage_Click);
            // 
            // BtReadBondLvdt
            // 
            this.BtReadBondLvdt.Location = new System.Drawing.Point(1149, 162);
            this.BtReadBondLvdt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtReadBondLvdt.Name = "BtReadBondLvdt";
            this.BtReadBondLvdt.Size = new System.Drawing.Size(157, 36);
            this.BtReadBondLvdt.TabIndex = 3;
            this.BtReadBondLvdt.Text = "读取Bond Lvdt";
            this.BtReadBondLvdt.Click += new System.EventHandler(this.BtReadBondLvdt_Click);
            // 
            // SpLvdt
            // 
            this.SpLvdt.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLvdt.Location = new System.Drawing.Point(857, 162);
            this.SpLvdt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SpLvdt.Name = "SpLvdt";
            this.SpLvdt.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLvdt.Size = new System.Drawing.Size(257, 28);
            this.SpLvdt.TabIndex = 4;
            // 
            // BtMessageBox
            // 
            this.BtMessageBox.Location = new System.Drawing.Point(579, 292);
            this.BtMessageBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtMessageBox.Name = "BtMessageBox";
            this.BtMessageBox.Size = new System.Drawing.Size(124, 57);
            this.BtMessageBox.TabIndex = 5;
            this.BtMessageBox.Text = "弹窗测试";
            this.BtMessageBox.Click += new System.EventHandler(this.BtMessageBox_Click);
            // 
            // BtReadBondVaccum
            // 
            this.BtReadBondVaccum.Location = new System.Drawing.Point(1149, 288);
            this.BtReadBondVaccum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtReadBondVaccum.Name = "BtReadBondVaccum";
            this.BtReadBondVaccum.Size = new System.Drawing.Size(214, 36);
            this.BtReadBondVaccum.TabIndex = 6;
            this.BtReadBondVaccum.Text = "启动读取Bond真空值";
            this.BtReadBondVaccum.Click += new System.EventHandler(this.BtReadBondVaccum_Click);
            // 
            // SpBondVaccum
            // 
            this.SpBondVaccum.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpBondVaccum.Location = new System.Drawing.Point(857, 289);
            this.SpBondVaccum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SpBondVaccum.Name = "SpBondVaccum";
            this.SpBondVaccum.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBondVaccum.Size = new System.Drawing.Size(257, 28);
            this.SpBondVaccum.TabIndex = 7;
            // 
            // BtReadBondVacuumManul
            // 
            this.BtReadBondVacuumManul.Location = new System.Drawing.Point(1149, 333);
            this.BtReadBondVacuumManul.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BtReadBondVacuumManul.Name = "BtReadBondVacuumManul";
            this.BtReadBondVacuumManul.Size = new System.Drawing.Size(214, 36);
            this.BtReadBondVacuumManul.TabIndex = 8;
            this.BtReadBondVacuumManul.Text = "读取Bond真空值";
            this.BtReadBondVacuumManul.Click += new System.EventHandler(this.BtReadBondVacuumManul_Click);
            // 
            // FrmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1627, 1249);
            this.Controls.Add(this.BtReadBondVacuumManul);
            this.Controls.Add(this.SpBondVaccum);
            this.Controls.Add(this.BtReadBondVaccum);
            this.Controls.Add(this.BtMessageBox);
            this.Controls.Add(this.SpLvdt);
            this.Controls.Add(this.BtReadBondLvdt);
            this.Controls.Add(this.BtSaveUplookImage);
            this.Controls.Add(this.BtTestMeasureHeight2);
            this.Controls.Add(this.BtTestMeasureHeight);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmTest";
            this.Text = "FrmTest";
            ((System.ComponentModel.ISupportInitialize)(this.SpLvdt.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBondVaccum.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtTestMeasureHeight;
        private DevExpress.XtraEditors.SimpleButton BtTestMeasureHeight2;
        private DevExpress.XtraEditors.SimpleButton BtSaveUplookImage;
        private DevExpress.XtraEditors.SimpleButton BtReadBondLvdt;
        private DevExpress.XtraEditors.SpinEdit SpLvdt;
        private DevExpress.XtraEditors.SimpleButton BtMessageBox;
        private DevExpress.XtraEditors.SimpleButton BtReadBondVaccum;
        private DevExpress.XtraEditors.SpinEdit SpBondVaccum;
        private DevExpress.XtraEditors.SimpleButton BtReadBondVacuumManul;
    }
}