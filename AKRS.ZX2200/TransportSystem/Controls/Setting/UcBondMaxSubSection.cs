namespace AKRS.ZX2200.TransportSystem.Controls.Setting
{
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Infrastructure.Interface;
    using AKRS.ZX2200.TransportSystem.Controllers;
    using AKRS.ZX2200.TransportSystem.Models;
    using AKRS.ZX2200.TransportSystem.Models.Programs;

    /// <summary>
    /// 载具编程
    /// </summary>
    public partial class UcBondMaxSubSection : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// 固晶载具
        /// </summary>
        private BondSubSectionController BondSubSectionController => TransportDomain.GetInstance().TransportController.BondSubSectionController;

        /// <summary>
        ///  载具编程
        /// </summary>
        public UcBondMaxSubSection()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            this.LueBondMaxSubSize.Properties.DataSource = EnumHelper.ConvertEnumToNameDisplayDto<BondMaxSubSectionSizeEnum>();

            this.LueBondMaxSubSize.EditValue = this.BondSubSectionController.bondSubSectionProgram.Size;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Confirm()
        {
            this.BondSubSectionController.bondSubSectionProgram.Size =
                (BondMaxSubSectionSizeEnum)this.LueBondMaxSubSize.EditValue;

            TransportProgram.GetInstance().Save();
        }
    }
}
