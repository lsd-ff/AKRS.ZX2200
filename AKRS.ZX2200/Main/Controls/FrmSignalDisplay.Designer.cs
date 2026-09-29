namespace AKRS.ZX2200.Main.Controls
{
    partial class FrmSignalDisplay
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtnWaferTaskState = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.BtnTransportTaskState = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.BtnBondTaskState = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnDispenseTaskState = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.GcSignal = new DevExpress.XtraGrid.GridControl();
            this.GvSignal = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GcSignal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvSignal)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnWaferTaskState);
            this.groupControl1.Controls.Add(this.label4);
            this.groupControl1.Controls.Add(this.BtnTransportTaskState);
            this.groupControl1.Controls.Add(this.label3);
            this.groupControl1.Controls.Add(this.BtnBondTaskState);
            this.groupControl1.Controls.Add(this.label2);
            this.groupControl1.Controls.Add(this.BtnDispenseTaskState);
            this.groupControl1.Controls.Add(this.label1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1107, 170);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Thread";
            // 
            // BtnWaferTaskState
            // 
            this.BtnWaferTaskState.Location = new System.Drawing.Point(766, 99);
            this.BtnWaferTaskState.Name = "BtnWaferTaskState";
            this.BtnWaferTaskState.Size = new System.Drawing.Size(147, 35);
            this.BtnWaferTaskState.TabIndex = 7;
            this.BtnWaferTaskState.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(510, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(143, 18);
            this.label4.TabIndex = 6;
            this.label4.Text = "Wafer  system  task";
            // 
            // BtnTransportTaskState
            // 
            this.BtnTransportTaskState.Location = new System.Drawing.Point(282, 99);
            this.BtnTransportTaskState.Name = "BtnTransportTaskState";
            this.BtnTransportTaskState.Size = new System.Drawing.Size(147, 35);
            this.BtnTransportTaskState.TabIndex = 5;
            this.BtnTransportTaskState.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 18);
            this.label3.TabIndex = 4;
            this.label3.Text = "Transport  system  task";
            // 
            // BtnBondTaskState
            // 
            this.BtnBondTaskState.Location = new System.Drawing.Point(766, 52);
            this.BtnBondTaskState.Name = "BtnBondTaskState";
            this.BtnBondTaskState.Size = new System.Drawing.Size(147, 35);
            this.BtnBondTaskState.TabIndex = 3;
            this.BtnBondTaskState.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(543, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 18);
            this.label2.TabIndex = 2;
            this.label2.Text = "Bond  task";
            // 
            // BtnDispenseTaskState
            // 
            this.BtnDispenseTaskState.Location = new System.Drawing.Point(282, 52);
            this.BtnDispenseTaskState.Name = "BtnDispenseTaskState";
            this.BtnDispenseTaskState.Size = new System.Drawing.Size(147, 35);
            this.BtnDispenseTaskState.TabIndex = 1;
            this.BtnDispenseTaskState.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Dispense  task";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.GcSignal);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 170);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(1107, 695);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "Signal";
            // 
            // GcSignal
            // 
            this.GcSignal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GcSignal.Location = new System.Drawing.Point(2, 28);
            this.GcSignal.MainView = this.GvSignal;
            this.GcSignal.Name = "GcSignal";
            this.GcSignal.Size = new System.Drawing.Size(1103, 665);
            this.GcSignal.TabIndex = 0;
            this.GcSignal.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvSignal});
            // 
            // GvSignal
            // 
            this.GvSignal.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.GvSignal.GridControl = this.GcSignal;
            this.GvSignal.Name = "GvSignal";
            this.GvSignal.OptionsBehavior.ReadOnly = true;
            this.GvSignal.OptionsView.ShowGroupPanel = false;
            this.GvSignal.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.Caption = "Signal";
            this.gridColumn1.FieldName = "name";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 745;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.Caption = "State";
            this.gridColumn2.FieldName = "State";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 330;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tag = "FrmSignal";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "State";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            this.gridColumn3.Width = 94;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "State";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 94;
            // 
            // FrmSignalDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1107, 865);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmSignalDisplay";
            this.Text = "Signal  display";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSignalDisplay_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GcSignal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvSignal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private System.Windows.Forms.Button BtnWaferTaskState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button BtnTransportTaskState;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnBondTaskState;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnDispenseTaskState;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraGrid.GridControl GcSignal;
        private DevExpress.XtraGrid.Views.Grid.GridView GvSignal;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}