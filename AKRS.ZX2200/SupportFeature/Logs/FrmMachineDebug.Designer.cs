namespace AKRS.ZX2200.SupportFeature.Logs
{
    partial class FrmMachineDebug
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
            this.MeBond = new DevExpress.XtraEditors.MemoEdit();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.SpPauseTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.LueDebugType = new DevExpress.XtraEditors.LookUpEdit();
            this.BtContinue = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.MeBond.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpPauseTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueDebugType.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MeBond
            // 
            this.tablePanel1.SetColumn(this.MeBond, 0);
            this.MeBond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MeBond.Location = new System.Drawing.Point(2, 81);
            this.MeBond.Margin = new System.Windows.Forms.Padding(2);
            this.MeBond.Name = "MeBond";
            this.tablePanel1.SetRow(this.MeBond, 1);
            this.MeBond.Size = new System.Drawing.Size(1197, 711);
            this.MeBond.TabIndex = 2;
            // 
            // panelControl1
            // 
            this.tablePanel1.SetColumn(this.panelControl1, 0);
            this.panelControl1.Controls.Add(this.BtContinue);
            this.panelControl1.Controls.Add(this.LueDebugType);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.SpPauseTime);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(3, 3);
            this.panelControl1.Name = "panelControl1";
            this.tablePanel1.SetRow(this.panelControl1, 0);
            this.panelControl1.Size = new System.Drawing.Size(1195, 73);
            this.panelControl1.TabIndex = 3;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(857, 33);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(15, 14);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "ms";
            // 
            // SpPauseTime
            // 
            this.SpPauseTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpPauseTime.Location = new System.Drawing.Point(751, 30);
            this.SpPauseTime.Name = "SpPauseTime";
            this.SpPauseTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpPauseTime.Size = new System.Drawing.Size(100, 20);
            this.SpPauseTime.TabIndex = 1;
            this.SpPauseTime.EditValueChanged += new System.EventHandler(this.SpPauseTime_EditValueChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(653, 33);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(72, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "调试停留时间";
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.panelControl1);
            this.tablePanel1.Controls.Add(this.MeBond);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 90F)});
            this.tablePanel1.Size = new System.Drawing.Size(1201, 794);
            this.tablePanel1.TabIndex = 4;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(11, 33);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "调试模式";
            // 
            // LueDebugType
            // 
            this.LueDebugType.Location = new System.Drawing.Point(66, 30);
            this.LueDebugType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LueDebugType.Name = "LueDebugType";
            this.LueDebugType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueDebugType.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "")});
            this.LueDebugType.Properties.DisplayMember = "Display";
            this.LueDebugType.Properties.NullText = "";
            this.LueDebugType.Properties.ValueMember = "Value";
            this.LueDebugType.Size = new System.Drawing.Size(222, 20);
            this.LueDebugType.TabIndex = 12;
            this.LueDebugType.EditValueChanged += new System.EventHandler(this.LueDebugType_EditValueChanged);
            // 
            // BtContinue
            // 
            this.BtContinue.Location = new System.Drawing.Point(330, 29);
            this.BtContinue.Name = "BtContinue";
            this.BtContinue.Size = new System.Drawing.Size(111, 23);
            this.BtContinue.TabIndex = 13;
            this.BtContinue.Text = "继续";
            this.BtContinue.Click += new System.EventHandler(this.BtContinue_Click);
            // 
            // FrmMachineDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1201, 794);
            this.Controls.Add(this.tablePanel1);
            this.Name = "FrmMachineDebug";
            this.Text = "调试界面";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmMachineDebug_FormClosed);
            this.Load += new System.EventHandler(this.FrmMachineDebug_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MeBond.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpPauseTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueDebugType.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.MemoEdit MeBond;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit SpPauseTime;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LookUpEdit LueDebugType;
        private DevExpress.XtraEditors.SimpleButton BtContinue;
    }
}
