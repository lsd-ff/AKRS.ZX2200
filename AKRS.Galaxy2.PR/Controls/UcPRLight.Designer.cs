namespace AKRS.Galaxy2.PR.Controls
{
    partial class UcPRLight
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GpLightName = new DevExpress.XtraEditors.GroupControl();
            this.TbLight = new DevExpress.XtraEditors.TrackBarControl();
            this.SpIntensity = new DevExpress.XtraEditors.SpinEdit();
            this.ImgCbxColorSelect = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GpLightName)).BeginInit();
            this.GpLightName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntensity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCbxColorSelect.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // GpLightName
            // 
            this.GpLightName.Controls.Add(this.TbLight);
            this.GpLightName.Controls.Add(this.SpIntensity);
            this.GpLightName.Controls.Add(this.ImgCbxColorSelect);
            this.GpLightName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GpLightName.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpLightName.Location = new System.Drawing.Point(0, 0);
            this.GpLightName.Margin = new System.Windows.Forms.Padding(4);
            this.GpLightName.Name = "GpLightName";
            this.GpLightName.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GpLightName.Size = new System.Drawing.Size(547, 71);
            this.GpLightName.TabIndex = 18;
            this.GpLightName.Text = "Light";
            // 
            // TbLight
            // 
            this.TbLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLight.EditValue = null;
            this.TbLight.Location = new System.Drawing.Point(6, 28);
            this.TbLight.Margin = new System.Windows.Forms.Padding(4);
            this.TbLight.Name = "TbLight";
            this.TbLight.Properties.AutoSize = false;
            this.TbLight.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TbLight.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TbLight.Properties.Maximum = 100;
            this.TbLight.Properties.TickFrequency = 2;
            this.TbLight.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbLight.Size = new System.Drawing.Size(339, 41);
            this.TbLight.TabIndex = 7;
            // 
            // SpIntensity
            // 
            this.SpIntensity.Dock = System.Windows.Forms.DockStyle.Right;
            this.SpIntensity.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpIntensity.Location = new System.Drawing.Point(345, 28);
            this.SpIntensity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SpIntensity.Name = "SpIntensity";
            this.SpIntensity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpIntensity.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpIntensity.Size = new System.Drawing.Size(100, 24);
            this.SpIntensity.TabIndex = 8;
            // 
            // ImgCbxColorSelect
            // 
            this.ImgCbxColorSelect.Dock = System.Windows.Forms.DockStyle.Right;
            this.ImgCbxColorSelect.EditValue = "imageComboBoxEdit1";
            this.ImgCbxColorSelect.Location = new System.Drawing.Point(445, 28);
            this.ImgCbxColorSelect.Margin = new System.Windows.Forms.Padding(11, 2, 3, 2);
            this.ImgCbxColorSelect.Name = "ImgCbxColorSelect";
            this.ImgCbxColorSelect.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ImgCbxColorSelect.Properties.DropDownRows = 10;
            this.ImgCbxColorSelect.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, 2),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 1, 1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 2, 8),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 3, 3),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 4, 7),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 5, 5),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 6, 4),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 7, 6)});
            this.ImgCbxColorSelect.Size = new System.Drawing.Size(96, 24);
            this.ImgCbxColorSelect.TabIndex = 17;
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("Customize", "Customize", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("Blue", "Blue", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("red", "red", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("Cyan", "Cyan", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("Magenta", "Magenta", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("White", "White", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("Yellow", "Yellow", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            this.svgImageCollection1.Add("Greed", "Greed", typeof(AKRS.Galaxy2.PR.Properties.Resources));
            // 
            // UcPRLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GpLightName);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcPRLight";
            this.Size = new System.Drawing.Size(547, 71);
            ((System.ComponentModel.ISupportInitialize)(this.GpLightName)).EndInit();
            this.GpLightName.ResumeLayout(false);
            this.GpLightName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntensity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImgCbxColorSelect.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GpLightName;
        private DevExpress.XtraEditors.TrackBarControl TbLight;
        public DevExpress.XtraEditors.SpinEdit SpIntensity;
        private DevExpress.XtraEditors.ImageComboBoxEdit ImgCbxColorSelect;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
    }
}
