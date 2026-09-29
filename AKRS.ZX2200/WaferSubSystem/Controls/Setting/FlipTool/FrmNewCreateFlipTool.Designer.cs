namespace AKRS.ZX2200.WaferSubSystem.Controls.Setting.FlipTool
{
    partial class FrmNewCreateFlipTool
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewCreateFlipTool));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.CmbFlipTool = new DevExpress.XtraEditors.ComboBoxEdit();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.BtCancel = new DevExpress.XtraEditors.SimpleButton();
            this.BtnOK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbFlipTool.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.CmbFlipTool);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(429, 167);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "翻转工具名称";
            // 
            // CmbFlipTool
            // 
            this.CmbFlipTool.Location = new System.Drawing.Point(38, 48);
            this.CmbFlipTool.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.CmbFlipTool.Name = "CmbFlipTool";
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.CmbFlipTool.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.CmbFlipTool.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CmbFlipTool.Size = new System.Drawing.Size(343, 26);
            this.CmbFlipTool.TabIndex = 6;
            this.CmbFlipTool.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.CmbFlipTool_ButtonClick);
            // 
            // tablePanel1
            // 
            this.tablePanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Default;
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel1.Controls.Add(this.BtCancel);
            this.tablePanel1.Controls.Add(this.BtnOK);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tablePanel1.Location = new System.Drawing.Point(0, 105);
            this.tablePanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(429, 62);
            this.tablePanel1.TabIndex = 23;
            // 
            // BtCancel
            // 
            this.tablePanel1.SetColumn(this.BtCancel, 1);
            this.BtCancel.Location = new System.Drawing.Point(253, 12);
            this.BtCancel.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtCancel.Name = "BtCancel";
            this.tablePanel1.SetRow(this.BtCancel, 0);
            this.BtCancel.Size = new System.Drawing.Size(139, 37);
            this.BtCancel.TabIndex = 6;
            this.BtCancel.Text = "取消";
            this.BtCancel.Click += new System.EventHandler(this.BtCancel_Click);
            // 
            // BtnOK
            // 
            this.tablePanel1.SetColumn(this.BtnOK, 0);
            this.BtnOK.Location = new System.Drawing.Point(38, 12);
            this.BtnOK.Margin = new System.Windows.Forms.Padding(38, 4, 38, 4);
            this.BtnOK.Name = "BtnOK";
            this.tablePanel1.SetRow(this.BtnOK, 0);
            this.BtnOK.Size = new System.Drawing.Size(139, 37);
            this.BtnOK.TabIndex = 6;
            this.BtnOK.Text = "确定";
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // FrmNewCreateFlipTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 167);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.groupControl1);
            this.Name = "FrmNewCreateFlipTool";
            this.Text = "新建翻转工具";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CmbFlipTool.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.XtraEditors.SimpleButton BtCancel;
        private DevExpress.XtraEditors.SimpleButton BtnOK;
        private DevExpress.XtraEditors.ComboBoxEdit CmbFlipTool;
    }
}