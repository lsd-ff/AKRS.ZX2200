namespace AKRS.ZX2200.Infrastructure.Controls.Feature.MeasuringTool
{
    partial class FrmMultipleHeightMeasurementType
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
            this.LueAdjustType = new DevExpress.XtraEditors.LookUpEdit();
            this.GpType = new DevExpress.XtraEditors.GroupControl();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.LueAdjustType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpType)).BeginInit();
            this.GpType.SuspendLayout();
            this.SuspendLayout();
            // 
            // LueAdjustType
            // 
            this.LueAdjustType.Location = new System.Drawing.Point(13, 42);
            this.LueAdjustType.Margin = new System.Windows.Forms.Padding(4);
            this.LueAdjustType.Name = "LueAdjustType";
            this.LueAdjustType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAdjustType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueAdjustType.Properties.DisplayMember = "Display";
            this.LueAdjustType.Properties.NullText = "";
            this.LueAdjustType.Properties.ValueMember = "Value";
            this.LueAdjustType.Size = new System.Drawing.Size(400, 24);
            this.LueAdjustType.TabIndex = 12;
            this.LueAdjustType.EditValueChanged += new System.EventHandler(this.LueAdjustType_EditValueChanged);
            // 
            // GpType
            // 
            this.GpType.Controls.Add(this.BtNext);
            this.GpType.Controls.Add(this.LueAdjustType);
            this.GpType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GpType.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpType.Location = new System.Drawing.Point(0, 0);
            this.GpType.Name = "GpType";
            this.GpType.Size = new System.Drawing.Size(427, 132);
            this.GpType.TabIndex = 13;
            this.GpType.Text = "Multiple Height Measurement Type";
            // 
            // BtNext
            // 
            this.BtNext.Location = new System.Drawing.Point(300, 91);
            this.BtNext.Name = "BtNext";
            this.BtNext.Size = new System.Drawing.Size(113, 29);
            this.BtNext.TabIndex = 13;
            this.BtNext.Text = "Next";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // FrmMultipleHeightMeasurementType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 132);
            this.Controls.Add(this.GpType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmMultipleHeightMeasurementType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmMultipleHeightMeasurementType";
            this.Load += new System.EventHandler(this.FrmMultipleHeightMeasurementType_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LueAdjustType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpType)).EndInit();
            this.GpType.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LookUpEdit LueAdjustType;
        private DevExpress.XtraEditors.GroupControl GpType;
        private DevExpress.XtraEditors.SimpleButton BtNext;
    }
}