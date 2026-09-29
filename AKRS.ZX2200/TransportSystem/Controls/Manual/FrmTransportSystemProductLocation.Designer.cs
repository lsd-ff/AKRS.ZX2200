namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    partial class FrmTransportSystemProductLocation
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
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            this.TileBarProductionLocation = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup2 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TileBarLoadingSubSection = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TileBarDispenseSubSection = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TileBarBondSubSection = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TileBarWaitingUnloadSubSection = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TileBarUnloadingSubSection = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.BtnDispense = new DevExpress.XtraBars.BarButtonItem();
            this.BtnBond = new DevExpress.XtraBars.BarButtonItem();
            this.BtnWaitingUnloading = new DevExpress.XtraBars.BarButtonItem();
            this.BtnDispenseSubsectionTake = new DevExpress.XtraBars.BarButtonItem();
            this.BtnBondSubsectionTake = new DevExpress.XtraBars.BarButtonItem();
            this.BtnWaitUnloadTake = new DevExpress.XtraBars.BarButtonItem();
            this.BtnUnloadTake = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.LbIsEmptyIndex = new DevExpress.XtraEditors.LabelControl();
            this.LbIsEmptyIndexOn = new DevExpress.XtraEditors.LabelControl();
            this.LbSingleStep = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.LbMachineStatus = new DevExpress.XtraEditors.LabelControl();
            this.labelControl8 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // TileBarProductionLocation
            // 
            this.TileBarProductionLocation.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.TileBarProductionLocation.Groups.Add(this.tileBarGroup2);
            this.TileBarProductionLocation.Location = new System.Drawing.Point(66, 60);
            this.TileBarProductionLocation.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TileBarProductionLocation.MaxId = 5;
            this.TileBarProductionLocation.Name = "TileBarProductionLocation";
            this.TileBarProductionLocation.Padding = new System.Windows.Forms.Padding(25, 9, 25, 9);
            this.TileBarProductionLocation.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarProductionLocation.Size = new System.Drawing.Size(1019, 131);
            this.TileBarProductionLocation.TabIndex = 0;
            this.TileBarProductionLocation.Text = "tileBar1";
            // 
            // tileBarGroup2
            // 
            this.tileBarGroup2.Items.Add(this.TileBarLoadingSubSection);
            this.tileBarGroup2.Items.Add(this.TileBarDispenseSubSection);
            this.tileBarGroup2.Items.Add(this.TileBarBondSubSection);
            this.tileBarGroup2.Items.Add(this.TileBarWaitingUnloadSubSection);
            this.tileBarGroup2.Items.Add(this.TileBarUnloadingSubSection);
            this.tileBarGroup2.Name = "tileBarGroup2";
            // 
            // TileBarLoadingSubSection
            // 
            this.TileBarLoadingSubSection.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.Text = "NoMaterial";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TileBarLoadingSubSection.Elements.Add(tileItemElement1);
            this.TileBarLoadingSubSection.Id = 0;
            this.TileBarLoadingSubSection.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TileBarLoadingSubSection.Name = "TileBarLoadingSubSection";
            // 
            // TileBarDispenseSubSection
            // 
            this.TileBarDispenseSubSection.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.Text = "NoMaterial";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TileBarDispenseSubSection.Elements.Add(tileItemElement2);
            this.TileBarDispenseSubSection.Id = 1;
            this.TileBarDispenseSubSection.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TileBarDispenseSubSection.Name = "TileBarDispenseSubSection";
            // 
            // TileBarBondSubSection
            // 
            this.TileBarBondSubSection.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.Text = "NoMaterial";
            tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TileBarBondSubSection.Elements.Add(tileItemElement3);
            this.TileBarBondSubSection.Id = 2;
            this.TileBarBondSubSection.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TileBarBondSubSection.Name = "TileBarBondSubSection";
            // 
            // TileBarWaitingUnloadSubSection
            // 
            this.TileBarWaitingUnloadSubSection.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement4.Text = "NoMaterial";
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TileBarWaitingUnloadSubSection.Elements.Add(tileItemElement4);
            this.TileBarWaitingUnloadSubSection.Id = 3;
            this.TileBarWaitingUnloadSubSection.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TileBarWaitingUnloadSubSection.Name = "TileBarWaitingUnloadSubSection";
            // 
            // TileBarUnloadingSubSection
            // 
            this.TileBarUnloadingSubSection.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement5.Text = "NoMaterial";
            tileItemElement5.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TileBarUnloadingSubSection.Elements.Add(tileItemElement5);
            this.TileBarUnloadingSubSection.Id = 4;
            this.TileBarUnloadingSubSection.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TileBarUnloadingSubSection.Name = "TileBarUnloadingSubSection";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(113, 199);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 18);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "入料载台";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(311, 199);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 18);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "点胶载台";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(519, 199);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(63, 18);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "Bond载台";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(685, 199);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(90, 18);
            this.labelControl4.TabIndex = 4;
            this.labelControl4.Text = "出料等待载台";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(894, 199);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 18);
            this.labelControl5.TabIndex = 5;
            this.labelControl5.Text = "出料载台";
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnDispense),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnBond),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnWaitingUnloading),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnDispenseSubsectionTake),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnBondSubsectionTake),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnWaitUnloadTake),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnUnloadTake)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // BtnDispense
            // 
            this.BtnDispense.Caption = "从点胶载台开始传料";
            this.BtnDispense.Id = 9;
            this.BtnDispense.Name = "BtnDispense";
            // 
            // BtnBond
            // 
            this.BtnBond.Caption = "从Bond载台开始传料";
            this.BtnBond.Id = 5;
            this.BtnBond.Name = "BtnBond";
            // 
            // BtnWaitingUnloading
            // 
            this.BtnWaitingUnloading.Caption = "从下料等待载台开始传料";
            this.BtnWaitingUnloading.Id = 3;
            this.BtnWaitingUnloading.Name = "BtnWaitingUnloading";
            // 
            // BtnDispenseSubsectionTake
            // 
            this.BtnDispenseSubsectionTake.Caption = "从点胶载台拔料";
            this.BtnDispenseSubsectionTake.Id = 10;
            this.BtnDispenseSubsectionTake.Name = "BtnDispenseSubsectionTake";
            // 
            // BtnBondSubsectionTake
            // 
            this.BtnBondSubsectionTake.Caption = "从Bond载台拔料";
            this.BtnBondSubsectionTake.Id = 11;
            this.BtnBondSubsectionTake.Name = "BtnBondSubsectionTake";
            // 
            // BtnWaitUnloadTake
            // 
            this.BtnWaitUnloadTake.Caption = "从下料等待载台拔料";
            this.BtnWaitUnloadTake.Id = 12;
            this.BtnWaitUnloadTake.Name = "BtnWaitUnloadTake";
            // 
            // BtnUnloadTake
            // 
            this.BtnUnloadTake.Caption = "从下料载台拔料";
            this.BtnUnloadTake.Id = 13;
            this.BtnUnloadTake.Name = "BtnUnloadTake";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BtnWaitingUnloading,
            this.BtnBond,
            this.BtnDispense,
            this.BtnDispenseSubsectionTake,
            this.BtnBondSubsectionTake,
            this.BtnWaitUnloadTake,
            this.BtnUnloadTake});
            this.barManager1.MaxItemId = 14;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlTop.Size = new System.Drawing.Size(1189, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 357);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlBottom.Size = new System.Drawing.Size(1189, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 357);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1189, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 357);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tag = "FrmTransport";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // LbIsEmptyIndex
            // 
            this.LbIsEmptyIndex.Location = new System.Drawing.Point(41, 324);
            this.LbIsEmptyIndex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbIsEmptyIndex.Name = "LbIsEmptyIndex";
            this.LbIsEmptyIndex.Size = new System.Drawing.Size(65, 18);
            this.LbIsEmptyIndex.TabIndex = 10;
            this.LbIsEmptyIndex.Text = "持续上料:";
            // 
            // LbIsEmptyIndexOn
            // 
            this.LbIsEmptyIndexOn.Location = new System.Drawing.Point(133, 324);
            this.LbIsEmptyIndexOn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbIsEmptyIndexOn.Name = "LbIsEmptyIndexOn";
            this.LbIsEmptyIndexOn.Size = new System.Drawing.Size(23, 18);
            this.LbIsEmptyIndexOn.TabIndex = 11;
            this.LbIsEmptyIndexOn.Text = "Off";
            // 
            // LbSingleStep
            // 
            this.LbSingleStep.Location = new System.Drawing.Point(133, 298);
            this.LbSingleStep.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbSingleStep.Name = "LbSingleStep";
            this.LbSingleStep.Size = new System.Drawing.Size(23, 18);
            this.LbSingleStep.TabIndex = 17;
            this.LbSingleStep.Text = "Off";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(41, 298);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(35, 18);
            this.labelControl7.TabIndex = 16;
            this.labelControl7.Text = "单步:";
            // 
            // LbMachineStatus
            // 
            this.LbMachineStatus.Location = new System.Drawing.Point(133, 273);
            this.LbMachineStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbMachineStatus.Name = "LbMachineStatus";
            this.LbMachineStatus.Size = new System.Drawing.Size(29, 18);
            this.LbMachineStatus.TabIndex = 23;
            this.LbMachineStatus.Text = "Stop";
            // 
            // labelControl8
            // 
            this.labelControl8.Location = new System.Drawing.Point(41, 273);
            this.labelControl8.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl8.Name = "labelControl8";
            this.labelControl8.Size = new System.Drawing.Size(65, 18);
            this.labelControl8.TabIndex = 22;
            this.labelControl8.Text = "机器状态:";
            // 
            // FrmTransportSystemProductLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1189, 357);
            this.Controls.Add(this.LbMachineStatus);
            this.Controls.Add(this.labelControl8);
            this.Controls.Add(this.LbSingleStep);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.LbIsEmptyIndexOn);
            this.Controls.Add(this.LbIsEmptyIndex);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.TileBarProductionLocation);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmTransportSystemProductLocation";
            this.Text = "流道状态实时显示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTransportSystemProductLocation_FormClosing);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FrmTransportSystemProductLocation_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBarProductionLocation;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup2;
        private DevExpress.XtraBars.Navigation.TileBarItem TileBarLoadingSubSection;
        private DevExpress.XtraBars.Navigation.TileBarItem TileBarDispenseSubSection;
        private DevExpress.XtraBars.Navigation.TileBarItem TileBarBondSubSection;
        private DevExpress.XtraBars.Navigation.TileBarItem TileBarWaitingUnloadSubSection;
        private DevExpress.XtraBars.Navigation.TileBarItem TileBarUnloadingSubSection;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem BtnWaitingUnloading;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem BtnBond;
        private DevExpress.XtraBars.BarButtonItem BtnDispense;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraBars.BarButtonItem BtnDispenseSubsectionTake;
        private DevExpress.XtraBars.BarButtonItem BtnBondSubsectionTake;
        private DevExpress.XtraBars.BarButtonItem BtnWaitUnloadTake;
        private DevExpress.XtraBars.BarButtonItem BtnUnloadTake;
        private DevExpress.XtraEditors.LabelControl LbIsEmptyIndex;
        private DevExpress.XtraEditors.LabelControl LbIsEmptyIndexOn;
        private DevExpress.XtraEditors.LabelControl LbSingleStep;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl LbMachineStatus;
        private DevExpress.XtraEditors.LabelControl labelControl8;
    }
}