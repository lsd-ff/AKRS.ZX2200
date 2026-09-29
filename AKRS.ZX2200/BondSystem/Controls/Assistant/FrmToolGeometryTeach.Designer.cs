namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmToolGeometryTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmToolGeometryTeach));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiCenterPoint1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiCenterPoint2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiCenterPoint3 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).BeginInit();
            this.SuspendLayout();
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
            this.TileBarTeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.TileBarTeach.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.TileBarTeach.Groups.Add(this.tileBarGroup4);
            this.TileBarTeach.ItemBackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            this.TileBarTeach.ItemImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TileBarTeach.Location = new System.Drawing.Point(0, 0);
            this.TileBarTeach.Margin = new System.Windows.Forms.Padding(0);
            this.TileBarTeach.MaxId = 32;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectedItem = this.TbiCenterPoint1;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(747, 98);
            this.TileBarTeach.TabIndex = 13;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiCenterPoint1);
            this.tileBarGroup4.Items.Add(this.TbiCenterPoint2);
            this.tileBarGroup4.Items.Add(this.TbiCenterPoint3);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiCenterPoint1
            // 
            this.TbiCenterPoint1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "第一点";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiCenterPoint1.Elements.Add(tileItemElement1);
            this.TbiCenterPoint1.Id = 3;
            this.TbiCenterPoint1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiCenterPoint1.Name = "TbiCenterPoint1";
            // 
            // TbiCenterPoint2
            // 
            this.TbiCenterPoint2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            tileItemElement2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement2.Text = "第二点";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiCenterPoint2.Elements.Add(tileItemElement2);
            this.TbiCenterPoint2.Id = 4;
            this.TbiCenterPoint2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiCenterPoint2.Name = "TbiCenterPoint2";
            // 
            // TbiCenterPoint3
            // 
            this.TbiCenterPoint3.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            tileItemElement3.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement3.Text = "第三点";
            this.TbiCenterPoint3.Elements.Add(tileItemElement3);
            this.TbiCenterPoint3.Id = 30;
            this.TbiCenterPoint3.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiCenterPoint3.Name = "TbiCenterPoint3";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LbDescription);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 98);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(747, 72);
            this.groupControl1.TabIndex = 15;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(11, 31);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(731, 22);
            this.LbDescription.TabIndex = 2;
            this.LbDescription.Text = "labelControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnVacuum);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 786);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(747, 86);
            this.panelControl1.TabIndex = 19;
            // 
            // BtnVacuum
            // 
            this.BtnVacuum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnVacuum.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnVacuum.Appearance.Options.UseFont = true;
            this.BtnVacuum.Location = new System.Drawing.Point(285, 15);
            this.BtnVacuum.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnVacuum.Name = "BtnVacuum";
            this.BtnVacuum.Size = new System.Drawing.Size(184, 50);
            this.BtnVacuum.TabIndex = 0;
            this.BtnVacuum.Text = "吸嘴真空 开/关";
            this.BtnVacuum.Click += new System.EventHandler(this.BtnVacuum_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 1F)});
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Controls.Add(this.BtBack);
            this.tablePanel1.Controls.Add(this.BtDone);
            this.tablePanel1.Controls.Add(this.BtNext);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 872);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(747, 69);
            this.tablePanel1.TabIndex = 22;
            // 
            // BtCancel
            // 
            this.BtCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCancel.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(581, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(145, 61);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.BtBack.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtBack.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtBack.Location = new System.Drawing.Point(21, 4);
            this.BtBack.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(145, 61);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "回退";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.BtDone.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtDone.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(395, 4);
            this.BtDone.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(145, 61);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "确认";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.BtNext.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtNext.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(208, 4);
            this.BtNext.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(145, 61);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 170);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(747, 616);
            this.PnlControl.TabIndex = 23;
            // 
            // FrmToolGeometryTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 941);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.TileBarTeach);
            this.Name = "FrmToolGeometryTeach";
            this.Text = "吸嘴尺寸示教";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmNozzleGeometryTeach_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmNozzleGeometryTeach_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiCenterPoint1;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiCenterPoint2;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiCenterPoint3;
        private DevExpress.XtraEditors.GroupControl groupControl1;        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnVacuum;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.PanelControl PnlControl;
        //private UcGuideMove ucGuideMove2;
    }
}