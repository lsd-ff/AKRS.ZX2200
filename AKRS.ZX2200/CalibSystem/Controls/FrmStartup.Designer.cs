namespace AKRS.ZX2200.CalibSystem.Controls
{
    partial class FrmStartup
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtLaserCalibration = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBondToCameraHighAccuracy = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBondToCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnBDCalib = new DevExpress.XtraEditors.SimpleButton();
            this.btnOnlyDispenseCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnOnlyWaferCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnOnlyUpLookCamera = new DevExpress.XtraEditors.SimpleButton();
            this.btnOnlyBondCamera = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtLaserCalibration);
            this.groupControl1.Controls.Add(this.simpleButton1);
            this.groupControl1.Controls.Add(this.BtnBondToCameraHighAccuracy);
            this.groupControl1.Controls.Add(this.BtnBondToCamera);
            this.groupControl1.Controls.Add(this.btnBDCalib);
            this.groupControl1.Controls.Add(this.btnOnlyDispenseCamera);
            this.groupControl1.Controls.Add(this.btnOnlyWaferCamera);
            this.groupControl1.Controls.Add(this.btnOnlyUpLookCamera);
            this.groupControl1.Controls.Add(this.btnOnlyBondCamera);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(479, 319);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Special";
            // 
            // BtLaserCalibration
            // 
            this.BtLaserCalibration.Location = new System.Drawing.Point(338, 185);
            this.BtLaserCalibration.Name = "BtLaserCalibration";
            this.BtLaserCalibration.Size = new System.Drawing.Size(129, 27);
            this.BtLaserCalibration.TabIndex = 8;
            this.BtLaserCalibration.Text = "激光测高标定";
            this.BtLaserCalibration.Click += new System.EventHandler(this.BtLaserCalibration_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.Location = new System.Drawing.Point(345, 231);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(129, 27);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "BondToCamera";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // BtnBondToCameraHighAccuracy
            // 
            this.BtnBondToCameraHighAccuracy.Location = new System.Drawing.Point(338, 94);
            this.BtnBondToCameraHighAccuracy.Name = "BtnBondToCameraHighAccuracy";
            this.BtnBondToCameraHighAccuracy.Size = new System.Drawing.Size(141, 27);
            this.BtnBondToCameraHighAccuracy.TabIndex = 6;
            this.BtnBondToCameraHighAccuracy.Text = "BondToCameraHighCalib";
            this.BtnBondToCameraHighAccuracy.Click += new System.EventHandler(this.BtnBondToCameraHighAccuracy_Click);
            // 
            // BtnBondToCamera
            // 
            this.BtnBondToCamera.Location = new System.Drawing.Point(12, 231);
            this.BtnBondToCamera.Name = "BtnBondToCamera";
            this.BtnBondToCamera.Size = new System.Drawing.Size(129, 27);
            this.BtnBondToCamera.TabIndex = 5;
            this.BtnBondToCamera.Text = "BondToCamera";
            this.BtnBondToCamera.Click += new System.EventHandler(this.BtnBondToCamera_Click);
            // 
            // btnBDCalib
            // 
            this.btnBDCalib.Location = new System.Drawing.Point(338, 48);
            this.btnBDCalib.Name = "btnBDCalib";
            this.btnBDCalib.Size = new System.Drawing.Size(129, 27);
            this.btnBDCalib.TabIndex = 4;
            this.btnBDCalib.Text = "轨道系统标定";
            this.btnBDCalib.Click += new System.EventHandler(this.btnBDCalib_Click);
            // 
            // btnOnlyDispenseCamera
            // 
            this.btnOnlyDispenseCamera.Location = new System.Drawing.Point(12, 185);
            this.btnOnlyDispenseCamera.Name = "btnOnlyDispenseCamera";
            this.btnOnlyDispenseCamera.Size = new System.Drawing.Size(129, 27);
            this.btnOnlyDispenseCamera.TabIndex = 3;
            this.btnOnlyDispenseCamera.Text = "点胶相机标定";
            this.btnOnlyDispenseCamera.Click += new System.EventHandler(this.btnOnlyDispenseCamera_Click);
            // 
            // btnOnlyWaferCamera
            // 
            this.btnOnlyWaferCamera.Location = new System.Drawing.Point(12, 139);
            this.btnOnlyWaferCamera.Name = "btnOnlyWaferCamera";
            this.btnOnlyWaferCamera.Size = new System.Drawing.Size(129, 27);
            this.btnOnlyWaferCamera.TabIndex = 2;
            this.btnOnlyWaferCamera.Text = "晶圆相机标定";
            this.btnOnlyWaferCamera.Click += new System.EventHandler(this.btnOnlyWaferCamera_Click);
            // 
            // btnOnlyUpLookCamera
            // 
            this.btnOnlyUpLookCamera.Location = new System.Drawing.Point(12, 94);
            this.btnOnlyUpLookCamera.Name = "btnOnlyUpLookCamera";
            this.btnOnlyUpLookCamera.Size = new System.Drawing.Size(129, 27);
            this.btnOnlyUpLookCamera.TabIndex = 1;
            this.btnOnlyUpLookCamera.Text = "上视相机标定";
            this.btnOnlyUpLookCamera.Click += new System.EventHandler(this.btnOnlyUpLookCamera_Click);
            // 
            // btnOnlyBondCamera
            // 
            this.btnOnlyBondCamera.Location = new System.Drawing.Point(12, 48);
            this.btnOnlyBondCamera.Name = "btnOnlyBondCamera";
            this.btnOnlyBondCamera.Size = new System.Drawing.Size(129, 27);
            this.btnOnlyBondCamera.TabIndex = 0;
            this.btnOnlyBondCamera.Text = "固晶相机标定";
            this.btnOnlyBondCamera.Click += new System.EventHandler(this.btnOnlyBondCamera_Click);
            // 
            // FrmStartup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 319);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmStartup";
            this.Text = "Single Calibration";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnOnlyDispenseCamera;
        private DevExpress.XtraEditors.SimpleButton btnOnlyWaferCamera;
        private DevExpress.XtraEditors.SimpleButton btnOnlyUpLookCamera;
        private DevExpress.XtraEditors.SimpleButton btnOnlyBondCamera;
        private DevExpress.XtraEditors.SimpleButton btnBDCalib;
        private DevExpress.XtraEditors.SimpleButton BtnBondToCamera;
        private DevExpress.XtraEditors.SimpleButton BtnBondToCameraHighAccuracy;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.SimpleButton BtLaserCalibration;
    }
}