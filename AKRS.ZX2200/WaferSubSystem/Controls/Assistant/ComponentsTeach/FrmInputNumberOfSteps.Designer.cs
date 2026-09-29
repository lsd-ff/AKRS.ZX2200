namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ComponentsTeach
{
    partial class FrmInputNumberOfSteps
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
            this.GcTestComponents = new DevExpress.XtraEditors.GroupControl();
            this.SpNumberOfSteps = new DevExpress.XtraEditors.SpinEdit();
            this.BtnBack = new DevExpress.XtraEditors.SimpleButton();
            this.BtnDone = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GcTestComponents)).BeginInit();
            this.GcTestComponents.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpNumberOfSteps.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GcTestComponents
            // 
            this.GcTestComponents.Controls.Add(this.SpNumberOfSteps);
            this.GcTestComponents.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcTestComponents.Location = new System.Drawing.Point(12, 12);
            this.GcTestComponents.Name = "GcTestComponents";
            this.GcTestComponents.Size = new System.Drawing.Size(466, 62);
            this.GcTestComponents.TabIndex = 16;
            this.GcTestComponents.Text = "请输入芯片间距校准自动跳过个数";
            // 
            // SpNumberOfSteps
            // 
            this.SpNumberOfSteps.EditValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpNumberOfSteps.Location = new System.Drawing.Point(5, 26);
            this.SpNumberOfSteps.Name = "SpNumberOfSteps";
            this.SpNumberOfSteps.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpNumberOfSteps.Properties.IsFloatValue = false;
            this.SpNumberOfSteps.Properties.MaskSettings.Set("mask", "N00");
            this.SpNumberOfSteps.Properties.MaxValue = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.SpNumberOfSteps.Properties.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SpNumberOfSteps.Size = new System.Drawing.Size(456, 20);
            this.SpNumberOfSteps.TabIndex = 0;
            // 
            // BtnBack
            // 
            this.BtnBack.Location = new System.Drawing.Point(17, 99);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(100, 23);
            this.BtnBack.TabIndex = 17;
            this.BtnBack.Text = "返回";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnDone
            // 
            this.BtnDone.Location = new System.Drawing.Point(195, 99);
            this.BtnDone.Name = "BtnDone";
            this.BtnDone.Size = new System.Drawing.Size(100, 23);
            this.BtnDone.TabIndex = 17;
            this.BtnDone.Text = "确认";
            this.BtnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(373, 99);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(100, 23);
            this.BtnCancel.TabIndex = 17;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // FrmInputNumberOfSteps
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 164);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnDone);
            this.Controls.Add(this.BtnBack);
            this.Controls.Add(this.GcTestComponents);
            this.Name = "FrmInputNumberOfSteps";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "跳过步数";
            ((System.ComponentModel.ISupportInitialize)(this.GcTestComponents)).EndInit();
            this.GcTestComponents.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.SpNumberOfSteps.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GcTestComponents;
        private DevExpress.XtraEditors.SpinEdit SpNumberOfSteps;
        private DevExpress.XtraEditors.SimpleButton BtnBack;
        private DevExpress.XtraEditors.SimpleButton BtnDone;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
    }
}