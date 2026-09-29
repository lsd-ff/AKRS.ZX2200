namespace AKRS.ZX2200.SupportFeature.Logs
{
    partial class FrmLogRecord
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GcLogRecord = new DevExpress.XtraGrid.GridControl();
            this.GvLogRecord = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.BtnStart = new DevExpress.XtraEditors.SimpleButton();
            this.BtnClear = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GcLogRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvLogRecord)).BeginInit();
            this.SuspendLayout();
            // 
            // GcLogRecord
            // 
            this.GcLogRecord.Location = new System.Drawing.Point(18, 19);
            this.GcLogRecord.MainView = this.GvLogRecord;
            this.GcLogRecord.Name = "GcLogRecord";
            this.GcLogRecord.Size = new System.Drawing.Size(949, 587);
            this.GcLogRecord.TabIndex = 0;
            this.GcLogRecord.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvLogRecord});
            // 
            // GvLogRecord
            // 
            this.GvLogRecord.GridControl = this.GcLogRecord;
            this.GvLogRecord.Name = "GvLogRecord";
            this.GvLogRecord.OptionsBehavior.Editable = false;
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(892, 637);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 23);
            this.BtnStart.TabIndex = 1;
            this.BtnStart.Text = "开启";
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(792, 637);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 23);
            this.BtnClear.TabIndex = 2;
            this.BtnClear.Text = "清空";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // FrmLogRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 689);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnStart);
            this.Controls.Add(this.GcLogRecord);
            this.Name = "FrmLogRecord";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmLogRecord_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GcLogRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvLogRecord)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl GcLogRecord;
        private DevExpress.XtraGrid.Views.Grid.GridView GvLogRecord;
        private DevExpress.XtraEditors.SimpleButton BtnStart;
        private DevExpress.XtraEditors.SimpleButton BtnClear;
    }
}
