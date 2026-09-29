

namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    partial class FrmAssistantHeightPoints
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
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            this.PnlDebugButton = new DevExpress.XtraEditors.PanelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.PnlDescription = new DevExpress.XtraEditors.PanelControl();
            this.TbiRotaryPoint2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiRotaryPoint1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.tileBarGroup4 = new DevExpress.XtraBars.Navigation.TileBarGroup();
            this.TbiHeightMeasurement = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiSpecifyOrigin1 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TbiSpecifyOrigin2 = new DevExpress.XtraBars.Navigation.TileBarItem();
            this.TileBarTeach = new DevExpress.XtraBars.Navigation.TileBar();
            this.PnlControl = new DevExpress.XtraEditors.PanelControl();
            this.Pnlbts = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.PnlDebugButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlDescription)).BeginInit();
            this.PnlDescription.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pnlbts)).BeginInit();
            this.Pnlbts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlDebugButton
            // 
            this.PnlDebugButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlDebugButton.Location = new System.Drawing.Point(0, 669);
            this.PnlDebugButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlDebugButton.Name = "PnlDebugButton";
            this.PnlDebugButton.Size = new System.Drawing.Size(698, 91);
            this.PnlDebugButton.TabIndex = 8;
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
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(694, 45);
            this.tablePanel1.TabIndex = 0;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 3);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(524, 3);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(168, 39);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "Cancel";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtBack
            // 
            this.tablePanel1.SetColumn(this.BtBack, 0);
            this.BtBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtBack.Location = new System.Drawing.Point(3, 3);
            this.BtBack.Name = "BtBack";
            this.tablePanel1.SetRow(this.BtBack, 0);
            this.BtBack.Size = new System.Drawing.Size(168, 39);
            this.BtBack.TabIndex = 0;
            this.BtBack.Text = "Back";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtDone
            // 
            this.tablePanel1.SetColumn(this.BtDone, 2);
            this.BtDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtDone.Location = new System.Drawing.Point(350, 3);
            this.BtDone.Name = "BtDone";
            this.tablePanel1.SetRow(this.BtDone, 0);
            this.BtDone.Size = new System.Drawing.Size(168, 39);
            this.BtDone.TabIndex = 3;
            this.BtDone.Text = "Done";
            this.BtDone.Click += new System.EventHandler(this.BtDone_Click);
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 1);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(177, 3);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(168, 39);
            this.BtNext.TabIndex = 1;
            this.BtNext.Text = "Next";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // LbDescription
            // 
            this.LbDescription.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LbDescription.Appearance.Options.UseFont = true;
            this.LbDescription.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.Vertical;
            this.LbDescription.Location = new System.Drawing.Point(30, 19);
            this.LbDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(682, 0);
            this.LbDescription.TabIndex = 0;
            // 
            // PnlDescription
            // 
            this.PnlDescription.Controls.Add(this.LbDescription);
            this.PnlDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlDescription.Location = new System.Drawing.Point(0, 115);
            this.PnlDescription.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlDescription.Name = "PnlDescription";
            this.PnlDescription.Size = new System.Drawing.Size(698, 89);
            this.PnlDescription.TabIndex = 6;
            // 
            // TbiRotaryPoint2
            // 
            this.TbiRotaryPoint2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement1.Text = "height measurement 2";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiRotaryPoint2.Elements.Add(tileItemElement1);
            this.TbiRotaryPoint2.Id = 32;
            this.TbiRotaryPoint2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiRotaryPoint2.Name = "TbiRotaryPoint2";
            // 
            // TbiRotaryPoint1
            // 
            this.TbiRotaryPoint1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement2.Text = "height measurement 1";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiRotaryPoint1.Elements.Add(tileItemElement2);
            this.TbiRotaryPoint1.Id = 33;
            this.TbiRotaryPoint1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiRotaryPoint1.Name = "TbiRotaryPoint1";
            // 
            // tileBarGroup4
            // 
            this.tileBarGroup4.Items.Add(this.TbiRotaryPoint1);
            this.tileBarGroup4.Items.Add(this.TbiRotaryPoint2);
            this.tileBarGroup4.Items.Add(this.TbiHeightMeasurement);
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin1);
            this.tileBarGroup4.Items.Add(this.TbiSpecifyOrigin2);
            this.tileBarGroup4.Name = "tileBarGroup4";
            // 
            // TbiHeightMeasurement
            // 
            this.TbiHeightMeasurement.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement3.Text = "height measurement 3";
            tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiHeightMeasurement.Elements.Add(tileItemElement3);
            this.TbiHeightMeasurement.Id = 3;
            this.TbiHeightMeasurement.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiHeightMeasurement.Name = "TbiHeightMeasurement";
            // 
            // TbiSpecifyOrigin1
            // 
            this.TbiSpecifyOrigin1.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement4.Text = "height measurement 4";
            tileItemElement4.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiSpecifyOrigin1.Elements.Add(tileItemElement4);
            this.TbiSpecifyOrigin1.Id = 4;
            this.TbiSpecifyOrigin1.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin1.Name = "TbiSpecifyOrigin1";
            // 
            // TbiSpecifyOrigin2
            // 
            this.TbiSpecifyOrigin2.DropDownOptions.BeakColor = System.Drawing.Color.Empty;
            tileItemElement5.Text = "height measurement 5";
            tileItemElement5.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleRight;
            this.TbiSpecifyOrigin2.Elements.Add(tileItemElement5);
            this.TbiSpecifyOrigin2.Id = 30;
            this.TbiSpecifyOrigin2.ItemSize = DevExpress.XtraBars.Navigation.TileBarItemSize.Wide;
            this.TbiSpecifyOrigin2.Name = "TbiSpecifyOrigin2";
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
            this.TileBarTeach.Location = new System.Drawing.Point(2, 21);
            this.TileBarTeach.Margin = new System.Windows.Forms.Padding(0);
            this.TileBarTeach.MaxId = 34;
            this.TileBarTeach.Name = "TileBarTeach";
            this.TileBarTeach.Padding = new System.Windows.Forms.Padding(30, 0, 0, 0);
            this.TileBarTeach.Position = 182;
            this.TileBarTeach.ScrollMode = DevExpress.XtraEditors.TileControlScrollMode.ScrollButtons;
            this.TileBarTeach.SelectedItem = this.TbiHeightMeasurement;
            this.TileBarTeach.SelectionBorderWidth = 0;
            this.TileBarTeach.ShowItemShadow = true;
            this.TileBarTeach.Size = new System.Drawing.Size(694, 92);
            this.TileBarTeach.TabIndex = 10;
            this.TileBarTeach.Text = "tileBar1";
            // 
            // PnlControl
            // 
            this.PnlControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlControl.Location = new System.Drawing.Point(0, 115);
            this.PnlControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.PnlControl.Name = "PnlControl";
            this.PnlControl.Size = new System.Drawing.Size(698, 645);
            this.PnlControl.TabIndex = 9;
            // 
            // Pnlbts
            // 
            this.Pnlbts.Controls.Add(this.tablePanel1);
            this.Pnlbts.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Pnlbts.Location = new System.Drawing.Point(0, 760);
            this.Pnlbts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Pnlbts.Name = "Pnlbts";
            this.Pnlbts.Size = new System.Drawing.Size(698, 49);
            this.Pnlbts.TabIndex = 7;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.TileBarTeach);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(698, 115);
            this.panelControl1.TabIndex = 5;
            // 
            // FrmAssistantHeightPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 809);
            this.Controls.Add(this.PnlDebugButton);
            this.Controls.Add(this.PnlDescription);
            this.Controls.Add(this.PnlControl);
            this.Controls.Add(this.Pnlbts);
            this.Controls.Add(this.panelControl1);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "FrmAssistantHeightPoints";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmSubstrateHeight";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmAssistantHeightPoints_FormClosed);
            this.Load += new System.EventHandler(this.FrmAssistantHeightPoints_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PnlDebugButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlDescription)).EndInit();
            this.PnlDescription.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PnlControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Pnlbts)).EndInit();
            this.Pnlbts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin2;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiHeightMeasurement;
        private DevExpress.XtraEditors.PanelControl PnlDebugButton;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiSpecifyOrigin1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtDone;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.XtraEditors.PanelControl PnlDescription;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiRotaryPoint2;
        private DevExpress.XtraBars.Navigation.TileBarItem TbiRotaryPoint1;
        private DevExpress.XtraBars.Navigation.TileBarGroup tileBarGroup4;
        private DevExpress.XtraBars.Navigation.TileBar TileBarTeach;
        private DevExpress.XtraEditors.PanelControl PnlControl;
        private DevExpress.XtraEditors.PanelControl Pnlbts;
        private DevExpress.XtraEditors.PanelControl panelControl1;
    }
}