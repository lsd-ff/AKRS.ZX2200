namespace AKRS.ZX2200.WaferSubSystem.Controls
{
    partial class UcCarrierMapping
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
            this.pMenuCarrier = new DevExpress.XtraBars.PopupMenu(this.components);
            this.BarSubItemState = new DevExpress.XtraBars.BarSubItem();
            this.BtnSeqToOne = new DevExpress.XtraBars.BarButtonItem();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.PnlCarrier = new DevExpress.XtraEditors.PanelControl();
            this.BtnSetStart = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.pMenuCarrier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlCarrier)).BeginInit();
            this.SuspendLayout();
            // 
            // pMenuCarrier
            // 
            this.pMenuCarrier.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BarSubItemState),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnSetStart),
            new DevExpress.XtraBars.LinkPersistInfo(this.BtnSeqToOne)});
            this.pMenuCarrier.Manager = this.barManager1;
            this.pMenuCarrier.Name = "pMenuCarrier";
            // 
            // BarSubItemState
            // 
            this.BarSubItemState.Caption = "StateSet";
            this.BarSubItemState.Id = 0;
            this.BarSubItemState.Name = "BarSubItemState";
            // 
            // BtnSeqToOne
            // 
            this.BtnSeqToOne.Caption = "SeqToOne";
            this.BtnSeqToOne.Id = 4;
            this.BtnSeqToOne.Name = "BtnSeqToOne";
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BarSubItemState,
            this.BtnSeqToOne,
            this.BtnSetStart});
            this.barManager1.MaxItemId = 7;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(2);
            this.barDockControlTop.Size = new System.Drawing.Size(700, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 636);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(2);
            this.barDockControlBottom.Size = new System.Drawing.Size(700, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(2);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 636);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(700, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(2);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 636);
            // 
            // PnlCarrier
            // 
            this.PnlCarrier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlCarrier.Location = new System.Drawing.Point(0, 0);
            this.PnlCarrier.Name = "PnlCarrier";
            this.PnlCarrier.Size = new System.Drawing.Size(700, 636);
            this.PnlCarrier.TabIndex = 4;
            // 
            // BtnSetStart
            // 
            this.BtnSetStart.Caption = "SetStart";
            this.BtnSetStart.Id = 6;
            this.BtnSetStart.Name = "BtnSetStart";
            // 
            // UcCarrierMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PnlCarrier);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UcCarrierMapping";
            this.Size = new System.Drawing.Size(700, 636);
            ((System.ComponentModel.ISupportInitialize)(this.pMenuCarrier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlCarrier)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraBars.PopupMenu pMenuCarrier;
        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem BarSubItemState;
        private DevExpress.XtraEditors.PanelControl PnlCarrier;
        private DevExpress.XtraBars.BarButtonItem BtnSeqToOne;
        private DevExpress.XtraBars.BarButtonItem BtnSetStart;
    }
}