namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    partial class UcTransportUnitView
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
            this.PlControl = new DevExpress.XtraEditors.PanelControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.BtAdd = new DevExpress.XtraBars.BarButtonItem();
            this.BtCopy = new DevExpress.XtraBars.BarButtonItem();
            this.BtDisplay = new DevExpress.XtraBars.BarButtonItem();
            this.BtOpen = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.PlControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlControl
            // 
            this.PlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlControl.Location = new System.Drawing.Point(0, 0);
            this.PlControl.Name = "PlControl";
            this.PlControl.Size = new System.Drawing.Size(1074, 646);
            this.PlControl.TabIndex = 1;
            this.PlControl.Click += new System.EventHandler(this.PlControl_Click);
            this.PlControl.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelControl1_Paint);
            this.PlControl.DoubleClick += new System.EventHandler(this.PlControl_DoubleClick);
            this.PlControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlControl_MouseDown);
            this.PlControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlControl_MouseUp);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BtAdd),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtCopy),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtDisplay),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtOpen)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // BtAdd
            // 
            this.BtAdd.Caption = "添加";
            this.BtAdd.Id = 1;
            this.BtAdd.Name = "BtAdd";
            this.BtAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtAdd_ItemClick);
            // 
            // BtCopy
            // 
            this.BtCopy.Caption = "复制";
            this.BtCopy.Id = 0;
            this.BtCopy.Name = "BtCopy";
            this.BtCopy.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtCopy_ItemClick_1);
            // 
            // BtDisplay
            // 
            this.BtDisplay.Caption = "屏蔽";
            this.BtDisplay.Id = 2;
            this.BtDisplay.Name = "BtDisplay";
            this.BtDisplay.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtDisplay_ItemClick);
            // 
            // BtOpen
            // 
            this.BtOpen.Caption = "开启";
            this.BtOpen.Id = 3;
            this.BtOpen.Name = "BtOpen";
            this.BtOpen.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtOpen_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BtCopy,
            this.BtAdd,
            this.BtDisplay,
            this.BtOpen});
            this.barManager1.MaxItemId = 4;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1074, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 646);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1074, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 646);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1074, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 646);
            // 
            // UcTransportUnitView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcTransportUnitView";
            this.Size = new System.Drawing.Size(1074, 646);
            ((System.ComponentModel.ISupportInitialize)(this.PlControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PlControl;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarButtonItem BtCopy;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem BtAdd;
        private DevExpress.XtraBars.BarButtonItem BtDisplay;
        private DevExpress.XtraBars.BarButtonItem BtOpen;
    }
}
