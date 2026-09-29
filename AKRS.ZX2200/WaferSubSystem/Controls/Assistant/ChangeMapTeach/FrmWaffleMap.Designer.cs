namespace AKRS.ZX2200.WaferSubSystem.Controls.Assistant.ChangeMapTeach
{
    using System.Windows.Forms;

    partial class FrmWaffleMap
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
            this.GcWaffleMap = new DevExpress.XtraEditors.GroupControl();
            this.LueWafflesName = new DevExpress.XtraEditors.LookUpEdit();
            this.BtnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.GcWaffleMap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueWafflesName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GcWaffleMap
            // 
            this.GcWaffleMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GcWaffleMap.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GcWaffleMap.Location = new System.Drawing.Point(0, 0);
            this.GcWaffleMap.Name = "GcWaffleMap";
            this.GcWaffleMap.ShowCaption = false;
            this.GcWaffleMap.Size = new System.Drawing.Size(598, 484);
            this.GcWaffleMap.TabIndex = 0;
            // 
            // LueWafflesName
            // 
            this.LueWafflesName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LueWafflesName.Location = new System.Drawing.Point(332, 520);
            this.LueWafflesName.Name = "LueWafflesName";
            this.LueWafflesName.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.LueWafflesName.Properties.NullText = "";
            this.LueWafflesName.Size = new System.Drawing.Size(140, 20);
            this.LueWafflesName.TabIndex = 1;
            this.LueWafflesName.EditValueChanged += new System.EventHandler(this.LueWafflesName_EditValueChanged);
            // 
            // BtnSave
            // 
            this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSave.Location = new System.Drawing.Point(489, 519);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(75, 23);
            this.BtnSave.TabIndex = 2;
            this.BtnSave.Text = "保存";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // FrmWaffleMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 568);
            this.Controls.Add(this.BtnSave);
            this.Controls.Add(this.LueWafflesName);
            this.Controls.Add(this.GcWaffleMap);
            this.MinimumSize = new System.Drawing.Size(600, 600);
            this.Name = "FrmWaffleMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FrmWaffleMap";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmWaffleMap_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.GcWaffleMap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LueWafflesName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl GcWaffleMap;
        private DevExpress.XtraEditors.LookUpEdit LueWafflesName;
        private DevExpress.XtraEditors.SimpleButton BtnSave;
    }
}