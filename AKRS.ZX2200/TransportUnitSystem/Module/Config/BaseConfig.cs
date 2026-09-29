using AKRS.Galaxy2.Infrastructure.CommonModel;
using AKRS.ZX2200.Models;
using AKRS.ZX2200.TransportUnitSystem.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKRS.ZX2200.Infrastructure.AOP.Module;

namespace AKRS.ZX2200.TransportUnitSystem.Module.Config
{
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using PropertyChanged;

    /// <summary>
    /// 基础配置文件
    /// </summary>
    [Serializable]
    [AddINotifyPropertyChangedInterface]
    public class BaseConfig : PropertyChangeAop
    {
        /// <summary>
        /// 定位配置
        /// </summary>
        public LocateConfig LocateConfig { get; set; } = new LocateConfig();

        /// <summary>
        /// 身份识别配置
        /// </summary>
        public IdentityConfig IdentityConfig { get; set; } = new IdentityConfig();

        /// <summary>
        /// 是否系统1测高
        /// </summary>
        [TreeProgramListArgs("点胶测高", (string)null)]
        public bool MeasureHeightInSystem1 { get; set; }

        /// <summary>
        /// 是否系统2测高
        /// </summary>
        [TreeProgramListArgs("固晶测高", (string)null)]
        public bool MeasureHeightInSystem2 { get; set; }

        /// <summary>
        /// 测高类型
        /// </summary>
        public MeasureHeightTypeEnum MeasureHeightType { get; set; } = MeasureHeightTypeEnum.Origin;

        /// <summary>
        /// 测高的点位集合
        /// 添加一个初始值，在没有示教的情况下就是原点测高
        /// </summary>
        public List<AKRSPoint3D> HeightMeasurementPoints { get; set; } = new List<AKRSPoint3D>();

        /// <summary>
        /// 类型
        /// </summary>
        public EntityTypeEnum EntityType { get; set; }

        /// <summary>
        /// 刷新测高点
        /// </summary>
        public void RefreshMeasureHeightPoint()
        {
            this.HeightMeasurementPoints = new List<AKRSPoint3D>() { new AKRSPoint3D() };
        }
    }

    /// <summary>
    /// 测高类型
    /// </summary>
    public enum MeasureHeightTypeEnum
    {
        /// <summary>
        /// 原点
        /// </summary>
        Origin,

        /// <summary>
        /// 自由选择
        /// </summary>
        Free
    }
}
