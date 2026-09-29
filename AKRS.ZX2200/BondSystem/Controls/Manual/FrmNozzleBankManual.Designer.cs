namespace AKRS.ZX2200.BondSystem.Controls.Manual
{
    partial class FrmNozzleBankManual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNozzleBankManual));
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtnFetchNozzle = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel5 = new DevExpress.Utils.Layout.TablePanel();
            this.LbSelectedNozzle = new DevExpress.XtraEditors.LabelControl();
            this.BtnDetails = new DevExpress.XtraEditors.SimpleButton();
            this.BtnRemoveNozzle = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMaintenance = new DevExpress.XtraEditors.SimpleButton();
            this.MeCurNozzleMes = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.LbSingleStep = new DevExpress.XtraEditors.LabelControl();
            this.BtnRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel3 = new DevExpress.Utils.Layout.TablePanel();
            this.LbCurNozzle = new DevExpress.XtraEditors.LabelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.GcNozzleBank = new DevExpress.XtraGrid.GridControl();
            this.GvNozzleBank = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gCBondHead = new DevExpress.XtraEditors.GroupControl();
            this.tablePanel6 = new DevExpress.Utils.Layout.TablePanel();
            this.LbCurNozzle2 = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel5)).BeginInit();
            this.tablePanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MeCurNozzleMes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).BeginInit();
            this.tablePanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcNozzleBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvNozzleBank)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCBondHead)).BeginInit();
            this.gCBondHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel6)).BeginInit();
            this.tablePanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.tablePanel1);
            this.groupControl3.Controls.Add(this.tablePanel5);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(2, 2);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(427, 125);
            this.groupControl3.TabIndex = 12;
            this.groupControl3.Text = "选择吸嘴";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.BtnFetchNozzle);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(2, 58);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 30.8F)});
            this.tablePanel1.Size = new System.Drawing.Size(423, 52);
            this.tablePanel1.TabIndex = 2;
            // 
            // BtnFetchNozzle
            // 
            this.BtnFetchNozzle.AutoSize = true;
            this.tablePanel1.SetColumn(this.BtnFetchNozzle, 0);
            this.BtnFetchNozzle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnFetchNozzle.Location = new System.Drawing.Point(3, 2);
            this.BtnFetchNozzle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnFetchNozzle.Name = "BtnFetchNozzle";
            this.tablePanel1.SetRow(this.BtnFetchNozzle, 0);
            this.BtnFetchNozzle.Size = new System.Drawing.Size(417, 48);
            this.BtnFetchNozzle.TabIndex = 0;
            this.BtnFetchNozzle.Text = "从吸嘴架取吸嘴";
            this.BtnFetchNozzle.Click += new System.EventHandler(this.BtnFetchNozzle_Click);
            // 
            // tablePanel5
            // 
            this.tablePanel5.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel5.Controls.Add(this.LbSelectedNozzle);
            this.tablePanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel5.Location = new System.Drawing.Point(2, 23);
            this.tablePanel5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel5.Name = "tablePanel5";
            this.tablePanel5.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel5.Size = new System.Drawing.Size(423, 35);
            this.tablePanel5.TabIndex = 18;
            // 
            // LbSelectedNozzle
            // 
            this.LbSelectedNozzle.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbSelectedNozzle.Appearance.Options.UseFont = true;
            this.LbSelectedNozzle.Appearance.Options.UseTextOptions = true;
            this.LbSelectedNozzle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LbSelectedNozzle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel5.SetColumn(this.LbSelectedNozzle, 0);
            this.LbSelectedNozzle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbSelectedNozzle.Location = new System.Drawing.Point(3, 2);
            this.LbSelectedNozzle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbSelectedNozzle.Name = "LbSelectedNozzle";
            this.tablePanel5.SetRow(this.LbSelectedNozzle, 0);
            this.LbSelectedNozzle.Size = new System.Drawing.Size(417, 31);
            this.LbSelectedNozzle.TabIndex = 0;
            this.LbSelectedNozzle.Text = "labelControl1";
            // 
            // BtnDetails
            // 
            this.BtnDetails.AutoSize = true;
            this.tablePanel2.SetColumn(this.BtnDetails, 1);
            this.BtnDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnDetails.Location = new System.Drawing.Point(245, 8);
            this.BtnDetails.Margin = new System.Windows.Forms.Padding(33, 8, 33, 8);
            this.BtnDetails.Name = "BtnDetails";
            this.tablePanel2.SetRow(this.BtnDetails, 0);
            this.BtnDetails.Size = new System.Drawing.Size(146, 19);
            this.BtnDetails.TabIndex = 2;
            this.BtnDetails.Text = "方向盘";
            this.BtnDetails.Click += new System.EventHandler(this.BtnDetails_Click);
            // 
            // BtnRemoveNozzle
            // 
            this.BtnRemoveNozzle.AutoSize = true;
            this.tablePanel2.SetColumn(this.BtnRemoveNozzle, 0);
            this.BtnRemoveNozzle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnRemoveNozzle.Location = new System.Drawing.Point(33, 8);
            this.BtnRemoveNozzle.Margin = new System.Windows.Forms.Padding(33, 8, 33, 8);
            this.BtnRemoveNozzle.Name = "BtnRemoveNozzle";
            this.tablePanel2.SetRow(this.BtnRemoveNozzle, 0);
            this.BtnRemoveNozzle.Size = new System.Drawing.Size(146, 19);
            this.BtnRemoveNozzle.TabIndex = 0;
            this.BtnRemoveNozzle.Text = "归还吸嘴";
            this.BtnRemoveNozzle.Click += new System.EventHandler(this.BtnRemoveNozzle_Click);
            // 
            // BtnMaintenance
            // 
            this.BtnMaintenance.Appearance.Options.UseBackColor = true;
            this.BtnMaintenance.AutoSize = true;
            this.tablePanel2.SetColumn(this.BtnMaintenance, 0);
            this.tablePanel2.SetColumnSpan(this.BtnMaintenance, 2);
            this.BtnMaintenance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnMaintenance.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.BtnMaintenance.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnMaintenance.ImageOptions.SvgImage")));
            this.BtnMaintenance.Location = new System.Drawing.Point(33, 43);
            this.BtnMaintenance.Margin = new System.Windows.Forms.Padding(33, 8, 33, 8);
            this.BtnMaintenance.Name = "BtnMaintenance";
            this.tablePanel2.SetRow(this.BtnMaintenance, 1);
            this.BtnMaintenance.Size = new System.Drawing.Size(358, 19);
            this.BtnMaintenance.TabIndex = 3;
            this.BtnMaintenance.Text = "吸嘴架 伸出/缩回";
            this.BtnMaintenance.Click += new System.EventHandler(this.BtnMaintenance_Click);
            // 
            // MeCurNozzleMes
            // 
            this.tablePanel6.SetColumn(this.MeCurNozzleMes, 0);
            this.MeCurNozzleMes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeCurNozzleMes.Location = new System.Drawing.Point(3, 50);
            this.MeCurNozzleMes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MeCurNozzleMes.Name = "MeCurNozzleMes";
            this.MeCurNozzleMes.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MeCurNozzleMes.Properties.Appearance.Options.UseFont = true;
            this.MeCurNozzleMes.Properties.ReadOnly = true;
            this.tablePanel6.SetRow(this.MeCurNozzleMes, 1);
            this.MeCurNozzleMes.Size = new System.Drawing.Size(306, 508);
            this.MeCurNozzleMes.TabIndex = 15;
            // 
            // panelControl1
            // 
            this.panelControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelControl1.Controls.Add(this.groupControl4);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Controls.Add(this.groupControl3);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(431, 585);
            this.panelControl1.TabIndex = 16;
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.tablePanel2);
            this.groupControl4.Controls.Add(this.tablePanel3);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl4.Location = new System.Drawing.Point(2, 367);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(427, 216);
            this.groupControl4.TabIndex = 16;
            this.groupControl4.Text = "当前焊头上的吸嘴";
            // 
            // tablePanel2
            // 
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel2.Controls.Add(this.LbSingleStep);
            this.tablePanel2.Controls.Add(this.BtnMaintenance);
            this.tablePanel2.Controls.Add(this.BtnRemoveNozzle);
            this.tablePanel2.Controls.Add(this.BtnDetails);
            this.tablePanel2.Controls.Add(this.BtnRefresh);
            this.tablePanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel2.Location = new System.Drawing.Point(2, 73);
            this.tablePanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 35.59999F)});
            this.tablePanel2.Size = new System.Drawing.Size(423, 141);
            this.tablePanel2.TabIndex = 3;
            // 
            // LbSingleStep
            // 
            this.tablePanel2.SetColumn(this.LbSingleStep, 1);
            this.LbSingleStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbSingleStep.Location = new System.Drawing.Point(215, 107);
            this.LbSingleStep.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbSingleStep.Name = "LbSingleStep";
            this.tablePanel2.SetRow(this.LbSingleStep, 3);
            this.LbSingleStep.Size = new System.Drawing.Size(206, 32);
            this.LbSingleStep.TabIndex = 18;
            this.LbSingleStep.Text = "                               单步";
            // 
            // BtnRefresh
            // 
            this.tablePanel2.SetColumn(this.BtnRefresh, 0);
            this.tablePanel2.SetColumnSpan(this.BtnRefresh, 2);
            this.BtnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnRefresh.Location = new System.Drawing.Point(88, 78);
            this.BtnRefresh.Margin = new System.Windows.Forms.Padding(88, 8, 88, 8);
            this.BtnRefresh.Name = "BtnRefresh";
            this.tablePanel2.SetRow(this.BtnRefresh, 2);
            this.BtnRefresh.Size = new System.Drawing.Size(248, 19);
            this.BtnRefresh.TabIndex = 17;
            this.BtnRefresh.Text = "刷新";
            this.BtnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // tablePanel3
            // 
            this.tablePanel3.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel3.Controls.Add(this.LbCurNozzle);
            this.tablePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel3.Location = new System.Drawing.Point(2, 23);
            this.tablePanel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel3.Name = "tablePanel3";
            this.tablePanel3.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel3.Size = new System.Drawing.Size(423, 50);
            this.tablePanel3.TabIndex = 4;
            // 
            // LbCurNozzle
            // 
            this.LbCurNozzle.Appearance.Font = new System.Drawing.Font("Tahoma", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LbCurNozzle.Appearance.Options.UseFont = true;
            this.LbCurNozzle.Appearance.Options.UseTextOptions = true;
            this.LbCurNozzle.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LbCurNozzle.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel3.SetColumn(this.LbCurNozzle, 0);
            this.LbCurNozzle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbCurNozzle.Location = new System.Drawing.Point(3, 2);
            this.LbCurNozzle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbCurNozzle.Name = "LbCurNozzle";
            this.tablePanel3.SetRow(this.LbCurNozzle, 0);
            this.LbCurNozzle.Size = new System.Drawing.Size(417, 46);
            this.LbCurNozzle.TabIndex = 0;
            this.LbCurNozzle.Text = "labelControl1";
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.GcNozzleBank);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(2, 127);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(427, 240);
            this.groupControl1.TabIndex = 15;
            this.groupControl1.Text = "吸嘴架";
            // 
            // GcNozzleBank
            // 
            this.GcNozzleBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcNozzleBank.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcNozzleBank.Location = new System.Drawing.Point(2, 23);
            this.GcNozzleBank.MainView = this.GvNozzleBank;
            this.GcNozzleBank.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcNozzleBank.Name = "GcNozzleBank";
            this.GcNozzleBank.Size = new System.Drawing.Size(423, 215);
            this.GcNozzleBank.TabIndex = 0;
            this.GcNozzleBank.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvNozzleBank});
            // 
            // GvNozzleBank
            // 
            this.GvNozzleBank.ColumnPanelRowHeight = 8;
            this.GvNozzleBank.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.GvNozzleBank.DetailHeight = 272;
            this.GvNozzleBank.GridControl = this.GcNozzleBank;
            this.GvNozzleBank.Name = "GvNozzleBank";
            this.GvNozzleBank.OptionsView.ShowGroupPanel = false;
            this.GvNozzleBank.RowHeight = 8;
            this.GvNozzleBank.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GvNozzleBank_FocusedRowChanged_1);
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "槽位";
            this.gridColumn1.FieldName = "SlotNum";
            this.gridColumn1.MinWidth = 22;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 82;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "状态";
            this.gridColumn2.FieldName = "NozzleState";
            this.gridColumn2.MinWidth = 22;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 82;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "吸嘴名称";
            this.gridColumn3.FieldName = "NozzleName";
            this.gridColumn3.MinWidth = 22;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 82;
            // 
            // gCBondHead
            // 
            this.gCBondHead.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gCBondHead.Controls.Add(this.tablePanel6);
            this.gCBondHead.Dock = System.Windows.Forms.DockStyle.Right;
            this.gCBondHead.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.gCBondHead.Location = new System.Drawing.Point(465, 0);
            this.gCBondHead.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gCBondHead.Name = "gCBondHead";
            this.gCBondHead.Size = new System.Drawing.Size(316, 585);
            this.gCBondHead.TabIndex = 17;
            this.gCBondHead.Text = "Bond  head";
            // 
            // tablePanel6
            // 
            this.tablePanel6.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel6.Controls.Add(this.LbCurNozzle2);
            this.tablePanel6.Controls.Add(this.MeCurNozzleMes);
            this.tablePanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel6.Location = new System.Drawing.Point(2, 23);
            this.tablePanel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tablePanel6.Name = "tablePanel6";
            this.tablePanel6.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 48.4F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel6.Size = new System.Drawing.Size(312, 560);
            this.tablePanel6.TabIndex = 0;
            // 
            // LbCurNozzle2
            // 
            this.LbCurNozzle2.Appearance.Options.UseTextOptions = true;
            this.LbCurNozzle2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.LbCurNozzle2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.tablePanel6.SetColumn(this.LbCurNozzle2, 0);
            this.LbCurNozzle2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbCurNozzle2.Location = new System.Drawing.Point(3, 2);
            this.LbCurNozzle2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.LbCurNozzle2.Name = "LbCurNozzle2";
            this.tablePanel6.SetRow(this.LbCurNozzle2, 0);
            this.LbCurNozzle2.Size = new System.Drawing.Size(306, 44);
            this.LbCurNozzle2.TabIndex = 16;
            this.LbCurNozzle2.Text = "labelControl1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "UcNozzleBank";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FrmNozzleBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(781, 585);
            this.Controls.Add(this.gCBondHead);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximumSize = new System.Drawing.Size(9999, 9999);
            this.Name = "FrmNozzleBank";
            this.Text = "吸嘴架调试";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UcNozzleBank_FormClosing);
            this.Load += new System.EventHandler(this.FrmNozzleBank_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmNozzleBank_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel5)).EndInit();
            this.tablePanel5.ResumeLayout(false);
            this.tablePanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MeCurNozzleMes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            this.tablePanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).EndInit();
            this.tablePanel3.ResumeLayout(false);
            this.tablePanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcNozzleBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvNozzleBank)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gCBondHead)).EndInit();
            this.gCBondHead.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel6)).EndInit();
            this.tablePanel6.ResumeLayout(false);
            this.tablePanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SimpleButton BtnFetchNozzle;
        private DevExpress.XtraEditors.SimpleButton BtnRemoveNozzle;
        private DevExpress.XtraEditors.SimpleButton BtnDetails;
        private DevExpress.XtraEditors.SimpleButton BtnMaintenance;
        private DevExpress.XtraEditors.MemoEdit MeCurNozzleMes;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.Utils.Layout.TablePanel tablePanel3;
        private DevExpress.Utils.Layout.TablePanel tablePanel5;
        private DevExpress.Utils.Layout.TablePanel tablePanel6;
        private DevExpress.XtraEditors.GroupControl gCBondHead;
        private DevExpress.XtraEditors.LabelControl LbSelectedNozzle;
        private DevExpress.XtraEditors.LabelControl LbCurNozzle;
        private DevExpress.XtraEditors.LabelControl LbCurNozzle2;
        private DevExpress.XtraGrid.GridControl GcNozzleBank;
        private DevExpress.XtraGrid.Views.Grid.GridView GvNozzleBank;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton BtnRefresh;
        private DevExpress.XtraEditors.LabelControl LbSingleStep;
        private System.Windows.Forms.Timer timer1;
    }
}