namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.WaferHandlingTeach
{
    partial class FrmRegripSet
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
            this.components = new System.ComponentModel.Container();
            this.groupControl9 = new DevExpress.XtraEditors.GroupControl();
            this.TbRegripFeedrate = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.CeIsNeedRegrip = new DevExpress.XtraEditors.CheckEdit();
            this.SpRegripFeedrate = new DevExpress.XtraEditors.SpinEdit();
            this.SpRegripDistance = new DevExpress.XtraEditors.SpinEdit();
            this.SpFirstPullDistance = new DevExpress.XtraEditors.SpinEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.TbSlowFeedrate = new DevExpress.XtraEditors.ZoomTrackBarControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl9 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl10 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl12 = new DevExpress.XtraEditors.LabelControl();
            this.CeIsNeedSlowTravel = new DevExpress.XtraEditors.CheckEdit();
            this.SpSlowFeedrate = new DevExpress.XtraEditors.SpinEdit();
            this.SpSlowDistance = new DevExpress.XtraEditors.SpinEdit();
            this.BtnNext = new DevExpress.XtraEditors.SimpleButton();
            this.BtnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).BeginInit();
            this.groupControl9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbRegripFeedrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbRegripFeedrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNeedRegrip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRegripFeedrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRegripDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpFirstPullDistance.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbSlowFeedrate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbSlowFeedrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNeedSlowTravel.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowFeedrate.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowDistance.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl9
            // 
            this.groupControl9.Controls.Add(this.TbRegripFeedrate);
            this.groupControl9.Controls.Add(this.labelControl6);
            this.groupControl9.Controls.Add(this.labelControl4);
            this.groupControl9.Controls.Add(this.labelControl2);
            this.groupControl9.Controls.Add(this.labelControl5);
            this.groupControl9.Controls.Add(this.labelControl3);
            this.groupControl9.Controls.Add(this.labelControl1);
            this.groupControl9.Controls.Add(this.CeIsNeedRegrip);
            this.groupControl9.Controls.Add(this.SpRegripFeedrate);
            this.groupControl9.Controls.Add(this.SpRegripDistance);
            this.groupControl9.Controls.Add(this.SpFirstPullDistance);
            this.groupControl9.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl9.Location = new System.Drawing.Point(3, 28);
            this.groupControl9.Name = "groupControl9";
            this.groupControl9.Size = new System.Drawing.Size(325, 240);
            this.groupControl9.TabIndex = 2;
            this.groupControl9.Text = "二次夹";
            // 
            // TbRegripFeedrate
            // 
            this.TbRegripFeedrate.EditValue = 20;
            this.TbRegripFeedrate.Location = new System.Drawing.Point(5, 206);
            this.TbRegripFeedrate.Name = "TbRegripFeedrate";
            this.TbRegripFeedrate.Properties.Maximum = 100;
            this.TbRegripFeedrate.Size = new System.Drawing.Size(271, 16);
            this.TbRegripFeedrate.TabIndex = 47;
            this.TbRegripFeedrate.Value = 20;
            this.TbRegripFeedrate.EditValueChanged += new System.EventHandler(this.TbRegripFeedrate_EditValueChanged);
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(283, 174);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(12, 14);
            this.labelControl6.TabIndex = 46;
            this.labelControl6.Text = "%";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(283, 125);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(20, 14);
            this.labelControl4.TabIndex = 46;
            this.labelControl4.Text = "mm";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(283, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(20, 14);
            this.labelControl2.TabIndex = 46;
            this.labelControl2.Text = "mm";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(5, 151);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(60, 14);
            this.labelControl5.TabIndex = 46;
            this.labelControl5.Text = "二次夹速率";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(5, 102);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(60, 14);
            this.labelControl3.TabIndex = 46;
            this.labelControl3.Text = "二次夹距离";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(5, 53);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(84, 14);
            this.labelControl1.TabIndex = 46;
            this.labelControl1.Text = "第一次拉出距离";
            // 
            // CeIsNeedRegrip
            // 
            this.CeIsNeedRegrip.Location = new System.Drawing.Point(5, 26);
            this.CeIsNeedRegrip.Name = "CeIsNeedRegrip";
            this.CeIsNeedRegrip.Properties.Caption = "是否使用";
            this.CeIsNeedRegrip.Size = new System.Drawing.Size(206, 20);
            this.CeIsNeedRegrip.TabIndex = 45;
            // 
            // SpRegripFeedrate
            // 
            this.SpRegripFeedrate.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRegripFeedrate.Location = new System.Drawing.Point(5, 171);
            this.SpRegripFeedrate.Name = "SpRegripFeedrate";
            this.SpRegripFeedrate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRegripFeedrate.Properties.IsFloatValue = false;
            this.SpRegripFeedrate.Properties.MaskSettings.Set("mask", "N00");
            this.SpRegripFeedrate.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpRegripFeedrate.Size = new System.Drawing.Size(271, 20);
            this.SpRegripFeedrate.TabIndex = 0;
            this.SpRegripFeedrate.EditValueChanged += new System.EventHandler(this.SpRegripFeedrate_EditValueChanged);
            // 
            // SpRegripDistance
            // 
            this.SpRegripDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpRegripDistance.Location = new System.Drawing.Point(5, 122);
            this.SpRegripDistance.Name = "SpRegripDistance";
            this.SpRegripDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpRegripDistance.Properties.MaskSettings.Set("mask", "");
            this.SpRegripDistance.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpRegripDistance.Size = new System.Drawing.Size(271, 20);
            this.SpRegripDistance.TabIndex = 0;
            // 
            // SpFirstPullDistance
            // 
            this.SpFirstPullDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpFirstPullDistance.Location = new System.Drawing.Point(5, 73);
            this.SpFirstPullDistance.Name = "SpFirstPullDistance";
            this.SpFirstPullDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpFirstPullDistance.Properties.MaskSettings.Set("mask", "");
            this.SpFirstPullDistance.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpFirstPullDistance.Size = new System.Drawing.Size(271, 20);
            this.SpFirstPullDistance.TabIndex = 0;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.TbSlowFeedrate);
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.labelControl9);
            this.groupControl1.Controls.Add(this.labelControl10);
            this.groupControl1.Controls.Add(this.labelControl12);
            this.groupControl1.Controls.Add(this.CeIsNeedSlowTravel);
            this.groupControl1.Controls.Add(this.SpSlowFeedrate);
            this.groupControl1.Controls.Add(this.SpSlowDistance);
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(3, 274);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(325, 193);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "慢速过程";
            // 
            // TbSlowFeedrate
            // 
            this.TbSlowFeedrate.EditValue = 20;
            this.TbSlowFeedrate.Location = new System.Drawing.Point(5, 157);
            this.TbSlowFeedrate.Name = "TbSlowFeedrate";
            this.TbSlowFeedrate.Properties.Maximum = 100;
            this.TbSlowFeedrate.Size = new System.Drawing.Size(271, 16);
            this.TbSlowFeedrate.TabIndex = 47;
            this.TbSlowFeedrate.Value = 20;
            this.TbSlowFeedrate.EditValueChanged += new System.EventHandler(this.TbSlowFeedrate_EditValueChanged);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(283, 125);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(12, 14);
            this.labelControl7.TabIndex = 46;
            this.labelControl7.Text = "%";
            // 
            // labelControl9
            // 
            this.labelControl9.Location = new System.Drawing.Point(283, 76);
            this.labelControl9.Name = "labelControl9";
            this.labelControl9.Size = new System.Drawing.Size(20, 14);
            this.labelControl9.TabIndex = 46;
            this.labelControl9.Text = "mm";
            // 
            // labelControl10
            // 
            this.labelControl10.Location = new System.Drawing.Point(5, 102);
            this.labelControl10.Name = "labelControl10";
            this.labelControl10.Size = new System.Drawing.Size(48, 14);
            this.labelControl10.TabIndex = 46;
            this.labelControl10.Text = "慢速速率";
            // 
            // labelControl12
            // 
            this.labelControl12.Location = new System.Drawing.Point(5, 53);
            this.labelControl12.Name = "labelControl12";
            this.labelControl12.Size = new System.Drawing.Size(48, 14);
            this.labelControl12.TabIndex = 46;
            this.labelControl12.Text = "慢速距离";
            // 
            // CeIsNeedSlowTravel
            // 
            this.CeIsNeedSlowTravel.Location = new System.Drawing.Point(5, 26);
            this.CeIsNeedSlowTravel.Name = "CeIsNeedSlowTravel";
            this.CeIsNeedSlowTravel.Properties.Caption = "是否使用";
            this.CeIsNeedSlowTravel.Size = new System.Drawing.Size(206, 20);
            this.CeIsNeedSlowTravel.TabIndex = 45;
            // 
            // SpSlowFeedrate
            // 
            this.SpSlowFeedrate.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSlowFeedrate.Location = new System.Drawing.Point(5, 122);
            this.SpSlowFeedrate.Name = "SpSlowFeedrate";
            this.SpSlowFeedrate.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowFeedrate.Properties.IsFloatValue = false;
            this.SpSlowFeedrate.Properties.MaskSettings.Set("mask", "N00");
            this.SpSlowFeedrate.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpSlowFeedrate.Size = new System.Drawing.Size(271, 20);
            this.SpSlowFeedrate.TabIndex = 0;
            this.SpSlowFeedrate.EditValueChanged += new System.EventHandler(this.SpSlowFeedrate_EditValueChanged);
            // 
            // SpSlowDistance
            // 
            this.SpSlowDistance.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpSlowDistance.Location = new System.Drawing.Point(5, 73);
            this.SpSlowDistance.Name = "SpSlowDistance";
            this.SpSlowDistance.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpSlowDistance.Properties.MaskSettings.Set("mask", "");
            this.SpSlowDistance.Properties.MaxValue = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SpSlowDistance.Size = new System.Drawing.Size(271, 20);
            this.SpSlowDistance.TabIndex = 0;
            // 
            // BtnNext
            // 
            this.BtnNext.Location = new System.Drawing.Point(3, 500);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(75, 23);
            this.BtnNext.TabIndex = 3;
            this.BtnNext.Text = "下一步";
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Location = new System.Drawing.Point(253, 500);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "取消";
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tag = "FrmRegripSet";
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FrmRegripSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(341, 543);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.groupControl9);
            this.Name = "FrmRegripSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "二次夹设置";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmRegripSet_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl9)).EndInit();
            this.groupControl9.ResumeLayout(false);
            this.groupControl9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbRegripFeedrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbRegripFeedrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNeedRegrip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRegripFeedrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpRegripDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpFirstPullDistance.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbSlowFeedrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbSlowFeedrate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CeIsNeedSlowTravel.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowFeedrate.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpSlowDistance.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl9;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.CheckEdit CeIsNeedRegrip;
        private DevExpress.XtraEditors.SpinEdit SpFirstPullDistance;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbRegripFeedrate;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.SpinEdit SpRegripFeedrate;
        private DevExpress.XtraEditors.SpinEdit SpRegripDistance;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.ZoomTrackBarControl TbSlowFeedrate;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl9;
        private DevExpress.XtraEditors.LabelControl labelControl10;
        private DevExpress.XtraEditors.LabelControl labelControl12;
        private DevExpress.XtraEditors.CheckEdit CeIsNeedSlowTravel;
        private DevExpress.XtraEditors.SpinEdit SpSlowFeedrate;
        private DevExpress.XtraEditors.SpinEdit SpSlowDistance;
        private DevExpress.XtraEditors.SimpleButton BtnNext;
        private DevExpress.XtraEditors.SimpleButton BtnCancel;
        private System.Windows.Forms.Timer timer1;
    }
}
