namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    partial class FrmMeasureHeightPoints
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.checkEdit5 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.BtContinuousMeasurement = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.GcNozzleShelf = new DevExpress.XtraGrid.GridControl();
            this.GvNozzleShelf = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Point = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtContinuousMeasurement.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNozzleShelf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvNozzleShelf)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.checkEdit5);
            this.groupControl1.Controls.Add(this.checkEdit4);
            this.groupControl1.Controls.Add(this.checkEdit3);
            this.groupControl1.Controls.Add(this.BtContinuousMeasurement);
            this.groupControl1.Controls.Add(this.checkEdit1);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(11, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(741, 192);
            this.groupControl1.TabIndex = 0;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(462, 45);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(86, 30);
            this.simpleButton1.TabIndex = 5;
            this.simpleButton1.Text = "simpleButton1";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // checkEdit5
            // 
            this.checkEdit5.Location = new System.Drawing.Point(5, 150);
            this.checkEdit5.Name = "checkEdit5";
            this.checkEdit5.Properties.Caption = "Switch TD Z-axis";
            this.checkEdit5.Size = new System.Drawing.Size(365, 24);
            this.checkEdit5.TabIndex = 4;
            // 
            // checkEdit4
            // 
            this.checkEdit4.Location = new System.Drawing.Point(5, 121);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "Measure matrix";
            this.checkEdit4.Size = new System.Drawing.Size(365, 24);
            this.checkEdit4.TabIndex = 3;
            // 
            // checkEdit3
            // 
            this.checkEdit3.Location = new System.Drawing.Point(5, 91);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "Move dipping plate";
            this.checkEdit3.Size = new System.Drawing.Size(365, 24);
            this.checkEdit3.TabIndex = 2;
            // 
            // BtContinuousMeasurement
            // 
            this.BtContinuousMeasurement.Location = new System.Drawing.Point(5, 60);
            this.BtContinuousMeasurement.Name = "BtContinuousMeasurement";
            this.BtContinuousMeasurement.Properties.Caption = "Continuous measurement";
            this.BtContinuousMeasurement.Size = new System.Drawing.Size(365, 24);
            this.BtContinuousMeasurement.TabIndex = 1;
            this.BtContinuousMeasurement.CheckedChanged += new System.EventHandler(this.BtContinuousMeasurement_CheckedChanged);
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(5, 31);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "Create file";
            this.checkEdit1.Size = new System.Drawing.Size(365, 24);
            this.checkEdit1.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(759, 12);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(386, 192);
            this.groupControl2.TabIndex = 1;
            this.groupControl2.Text = "groupControl2";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "FrmMeasureHeightPoint";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // GcNozzleShelf
            // 
            this.GcNozzleShelf.Location = new System.Drawing.Point(11, 210);
            this.GcNozzleShelf.MainView = this.GvNozzleShelf;
            this.GcNozzleShelf.Name = "GcNozzleShelf";
            this.GcNozzleShelf.Size = new System.Drawing.Size(1133, 391);
            this.GcNozzleShelf.TabIndex = 4;
            this.GcNozzleShelf.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvNozzleShelf});
            // 
            // GvNozzleShelf
            // 
            this.GvNozzleShelf.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.Point,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.GvNozzleShelf.GridControl = this.GcNozzleShelf;
            this.GvNozzleShelf.Name = "GvNozzleShelf";
            this.GvNozzleShelf.OptionsBehavior.Editable = false;
            this.GvNozzleShelf.OptionsBehavior.ReadOnly = true;
            this.GvNozzleShelf.OptionsCustomization.AllowSort = false;
            this.GvNozzleShelf.OptionsView.ShowGroupPanel = false;
            // 
            // Point
            // 
            this.Point.Caption = "Point";
            this.Point.FieldName = "Point";
            this.Point.MinWidth = 25;
            this.Point.Name = "Point";
            this.Point.Visible = true;
            this.Point.VisibleIndex = 0;
            this.Point.Width = 99;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Value1";
            this.gridColumn2.FieldName = "Result1";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 99;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "value2";
            this.gridColumn3.FieldName = "Result2";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 99;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Value3";
            this.gridColumn4.FieldName = "Result3";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 99;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Value4";
            this.gridColumn5.FieldName = "Result4";
            this.gridColumn5.MinWidth = 25;
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 99;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Value5";
            this.gridColumn6.FieldName = "Result5";
            this.gridColumn6.MinWidth = 25;
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            this.gridColumn6.Width = 99;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Value6";
            this.gridColumn7.FieldName = "Result6";
            this.gridColumn7.MinWidth = 25;
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 6;
            this.gridColumn7.Width = 99;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Value7";
            this.gridColumn8.FieldName = "Result7";
            this.gridColumn8.MinWidth = 25;
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            this.gridColumn8.Width = 99;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Value8";
            this.gridColumn9.FieldName = "Result8";
            this.gridColumn9.MinWidth = 25;
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 8;
            this.gridColumn9.Width = 99;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Value9";
            this.gridColumn10.FieldName = "Result9";
            this.gridColumn10.MinWidth = 25;
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 9;
            this.gridColumn10.Width = 99;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Value10";
            this.gridColumn11.FieldName = "Result10";
            this.gridColumn11.MinWidth = 25;
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            this.gridColumn11.Width = 99;
            // 
            // FrmMeasureHeightPoints
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 613);
            this.Controls.Add(this.GcNozzleShelf);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmMeasureHeightPoints";
            this.Text = "FrmMeasureHeightPoints";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMeasureHeightPoints_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMeasureHeightPoints_FormClosed);
            this.Load += new System.EventHandler(this.FrmMeasureHeightPoints_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit5.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtContinuousMeasurement.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcNozzleShelf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvNozzleShelf)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.CheckEdit BtContinuousMeasurement;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit5;
        private DevExpress.XtraEditors.CheckEdit checkEdit4;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraGrid.GridControl GcNozzleShelf;
        private DevExpress.XtraGrid.Views.Grid.GridView GvNozzleShelf;
        private DevExpress.XtraGrid.Columns.GridColumn Point;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}