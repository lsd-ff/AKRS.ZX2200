namespace AKRS.Galaxy2.Dispense
{
    partial class ManageRepositoryExDialogBox
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
            this.components = new System.ComponentModel.Container();
            this.buttonXExit = new System.Windows.Forms.Button();
            this.buttonXDeleteSelectedItems = new System.Windows.Forms.Button();
            this.labelX3 = new System.Windows.Forms.Label();
            this.buttonXDeleteAllItems = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonXExit
            // 
            this.buttonXExit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonXExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.buttonXExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonXExit.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonXExit.Location = new System.Drawing.Point(272, 3);
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
            this.buttonXDeleteSelectedItems.Location = new System.Drawing.Point(12, 4);
            this.buttonXDeleteSelectedItems.Name = "buttonXDeleteSelectedItems";
            this.buttonXDeleteSelectedItems.Size = new System.Drawing.Size(100, 34);
            this.buttonXDeleteSelectedItems.TabIndex = 15;
            this.buttonXDeleteSelectedItems.Text = "删除选中";
            this.buttonXDeleteSelectedItems.Click += new System.EventHandler(this.buttonXDeleteSelectedItems_Click);
            // 
            // labelX3
            // 
            this.labelX3.AutoSize = true;
            this.labelX3.Font = new System.Drawing.Font("SimSun", 16F, System.Drawing.FontStyle.Bold);
            this.labelX3.Location = new System.Drawing.Point(141, 12);
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
            this.buttonXDeleteAllItems.Location = new System.Drawing.Point(118, 4);
            this.buttonXDeleteAllItems.Name = "buttonXDeleteAllItems";
            this.buttonXDeleteAllItems.Size = new System.Drawing.Size(100, 34);
            this.buttonXDeleteAllItems.TabIndex = 15;
            this.buttonXDeleteAllItems.Text = "删除所有";
            this.buttonXDeleteAllItems.Click += new System.EventHandler(this.buttonXDeleteAllItems_Click);
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 46);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(384, 222);
            this.listView1.TabIndex = 19;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelX3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(384, 46);
            this.panel1.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonXExit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 309);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(384, 50);
            this.panel2.TabIndex = 21;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonXDeleteSelectedItems);
            this.panel3.Controls.Add(this.buttonXDeleteAllItems);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 268);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(384, 41);
            this.panel3.TabIndex = 22;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ManageRepositoryExDialogBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(384, 359);
            this.ControlBox = false;
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ManageRepositoryExDialogBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "管理图库";
            this.Load += new System.EventHandler(this.ManageRepositoryDialogBox_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonXExit;
        private System.Windows.Forms.Button buttonXDeleteSelectedItems;
        private System.Windows.Forms.Label labelX3;
        private System.Windows.Forms.Button buttonXDeleteAllItems;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ImageList imageList1;
    }
}