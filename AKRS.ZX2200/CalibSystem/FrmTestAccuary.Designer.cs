namespace AKRS.ZX2200.CalibSystem
{
    partial class FrmTestAccuary
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.BtnGetA = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGetB = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.BtnStartOnlyA = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStartAngle = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.simpleButton8 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton7 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton6 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton5 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton4 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnGetStart = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnRemoveA = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton9 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton10 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton11 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton12 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton13 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton14 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton15 = new DevExpress.XtraEditors.SimpleButton();
            this.btnABAuto = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton16 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton17 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton18 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTestLight = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.SpBlueLight = new DevExpress.XtraEditors.SpinEdit();
            this.SpGreenLight = new DevExpress.XtraEditors.SpinEdit();
            this.SpRedLight = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.simpleButton19 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton20 = new DevExpress.XtraEditors.SimpleButton();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.btnTestPin = new System.Windows.Forms.Button();
            this.btnTestChangeBond = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlueLight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpGreenLight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRedLight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(887, 56);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "PR";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(80, 186);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(132, 23);
            this.BtnStart.TabIndex = 1;
            this.BtnStart.Text = "Start A->B->A";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(80, 41);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(12, 14);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "A:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(80, 98);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(11, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "B:";
            // 
            // BtnGetA
            // 
            this.BtnGetA.Location = new System.Drawing.Point(105, 37);
            this.BtnGetA.Name = "BtnGetA";
            this.BtnGetA.TabIndex = 6;
            this.BtnGetA.Text = "Add A";
            this.BtnGetA.Click += new System.EventHandler(this.BtnGetA_Click);
            // 
            // BtnGetB
            // 
            this.BtnGetB.Location = new System.Drawing.Point(105, 94);
            this.BtnGetB.Name = "BtnGetB";
            this.BtnGetB.TabIndex = 7;
            this.BtnGetB.Text = "Get B";
            this.BtnGetB.Click += new System.EventHandler(this.BtnGetB_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(105, 137);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.TabIndex = 8;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(55, 139);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(36, 14);
            this.labelControl3.TabIndex = 9;
            this.labelControl3.Text = "Times:";
            // 
            // BtnStartOnlyA
            // 
            this.BtnStartOnlyA.Location = new System.Drawing.Point(80, 249);
            this.BtnStartOnlyA.Name = "BtnStartOnlyA";
            this.BtnStartOnlyA.Size = new System.Drawing.Size(132, 23);
            this.BtnStartOnlyA.TabIndex = 10;
            this.BtnStartOnlyA.Text = "Start OnlyA";
            this.BtnStartOnlyA.Click += new System.EventHandler(this.BtnStartOnlyA_Click);
            // 
            // BtnStartAngle
            // 
            this.BtnStartAngle.Location = new System.Drawing.Point(80, 312);
            this.BtnStartAngle.Name = "BtnStartAngle";
            this.BtnStartAngle.Size = new System.Drawing.Size(132, 23);
            this.BtnStartAngle.TabIndex = 11;
            this.BtnStartAngle.Text = "Start Angle";
            this.BtnStartAngle.Click += new System.EventHandler(this.BtnStartAngle_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton8);
            this.groupControl1.Controls.Add(this.simpleButton7);
            this.groupControl1.Controls.Add(this.simpleButton6);
            this.groupControl1.Controls.Add(this.simpleButton5);
            this.groupControl1.Controls.Add(this.simpleButton4);
            this.groupControl1.Controls.Add(this.simpleButton3);
            this.groupControl1.Controls.Add(this.BtnGetStart);
            this.groupControl1.Location = new System.Drawing.Point(536, 37);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(319, 395);
            this.groupControl1.TabIndex = 14;
            this.groupControl1.Text = "TestCalib";
            // 
            // simpleButton8
            // 
            this.simpleButton8.Location = new System.Drawing.Point(53, 322);
            this.simpleButton8.Name = "simpleButton8";
            this.simpleButton8.Size = new System.Drawing.Size(132, 23);
            this.simpleButton8.TabIndex = 21;
            this.simpleButton8.Text = "TestRotate";
            this.simpleButton8.Click += new System.EventHandler(this.simpleButton8_Click);
            // 
            // simpleButton7
            // 
            this.simpleButton7.Location = new System.Drawing.Point(232, 56);
            this.simpleButton7.Name = "simpleButton7";
            this.simpleButton7.TabIndex = 20;
            this.simpleButton7.Text = "Add A";
            this.simpleButton7.Click += new System.EventHandler(this.simpleButton7_Click);
            // 
            // simpleButton6
            // 
            this.simpleButton6.Location = new System.Drawing.Point(53, 246);
            this.simpleButton6.Name = "simpleButton6";
            this.simpleButton6.Size = new System.Drawing.Size(132, 23);
            this.simpleButton6.TabIndex = 19;
            this.simpleButton6.Text = "UpLook";
            this.simpleButton6.Click += new System.EventHandler(this.simpleButton6_Click);
            // 
            // simpleButton5
            // 
            this.simpleButton5.Location = new System.Drawing.Point(232, 190);
            this.simpleButton5.Name = "simpleButton5";
            this.simpleButton5.TabIndex = 18;
            this.simpleButton5.Text = "Add A";
            this.simpleButton5.Click += new System.EventHandler(this.simpleButton5_Click);
            // 
            // simpleButton4
            // 
            this.simpleButton4.Location = new System.Drawing.Point(53, 190);
            this.simpleButton4.Name = "simpleButton4";
            this.simpleButton4.Size = new System.Drawing.Size(132, 23);
            this.simpleButton4.TabIndex = 17;
            this.simpleButton4.Text = "Wafer";
            this.simpleButton4.Click += new System.EventHandler(this.simpleButton4_Click);
            // 
            // simpleButton3
            // 
            this.simpleButton3.Location = new System.Drawing.Point(53, 128);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(132, 23);
            this.simpleButton3.TabIndex = 16;
            this.simpleButton3.Text = "Bond";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // BtnGetStart
            // 
            this.BtnGetStart.Location = new System.Drawing.Point(53, 56);
            this.BtnGetStart.Name = "BtnGetStart";
            this.BtnGetStart.Size = new System.Drawing.Size(132, 23);
            this.BtnGetStart.TabIndex = 15;
            this.BtnGetStart.Text = "Dispense";
            this.BtnGetStart.Click += new System.EventHandler(this.BtnGetStart_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Location = new System.Drawing.Point(80, 426);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.TabIndex = 15;
            this.simpleButton2.Text = "simpleButton2";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // BtnRemoveA
            // 
            this.BtnRemoveA.Location = new System.Drawing.Point(212, 37);
            this.BtnRemoveA.Name = "BtnRemoveA";
            this.BtnRemoveA.TabIndex = 16;
            this.BtnRemoveA.Text = "Remove A";
            this.BtnRemoveA.Click += new System.EventHandler(this.BtnRemoveA_Click);
            // 
            // simpleButton9
            // 
            this.simpleButton9.Location = new System.Drawing.Point(887, 107);
            this.simpleButton9.Name = "simpleButton9";
            this.simpleButton9.Size = new System.Drawing.Size(93, 23);
            this.simpleButton9.TabIndex = 17;
            this.simpleButton9.Text = "MoveToCenter";
            this.simpleButton9.Click += new System.EventHandler(this.simpleButton9_Click);
            // 
            // simpleButton10
            // 
            this.simpleButton10.Location = new System.Drawing.Point(887, 168);
            this.simpleButton10.Name = "simpleButton10";
            this.simpleButton10.Size = new System.Drawing.Size(93, 23);
            this.simpleButton10.TabIndex = 18;
            this.simpleButton10.Text = "AutoFocusing";
            this.simpleButton10.Click += new System.EventHandler(this.simpleButton10_Click);
            // 
            // simpleButton11
            // 
            this.simpleButton11.Location = new System.Drawing.Point(887, 215);
            this.simpleButton11.Name = "simpleButton11";
            this.simpleButton11.Size = new System.Drawing.Size(93, 23);
            this.simpleButton11.TabIndex = 19;
            this.simpleButton11.Text = "TestPRtime";
            this.simpleButton11.Click += new System.EventHandler(this.simpleButton11_Click);
            // 
            // simpleButton12
            // 
            this.simpleButton12.Location = new System.Drawing.Point(218, 186);
            this.simpleButton12.Name = "simpleButton12";
            this.simpleButton12.Size = new System.Drawing.Size(135, 23);
            this.simpleButton12.TabIndex = 20;
            this.simpleButton12.Text = "Start A->B MutiSpeed";
            this.simpleButton12.Click += new System.EventHandler(this.simpleButton12_Click);
            // 
            // simpleButton13
            // 
            this.simpleButton13.Location = new System.Drawing.Point(232, 249);
            this.simpleButton13.Name = "simpleButton13";
            this.simpleButton13.Size = new System.Drawing.Size(93, 23);
            this.simpleButton13.TabIndex = 21;
            this.simpleButton13.Text = "Putdown";
            this.simpleButton13.Click += new System.EventHandler(this.simpleButton13_Click);
            // 
            // simpleButton14
            // 
            this.simpleButton14.Location = new System.Drawing.Point(872, 364);
            this.simpleButton14.Name = "simpleButton14";
            this.simpleButton14.Size = new System.Drawing.Size(146, 23);
            this.simpleButton14.TabIndex = 22;
            this.simpleButton14.Text = "CreateTransportSystem";
            this.simpleButton14.Click += new System.EventHandler(this.simpleButton14_Click);
            // 
            // simpleButton15
            // 
            this.simpleButton15.Location = new System.Drawing.Point(218, 312);
            this.simpleButton15.Name = "simpleButton15";
            this.simpleButton15.Size = new System.Drawing.Size(135, 23);
            this.simpleButton15.TabIndex = 23;
            this.simpleButton15.Text = "Start A->B";
            this.simpleButton15.Click += new System.EventHandler(this.simpleButton15_Click);
            // 
            // btnABAuto
            // 
            this.btnABAuto.Location = new System.Drawing.Point(536, 471);
            this.btnABAuto.Name = "btnABAuto";
            this.btnABAuto.Size = new System.Drawing.Size(132, 23);
            this.btnABAuto.TabIndex = 24;
            this.btnABAuto.Text = "A->B auto";
            this.btnABAuto.Click += new System.EventHandler(this.btnABAuto_Click);
            // 
            // simpleButton16
            // 
            this.simpleButton16.Location = new System.Drawing.Point(221, 426);
            this.simpleButton16.Name = "simpleButton16";
            this.simpleButton16.Size = new System.Drawing.Size(132, 23);
            this.simpleButton16.TabIndex = 25;
            this.simpleButton16.Text = "TestRotate";
            this.simpleButton16.Click += new System.EventHandler(this.simpleButton16_Click);
            // 
            // simpleButton17
            // 
            this.simpleButton17.Location = new System.Drawing.Point(80, 359);
            this.simpleButton17.Name = "simpleButton17";
            this.simpleButton17.Size = new System.Drawing.Size(132, 23);
            this.simpleButton17.TabIndex = 26;
            this.simpleButton17.Text = "Start BondForceTest";
            this.simpleButton17.Click += new System.EventHandler(this.simpleButton17_Click);
            // 
            // simpleButton18
            // 
            this.simpleButton18.Location = new System.Drawing.Point(251, 359);
            this.simpleButton18.Name = "simpleButton18";
            this.simpleButton18.Size = new System.Drawing.Size(132, 23);
            this.simpleButton18.TabIndex = 27;
            this.simpleButton18.Text = "Start Dispense only a Test";
            this.simpleButton18.Click += new System.EventHandler(this.simpleButton18_Click);
            // 
            // BtnTestLight
            // 
            this.BtnTestLight.Location = new System.Drawing.Point(36, 143);
            this.BtnTestLight.Name = "BtnTestLight";
            this.BtnTestLight.Size = new System.Drawing.Size(132, 23);
            this.BtnTestLight.TabIndex = 28;
            this.BtnTestLight.Text = "TestLightTime";
            this.BtnTestLight.Click += new System.EventHandler(this.BtnTestLightTime_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.SpBlueLight);
            this.groupControl2.Controls.Add(this.SpGreenLight);
            this.groupControl2.Controls.Add(this.SpRedLight);
            this.groupControl2.Controls.Add(this.labelControl6);
            this.groupControl2.Controls.Add(this.labelControl5);
            this.groupControl2.Controls.Add(this.labelControl4);
            this.groupControl2.Controls.Add(this.BtnTestLight);
            this.groupControl2.Location = new System.Drawing.Point(55, 459);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(452, 188);
            this.groupControl2.TabIndex = 29;
            this.groupControl2.Text = "TestCalib";
            // 
            // SpBlueLight
            // 
            this.SpBlueLight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpBlueLight.Location = new System.Drawing.Point(91, 105);
            this.SpBlueLight.Name = "SpBlueLight";
            // 
            // 
            // 
            this.SpBlueLight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpBlueLight.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SpBlueLight.TabIndex = 37;
            // 
            // SpGreenLight
            // 
            this.SpGreenLight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpGreenLight.Location = new System.Drawing.Point(91, 69);
            this.SpGreenLight.Name = "SpGreenLight";
            // 
            // 
            // 
            this.SpGreenLight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpGreenLight.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SpGreenLight.TabIndex = 36;
            // 
            // SpRedLight
            // 
            this.SpRedLight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRedLight.Location = new System.Drawing.Point(91, 34);
            this.SpRedLight.Name = "SpRedLight";
            // 
            // 
            // 
            this.SpRedLight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRedLight.Properties.MaxValue = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.SpRedLight.TabIndex = 35;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(36, 107);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(27, 14);
            this.labelControl6.TabIndex = 34;
            this.labelControl6.Text = "Blue:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(36, 70);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(37, 14);
            this.labelControl5.TabIndex = 32;
            this.labelControl5.Text = "Green:";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(36, 37);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(25, 14);
            this.labelControl4.TabIndex = 30;
            this.labelControl4.Text = "Red:";
            // 
            // simpleButton19
            // 
            this.simpleButton19.Location = new System.Drawing.Point(872, 471);
            this.simpleButton19.Name = "simpleButton19";
            this.simpleButton19.Size = new System.Drawing.Size(146, 23);
            this.simpleButton19.TabIndex = 30;
            this.simpleButton19.Text = "CalibLightness";
            this.simpleButton19.Click += new System.EventHandler(this.simpleButton19_Click);
            // 
            // simpleButton20
            // 
            this.simpleButton20.Location = new System.Drawing.Point(872, 593);
            this.simpleButton20.Name = "simpleButton20";
            this.simpleButton20.Size = new System.Drawing.Size(146, 23);
            this.simpleButton20.TabIndex = 31;
            this.simpleButton20.Text = "TestThreePointAlign";
            this.simpleButton20.Click += new System.EventHandler(this.simpleButton20_Click);
            // 
            // dockManager1
            // 
            this.dockManager1.Form = this;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // btnTestPin
            // 
            this.btnTestPin.Location = new System.Drawing.Point(541, 593);
            this.btnTestPin.Name = "btnTestPin";
            this.btnTestPin.Size = new System.Drawing.Size(136, 23);
            this.btnTestPin.TabIndex = 32;
            this.btnTestPin.Text = "顶针重复性测试";
            this.btnTestPin.UseVisualStyleBackColor = true;
            this.btnTestPin.Click += new System.EventHandler(this.btnTestPin_Click);
            // 
            // btnTestChangeBond
            // 
            this.btnTestChangeBond.Location = new System.Drawing.Point(541, 551);
            this.btnTestChangeBond.Name = "btnTestChangeBond";
            this.btnTestChangeBond.Size = new System.Drawing.Size(136, 23);
            this.btnTestChangeBond.TabIndex = 33;
            this.btnTestChangeBond.Text = "更换焊头重复性测试";
            this.btnTestChangeBond.UseVisualStyleBackColor = true;
            this.btnTestChangeBond.Click += new System.EventHandler(this.btnTestChangeBond_Click);
            // 
            // FrmTestAccuary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 659);
            this.Controls.Add(this.btnTestChangeBond);
            this.Controls.Add(this.btnTestPin);
            this.Controls.Add(this.simpleButton20);
            this.Controls.Add(this.simpleButton19);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.simpleButton18);
            this.Controls.Add(this.simpleButton17);
            this.Controls.Add(this.simpleButton16);
            this.Controls.Add(this.btnABAuto);
            this.Controls.Add(this.simpleButton15);
            this.Controls.Add(this.simpleButton14);
            this.Controls.Add(this.simpleButton13);
            this.Controls.Add(this.simpleButton12);
            this.Controls.Add(this.simpleButton11);
            this.Controls.Add(this.simpleButton10);
            this.Controls.Add(this.simpleButton9);
            this.Controls.Add(this.BtnRemoveA);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.BtnStartAngle);
            this.Controls.Add(this.BtnStartOnlyA);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.BtnGetB);
            this.Controls.Add(this.BtnGetA);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.simpleButton1);
            this.Name = "FrmTestAccuary";
            this.Text = "FrmTestAccuary";
            this.Load += new System.EventHandler(this.FrmTestAccuary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpBlueLight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpGreenLight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRedLight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton BtnGetA;
        private DevExpress.XtraEditors.SimpleButton BtnGetB;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SimpleButton BtnStartOnlyA;
        private DevExpress.XtraEditors.SimpleButton BtnStartAngle;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnGetStart;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton BtnRemoveA;
        private DevExpress.XtraEditors.SimpleButton simpleButton4;
        private DevExpress.XtraEditors.SimpleButton simpleButton5;
        private DevExpress.XtraEditors.SimpleButton simpleButton6;
        private DevExpress.XtraEditors.SimpleButton simpleButton7;
        private DevExpress.XtraEditors.SimpleButton simpleButton8;
        private DevExpress.XtraEditors.SimpleButton simpleButton9;
        private DevExpress.XtraEditors.SimpleButton simpleButton10;
        private DevExpress.XtraEditors.SimpleButton simpleButton11;
        private DevExpress.XtraEditors.SimpleButton simpleButton12;
        private DevExpress.XtraEditors.SimpleButton simpleButton13;
        private DevExpress.XtraEditors.SimpleButton simpleButton14;
        private DevExpress.XtraEditors.SimpleButton simpleButton15;
        private DevExpress.XtraEditors.SimpleButton btnABAuto;
        private DevExpress.XtraEditors.SimpleButton simpleButton16;
        private DevExpress.XtraEditors.SimpleButton simpleButton17;
        private DevExpress.XtraEditors.SimpleButton simpleButton18;
        private DevExpress.XtraEditors.SimpleButton BtnTestLight;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SpinEdit SpBlueLight;
        private DevExpress.XtraEditors.SpinEdit SpGreenLight;
        private DevExpress.XtraEditors.SpinEdit SpRedLight;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton simpleButton19;
        private DevExpress.XtraEditors.SimpleButton simpleButton20;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private System.Windows.Forms.Button btnTestPin;
        private System.Windows.Forms.Button btnTestChangeBond;
    }
}