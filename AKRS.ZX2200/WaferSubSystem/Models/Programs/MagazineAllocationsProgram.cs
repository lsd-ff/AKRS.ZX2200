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
    using AKRS.ZX2200.SupportFeature.Parameters.Model;
    using AKRS.ZX2200.WaferSubSystem.Models.Repositories.Adapter;
    using AKRS.ZX2200.WaferSubSystem.Models.DeviceParams;

    /// <summary>
    /// 描述：料架配置程式
    /// </summary>
    public class MagazineAllocationsProgram
    {
        /// <summary>
        /// 当前Recipe 使用的料架配置名称
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 当前使用的料架配置
        /// </summary>
        [JsonIgnore]
        [TreeProgramListArgs("CurrentAllocationsSetting", "MagazineAllocationsProgram")]
        public MagazineAllocationsConfig CurrentAllocationsConfig => MagazineAllocationsConfigRepository.GetInstance().BaseDsSettingList.Find(item => item.Name == this.Name);

        /// <summary>
        /// 获取所使用的芯片
        /// </summary>
        /// <returns>result</returns>
        public List<BaseCarrierConfig> GetCarriers()
        {
            List<BaseCarrierConfig> list = new List<BaseCarrierConfig>();

            List<WaferTablet> waferTabletList = this.CurrentAllocationsConfig?.TabletArray?.OfType<WaferTablet>().ToList();
            List<AdapterTablet> adapterTabletList = this.CurrentAllocationsConfig?.TabletArray?.OfType<AdapterTablet>().ToList();         

            if (waferTabletList?.Count != 0 && waferTabletList != null)
            {
                list.AddRange(waferTabletList.Select(a => a.CarrierConfigWithWafer));
            }

            if (adapterTabletList?.Count != 0 && adapterTabletList != null)
            {
                List<AdapterSlotConfig[]> s = adapterTabletList.Select(a => a.AdapterSetting.WaffleArray).ToList();

                var wafflers = adapterTabletList
                    .Select(a => a.AdapterSetting.WaffleArray)
                    .SelectMany(a => a)
                    .Select(a => a.CarrierWithWaffleConfig);

                list.AddRange(wafflers);
            }

            list = list.Filter(a => a != null).Distinct().ToList();

            return list;
        }

        /// <summary>
        /// 获取华夫盘配置
        /// </summary>
        /// <returns>result</returns>
        public List<AdapterConfig> GetAdapterConfig()
        {
            List<AdapterConfig> adapterConfigList = new List<AdapterConfig>();

            adapterConfigList = this.CurrentAllocationsConfig?.TabletArray?.OfType<AdapterTablet>().Select(a => a.AdapterSetting).ToList();

            adapterConfigList = adapterConfigList?.Filter(a => a != null).Distinct().ToList();
            return adapterConfigList;
        }
    }
}
