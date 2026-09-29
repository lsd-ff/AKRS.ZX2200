namespace AKRS.ZX2200.WaferSubSystem.Models.Programs
{
    using AKRS.ZX2200.WaferSubSystem.Models.Entities.Tablet;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Ejection;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.MagazineAllocations;

    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;

    using AKRS.ZX2200.WaferSubSystem.Models.Entities;
    using AKRS.Galaxy2.Infrastructure.Helper;
    using AKRS.ZX2200.Main.Machine.MachineSupport;
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    /// <summary>
    /// 描述：静态华夫盘配置程式
    /// </summary>
    public class StaticAdapterProgram
    {
        /// <summary>
        /// 当前Recipe 使用的料架配置名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 当前静态华夫盘配置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("CurrentStaticAdapterConfig", "StaticAdapterProgram")]
        public AdapterConfig CurrentStaticAdapterConfig => AdapterConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 获取静态华夫盘所使用的芯片
        /// </summary>
        /// <returns>result</returns>
        public List<BaseCarrierConfig> GetCarriers()
        {
            List<BaseCarrierConfig> list = new List<BaseCarrierConfig>();

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                List<AdapterSlotConfig> staticAdapterSlotConfigList = this.CurrentStaticAdapterConfig?.WaffleArray?.ToList();

                var staticWafflers = staticAdapterSlotConfigList?.Select(a => a?.CarrierWithWaffleConfig);
                if(staticWafflers != null)
                {
                    list.AddRange(staticWafflers);
                }                
            }

            list = list.Filter(a => a != null).Distinct().ToList();

            return list;
        }

        /// <summary>
        /// 获取静态华夫盘配置
        /// </summary>
        /// <returns>result</returns>
        public List<AdapterConfig> GetAdapterConfig()
        {
            List<AdapterConfig> adapterConfigList = new List<AdapterConfig>();

            if (MachineHardwareConfiguration.GetInstance().IsStaticWaffleConfigrated)
            {
                adapterConfigList.Add(this.CurrentStaticAdapterConfig);
            }

            adapterConfigList = adapterConfigList.Filter(a => a != null).Distinct().ToList();

            return adapterConfigList;
        }
    }
}
