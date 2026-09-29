namespace AKRS.Galaxy2.Dispense
{
    partial class ManageRepositoryDialogBox
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.buttonXExit = new System.Windows.Forms.Button();
            this.buttonXDeleteSelectedItems = new System.Windows.Forms.Button();
            this.labelX1 = new System.Windows.Forms.Label();
            this.labelX3 = new System.Windows.Forms.Label();
            this.buttonXDeleteAllItems = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.listBox1.Location = new System.Drawing.Point(14, 78);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(360, 132);
            this.listBox1.TabIndex = 14;
            // 
            // buttonXExit
            // 
            this.buttonXExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonXExit.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXExit.Location = new System.Drawing.Point(272, 303);
            this.buttonXExit.Name = "buttonXExit";
            this.buttonXExit.Size = new System.Drawing.Size(100, 44);
            this.buttonXExit.TabIndex = 16;
            this.buttonXExit.Text = "退出";
            this.buttonXExit.Click += new System.EventHandler(this.buttonXExit_Click);
            // 
            // buttonXDeleteSelectedItems
            // 
            this.buttonXDeleteSelectedItems.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXDeleteSelectedItems.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXDeleteSelectedItems.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXDeleteSelectedItems.Location = new System.Drawing.Point(12, 233);
            this.buttonXDeleteSelectedItems.Name = "buttonXDeleteSelectedItems";
            this.buttonXDeleteSelectedItems.Size = new System.Drawing.Size(100, 34);
            this.buttonXDeleteSelectedItems.TabIndex = 15;
            this.buttonXDeleteSelectedItems.Text = "删除选中";
            this.buttonXDeleteSelectedItems.Click += new System.EventHandler(this.buttonXDeleteSelectedItems_Click);
            // 
            // labelX1
            // 
            this.labelX1.AutoSize = true;
            this.labelX1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelX1.Location = new System.Drawing.Point(14, 49);
            this.labelX1.Name = "labelX1";
            this.labelX1.Size = new System.Drawing.Size(88, 16);
            this.labelX1.TabIndex = 17;
            this.labelX1.Text = "图形名称：";
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold);
            this.labelX3.Location = new System.Drawing.Point(145, 12);
            this.labelX3.Name = "labelX3";
            this.labelX3.Size = new System.Drawing.Size(102, 22);
            this.labelX3.TabIndex = 18;
            this.labelX3.Text = "调用图库";
            // 
            // buttonXDeleteAllItems
            // 
            this.buttonXDeleteAllItems.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXDeleteAllItems.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXDeleteAllItems.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXDeleteAllItems.Location = new System.Drawing.Point(118, 233);
            this.buttonXDeleteAllItems.Name = "buttonXDeleteAllItems";
            this.buttonXDeleteAllItems.Size = new System.Drawing.Size(100, 34);
            this.buttonXDeleteAllItems.TabIndex = 15;
            this.buttonXDeleteAllItems.Text = "删除所有";
            this.buttonXDeleteAllItems.Click += new System.EventHandler(this.buttonXDeleteAllItems_Click);
            // 
            // ManageRepositoryDialogBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 359);
            this.ControlBox = false;
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.buttonXExit);
            this.Controls.Add(this.buttonXDeleteAllItems);
            this.Controls.Add(this.buttonXDeleteSelectedItems);
            this.Controls.Add(this.labelX1);
            this.Controls.Add(this.labelX3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ManageRepositoryDialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "管理图库";
            this.Load += new System.EventHandler(this.ManageRepositoryDialogBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button buttonXExit;
        private System.Windows.Forms.Button buttonXDeleteSelectedItems;
        private System.Windows.Forms.Label labelX1;
        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Button buttonXDeleteAllItems;
    }
}