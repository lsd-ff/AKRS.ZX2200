namespace AKRS.ZX2200.TransportUnitSystem.Controls.TransportUnitEdit
{
    partial class FrmTransportUnitEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTransportUnitEdit));
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.TreeTransportUnit = new DevExpress.XtraTreeList.TreeList();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.BtClear = new DevExpress.XtraEditors.SimpleButton();
            this.BtCopy = new DevExpress.XtraEditors.SimpleButton();
            this.BtDelete = new DevExpress.XtraEditors.SimpleButton();
            this.BtAdd = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.PlFunction = new DevExpress.XtraTab.XtraTabPage();
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.BtLock = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TreeTransportUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 21.6F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 78.4F)});
            this.tablePanel1.Controls.Add(this.tablePanel2);
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(1241, 637);
            this.tablePanel1.TabIndex = 0;
            // 
            // tablePanel2
            // 
            this.tablePanel1.SetColumn(this.tablePanel2, 0);
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel2.Controls.Add(this.TreeTransportUnit);
            this.tablePanel2.Controls.Add(this.panelControl2);
            this.tablePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel2.Location = new System.Drawing.Point(3, 3);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel1.SetRow(this.tablePanel2, 0);
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 51F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel2.Size = new System.Drawing.Size(262, 631);
            this.tablePanel2.TabIndex = 2;
            // 
            // TreeTransportUnit
            // 
            this.tablePanel2.SetColumn(this.TreeTransportUnit, 0);
            this.TreeTransportUnit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TreeTransportUnit.Location = new System.Drawing.Point(3, 54);
            this.TreeTransportUnit.Name = "TreeTransportUnit";
            this.TreeTransportUnit.OptionsBehavior.Editable = false;
            this.tablePanel2.SetRow(this.TreeTransportUnit, 1);
            this.TreeTransportUnit.Size = new System.Drawing.Size(256, 574);
            this.TreeTransportUnit.TabIndex = 0;
            this.TreeTransportUnit.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.TreeTransportUnit_FocusedNodeChanged);
            // 
            // panelControl2
            // 
            this.tablePanel2.SetColumn(this.panelControl2, 0);
            this.panelControl2.Controls.Add(this.BtLock);
            this.panelControl2.Controls.Add(this.BtClear);
            this.panelControl2.Controls.Add(this.BtCopy);
            this.panelControl2.Controls.Add(this.BtDelete);
            this.panelControl2.Controls.Add(this.BtAdd);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Name = "panelControl2";
            this.tablePanel2.SetRow(this.panelControl2, 0);
            this.panelControl2.Size = new System.Drawing.Size(256, 45);
            this.panelControl2.TabIndex = 1;
            // 
            // BtClear
            // 
            this.BtClear.Location = new System.Drawing.Point(187, 5);
            this.BtClear.Name = "BtClear";
            this.BtClear.Size = new System.Drawing.Size(64, 34);
            this.BtClear.TabIndex = 3;
            this.BtClear.Text = "全部清空";
            this.BtClear.Click += new System.EventHandler(this.BtClear_Click);
            // 
            // BtCopy
            // 
            this.BtCopy.Location = new System.Drawing.Point(136, 6);
            this.BtCopy.Name = "BtCopy";
            this.BtCopy.Size = new System.Drawing.Size(45, 34);
            this.BtCopy.TabIndex = 2;
            this.BtCopy.Text = "复制";
            this.BtCopy.Click += new System.EventHandler(this.BtCopy_Click);
            // 
            // BtDelete
            // 
            this.BtDelete.Location = new System.Drawing.Point(46, 5);
            this.BtDelete.Name = "BtDelete";
            this.BtDelete.Size = new System.Drawing.Size(39, 34);
            this.BtDelete.TabIndex = 1;
            this.BtDelete.Text = "删除";
            this.BtDelete.Click += new System.EventHandler(this.BtDelete_Click);
            // 
            // BtAdd
            // 
            this.BtAdd.Location = new System.Drawing.Point(5, 5);
            this.BtAdd.Name = "BtAdd";
            this.BtAdd.Size = new System.Drawing.Size(35, 34);
            this.BtAdd.TabIndex = 0;
            this.BtAdd.Text = "添加";
            this.BtAdd.Click += new System.EventHandler(this.BtAdd_Click);
            // 
            // panelControl1
            // 
            this.tablePanel1.SetColumn(this.panelControl1, 1);
            this.panelControl1.Controls.Add(this.xtraTabControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(271, 3);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 0);
            this.panelControl1.Size = new System.Drawing.Size(967, 631);
            this.panelControl1.TabIndex = 1;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(963, 627);
            this.xtraTabControl1.TabIndex = 0;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.PlFunction});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(961, 601);
            this.xtraTabPage1.Text = "视图";
            // 
            // PlFunction
            // 
            this.PlFunction.Name = "PlFunction";
            this.PlFunction.Size = new System.Drawing.Size(961, 601);
            this.PlFunction.Text = "功能";
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(0, "checkbox_16x16.png");
            this.imageCollection1.Images.SetKeyName(1, "question_16x16.png");
            // 
            // BtLock
            // 
            this.BtLock.Location = new System.Drawing.Point(91, 6);
            this.BtLock.Name = "BtLock";
            this.BtLock.Size = new System.Drawing.Size(39, 34);
            this.BtLock.TabIndex = 4;
            this.BtLock.Text = "加锁";
            this.BtLock.Click += new System.EventHandler(this.BtLock_Click);
            // 
            // FrmTransportUnitEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmTransportUnitEdit";
            this.Size = new System.Drawing.Size(1241, 637);
            this.Load += new System.EventHandler(this.FrmTransportUnitEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TreeTransportUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraTreeList.TreeList TreeTransportUnit;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage PlFunction;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtClear;
        private DevExpress.XtraEditors.SimpleButton BtCopy;
        private DevExpress.XtraEditors.SimpleButton BtDelete;
        private DevExpress.XtraEditors.SimpleButton BtAdd;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraEditors.SimpleButton BtLock;
    }
}