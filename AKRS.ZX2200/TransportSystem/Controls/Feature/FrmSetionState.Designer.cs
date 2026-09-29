namespace AKRS.ZX2200.TransportSystem.Controls.Feature
{
    partial class FrmSetionState
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
            this.LbLoading = new DevExpress.XtraEditors.LabelControl();
            this.LbDispense = new DevExpress.XtraEditors.LabelControl();
            this.LbWorkTable = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.MpLoadingTable = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.MpDispenseTable = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.MpWorkTable = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.MpWaitingUnloadTable = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.MpUnloadingTable = new DevExpress.XtraEditors.MarqueeProgressBarControl();
            this.TimerState = new System.Windows.Forms.Timer(this.components);
            this.BtLoadingHasMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtLoadingNoMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtDispenseNoMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtDispenseHasMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtBondNoMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtBondHasMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtWaitUnloadNoMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtWaitUnloadHasMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtUnloadingNoMaterial = new DevExpress.XtraEditors.SimpleButton();
            this.BtUnloadingHasMaterial = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.MpLoadingTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpDispenseTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpWorkTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpWaitingUnloadTable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpUnloadingTable.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // LbLoading
            // 
            this.LbLoading.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.LbLoading.Appearance.Options.UseFont = true;
            this.LbLoading.Location = new System.Drawing.Point(169, 190);
            this.LbLoading.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbLoading.Name = "LbLoading";
            this.LbLoading.Size = new System.Drawing.Size(105, 24);
            this.LbLoading.TabIndex = 6;
            this.LbLoading.Text = "入料运输台";
            // 
            // LbDispense
            // 
            this.LbDispense.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.LbDispense.Appearance.Options.UseFont = true;
            this.LbDispense.Location = new System.Drawing.Point(382, 190);
            this.LbDispense.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbDispense.Name = "LbDispense";
            this.LbDispense.Size = new System.Drawing.Size(105, 24);
            this.LbDispense.TabIndex = 7;
            this.LbDispense.Text = "点胶工作台";
            // 
            // LbWorkTable
            // 
            this.LbWorkTable.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.LbWorkTable.Appearance.Options.UseFont = true;
            this.LbWorkTable.Location = new System.Drawing.Point(610, 190);
            this.LbWorkTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbWorkTable.Name = "LbWorkTable";
            this.LbWorkTable.Size = new System.Drawing.Size(105, 24);
            this.LbWorkTable.TabIndex = 8;
            this.LbWorkTable.Text = "固晶工作台";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(811, 190);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(147, 24);
            this.labelControl1.TabIndex = 9;
            this.labelControl1.Text = "等待下料运输台";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(1034, 190);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(105, 24);
            this.labelControl2.TabIndex = 10;
            this.labelControl2.Text = "下料运输台";
            // 
            // MpLoadingTable
            // 
            this.MpLoadingTable.EditValue = 0;
            this.MpLoadingTable.Location = new System.Drawing.Point(150, 253);
            this.MpLoadingTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MpLoadingTable.Name = "MpLoadingTable";
            this.MpLoadingTable.Properties.Appearance.BackColor = System.Drawing.Color.Khaki;
            this.MpLoadingTable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MpLoadingTable.Properties.MarqueeWidth = 0;
            this.MpLoadingTable.Size = new System.Drawing.Size(174, 23);
            this.MpLoadingTable.TabIndex = 12;
            // 
            // MpDispenseTable
            // 
            this.MpDispenseTable.EditValue = 0;
            this.MpDispenseTable.Location = new System.Drawing.Point(370, 253);
            this.MpDispenseTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MpDispenseTable.Name = "MpDispenseTable";
            this.MpDispenseTable.Properties.Appearance.BackColor = System.Drawing.Color.Khaki;
            this.MpDispenseTable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MpDispenseTable.Size = new System.Drawing.Size(174, 23);
            this.MpDispenseTable.TabIndex = 13;
            // 
            // MpWorkTable
            // 
            this.MpWorkTable.EditValue = 0;
            this.MpWorkTable.Location = new System.Drawing.Point(584, 253);
            this.MpWorkTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MpWorkTable.Name = "MpWorkTable";
            this.MpWorkTable.Properties.Appearance.BackColor = System.Drawing.Color.Khaki;
            this.MpWorkTable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MpWorkTable.Size = new System.Drawing.Size(174, 23);
            this.MpWorkTable.TabIndex = 14;
            // 
            // MpWaitingUnloadTable
            // 
            this.MpWaitingUnloadTable.EditValue = 0;
            this.MpWaitingUnloadTable.Location = new System.Drawing.Point(797, 253);
            this.MpWaitingUnloadTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MpWaitingUnloadTable.Name = "MpWaitingUnloadTable";
            this.MpWaitingUnloadTable.Properties.Appearance.BackColor = System.Drawing.Color.Khaki;
            this.MpWaitingUnloadTable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MpWaitingUnloadTable.Size = new System.Drawing.Size(174, 23);
            this.MpWaitingUnloadTable.TabIndex = 15;
            // 
            // MpUnloadingTable
            // 
            this.MpUnloadingTable.EditValue = 0;
            this.MpUnloadingTable.Location = new System.Drawing.Point(1025, 253);
            this.MpUnloadingTable.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MpUnloadingTable.Name = "MpUnloadingTable";
            this.MpUnloadingTable.Properties.Appearance.BackColor = System.Drawing.Color.Khaki;
            this.MpUnloadingTable.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MpUnloadingTable.Size = new System.Drawing.Size(174, 23);
            this.MpUnloadingTable.TabIndex = 16;
            // 
            // TimerState
            // 
            this.TimerState.Interval = 500;
            this.TimerState.Tag = "FrmSectionState";
            this.TimerState.Tick += new System.EventHandler(this.TimerState_Tick);
            // 
            // BtLoadingHasMaterial
            // 
            this.BtLoadingHasMaterial.Location = new System.Drawing.Point(182, 306);
            this.BtLoadingHasMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtLoadingHasMaterial.Name = "BtLoadingHasMaterial";
            this.BtLoadingHasMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtLoadingHasMaterial.TabIndex = 17;
            this.BtLoadingHasMaterial.Text = "有料";
            this.BtLoadingHasMaterial.Click += new System.EventHandler(this.BtLoadingHasMaterial_Click);
            // 
            // BtLoadingNoMaterial
            // 
            this.BtLoadingNoMaterial.Location = new System.Drawing.Point(182, 343);
            this.BtLoadingNoMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtLoadingNoMaterial.Name = "BtLoadingNoMaterial";
            this.BtLoadingNoMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtLoadingNoMaterial.TabIndex = 18;
            this.BtLoadingNoMaterial.Text = "无料";
            this.BtLoadingNoMaterial.Click += new System.EventHandler(this.BtLoadingNoMaterial_Click);
            // 
            // BtDispenseNoMaterial
            // 
            this.BtDispenseNoMaterial.Location = new System.Drawing.Point(403, 343);
            this.BtDispenseNoMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtDispenseNoMaterial.Name = "BtDispenseNoMaterial";
            this.BtDispenseNoMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtDispenseNoMaterial.TabIndex = 20;
            this.BtDispenseNoMaterial.Text = "无料";
            this.BtDispenseNoMaterial.Click += new System.EventHandler(this.BtDispenseNoMaterial_Click);
            // 
            // BtDispenseHasMaterial
            // 
            this.BtDispenseHasMaterial.Location = new System.Drawing.Point(403, 306);
            this.BtDispenseHasMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtDispenseHasMaterial.Name = "BtDispenseHasMaterial";
            this.BtDispenseHasMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtDispenseHasMaterial.TabIndex = 19;
            this.BtDispenseHasMaterial.Text = "有料";
            this.BtDispenseHasMaterial.Click += new System.EventHandler(this.BtDispenseHasMaterial_Click);
            // 
            // BtBondNoMaterial
            // 
            this.BtBondNoMaterial.Location = new System.Drawing.Point(616, 343);
            this.BtBondNoMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtBondNoMaterial.Name = "BtBondNoMaterial";
            this.BtBondNoMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtBondNoMaterial.TabIndex = 22;
            this.BtBondNoMaterial.Text = "无料";
            this.BtBondNoMaterial.Click += new System.EventHandler(this.BtBondNoMaterial_Click);
            // 
            // BtBondHasMaterial
            // 
            this.BtBondHasMaterial.Location = new System.Drawing.Point(616, 306);
            this.BtBondHasMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtBondHasMaterial.Name = "BtBondHasMaterial";
            this.BtBondHasMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtBondHasMaterial.TabIndex = 21;
            this.BtBondHasMaterial.Text = "有料";
            this.BtBondHasMaterial.Click += new System.EventHandler(this.BtBondHasMaterial_Click);
            // 
            // BtWaitUnloadNoMaterial
            // 
            this.BtWaitUnloadNoMaterial.Location = new System.Drawing.Point(834, 343);
            this.BtWaitUnloadNoMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtWaitUnloadNoMaterial.Name = "BtWaitUnloadNoMaterial";
            this.BtWaitUnloadNoMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtWaitUnloadNoMaterial.TabIndex = 24;
            this.BtWaitUnloadNoMaterial.Text = "无料";
            this.BtWaitUnloadNoMaterial.Click += new System.EventHandler(this.BtWaitUnloadNoMaterial_Click);
            // 
            // BtWaitUnloadHasMaterial
            // 
            this.BtWaitUnloadHasMaterial.Location = new System.Drawing.Point(834, 306);
            this.BtWaitUnloadHasMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtWaitUnloadHasMaterial.Name = "BtWaitUnloadHasMaterial";
            this.BtWaitUnloadHasMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtWaitUnloadHasMaterial.TabIndex = 23;
            this.BtWaitUnloadHasMaterial.Text = "有料";
            this.BtWaitUnloadHasMaterial.Click += new System.EventHandler(this.BtWaitUnloadHasMaterial_Click);
            // 
            // BtUnloadingNoMaterial
            // 
            this.BtUnloadingNoMaterial.Location = new System.Drawing.Point(1066, 343);
            this.BtUnloadingNoMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtUnloadingNoMaterial.Name = "BtUnloadingNoMaterial";
            this.BtUnloadingNoMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtUnloadingNoMaterial.TabIndex = 26;
            this.BtUnloadingNoMaterial.Text = "无料";
            this.BtUnloadingNoMaterial.Click += new System.EventHandler(this.BtUnloadingNoMaterial_Click);
            // 
            // BtUnloadingHasMaterial
            // 
            this.BtUnloadingHasMaterial.Location = new System.Drawing.Point(1066, 306);
            this.BtUnloadingHasMaterial.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtUnloadingHasMaterial.Name = "BtUnloadingHasMaterial";
            this.BtUnloadingHasMaterial.Size = new System.Drawing.Size(102, 30);
            this.BtUnloadingHasMaterial.TabIndex = 25;
            this.BtUnloadingHasMaterial.Text = "有料";
            this.BtUnloadingHasMaterial.Click += new System.EventHandler(this.BtUnloadingHasMaterial_Click);
            // 
            // FrmSetionState
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1342, 518);
            this.Controls.Add(this.BtUnloadingNoMaterial);
            this.Controls.Add(this.BtUnloadingHasMaterial);
            this.Controls.Add(this.BtWaitUnloadNoMaterial);
            this.Controls.Add(this.BtWaitUnloadHasMaterial);
            this.Controls.Add(this.BtBondNoMaterial);
            this.Controls.Add(this.BtBondHasMaterial);
            this.Controls.Add(this.BtDispenseNoMaterial);
            this.Controls.Add(this.BtDispenseHasMaterial);
            this.Controls.Add(this.BtLoadingNoMaterial);
            this.Controls.Add(this.BtLoadingHasMaterial);
            this.Controls.Add(this.MpUnloadingTable);
            this.Controls.Add(this.MpWaitingUnloadTable);
            this.Controls.Add(this.MpWorkTable);
            this.Controls.Add(this.MpDispenseTable);
            this.Controls.Add(this.MpLoadingTable);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.LbWorkTable);
            this.Controls.Add(this.LbDispense);
            this.Controls.Add(this.LbLoading);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FrmSetionState";
            this.Text = "Transport section state";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmSetionState_FormClosing);
            this.Load += new System.EventHandler(this.FrmSetionState_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MpLoadingTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpDispenseTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpWorkTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpWaitingUnloadTable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MpUnloadingTable.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl LbLoading;
        private DevExpress.XtraEditors.LabelControl LbDispense;
        private DevExpress.XtraEditors.LabelControl LbWorkTable;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MarqueeProgressBarControl MpLoadingTable;
        private DevExpress.XtraEditors.MarqueeProgressBarControl MpDispenseTable;
        private DevExpress.XtraEditors.MarqueeProgressBarControl MpWorkTable;
        private DevExpress.XtraEditors.MarqueeProgressBarControl MpWaitingUnloadTable;
        private DevExpress.XtraEditors.MarqueeProgressBarControl MpUnloadingTable;
        private System.Windows.Forms.Timer TimerState;
        private DevExpress.XtraEditors.SimpleButton BtLoadingHasMaterial;
        private DevExpress.XtraEditors.SimpleButton BtLoadingNoMaterial;
        private DevExpress.XtraEditors.SimpleButton BtDispenseNoMaterial;
        private DevExpress.XtraEditors.SimpleButton BtDispenseHasMaterial;
        private DevExpress.XtraEditors.SimpleButton BtBondNoMaterial;
        private DevExpress.XtraEditors.SimpleButton BtBondHasMaterial;
        private DevExpress.XtraEditors.SimpleButton BtWaitUnloadNoMaterial;
        private DevExpress.XtraEditors.SimpleButton BtWaitUnloadHasMaterial;
        private DevExpress.XtraEditors.SimpleButton BtUnloadingNoMaterial;
        private DevExpress.XtraEditors.SimpleButton BtUnloadingHasMaterial;
    }
}