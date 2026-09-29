namespace AKRS.ZX2200.TransportSystem.Controls.Manual
{
    partial class UcTransportSystemManual
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
            this.BtTransportUnitIndex = new DevExpress.XtraEditors.SimpleButton();
            this.BtClampTransportUnit = new DevExpress.XtraEditors.SimpleButton();
            this.BtUnclampTransportUnit = new DevExpress.XtraEditors.SimpleButton();
            this.ChkContinuousFeeding = new DevExpress.XtraEditors.CheckEdit();
            this.BtInitializeTS = new DevExpress.XtraEditors.SimpleButton();
            this.BtSearchBelt1Sys1 = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtClearBond = new DevExpress.XtraEditors.SimpleButton();
            this.BtClearDispense = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSearchBelt2Sys2 = new DevExpress.XtraEditors.SimpleButton();
            this.BtManualOpenVacuum = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.ChkContinuousFeeding.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtTransportUnitIndex
            // 
            this.tablePanel1.SetColumn(this.BtTransportUnitIndex, 0);
            this.BtTransportUnitIndex.Location = new System.Drawing.Point(33, 61);
            this.BtTransportUnitIndex.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtTransportUnitIndex.Name = "BtTransportUnitIndex";
            this.tablePanel1.SetRow(this.BtTransportUnitIndex, 1);
            this.BtTransportUnitIndex.Size = new System.Drawing.Size(256, 37);
            this.BtTransportUnitIndex.TabIndex = 1;
            this.BtTransportUnitIndex.Text = "传送载具到下一个位置";
            this.BtTransportUnitIndex.Click += new System.EventHandler(this.BtTransportUnitIndex_Click);
            // 
            // BtClampTransportUnit
            // 
            this.tablePanel1.SetColumn(this.BtClampTransportUnit, 0);
            this.BtClampTransportUnit.Location = new System.Drawing.Point(33, 273);
            this.BtClampTransportUnit.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtClampTransportUnit.Name = "BtClampTransportUnit";
            this.tablePanel1.SetRow(this.BtClampTransportUnit, 5);
            this.BtClampTransportUnit.Size = new System.Drawing.Size(256, 37);
            this.BtClampTransportUnit.TabIndex = 2;
            this.BtClampTransportUnit.Text = "夹紧载具";
            this.BtClampTransportUnit.Click += new System.EventHandler(this.BtClampTransportUnit_Click);
            // 
            // BtUnclampTransportUnit
            // 
            this.tablePanel1.SetColumn(this.BtUnclampTransportUnit, 0);
            this.BtUnclampTransportUnit.Location = new System.Drawing.Point(33, 326);
            this.BtUnclampTransportUnit.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtUnclampTransportUnit.Name = "BtUnclampTransportUnit";
            this.tablePanel1.SetRow(this.BtUnclampTransportUnit, 6);
            this.BtUnclampTransportUnit.Size = new System.Drawing.Size(256, 37);
            this.BtUnclampTransportUnit.TabIndex = 3;
            this.BtUnclampTransportUnit.Text = "松开载具";
            this.BtUnclampTransportUnit.Click += new System.EventHandler(this.BtUnclampTransportUnit_Click);
            // 
            // ChkContinuousFeeding
            // 
            this.tablePanel1.SetColumn(this.ChkContinuousFeeding, 0);
            this.ChkContinuousFeeding.EditValue = true;
            this.ChkContinuousFeeding.Location = new System.Drawing.Point(33, 15);
            this.ChkContinuousFeeding.Margin = new System.Windows.Forms.Padding(33, 3, 3, 3);
            this.ChkContinuousFeeding.Name = "ChkContinuousFeeding";
            this.ChkContinuousFeeding.Properties.Caption = "连续上料";
            this.ChkContinuousFeeding.Properties.CheckBoxOptions.Style = DevExpress.XtraEditors.Controls.CheckBoxStyle.SvgCheckBox1;
            this.tablePanel1.SetRow(this.ChkContinuousFeeding, 0);
            this.ChkContinuousFeeding.Size = new System.Drawing.Size(286, 22);
            this.ChkContinuousFeeding.TabIndex = 4;
            this.ChkContinuousFeeding.Visible = false;
            this.ChkContinuousFeeding.CheckedChanged += new System.EventHandler(this.ChkEmptyIndexOn_CheckedChanged);
            // 
            // BtInitializeTS
            // 
            this.tablePanel1.SetColumn(this.BtInitializeTS, 0);
            this.BtInitializeTS.Location = new System.Drawing.Point(33, 220);
            this.BtInitializeTS.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtInitializeTS.Name = "BtInitializeTS";
            this.tablePanel1.SetRow(this.BtInitializeTS, 4);
            this.BtInitializeTS.Size = new System.Drawing.Size(256, 37);
            this.BtInitializeTS.TabIndex = 5;
            this.BtInitializeTS.Text = "清空轨道内载具";
            this.BtInitializeTS.Click += new System.EventHandler(this.BtInitializeTS_Click);
            // 
            // BtSearchBelt1Sys1
            // 
            this.tablePanel1.SetColumn(this.BtSearchBelt1Sys1, 0);
            this.BtSearchBelt1Sys1.Location = new System.Drawing.Point(33, 379);
            this.BtSearchBelt1Sys1.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtSearchBelt1Sys1.Name = "BtSearchBelt1Sys1";
            this.tablePanel1.SetRow(this.BtSearchBelt1Sys1, 7);
            this.BtSearchBelt1Sys1.Size = new System.Drawing.Size(256, 37);
            this.BtSearchBelt1Sys1.TabIndex = 6;
            this.BtSearchBelt1Sys1.Text = "搜索点胶载具";
            this.BtSearchBelt1Sys1.Click += new System.EventHandler(this.BtSearchBelt1Sys1_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel1.Controls.Add(this.BtManualOpenVacuum);
            this.tablePanel1.Controls.Add(this.BtClearBond);
            this.tablePanel1.Controls.Add(this.BtClearDispense);
            this.tablePanel1.Controls.Add(this.BtnSearchBelt2Sys2);
            this.tablePanel1.Controls.Add(this.BtSearchBelt1Sys1);
            this.tablePanel1.Controls.Add(this.BtClampTransportUnit);
            this.tablePanel1.Controls.Add(this.BtInitializeTS);
            this.tablePanel1.Controls.Add(this.BtUnclampTransportUnit);
            this.tablePanel1.Controls.Add(this.ChkContinuousFeeding);
            this.tablePanel1.Controls.Add(this.BtTransportUnitIndex);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(322, 528);
            this.tablePanel1.TabIndex = 7;
            // 
            // BtClearBond
            // 
            this.tablePanel1.SetColumn(this.BtClearBond, 0);
            this.BtClearBond.Location = new System.Drawing.Point(33, 167);
            this.BtClearBond.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtClearBond.Name = "BtClearBond";
            this.tablePanel1.SetRow(this.BtClearBond, 3);
            this.BtClearBond.Size = new System.Drawing.Size(256, 37);
            this.BtClearBond.TabIndex = 10;
            this.BtClearBond.Text = "清空固晶载具";
            this.BtClearBond.Click += new System.EventHandler(this.BtClearBond_Click);
            // 
            // BtClearDispense
            // 
            this.tablePanel1.SetColumn(this.BtClearDispense, 0);
            this.BtClearDispense.Location = new System.Drawing.Point(33, 114);
            this.BtClearDispense.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtClearDispense.Name = "BtClearDispense";
            this.tablePanel1.SetRow(this.BtClearDispense, 2);
            this.BtClearDispense.Size = new System.Drawing.Size(256, 37);
            this.BtClearDispense.TabIndex = 9;
            this.BtClearDispense.Text = "清空点胶载具";
            this.BtClearDispense.Click += new System.EventHandler(this.BtClearDispense_Click);
            // 
            // BtnSearchBelt2Sys2
            // 
            this.tablePanel1.SetColumn(this.BtnSearchBelt2Sys2, 0);
            this.BtnSearchBelt2Sys2.Location = new System.Drawing.Point(33, 432);
            this.BtnSearchBelt2Sys2.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtnSearchBelt2Sys2.Name = "BtnSearchBelt2Sys2";
            this.tablePanel1.SetRow(this.BtnSearchBelt2Sys2, 8);
            this.BtnSearchBelt2Sys2.Size = new System.Drawing.Size(256, 37);
            this.BtnSearchBelt2Sys2.TabIndex = 8;
            this.BtnSearchBelt2Sys2.Text = "搜索固晶载具";
            this.BtnSearchBelt2Sys2.Click += new System.EventHandler(this.BtnSearchBelt2Sys2_Click);
            // 
            // BtManualOpenVacuum
            // 
            this.tablePanel1.SetColumn(this.BtManualOpenVacuum, 0);
            this.BtManualOpenVacuum.Location = new System.Drawing.Point(33, 484);
            this.BtManualOpenVacuum.Margin = new System.Windows.Forms.Padding(33, 3, 33, 3);
            this.BtManualOpenVacuum.Name = "BtManualOpenVacuum";
            this.tablePanel1.SetRow(this.BtManualOpenVacuum, 9);
            this.BtManualOpenVacuum.Size = new System.Drawing.Size(256, 37);
            this.BtManualOpenVacuum.TabIndex = 11;
            this.BtManualOpenVacuum.Text = "释放真空";
            this.BtManualOpenVacuum.Click += new System.EventHandler(this.BtManualOpenVacuum_Click);
            // 
            // UcTransportSystemManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel1);
            this.Name = "UcTransportSystemManual";
            this.Size = new System.Drawing.Size(322, 528);
            ((System.ComponentModel.ISupportInitialize)(this.ChkContinuousFeeding.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton BtTransportUnitIndex;
        private DevExpress.XtraEditors.SimpleButton BtClampTransportUnit;
        private DevExpress.XtraEditors.SimpleButton BtUnclampTransportUnit;
        private DevExpress.XtraEditors.CheckEdit ChkContinuousFeeding;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtSearchBelt1Sys1;
        private DevExpress.XtraEditors.SimpleButton BtInitializeTS;
        private DevExpress.XtraEditors.SimpleButton BtnSearchBelt2Sys2;
        private DevExpress.XtraEditors.SimpleButton BtClearBond;
        private DevExpress.XtraEditors.SimpleButton BtClearDispense;
        private DevExpress.XtraEditors.SimpleButton BtManualOpenVacuum;
    }
}