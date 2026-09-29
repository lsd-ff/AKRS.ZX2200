namespace AKRS.ZX2200.BondSystem.Controls.Setting.PostBond
{
    partial class UcAddPostBond
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcAddPostBond));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.GcPostBond = new DevExpress.XtraGrid.GridControl();
            this.GvPostBond = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.BtnManage = new DevExpress.XtraEditors.SimpleButton();
            this.BtnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcPostBond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPostBond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.panelControl2);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.panelControl1);
            this.groupControl1.Controls.Add(this.barDockControlTop);
            this.groupControl1.Controls.Add(this.barDockControl1);
            this.groupControl1.Controls.Add(this.barDockControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(10, 15, 0, 0);
            this.groupControl1.Size = new System.Drawing.Size(1117, 618);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "焊后检测";
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.GcPostBond);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(12, 56);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.panelControl2.Size = new System.Drawing.Size(987, 560);
            this.panelControl2.TabIndex = 11;
            // 
            // GcPostBond
            // 
            this.GcPostBond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcPostBond.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcPostBond.Location = new System.Drawing.Point(0, 15);
            this.GcPostBond.MainView = this.GvPostBond;
            this.GcPostBond.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcPostBond.Name = "GcPostBond";
            this.GcPostBond.Size = new System.Drawing.Size(987, 545);
            this.GcPostBond.TabIndex = 6;
            this.GcPostBond.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvPostBond});
            // 
            // GvPostBond
            // 
            this.GvPostBond.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.name});
            this.GvPostBond.DetailHeight = 272;
            this.GvPostBond.GridControl = this.GcPostBond;
            this.GvPostBond.Name = "GvPostBond";
            this.GvPostBond.OptionsBehavior.ReadOnly = true;
            this.GvPostBond.OptionsDetail.EnableMasterViewMode = false;
            this.GvPostBond.OptionsView.ShowGroupPanel = false;
            this.GvPostBond.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.name, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // name
            // 
            this.name.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.AppearanceCell.Options.UseFont = true;
            this.name.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.AppearanceHeader.Options.UseFont = true;
            this.name.Caption = "焊后检测名称";
            this.name.FieldName = "Name";
            this.name.MinWidth = 22;
            this.name.Name = "name";
            this.name.Visible = true;
            this.name.VisibleIndex = 0;
            this.name.Width = 82;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelControl1.Location = new System.Drawing.Point(12, 38);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(195, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "新建焊后检测然后开始示教。";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.BtnRefresh);
            this.panelControl1.Controls.Add(this.BtnManage);
            this.panelControl1.Controls.Add(this.BtnAdd);
            this.panelControl1.Controls.Add(this.BtnDelete);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(999, 38);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(116, 578);
            this.panelControl1.TabIndex = 10;
            // 
            // BtnRefresh
            // 
            this.BtnRefresh.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F);
            this.BtnRefresh.Appearance.Options.UseFont = true;
            this.BtnRefresh.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnRefresh.ImageOptions.Image")));
            this.BtnRefresh.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnRefresh.Location = new System.Drawing.Point(15, 213);
            this.BtnRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnRefresh.Name = "BtnRefresh";
            this.BtnRefresh.Size = new System.Drawing.Size(91, 34);
            this.BtnRefresh.TabIndex = 4;
            this.BtnRefresh.Text = "刷新";
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // BtnManage
            // 
            this.BtnManage.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnManage.Appearance.Options.UseFont = true;
            this.BtnManage.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnManage.ImageOptions.Image")));
            this.BtnManage.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnManage.Location = new System.Drawing.Point(15, 159);
            this.BtnManage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnManage.Name = "BtnManage";
            this.BtnManage.Size = new System.Drawing.Size(91, 34);
            this.BtnManage.TabIndex = 3;
            this.BtnManage.Text = "管理";
            this.BtnManage.Click += new System.EventHandler(this.BtnManage_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnAdd.ImageOptions.Image")));
            this.BtnAdd.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnAdd.Location = new System.Drawing.Point(15, 44);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(91, 34);
            this.BtnAdd.TabIndex = 1;
            this.BtnAdd.Text = "新建";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnDelete.ImageOptions.Image")));
            this.BtnDelete.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnDelete.Location = new System.Drawing.Point(15, 97);
            this.BtnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(91, 34);
            this.BtnDelete.TabIndex = 2;
            this.BtnDelete.Text = "删除";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(12, 38);
            this.barDockControlTop.Manager = null;
            this.barDockControlTop.Size = new System.Drawing.Size(1103, 0);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(12, 38);
            this.barDockControl1.Manager = null;
            this.barDockControl1.Size = new System.Drawing.Size(1103, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl2.Location = new System.Drawing.Point(12, 38);
            this.barDockControl2.Manager = null;
            this.barDockControl2.Size = new System.Drawing.Size(1103, 0);
            // 
            // UcAddPostBond
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcAddPostBond";
            this.Size = new System.Drawing.Size(1117, 618);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcPostBond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvPostBond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl GcPostBond;
        private DevExpress.XtraGrid.Views.Grid.GridView GvPostBond;
        private DevExpress.XtraGrid.Columns.GridColumn name;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnAdd;
        private DevExpress.XtraEditors.SimpleButton BtnDelete;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraEditors.SimpleButton BtnManage;
        private DevExpress.XtraEditors.SimpleButton BtnRefresh;
    }
}
