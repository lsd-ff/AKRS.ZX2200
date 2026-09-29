namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    partial class UcBondMaxSubSection
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.LueBondMaxSubSize = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.LueBondMaxSubSize.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(3, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(48, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "载台大小";
            // 
            // LueBondMaxSubSize
            // 
            this.LueBondMaxSubSize.Location = new System.Drawing.Point(80, 30);
            this.LueBondMaxSubSize.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LueBondMaxSubSize.Name = "LueBondMaxSubSize";
            this.LueBondMaxSubSize.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueBondMaxSubSize.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "")});
            this.LueBondMaxSubSize.Properties.DisplayMember = "Display";
            this.LueBondMaxSubSize.Properties.NullText = "";
            this.LueBondMaxSubSize.Properties.ValueMember = "Value";
            this.LueBondMaxSubSize.Size = new System.Drawing.Size(233, 20);
            this.LueBondMaxSubSize.TabIndex = 20;
            // 
            // UcBondMaxSubSection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LueBondMaxSubSize);
            this.Controls.Add(this.labelControl1);
            this.Name = "UcBondMaxSubSection";
            this.Size = new System.Drawing.Size(667, 590);
            ((System.ComponentModel.ISupportInitialize)(this.LueBondMaxSubSize.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit LueBondMaxSubSize;
    }
}
