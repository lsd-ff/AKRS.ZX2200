namespace AKRS.ZX2200.Main.Controls.Ucmain
{
    using System;
    using System.Windows.Forms;

    using AKRS.Galaxy2.UserManager;
    using AKRS.ZX2200.Infrastructure.Models.Enums;

    using DevExpress.XtraEditors;

    using BaseControl = AKRS.ZX2200.Infrastructure.Controls.Currency.BaseControl;

    public partial class UcUserManager : BaseControl
    {
        private Galaxy2.UserManager.CustomControls.UcUserManager ucUserManager;

        public UcUserManager()
        {
            this.InitializeComponent();
        }

        private void UcUserManager_Load(object sender, EventArgs e)
        {
            this.ucUserManager = new Galaxy2.UserManager.CustomControls.UcUserManager() { Dock = DockStyle.Fill };
            this.Controls.Add(this.ucUserManager);
        }
    }
}