namespace AKRS.ZX2200.Main.Machine.Controls
{
    partial class FrmMachineLevelCheck
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
            this.ucObjectLevel1 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.ucObjectLevel2 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            this.ucObjectLevel3 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            this.ucObjectLevel4 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            this.ucObjectLevel5 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            this.ucObjectLevel6 = new AKRS.ZX2200.Main.Machine.ObjectLevel.UcObjectLevel();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucObjectLevel1
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel1, 0);
            this.ucObjectLevel1.Location = new System.Drawing.Point(3, 26);
            this.ucObjectLevel1.Name = "ucObjectLevel1";
            this.tablePanel1.SetRow(this.ucObjectLevel1, 0);
            this.ucObjectLevel1.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel1.TabIndex = 0;
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.ucObjectLevel6);
            this.tablePanel1.Controls.Add(this.ucObjectLevel5);
            this.tablePanel1.Controls.Add(this.ucObjectLevel4);
            this.tablePanel1.Controls.Add(this.ucObjectLevel3);
            this.tablePanel1.Controls.Add(this.ucObjectLevel2);
            this.tablePanel1.Controls.Add(this.ucObjectLevel1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(1012, 741);
            this.tablePanel1.TabIndex = 1;
            // 
            // ucObjectLevel2
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel2, 0);
            this.ucObjectLevel2.Location = new System.Drawing.Point(3, 150);
            this.ucObjectLevel2.Name = "ucObjectLevel2";
            this.tablePanel1.SetRow(this.ucObjectLevel2, 1);
            this.ucObjectLevel2.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel2.TabIndex = 1;
            // 
            // ucObjectLevel3
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel3, 0);
            this.ucObjectLevel3.Location = new System.Drawing.Point(3, 274);
            this.ucObjectLevel3.Name = "ucObjectLevel3";
            this.tablePanel1.SetRow(this.ucObjectLevel3, 2);
            this.ucObjectLevel3.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel3.TabIndex = 2;
            // 
            // ucObjectLevel4
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel4, 0);
            this.ucObjectLevel4.Location = new System.Drawing.Point(3, 398);
            this.ucObjectLevel4.Name = "ucObjectLevel4";
            this.tablePanel1.SetRow(this.ucObjectLevel4, 3);
            this.ucObjectLevel4.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel4.TabIndex = 3;
            // 
            // ucObjectLevel5
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel5, 0);
            this.ucObjectLevel5.Location = new System.Drawing.Point(3, 522);
            this.ucObjectLevel5.Name = "ucObjectLevel5";
            this.tablePanel1.SetRow(this.ucObjectLevel5, 4);
            this.ucObjectLevel5.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel5.TabIndex = 4;
            // 
            // ucObjectLevel6
            // 
            this.tablePanel1.SetColumn(this.ucObjectLevel6, 0);
            this.ucObjectLevel6.Location = new System.Drawing.Point(3, 645);
            this.ucObjectLevel6.Name = "ucObjectLevel6";
            this.tablePanel1.SetRow(this.ucObjectLevel6, 5);
            this.ucObjectLevel6.Size = new System.Drawing.Size(1006, 71);
            this.ucObjectLevel6.TabIndex = 5;
            // 
            // FrmMachineLevelCheck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1012, 741);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmMachineLevelCheck";
            this.Text = "设备水平检测";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ObjectLevel.UcObjectLevel ucObjectLevel1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private ObjectLevel.UcObjectLevel ucObjectLevel6;
        private ObjectLevel.UcObjectLevel ucObjectLevel5;
        private ObjectLevel.UcObjectLevel ucObjectLevel4;
        private ObjectLevel.UcObjectLevel ucObjectLevel3;
        private ObjectLevel.UcObjectLevel ucObjectLevel2;
    }
}