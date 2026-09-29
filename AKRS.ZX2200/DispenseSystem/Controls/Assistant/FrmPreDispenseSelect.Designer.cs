namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    partial class FrmPreDispenseSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPreDispenseSelect));
            this.ucLight1 = new AKRS.ZX2200.Infrastructure.Controls.Currency.UcLight();
            this.ucLight2 = new AKRS.ZX2200.Infrastructure.Controls.Currency.UcLight();
            this.GcSubstrate = new DevExpress.XtraEditors.GroupControl();
            this.BtnSubstrateMoveDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSubstrateMoveRight = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSubstrateMoveLeft = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSubstrateMoveUp = new DevExpress.XtraEditors.SimpleButton();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtNext = new DevExpress.XtraEditors.SimpleButton();
            this.BtVision = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.GcSubstrate)).BeginInit();
            this.GcSubstrate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ucLight1
            // 
            this.ucLight1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucLight1.Enabled = false;
            this.ucLight1.Location = new System.Drawing.Point(0, 0);
            this.ucLight1.Margin = new System.Windows.Forms.Padding(1);
            this.ucLight1.Name = "ucLight1";
            this.ucLight1.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucLight1.Size = new System.Drawing.Size(797, 68);
            this.ucLight1.TabIndex = 24;
            // 
            // ucLight2
            // 
            this.ucLight2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucLight2.Enabled = false;
            this.ucLight2.Location = new System.Drawing.Point(0, 68);
            this.ucLight2.Margin = new System.Windows.Forms.Padding(1);
            this.ucLight2.Name = "ucLight2";
            this.ucLight2.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.ucLight2.Size = new System.Drawing.Size(797, 72);
            this.ucLight2.TabIndex = 25;
            // 
            // GcSubstrate
            // 
            this.GcSubstrate.Controls.Add(this.labelControl1);
            this.GcSubstrate.Controls.Add(this.BtnSubstrateMoveDown);
            this.GcSubstrate.Controls.Add(this.BtnSubstrateMoveRight);
            this.GcSubstrate.Controls.Add(this.BtnSubstrateMoveLeft);
            this.GcSubstrate.Controls.Add(this.BtnSubstrateMoveUp);
            this.GcSubstrate.Dock = System.Windows.Forms.DockStyle.Top;
            this.GcSubstrate.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcSubstrate.Location = new System.Drawing.Point(0, 140);
            this.GcSubstrate.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.GcSubstrate.Name = "GcSubstrate";
            this.GcSubstrate.Size = new System.Drawing.Size(797, 295);
            this.GcSubstrate.TabIndex = 26;
            this.GcSubstrate.Text = "基板";
            // 
            // BtnSubstrateMoveDown
            // 
            this.BtnSubstrateMoveDown.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.BtnSubstrateMoveDown.Appearance.Options.UseBorderColor = true;
            this.BtnSubstrateMoveDown.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnSubstrateMoveDown.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubstrateMoveDown.ImageOptions.SvgImage")));
            this.BtnSubstrateMoveDown.Location = new System.Drawing.Point(345, 213);
            this.BtnSubstrateMoveDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSubstrateMoveDown.Name = "BtnSubstrateMoveDown";
            this.BtnSubstrateMoveDown.Size = new System.Drawing.Size(70, 62);
            this.BtnSubstrateMoveDown.TabIndex = 10;
            this.BtnSubstrateMoveDown.Click += new System.EventHandler(this.BtnSubstrateMoveDown_Click);
            // 
            // BtnSubstrateMoveRight
            // 
            this.BtnSubstrateMoveRight.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.BtnSubstrateMoveRight.Appearance.Options.UseBorderColor = true;
            this.BtnSubstrateMoveRight.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnSubstrateMoveRight.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubstrateMoveRight.ImageOptions.SvgImage")));
            this.BtnSubstrateMoveRight.Location = new System.Drawing.Point(421, 147);
            this.BtnSubstrateMoveRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSubstrateMoveRight.Name = "BtnSubstrateMoveRight";
            this.BtnSubstrateMoveRight.Size = new System.Drawing.Size(70, 62);
            this.BtnSubstrateMoveRight.TabIndex = 9;
            this.BtnSubstrateMoveRight.Click += new System.EventHandler(this.BtnSubstrateMoveRight_Click);
            // 
            // BtnSubstrateMoveLeft
            // 
            this.BtnSubstrateMoveLeft.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.BtnSubstrateMoveLeft.Appearance.Options.UseBorderColor = true;
            this.BtnSubstrateMoveLeft.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnSubstrateMoveLeft.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubstrateMoveLeft.ImageOptions.SvgImage")));
            this.BtnSubstrateMoveLeft.Location = new System.Drawing.Point(269, 147);
            this.BtnSubstrateMoveLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSubstrateMoveLeft.Name = "BtnSubstrateMoveLeft";
            this.BtnSubstrateMoveLeft.Size = new System.Drawing.Size(70, 62);
            this.BtnSubstrateMoveLeft.TabIndex = 8;
            this.BtnSubstrateMoveLeft.Click += new System.EventHandler(this.BtnSubstrateMoveLeft_Click);
            // 
            // BtnSubstrateMoveUp
            // 
            this.BtnSubstrateMoveUp.Appearance.BorderColor = System.Drawing.Color.Transparent;
            this.BtnSubstrateMoveUp.Appearance.Options.UseBorderColor = true;
            this.BtnSubstrateMoveUp.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.BtnSubstrateMoveUp.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubstrateMoveUp.ImageOptions.SvgImage")));
            this.BtnSubstrateMoveUp.Location = new System.Drawing.Point(345, 80);
            this.BtnSubstrateMoveUp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.BtnSubstrateMoveUp.Name = "BtnSubstrateMoveUp";
            this.BtnSubstrateMoveUp.Size = new System.Drawing.Size(70, 62);
            this.BtnSubstrateMoveUp.TabIndex = 7;
            this.BtnSubstrateMoveUp.Click += new System.EventHandler(this.BtnSubstrateMoveUp_Click);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 55F)});
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Controls.Add(this.BtVision);
            this.tablePanel1.Controls.Add(this.BtNext);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(0, 435);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(797, 82);
            this.tablePanel1.TabIndex = 27;
            // 
            // BtNext
            // 
            this.tablePanel1.SetColumn(this.BtNext, 0);
            this.BtNext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtNext.Location = new System.Drawing.Point(3, 3);
            this.BtNext.Name = "BtNext";
            this.tablePanel1.SetRow(this.BtNext, 0);
            this.BtNext.Size = new System.Drawing.Size(260, 76);
            this.BtNext.TabIndex = 0;
            this.BtNext.Text = "下一步";
            this.BtNext.Click += new System.EventHandler(this.BtNext_Click);
            // 
            // BtVision
            // 
            this.tablePanel1.SetColumn(this.BtVision, 1);
            this.BtVision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtVision.Location = new System.Drawing.Point(269, 3);
            this.BtVision.Name = "BtVision";
            this.tablePanel1.SetRow(this.BtVision, 0);
            this.BtVision.Size = new System.Drawing.Size(260, 76);
            this.BtVision.TabIndex = 1;
            this.BtVision.Text = "实时界面";
            this.BtVision.Click += new System.EventHandler(this.BtVision_Click);
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 2);
            this.BtCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtCancel.Location = new System.Drawing.Point(534, 3);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(260, 76);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(142, 26);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(522, 35);
            this.labelControl1.TabIndex = 11;
            this.labelControl1.Text = "移动方向盘箭头到需要检测预点胶的位置";
            // 
            // FrmPreDispenseSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(797, 518);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.GcSubstrate);
            this.Controls.Add(this.ucLight2);
            this.Controls.Add(this.ucLight1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FrmPreDispenseSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPreDispenseSelect";
            this.Load += new System.EventHandler(this.FrmPreDispenseSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GcSubstrate)).EndInit();
            this.GcSubstrate.ResumeLayout(false);
            this.GcSubstrate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Infrastructure.Controls.Currency.UcLight ucLight1;
        private Infrastructure.Controls.Currency.UcLight ucLight2;
        private DevExpress.XtraEditors.GroupControl GcSubstrate;
        private DevExpress.XtraEditors.SimpleButton BtnSubstrateMoveDown;
        private DevExpress.XtraEditors.SimpleButton BtnSubstrateMoveRight;
        private DevExpress.XtraEditors.SimpleButton BtnSubstrateMoveLeft;
        private DevExpress.XtraEditors.SimpleButton BtnSubstrateMoveUp;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtVision;
        private DevExpress.XtraEditors.SimpleButton BtNext;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}