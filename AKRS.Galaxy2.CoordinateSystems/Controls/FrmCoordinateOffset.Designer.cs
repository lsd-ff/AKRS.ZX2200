namespace AKRS.Galaxy2.CoordinateSystems.Controls
{
    partial class FrmCoordinateOffset
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
            this.CmbCoordinate = new DevExpress.XtraEditors.ComboBoxEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.spinEdit1 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit2 = new DevExpress.XtraEditors.SpinEdit();
            this.spinEdit3 = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.BtSave = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.SpOffsetAngle = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.CmbCoordinate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetAngle.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // CmbCoordinate
            // 
            this.CmbCoordinate.Location = new System.Drawing.Point(161, 58);
            this.CmbCoordinate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CmbCoordinate.Name = "CmbCoordinate";
            this.CmbCoordinate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbCoordinate.Size = new System.Drawing.Size(175, 20);
            this.CmbCoordinate.TabIndex = 0;
            this.CmbCoordinate.SelectedIndexChanged += new System.EventHandler(this.CmbCoordinate_SelectedIndexChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 60);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "坐标系名称";
            // 
            // spinEdit1
            // 
            this.spinEdit1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit1.Location = new System.Drawing.Point(161, 157);
            this.spinEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEdit1.Name = "spinEdit1";
            this.spinEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit1.Size = new System.Drawing.Size(109, 20);
            this.spinEdit1.TabIndex = 2;
            // 
            // spinEdit2
            // 
            this.spinEdit2.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit2.Location = new System.Drawing.Point(161, 205);
            this.spinEdit2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEdit2.Name = "spinEdit2";
            this.spinEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit2.Size = new System.Drawing.Size(109, 20);
            this.spinEdit2.TabIndex = 3;
            // 
            // spinEdit3
            // 
            this.spinEdit3.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinEdit3.Location = new System.Drawing.Point(161, 256);
            this.spinEdit3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.spinEdit3.Name = "spinEdit3";
            this.spinEdit3.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinEdit3.Size = new System.Drawing.Size(109, 20);
            this.spinEdit3.TabIndex = 4;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(18, 162);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(43, 14);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "偏移值X";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 210);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(44, 14);
            this.labelControl3.TabIndex = 6;
            this.labelControl3.Text = "偏移值Y";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(18, 260);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(43, 14);
            this.labelControl4.TabIndex = 7;
            this.labelControl4.Text = "偏移值Z";
            // 
            // BtSave
            // 
            this.BtSave.Location = new System.Drawing.Point(161, 354);
            this.BtSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtSave.Name = "BtSave";
            this.BtSave.Size = new System.Drawing.Size(125, 70);
            this.BtSave.TabIndex = 8;
            this.BtSave.Text = "保存";
            this.BtSave.Click += new System.EventHandler(this.BtOffsetAdd_Click);
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(20, 304);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(48, 14);
            this.labelControl5.TabIndex = 9;
            this.labelControl5.Text = "偏移角度";
            // 
            // SpOffsetAngle
            // 
            this.SpOffsetAngle.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpOffsetAngle.Location = new System.Drawing.Point(161, 299);
            this.SpOffsetAngle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpOffsetAngle.Name = "SpOffsetAngle";
            this.SpOffsetAngle.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpOffsetAngle.Size = new System.Drawing.Size(109, 20);
            this.SpOffsetAngle.TabIndex = 10;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(18, 114);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(84, 14);
            this.labelControl6.TabIndex = 11;
            this.labelControl6.Text = "上层坐标系名称";
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(161, 114);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(84, 14);
            this.labelControl7.TabIndex = 12;
            this.labelControl7.Text = "上层坐标系名称";
            // 
            // FrmCoordinateOffset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 464);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.labelControl6);
            this.Controls.Add(this.SpOffsetAngle);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.BtSave);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.spinEdit3);
            this.Controls.Add(this.spinEdit2);
            this.Controls.Add(this.spinEdit1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.CmbCoordinate);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmCoordinateOffset";
            this.Text = "坐标系偏移";
            this.Load += new System.EventHandler(this.FrmCoordinateOffset_Load);
            ((System.ComponentModel.ISupportInitialize)(this.CmbCoordinate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpOffsetAngle.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ComboBoxEdit CmbCoordinate;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit spinEdit1;
        private DevExpress.XtraEditors.SpinEdit spinEdit2;
        private DevExpress.XtraEditors.SpinEdit spinEdit3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton BtSave;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.SpinEdit SpOffsetAngle;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
    }
}