namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmTeachULMPositions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTeachULMPositions));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement6 = new DevExpress.XtraEditors.TileItemElement();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiTransportUnitEdgePos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiAboveRingLightPos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiWaferRingLightLeftLimitPos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiWaferRingLightRightLimitPos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiAboveIPTRightPos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiRightIPTLeftPos = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
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
            this.TileBarTeach.MaxId = 43;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(839, 117);
            this.TileBarTeach.TabIndex = 16;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiTransportUnitEdgePos);
            this.tileBarGroup4.Items.Add(this.TbiAboveRingLightPos);
            this.tileBarGroup4.Items.Add(this.TbiWaferRingLightLeftLimitPos);
            this.tileBarGroup4.Items.Add(this.TbiWaferRingLightRightLimitPos);
            this.tileBarGroup4.Items.Add(this.TbiAboveIPTRightPos);
            this.tileBarGroup4.Items.Add(this.TbiRightIPTLeftPos);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiTransportUnitEdgePos
            // 
            this.TbiTransportUnitEdgePos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage")));
            tileItemElement1.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement1.Text = "流道外延";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiTransportUnitEdgePos.Elements.Add(tileItemElement1);
            this.TbiTransportUnitEdgePos.Id = 4;
            this.TbiTransportUnitEdgePos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiTransportUnitEdgePos.Name = "TbiTransportUnitEdgePos";
            // 
            // TbiAboveRingLightPos
            // 
            this.TbiAboveRingLightPos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement2.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage1")));
            tileItemElement2.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement2.Text = "晶圆环光正上方";
            this.TbiAboveRingLightPos.Elements.Add(tileItemElement2);
            this.TbiAboveRingLightPos.Id = 39;
            this.TbiAboveRingLightPos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiAboveRingLightPos.Name = "TbiAboveRingLightPos";
            // 
            // TbiWaferRingLightLeftLimitPos
            // 
            this.TbiWaferRingLightLeftLimitPos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement3.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage2")));
            tileItemElement3.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement3.Text = "晶圆环光左极限";
            this.TbiWaferRingLightLeftLimitPos.Elements.Add(tileItemElement3);
            this.TbiWaferRingLightLeftLimitPos.Id = 41;
            this.TbiWaferRingLightLeftLimitPos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiWaferRingLightLeftLimitPos.Name = "TbiWaferRingLightLeftLimitPos";
            // 
            // TbiWaferRingLightRightLimitPos
            // 
            this.TbiWaferRingLightRightLimitPos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement4.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            tileItemElement4.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement4.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage3")));
            tileItemElement4.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement4.Text = "晶圆环光右极限";
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiWaferRingLightRightLimitPos.Elements.Add(tileItemElement4);
            this.TbiWaferRingLightRightLimitPos.Id = 40;
            this.TbiWaferRingLightRightLimitPos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiWaferRingLightRightLimitPos.Name = "TbiWaferRingLightRightLimitPos";
            // 
            // TbiAboveIPTRightPos
            // 
            this.TbiAboveIPTRightPos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement5.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement5.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage4")));
            tileItemElement5.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement5.Text = "中转台右上方/翻转台正上方";
            this.TbiAboveIPTRightPos.Elements.Add(tileItemElement5);
            this.TbiAboveIPTRightPos.Id = 36;
            this.TbiAboveIPTRightPos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiAboveIPTRightPos.Name = "TbiAboveIPTRightPos";
            // 
            // TbiRightIPTLeftPos
            // 
            this.TbiRightIPTLeftPos.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement6.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement6.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("resource.SvgImage5")));
            tileItemElement6.ImageOptions.SvgImageSize = new System.Drawing.Size(24, 24);
            tileItemElement6.Text = "右中转台左上角";
            this.TbiRightIPTLeftPos.Elements.Add(tileItemElement6);
            this.TbiRightIPTLeftPos.Id = 42;
            this.TbiRightIPTLeftPos.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiRightIPTLeftPos.Name = "TbiRightIPTLeftPos";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.LbDescription);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 117);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(839, 69);
            this.groupControl2.TabIndex = 18;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(5, 31);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(738, 18);
            this.LbDescription.TabIndex = 3;
            this.LbDescription.Text = "labelControl1";
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 903);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(839, 67);
            this.tablePanel1.TabIndex = 21;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(650, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(168, 59);
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
            this.BtBack.Size = new System.Drawing.Size(168, 59);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "回退";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(441, 4);
            this.BtDone.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(168, 59);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "确定";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(231, 4);
            this.BtNext.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(168, 59);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 186);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(839, 717);
            this.PnlControl.TabIndex = 22;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmTeachULMPositions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 970);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.TileBarTeach);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmTeachULMPositions";
            this.Text = "运动点位示教";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmTeachULMPositions_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiTransportUnitEdgePos;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiAboveIPTRightPos;
        //private UcGuideMove ucGuideMove1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiAboveRingLightPos;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiWaferRingLightRightLimitPos;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiWaferRingLightLeftLimitPos;
        private DevExpress.XtraEditors.PanelControl PnlControl;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiRightIPTLeftPos;
        private System.Windows.Forms.Timer timer1;
    }
}