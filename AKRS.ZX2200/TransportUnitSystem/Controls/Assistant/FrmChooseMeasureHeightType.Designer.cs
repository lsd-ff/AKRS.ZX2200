namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    partial class FrmChooseMeasureHeightType
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
            this.LueHeightType = new DevExpress.XtraEditors.LookUpEdit();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueHeightType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.LueHeightType);
            this.groupControl1.Controls.Add(this.BtCancel);
            this.groupControl1.Controls.Add(this.BtOK);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(343, 149);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "测高类型";
            // 
            // LueHeightType
            // 
            this.LueHeightType.Location = new System.Drawing.Point(39, 46);
            this.LueHeightType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LueHeightType.Name = "LueHeightType";
            this.LueHeightType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueHeightType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "", 23, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.LueHeightType.Properties.DisplayMember = "Display";
            this.LueHeightType.Properties.NullText = "";
            this.LueHeightType.Properties.ValueMember = "Value";
            this.LueHeightType.Size = new System.Drawing.Size(254, 24);
            this.LueHeightType.TabIndex = 8;
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(213, 95);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(79, 35);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtOK
            // 
            this.BtOK.Location = new System.Drawing.Point(39, 95);
            this.BtOK.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.BtOK.Name = "BtOK";
            this.BtOK.Size = new System.Drawing.Size(82, 35);
            this.BtOK.TabIndex = 1;
            this.BtOK.Text = "确定";
            this.BtOK.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // FrmChooseMeasureHeightType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 149);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.Name = "FrmChooseMeasureHeightType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmChooseMeasureHeightType";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueHeightType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtOK;
        private DevExpress.XtraEditors.LookUpEdit LueHeightType;
    }
}