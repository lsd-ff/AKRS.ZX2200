namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmTestMoveAccuracy
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
            this.virtualServerModeSource1 = new DevExpress.Data.VirtualServerModeSource(this.components);
            this.BtnLeft = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtnRight = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUp = new DevExpress.XtraEditors.SimpleButton();
            this.txtStep = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.BtnEditPr = new DevExpress.XtraEditors.SimpleButton();
            this.BtnMoveToCenter = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLocate = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.txtResultX = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.txtResultY = new DevExpress.XtraEditors.TextEdit();
            this.BtnTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtnTestMoveCenter = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.virtualServerModeSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStep.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnLeft
            // 
            this.BtnLeft.Appearance.Font = new System.Drawing.Font("Webdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.BtnLeft.Appearance.Options.UseFont = true;
            this.BtnLeft.Location = new System.Drawing.Point(27, 157);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(61, 63);
            this.BtnLeft.TabIndex = 1;
            this.BtnLeft.Text = "3";
            this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.Appearance.Font = new System.Drawing.Font("Webdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.BtnDown.Appearance.Options.UseFont = true;
            this.BtnDown.Location = new System.Drawing.Point(106, 228);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(61, 63);
            this.BtnDown.TabIndex = 3;
            this.BtnDown.Text = "6";
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnRight
            // 
            this.BtnRight.Appearance.Font = new System.Drawing.Font("Webdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.BtnRight.Appearance.Options.UseFont = true;
            this.BtnRight.Location = new System.Drawing.Point(183, 157);
            this.BtnRight.Name = "BtnRight";
            this.BtnRight.Size = new System.Drawing.Size(61, 63);
            this.BtnRight.TabIndex = 4;
            this.BtnRight.Text = "4";
            this.BtnRight.Click += new System.EventHandler(this.BtnRight_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Appearance.Font = new System.Drawing.Font("Webdings", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.BtnUp.Appearance.Options.UseFont = true;
            this.BtnUp.Location = new System.Drawing.Point(106, 90);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(61, 63);
            this.BtnUp.TabIndex = 5;
            this.BtnUp.Text = "5";
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // txtStep
            // 
            this.txtStep.EditValue = "0";
            this.txtStep.Location = new System.Drawing.Point(90, 35);
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(100, 20);
            this.txtStep.TabIndex = 6;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(40, 38);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(28, 14);
            this.labelControl1.TabIndex = 7;
            this.labelControl1.Text = "step:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(205, 38);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(20, 14);
            this.labelControl2.TabIndex = 8;
            this.labelControl2.Text = "mm";
            // 
            // BtnEditPr
            // 
            this.BtnEditPr.Location = new System.Drawing.Point(40, 361);
            this.BtnEditPr.Name = "BtnEditPr";
            this.BtnEditPr.Size = new System.Drawing.Size(75, 23);
            this.BtnEditPr.TabIndex = 9;
            this.BtnEditPr.Text = "Edit PR";
            this.BtnEditPr.Click += new System.EventHandler(this.BtnEditPr_Click);
            // 
            // BtnMoveToCenter
            // 
            this.BtnMoveToCenter.Location = new System.Drawing.Point(341, 38);
            this.BtnMoveToCenter.Name = "BtnMoveToCenter";
            this.BtnMoveToCenter.Size = new System.Drawing.Size(128, 25);
            this.BtnMoveToCenter.TabIndex = 10;
            this.BtnMoveToCenter.Text = "Move To CamCenter";
            this.BtnMoveToCenter.Click += new System.EventHandler(this.BtnMoveToCenter_Click);
            // 
            // BtnLocate
            // 
            this.BtnLocate.Location = new System.Drawing.Point(341, 93);
            this.BtnLocate.Name = "BtnLocate";
            this.BtnLocate.Size = new System.Drawing.Size(128, 25);
            this.BtnLocate.TabIndex = 11;
            this.BtnLocate.Text = "Locate";
            this.BtnLocate.Click += new System.EventHandler(this.BtnLocate_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(505, 157);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(24, 14);
            this.labelControl3.TabIndex = 14;
            this.labelControl3.Text = "pixel";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(340, 157);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(11, 14);
            this.labelControl4.TabIndex = 13;
            this.labelControl4.Text = "X:";
            // 
            // txtResultX
            // 
            this.txtResultX.EditValue = "0";
            this.txtResultX.Location = new System.Drawing.Point(390, 154);
            this.txtResultX.Name = "txtResultX";
            this.txtResultX.Size = new System.Drawing.Size(100, 20);
            this.txtResultX.TabIndex = 12;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(505, 195);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(24, 14);
            this.labelControl5.TabIndex = 17;
            this.labelControl5.Text = "pixel";
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(340, 195);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(12, 14);
            this.labelControl6.TabIndex = 16;
            this.labelControl6.Text = "Y:";
            // 
            // txtResultY
            // 
            this.txtResultY.EditValue = "0";
            this.txtResultY.Location = new System.Drawing.Point(390, 192);
            this.txtResultY.Name = "txtResultY";
            this.txtResultY.Size = new System.Drawing.Size(100, 20);
            this.txtResultY.TabIndex = 15;
            // 
            // BtnTest
            // 
            this.BtnTest.Location = new System.Drawing.Point(169, 361);
            this.BtnTest.Name = "BtnTest";
            this.BtnTest.Size = new System.Drawing.Size(75, 23);
            this.BtnTest.TabIndex = 18;
            this.BtnTest.Text = "Test";
            this.BtnTest.Click += new System.EventHandler(this.BtnTest_Click);
            // 
            // BtnTestMoveCenter
            // 
            this.BtnTestMoveCenter.Location = new System.Drawing.Point(40, 404);
            this.BtnTestMoveCenter.Name = "BtnTestMoveCenter";
            this.BtnTestMoveCenter.Size = new System.Drawing.Size(150, 23);
            this.BtnTestMoveCenter.TabIndex = 19;
            this.BtnTestMoveCenter.Text = "Test Move To Center";
            this.BtnTestMoveCenter.Click += new System.EventHandler(this.BtnTestMoveCenter_Click);
            // 
            // FrmTestMoveAccuracy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 450);
            this.Controls.Add(this.BtnTestMoveCenter);
            this.Controls.Add(this.BtnTest);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.txtResultY);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtResultX);
            this.Controls.Add(this.BtnLocate);
            this.Controls.Add(this.BtnMoveToCenter);
            this.Controls.Add(this.BtnEditPr);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.txtStep);
            this.Controls.Add(this.BtnUp);
            this.Controls.Add(this.BtnRight);
            this.Controls.Add(this.BtnDown);
            this.Controls.Add(this.BtnLeft);
            this.Name = "FrmTestMoveAccuracy";
            this.Text = "FrmTestMoveAccuracy";
            ((System.ComponentModel.ISupportInitialize)(this.virtualServerModeSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStep.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtResultY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.Data.VirtualServerModeSource virtualServerModeSource1;
        private DevExpress.XtraEditors.SimpleButton BtnLeft;
        private DevExpress.XtraEditors.SimpleButton BtnDown;
        private DevExpress.XtraEditors.SimpleButton BtnRight;
        private DevExpress.XtraEditors.SimpleButton BtnUp;
        private DevExpress.XtraEditors.TextEdit txtStep;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton BtnEditPr;
        private DevExpress.XtraEditors.SimpleButton BtnMoveToCenter;
        private DevExpress.XtraEditors.SimpleButton BtnLocate;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit txtResultX;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.TextEdit txtResultY;
        private DevExpress.XtraEditors.SimpleButton BtnTest;
        private DevExpress.XtraEditors.SimpleButton BtnTestMoveCenter;
    }
}