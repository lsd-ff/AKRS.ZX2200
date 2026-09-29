namespace AKRS.ZX2200.Infrastructure.Controls.Common
{
    partial class UcAddOffset
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcAddOffset));
            this.BtUp = new DevExpress.XtraEditors.SimpleButton();
            this.BtDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtLeft = new DevExpress.XtraEditors.SimpleButton();
            this.BtRight = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.BtAngleAdd = new DevExpress.XtraEditors.SimpleButton();
            this.BtAngleDe = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.SpValueY = new DevExpress.XtraEditors.SpinEdit();
            this.SpValueAngle = new DevExpress.XtraEditors.SpinEdit();
            this.Ck10um = new DevExpress.XtraEditors.CheckEdit();
            this.Ck5um = new DevExpress.XtraEditors.CheckEdit();
            this.Ck1um = new DevExpress.XtraEditors.CheckEdit();
            this.SpValueX = new DevExpress.XtraEditors.SpinEdit();
            this.BtSure = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.SpHeight = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.BtHeightAdd = new DevExpress.XtraEditors.SimpleButton();
            this.BtHeightDe = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.SpValueY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpValueAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck10um.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck5um.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck1um.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpValueX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpHeight.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtUp
            // 
            this.BtUp.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtUp.ImageOptions.SvgImage")));
            this.BtUp.Location = new System.Drawing.Point(244, 21);
            this.BtUp.Name = "BtUp";
            this.BtUp.Size = new System.Drawing.Size(83, 36);
            this.BtUp.TabIndex = 5;
            this.BtUp.Text = "上移";
            this.BtUp.Click += new System.EventHandler(this.BtAdd_Click);
            // 
            // BtDown
            // 
            this.BtDown.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtDown.ImageOptions.SvgImage")));
            this.BtDown.Location = new System.Drawing.Point(244, 294);
            this.BtDown.Name = "BtDown";
            this.BtDown.Size = new System.Drawing.Size(83, 45);
            this.BtDown.TabIndex = 6;
            this.BtDown.Text = "下移";
            this.BtDown.Click += new System.EventHandler(this.BtDe_Click);
            // 
            // BtLeft
            // 
            this.BtLeft.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtLeft.ImageOptions.SvgImage")));
            this.BtLeft.Location = new System.Drawing.Point(11, 157);
            this.BtLeft.Name = "BtLeft";
            this.BtLeft.Size = new System.Drawing.Size(83, 32);
            this.BtLeft.TabIndex = 9;
            this.BtLeft.Text = "左移";
            this.BtLeft.Click += new System.EventHandler(this.BtLeft_Click);
            // 
            // BtRight
            // 
            this.BtRight.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtRight.ImageOptions.SvgImage")));
            this.BtRight.Location = new System.Drawing.Point(512, 154);
            this.BtRight.Name = "BtRight";
            this.BtRight.Size = new System.Drawing.Size(90, 39);
            this.BtRight.TabIndex = 10;
            this.BtRight.Text = "右移";
            this.BtRight.Click += new System.EventHandler(this.BtRight_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.BackColor = System.Drawing.Color.Red;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 15F);
            this.labelControl1.Appearance.Options.UseBackColor = true;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(11, 10);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(114, 24);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "单位 微米/度";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(171, 102);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(7, 14);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "X";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(171, 141);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(8, 14);
            this.labelControl3.TabIndex = 13;
            this.labelControl3.Text = "Y";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(171, 232);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(24, 14);
            this.labelControl4.TabIndex = 14;
            this.labelControl4.Text = "角度";
            // 
            // BtAngleAdd
            // 
            this.BtAngleAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtAngleAdd.ImageOptions.Image")));
            this.BtAngleAdd.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleRight;
            this.BtAngleAdd.Location = new System.Drawing.Point(11, 318);
            this.BtAngleAdd.Name = "BtAngleAdd";
            this.BtAngleAdd.Size = new System.Drawing.Size(83, 33);
            this.BtAngleAdd.TabIndex = 15;
            this.BtAngleAdd.Text = "角度";
            this.BtAngleAdd.Click += new System.EventHandler(this.BtAngleAdd_Click);
            // 
            // BtAngleDe
            // 
            this.BtAngleDe.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtAngleDe.ImageOptions.Image")));
            this.BtAngleDe.Location = new System.Drawing.Point(470, 318);
            this.BtAngleDe.Name = "BtAngleDe";
            this.BtAngleDe.Size = new System.Drawing.Size(83, 33);
            this.BtAngleDe.TabIndex = 16;
            this.BtAngleDe.Text = "角度";
            this.BtAngleDe.Click += new System.EventHandler(this.BtAngleDe_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(378, 102);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(17, 14);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "um";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(378, 143);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(17, 14);
            this.labelControl6.TabIndex = 18;
            this.labelControl6.Text = "um";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(378, 232);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(12, 14);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "度";
            // 
            // SpValueY
            // 
            this.SpValueY.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpValueY.Location = new System.Drawing.Point(235, 139);
            this.SpValueY.Name = "SpValueY";
            this.SpValueY.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpValueY.Size = new System.Drawing.Size(109, 20);
            this.SpValueY.TabIndex = 8;
            // 
            // SpValueAngle
            // 
            this.SpValueAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpValueAngle.Location = new System.Drawing.Point(235, 230);
            this.SpValueAngle.Name = "SpValueAngle";
            this.SpValueAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpValueAngle.Size = new System.Drawing.Size(109, 20);
            this.SpValueAngle.TabIndex = 7;
            // 
            // Ck10um
            // 
            this.Ck10um.Location = new System.Drawing.Point(665, 255);
            this.Ck10um.Name = "Ck10um";
            this.Ck10um.Properties.Caption = "10um / 1°";
            this.Ck10um.Size = new System.Drawing.Size(97, 20);
            this.Ck10um.TabIndex = 4;
            this.Ck10um.CheckedChanged += new System.EventHandler(this.Ck10um_CheckedChanged);
            // 
            // Ck5um
            // 
            this.Ck5um.Location = new System.Drawing.Point(665, 164);
            this.Ck5um.Name = "Ck5um";
            this.Ck5um.Properties.Caption = "5um / 0.5°";
            this.Ck5um.Size = new System.Drawing.Size(97, 20);
            this.Ck5um.TabIndex = 3;
            this.Ck5um.CheckedChanged += new System.EventHandler(this.Ck5um_CheckedChanged);
            // 
            // Ck1um
            // 
            this.Ck1um.Location = new System.Drawing.Point(665, 76);
            this.Ck1um.Name = "Ck1um";
            this.Ck1um.Properties.Caption = "1um / 0.1°";
            this.Ck1um.Size = new System.Drawing.Size(97, 20);
            this.Ck1um.TabIndex = 2;
            this.Ck1um.CheckedChanged += new System.EventHandler(this.Ck1um_CheckedChanged);
            // 
            // SpValueX
            // 
            this.SpValueX.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpValueX.Location = new System.Drawing.Point(235, 99);
            this.SpValueX.Name = "SpValueX";
            this.SpValueX.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpValueX.Size = new System.Drawing.Size(109, 20);
            this.SpValueX.TabIndex = 1;
            // 
            // BtSure
            // 
            this.BtSure.Location = new System.Drawing.Point(734, 318);
            this.BtSure.Name = "BtSure";
            this.BtSure.Size = new System.Drawing.Size(129, 62);
            this.BtSure.TabIndex = 20;
            this.BtSure.Text = "保存";
            this.BtSure.Click += new System.EventHandler(this.BtSure_Click);
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(171, 185);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(7, 14);
            this.labelControl8.TabIndex = 21;
            this.labelControl8.Text = "Z";
            // 
            // SpHeight
            // 
            this.SpHeight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpHeight.Location = new System.Drawing.Point(235, 183);
            this.SpHeight.Name = "SpHeight";
            this.SpHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpHeight.Size = new System.Drawing.Size(109, 20);
            this.SpHeight.TabIndex = 22;
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(378, 188);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(17, 14);
            this.labelControl9.TabIndex = 23;
            this.labelControl9.Text = "um";
            // 
            // BtHeightAdd
            // 
            this.BtHeightAdd.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            this.BtHeightAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtHeightAdd.ImageOptions.SvgImage")));
            this.BtHeightAdd.Location = new System.Drawing.Point(435, 21);
            this.BtHeightAdd.Name = "BtHeightAdd";
            this.BtHeightAdd.Size = new System.Drawing.Size(52, 97);
            this.BtHeightAdd.TabIndex = 24;
            this.BtHeightAdd.Text = "高/n度/n增/n加";
            this.BtHeightAdd.Click += new System.EventHandler(this.BtHeightAdd_Click);
            // 
            // BtHeightDe
            // 
            this.BtHeightDe.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.BottomCenter;
            this.BtHeightDe.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtHeightDe.ImageOptions.SvgImage")));
            this.BtHeightDe.Location = new System.Drawing.Point(435, 209);
            this.BtHeightDe.Name = "BtHeightDe";
            this.BtHeightDe.Size = new System.Drawing.Size(52, 95);
            this.BtHeightDe.TabIndex = 25;
            this.BtHeightDe.Text = "高\\n度\\n增\\n加";
            this.BtHeightDe.Click += new System.EventHandler(this.BtHeightDe_Click);
            // 
            // labelControl10
            // 
            this.labelControl10.Appearance.BackColor = System.Drawing.Color.Cyan;
            this.labelControl10.Appearance.Font = new System.Drawing.Font("Tahoma", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl10.Appearance.ForeColor = System.Drawing.Color.Red;
            this.labelControl10.Appearance.Options.UseBackColor = true;
            this.labelControl10.Appearance.Options.UseFont = true;
            this.labelControl10.Appearance.Options.UseForeColor = true;
            this.labelControl10.Location = new System.Drawing.Point(171, 161);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(560, 42);
            this.labelControl10.TabIndex = 26;
            this.labelControl10.Text = "焊后动态补偿开启中，无法修改数值";
            this.labelControl10.Visible = false;
            // 
            // labelControl11
            // 
            this.labelControl11.Appearance.BackColor = System.Drawing.Color.Red;
            this.labelControl11.Appearance.Options.UseBackColor = true;
            this.labelControl11.Location = new System.Drawing.Point(357, 258);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(60, 14);
            this.labelControl11.TabIndex = 27;
            this.labelControl11.Text = "逆时针为正";
            // 
            // UcAddOffset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl11);
            this.Controls.Add(this.labelControl10);
            this.Controls.Add(this.BtHeightDe);
            this.Controls.Add(this.BtHeightAdd);
            this.Controls.Add(this.labelControl9);
            this.Controls.Add(this.SpHeight);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.BtSure);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.BtAngleDe);
            this.Controls.Add(this.BtAngleAdd);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtRight);
            this.Controls.Add(this.BtLeft);
            this.Controls.Add(this.SpValueY);
            this.Controls.Add(this.SpValueAngle);
            this.Controls.Add(this.BtDown);
            this.Controls.Add(this.BtUp);
            this.Controls.Add(this.Ck10um);
            this.Controls.Add(this.Ck5um);
            this.Controls.Add(this.Ck1um);
            this.Controls.Add(this.SpValueX);
            this.Name = "UcAddOffset";
            this.Size = new System.Drawing.Size(874, 388);
            this.Load += new System.EventHandler(this.FrmAddOffset_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SpValueY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpValueAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck10um.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck5um.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Ck1um.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpValueX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpHeight.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.SpinEdit SpValueX;
        private DevExpress.XtraEditors.CheckEdit Ck1um;
        private DevExpress.XtraEditors.CheckEdit Ck5um;
        private DevExpress.XtraEditors.CheckEdit Ck10um;
        private DevExpress.XtraEditors.SimpleButton BtUp;
        private DevExpress.XtraEditors.SimpleButton BtDown;
        private DevExpress.XtraEditors.SpinEdit SpValueAngle;
        private DevExpress.XtraEditors.SpinEdit SpValueY;
        private DevExpress.XtraEditors.SimpleButton BtLeft;
        private DevExpress.XtraEditors.SimpleButton BtRight;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton BtAngleAdd;
        private DevExpress.XtraEditors.SimpleButton BtAngleDe;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton BtSure;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.SpinEdit SpHeight;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SimpleButton BtHeightAdd;
        private DevExpress.XtraEditors.SimpleButton BtHeightDe;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl11;
    }
}