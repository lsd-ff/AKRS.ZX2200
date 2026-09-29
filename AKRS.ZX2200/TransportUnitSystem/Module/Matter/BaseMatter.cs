using AKRS.Galaxy2.CoordinateSystems.CoordinateSystems;
using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.TransportUnitSystem.Model;
using AKRS.ZX2200.TransportUnitSystem.Module.Config;
using AKRS.ZX2200.TransportUnitSystem.Module.Information;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Matter
{
    using AKRS.ZX2200.Infrastructure.AOP.Module;

    /// <summary>
    /// 基础实体类
    /// </summary>
    public abstract class BaseMatter
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 配置文件
        /// </summary>
        [JsonIgnore]
        public BaseConfig Config => this.GetConfigByType(this.MatterTypeEnum);

        /// <summary>
        /// 异性配置
        /// </summary>
        public OppositeSex OppositeSex { get; set; }

        /// <summary>
        /// 信息
        /// </summary>
        public BaseInformation BaseInfo { get; set; }

        /// <summary>
        /// 实体状态
        /// </summary>
        public EntityState EntityState { get; set; } = EntityState.Process;

        /// <summary>
        /// 产品的工艺
        /// </summary>
        public virtual MatterProductState MatterProductState { get; set; }

        /// <summary>
        /// 实物的状态
        /// </summary>
        public EntityTypeEnum MatterTypeEnum { get; set; }

        /// <summary>
        /// 坐标系
        /// </summary>
        [JsonIgnore]
        public virtual GeneralCoordinateSystem CoordinateSystem { get; set; } = new GeneralCoordinateSystem();

        /// <summary>
        /// 图像显示时的矩形
        /// </summary>
        public Rectangle Rectangle { get; set; }

        /// <summary>
        /// 编辑状态
        /// </summary>
        public EditStateEnum EditState { get; set; } = EditStateEnum.NewCreate;

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 工作索引
        /// </summary>
        public int WorkIndex { get; set; }

        /// <summary>
        /// 设置实体的状态为disable
        /// </summary>
        public abstract void SetMatterDisable();

        /// <summary>
        /// 设置实体的状态为disable
        /// </summary>
        public abstract void SetMatterOnlyDispense();

        /// <summary>
        /// 设置实体的状态为disable
        /// </summary>
        public abstract void SetMatterOnlyBond();

        /// <summary>
        /// 设置实体的状态为disable
        /// </summary>
        public abstract void SetMatterEnable();

        /// <summary>
        /// 获取配置对象
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>配置对象</returns>
        public BaseConfig GetConfigByType(EntityTypeEnum type)
        {
            if (ProductConfiguration.GetInstance().TransportUnitConfig.IsOppositeSex)
            {
                return this.OppositeSex.Config;
            }
            else
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
                        .Find(it => it.Name == this.Name);
                }

                return null;
            }
        }
    }
}
