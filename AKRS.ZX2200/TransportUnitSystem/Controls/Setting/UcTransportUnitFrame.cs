using AKRS.ZX2200.Infrastructure.Action;
using AKRS.ZX2200.TransportUnitSystem;
using AKRS.ZX2200.TransportUnitSystem.Controls.Setting;
using System.Windows.Forms;

namespace AKRS.ZX2200.Controls.ToolControls.Programming
{
    using AKRS.ZX2200.Infrastructure.Interface;

    /// <summary>
    /// Transport unit UI 框架
    /// </summary>
    public partial class UcTransportUnitFrame : DevExpress.XtraEditors.XtraUserControl, IProgrammingControl
    {
        /// <summary>
        /// TransportUnit 配置
        /// </summary>
        private UcTransportUnit ucTransportUnit;

        /// <summary>
        /// Substrate 配置
        /// </summary>
        private UcSubstrate ucSubstrate;

        /// <summary>
        /// Module 配置
        /// </summary>
        private UcModule ucModule;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UcTransportUnitFrame()
        {
            this.InitializeComponent();

            this.ucTransportUnit = new UcTransportUnit { Dock = DockStyle.Fill };
            this.ucSubstrate = new UcSubstrate { Dock = DockStyle.Fill };
            this.ucModule = new UcModule { Dock = DockStyle.Fill };

            this.TpTransportUnit.Controls.Add(this.ucTransportUnit);
            this.TpSubstrate.Controls.Add(this.ucSubstrate);
            this.TpModule.Controls.Add(this.ucModule);
        }

        /// <summary>
        /// 确定
        /// </summary>
        public void Confirm()
        {
            this.ucTransportUnit.Save();
            this.ucSubstrate.Save();
            this.ucModule.Save();

            ProductConfiguration.GetInstance().Save();
        }

        /// <summary>
        /// 判断substrate是否可用
        /// </summary>
        /// <returns>结果</returns>
        public bool GetSubstrateProcessingEnable()
        {
            return this.ucSubstrate.IsMultipleSubstrates();
        }
    }
}
