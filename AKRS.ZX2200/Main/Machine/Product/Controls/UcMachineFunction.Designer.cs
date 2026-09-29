namespace AKRS.ZX2200.Main.Machine.Product.Controls
{
    partial class UcMachineFunction
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
            this.ChkMarkTemperatureOffset = new DevExpress.XtraEditors.CheckEdit();
            this.ChkPostBondOffset = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkMarkTemperatureOffset.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPostBondOffset.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // ChkMarkTemperatureOffset
            // 
            this.ChkMarkTemperatureOffset.Location = new System.Drawing.Point(3, 3);
            this.ChkMarkTemperatureOffset.Name = "ChkMarkTemperatureOffset";
            this.ChkMarkTemperatureOffset.Properties.Caption = "固定位置温度补偿";
            this.ChkMarkTemperatureOffset.Size = new System.Drawing.Size(301, 27);
            this.ChkMarkTemperatureOffset.TabIndex = 0;
            // 
            // ChkPostBondOffset
            // 
            this.ChkPostBondOffset.Location = new System.Drawing.Point(3, 75);
            this.ChkPostBondOffset.Name = "ChkPostBondOffset";
            this.ChkPostBondOffset.Properties.Caption = "固定位置温度补偿";
            this.ChkPostBondOffset.Size = new System.Drawing.Size(301, 27);
            this.ChkPostBondOffset.TabIndex = 1;
            // 
            // UcMachineFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 22F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ChkPostBondOffset);
            this.Controls.Add(this.ChkMarkTemperatureOffset);
            this.Name = "UcMachineFunction";
            this.Size = new System.Drawing.Size(975, 853);
            this.Load += new System.EventHandler(this.UcMachineFunction_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ChkMarkTemperatureOffset.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkPostBondOffset.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.CheckEdit ChkMarkTemperatureOffset;
        private DevExpress.XtraEditors.CheckEdit ChkPostBondOffset;
    }
}
