namespace AKRS.ZX2200.Main.Controls
{
    partial class FrmShutDown
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
            this.LbDescription = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtnRebort = new DevExpress.XtraEditors.SimpleButton();
            this.BtnShutDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LbDescription
            // 
            this.LbDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LbDescription.Location = new System.Drawing.Point(0, 0);
            this.LbDescription.Name = "LbDescription";
            this.LbDescription.Size = new System.Drawing.Size(81, 18);
            this.LbDescription.TabIndex = 0;
            this.LbDescription.Text = "labelControl1";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtnRebort);
            this.tablePanel1.Controls.Add(this.BtnShutDown);
            this.tablePanel1.Controls.Add(this.BtnCancel);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 51);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(631, 60);
            this.tablePanel1.TabIndex = 1;
            // 
            // BtnRebort
            // 
            this.tablePanel1.SetColumn(this.BtnRebort, 1);
            this.BtnRebort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnRebort.Location = new System.Drawing.Point(248, 3);
            this.BtnRebort.Margin = new System.Windows.Forms.Padding(38, 3, 38, 3);
            this.BtnRebort.Name = "BtnRebort";
            this.tablePanel1.SetRow(this.BtnRebort, 0);
            this.BtnRebort.Size = new System.Drawing.Size(134, 54);
            this.BtnRebort.TabIndex = 2;
            this.BtnRebort.Text = "重启";
            this.BtnRebort.Click += new System.EventHandler(this.BtnRebort_Click);
            // 
            // BtnShutDown
            // 
            this.tablePanel1.SetColumn(this.BtnShutDown, 0);
            this.BtnShutDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnShutDown.Location = new System.Drawing.Point(38, 3);
            this.BtnShutDown.Margin = new System.Windows.Forms.Padding(38, 3, 38, 3);
            this.BtnShutDown.Name = "BtnShutDown";
            this.tablePanel1.SetRow(this.BtnShutDown, 0);
            this.BtnShutDown.Size = new System.Drawing.Size(134, 54);
            this.BtnShutDown.TabIndex = 1;
            this.BtnShutDown.Text = "关机";
            this.BtnShutDown.Click += new System.EventHandler(this.BtnShutDown_Click);
            // 
            // BtnCancel
            // 
            this.tablePanel1.SetColumn(this.BtnCancel, 2);
            this.BtnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnCancel.Location = new System.Drawing.Point(459, 3);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(38, 3, 38, 3);
            this.BtnCancel.Name = "BtnCancel";
            this.tablePanel1.SetRow(this.BtnCancel, 0);
            this.BtnCancel.Size = new System.Drawing.Size(134, 54);
            this.BtnCancel.TabIndex = 0;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmShutDown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 111);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.LbDescription);
            this.Name = "FrmShutDown";
            this.Text = "关机";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl LbDescription;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtnRebort;
        private DevExpress.XtraEditors.SimpleButton BtnShutDown;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
    }
}