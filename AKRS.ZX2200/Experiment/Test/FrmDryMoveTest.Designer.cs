namespace AKRS.ZX2200.Experiment.Test
{
    partial class FrmDryMoveTest
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
            this.BtStop = new DevExpress.XtraEditors.SimpleButton();
            this.BtWaferTableTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtDispenseMoveTest = new DevExpress.XtraEditors.SimpleButton();
            this.BtBondMoveTest = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // BtStop
            // 
            this.BtStop.Location = new System.Drawing.Point(203, 179);
            this.BtStop.Name = "BtStop";
            this.BtStop.Size = new System.Drawing.Size(178, 57);
            this.BtStop.TabIndex = 36;
            this.BtStop.Text = "停止";
            this.BtStop.Click += new System.EventHandler(this.BtStop_Click);
            // 
            // BtWaferTableTest
            // 
            this.BtWaferTableTest.Location = new System.Drawing.Point(364, 83);
            this.BtWaferTableTest.Name = "BtWaferTableTest";
            this.BtWaferTableTest.Size = new System.Drawing.Size(100, 32);
            this.BtWaferTableTest.TabIndex = 35;
            this.BtWaferTableTest.Text = "晶圆台运动测试";
            this.BtWaferTableTest.Click += new System.EventHandler(this.BtWaferTableTest_Click);
            // 
            // BtDispenseMoveTest
            // 
            this.BtDispenseMoveTest.Location = new System.Drawing.Point(229, 83);
            this.BtDispenseMoveTest.Name = "BtDispenseMoveTest";
            this.BtDispenseMoveTest.Size = new System.Drawing.Size(100, 32);
            this.BtDispenseMoveTest.TabIndex = 34;
            this.BtDispenseMoveTest.Text = "点胶运动测试";
            this.BtDispenseMoveTest.Click += new System.EventHandler(this.BtDispenseMoveTest_Click);
            // 
            // BtBondMoveTest
            // 
            this.BtBondMoveTest.Location = new System.Drawing.Point(79, 83);
            this.BtBondMoveTest.Name = "BtBondMoveTest";
            this.BtBondMoveTest.Size = new System.Drawing.Size(100, 32);
            this.BtBondMoveTest.TabIndex = 33;
            this.BtBondMoveTest.Text = "Bond运动测试";
            this.BtBondMoveTest.Click += new System.EventHandler(this.BtBondMoveTest_Click);
            // 
            // FrmDryMoveTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 401);
            this.Controls.Add(this.BtStop);
            this.Controls.Add(this.BtWaferTableTest);
            this.Controls.Add(this.BtDispenseMoveTest);
            this.Controls.Add(this.BtBondMoveTest);
            this.Name = "FrmDryMoveTest";
            this.Text = "Dry Move Test";
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtStop;
        private DevExpress.XtraEditors.SimpleButton BtWaferTableTest;
        private DevExpress.XtraEditors.SimpleButton BtDispenseMoveTest;
        private DevExpress.XtraEditors.SimpleButton BtBondMoveTest;
    }
}