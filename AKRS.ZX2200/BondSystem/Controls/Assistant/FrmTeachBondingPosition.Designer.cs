namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmTeachBondingPosition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTeachBondingPosition));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiSpecifyOrigin1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiSpecifyOrigin2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiSpecifyOrigin3 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiSpecifyOrigin4 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnAutoFocus = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.tileBarItem1 = new DevExpress.XtraBars.Navigation.TileBarItem();
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
            this.TileBarTeach.MaxId = 35;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(747, 98);
            this.TileBarTeach.TabIndex = 14;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin1);
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin2);
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin3);
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin4);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiSpecifyOrigin1
            // 
            this.TbiSpecifyOrigin1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "左上角（中心）";
            this.TbiSpecifyOrigin1.Elements.Add(tileItemElement1);
            this.TbiSpecifyOrigin1.Id = 30;
            this.TbiSpecifyOrigin1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin1.Name = "TbiSpecifyOrigin1";
            // 
            // TbiSpecifyOrigin2
            // 
            this.TbiSpecifyOrigin2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            tileItemElement2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement2.Text = "右上角（右下）";
            this.TbiSpecifyOrigin2.Elements.Add(tileItemElement2);
            this.TbiSpecifyOrigin2.Id = 32;
            this.TbiSpecifyOrigin2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin2.Name = "TbiSpecifyOrigin2";
            // 
            // TbiSpecifyOrigin3
            // 
            this.TbiSpecifyOrigin3.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image2")));
            tileItemElement3.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement3.Text = "右下";
            this.TbiSpecifyOrigin3.Elements.Add(tileItemElement3);
            this.TbiSpecifyOrigin3.Id = 33;
            this.TbiSpecifyOrigin3.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin3.Name = "TbiSpecifyOrigin3";
            // 
            // TbiSpecifyOrigin4
            // 
            this.TbiSpecifyOrigin4.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement4.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image3")));
            tileItemElement4.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement4.Text = "左下";
            this.TbiSpecifyOrigin4.Elements.Add(tileItemElement4);
            this.TbiSpecifyOrigin4.Id = 34;
            this.TbiSpecifyOrigin4.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin4.Name = "TbiSpecifyOrigin4";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LbDescription);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 98);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(747, 90);
            this.groupControl1.TabIndex = 15;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(5, 31);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(731, 18);
            this.LbDescription.TabIndex = 2;
            this.LbDescription.Text = "labelControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnAutoFocus);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 790);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(747, 90);
            this.panelControl1.TabIndex = 18;
            // 
            // BtnAutoFocus
            // 
            this.BtnAutoFocus.Location = new System.Drawing.Point(299, 17);
            this.BtnAutoFocus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnAutoFocus.Name = "BtnAutoFocus";
            this.BtnAutoFocus.Size = new System.Drawing.Size(147, 53);
            this.BtnAutoFocus.TabIndex = 1;
            this.BtnAutoFocus.Text = "自动对焦";
            this.BtnAutoFocus.Click += new System.EventHandler(this.BtnAutoFocus_Click);
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 880);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(747, 62);
            this.tablePanel1.TabIndex = 19;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(581, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(145, 54);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtBack.Location = new System.Drawing.Point(21, 4);
            this.BtBack.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(145, 54);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "回退";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(395, 4);
            this.BtDone.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(145, 54);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "确定";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(208, 4);
            this.BtNext.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(145, 54);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // tileBarItem1
            // 
            this.tileBarItem1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement5.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image4")));
            tileItemElement5.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement5.Text = "Specify origin1";
            this.tileBarItem1.Elements.Add(tileItemElement5);
            this.tileBarItem1.Id = 30;
            this.tileBarItem1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.tileBarItem1.Name = "tileBarItem1";
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 188);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(747, 602);
            this.PnlControl.TabIndex = 20;
            // 
            // FrmTeachBondingPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 942);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.TileBarTeach);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmTeachBondingPosition";
            this.Text = "示教焊点位置";
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
        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        //private UcGuideMove ucGuideMove1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.SimpleButton BtnAutoFocus;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin2;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin3;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin4;
        private DevExpress.XtraBars.Navigation.TileBarItem tileBarItem1;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.XtraEditors.PanelControl PnlControl;
    }
}