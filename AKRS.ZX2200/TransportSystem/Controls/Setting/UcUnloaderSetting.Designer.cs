namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    partial class UcUnloaderSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcUnloaderSetting));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.GcAllocations = new DevExpress.XtraEditors.GroupControl();
            this.CmbCurUnLoader = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).BeginInit();
            this.GcAllocations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbCurUnLoader.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GcAllocations
            // 
            this.GcAllocations.Controls.Add(this.CmbCurUnLoader);
            this.GcAllocations.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcAllocations.Location = new System.Drawing.Point(33, 28);
            this.GcAllocations.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcAllocations.Name = "GcAllocations";
            this.GcAllocations.Size = new System.Drawing.Size(592, 80);
            this.GcAllocations.TabIndex = 4;
            this.GcAllocations.Text = "当前下料仓";
            // 
            // CmbCurUnLoader
            // 
            this.CmbCurUnLoader.Location = new System.Drawing.Point(61, 32);
            this.CmbCurUnLoader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbCurUnLoader.Name = "CmbCurUnLoader";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbCurUnLoader.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbCurUnLoader.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbCurUnLoader.Size = new System.Drawing.Size(464, 26);
            this.CmbCurUnLoader.TabIndex = 5;
            this.CmbCurUnLoader.SelectedIndexChanged += new System.EventHandler(this.CmbCurUnLoader_SelectedIndexChanged);
            this.CmbCurUnLoader.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbCurUnLoader_ButtonClick);
            // 
            // UcUnloaderSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GcAllocations);
            this.Name = "UcUnloaderSetting";
            this.Size = new System.Drawing.Size(692, 727);
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).EndInit();
            this.GcAllocations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbCurUnLoader.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GcAllocations;
        private DevExpress.XtraEditors.ComboBoxEdit CmbCurUnLoader;
    }
}
