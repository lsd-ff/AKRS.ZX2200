namespace AKRS.ZX2200.BondSystem.Controls.Assistant
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmToolHeightTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmToolHeightTeach));
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiHeightMeasurement = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnVacuum = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtnDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
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
            this.TileBarTeach.MaxId = 34;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(34, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectedItem = this.TbiHeightMeasurement;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(747, 98);
            this.TileBarTeach.TabIndex = 12;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiHeightMeasurement);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiHeightMeasurement
            // 
            this.TbiHeightMeasurement.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "测高";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiHeightMeasurement.Elements.Add(tileItemElement1);
            this.TbiHeightMeasurement.Id = 3;
            this.TbiHeightMeasurement.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiHeightMeasurement.Name = "TbiHeightMeasurement";
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
            this.groupControl1.TabIndex = 13;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(11, 31);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(720, 18);
            this.LbDescription.TabIndex = 2;
            this.LbDescription.Text = "labelControl1";
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnVacuum);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 788);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(747, 95);
            this.panelControl1.TabIndex = 15;
            // 
            // BtnVacuum
            // 
            this.BtnVacuum.Location = new System.Drawing.Point(273, 18);
            this.BtnVacuum.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnVacuum.Name = "BtnVacuum";
            this.BtnVacuum.Size = new System.Drawing.Size(219, 55);
            this.BtnVacuum.TabIndex = 0;
            this.BtnVacuum.Text = "吸嘴真空 开/关";
            this.BtnVacuum.Click += new System.EventHandler(this.BtnVacuum_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtnDone);
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 883);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(747, 63);
            this.tablePanel1.TabIndex = 16;
            // 
            // BtnDone
            // 
            this.tablePanel1.SetColumn(this.BtnDone, 0);
            this.BtnDone.Location = new System.Drawing.Point(30, 4);
            this.BtnDone.Margin = new System.Windows.Forms.Padding(30, 4, 30, 4);
            this.BtnDone.Name = "BtnDone";
            this.tablePanel1.SetRow(this.BtnDone, 0);
            this.BtnDone.Size = new System.Drawing.Size(127, 55);
            this.BtnDone.TabIndex = 7;
            this.BtnDone.Text = "确定";
            this.BtnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(590, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(30, 4, 30, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(127, 55);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 170);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(747, 618);
            this.PnlControl.TabIndex = 17;
            // 
            // FrmToolHeightTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(747, 946);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.TileBarTeach);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmToolHeightTeach";
            this.Text = "吸嘴测高";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmNozzleHeightTeach_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmNozzleHeightTeach_FormClosed);
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
        private DevExpress.XtraBars.Navigation.TileBarItem TbiHeightMeasurement;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        //private UcGuideMove ucGuideMove1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnVacuum;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnDone;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.XtraEditors.PanelControl PnlControl;
    }
}