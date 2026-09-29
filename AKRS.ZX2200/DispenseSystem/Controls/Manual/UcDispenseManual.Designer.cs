namespace AKRS.ZX2200.DispenseSystem.Controls.Manual{
    partial class UcDispenseManual
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
            this.BtChangeDispensing = new DevExpress.XtraEditors.SimpleButton();
            this.BtChangeDispenser = new DevExpress.XtraEditors.SimpleButton();
            this.BtAssistantPrePlate = new DevExpress.XtraEditors.SimpleButton();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.BtCleanPreDispensePlate = new DevExpress.XtraEditors.SimpleButton();
            this.BtPredispense = new DevExpress.XtraEditors.SimpleButton();
            this.BtPrintEpoxyMove = new DevExpress.XtraEditors.SimpleButton();
            this.BtAssistantDistance = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // BtChangeDispensing
            // 
            this.BtChangeDispensing.Location = new System.Drawing.Point(49, 9);
            this.BtChangeDispensing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtChangeDispensing.Name = "BtChangeDispensing";
            this.BtChangeDispensing.Size = new System.Drawing.Size(285, 54);
            this.BtChangeDispensing.TabIndex = 0;
            this.BtChangeDispensing.Text = "挤胶";
            this.BtChangeDispensing.Click += new System.EventHandler(this.BtChangeDispensing_Click);
            // 
            // BtChangeDispenser
            // 
            this.BtChangeDispenser.Location = new System.Drawing.Point(49, 93);
            this.BtChangeDispenser.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtChangeDispenser.Name = "BtChangeDispenser";
            this.BtChangeDispenser.Size = new System.Drawing.Size(285, 54);
            this.BtChangeDispenser.TabIndex = 2;
            this.BtChangeDispenser.Text = "更换点胶头";
            this.BtChangeDispenser.Click += new System.EventHandler(this.BtChangeDispenser_Click);
            // 
            // BtAssistantPrePlate
            // 
            this.BtAssistantPrePlate.Location = new System.Drawing.Point(49, 248);
            this.BtAssistantPrePlate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtAssistantPrePlate.Name = "BtAssistantPrePlate";
            this.BtAssistantPrePlate.Size = new System.Drawing.Size(285, 54);
            this.BtAssistantPrePlate.TabIndex = 3;
            this.BtAssistantPrePlate.Text = "示教预点胶板";
            this.BtAssistantPrePlate.Click += new System.EventHandler(this.BtAssistantPrePlate_Click);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tag = "UcDispenseManual";
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // BtCleanPreDispensePlate
            // 
            this.BtCleanPreDispensePlate.Location = new System.Drawing.Point(49, 324);
            this.BtCleanPreDispensePlate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtCleanPreDispensePlate.Name = "BtCleanPreDispensePlate";
            this.BtCleanPreDispensePlate.Size = new System.Drawing.Size(285, 54);
            this.BtCleanPreDispensePlate.TabIndex = 4;
            this.BtCleanPreDispensePlate.Text = "清洁预点胶板";
            this.BtCleanPreDispensePlate.Click += new System.EventHandler(this.BtCleanPreDispensePlate_Click);
            // 
            // BtPredispense
            // 
            this.BtPredispense.Location = new System.Drawing.Point(49, 174);
            this.BtPredispense.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtPredispense.Name = "BtPredispense";
            this.BtPredispense.Size = new System.Drawing.Size(285, 54);
            this.BtPredispense.TabIndex = 5;
            this.BtPredispense.Text = "预点胶";
            this.BtPredispense.Click += new System.EventHandler(this.BtPreDispense_Click);
            // 
            // BtPrintEpoxyMove
            // 
            this.BtPrintEpoxyMove.Location = new System.Drawing.Point(49, 403);
            this.BtPrintEpoxyMove.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtPrintEpoxyMove.Name = "BtPrintEpoxyMove";
            this.BtPrintEpoxyMove.Size = new System.Drawing.Size(285, 54);
            this.BtPrintEpoxyMove.TabIndex = 6;
            this.BtPrintEpoxyMove.Text = "刮胶盘转动";
            this.BtPrintEpoxyMove.Click += new System.EventHandler(this.SpPrintEpoxyMove_Click);
            // 
            // BtAssistantDistance
            // 
            this.BtAssistantDistance.Location = new System.Drawing.Point(49, 480);
            this.BtAssistantDistance.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtAssistantDistance.Name = "BtAssistantDistance";
            this.BtAssistantDistance.Size = new System.Drawing.Size(285, 54);
            this.BtAssistantDistance.TabIndex = 7;
            this.BtAssistantDistance.Text = "示教预点胶板信号触发距离";
            this.BtAssistantDistance.Click += new System.EventHandler(this.BtAssistantDistance_Click);
            // 
            // UcDispenseManual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtAssistantDistance);
            this.Controls.Add(this.BtPrintEpoxyMove);
            this.Controls.Add(this.BtPredispense);
            this.Controls.Add(this.BtCleanPreDispensePlate);
            this.Controls.Add(this.BtAssistantPrePlate);
            this.Controls.Add(this.BtChangeDispenser);
            this.Controls.Add(this.BtChangeDispensing);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UcDispenseManual";
            this.Size = new System.Drawing.Size(410, 545);
            this.Load += new System.EventHandler(this.UcDispenseManual_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton BtChangeDispensing;
        private DevExpress.XtraEditors.SimpleButton BtChangeDispenser;
        private DevExpress.XtraEditors.SimpleButton BtAssistantPrePlate;
        private System.Windows.Forms.Timer Timer;
        private DevExpress.XtraEditors.SimpleButton BtCleanPreDispensePlate;
        private DevExpress.XtraEditors.SimpleButton BtPredispense;
        private DevExpress.XtraEditors.SimpleButton BtPrintEpoxyMove;
        private DevExpress.XtraEditors.SimpleButton BtAssistantDistance;
    }
}