namespace AKRS.ZX2200.SupportFeature.Calibrate
{
    partial class FrmEjectionCaliTeach
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEjectionCaliTeach));
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiEditPr = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtnChangeEjection = new DevExpress.XtraEditors.SimpleButton();
            this.CmbEjectionName = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BtnAutoFocus = new DevExpress.XtraEditors.SimpleButton();
            this.BtnESUpOrDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtnEditPR = new DevExpress.XtraEditors.SimpleButton();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).BeginInit();
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
            this.TileBarTeach.MaxId = 40;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(654, 100);
            this.TileBarTeach.TabIndex = 17;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiEditPr);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiEditPr
            // 
            this.TbiEditPr.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.TileControlImageToTextAlignment.Left;
            tileItemElement1.Text = "编辑模板";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiEditPr.Elements.Add(tileItemElement1);
            this.TbiEditPr.Id = 4;
            this.TbiEditPr.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiEditPr.Name = "TbiEditPr";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.LbDescription);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 100);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(654, 58);
            this.groupControl2.TabIndex = 18;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(4, 24);
            this.LbDescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(646, 14);
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
            this.tablePanel1.Location = new System.Drawing.Point(0, 710);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(654, 52);
            this.tablePanel1.TabIndex = 22;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(509, 3);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(128, 46);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtBack.Location = new System.Drawing.Point(18, 3);
            this.BtBack.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(128, 46);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "回退";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(345, 3);
            this.BtDone.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(128, 46);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "确定";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(182, 3);
            this.BtNext.Margin = new System.Windows.Forms.Padding(18, 3, 18, 3);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(128, 46);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.BtnChangeEjection);
            this.panelControl2.Controls.Add(this.CmbEjectionName);
            this.panelControl2.Controls.Add(this.BtnAutoFocus);
            this.panelControl2.Controls.Add(this.BtnESUpOrDown);
            this.panelControl2.Controls.Add(this.BtnEditPR);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 565);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(654, 145);
            this.panelControl2.TabIndex = 23;
            // 
            // BtnChangeEjection
            // 
            this.BtnChangeEjection.Location = new System.Drawing.Point(430, 22);
            this.BtnChangeEjection.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnChangeEjection.Name = "BtnChangeEjection";
            this.BtnChangeEjection.Size = new System.Drawing.Size(102, 31);
            this.BtnChangeEjection.TabIndex = 44;
            this.BtnChangeEjection.Text = "更换顶针";
            this.BtnChangeEjection.Click += new System.EventHandler(this.BtnChangeEjection_Click);
            // 
            // CmbEjectionName
            // 
            this.CmbEjectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbEjectionName.Location = new System.Drawing.Point(96, 28);
            this.CmbEjectionName.Name = "CmbEjectionName";
            this.CmbEjectionName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbEjectionName.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbEjectionName.Size = new System.Drawing.Size(250, 20);
            this.CmbEjectionName.TabIndex = 43;
            // 
            // BtnAutoFocus
            // 
            this.BtnAutoFocus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAutoFocus.Location = new System.Drawing.Point(262, 80);
            this.BtnAutoFocus.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAutoFocus.Name = "BtnAutoFocus";
            this.BtnAutoFocus.Size = new System.Drawing.Size(129, 46);
            this.BtnAutoFocus.TabIndex = 2;
            this.BtnAutoFocus.Text = "相机自动对焦";
            this.BtnAutoFocus.Click += new System.EventHandler(this.BtnAutoFocus_Click);
            // 
            // BtnESUpOrDown
            // 
            this.BtnESUpOrDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnESUpOrDown.Location = new System.Drawing.Point(96, 79);
            this.BtnESUpOrDown.Margin = new System.Windows.Forms.Padding(2);
            this.BtnESUpOrDown.Name = "BtnESUpOrDown";
            this.BtnESUpOrDown.Size = new System.Drawing.Size(129, 46);
            this.BtnESUpOrDown.TabIndex = 1;
            this.BtnESUpOrDown.Text = "顶针系统 上 / 下";
            this.BtnESUpOrDown.Click += new System.EventHandler(this.BtnESUpOrDown_Click);
            // 
            // BtnEditPR
            // 
            this.BtnEditPR.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnEditPR.Location = new System.Drawing.Point(422, 79);
            this.BtnEditPR.Margin = new System.Windows.Forms.Padding(2);
            this.BtnEditPR.Name = "BtnEditPR";
            this.BtnEditPR.Size = new System.Drawing.Size(129, 46);
            this.BtnEditPR.TabIndex = 0;
            this.BtnEditPR.Text = "编辑视觉模板";
            this.BtnEditPR.Click += new System.EventHandler(this.BtnEditPR_Click);
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 158);
            this.PnlControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(654, 407);
            this.PnlControl.TabIndex = 24;
            // 
            // FrmEjectionCaliTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 762);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.TileBarTeach);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmEjectionCaliTeach";
            this.Text = "顶针校准示教";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmEjectionCaliTeach_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbEjectionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiEditPr;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtnEditPR;
        private DevExpress.XtraEditors.SimpleButton BtnESUpOrDown;
        private DevExpress.XtraEditors.PanelControl PnlControl;
        private DevExpress.XtraEditors.SimpleButton BtnAutoFocus;
        private DevExpress.XtraEditors.ComboBoxEdit CmbEjectionName;
        private DevExpress.XtraEditors.SimpleButton BtnChangeEjection;
    }
}