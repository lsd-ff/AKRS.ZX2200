namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    partial class UcLoaderAndUnloaderManual
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GpLoader = new DevExpress.XtraEditors.GroupControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.LbLoaderCurrentTablet = new DevExpress.XtraEditors.LabelControl();
            this.LbLoaderCurrentBin = new DevExpress.XtraEditors.LabelControl();
            this.BtnloaderPushRod = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderNextBin = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderNextTablet = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderLastBin = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderLastTablet = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderStartFromFirst = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoaderChooseTablet = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.LbUnLoaderCurrentBin = new DevExpress.XtraEditors.LabelControl();
            this.LbUnLoaderCurrentTablet = new DevExpress.XtraEditors.LabelControl();
            this.BtnUnloaderPushRod = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderNextBin = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderLastBin = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderNextTablet = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderLastTablet = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderChooseTablet = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUnLoaderStartFromFirst = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GpLoader)).BeginInit();
            this.GpLoader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpLoader
            // 
            this.GpLoader.Controls.Add(this.tablePanel1);
            this.GpLoader.Dock = System.Windows.Forms.DockStyle.Top;
            this.GpLoader.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpLoader.Location = new System.Drawing.Point(0, 0);
            this.GpLoader.Name = "GpLoader";
            this.GpLoader.Size = new System.Drawing.Size(442, 389);
            this.GpLoader.TabIndex = 0;
            this.GpLoader.Text = "上料仓";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 49.77F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.23F)});
            this.tablePanel1.Controls.Add(this.LbLoaderCurrentTablet);
            this.tablePanel1.Controls.Add(this.LbLoaderCurrentBin);
            this.tablePanel1.Controls.Add(this.BtnloaderPushRod);
            this.tablePanel1.Controls.Add(this.BtnLoaderNextBin);
            this.tablePanel1.Controls.Add(this.BtnLoaderNextTablet);
            this.tablePanel1.Controls.Add(this.BtnLoaderLastBin);
            this.tablePanel1.Controls.Add(this.BtnLoaderLastTablet);
            this.tablePanel1.Controls.Add(this.BtnLoaderStartFromFirst);
            this.tablePanel1.Controls.Add(this.BtnLoaderChooseTablet);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(2, 28);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Size = new System.Drawing.Size(438, 359);
            this.tablePanel1.TabIndex = 0;
            // 
            // LbLoaderCurrentTablet
            // 
            this.tablePanel1.SetColumn(this.LbLoaderCurrentTablet, 1);
            this.LbLoaderCurrentTablet.Location = new System.Drawing.Point(243, 27);
            this.LbLoaderCurrentTablet.Margin = new System.Windows.Forms.Padding(25, 0, 3, 0);
            this.LbLoaderCurrentTablet.Name = "LbLoaderCurrentTablet";
            this.tablePanel1.SetRow(this.LbLoaderCurrentTablet, 0);
            this.LbLoaderCurrentTablet.Size = new System.Drawing.Size(65, 18);
            this.LbLoaderCurrentTablet.TabIndex = 10;
            this.LbLoaderCurrentTablet.Text = "当前料片:";
            // 
            // LbLoaderCurrentBin
            // 
            this.tablePanel1.SetColumn(this.LbLoaderCurrentBin, 0);
            this.LbLoaderCurrentBin.Location = new System.Drawing.Point(25, 27);
            this.LbLoaderCurrentBin.Margin = new System.Windows.Forms.Padding(25, 0, 3, 0);
            this.LbLoaderCurrentBin.Name = "LbLoaderCurrentBin";
            this.tablePanel1.SetRow(this.LbLoaderCurrentBin, 0);
            this.LbLoaderCurrentBin.Size = new System.Drawing.Size(65, 18);
            this.LbLoaderCurrentBin.TabIndex = 9;
            this.LbLoaderCurrentBin.Text = "当前料仓:";
            // 
            // BtnloaderPushRod
            // 
            this.tablePanel1.SetColumn(this.BtnloaderPushRod, 0);
            this.BtnloaderPushRod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnloaderPushRod.Location = new System.Drawing.Point(25, 303);
            this.BtnloaderPushRod.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnloaderPushRod.Name = "BtnloaderPushRod";
            this.tablePanel1.SetRow(this.BtnloaderPushRod, 4);
            this.BtnloaderPushRod.Size = new System.Drawing.Size(168, 41);
            this.BtnloaderPushRod.TabIndex = 6;
            this.BtnloaderPushRod.Text = "推杆伸出/缩回";
            this.BtnloaderPushRod.Click += new System.EventHandler(this.BtnloaderPushRod_Click);
            // 
            // BtnLoaderNextBin
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderNextBin, 1);
            this.BtnLoaderNextBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderNextBin.Location = new System.Drawing.Point(243, 231);
            this.BtnLoaderNextBin.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderNextBin.Name = "BtnLoaderNextBin";
            this.tablePanel1.SetRow(this.BtnLoaderNextBin, 3);
            this.BtnLoaderNextBin.Size = new System.Drawing.Size(170, 42);
            this.BtnLoaderNextBin.TabIndex = 5;
            this.BtnLoaderNextBin.Text = "下一个料盒";
            this.BtnLoaderNextBin.Click += new System.EventHandler(this.BtnNextBin_Click);
            // 
            // BtnLoaderNextTablet
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderNextTablet, 1);
            this.BtnLoaderNextTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderNextTablet.Location = new System.Drawing.Point(243, 159);
            this.BtnLoaderNextTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderNextTablet.Name = "BtnLoaderNextTablet";
            this.tablePanel1.SetRow(this.BtnLoaderNextTablet, 2);
            this.BtnLoaderNextTablet.Size = new System.Drawing.Size(170, 42);
            this.BtnLoaderNextTablet.TabIndex = 4;
            this.BtnLoaderNextTablet.Text = "下一片料";
            this.BtnLoaderNextTablet.Click += new System.EventHandler(this.BtnNextTablet_Click);
            // 
            // BtnLoaderLastBin
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderLastBin, 0);
            this.BtnLoaderLastBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderLastBin.Location = new System.Drawing.Point(25, 231);
            this.BtnLoaderLastBin.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderLastBin.Name = "BtnLoaderLastBin";
            this.tablePanel1.SetRow(this.BtnLoaderLastBin, 3);
            this.BtnLoaderLastBin.Size = new System.Drawing.Size(168, 42);
            this.BtnLoaderLastBin.TabIndex = 3;
            this.BtnLoaderLastBin.Text = "上一个料盒";
            this.BtnLoaderLastBin.Click += new System.EventHandler(this.BtnLastBin_Click);
            // 
            // BtnLoaderLastTablet
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderLastTablet, 0);
            this.BtnLoaderLastTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderLastTablet.Location = new System.Drawing.Point(25, 159);
            this.BtnLoaderLastTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderLastTablet.Name = "BtnLoaderLastTablet";
            this.tablePanel1.SetRow(this.BtnLoaderLastTablet, 2);
            this.BtnLoaderLastTablet.Size = new System.Drawing.Size(168, 42);
            this.BtnLoaderLastTablet.TabIndex = 2;
            this.BtnLoaderLastTablet.Text = "最后一片料";
            this.BtnLoaderLastTablet.Click += new System.EventHandler(this.BtnLastTablet_Click);
            // 
            // BtnLoaderStartFromFirst
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderStartFromFirst, 0);
            this.BtnLoaderStartFromFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderStartFromFirst.Location = new System.Drawing.Point(25, 87);
            this.BtnLoaderStartFromFirst.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderStartFromFirst.Name = "BtnLoaderStartFromFirst";
            this.tablePanel1.SetRow(this.BtnLoaderStartFromFirst, 1);
            this.BtnLoaderStartFromFirst.Size = new System.Drawing.Size(168, 42);
            this.BtnLoaderStartFromFirst.TabIndex = 1;
            this.BtnLoaderStartFromFirst.Text = "从第一片开始";
            this.BtnLoaderStartFromFirst.Click += new System.EventHandler(this.BtnLoadFromFirst_Click);
            // 
            // BtnLoaderChooseTablet
            // 
            this.tablePanel1.SetColumn(this.BtnLoaderChooseTablet, 1);
            this.BtnLoaderChooseTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnLoaderChooseTablet.Location = new System.Drawing.Point(243, 87);
            this.BtnLoaderChooseTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnLoaderChooseTablet.Name = "BtnLoaderChooseTablet";
            this.tablePanel1.SetRow(this.BtnLoaderChooseTablet, 1);
            this.BtnLoaderChooseTablet.Size = new System.Drawing.Size(170, 42);
            this.BtnLoaderChooseTablet.TabIndex = 0;
            this.BtnLoaderChooseTablet.Text = "选择料片";
            this.BtnLoaderChooseTablet.Click += new System.EventHandler(this.BtnChooseTablet_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.tablePanel2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 389);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(442, 389);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "下料仓";
            // 
            // tablePanel2
            // 
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel2.Controls.Add(this.LbUnLoaderCurrentBin);
            this.tablePanel2.Controls.Add(this.LbUnLoaderCurrentTablet);
            this.tablePanel2.Controls.Add(this.BtnUnloaderPushRod);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderNextBin);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderLastBin);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderNextTablet);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderLastTablet);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderChooseTablet);
            this.tablePanel2.Controls.Add(this.BtnUnLoaderStartFromFirst);
            this.tablePanel2.Location = new System.Drawing.Point(2, 45);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel2.Size = new System.Drawing.Size(438, 342);
            this.tablePanel2.TabIndex = 1;
            // 
            // LbUnLoaderCurrentBin
            // 
            this.tablePanel2.SetColumn(this.LbUnLoaderCurrentBin, 0);
            this.LbUnLoaderCurrentBin.Location = new System.Drawing.Point(25, 25);
            this.LbUnLoaderCurrentBin.Margin = new System.Windows.Forms.Padding(25, 0, 3, 0);
            this.LbUnLoaderCurrentBin.Name = "LbUnLoaderCurrentBin";
            this.tablePanel2.SetRow(this.LbUnLoaderCurrentBin, 0);
            this.LbUnLoaderCurrentBin.Size = new System.Drawing.Size(65, 18);
            this.LbUnLoaderCurrentBin.TabIndex = 12;
            this.LbUnLoaderCurrentBin.Text = "当前料仓:";
            // 
            // LbUnLoaderCurrentTablet
            // 
            this.tablePanel2.SetColumn(this.LbUnLoaderCurrentTablet, 1);
            this.LbUnLoaderCurrentTablet.Location = new System.Drawing.Point(244, 25);
            this.LbUnLoaderCurrentTablet.Margin = new System.Windows.Forms.Padding(25, 0, 3, 0);
            this.LbUnLoaderCurrentTablet.Name = "LbUnLoaderCurrentTablet";
            this.tablePanel2.SetRow(this.LbUnLoaderCurrentTablet, 0);
            this.LbUnLoaderCurrentTablet.Size = new System.Drawing.Size(65, 18);
            this.LbUnLoaderCurrentTablet.TabIndex = 11;
            this.LbUnLoaderCurrentTablet.Text = "当前料片:";
            // 
            // BtnUnloaderPushRod
            // 
            this.tablePanel2.SetColumn(this.BtnUnloaderPushRod, 0);
            this.BtnUnloaderPushRod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnloaderPushRod.Location = new System.Drawing.Point(25, 287);
            this.BtnUnloaderPushRod.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnloaderPushRod.Name = "BtnUnloaderPushRod";
            this.tablePanel2.SetRow(this.BtnUnloaderPushRod, 4);
            this.BtnUnloaderPushRod.Size = new System.Drawing.Size(169, 40);
            this.BtnUnloaderPushRod.TabIndex = 7;
            this.BtnUnloaderPushRod.Text = "推杆伸出/缩回";
            this.BtnUnloaderPushRod.Click += new System.EventHandler(this.BtnUnloaderPushRod_Click);
            // 
            // BtnUnLoaderNextBin
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderNextBin, 1);
            this.BtnUnLoaderNextBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderNextBin.Location = new System.Drawing.Point(244, 219);
            this.BtnUnLoaderNextBin.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderNextBin.Name = "BtnUnLoaderNextBin";
            this.tablePanel2.SetRow(this.BtnUnLoaderNextBin, 3);
            this.BtnUnLoaderNextBin.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderNextBin.TabIndex = 9;
            this.BtnUnLoaderNextBin.Text = "下一个料盒";
            this.BtnUnLoaderNextBin.Click += new System.EventHandler(this.BtnUnLoaderNextBin_Click);
            // 
            // BtnUnLoaderLastBin
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderLastBin, 0);
            this.BtnUnLoaderLastBin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderLastBin.Location = new System.Drawing.Point(25, 219);
            this.BtnUnLoaderLastBin.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderLastBin.Name = "BtnUnLoaderLastBin";
            this.tablePanel2.SetRow(this.BtnUnLoaderLastBin, 3);
            this.BtnUnLoaderLastBin.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderLastBin.TabIndex = 8;
            this.BtnUnLoaderLastBin.Text = "上一个料盒";
            this.BtnUnLoaderLastBin.Click += new System.EventHandler(this.BtnUnLoaderLastBin_Click);
            // 
            // BtnUnLoaderNextTablet
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderNextTablet, 1);
            this.BtnUnLoaderNextTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderNextTablet.Location = new System.Drawing.Point(244, 151);
            this.BtnUnLoaderNextTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderNextTablet.Name = "BtnUnLoaderNextTablet";
            this.tablePanel2.SetRow(this.BtnUnLoaderNextTablet, 2);
            this.BtnUnLoaderNextTablet.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderNextTablet.TabIndex = 7;
            this.BtnUnLoaderNextTablet.Text = "下一片料";
            this.BtnUnLoaderNextTablet.Click += new System.EventHandler(this.BtnUnLoaderNextTablet_Click);
            // 
            // BtnUnLoaderLastTablet
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderLastTablet, 0);
            this.BtnUnLoaderLastTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderLastTablet.Location = new System.Drawing.Point(25, 151);
            this.BtnUnLoaderLastTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderLastTablet.Name = "BtnUnLoaderLastTablet";
            this.tablePanel2.SetRow(this.BtnUnLoaderLastTablet, 2);
            this.BtnUnLoaderLastTablet.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderLastTablet.TabIndex = 6;
            this.BtnUnLoaderLastTablet.Text = "最后一片料";
            this.BtnUnLoaderLastTablet.Click += new System.EventHandler(this.BtnUnLoaderLastTablet_Click);
            // 
            // BtnUnLoaderChooseTablet
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderChooseTablet, 1);
            this.BtnUnLoaderChooseTablet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderChooseTablet.Location = new System.Drawing.Point(244, 83);
            this.BtnUnLoaderChooseTablet.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderChooseTablet.Name = "BtnUnLoaderChooseTablet";
            this.tablePanel2.SetRow(this.BtnUnLoaderChooseTablet, 1);
            this.BtnUnLoaderChooseTablet.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderChooseTablet.TabIndex = 6;
            this.BtnUnLoaderChooseTablet.Text = "选择料片";
            this.BtnUnLoaderChooseTablet.Click += new System.EventHandler(this.BtnUnLoaderChooseTablet_Click);
            // 
            // BtnUnLoaderStartFromFirst
            // 
            this.tablePanel2.SetColumn(this.BtnUnLoaderStartFromFirst, 0);
            this.BtnUnLoaderStartFromFirst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnUnLoaderStartFromFirst.Location = new System.Drawing.Point(25, 83);
            this.BtnUnLoaderStartFromFirst.Margin = new System.Windows.Forms.Padding(25, 15, 25, 15);
            this.BtnUnLoaderStartFromFirst.Name = "BtnUnLoaderStartFromFirst";
            this.tablePanel2.SetRow(this.BtnUnLoaderStartFromFirst, 1);
            this.BtnUnLoaderStartFromFirst.Size = new System.Drawing.Size(169, 38);
            this.BtnUnLoaderStartFromFirst.TabIndex = 6;
            this.BtnUnLoaderStartFromFirst.Text = "从第一片开始";
            this.BtnUnLoaderStartFromFirst.Click += new System.EventHandler(this.BtnUnLoaderStartFromFirst_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tag = "UcLoaderManual";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UcLoaderAndUnloaderManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.GpLoader);
            this.Name = "UcLoaderAndUnloaderManual";
            this.Size = new System.Drawing.Size(442, 778);
            ((System.ComponentModel.ISupportInitialize)(this.GpLoader)).EndInit();
            this.GpLoader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            this.tablePanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GpLoader;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderLastTablet;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderChooseTablet;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderStartFromFirst;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderNextBin;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderLastBin;
        private DevExpress.XtraEditors.SimpleButton BtnUnLoaderNextTablet;
        private DevExpress.XtraEditors.SimpleButton BtnUnloaderPushRod;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtnloaderPushRod;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderNextBin;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderNextTablet;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderLastBin;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderLastTablet;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderStartFromFirst;
        private DevExpress.XtraEditors.SimpleButton BtnLoaderChooseTablet;
        private DevExpress.XtraEditors.LabelControl LbUnLoaderCurrentBin;
        private DevExpress.XtraEditors.LabelControl LbUnLoaderCurrentTablet;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.LabelControl LbLoaderCurrentTablet;
        private DevExpress.XtraEditors.LabelControl LbLoaderCurrentBin;
    }
}
