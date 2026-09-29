using AKRS.ZX2200.Main.Machine.MachineSupport;

namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    partial class UcGuideMove
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
            this.BsAxis = new System.Windows.Forms.BindingSource(this.components);
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.SpExposure = new DevExpress.XtraEditors.SpinEdit();
            this.TbExposure = new DevExpress.XtraEditors.TrackBarControl();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.LbExposureTime = new DevExpress.XtraEditors.LabelControl();
            this.tablePanel3 = new DevExpress.Utils.Layout.TablePanel();
            this.tablePanel4 = new DevExpress.Utils.Layout.TablePanel();
            this.rGSpeedMode = new DevExpress.XtraEditors.RadioGroup();
            this.BtnJoystick = new DevExpress.XtraEditors.SimpleButton();
            this.BtVision = new DevExpress.XtraEditors.SimpleButton();
            this.TxX1 = new DevExpress.XtraEditors.TextEdit();
            this.TxY = new DevExpress.XtraEditors.TextEdit();
            this.TxZ = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.GpModule = new DevExpress.XtraEditors.GroupControl();
            this.CbChangeModule = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ucLight2 = new AKRS.ZX2200.Infrastructure.Controls.Currency.UcLight();
            this.ucLight1 = new AKRS.ZX2200.Infrastructure.Controls.Currency.UcLight();
            this.tablePanel2 = new DevExpress.Utils.Layout.TablePanel();
            this.tablePanel5 = new DevExpress.Utils.Layout.TablePanel();
            this.tablePanel7 = new DevExpress.Utils.Layout.TablePanel();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.SpGamma = new DevExpress.XtraEditors.SpinEdit();
            this.TbGama = new DevExpress.XtraEditors.TrackBarControl();
            this.tablePanel6 = new DevExpress.Utils.Layout.TablePanel();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.SpGain = new DevExpress.XtraEditors.SpinEdit();
            this.TbGain = new DevExpress.XtraEditors.TrackBarControl();
            ((System.ComponentModel.ISupportInitialize)(this.BsAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpExposure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbExposure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbExposure.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).BeginInit();
            this.tablePanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel4)).BeginInit();
            this.tablePanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rGSpeedMode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxX1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxY.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxZ.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpModule)).BeginInit();
            this.GpModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CbChangeModule.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).BeginInit();
            this.tablePanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel5)).BeginInit();
            this.tablePanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel7)).BeginInit();
            this.tablePanel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpGamma.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGama)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel6)).BeginInit();
            this.tablePanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpGain.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGain.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tag = "UcGuideMove";
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // SpExposure
            // 
            this.SpExposure.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpExposure.Enabled = false;
            this.SpExposure.Location = new System.Drawing.Point(601, 8);
            this.SpExposure.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SpExposure.Name = "SpExposure";
            this.SpExposure.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpExposure.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.SpExposure.Size = new System.Drawing.Size(115, 24);
            this.SpExposure.TabIndex = 11;
            // 
            // TbExposure
            // 
            this.TbExposure.EditValue = 1000;
            this.TbExposure.Location = new System.Drawing.Point(89, 4);
            this.TbExposure.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TbExposure.Name = "TbExposure";
            this.TbExposure.Properties.AutoSize = false;
            this.TbExposure.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TbExposure.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TbExposure.Properties.Maximum = 50000;
            this.TbExposure.Properties.Minimum = 1000;
            this.TbExposure.Properties.TickFrequency = 2;
            this.TbExposure.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.TbExposure.Size = new System.Drawing.Size(507, 33);
            this.TbExposure.TabIndex = 10;
            this.TbExposure.Value = 1000;
            this.TbExposure.EditValueChanged += new System.EventHandler(this.TbExposure_EditValueChanged);
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 14.22F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 85.78F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 20F)});
            this.tablePanel1.Controls.Add(this.LbExposureTime);
            this.tablePanel1.Controls.Add(this.SpExposure);
            this.tablePanel1.Controls.Add(this.TbExposure);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel1.Location = new System.Drawing.Point(11, 88);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(719, 41);
            this.tablePanel1.TabIndex = 12;
            // 
            // LbExposureTime
            // 
            this.tablePanel1.SetColumn(this.LbExposureTime, 0);
            this.LbExposureTime.Location = new System.Drawing.Point(3, 11);
            this.LbExposureTime.Name = "LbExposureTime";
            this.tablePanel1.SetRow(this.LbExposureTime, 0);
            this.LbExposureTime.Size = new System.Drawing.Size(65, 18);
            this.LbExposureTime.TabIndex = 12;
            this.LbExposureTime.Text = " 曝光时间";
            // 
            // tablePanel3
            // 
            this.tablePanel3.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 79.37F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 20.63F)});
            this.tablePanel3.Controls.Add(this.tablePanel4);
            this.tablePanel3.Controls.Add(this.panelControl2);
            this.tablePanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel3.Location = new System.Drawing.Point(11, 242);
            this.tablePanel3.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tablePanel3.Name = "tablePanel3";
            this.tablePanel3.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 103.5F)});
            this.tablePanel3.Size = new System.Drawing.Size(719, 460);
            this.tablePanel3.TabIndex = 14;
            // 
            // tablePanel4
            // 
            this.tablePanel3.SetColumn(this.tablePanel4, 1);
            this.tablePanel4.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 5F)});
            this.tablePanel4.Controls.Add(this.rGSpeedMode);
            this.tablePanel4.Controls.Add(this.BtnJoystick);
            this.tablePanel4.Controls.Add(this.BtVision);
            this.tablePanel4.Controls.Add(this.TxX1);
            this.tablePanel4.Controls.Add(this.TxY);
            this.tablePanel4.Controls.Add(this.TxZ);
            this.tablePanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel4.Location = new System.Drawing.Point(573, 1);
            this.tablePanel4.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.tablePanel4.Name = "tablePanel4";
            this.tablePanel3.SetRow(this.tablePanel4, 0);
            this.tablePanel4.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 19.16F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 32.84F)});
            this.tablePanel4.Size = new System.Drawing.Size(144, 458);
            this.tablePanel4.TabIndex = 6;
            // 
            // rGSpeedMode
            // 
            this.tablePanel4.SetColumn(this.rGSpeedMode, 0);
            this.rGSpeedMode.Location = new System.Drawing.Point(3, 317);
            this.rGSpeedMode.Name = "rGSpeedMode";
            this.tablePanel4.SetRow(this.rGSpeedMode, 4);
            this.rGSpeedMode.Size = new System.Drawing.Size(138, 30);
            this.rGSpeedMode.TabIndex = 5;
            this.rGSpeedMode.SelectedIndexChanged += new System.EventHandler(this.rGSpeedMode_SelectedIndexChanged);
            // 
            // BtnJoystick
            // 
            this.tablePanel4.SetColumn(this.BtnJoystick, 0);
            this.BtnJoystick.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnJoystick.Location = new System.Drawing.Point(3, 363);
            this.BtnJoystick.Margin = new System.Windows.Forms.Padding(3, 3, 3, 18);
            this.BtnJoystick.Name = "BtnJoystick";
            this.tablePanel4.SetRow(this.BtnJoystick, 5);
            this.BtnJoystick.Size = new System.Drawing.Size(138, 77);
            this.BtnJoystick.TabIndex = 4;
            this.BtnJoystick.Text = "摇杆";
            this.BtnJoystick.Click += new System.EventHandler(this.BtnJoystick_Click);
            // 
            // BtVision
            // 
            this.tablePanel4.SetColumn(this.BtVision, 0);
            this.BtVision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtVision.Location = new System.Drawing.Point(2, 229);
            this.BtVision.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.BtVision.Name = "BtVision";
            this.tablePanel4.SetRow(this.BtVision, 3);
            this.BtVision.Size = new System.Drawing.Size(140, 74);
            this.BtVision.TabIndex = 3;
            this.BtVision.Text = "相机实时界面";
            this.BtVision.Click += new System.EventHandler(this.BtVision_Click_1);
            // 
            // TxX1
            // 
            this.tablePanel4.SetColumn(this.TxX1, 0);
            this.TxX1.Location = new System.Drawing.Point(2, 26);
            this.TxX1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.TxX1.Name = "TxX1";
            this.tablePanel4.SetRow(this.TxX1, 0);
            this.TxX1.Size = new System.Drawing.Size(140, 24);
            this.TxX1.TabIndex = 0;
            // 
            // TxY
            // 
            this.tablePanel4.SetColumn(this.TxY, 0);
            this.TxY.Location = new System.Drawing.Point(2, 102);
            this.TxY.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.TxY.Name = "TxY";
            this.tablePanel4.SetRow(this.TxY, 1);
            this.TxY.Size = new System.Drawing.Size(140, 24);
            this.TxY.TabIndex = 1;
            // 
            // TxZ
            // 
            this.tablePanel4.SetColumn(this.TxZ, 0);
            this.TxZ.Location = new System.Drawing.Point(2, 178);
            this.TxZ.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.TxZ.Name = "TxZ";
            this.tablePanel4.SetRow(this.TxZ, 2);
            this.TxZ.Size = new System.Drawing.Size(140, 24);
            this.TxZ.TabIndex = 2;
            // 
            // panelControl2
            // 
            this.tablePanel3.SetColumn(this.panelControl2, 0);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(2, 1);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.panelControl2.Name = "panelControl2";
            this.tablePanel3.SetRow(this.panelControl2, 0);
            this.panelControl2.Size = new System.Drawing.Size(567, 458);
            this.panelControl2.TabIndex = 0;
            // 
            // GpModule
            // 
            this.GpModule.Controls.Add(this.CbChangeModule);
            this.GpModule.Dock = System.Windows.Forms.DockStyle.Top;
            this.GpModule.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.GpModule.Location = new System.Drawing.Point(11, 170);
            this.GpModule.Margin = new System.Windows.Forms.Padding(2, 3, 2, 10);
            this.GpModule.Name = "GpModule";
            this.GpModule.Size = new System.Drawing.Size(719, 72);
            this.GpModule.TabIndex = 13;
            this.GpModule.Text = "Module";
            // 
            // CbChangeModule
            // 
            this.CbChangeModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CbChangeModule.Location = new System.Drawing.Point(2, 28);
            this.CbChangeModule.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CbChangeModule.Name = "CbChangeModule";
            this.CbChangeModule.Properties.AllowMouseWheel = false;
            this.CbChangeModule.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CbChangeModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.CbChangeModule.Size = new System.Drawing.Size(715, 24);
            this.CbChangeModule.TabIndex = 0;
            this.CbChangeModule.SelectedIndexChanged += new System.EventHandler(this.CbChangeModule_SelectedIndexChanged);
            // 
            // ucLight2
            // 
            this.tablePanel2.SetColumn(this.ucLight2, 1);
            this.ucLight2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucLight2.Location = new System.Drawing.Point(362, 3);
            this.ucLight2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ucLight2.Name = "ucLight2";
            this.ucLight2.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tablePanel2.SetRow(this.ucLight2, 0);
            this.ucLight2.Size = new System.Drawing.Size(356, 69);
            this.ucLight2.TabIndex = 3;
            // 
            // ucLight1
            // 
            this.tablePanel2.SetColumn(this.ucLight1, 0);
            this.ucLight1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucLight1.Location = new System.Drawing.Point(2, 3);
            this.ucLight1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ucLight1.Name = "ucLight1";
            this.ucLight1.Padding = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.tablePanel2.SetRow(this.ucLight1, 0);
            this.ucLight1.Size = new System.Drawing.Size(356, 69);
            this.ucLight1.TabIndex = 2;
            // 
            // tablePanel2
            // 
            this.tablePanel2.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F)});
            this.tablePanel2.Controls.Add(this.ucLight1);
            this.tablePanel2.Controls.Add(this.ucLight2);
            this.tablePanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel2.Location = new System.Drawing.Point(11, 13);
            this.tablePanel2.Name = "tablePanel2";
            this.tablePanel2.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel2.Size = new System.Drawing.Size(719, 75);
            this.tablePanel2.TabIndex = 13;
            // 
            // tablePanel5
            // 
            this.tablePanel5.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50.16F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 49.84F)});
            this.tablePanel5.Controls.Add(this.tablePanel7);
            this.tablePanel5.Controls.Add(this.tablePanel6);
            this.tablePanel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.tablePanel5.Location = new System.Drawing.Point(11, 129);
            this.tablePanel5.Name = "tablePanel5";
            this.tablePanel5.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel5.Size = new System.Drawing.Size(719, 41);
            this.tablePanel5.TabIndex = 15;
            // 
            // tablePanel7
            // 
            this.tablePanel5.SetColumn(this.tablePanel7, 1);
            this.tablePanel7.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 60F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 20F)});
            this.tablePanel7.Controls.Add(this.labelControl2);
            this.tablePanel7.Controls.Add(this.SpGamma);
            this.tablePanel7.Controls.Add(this.TbGama);
            this.tablePanel7.Location = new System.Drawing.Point(364, 3);
            this.tablePanel7.Name = "tablePanel7";
            this.tablePanel5.SetRow(this.tablePanel7, 0);
            this.tablePanel7.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel7.Size = new System.Drawing.Size(352, 35);
            this.tablePanel7.TabIndex = 14;
            // 
            // labelControl2
            // 
            this.tablePanel7.SetColumn(this.labelControl2, 0);
            this.labelControl2.Location = new System.Drawing.Point(3, 8);
            this.labelControl2.Name = "labelControl2";
            this.tablePanel7.SetRow(this.labelControl2, 0);
            this.labelControl2.Size = new System.Drawing.Size(30, 18);
            this.labelControl2.TabIndex = 12;
            this.labelControl2.Text = "伽马";
            // 
            // SpGamma
            // 
            this.tablePanel7.SetColumn(this.SpGamma, 2);
            this.SpGamma.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpGamma.Enabled = false;
            this.SpGamma.Location = new System.Drawing.Point(276, 5);
            this.SpGamma.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SpGamma.Name = "SpGamma";
            this.SpGamma.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpGamma.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.tablePanel7.SetRow(this.SpGamma, 0);
            this.SpGamma.Size = new System.Drawing.Size(74, 24);
            this.SpGamma.TabIndex = 11;
            // 
            // TbGama
            // 
            this.tablePanel7.SetColumn(this.TbGama, 1);
            this.TbGama.EditValue = 10;
            this.TbGama.Location = new System.Drawing.Point(42, 4);
            this.TbGama.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TbGama.Name = "TbGama";
            this.TbGama.Properties.AutoSize = false;
            this.TbGama.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TbGama.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TbGama.Properties.Maximum = 50000;
            this.TbGama.Properties.Minimum = 10;
            this.TbGama.Properties.TickFrequency = 2;
            this.TbGama.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tablePanel7.SetRow(this.TbGama, 0);
            this.TbGama.Size = new System.Drawing.Size(229, 27);
            this.TbGama.TabIndex = 10;
            this.TbGama.Value = 10;
            this.TbGama.EditValueChanged += new System.EventHandler(this.TbGama_EditValueChanged);
            // 
            // tablePanel6
            // 
            this.tablePanel5.SetColumn(this.tablePanel6, 0);
            this.tablePanel6.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 60F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 20F)});
            this.tablePanel6.Controls.Add(this.labelControl1);
            this.tablePanel6.Controls.Add(this.SpGain);
            this.tablePanel6.Controls.Add(this.TbGain);
            this.tablePanel6.Location = new System.Drawing.Point(3, 3);
            this.tablePanel6.Name = "tablePanel6";
            this.tablePanel5.SetRow(this.tablePanel6, 0);
            this.tablePanel6.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel6.Size = new System.Drawing.Size(355, 35);
            this.tablePanel6.TabIndex = 13;
            // 
            // labelControl1
            // 
            this.tablePanel6.SetColumn(this.labelControl1, 0);
            this.labelControl1.Location = new System.Drawing.Point(3, 8);
            this.labelControl1.Name = "labelControl1";
            this.tablePanel6.SetRow(this.labelControl1, 0);
            this.labelControl1.Size = new System.Drawing.Size(30, 18);
            this.labelControl1.TabIndex = 12;
            this.labelControl1.Text = "增益";
            // 
            // SpGain
            // 
            this.tablePanel6.SetColumn(this.SpGain, 2);
            this.SpGain.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.SpGain.Enabled = false;
            this.SpGain.Location = new System.Drawing.Point(278, 5);
            this.SpGain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.SpGain.Name = "SpGain";
            this.SpGain.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.SpGain.Properties.MaxValue = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.tablePanel6.SetRow(this.SpGain, 0);
            this.SpGain.Size = new System.Drawing.Size(75, 24);
            this.SpGain.TabIndex = 11;
            // 
            // TbGain
            // 
            this.tablePanel6.SetColumn(this.TbGain, 1);
            this.TbGain.EditValue = 1;
            this.TbGain.Location = new System.Drawing.Point(42, 4);
            this.TbGain.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TbGain.Name = "TbGain";
            this.TbGain.Properties.AutoSize = false;
            this.TbGain.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.TbGain.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TbGain.Properties.Maximum = 50000;
            this.TbGain.Properties.Minimum = 1;
            this.TbGain.Properties.TickFrequency = 2;
            this.TbGain.Properties.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tablePanel6.SetRow(this.TbGain, 0);
            this.TbGain.Size = new System.Drawing.Size(231, 27);
            this.TbGain.TabIndex = 10;
            this.TbGain.Value = 1;
            this.TbGain.EditValueChanged += new System.EventHandler(this.TbGain_EditValueChanged);
            // 
            // UcGuideMove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tablePanel3);
            this.Controls.Add(this.GpModule);
            this.Controls.Add(this.tablePanel5);
            this.Controls.Add(this.tablePanel1);
            this.Controls.Add(this.tablePanel2);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "UcGuideMove";
            this.Padding = new System.Windows.Forms.Padding(11, 13, 11, 13);
            this.Size = new System.Drawing.Size(741, 715);
            this.Load += new System.EventHandler(this.UcGuideMove_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BsAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpExposure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbExposure.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbExposure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            this.tablePanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel3)).EndInit();
            this.tablePanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel4)).EndInit();
            this.tablePanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rGSpeedMode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxX1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxY.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxZ.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GpModule)).EndInit();
            this.GpModule.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CbChangeModule.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel2)).EndInit();
            this.tablePanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel5)).EndInit();
            this.tablePanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel7)).EndInit();
            this.tablePanel7.ResumeLayout(false);
            this.tablePanel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpGamma.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGama)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel6)).EndInit();
            this.tablePanel6.ResumeLayout(false);
            this.tablePanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SpGain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGain.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TbGain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource BsAxis;
        private System.Windows.Forms.Timer Timer;
        private UcLight ucLight1;
        private UcLight ucLight2;
        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.Utils.Layout.TablePanel tablePanel2;
        private DevExpress.XtraEditors.SpinEdit SpExposure;
        private DevExpress.XtraEditors.TrackBarControl TbExposure;
        private DevExpress.XtraEditors.LabelControl LbExposureTime;
        private DevExpress.Utils.Layout.TablePanel tablePanel3;
        private DevExpress.Utils.Layout.TablePanel tablePanel4;
        private DevExpress.XtraEditors.SimpleButton BtVision;
        private DevExpress.XtraEditors.TextEdit TxX1;
        private DevExpress.XtraEditors.TextEdit TxY;
        private DevExpress.XtraEditors.TextEdit TxZ;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl GpModule;
        private DevExpress.XtraEditors.ComboBoxEdit CbChangeModule;
        private DevExpress.XtraEditors.SimpleButton BtnJoystick;
        private DevExpress.Utils.Layout.TablePanel tablePanel5;
        private DevExpress.Utils.Layout.TablePanel tablePanel7;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SpinEdit SpGamma;
        private DevExpress.XtraEditors.TrackBarControl TbGama;
        private DevExpress.Utils.Layout.TablePanel tablePanel6;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SpinEdit SpGain;
        private DevExpress.XtraEditors.TrackBarControl TbGain;
        private DevExpress.XtraEditors.RadioGroup rGSpeedMode;
    }
}
