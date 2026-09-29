namespace AKRS.ZX2200.BondSystem.BondForce.Forms.Assistant
{
    partial class FrmCalibrateTeach
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
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCalibrateTeach));
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiMove = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiMeasureHeight = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.PnlDescription = new DevExpress.XtraEditors.PanelControl();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.Pnlbts = new DevExpress.XtraEditors.PanelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.PnlDebugButton = new DevExpress.XtraEditors.PanelControl();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlDescription)).BeginInit();
            this.PnlDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pnlbts)).BeginInit();
            this.Pnlbts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlDebugButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.TileBarTeach);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(654, 115);
            this.panelControl1.TabIndex = 0;
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
            this.TileBarTeach.Groups.Add(this.tileBarGroup4);
            this.TileBarTeach.ItemBackgroundImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.Stretch;
            this.TileBarTeach.ItemImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleLeft;
            this.TileBarTeach.Location = new System.Drawing.Point(2, 20);
            this.TileBarTeach.Margin = new System.Windows.Forms.Padding(0);
            this.TileBarTeach.MaxId = 33;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(650, 93);
            this.TileBarTeach.TabIndex = 10;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiMove);
            this.tileBarGroup4.Items.Add(this.TbiMeasureHeight);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiMove
            // 
            this.TbiMove.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement1.Text = "移动XY";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiMove.Elements.Add(tileItemElement1);
            this.TbiMove.Id = 31;
            this.TbiMove.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiMove.Name = "TbiMove";
            // 
            // TbiMeasureHeight
            // 
            this.TbiMeasureHeight.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image1")));
            tileItemElement2.Text = "测高";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.TbiMeasureHeight.Elements.Add(tileItemElement2);
            this.TbiMeasureHeight.Id = 32;
            this.TbiMeasureHeight.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiMeasureHeight.Name = "TbiMeasureHeight";
            // 
            // PnlDescription
            // 
            this.PnlDescription.Controls.Add(this.LbDescription);
            this.PnlDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlDescription.Location = new System.Drawing.Point(0, 115);
            this.PnlDescription.Name = "PnlDescription";
            this.PnlDescription.Size = new System.Drawing.Size(654, 30);
            this.PnlDescription.TabIndex = 1;
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.Location = new System.Drawing.Point(12, 6);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(63, 16);
            this.LbDescription.TabIndex = 0;
            this.LbDescription.Text = "Description";
            // 
            // Pnlbts
            // 
            this.Pnlbts.Controls.Add(this.tablePanel1);
            this.Pnlbts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnlbts.Location = new System.Drawing.Point(0, 676);
            this.Pnlbts.Name = "Pnlbts";
            this.Pnlbts.Size = new System.Drawing.Size(654, 60);
            this.Pnlbts.TabIndex = 2;
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
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(2, 2);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(650, 56);
            this.tablePanel1.TabIndex = 5;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(521, 3);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(97, 50);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtBack.Location = new System.Drawing.Point(33, 3);
            this.BtBack.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(97, 50);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "回退";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(358, 3);
            this.BtDone.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(97, 50);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "确认";
            this.BtDone.Visible = false;
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(196, 3);
            this.BtNext.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(97, 50);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // PnlDebugButton
            // 
            this.PnlDebugButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlDebugButton.Location = new System.Drawing.Point(0, 666);
            this.PnlDebugButton.Name = "PnlDebugButton";
            this.PnlDebugButton.Size = new System.Drawing.Size(654, 10);
            this.PnlDebugButton.TabIndex = 3;
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 145);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(654, 521);
            this.PnlControl.TabIndex = 4;
            // 
            // FrmCalibrateTeach
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 736);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.PnlDebugButton);
            this.Controls.Add(this.Pnlbts);
            this.Controls.Add(this.PnlDescription);
            this.Controls.Add(this.panelControl1);
            this.Name = "FrmCalibrateTeach";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "位置示教";
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlDescription)).EndInit();
            this.PnlDescription.ResumeLayout(false);
            this.PnlDescription.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Pnlbts)).EndInit();
            this.Pnlbts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlDebugButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl PnlDescription;
        private DevExpress.XtraEditors.PanelControl Pnlbts;
        private DevExpress.XtraEditors.PanelControl PnlDebugButton;
        private DevExpress.XtraEditors.PanelControl PnlControl;
        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiMove;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiMeasureHeight;
    }
}