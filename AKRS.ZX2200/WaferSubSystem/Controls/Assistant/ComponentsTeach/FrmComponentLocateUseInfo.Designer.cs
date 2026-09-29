namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    partial class FrmComponentLocateUseInfo
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
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.CkIsCalculateCenterByThreePoint = new DevExpress.XtraEditors.CheckEdit();
            this.CkIsCalculateAngelByTwoPoint = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpAroundDistance = new DevExpress.XtraEditors.SpinEdit();
            this.CKIsAround = new DevExpress.XtraEditors.CheckEdit();
            this.BtOK = new DevExpress.XtraEditors.SimpleButton();
            this.CkUseAngle2 = new DevExpress.XtraEditors.CheckEdit();
            this.CkUseY2 = new DevExpress.XtraEditors.CheckEdit();
            this.CkUseX2 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.CkUseAngle1 = new DevExpress.XtraEditors.CheckEdit();
            this.CkUseY1 = new DevExpress.XtraEditors.CheckEdit();
            this.CkUseX1 = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.CkIsCalculateCenterByThreePoint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkIsCalculateAngelByTwoPoint.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAroundDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CKIsAround.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseAngle2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseY2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseX2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseAngle1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseY1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseX1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(356, 328);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(26, 18);
            this.labelControl2.TabIndex = 21;
            this.labelControl2.Text = "mm";
            // 
            // CkIsCalculateCenterByThreePoint
            // 
            this.CkIsCalculateCenterByThreePoint.Location = new System.Drawing.Point(51, 446);
            this.CkIsCalculateCenterByThreePoint.Name = "CkIsCalculateCenterByThreePoint";
            this.CkIsCalculateCenterByThreePoint.Properties.Caption = "三点确定圆";
            this.CkIsCalculateCenterByThreePoint.Size = new System.Drawing.Size(129, 24);
            this.CkIsCalculateCenterByThreePoint.TabIndex = 20;
            // 
            // CkIsCalculateAngelByTwoPoint
            // 
            this.CkIsCalculateAngelByTwoPoint.Location = new System.Drawing.Point(51, 390);
            this.CkIsCalculateAngelByTwoPoint.Name = "CkIsCalculateAngelByTwoPoint";
            this.CkIsCalculateAngelByTwoPoint.Properties.Caption = "两点确定角度(多点时使用前两点)";
            this.CkIsCalculateAngelByTwoPoint.Size = new System.Drawing.Size(274, 24);
            this.CkIsCalculateAngelByTwoPoint.TabIndex = 19;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(186, 328);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(30, 18);
            this.labelControl1.TabIndex = 18;
            this.labelControl1.Text = "间距";
            // 
            // SpAroundDistance
            // 
            this.SpAroundDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAroundDistance.Location = new System.Drawing.Point(224, 324);
            this.SpAroundDistance.Name = "SpAroundDistance";
            this.SpAroundDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAroundDistance.Size = new System.Drawing.Size(125, 24);
            this.SpAroundDistance.TabIndex = 17;
            // 
            // CKIsAround
            // 
            this.CKIsAround.Location = new System.Drawing.Point(51, 324);
            this.CKIsAround.Name = "CKIsAround";
            this.CKIsAround.Properties.Caption = "四周定位";
            this.CKIsAround.Size = new System.Drawing.Size(94, 24);
            this.CKIsAround.TabIndex = 16;
            // 
            // BtOK
            // 
            this.BtOK.Location = new System.Drawing.Point(281, 492);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(146, 57);
            this.BtOK.TabIndex = 13;
            this.BtOK.Text = "确认";
            // 
            // CkUseAngle2
            // 
            this.CkUseAngle2.Location = new System.Drawing.Point(42, 221);
            this.CkUseAngle2.Name = "CkUseAngle2";
            this.CkUseAngle2.Properties.Caption = "使用定位结果 角度";
            this.CkUseAngle2.Size = new System.Drawing.Size(162, 24);
            this.CkUseAngle2.TabIndex = 2;
            // 
            // CkUseY2
            // 
            this.CkUseY2.Location = new System.Drawing.Point(42, 138);
            this.CkUseY2.Name = "CkUseY2";
            this.CkUseY2.Properties.Caption = "使用定位结果 Y";
            this.CkUseY2.Size = new System.Drawing.Size(139, 24);
            this.CkUseY2.TabIndex = 1;
            // 
            // CkUseX2
            // 
            this.CkUseX2.Location = new System.Drawing.Point(42, 60);
            this.CkUseX2.Name = "CkUseX2";
            this.CkUseX2.Properties.Caption = "使用定位结果 X";
            this.CkUseX2.Size = new System.Drawing.Size(139, 24);
            this.CkUseX2.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.CkUseAngle2);
            this.groupControl2.Controls.Add(this.CkUseY2);
            this.groupControl2.Controls.Add(this.CkUseX2);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(396, 12);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(209, 280);
            this.groupControl2.TabIndex = 12;
            this.groupControl2.Text = "模板2结果使用方式";
            // 
            // CkUseAngle1
            // 
            this.CkUseAngle1.Location = new System.Drawing.Point(42, 221);
            this.CkUseAngle1.Name = "CkUseAngle1";
            this.CkUseAngle1.Properties.Caption = "使用定位结果 角度";
            this.CkUseAngle1.Size = new System.Drawing.Size(162, 24);
            this.CkUseAngle1.TabIndex = 2;
            // 
            // CkUseY1
            // 
            this.CkUseY1.Location = new System.Drawing.Point(42, 138);
            this.CkUseY1.Name = "CkUseY1";
            this.CkUseY1.Properties.Caption = "使用定位结果 Y";
            this.CkUseY1.Size = new System.Drawing.Size(139, 24);
            this.CkUseY1.TabIndex = 1;
            // 
            // CkUseX1
            // 
            this.CkUseX1.Location = new System.Drawing.Point(42, 60);
            this.CkUseX1.Name = "CkUseX1";
            this.CkUseX1.Properties.Caption = "使用定位结果 X";
            this.CkUseX1.Size = new System.Drawing.Size(139, 24);
            this.CkUseX1.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.CkUseAngle1);
            this.groupControl1.Controls.Add(this.CkUseY1);
            this.groupControl1.Controls.Add(this.CkUseX1);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(32, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(209, 280);
            this.groupControl1.TabIndex = 11;
            this.groupControl1.Text = "模板1结果使用方式";
            // 
            // FrmComponentLocateUseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 561);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.CkIsCalculateCenterByThreePoint);
            this.Controls.Add(this.CkIsCalculateAngelByTwoPoint);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.SpAroundDistance);
            this.Controls.Add(this.CKIsAround);
            this.Controls.Add(this.BtOK);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmComponentLocateUseInfo";
            this.Text = "芯片定位配置";
            ((System.ComponentModel.ISupportInitialize)(this.CkIsCalculateCenterByThreePoint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkIsCalculateAngelByTwoPoint.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpAroundDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CKIsAround.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseAngle2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseY2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseX2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CkUseAngle1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseY1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CkUseX1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.CheckEdit CkIsCalculateCenterByThreePoint;
        private DevExpress.XtraEditors.CheckEdit CkIsCalculateAngelByTwoPoint;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpAroundDistance;
        private DevExpress.XtraEditors.CheckEdit CKIsAround;
        private DevExpress.XtraEditors.SimpleButton BtOK;
        private DevExpress.XtraEditors.CheckEdit CkUseAngle2;
        private DevExpress.XtraEditors.CheckEdit CkUseY2;
        private DevExpress.XtraEditors.CheckEdit CkUseX2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.CheckEdit CkUseAngle1;
        private DevExpress.XtraEditors.CheckEdit CkUseY1;
        private DevExpress.XtraEditors.CheckEdit CkUseX1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}