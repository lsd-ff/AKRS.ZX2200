namespace AKRS.ZX2200.CalibSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmBondToCameraOffset
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
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBondToCameraOffset));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            this.TbiToolCenter = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.BtnPattern = new DevExpress.XtraEditors.SimpleButton();
            this.tileBarGroup1 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.BtnDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.TbiDispenseCenter = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.tileBarGroup2 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TbiToolCenter
            // 
            this.TbiToolCenter.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.Text = "Tool Center                       2";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiToolCenter.Elements.Add(tileItemElement1);
            this.TbiToolCenter.Id = 33;
            this.TbiToolCenter.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiToolCenter.Name = "TbiToolCenter";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.BtnPattern);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 676);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(706, 49);
            this.panelControl3.TabIndex = 5;
            // 
            // BtnPattern
            // 
            this.BtnPattern.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnPattern.Location = new System.Drawing.Point(2, 2);
            this.BtnPattern.Name = "BtnPattern";
            this.BtnPattern.Size = new System.Drawing.Size(702, 45);
            this.BtnPattern.TabIndex = 1;
            this.BtnPattern.Text = "Search Pattern";
            this.BtnPattern.Click += new System.EventHandler(this.BtnPattern_Click);
            // 
            // tileBarGroup1
            // 
            this.tileBarGroup1.Name = "tileBarGroup1";
            // 
            // BtnDone
            // 
            this.BtnDone.Location = new System.Drawing.Point(490, 13);
            this.BtnDone.Name = "BtnDone";
            this.BtnDone.Size = new System.Drawing.Size(75, 23);
            this.BtnDone.TabIndex = 3;
            this.BtnDone.Text = "Done";
            this.BtnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(595, 13);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 2;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.BtnDone);
            this.panelControl2.Controls.Add(this.BtnCancel);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 729);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(706, 48);
            this.panelControl2.TabIndex = 4;
            // 
            // TbiDispenseCenter
            // 
            this.TbiDispenseCenter.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            tileItemElement2.Text = "Dispense Center                1";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiDispenseCenter.Elements.Add(tileItemElement2);
            this.TbiDispenseCenter.Id = 32;
            this.TbiDispenseCenter.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiDispenseCenter.Name = "TbiDispenseCenter";
            // 
            // tileBarGroup2
            // 
            this.tileBarGroup2.Items.Add(this.TbiDispenseCenter);
            this.tileBarGroup2.Name = "tileBarGroup2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.TileBarTeach);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(702, 105);
            this.panel1.TabIndex = 1;
            // 
            // TileBarTeach
            // 
            this.TileBarTeach.AllowSelectedItem = true;
            this.TileBarTeach.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TileBarTeach.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.SeaGreen;
            this.TileBarTeach.AppearanceItem.Normal.Options.UseBackColor = true;
            this.TileBarTeach.AppearanceItem.Selected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TileBarTeach.AppearanceItem.Selected.BackColor2 = System.Drawing.Color.Sienna;
            this.TileBarTeach.AppearanceItem.Selected.Options.UseBackColor = true;
            this.TileBarTeach.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TileBarTeach.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.TileBarTeach.Groups.Add(this.tileBarGroup2);
            this.TileBarTeach.ItemBackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            this.TileBarTeach.ItemImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TileBarTeach.Location = new System.Drawing.Point(0, 0);
            this.TileBarTeach.Margin = new System.Windows.Forms.Padding(0);
            this.TileBarTeach.MaxId = 49;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(702, 105);
            this.TileBarTeach.TabIndex = 13;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // LbDescription
            // 
            this.LbDescription.Location = new System.Drawing.Point(21, 28);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(70, 14);
            this.LbDescription.TabIndex = 0;
            this.LbDescription.Text = "labelControl1";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.LbDescription);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl4.Location = new System.Drawing.Point(2, 107);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(702, 72);
            this.panelControl4.TabIndex = 2;
            // 
            // ucGuideMove1
            // 
            // 
            // panelControl5
            // 
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl5.Location = new System.Drawing.Point(2, 179);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(702, 495);
            this.panelControl5.TabIndex = 3;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl5);
            this.panelControl1.Controls.Add(this.panelControl4);
            this.panelControl1.Controls.Add(this.panel1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(706, 676);
            this.panelControl1.TabIndex = 3;
            // 
            // FrmBondToCameraOffset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(706, 777);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmBondToCameraOffset";
            this.Text = "FrmBondToCameraOffset";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBarItem TbiToolCenter;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton BtnPattern;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup1;
        private DevExpress.XtraEditors.SimpleButton BtnDone;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiDispenseCenter;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup2;
        private System.Windows.Forms.Panel panel1;
        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}