using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.Galaxy2.MachineSupport.Config;
using AKRS.ZX2200.Infrastructure.Controls.Currency;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Matter;
using DevExpress.XtraEditors;
using LanguageExt.ClassInstances;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem
{
    /// <summary>
    /// 产品配置对象
    /// </summary>
    public class ProductConfiguration : Singleton<ProductConfiguration>
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static ProductConfiguration()
        {
            ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeDirPath, "ProductConfiguration.json");
        }

        /// <summary>
        /// 传输单元的配置对象
        /// </summary>
        public TransportUnitConfig TransportUnitConfig { get; set; } =
            new TransportUnitConfig() { EntityType = EntityTypeEnum.TransportUnit };

        /// <summary>
        /// 基板的配置对象
        /// </summary>
        public SubstrateConfig SubstrateConfig { get; set; } = new SubstrateConfig() { EntityType = EntityTypeEnum.Substrate };

        /// <summary>
        /// 基岛的配置对象
        /// </summary>
        public ModuleConfig ModuleConfig { get; set; } = new ModuleConfig() { EntityType = EntityTypeEnum.Module };

        /// <summary>
        /// 焊点的配置对象
        /// </summary>
        public BondPositionConfig BondPositionConfig { get; set; } = new BondPositionConfig();

        /// <summary>
        /// 其他的配置对象
        /// </summary>
        public OtherConfig OtherConfig { get; set; } = new OtherConfig();

        /// <summary>
        /// 编辑状态
        /// </summary>
        public EditStateEnum EditState { get; set; } = EditStateEnum.NewCreate;

        /// <summary>
        /// 异性基板配置
        /// </summary>
        public OppositeSexConfiguration OppositeSexConfiguration { get; set; } = new OppositeSexConfiguration();

        /// <summary>
        /// 初始化ProductConfiguration
        /// </summary>
        public void InitProductConfiguration()
        {
            ProductConfiguration.FilePath = Path.Combine(PathConfig.RecipeDirPath, "ProductConfiguration.json");
            instance = null;
        }

        /// <summary>
        /// 清除实力对象
        /// </summary>
        public void ClearProductConfigurationInstance()
        {
            instance = null;
        }

        /// <summary>
        /// TU有没有准备好
        /// </summary>
        /// <returns>结果</returns>
        public bool IsTransportUnitReady()
        {
            // 判断TU有没有示教完成
            if (!this.TransportUnitConfig.IsAssistantSucceed)
            {
                AKRSXtraMessageBox.Show("框架未示教完成，请示教完成后启动");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>配置对象</returns>
        public BaseConfig GetConfigByType(EntityTypeEnum type,string name = null)
        {
            if (type == EntityTypeEnum.TransportUnit)
            {
                return ProductConfiguration.GetInstance().TransportUnitConfig;
            }
            else if (type == EntityTypeEnum.Substrate)
            {
                return ProductConfiguration.GetInstance().SubstrateConfig;
            }
            else if (type == EntityTypeEnum.Module)
            {
                return ProductConfiguration.GetInstance().ModuleConfig;
            }
            else if (type == EntityTypeEnum.BondPosition)
            {
                // 这个地方有问题
                return ProductConfiguration.GetInstance().BondPositionConfig.SingleBpPositionConfigList
                    .Find(it => it.Name == name);
            }

            return null;
        }
    }
}
