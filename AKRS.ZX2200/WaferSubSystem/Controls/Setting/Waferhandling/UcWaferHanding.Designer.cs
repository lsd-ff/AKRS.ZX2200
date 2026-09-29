namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.Waferhandling
{
    partial class UcWaferHanding
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcWaferHanding));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.GpWaferChangeDataset = new DevExpress.XtraEditors.GroupControl();
            this.CmbWafer = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GpWaferChangeDataset)).BeginInit();
            this.GpWaferChangeDataset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbWafer.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GpWaferChangeDataset
            // 
            this.GpWaferChangeDataset.Controls.Add(this.CmbWafer);
            this.GpWaferChangeDataset.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpWaferChangeDataset.Location = new System.Drawing.Point(38, 38);
            this.GpWaferChangeDataset.Name = "GpWaferChangeDataset";
            this.GpWaferChangeDataset.Size = new System.Drawing.Size(471, 127);
            this.GpWaferChangeDataset.TabIndex = 0;
            this.GpWaferChangeDataset.Text = "晶圆更换数据集";
            // 
            // CmbWafer
            // 
            this.CmbWafer.Location = new System.Drawing.Point(58, 58);
            this.CmbWafer.Name = "CmbWafer";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbWafer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbWafer.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbWafer.Size = new System.Drawing.Size(356, 24);
            this.CmbWafer.TabIndex = 4;
            this.CmbWafer.SelectedIndexChanged += new System.EventHandler(this.CmbWafer_SelectedIndexChanged);
            this.CmbWafer.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbWafer_ButtonClick);
            // 
            // UcWaferHanding
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GpWaferChangeDataset);
            this.Name = "UcWaferHanding";
            this.Size = new System.Drawing.Size(1139, 790);
            ((System.ComponentModel.ISupportInitialize)(this.GpWaferChangeDataset)).EndInit();
            this.GpWaferChangeDataset.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbWafer.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GpWaferChangeDataset;
        private DevExpress.XtraEditors.ComboBoxEdit CmbWafer;
    }
}
