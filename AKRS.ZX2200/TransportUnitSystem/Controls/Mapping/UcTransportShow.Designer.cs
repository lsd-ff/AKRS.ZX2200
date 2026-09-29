namespace AKRS.ZX2200.TransportUnitSystem.Controls.Mapping
{
    partial class UcTransportShow
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.BtFail = new DevExpress.XtraBars.BarButtonItem();
            this.BtSuccess = new DevExpress.XtraBars.BarButtonItem();
            this.BtNormal = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.PlControl)).BeginInit();
            this.PlControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // PlControl
            // 
            this.PlControl.Controls.Add(this.labelControl1);
            this.PlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlControl.Location = new System.Drawing.Point(0, 0);
            this.PlControl.Name = "PlControl";
            this.PlControl.Size = new System.Drawing.Size(1136, 654);
            this.PlControl.TabIndex = 0;
            this.PlControl.Paint += new System.Windows.Forms.PaintEventHandler(this.PlControl_Paint);
            this.PlControl.DoubleClick += new System.EventHandler(this.PlControl_DoubleClick);
            this.PlControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PlControl_MouseDown);
            this.PlControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PlControl_MouseMove);
            this.PlControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PlControl_MouseUp);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(379, 281);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(350, 42);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "等待载具传入。。。。";
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BtFail),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtSuccess),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtNormal)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // BtFail
            // 
            this.BtFail.Caption = "失败";
            this.BtFail.Id = 0;
            this.BtFail.Name = "BtFail";
            this.BtFail.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtDisable_ItemClick);
            // 
            // BtSuccess
            // 
            this.BtSuccess.Caption = "成功";
            this.BtSuccess.Id = 1;
            this.BtSuccess.Name = "BtSuccess";
            this.BtSuccess.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtSuccess_ItemClick);
            // 
            // BtNormal
            // 
            this.BtNormal.Caption = "正常";
            this.BtNormal.Id = 2;
            this.BtNormal.Name = "BtNormal";
            this.BtNormal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BtNormal_ItemClick);
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BtFail,
            this.BtSuccess,
            this.BtNormal});
            this.barManager1.MaxItemId = 3;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1136, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 654);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1136, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 654);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1136, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 654);
            // 
            // UcTransportShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PlControl);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "UcTransportShow";
            this.Size = new System.Drawing.Size(1136, 654);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UcTransportShow_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.PlControl)).EndInit();
            this.PlControl.ResumeLayout(false);
            this.PlControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl PlControl;
        
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraBars.PopupMenu popupMenu1;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarButtonItem BtFail;
        private DevExpress.XtraBars.BarButtonItem BtSuccess;
        private DevExpress.XtraBars.BarButtonItem BtNormal;
    }
}
