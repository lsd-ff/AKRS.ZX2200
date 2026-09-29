namespace AKRS.Galaxy2.PR.Controls
{
    partial class FrmVmVisionResult
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
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.ucVmResultComponent = new AKRS.Galaxy2.PR.Controls.UcVmResultShow();
            this.ucVmResultUpLook = new AKRS.Galaxy2.PR.Controls.UcVmResultShow();
            this.ucVmResultBond = new AKRS.Galaxy2.PR.Controls.UcVmResultShow();
            this.ucVmResultDispense = new AKRS.Galaxy2.PR.Controls.UcVmResultShow();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.ucVmResultComponent);
            this.tablePanel1.Controls.Add(this.ucVmResultUpLook);
            this.tablePanel1.Controls.Add(this.ucVmResultBond);
            this.tablePanel1.Controls.Add(this.ucVmResultDispense);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Size = new System.Drawing.Size(1061, 714);
            this.tablePanel1.TabIndex = 1;
            // 
            // ucVmResultComponent
            // 
            this.ucVmResultComponent.CameraName = null;
            this.tablePanel1.SetColumn(this.ucVmResultComponent, 1);
            this.ucVmResultComponent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucVmResultComponent.ImageCount = 20;
            this.ucVmResultComponent.Location = new System.Drawing.Point(534, 360);
            this.ucVmResultComponent.MaxImageCount = 50;
            this.ucVmResultComponent.Name = "ucVmResultComponent";
            this.tablePanel1.SetRow(this.ucVmResultComponent, 1);
            this.ucVmResultComponent.Size = new System.Drawing.Size(525, 351);
            this.ucVmResultComponent.TabIndex = 3;
            // 
            // ucVmResultUpLook
            // 
            this.ucVmResultUpLook.CameraName = null;
            this.tablePanel1.SetColumn(this.ucVmResultUpLook, 0);
            this.ucVmResultUpLook.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucVmResultUpLook.ImageCount = 20;
            this.ucVmResultUpLook.Location = new System.Drawing.Point(3, 360);
            this.ucVmResultUpLook.MaxImageCount = 50;
            this.ucVmResultUpLook.Name = "ucVmResultUpLook";
            this.tablePanel1.SetRow(this.ucVmResultUpLook, 1);
            this.ucVmResultUpLook.Size = new System.Drawing.Size(525, 351);
            this.ucVmResultUpLook.TabIndex = 2;
            // 
            // ucVmResultBond
            // 
            this.ucVmResultBond.CameraName = null;
            this.tablePanel1.SetColumn(this.ucVmResultBond, 1);
            this.ucVmResultBond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucVmResultBond.ImageCount = 20;
            this.ucVmResultBond.Location = new System.Drawing.Point(534, 3);
            this.ucVmResultBond.MaxImageCount = 50;
            this.ucVmResultBond.Name = "ucVmResultBond";
            this.tablePanel1.SetRow(this.ucVmResultBond, 0);
            this.ucVmResultBond.Size = new System.Drawing.Size(525, 351);
            this.ucVmResultBond.TabIndex = 1;
            // 
            // ucVmResultDispense
            // 
            this.ucVmResultDispense.CameraName = null;
            this.tablePanel1.SetColumn(this.ucVmResultDispense, 0);
            this.ucVmResultDispense.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucVmResultDispense.ImageCount = 20;
            this.ucVmResultDispense.Location = new System.Drawing.Point(3, 3);
            this.ucVmResultDispense.MaxImageCount = 50;
            this.ucVmResultDispense.Name = "ucVmResultDispense";
            this.tablePanel1.SetRow(this.ucVmResultDispense, 0);
            this.ucVmResultDispense.Size = new System.Drawing.Size(525, 351);
            this.ucVmResultDispense.TabIndex = 0;
            // 
            // FrmVmVisionResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 714);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmVmVisionResult";
            this.Text = "FrmVmVisionResult";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmVmVisionResult_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmVmVisionResult_FormClosed);
            this.Shown += new System.EventHandler(this.FrmVmVisionResult_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UcVmResultShow ucVmResultDispense;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private UcVmResultShow ucVmResultComponent;
        private UcVmResultShow ucVmResultUpLook;
        private UcVmResultShow ucVmResultBond;
    }
}