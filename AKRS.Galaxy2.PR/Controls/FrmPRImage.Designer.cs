namespace AKRS.Galaxy2.PR.Controls
{
    partial class FrmPRImage
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
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtX = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txtY = new DevExpress.XtraEditors.TextEdit();
            this.BtnOK = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.txtX.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtY.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(635, 491);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(635, 491);
            this.hWindowControl1.HMouseDown += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseDown_1);
            this.hWindowControl1.HMouseUp += new HalconDotNet.HMouseEventHandler(this.hWindowControl1_HMouseUp_1);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 509);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(15, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "X :";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(48, 509);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(46, 20);
            this.txtX.TabIndex = 2;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(114, 509);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(16, 14);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Y :";
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(136, 509);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(46, 20);
            this.txtY.TabIndex = 4;
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(395, 508);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(50, 23);
            this.BtnOK.TabIndex = 5;
            this.BtnOK.Text = "OK";
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(476, 509);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(50, 23);
            this.BtnCancel.TabIndex = 6;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmPRImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 542);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.hWindowControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmPRImage";
            this.Text = "晶圆实时图像";
            ((System.ComponentModel.ISupportInitialize)(this.txtX.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtY.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtX;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txtY;
        private DevExpress.XtraEditors.SimpleButton BtnOK;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
    }
}

