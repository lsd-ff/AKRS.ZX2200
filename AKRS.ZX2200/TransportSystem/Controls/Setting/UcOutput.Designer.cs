namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    partial class UcOutput
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpForwardVelRate = new DevExpress.XtraEditors.SpinEdit();
            this.TrackBarForwardVelRate = new DevExpress.XtraEditors.TrackBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForwardVelRate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarForwardVelRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarForwardVelRate.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.SpForwardVelRate);
            this.groupControl1.Controls.Add(this.TrackBarForwardVelRate);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(11, 13);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1131, 147);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "皮带设置";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(1088, 71);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(15, 18);
            this.labelControl1.TabIndex = 3;
            this.labelControl1.Text = "%";
            // 
            // SpForwardVelRate
            // 
            this.SpForwardVelRate.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpForwardVelRate.Location = new System.Drawing.Point(1018, 67);
            this.SpForwardVelRate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpForwardVelRate.Name = "SpForwardVelRate";
            this.SpForwardVelRate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpForwardVelRate.Size = new System.Drawing.Size(63, 24);
            this.SpForwardVelRate.TabIndex = 2;
            this.SpForwardVelRate.EditValueChanged += new System.EventHandler(this.SpForwardVelRate_EditValueChanged);
            // 
            // TrackBarForwardVelRate
            // 
            this.TrackBarForwardVelRate.EditValue = 20;
            this.TrackBarForwardVelRate.Location = new System.Drawing.Point(25, 51);
            this.TrackBarForwardVelRate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TrackBarForwardVelRate.Name = "TrackBarForwardVelRate";
            this.TrackBarForwardVelRate.Properties.DistanceFromTickToLabel = 10;
            this.TrackBarForwardVelRate.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TrackBarForwardVelRate.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TrackBarForwardVelRate.Properties.Maximum = 100;
            this.TrackBarForwardVelRate.Properties.Minimum = 10;
            this.TrackBarForwardVelRate.Properties.ShowLabels = true;
            this.TrackBarForwardVelRate.Properties.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TrackBarForwardVelRate.Size = new System.Drawing.Size(974, 56);
            this.TrackBarForwardVelRate.TabIndex = 1;
            this.TrackBarForwardVelRate.Value = 20;
            this.TrackBarForwardVelRate.EditValueChanged += new System.EventHandler(this.TrackBarForwardVelRate_EditValueChanged);
            // 
            // UcOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.Name = "UcOutput";
            this.Padding = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.Size = new System.Drawing.Size(1153, 923);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpForwardVelRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarForwardVelRate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarForwardVelRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpForwardVelRate;
        private DevExpress.XtraEditors.TrackBarControl TrackBarForwardVelRate;
    }
}
