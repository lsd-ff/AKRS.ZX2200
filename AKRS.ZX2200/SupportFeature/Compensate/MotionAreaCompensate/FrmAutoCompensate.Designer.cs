namespace AKRS.ZX2200.SupportFeature.Compensate.MotionAreaCompensate
{
    partial class FrmAutoCompensate
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
            this.btnEnable = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.gcCompensate = new DevExpress.XtraGrid.GridControl();
            this.gvCompensate = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnStartCollect = new DevExpress.XtraEditors.SimpleButton();
            this.btnStopCollect = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.chkAutoEnable = new DevExpress.XtraEditors.CheckEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.spinCollectRounds = new DevExpress.XtraEditors.SpinEdit();
            this.label1 = new System.Windows.Forms.Label();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.label3 = new System.Windows.Forms.Label();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.lblLastCalcTime = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblRoundInfo = new System.Windows.Forms.Label();
            this.lblStateText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this._pollTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcCompensate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompensate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCollectRounds.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnEnable
            // 
            this.btnEnable.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEnable.Appearance.Options.UseFont = true;
            this.btnEnable.Location = new System.Drawing.Point(556, 663);
            this.btnEnable.Name = "btnEnable";
            this.btnEnable.Size = new System.Drawing.Size(170, 98);
            this.btnEnable.TabIndex = 0;
            this.btnEnable.Text = "启用补偿";
            this.btnEnable.Click += new System.EventHandler(this.BtnEnable_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.gcCompensate);
            this.panelControl1.Location = new System.Drawing.Point(0, 445);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(953, 195);
            this.panelControl1.TabIndex = 1;
            // 
            // gcCompensate
            // 
            this.gcCompensate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcCompensate.Location = new System.Drawing.Point(2, 2);
            this.gcCompensate.MainView = this.gvCompensate;
            this.gcCompensate.Name = "gcCompensate";
            this.gcCompensate.Size = new System.Drawing.Size(949, 191);
            this.gcCompensate.TabIndex = 0;
            this.gcCompensate.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvCompensate});
            // 
            // gvCompensate
            // 
            this.gvCompensate.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvCompensate.GridControl = this.gcCompensate;
            this.gvCompensate.IndicatorWidth = 50;
            this.gvCompensate.Name = "gvCompensate";
            this.gvCompensate.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "补偿X";
            this.gridColumn1.DisplayFormat.FormatString = "F4";
            this.gridColumn1.FieldName = "X";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "补偿Y";
            this.gridColumn2.DisplayFormat.FormatString = "F4";
            this.gridColumn2.FieldName = "y";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            // 
            // btnStartCollect
            // 
            this.btnStartCollect.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartCollect.Appearance.Options.UseFont = true;
            this.btnStartCollect.Location = new System.Drawing.Point(12, 663);
            this.btnStartCollect.Name = "btnStartCollect";
            this.btnStartCollect.Size = new System.Drawing.Size(221, 98);
            this.btnStartCollect.TabIndex = 2;
            this.btnStartCollect.Text = "开始采集并计算";
            this.btnStartCollect.Click += new System.EventHandler(this.BtnStartCollect_Click);
            // 
            // btnStopCollect
            // 
            this.btnStopCollect.Appearance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStopCollect.Appearance.Options.UseFont = true;
            this.btnStopCollect.Location = new System.Drawing.Point(319, 663);
            this.btnStopCollect.Name = "btnStopCollect";
            this.btnStopCollect.Size = new System.Drawing.Size(170, 98);
            this.btnStopCollect.TabIndex = 3;
            this.btnStopCollect.Text = "停止采集";
            this.btnStopCollect.Click += new System.EventHandler(this.BtnStopCollect_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.chkAutoEnable);
            this.panelControl2.Controls.Add(this.label2);
            this.panelControl2.Controls.Add(this.spinCollectRounds);
            this.panelControl2.Controls.Add(this.label1);
            this.panelControl2.Location = new System.Drawing.Point(0, 46);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(953, 202);
            this.panelControl2.TabIndex = 4;
            // 
            // chkAutoEnable
            // 
            this.chkAutoEnable.Location = new System.Drawing.Point(43, 105);
            this.chkAutoEnable.Name = "chkAutoEnable";
            this.chkAutoEnable.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoEnable.Properties.Appearance.Options.UseFont = true;
            this.chkAutoEnable.Properties.Caption = "计算完成后自动启用补偿";
            this.chkAutoEnable.Size = new System.Drawing.Size(253, 29);
            this.chkAutoEnable.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "目标有效轮数：";
            // 
            // spinCollectRounds
            // 
            this.spinCollectRounds.EditValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinCollectRounds.Location = new System.Drawing.Point(137, 53);
            this.spinCollectRounds.Name = "spinCollectRounds";
            this.spinCollectRounds.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinCollectRounds.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.spinCollectRounds.Properties.MinValue = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.spinCollectRounds.Size = new System.Drawing.Size(92, 20);
            this.spinCollectRounds.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "采集与计算参数";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.label3);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(953, 47);
            this.panelControl3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(437, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "自动贴片补偿计算";
            // 
            // panelControl4
            // 
            this.panelControl4.Controls.Add(this.lblLastCalcTime);
            this.panelControl4.Controls.Add(this.progressBar);
            this.panelControl4.Controls.Add(this.lblRoundInfo);
            this.panelControl4.Controls.Add(this.lblStateText);
            this.panelControl4.Controls.Add(this.label5);
            this.panelControl4.Location = new System.Drawing.Point(0, 246);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(953, 195);
            this.panelControl4.TabIndex = 6;
            // 
            // lblLastCalcTime
            // 
            this.lblLastCalcTime.AutoSize = true;
            this.lblLastCalcTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastCalcTime.Location = new System.Drawing.Point(40, 145);
            this.lblLastCalcTime.Name = "lblLastCalcTime";
            this.lblLastCalcTime.Size = new System.Drawing.Size(102, 17);
            this.lblLastCalcTime.TabIndex = 9;
            this.lblLastCalcTime.Text = "上次计算时间：--";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(43, 105);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(253, 23);
            this.progressBar.TabIndex = 8;
            // 
            // lblRoundInfo
            // 
            this.lblRoundInfo.AutoSize = true;
            this.lblRoundInfo.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoundInfo.Location = new System.Drawing.Point(39, 73);
            this.lblRoundInfo.Name = "lblRoundInfo";
            this.lblRoundInfo.Size = new System.Drawing.Size(257, 19);
            this.lblRoundInfo.TabIndex = 7;
            this.lblRoundInfo.Text = "已完成：0轮 | 有效：0轮 | 废轮：0轮";
            // 
            // lblStateText
            // 
            this.lblStateText.AutoSize = true;
            this.lblStateText.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStateText.Location = new System.Drawing.Point(39, 45);
            this.lblStateText.Name = "lblStateText";
            this.lblStateText.Size = new System.Drawing.Size(114, 19);
            this.lblStateText.TabIndex = 6;
            this.lblStateText.Text = "状态：待机就绪";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 14);
            this.label5.TabIndex = 0;
            this.label5.Text = "采集运行状态";
            // 
            // _pollTimer
            // 
            this._pollTimer.Interval = 20000;
            this._pollTimer.Tick += new System.EventHandler(this.PollTimer_Tick);
            // 
            // FrmAutoCompensate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(953, 773);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnStopCollect);
            this.Controls.Add(this.btnStartCollect);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.btnEnable);
            this.Name = "FrmAutoCompensate";
            this.Text = "位置补偿列表";
            this.Load += new System.EventHandler(this.FrmAutoCompensate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcCompensate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCompensate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            this.panelControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkAutoEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinCollectRounds.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            this.panelControl3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            this.panelControl4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnEnable;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnStartCollect;
        private DevExpress.XtraGrid.GridControl gcCompensate;
        private DevExpress.XtraGrid.Views.Grid.GridView gvCompensate;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.SimpleButton btnStopCollect;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit spinCollectRounds;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.CheckEdit chkAutoEnable;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblStateText;
        private System.Windows.Forms.Label lblRoundInfo;
        private System.Windows.Forms.Label lblLastCalcTime;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Timer _pollTimer;
    }
}