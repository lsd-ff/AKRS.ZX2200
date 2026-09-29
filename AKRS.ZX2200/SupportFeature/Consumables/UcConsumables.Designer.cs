namespace AKRS.ZX2200.SupportFeature.Consumables
{
    partial class UcConsumables
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ChkPPTools1 = new DevExpress.XtraEditors.CheckEdit();
            this.LbPPTools1 = new DevExpress.XtraEditors.LabelControl();
            this.SpPPToolTotalTimes1 = new DevExpress.XtraEditors.SpinEdit();
            this.BtnClearNozzle1 = new DevExpress.XtraEditors.SimpleButton();
            this.SpPPToolUseTimes1 = new DevExpress.XtraEditors.SpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPPTools1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPPToolTotalTimes1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPPToolUseTimes1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ChkPPTools1
            // 
            this.ChkPPTools1.Location = new System.Drawing.Point(91, 1);
            this.ChkPPTools1.Name = "ChkPPTools1";
            this.ChkPPTools1.Properties.Caption = "开启";
            this.ChkPPTools1.Size = new System.Drawing.Size(94, 24);
            this.ChkPPTools1.TabIndex = 8;
            this.ChkPPTools1.CheckedChanged += new System.EventHandler(this.ChkPPTools1_CheckedChanged);
            // 
            // LbPPTools1
            // 
            this.LbPPTools1.Location = new System.Drawing.Point(3, 4);
            this.LbPPTools1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.LbPPTools1.Name = "LbPPTools1";
            this.LbPPTools1.Size = new System.Drawing.Size(38, 18);
            this.LbPPTools1.TabIndex = 4;
            this.LbPPTools1.Tag = "Needle";
            this.LbPPTools1.Text = "吸嘴1";
            // 
            // SpPPToolTotalTimes1
            // 
            this.SpPPToolTotalTimes1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpPPToolTotalTimes1.Location = new System.Drawing.Point(236, 4);
            this.SpPPToolTotalTimes1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpPPToolTotalTimes1.Name = "SpPPToolTotalTimes1";
            this.SpPPToolTotalTimes1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpPPToolTotalTimes1.Properties.IsFloatValue = false;
            this.SpPPToolTotalTimes1.Properties.MaskSettings.Set("mask", "N00");
            this.SpPPToolTotalTimes1.Size = new System.Drawing.Size(110, 24);
            this.SpPPToolTotalTimes1.TabIndex = 6;
            this.SpPPToolTotalTimes1.Tag = "";
            // 
            // BtnClearNozzle1
            // 
            this.BtnClearNozzle1.Location = new System.Drawing.Point(524, 1);
            this.BtnClearNozzle1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnClearNozzle1.Name = "BtnClearNozzle1";
            this.BtnClearNozzle1.Size = new System.Drawing.Size(95, 30);
            this.BtnClearNozzle1.TabIndex = 5;
            this.BtnClearNozzle1.Tag = "Needle";
            this.BtnClearNozzle1.Text = "清空记忆";
            this.BtnClearNozzle1.Click += new System.EventHandler(this.BtnClearNozzle1_Click);
            // 
            // SpPPToolUseTimes1
            // 
            this.SpPPToolUseTimes1.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpPPToolUseTimes1.Enabled = false;
            this.SpPPToolUseTimes1.Location = new System.Drawing.Point(379, 4);
            this.SpPPToolUseTimes1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SpPPToolUseTimes1.Name = "SpPPToolUseTimes1";
            this.SpPPToolUseTimes1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpPPToolUseTimes1.Properties.IsFloatValue = false;
            this.SpPPToolUseTimes1.Properties.MaskSettings.Set("mask", "N00");
            this.SpPPToolUseTimes1.Size = new System.Drawing.Size(110, 24);
            this.SpPPToolUseTimes1.TabIndex = 7;
            this.SpPPToolUseTimes1.Tag = "顶针";
            // 
            // UcConsumables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChkPPTools1);
            this.Controls.Add(this.LbPPTools1);
            this.Controls.Add(this.SpPPToolTotalTimes1);
            this.Controls.Add(this.BtnClearNozzle1);
            this.Controls.Add(this.SpPPToolUseTimes1);
            this.Name = "UcConsumables";
            this.Size = new System.Drawing.Size(667, 34);
            ((System.ComponentModel.ISupportInitialize)(this.ChkPPTools1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPPToolTotalTimes1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpPPToolUseTimes1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit ChkPPTools1;
        private DevExpress.XtraEditors.LabelControl LbPPTools1;
        private DevExpress.XtraEditors.SpinEdit SpPPToolTotalTimes1;
        private DevExpress.XtraEditors.SimpleButton BtnClearNozzle1;
        private DevExpress.XtraEditors.SpinEdit SpPPToolUseTimes1;
    }
}
