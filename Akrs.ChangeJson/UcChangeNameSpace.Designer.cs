namespace Akrs.ChangeJson
{
    partial class UcChangeNameSpace
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
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.xtraFolderBrowserDialog1 = new DevExpress.XtraEditors.XtraFolderBrowserDialog(this.components);
            this.TeDllDir = new DevExpress.XtraEditors.TextEdit();
            this.BtReflectObject = new DevExpress.XtraEditors.SimpleButton();
            this.GcClassNameSpace = new DevExpress.XtraGrid.GridControl();
            this.GvClassNameSpace = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.GcDll = new DevExpress.XtraGrid.GridControl();
            this.GvDll = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtOpenDllDir = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.TeJsonDir = new DevExpress.XtraEditors.TextEdit();
            this.GcJson = new DevExpress.XtraGrid.GridControl();
            this.GvJson = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtReadJson = new DevExpress.XtraEditors.SimpleButton();
            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
            this.BtUpdateJson = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TeDllDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcClassNameSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvClassNameSpace)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcDll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvDll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeJsonDir.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcJson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvJson)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(95, 31);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(52, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Dll 路径：";
            // 
            // xtraFolderBrowserDialog1
            // 
            this.xtraFolderBrowserDialog1.SelectedPath = "xtraFolderBrowserDialog1";
            // 
            // TeDllDir
            // 
            this.TeDllDir.Location = new System.Drawing.Point(165, 28);
            this.TeDllDir.Name = "TeDllDir";
            this.TeDllDir.Size = new System.Drawing.Size(411, 20);
            this.TeDllDir.TabIndex = 1;
            // 
            // BtReflectObject
            // 
            this.BtReflectObject.Location = new System.Drawing.Point(706, 22);
            this.BtReflectObject.Name = "BtReflectObject";
            this.BtReflectObject.Size = new System.Drawing.Size(81, 32);
            this.BtReflectObject.TabIndex = 2;
            this.BtReflectObject.Text = "反射对象";
            this.BtReflectObject.Click += new System.EventHandler(this.BtReflectObject_Click);
            // 
            // GcClassNameSpace
            // 
            this.GcClassNameSpace.Location = new System.Drawing.Point(577, 120);
            this.GcClassNameSpace.MainView = this.GvClassNameSpace;
            this.GcClassNameSpace.Name = "GcClassNameSpace";
            this.GcClassNameSpace.Size = new System.Drawing.Size(598, 524);
            this.GcClassNameSpace.TabIndex = 3;
            this.GcClassNameSpace.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvClassNameSpace});
            // 
            // GvClassNameSpace
            // 
            this.GvClassNameSpace.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.GvClassNameSpace.GridControl = this.GcClassNameSpace;
            this.GvClassNameSpace.Name = "GvClassNameSpace";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "类名";
            this.gridColumn1.FieldName = "ClassName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 139;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "命名空间";
            this.gridColumn2.FieldName = "NameSpace";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 472;
            // 
            // GcDll
            // 
            this.GcDll.Location = new System.Drawing.Point(36, 120);
            this.GcDll.MainView = this.GvDll;
            this.GcDll.Name = "GcDll";
            this.GcDll.Size = new System.Drawing.Size(540, 524);
            this.GcDll.TabIndex = 4;
            this.GcDll.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvDll});
            // 
            // GvDll
            // 
            this.GvDll.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3});
            this.GvDll.GridControl = this.GcDll;
            this.GvDll.Name = "GvDll";
            this.GvDll.OptionsSelection.MultiSelect = true;
            this.GvDll.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Dll名称";
            this.gridColumn3.FieldName = "Name";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 1;
            // 
            // BtOpenDllDir
            // 
            this.BtOpenDllDir.Location = new System.Drawing.Point(619, 22);
            this.BtOpenDllDir.Name = "BtOpenDllDir";
            this.BtOpenDllDir.Size = new System.Drawing.Size(81, 32);
            this.BtOpenDllDir.TabIndex = 5;
            this.BtOpenDllDir.Text = "打开Dll 路径";
            this.BtOpenDllDir.Click += new System.EventHandler(this.BtOpenDllDir_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(78, 69);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(69, 14);
            this.labelControl2.TabIndex = 6;
            this.labelControl2.Text = "JSON 路径：";
            // 
            // TeJsonDir
            // 
            this.TeJsonDir.Location = new System.Drawing.Point(165, 66);
            this.TeJsonDir.Name = "TeJsonDir";
            this.TeJsonDir.Size = new System.Drawing.Size(411, 20);
            this.TeJsonDir.TabIndex = 7;
            // 
            // GcJson
            // 
            this.GcJson.Location = new System.Drawing.Point(36, 650);
            this.GcJson.MainView = this.GvJson;
            this.GcJson.Name = "GcJson";
            this.GcJson.Size = new System.Drawing.Size(1139, 709);
            this.GcJson.TabIndex = 8;
            this.GcJson.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GvJson});
            // 
            // GvJson
            // 
            this.GvJson.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4});
            this.GvJson.GridControl = this.GcJson;
            this.GvJson.Name = "GvJson";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Json文件名称";
            this.gridColumn4.FieldName = "Name";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 142;
            // 
            // BtReadJson
            // 
            this.BtReadJson.Location = new System.Drawing.Point(619, 60);
            this.BtReadJson.Name = "BtReadJson";
            this.BtReadJson.Size = new System.Drawing.Size(168, 32);
            this.BtReadJson.TabIndex = 9;
            this.BtReadJson.Text = "读取Json文件";
            this.BtReadJson.Click += new System.EventHandler(this.BtReadJson_Click);
            // 
            // memoEdit1
            // 
            this.memoEdit1.Location = new System.Drawing.Point(1181, 119);
            this.memoEdit1.Name = "memoEdit1";
            this.memoEdit1.Size = new System.Drawing.Size(440, 800);
            this.memoEdit1.TabIndex = 10;
            // 
            // BtUpdateJson
            // 
            this.BtUpdateJson.Location = new System.Drawing.Point(1037, 22);
            this.BtUpdateJson.Name = "BtUpdateJson";
            this.BtUpdateJson.Size = new System.Drawing.Size(208, 57);
            this.BtUpdateJson.TabIndex = 11;
            this.BtUpdateJson.Text = "更新Json";
            this.BtUpdateJson.Click += new System.EventHandler(this.BtUpdateJson_Click);
            // 
            // UcChangeNameSpace
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtUpdateJson);
            this.Controls.Add(this.memoEdit1);
            this.Controls.Add(this.BtReadJson);
            this.Controls.Add(this.GcJson);
            this.Controls.Add(this.TeJsonDir);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.BtOpenDllDir);
            this.Controls.Add(this.GcDll);
            this.Controls.Add(this.GcClassNameSpace);
            this.Controls.Add(this.BtReflectObject);
            this.Controls.Add(this.TeDllDir);
            this.Controls.Add(this.labelControl1);
            this.Name = "UcChangeNameSpace";
            this.Size = new System.Drawing.Size(1637, 922);
            ((System.ComponentModel.ISupportInitialize)(this.TeDllDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcClassNameSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvClassNameSpace)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcDll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvDll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TeJsonDir.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GcJson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GvJson)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.XtraFolderBrowserDialog xtraFolderBrowserDialog1;
        private DevExpress.XtraEditors.TextEdit TeDllDir;
        private DevExpress.XtraEditors.SimpleButton BtReflectObject;
        private DevExpress.XtraGrid.GridControl GcClassNameSpace;
        private DevExpress.XtraGrid.Views.Grid.GridView GvClassNameSpace;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.GridControl GcDll;
        private DevExpress.XtraGrid.Views.Grid.GridView GvDll;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.SimpleButton BtOpenDllDir;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit TeJsonDir;
        private DevExpress.XtraGrid.GridControl GcJson;
        private DevExpress.XtraGrid.Views.Grid.GridView GvJson;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.SimpleButton BtReadJson;
        private DevExpress.XtraEditors.MemoEdit memoEdit1;
        private DevExpress.XtraEditors.SimpleButton BtUpdateJson;
    }
}
