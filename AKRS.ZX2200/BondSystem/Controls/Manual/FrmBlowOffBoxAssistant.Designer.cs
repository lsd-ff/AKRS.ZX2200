namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmBlowOffBoxAssistant
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBlowOffBoxAssistant));
            this.TileBar = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiDeterminePosition = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnAutoFocus = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBlow = new DevExpress.XtraEditors.SimpleButton();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).BeginInit();
            this.SuspendLayout();
            // 
            // TileBar
            // 
            this.TileBar.AllowSelectedItem = true;
            this.TileBar.AppearanceItem.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TileBar.AppearanceItem.Normal.BackColor2 = System.Drawing.Color.SeaGreen;
            this.TileBar.AppearanceItem.Normal.Options.UseBackColor = true;
            this.TileBar.AppearanceItem.Selected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.TileBar.AppearanceItem.Selected.BackColor2 = System.Drawing.Color.Sienna;
            this.TileBar.AppearanceItem.Selected.Options.UseBackColor = true;
            this.TileBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.TileBar.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            this.TileBar.Groups.Add(this.tileBarGroup4);
            this.TileBar.ItemBackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            this.TileBar.ItemImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TileBar.Location = new System.Drawing.Point(0, 0);
            this.TileBar.Margin = new System.Windows.Forms.Padding(0);
            this.TileBar.MaxId = 32;
            this.TileBar.Name = "TileBar";
            this.TileBar.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
            this.TileBar.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBar.SelectedItem = this.TbiDeterminePosition;
            this.TileBar.SelectionBorderWidth = 0;
            this.TileBar.ShowItemShadow = true;
            this.TileBar.Size = new System.Drawing.Size(747, 125);
            this.TileBar.TabIndex = 14;
            this.TileBar.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiDeterminePosition);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiDeterminePosition
            // 
            this.TbiDeterminePosition.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "确定位置";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiDeterminePosition.Elements.Add(tileItemElement1);
            this.TbiDeterminePosition.Id = 3;
            this.TbiDeterminePosition.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiDeterminePosition.Name = "TbiDeterminePosition";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LbDescription);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 125);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(747, 72);
            this.groupControl1.TabIndex = 15;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(5, 31);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(738, 18);
            this.LbDescription.TabIndex = 2;
            this.LbDescription.Text = "labelControl1";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtDone);
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 915);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(747, 66);
            this.tablePanel1.TabIndex = 18;
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 0);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(21, 4);
            this.BtDone.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(145, 58);
            this.BtDone.TabIndex = 6;
            this.BtDone.Text = "确认";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(581, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(21, 4, 21, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(145, 58);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnAutoFocus);
            this.panelControl1.Controls.Add(this.BtnBlow);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 815);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(747, 100);
            this.panelControl1.TabIndex = 20;
            // 
            // BtnAutoFocus
            // 
            this.BtnAutoFocus.Location = new System.Drawing.Point(88, 19);
            this.BtnAutoFocus.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnAutoFocus.Name = "BtnAutoFocus";
            this.BtnAutoFocus.Size = new System.Drawing.Size(203, 58);
            this.BtnAutoFocus.TabIndex = 3;
            this.BtnAutoFocus.Text = "自动对焦";
            this.BtnAutoFocus.Click += new System.EventHandler(this.BtnAutoFocus_Click);
            // 
            // BtnBlow
            // 
            this.BtnBlow.Location = new System.Drawing.Point(461, 19);
            this.BtnBlow.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnBlow.Name = "BtnBlow";
            this.BtnBlow.Size = new System.Drawing.Size(203, 58);
            this.BtnBlow.TabIndex = 2;
            this.BtnBlow.Text = "吸嘴真空开/关";
            this.BtnBlow.Click += new System.EventHandler(this.BtnBlow_Click);
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 197);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(747, 618);
            this.PnlControl.TabIndex = 21;
            // 
            // FrmBlowOffBoxAssistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 981);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.TileBar);
            this.Name = "FrmBlowOffBoxAssistant";
            this.Text = "抛料盒位置示教";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmBlowOffBoxAssistant_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBar;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiDeterminePosition;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        //private UcGuideMove ucGuideMove1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnAutoFocus;
        private DevExpress.XtraEditors.SimpleButton BtnBlow;
        private DevExpress.XtraEditors.PanelControl PnlControl;
    }
}