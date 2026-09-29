namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    partial class UcMeasuringTool
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
            this.BtnSystem1FlatLevelMeasure = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSystem2FlatLevelMeasure = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLevelMeasurement = new DevExpress.XtraEditors.SimpleButton();
            this.BtDistanceMeasurement = new DevExpress.XtraEditors.SimpleButton();
            this.BtMultipleHeightMeasurement = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BtnSystem1FlatLevelMeasure);
            this.groupControl1.Controls.Add(this.BtnSystem2FlatLevelMeasure);
            this.groupControl1.Controls.Add(this.BtnLevelMeasurement);
            this.groupControl1.Controls.Add(this.BtDistanceMeasurement);
            this.groupControl1.Controls.Add(this.BtMultipleHeightMeasurement);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(452, 400);
            this.groupControl1.TabIndex = 0;
            // 
            // BtnSystem1FlatLevelMeasure
            // 
            this.BtnSystem1FlatLevelMeasure.Location = new System.Drawing.Point(10, 246);
            this.BtnSystem1FlatLevelMeasure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSystem1FlatLevelMeasure.Name = "BtnSystem1FlatLevelMeasure";
            this.BtnSystem1FlatLevelMeasure.Size = new System.Drawing.Size(431, 52);
            this.BtnSystem1FlatLevelMeasure.TabIndex = 6;
            this.BtnSystem1FlatLevelMeasure.Text = "系统1水平测试";
            this.BtnSystem1FlatLevelMeasure.Click += new System.EventHandler(this.BtnSystem1FlatLevelMeasure_Click);
            // 
            // BtnSystem2FlatLevelMeasure
            // 
            this.BtnSystem2FlatLevelMeasure.Location = new System.Drawing.Point(10, 328);
            this.BtnSystem2FlatLevelMeasure.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSystem2FlatLevelMeasure.Name = "BtnSystem2FlatLevelMeasure";
            this.BtnSystem2FlatLevelMeasure.Size = new System.Drawing.Size(431, 52);
            this.BtnSystem2FlatLevelMeasure.TabIndex = 5;
            this.BtnSystem2FlatLevelMeasure.Text = "系统2水平测试";
            this.BtnSystem2FlatLevelMeasure.Click += new System.EventHandler(this.BtnSystem2FlatLevelMeasure_Click);
            // 
            // BtnLevelMeasurement
            // 
            this.BtnLevelMeasurement.Location = new System.Drawing.Point(10, 173);
            this.BtnLevelMeasurement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnLevelMeasurement.Name = "BtnLevelMeasurement";
            this.BtnLevelMeasurement.Size = new System.Drawing.Size(431, 52);
            this.BtnLevelMeasurement.TabIndex = 4;
            this.BtnLevelMeasurement.Text = "焊头水平测试";
            this.BtnLevelMeasurement.Click += new System.EventHandler(this.BtnLevelMeasurement_Click);
            // 
            // BtDistanceMeasurement
            // 
            this.BtDistanceMeasurement.Enabled = false;
            this.BtDistanceMeasurement.Location = new System.Drawing.Point(10, 99);
            this.BtDistanceMeasurement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtDistanceMeasurement.Name = "BtDistanceMeasurement";
            this.BtDistanceMeasurement.Size = new System.Drawing.Size(431, 52);
            this.BtDistanceMeasurement.TabIndex = 2;
            this.BtDistanceMeasurement.Text = "距离测量";
            this.BtDistanceMeasurement.Click += new System.EventHandler(this.BtDistanceMeasurement_Click);
            // 
            // BtMultipleHeightMeasurement
            // 
            this.BtMultipleHeightMeasurement.Enabled = false;
            this.BtMultipleHeightMeasurement.Location = new System.Drawing.Point(10, 24);
            this.BtMultipleHeightMeasurement.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtMultipleHeightMeasurement.Name = "BtMultipleHeightMeasurement";
            this.BtMultipleHeightMeasurement.Size = new System.Drawing.Size(431, 52);
            this.BtMultipleHeightMeasurement.TabIndex = 0;
            this.BtMultipleHeightMeasurement.Text = "多功能测高";
            this.BtMultipleHeightMeasurement.Click += new System.EventHandler(this.BtMultipleHeightMeasurement_Click);
            // 
            // timer1
            // 
            this.timer1.Tag = "UcMeasuringTool";
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // UcMeasuringTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcMeasuringTool";
            this.Size = new System.Drawing.Size(452, 400);
            this.Load += new System.EventHandler(this.UcMeasuringTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtMultipleHeightMeasurement;
        private DevExpress.XtraEditors.SimpleButton BtDistanceMeasurement;
        private DevExpress.XtraEditors.SimpleButton BtnLevelMeasurement;
        private DevExpress.XtraEditors.SimpleButton BtnSystem2FlatLevelMeasure;
        private DevExpress.XtraEditors.SimpleButton BtnSystem1FlatLevelMeasure;
        private System.Windows.Forms.Timer timer1;
    }
}