namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class FrmAutoSlideFlux
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
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.LbTip = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.SpSlideInterval = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.SpSlideTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlideInterval.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlideTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(178, 337);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(220, 46);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "开始";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LbTip
            // 
            this.LbTip.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTip.Appearance.ForeColor = System.Drawing.Color.Red;
            this.LbTip.Appearance.Options.UseFont = true;
            this.LbTip.Appearance.Options.UseForeColor = true;
            this.LbTip.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LbTip.Location = new System.Drawing.Point(47, 206);
            this.LbTip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbTip.Name = "LbTip";
            this.LbTip.Size = new System.Drawing.Size(547, 86);
            this.LbTip.TabIndex = 2;
            this.LbTip.Text = "刮胶盘自动刮胶中......";
            this.LbTip.Visible = false;
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(430, 54);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(19, 14);
            this.labelControl12.TabIndex = 87;
            this.labelControl12.Text = "min";
            // 
            // SpSlideInterval
            // 
            this.SpSlideInterval.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpSlideInterval.Location = new System.Drawing.Point(178, 51);
            this.SpSlideInterval.Name = "SpSlideInterval";
            this.SpSlideInterval.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlideInterval.Size = new System.Drawing.Size(223, 20);
            this.SpSlideInterval.TabIndex = 86;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(87, 54);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 85;
            this.labelControl8.Text = "刮胶间隔";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(430, 100);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(0, 14);
            this.labelControl3.TabIndex = 93;
            // 
            // SpSlideTime
            // 
            this.SpSlideTime.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpSlideTime.Location = new System.Drawing.Point(178, 97);
            this.SpSlideTime.Name = "SpSlideTime";
            this.SpSlideTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlideTime.Size = new System.Drawing.Size(223, 20);
            this.SpSlideTime.TabIndex = 92;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(87, 100);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 91;
            this.labelControl4.Text = "刮胶次数";
            // 
            // FrmAutoSlideFlux
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 557);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.SpSlideTime);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl12);
            this.Controls.Add(this.SpSlideInterval);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.LbTip);
            this.Controls.Add(this.BtnStart);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmAutoSlideFlux";
            this.Text = "自动刮胶";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAutoSlideFlux_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SpSlideInterval.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlideTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl LbTip;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.SpinEdit SpSlideInterval;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit SpSlideTime;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}