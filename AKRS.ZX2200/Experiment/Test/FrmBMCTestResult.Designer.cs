namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmBMCTestResult
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
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.SpAngleAccuracy = new DevExpress.XtraEditors.SpinEdit();
            this.SpXYAccuracy = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.GcTestResult = new DevExpress.XtraGrid.GridControl();
            this.GvTestResult = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtnDetail = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngleAccuracy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXYAccuracy.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestResult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTestResult)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl7);
            this.groupControl2.Controls.Add(this.SpAngleAccuracy);
            this.groupControl2.Controls.Add(this.SpXYAccuracy);
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(0, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(733, 150);
            this.groupControl2.TabIndex = 12;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(498, 90);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(7, 18);
            this.labelControl7.TabIndex = 7;
            this.labelControl7.Text = "°";
            // 
            // SpAngleAccuracy
            // 
            this.SpAngleAccuracy.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAngleAccuracy.Location = new System.Drawing.Point(201, 87);
            this.SpAngleAccuracy.Name = "SpAngleAccuracy";
            this.SpAngleAccuracy.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAngleAccuracy.Size = new System.Drawing.Size(291, 24);
            this.SpAngleAccuracy.TabIndex = 6;
            // 
            // SpXYAccuracy
            // 
            this.SpXYAccuracy.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpXYAccuracy.Location = new System.Drawing.Point(201, 41);
            this.SpXYAccuracy.Name = "SpXYAccuracy";
            this.SpXYAccuracy.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpXYAccuracy.Size = new System.Drawing.Size(291, 24);
            this.SpXYAccuracy.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(40, 90);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(134, 18);
            this.labelControl3.TabIndex = 5;
            this.labelControl3.Text = "Theta  accuracy +/-";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(51, 48);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(109, 18);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "XY accuracy +/-";
            // 
            // GcTestResult
            // 
            this.GcTestResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcTestResult.Location = new System.Drawing.Point(0, 150);
            this.GcTestResult.MainView = this.GvTestResult;
            this.GcTestResult.Name = "GcTestResult";
            this.GcTestResult.Size = new System.Drawing.Size(733, 387);
            this.GcTestResult.TabIndex = 13;
            this.GcTestResult.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvTestResult});
            // 
            // GvTestResult
            // 
            this.GvTestResult.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.GvTestResult.GridControl = this.GcTestResult;
            this.GvTestResult.Name = "GvTestResult";
            this.GvTestResult.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "Name";
            this.gridColumn1.MinWidth = 25;
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 94;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "X（μm）";
            this.gridColumn2.FieldName = "XRes";
            this.gridColumn2.MinWidth = 25;
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 94;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Y（μm）";
            this.gridColumn3.FieldName = "YRes";
            this.gridColumn3.MinWidth = 25;
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 94;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Theta";
            this.gridColumn4.FieldName = "ThetaRes";
            this.gridColumn4.MinWidth = 25;
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 94;
            // 
            // BtnDetail
            // 
            this.BtnDetail.Location = new System.Drawing.Point(150, 604);
            this.BtnDetail.Name = "BtnDetail";
            this.BtnDetail.Size = new System.Drawing.Size(125, 35);
            this.BtnDetail.TabIndex = 15;
            this.BtnDetail.Text = "Detail";
            this.BtnDetail.Click += new System.EventHandler(this.BtnDetail_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(427, 604);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(125, 35);
            this.BtnCancel.TabIndex = 14;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmBMCTestResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 694);
            this.Controls.Add(this.BtnDetail);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.GcTestResult);
            this.Controls.Add(this.groupControl2);
            this.Name = "FrmBMCTestResult";
            this.Text = "BMC  test  result";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAngleAccuracy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpXYAccuracy.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestResult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvTestResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SpinEdit SpAngleAccuracy;
        private DevExpress.XtraEditors.SpinEdit SpXYAccuracy;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraGrid.GridControl GcTestResult;
        private DevExpress.XtraGrid.Views.Grid.GridView GvTestResult;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton BtnDetail;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
    }
}