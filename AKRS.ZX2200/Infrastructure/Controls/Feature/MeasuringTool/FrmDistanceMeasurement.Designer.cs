namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    using AKRS.ZX2200.Infrastructure.Controls.Currency;

    partial class FrmDistanceMeasurement
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDistanceMeasurement));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiPosition1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiPosition2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnObjectToCenter = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
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
            this.TileBarTeach.SelectedItem = this.TbiPosition1;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(748, 109);
            this.TileBarTeach.TabIndex = 14;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiPosition1);
            this.tileBarGroup4.Items.Add(this.TbiPosition2);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiPosition1
            // 
            this.TbiPosition1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "Position  1";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiPosition1.Elements.Add(tileItemElement1);
            this.TbiPosition1.Id = 3;
            this.TbiPosition1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiPosition1.Name = "TbiPosition1";
            // 
            // TbiPosition2
            // 
            this.TbiPosition2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            tileItemElement2.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement2.Text = "Position  2";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiPosition2.Elements.Add(tileItemElement2);
            this.TbiPosition2.Id = 4;
            this.TbiPosition2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiPosition2.Name = "TbiPosition2";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LbDescription);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 109);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(748, 91);
            this.groupControl1.TabIndex = 23;
            // 
            // LbDescription
            // 
            this.LbDescription.Location = new System.Drawing.Point(12, 41);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(81, 18);
            this.LbDescription.TabIndex = 0;
            this.LbDescription.Text = "labelControl1";
            // 
            // ucGuideMove1
            // 
            this.ucGuideMove1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucGuideMove1.Location = new System.Drawing.Point(0, 200);
            this.ucGuideMove1.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.ucGuideMove1.Name = "ucGuideMove1";
            this.ucGuideMove1.Size = new System.Drawing.Size(748, 611);
            this.ucGuideMove1.TabIndex = 24;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnObjectToCenter);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 811);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(748, 79);
            this.panelControl1.TabIndex = 25;
            // 
            // BtnObjectToCenter
            // 
            this.BtnObjectToCenter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnObjectToCenter.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnObjectToCenter.Appearance.Options.UseFont = true;
            this.BtnObjectToCenter.Location = new System.Drawing.Point(300, 9);
            this.BtnObjectToCenter.Margin = new System.Windows.Forms.Padding(2);
            this.BtnObjectToCenter.Name = "BtnObjectToCenter";
            this.BtnObjectToCenter.Size = new System.Drawing.Size(147, 59);
            this.BtnObjectToCenter.TabIndex = 0;
            this.BtnObjectToCenter.Text = "Object  to  center";
            this.BtnObjectToCenter.Click += new System.EventHandler(this.BtnObjectToCenter_Click);
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 890);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(748, 67);
            this.tablePanel1.TabIndex = 26;
            // 
            // BtCancel
            // 
            this.BtCancel.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtCancel.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtCancel, 2);
            this.BtCancel.Location = new System.Drawing.Point(394, 4);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(147, 59);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "Cancel";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.BtBack.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtBack.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Location = new System.Drawing.Point(20, 4);
            this.BtBack.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(147, 59);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "Back";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.BtDone.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtDone.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtDone, 3);
            this.BtDone.Location = new System.Drawing.Point(581, 4);
            this.BtDone.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(147, 59);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "Done";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.BtNext.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtNext.Appearance.Options.UseFont = true;
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Location = new System.Drawing.Point(207, 4);
            this.BtNext.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(147, 59);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "Next";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // FrmDistanceMeasurement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 957);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.ucGuideMove1);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.TileBarTeach);
            this.Name = "FrmDistanceMeasurement";
            this.Text = "FrmDistanceMeasurement";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiPosition1;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiPosition2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private UcGuideMove ucGuideMove1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnObjectToCenter;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
    }
}