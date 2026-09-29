namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    partial class UcLight
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
            this.components = new System.ComponentModel.Container();
            this.GpLightName = new DevExpress.XtraEditors.GroupControl();
            this.TbLight = new DevExpress.XtraEditors.TrackBarControl();
            this.SpIntensity = new DevExpress.XtraEditors.SpinEdit();
            this.imageComboBoxEdit1 = new DevExpress.XtraEditors.ImageComboBoxEdit();
            this.svgImageCollection1 = new DevExpress.Utils.SvgImageCollection(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GpLightName)).BeginInit();
            this.GpLightName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntensity.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).BeginInit();
            this.SuspendLayout();
            // 
            // GpLightName
            // 
            this.GpLightName.Controls.Add(this.TbLight);
            this.GpLightName.Controls.Add(this.SpIntensity);
            this.GpLightName.Controls.Add(this.imageComboBoxEdit1);
            this.GpLightName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GpLightName.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpLightName.Location = new System.Drawing.Point(5, 5);
            this.GpLightName.Name = "GpLightName";
            this.GpLightName.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.GpLightName.Size = new System.Drawing.Size(400, 47);
            this.GpLightName.TabIndex = 16;
            this.GpLightName.Text = "Light";
            // 
            // TbLight
            // 
            this.TbLight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbLight.EditValue = null;
            this.TbLight.Location = new System.Drawing.Point(5, 23);
            this.TbLight.Name = "TbLight";
            this.TbLight.Properties.AutoSize = false;
            this.TbLight.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TbLight.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TbLight.Properties.Maximum = 100;
            this.TbLight.Properties.TickFrequency = 2;
            this.TbLight.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbLight.Size = new System.Drawing.Size(243, 22);
            this.TbLight.TabIndex = 7;
            this.TbLight.EditValueChanged += new System.EventHandler(this.TbLight_EditValueChanged);
            // 
            // SpIntensity
            // 
            this.SpIntensity.Dock = System.Windows.Forms.DockStyle.Right;
            this.SpIntensity.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpIntensity.Location = new System.Drawing.Point(248, 23);
            this.SpIntensity.Margin = new System.Windows.Forms.Padding(2);
            this.SpIntensity.Name = "SpIntensity";
            this.SpIntensity.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpIntensity.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpIntensity.Size = new System.Drawing.Size(75, 20);
            this.SpIntensity.TabIndex = 8;
            this.SpIntensity.EditValueChanged += new System.EventHandler(this.SpIntensity_EditValueChanged);
            // 
            // imageComboBoxEdit1
            // 
            this.imageComboBoxEdit1.Dock = System.Windows.Forms.DockStyle.Right;
            this.imageComboBoxEdit1.EditValue = "imageComboBoxEdit1";
            this.imageComboBoxEdit1.Location = new System.Drawing.Point(323, 23);
            this.imageComboBoxEdit1.Margin = new System.Windows.Forms.Padding(8, 2, 2, 2);
            this.imageComboBoxEdit1.Name = "imageComboBoxEdit1";
            this.imageComboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.imageComboBoxEdit1.Properties.DropDownRows = 10;
            this.imageComboBoxEdit1.Properties.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, 2),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 1, 1),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 2, 8),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 3, 3),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 4, 7),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 5, 5),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 6, 4),
            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 7, 6)});
            this.imageComboBoxEdit1.Properties.SmallImages = this.svgImageCollection1;
            this.imageComboBoxEdit1.Size = new System.Drawing.Size(72, 20);
            this.imageComboBoxEdit1.TabIndex = 17;
            this.imageComboBoxEdit1.SelectedIndexChanged += new System.EventHandler(this.ImageComboBoxEdit1_SelectedIndexChanged);
            // 
            // svgImageCollection1
            // 
            this.svgImageCollection1.Add("Customize", "Customize", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Blue", "Blue", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("red", "red", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Cyan", "Cyan", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Customize_1", "Customize", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Magenta", "Magenta", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("White", "White", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Yellow", "Yellow", typeof(AKRS.ZX2200.Properties.Resources));
            this.svgImageCollection1.Add("Greed", "Greed", typeof(AKRS.ZX2200.Properties.Resources));
            // 
            // UcLight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GpLightName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UcLight";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(410, 57);
            ((System.ComponentModel.ISupportInitialize)(this.GpLightName)).EndInit();
            this.GpLightName.ResumeLayout(false);
            this.GpLightName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbLight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpIntensity.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageComboBoxEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.svgImageCollection1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl GpLightName;
        private DevExpress.XtraEditors.TrackBarControl TbLight;
        private DevExpress.Utils.SvgImageCollection svgImageCollection1;
        private DevExpress.XtraEditors.ImageComboBoxEdit imageComboBoxEdit1;
        private DevExpress.XtraEditors.SpinEdit SpIntensity;
    }
}
