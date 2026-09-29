namespace AKRS.ZX2200.BondSystem.Controls.Experiment
{
    partial class FrmBondheadTest
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStart2 = new DevExpress.XtraEditors.SimpleButton();
            this.SpAngle = new DevExpress.XtraEditors.SpinEdit();
            this.SpForce = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.BtnStart3 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpAngle3 = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.BtnStart4 = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl14 = new DevExpress.XtraEditors.LabelControl();
            this.SpAngleDistance = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl15 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.SpSlowTravelDistance = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl13 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl11 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.SpDelay = new DevExpress.XtraEditors.SpinEdit();
            this.SpForceSpacing = new DevExpress.XtraEditors.SpinEdit();
            this.SpMaxForce = new DevExpress.XtraEditors.SpinEdit();
            this.SpMinForce = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl21 = new DevExpress.XtraEditors.LabelControl();
            this.SpEclispTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl17 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngle3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngleDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravelDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceSpacing.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMaxForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMinForce.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpEclispTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(242, 35);
            this.BtnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(148, 34);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "Start1";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // LbTip
            // 
            this.LbTip.Appearance.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbTip.Appearance.ForeColor = System.Drawing.Color.Red;
            this.LbTip.Appearance.Options.UseFont = true;
            this.LbTip.Appearance.Options.UseForeColor = true;
            this.LbTip.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.LbTip.Location = new System.Drawing.Point(10, 24);
            this.LbTip.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbTip.Name = "LbTip";
            this.LbTip.Size = new System.Drawing.Size(397, 79);
            this.LbTip.TabIndex = 1;
            this.LbTip.Text = "测试中。。。。。。";
            this.LbTip.Visible = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnStart);
            this.groupControl1.Controls.Add(this.LbTip);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 243);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(544, 82);
            this.groupControl1.TabIndex = 75;
            this.groupControl1.Text = "测试1：焊头转不同角度，以不同力去下压测试";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.BtnStart2);
            this.groupControl2.Controls.Add(this.SpAngle);
            this.groupControl2.Controls.Add(this.SpForce);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.Location = new System.Drawing.Point(0, 325);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(544, 100);
            this.groupControl2.TabIndex = 76;
            this.groupControl2.Text = "测试2：以特定的力和角度去下压";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(13, 74);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 81;
            this.labelControl5.Text = "角度";
            // 
            // BtnStart2
            // 
            this.BtnStart2.Location = new System.Drawing.Point(242, 41);
            this.BtnStart2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart2.Name = "BtnStart2";
            this.BtnStart2.Size = new System.Drawing.Size(148, 34);
            this.BtnStart2.TabIndex = 74;
            this.BtnStart2.Text = "Start2";
            this.BtnStart2.Click += new System.EventHandler(this.BtnStart2_Click);
            // 
            // SpAngle
            // 
            this.SpAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAngle.Location = new System.Drawing.Point(90, 75);
            this.SpAngle.Name = "SpAngle";
            this.SpAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAngle.Size = new System.Drawing.Size(100, 20);
            this.SpAngle.TabIndex = 80;
            // 
            // SpForce
            // 
            this.SpForce.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpForce.Location = new System.Drawing.Point(90, 39);
            this.SpForce.Name = "SpForce";
            this.SpForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForce.Size = new System.Drawing.Size(100, 20);
            this.SpForce.TabIndex = 77;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(13, 38);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(12, 14);
            this.labelControl3.TabIndex = 78;
            this.labelControl3.Text = "力";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.BtnStart3);
            this.groupControl3.Controls.Add(this.labelControl1);
            this.groupControl3.Controls.Add(this.SpAngle3);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.Location = new System.Drawing.Point(0, 425);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(544, 82);
            this.groupControl3.TabIndex = 78;
            this.groupControl3.Text = "测试3：在特定角度下用最小力~最大力去下压";
            // 
            // BtnStart3
            // 
            this.BtnStart3.Location = new System.Drawing.Point(242, 32);
            this.BtnStart3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart3.Name = "BtnStart3";
            this.BtnStart3.Size = new System.Drawing.Size(148, 34);
            this.BtnStart3.TabIndex = 82;
            this.BtnStart3.Text = "Start3";
            this.BtnStart3.Click += new System.EventHandler(this.BtnStart3_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(13, 42);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 83;
            this.labelControl1.Text = "角度";
            // 
            // SpAngle3
            // 
            this.SpAngle3.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAngle3.Location = new System.Drawing.Point(90, 43);
            this.SpAngle3.Name = "SpAngle3";
            this.SpAngle3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAngle3.Size = new System.Drawing.Size(100, 20);
            this.SpAngle3.TabIndex = 82;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.BtnStart4);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl4.Location = new System.Drawing.Point(0, 507);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(544, 82);
            this.groupControl4.TabIndex = 84;
            this.groupControl4.Text = "测试4：T轴转不同的角度去读取焊头力值，记录读压力表和压力表清零时间";
            // 
            // BtnStart4
            // 
            this.BtnStart4.Enabled = false;
            this.BtnStart4.Location = new System.Drawing.Point(242, 33);
            this.BtnStart4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnStart4.Name = "BtnStart4";
            this.BtnStart4.Size = new System.Drawing.Size(148, 34);
            this.BtnStart4.TabIndex = 82;
            this.BtnStart4.Tag = "start4";
            this.BtnStart4.Text = "start4";
            this.BtnStart4.Click += new System.EventHandler(this.BtnStart4_Click);
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.labelControl14);
            this.groupControl6.Controls.Add(this.SpAngleDistance);
            this.groupControl6.Controls.Add(this.labelControl15);
            this.groupControl6.Controls.Add(this.labelControl2);
            this.groupControl6.Controls.Add(this.SpSlowTravelDistance);
            this.groupControl6.Controls.Add(this.labelControl13);
            this.groupControl6.Controls.Add(this.labelControl12);
            this.groupControl6.Controls.Add(this.labelControl11);
            this.groupControl6.Controls.Add(this.labelControl10);
            this.groupControl6.Controls.Add(this.labelControl9);
            this.groupControl6.Controls.Add(this.SpDelay);
            this.groupControl6.Controls.Add(this.SpForceSpacing);
            this.groupControl6.Controls.Add(this.SpMaxForce);
            this.groupControl6.Controls.Add(this.SpMinForce);
            this.groupControl6.Controls.Add(this.labelControl8);
            this.groupControl6.Controls.Add(this.labelControl7);
            this.groupControl6.Controls.Add(this.labelControl6);
            this.groupControl6.Controls.Add(this.labelControl4);
            this.groupControl6.Controls.Add(this.labelControl21);
            this.groupControl6.Controls.Add(this.SpEclispTime);
            this.groupControl6.Controls.Add(this.labelControl17);
            this.groupControl6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl6.Location = new System.Drawing.Point(0, 0);
            this.groupControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(544, 243);
            this.groupControl6.TabIndex = 86;
            this.groupControl6.Text = "参数设置";
            // 
            // labelControl14
            // 
            this.labelControl14.Location = new System.Drawing.Point(409, 147);
            this.labelControl14.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl14.Name = "labelControl14";
            this.labelControl14.Size = new System.Drawing.Size(6, 14);
            this.labelControl14.TabIndex = 90;
            this.labelControl14.Text = "°";
            // 
            // SpAngleDistance
            // 
            this.SpAngleDistance.EditValue = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.SpAngleDistance.Location = new System.Drawing.Point(135, 144);
            this.SpAngleDistance.Name = "SpAngleDistance";
            this.SpAngleDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAngleDistance.Size = new System.Drawing.Size(223, 20);
            this.SpAngleDistance.TabIndex = 89;
            // 
            // labelControl15
            // 
            this.labelControl15.Location = new System.Drawing.Point(39, 147);
            this.labelControl15.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl15.Name = "labelControl15";
            this.labelControl15.Size = new System.Drawing.Size(48, 14);
            this.labelControl15.TabIndex = 88;
            this.labelControl15.Text = "角度间隔";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(403, 210);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(20, 14);
            this.labelControl2.TabIndex = 87;
            this.labelControl2.Text = "mm";
            // 
            // SpSlowTravelDistance
            // 
            this.SpSlowTravelDistance.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.SpSlowTravelDistance.Location = new System.Drawing.Point(135, 205);
            this.SpSlowTravelDistance.Name = "SpSlowTravelDistance";
            this.SpSlowTravelDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowTravelDistance.Size = new System.Drawing.Size(223, 20);
            this.SpSlowTravelDistance.TabIndex = 86;
            // 
            // labelControl13
            // 
            this.labelControl13.Location = new System.Drawing.Point(24, 208);
            this.labelControl13.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl13.Name = "labelControl13";
            this.labelControl13.Size = new System.Drawing.Size(60, 14);
            this.labelControl13.TabIndex = 85;
            this.labelControl13.Text = "二段速距离";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(403, 180);
            this.labelControl12.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(15, 14);
            this.labelControl12.TabIndex = 84;
            this.labelControl12.Text = "ms";
            // 
            // labelControl11
            // 
            this.labelControl11.Location = new System.Drawing.Point(409, 122);
            this.labelControl11.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl11.Name = "labelControl11";
            this.labelControl11.Size = new System.Drawing.Size(7, 14);
            this.labelControl11.TabIndex = 83;
            this.labelControl11.Text = "g";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(409, 95);
            this.labelControl10.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(7, 14);
            this.labelControl10.TabIndex = 82;
            this.labelControl10.Text = "g";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(409, 69);
            this.labelControl9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(7, 14);
            this.labelControl9.TabIndex = 81;
            this.labelControl9.Text = "g";
            // 
            // SpDelay
            // 
            this.SpDelay.EditValue = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.SpDelay.Location = new System.Drawing.Point(135, 177);
            this.SpDelay.Name = "SpDelay";
            this.SpDelay.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpDelay.Size = new System.Drawing.Size(223, 20);
            this.SpDelay.TabIndex = 80;
            // 
            // SpForceSpacing
            // 
            this.SpForceSpacing.EditValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.SpForceSpacing.Location = new System.Drawing.Point(135, 116);
            this.SpForceSpacing.Name = "SpForceSpacing";
            this.SpForceSpacing.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForceSpacing.Size = new System.Drawing.Size(223, 20);
            this.SpForceSpacing.TabIndex = 79;
            // 
            // SpMaxForce
            // 
            this.SpMaxForce.EditValue = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.SpMaxForce.Location = new System.Drawing.Point(135, 93);
            this.SpMaxForce.Name = "SpMaxForce";
            this.SpMaxForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMaxForce.Size = new System.Drawing.Size(223, 20);
            this.SpMaxForce.TabIndex = 78;
            // 
            // SpMinForce
            // 
            this.SpMinForce.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpMinForce.Location = new System.Drawing.Point(135, 66);
            this.SpMinForce.Name = "SpMinForce";
            this.SpMinForce.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMinForce.Size = new System.Drawing.Size(223, 20);
            this.SpMinForce.TabIndex = 77;
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(32, 178);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(48, 14);
            this.labelControl8.TabIndex = 76;
            this.labelControl8.Text = "下压延时";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(39, 122);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(36, 14);
            this.labelControl7.TabIndex = 75;
            this.labelControl7.Text = "力间隔";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(39, 95);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(36, 14);
            this.labelControl6.TabIndex = 74;
            this.labelControl6.Text = "最大力";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(39, 69);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(36, 14);
            this.labelControl4.TabIndex = 73;
            this.labelControl4.Text = "最小力";
            // 
            // labelControl21
            // 
            this.labelControl21.Location = new System.Drawing.Point(409, 39);
            this.labelControl21.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl21.Name = "labelControl21";
            this.labelControl21.Size = new System.Drawing.Size(7, 14);
            this.labelControl21.TabIndex = 72;
            this.labelControl21.Text = "h";
            // 
            // SpEclispTime
            // 
            this.SpEclispTime.EditValue = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.SpEclispTime.Location = new System.Drawing.Point(135, 37);
            this.SpEclispTime.Name = "SpEclispTime";
            this.SpEclispTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpEclispTime.Size = new System.Drawing.Size(223, 20);
            this.SpEclispTime.TabIndex = 70;
            // 
            // labelControl17
            // 
            this.labelControl17.Location = new System.Drawing.Point(32, 39);
            this.labelControl17.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl17.Name = "labelControl17";
            this.labelControl17.Size = new System.Drawing.Size(48, 14);
            this.labelControl17.TabIndex = 71;
            this.labelControl17.Text = "实验时间";
            // 
            // FrmBondheadTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(544, 591);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl6);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmBondheadTest";
            this.Text = "焊头测试";
            this.Load += new System.EventHandler(this.FrmBondheadTest_Load);
            this.Shown += new System.EventHandler(this.FrmBondheadTest_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.groupControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngle3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            this.groupControl6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngleDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowTravelDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpDelay.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpForceSpacing.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMaxForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMinForce.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpEclispTime.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl LbTip;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton BtnStart2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit SpAngle;
        private DevExpress.XtraEditors.SpinEdit SpForce;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton BtnStart3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpAngle3;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SimpleButton BtnStart4;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.SpinEdit SpDelay;
        private DevExpress.XtraEditors.SpinEdit SpForceSpacing;
        private DevExpress.XtraEditors.SpinEdit SpMaxForce;
        private DevExpress.XtraEditors.SpinEdit SpMinForce;
        private DevExpress.XtraEditors.LabelControl labelControl8;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl21;
        private DevExpress.XtraEditors.SpinEdit SpEclispTime;
        private DevExpress.XtraEditors.LabelControl labelControl17;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.LabelControl labelControl11;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.SpinEdit SpSlowTravelDistance;
        private DevExpress.XtraEditors.LabelControl labelControl13;
        private DevExpress.XtraEditors.LabelControl labelControl14;
        private DevExpress.XtraEditors.SpinEdit SpAngleDistance;
        private DevExpress.XtraEditors.LabelControl labelControl15;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}