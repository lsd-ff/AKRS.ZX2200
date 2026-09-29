namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    partial class UcLoderSetting
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcLoderSetting));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.GcAllocations = new DevExpress.XtraEditors.GroupControl();
            this.CmbCurLoader = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).BeginInit();
            this.GcAllocations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbCurLoader.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GcAllocations
            // 
            this.GcAllocations.Controls.Add(this.CmbCurLoader);
            this.GcAllocations.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcAllocations.Location = new System.Drawing.Point(26, 24);
            this.GcAllocations.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.GcAllocations.Name = "GcAllocations";
            this.GcAllocations.Size = new System.Drawing.Size(592, 93);
            this.GcAllocations.TabIndex = 3;
            this.GcAllocations.Text = "当前上料仓";
            // 
            // CmbCurLoader
            // 
            this.CmbCurLoader.Location = new System.Drawing.Point(63, 42);
            this.CmbCurLoader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbCurLoader.Name = "CmbCurLoader";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbCurLoader.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbCurLoader.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbCurLoader.Size = new System.Drawing.Size(476, 26);
            this.CmbCurLoader.TabIndex = 5;
            this.CmbCurLoader.SelectedIndexChanged += new System.EventHandler(this.CmbCurLoader_SelectedIndexChanged);
            this.CmbCurLoader.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbCurLoader_ButtonClick);
            // 
            // UcLoderSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GcAllocations);
            this.Name = "UcLoderSetting";
            this.Size = new System.Drawing.Size(696, 682);
            ((System.ComponentModel.ISupportInitialize)(this.GcAllocations)).EndInit();
            this.GcAllocations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbCurLoader.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GcAllocations;
        private DevExpress.XtraEditors.ComboBoxEdit CmbCurLoader;
    }
}
