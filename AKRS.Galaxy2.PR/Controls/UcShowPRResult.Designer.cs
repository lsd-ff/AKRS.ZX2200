namespace AKRS.Galaxy2.PR.Controls
{
    partial class UcShowPRResult
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.LbTime = new DevExpress.XtraEditors.LabelControl();
            this.LbPRName = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.LbAngle = new DevExpress.XtraEditors.LabelControl();
            this.LbY = new DevExpress.XtraEditors.LabelControl();
            this.LbX = new DevExpress.XtraEditors.LabelControl();
            this.LbScore = new DevExpress.XtraEditors.LabelControl();
            this.PePRResultImage = new DevExpress.XtraEditors.PictureEdit();
            this.tablePanel3 = new DevExpress.Utils.Layout.TablePanel();
            this.LbTimes = new DevExpress.XtraEditors.LabelControl();
            this.LbWidth = new DevExpress.XtraEditors.LabelControl();
            this.LbLength = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PePRResultImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).BeginInit();
            this.tablePanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 56.29F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 22.07F)});
            this.tablePanel1.Controls.Add(this.LbTime);
            this.tablePanel1.Controls.Add(this.LbPRName);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(364, 23);
            this.tablePanel1.TabIndex = 0;
            // 
            // LbTime
            // 
            this.LbTime.Location = new System.Drawing.Point(264, 4);
            this.LbTime.Name = "LbTime";
            this.LbTime.Size = new System.Drawing.Size(0, 14);
            this.LbTime.TabIndex = 1;
            // 
            // LbPRName
            // 
            this.LbPRName.Appearance.Font = new System.Drawing.Font("黑体", 11F);
            this.LbPRName.Appearance.Options.UseFont = true;
            this.LbPRName.Location = new System.Drawing.Point(3, 4);
            this.LbPRName.Name = "LbPRName";
            this.LbPRName.Size = new System.Drawing.Size(0, 13);
            this.LbPRName.TabIndex = 0;
            // 
            // tablePanel2
            // 
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 36.5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 41.86F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel2.Controls.Add(this.LbAngle);
            this.tablePanel2.Controls.Add(this.LbY);
            this.tablePanel2.Controls.Add(this.LbX);
            this.tablePanel2.Controls.Add(this.LbScore);
            this.tablePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel2.Location = new System.Drawing.Point(0, 23);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel2.Size = new System.Drawing.Size(364, 19);
            this.tablePanel2.TabIndex = 1;
            // 
            // LbAngle
            // 
            this.LbAngle.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbAngle.Appearance.Options.UseFont = true;
            this.tablePanel2.SetColumn(this.LbAngle, 3);
            this.LbAngle.Location = new System.Drawing.Point(265, 3);
            this.LbAngle.Name = "LbAngle";
            this.tablePanel2.SetRow(this.LbAngle, 0);
            this.LbAngle.Size = new System.Drawing.Size(49, 13);
            this.LbAngle.TabIndex = 2;
            this.LbAngle.Text = "Angle：";
            // 
            // LbY
            // 
            this.LbY.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbY.Appearance.Options.UseFont = true;
            this.tablePanel2.SetColumn(this.LbY, 2);
            this.LbY.Location = new System.Drawing.Point(163, 3);
            this.LbY.Name = "LbY";
            this.tablePanel2.SetRow(this.LbY, 0);
            this.LbY.Size = new System.Drawing.Size(21, 13);
            this.LbY.TabIndex = 2;
            this.LbY.Text = "Y：";
            // 
            // LbX
            // 
            this.LbX.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbX.Appearance.Options.UseFont = true;
            this.tablePanel2.SetColumn(this.LbX, 1);
            this.LbX.Location = new System.Drawing.Point(77, 3);
            this.LbX.Name = "LbX";
            this.tablePanel2.SetRow(this.LbX, 0);
            this.LbX.Size = new System.Drawing.Size(21, 13);
            this.LbX.TabIndex = 1;
            this.LbX.Text = "X：";
            // 
            // LbScore
            // 
            this.LbScore.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbScore.Appearance.Options.UseFont = true;
            this.tablePanel2.SetColumn(this.LbScore, 0);
            this.LbScore.Location = new System.Drawing.Point(3, 3);
            this.LbScore.Name = "LbScore";
            this.tablePanel2.SetRow(this.LbScore, 0);
            this.LbScore.Size = new System.Drawing.Size(28, 13);
            this.LbScore.TabIndex = 0;
            this.LbScore.Text = "分值";
            // 
            // PePRResultImage
            // 
            this.PePRResultImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PePRResultImage.Location = new System.Drawing.Point(0, 61);
            this.PePRResultImage.Name = "PePRResultImage";
            this.PePRResultImage.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.PePRResultImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.PePRResultImage.Size = new System.Drawing.Size(364, 305);
            this.PePRResultImage.TabIndex = 2;
            // 
            // tablePanel3
            // 
            this.tablePanel3.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 36.5F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 41.86F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel3.Controls.Add(this.LbTimes);
            this.tablePanel3.Controls.Add(this.LbWidth);
            this.tablePanel3.Controls.Add(this.LbLength);
            this.tablePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel3.Location = new System.Drawing.Point(0, 42);
            this.tablePanel3.Name = "tablePanel3";
            this.tablePanel3.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel3.Size = new System.Drawing.Size(364, 19);
            this.tablePanel3.TabIndex = 3;
            // 
            // LbTimes
            // 
            this.LbTimes.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbTimes.Appearance.Options.UseFont = true;
            this.tablePanel3.SetColumn(this.LbTimes, 2);
            this.LbTimes.Location = new System.Drawing.Point(163, 3);
            this.LbTimes.Name = "LbTimes";
            this.tablePanel3.SetRow(this.LbTimes, 0);
            this.LbTimes.Size = new System.Drawing.Size(35, 13);
            this.LbTimes.TabIndex = 3;
            this.LbTimes.Text = "耗时:";
            // 
            // LbWidth
            // 
            this.LbWidth.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbWidth.Appearance.Options.UseFont = true;
            this.tablePanel3.SetColumn(this.LbWidth, 1);
            this.LbWidth.Location = new System.Drawing.Point(77, 3);
            this.LbWidth.Name = "LbWidth";
            this.tablePanel3.SetRow(this.LbWidth, 0);
            this.LbWidth.Size = new System.Drawing.Size(21, 13);
            this.LbWidth.TabIndex = 2;
            this.LbWidth.Text = "宽:";
            // 
            // LbLength
            // 
            this.LbLength.Appearance.Font = new System.Drawing.Font("黑体", 10F);
            this.LbLength.Appearance.Options.UseFont = true;
            this.tablePanel3.SetColumn(this.LbLength, 0);
            this.LbLength.Location = new System.Drawing.Point(3, 3);
            this.LbLength.Name = "LbLength";
            this.tablePanel3.SetRow(this.LbLength, 0);
            this.LbLength.Size = new System.Drawing.Size(21, 13);
            this.LbLength.TabIndex = 0;
            this.LbLength.Text = "长:";
            // 
            // UcShowPRResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PePRResultImage);
            this.Controls.Add(this.tablePanel3);
            this.Controls.Add(this.tablePanel2);
            this.Controls.Add(this.tablePanel1);
            this.Name = "UcShowPRResult";
            this.Size = new System.Drawing.Size(364, 366);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            this.tablePanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PePRResultImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).EndInit();
            this.tablePanel3.ResumeLayout(false);
            this.tablePanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.LabelControl LbPRName;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.LabelControl LbAngle;
        private DevExpress.XtraEditors.LabelControl LbY;
        private DevExpress.XtraEditors.LabelControl LbX;
        private DevExpress.XtraEditors.LabelControl LbScore;
        private DevExpress.XtraEditors.PictureEdit PePRResultImage;
        private DevExpress.XtraEditors.LabelControl LbTime;
        private DevExpress.Utils.Layout.TablePanel tablePanel3;
        private DevExpress.XtraEditors.LabelControl LbWidth;
        private DevExpress.XtraEditors.LabelControl LbLength;
        private DevExpress.XtraEditors.LabelControl LbTimes;
    }
}
