namespace AKRS.ZX2200.BondSystem.BondForce.Forms
{
    partial class UcBondForceMeasurement
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
            this.BtnInspectForce = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBondingForceSensorSetUp = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBondingForce = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnRefreshForceInitialVal = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnRefreshForceInitialVal);
            this.groupControl1.Controls.Add(this.BtnInspectForce);
            this.groupControl1.Controls.Add(this.BtnBondingForceSensorSetUp);
            this.groupControl1.Controls.Add(this.BtnBondingForce);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(35, 31);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(343, 329);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "力控";
            // 
            // BtnInspectForce
            // 
            this.BtnInspectForce.Location = new System.Drawing.Point(54, 186);
            this.BtnInspectForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnInspectForce.Name = "BtnInspectForce";
            this.BtnInspectForce.Size = new System.Drawing.Size(232, 44);
            this.BtnInspectForce.TabIndex = 5;
            this.BtnInspectForce.Text = "力值检验";
            this.BtnInspectForce.Click += new System.EventHandler(this.BtnInspectForce_Click);
            // 
            // BtnBondingForceSensorSetUp
            // 
            this.BtnBondingForceSensorSetUp.Location = new System.Drawing.Point(54, 116);
            this.BtnBondingForceSensorSetUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnBondingForceSensorSetUp.Name = "BtnBondingForceSensorSetUp";
            this.BtnBondingForceSensorSetUp.Size = new System.Drawing.Size(232, 42);
            this.BtnBondingForceSensorSetUp.TabIndex = 1;
            this.BtnBondingForceSensorSetUp.Text = "力控标定";
            this.BtnBondingForceSensorSetUp.Click += new System.EventHandler(this.BtnBondingForceSensorSetUp_Click);
            // 
            // BtnBondingForce
            // 
            this.BtnBondingForce.Location = new System.Drawing.Point(54, 55);
            this.BtnBondingForce.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnBondingForce.Name = "BtnBondingForce";
            this.BtnBondingForce.Size = new System.Drawing.Size(232, 41);
            this.BtnBondingForce.TabIndex = 0;
            this.BtnBondingForce.Text = "平台压力表实时曲线";
            this.BtnBondingForce.Click += new System.EventHandler(this.BtnBondingForce_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnRefreshForceInitialVal
            // 
            this.BtnRefreshForceInitialVal.Location = new System.Drawing.Point(54, 260);
            this.BtnRefreshForceInitialVal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnRefreshForceInitialVal.Name = "BtnRefreshForceInitialVal";
            this.BtnRefreshForceInitialVal.Size = new System.Drawing.Size(232, 42);
            this.BtnRefreshForceInitialVal.TabIndex = 6;
            this.BtnRefreshForceInitialVal.Text = "焊头力控初始值刷新";
            this.BtnRefreshForceInitialVal.Click += new System.EventHandler(this.BtnRefreshForceInitialVal_Click);
            // 
            // UcBondForceMeasurement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcBondForceMeasurement";
            this.Size = new System.Drawing.Size(418, 380);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnBondingForceSensorSetUp;
        private DevExpress.XtraEditors.SimpleButton BtnBondingForce;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.SimpleButton BtnInspectForce;
        private DevExpress.XtraEditors.SimpleButton BtnRefreshForceInitialVal;
    }
}