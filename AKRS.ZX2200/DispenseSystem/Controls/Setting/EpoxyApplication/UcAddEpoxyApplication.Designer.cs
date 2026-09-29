namespace AKRS.ZX2200.DispenseSystem.Controls.Setting.EpoxyApplication
{
    partial class UcAddEpoxyApplication
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcAddEpoxyApplication));
            this.GpDispenserApplication = new DevExpress.XtraEditors.GroupControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.GcEpoxyApplication = new DevExpress.XtraGrid.GridControl();
            this.GvEpoxyApplication = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtEpoxyApplicationRepository = new DevExpress.XtraEditors.SimpleButton();
            this.BtnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.GpDispenserApplication)).BeginInit();
            this.GpDispenserApplication.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcEpoxyApplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEpoxyApplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GpDispenserApplication
            // 
            this.GpDispenserApplication.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GpDispenserApplication.AppearanceCaption.Options.UseFont = true;
            this.GpDispenserApplication.Controls.Add(this.panelControl2);
            this.GpDispenserApplication.Controls.Add(this.labelControl1);
            this.GpDispenserApplication.Controls.Add(this.panelControl1);
            this.GpDispenserApplication.Controls.Add(this.barDockControlTop);
            this.GpDispenserApplication.Controls.Add(this.barDockControl1);
            this.GpDispenserApplication.Controls.Add(this.barDockControl2);
            this.GpDispenserApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GpDispenserApplication.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpDispenserApplication.Location = new System.Drawing.Point(0, 0);
            this.GpDispenserApplication.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GpDispenserApplication.Name = "GpDispenserApplication";
            this.GpDispenserApplication.Padding = new System.Windows.Forms.Padding(10, 15, 0, 0);
            this.GpDispenserApplication.Size = new System.Drawing.Size(944, 728);
            this.GpDispenserApplication.TabIndex = 0;
            this.GpDispenserApplication.Text = "点胶图形";
            // 
            // panelControl2
            // 
            this.panelControl2.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl2.Controls.Add(this.GcEpoxyApplication);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(12, 56);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Padding = new System.Windows.Forms.Padding(0, 15, 0, 0);
            this.panelControl2.Size = new System.Drawing.Size(814, 670);
            this.panelControl2.TabIndex = 11;
            // 
            // GcEpoxyApplication
            // 
            this.GcEpoxyApplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcEpoxyApplication.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEpoxyApplication.Location = new System.Drawing.Point(0, 15);
            this.GcEpoxyApplication.MainView = this.GvEpoxyApplication;
            this.GcEpoxyApplication.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcEpoxyApplication.Name = "GcEpoxyApplication";
            this.GcEpoxyApplication.Size = new System.Drawing.Size(814, 655);
            this.GcEpoxyApplication.TabIndex = 6;
            this.GcEpoxyApplication.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvEpoxyApplication});
            // 
            // GvEpoxyApplication
            // 
            this.GvEpoxyApplication.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.name});
            this.GvEpoxyApplication.DetailHeight = 272;
            this.GvEpoxyApplication.GridControl = this.GcEpoxyApplication;
            this.GvEpoxyApplication.Name = "GvEpoxyApplication";
            this.GvEpoxyApplication.OptionsDetail.EnableMasterViewMode = false;
            this.GvEpoxyApplication.OptionsView.ShowColumnHeaders = false;
            this.GvEpoxyApplication.OptionsView.ShowGroupPanel = false;
            // 
            // name
            // 
            this.name.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.AppearanceCell.Options.UseFont = true;
            this.name.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.name.AppearanceHeader.Options.UseFont = true;
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
            this.labelControl1.Size = new System.Drawing.Size(186, 18);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "创建所需要的点胶/画胶图形";
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.BtEpoxyApplicationRepository);
            this.panelControl1.Controls.Add(this.BtnAdd);
            this.panelControl1.Controls.Add(this.BtnDelete);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl1.Location = new System.Drawing.Point(826, 38);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(116, 688);
            this.panelControl1.TabIndex = 10;
            // 
            // BtEpoxyApplicationRepository
            // 
            this.BtEpoxyApplicationRepository.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtEpoxyApplicationRepository.Appearance.Options.UseFont = true;
            this.BtEpoxyApplicationRepository.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtEpoxyApplicationRepository.Location = new System.Drawing.Point(15, 152);
            this.BtEpoxyApplicationRepository.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtEpoxyApplicationRepository.Name = "BtEpoxyApplicationRepository";
            this.BtEpoxyApplicationRepository.Size = new System.Drawing.Size(91, 34);
            this.BtEpoxyApplicationRepository.TabIndex = 3;
            this.BtEpoxyApplicationRepository.Text = "仓库";
            this.BtEpoxyApplicationRepository.Click += new System.EventHandler(this.BtEpoxyApplicationRepository_Click);
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
            this.BtnAdd.Text = "添加";
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
            this.barDockControlTop.Size = new System.Drawing.Size(930, 0);
            // 
            // barDockControl1
            // 
            this.barDockControl1.CausesValidation = false;
            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl1.Location = new System.Drawing.Point(12, 38);
            this.barDockControl1.Manager = null;
            this.barDockControl1.Size = new System.Drawing.Size(930, 0);
            // 
            // barDockControl2
            // 
            this.barDockControl2.CausesValidation = false;
            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControl2.Location = new System.Drawing.Point(12, 38);
            this.barDockControl2.Manager = null;
            this.barDockControl2.Size = new System.Drawing.Size(930, 0);
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "gridColumn1";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 94;
            // 
            // UcAddEpoxyApplication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GpDispenserApplication);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcAddEpoxyApplication";
            this.Size = new System.Drawing.Size(944, 728);
            ((System.ComponentModel.ISupportInitialize)(this.GpDispenserApplication)).EndInit();
            this.GpDispenserApplication.ResumeLayout(false);
            this.GpDispenserApplication.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcEpoxyApplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvEpoxyApplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GpDispenserApplication;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControl1;
        private DevExpress.XtraBars.BarDockControl barDockControl2;
        private DevExpress.XtraEditors.SimpleButton BtnAdd;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnDelete;
        private DevExpress.XtraGrid.GridControl GcEpoxyApplication;
        private DevExpress.XtraGrid.Views.Grid.GridView GvEpoxyApplication;
        private DevExpress.XtraGrid.Columns.GridColumn name;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton BtEpoxyApplicationRepository;
    }
}
