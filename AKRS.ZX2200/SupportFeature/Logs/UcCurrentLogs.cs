namespace AKRS.ZX2200.SupportFeature.Logs
{
    using AKRS.Galaxy2.Log.Control;

    public partial class UcCurrentLogs : DevExpress.XtraEditors.XtraUserControl
    {
        public UcCurrentLogs()
        {
            this.InitializeComponent();
            AKRS.Galaxy2.Log.Control.UcLogControl ucLogControl1;
            ucLogControl1 = new UcLogControl();
            ucLogControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Controls.Add(ucLogControl1);
        }
    }
}
