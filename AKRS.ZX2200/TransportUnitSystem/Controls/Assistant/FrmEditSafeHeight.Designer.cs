namespace AKRS.ZX2200.TransportUnitSystem.Controls.Assistant
{
    partial class FrmEditSafeHeight
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.SpSafetyHeight = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl7 = new DevExpress.XtraEditors.GroupControl();
            this.SpAdditionalSafetyHeight = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl6 = new DevExpress.XtraEditors.GroupControl();
            this.LueAdditionalSafetyHeight = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl5 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.BtBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpSafetyHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).BeginInit();
            this.groupControl7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpAdditionalSafetyHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).BeginInit();
            this.groupControl6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LueAdditionalSafetyHeight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).BeginInit();
            this.groupControl5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.groupControl3);
            this.groupControl1.Controls.Add(this.groupControl2);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(4, 15);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(441, 181);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "载具的安全高度";
            // 
            // groupControl3
            // 
            this.groupControl3.Controls.Add(this.SpSafetyHeight);
            this.groupControl3.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl3.Location = new System.Drawing.Point(9, 94);
            this.groupControl3.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(418, 72);
            this.groupControl3.TabIndex = 1;
            this.groupControl3.Text = "Min:2mm - Max:50mm";
            // 
            // SpSafetyHeight
            // 
            this.SpSafetyHeight.EditValue = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.SpSafetyHeight.Location = new System.Drawing.Point(19, 30);
            this.SpSafetyHeight.Margin = new System.Windows.Forms.Padding(2);
            this.SpSafetyHeight.Name = "SpSafetyHeight";
            this.SpSafetyHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSafetyHeight.Properties.IsFloatValue = false;
            this.SpSafetyHeight.Properties.MaskSettings.Set("mask", "N00");
            this.SpSafetyHeight.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.SpSafetyHeight.Properties.MinValue = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.SpSafetyHeight.Size = new System.Drawing.Size(363, 20);
            this.SpSafetyHeight.TabIndex = 0;
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(9, 23);
            this.groupControl2.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(418, 59);
            this.groupControl2.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(19, 23);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(31, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "label1";
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.groupControl7);
            this.groupControl4.Controls.Add(this.groupControl6);
            this.groupControl4.Controls.Add(this.groupControl5);
            this.groupControl4.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl4.Location = new System.Drawing.Point(4, 199);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(441, 282);
            this.groupControl4.TabIndex = 1;
            this.groupControl4.Text = "Additional safety height";
            this.groupControl4.Visible = false;
            // 
            // groupControl7
            // 
            this.groupControl7.Controls.Add(this.SpAdditionalSafetyHeight);
            this.groupControl7.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl7.Location = new System.Drawing.Point(9, 217);
            this.groupControl7.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl7.Name = "groupControl7";
            this.groupControl7.Size = new System.Drawing.Size(418, 62);
            this.groupControl7.TabIndex = 1;
            this.groupControl7.Text = "Min:20mm - Max:50mm";
            // 
            // SpAdditionalSafetyHeight
            // 
            this.SpAdditionalSafetyHeight.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpAdditionalSafetyHeight.Location = new System.Drawing.Point(19, 23);
            this.SpAdditionalSafetyHeight.Margin = new System.Windows.Forms.Padding(2);
            this.SpAdditionalSafetyHeight.Name = "SpAdditionalSafetyHeight";
            this.SpAdditionalSafetyHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpAdditionalSafetyHeight.Size = new System.Drawing.Size(363, 20);
            this.SpAdditionalSafetyHeight.TabIndex = 1;
            // 
            // groupControl6
            // 
            this.groupControl6.Controls.Add(this.LueAdditionalSafetyHeight);
            this.groupControl6.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl6.Location = new System.Drawing.Point(9, 135);
            this.groupControl6.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl6.Name = "groupControl6";
            this.groupControl6.Size = new System.Drawing.Size(418, 79);
            this.groupControl6.TabIndex = 0;
            this.groupControl6.Text = "Additional safety height";
            // 
            // LueAdditionalSafetyHeight
            // 
            this.LueAdditionalSafetyHeight.Location = new System.Drawing.Point(19, 39);
            this.LueAdditionalSafetyHeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.LueAdditionalSafetyHeight.Name = "LueAdditionalSafetyHeight";
            this.LueAdditionalSafetyHeight.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueAdditionalSafetyHeight.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "")});
            this.LueAdditionalSafetyHeight.Properties.DisplayMember = "Display";
            this.LueAdditionalSafetyHeight.Properties.NullText = "";
            this.LueAdditionalSafetyHeight.Properties.ValueMember = "Value";
            this.LueAdditionalSafetyHeight.Size = new System.Drawing.Size(363, 20);
            this.LueAdditionalSafetyHeight.TabIndex = 8;
            // 
            // groupControl5
            // 
            this.groupControl5.Controls.Add(this.labelControl2);
            this.groupControl5.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl5.Location = new System.Drawing.Point(9, 20);
            this.groupControl5.Margin = new System.Windows.Forms.Padding(2);
            this.groupControl5.Name = "groupControl5";
            this.groupControl5.Size = new System.Drawing.Size(418, 111);
            this.groupControl5.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(36, 30);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(2);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(31, 14);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "label1";
            // 
            // BtBack
            // 
            this.BtBack.Location = new System.Drawing.Point(32, 495);
            this.BtBack.Margin = new System.Windows.Forms.Padding(2);
            this.BtBack.Name = "BtBack";
            this.BtBack.Size = new System.Drawing.Size(80, 23);
            this.BtBack.TabIndex = 2;
            this.BtBack.Text = "返回";
            this.BtBack.Click += new System.EventHandler(this.BtBack_Click);
            // 
            // BtNext
            // 
            this.BtNext.Location = new System.Drawing.Point(175, 495);
            this.BtNext.Margin = new System.Windows.Forms.Padding(2);
            this.BtNext.Name = "BtNext";
            this.BtNext.Size = new System.Drawing.Size(80, 23);
            this.BtNext.TabIndex = 3;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(329, 495);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(2);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(80, 23);
            this.BtCancel.TabIndex = 4;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // FrmEditSafeHeight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 538);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.BtNext);
            this.Controls.Add(this.BtBack);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmEditSafeHeight";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmEditSafeHeight";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpSafetyHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl7)).EndInit();
            this.groupControl7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpAdditionalSafetyHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl6)).EndInit();
            this.groupControl6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LueAdditionalSafetyHeight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl5)).EndInit();
            this.groupControl5.ResumeLayout(false);
            this.groupControl5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.SpinEdit SpSafetyHeight;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.GroupControl groupControl5;
        private DevExpress.XtraEditors.GroupControl groupControl6;
        private DevExpress.XtraEditors.GroupControl groupControl7;
        private DevExpress.XtraEditors.SpinEdit SpAdditionalSafetyHeight;
        private DevExpress.XtraEditors.SimpleButton BtBack;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LookUpEdit LueAdditionalSafetyHeight;
    }
}