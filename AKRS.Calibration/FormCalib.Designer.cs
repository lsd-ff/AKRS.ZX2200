namespace AKRS.Calibration
{
    partial class FormCalib
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
            this.renderPanel = new DevExpress.XtraEditors.PanelControl();
            this.BtnRender = new DevExpress.XtraEditors.SimpleButton();
            this.BtnConfig = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BtnSave = new DevExpress.XtraEditors.SimpleButton();
            this.BtnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.BtnRunOnce = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSelectPrc = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboProcedure = new DevExpress.XtraEditors.ComboBoxEdit();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            this.listViewLog = new System.Windows.Forms.ListView();
            this.timeStampHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.infoHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.renderPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboProcedure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // renderPanel
            // 
            this.renderPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.renderPanel.Location = new System.Drawing.Point(0, 0);
            this.renderPanel.Name = "renderPanel";
            this.renderPanel.Size = new System.Drawing.Size(1084, 770);
            this.renderPanel.TabIndex = 0;
            // 
            // BtnRender
            // 
            this.BtnRender.Location = new System.Drawing.Point(1083, 0);
            this.BtnRender.Name = "BtnRender";
            this.BtnRender.Size = new System.Drawing.Size(88, 29);
            this.BtnRender.TabIndex = 1;
            this.BtnRender.Text = "图像显示";
            this.BtnRender.Click += new System.EventHandler(this.BtnRender_Click);
            // 
            // BtnConfig
            // 
            this.BtnConfig.Location = new System.Drawing.Point(1177, 0);
            this.BtnConfig.Name = "BtnConfig";
            this.BtnConfig.Size = new System.Drawing.Size(89, 29);
            this.BtnConfig.TabIndex = 2;
            this.BtnConfig.Text = "参数配置";
            this.BtnConfig.Click += new System.EventHandler(this.BtnConfig_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.BorderColor = System.Drawing.Color.Black;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.Appearance.Options.UseBorderColor = true;
            this.groupControl1.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.groupControl1.AppearanceCaption.BorderColor = System.Drawing.Color.Gray;
            this.groupControl1.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl1.Controls.Add(this.BtnSave);
            this.groupControl1.Controls.Add(this.BtnLoad);
            this.groupControl1.Controls.Add(this.BtnSelect);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(2, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(337, 96);
            this.groupControl1.TabIndex = 3;
            this.groupControl1.Text = "方案操作";
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(220, 38);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(101, 38);
            this.BtnSave.TabIndex = 4;
            this.BtnSave.Text = "保存方案";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnLoad
            // 
            this.BtnLoad.Location = new System.Drawing.Point(113, 38);
            this.BtnLoad.Name = "BtnLoad";
            this.BtnLoad.Size = new System.Drawing.Size(99, 38);
            this.BtnLoad.TabIndex = 3;
            this.BtnLoad.Text = "加载方案";
            this.BtnLoad.Click += new System.EventHandler(this.BtnLoad_Click);
            // 
            // BtnSelect
            // 
            this.BtnSelect.Location = new System.Drawing.Point(5, 38);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(101, 38);
            this.BtnSelect.TabIndex = 2;
            this.BtnSelect.Text = "选择方案";
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // groupControl2
            // 
            this.groupControl2.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl2.Appearance.BorderColor = System.Drawing.Color.Black;
            this.groupControl2.Appearance.Options.UseBackColor = true;
            this.groupControl2.Appearance.Options.UseBorderColor = true;
            this.groupControl2.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.groupControl2.AppearanceCaption.BorderColor = System.Drawing.Color.Gray;
            this.groupControl2.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl2.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl2.AutoSize = true;
            this.groupControl2.Controls.Add(this.BtnRunOnce);
            this.groupControl2.Controls.Add(this.BtnSelectPrc);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.comboProcedure);
            this.groupControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl2.Location = new System.Drawing.Point(2, 98);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(337, 286);
            this.groupControl2.TabIndex = 4;
            this.groupControl2.Text = "方案操作";
            // 
            // BtnRunOnce
            // 
            this.BtnRunOnce.Location = new System.Drawing.Point(25, 156);
            this.BtnRunOnce.Name = "BtnRunOnce";
            this.BtnRunOnce.Size = new System.Drawing.Size(101, 38);
            this.BtnRunOnce.TabIndex = 4;
            this.BtnRunOnce.Text = "运行一次";
            this.BtnRunOnce.Click += new System.EventHandler(this.BtnRunOnce_Click);
            // 
            // BtnSelectPrc
            // 
            this.BtnSelectPrc.Location = new System.Drawing.Point(25, 92);
            this.BtnSelectPrc.Name = "BtnSelectPrc";
            this.BtnSelectPrc.Size = new System.Drawing.Size(101, 38);
            this.BtnSelectPrc.TabIndex = 3;
            this.BtnSelectPrc.Text = "配置流程";
            this.BtnSelectPrc.Click += new System.EventHandler(this.BtnSelectPrc_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(25, 49);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(60, 14);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "选择流程：";
            // 
            // comboProcedure
            // 
            this.comboProcedure.EditValue = "";
            this.comboProcedure.Location = new System.Drawing.Point(116, 47);
            this.comboProcedure.Name = "comboProcedure";
            this.comboProcedure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboProcedure.Size = new System.Drawing.Size(135, 20);
            this.comboProcedure.TabIndex = 0;
            // 
            // groupControl3
            // 
            this.groupControl3.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl3.Appearance.BorderColor = System.Drawing.Color.Black;
            this.groupControl3.Appearance.Options.UseBackColor = true;
            this.groupControl3.Appearance.Options.UseBorderColor = true;
            this.groupControl3.AppearanceCaption.BackColor = System.Drawing.Color.White;
            this.groupControl3.AppearanceCaption.BorderColor = System.Drawing.Color.Gray;
            this.groupControl3.AppearanceCaption.Options.UseBackColor = true;
            this.groupControl3.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl3.Controls.Add(this.listViewLog);
            this.groupControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl3.Location = new System.Drawing.Point(2, 384);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(337, 320);
            this.groupControl3.TabIndex = 5;
            this.groupControl3.Text = "日志消息";
            // 
            // listViewLog
            // 
            this.listViewLog.BackColor = System.Drawing.Color.Gainsboro;
            this.listViewLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.timeStampHeader,
            this.infoHeader});
            this.listViewLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewLog.ForeColor = System.Drawing.Color.Black;
            this.listViewLog.HideSelection = false;
            this.listViewLog.Location = new System.Drawing.Point(2, 23);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(333, 295);
            this.listViewLog.TabIndex = 1;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // timeStampHeader
            // 
            this.timeStampHeader.Text = "时间";
            this.timeStampHeader.Width = 98;
            // 
            // infoHeader
            // 
            this.infoHeader.Text = "消息";
            this.infoHeader.Width = 120;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl3);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(1084, 64);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(341, 706);
            this.panelControl1.TabIndex = 3;
            // 
            // FormCalib
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1425, 770);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.BtnConfig);
            this.Controls.Add(this.BtnRender);
            this.Controls.Add(this.renderPanel);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FormCalib";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCalib_FormClosing);
            //this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormCalib_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.renderPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboProcedure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl renderPanel;
        private DevExpress.XtraEditors.SimpleButton BtnRender;
        private DevExpress.XtraEditors.SimpleButton BtnConfig;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton BtnSave;
        private DevExpress.XtraEditors.SimpleButton BtnLoad;
        private DevExpress.XtraEditors.SimpleButton BtnSelect;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ListView listViewLog;
        private System.Windows.Forms.ColumnHeader timeStampHeader;
        private System.Windows.Forms.ColumnHeader infoHeader;
        private DevExpress.XtraEditors.ComboBoxEdit comboProcedure;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnSelectPrc;
        private DevExpress.XtraEditors.SimpleButton BtnRunOnce;
    }
}

