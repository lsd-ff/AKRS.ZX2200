namespace AKRS.ZX2200.CalibSystem.GlobalCalibration;

partial class FrmNewCompensationRegion
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.TxtRegionName = new DevExpress.XtraEditors.TextEdit();
            this.SpPriority = new DevExpress.XtraEditors.SpinEdit();
            this.BtnConfirm = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TxtRegionName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPriority.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(64, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "区域名称";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(52, 65);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(60, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "区域优先级";
            // 
            // TxtRegionName
            // 
            this.TxtRegionName.Location = new System.Drawing.Point(118, 20);
            this.TxtRegionName.Name = "TxtRegionName";
            this.TxtRegionName.Size = new System.Drawing.Size(138, 20);
            this.TxtRegionName.TabIndex = 1;
            // 
            // SpPriority
            // 
            this.SpPriority.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpPriority.Location = new System.Drawing.Point(118, 62);
            this.SpPriority.Name = "SpPriority";
            this.SpPriority.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpPriority.Properties.IsFloatValue = false;
            this.SpPriority.Properties.MaskSettings.Set("mask", "N00");
            this.SpPriority.Size = new System.Drawing.Size(138, 20);
            this.SpPriority.TabIndex = 2;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.Location = new System.Drawing.Point(135, 103);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(75, 28);
            this.BtnConfirm.TabIndex = 3;
            this.BtnConfirm.Text = "确认";
            this.BtnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // FrmNewCompensationRegion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 153);
            this.Controls.Add(this.BtnConfirm);
            this.Controls.Add(this.SpPriority);
            this.Controls.Add(this.TxtRegionName);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FrmNewCompensationRegion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "新补偿区域创建";
            this.Load += new System.EventHandler(this.FrmNewCompensationRegion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TxtRegionName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPriority.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private DevExpress.XtraEditors.LabelControl labelControl1;
    private DevExpress.XtraEditors.LabelControl labelControl2;
    private DevExpress.XtraEditors.TextEdit TxtRegionName;
    private DevExpress.XtraEditors.SpinEdit SpPriority;
    private DevExpress.XtraEditors.SimpleButton BtnConfirm;
}