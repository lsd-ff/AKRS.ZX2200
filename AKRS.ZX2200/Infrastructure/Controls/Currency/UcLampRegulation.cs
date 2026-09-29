namespace AKRS.ZX2200.Infrastructure.Controls.Currency
{
    using System;

    using AKRS.Galaxy2.Machine.Enums;
    using AKRS.Galaxy2.Machine.Models;

    /// <summary>
    /// 光源
    /// </summary>
    public partial class UcLampRegulation : DevExpress.XtraEditors.XtraUserControl
    {
        /// <summary>
        /// 根据模组名字初始化
        /// </summary>
        public UcLampRegulation()
        {
            this.InitializeComponent();
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            // Bond光源配置
            this.UcBondSpotLight.Init("系统2点光", "邦头三色点光-红", "邦头三色点光-绿", "邦头三色点光-蓝");
            this.UcBondAmbientLight.Init("系统2环光", "邦头三色环光-红", "邦头三色环光-绿", "邦头三色环光-蓝");

            // 点胶光源配置
            this.UcDispenseSpotLight.Init("系统1点光", "点胶三色点光-红", "点胶三色点光-绿", "点胶三色点光-蓝");
            this.UcDispenseAmbientLight.Init("系统1环光", "点胶三色环光-红", "点胶三色环光-绿", "点胶三色环光-蓝");

            // UpLook光源配置
            this.UcUplookSpotLight.Init("上视点光", "上视点光", "上视点光", "上视点光");
            this.UcUplookAmbientLight.Init("上视环光", "上视环光", "上视环光", "上视环光");

            // 晶圆光源配置
            this.UcWaferSpotLight.Init("晶圆点光", "晶圆三色点光-红", "晶圆三色点光-绿", "晶圆三色点光-蓝");
            this.UcWaferAmbientLight.Init("晶圆环光", "晶圆点光环光", "晶圆点光环光", "晶圆环光蓝光");
        }

        /// <summary>
        /// 关闭灯光
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">封装参数</param>
        private void BtCloseLight_Click(object sender, EventArgs e)
        {
            this.UcBondSpotLight.CloseLight();
            this.UcBondAmbientLight.CloseLight();
            this.UcDispenseSpotLight.CloseLight();
            this.UcDispenseAmbientLight.CloseLight();
            this.UcUplookSpotLight.CloseLight();
            this.UcUplookAmbientLight.CloseLight();
            this.UcWaferSpotLight.CloseLight();
            this.UcWaferAmbientLight.CloseLight();
        }
    }
}
