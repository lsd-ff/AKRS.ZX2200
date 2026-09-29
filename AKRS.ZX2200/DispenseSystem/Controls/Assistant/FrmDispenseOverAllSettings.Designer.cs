namespace AKRS.ZX2200.DispenseSystem.Controls.Assistant
{
    partial class FrmDispenseOverAllSettings
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions3 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDispenseOverAllSettings));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject9 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject10 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject11 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject12 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.GpMaterial = new DevExpress.XtraEditors.GroupControl();
            this.CmbDispensingMeterialReference = new DevExpress.XtraEditors.ComboBoxEdit();
            this.GpDispenser = new DevExpress.XtraEditors.GroupControl();
            this.CmbDispenser = new DevExpress.XtraEditors.ComboBoxEdit();
            this.BtOk = new DevExpress.XtraEditors.SimpleButton();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GpMaterial)).BeginInit();
            this.GpMaterial.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDispensingMeterialReference.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpDispenser)).BeginInit();
            this.GpDispenser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDispenser.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GpMaterial
            // 
            this.GpMaterial.Controls.Add(this.CmbDispensingMeterialReference);
            this.GpMaterial.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpMaterial.Location = new System.Drawing.Point(107, 285);
            this.GpMaterial.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.GpMaterial.Name = "GpMaterial";
            this.GpMaterial.Size = new System.Drawing.Size(481, 151);
            this.GpMaterial.TabIndex = 5;
            this.GpMaterial.Text = "胶水材料";
            // 
            // CmbDispensingMeterialReference
            // 
            this.CmbDispensingMeterialReference.Location = new System.Drawing.Point(41, 72);
            this.CmbDispensingMeterialReference.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CmbDispensingMeterialReference.Name = "CmbDispensingMeterialReference";
            editorButtonImageOptions3.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions3.Image")));
            this.CmbDispensingMeterialReference.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions3, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject9, serializableAppearanceObject10, serializableAppearanceObject11, serializableAppearanceObject12, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbDispensingMeterialReference.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbDispensingMeterialReference.Size = new System.Drawing.Size(408, 44);
            this.CmbDispensingMeterialReference.TabIndex = 4;
            this.CmbDispensingMeterialReference.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbDispensingMeterialReference_ButtonClick);
            // 
            // GpDispenser
            // 
            this.GpDispenser.Controls.Add(this.CmbDispenser);
            this.GpDispenser.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpDispenser.Location = new System.Drawing.Point(107, 68);
            this.GpDispenser.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.GpDispenser.Name = "GpDispenser";
            this.GpDispenser.Size = new System.Drawing.Size(481, 158);
            this.GpDispenser.TabIndex = 1;
            this.GpDispenser.Text = "点胶头";
            // 
            // CmbDispenser
            // 
            this.CmbDispenser.Location = new System.Drawing.Point(41, 74);
            this.CmbDispenser.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.CmbDispenser.Name = "CmbDispenser";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbDispenser.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbDispenser.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbDispenser.Size = new System.Drawing.Size(408, 44);
            this.CmbDispenser.TabIndex = 4;
            this.CmbDispenser.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbDispenser_ButtonClick);
            // 
            // BtOk
            // 
            this.BtOk.Location = new System.Drawing.Point(158, 564);
            this.BtOk.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtOk.Name = "BtOk";
            this.BtOk.Size = new System.Drawing.Size(141, 68);
            this.BtOk.TabIndex = 1;
            this.BtOk.Text = "确定";
            this.BtOk.Click += new System.EventHandler(this.BtOK_Click);
            // 
            // BtCancel
            // 
            this.BtCancel.Location = new System.Drawing.Point(374, 564);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.BtCancel.Name = "BtCancel";
            this.BtCancel.Size = new System.Drawing.Size(141, 68);
            this.BtCancel.TabIndex = 2;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // FrmDispenseOverAllSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 704);
            this.Controls.Add(this.GpMaterial);
            this.Controls.Add(this.BtCancel);
            this.Controls.Add(this.GpDispenser);
            this.Controls.Add(this.BtOk);
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "FrmDispenseOverAllSettings";
            this.Text = "点胶头";
            ((System.ComponentModel.ISupportInitialize)(this.GpMaterial)).EndInit();
            this.GpMaterial.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbDispensingMeterialReference.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpDispenser)).EndInit();
            this.GpDispenser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbDispenser.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl GpDispenser;
        private DevExpress.XtraEditors.GroupControl GpMaterial;
        private DevExpress.XtraEditors.ComboBoxEdit CmbDispensingMeterialReference;
        private DevExpress.XtraEditors.ComboBoxEdit CmbDispenser;
        private DevExpress.XtraEditors.SimpleButton BtOk;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
    }
}
