namespace AKRS.Galaxy2.Dispense
{
    partial class SaveEpoxyPatternDialogBox
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
            this.labelX3 = new System.Windows.Forms.Label();
            this.labelX1 = new System.Windows.Forms.Label();
            this.buttonXCancel = new System.Windows.Forms.Button();
            this.buttonXConfirm = new System.Windows.Forms.Button();
            this.textBoxPatternName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold);
            this.labelX3.Location = new System.Drawing.Point(143, 12);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(102, 22);
            this.labelX3.TabIndex = 8;
            this.labelX3.Text = "添加入库";
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(12, 70);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(88, 16);
            this.labelX1.TabIndex = 8;
            this.labelX1.Text = "图形名称：";
            // 
            // buttonXCancel
            // 
            this.buttonXCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonXCancel.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXCancel.Location = new System.Drawing.Point(195, 110);
            this.buttonXCancel.Name = "buttonXCancel";
            this.buttonXCancel.Size = new System.Drawing.Size(100, 44);
            this.buttonXCancel.TabIndex = 3;
            this.buttonXCancel.Text = "取消";
            // 
            // buttonXConfirm
            // 
            this.buttonXConfirm.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXConfirm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXConfirm.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXConfirm.Location = new System.Drawing.Point(89, 110);
            this.buttonXConfirm.Name = "buttonXConfirm";
            this.buttonXConfirm.Size = new System.Drawing.Size(100, 44);
            this.buttonXConfirm.TabIndex = 2;
            this.buttonXConfirm.Text = "确定";
            this.buttonXConfirm.Click += new System.EventHandler(this.buttonXConfirm_Click);
            // 
            // textBoxPatternName
            // 
            this.textBoxPatternName.Font = new System.Drawing.Font("SimSun", 12F);
            this.textBoxPatternName.Location = new System.Drawing.Point(102, 70);
            this.textBoxPatternName.MaxLength = 30;
            this.textBoxPatternName.Multiline = true;
            this.textBoxPatternName.Name = "textBoxPatternName";
            this.textBoxPatternName.Size = new System.Drawing.Size(271, 23);
            this.textBoxPatternName.TabIndex = 1;
            this.textBoxPatternName.Text = "12343456789012345678901234567890\r\n";
            // 
            // SaveEpoxyPatternDialogBox
            // 
            this.AcceptButton = this.buttonXConfirm;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.CancelButton = this.buttonXCancel;
            this.ClientSize = new System.Drawing.Size(384, 166);
            this.ControlBox = false;
            this.Controls.Add(this.textBoxPatternName);
            this.Controls.Add(this.buttonXCancel);
            this.Controls.Add(this.buttonXConfirm);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SaveEpoxyPatternDialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "保存图形";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Label labelX1;
        private System.Windows.Forms.Button buttonXCancel;
        private System.Windows.Forms.Button buttonXConfirm;
        private System.Windows.Forms.TextBox textBoxPatternName;
    }
}