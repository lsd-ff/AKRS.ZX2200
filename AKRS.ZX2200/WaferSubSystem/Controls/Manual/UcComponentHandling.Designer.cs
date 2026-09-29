namespace AKRS.ZX2200.WaferSubSystem.Controls.Manual
{
    partial class UcComponentHandling
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
            this.components = new System.ComponentModel.Container();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.BtnMoveToWaferClampSafePosition = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToMagazineSafePosition = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToAutoChangePosition = new DevExpress.XtraEditors.SimpleButton();
            this.BtnWaferChangeTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToExpandDownPosition = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToUpDownMarkPosition = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.CeIsActive = new DevExpress.XtraEditors.CheckEdit();
            this.SpMagazineBoxLayer = new DevExpress.XtraEditors.SpinEdit();
            this.LcLayerNo = new DevExpress.XtraEditors.LabelControl();
            this.LcCurrentLayerNo = new DevExpress.XtraEditors.LabelControl();
            this.BtnSlotScan = new DevExpress.XtraEditors.SimpleButton();
            this.BtRemoveWaferFromSlot = new DevExpress.XtraEditors.SimpleButton();
            this.BtnPlaceWaferInSlot = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToPreviousSlot = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToNextSlot = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveWaferMagazineToSlot = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.BtnMagazineFixedCyc = new DevExpress.XtraEditors.SimpleButton();
            this.BtnWaffleVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.BtnStaticWaffleVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.BtnFixedCyc = new DevExpress.XtraEditors.SimpleButton();
            this.BtnInitialization = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBlockCyc = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCloseGripper = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMovePusherOut = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsActive.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMagazineBoxLayer.Properties)).BeginInit();
            this.xtraTabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(567, 408);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.BtnMoveToWaferClampSafePosition);
            this.xtraTabPage1.Controls.Add(this.BtnMoveToMagazineSafePosition);
            this.xtraTabPage1.Controls.Add(this.BtnMoveToAutoChangePosition);
            this.xtraTabPage1.Controls.Add(this.BtnWaferChangeTest);
            this.xtraTabPage1.Controls.Add(this.BtnMoveToExpandDownPosition);
            this.xtraTabPage1.Controls.Add(this.BtnMoveToUpDownMarkPosition);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(565, 382);
            this.xtraTabPage1.Text = "测试/展示";
            // 
            // BtnMoveToWaferClampSafePosition
            // 
            this.BtnMoveToWaferClampSafePosition.Location = new System.Drawing.Point(73, 208);
            this.BtnMoveToWaferClampSafePosition.Name = "BtnMoveToWaferClampSafePosition";
            this.BtnMoveToWaferClampSafePosition.Size = new System.Drawing.Size(246, 30);
            this.BtnMoveToWaferClampSafePosition.TabIndex = 6;
            this.BtnMoveToWaferClampSafePosition.Tag = "Move to WaferClampSafePosition";
            this.BtnMoveToWaferClampSafePosition.Text = "晶圆夹到安全位置";
            this.BtnMoveToWaferClampSafePosition.Click += new System.EventHandler(this.BtnMoveToWaferClampSafePosition_Click);
            // 
            // BtnMoveToMagazineSafePosition
            // 
            this.BtnMoveToMagazineSafePosition.Location = new System.Drawing.Point(73, 160);
            this.BtnMoveToMagazineSafePosition.Name = "BtnMoveToMagazineSafePosition";
            this.BtnMoveToMagazineSafePosition.Size = new System.Drawing.Size(246, 30);
            this.BtnMoveToMagazineSafePosition.TabIndex = 5;
            this.BtnMoveToMagazineSafePosition.Tag = "Move to MagazineSafePosition";
            this.BtnMoveToMagazineSafePosition.Text = "magazine到安全位置";
            this.BtnMoveToMagazineSafePosition.Click += new System.EventHandler(this.BtnMoveToMagazineSafePosition_Click);
            // 
            // BtnMoveToAutoChangePosition
            // 
            this.BtnMoveToAutoChangePosition.Location = new System.Drawing.Point(73, 112);
            this.BtnMoveToAutoChangePosition.Name = "BtnMoveToAutoChangePosition";
            this.BtnMoveToAutoChangePosition.Size = new System.Drawing.Size(246, 30);
            this.BtnMoveToAutoChangePosition.TabIndex = 4;
            this.BtnMoveToAutoChangePosition.Tag = "Move to AutoChangePosition";
            this.BtnMoveToAutoChangePosition.Text = "晶圆台到自动换料位";
            this.BtnMoveToAutoChangePosition.Click += new System.EventHandler(this.BtnMoveToAutoChangePosition_Click);
            // 
            // BtnWaferChangeTest
            // 
            this.BtnWaferChangeTest.Location = new System.Drawing.Point(73, 266);
            this.BtnWaferChangeTest.Name = "BtnWaferChangeTest";
            this.BtnWaferChangeTest.Size = new System.Drawing.Size(246, 30);
            this.BtnWaferChangeTest.TabIndex = 3;
            this.BtnWaferChangeTest.Tag = "Wafer change test-run on";
            this.BtnWaferChangeTest.Text = "自动换晶圆测试";
            this.BtnWaferChangeTest.Click += new System.EventHandler(this.BtnWaferChangeTest_Click);
            // 
            // BtnMoveToExpandDownPosition
            // 
            this.BtnMoveToExpandDownPosition.Location = new System.Drawing.Point(73, 66);
            this.BtnMoveToExpandDownPosition.Name = "BtnMoveToExpandDownPosition";
            this.BtnMoveToExpandDownPosition.Size = new System.Drawing.Size(246, 30);
            this.BtnMoveToExpandDownPosition.TabIndex = 2;
            this.BtnMoveToExpandDownPosition.Tag = "Move to ExpandDownPosition";
            this.BtnMoveToExpandDownPosition.Text = "扩晶环到安全位置";
            this.BtnMoveToExpandDownPosition.Click += new System.EventHandler(this.BtnMoveToExpandDownPosition_Click);
            // 
            // BtnMoveToUpDownMarkPosition
            // 
            this.BtnMoveToUpDownMarkPosition.Location = new System.Drawing.Point(73, 20);
            this.BtnMoveToUpDownMarkPosition.Name = "BtnMoveToUpDownMarkPosition";
            this.BtnMoveToUpDownMarkPosition.Size = new System.Drawing.Size(246, 30);
            this.BtnMoveToUpDownMarkPosition.TabIndex = 2;
            this.BtnMoveToUpDownMarkPosition.Tag = "Move to UpDownMark / Safe Position";
            this.BtnMoveToUpDownMarkPosition.Text = "顶针台到Mark/安全位置";
            this.BtnMoveToUpDownMarkPosition.Click += new System.EventHandler(this.BtnMoveToUpDownMarkPosition_Click);
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.CeIsActive);
            this.xtraTabPage2.Controls.Add(this.SpMagazineBoxLayer);
            this.xtraTabPage2.Controls.Add(this.LcLayerNo);
            this.xtraTabPage2.Controls.Add(this.LcCurrentLayerNo);
            this.xtraTabPage2.Controls.Add(this.BtnSlotScan);
            this.xtraTabPage2.Controls.Add(this.BtRemoveWaferFromSlot);
            this.xtraTabPage2.Controls.Add(this.BtnPlaceWaferInSlot);
            this.xtraTabPage2.Controls.Add(this.BtnMoveToPreviousSlot);
            this.xtraTabPage2.Controls.Add(this.BtnMoveToNextSlot);
            this.xtraTabPage2.Controls.Add(this.BtnMoveWaferMagazineToSlot);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(565, 382);
            this.xtraTabPage2.Text = "晶圆更换";
            // 
            // CeIsActive
            // 
            this.CeIsActive.EditValue = true;
            this.CeIsActive.Enabled = false;
            this.CeIsActive.Location = new System.Drawing.Point(322, 19);
            this.CeIsActive.Name = "CeIsActive";
            this.CeIsActive.Properties.Caption = "";
            this.CeIsActive.Size = new System.Drawing.Size(75, 20);
            this.CeIsActive.TabIndex = 138;
            // 
            // SpMagazineBoxLayer
            // 
            this.SpMagazineBoxLayer.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpMagazineBoxLayer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SpMagazineBoxLayer.Location = new System.Drawing.Point(255, 19);
            this.SpMagazineBoxLayer.Name = "SpMagazineBoxLayer";
            this.SpMagazineBoxLayer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpMagazineBoxLayer.Properties.IsFloatValue = false;
            this.SpMagazineBoxLayer.Properties.MaskSettings.Set("mask", "N00");
            this.SpMagazineBoxLayer.Properties.MaxValue = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.SpMagazineBoxLayer.Properties.MinValue = new decimal(new int[] {
            9999999,
            0,
            0,
            -2147483648});
            this.SpMagazineBoxLayer.Size = new System.Drawing.Size(61, 20);
            this.SpMagazineBoxLayer.TabIndex = 135;
            // 
            // LcLayerNo
            // 
            this.LcLayerNo.Location = new System.Drawing.Point(16, 60);
            this.LcLayerNo.Name = "LcLayerNo";
            this.LcLayerNo.Size = new System.Drawing.Size(15, 14);
            this.LcLayerNo.TabIndex = 137;
            this.LcLayerNo.Text = "No";
            // 
            // LcCurrentLayerNo
            // 
            this.LcCurrentLayerNo.Location = new System.Drawing.Point(16, 22);
            this.LcCurrentLayerNo.Name = "LcCurrentLayerNo";
            this.LcCurrentLayerNo.Size = new System.Drawing.Size(15, 14);
            this.LcCurrentLayerNo.TabIndex = 137;
            this.LcCurrentLayerNo.Text = "No";
            // 
            // BtnSlotScan
            // 
            this.BtnSlotScan.Location = new System.Drawing.Point(49, 244);
            this.BtnSlotScan.Name = "BtnSlotScan";
            this.BtnSlotScan.Size = new System.Drawing.Size(201, 30);
            this.BtnSlotScan.TabIndex = 1;
            this.BtnSlotScan.Tag = "slot scan";
            this.BtnSlotScan.Text = "magazine槽位扫描";
            this.BtnSlotScan.Click += new System.EventHandler(this.BtnSlotScan_Click);
            // 
            // BtRemoveWaferFromSlot
            // 
            this.BtRemoveWaferFromSlot.Location = new System.Drawing.Point(49, 198);
            this.BtRemoveWaferFromSlot.Name = "BtRemoveWaferFromSlot";
            this.BtRemoveWaferFromSlot.Size = new System.Drawing.Size(201, 30);
            this.BtRemoveWaferFromSlot.TabIndex = 2;
            this.BtRemoveWaferFromSlot.Tag = "Remove wafer from slot";
            this.BtRemoveWaferFromSlot.Text = "更换某层料片到晶圆台";
            this.BtRemoveWaferFromSlot.Click += new System.EventHandler(this.BtRemoveWaferFromSlot_Click);
            // 
            // BtnPlaceWaferInSlot
            // 
            this.BtnPlaceWaferInSlot.Location = new System.Drawing.Point(49, 152);
            this.BtnPlaceWaferInSlot.Name = "BtnPlaceWaferInSlot";
            this.BtnPlaceWaferInSlot.Size = new System.Drawing.Size(201, 30);
            this.BtnPlaceWaferInSlot.TabIndex = 3;
            this.BtnPlaceWaferInSlot.Tag = "Place wafer in slot";
            this.BtnPlaceWaferInSlot.Text = "归还料片";
            this.BtnPlaceWaferInSlot.Click += new System.EventHandler(this.BtnPlaceWaferInSlot_Click);
            // 
            // BtnMoveToPreviousSlot
            // 
            this.BtnMoveToPreviousSlot.Location = new System.Drawing.Point(49, 106);
            this.BtnMoveToPreviousSlot.Name = "BtnMoveToPreviousSlot";
            this.BtnMoveToPreviousSlot.Size = new System.Drawing.Size(201, 30);
            this.BtnMoveToPreviousSlot.TabIndex = 4;
            this.BtnMoveToPreviousSlot.Tag = "Move to previous slot";
            this.BtnMoveToPreviousSlot.Text = "magazine到上一个槽位";
            this.BtnMoveToPreviousSlot.Click += new System.EventHandler(this.BtnMoveToPreviousSlot_Click);
            // 
            // BtnMoveToNextSlot
            // 
            this.BtnMoveToNextSlot.Location = new System.Drawing.Point(49, 60);
            this.BtnMoveToNextSlot.Name = "BtnMoveToNextSlot";
            this.BtnMoveToNextSlot.Size = new System.Drawing.Size(201, 30);
            this.BtnMoveToNextSlot.TabIndex = 5;
            this.BtnMoveToNextSlot.Tag = "Move to next slot";
            this.BtnMoveToNextSlot.Text = "magazine到下一个槽位";
            this.BtnMoveToNextSlot.Click += new System.EventHandler(this.BtnMoveToNextSlot_Click);
            // 
            // BtnMoveWaferMagazineToSlot
            // 
            this.BtnMoveWaferMagazineToSlot.Location = new System.Drawing.Point(49, 14);
            this.BtnMoveWaferMagazineToSlot.Name = "BtnMoveWaferMagazineToSlot";
            this.BtnMoveWaferMagazineToSlot.Size = new System.Drawing.Size(201, 30);
            this.BtnMoveWaferMagazineToSlot.TabIndex = 6;
            this.BtnMoveWaferMagazineToSlot.Tag = "Move wafer magazine to slot";
            this.BtnMoveWaferMagazineToSlot.Text = "magazine到某个槽位";
            this.BtnMoveWaferMagazineToSlot.Click += new System.EventHandler(this.BtnMoveWaferMagazineToSlot_Click);
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Controls.Add(this.BtnMagazineFixedCyc);
            this.xtraTabPage3.Controls.Add(this.BtnWaffleVacuum);
            this.xtraTabPage3.Controls.Add(this.BtnStaticWaffleVacuum);
            this.xtraTabPage3.Controls.Add(this.BtnFixedCyc);
            this.xtraTabPage3.Controls.Add(this.BtnInitialization);
            this.xtraTabPage3.Controls.Add(this.BtnBlockCyc);
            this.xtraTabPage3.Controls.Add(this.BtnCloseGripper);
            this.xtraTabPage3.Controls.Add(this.BtnMovePusherOut);
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(565, 382);
            this.xtraTabPage3.Text = "信号";
            // 
            // BtnMagazineFixedCyc
            // 
            this.BtnMagazineFixedCyc.Location = new System.Drawing.Point(274, 76);
            this.BtnMagazineFixedCyc.Name = "BtnMagazineFixedCyc";
            this.BtnMagazineFixedCyc.Size = new System.Drawing.Size(201, 30);
            this.BtnMagazineFixedCyc.TabIndex = 7;
            this.BtnMagazineFixedCyc.Tag = "";
            this.BtnMagazineFixedCyc.Text = "提篮夹紧气缸动作";
            this.BtnMagazineFixedCyc.Click += new System.EventHandler(this.BtnMagazineFixedCyc_Click);
            // 
            // BtnWaffleVacuum
            // 
            this.BtnWaffleVacuum.Location = new System.Drawing.Point(274, 28);
            this.BtnWaffleVacuum.Name = "BtnWaffleVacuum";
            this.BtnWaffleVacuum.Size = new System.Drawing.Size(201, 30);
            this.BtnWaffleVacuum.TabIndex = 6;
            this.BtnWaffleVacuum.Tag = "Close / Open Waffle Vacuum";
            this.BtnWaffleVacuum.Text = "华夫盒真空";
            this.BtnWaffleVacuum.Click += new System.EventHandler(this.BtnWaffleVacuum_Click);
            // 
            // BtnStaticWaffleVacuum
            // 
            this.BtnStaticWaffleVacuum.Location = new System.Drawing.Point(49, 223);
            this.BtnStaticWaffleVacuum.Name = "BtnStaticWaffleVacuum";
            this.BtnStaticWaffleVacuum.Size = new System.Drawing.Size(201, 30);
            this.BtnStaticWaffleVacuum.TabIndex = 5;
            this.BtnStaticWaffleVacuum.Tag = "Close / Open StaticWaffle Vacuum";
            this.BtnStaticWaffleVacuum.Text = "静态华夫盒真空";
            this.BtnStaticWaffleVacuum.Click += new System.EventHandler(this.BtnStaticWaffleVacuum_Click);
            // 
            // BtnFixedCyc
            // 
            this.BtnFixedCyc.Location = new System.Drawing.Point(49, 174);
            this.BtnFixedCyc.Name = "BtnFixedCyc";
            this.BtnFixedCyc.Size = new System.Drawing.Size(201, 30);
            this.BtnFixedCyc.TabIndex = 4;
            this.BtnFixedCyc.Tag = "Close / Open fixedCyc";
            this.BtnFixedCyc.Text = "顶针固定气缸动作";
            this.BtnFixedCyc.Click += new System.EventHandler(this.BtnFixedCyc_Click);
            // 
            // BtnInitialization
            // 
            this.BtnInitialization.Location = new System.Drawing.Point(49, 278);
            this.BtnInitialization.Name = "BtnInitialization";
            this.BtnInitialization.Size = new System.Drawing.Size(201, 30);
            this.BtnInitialization.TabIndex = 1;
            this.BtnInitialization.Tag = "Initialization";
            this.BtnInitialization.Text = "晶圆模块复位";
            this.BtnInitialization.Click += new System.EventHandler(this.BtnInitialization_Click);
            // 
            // BtnBlockCyc
            // 
            this.BtnBlockCyc.Location = new System.Drawing.Point(49, 125);
            this.BtnBlockCyc.Name = "BtnBlockCyc";
            this.BtnBlockCyc.Size = new System.Drawing.Size(201, 30);
            this.BtnBlockCyc.TabIndex = 2;
            this.BtnBlockCyc.Tag = "Close / Open blockCyc";
            this.BtnBlockCyc.Text = "晶圆夹持气缸动作";
            this.BtnBlockCyc.Click += new System.EventHandler(this.BtnBlockCyc_Click);
            // 
            // BtnCloseGripper
            // 
            this.BtnCloseGripper.Location = new System.Drawing.Point(49, 76);
            this.BtnCloseGripper.Name = "BtnCloseGripper";
            this.BtnCloseGripper.Size = new System.Drawing.Size(201, 30);
            this.BtnCloseGripper.TabIndex = 2;
            this.BtnCloseGripper.Tag = "Close / Open gripper";
            this.BtnCloseGripper.Text = "晶圆夹动作";
            this.BtnCloseGripper.Click += new System.EventHandler(this.BtnCloseGripper_Click);
            // 
            // BtnMovePusherOut
            // 
            this.BtnMovePusherOut.Location = new System.Drawing.Point(49, 28);
            this.BtnMovePusherOut.Name = "BtnMovePusherOut";
            this.BtnMovePusherOut.Size = new System.Drawing.Size(201, 30);
            this.BtnMovePusherOut.TabIndex = 3;
            this.BtnMovePusherOut.Tag = "Move pusher out / in";
            this.BtnMovePusherOut.Text = "推杆动作";
            this.BtnMovePusherOut.Click += new System.EventHandler(this.BtnMovePusherOut_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "UcComponentHandling";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // UcComponentHandling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "UcComponentHandling";
            this.Size = new System.Drawing.Size(567, 408);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            this.xtraTabPage2.ResumeLayout(false);
            this.xtraTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsActive.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpMagazineBoxLayer.Properties)).EndInit();
            this.xtraTabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.SpinEdit SpMagazineBoxLayer;
        private DevExpress.XtraEditors.LabelControl LcCurrentLayerNo;
        private DevExpress.XtraEditors.SimpleButton BtnSlotScan;
        private DevExpress.XtraEditors.SimpleButton BtRemoveWaferFromSlot;
        private DevExpress.XtraEditors.SimpleButton BtnPlaceWaferInSlot;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToPreviousSlot;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToNextSlot;
        private DevExpress.XtraEditors.SimpleButton BtnMoveWaferMagazineToSlot;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraEditors.SimpleButton BtnInitialization;
        private DevExpress.XtraEditors.SimpleButton BtnCloseGripper;
        private DevExpress.XtraEditors.SimpleButton BtnMovePusherOut;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton BtnBlockCyc;
        private DevExpress.XtraEditors.CheckEdit CeIsActive;
        private DevExpress.XtraEditors.LabelControl LcLayerNo;
        private DevExpress.XtraEditors.SimpleButton BtnFixedCyc;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToUpDownMarkPosition;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToExpandDownPosition;
        private DevExpress.XtraEditors.SimpleButton BtnWaferChangeTest;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToAutoChangePosition;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToMagazineSafePosition;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToWaferClampSafePosition;
        private DevExpress.XtraEditors.SimpleButton BtnStaticWaffleVacuum;
        private DevExpress.XtraEditors.SimpleButton BtnWaffleVacuum;
        private DevExpress.XtraEditors.SimpleButton BtnMagazineFixedCyc;
    }
}
