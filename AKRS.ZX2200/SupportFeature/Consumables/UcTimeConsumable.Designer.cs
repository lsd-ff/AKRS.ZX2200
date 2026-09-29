namespace AKRS.ZX2200.Consumables
{
    partial class UcTimeConsumable
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
            this.LbPPTools1 = new DevExpress.XtraEditors.LabelControl();
            this.BtnClearNozzle1 = new DevExpress.XtraEditors.SimpleButton();
            this.ChkPPTools1 = new DevExpress.XtraEditors.CheckEdit();
            this.SpRemainingTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.dateEdit1 = new DevExpress.XtraEditors.DateEdit();
            this.dateEdit2 = new DevExpress.XtraEditors.DateEdit();
            this.SpLifeSpanTime = new DevExpress.XtraEditors.SpinEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPPTools1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRemainingTime.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLifeSpanTime.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // LbPPTools1
            // 
            this.LbPPTools1.Location = new System.Drawing.Point(5, 11);
            this.LbPPTools1.Name = "LbPPTools1";
            this.LbPPTools1.Size = new System.Drawing.Size(31, 14);
            this.LbPPTools1.TabIndex = 9;
            this.LbPPTools1.Tag = "Needle";
            this.LbPPTools1.Text = "吸嘴1";
            // 
            // BtnClearNozzle1
            // 
            this.BtnClearNozzle1.Location = new System.Drawing.Point(1030, 8);
            this.BtnClearNozzle1.Name = "BtnClearNozzle1";
            this.BtnClearNozzle1.Size = new System.Drawing.Size(83, 23);
            this.BtnClearNozzle1.TabIndex = 10;
            this.BtnClearNozzle1.Tag = "Needle";
            this.BtnClearNozzle1.Text = "清空记忆";
            this.BtnClearNozzle1.Click += new System.EventHandler(this.BtnClearNozzle1_Click);
            // 
            // ChkPPTools1
            // 
            this.ChkPPTools1.Location = new System.Drawing.Point(82, 9);
            this.ChkPPTools1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ChkPPTools1.Name = "ChkPPTools1";
            this.ChkPPTools1.Properties.Caption = "开启";
            this.ChkPPTools1.Size = new System.Drawing.Size(82, 20);
            this.ChkPPTools1.TabIndex = 13;
            this.ChkPPTools1.CheckedChanged += new System.EventHandler(this.ChkPPTools1_CheckedChanged_1);
            // 
            // SpRemainingTime
            // 
            this.SpRemainingTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRemainingTime.Location = new System.Drawing.Point(634, 9);
            this.SpRemainingTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpRemainingTime.Name = "SpRemainingTime";
            this.SpRemainingTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRemainingTime.Size = new System.Drawing.Size(109, 20);
            this.SpRemainingTime.TabIndex = 18;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(749, 11);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(24, 14);
            this.labelControl1.TabIndex = 19;
            this.labelControl1.Tag = "Needle";
            this.labelControl1.Text = "分钟";
            // 
            // dateEdit1
            // 
            this.dateEdit1.EditValue = null;
            this.dateEdit1.Location = new System.Drawing.Point(170, 9);
            this.dateEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateEdit1.Name = "dateEdit1";
            this.dateEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit1.Size = new System.Drawing.Size(192, 20);
            this.dateEdit1.TabIndex = 20;
            // 
            // dateEdit2
            // 
            this.dateEdit2.EditValue = null;
            this.dateEdit2.Location = new System.Drawing.Point(412, 9);
            this.dateEdit2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateEdit2.Name = "dateEdit2";
            this.dateEdit2.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateEdit2.Size = new System.Drawing.Size(179, 20);
            this.dateEdit2.TabIndex = 21;
            // 
            // SpLifeSpanTime
            // 
            this.SpLifeSpanTime.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpLifeSpanTime.Location = new System.Drawing.Point(817, 8);
            this.SpLifeSpanTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpLifeSpanTime.Name = "SpLifeSpanTime";
            this.SpLifeSpanTime.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpLifeSpanTime.Size = new System.Drawing.Size(109, 20);
            this.SpLifeSpanTime.TabIndex = 22;
            this.SpLifeSpanTime.EditValueChanged += new System.EventHandler(this.SpLifeSpanTime_EditValueChanged);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(932, 11);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(24, 14);
            this.labelControl2.TabIndex = 23;
            this.labelControl2.Tag = "Needle";
            this.labelControl2.Text = "分钟";
            // 
            // UcTimeConsumable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.SpLifeSpanTime);
            this.Controls.Add(this.dateEdit2);
            this.Controls.Add(this.dateEdit1);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.SpRemainingTime);
            this.Controls.Add(this.ChkPPTools1);
            this.Controls.Add(this.LbPPTools1);
            this.Controls.Add(this.BtnClearNozzle1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcTimeConsumable";
            this.Size = new System.Drawing.Size(1116, 39);
            ((System.ComponentModel.ISupportInitialize)(this.ChkPPTools1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRemainingTime.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpLifeSpanTime.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit ChkPPTools1;
        private DevExpress.XtraEditors.LabelControl LbPPTools1;
        private DevExpress.XtraEditors.SimpleButton BtnClearNozzle1;
        private DevExpress.XtraEditors.SpinEdit SpRemainingTime;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dateEdit1;
        private DevExpress.XtraEditors.DateEdit dateEdit2;
        private DevExpress.XtraEditors.SpinEdit SpLifeSpanTime;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
